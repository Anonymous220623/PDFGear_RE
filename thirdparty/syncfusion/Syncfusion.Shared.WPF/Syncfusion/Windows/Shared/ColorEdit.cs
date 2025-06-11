// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.ColorEdit
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Licensing;
using Syncfusion.Windows.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(true)]
[SkinType(SkinVisualStyle = Skin.Blend, Type = typeof (ColorEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPicker/Themes/BlendStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Blue, Type = typeof (ColorEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPicker/Themes/Office2007BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Transparent, Type = typeof (ColorEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPicker/Themes/TransparentStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Silver, Type = typeof (ColorEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPicker/Themes/Office2007SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Blue, Type = typeof (ColorEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPicker/Themes/Office2010BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Black, Type = typeof (ColorEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPicker/Themes/Office2010BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Silver, Type = typeof (ColorEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPicker/Themes/Office2010SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2003, Type = typeof (ColorEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPicker/Themes/Office2003Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Black, Type = typeof (ColorEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPicker/Themes/Office2007BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.SyncOrange, Type = typeof (ColorEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPicker/Themes/SyncOrange.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyRed, Type = typeof (ColorEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPicker/Themes/ShinyRed.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyBlue, Type = typeof (ColorEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPicker/Themes/ShinyBlue.xaml")]
[SkinType(SkinVisualStyle = Skin.Default, Type = typeof (ColorEdit), XamlResource = "/Syncfusion.Shared.Wpf;component/Controls/ColorPicker/Themes/generic.xaml")]
[SkinType(SkinVisualStyle = Skin.VS2010, Type = typeof (ColorEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPicker/Themes/VS2010Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Metro, Type = typeof (ColorEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorPicker/Themes/MetroStyle.xaml")]
public class ColorEdit : Control, IDisposable
{
  private const string C_pickerColorBar = "PickerColorBar";
  private const string C_colorToggleButton = "colorToggleButton";
  private const string C_colorPalette = "ColorPalitte";
  private const string C_defaultSkinName = "Default";
  private const string C_colorEditContainerBrush = "ColorEditContainerBrush";
  private const string C_systemColors = "systemColors";
  private const string C_buttomH = "ButtomH";
  private const string C_buttomS = "ButtomS";
  private const string C_buttomV = "ButtomV";
  private const string C_wordKnownColorsTextBox = "WordKnownColorsTextBox";
  private const string C_colorStringEditor = "PART_ColorStringEditor";
  private const string C_suchInRed = "Such in Red:";
  private const string C_suchInGreen = "Such in Green:";
  private const string C_suchInBlue = "Such in Blue:";
  internal bool changeHSVBackground = true;
  internal bool changeColor;
  internal int Hcount;
  internal string CurrentHSV;
  internal bool CanChange = true;
  internal int count;
  internal bool allow = true;
  private bool breakLoop = true;
  internal Brush previousSelectedBrush = (Brush) new SolidColorBrush(Colors.White);
  private bool mouseLeftDown;
  private ToggleButton toggle;
  private ContentPresenter content;
  private bool colorchangedInternally;
  internal bool rev;
  internal float m_r;
  internal float m_g;
  internal float m_b;
  internal float m_a;
  public static RoutedCommand M_changeColorWhite;
  public static RoutedCommand M_changeColorBlack;
  private FrameworkElement m_colorPalette;
  private TextBox m_wordKnownColorsTextBox;
  internal Popup m_wordKnownColorPopup;
  private RadioButton m_buttomH;
  private RadioButton m_buttomS;
  private RadioButton m_buttomV;
  internal Color m_color;
  internal float A_value;
  private ComboBox m_systemColors;
  private ToggleButton m_colorToggleButton;
  private Color m_colorBeforeEyeDropStart;
  private bool m_bColorUpdating;
  private TextBox m_colorStringEditor;
  private ColorBar m_editColorBar;
  private bool m_bNeedChangeHSV = true;
  private Button linear;
  private Button radial;
  private Button Reverse;
  private Button Solid;
  private Button Gradient;
  private ToggleButton popupButton;
  private bool isLinear = true;
  internal StackPanel enableSwitch;
  internal Popup GradPopup;
  internal ColorPicker m_colorPicker;
  internal bool bindedmanually;
  internal bool flag;
  internal Path selectorEllipse;
  private ComboBox obj;
  public static readonly DependencyProperty AProperty = DependencyProperty.Register(nameof (A), typeof (float), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0.0f, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(ColorEdit.OnAChanged)));
  public static readonly DependencyProperty RProperty = DependencyProperty.Register(nameof (R), typeof (float), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) 1f, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(ColorEdit.OnRChanged)));
  public static readonly DependencyProperty GProperty = DependencyProperty.Register(nameof (G), typeof (float), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) 1f, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(ColorEdit.OnGChanged)));
  public static readonly DependencyProperty BProperty = DependencyProperty.Register(nameof (B), typeof (float), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) 1f, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(ColorEdit.OnBChanged)));
  public static readonly DependencyProperty BackgroundRProperty = DependencyProperty.Register(nameof (BackgroundR), typeof (Brush), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) new LinearGradientBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 0, (byte) 0), Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 0, (byte) 0), 0.0)));
  public static readonly DependencyProperty BackgroundGProperty = DependencyProperty.Register(nameof (BackgroundG), typeof (Brush), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) new LinearGradientBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 0, (byte) 0), Color.FromArgb(byte.MaxValue, (byte) 0, byte.MaxValue, (byte) 0), 0.0)));
  public static readonly DependencyProperty BackgroundBProperty = DependencyProperty.Register(nameof (BackgroundB), typeof (Brush), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) new LinearGradientBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 0, (byte) 0), Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 0, byte.MaxValue), 0.0)));
  public static readonly DependencyProperty BackgroundAProperty = DependencyProperty.Register(nameof (BackgroundA), typeof (Brush), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) new LinearGradientBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 0, (byte) 0), Color.FromArgb((byte) 0, (byte) 0, (byte) 0, (byte) 0), 90.0)));
  public static readonly DependencyProperty IsScRGBColorProperty = DependencyProperty.Register(nameof (IsScRGBColor), typeof (bool), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) false));
  public static readonly DependencyProperty IsAlphaVisibleProperty = DependencyProperty.Register(nameof (IsAlphaVisible), typeof (bool), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) true));
  public static readonly DependencyProperty HSVProperty = DependencyProperty.Register(nameof (HSV), typeof (HSV), typeof (ColorEdit), (PropertyMetadata) new UIPropertyMetadata((object) HSV.H));
  internal static readonly DependencyProperty HProperty = DependencyProperty.Register(nameof (H), typeof (float), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0.0f, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(ColorEdit.OnHChanged)));
  internal static readonly DependencyProperty SProperty = DependencyProperty.Register(nameof (S), typeof (float), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0.1f, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(ColorEdit.OnSChanged)));
  internal static readonly DependencyProperty VProperty = DependencyProperty.Register(nameof (V), typeof (float), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) 1f, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(ColorEdit.OnVChanged)));
  internal static readonly DependencyProperty SliderValueHSVProperty = DependencyProperty.Register(nameof (SliderValueHSV), typeof (float), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0.0f, FrameworkPropertyMetadataOptions.AffectsArrange, new PropertyChangedCallback(ColorEdit.OnSliderValueHSVChanged)));
  internal static readonly DependencyProperty SliderMaxValueHSVProperty = DependencyProperty.Register(nameof (SliderMaxValueHSV), typeof (float), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) 360f));
  internal static readonly DependencyProperty SelectorPositionXProperty = DependencyProperty.Register(nameof (SelectorPositionX), typeof (float), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0.0f));
  internal static readonly DependencyProperty SelectorPositionYProperty = DependencyProperty.Register(nameof (SelectorPositionY), typeof (float), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0.0f));
  internal static readonly DependencyProperty WordKnownColorsPositionXProperty = DependencyProperty.Register(nameof (WordKnownColorsPositionX), typeof (float), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) 5f));
  internal static readonly DependencyProperty WordKnownColorsPositionYProperty = DependencyProperty.Register(nameof (WordKnownColorsPositionY), typeof (float), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) 5f));
  internal static readonly DependencyProperty VisualizationStyleProperty = DependencyProperty.Register(nameof (VisualizationStyle), typeof (ColorSelectionMode), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) ColorSelectionMode.RGB, new PropertyChangedCallback(ColorEdit.OnVisualizationStyleChanged)));
  public static readonly DependencyProperty ThumbTemplateProperty = DependencyProperty.Register(nameof (ThumbTemplate), typeof (ControlTemplate), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof (Color), typeof (Color), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) Colors.Transparent, FrameworkPropertyMetadataOptions.AffectsArrange, new PropertyChangedCallback(ColorEdit.OnColorChanged)));
  public static readonly DependencyProperty StartpointProperty = DependencyProperty.Register(nameof (Startpoint), typeof (Point), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) new Point(0.5, 0.0)));
  public static readonly DependencyProperty EndpointProperty = DependencyProperty.Register(nameof (Endpoint), typeof (Point), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) new Point(0.5, 1.0)));
  public static readonly DependencyProperty CentrePointProperty = DependencyProperty.Register(nameof (CentrePoint), typeof (Point), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) new Point(0.5, 0.5)));
  public static readonly DependencyProperty GradientOriginProperty = DependencyProperty.Register(nameof (GradientOrigin), typeof (Point), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) new Point(0.5, 0.5)));
  public static readonly DependencyProperty RadiusXProperty = DependencyProperty.Register(nameof (RadiusX), typeof (double), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0.5));
  public static readonly DependencyProperty RadiusYProperty = DependencyProperty.Register(nameof (RadiusY), typeof (double), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0.5));
  public static readonly DependencyProperty BrushProperty = DependencyProperty.Register(nameof (Brush), typeof (Brush), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) new SolidColorBrush(Colors.Transparent), new PropertyChangedCallback(ColorEdit.OnSelectedBrushChanged)));
  public static readonly DependencyProperty BrushModeProperty = DependencyProperty.Register(nameof (BrushMode), typeof (BrushModes), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) BrushModes.Solid, new PropertyChangedCallback(ColorEdit.OnBrushModeChanged)));
  public static readonly DependencyProperty EnableGradientToSolidSwitchProperty = DependencyProperty.Register(nameof (EnableGradientToSolidSwitch), typeof (bool), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, new PropertyChangedCallback(ColorEdit.OnEnableSwitchChanged)));
  public static readonly DependencyProperty GradientPropertyEditorModeProperty = DependencyProperty.Register(nameof (GradientPropertyEditorMode), typeof (GradientPropertyEditorMode), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) GradientPropertyEditorMode.Popup, new PropertyChangedCallback(ColorEdit.OnGradientPropertyEditorModeChanged)));
  public static readonly DependencyProperty IsOpenGradientPropertyEditorProperty = DependencyProperty.Register(nameof (IsOpenGradientPropertyEditor), typeof (bool), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, new PropertyChangedCallback(ColorEdit.OnIsOpenGradientPropertyEditorChanged)));
  public static readonly DependencyProperty IsGradientPropertyEnabledProperty = DependencyProperty.Register(nameof (IsGradientPropertyEnabled), typeof (bool), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, new PropertyChangedCallback(ColorEdit.OnIsGradientPropertyEnabledChanged)));
  internal static readonly DependencyProperty InvertColorProperty = DependencyProperty.Register(nameof (InvertColor), typeof (Color), typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) Colors.Green));
  public static readonly DependencyProperty EnableToolTipProperty = DependencyProperty.Register(nameof (EnableToolTip), typeof (bool), typeof (ColorEdit), (PropertyMetadata) new UIPropertyMetadata((object) true, new PropertyChangedCallback(ColorEdit.OnEnableToolTipChanged)));
  private CommandBinding colorWhiteBinding;
  private CommandBinding colorBlackBinding;
  private bool isEyeDropper;
  internal UpDown startx;
  internal UpDown starty;
  internal UpDown endx;
  internal UpDown endy;
  internal UpDown centrex;
  internal UpDown centrey;
  internal UpDown gradx;
  internal UpDown grady;
  internal UpDown radiusx;
  internal UpDown radiusy;
  private GradientStartPoint mys;
  internal bool rgbChanged;
  internal bool blackWhitePressed;
  private GradientStartPoint mye;
  private GradientStartPoint Centre;
  private GradientStartPoint Gradientorigin;
  internal TextBox tb;
  internal TextBox rval;
  internal TextBox gval;
  internal TextBox bval;
  internal TextBox hval;
  internal TextBox sval;
  internal TextBox vval;
  private Grid RadialGrid;
  private Grid LinearGrid;
  private Border GradBorder;
  private Window window;
  private GradientBrush gb;
  internal bool blackWhite;
  internal bool loaded;
  internal bool Edited;
  internal bool mousedown;
  internal bool setnocolor;
  internal Slider SliderH;
  internal Slider SliderS;
  internal Slider SliderV;

  private Rectangle rectBar { get; set; }

  internal Canvas canvasBar { get; set; }

  internal Grid gradientGrid { get; set; }

  internal GradientItemCollection gradientItemCollection { get; set; }

  internal Rectangle CurrentColor { get; set; }

  internal Rectangle SelectedColor { get; set; }

  public bool EnableToolTip
  {
    get => (bool) this.GetValue(ColorEdit.EnableToolTipProperty);
    set => this.SetValue(ColorEdit.EnableToolTipProperty, (object) value);
  }

  public bool IsScRGBColor
  {
    get => (bool) this.GetValue(ColorEdit.IsScRGBColorProperty);
    set => this.SetValue(ColorEdit.IsScRGBColorProperty, (object) value);
  }

  public float R
  {
    get => this.m_r;
    set => this.SetValue(ColorEdit.RProperty, (object) value);
  }

  public float G
  {
    get => this.m_g;
    set => this.SetValue(ColorEdit.GProperty, (object) value);
  }

  public float B
  {
    get => this.m_b;
    set => this.SetValue(ColorEdit.BProperty, (object) value);
  }

  public float A
  {
    get => this.m_a;
    set => this.SetValue(ColorEdit.AProperty, (object) value);
  }

  public Brush BackgroundA
  {
    get => (Brush) this.GetValue(ColorEdit.BackgroundAProperty);
    set => this.SetValue(ColorEdit.BackgroundAProperty, (object) value);
  }

  public Brush BackgroundR
  {
    get => (Brush) this.GetValue(ColorEdit.BackgroundRProperty);
    set => this.SetValue(ColorEdit.BackgroundRProperty, (object) value);
  }

  public Brush BackgroundG
  {
    get => (Brush) this.GetValue(ColorEdit.BackgroundGProperty);
    set => this.SetValue(ColorEdit.BackgroundGProperty, (object) value);
  }

  public Brush BackgroundB
  {
    get => (Brush) this.GetValue(ColorEdit.BackgroundBProperty);
    set => this.SetValue(ColorEdit.BackgroundBProperty, (object) value);
  }

  public float H
  {
    get => (float) this.GetValue(ColorEdit.HProperty);
    set => this.SetValue(ColorEdit.HProperty, (object) value);
  }

  public float S
  {
    get => (float) this.GetValue(ColorEdit.SProperty);
    set => this.SetValue(ColorEdit.SProperty, (object) value);
  }

  public float V
  {
    get => (float) this.GetValue(ColorEdit.VProperty);
    set => this.SetValue(ColorEdit.VProperty, (object) value);
  }

  private float SelectorPositionX
  {
    get => (float) this.GetValue(ColorEdit.SelectorPositionXProperty);
    set => this.SetValue(ColorEdit.SelectorPositionXProperty, (object) value);
  }

  private float SelectorPositionY
  {
    get => (float) this.GetValue(ColorEdit.SelectorPositionYProperty);
    set => this.SetValue(ColorEdit.SelectorPositionYProperty, (object) value);
  }

  private float SliderValueHSV
  {
    get => (float) this.GetValue(ColorEdit.SliderValueHSVProperty);
    set => this.SetValue(ColorEdit.SliderValueHSVProperty, (object) value);
  }

  private float SliderMaxValueHSV
  {
    get => (float) this.GetValue(ColorEdit.SliderMaxValueHSVProperty);
    set => this.SetValue(ColorEdit.SliderMaxValueHSVProperty, (object) value);
  }

  public HSV HSV
  {
    get => (HSV) this.GetValue(ColorEdit.HSVProperty);
    set => this.SetValue(ColorEdit.HSVProperty, (object) value);
  }

  public ControlTemplate ThumbTemplate
  {
    get => (ControlTemplate) this.GetValue(ColorEdit.ThumbTemplateProperty);
    set => this.SetValue(ColorEdit.ThumbTemplateProperty, (object) value);
  }

  private float WordKnownColorsPositionX
  {
    get => (float) this.GetValue(ColorEdit.WordKnownColorsPositionXProperty);
    set => this.SetValue(ColorEdit.WordKnownColorsPositionXProperty, (object) value);
  }

  private float WordKnownColorsPositionY
  {
    get => (float) this.GetValue(ColorEdit.WordKnownColorsPositionYProperty);
    set => this.SetValue(ColorEdit.WordKnownColorsPositionYProperty, (object) value);
  }

  public ColorSelectionMode VisualizationStyle
  {
    get => (ColorSelectionMode) this.GetValue(ColorEdit.VisualizationStyleProperty);
    set => this.SetValue(ColorEdit.VisualizationStyleProperty, (object) value);
  }

  public Color Color
  {
    get => (Color) this.GetValue(ColorEdit.ColorProperty);
    set => this.SetValue(ColorEdit.ColorProperty, (object) value);
  }

  public Point Startpoint
  {
    get => (Point) this.GetValue(ColorEdit.StartpointProperty);
    set
    {
      this.SetValue(ColorEdit.StartpointProperty, (object) value);
      this.mys = new GradientStartPoint()
      {
        X = value.X,
        Y = value.Y
      };
    }
  }

  public Point RadiusX
  {
    get => (Point) this.GetValue(ColorEdit.RadiusXProperty);
    set => this.SetValue(ColorEdit.RadiusXProperty, (object) value);
  }

  public Point RadiusY
  {
    get => (Point) this.GetValue(ColorEdit.RadiusYProperty);
    set => this.SetValue(ColorEdit.RadiusYProperty, (object) value);
  }

  public Point GradientOrigin
  {
    get => (Point) this.GetValue(ColorEdit.GradientOriginProperty);
    set => this.SetValue(ColorEdit.GradientOriginProperty, (object) value);
  }

  public Point Endpoint
  {
    get => (Point) this.GetValue(ColorEdit.EndpointProperty);
    set
    {
      this.SetValue(ColorEdit.EndpointProperty, (object) value);
      this.mye = new GradientStartPoint()
      {
        X = value.X,
        Y = value.Y
      };
    }
  }

  public Point CentrePoint
  {
    get => (Point) this.GetValue(ColorEdit.CentrePointProperty);
    set => this.SetValue(ColorEdit.CentrePointProperty, (object) value);
  }

  public Brush Brush
  {
    get => (Brush) this.GetValue(ColorEdit.BrushProperty);
    set
    {
      try
      {
        this.SetValue(ColorEdit.BrushProperty, (object) value);
      }
      catch (Exception ex)
      {
      }
    }
  }

  public Color InvertColor
  {
    get => (Color) this.GetValue(ColorEdit.InvertColorProperty);
    set => this.SetValue(ColorEdit.InvertColorProperty, (object) value);
  }

  public bool IsAlphaVisible
  {
    get => (bool) this.GetValue(ColorEdit.IsAlphaVisibleProperty);
    set => this.SetValue(ColorEdit.IsAlphaVisibleProperty, (object) value);
  }

  public BrushModes BrushMode
  {
    get => (BrushModes) this.GetValue(ColorEdit.BrushModeProperty);
    set => this.SetValue(ColorEdit.BrushModeProperty, (object) value);
  }

  public GradientPropertyEditorMode GradientPropertyEditorMode
  {
    get => (GradientPropertyEditorMode) this.GetValue(ColorEdit.GradientPropertyEditorModeProperty);
    set => this.SetValue(ColorEdit.GradientPropertyEditorModeProperty, (object) value);
  }

  public bool IsOpenGradientPropertyEditor
  {
    get => (bool) this.GetValue(ColorEdit.IsOpenGradientPropertyEditorProperty);
    set => this.SetValue(ColorEdit.IsOpenGradientPropertyEditorProperty, (object) value);
  }

  public bool IsGradientPropertyEnabled
  {
    get => (bool) this.GetValue(ColorEdit.IsGradientPropertyEnabledProperty);
    set => this.SetValue(ColorEdit.IsGradientPropertyEnabledProperty, (object) value);
  }

  public bool EnableGradientToSolidSwitch
  {
    get => (bool) this.GetValue(ColorEdit.EnableGradientToSolidSwitchProperty);
    set => this.SetValue(ColorEdit.EnableGradientToSolidSwitchProperty, (object) value);
  }

  static ColorEdit()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ColorEdit), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ColorEdit)));
    ColorEdit.M_changeColorWhite = new RoutedCommand();
    ColorEdit.M_changeColorBlack = new RoutedCommand();
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  public ColorEdit()
  {
    this.Initialize();
    this.mys = new GradientStartPoint() { X = 0.5, Y = 0.0 };
    this.mye = new GradientStartPoint() { X = 0.5, Y = 1.0 };
    this.Centre = new GradientStartPoint()
    {
      X = 0.5,
      Y = 0.5
    };
    this.Gradientorigin = new GradientStartPoint()
    {
      X = 0.5,
      Y = 0.5
    };
    this.Loaded += new RoutedEventHandler(this.ColorEdit_Loaded);
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  private void ColorEdit_Loaded(object sender, RoutedEventArgs e)
  {
    this.mys.PropertyChanged -= new PropertyChangedEventHandler(this.mys_PropertyChanged);
    this.mye.PropertyChanged -= new PropertyChangedEventHandler(this.mye_PropertyChanged);
    this.Centre.PropertyChanged -= new PropertyChangedEventHandler(this.Centre_PropertyChanged);
    this.Gradientorigin.PropertyChanged -= new PropertyChangedEventHandler(this.Gradientorigin_PropertyChanged);
    this.RemoveHandler(BorderEyeDrop.BeginColorPickingEvent, (Delegate) new RoutedEventHandler(this.ProcessColorPickingStart));
    this.RemoveHandler(BorderEyeDrop.CancelColorPickingEvent, (Delegate) new RoutedEventHandler(this.ProcessColorPickingCancel));
    this.AddHandler(BorderEyeDrop.BeginColorPickingEvent, (Delegate) new RoutedEventHandler(this.ProcessColorPickingStart));
    this.AddHandler(BorderEyeDrop.CancelColorPickingEvent, (Delegate) new RoutedEventHandler(this.ProcessColorPickingCancel));
    this.mys.PropertyChanged += new PropertyChangedEventHandler(this.mys_PropertyChanged);
    this.mye.PropertyChanged += new PropertyChangedEventHandler(this.mye_PropertyChanged);
    this.Centre.PropertyChanged += new PropertyChangedEventHandler(this.Centre_PropertyChanged);
    this.Gradientorigin.PropertyChanged += new PropertyChangedEventHandler(this.Gradientorigin_PropertyChanged);
  }

  private void Gradientorigin_PropertyChanged(object sender, PropertyChangedEventArgs e)
  {
    this.GradientOrigin = new Point((sender as GradientStartPoint).X, (sender as GradientStartPoint).Y);
  }

  private void Centre_PropertyChanged(object sender, PropertyChangedEventArgs e)
  {
    this.CentrePoint = new Point((sender as GradientStartPoint).X, (sender as GradientStartPoint).Y);
  }

  private void mye_PropertyChanged(object sender, PropertyChangedEventArgs e)
  {
    this.Endpoint = new Point((sender as GradientStartPoint).X, (sender as GradientStartPoint).Y);
  }

  private void Initialize()
  {
    if (this.colorWhiteBinding != null)
      this.colorWhiteBinding.Executed -= new ExecutedRoutedEventHandler(this.ChangeColorWhite);
    if (this.colorBlackBinding != null)
      this.colorBlackBinding.Executed -= new ExecutedRoutedEventHandler(this.ChangeColorBlack);
    this.SizeChanged -= new SizeChangedEventHandler(this.ColorEdit_SizeChanged);
    this.colorWhiteBinding = new CommandBinding((ICommand) ColorEdit.M_changeColorWhite);
    this.colorWhiteBinding.Executed += new ExecutedRoutedEventHandler(this.ChangeColorWhite);
    this.colorBlackBinding = new CommandBinding((ICommand) ColorEdit.M_changeColorBlack);
    this.colorBlackBinding.Executed += new ExecutedRoutedEventHandler(this.ChangeColorBlack);
    this.CommandBindings.Add(this.colorWhiteBinding);
    this.CommandBindings.Add(this.colorBlackBinding);
    this.SizeChanged += new SizeChangedEventHandler(this.ColorEdit_SizeChanged);
  }

  public event PropertyChangedCallback RChanged;

  public event PropertyChangedCallback GChanged;

  public event PropertyChangedCallback BChanged;

  public event PropertyChangedCallback AChanged;

  public event PropertyChangedCallback HChanged;

  public event PropertyChangedCallback SChanged;

  public event PropertyChangedCallback VChanged;

  public event PropertyChangedCallback SliderValueHSVChanged;

  public event PropertyChangedCallback VisualizationStyleChanged;

  public event PropertyChangedCallback ColorChanged;

  public event PropertyChangedCallback GradientPropertyEditorModeChanged;

  public event PropertyChangedCallback IsOpenGradientPropertyEditorChanged;

  public event PropertyChangedCallback IsGradientPropertyEnabledChanged;

  public event PropertyChangedCallback EnableToolTipChanged;

  private static void OnRChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((ColorEdit) d).OnRChanged(e);
  }

  private static void OnGChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((ColorEdit) d).OnGChanged(e);
  }

  private static void OnBChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((ColorEdit) d).OnBChanged(e);
  }

  private static void OnAChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((ColorEdit) d).OnAChanged(e);
  }

  private static void OnHChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((ColorEdit) d).OnHChanged(e);
  }

  private static void OnSChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((ColorEdit) d).OnSChanged(e);
  }

  private static void OnVChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((ColorEdit) d).OnVChanged(e);
  }

  private static void OnSliderValueHSVChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((ColorEdit) d).OnSliderValueHSVChanged(e);
  }

  private static void OnVisualizationStyleChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((ColorEdit) d).OnVisualizationStyleChanged(e);
  }

  private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((ColorEdit) d).OnColorChanged(e);
  }

  private static void OnSelectedBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((ColorEdit) d).OnSelectedBrushChanged(e);
  }

  private static void OnEnableSwitchChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((ColorEdit) d).UpdateVisibility((bool) e.NewValue ? Visibility.Visible : Visibility.Collapsed);
  }

  private void OnSelectedBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue == null)
      return;
    if (this.m_colorPicker == null)
    {
      if (e.NewValue is SolidColorBrush)
      {
        this.Color = ((SolidColorBrush) e.NewValue).Color;
        this.BrushMode = BrushModes.Solid;
      }
      else if (e.NewValue is GradientBrush)
      {
        this.BrushMode = BrushModes.Gradient;
        if (this.canvasBar != null && !this.colorchangedInternally)
          this.RefreshGradientStoppers((Brush) e.NewValue);
        if (((GradientBrush) e.NewValue).GradientStops.Count > 0 && this.breakLoop && this.gradientItemCollection != null && this.gradientItemCollection.Items != null && this.gradientItemCollection.Items.Count > 0)
        {
          foreach (GradientStopItem gradientStopItem in (IEnumerable) this.gradientItemCollection.Items)
          {
            if (gradientStopItem.isselected)
            {
              this.Color = gradientStopItem.color;
              break;
            }
          }
        }
      }
    }
    else if (this.m_colorPicker == null)
      this.Hcount = 0;
    if (this.SelectedColor == null || this.CurrentColor == null)
      return;
    this.CurrentColor.Fill = this.previousSelectedBrush;
    this.SelectedColor.Fill = (Brush) new SolidColorBrush(this.Color);
    this.Hcount = 0;
  }

  internal void RefreshGradientStoppers(Brush brush)
  {
    if (brush == null)
      return;
    if (this.canvasBar != null && this.canvasBar.Children.Count > 2)
    {
      List<UIElement> uiElementList = new List<UIElement>();
      foreach (UIElement child in this.canvasBar.Children)
      {
        if (!(child is Rectangle))
          uiElementList.Add(child);
      }
      foreach (UIElement element in uiElementList)
        this.canvasBar.Children.Remove(element);
    }
    if (this.gradientItemCollection != null)
    {
      this.gradientItemCollection.Items.Clear();
      this.gradientItemCollection = (GradientItemCollection) null;
    }
    this.SetBrush(brush as GradientBrush);
    if (this.canvasBar != null)
    {
      foreach (GradientStopItem gradientStopItem in (IEnumerable) this.gradientItemCollection.Items)
        this.canvasBar.Children.Add((UIElement) gradientStopItem.gradientitem);
    }
    this.fillGradient(this.gradientItemCollection.gradientItem);
    switch (brush)
    {
      case LinearGradientBrush _:
        this.Startpoint = (brush as LinearGradientBrush).StartPoint;
        this.Endpoint = (brush as LinearGradientBrush).EndPoint;
        break;
      case RadialGradientBrush _:
        this.GradientOrigin = (brush as RadialGradientBrush).GradientOrigin;
        this.CentrePoint = (brush as RadialGradientBrush).Center;
        break;
    }
  }

  private static void OnBrushModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((ColorEdit) d).OnBrushModeChanged(e);
  }

  private static void OnGradientPropertyEditorModeChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((ColorEdit) d).OnGradientPropertyEditorModeChanged(e);
  }

  private static void OnIsOpenGradientPropertyEditorChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((ColorEdit) d).OnIsOpenGradientPropertyEditorChanged(e);
  }

  private static void OnEnableToolTipChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((ColorEdit) d).OnEnableToolTipChanged(e);
  }

  private void OnGradientPropertyEditorModeChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.GradientPropertyEditorModeChanged == null)
      return;
    this.GradientPropertyEditorModeChanged((DependencyObject) this, e);
  }

  private void OnIsOpenGradientPropertyEditorChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.BrushMode == BrushModes.Gradient)
    {
      if ((this.VisualizationStyle == ColorSelectionMode.ClassicHSV || this.VisualizationStyle == ColorSelectionMode.ClassicRGB) && this.GradPopup != null && (!this.GradPopup.IsOpen && this.IsGradientPropertyEnabled || this.GradPopup.IsOpen))
        this.GradPopup.IsOpen = this.IsOpenGradientPropertyEditor;
      if (this.toggle != null)
        this.toggle.IsChecked = new bool?(this.IsOpenGradientPropertyEditor);
    }
    if (this.IsOpenGradientPropertyEditorChanged == null)
      return;
    this.IsOpenGradientPropertyEditorChanged((DependencyObject) this, e);
  }

  private void OnIsGradientPropertyEnabledChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.BrushMode == BrushModes.Gradient)
      this.BrushMode = BrushModes.Solid;
    this.UpdateVisibility((bool) e.NewValue ? Visibility.Visible : Visibility.Collapsed);
    this.IsOpenGradientPropertyEditor = false;
    if (this.IsGradientPropertyEnabledChanged == null)
      return;
    this.IsGradientPropertyEnabledChanged((DependencyObject) this, e);
  }

  private static void OnIsGradientPropertyEnabledChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((ColorEdit) d).OnIsGradientPropertyEnabledChanged(e);
  }

  private void OnEnableToolTipChanged(DependencyPropertyChangedEventArgs e)
  {
    if ((bool) e.NewValue)
      this.CalculateHSVSelectorPosition();
    if (this.m_wordKnownColorPopup != null)
      this.m_wordKnownColorPopup.IsOpen = (bool) e.NewValue;
    if (this.EnableToolTipChanged == null)
      return;
    this.EnableToolTipChanged((DependencyObject) this, e);
  }

  private void OnBrushModeChanged(DependencyPropertyChangedEventArgs e)
  {
    if (!this.IsLoaded)
      return;
    this.updateBrushMode(this.BrushMode == BrushModes.Solid ? Visibility.Collapsed : Visibility.Visible);
    if (this.BrushMode == BrushModes.Gradient)
    {
      if (this.Brush is SolidColorBrush)
      {
        if (this.gradientItemCollection != null && this.gradientItemCollection.Items.Count > 0)
        {
          this.isLinear = true;
          this.fillGradient(this.gradientItemCollection.gradientItem);
        }
        else
          this.applygradient();
      }
      if (this.GradBorder == null)
        return;
      if (this.GradientPropertyEditorMode == GradientPropertyEditorMode.Extended)
        this.GradBorder.Visibility = Visibility.Visible;
      else
        this.GradBorder.Visibility = Visibility.Collapsed;
    }
    else
    {
      if (!(this.Brush is GradientBrush))
        return;
      this.Brush = (Brush) new SolidColorBrush(this.Color);
      if (this.m_colorPicker == null)
        return;
      this.m_colorPicker.Brush = this.Brush;
    }
  }

  public static string[] SuchColor(Color color)
  {
    int num1 = int.MaxValue;
    int num2 = int.MaxValue;
    int num3 = int.MaxValue;
    int num4 = int.MaxValue;
    int index1 = 0;
    int index2 = 0;
    int index3 = 0;
    int index4 = 0;
    uint[] numArray = ColorEdit.InitColorTable();
    for (int index5 = 0; index5 < numArray.Length; ++index5)
    {
      Color color1 = ColorEdit.FromUInt32(numArray[index5]);
      int num5 = Math.Abs((int) color1.R - (int) color.R);
      int num6 = Math.Abs((int) color1.G - (int) color.G);
      int num7 = Math.Abs((int) color1.B - (int) color.B);
      Math.Abs((int) color1.A - (int) color.A);
      int num8 = num5 + num7 + num6;
      if (num8 < num1)
      {
        if (color1.A.Equals(color.A))
        {
          index4 = index5;
          num1 = num8;
        }
        else if (color.A != (byte) 0)
        {
          index4 = index5;
          num1 = num8;
        }
      }
      if (num5 < num2)
      {
        index1 = index5;
        num2 = num5;
      }
      if (num6 < num3)
      {
        index2 = index5;
        num3 = num6;
      }
      if (num7 < num4)
      {
        index3 = index5;
        num4 = num7;
      }
    }
    Syncfusion.Windows.Tools.KnownColor knownColor1 = (Syncfusion.Windows.Tools.KnownColor) numArray[index4];
    Syncfusion.Windows.Tools.KnownColor knownColor2 = (Syncfusion.Windows.Tools.KnownColor) numArray[index1];
    Syncfusion.Windows.Tools.KnownColor knownColor3 = (Syncfusion.Windows.Tools.KnownColor) numArray[index2];
    Syncfusion.Windows.Tools.KnownColor knownColor4 = (Syncfusion.Windows.Tools.KnownColor) numArray[index3];
    return new string[4]
    {
      knownColor1.ToString(),
      knownColor2.ToString(),
      knownColor3.ToString(),
      knownColor4.ToString()
    };
  }

  private static uint[] InitColorTable()
  {
    return new uint[142]
    {
      4293982463U,
      4294634455U,
      4278255615U,
      4286578644U,
      4293984255U,
      4294309340U,
      4294960324U,
      4278190080U /*0xFF000000*/,
      4294962125U,
      4278190335U,
      4287245282U,
      4289014314U,
      4292786311U,
      4284456608U,
      4286578432U,
      4291979550U,
      4294934352U,
      4284782061U,
      4294965468U,
      4292613180U,
      4278255615U,
      4278190219U,
      4278225803U,
      4290283019U,
      4289309097U,
      4278215680U,
      4290623339U,
      4287299723U,
      4283788079U,
      4294937600U,
      4288230092U,
      4287299584U,
      4293498490U,
      4287609999U,
      4282924427U,
      4281290575U,
      4278243025U,
      4287889619U,
      4294907027U,
      4278239231U,
      4285098345U,
      4280193279U,
      4289864226U,
      4294966000U,
      4280453922U,
      4294902015U,
      4292664540U,
      4294506751U,
      4294956800U,
      4292519200U,
      4286611584U,
      4278222848U /*0xFF008000*/,
      4289593135U,
      4293984240U /*0xFFF0FFF0*/,
      4294928820U,
      4291648604U,
      4283105410U,
      4294967280U,
      4293977740U,
      4293322490U,
      4294963445U,
      4286381056U,
      4294965965U,
      4289583334U,
      4293951616U,
      4292935679U,
      4294638290U,
      4292072403U,
      4287688336U,
      4294948545U,
      4294942842U,
      4280332970U,
      4287090426U,
      4286023833U,
      4289774814U,
      4294967264U,
      4278255360U /*0xFF00FF00*/,
      4281519410U,
      4294635750U,
      4294902015U,
      4286578688U /*0xFF800000*/,
      4284927402U,
      4278190285U,
      4290401747U,
      4287852763U,
      4282168177U,
      4286277870U,
      4278254234U,
      4282962380U,
      4291237253U,
      4279834992U,
      4294311930U,
      4294960353U,
      4294960309U,
      4294958765U,
      4278190208U /*0xFF000080*/,
      4294833638U,
      4286611456U,
      4285238819U,
      4294944000U,
      4294919424U,
      4292505814U,
      4293847210U,
      4288215960U,
      4289720046U,
      4292571283U,
      4294963157U,
      4294957753U,
      4291659071U,
      4294951115U,
      4292714717U,
      4289781990U,
      4286578816U,
      4294901760U,
      4290547599U,
      4282477025U,
      4287317267U,
      4294606962U,
      4294222944U,
      4281240407U,
      4294964718U,
      4288696877U,
      4290822336U,
      4287090411U,
      4285160141U,
      4285563024U,
      4294966010U,
      4278255487U,
      4282811060U,
      4291998860U,
      4278222976U,
      4292394968U,
      4294927175U,
      uint.MaxValue,
      4282441936U,
      16777215U /*0xFFFFFF*/,
      4293821166U,
      4294303411U,
      uint.MaxValue,
      4294309365U,
      4294967040U,
      4288335154U
    };
  }

  private static Color FromUInt32(uint argb)
  {
    return new Color()
    {
      A = (byte) ((argb & 4278190080U /*0xFF000000*/) >> 24),
      R = (byte) ((argb & 16711680U /*0xFF0000*/) >> 16 /*0x10*/),
      G = (byte) ((argb & 65280U) >> 8),
      B = (byte) (argb & (uint) byte.MaxValue)
    };
  }

  private void OnRChanged(DependencyPropertyChangedEventArgs e)
  {
    if ((double) this.m_r == (double) (float) e.NewValue)
      return;
    this.m_r = (float) e.NewValue;
    if (this.RChanged != null)
      this.RChanged((DependencyObject) this, e);
    this.UpdateColor();
  }

  private void OnGChanged(DependencyPropertyChangedEventArgs e)
  {
    if ((double) this.m_g == (double) (float) e.NewValue)
      return;
    this.m_g = (float) e.NewValue;
    if (this.GChanged != null)
      this.GChanged((DependencyObject) this, e);
    this.UpdateColor();
  }

  private void OnBChanged(DependencyPropertyChangedEventArgs e)
  {
    if ((double) this.m_b == (double) (float) e.NewValue)
      return;
    this.m_b = (float) e.NewValue;
    if (this.BChanged != null)
      this.BChanged((DependencyObject) this, e);
    this.UpdateColor();
  }

  private void UpdateColor()
  {
    if (this.m_bColorUpdating)
      return;
    this.Color = Color.FromScRgb(this.m_a, this.m_r, this.m_g, this.m_b);
    this.CalculateBackground();
  }

  private float GetHPart()
  {
    return (float) HsvColor.ConvertRgbToHsv((int) this.Color.R, (int) this.Color.B, (int) this.Color.G).H;
  }

  private void OnAChanged(DependencyPropertyChangedEventArgs e)
  {
    if ((double) this.m_a == (double) (float) e.NewValue)
      return;
    this.m_a = (float) e.NewValue;
    this.A_value = (float) e.NewValue;
    if (this.AChanged != null)
      this.AChanged((DependencyObject) this, e);
    if (this.m_bColorUpdating)
      return;
    this.Color = Color.FromScRgb(this.m_a, this.m_r, this.m_g, this.m_b);
    this.CalculateBackground();
  }

  private void ChangeColorWhite(object sender, ExecutedRoutedEventArgs e)
  {
    this.blackWhitePressed = true;
    this.Color = Colors.White;
  }

  private void ChangeColorBlack(object sender, ExecutedRoutedEventArgs e)
  {
    this.blackWhitePressed = true;
    this.Color = Colors.Black;
  }

  private void UpdateColorBarSlider()
  {
    if (this.m_editColorBar == null)
      return;
    this.m_editColorBar.SliderValue = this.GetHPart();
  }

  private void OnHChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.HSV == HSV.H)
      this.SliderValueHSV = this.H;
    if (this.HChanged != null)
      this.HChanged((DependencyObject) this, e);
    if (this.m_bColorUpdating)
      return;
    Color rgb = HsvColor.ConvertHsvToRgb((double) this.H, (double) this.S, (double) this.V);
    this.Color = Color.FromArgb(this.Color.A, rgb.R, rgb.G, rgb.B);
  }

  private void OnSChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.HSV == HSV.S)
      this.SliderValueHSV = this.S;
    if (this.SChanged != null)
      this.SChanged((DependencyObject) this, e);
    if (this.m_bColorUpdating)
      return;
    Color rgb = HsvColor.ConvertHsvToRgb((double) this.H, (double) this.S, (double) this.V);
    this.Color = Color.FromArgb(this.Color.A, rgb.R, rgb.G, rgb.B);
    this.CalculateHSVBackground();
  }

  private void OnVChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.HSV == HSV.V)
      this.SliderValueHSV = this.V;
    if (this.VChanged != null)
      this.VChanged((DependencyObject) this, e);
    if (this.m_bColorUpdating)
      return;
    Color rgb = HsvColor.ConvertHsvToRgb((double) this.H, (double) this.S, (double) this.V);
    this.Color = Color.FromArgb(this.Color.A, rgb.R, rgb.G, rgb.B);
    this.CalculateHSVBackground();
  }

  private void OnSliderValueHSVChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.SliderValueHSVChanged != null)
      this.SliderValueHSVChanged((DependencyObject) this, e);
    this.SliderValueHSV = (float) e.NewValue;
    if (!this.mouseLeftDown && this.SelectedColor != null && this.CanChange)
    {
      if (this.m_colorPicker == null)
        this.previousSelectedBrush = this.SelectedColor.Fill;
      else if (this.allow)
        this.previousSelectedBrush = this.SelectedColor.Fill;
      this.allow = false;
    }
    this.CanChange = true;
    switch (this.HSV)
    {
      case HSV.H:
        this.H = this.SliderValueHSV;
        break;
      case HSV.S:
        this.S = this.SliderValueHSV;
        break;
      case HSV.V:
        this.V = this.SliderValueHSV;
        break;
      default:
        this.H = this.SliderValueHSV;
        break;
    }
  }

  private void HSVSelected(object sender, RoutedEventArgs args)
  {
    Point point = new Point(0.0, 0.0);
    switch ((sender as RadioButton).Content.ToString())
    {
      case "H":
        this.HSV = HSV.H;
        this.SliderMaxValueHSV = 360f;
        this.SliderValueHSV = this.H;
        point.X = (double) this.GetXPositionForH();
        point.Y = (double) this.GetYPositionForHS();
        break;
      case "S":
        this.HSV = HSV.S;
        this.SliderMaxValueHSV = 1f;
        this.SliderValueHSV = this.S;
        point.X = (double) this.GetXPositionForSV();
        point.Y = (double) this.GetYPositionForHS();
        break;
      case "V":
        this.HSV = HSV.V;
        this.SliderMaxValueHSV = 1f;
        this.SliderValueHSV = this.V;
        point.X = (double) this.GetXPositionForSV();
        point.Y = (double) this.GetYPositionForV();
        break;
    }
    this.CalculateWordKnownColorsPosition(point);
  }

  private void UpdateVisibility(Visibility visibility)
  {
    if (this.Gradient != null)
      this.Gradient.Visibility = visibility;
    if (this.radial != null)
      this.radial.Visibility = visibility;
    if (this.Solid != null)
      this.Solid.Visibility = visibility;
    if (!this.EnableGradientToSolidSwitch)
      visibility = Visibility.Visible;
    this.updateBrushMode(visibility);
  }

  private void updateBrushMode(Visibility visibility)
  {
    if ((this.VisualizationStyle == ColorSelectionMode.ClassicRGB || this.VisualizationStyle == ColorSelectionMode.ClassicHSV) && this.GradPopup != null)
      this.GradPopup.IsOpen = false;
    if (this.GradBorder != null)
      this.GradBorder.Visibility = visibility;
    if (this.canvasBar != null)
      this.canvasBar.Visibility = visibility;
    if (this.Reverse != null)
      this.Reverse.Visibility = visibility;
    if (this.popupButton != null)
      this.popupButton.Visibility = visibility;
    if (this.gradientGrid == null)
      return;
    this.gradientGrid.Visibility = visibility;
  }

  private void ProcessColorPickingStart(object sender, RoutedEventArgs e)
  {
    this.m_colorBeforeEyeDropStart = this.Color;
    this.isEyeDropper = true;
  }

  private void ProcessColorPickingCancel(object sender, RoutedEventArgs e)
  {
    this.Color = this.m_colorBeforeEyeDropStart;
  }

  public override void OnApplyTemplate()
  {
    if (this.m_systemColors != null)
      this.m_systemColors.SelectionChanged -= new SelectionChangedEventHandler(this.M_systemColors_SelectionChanged);
    if (this.rectBar != null)
      this.rectBar.MouseLeftButtonDown -= new MouseButtonEventHandler(this.rectBar_MouseLeftButtonDown);
    if (this.popupButton != null)
      this.popupButton.Click -= new RoutedEventHandler(this.popupButton_Click);
    if (this.Solid != null)
      this.Solid.Click -= new RoutedEventHandler(this.Solid_Click);
    if (this.Gradient != null)
      this.Gradient.Click -= new RoutedEventHandler(this.Gradient_Click);
    if (this.linear != null)
      this.linear.Click -= new RoutedEventHandler(this.linear_Click);
    if (this.radial != null)
      this.radial.Click -= new RoutedEventHandler(this.radial_Click);
    if (this.Reverse != null)
      this.Reverse.Click -= new RoutedEventHandler(this.Reverse_Click);
    if (this.m_colorPalette != null)
    {
      this.m_colorPalette.MouseLeave -= new MouseEventHandler(this.M_colorPalette_MouseLeave);
      this.m_colorPalette.MouseLeftButtonDown -= new MouseButtonEventHandler(this.OnMouseLeftButtonDown);
      this.m_colorPalette.PreviewMouseMove -= new MouseEventHandler(this.OnMouseMove);
      this.m_colorPalette.MouseLeftButtonUp -= new MouseButtonEventHandler(this.m_colorPalette_MouseLeftButtonUp);
    }
    if (this.SelectedColor != null)
      this.SelectedColor.MouseLeftButtonUp -= new MouseButtonEventHandler(this.SelectedColor_MouseLeftButtonDown);
    if (this.CurrentColor != null)
      this.CurrentColor.MouseLeftButtonUp -= new MouseButtonEventHandler(this.CurrentColor_MouseLeftButtonDown);
    if (this.m_buttomH != null)
      this.m_buttomH.Checked -= new RoutedEventHandler(this.HSVSelected);
    if (this.m_buttomS != null)
      this.m_buttomS.Checked -= new RoutedEventHandler(this.HSVSelected);
    if (this.m_buttomV != null)
      this.m_buttomV.Checked -= new RoutedEventHandler(this.HSVSelected);
    if (this.hval != null)
      this.hval.LostFocus -= new RoutedEventHandler(this.sval_LostFocus);
    if (this.vval != null)
      this.vval.LostFocus -= new RoutedEventHandler(this.sval_LostFocus);
    if (this.sval != null)
      this.sval.LostFocus -= new RoutedEventHandler(this.sval_LostFocus);
    if (this.rval != null)
      this.rval.LostFocus -= new RoutedEventHandler(this.tb_LostFocus);
    if (this.gval != null)
      this.gval.LostFocus -= new RoutedEventHandler(this.tb_LostFocus);
    if (this.bval != null)
      this.bval.LostFocus -= new RoutedEventHandler(this.tb_LostFocus);
    if (this.tb != null)
      this.tb.LostFocus -= new RoutedEventHandler(this.tb_LostFocus);
    if (this.m_colorPalette != null)
      this.m_colorPalette.TouchUp -= new EventHandler<TouchEventArgs>(this.m_colorPalette_TouchUp);
    if (this.m_colorPalette != null)
      this.m_colorPalette.TouchDown -= new EventHandler<TouchEventArgs>(this.m_colorPalette_TouchDown);
    if (this.m_editColorBar != null)
      this.m_editColorBar.ColorChanged -= new PropertyChangedCallback(this.PickerColorBar_ColorChanged);
    if (this.m_colorStringEditor != null)
    {
      this.m_colorStringEditor.LostFocus -= new RoutedEventHandler(this.ColorStringEditor_LostFocus);
      this.m_colorStringEditor.TextChanged -= new TextChangedEventHandler(this.m_colorStringEditor_TextChanged);
      this.m_colorStringEditor.MouseDown -= new MouseButtonEventHandler(this.m_colorStringEditor_MouseDown);
      this.m_colorStringEditor.GotFocus -= new RoutedEventHandler(this.m_colorStringEditor_GotFocus);
      this.m_colorStringEditor.KeyDown -= new KeyEventHandler(this.m_colorStringEditor_KeyDown);
    }
    base.OnApplyTemplate();
    this.m_colorToggleButton = this.FindName("colorToggleButton") as ToggleButton;
    this.m_systemColors = this.FindName("systemColors") as ComboBox;
    if (this.m_systemColors != null)
      this.m_systemColors.SelectionChanged += new SelectionChangedEventHandler(this.M_systemColors_SelectionChanged);
    this.rectBar = this.GetTemplateChild("GradRect") as Rectangle;
    if (this.rectBar != null)
      this.rectBar.MouseLeftButtonDown += new MouseButtonEventHandler(this.rectBar_MouseLeftButtonDown);
    this.Reverse = this.GetTemplateChild("Reverse") as Button;
    this.canvasBar = this.GetTemplateChild("GradientBar") as Canvas;
    this.gradientGrid = this.GetTemplateChild("GridGradient") as Grid;
    this.Gradient = this.GetTemplateChild("Gradient") as Button;
    this.radial = this.GetTemplateChild("radial") as Button;
    this.Solid = this.GetTemplateChild("Solid") as Button;
    if (this.VisualizationStyle == ColorSelectionMode.ClassicRGB || this.VisualizationStyle == ColorSelectionMode.ClassicHSV)
    {
      this.startx = this.GetTemplateChild("startx") as UpDown;
      this.starty = this.GetTemplateChild("starty") as UpDown;
      this.endx = this.GetTemplateChild("endx") as UpDown;
      this.endy = this.GetTemplateChild("endy") as UpDown;
      this.centrex = this.GetTemplateChild("centrex") as UpDown;
      this.centrey = this.GetTemplateChild("centrey") as UpDown;
      this.gradx = this.GetTemplateChild("gradx") as UpDown;
      this.grady = this.GetTemplateChild("grady") as UpDown;
      this.radiusx = this.GetTemplateChild("radiusx") as UpDown;
      this.radiusy = this.GetTemplateChild("radiusy") as UpDown;
      this.linear = this.GetTemplateChild("linear") as Button;
      this.popupButton = this.GetTemplateChild("PopButton") as ToggleButton;
      if (this.popupButton != null)
        this.popupButton.Click += new RoutedEventHandler(this.popupButton_Click);
      this.enableSwitch = this.GetTemplateChild("EnableSwitch") as StackPanel;
      this.GradPopup = this.GetTemplateChild("GradPopup") as Popup;
      this.GradBorder = this.GetTemplateChild("GradPopup") as Border;
      this.RadialGrid = this.GetTemplateChild("RadialGrid") as Grid;
      this.LinearGrid = this.GetTemplateChild("LinearGrid") as Grid;
    }
    if ((this.VisualizationStyle == ColorSelectionMode.RGB || this.VisualizationStyle == ColorSelectionMode.HSV) && this.GetTemplateChild("GradientEditor") is ContentPresenter templateChild)
      templateChild.Loaded += new RoutedEventHandler(this.Content_Loaded);
    if (this.m_colorPicker == null)
    {
      this.window = Window.GetWindow((DependencyObject) this);
      if (this.window != null)
      {
        this.window.MouseDown += new MouseButtonEventHandler(this.Window_MouseDown);
        this.window.PreviewMouseDown += new MouseButtonEventHandler(this.OnPreviewMouseDown);
        this.window.Deactivated += new EventHandler(this.Window_Deactivated);
        this.window.LocationChanged += new EventHandler(this.Window_LocationChanged);
      }
    }
    this.m_wordKnownColorPopup = this.GetTemplateChild("WordKnownColorsPopup") as Popup;
    this.selectorEllipse = this.GetTemplateChild("SelectorEllipse") as Path;
    this.Hcount = 0;
    if (this.Solid != null)
      this.Solid.Click += new RoutedEventHandler(this.Solid_Click);
    if (this.Gradient != null)
      this.Gradient.Click += new RoutedEventHandler(this.Gradient_Click);
    if (this.linear != null)
      this.linear.Click += new RoutedEventHandler(this.linear_Click);
    if (this.radial != null)
      this.radial.Click += new RoutedEventHandler(this.radial_Click);
    if (this.Reverse != null)
      this.Reverse.Click += new RoutedEventHandler(this.Reverse_Click);
    if (this.VisualizationStyle == ColorSelectionMode.ClassicHSV || this.VisualizationStyle == ColorSelectionMode.ClassicRGB)
    {
      if (this.startx != null)
      {
        Binding binding = new Binding();
        binding.Source = (object) this.mys;
        binding.Mode = BindingMode.TwoWay;
        binding.Path = new PropertyPath("X", new object[0]);
        double x = this.mys.X;
        BindingOperations.SetBinding((DependencyObject) this.startx, UpDown.ValueProperty, (BindingBase) binding);
      }
      if (this.starty != null)
        BindingOperations.SetBinding((DependencyObject) this.starty, UpDown.ValueProperty, (BindingBase) new Binding()
        {
          Source = (object) this.mys,
          Mode = BindingMode.TwoWay,
          Path = new PropertyPath("Y", new object[0])
        });
      if (this.endx != null)
        BindingOperations.SetBinding((DependencyObject) this.endx, UpDown.ValueProperty, (BindingBase) new Binding()
        {
          Source = (object) this.mye,
          Mode = BindingMode.TwoWay,
          Path = new PropertyPath("X", new object[0])
        });
      if (this.endy != null)
        BindingOperations.SetBinding((DependencyObject) this.endy, UpDown.ValueProperty, (BindingBase) new Binding()
        {
          Source = (object) this.mye,
          Mode = BindingMode.TwoWay,
          Path = new PropertyPath("Y", new object[0])
        });
      if (this.centrex != null)
        BindingOperations.SetBinding((DependencyObject) this.centrex, UpDown.ValueProperty, (BindingBase) new Binding()
        {
          Source = (object) this.Centre,
          Mode = BindingMode.TwoWay,
          Path = new PropertyPath("X", new object[0])
        });
      if (this.centrey != null)
        BindingOperations.SetBinding((DependencyObject) this.centrey, UpDown.ValueProperty, (BindingBase) new Binding()
        {
          Source = (object) this.Centre,
          Mode = BindingMode.TwoWay,
          Path = new PropertyPath("Y", new object[0])
        });
      if (this.gradx != null)
        BindingOperations.SetBinding((DependencyObject) this.gradx, UpDown.ValueProperty, (BindingBase) new Binding()
        {
          Source = (object) this.Gradientorigin,
          Mode = BindingMode.TwoWay,
          Path = new PropertyPath("X", new object[0])
        });
      if (this.grady != null)
        BindingOperations.SetBinding((DependencyObject) this.grady, UpDown.ValueProperty, (BindingBase) new Binding()
        {
          Source = (object) this.Gradientorigin,
          Mode = BindingMode.TwoWay,
          Path = new PropertyPath("Y", new object[0])
        });
      if (this.radiusx != null)
        BindingOperations.SetBinding((DependencyObject) this.radiusx, UpDown.ValueProperty, (BindingBase) new Binding()
        {
          Source = (object) this,
          Mode = BindingMode.TwoWay,
          Path = new PropertyPath("RadiusX", new object[0])
        });
      if (this.radiusy != null)
        BindingOperations.SetBinding((DependencyObject) this.radiusy, UpDown.ValueProperty, (BindingBase) new Binding()
        {
          Source = (object) this,
          Mode = BindingMode.TwoWay,
          Path = new PropertyPath("RadiusY", new object[0])
        });
    }
    if (this.VisualizationStyle == ColorSelectionMode.HSV || this.VisualizationStyle == ColorSelectionMode.ClassicHSV || this.VisualizationStyle == ColorSelectionMode.RGB)
    {
      this.m_colorPalette = this.GetTemplateChild("ColorPalitte") as FrameworkElement;
      if (this.m_colorPalette != null)
      {
        this.CalculateHSVSelectorPosition();
        this.m_colorPalette.MouseLeave += new MouseEventHandler(this.M_colorPalette_MouseLeave);
        this.m_colorPalette.MouseLeftButtonDown += new MouseButtonEventHandler(this.OnMouseLeftButtonDown);
        this.m_colorPalette.MouseLeftButtonUp += new MouseButtonEventHandler(this.m_colorPalette_MouseLeftButtonUp);
        this.m_colorPalette.PreviewMouseMove += new MouseEventHandler(this.OnMouseMove);
        if (SkinStorage.GetEnableTouch((DependencyObject) this))
        {
          this.m_colorPalette.TouchUp += new EventHandler<TouchEventArgs>(this.m_colorPalette_TouchUp);
          this.m_colorPalette.TouchDown += new EventHandler<TouchEventArgs>(this.m_colorPalette_TouchDown);
        }
      }
      this.m_wordKnownColorsTextBox = this.GetTemplateChild("WordKnownColorsTextBox") as TextBox;
      if (this.VisualizationStyle != ColorSelectionMode.ClassicHSV)
      {
        this.CurrentColor = this.GetTemplateChild("CurrentColor") as Rectangle;
        this.SelectedColor = this.GetTemplateChild("SelectedColor") as Rectangle;
        if (this.SelectedColor != null)
          this.SelectedColor.MouseLeftButtonUp += new MouseButtonEventHandler(this.SelectedColor_MouseLeftButtonDown);
        if (this.CurrentColor != null)
          this.CurrentColor.MouseLeftButtonUp += new MouseButtonEventHandler(this.CurrentColor_MouseLeftButtonDown);
      }
    }
    if (this.VisualizationStyle == ColorSelectionMode.HSV || this.VisualizationStyle == ColorSelectionMode.ClassicHSV)
    {
      if (this.m_buttomH != null && this.m_buttomS != null && this.m_buttomV != null)
      {
        bool? isChecked1 = this.m_buttomH.IsChecked;
        if ((!isChecked1.GetValueOrDefault() ? 0 : (isChecked1.HasValue ? 1 : 0)) != 0)
          this.CurrentHSV = "H";
        bool? isChecked2 = this.m_buttomS.IsChecked;
        if ((!isChecked2.GetValueOrDefault() ? 0 : (isChecked2.HasValue ? 1 : 0)) != 0)
          this.CurrentHSV = "S";
        bool? isChecked3 = this.m_buttomV.IsChecked;
        if ((!isChecked3.GetValueOrDefault() ? 0 : (isChecked3.HasValue ? 1 : 0)) != 0)
          this.CurrentHSV = "V";
      }
      this.m_buttomH = this.GetTemplateChild("ButtomH") as RadioButton;
      this.m_buttomS = this.GetTemplateChild("ButtomS") as RadioButton;
      this.m_buttomV = this.GetTemplateChild("ButtomV") as RadioButton;
      if (this.m_buttomH != null && this.m_buttomS != null && this.m_buttomV != null)
      {
        bool? isChecked4 = this.m_buttomH.IsChecked;
        if ((isChecked4.GetValueOrDefault() ? 0 : (isChecked4.HasValue ? 1 : 0)) != 0)
        {
          bool? isChecked5 = this.m_buttomS.IsChecked;
          if ((isChecked5.GetValueOrDefault() ? 0 : (isChecked5.HasValue ? 1 : 0)) != 0)
          {
            bool? isChecked6 = this.m_buttomV.IsChecked;
            if ((isChecked6.GetValueOrDefault() ? 0 : (isChecked6.HasValue ? 1 : 0)) != 0)
              this.m_buttomH.IsChecked = new bool?(true);
          }
        }
        if (this.CurrentHSV == "H")
          this.m_buttomH.IsChecked = new bool?(true);
        else if (this.CurrentHSV == "S")
          this.m_buttomS.IsChecked = new bool?(true);
        else if (this.CurrentHSV == "V")
          this.m_buttomV.IsChecked = new bool?(true);
      }
      if (this.m_buttomH != null)
        this.m_buttomH.Checked += new RoutedEventHandler(this.HSVSelected);
      if (this.m_buttomS != null)
        this.m_buttomS.Checked += new RoutedEventHandler(this.HSVSelected);
      if (this.m_buttomV != null)
        this.m_buttomV.Checked += new RoutedEventHandler(this.HSVSelected);
      this.hval = this.GetTemplateChild("Hval") as TextBox;
      this.sval = this.GetTemplateChild("Sval") as TextBox;
      this.vval = this.GetTemplateChild("Vval") as TextBox;
      if (this.hval != null)
        this.hval.LostFocus += new RoutedEventHandler(this.sval_LostFocus);
      if (this.vval != null)
        this.vval.LostFocus += new RoutedEventHandler(this.sval_LostFocus);
      if (this.sval != null)
        this.sval.LostFocus += new RoutedEventHandler(this.sval_LostFocus);
      this.SliderH = this.GetTemplateChild("SliderH") as Slider;
      this.SliderS = this.GetTemplateChild("SliderS") as Slider;
      this.SliderV = this.GetTemplateChild("SliderV") as Slider;
      if (this.VisualizationStyle == ColorSelectionMode.ClassicHSV)
      {
        this.tb = this.GetTemplateChild("AlphaVal") as TextBox;
        this.rval = this.GetTemplateChild("Rval") as TextBox;
        this.gval = this.GetTemplateChild("Gval") as TextBox;
        this.bval = this.GetTemplateChild("Bval") as TextBox;
        if (this.tb != null)
          this.tb.LostFocus += new RoutedEventHandler(this.tb_LostFocus);
        if (this.rval != null)
          this.rval.LostFocus += new RoutedEventHandler(this.tb_LostFocus);
        if (this.gval != null)
          this.gval.LostFocus += new RoutedEventHandler(this.tb_LostFocus);
        if (this.bval != null)
          this.bval.LostFocus += new RoutedEventHandler(this.tb_LostFocus);
      }
    }
    else
    {
      this.m_editColorBar = this.GetTemplateChild("PickerColorBar") as ColorBar;
      if (this.m_editColorBar != null)
        this.m_editColorBar.ColorChanged += new PropertyChangedCallback(this.PickerColorBar_ColorChanged);
    }
    this.m_colorStringEditor = this.GetTemplateChild("PART_ColorStringEditor") as TextBox;
    if (this.m_colorStringEditor != null)
    {
      this.m_colorStringEditor.LostFocus += new RoutedEventHandler(this.ColorStringEditor_LostFocus);
      this.m_colorStringEditor.TextChanged += new TextChangedEventHandler(this.m_colorStringEditor_TextChanged);
      this.m_colorStringEditor.MouseDown += new MouseButtonEventHandler(this.m_colorStringEditor_MouseDown);
      this.m_colorStringEditor.GotFocus += new RoutedEventHandler(this.m_colorStringEditor_GotFocus);
      this.m_colorStringEditor.KeyDown += new KeyEventHandler(this.m_colorStringEditor_KeyDown);
    }
    this.gradientItemCollection = (GradientItemCollection) null;
    if (this.VisualizationStyle == ColorSelectionMode.RGB || this.VisualizationStyle == ColorSelectionMode.HSV)
    {
      if (this.IsLoaded && this.ActualWidth > 0.0)
      {
        if (this.rectBar != null)
          this.rectBar.Width = this.ActualWidth - 50.0;
        if (this.canvasBar != null)
          this.canvasBar.Width = this.ActualWidth - 50.0;
      }
    }
    else
      this.canvasBar.Width = this.rectBar.Width;
    this.updateBrushMode(this.BrushMode == BrushModes.Solid ? Visibility.Collapsed : Visibility.Visible);
    if (this.Brush is GradientBrush)
      this.RefreshGradientStoppers(this.Brush);
    if (this.BrushMode == BrushModes.Solid)
    {
      if (this.SelectedColor == null || this.CurrentColor == null)
        return;
      this.CurrentColor.Fill = this.previousSelectedBrush;
      this.SelectedColor.Fill = (Brush) (this.Brush as SolidColorBrush);
    }
    else
    {
      if (this.BrushMode != BrushModes.Gradient || this.gb == null)
        return;
      this.colorchangedInternally = true;
      this.Brush = (Brush) this.gb;
      this.colorchangedInternally = false;
      if (this.m_colorPicker == null)
        return;
      this.flag = true;
      this.m_colorPicker.flag = true;
      this.UpdateVisibility(!this.m_colorPicker.IsGradientPropertyEnabled || !this.m_colorPicker.EnableSolidToGradientSwitch ? Visibility.Collapsed : Visibility.Visible);
    }
  }

  private void M_systemColors_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    if (this.m_colorToggleButton == null)
      return;
    ColorItem selectedValue = (ColorItem) this.m_systemColors.SelectedValue;
    if (this.changeColor)
    {
      if (this.changeHSVBackground)
        this.Color = selectedValue.Brush.Color;
    }
    else if (this.m_colorPicker != null)
      this.m_colorPicker.SelectPaletteColor();
    this.UpdateColorBarSlider();
  }

  private void M_colorPalette_MouseLeave(object sender, MouseEventArgs e)
  {
    this.m_wordKnownColorPopup.IsOpen = false;
  }

  private void Window_MouseDown(object sender, MouseButtonEventArgs e)
  {
    if (this.toggle == null)
      return;
    this.toggle.IsChecked = new bool?(false);
  }

  private void Content_Loaded(object sender, RoutedEventArgs e)
  {
    if (this.GradientPropertyEditorMode != GradientPropertyEditorMode.Popup || (sender as ContentPresenter).ContentTemplate == null)
      return;
    this.content = sender as ContentPresenter;
    this.content.ApplyTemplate();
    this.toggle = this.content.ContentTemplate.FindName("PopButton", (FrameworkElement) this.content) as ToggleButton;
    this.GradPopup = this.content.ContentTemplate.FindName("GradPopup", (FrameworkElement) this.content) as Popup;
    if (this.toggle == null)
      return;
    this.toggle.LostFocus -= new RoutedEventHandler(this.Toggle_LostFocus);
    this.toggle.LostFocus += new RoutedEventHandler(this.Toggle_LostFocus);
  }

  private void Toggle_LostFocus(object sender, RoutedEventArgs e)
  {
    if (this.GradPopup == null || this.GradPopup.IsMouseOver)
      return;
    this.toggle.IsChecked = new bool?(false);
  }

  private void m_colorPalette_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    if (!this.EnableToolTip)
      return;
    if (this.m_wordKnownColorPopup != null)
      this.m_wordKnownColorPopup.IsOpen = false;
    if (this.m_wordKnownColorsTextBox == null)
      return;
    this.m_wordKnownColorsTextBox.Visibility = Visibility.Collapsed;
  }

  private void m_colorPalette_TouchUp(object sender, TouchEventArgs e)
  {
    if (this.selectorEllipse.IsVisible || this.selectorEllipse == null)
      return;
    this.selectorEllipse.Visibility = Visibility.Collapsed;
  }

  private void m_colorPalette_TouchDown(object sender, TouchEventArgs e)
  {
    if (this.selectorEllipse.IsVisible || this.selectorEllipse == null)
      return;
    this.selectorEllipse.Visibility = Visibility.Visible;
  }

  private void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
  {
    if (this.GradPopup == null || !this.GradPopup.IsOpen || e.Source is ColorEdit)
      return;
    this.GradPopup.IsOpen = false;
  }

  private void Window_LocationChanged(object sender, EventArgs e)
  {
    if (this.GradPopup != null && this.GradPopup.IsOpen)
      this.GradPopup.IsOpen = false;
    if (this.toggle == null)
      return;
    this.toggle.IsChecked = new bool?(false);
  }

  private void Window_Deactivated(object sender, EventArgs e)
  {
    if (this.GradPopup != null && this.GradPopup.IsOpen)
      this.GradPopup.IsOpen = false;
    if (this.toggle == null)
      return;
    this.toggle.IsChecked = new bool?(false);
  }

  internal void SelectedColor_MouseLeftButtonDown(object sender, MouseEventArgs args)
  {
    this.SwapColors();
  }

  internal void CurrentColor_MouseLeftButtonDown(object sender, MouseEventArgs args)
  {
    this.SwapColor();
  }

  internal void SwapColors()
  {
    if (this.CurrentColor == null || this.SelectedColor == null)
      return;
    this.CurrentColor.Fill = this.SelectedColor.Fill;
  }

  internal void SwapColor()
  {
    if (this.SelectedColor == null || this.CurrentColor == null)
      return;
    this.CanChange = false;
    this.SelectedColor.Fill = this.CurrentColor.Fill;
    this.Brush = this.SelectedColor.Fill;
    if (this.m_colorPicker == null)
      return;
    this.m_colorPicker.Brush = this.Brush;
  }

  internal void SetBrush(GradientBrush gbrush)
  {
    if (this.gradientItemCollection == null)
      this.gradientItemCollection = new GradientItemCollection();
    if (gbrush != null)
    {
      this.isLinear = gbrush is LinearGradientBrush;
      foreach (GradientStop gradientStop in gbrush.GradientStops)
      {
        GradientStopItem newItem = new GradientStopItem(gradientStop.Color, true, gradientStop.Offset, this);
        if (this.rectBar != null)
          newItem.gradientitem.SetValue(Canvas.TopProperty, (object) (this.rectBar.Height + 4.0));
        else if (this.VisualizationStyle == ColorSelectionMode.RGB || this.VisualizationStyle == ColorSelectionMode.HSV)
          newItem.gradientitem.SetValue(Canvas.TopProperty, (object) 13.0);
        else
          newItem.gradientitem.SetValue(Canvas.TopProperty, (object) 20.0);
        if (this.rectBar != null)
          newItem.gradientitem.SetValue(Canvas.LeftProperty, (object) (gradientStop.Offset * this.rectBar.Width));
        this.gradientItemCollection.Items.Add((object) newItem);
        this.gradientItemCollection.gradientItem = newItem;
      }
    }
    else
    {
      if (!(this.Brush is SolidColorBrush))
        return;
      this.applygradient();
    }
  }

  private void m_colorStringEditor_GotFocus(object sender, RoutedEventArgs e)
  {
    this.mousedown = true;
  }

  private void m_colorStringEditor_MouseDown(object sender, MouseButtonEventArgs e)
  {
    this.mousedown = true;
  }

  private void sval_LostFocus(object sender, RoutedEventArgs e)
  {
    if (!((sender as TextBox).Text == string.Empty))
      return;
    (sender as TextBox).Text = "1.00";
  }

  private void tb_LostFocus(object sender, RoutedEventArgs e)
  {
    if (!((sender as TextBox).Text == string.Empty))
      return;
    (sender as TextBox).Text = "255";
  }

  private void m_colorStringEditor_TextChanged(object sender, TextChangedEventArgs e)
  {
    if (!this.mousedown)
      return;
    this.Edited = true;
  }

  private void mys_PropertyChanged(object sender, PropertyChangedEventArgs e)
  {
    this.Startpoint = new Point((sender as GradientStartPoint).X, (sender as GradientStartPoint).Y);
  }

  private void popupButton_Click(object sender, RoutedEventArgs e)
  {
    if (this.GradPopup == null)
      return;
    if (!this.GradPopup.IsOpen)
    {
      this.GradPopup.IsOpen = true;
      if (this.gradientItemCollection == null || this.gradientItemCollection.Items.Count <= 0)
        return;
      this.gradientGrid.Visibility = Visibility.Visible;
      if (this.isLinear)
      {
        this.LinearGrid.Visibility = Visibility.Visible;
        this.RadialGrid.Visibility = Visibility.Collapsed;
      }
      else
      {
        this.LinearGrid.Visibility = Visibility.Collapsed;
        this.RadialGrid.Visibility = Visibility.Visible;
      }
    }
    else
      this.GradPopup.IsOpen = false;
  }

  private void Gradient_Click(object sender, RoutedEventArgs e)
  {
    this.BrushMode = BrushModes.Gradient;
    if ((this.VisualizationStyle == ColorSelectionMode.HSV || this.VisualizationStyle == ColorSelectionMode.RGB) && this.gradientItemCollection.Items.Count > 0)
    {
      this.isLinear = true;
      this.fillGradient(this.gradientItemCollection.gradientItem);
    }
    this.colorchangedInternally = true;
    this.Brush = (Brush) this.gb;
    this.colorchangedInternally = false;
    if (this.m_colorPicker == null)
      return;
    this.m_colorPicker.flag = true;
    this.m_colorPicker.Brush = (Brush) this.gb;
  }

  private void Solid_Click(object sender, RoutedEventArgs e)
  {
    this.BrushMode = BrushModes.Solid;
    if (this.m_colorPicker == null)
      return;
    this.m_colorPicker.flag = true;
    this.m_colorPicker.BrushMode = BrushModes.Solid;
    this.m_colorPicker.Brush = this.Brush;
  }

  private void Reverse_Click(object sender, RoutedEventArgs e)
  {
    if (this.gradientItemCollection.Items.Count <= 0)
      return;
    this.rev = true;
    this.fillGradient(this.gradientItemCollection.gradientItem);
  }

  private void radial_Click(object sender, RoutedEventArgs e)
  {
    this.BrushMode = BrushModes.Gradient;
    if (this.gradientItemCollection.Items.Count <= 0)
      return;
    this.isLinear = false;
    this.fillGradient(this.gradientItemCollection.gradientItem);
  }

  private void linear_Click(object sender, RoutedEventArgs e)
  {
    this.BrushMode = BrushModes.Gradient;
    if (this.gradientItemCollection.Items.Count <= 0)
      return;
    this.isLinear = true;
    this.fillGradient(this.gradientItemCollection.gradientItem);
  }

  private void rectBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    double off = e.GetPosition((IInputElement) this.canvasBar).X / this.rectBar.Width;
    Point position = e.GetPosition((IInputElement) this.rectBar);
    if (position.X <= this.rectBar.StrokeThickness || position.X >= this.rectBar.Width - this.rectBar.StrokeThickness || position.Y <= this.rectBar.StrokeThickness || position.Y >= this.rectBar.Height - this.rectBar.StrokeThickness)
      return;
    RenderTargetBitmap source = new RenderTargetBitmap((int) this.rectBar.ActualWidth, (int) this.rectBar.ActualHeight, 96.0, 96.0, PixelFormats.Default);
    source.Render((Visual) this.rectBar);
    Color col = new Color();
    if (position.X <= (double) source.PixelWidth && position.Y <= (double) source.PixelHeight)
    {
      CroppedBitmap croppedBitmap = new CroppedBitmap((BitmapSource) source, new Int32Rect((int) position.X, (int) position.Y, 1, 1));
      byte[] pixels = new byte[4];
      croppedBitmap.CopyPixels((Array) pixels, 4, 0);
      col = Color.FromArgb(byte.MaxValue, pixels[2], pixels[1], pixels[0]);
    }
    GradientStopItem gradientStopItem = new GradientStopItem(col, true, off, this);
    this.Focus();
    gradientStopItem.gradientitem.SetValue(Canvas.LeftProperty, (object) e.GetPosition((IInputElement) this.canvasBar).X);
    gradientStopItem.gradientitem.SetValue(Canvas.TopProperty, (object) (this.rectBar.Height + 4.0));
    this.canvasBar.Children.Add((UIElement) gradientStopItem.gradientitem);
    this.gradientItemCollection.Items.Add((object) gradientStopItem);
    this.gradientItemCollection.gradientItem = gradientStopItem;
    this.fillGradient(gradientStopItem);
  }

  protected override void OnKeyDown(KeyEventArgs e)
  {
    if (e.Key == Key.Delete && this.gradientItemCollection.Items.Count > 2)
    {
      for (int index = 0; index < this.gradientItemCollection.Items.Count; ++index)
      {
        if (this.canvasBar.Children != null && this.gradientItemCollection.Items[index] is GradientStopItem && (this.gradientItemCollection.Items[index] as GradientStopItem).isselected)
        {
          this.canvasBar.Children.Remove((UIElement) (this.gradientItemCollection.Items[index] as GradientStopItem).gradientitem);
          this.gradientItemCollection.Items.Remove(this.gradientItemCollection.Items[index]);
          if (index < this.gradientItemCollection.Items.Count)
          {
            (this.gradientItemCollection.Items[index] as GradientStopItem).isselected = true;
            this.fillGradient(this.gradientItemCollection.Items[index] as GradientStopItem);
            return;
          }
          (this.gradientItemCollection.Items[index - 1] as GradientStopItem).isselected = true;
          this.fillGradient(this.gradientItemCollection.Items[index - 1] as GradientStopItem);
          return;
        }
      }
    }
    base.OnKeyDown(e);
  }

  internal void RemoveSelection()
  {
    for (int index = 0; index < this.gradientItemCollection.Items.Count; ++index)
    {
      GradientStopItem gradientStopItem = (GradientStopItem) this.gradientItemCollection.Items[index];
      gradientStopItem.isselected = false;
      gradientStopItem.gradientitem.SetValue(Panel.ZIndexProperty, (object) 0);
    }
  }

  internal void fillGradient(GradientStopItem gradstop)
  {
    if (this.isLinear)
    {
      this.gb = (GradientBrush) new LinearGradientBrush();
      BindingOperations.SetBinding((DependencyObject) (this.gb as LinearGradientBrush), LinearGradientBrush.StartPointProperty, (BindingBase) new Binding()
      {
        Source = (object) this,
        Mode = BindingMode.TwoWay,
        Path = new PropertyPath("Startpoint", new object[0])
      });
      BindingOperations.SetBinding((DependencyObject) (this.gb as LinearGradientBrush), LinearGradientBrush.EndPointProperty, (BindingBase) new Binding()
      {
        Source = (object) this,
        Mode = BindingMode.TwoWay,
        Path = new PropertyPath("Endpoint", new object[0])
      });
    }
    else
    {
      this.gb = (GradientBrush) new RadialGradientBrush();
      BindingOperations.SetBinding((DependencyObject) (this.gb as RadialGradientBrush), RadialGradientBrush.CenterProperty, (BindingBase) new Binding()
      {
        Source = (object) this,
        Mode = BindingMode.TwoWay,
        Path = new PropertyPath("CentrePoint", new object[0])
      });
      BindingOperations.SetBinding((DependencyObject) (this.gb as RadialGradientBrush), RadialGradientBrush.GradientOriginProperty, (BindingBase) new Binding()
      {
        Source = (object) this,
        Mode = BindingMode.TwoWay,
        Path = new PropertyPath("GradientOrigin", new object[0])
      });
      BindingOperations.SetBinding((DependencyObject) (this.gb as RadialGradientBrush), RadialGradientBrush.RadiusXProperty, (BindingBase) new Binding()
      {
        Source = (object) this,
        Mode = BindingMode.TwoWay,
        Path = new PropertyPath("RadiusX", new object[0])
      });
      BindingOperations.SetBinding((DependencyObject) (this.gb as RadialGradientBrush), RadialGradientBrush.RadiusYProperty, (BindingBase) new Binding()
      {
        Source = (object) this,
        Mode = BindingMode.TwoWay,
        Path = new PropertyPath("RadiusY", new object[0])
      });
    }
    if ((this.VisualizationStyle == ColorSelectionMode.ClassicHSV || this.VisualizationStyle == ColorSelectionMode.ClassicRGB) && this.LinearGrid != null && this.RadialGrid != null)
    {
      if (this.isLinear)
      {
        this.LinearGrid.Visibility = Visibility.Visible;
        this.RadialGrid.Visibility = Visibility.Collapsed;
      }
      else
      {
        this.LinearGrid.Visibility = Visibility.Collapsed;
        this.RadialGrid.Visibility = Visibility.Visible;
      }
    }
    for (int index = 0; index < this.gradientItemCollection.Items.Count; ++index)
    {
      GradientStopItem gradientStopItem = (GradientStopItem) this.gradientItemCollection.Items[index];
      GradientStop gradientStop = new GradientStop();
      if (gradstop == gradientStopItem || this.gradientItemCollection.gradientItem == gradientStopItem)
      {
        gradientStopItem.isselected = true;
        this.gradientItemCollection.gradientItem = (GradientStopItem) this.gradientItemCollection.Items[index];
        (gradientStopItem.gradientitem.Children[0] as Control).SetValue(Panel.ZIndexProperty, (object) 1);
      }
      else
      {
        gradientStopItem.isselected = false;
        (gradientStopItem.gradientitem.Children[0] as Control).SetValue(Panel.ZIndexProperty, (object) 0);
      }
      gradientStop.Color = gradientStopItem.color;
      if (this.rev)
      {
        gradientStopItem.offset = 1.0 - gradientStopItem.offset;
        gradientStopItem.gradientitem.SetValue(Canvas.LeftProperty, (object) (gradientStopItem.offset * this.rectBar.Width));
      }
      gradientStop.Offset = gradientStopItem.offset;
      this.gb.GradientStops.Add(gradientStop);
    }
    this.rev = false;
    if (this.rectBar != null)
    {
      LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
      linearGradientBrush.StartPoint = new Point(0.0, 0.5);
      linearGradientBrush.EndPoint = new Point(1.0, 0.5);
      linearGradientBrush.GradientStops = this.gb.GradientStops;
      this.rectBar.Fill = (Brush) linearGradientBrush;
    }
    if (this.BrushMode == BrushModes.Solid)
      return;
    if (!(this.Brush is GradientBrush) || !((this.Brush as GradientBrush).GradientStops.ToString() == this.gb.GradientStops.ToString()) || !((this.Brush as GradientBrush).ToString() == this.gb.ToString()))
    {
      this.colorchangedInternally = true;
      this.Brush = (Brush) this.gb;
      this.colorchangedInternally = false;
    }
    if (this.m_colorPicker == null || this.m_colorPicker.Brush is GradientBrush && (this.m_colorPicker.Brush as GradientBrush).GradientStops.ToString() == this.gb.GradientStops.ToString() && (this.m_colorPicker.Brush as GradientBrush).ToString() == this.gb.ToString())
      return;
    if (!this.bindedmanually)
    {
      this.flag = true;
      this.m_colorPicker.Brush = (Brush) this.gb;
    }
    else
      this.bindedmanually = false;
  }

  private void applygradient()
  {
    if (this.gradientItemCollection == null)
      this.gradientItemCollection = new GradientItemCollection();
    GradientStopItem newItem1 = new GradientStopItem(Colors.Black, true, 1.0, this);
    GradientStopItem newItem2 = new GradientStopItem(this.Color, true, 0.0, this);
    if (this.rectBar != null)
    {
      newItem1.gradientitem.SetValue(Canvas.LeftProperty, (object) this.rectBar.Width);
      newItem1.gradientitem.SetValue(Canvas.TopProperty, (object) (this.rectBar.Height + 4.0));
      newItem2.gradientitem.SetValue(Canvas.LeftProperty, (object) 0.0);
      newItem2.gradientitem.SetValue(Canvas.TopProperty, (object) (this.rectBar.Height + 4.0));
    }
    this.gradientItemCollection.Items.Add((object) newItem1);
    this.gradientItemCollection.Items.Add((object) newItem2);
    this.gradientItemCollection.gradientItem = newItem2;
    if (this.canvasBar != null)
    {
      foreach (GradientStopItem gradientStopItem in (IEnumerable) this.gradientItemCollection.Items)
        this.canvasBar.Children.Add((UIElement) gradientStopItem.gradientitem);
    }
    this.fillGradient(this.gradientItemCollection.gradientItem);
  }

  private void PickerColorBar_ColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    this.Color = (Color) e.NewValue;
  }

  private void m_colorStringEditor_KeyDown(object sender, KeyEventArgs e)
  {
    if (e.Key != Key.Return)
      return;
    this.m_colorStringEditor.GetBindingExpression(TextBox.TextProperty).UpdateSource();
  }

  private void ColorStringEditor_LostFocus(object sender, RoutedEventArgs e)
  {
    Color color = this.Color;
    color = Color.FromArgb(color.A, color.R, color.G, color.B);
    this.m_colorStringEditor.Text = color.ToString();
  }

  private void ColorEdit_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    if (this.VisualizationStyle == ColorSelectionMode.HSV || this.VisualizationStyle == ColorSelectionMode.RGB || this.VisualizationStyle == ColorSelectionMode.ClassicHSV && this.m_colorPalette != null)
    {
      this.CalculateHSVSelectorPosition();
      this.CalculateHSVBackground();
    }
    if (this.VisualizationStyle != ColorSelectionMode.RGB && this.VisualizationStyle != ColorSelectionMode.HSV || this.ActualWidth <= 50.0)
      return;
    if (this.canvasBar != null)
      this.canvasBar.Width = this.ActualWidth - 50.0;
    if (this.rectBar == null)
      return;
    this.rectBar.Width = this.ActualWidth - 50.0;
    if (this.gradientItemCollection == null)
      return;
    foreach (GradientStopItem gradientStopItem in (IEnumerable) this.gradientItemCollection.Items)
      gradientStopItem.gradientitem.SetValue(Canvas.LeftProperty, (object) (gradientStopItem.offset * this.rectBar.Width));
  }

  private void OnVisualizationStyleChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.m_colorPalette != null)
    {
      if (this.VisualizationStyle != ColorSelectionMode.ClassicHSV)
        this.CalculateBackground();
      if (this.VisualizationStyle == ColorSelectionMode.ClassicRGB)
        this.UpdateColorBarSlider();
      if (this.VisualizationStyle == ColorSelectionMode.HSV || this.VisualizationStyle == ColorSelectionMode.ClassicHSV && this.m_colorPalette != null && this.m_bNeedChangeHSV)
        this.CalculateHSVSelectorPosition();
    }
    if (this.VisualizationStyle == ColorSelectionMode.HSV)
      this.CalculateHSVBackground();
    if (this.VisualizationStyleChanged != null)
      this.VisualizationStyleChanged((DependencyObject) this, e);
    if (this.gradientItemCollection == null || this.gradientItemCollection.Items.Count == 0 || this.canvasBar == null)
      return;
    this.canvasBar.Children.Clear();
  }

  private void OnColorChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.m_colorPalette == null)
      this.m_color = (Color) e.NewValue;
    else if (this.Edited || this.mousedown)
    {
      this.m_color = (Color) e.NewValue;
      this.mousedown = false;
      this.Edited = false;
    }
    else if (!this.loaded && this.VisualizationStyle == ColorSelectionMode.HSV || this.VisualizationStyle == ColorSelectionMode.ClassicHSV)
    {
      this.m_color = this.Color;
      if (this.rgbChanged)
        this.m_color = (Color) e.NewValue;
      else
        this.Color = this.m_color;
      this.loaded = true;
    }
    else
    {
      this.m_color = (Color) e.NewValue;
      if (this.VisualizationStyle == ColorSelectionMode.ClassicRGB && (double) this.A_value != (double) byte.MaxValue && (double) this.A_value != 0.0)
        this.m_color.ScA = this.A_value;
      int num = this.blackWhitePressed ? 1 : 0;
      this.blackWhitePressed = false;
      this.Color = this.m_color;
      this.breakLoop = false;
    }
    if (!this.breakLoop || this.VisualizationStyle == ColorSelectionMode.HSV || this.VisualizationStyle == ColorSelectionMode.ClassicHSV || this.VisualizationStyle == ColorSelectionMode.ClassicRGB || this.VisualizationStyle == ColorSelectionMode.RGB)
      this.ChangetheColorValues(e);
    else
      this.breakLoop = false;
  }

  private void ChangetheColorValues(DependencyPropertyChangedEventArgs e)
  {
    if (this.m_colorPicker != null)
      this.BrushMode = this.m_colorPicker.BrushMode;
    if (this.BrushMode != BrushModes.Gradient)
    {
      SolidColorBrush solidColorBrush = new SolidColorBrush(this.m_color);
      if (this.Brush == null || solidColorBrush.ToString() != this.Brush.ToString())
        this.Brush = (Brush) solidColorBrush;
      if (this.m_colorPicker != null)
      {
        this.m_colorPicker.flag = true;
        this.flag = true;
        this.m_colorPicker.Brush = this.Brush;
      }
    }
    this.Edited = false;
    this.m_bColorUpdating = true;
    this.m_r = this.m_color.ScR;
    this.m_g = this.m_color.ScG;
    this.m_b = this.m_color.ScB;
    this.m_a = this.m_color.ScA;
    this.R = this.m_color.ScR;
    this.G = this.m_color.ScG;
    this.B = this.m_color.ScB;
    this.A = this.m_color.ScA;
    HsvColor hsv = HsvColor.ConvertRgbToHsv((int) this.m_color.R, (int) this.m_color.B, (int) this.m_color.G);
    this.S = (float) hsv.S;
    this.V = (float) hsv.V;
    if (this.VisualizationStyle != ColorSelectionMode.ClassicHSV)
    {
      this.CalculateHSVSelectorPosition();
      this.CalculateBackground();
    }
    if (this.VisualizationStyle == ColorSelectionMode.ClassicHSV && this.m_colorPalette != null && this.m_bNeedChangeHSV)
    {
      this.CalculateHSVSelectorPosition();
      this.CalculateHSVBackground();
    }
    this.InvertColor = Color.FromRgb((byte) ((uint) byte.MaxValue - (uint) this.m_color.R), (byte) ((uint) byte.MaxValue - (uint) this.m_color.G), (byte) ((uint) byte.MaxValue - (uint) this.m_color.B));
    if (this.ColorChanged != null && (double) this.A == (double) ((Color) e.NewValue).ScA)
      this.ColorChanged((DependencyObject) this, e);
    this.m_bColorUpdating = false;
    if (!this.setnocolor)
    {
      if (this.gradientItemCollection != null && this.gradientItemCollection.gradientItem != null)
      {
        this.gradientItemCollection.gradientItem.color = this.Color;
        this.fillGradient(this.gradientItemCollection.gradientItem);
      }
    }
    else
      this.setnocolor = false;
    if (this.m_systemColors != null)
    {
      this.obj = this.m_systemColors;
      if (this.obj != null)
      {
        this.obj.DropDownOpened += new EventHandler(this.obj_DropDownOpened);
        IList<ColorItem> itemsSource = this.obj.ItemsSource as IList<ColorItem>;
        int num = -1;
        if (itemsSource != null)
        {
          int index = 0;
          for (int count = itemsSource.Count; index < count; ++index)
          {
            if (itemsSource[index].Name == ColorEdit.SuchColor((Color) e.NewValue)[0])
            {
              num = index;
              this.count = 0;
              this.allow = true;
              break;
            }
          }
          if (num != -1 && this.obj.SelectedIndex != num)
            this.obj.SelectedIndex = num;
        }
        else
          this.count = 0;
      }
    }
    this.UpdateColorBarSlider();
    this.breakLoop = true;
  }

  private void obj_DropDownOpened(object sender, EventArgs e)
  {
    this.changeHSVBackground = true;
    this.changeColor = true;
    this.Hcount = 0;
  }

  internal void CalculateHSVSelectorPosition()
  {
    HsvColor hsv = HsvColor.ConvertRgbToHsv((int) this.m_color.R, (int) this.m_color.B, (int) this.m_color.G);
    if (this.changeHSVBackground)
    {
      if (hsv.H != 0.0 || this.m_color.R != (byte) 0 || this.m_color.G != (byte) 0 || this.m_color.B != (byte) 0)
      {
        if (hsv.H == 0.0)
        {
          if (this.m_colorPicker == null && this.m_color.R != (byte) 0)
          {
            this.H = (float) hsv.H;
          }
          else
          {
            this.H = (float) hsv.H;
            if (this.SelectedColor != null)
              this.SelectedColor.Fill = (Brush) new SolidColorBrush(this.Color);
            this.Hcount = 0;
          }
        }
        else if (this.Hcount == 0)
        {
          this.H = (float) hsv.H;
          ++this.Hcount;
        }
      }
      if (hsv.S != 0.0)
        this.S = (float) hsv.S;
      if (hsv.V != 0.0)
        this.V = (float) hsv.V;
    }
    else if (this.m_colorPicker == null)
    {
      if ((double) this.H == 0.0)
        this.H = (float) hsv.H;
      else if ((this.m_color.R != (byte) 0 || this.m_color.G != (byte) 0 || this.m_color.B != (byte) 0) && !this.mouseLeftDown)
        this.H = (float) hsv.H;
      if ((double) this.S == 0.0)
        this.S = (float) hsv.S;
      if ((double) this.V == 0.0)
        this.V = (float) hsv.V;
    }
    else
    {
      if ((hsv.H != 0.0 || (int) this.m_color.R > (int) this.m_color.G && (int) this.m_color.R > (int) this.m_color.B) && this.Hcount == 0)
      {
        if ((double) this.H == 0.0)
          this.H = (float) hsv.H;
        else if ((this.m_color.R != (byte) 0 || this.m_color.G != (byte) 0 || this.m_color.B != (byte) 0) && !this.mouseLeftDown)
          this.H = (float) hsv.H;
        ++this.Hcount;
      }
      if (hsv.S != 0.0 && (double) this.S == 0.0)
        this.S = (float) hsv.S;
      if (hsv.V != 0.0)
        this.V = (float) hsv.V;
    }
    if (this.isEyeDropper)
    {
      this.H = (float) hsv.H;
      this.S = (float) hsv.S;
    }
    Point point = new Point(0.0, 0.0);
    switch (this.HSV)
    {
      case HSV.H:
        point.X = (double) this.GetXPositionForH();
        point.Y = (double) this.GetYPositionForHS();
        this.SliderValueHSV = this.H;
        break;
      case HSV.S:
        point.X = (double) this.GetXPositionForSV();
        point.Y = (double) this.GetYPositionForHS();
        this.SliderValueHSV = this.S;
        break;
      case HSV.V:
        point.X = (double) this.GetXPositionForSV();
        point.Y = (double) this.GetYPositionForV();
        this.SliderValueHSV = this.V;
        break;
    }
    this.CalculateWordKnownColorsPosition(point);
  }

  private float GetXPositionForSV()
  {
    return this.m_colorPalette != null ? (float) ((double) this.H / 3.5999999046325684 * (this.m_colorPalette.ActualWidth / 100.0)) : 0.0f;
  }

  private float GetYPositionForV()
  {
    return this.m_colorPalette != null ? (float) this.m_colorPalette.ActualHeight - this.S * (float) this.m_colorPalette.ActualHeight : 0.0f;
  }

  private float GetYPositionForHS()
  {
    return this.m_colorPalette != null ? (float) this.m_colorPalette.ActualHeight - this.V * (float) this.m_colorPalette.ActualHeight : 0.0f;
  }

  private float GetXPositionForH()
  {
    return this.m_colorPalette != null ? this.S * (float) this.m_colorPalette.ActualWidth : 0.0f;
  }

  private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    this.changeHSVBackground = false;
    this.Hcount = 0;
    this.mouseLeftDown = true;
    this.Focus();
    Point position = e.GetPosition((IInputElement) this.m_colorPalette);
    if (this.SelectedColor != null)
      this.previousSelectedBrush = (Brush) new SolidColorBrush(this.Color);
    if (this.m_colorPalette == null)
      return;
    this.m_bColorUpdating = true;
    this.m_bNeedChangeHSV = false;
    Point colorHsv = this.FindColorHSV(position);
    this.m_bNeedChangeHSV = true;
    if ((double) this.A == 0.0)
    {
      this.Color = HsvColor.ConvertHsvToRgb((double) this.H, (double) this.S, (double) this.V);
    }
    else
    {
      Color rgb = HsvColor.ConvertHsvToRgb((double) this.H, (double) this.S, (double) this.V);
      this.Color = Color.FromArgb(this.Color.A, rgb.R, rgb.G, rgb.B);
    }
    this.CalculateWordKnownColorsPosition(colorHsv);
    if (SkinStorage.GetEnableTouch((DependencyObject) this) && this.selectorEllipse != null)
      this.selectorEllipse.Visibility = Visibility.Visible;
    ++this.count;
  }

  private void OnMouseMove(object sender, MouseEventArgs e)
  {
    if (e.LeftButton == MouseButtonState.Pressed && this.mouseLeftDown)
    {
      if (this.SelectedColor != null)
        this.previousSelectedBrush = (Brush) new SolidColorBrush(this.Color);
      this.changeHSVBackground = false;
      this.mouseLeftDown = true;
      this.Hcount = 0;
      Point point = e.GetPosition((IInputElement) this.m_colorPalette);
      if (point.X > this.m_colorPalette.ActualWidth)
        point.X = this.m_colorPalette.ActualWidth;
      if (point.X < 0.0)
        point.X = 0.0;
      if (point.Y > this.m_colorPalette.ActualHeight)
        point.Y = this.m_colorPalette.ActualHeight;
      if (point.Y < 0.0)
        point.Y = 0.0;
      this.m_bColorUpdating = true;
      if (point.X < 0.0 || point.X > this.m_colorPalette.ActualWidth || point.Y < 0.0 || point.Y > this.m_colorPalette.ActualHeight)
        return;
      this.m_bNeedChangeHSV = false;
      point = this.FindColorHSV(point);
      this.m_bNeedChangeHSV = true;
      Color rgb = HsvColor.ConvertHsvToRgb((double) this.H, (double) this.S, (double) this.V);
      this.Color = Color.FromArgb(this.Color.A, rgb.R, rgb.G, rgb.B);
      if (this.EnableToolTip && this.m_wordKnownColorPopup != null)
        this.m_wordKnownColorPopup.IsOpen = true;
      this.CalculateWordKnownColorsPosition(point);
    }
    else
      this.mouseLeftDown = false;
  }

  private Point FindColorHSV(Point p)
  {
    switch (this.HSV)
    {
      case HSV.H:
        this.FindColorH(p);
        break;
      case HSV.S:
        this.FindColorS(p);
        break;
      case HSV.V:
        this.FindColorV(p);
        break;
      default:
        this.FindColorH(p);
        break;
    }
    return p;
  }

  protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    if ((this.VisualizationStyle == ColorSelectionMode.ClassicRGB || this.VisualizationStyle == ColorSelectionMode.ClassicHSV) && this.m_colorPalette != null && this.m_colorPalette.IsMouseOver)
      this.m_colorPalette.CaptureMouse();
    base.OnMouseLeftButtonDown(e);
  }

  protected override void OnLostFocus(RoutedEventArgs e)
  {
    if (this.EnableToolTip)
    {
      if (this.m_wordKnownColorPopup != null)
        this.m_wordKnownColorPopup.IsOpen = false;
      if (this.m_wordKnownColorsTextBox != null)
        this.m_wordKnownColorsTextBox.Visibility = Visibility.Collapsed;
    }
    base.OnLostFocus(e);
  }

  protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    if (this.mouseLeftDown)
      this.mouseLeftDown = false;
    if (this.m_colorPalette != null && this.m_colorPalette.IsMouseOver)
      this.m_colorPalette.ReleaseMouseCapture();
    if (SkinStorage.GetEnableTouch((DependencyObject) this) && this.selectorEllipse != null)
      this.selectorEllipse.Visibility = Visibility.Collapsed;
    if (this.EnableToolTip)
    {
      if (this.m_wordKnownColorPopup != null)
        this.m_wordKnownColorPopup.IsOpen = false;
      if (this.m_wordKnownColorsTextBox != null)
        this.m_wordKnownColorsTextBox.Visibility = Visibility.Collapsed;
    }
    base.OnMouseLeftButtonUp(e);
  }

  protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
  {
    base.OnPropertyChanged(e);
    if (e.Property == ColorEdit.StartpointProperty)
      return;
    DependencyProperty property = e.Property;
    DependencyProperty radiusXproperty = ColorEdit.RadiusXProperty;
  }

  private void CalculateWordKnownColorsPosition(Point point)
  {
    if (this.m_wordKnownColorsTextBox == null)
      return;
    this.SelectorPositionX = (float) point.X - 5f;
    this.SelectorPositionY = (float) point.Y - 5f;
    if (SkinStorage.GetEnableTouch((DependencyObject) this))
    {
      this.SelectorPositionX = (float) point.X - 45f;
      this.SelectorPositionY = (float) point.Y - 45f;
      if (point.Y < 0.0)
        this.SelectorPositionY = -45f;
      if (point.X < 0.0)
        this.SelectorPositionX = -45f;
      if (point.X > this.m_colorPalette.ActualWidth)
        this.SelectorPositionX = (float) (this.m_colorPalette.ActualWidth + 45.0);
      if (point.Y > this.m_colorPalette.ActualHeight)
        this.SelectorPositionY = (float) (this.m_colorPalette.ActualHeight + 45.0);
    }
    if (this.m_colorPicker != null && !this.m_colorPicker.EnableToolTip)
    {
      if (this.m_colorPicker.m_colorEditor.m_wordKnownColorPopup != null)
        this.m_colorPicker.m_colorEditor.m_wordKnownColorPopup.IsOpen = false;
      this.m_wordKnownColorsTextBox.Visibility = Visibility.Collapsed;
    }
    else if (!this.EnableToolTip)
    {
      if (this.m_wordKnownColorPopup != null)
        this.m_wordKnownColorPopup.IsOpen = false;
      this.m_wordKnownColorsTextBox.Visibility = Visibility.Collapsed;
    }
    else
    {
      double actualWidth = this.m_wordKnownColorsTextBox.ActualWidth;
      double actualHeight = this.m_wordKnownColorsTextBox.ActualHeight;
      this.m_wordKnownColorsTextBox.Visibility = Visibility.Visible;
      this.WordKnownColorsPositionX = point.X - actualWidth - 5.0 <= 0.0 ? (float) point.X + 5f : (float) (point.X - actualWidth - 5.0);
      this.WordKnownColorsPositionY = point.Y - actualHeight - 5.0 <= 0.0 ? (float) point.Y + 5f : (float) (point.Y - actualHeight - 5.0);
      if (this.VisualizationStyle != ColorSelectionMode.ClassicHSV && this.VisualizationStyle != ColorSelectionMode.ClassicRGB)
        return;
      this.m_wordKnownColorsTextBox.ToolTip = (object) this.GetColorsTooltip();
    }
  }

  private System.Windows.Controls.ToolTip GetColorsTooltip()
  {
    string[] strArray = ColorEdit.SuchColor(this.Color);
    System.Windows.Controls.ToolTip colorsTooltip = new System.Windows.Controls.ToolTip();
    colorsTooltip.Background = (Brush) new SolidColorBrush(Colors.Transparent);
    StackPanel stackPanel = new StackPanel();
    stackPanel.Orientation = Orientation.Vertical;
    SolidColorBrush solidColorBrush = new SolidColorBrush(Color.FromArgb((byte) 125, byte.MaxValue, byte.MaxValue, byte.MaxValue));
    TextBox element1 = new TextBox();
    element1.Background = (Brush) solidColorBrush;
    element1.BorderThickness = new Thickness(0.0);
    element1.Text = "Such in Red:" + strArray[1];
    TextBox element2 = new TextBox();
    element2.Background = (Brush) solidColorBrush;
    element1.BorderThickness = new Thickness(0.0);
    element2.Text = "Such in Green:" + strArray[2];
    TextBox element3 = new TextBox();
    element3.Background = (Brush) solidColorBrush;
    element1.BorderThickness = new Thickness(0.0);
    element3.Text = "Such in Blue:" + strArray[3];
    stackPanel.Children.Add((UIElement) element1);
    stackPanel.Children.Add((UIElement) element2);
    stackPanel.Children.Add((UIElement) element3);
    colorsTooltip.Content = (object) stackPanel;
    return colorsTooltip;
  }

  private void FindColorH(Point p)
  {
    this.V = (float) (1.0 - p.Y / this.m_colorPalette.ActualHeight);
    this.S = (float) (p.X / this.m_colorPalette.ActualWidth);
  }

  private void FindColorS(Point p)
  {
    this.H = (float) (360.0 * p.X / this.m_colorPalette.ActualWidth);
    this.V = (float) (1.0 - p.Y / this.m_colorPalette.ActualHeight);
  }

  private void FindColorV(Point p)
  {
    this.H = (float) (360.0 * p.X / this.m_colorPalette.ActualWidth);
    this.S = (float) (1.0 - p.Y / this.m_colorPalette.ActualHeight);
  }

  internal void CalculateBackground()
  {
    Color startColor = Color.FromScRgb(this.m_a, 0.0f, this.m_g, this.m_b);
    Color endColor = Color.FromScRgb(this.m_a, 1f, this.m_g, this.m_b);
    if (this.VisualizationStyle == ColorSelectionMode.ClassicRGB || this.VisualizationStyle == ColorSelectionMode.ClassicHSV)
    {
      this.BackgroundR = (Brush) new LinearGradientBrush(startColor, endColor, 0.0);
      this.BackgroundG = (Brush) new LinearGradientBrush(Color.FromScRgb(this.m_a, this.m_r, 0.0f, this.m_b), Color.FromScRgb(this.m_a, this.m_r, 1f, this.m_b), 0.0);
      this.BackgroundB = (Brush) new LinearGradientBrush(Color.FromScRgb(this.m_a, this.m_r, this.m_g, 0.0f), Color.FromScRgb(this.m_a, this.m_r, this.m_g, 1f), 0.0);
    }
    this.BackgroundA = (Brush) new LinearGradientBrush(Color.FromScRgb(1f, this.m_r, this.m_g, this.m_b), Color.FromScRgb(0.0f, this.m_r, this.m_g, this.m_b), 90.0);
  }

  public void CalculateHSVBackground()
  {
    if (this.SliderH == null || this.SliderS == null || this.SliderV == null)
      return;
    LinearGradientBrush linearGradientBrush1 = new LinearGradientBrush();
    linearGradientBrush1.GradientStops.Add(new GradientStop()
    {
      Color = this.Color,
      Offset = 0.0
    });
    linearGradientBrush1.GradientStops.Add(new GradientStop()
    {
      Color = this.Color,
      Offset = 1.0
    });
    linearGradientBrush1.StartPoint = new Point(0.0, 360.0);
    linearGradientBrush1.EndPoint = new Point(360.0, 360.0);
    this.SliderH.Background = (Brush) linearGradientBrush1;
    LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush();
    linearGradientBrush2.GradientStops.Add(new GradientStop()
    {
      Color = Colors.White,
      Offset = 0.0
    });
    linearGradientBrush2.GradientStops.Add(new GradientStop()
    {
      Color = this.Color,
      Offset = 1.0
    });
    linearGradientBrush2.StartPoint = new Point(0.0, 1.0);
    linearGradientBrush2.EndPoint = new Point(1.0, 1.0);
    this.SliderS.Background = (Brush) linearGradientBrush2;
    LinearGradientBrush linearGradientBrush3 = new LinearGradientBrush();
    linearGradientBrush3.GradientStops.Add(new GradientStop()
    {
      Color = Colors.Black,
      Offset = 0.0
    });
    linearGradientBrush3.GradientStops.Add(new GradientStop()
    {
      Color = this.Color,
      Offset = 1.0
    });
    linearGradientBrush3.StartPoint = new Point(0.0, 1.0);
    linearGradientBrush3.EndPoint = new Point(1.0, 1.0);
    this.SliderV.Background = (Brush) linearGradientBrush3;
  }

  internal void Dispose(bool disposing)
  {
    if (!disposing)
      return;
    this.Loaded -= new RoutedEventHandler(this.ColorEdit_Loaded);
    if (this.mys != null)
    {
      this.mys.Dispose();
      this.mys.PropertyChanged -= new PropertyChangedEventHandler(this.mys_PropertyChanged);
      this.mys = (GradientStartPoint) null;
    }
    if (this.mye != null)
    {
      this.mye.Dispose();
      this.mye.PropertyChanged -= new PropertyChangedEventHandler(this.mye_PropertyChanged);
      this.mye = (GradientStartPoint) null;
    }
    if (this.Centre != null)
    {
      this.Centre.Dispose();
      this.Centre.PropertyChanged -= new PropertyChangedEventHandler(this.Centre_PropertyChanged);
      this.Centre = (GradientStartPoint) null;
    }
    if (this.Gradientorigin != null)
    {
      this.Gradientorigin.Dispose();
      this.Gradientorigin.PropertyChanged -= new PropertyChangedEventHandler(this.Gradientorigin_PropertyChanged);
      this.Gradientorigin = (GradientStartPoint) null;
    }
    this.RemoveHandler(BorderEyeDrop.BeginColorPickingEvent, (Delegate) new RoutedEventHandler(this.ProcessColorPickingStart));
    this.RemoveHandler(BorderEyeDrop.CancelColorPickingEvent, (Delegate) new RoutedEventHandler(this.ProcessColorPickingCancel));
    if (this.colorWhiteBinding != null)
    {
      this.colorWhiteBinding.Executed -= new ExecutedRoutedEventHandler(this.ChangeColorWhite);
      this.CommandBindings.Remove(this.colorWhiteBinding);
      this.colorWhiteBinding = (CommandBinding) null;
    }
    if (this.colorBlackBinding != null)
    {
      this.colorBlackBinding.Executed -= new ExecutedRoutedEventHandler(this.ChangeColorBlack);
      this.CommandBindings.Remove(this.colorBlackBinding);
      this.colorBlackBinding = (CommandBinding) null;
    }
    this.SizeChanged -= new SizeChangedEventHandler(this.ColorEdit_SizeChanged);
    if (this.rectBar != null)
    {
      this.rectBar.MouseLeftButtonDown -= new MouseButtonEventHandler(this.rectBar_MouseLeftButtonDown);
      this.rectBar = (Rectangle) null;
    }
    if (this.popupButton != null)
    {
      this.popupButton.Click -= new RoutedEventHandler(this.popupButton_Click);
      this.popupButton = (ToggleButton) null;
    }
    if (this.window != null)
    {
      this.window.MouseDown -= new MouseButtonEventHandler(this.Window_MouseDown);
      this.window.PreviewMouseDown -= new MouseButtonEventHandler(this.OnPreviewMouseDown);
      this.window.Deactivated -= new EventHandler(this.Window_Deactivated);
      this.window.LocationChanged -= new EventHandler(this.Window_LocationChanged);
      this.window = (Window) null;
    }
    if (this.content != null)
      this.content.Loaded -= new RoutedEventHandler(this.Content_Loaded);
    if (this.toggle != null)
      this.toggle.LostFocus -= new RoutedEventHandler(this.Toggle_LostFocus);
    if (this.Solid != null)
    {
      this.Solid.Click -= new RoutedEventHandler(this.Solid_Click);
      this.Solid = (Button) null;
    }
    if (this.Gradient != null)
    {
      this.Gradient.Click -= new RoutedEventHandler(this.Gradient_Click);
      this.Gradient = (Button) null;
    }
    if (this.linear != null)
    {
      this.linear.Click -= new RoutedEventHandler(this.linear_Click);
      this.linear = (Button) null;
    }
    if (this.radial != null)
    {
      this.radial.Click -= new RoutedEventHandler(this.radial_Click);
      this.radial = (Button) null;
    }
    if (this.Reverse != null)
    {
      this.Reverse.Click -= new RoutedEventHandler(this.Reverse_Click);
      this.Reverse = (Button) null;
    }
    if (this.m_colorPalette != null)
    {
      this.m_colorPalette.MouseLeave -= new MouseEventHandler(this.M_colorPalette_MouseLeave);
      this.m_colorPalette.MouseLeftButtonDown -= new MouseButtonEventHandler(this.OnMouseLeftButtonDown);
      this.m_colorPalette.MouseLeftButtonUp -= new MouseButtonEventHandler(this.m_colorPalette_MouseLeftButtonUp);
      this.m_colorPalette.PreviewMouseMove -= new MouseEventHandler(this.OnMouseMove);
      this.m_colorPalette.TouchUp -= new EventHandler<TouchEventArgs>(this.m_colorPalette_TouchUp);
      this.m_colorPalette.TouchDown -= new EventHandler<TouchEventArgs>(this.m_colorPalette_TouchDown);
      this.m_colorPalette = (FrameworkElement) null;
    }
    if (this.SelectedColor != null)
    {
      this.SelectedColor.MouseLeftButtonUp -= new MouseButtonEventHandler(this.SelectedColor_MouseLeftButtonDown);
      this.SelectedColor = (Rectangle) null;
    }
    if (this.CurrentColor != null)
    {
      this.CurrentColor.MouseLeftButtonUp -= new MouseButtonEventHandler(this.CurrentColor_MouseLeftButtonDown);
      this.CurrentColor = (Rectangle) null;
    }
    if (this.m_buttomH != null)
    {
      this.m_buttomH.Checked -= new RoutedEventHandler(this.HSVSelected);
      this.m_buttomH = (RadioButton) null;
    }
    if (this.m_buttomS != null)
    {
      this.m_buttomS.Checked -= new RoutedEventHandler(this.HSVSelected);
      this.m_buttomS = (RadioButton) null;
    }
    if (this.m_buttomV != null)
    {
      this.m_buttomV.Checked -= new RoutedEventHandler(this.HSVSelected);
      this.m_buttomV = (RadioButton) null;
    }
    if (this.hval != null)
    {
      this.hval.LostFocus -= new RoutedEventHandler(this.sval_LostFocus);
      this.hval = (TextBox) null;
    }
    if (this.vval != null)
    {
      this.vval.LostFocus -= new RoutedEventHandler(this.sval_LostFocus);
      this.vval = (TextBox) null;
    }
    if (this.sval != null)
    {
      this.sval.LostFocus -= new RoutedEventHandler(this.sval_LostFocus);
      this.sval = (TextBox) null;
    }
    if (this.tb != null)
    {
      this.tb.LostFocus -= new RoutedEventHandler(this.tb_LostFocus);
      this.tb = (TextBox) null;
    }
    if (this.rval != null)
    {
      this.rval.LostFocus -= new RoutedEventHandler(this.tb_LostFocus);
      this.rval = (TextBox) null;
    }
    if (this.gval != null)
    {
      this.gval.LostFocus -= new RoutedEventHandler(this.tb_LostFocus);
      this.gval = (TextBox) null;
    }
    if (this.bval != null)
    {
      this.bval.LostFocus -= new RoutedEventHandler(this.tb_LostFocus);
      this.bval = (TextBox) null;
    }
    if (this.m_colorStringEditor != null)
    {
      BindingOperations.ClearAllBindings((DependencyObject) this.m_colorStringEditor);
      this.m_colorStringEditor.LostFocus -= new RoutedEventHandler(this.ColorStringEditor_LostFocus);
      this.m_colorStringEditor.TextChanged -= new TextChangedEventHandler(this.m_colorStringEditor_TextChanged);
      this.m_colorStringEditor.MouseDown -= new MouseButtonEventHandler(this.m_colorStringEditor_MouseDown);
      this.m_colorStringEditor.GotFocus -= new RoutedEventHandler(this.m_colorStringEditor_GotFocus);
      this.m_colorStringEditor.KeyDown -= new KeyEventHandler(this.m_colorStringEditor_KeyDown);
      this.m_colorStringEditor = (TextBox) null;
    }
    if (this.startx != null)
    {
      this.startx.Dispose();
      this.startx = (UpDown) null;
    }
    if (this.starty != null)
    {
      this.starty.Dispose();
      this.starty = (UpDown) null;
    }
    if (this.endx != null)
    {
      this.endx.Dispose();
      this.endx = (UpDown) null;
    }
    if (this.endy != null)
    {
      this.endy.Dispose();
      this.endy = (UpDown) null;
    }
    if (this.centrex != null)
    {
      this.centrex.Dispose();
      this.centrex = (UpDown) null;
    }
    if (this.centrey != null)
    {
      this.centrey.Dispose();
      this.centrey = (UpDown) null;
    }
    if (this.gradx != null)
    {
      this.gradx.Dispose();
      this.gradx = (UpDown) null;
    }
    if (this.grady != null)
    {
      this.grady.Dispose();
      this.grady = (UpDown) null;
    }
    if (this.radiusx != null)
    {
      this.radiusx.Dispose();
      this.radiusx = (UpDown) null;
    }
    if (this.radiusy != null)
    {
      this.radiusy.Dispose();
      this.radiusy = (UpDown) null;
    }
    if (this.obj != null)
    {
      this.obj.DropDownOpened -= new EventHandler(this.obj_DropDownOpened);
      if (this.obj.ItemsSource is IList itemsSource)
      {
        for (int index = 0; index < itemsSource.Count; ++index)
          (itemsSource[0] as ColorItem).Dispose();
      }
      this.obj.ItemsSource = (IEnumerable) new ObservableCollection<ColorItem>();
      if (this.obj.ItemsSource == null)
        this.obj.Items.Clear();
      this.obj = (ComboBox) null;
    }
    if (this.m_systemColors != null)
    {
      this.m_systemColors.SelectionChanged -= new SelectionChangedEventHandler(this.M_systemColors_SelectionChanged);
      this.m_systemColors.ItemsSource = (IEnumerable) new ObservableCollection<ColorItem>();
      if (this.m_systemColors.ItemsSource == null)
        this.m_systemColors.Items.Clear();
      this.m_systemColors = (ComboBox) null;
    }
    if (this.previousSelectedBrush != null)
      this.previousSelectedBrush = (Brush) null;
    if (this.GradientPropertyEditorModeChanged != null)
      this.GradientPropertyEditorModeChanged = (PropertyChangedCallback) null;
    if (this.gradientItemCollection != null)
    {
      for (int index = 0; index < this.gradientItemCollection.Items.Count; ++index)
      {
        if (this.gradientItemCollection.Items[index] is GradientStopItem)
          (this.gradientItemCollection.Items[index] as GradientStopItem).Dispose();
      }
      if (this.gradientItemCollection != null)
      {
        this.gradientItemCollection.gradientItem.Dispose();
        this.gradientItemCollection.gradientItem = (GradientStopItem) null;
      }
      this.gradientItemCollection = (GradientItemCollection) null;
    }
    if (this.SliderH != null)
      this.SliderH = (Slider) null;
    if (this.SliderS != null)
      this.SliderS = (Slider) null;
    if (this.SliderV != null)
      this.SliderV = (Slider) null;
    if (this.gradientGrid != null)
      this.gradientGrid = (Grid) null;
    if (this.canvasBar != null)
      this.canvasBar = (Canvas) null;
    if (this.gb != null)
      this.gb = (GradientBrush) null;
    if (this.LinearGrid != null)
      this.LinearGrid = (Grid) null;
    if (this.enableSwitch != null)
      this.enableSwitch = (StackPanel) null;
    if (this.enableSwitch != null)
      this.enableSwitch = (StackPanel) null;
    if (this.m_wordKnownColorPopup != null)
      this.m_wordKnownColorPopup = (Popup) null;
    if (this.m_wordKnownColorsTextBox != null)
      this.m_wordKnownColorsTextBox = (TextBox) null;
    if (this.RadialGrid != null)
      this.RadialGrid = (Grid) null;
    if (this.GradPopup != null)
      this.GradPopup = (Popup) null;
    BindingOperations.ClearAllBindings((DependencyObject) this);
  }

  public void Dispose() => this.Dispose(true);
}
