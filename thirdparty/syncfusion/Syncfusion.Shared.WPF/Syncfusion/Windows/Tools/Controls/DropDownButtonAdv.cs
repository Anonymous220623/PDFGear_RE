// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.DropDownButtonAdv
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Licensing;
using Syncfusion.Windows.Controls.Tools.AutomationPeer;
using Syncfusion.Windows.Shared;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

[SkinType(SkinVisualStyle = Skin.Blend, Type = typeof (DropDownButtonAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/BlendStyle.xaml")]
[Syncfusion.Windows.TemplateVisualState(GroupName = "RibbonButtonStates", Name = "Normal")]
[Syncfusion.Windows.TemplateVisualState(GroupName = "RibbonButtonStates", Name = "Pressed")]
[Syncfusion.Windows.TemplateVisualState(GroupName = "RibbonButtonStates", Name = "Disabled")]
[SkinType(SkinVisualStyle = Skin.Office2007Blue, Type = typeof (DropDownButtonAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/Office2007BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Black, Type = typeof (DropDownButtonAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/Office2007BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Silver, Type = typeof (DropDownButtonAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/Office2007SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Blue, Type = typeof (DropDownButtonAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/Office2010BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Black, Type = typeof (DropDownButtonAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/Office2010BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Silver, Type = typeof (DropDownButtonAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/Office2010SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.VS2010, Type = typeof (DropDownButtonAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/VS2010Style.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyRed, Type = typeof (DropDownButtonAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/ShinyRedStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyBlue, Type = typeof (DropDownButtonAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/ShinyBlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Default, Type = typeof (DropDownButtonAdv), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/ButtonControls/DropDownButton/Themes/DropDownButton.xaml")]
[SkinType(SkinVisualStyle = Skin.Metro, Type = typeof (DropDownButtonAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/MetroStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Transparent, Type = typeof (DropDownButtonAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/TransparentStyle.xaml")]
[Syncfusion.Windows.TemplateVisualState(GroupName = "RibbonButtonStates", Name = "MouseOver")]
public class DropDownButtonAdv : ContentControl, IButtonAdv, IDisposable
{
  private const double SmallIconHeight = 16.0;
  private const double SmallIconWidth = 16.0;
  private const double NormalIconHeight = 16.0;
  private const double NormalIconWidth = 16.0;
  private const double LargeIconHeight = 26.0;
  private const double LargeIconWidth = 26.0;
  internal DropDownMenuItem dropitem = new DropDownMenuItem();
  private TextBlock accessText;
  internal Popup _dropdown;
  protected internal bool isopened;
  private ContentPresenter smallIconContent;
  private ContentPresenter largeIconContent;
  public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(nameof (Label), typeof (string), typeof (DropDownButtonAdv), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty SmallIconProperty = DependencyProperty.Register(nameof (SmallIcon), typeof (ImageSource), typeof (DropDownButtonAdv), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty LargeIconProperty = DependencyProperty.Register(nameof (LargeIcon), typeof (ImageSource), typeof (DropDownButtonAdv), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty IconTemplateProperty = DependencyProperty.Register(nameof (IconTemplate), typeof (DataTemplate), typeof (DropDownButtonAdv), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty IconTemplateSelectorProperty = DependencyProperty.Register(nameof (IconTemplateSelector), typeof (DataTemplateSelector), typeof (DropDownButtonAdv), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty IconWidthProperty = DependencyProperty.Register(nameof (IconWidth), typeof (double), typeof (DropDownButtonAdv), new PropertyMetadata((object) 16.0, new PropertyChangedCallback(DropDownButtonAdv.OnSizeChanged)));
  public static readonly DependencyProperty IconHeightProperty = DependencyProperty.Register(nameof (IconHeight), typeof (double), typeof (DropDownButtonAdv), new PropertyMetadata((object) 16.0, new PropertyChangedCallback(DropDownButtonAdv.OnSizeChanged)));
  public static readonly DependencyProperty SizeModeProperty = DependencyProperty.Register(nameof (SizeMode), typeof (SizeMode), typeof (DropDownButtonAdv), new PropertyMetadata((object) SizeMode.Normal, new PropertyChangedCallback(DropDownButtonAdv.OnSizeChanged)));
  public static readonly DependencyProperty IsMultiLineProperty = DependencyProperty.Register(nameof (IsMultiLine), typeof (bool), typeof (DropDownButtonAdv), new PropertyMetadata((object) true));
  public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(nameof (IsDropDownOpen), typeof (bool), typeof (DropDownButtonAdv), new PropertyMetadata((object) false, new PropertyChangedCallback(DropDownButtonAdv.OnIsDropDownOpenChanged), new CoerceValueCallback(DropDownButtonAdv.OnIsDropDownCoerceValueChanged)));
  public static readonly DependencyProperty StayDropDownOnClickProperty = DependencyProperty.Register(nameof (StayDropDownOnClick), typeof (bool), typeof (DropDownButtonAdv), new PropertyMetadata((object) false));
  public static readonly DependencyProperty StaysOpenProperty = DependencyProperty.Register(nameof (StaysOpen), typeof (bool), typeof (DropDownButtonAdv), new PropertyMetadata((object) false));
  public static readonly DependencyProperty DropDirectionProperty = DependencyProperty.Register(nameof (DropDirection), typeof (DropDirection), typeof (DropDownButtonAdv), new PropertyMetadata((object) DropDirection.BottomLeft));
  public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register(nameof (IsPressed), typeof (bool), typeof (DropDownButtonAdv), new PropertyMetadata((object) false));

  public DropDownButtonAdv()
  {
    this.DefaultStyleKey = (object) typeof (DropDownButtonAdv);
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
    this.Loaded += new RoutedEventHandler(this.DropDownButtonAdv_Loaded);
  }

  [Description("The Label Property of this element can be set to any string value")]
  [Category("Common Properties")]
  public string Label
  {
    get => (string) this.GetValue(DropDownButtonAdv.LabelProperty);
    set => this.SetValue(DropDownButtonAdv.LabelProperty, (object) value);
  }

  [Description("Represents the Image displayed in the element, when size form is Small or Normal")]
  [Category("Common Properties")]
  public ImageSource SmallIcon
  {
    get => (ImageSource) this.GetValue(DropDownButtonAdv.SmallIconProperty);
    set => this.SetValue(DropDownButtonAdv.SmallIconProperty, (object) value);
  }

  [Description("Represents the Image displayed in the element, when size form is Large")]
  [Category("Common Properties")]
  public ImageSource LargeIcon
  {
    get => (ImageSource) this.GetValue(DropDownButtonAdv.LargeIconProperty);
    set => this.SetValue(DropDownButtonAdv.LargeIconProperty, (object) value);
  }

  public DataTemplate IconTemplate
  {
    get => (DataTemplate) this.GetValue(DropDownButtonAdv.IconTemplateProperty);
    set => this.SetValue(DropDownButtonAdv.IconTemplateProperty, (object) value);
  }

  public DataTemplateSelector IconTemplateSelector
  {
    get => (DataTemplateSelector) this.GetValue(DropDownButtonAdv.IconTemplateSelectorProperty);
    set => this.SetValue(DropDownButtonAdv.IconTemplateSelectorProperty, (object) value);
  }

  [Description("Represents to set the Image width")]
  [Category("Common Properties")]
  public double IconWidth
  {
    get => (double) this.GetValue(DropDownButtonAdv.IconWidthProperty);
    set => this.SetValue(DropDownButtonAdv.IconWidthProperty, (object) value);
  }

  [Category("Common Properties")]
  [Description("Represents to set the Image height")]
  public double IconHeight
  {
    get => (double) this.GetValue(DropDownButtonAdv.IconHeightProperty);
    set => this.SetValue(DropDownButtonAdv.IconHeightProperty, (object) value);
  }

  [Category("Appearance")]
  [Description("Represents the Size of the element, which may be Normal, Small or Large")]
  public SizeMode SizeMode
  {
    get => (SizeMode) this.GetValue(DropDownButtonAdv.SizeModeProperty);
    set => this.SetValue(DropDownButtonAdv.SizeModeProperty, (object) value);
  }

  [Description("Represents the value, whether the text in the element can be multilined or not")]
  [Category("Appearance")]
  public bool IsMultiLine
  {
    get => (bool) this.GetValue(DropDownButtonAdv.IsMultiLineProperty);
    set => this.SetValue(DropDownButtonAdv.IsMultiLineProperty, (object) value);
  }

  [Description("Represents the value, whether the drop down menu is open or not")]
  public bool IsDropDownOpen
  {
    get => (bool) this.GetValue(DropDownButtonAdv.IsDropDownOpenProperty);
    set => this.SetValue(DropDownButtonAdv.IsDropDownOpenProperty, (object) value);
  }

  public bool StayDropDownOnClick
  {
    get => (bool) this.GetValue(DropDownButtonAdv.StayDropDownOnClickProperty);
    set => this.SetValue(DropDownButtonAdv.StayDropDownOnClickProperty, (object) value);
  }

  public bool StaysOpen
  {
    get => (bool) this.GetValue(DropDownButtonAdv.StaysOpenProperty);
    set => this.SetValue(DropDownButtonAdv.StaysOpenProperty, (object) value);
  }

  [Category("Common Properties")]
  [Description("Represents the direction, in which the drop down of this element has to be displayed.")]
  public DropDirection DropDirection
  {
    get => (DropDirection) this.GetValue(DropDownButtonAdv.DropDirectionProperty);
    set => this.SetValue(DropDownButtonAdv.DropDirectionProperty, (object) value);
  }

  public bool IsPressed
  {
    get => (bool) this.GetValue(DropDownButtonAdv.IsPressedProperty);
    protected internal set => this.SetValue(DropDownButtonAdv.IsPressedProperty, (object) value);
  }

  public event CancelEventHandler DropDownOpening;

  public event RoutedEventHandler DropDownOpened;

  public event CancelEventHandler DropDownClosing;

  public event RoutedEventHandler DropDownClosed;

  private void Initialize()
  {
    this.accessText = this.GetTemplateChild("PART_NormalText") as TextBlock;
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

  private static void OnSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    DropDownButtonAdv dropDownButtonAdv = d as DropDownButtonAdv;
    DataTemplateSelector templateSelector = dropDownButtonAdv.IconTemplateSelector;
    if (dropDownButtonAdv.IconTemplateSelector != null)
    {
      dropDownButtonAdv.IconTemplateSelector = (DataTemplateSelector) null;
      dropDownButtonAdv.IconTemplateSelector = templateSelector;
    }
    dropDownButtonAdv.OnSizeChanged();
  }

  private void OnSizeChanged() => this.UpdateSize();

  private static void OnDropDirectionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as DropDownButtonAdv).OnDropDirectionChanged();
  }

  private void OnDropDirectionChanged() => this.UpdateDropDirection();

  private static object OnIsDropDownCoerceValueChanged(DependencyObject d, object basevalue)
  {
    bool flag = true;
    if (d is DropDownButtonAdv)
      flag = (d as DropDownButtonAdv).OnIsDropDownCoerceValueChanged((bool) basevalue);
    return (object) flag;
  }

  private bool OnIsDropDownCoerceValueChanged(bool basevalue)
  {
    SplitButtonAdv ancestor = (SplitButtonAdv) VisualUtils.FindAncestor((Visual) this._dropdown, typeof (SplitButtonAdv));
    if (basevalue)
    {
      CancelEventArgs e = new CancelEventArgs();
      if (this.DropDownOpening != null && this._dropdown != null)
      {
        this.DropDownOpening((object) this, e);
        basevalue = !e.Cancel;
        if (ancestor != null && !basevalue)
        {
          ancestor.IsDropDownPressed = basevalue;
          ancestor.IsPressed = basevalue;
        }
      }
    }
    else
    {
      CancelEventArgs e = new CancelEventArgs();
      if (this.DropDownClosing != null && this._dropdown != null)
      {
        this.DropDownClosing((object) this, e);
        if (this.StaysOpen)
          basevalue = e.Cancel;
      }
    }
    return basevalue;
  }

  protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    this.Focus();
    this.IsPressed = true;
    this.isopened = this.IsDropDownOpen;
    e.Handled = true;
  }

  private static void OnIsDropDownOpenChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as DropDownButtonAdv).OnIsDropDownOpenChanged();
  }

  internal void OnIsDropDownOpenChanged()
  {
    if (this._dropdown == null)
      return;
    this.UpdateDropDirection();
    this._dropdown.IsOpen = this.IsDropDownOpen;
  }

  protected override void OnMouseWheel(MouseWheelEventArgs e)
  {
    if (this.IsDropDownOpen)
      e.Handled = true;
    else
      base.OnMouseWheel(e);
  }

  protected override void OnKeyDown(KeyEventArgs e)
  {
    switch (e.Key)
    {
      case Key.Return:
      case Key.Space:
      case Key.F4:
        this.IsPressed = true;
        if (this.Content != null && this.Content is DropDownMenuGroup && this.Content is DropDownMenuGroup && (this.Content as DropDownMenuGroup).Items.Count != 0 || this.Content is FrameworkElement && this.Content is FrameworkElement)
        {
          if (this.IsDropDownOpen)
          {
            this.IsDropDownOpen = false;
            this.IsPressed = false;
            this.Focus();
          }
          else
          {
            this.IsDropDownOpen = true;
            this.IsPressed = true;
            if (this.Content is FrameworkElement)
              (this.Content as FrameworkElement).Focus();
          }
        }
        e.Handled = true;
        break;
      case Key.Escape:
        if (this.IsDropDownOpen)
        {
          this.IsDropDownOpen = false;
          this.Focus();
          break;
        }
        break;
    }
    if (ModifierKeys.Alt == Keyboard.Modifiers && e.SystemKey == Key.Down || e.SystemKey == Key.Up)
    {
      FrameworkElement content = this.Content as FrameworkElement;
      if (this.IsDropDownOpen && content != null)
      {
        this.IsDropDownOpen = false;
        this.Focus();
      }
      else
      {
        this.IsDropDownOpen = true;
        content.Focus();
      }
    }
    base.OnKeyDown(e);
  }

  private void UpdateDropDirection()
  {
    if (this._dropdown == null)
      return;
    switch (this.DropDirection)
    {
      case DropDirection.BottomLeft:
      case DropDirection.BottomRight:
      case DropDirection.TopLeft:
      case DropDirection.TopRight:
        this._dropdown.Placement = PlacementMode.Custom;
        this._dropdown.CustomPopupPlacementCallback = new CustomPopupPlacementCallback(this.UpdatePopupPlacement);
        break;
      case DropDirection.Right:
        this._dropdown.Placement = PlacementMode.Right;
        break;
      case DropDirection.Left:
        this._dropdown.Placement = PlacementMode.Left;
        break;
    }
  }

  protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    FrameworkElement content = this.Content as FrameworkElement;
    if (this._dropdown != null)
    {
      CancelEventArgs e1 = new CancelEventArgs();
      if (this.IsDropDownOpen)
      {
        if (this.DropDownClosing != null)
          this.DropDownClosing((object) this, e1);
        if (e1.Cancel)
        {
          this.StaysOpen = true;
          this.IsDropDownOpen = true;
        }
      }
      else if (this.DropDownOpening != null)
        this.DropDownOpening((object) this, e1);
      if (this.IsDropDownOpen)
      {
        this.IsPressed = false;
        this.IsDropDownOpen = false;
        this.Focus();
      }
      else if (!this.isopened)
      {
        if (!e1.Cancel)
        {
          if (content is DropDownMenuGroup && e.Source is DropDownButtonAdv && (content as DropDownMenuGroup).Items.Count > 0 || !(content is DropDownMenuGroup))
          {
            this.IsPressed = true;
            this.IsDropDownOpen = true;
            content?.Focus();
          }
          else
            this.IsPressed = false;
        }
        else
          this.IsPressed = false;
      }
    }
    e.Handled = true;
  }

  public CustomPopupPlacement[] UpdatePopupPlacement(Size popupSize, Size targetSize, Point offset)
  {
    CustomPopupPlacement customPopupPlacement = new CustomPopupPlacement();
    switch (this.DropDirection)
    {
      case DropDirection.BottomLeft:
        customPopupPlacement = this._dropdown == null || this._dropdown.Child == null || !(this._dropdown.Child is Border) ? new CustomPopupPlacement(new Point(0.0, targetSize.Height), PopupPrimaryAxis.Vertical) : new CustomPopupPlacement(new Point(-(this._dropdown.Child as Border).Margin.Left, targetSize.Height), PopupPrimaryAxis.Vertical);
        break;
      case DropDirection.BottomRight:
        customPopupPlacement = new CustomPopupPlacement(new Point(targetSize.Width, targetSize.Height), PopupPrimaryAxis.Vertical);
        break;
      case DropDirection.TopLeft:
        customPopupPlacement = new CustomPopupPlacement(new Point(0.0, -popupSize.Height), PopupPrimaryAxis.Vertical);
        break;
      case DropDirection.TopRight:
        customPopupPlacement = new CustomPopupPlacement(new Point(targetSize.Width, -popupSize.Height), PopupPrimaryAxis.Vertical);
        break;
    }
    return new CustomPopupPlacement[1]
    {
      customPopupPlacement
    };
  }

  public override void OnApplyTemplate()
  {
    this.Initialize();
    this._dropdown = this.GetTemplateChild("PART_DropDown") as Popup;
    if (this._dropdown != null)
    {
      this._dropdown.MouseLeftButtonUp += new MouseButtonEventHandler(this._dropdown_MouseLeftButtonUp);
      this._dropdown.Closed += new EventHandler(this._dropdown_Closed);
      this._dropdown.Opened += new EventHandler(this._dropdown_Opened);
    }
    this.OnIsDropDownOpenChanged();
    base.OnApplyTemplate();
  }

  private void DropDownButtonAdv_Loaded(object sender, RoutedEventArgs e)
  {
    this.UpdateSize();
    Window window = Window.GetWindow((DependencyObject) this);
    if (window == null || this._dropdown == null)
      return;
    window.LocationChanged += (EventHandler) ((sender1, args) =>
    {
      if (!this.IsDropDownOpen || !this.StaysOpen)
        return;
      this._dropdown.HorizontalOffset = this._dropdown.HorizontalOffset++;
    });
    window.SizeChanged += (SizeChangedEventHandler) ((sender2, e2) =>
    {
      if (!this.IsDropDownOpen || !this.StaysOpen)
        return;
      this._dropdown.HorizontalOffset = this._dropdown.HorizontalOffset++;
    });
  }

  private void _dropdown_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    if (this.StayDropDownOnClick)
      e.Handled = true;
    base.OnMouseLeftButtonUp(e);
  }

  protected override System.Windows.Automation.Peers.AutomationPeer OnCreateAutomationPeer()
  {
    return (System.Windows.Automation.Peers.AutomationPeer) new DropDownButtonAdvAutomationPeer(this);
  }

  private void _dropdown_Opened(object sender, EventArgs e)
  {
    if (this.DropDownOpened == null)
      return;
    this.DropDownOpened((object) this, new RoutedEventArgs());
  }

  private void _dropdown_Closed(object sender, EventArgs e)
  {
    if (!this._dropdown.StaysOpen && this.IsDropDownOpen)
      this.IsDropDownOpen = false;
    this.IsPressed = false;
    if (this.DropDownClosed == null)
      return;
    this.DropDownClosed((object) this, new RoutedEventArgs());
  }

  private void _dropdown_IsOpenChanged(object sender, EventArgs e)
  {
    if (!this._dropdown.IsOpen)
    {
      this.IsDropDownOpen = false;
      this.IsPressed = false;
      if (this.IsDropDownOpen || this.DropDownClosed == null)
        return;
      this.DropDownClosed((object) this, new RoutedEventArgs());
    }
    else
    {
      this.IsPressed = true;
      if (this.DropDownOpened == null)
        return;
      this.DropDownOpened((object) this, new RoutedEventArgs());
    }
  }

  public void Dispose()
  {
    if (this._dropdown == null)
      return;
    this.Loaded -= new RoutedEventHandler(this.DropDownButtonAdv_Loaded);
    this._dropdown.MouseLeftButtonUp -= new MouseButtonEventHandler(this._dropdown_MouseLeftButtonUp);
    this._dropdown.Closed -= new EventHandler(this._dropdown_Closed);
    this._dropdown.Opened -= new EventHandler(this._dropdown_Opened);
  }
}
