// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.MenuAdv
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Licensing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

[SkinType(SkinVisualStyle = Skin.Default, Type = typeof (MenuAdv), XamlResource = "/Syncfusion.Shared.Wpf;component/Controls/MenuAdv/Themes/MenuAdvResources.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Blue, Type = typeof (MenuAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/MenuAdv/Themes/Office2010BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Silver, Type = typeof (MenuAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/MenuAdv/Themes/Office2010SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2013, Type = typeof (MenuAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/MenuAdv/Themes/Office2013Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Blue, Type = typeof (MenuAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/MenuAdv/Themes/Office2007BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Silver, Type = typeof (MenuAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/MenuAdv/Themes/Office2007SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Black, Type = typeof (MenuAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/MenuAdv/Themes/Office2007BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Black, Type = typeof (MenuAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/MenuAdv/Themes/Office2010BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2003, Type = typeof (MenuAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/MenuAdv/Themes/Office2003Style.xaml")]
[SkinType(SkinVisualStyle = Skin.VS2010, Type = typeof (MenuAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/MenuAdv/Themes/VS2010Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Blend, Type = typeof (MenuAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/MenuAdv/Themes/BlendStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyRed, Type = typeof (MenuAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/MenuAdv/Themes/ShinyRedStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyBlue, Type = typeof (MenuAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/MenuAdv/Themes/ShinyBlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.SyncOrange, Type = typeof (MenuAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/MenuAdv/Themes/SyncOrangeStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Metro, Type = typeof (MenuAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/MenuAdv/Themes/MetroStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Transparent, Type = typeof (MenuAdv), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/MenuAdv/Themes/TransparentStyle.xaml")]
public class MenuAdv : ItemsControl
{
  internal Canvas OutsidePopupCanvas;
  internal bool IsItemSelected;
  internal bool IsItemMouseOver;
  internal bool isMouseOver;
  internal bool isInitialOrientation = true;
  internal bool firstClick;
  internal bool isSecondClick;
  internal double PanelHeight;
  internal double PanelWidth;
  private double menuItemWidth;
  internal bool IsAltKeyPressed;
  internal bool IsMenuItemOpened;
  internal int accesscount;
  internal bool IsTopLevelItem;
  internal bool IsExecuteonLostFocus;
  internal MenuItemAdv currentOpenMenu;
  internal MenuItemAdv MouseOverItem;
  internal bool isMouseEnterpopup;
  internal bool IsAllPopupClosed;
  internal Window mainWindow;
  private bool isMenuLoaded;
  public static readonly DependencyProperty IsScrollEnabledProperty = DependencyProperty.Register(nameof (IsScrollEnabled), typeof (bool), typeof (MenuAdv), new PropertyMetadata((object) true));
  public static readonly DependencyProperty ExpandModeProperty = DependencyProperty.Register(nameof (ExpandMode), typeof (ExpandModes), typeof (MenuAdv), new PropertyMetadata((object) ExpandModes.ExpandOnClick));
  public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (Orientation), typeof (MenuAdv), new PropertyMetadata((object) Orientation.Horizontal, new PropertyChangedCallback(MenuAdv.OnOrientationChanged)));
  public static readonly DependencyProperty PopUpAnimationTypeProperty = DependencyProperty.Register(nameof (PopUpAnimationType), typeof (AnimationTypes), typeof (MenuAdv), new PropertyMetadata((object) AnimationTypes.None));
  public static readonly DependencyProperty FocusOnAltProperty = DependencyProperty.Register(nameof (FocusOnAlt), typeof (bool), typeof (MenuAdv), new PropertyMetadata((object) false));

  public MenuAdv()
  {
    this.DefaultStyleKey = (object) typeof (MenuAdv);
    this.ContainersToItems = (IDictionary<DependencyObject, object>) new Dictionary<DependencyObject, object>();
    EventManager.RegisterClassHandler(typeof (MenuAdv), AccessKeyManager.AccessKeyPressedEvent, (Delegate) new AccessKeyPressedEventHandler(MenuAdv.OnAccessKeyPressed));
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  static MenuAdv() => FusionLicenseProvider.GetLicenseType(Platform.WPF);

  private static void OnAccessKeyPressed(object sender, AccessKeyPressedEventArgs e)
  {
    if (!Keyboard.IsKeyDown(Key.LeftAlt) && !Keyboard.IsKeyDown(Key.RightAlt))
    {
      e.Scope = sender;
      if (sender != null && sender is MenuAdv)
        (sender as MenuAdv).IsAltKeyPressed = false;
      e.Handled = true;
    }
    else
    {
      if (sender == null || !(sender is MenuAdv))
        return;
      (sender as MenuAdv).IsAltKeyPressed = true;
    }
  }

  internal IDictionary<DependencyObject, object> ContainersToItems { get; set; }

  [Description("Represents the Orientation of the MenuAdv, Which may be Horizontal or Vertical.")]
  [Category("Appearance")]
  public Orientation Orientation
  {
    get => (Orientation) this.GetValue(MenuAdv.OrientationProperty);
    set => this.SetValue(MenuAdv.OrientationProperty, (object) value);
  }

  [Category("Common Properties")]
  [Description("Represents the Expand modes of MenuItems which present in the MenuAdv")]
  public ExpandModes ExpandMode
  {
    get => (ExpandModes) this.GetValue(MenuAdv.ExpandModeProperty);
    set => this.SetValue(MenuAdv.ExpandModeProperty, (object) value);
  }

  [Description("Represents menu items present in submenu popup can be scrollable or not")]
  [Category("Common Properties")]
  public bool IsScrollEnabled
  {
    get => (bool) this.GetValue(MenuAdv.IsScrollEnabledProperty);
    set => this.SetValue(MenuAdv.IsScrollEnabledProperty, (object) value);
  }

  [Description("Represents the ANimation type to open the subMenu popup, which may be None, Fade, Slide or Scroll.")]
  [Category("Common Properties")]
  public AnimationTypes PopUpAnimationType
  {
    get => (AnimationTypes) this.GetValue(MenuAdv.PopUpAnimationTypeProperty);
    set => this.SetValue(MenuAdv.PopUpAnimationTypeProperty, (object) value);
  }

  [Description("Represent the Focus Enable when pressed on Alt Key")]
  [Category("Common Properties")]
  public bool FocusOnAlt
  {
    get => (bool) this.GetValue(MenuAdv.FocusOnAltProperty);
    set => this.SetValue(MenuAdv.FocusOnAltProperty, (object) value);
  }

  private static void OnItemContainerStyleChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
  }

  private static void OnOrientationChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs e)
  {
    MenuAdv menuAdv = (MenuAdv) obj;
    if (!menuAdv.isInitialOrientation && obj != null && obj is UIElement)
    {
      StackPanel stackpanel = MenuAdv.GetStackpanel(obj as UIElement);
      if (stackpanel != null && e.NewValue != null)
        stackpanel.Orientation = (Orientation) e.NewValue;
    }
    menuAdv.CloseAllPopUps();
    menuAdv.ChangeExtendButtonVisibility();
  }

  private static StackPanel GetStackpanel(UIElement parent)
  {
    int childrenCount = VisualTreeHelper.GetChildrenCount((DependencyObject) parent);
    if (childrenCount > 0)
    {
      int childIndex = 0;
      while (childIndex < childrenCount)
      {
        UIElement child = (UIElement) VisualTreeHelper.GetChild((DependencyObject) parent, childIndex);
        ++childIndex;
        if (!(child.GetType() != typeof (StackPanel)))
          return child as StackPanel;
        StackPanel stackpanel = MenuAdv.GetStackpanel(child);
        if (stackpanel != null)
          return stackpanel;
      }
    }
    return (StackPanel) null;
  }

  protected override void OnPreviewKeyUp(KeyEventArgs e)
  {
    this.accesscount = 0;
    base.OnPreviewKeyUp(e);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.isInitialOrientation = true;
    this.Unloaded -= new RoutedEventHandler(this.MenuAdv_Unloaded);
    this.Unloaded += new RoutedEventHandler(this.MenuAdv_Unloaded);
    this.Loaded -= new RoutedEventHandler(this.MenuAdv_Loaded);
    this.Loaded += new RoutedEventHandler(this.MenuAdv_Loaded);
  }

  private void MenuAdv_Loaded(object sender, RoutedEventArgs e)
  {
    this.isMenuLoaded = true;
    this.mainWindow = Window.GetWindow((DependencyObject) this);
    if (this.mainWindow == null)
      return;
    this.MouseLeftButtonDown -= new MouseButtonEventHandler(this.MenuAdv_MouseLeftButtonDown);
    this.mainWindow.PreviewMouseLeftButtonUp -= new MouseButtonEventHandler(this.MainWindow_PreviewMouseLeftButtonUp);
    this.mainWindow.PreviewMouseLeftButtonDown -= new MouseButtonEventHandler(this.MainWindow_PreviewMouseLeftButtonDown);
    this.mainWindow.PreviewMouseRightButtonUp -= new MouseButtonEventHandler(this.MainWindow_PreviewMouseRightButtonUp);
    this.mainWindow.Deactivated -= new EventHandler(this.MainWindow_Deactivated);
    this.mainWindow.LocationChanged -= new EventHandler(this.MainWindow_LocationChanged);
    this.MouseLeftButtonDown += new MouseButtonEventHandler(this.MenuAdv_MouseLeftButtonDown);
    this.mainWindow.Deactivated += new EventHandler(this.MainWindow_Deactivated);
    this.mainWindow.LocationChanged += new EventHandler(this.MainWindow_LocationChanged);
    this.mainWindow.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(this.MainWindow_PreviewMouseLeftButtonUp);
    this.mainWindow.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.MainWindow_PreviewMouseLeftButtonDown);
    this.mainWindow.PreviewMouseRightButtonUp += new MouseButtonEventHandler(this.MainWindow_PreviewMouseRightButtonUp);
    this.mainWindow.KeyUp += new KeyEventHandler(this.MainWindow_KeyUp);
  }

  private void MainWindow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    this.IsExecuteonLostFocus = true;
  }

  private void MenuAdv_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if ((FrameworkElement) sender != this || this.ExpandMode != ExpandModes.ExpandOnClick)
      return;
    e.Handled = true;
  }

  private void MenuAdv_Unloaded(object sender, RoutedEventArgs e)
  {
    if (this.mainWindow != null)
    {
      this.MouseLeftButtonDown -= new MouseButtonEventHandler(this.MenuAdv_MouseLeftButtonDown);
      this.mainWindow.PreviewMouseLeftButtonUp -= new MouseButtonEventHandler(this.MainWindow_PreviewMouseLeftButtonUp);
      this.mainWindow.PreviewMouseLeftButtonDown -= new MouseButtonEventHandler(this.MainWindow_PreviewMouseLeftButtonDown);
      this.mainWindow.PreviewMouseRightButtonUp -= new MouseButtonEventHandler(this.MainWindow_PreviewMouseRightButtonUp);
      this.mainWindow.Deactivated -= new EventHandler(this.MainWindow_Deactivated);
      this.mainWindow.LocationChanged -= new EventHandler(this.MainWindow_LocationChanged);
      this.mainWindow.KeyUp -= new KeyEventHandler(this.MainWindow_KeyUp);
    }
    this.Unloaded -= new RoutedEventHandler(this.MenuAdv_Unloaded);
  }

  private void MainWindow_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
  {
    for (int index = 0; index < this.ContainersToItems.Count; ++index)
    {
      if (this.ItemContainerGenerator.ContainerFromIndex(index) is MenuItemAdv && ((MenuItemAdv) this.ItemContainerGenerator.ContainerFromIndex(index)).ParentMenuAdv != null && !((MenuItemAdv) this.ItemContainerGenerator.ContainerFromIndex(index)).IsSubMenuOpen)
        this.IsItemMouseOver = true;
    }
    if (!this.IsItemMouseOver)
    {
      this.IsItemSelected = false;
      this.CloseAllPopUps();
    }
    if (!this.IsMenuItemOpened || e.Source == null || e.Source is MenuItemAdv || e.OriginalSource == null || !(e.OriginalSource is Visual) || VisualUtils.FindAncestor((Visual) e.OriginalSource, typeof (MenuItemAdv)) is MenuItemAdv)
      return;
    this.CloseAllPopUps();
  }

  private void MainWindow_KeyUp(object sender, KeyEventArgs e)
  {
    if (!this.FocusOnAlt || e.SystemKey != Key.LeftAlt && e.SystemKey != Key.RightAlt || this.Items[0] == null)
      return;
    this.IsAltKeyPressed = true;
    MenuItemAdv menuItemAdv = !(this.Items[0] is MenuItemAdv) ? this.ItemContainerGenerator.ContainerFromIndex(0) as MenuItemAdv : this.Items[0] as MenuItemAdv;
    if (menuItemAdv != null && !menuItemAdv.IsFocused && this.IsEnabled)
    {
      menuItemAdv.Focus();
      menuItemAdv.CallVisualState(menuItemAdv, "MenuItemSelected");
      e.Handled = true;
    }
    else
    {
      if (menuItemAdv == null || !this.IsEnabled)
        return;
      TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Down);
      if (Keyboard.FocusedElement is UIElement focusedElement)
        focusedElement.MoveFocus(request);
      menuItemAdv.CallVisualState(menuItemAdv, "Normal");
      e.Handled = true;
    }
  }

  private void MainWindow_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    for (int index = 0; index < this.ContainersToItems.Count; ++index)
    {
      if (this.ItemContainerGenerator.ContainerFromIndex(index) is MenuItemAdv && ((MenuItemAdv) this.ItemContainerGenerator.ContainerFromIndex(index)).ParentMenuAdv != null && !((MenuItemAdv) this.ItemContainerGenerator.ContainerFromIndex(index)).IsSubMenuOpen)
        this.IsItemMouseOver = true;
    }
    if (!this.IsItemMouseOver)
    {
      this.IsItemSelected = false;
      this.CloseAllPopUps();
    }
    if (this.IsMenuItemOpened && e.Source != null && !(e.Source is MenuItemAdv) && e.OriginalSource != null && e.OriginalSource is Visual && !(VisualUtils.FindAncestor((Visual) e.OriginalSource, typeof (MenuItemAdv)) is MenuItemAdv))
      this.CloseAllPopUps();
    this.IsExecuteonLostFocus = false;
  }

  private void MainWindow_LocationChanged(object sender, EventArgs e) => this.CloseAllPopUps();

  private void MainWindow_Deactivated(object sender, EventArgs e) => this.CloseAllPopUps();

  protected override bool IsItemItsOwnContainerOverride(object item)
  {
    return item is MenuItemAdv || item is MenuItemSeparator;
  }

  protected override DependencyObject GetContainerForItemOverride()
  {
    return (DependencyObject) new MenuItemAdv();
  }

  internal void UpdateSeparatorVisibility(MenuItemSeparator item)
  {
  }

  protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
  {
    if (element is MenuItemAdv element1)
    {
      element1.ParentMenuAdv = this;
      element1.ParentMenuItemAdv = (MenuItemAdv) null;
      element1.Parent = this;
      element1.ParentMenu = this;
    }
    if (!(item is MenuItemSeparator))
      this.ContainersToItems[element] = item;
    if (this.isMenuLoaded)
    {
      IDictionary<DependencyObject, object> dictionary = (IDictionary<DependencyObject, object>) new Dictionary<DependencyObject, object>();
      IDictionary<DependencyObject, object> containersToItems = this.ContainersToItems;
      int num = this.Items.IndexOf(item);
      KeyValuePair<DependencyObject, object> keyValuePair1 = this.ContainersToItems.ElementAt<KeyValuePair<DependencyObject, object>>(this.ContainersToItems.Count - 1);
      ((ICollection<KeyValuePair<DependencyObject, object>>) containersToItems).Remove(keyValuePair1);
      this.ContainersToItems = (IDictionary<DependencyObject, object>) new Dictionary<DependencyObject, object>();
      foreach (KeyValuePair<DependencyObject, object> keyValuePair2 in (IEnumerable<KeyValuePair<DependencyObject, object>>) containersToItems)
      {
        if (this.ContainersToItems.Count == num)
          this.ContainersToItems.Add(keyValuePair1);
        this.ContainersToItems.Add(keyValuePair2);
      }
      if (this.Items.Count != this.ContainersToItems.Count && !this.ContainersToItems.Contains(keyValuePair1))
        this.ContainersToItems.Add(keyValuePair1);
      this.isMenuLoaded = false;
    }
    if (item is MenuItemSeparator)
      this.UpdateSeparatorVisibility(item as MenuItemSeparator);
    base.PrepareContainerForItemOverride((DependencyObject) element1, item);
  }

  protected override void OnMouseEnter(MouseEventArgs e)
  {
    base.OnMouseEnter(e);
    this.IsItemMouseOver = true;
  }

  protected override void OnMouseLeave(MouseEventArgs e)
  {
    base.OnMouseLeave(e);
    this.IsItemMouseOver = false;
  }

  public void Dispose()
  {
    this.ItemsSource = (IEnumerable) null;
    this.OutsidePopupCanvas = (Canvas) null;
    if (this.currentOpenMenu != null)
    {
      this.currentOpenMenu.Dispose();
      this.currentOpenMenu = (MenuItemAdv) null;
    }
    if (this.MouseOverItem != null)
    {
      this.MouseOverItem.Dispose();
      this.MouseOverItem = (MenuItemAdv) null;
    }
    if (this.mainWindow != null)
    {
      this.MouseLeftButtonDown -= new MouseButtonEventHandler(this.MenuAdv_MouseLeftButtonDown);
      this.mainWindow.PreviewMouseLeftButtonUp -= new MouseButtonEventHandler(this.MainWindow_PreviewMouseLeftButtonUp);
      this.mainWindow.PreviewMouseLeftButtonDown -= new MouseButtonEventHandler(this.MainWindow_PreviewMouseLeftButtonDown);
      this.mainWindow.PreviewMouseRightButtonUp -= new MouseButtonEventHandler(this.MainWindow_PreviewMouseRightButtonUp);
      this.mainWindow.Deactivated -= new EventHandler(this.MainWindow_Deactivated);
      this.mainWindow.LocationChanged -= new EventHandler(this.MainWindow_LocationChanged);
      this.mainWindow.KeyUp -= new KeyEventHandler(this.MainWindow_KeyUp);
      this.mainWindow = (Window) null;
    }
    if (this.ContainersToItems != null)
    {
      this.ContainersToItems.Clear();
      this.ContainersToItems = (IDictionary<DependencyObject, object>) null;
    }
    this.Loaded -= new RoutedEventHandler(this.MenuAdv_Loaded);
    this.Unloaded -= new RoutedEventHandler(this.MenuAdv_Unloaded);
    GC.SuppressFinalize((object) this);
  }

  private void RootVisual_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    for (int index = 0; index < this.ContainersToItems.Count; ++index)
    {
      if (this.ItemContainerGenerator.ContainerFromIndex(index) is MenuItemAdv)
      {
        MenuAdv parentMenuAdv = ((MenuItemAdv) this.ItemContainerGenerator.ContainerFromIndex(index)).ParentMenuAdv;
      }
    }
    if (this.IsItemMouseOver)
      return;
    this.IsItemSelected = false;
    this.CloseAllPopUps();
  }

  internal StackPanel GetStackPanel()
  {
    return (StackPanel) ((Panel) null ?? VisualTreeHelper.GetParent((DependencyObject) (this.Items[0] as MenuItemAdv)) as Panel);
  }

  internal void GetMenuItem(MenuItemAdv menuItem)
  {
    if (menuItem == null || this.menuItemWidth == menuItem.ActualWidth)
      return;
    this.PanelWidth = menuItem.ActualWidth;
    menuItem.HandlePopupOpen();
    menuItem.SelectPopUpAnimation();
    this.menuItemWidth = menuItem.ActualWidth;
  }

  internal void CloseAllPopupsInternal(MenuItemAdv item)
  {
    for (int index = 0; index < item.ContainersToItems.Count; ++index)
    {
      if (item.ItemContainerGenerator.ContainerFromIndex(index) is MenuItemAdv control && control.ParentMenuItemAdv != null)
      {
        control.IsSubMenuOpen = false;
        if (control.IsEnabled)
        {
          System.Windows.VisualStateManager.GoToState((FrameworkElement) control, "Normal", true);
          control.CallVisualState(control, "Normal");
        }
        if (control.CheckBoxPanel != null)
          System.Windows.VisualStateManager.GoToState((FrameworkElement) control.CheckBoxPanel, "Normal", false);
        if (control.RadioButtonPanel != null)
          System.Windows.VisualStateManager.GoToState((FrameworkElement) control.RadioButtonPanel, "Normal", false);
        control.IsBoundaryDetected = false;
        if (control.PART_BottomScroll != null && control.PART_BottomScroll.Visibility == Visibility.Visible)
          control.PART_BottomScroll.Visibility = Visibility.Collapsed;
        if (control.PART_TopScroll != null && control.PART_TopScroll.Visibility == Visibility.Visible)
          control.PART_TopScroll.Visibility = Visibility.Collapsed;
        this.IsAltKeyPressed = false;
        if (control.Items.Count > 0)
          this.CloseAllPopupsInternal(control);
      }
    }
  }

  internal void CloseAllPopUps()
  {
    if (this.IsAllPopupClosed)
      return;
    for (int index = 0; index < this.ContainersToItems.Count; ++index)
    {
      if (this.ItemContainerGenerator.ContainerFromIndex(index) is MenuItemAdv && ((MenuItemAdv) this.ItemContainerGenerator.ContainerFromIndex(index)).ParentMenuAdv != null)
      {
        this.CloseAllPopupsInternal((MenuItemAdv) this.ItemContainerGenerator.ContainerFromIndex(index));
        ((MenuItemAdv) this.ItemContainerGenerator.ContainerFromIndex(index)).IsSubMenuOpen = false;
      }
    }
    this.IsTopLevelItem = false;
    if (this.ExpandMode == ExpandModes.ExpandOnClick)
      this.firstClick = false;
    this.IsAltKeyPressed = false;
    this.IsAllPopupClosed = true;
  }

  internal void ChangeExtendButtonVisibility()
  {
    foreach (object obj in (IEnumerable) this.Items)
    {
      if (obj is MenuItemAdv)
        ((MenuItemAdv) obj).ChangeExtendButtonVisibility();
      if (obj is MenuItemSeparator)
        this.UpdateSeparatorVisibility(obj as MenuItemSeparator);
    }
  }
}
