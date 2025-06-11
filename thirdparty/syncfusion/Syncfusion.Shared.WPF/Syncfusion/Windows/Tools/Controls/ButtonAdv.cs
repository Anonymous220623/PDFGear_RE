// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.ButtonAdv
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Licensing;
using Syncfusion.Windows.Controls.Tools.AutomationPeer;
using Syncfusion.Windows.Shared;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

[SkinType(SkinVisualStyle = Skin.Office2010Black, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/Button/Themes/Office2010BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Blend, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/Button/Themes/BlendStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.VS2010, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/Button/Themes/VS2010Style.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyRed, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/Button/Themes/ShinyRedStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyBlue, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/Button/Themes/ShinyBlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Default, Type = typeof (TileViewControl), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/ButtonControls/Button/Themes/ButtonAdv.xaml")]
[SkinType(SkinVisualStyle = Skin.Metro, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/Button/Themes/MetroStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Transparent, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/Button/Themes/TransparentStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Black, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/Button/Themes/Office2007BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Silver, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/Button/Themes/Office2007SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Blue, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/Button/Themes/Office2010BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Blue, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/Button/Themes/Office2007BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Silver, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/Button/Themes/Office2010SilverStyle.xaml")]
public class ButtonAdv : ButtonBase, ICommandSource, IButtonAdv
{
  private const double SmallIconHeight = 16.0;
  private const double SmallIconWidth = 16.0;
  private const double NormalIconHeight = 16.0;
  private const double NormalIconWidth = 16.0;
  private const double LargeIconHeight = 26.0;
  private const double LargeIconWidth = 26.0;
  private AccessText accessText;
  private ContentPresenter smallIconContent;
  private ContentPresenter largeIconContent;
  public static readonly DependencyProperty IsCancelProperty = DependencyProperty.Register(nameof (IsCancel), typeof (bool), typeof (ButtonAdv), new PropertyMetadata(new PropertyChangedCallback(ButtonAdv.OnIsCancelChanged)));
  public static readonly DependencyProperty IsDefaultProperty = DependencyProperty.Register(nameof (IsDefault), typeof (bool), typeof (ButtonAdv), new PropertyMetadata(new PropertyChangedCallback(ButtonAdv.OnIsDefaultChanged)));
  public static readonly DependencyProperty IconStretchProperty = DependencyProperty.Register(nameof (IconStretch), typeof (Stretch), typeof (ButtonAdv), new PropertyMetadata((object) Stretch.Uniform));
  public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(nameof (Label), typeof (string), typeof (ButtonAdv), new PropertyMetadata((object) "Button"));
  public static readonly DependencyProperty IconTemplateProperty = DependencyProperty.Register(nameof (IconTemplate), typeof (DataTemplate), typeof (ButtonAdv), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty IconTemplateSelectorProperty = DependencyProperty.Register(nameof (IconTemplateSelector), typeof (DataTemplateSelector), typeof (ButtonAdv), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty SmallIconProperty = DependencyProperty.Register(nameof (SmallIcon), typeof (ImageSource), typeof (ButtonAdv), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty IsCheckableProperty = DependencyProperty.Register(nameof (IsCheckable), typeof (bool), typeof (ButtonAdv), new PropertyMetadata((object) false, new PropertyChangedCallback(ButtonAdv.OnIsCheckableChanged)));
  public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof (IsChecked), typeof (bool), typeof (ButtonAdv), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(ButtonAdv.OnIsCheckedChanged)));
  public static readonly DependencyProperty LargeIconProperty = DependencyProperty.Register(nameof (LargeIcon), typeof (ImageSource), typeof (ButtonAdv), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty IconWidthProperty = DependencyProperty.Register(nameof (IconWidth), typeof (double), typeof (ButtonAdv), new PropertyMetadata((object) 16.0, new PropertyChangedCallback(ButtonAdv.OnSizeChanged)));
  public static readonly DependencyProperty IconHeightProperty = DependencyProperty.Register(nameof (IconHeight), typeof (double), typeof (ButtonAdv), new PropertyMetadata((object) 16.0, new PropertyChangedCallback(ButtonAdv.OnSizeChanged)));
  public static readonly DependencyProperty SizeModeProperty = DependencyProperty.Register(nameof (SizeMode), typeof (SizeMode), typeof (ButtonAdv), new PropertyMetadata((object) SizeMode.Normal, new PropertyChangedCallback(ButtonAdv.OnSizeChanged)));
  public static readonly DependencyProperty IsMultiLineProperty = DependencyProperty.Register(nameof (IsMultiLine), typeof (bool), typeof (ButtonAdv), new PropertyMetadata((object) true));

  public ButtonAdv()
  {
    this.DefaultStyleKey = (object) typeof (ButtonAdv);
    this.Initialize();
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  static ButtonAdv() => FusionLicenseProvider.GetLicenseType(Platform.WPF);

  public bool IsCancel
  {
    get => (bool) this.GetValue(ButtonAdv.IsCancelProperty);
    set => this.SetValue(ButtonAdv.IsCancelProperty, (object) value);
  }

  public bool IsDefault
  {
    get => (bool) this.GetValue(ButtonAdv.IsDefaultProperty);
    set => this.SetValue(ButtonAdv.IsDefaultProperty, (object) value);
  }

  public Stretch IconStretch
  {
    get => (Stretch) this.GetValue(ButtonAdv.IconStretchProperty);
    set => this.SetValue(ButtonAdv.IconStretchProperty, (object) value);
  }

  [Description("The Label Property of this element can be set to any string value")]
  [Category("Common Properties")]
  public string Label
  {
    get => (string) this.GetValue(ButtonAdv.LabelProperty);
    set => this.SetValue(ButtonAdv.LabelProperty, (object) value);
  }

  public DataTemplate IconTemplate
  {
    get => (DataTemplate) this.GetValue(ButtonAdv.IconTemplateProperty);
    set => this.SetValue(ButtonAdv.IconTemplateProperty, (object) value);
  }

  public DataTemplateSelector IconTemplateSelector
  {
    get => (DataTemplateSelector) this.GetValue(ButtonAdv.IconTemplateSelectorProperty);
    set => this.SetValue(ButtonAdv.IconTemplateSelectorProperty, (object) value);
  }

  [Description("Represents the Image displayed in the element, when size form is Small or Normal")]
  [Category("Common Properties")]
  public ImageSource SmallIcon
  {
    get => (ImageSource) this.GetValue(ButtonAdv.SmallIconProperty);
    set => this.SetValue(ButtonAdv.SmallIconProperty, (object) value);
  }

  [Category("Common Properties")]
  [Description("Represents the value, whether the element can be checkable or not")]
  public bool IsCheckable
  {
    get => (bool) this.GetValue(ButtonAdv.IsCheckableProperty);
    set => this.SetValue(ButtonAdv.IsCheckableProperty, (object) value);
  }

  [Category("Common Properties")]
  [Description("Represents the value, whether the element is checked or not")]
  public bool IsChecked
  {
    get => (bool) this.GetValue(ButtonAdv.IsCheckedProperty);
    set => this.SetValue(ButtonAdv.IsCheckedProperty, (object) value);
  }

  [Description("Represents the Image displayed in the element, when size form is Large")]
  [Category("Common Properties")]
  public ImageSource LargeIcon
  {
    get => (ImageSource) this.GetValue(ButtonAdv.LargeIconProperty);
    set => this.SetValue(ButtonAdv.LargeIconProperty, (object) value);
  }

  [Category("Common Properties")]
  [Description("Represents to set the Image width")]
  public double IconWidth
  {
    get => (double) this.GetValue(ButtonAdv.IconWidthProperty);
    set => this.SetValue(ButtonAdv.IconWidthProperty, (object) value);
  }

  [Description("Represents to set the Image height")]
  [Category("Common Properties")]
  public double IconHeight
  {
    get => (double) this.GetValue(ButtonAdv.IconHeightProperty);
    set => this.SetValue(ButtonAdv.IconHeightProperty, (object) value);
  }

  [Category("Appearance")]
  [Description("Represents the Size of the element, which may be Normal, Small or Large")]
  public SizeMode SizeMode
  {
    get => (SizeMode) this.GetValue(ButtonAdv.SizeModeProperty);
    set => this.SetValue(ButtonAdv.SizeModeProperty, (object) value);
  }

  [Category("Appearance")]
  [Description("Represents the value, whether the text in the element can be multilined or not")]
  public bool IsMultiLine
  {
    get => (bool) this.GetValue(ButtonAdv.IsMultiLineProperty);
    set => this.SetValue(ButtonAdv.IsMultiLineProperty, (object) value);
  }

  public event RoutedEventHandler Checked;

  private void Initialize()
  {
    this.accessText = this.GetTemplateChild("PART_NormalText") as AccessText;
    this.smallIconContent = this.GetTemplateChild("SmallIconContent") as ContentPresenter;
    this.largeIconContent = this.GetTemplateChild("LargeIconContent") as ContentPresenter;
    this.UpdateSize();
  }

  public void UpdateSize()
  {
    if (this.SizeMode == SizeMode.Large)
    {
      if (this.largeIconContent == null)
        return;
      this.largeIconContent.Width = this.IconWidth == 16.0 ? (this.LargeIcon != null || this.IconTemplate != null || this.IconTemplateSelector != null ? 26.0 : 0.0) : this.IconWidth;
      this.largeIconContent.Height = this.IconHeight == 16.0 ? (this.LargeIcon != null || this.IconTemplate != null || this.IconTemplateSelector != null ? 26.0 : 0.0) : this.IconHeight;
    }
    else
    {
      double num1 = this.SizeMode == SizeMode.Normal ? 16.0 : 16.0;
      double num2 = this.SizeMode == SizeMode.Normal ? 16.0 : 16.0;
      if (this.smallIconContent != null)
      {
        this.smallIconContent.Width = this.IconWidth == 16.0 ? (this.SmallIcon != null || this.IconTemplate != null || this.IconTemplateSelector != null ? num1 : 0.0) : this.IconWidth;
        this.smallIconContent.Height = this.IconHeight == 16.0 ? (this.SmallIcon != null || this.IconTemplate != null || this.IconTemplateSelector != null ? num2 : 0.0) : this.IconHeight;
      }
      if (this.accessText == null)
        return;
      this.accessText.Visibility = this.SizeMode == SizeMode.Normal ? Visibility.Visible : Visibility.Collapsed;
    }
  }

  private static void OnIsCheckableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as ButtonAdv).OnIsCheckableChanged();
  }

  private void OnIsCheckableChanged()
  {
    if (this.IsChecked)
      this.IsChecked = false;
    this.OnIsCheckedChanged();
  }

  private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as ButtonAdv).OnIsCheckedChanged();
  }

  private void OnIsCheckedChanged()
  {
    if (!this.IsCheckable || this.Checked == null)
      return;
    this.Checked((object) this, new RoutedEventArgs());
  }

  private static void OnSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ButtonAdv buttonAdv = d as ButtonAdv;
    DataTemplateSelector templateSelector = buttonAdv.IconTemplateSelector;
    if (buttonAdv.IconTemplateSelector != null)
    {
      buttonAdv.IconTemplateSelector = (DataTemplateSelector) null;
      buttonAdv.IconTemplateSelector = templateSelector;
    }
    buttonAdv.OnSizeChanged();
  }

  private void OnSizeChanged() => this.UpdateSize();

  private static void OnIsCancelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ButtonAdv element = d as ButtonAdv;
    if ((bool) e.NewValue)
      AccessKeyManager.Register("\u001B", (IInputElement) element);
    else
      AccessKeyManager.Unregister("\u001B", (IInputElement) element);
  }

  private static void OnIsDefaultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ButtonAdv element = d as ButtonAdv;
    if ((bool) e.NewValue)
      AccessKeyManager.Register("\r", (IInputElement) element);
    else
      AccessKeyManager.Unregister("\r", (IInputElement) element);
  }

  protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    if (this.IsCheckable)
      this.IsChecked = !this.IsChecked;
    base.OnMouseLeftButtonDown(e);
  }

  protected override void OnClick() => base.OnClick();

  protected override System.Windows.Automation.Peers.AutomationPeer OnCreateAutomationPeer()
  {
    return (System.Windows.Automation.Peers.AutomationPeer) new ButtonAdvAutomationPeer(this);
  }

  public override void OnApplyTemplate()
  {
    this.Initialize();
    base.OnApplyTemplate();
  }
}
