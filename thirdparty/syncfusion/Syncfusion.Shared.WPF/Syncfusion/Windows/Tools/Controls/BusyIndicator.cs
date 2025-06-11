// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.BusyIndicator
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Licensing;
using Syncfusion.Windows.Shared;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Threading;

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

[SkinType(SkinVisualStyle = Skin.Office2007Silver, Type = typeof (BusyIndicator), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/BusyIndicator/Themes/Office2007SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Blue, Type = typeof (BusyIndicator), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/BusyIndicator/Themes/Office2010BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Silver, Type = typeof (BusyIndicator), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/BusyIndicator/Themes/Office2010SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Blend, Type = typeof (BusyIndicator), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/BusyIndicator/Themes/BlendStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Black, Type = typeof (BusyIndicator), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/BusyIndicator/Themes/Office2007BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.VS2010, Type = typeof (BusyIndicator), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/BusyIndicator/Themes/VS2010Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Metro, Type = typeof (BusyIndicator), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/BusyIndicator/Themes/MetroStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Transparent, Type = typeof (BusyIndicator), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/BusyIndicator/Themes/TransparentStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Default, Type = typeof (BusyIndicator), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/BusyIndicator/Themes/Generic.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Black, Type = typeof (BusyIndicator), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/BusyIndicator/Themes/Office2010BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Blue, Type = typeof (BusyIndicator), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/BusyIndicator/Themes/Office2007BlueStyle.xaml")]
public class BusyIndicator : ContentControl
{
  internal Button cancelButton;
  internal ToggleButton closeButton;
  internal DispatcherTimer timer = new DispatcherTimer();
  internal DisableEffect disableEffect = new DisableEffect();
  internal FrameworkElement content;
  internal ContentControl description;
  internal ProgressBar progressBar;
  internal Grid progressGrid;
  internal static readonly DependencyProperty BusyProperty = DependencyProperty.Register(nameof (Busy), typeof (bool), typeof (BusyIndicator), new PropertyMetadata((object) false));
  public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(nameof (IsBusy), typeof (bool), typeof (BusyIndicator), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(BusyIndicator.OnIsBusyChanged)));
  public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof (Header), typeof (object), typeof (BusyIndicator), new PropertyMetadata((object) nameof (Busy)));
  public static readonly DependencyProperty HeaderAlignmentProperty = DependencyProperty.Register(nameof (HeaderAlignment), typeof (HorizontalAlignment), typeof (BusyIndicator), new PropertyMetadata((object) HorizontalAlignment.Stretch));
  public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof (HeaderTemplate), typeof (DataTemplate), typeof (BusyIndicator), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty LoadingDescriptionProperty = DependencyProperty.Register(nameof (LoadingDescription), typeof (object), typeof (BusyIndicator), new PropertyMetadata((object) "Loading..."));
  public static readonly DependencyProperty LoadingDescriptionTemplateProperty = DependencyProperty.Register(nameof (LoadingDescriptionTemplate), typeof (DataTemplate), typeof (BusyIndicator), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ProgressValueProperty = DependencyProperty.Register(nameof (ProgressValue), typeof (double), typeof (BusyIndicator), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(BusyIndicator.OnProgressValueChanged)));
  public static readonly DependencyProperty DelayProperty = DependencyProperty.Register(nameof (Delay), typeof (TimeSpan), typeof (BusyIndicator), new PropertyMetadata((object) new TimeSpan(0L)));
  public static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register(nameof (IsIndeterminate), typeof (bool), typeof (BusyIndicator), new PropertyMetadata((object) false));
  public static readonly DependencyProperty CloseButtonVisibilityProperty = DependencyProperty.Register(nameof (CloseButtonVisibility), typeof (Visibility), typeof (BusyIndicator), new PropertyMetadata((object) Visibility.Visible));
  public static readonly DependencyProperty CancelButtonVisibilityProperty = DependencyProperty.Register(nameof (CancelButtonVisibility), typeof (Visibility), typeof (BusyIndicator), new PropertyMetadata((object) Visibility.Visible));
  public static readonly DependencyProperty CloseButtonStyleProperty = DependencyProperty.Register(nameof (CloseButtonStyle), typeof (Style), typeof (BusyIndicator), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ProgressBarStyleProperty = DependencyProperty.Register(nameof (ProgressBarStyle), typeof (Style), typeof (BusyIndicator), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty OverlayOpacityProperty = DependencyProperty.Register(nameof (OverlayOpacity), typeof (double), typeof (BusyIndicator), new PropertyMetadata((object) 0.6));
  public static readonly DependencyProperty OverlayBrushProperty = DependencyProperty.Register(nameof (OverlayBrush), typeof (Brush), typeof (BusyIndicator), new PropertyMetadata((object) new SolidColorBrush(Colors.White)));
  public static readonly DependencyProperty DescriptionPlacementProperty = DependencyProperty.Register(nameof (DescriptionPlacement), typeof (DescriptionPlacement), typeof (BusyIndicator), new PropertyMetadata((object) DescriptionPlacement.Top, new PropertyChangedCallback(BusyIndicator.OnDescriptionPlacementChanged)));
  public static readonly DependencyProperty EnableGrayScaleEffectProperty = DependencyProperty.Register(nameof (EnableGrayScaleEffect), typeof (bool), typeof (BusyIndicator), new PropertyMetadata((object) false, new PropertyChangedCallback(BusyIndicator.OnEnableGrayScaleEffectChanged)));

  public event PropertyChangedCallback IsBusyChanged;

  public event PropertyChangedCallback DescriptionPlacementChanged;

  public event PropertyChangedCallback ProgressValueChanged;

  public event PropertyChangedCallback EnableGrayScaleEffectChanged;

  public event CancelEventHandler CancelClick;

  public event CancelEventHandler Closing;

  public event RoutedEventHandler Closed;

  public BusyIndicator()
  {
    this.DefaultStyleKey = (object) typeof (BusyIndicator);
    this.Loaded += new RoutedEventHandler(this.OnBusyIndicator_Loaded);
    this.Unloaded += new RoutedEventHandler(this.OnBusyIndicator_Unloaded);
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  private void OnBusyIndicator_Unloaded(object sender, RoutedEventArgs e)
  {
    this.Loaded -= new RoutedEventHandler(this.OnBusyIndicator_Loaded);
    this.Unloaded -= new RoutedEventHandler(this.OnBusyIndicator_Unloaded);
    this.timer.Tick -= new EventHandler(this.timer_Tick);
  }

  private void OnBusyIndicator_Loaded(object sender, RoutedEventArgs e)
  {
    this.timer.Tick += new EventHandler(this.timer_Tick);
    this.UpdateIsBusy(this.IsBusy);
  }

  internal bool Busy
  {
    get => (bool) this.GetValue(BusyIndicator.BusyProperty);
    set => this.SetValue(BusyIndicator.BusyProperty, (object) value);
  }

  public bool IsBusy
  {
    get => (bool) this.GetValue(BusyIndicator.IsBusyProperty);
    set => this.SetValue(BusyIndicator.IsBusyProperty, (object) value);
  }

  public object Header
  {
    get => this.GetValue(BusyIndicator.HeaderProperty);
    set => this.SetValue(BusyIndicator.HeaderProperty, value);
  }

  public HorizontalAlignment HeaderAlignment
  {
    get => (HorizontalAlignment) this.GetValue(BusyIndicator.HeaderAlignmentProperty);
    set => this.SetValue(BusyIndicator.HeaderAlignmentProperty, (object) value);
  }

  public DataTemplate HeaderTemplate
  {
    get => (DataTemplate) this.GetValue(BusyIndicator.HeaderTemplateProperty);
    set => this.SetValue(BusyIndicator.HeaderTemplateProperty, (object) value);
  }

  public object LoadingDescription
  {
    get => this.GetValue(BusyIndicator.LoadingDescriptionProperty);
    set => this.SetValue(BusyIndicator.LoadingDescriptionProperty, value);
  }

  public DataTemplate LoadingDescriptionTemplate
  {
    get => (DataTemplate) this.GetValue(BusyIndicator.LoadingDescriptionTemplateProperty);
    set => this.SetValue(BusyIndicator.LoadingDescriptionTemplateProperty, (object) value);
  }

  public double ProgressValue
  {
    get => (double) this.GetValue(BusyIndicator.ProgressValueProperty);
    set => this.SetValue(BusyIndicator.ProgressValueProperty, (object) value);
  }

  public TimeSpan Delay
  {
    get => (TimeSpan) this.GetValue(BusyIndicator.DelayProperty);
    set => this.SetValue(BusyIndicator.DelayProperty, (object) value);
  }

  public bool IsIndeterminate
  {
    get => (bool) this.GetValue(BusyIndicator.IsIndeterminateProperty);
    set => this.SetValue(BusyIndicator.IsIndeterminateProperty, (object) value);
  }

  public Visibility CloseButtonVisibility
  {
    get => (Visibility) this.GetValue(BusyIndicator.CloseButtonVisibilityProperty);
    set => this.SetValue(BusyIndicator.CloseButtonVisibilityProperty, (object) value);
  }

  public Visibility CancelButtonVisibility
  {
    get => (Visibility) this.GetValue(BusyIndicator.CancelButtonVisibilityProperty);
    set => this.SetValue(BusyIndicator.CancelButtonVisibilityProperty, (object) value);
  }

  public Style CloseButtonStyle
  {
    get => (Style) this.GetValue(BusyIndicator.CloseButtonStyleProperty);
    set => this.SetValue(BusyIndicator.CloseButtonStyleProperty, (object) value);
  }

  public Style ProgressBarStyle
  {
    get => (Style) this.GetValue(BusyIndicator.ProgressBarStyleProperty);
    set => this.SetValue(BusyIndicator.ProgressBarStyleProperty, (object) value);
  }

  public double OverlayOpacity
  {
    get => (double) this.GetValue(BusyIndicator.OverlayOpacityProperty);
    set => this.SetValue(BusyIndicator.OverlayOpacityProperty, (object) value);
  }

  public Brush OverlayBrush
  {
    get => (Brush) this.GetValue(BusyIndicator.OverlayBrushProperty);
    set => this.SetValue(BusyIndicator.OverlayBrushProperty, (object) value);
  }

  public DescriptionPlacement DescriptionPlacement
  {
    get => (DescriptionPlacement) this.GetValue(BusyIndicator.DescriptionPlacementProperty);
    set => this.SetValue(BusyIndicator.DescriptionPlacementProperty, (object) value);
  }

  public bool EnableGrayScaleEffect
  {
    get => (bool) this.GetValue(BusyIndicator.EnableGrayScaleEffectProperty);
    set => this.SetValue(BusyIndicator.EnableGrayScaleEffectProperty, (object) value);
  }

  public static void OnIsBusyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
  {
    ((BusyIndicator) obj)?.OnIsBusyChanged(args);
  }

  protected void OnIsBusyChanged(DependencyPropertyChangedEventArgs args)
  {
    this.content = this.Content as FrameworkElement;
    if (this.IsLoaded)
      this.UpdateIsBusy((bool) args.NewValue);
    if (this.IsBusyChanged == null)
      return;
    this.IsBusyChanged((DependencyObject) this, args);
  }

  private void timer_Tick(object sender, EventArgs e)
  {
    this.Busy = true;
    this.IsBusy = this.Busy;
    this.UpdateGrayScaleEffect();
    this.timer.Tick -= new EventHandler(this.timer_Tick);
    this.timer.Stop();
  }

  public static void OnDescriptionPlacementChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((BusyIndicator) obj)?.OnDescriptionPlacementChanged(args);
  }

  protected void OnDescriptionPlacementChanged(DependencyPropertyChangedEventArgs args)
  {
    this.SetDescriptionPlacement();
    if (this.DescriptionPlacementChanged == null)
      return;
    this.DescriptionPlacementChanged((DependencyObject) this, args);
  }

  private void SetDescriptionPlacement()
  {
    if (this.progressBar == null || this.description == null)
      return;
    if (this.DescriptionPlacement == DescriptionPlacement.Left)
    {
      Grid.SetRow((UIElement) this.progressBar, 0);
      Grid.SetRowSpan((UIElement) this.progressBar, 2);
      Grid.SetColumn((UIElement) this.progressBar, 1);
      Grid.SetColumnSpan((UIElement) this.progressBar, 1);
      Grid.SetRow((UIElement) this.description, 0);
      Grid.SetRowSpan((UIElement) this.description, 2);
      Grid.SetColumn((UIElement) this.description, 0);
      Grid.SetColumnSpan((UIElement) this.description, 1);
    }
    else if (this.DescriptionPlacement == DescriptionPlacement.Top)
    {
      Grid.SetRow((UIElement) this.progressBar, 1);
      Grid.SetRowSpan((UIElement) this.progressBar, 1);
      Grid.SetColumn((UIElement) this.progressBar, 0);
      Grid.SetColumnSpan((UIElement) this.progressBar, 2);
      Grid.SetRow((UIElement) this.description, 0);
      Grid.SetRowSpan((UIElement) this.description, 1);
      Grid.SetColumn((UIElement) this.description, 0);
      Grid.SetColumnSpan((UIElement) this.description, 2);
    }
    else if (this.DescriptionPlacement == DescriptionPlacement.Right)
    {
      Grid.SetRow((UIElement) this.progressBar, 0);
      Grid.SetRowSpan((UIElement) this.progressBar, 2);
      Grid.SetColumn((UIElement) this.progressBar, 0);
      Grid.SetColumnSpan((UIElement) this.progressBar, 1);
      Grid.SetRow((UIElement) this.description, 0);
      Grid.SetRowSpan((UIElement) this.description, 2);
      Grid.SetColumn((UIElement) this.description, 1);
      Grid.SetColumnSpan((UIElement) this.description, 1);
    }
    else
    {
      if (this.DescriptionPlacement != DescriptionPlacement.Bottom)
        return;
      Grid.SetRow((UIElement) this.progressBar, 0);
      Grid.SetRowSpan((UIElement) this.progressBar, 1);
      Grid.SetColumn((UIElement) this.progressBar, 0);
      Grid.SetColumnSpan((UIElement) this.progressBar, 2);
      Grid.SetRow((UIElement) this.description, 1);
      Grid.SetRowSpan((UIElement) this.description, 1);
      Grid.SetColumn((UIElement) this.description, 0);
      Grid.SetColumnSpan((UIElement) this.description, 2);
    }
  }

  private void UpdateGrayScaleEffect()
  {
    this.content = this.Content as FrameworkElement;
    if (this.content == null || !this.Busy)
      return;
    if (this.EnableGrayScaleEffect)
      this.content.Effect = (Effect) this.disableEffect;
    else
      this.content.Effect = (Effect) null;
  }

  public static void OnProgressValueChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((BusyIndicator) obj)?.OnProgressValueChanged(args);
  }

  protected void OnProgressValueChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.ProgressValueChanged == null)
      return;
    this.ProgressValueChanged((DependencyObject) this, args);
  }

  public static void OnEnableGrayScaleEffectChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((BusyIndicator) obj)?.OnEnableGrayScaleEffectChanged(args);
  }

  protected void OnEnableGrayScaleEffectChanged(DependencyPropertyChangedEventArgs args)
  {
    this.UpdateGrayScaleEffect();
    if (this.EnableGrayScaleEffectChanged == null)
      return;
    this.EnableGrayScaleEffectChanged((DependencyObject) this, args);
  }

  public override void OnApplyTemplate()
  {
    if (this.closeButton != null)
      this.closeButton.PreviewMouseLeftButtonDown -= new MouseButtonEventHandler(this.closeButton_PreviewMouseLeftButtonDown);
    if (this.cancelButton != null)
      this.cancelButton.Click -= new RoutedEventHandler(this.cancelButton_Click);
    this.closeButton = this.GetTemplateChild("PART_Close") as ToggleButton;
    this.cancelButton = this.GetTemplateChild("PART_Cancel") as Button;
    this.description = this.GetTemplateChild("PART_Description") as ContentControl;
    this.progressBar = this.GetTemplateChild("PART_ProgressBar") as ProgressBar;
    this.UpdateGrayScaleEffect();
    this.SetDescriptionPlacement();
    if (this.closeButton != null)
      this.closeButton.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.closeButton_PreviewMouseLeftButtonDown);
    if (this.cancelButton != null)
      this.cancelButton.Click += new RoutedEventHandler(this.cancelButton_Click);
    base.OnApplyTemplate();
  }

  private void closeButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    this.Close(new CancelEventArgs(), new RoutedEventArgs());
  }

  private void cancelButton_Click(object sender, RoutedEventArgs e)
  {
    CancelEventArgs e1 = new CancelEventArgs();
    CancelEventArgs args = new CancelEventArgs();
    RoutedEventArgs e2 = new RoutedEventArgs();
    if (this.CancelClick != null)
      this.CancelClick((object) this, e1);
    this.Close(args, e2);
  }

  private void Close(CancelEventArgs args, RoutedEventArgs e)
  {
    if (this.Closing != null)
      this.Closing((object) this, args);
    if (args.Cancel)
    {
      this.closeButton.IsChecked = new bool?(true);
    }
    else
    {
      this.IsBusy = false;
      if (this.Closed == null)
        return;
      this.Closed((object) this, e);
    }
  }

  private void UpdateIsBusy(bool isBusy)
  {
    if (isBusy)
    {
      this.timer.Interval = this.Delay;
      if (this.Delay > new TimeSpan(0, 0, 0, 0))
        this.timer.Start();
      else
        this.Busy = this.IsBusy;
    }
    else
    {
      if (this.content != null)
        this.content.Effect = (Effect) null;
      TimeSpan interval = this.timer.Interval;
      this.timer.Stop();
      this.Busy = this.IsBusy;
    }
    this.UpdateGrayScaleEffect();
  }
}
