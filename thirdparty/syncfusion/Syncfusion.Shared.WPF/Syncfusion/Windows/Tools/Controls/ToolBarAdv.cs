// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.ToolBarAdv
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Licensing;
using Syncfusion.Windows.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

[SkinType(SkinVisualStyle = Skin.Transparent, Type = typeof (ToolBarAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ToolBarAdv/Themes/Transparent/TransparentStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Silver, Type = typeof (ToolBarAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ToolBarAdv/Themes/Office2010Silver/Office2010SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Blue, Type = typeof (ToolBarAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ToolBarAdv/Themes/Office2007Blue/Office2007BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Blend, Type = typeof (ToolBarAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ToolBarAdv/Themes/Blend/BlendStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Black, Type = typeof (ToolBarAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ToolBarAdv/Themes/Office2007Black/Office2007BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Default, Type = typeof (ToolBarAdv), XamlResource = "/Syncfusion.Shared.Wpf;component/Controls/ToolBarAdv/Themes/Default/DefaultStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.VS2010, Type = typeof (ToolBarAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ToolBarAdv/Themes/VS2010/VS2010Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Blue, Type = typeof (ToolBarAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ToolBarAdv/Themes/Office2010Blue/Office2010BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Black, Type = typeof (ToolBarAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ToolBarAdv/Themes/Office2010Black/Office2010BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Metro, Type = typeof (ToolBarAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ToolBarAdv/Themes/Metro/MetroStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Silver, Type = typeof (ToolBarAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ToolBarAdv/Themes/Office2007Silver/Office2007SilverStyle.xaml")]
public class ToolBarAdv : ItemsControl
{
  private ToolBarPanelAdv ToolBarPanel;
  private DropDownButtonAdv addorRemoveButton;
  internal bool isInternallyChangingState;
  internal Dictionary<object, DependencyObject> generatedConatiner;
  internal List<object> ToolStripItems;
  internal List<object> OverflowItems;
  internal Size RequiredSize = new Size();
  private DropDownMenuGroup PART_AddRemoveItems;
  internal Size EmptySpace = new Size();
  internal bool CanToolStripItemsMoveToOverflow = true;
  internal bool CanOverflowItemMoveToToolStrip = true;
  internal Grid DraggingThumb;
  internal Rect BoundingRectangle;
  internal ToolBarBand ToolBarBand;
  private bool isDragging;
  private FrameworkElement OverflowButton;
  private Panel OverflowPanel;
  internal Size ExtraSize = new Size();
  private Popup OverflowPopup;
  private bool hasOverflowItems;
  internal FloatingToolBar floatingToolBar;
  private Path overflowHorizantalPath;
  private Path overflowHorizantalPathRight;
  private Path overflowVerticalPath;
  private Path overflowVerticalPathBottom;
  private DataTemplate defaultDropDownIconTemplate;
  internal bool isArranged;
  internal bool isMeasured;
  private bool ispressed;
  private bool isLoaded;
  private bool canMouseMoveExecute = true;
  public static readonly DependencyProperty EnableAddRemoveButtonProperty = DependencyProperty.Register(nameof (EnableAddRemoveButton), typeof (bool), typeof (ToolBarAdv), new PropertyMetadata((object) false));
  public static readonly DependencyProperty FloatingBarLocationProperty = DependencyProperty.Register(nameof (FloatingBarLocation), typeof (Point), typeof (ToolBarAdv), new PropertyMetadata((object) new Point(0.0, 0.0), new PropertyChangedCallback(ToolBarAdv.OnFloatingBarLocationChanged)));
  public static readonly DependencyProperty OverflowModeProperty = DependencyProperty.RegisterAttached("OverflowMode", typeof (OverflowMode), typeof (ToolBarAdv), (PropertyMetadata) null);
  internal static readonly DependencyProperty IsOverflowItemProperty = DependencyProperty.RegisterAttached(nameof (IsOverflowItem), typeof (bool), typeof (ToolBarAdv), (PropertyMetadata) null);
  public static readonly DependencyProperty GripperVisibilityProperty = DependencyProperty.Register(nameof (GripperVisibility), typeof (Visibility), typeof (ToolBarAdv), new PropertyMetadata((object) Visibility.Visible));
  public static readonly DependencyProperty OverflowButtonVisibilityProperty = DependencyProperty.Register(nameof (OverflowButtonVisibility), typeof (Visibility), typeof (ToolBarAdv), new PropertyMetadata((object) Visibility.Visible));
  public static readonly DependencyProperty BandProperty = DependencyProperty.Register(nameof (Band), typeof (int), typeof (ToolBarAdv), new PropertyMetadata((object) 0, new PropertyChangedCallback(ToolBarAdv.OnBandChanged)));
  public static readonly DependencyProperty BandIndexProperty = DependencyProperty.Register(nameof (BandIndex), typeof (int), typeof (ToolBarAdv), new PropertyMetadata((object) 0, new PropertyChangedCallback(ToolBarAdv.OnBandIndexChanged)));
  public static readonly DependencyProperty ToolBarNameProperty = DependencyProperty.Register(nameof (ToolBarName), typeof (string), typeof (ToolBarAdv), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(ToolBarAdv.OnToolBarNamechanged)));
  public static readonly DependencyProperty ControlsResourceDictionaryProperty = DependencyProperty.Register(nameof (ControlsResourceDictionary), typeof (ResourceDictionary), typeof (ToolBarAdv), new PropertyMetadata((object) new ResourceDictionary()
  {
    Source = new Uri("/Syncfusion.Shared.Wpf;component/Controls/ToolBarAdv/Themes/generic.xaml", UriKind.RelativeOrAbsolute)
  }, new PropertyChangedCallback(ToolBarAdv.OnControlsResourceDictionaryPropertyChanged)));
  public static readonly DependencyProperty LabelProperty = DependencyProperty.RegisterAttached("Label", typeof (string), typeof (ToolBarAdv), new PropertyMetadata((object) string.Empty));
  public static readonly DependencyProperty IconProperty = DependencyProperty.RegisterAttached("Icon", typeof (ImageSource), typeof (ToolBarAdv), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty IsAvailableProperty = DependencyProperty.RegisterAttached("IsAvailable", typeof (bool), typeof (ToolBarAdv), new PropertyMetadata((object) true, new PropertyChangedCallback(ToolBarAdv.OnIsAvailableChanged)));
  public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (System.Windows.Controls.Orientation), typeof (ToolBarAdv), (PropertyMetadata) new UIPropertyMetadata((object) System.Windows.Controls.Orientation.Horizontal, new PropertyChangedCallback(ToolBarAdv.OnOrientationChanged)));
  public static readonly DependencyProperty IsOverflowOpenProperty = DependencyProperty.Register(nameof (IsOverflowOpen), typeof (bool), typeof (ToolBarAdv), new PropertyMetadata((object) false));
  public static readonly DependencyProperty ToolBarItemInfoCollectionProperty = DependencyProperty.Register(nameof (ToolBarItemInfoCollection), typeof (ObservableCollection<ToolBarIteminfo>), typeof (ToolBarAdv), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty IconTemplateProperty = DependencyProperty.Register("IconTemplate", typeof (DataTemplate), typeof (ToolBarAdv), new PropertyMetadata((PropertyChangedCallback) null));

  internal ToolBarTrayAdv Tray { get; set; }

  public ToolBarAdv()
  {
    this.DefaultStyleKey = (object) typeof (ToolBarAdv);
    this.ToolStripItems = new List<object>();
    this.OverflowItems = new List<object>();
    this.ToolBarItemInfoCollection = new ObservableCollection<ToolBarIteminfo>();
    this.Loaded += new RoutedEventHandler(this.ToolBarAdv_Loaded);
    this.generatedConatiner = new Dictionary<object, DependencyObject>();
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  internal bool IsHostedInsideTray => this.Tray != null;

  internal bool IsDragging
  {
    get => this.isDragging;
    set
    {
      if (this.isDragging == value)
        return;
      this.isDragging = value;
      if (this.DraggingThumb == null)
        return;
      if (this.isDragging)
        this.DraggingThumb.CaptureMouse();
      else
        this.DraggingThumb.ReleaseMouseCapture();
    }
  }

  public bool EnableAddRemoveButton
  {
    get => (bool) this.GetValue(ToolBarAdv.EnableAddRemoveButtonProperty);
    set => this.SetValue(ToolBarAdv.EnableAddRemoveButtonProperty, (object) value);
  }

  public Point FloatingBarLocation
  {
    get => (Point) this.GetValue(ToolBarAdv.FloatingBarLocationProperty);
    set => this.SetValue(ToolBarAdv.FloatingBarLocationProperty, (object) value);
  }

  public static OverflowMode GetOverflowMode(DependencyObject obj)
  {
    return (OverflowMode) obj.GetValue(ToolBarAdv.OverflowModeProperty);
  }

  public static void SetOverflowMode(DependencyObject obj, OverflowMode value)
  {
    obj.SetValue(ToolBarAdv.OverflowModeProperty, (object) value);
  }

  public static bool GetIsOverflowItem(DependencyObject obj)
  {
    return (bool) obj.GetValue(ToolBarAdv.IsOverflowItemProperty);
  }

  private static void SetIsOverflowItem(DependencyObject obj, bool value)
  {
    obj.SetValue(ToolBarAdv.IsOverflowItemProperty, (object) value);
  }

  internal bool IsOverflowItem
  {
    get => (bool) this.GetValue(ToolBarAdv.IsOverflowItemProperty);
    set => this.SetValue(ToolBarAdv.IsOverflowItemProperty, (object) value);
  }

  public Visibility GripperVisibility
  {
    get => (Visibility) this.GetValue(ToolBarAdv.GripperVisibilityProperty);
    set => this.SetValue(ToolBarAdv.GripperVisibilityProperty, (object) value);
  }

  public Visibility OverflowButtonVisibility
  {
    get => (Visibility) this.GetValue(ToolBarAdv.OverflowButtonVisibilityProperty);
    set => this.SetValue(ToolBarAdv.OverflowButtonVisibilityProperty, (object) value);
  }

  public int Band
  {
    get => (int) this.GetValue(ToolBarAdv.BandProperty);
    set => this.SetValue(ToolBarAdv.BandProperty, (object) value);
  }

  public int BandIndex
  {
    get => (int) this.GetValue(ToolBarAdv.BandIndexProperty);
    set => this.SetValue(ToolBarAdv.BandIndexProperty, (object) value);
  }

  public string ToolBarName
  {
    get => (string) this.GetValue(ToolBarAdv.ToolBarNameProperty);
    set => this.SetValue(ToolBarAdv.ToolBarNameProperty, (object) value);
  }

  private static void OnToolBarNamechanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    (obj as ToolBarAdv).OnToolBarNamechanged(args);
  }

  private void OnToolBarNamechanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.floatingToolBar == null || args.NewValue == null)
      return;
    this.floatingToolBar.Title = args.NewValue.ToString();
  }

  public ResourceDictionary ControlsResourceDictionary
  {
    get => (ResourceDictionary) this.GetValue(ToolBarAdv.ControlsResourceDictionaryProperty);
    set => this.SetValue(ToolBarAdv.ControlsResourceDictionaryProperty, (object) value);
  }

  public static string GetLabel(DependencyObject obj)
  {
    return (string) obj.GetValue(ToolBarAdv.LabelProperty);
  }

  public static void SetLabel(DependencyObject obj, string value)
  {
    obj.SetValue(ToolBarAdv.LabelProperty, (object) value);
  }

  public static ImageSource GetIcon(DependencyObject obj)
  {
    return (ImageSource) obj.GetValue(ToolBarAdv.IconProperty);
  }

  public static void SetIcon(DependencyObject obj, ImageSource value)
  {
    obj.SetValue(ToolBarAdv.IconProperty, (object) value);
  }

  public static bool GetIsAvailable(DependencyObject obj)
  {
    return (bool) obj.GetValue(ToolBarAdv.IsAvailableProperty);
  }

  public static void SetIsAvailable(DependencyObject obj, bool value)
  {
    obj.SetValue(ToolBarAdv.IsAvailableProperty, (object) value);
  }

  private static void OnIsAvailableChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(sender is FrameworkElement startingFrom))
      return;
    if ((bool) e.NewValue)
      startingFrom.Visibility = Visibility.Visible;
    else
      startingFrom.Visibility = Visibility.Collapsed;
    ToolBarManager ancestor1 = VisualUtils.FindAncestor((Visual) startingFrom, typeof (ToolBarManager)) as ToolBarManager;
    ToolBarTrayAdv ancestor2 = VisualUtils.FindAncestor((Visual) startingFrom, typeof (ToolBarTrayAdv)) as ToolBarTrayAdv;
    ToolBarAdv ancestor3 = VisualUtils.FindAncestor((Visual) startingFrom, typeof (ToolBarAdv)) as ToolBarAdv;
    if (ancestor1 != null)
      ancestor1.InvalidateLayout();
    else if (ancestor2 != null)
    {
      ancestor2.InvalidateLayout();
    }
    else
    {
      if (ancestor3 == null)
        return;
      ancestor3.InvalidateMeasure();
      ancestor3.InvalidateArrange();
    }
  }

  public bool HasOverflowItems
  {
    get => this.hasOverflowItems;
    internal set
    {
      this.hasOverflowItems = value;
      this.UpdateOverflowPathsVisibility();
    }
  }

  public System.Windows.Controls.Orientation Orientation
  {
    get => (System.Windows.Controls.Orientation) this.GetValue(ToolBarAdv.OrientationProperty);
    internal set => this.SetValue(ToolBarAdv.OrientationProperty, (object) value);
  }

  private static void OnOrientationChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs args)
  {
    ToolBarAdv toolBarAdv = sender as ToolBarAdv;
    if (toolBarAdv.ToolBarPanel == null)
      return;
    toolBarAdv.ToolBarPanel.Orientation = (System.Windows.Controls.Orientation) args.NewValue;
    toolBarAdv.UpdateVisualState();
  }

  public bool IsOverflowOpen
  {
    get => (bool) this.GetValue(ToolBarAdv.IsOverflowOpenProperty);
    set => this.SetValue(ToolBarAdv.IsOverflowOpenProperty, (object) value);
  }

  public ObservableCollection<ToolBarIteminfo> ToolBarItemInfoCollection
  {
    get
    {
      return (ObservableCollection<ToolBarIteminfo>) this.GetValue(ToolBarAdv.ToolBarItemInfoCollectionProperty);
    }
    set => this.SetValue(ToolBarAdv.ToolBarItemInfoCollectionProperty, (object) value);
  }

  public static DataTemplate GetIconTemplate(DependencyObject obj)
  {
    return (DataTemplate) obj.GetValue(ToolBarAdv.IconTemplateProperty);
  }

  public static void SetIconTemplate(DependencyObject obj, DataTemplate value)
  {
    obj.SetValue(ToolBarAdv.IconTemplateProperty, (object) value);
  }

  private void ToolBarAdv_Loaded(object sender, RoutedEventArgs e)
  {
    if (!this.isLoaded)
    {
      this.isLoaded = true;
      if (ToolBarManager.GetToolBarState(this) != ToolBarState.Docking)
        this.OnToolBarStateChanged(ToolBarState.Docking, ToolBarManager.GetToolBarState(this));
    }
    foreach (ToolBarAdv visualChild in ToolBarAdv.FindVisualChildren<ToolBarAdv>((DependencyObject) this.Tray))
    {
      if (visualChild != null && visualChild.DraggingThumb != null)
      {
        visualChild.DraggingThumb.MouseLeftButtonDown += new MouseButtonEventHandler(this.DraggingThumb_MouseLeftButtonDown1);
        visualChild.DraggingThumb.MouseLeftButtonUp += new MouseButtonEventHandler(this.DraggingThumb_MouseLeftButtonUp1);
        visualChild.DraggingThumb.MouseMove += new MouseEventHandler(this.DraggingThumb_MouseMove1);
      }
    }
  }

  private void DraggingThumb_MouseMove1(object sender, MouseEventArgs e) => this.ispressed = true;

  private void DraggingThumb_MouseLeftButtonUp1(object sender, MouseButtonEventArgs e)
  {
    this.ispressed = false;
  }

  private void DraggingThumb_MouseLeftButtonDown1(object sender, MouseButtonEventArgs e)
  {
    this.ispressed = true;
  }

  internal static IEnumerable<T> FindVisualChildren<T>(DependencyObject parent) where T : DependencyObject
  {
    int childrenCount = 0;
    if (parent != null)
      childrenCount = VisualTreeHelper.GetChildrenCount(parent);
    for (int i = 0; i < childrenCount; ++i)
    {
      DependencyObject child = VisualTreeHelper.GetChild(parent, i);
      if (child is T _)
        yield return (T) child;
      foreach (T other in ToolBarAdv.FindVisualChildren<T>(child))
        yield return other;
    }
  }

  public override void OnApplyTemplate()
  {
    FrameworkElement overflowButton = this.OverflowButton;
    if (this.DraggingThumb != null)
    {
      this.DraggingThumb.MouseLeftButtonDown -= new MouseButtonEventHandler(this.DraggingThumb_MouseLeftButtonDown);
      this.DraggingThumb.MouseMove -= new MouseEventHandler(this.DraggingThumb_MouseMove);
      this.DraggingThumb.MouseLeftButtonUp -= new MouseButtonEventHandler(this.DraggingThumb_MouseLeftButtonUp);
    }
    this.ToolBarPanel = this.GetTemplateChild("PART_ToolBarPanel") as ToolBarPanelAdv;
    this.DraggingThumb = this.GetTemplateChild("PART_DragThumb") as Grid;
    this.OverflowButton = this.GetTemplateChild("PART_OverflowButton") as FrameworkElement;
    this.OverflowPanel = (Panel) (this.GetTemplateChild("PART_ToolBarOverflowPanel") as ToolBarOverflowPanel);
    this.addorRemoveButton = this.GetTemplateChild("PART_AddRemoveButtons") as DropDownButtonAdv;
    if (this.addorRemoveButton != null)
      this.addorRemoveButton.DropDownClosed += new RoutedEventHandler(this.addorRemoveButton_DropDownClosed);
    Popup overflowPopup = this.OverflowPopup;
    this.OverflowPopup = this.GetTemplateChild("PART_OverflowPopup") as Popup;
    if (this.OverflowPopup != null)
    {
      this.OverflowPopup.Closed -= new EventHandler(this.OverflowPopup_Closed);
      this.OverflowPopup.Closed += new EventHandler(this.OverflowPopup_Closed);
    }
    if (overflowPopup != this.OverflowPopup && overflowPopup != null)
      overflowPopup.IsOpen = false;
    if (this.DraggingThumb != null)
    {
      this.DraggingThumb.MouseLeftButtonDown += new MouseButtonEventHandler(this.DraggingThumb_MouseLeftButtonDown);
      this.DraggingThumb.MouseMove += new MouseEventHandler(this.DraggingThumb_MouseMove);
      this.DraggingThumb.MouseLeftButtonUp += new MouseButtonEventHandler(this.DraggingThumb_MouseLeftButtonUp);
    }
    if (this.OverflowButton != null)
      (this.OverflowButton as ToggleButton).Click += new RoutedEventHandler(this.OverflowButton_Click);
    if (this.ToolBarPanel != null && this.ToolBarPanel.Orientation != this.Orientation)
      this.ToolBarPanel.Orientation = this.Orientation;
    this.PART_AddRemoveItems = this.GetTemplateChild("PART_AddRemoveItems") as DropDownMenuGroup;
    this.overflowHorizantalPath = this.GetTemplateChild("horizontalLeftOverflowPath") as Path;
    this.overflowHorizantalPathRight = this.GetTemplateChild("horizontalRightOverflowPath") as Path;
    this.overflowVerticalPath = this.GetTemplateChild("verticalTopOverflowPath") as Path;
    this.overflowVerticalPathBottom = this.GetTemplateChild("verticalBottomOverflowPath") as Path;
    this.defaultDropDownIconTemplate = this.Template.Resources[(object) "dropDownIconTemplate"] as DataTemplate;
    if (this.DraggingThumb != null && this.IsDragging)
    {
      this.canMouseMoveExecute = false;
      this.DraggingThumb.CaptureMouse();
    }
    base.OnApplyTemplate();
    this.InsertToolStripItems();
    if (this.Tray != null)
      this.Orientation = this.Tray.Orientation;
    this.UpdateVisualState();
  }

  protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.Visibility == Visibility.Collapsed && this.Tray != null)
      this.Tray.InvalidateMeasure();
    base.OnPropertyChanged(e);
  }

  private void addorRemoveButton_DropDownClosed(object sender, RoutedEventArgs e)
  {
    this.OverflowPopup.IsOpen = false;
    this.OverflowPopup.StaysOpen = false;
  }

  private void OverflowPopup_Closed(object sender, EventArgs e)
  {
    if (this.addorRemoveButton == null)
      return;
    this.addorRemoveButton.IsPressed = false;
    this.addorRemoveButton.IsDropDownOpen = false;
    Syncfusion.Windows.VisualStateManager.GoToState((Control) this.addorRemoveButton, "Normal", true);
  }

  private static void OnFloatingBarLocationChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    (obj as ToolBarAdv).OnFloatingBarLocationChanged(args);
  }

  private void OnFloatingBarLocationChanged(DependencyPropertyChangedEventArgs args)
  {
    Point newValue = (Point) args.NewValue;
    if (this.floatingToolBar == null)
      return;
    this.floatingToolBar.popup.HorizontalOffset = newValue.X;
    this.floatingToolBar.popup.VerticalOffset = newValue.Y;
  }

  private static void OnControlsResourceDictionaryPropertyChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    (obj as ToolBarAdv).OnControlsResourceDictionaryPropertyChanged(args);
  }

  private void OnControlsResourceDictionaryPropertyChanged(DependencyPropertyChangedEventArgs args)
  {
    this.InvalidateMeasure();
    ResourceDictionary resourceDictionary = this.ControlsResourceDictionary;
    if (this.generatedConatiner == null)
      return;
    foreach (FrameworkElement frameworkElement in this.generatedConatiner.Values)
    {
      if (frameworkElement.GetType().Name == "ButtonAdv" || frameworkElement.GetType().Name == "DropDownButtonAdv" || frameworkElement.GetType().Name == "SplitButtonAdv")
      {
        if (resourceDictionary.Contains((object) $"SyncfusionToolBar{frameworkElement.GetType().Name}Style"))
          frameworkElement.Style = resourceDictionary[(object) $"SyncfusionToolBar{frameworkElement.GetType().Name}Style"] as Style;
      }
      else if (resourceDictionary.Contains((object) $"ToolBar{frameworkElement.GetType().Name}Style"))
        frameworkElement.Style = resourceDictionary[(object) $"ToolBar{frameworkElement.GetType().Name}Style"] as Style;
    }
  }

  internal void OnToolBarStateChanged(ToolBarState oldState, ToolBarState newState)
  {
    if (this.isInternallyChangingState || this.Tray == null || !this.Tray.IsHostedInToolBarManager || !this.isLoaded)
      return;
    switch (newState)
    {
      case ToolBarState.Docking:
        if (oldState == ToolBarState.Floating || this.floatingToolBar != null)
        {
          this.DockToolBar(DockArea.Top);
          if (this.Visibility == Visibility.Hidden)
          {
            this.Visibility = Visibility.Visible;
            break;
          }
          break;
        }
        if (oldState == ToolBarState.Hidden)
        {
          this.Visibility = Visibility.Visible;
          if (this.Tray != null)
          {
            this.Tray.InvalidateLayout();
            if (this.Tray.IsHostedInToolBarManager)
            {
              this.Tray.ToolBarManager.InvalidateLayout();
              break;
            }
            break;
          }
          break;
        }
        break;
      case ToolBarState.Hidden:
        switch (oldState)
        {
          case ToolBarState.Docking:
            this.Visibility = Visibility.Collapsed;
            if (this.Tray != null)
            {
              this.Tray.InvalidateLayout();
              if (this.Tray.IsHostedInToolBarManager)
              {
                this.Tray.ToolBarManager.InvalidateLayout();
                break;
              }
              break;
            }
            break;
          case ToolBarState.Floating:
            if (this.floatingToolBar != null)
            {
              this.floatingToolBar.popup.IsOpen = false;
              break;
            }
            break;
        }
        break;
      default:
        if (oldState == ToolBarState.Docking || this.floatingToolBar == null)
        {
          this.FloatToolBar(this.FloatingBarLocation, false);
          break;
        }
        if (oldState == ToolBarState.Hidden)
        {
          this.floatingToolBar.popup.IsOpen = true;
          break;
        }
        break;
    }
    this.ChangeStateInternally(newState);
  }

  internal void DockToolBar(DockArea area)
  {
    if (area == DockArea.None)
      return;
    if (this.floatingToolBar != null)
    {
      this.floatingToolBar.Visibility = Visibility.Collapsed;
      if (this.floatingToolBar.popup != null)
        this.floatingToolBar.popup.IsOpen = false;
      this.floatingToolBar.panel.Children.Clear();
      if (this.Tray.ToolBarManager.FloatingToolBars.Contains(this.floatingToolBar))
        this.Tray.ToolBarManager.FloatingToolBars.Remove(this.floatingToolBar);
      this.floatingToolBar = (FloatingToolBar) null;
    }
    if (this.Tray == null || !this.Tray.IsHostedInToolBarManager)
      return;
    this.Tray.ToolBarManager.DockToolBar(this, area);
  }

  internal void ChangeStateInternally(ToolBarState state)
  {
    this.isInternallyChangingState = true;
    ToolBarManager.SetToolBarState(this, state);
    this.isInternallyChangingState = false;
  }

  private void RootVisual_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (!this.IsOverflowOpen || this.OverflowPopup == null)
      return;
    this.IsOverflowOpen = false;
    (this.OverflowButton as ToggleButton).IsChecked = new bool?(false);
  }

  private void UpdateOverflowPathsVisibility()
  {
    Visibility visibility = this.HasOverflowItems ? Visibility.Visible : Visibility.Collapsed;
    if (this.overflowVerticalPath != null)
      this.overflowVerticalPath.Visibility = visibility;
    if (this.overflowVerticalPathBottom != null)
      this.overflowVerticalPathBottom.Visibility = visibility;
    if (this.overflowHorizantalPath != null)
      this.overflowHorizantalPath.Visibility = visibility;
    if (this.overflowHorizantalPathRight == null)
      return;
    this.overflowHorizantalPathRight.Visibility = visibility;
  }

  private void DraggingThumb_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    this.IsDragging = true;
  }

  private void OverflowButton_Click(object sender, RoutedEventArgs e)
  {
    this.IsOverflowOpen = !this.IsOverflowOpen;
    if (!this.HasOverflowItems && !this.EnableAddRemoveButton)
      this.OverflowPopup.IsOpen = false;
    else
      this.OverflowPopup.IsOpen = true;
  }

  private void DraggingThumb_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    this.IsDragging = false;
  }

  private void DraggingThumb_MouseMove(object sender, MouseEventArgs e)
  {
    if (!this.canMouseMoveExecute)
    {
      this.canMouseMoveExecute = true;
    }
    else
    {
      if (!this.IsDragging || !this.IsHostedInsideTray || this.Tray.IsLocked)
        return;
      Point position1 = e.GetPosition((IInputElement) this.Tray);
      ToolBarBand band = this.Tray.GetBandFromPoint(position1);
      if (band == null && this.ToolBarBand.ToolBars.Count > 1)
        band = this.Tray.TryCreateNewBand(position1);
      if (band != this.ToolBarBand && band != null)
        this.Tray.MoveBarToBand(this, band, OrientedValue.GetOrientedXValue(position1, this.Orientation));
      else if (band == this.ToolBarBand)
      {
        band.IsWindowResizing = false;
        double orientedXvalue = OrientedValue.GetOrientedXValue(e.GetPosition((IInputElement) this.Tray), this.Orientation);
        Size size = new Size(this.MinWidth, this.MinHeight);
        if (orientedXvalue <= OrientedValue.GetOrientedWidthValue(size, this.Orientation))
          return;
        int index = band.ToolBars.IndexOf(this) - 1;
        if (!(size != OrientedValue.GetOrientedSize(orientedXvalue, band.Size, this.Orientation)))
          return;
        Size orientedSize = OrientedValue.GetOrientedSize(orientedXvalue, band.Size, this.Orientation);
        band.Measure(orientedSize, index);
        band.ArrangeToolBars(band.BoundingRectangle.X, band.BoundingRectangle.Y);
        band.IsWindowResizing = true;
      }
      else
      {
        if (!this.Tray.IsHostedInToolBarManager)
          return;
        DockArea dockArea = this.Tray.FindDockArea(e.GetPosition((IInputElement) this.Tray.ToolBarManager));
        if (dockArea != DockArea.None)
        {
          if (this.Tray.ToolBarManager.FloatingToolBars.Contains(this.floatingToolBar))
            this.Tray.ToolBarManager.FloatingToolBars.Remove(this.floatingToolBar);
          this.floatingToolBar = (FloatingToolBar) null;
          this.Tray.DockToolBar(this, dockArea);
          ToolBarManager.SetDockArea((DependencyObject) this, dockArea);
          this.ChangeStateInternally(ToolBarState.Docking);
        }
        else
        {
          this.IsDragging = false;
          Point position2 = e.GetPosition((IInputElement) null);
          position2.X -= 10.0;
          position2.Y -= 10.0;
          this.FloatToolBar(position2, true);
        }
      }
    }
  }

  internal void FloatToolBar(Point point, bool forceDrag)
  {
    try
    {
      if (this.floatingToolBar != null)
      {
        this.floatingToolBar.popup.IsOpen = true;
      }
      else
      {
        this.ClearTempItems();
        FloatingToolBar floatingToolBar = new FloatingToolBar();
        floatingToolBar.DataContext = this.DataContext;
        this.floatingToolBar = floatingToolBar;
        floatingToolBar.Title = this.ToolBarName;
        floatingToolBar.InsertItems(this);
        floatingToolBar.ToolBar = this;
        floatingToolBar.Manager = this.Tray.ToolBarManager;
        floatingToolBar.ForceDrag = forceDrag;
        this.floatingToolBar.Style = this.Tray.ToolBarManager.FloatingToolBarStyle;
        this.Tray.ToolBarManager.FloatingToolBars.Add(this.floatingToolBar);
        if (this.Tray != null && this.Tray.ToolBars.Contains(this))
        {
          this.Tray.Remove(this);
          this.Tray.InvalidateLayout();
          this.Tray.ToolBarManager.Invalidate();
        }
        this.Tray = (ToolBarTrayAdv) null;
        Popup popup = new Popup();
        popup.Child = (UIElement) floatingToolBar;
        floatingToolBar.popup = popup;
        this.FloatingBarLocation = point;
        popup.HorizontalOffset = this.FloatingBarLocation.X;
        popup.VerticalOffset = this.FloatingBarLocation.Y;
        popup.IsOpen = true;
        ToolBarManager.SetDockArea((DependencyObject) this, DockArea.None);
        this.ChangeStateInternally(ToolBarState.Floating);
        this.floatingToolBar.ApplyStyleForControls();
      }
    }
    catch
    {
    }
  }

  private static void OnBandChanged(DependencyObject dp, DependencyPropertyChangedEventArgs args)
  {
    ((ToolBarAdv) dp).OnBandChanged(args);
  }

  protected void OnBandChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.Tray == null || this.Tray.Bands == null)
      return;
    bool flag = false;
    List<ToolBarBand> bands = this.Tray.Bands;
    for (int index = 0; index < bands.Count; ++index)
    {
      if ((int) args.OldValue == bands[index].BandNo && bands[index].ToolBars != null && bands[index].ToolBars.Contains(this))
      {
        bands[index].ToolBars.Remove(this);
        if (bands[index].ToolBars.Count == 0)
        {
          this.Tray.Bands.Remove(bands[index]);
          int num = index - 1;
          break;
        }
        break;
      }
    }
    foreach (ToolBarBand band in this.Tray.Bands)
    {
      if ((int) args.NewValue == band.BandNo)
      {
        flag = true;
        if (band.ToolBars != null)
        {
          if (!band.ToolBars.Contains(this))
          {
            band.ToolBars.Add(this);
            break;
          }
          break;
        }
        break;
      }
    }
    if (!flag)
    {
      ToolBarBand toolBarBand = new ToolBarBand();
      toolBarBand.BandNo = this.Band;
      toolBarBand.Insert(this);
      this.Tray.Bands.Add(toolBarBand);
    }
    this.Tray.Bands.Sort(new Comparison<ToolBarBand>(ToolBarBand.CompareBand));
  }

  private static void OnBandIndexChanged(
    DependencyObject dp,
    DependencyPropertyChangedEventArgs args)
  {
    ((ToolBarAdv) dp).OnBandIndexChanged(args);
  }

  protected void OnBandIndexChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.ToolBarBand == null)
      return;
    this.ToolBarBand.CorrectOrder();
  }

  protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    base.OnItemsSourceChanged(oldValue, newValue);
    this.InvalidateMeasure();
    this.InvalidateArrange();
    if (!this.IsHostedInsideTray)
      return;
    this.Tray.InvalidateMeasure();
    this.Tray.InvalidateArrange();
  }

  protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
  {
    if (this.defaultDropDownIconTemplate == null)
      this.defaultDropDownIconTemplate = this.Template.Resources[(object) "dropDownIconTemplate"] as DataTemplate;
    this.ToolBarItemInfoCollection.Clear();
    foreach (object obj in (IEnumerable) this.Items)
    {
      if (obj is FrameworkElement frameworkElement)
      {
        ToolBarIteminfo toolBarIteminfo = new ToolBarIteminfo();
        toolBarIteminfo.Label = ToolBarAdv.GetLabel((DependencyObject) frameworkElement);
        toolBarIteminfo.Icon = ToolBarAdv.GetIcon((DependencyObject) frameworkElement);
        DataTemplate iconTemplate = ToolBarAdv.GetIconTemplate((DependencyObject) frameworkElement);
        toolBarIteminfo.IconTemplate = iconTemplate ?? this.defaultDropDownIconTemplate;
        toolBarIteminfo.Host = (object) frameworkElement;
        if (!this.ToolBarItemInfoCollection.Contains(toolBarIteminfo) && (!string.IsNullOrEmpty(toolBarIteminfo.Label) || toolBarIteminfo.Icon != null))
          this.ToolBarItemInfoCollection.Add(toolBarIteminfo);
      }
    }
    if (e.NewItems == null)
      return;
    object newItem1 = e.NewItems[0];
    if (e.Action == NotifyCollectionChangedAction.Add)
    {
      if (!this.IsOverflowAlways(newItem1))
      {
        this.ToolStripItems.Add(newItem1);
        this.InsertToolStripItems();
        ToolBarAdv.SetIsOverflowItem(this.GetContainerOfItem(newItem1), false);
      }
      else
      {
        this.OverflowItems.Add(newItem1);
        ToolBarAdv.SetIsOverflowItem(this.GetContainerOfItem(newItem1), true);
      }
    }
    else if (e.Action == NotifyCollectionChangedAction.Remove)
    {
      foreach (object newItem2 in (IEnumerable) e.NewItems)
      {
        if (this.generatedConatiner.ContainsKey(newItem2))
          this.generatedConatiner.Remove(newItem2);
      }
    }
    base.OnItemsChanged(e);
  }

  private bool IsOverflowAlways(object item)
  {
    DependencyObject containerOfItem = this.GetContainerOfItem(item);
    return containerOfItem != null && ToolBarAdv.GetOverflowMode(containerOfItem) == OverflowMode.Always;
  }

  private bool IsAlwaysInToolStrip(object item)
  {
    DependencyObject containerOfItem = this.GetContainerOfItem(item);
    return containerOfItem != null && ToolBarAdv.GetOverflowMode(containerOfItem) == OverflowMode.Never;
  }

  private new void UpdateVisualState()
  {
    if (this.Orientation == System.Windows.Controls.Orientation.Horizontal)
      Syncfusion.Windows.VisualStateManager.GoToState((Control) this, "Horizontal", true);
    else
      Syncfusion.Windows.VisualStateManager.GoToState((Control) this, "Vertical", true);
  }

  internal void ClearTempItems()
  {
    this.ToolStripItems.Clear();
    this.OverflowItems.Clear();
    this.ClearToolBarPanel();
    this.ClearOverflowPanel();
    this.CanToolStripItemsMoveToOverflow = true;
    this.CanOverflowItemMoveToToolStrip = true;
  }

  private void GenerateToolStripAndOverflowItems(Size size)
  {
    this.ClearTempItems();
    List<object> objectList = new List<object>();
    double num = 0.0;
    for (int index = 0; index < this.Items.Count; ++index)
    {
      UIElement containerOfItem = this.GetContainerOfItem(this.Items[index]) as UIElement;
      if (this.ToolBarPanel != null && !this.ToolBarPanel.Children.Contains(containerOfItem))
        this.InsertItemToPanel((Panel) this.ToolBarPanel, this.Items[index]);
      bool flag = true;
      if (this.IsAlwaysInToolStrip(this.Items[index]) && this.ToolStripItems != null)
      {
        this.ToolStripItems.Add(this.Items[index]);
        ToolBarAdv.SetIsOverflowItem((DependencyObject) containerOfItem, false);
        num += OrientedValue.GetOrientedWidthValue(this.GetSize((object) containerOfItem), this.Orientation);
      }
      else if (this.IsOverflowAlways(this.Items[index]) && this.OverflowItems != null)
      {
        this.OverflowItems.Add(this.Items[index]);
        ToolBarAdv.SetIsOverflowItem((DependencyObject) containerOfItem, true);
      }
      else
      {
        objectList.Add(this.Items[index]);
        flag = false;
      }
      if (flag && this.ToolBarPanel != null && this.ToolBarPanel.Children.Contains(containerOfItem))
        this.ToolBarPanel.Children.Remove(containerOfItem);
    }
    this.CanToolStripItemsMoveToOverflow = false;
    this.CanOverflowItemMoveToToolStrip = false;
    foreach (object obj in objectList)
    {
      if (obj != null)
      {
        UIElement containerOfItem = this.GetContainerOfItem(obj) as UIElement;
        double orientedWidthValue = OrientedValue.GetOrientedWidthValue(this.GetSize((object) containerOfItem), this.Orientation);
        if (num + orientedWidthValue <= OrientedValue.GetOrientedWidthValue(size, this.Orientation) && this.ToolStripItems != null)
        {
          this.ToolStripItems.Add(obj);
          ToolBarAdv.SetIsOverflowItem((DependencyObject) containerOfItem, false);
          num += OrientedValue.GetOrientedWidthValue(this.GetSize((object) containerOfItem), this.Orientation);
          this.CanToolStripItemsMoveToOverflow = true;
        }
        else if (this.OverflowItems != null)
        {
          this.CanOverflowItemMoveToToolStrip = true;
          this.OverflowItems.Add(obj);
          ToolBarAdv.SetIsOverflowItem((DependencyObject) containerOfItem, true);
        }
        if (this.ToolBarPanel != null && this.ToolBarPanel.Children.Contains(containerOfItem))
          this.ToolBarPanel.Children.Remove(containerOfItem);
      }
    }
    if (this.OverflowItems != null)
      this.HasOverflowItems = this.OverflowItems.Count > 0;
    if (!this.HasOverflowItems && !this.EnableAddRemoveButton && this.OverflowButton is ToggleButton)
      (this.OverflowButton as ToggleButton).IsEnabled = false;
    else if (this.OverflowButton is ToggleButton)
      (this.OverflowButton as ToggleButton).IsEnabled = true;
    objectList.Clear();
  }

  internal void InsertItemToPanel(Panel panel, object item)
  {
    if (panel == null || item == null)
      return;
    UIElement containerOfItem = this.GetContainerOfItem(item) as UIElement;
    if (panel.Children.Contains(containerOfItem) || !(containerOfItem is FrameworkElement))
      return;
    if ((containerOfItem as FrameworkElement).Parent != null && (containerOfItem as FrameworkElement).Parent is Panel && (containerOfItem as FrameworkElement).Parent is Panel)
      ((containerOfItem as FrameworkElement).Parent as Panel).Children.Remove(containerOfItem);
    this.RemoveLogicalChild((object) containerOfItem);
    panel.Children.Add(containerOfItem);
  }

  private void CacheContainer(object item, DependencyObject container)
  {
    if (this.generatedConatiner.ContainsKey(item))
      this.generatedConatiner[item] = container;
    else
      this.generatedConatiner.Add(item, container);
  }

  private DependencyObject GetContainerOfItem(object item)
  {
    if (item == null)
      return (DependencyObject) null;
    if (this.generatedConatiner.ContainsKey(item))
      return this.generatedConatiner[item];
    UIElement element = !this.IsItemItsOwnContainerOverride(item) ? this.GetContainerForItemOverride() as UIElement : item as UIElement;
    if (element != null)
    {
      this.PrepareContainerForItemOverride((DependencyObject) element, item);
      this.generatedConatiner.Add(item, (DependencyObject) element);
    }
    return (DependencyObject) element;
  }

  private void GenerateContainers()
  {
    foreach (object obj in (IEnumerable) this.Items)
      this.GetContainerOfItem(obj);
  }

  private Size GetSize(object item)
  {
    if (!(item is UIElement uiElement))
      return new Size();
    uiElement.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
    Size size = uiElement.DesiredSize;
    if ((uiElement.DesiredSize.IsEmpty || uiElement.DesiredSize.Width == 0.0 || uiElement.DesiredSize.Height == 0.0) && uiElement is FrameworkElement)
      size = new Size((uiElement as FrameworkElement).ActualWidth, (uiElement as FrameworkElement).ActualHeight);
    return size;
  }

  private void InsertToolStripItems()
  {
    this.ClearToolBarPanel();
    this.ClearContainers();
    for (int index = 0; index < this.ToolStripItems.Count; ++index)
      this.InsertItemToPanel((Panel) this.ToolBarPanel, this.ToolStripItems[index]);
  }

  private void InsertOverflowItems()
  {
    this.ClearOverflowPanel();
    for (int index = 0; index < this.OverflowItems.Count; ++index)
      this.InsertItemToPanel(this.OverflowPanel, this.OverflowItems[index]);
  }

  private void ClearContainers()
  {
    for (int index = 0; index < this.Items.Count; ++index)
    {
      DependencyObject element = this.ItemContainerGenerator.ContainerFromItem(this.Items[index]);
      if (element != null)
        this.ClearContainerForItemOverride(element, this.Items[index]);
    }
  }

  private void ClearToolBarPanel()
  {
    if (this.ToolBarPanel == null)
      return;
    this.ToolBarPanel.Children.Clear();
  }

  private void ClearOverflowPanel()
  {
    if (this.OverflowPanel == null)
      return;
    this.OverflowPanel.Children.Clear();
  }

  internal Size GetDesiredSize()
  {
    return OrientedValue.GetOrientedWidthValue(this.DesiredSize, this.Orientation) < OrientedValue.GetOrientedWidthValue(this.ExtraSize, this.Orientation) ? new Size(this.RequiredSize.Width + this.EmptySpace.Width, this.RequiredSize.Height + this.EmptySpace.Height) : new Size(this.DesiredSize.Width + this.EmptySpace.Width, this.DesiredSize.Height + this.EmptySpace.Height);
  }

  internal void Arrange(Size size) => this.ArrangeOverride(size);

  protected override Size ArrangeOverride(Size finalSize)
  {
    this.isArranged = true;
    return base.ArrangeOverride(finalSize);
  }

  protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
  {
    base.PrepareContainerForItemOverride(element, item);
    FrameworkElement frameworkElement = element as FrameworkElement;
    ResourceDictionary resourceDictionary = this.ControlsResourceDictionary;
    if (this.ItemContainerStyle != null && frameworkElement != null)
      frameworkElement.Style = this.ItemContainerStyle;
    if (this.floatingToolBar != null && ToolBarManager.GetToolBarState(this) == ToolBarState.Floating)
      resourceDictionary = this.floatingToolBar.ControlsResourceDictionary;
    if (frameworkElement != null)
    {
      if (frameworkElement.GetType().Name == "ButtonAdv" || frameworkElement.GetType().Name == "DropDownButtonAdv" || frameworkElement.GetType().Name == "SplitButtonAdv")
      {
        if (resourceDictionary.Contains((object) $"SyncfusionToolBar{frameworkElement.GetType().Name}Style"))
          frameworkElement.Style = resourceDictionary[(object) $"SyncfusionToolBar{frameworkElement.GetType().Name}Style"] as Style;
      }
      else if (resourceDictionary.Contains((object) $"ToolBar{frameworkElement.GetType().Name}Style"))
        frameworkElement.Style = resourceDictionary[(object) $"ToolBar{frameworkElement.GetType().Name}Style"] as Style;
    }
    if (!(frameworkElement is ToolBarItemSeparator))
      return;
    if (this.floatingToolBar != null)
      (frameworkElement as ToolBarItemSeparator).Orientation = System.Windows.Controls.Orientation.Horizontal;
    else
      (frameworkElement as ToolBarItemSeparator).Orientation = this.Orientation;
  }

  protected override DependencyObject GetContainerForItemOverride()
  {
    return base.GetContainerForItemOverride();
  }

  internal void Resize(Size availableSize)
  {
    this.isMeasured = false;
    this.Measure(availableSize);
    if (this.isMeasured)
      return;
    this.MeasureOverride(availableSize);
  }

  protected override Size MeasureOverride(Size availableSize)
  {
    this.GenerateContainers();
    if (this.IsLoaded && !this.ispressed && !this.IsDragging && this.Tray != null && this.Tray.ToolBarManager != null)
      this.Tray.ToolBarManager.InvalidateLayout();
    this.isMeasured = true;
    this.EmptySpace = new Size();
    double num1 = 0.0;
    double num2 = 0.0;
    if (this.OverflowButton != null)
      this.OverflowButton.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
    if (this.DraggingThumb != null)
      this.DraggingThumb.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
    if (this.OverflowButton != null && this.DraggingThumb != null)
      this.ExtraSize = this.Orientation != System.Windows.Controls.Orientation.Horizontal ? new Size(0.0, this.OverflowButton.DesiredSize.Height + this.DraggingThumb.DesiredSize.Height) : new Size(this.OverflowButton.DesiredSize.Width + this.DraggingThumb.DesiredSize.Width, 0.0);
    this.GenerateToolStripAndOverflowItems(this.GetValidSize(availableSize));
    this.InsertToolStripItems();
    this.InsertOverflowItems();
    if (this.ToolBarPanel != null)
      this.ToolBarPanel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
    if (this.OverflowPanel != null)
      this.OverflowPanel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
    if (this.ToolBarPanel != null)
      num2 = this.Orientation == System.Windows.Controls.Orientation.Horizontal ? (this.ToolBarPanel.DesiredSize.Height < this.MinHeight ? this.MinHeight : this.ToolBarPanel.DesiredSize.Height) : this.ToolBarPanel.DesiredSize.Height;
    if (this.ToolBarPanel != null)
      num1 = this.Orientation == System.Windows.Controls.Orientation.Vertical ? (this.ToolBarPanel.DesiredSize.Width < this.MinWidth ? this.MinWidth : this.ToolBarPanel.DesiredSize.Width) : this.ToolBarPanel.DesiredSize.Width;
    this.RequiredSize = new Size(num1 + this.ExtraSize.Width, num2 + this.ExtraSize.Height);
    return base.MeasureOverride(availableSize);
  }

  private Size GetValidSize(Size size)
  {
    Size validSize = size;
    if (double.IsInfinity(size.Width))
      validSize.Width = double.MaxValue;
    if (double.IsInfinity(size.Height))
      validSize.Height = double.MaxValue;
    if (this.Orientation == System.Windows.Controls.Orientation.Horizontal)
      validSize.Width = Math.Max(0.0, validSize.Width - this.ExtraSize.Width);
    else
      validSize.Height = Math.Max(0.0, validSize.Height - this.ExtraSize.Height);
    return validSize;
  }
}
