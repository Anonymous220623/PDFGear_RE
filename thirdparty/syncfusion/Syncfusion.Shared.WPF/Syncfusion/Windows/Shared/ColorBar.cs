// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.ColorBar
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Licensing;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

[SkinType(SkinVisualStyle = Skin.Office2007Black, Type = typeof (ColorBar), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorBar/Themes/Office2007BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Blue, Type = typeof (ColorBar), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorBar/Themes/Office2007BlueStyle.xaml")]
[DesignTimeVisible(false)]
[SkinType(SkinVisualStyle = Skin.Office2007Silver, Type = typeof (ColorBar), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorBar/Themes/Office2007SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Blue, Type = typeof (ColorBar), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorBar/Themes/Office2010BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Black, Type = typeof (ColorBar), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorBar/Themes/Office2010BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Silver, Type = typeof (ColorBar), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorBar/Themes/Office2010SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2003, Type = typeof (ColorBar), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorBar/Themes/Office2003Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Blend, Type = typeof (ColorBar), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorBar/Themes/BlendStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.SyncOrange, Type = typeof (ColorBar), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorBar/Themes/SyncOrange.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyRed, Type = typeof (ColorBar), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorBar/Themes/ShinyRed.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyBlue, Type = typeof (ColorBar), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ColorBar/Themes/ShinyBlue.xaml")]
[SkinType(SkinVisualStyle = Skin.Default, Type = typeof (ColorBar), XamlResource = "/Syncfusion.Shared.Wpf;component/Controls/ColorBar/Themes/Generic.xaml")]
public class ColorBar : Control
{
  private const string ColorBarSlider = "ColorBarSlider";
  private float m_sliderValue;
  private bool m_mouseClicked;
  public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof (Color), typeof (Color), typeof (ColorBar), (PropertyMetadata) new FrameworkPropertyMetadata((object) Colors.White, FrameworkPropertyMetadataOptions.AffectsArrange, new PropertyChangedCallback(ColorBar.OnColorChanged)));
  internal static readonly DependencyProperty SliderValueProperty = DependencyProperty.Register(nameof (SliderValue), typeof (float), typeof (ColorBar), (PropertyMetadata) new FrameworkPropertyMetadata((object) 1f, FrameworkPropertyMetadataOptions.AffectsArrange, new PropertyChangedCallback(ColorBar.OnSliderValueChanged)));
  internal static readonly DependencyProperty SliderMaxValueProperty = DependencyProperty.Register(nameof (SliderMaxValue), typeof (float), typeof (ColorBar), (PropertyMetadata) new FrameworkPropertyMetadata((object) 360f));

  static ColorBar()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ColorBar), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ColorBar)));
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  public ColorBar() => FusionLicenseProvider.GetLicenseType(Platform.WPF);

  public event PropertyChangedCallback SliderValueChanged;

  public event PropertyChangedCallback ColorChanged;

  public override void OnApplyTemplate() => base.OnApplyTemplate();

  private static void OnSliderValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((ColorBar) d).OnSliderValueChanged(e);
  }

  protected override void OnGotMouseCapture(MouseEventArgs e)
  {
    this.m_mouseClicked = true;
    base.OnGotMouseCapture(e);
  }

  protected override void OnLostMouseCapture(MouseEventArgs e)
  {
    this.m_mouseClicked = false;
    base.OnLostMouseCapture(e);
  }

  protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    this.m_mouseClicked = true;
    base.OnPreviewMouseLeftButtonDown(e);
  }

  protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    this.m_mouseClicked = false;
    base.OnMouseLeftButtonUp(e);
  }

  private void OnSliderValueChanged(DependencyPropertyChangedEventArgs e)
  {
    if ((double) this.m_sliderValue == (double) (float) e.NewValue)
      return;
    this.m_sliderValue = (float) e.NewValue;
    if (this.SliderValueChanged != null)
      this.SliderValueChanged((DependencyObject) this, e);
    if (!this.m_mouseClicked)
      return;
    this.Color = HsvColor.ConvertHsvToRgb((double) this.m_sliderValue, 1.0, 1.0);
  }

  private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((ColorBar) d).OnColorChanged(e);
  }

  private void OnColorChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.ColorChanged == null)
      return;
    this.ColorChanged((DependencyObject) this, e);
  }

  public Color Color
  {
    get => (Color) this.GetValue(ColorBar.ColorProperty);
    set => this.SetValue(ColorBar.ColorProperty, (object) value);
  }

  internal float SliderValue
  {
    get => this.m_sliderValue;
    set => this.SetValue(ColorBar.SliderValueProperty, (object) value);
  }

  private float SliderMaxValue
  {
    get => (float) this.GetValue(ColorBar.SliderMaxValueProperty);
    set => this.SetValue(ColorBar.SliderMaxValueProperty, (object) value);
  }
}
