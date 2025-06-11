// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.MenuItemAdv
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

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
using System.Windows.Media.Animation;
using System.Windows.Threading;

#nullable disable
namespace Syncfusion.Windows.Shared;

[StyleTypedProperty(Property = "BottomScrollButtonStyle", StyleTargetType = typeof (Button))]
[Syncfusion.Windows.TemplateVisualState(Name = "SubMenuItemFocused", GroupName = "CommonStates")]
[StyleTypedProperty(Property = "TopScrollButtonStyle", StyleTargetType = typeof (Button))]
[DesignTimeVisible(false)]
[StyleTypedProperty(Property = "CheckBoxStyle", StyleTargetType = typeof (CheckBox))]
[StyleTypedProperty(Property = "RadioButtonStyle", StyleTargetType = typeof (RadioButton))]
[Syncfusion.Windows.TemplateVisualState(Name = "MenuItemSelected", GroupName = "CommonStates")]
[Syncfusion.Windows.TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
[ToolboxItem(false)]
[Syncfusion.Windows.TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
[Syncfusion.Windows.TemplateVisualState(Name = "Selected", GroupName = "CommonStates")]
[Syncfusion.Windows.TemplateVisualState(Name = "MenuItemFocused", GroupName = "CommonStates")]
public class MenuItemAdv : HeaderedItemsControl, ICommandSource
{
  internal CheckBox CheckBoxPanel;
  internal RadioButton RadioButtonPanel;
  private Popup SubMenuItemPopUp;
  private Border PopUpBorder;
  private Grid IconGrid;
  private Grid PopUpGrid;
  internal Grid menuItemAdvGrid;
  private StackPanel menuAdvStackPanel;
  private StackPanel panel;
  private ScrollViewer PART_ScrollViewer;
  internal Button PART_BottomScroll;
  internal Button PART_TopScroll;
  private TextBlock GestureTextBlock;
  private bool canGestureTextBlockVisible;
  private DispatcherTimer ScrollerTick;
  private double scrollableValue = 2.0;
  private IEnumerator<DependencyObject> items;
  private bool ScrollabilityEnabled;
  internal bool IsBoundaryDetected;
  internal bool isSubMenuItemsPopupOpenedThroughRightKey;
  internal bool isSubMenuItemsPopupOpenedThroughEnterKey;
  internal bool isSubMenuItemsPopupOpenedThroughLeftKey;
  internal bool isSubMenuItemsPopupOpenedThroughDownKey;
  internal bool canFocused = true;
  private Border MenuItemBorder;
  internal bool isActiveScope;
  internal bool IsRightLeftKeyPressed;
  private ContentControl IconContent;
  private double PanelHeight;
  private bool _isExecute;
  internal ContentControl cpresenter;
  public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (MenuItemAdv));
  public static readonly DependencyProperty RoleProperty = DependencyProperty.Register(nameof (Role), typeof (Role), typeof (MenuItemAdv), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty InputGestureTextProperty = DependencyProperty.Register(nameof (InputGestureText), typeof (string), typeof (MenuItemAdv), new PropertyMetadata((object) string.Empty));
  public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register(nameof (IsPressed), typeof (bool), typeof (MenuItemAdv), new PropertyMetadata((object) false));
  public static readonly DependencyProperty IsSubMenuOpenProperty = DependencyProperty.Register(nameof (IsSubMenuOpen), typeof (bool), typeof (MenuItemAdv), new PropertyMetadata((object) false, new PropertyChangedCallback(MenuItemAdv.OnIsSubMenuOpenChanged)));
  public static readonly DependencyProperty ExtendButtonVisibilityProperty = DependencyProperty.Register(nameof (ExtendButtonVisibility), typeof (Visibility), typeof (MenuItemAdv), new PropertyMetadata((object) Visibility.Collapsed));
  public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof (Icon), typeof (object), typeof (MenuItemAdv), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty IsCheckableProperty = DependencyProperty.Register(nameof (IsCheckable), typeof (bool), typeof (MenuItemAdv), new PropertyMetadata((object) false, new PropertyChangedCallback(MenuItemAdv.OnIsCheckablePropertyChanged)));
  public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof (IsChecked), typeof (bool), typeof (MenuItemAdv), new PropertyMetadata((object) false, new PropertyChangedCallback(MenuItemAdv.OnIsCheckedPropertyChanged)));
  internal static readonly DependencyProperty CheckIconTypeProperty = DependencyProperty.Register(nameof (CheckIconType), typeof (CheckIconType), typeof (MenuItemAdv), new PropertyMetadata((object) CheckIconType.CheckBox, new PropertyChangedCallback(MenuItemAdv.OnCheckIconTypeChanged)));
  public static readonly DependencyProperty CheckBoxVisibilityProperty = DependencyProperty.Register(nameof (CheckBoxVisibility), typeof (Visibility), typeof (MenuItemAdv), new PropertyMetadata((object) Visibility.Collapsed));
  public static readonly DependencyProperty RadioButtonVisibilityProperty = DependencyProperty.Register(nameof (RadioButtonVisibility), typeof (Visibility), typeof (MenuItemAdv), new PropertyMetadata((object) Visibility.Collapsed));
  public static readonly DependencyProperty GroupNameProperty = DependencyProperty.Register(nameof (GroupName), typeof (string), typeof (MenuItemAdv), new PropertyMetadata((object) string.Empty));
  public static readonly DependencyProperty ScrollerHeightProperty = DependencyProperty.Register(nameof (ScrollerHeight), typeof (double), typeof (MenuItemAdv), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty StaysOpenOnClickProperty = DependencyProperty.Register(nameof (StaysOpenOnClick), typeof (bool), typeof (MenuItemAdv), new PropertyMetadata((object) false));
  public static readonly DependencyProperty TopScrollButtonStyleProperty = DependencyProperty.Register(nameof (TopScrollButtonStyle), typeof (Style), typeof (MenuItemAdv), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty BottomScrollButtonStyleProperty = DependencyProperty.Register(nameof (BottomScrollButtonStyle), typeof (Style), typeof (MenuItemAdv), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty CheckBoxStyleProperty = DependencyProperty.Register(nameof (CheckBoxStyle), typeof (Style), typeof (MenuItemAdv), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty RadioButtonStyleProperty = DependencyProperty.Register(nameof (RadioButtonStyle), typeof (Style), typeof (MenuItemAdv), new PropertyMetadata((PropertyChangedCallback) null));
  internal static readonly DependencyProperty PanelWidthProperty = DependencyProperty.Register(nameof (PanelWidth), typeof (double), typeof (MenuItemAdv), new PropertyMetadata(new PropertyChangedCallback(MenuItemAdv.OnPanelWidthChanged)));
  internal bool mouseEnterOnPopup;
  public static readonly DependencyProperty CommandProperty = ButtonBase.CommandProperty.AddOwner(typeof (MenuItemAdv), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, new PropertyChangedCallback(MenuItemAdv.CommandChanged)));
  public static readonly DependencyProperty CommandParameterProperty = ButtonBase.CommandParameterProperty.AddOwner(typeof (MenuItemAdv), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty CommandTargetProperty = ButtonBase.CommandTargetProperty.AddOwner(typeof (MenuItemAdv), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));
  private EventHandler CanExecuteChangedHandler;

  public MenuItemAdv()
  {
    this.DefaultStyleKey = (object) typeof (MenuItemAdv);
    this.ContainersToItems = (IDictionary<DependencyObject, object>) new Dictionary<DependencyObject, object>();
    this.Loaded -= new RoutedEventHandler(this.MenuItemAdv_Loaded);
    this.Unloaded -= new RoutedEventHandler(this.MenuItemAdv_Unloaded);
    this.IsEnabledChanged -= new DependencyPropertyChangedEventHandler(this.MenuItemAdv_IsEnabledChanged);
    this.Loaded += new RoutedEventHandler(this.MenuItemAdv_Loaded);
    this.Unloaded += new RoutedEventHandler(this.MenuItemAdv_Unloaded);
    this.IsEnabledChanged += new DependencyPropertyChangedEventHandler(this.MenuItemAdv_IsEnabledChanged);
  }

  private void MenuItemAdv_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
  {
    if (sender is MenuItemAdv menuItemAdv && menuItemAdv.IsEnabled)
    {
      menuItemAdv.IsSubMenuOpen = false;
      this.CallVisualState(menuItemAdv, "Normal");
    }
    else
    {
      if (menuItemAdv == null || menuItemAdv.IsEnabled)
        return;
      Syncfusion.Windows.VisualStateManager.GoToState((Control) menuItemAdv, "Disabled", false);
    }
  }

  private void MenuItemAdv_Unloaded(object sender, RoutedEventArgs e)
  {
    this.isActiveScope = false;
    if (this.panel != null)
      this.panel.LayoutUpdated -= new EventHandler(this.panel_LayoutUpdated);
    if (this.CheckBoxPanel != null)
    {
      this.CheckBoxPanel.MouseLeave -= new MouseEventHandler(this.CheckBoxPanel_MouseLeave);
      this.CheckBoxPanel.Click -= new RoutedEventHandler(this.CheckBoxPanel_Click);
    }
    if (this.RadioButtonPanel != null)
    {
      this.RadioButtonPanel.MouseLeave -= new MouseEventHandler(this.RadioButtonPanel_MouseLeave);
      this.RadioButtonPanel.Click -= new RoutedEventHandler(this.RadioButtonPanel_Click);
    }
    if (this.PART_TopScroll != null)
    {
      this.PART_TopScroll.MouseEnter -= new MouseEventHandler(this.PART_TopScroll_MouseEnter);
      this.PART_TopScroll.MouseLeave -= new MouseEventHandler(this.PART_TopScroll_MouseLeave);
    }
    if (this.PART_BottomScroll != null)
    {
      this.PART_BottomScroll.MouseEnter -= new MouseEventHandler(this.PART_BottomScroll_MouseEnter);
      this.PART_BottomScroll.MouseLeave -= new MouseEventHandler(this.PART_BottomScroll_MouseLeave);
    }
    if (this.SubMenuItemPopUp != null)
      this.SubMenuItemPopUp.LayoutUpdated -= new EventHandler(this.SubMenuItemPopUp_LayoutUpdated);
    if (this.PopUpClosing != null)
      this.PopUpClosing -= new RoutedEventHandler(this.MenuItemAdv_PopUpClosing);
    if (this.MenuItemBorder != null)
      this.MenuItemBorder.MouseEnter -= new MouseEventHandler(this.menuItemAdvGrid_MouseEnter);
    if (this.ScrollerTick == null)
      return;
    this.ScrollerTick.Stop();
  }

  protected override void OnLostFocus(RoutedEventArgs e)
  {
    if (!this.IsSubMenuOpen)
      this.CallVisualState(this, "Normal");
    base.OnLostFocus(e);
  }

  public event RoutedEventHandler PopUpClosed;

  public event RoutedEventHandler PopUpClosing;

  public event RoutedEventHandler PopUpOpened;

  public event RoutedEventHandler PopUpOpening;

  public event RoutedEventHandler Click
  {
    add => this.AddHandler(MenuItemAdv.ClickEvent, (Delegate) value);
    remove => this.RemoveHandler(MenuItemAdv.ClickEvent, (Delegate) value);
  }

  public event RoutedEventHandler Checked;

  public event RoutedEventHandler UnChecked;

  internal IDictionary<DependencyObject, object> ContainersToItems { get; set; }

  internal MenuAdv ParentMenuAdv { get; set; }

  internal MenuItemAdv ParentMenuItemAdv { get; set; }

  internal MenuAdv Parent { get; set; }

  internal MenuAdv ParentMenu { get; set; }

  public Role Role
  {
    get => (Role) this.GetValue(MenuItemAdv.RoleProperty);
    private set => this.SetValue(MenuItemAdv.RoleProperty, (object) value);
  }

  [Category("Common properties")]
  [Description("Represents the SubMenu popup can be open")]
  public bool IsSubMenuOpen
  {
    get => (bool) this.GetValue(MenuItemAdv.IsSubMenuOpenProperty);
    set => this.SetValue(MenuItemAdv.IsSubMenuOpenProperty, (object) value);
  }

  [Description("Represents the object which is to be placed as Icon.")]
  [Category("Appearance")]
  public object Icon
  {
    get => this.GetValue(MenuItemAdv.IconProperty);
    set => this.SetValue(MenuItemAdv.IconProperty, value);
  }

  [Description("Represents the MenuItem can be checkable or not")]
  [Category("Appearance")]
  public bool IsCheckable
  {
    get => (bool) this.GetValue(MenuItemAdv.IsCheckableProperty);
    set => this.SetValue(MenuItemAdv.IsCheckableProperty, (object) value);
  }

  public bool IsPressed
  {
    get => (bool) this.GetValue(MenuItemAdv.IsPressedProperty);
    set => this.SetValue(MenuItemAdv.IsPressedProperty, (object) value);
  }

  [Category("Appearance")]
  [Description("Represents the MenuItem is Checked or not")]
  public bool IsChecked
  {
    get => (bool) this.GetValue(MenuItemAdv.IsCheckedProperty);
    set => this.SetValue(MenuItemAdv.IsCheckedProperty, (object) value);
  }

  [Description("Represents the CheckIconType of the MenuItem, Which may be a Check Box or Radio Button")]
  [Category("Appearance")]
  public CheckIconType CheckIconType
  {
    get => (CheckIconType) this.GetValue(MenuItemAdv.CheckIconTypeProperty);
    set => this.SetValue(MenuItemAdv.CheckIconTypeProperty, (object) value);
  }

  [Description("Represents the Group Name of Menu Items which are to be used as Radio Buttons")]
  [Category("Common Properties")]
  public string GroupName
  {
    get => (string) this.GetValue(MenuItemAdv.GroupNameProperty);
    set => this.SetValue(MenuItemAdv.GroupNameProperty, (object) value);
  }

  [Description("Represents the Command Paramenter which has to be passed with the Command to execute it.")]
  [Category("Common Properties")]
  public string InputGestureText
  {
    get => (string) this.GetValue(MenuItemAdv.InputGestureTextProperty);
    set => this.SetValue(MenuItemAdv.InputGestureTextProperty, (object) value);
  }

  [Category("Appearance")]
  [Description("Represents the style of the Scroll Button present in the Top of the SubMenu popup")]
  public Style TopScrollButtonStyle
  {
    get => (Style) this.GetValue(MenuItemAdv.TopScrollButtonStyleProperty);
    set => this.SetValue(MenuItemAdv.TopScrollButtonStyleProperty, (object) value);
  }

  [Category("Appearance")]
  [Description("Represents the style of the Scroll Button present in the Bottom of the SubMenu popup")]
  public Style BottomScrollButtonStyle
  {
    get => (Style) this.GetValue(MenuItemAdv.BottomScrollButtonStyleProperty);
    set => this.SetValue(MenuItemAdv.BottomScrollButtonStyleProperty, (object) value);
  }

  [Category("Appearance")]
  [Description("Represents the style of the Check Box present in the MenuItem Icon")]
  public Style CheckBoxStyle
  {
    get => (Style) this.GetValue(MenuItemAdv.CheckBoxStyleProperty);
    set => this.SetValue(MenuItemAdv.CheckBoxStyleProperty, (object) value);
  }

  [Description("Represents the style of the Radio Button present in the MenuItem Icon")]
  [Category("Appearance")]
  public Style RadioButtonStyle
  {
    get => (Style) this.GetValue(MenuItemAdv.RadioButtonStyleProperty);
    set => this.SetValue(MenuItemAdv.RadioButtonStyleProperty, (object) value);
  }

  [Category("Common Properties")]
  [Description("Represents the SubMenu popup can be closed while selecting a MenuItem")]
  public bool StaysOpenOnClick
  {
    get => (bool) this.GetValue(MenuItemAdv.StaysOpenOnClickProperty);
    set => this.SetValue(MenuItemAdv.StaysOpenOnClickProperty, (object) value);
  }

  internal double PanelWidth
  {
    get => (double) this.GetValue(MenuItemAdv.PanelWidthProperty);
    set => this.SetValue(MenuItemAdv.PanelWidthProperty, (object) value);
  }

  public Visibility ExtendButtonVisibility
  {
    get => (Visibility) this.GetValue(MenuItemAdv.ExtendButtonVisibilityProperty);
    set => this.SetValue(MenuItemAdv.ExtendButtonVisibilityProperty, (object) value);
  }

  public Visibility CheckBoxVisibility
  {
    get => (Visibility) this.GetValue(MenuItemAdv.CheckBoxVisibilityProperty);
    set => this.SetValue(MenuItemAdv.CheckBoxVisibilityProperty, (object) value);
  }

  public Visibility RadioButtonVisibility
  {
    get => (Visibility) this.GetValue(MenuItemAdv.RadioButtonVisibilityProperty);
    set => this.SetValue(MenuItemAdv.RadioButtonVisibilityProperty, (object) value);
  }

  public double ScrollerHeight
  {
    get => (double) this.GetValue(MenuItemAdv.ScrollerHeightProperty);
    set => this.SetValue(MenuItemAdv.ScrollerHeightProperty, (object) value);
  }

  private static void OnIsCheckedPropertyChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    if (!((MenuItemAdv) obj).IsCheckable)
      return;
    ((MenuItemAdv) obj).OnIsCheckedPropertyChanged(args);
  }

  internal static void OnPanelWidthChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((MenuItemAdv) obj)?.OnPanelWidthChanged(args);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.ContainersToItems.Clear();
    this.GetTemplateChildrens();
    this.RegisterEvents();
    if (this.ParentMenuAdv != null && this.ParentMenuAdv.isInitialOrientation)
    {
      this.menuAdvStackPanel = this.GetStackPanel();
      if (this.menuAdvStackPanel != null)
        this.menuAdvStackPanel.SetBinding(StackPanel.OrientationProperty, (BindingBase) new Binding()
        {
          Source = (object) this.ParentMenuAdv.Orientation,
          Mode = BindingMode.OneWay
        });
      this.ParentMenuAdv.isInitialOrientation = false;
    }
    if (this.panel != null)
      this.panel.LayoutUpdated -= new EventHandler(this.panel_LayoutUpdated);
    this.panel = this.GetStackPanel();
    if (this.panel != null)
      this.panel.LayoutUpdated += new EventHandler(this.panel_LayoutUpdated);
    if (this.Command == null || !(this.Command is RoutedUICommand))
      return;
    if (this.Header == null)
      this.Header = (object) (this.Command as RoutedUICommand).Text;
    if (!(this.InputGestureText == string.Empty) || (this.Command as RoutedUICommand).InputGestures.Count <= 0)
      return;
    this.InputGestureText = ((this.Command as RoutedUICommand).InputGestures[0] as KeyGesture).DisplayString;
  }

  private void GetTemplateChildrens()
  {
    if (this.IconContent != null)
    {
      this.IconContent.Content = (object) null;
      this.IconContent = (ContentControl) null;
    }
    if (this.CheckBoxPanel != null)
      this.CheckBoxPanel.MouseLeave -= new MouseEventHandler(this.CheckBoxPanel_MouseLeave);
    if (this.RadioButtonPanel != null)
      this.RadioButtonPanel.MouseLeave -= new MouseEventHandler(this.RadioButtonPanel_MouseLeave);
    if (this.PART_TopScroll != null)
    {
      this.PART_TopScroll.MouseEnter -= new MouseEventHandler(this.PART_TopScroll_MouseEnter);
      this.PART_TopScroll.MouseLeave -= new MouseEventHandler(this.PART_TopScroll_MouseLeave);
    }
    if (this.PART_BottomScroll != null)
    {
      this.PART_BottomScroll.MouseEnter -= new MouseEventHandler(this.PART_BottomScroll_MouseEnter);
      this.PART_BottomScroll.MouseLeave -= new MouseEventHandler(this.PART_BottomScroll_MouseLeave);
    }
    if (this.SubMenuItemPopUp != null)
    {
      this.SubMenuItemPopUp.LayoutUpdated -= new EventHandler(this.SubMenuItemPopUp_LayoutUpdated);
      this.PopUpClosing -= new RoutedEventHandler(this.MenuItemAdv_PopUpClosing);
    }
    this.IconContent = this.GetTemplateChild("IconContent") as ContentControl;
    if (this.IconContent != null)
      this.IconContent.Content = this.Icon;
    this.CheckBoxPanel = this.GetTemplateChild("CheckBoxPanel") as CheckBox;
    this.RadioButtonPanel = this.GetTemplateChild("RadioButtonPanel") as RadioButton;
    this.menuItemAdvGrid = this.GetTemplateChild("menuItemAdvGrid") as Grid;
    this.SubMenuItemPopUp = this.GetTemplateChild("SubMenuPopup") as Popup;
    if (this.SubMenuItemPopUp != null)
    {
      this.SubMenuItemPopUp.MouseEnter += new MouseEventHandler(this.SubMenuItemPopUp_MouseEnter);
      this.SubMenuItemPopUp.MouseLeave += new MouseEventHandler(this.SubMenuItemPopUp_MouseLeave);
    }
    this.IconGrid = this.GetTemplateChild("IconGrid") as Grid;
    this.PART_TopScroll = this.GetTemplateChild("PART_TopScroll") as Button;
    this.PART_BottomScroll = this.GetTemplateChild("PART_BottomScroll") as Button;
    this.PART_ScrollViewer = this.GetTemplateChild("PART_ScrollViewer") as ScrollViewer;
    this.PopUpGrid = this.GetTemplateChild("PopUpGrid") as Grid;
    this.PopUpBorder = this.GetTemplateChild("PopUpBorder") as Border;
    this.GestureTextBlock = this.GetTemplateChild("GestureTextBlock") as TextBlock;
    this.MenuItemBorder = this.GetTemplateChild("MenuItemBorder") as Border;
    EventManager.RegisterClassHandler(typeof (MenuItemAdv), AccessKeyManager.AccessKeyPressedEvent, (Delegate) new AccessKeyPressedEventHandler(MenuItemAdv.OnAccessKeyPressed));
  }

  private void SubMenuItemPopUp_MouseLeave(object sender, MouseEventArgs e)
  {
    if (this.Parent != null && this.Parent.MouseOverItem != null && this.ContainersToItems.ContainsKey((DependencyObject) this.Parent.MouseOverItem))
    {
      this.Parent.isMouseEnterpopup = true;
      this.MenuItemCanExecute(this);
    }
    this.mouseEnterOnPopup = false;
  }

  private void MenuItemCanExecute(MenuItemAdv menuitem)
  {
    foreach (object obj in (IEnumerable) menuitem.Items)
    {
      MenuItemAdv menuItemAdv = !(obj is MenuItemAdv) ? menuitem.ItemContainerGenerator.ContainerFromItem(obj) as MenuItemAdv : obj as MenuItemAdv;
      if (menuItemAdv != null && menuItemAdv.Command != null)
        menuItemAdv.UpdateCanExecute();
    }
  }

  private void SubMenuItemPopUp_MouseEnter(object sender, MouseEventArgs e)
  {
    this.mouseEnterOnPopup = true;
  }

  private static void OnAccessKeyPressed(object sender, AccessKeyPressedEventArgs e)
  {
    MenuItemAdv menuItemAdv = sender as MenuItemAdv;
    bool flag = false;
    if (menuItemAdv.Parent == null || menuItemAdv.Parent == null || !menuItemAdv.Parent.IsAltKeyPressed || !menuItemAdv.Parent.IsEnabled)
      return;
    if (e.Target == null)
    {
      switch (Mouse.Captured)
      {
        case null:
        case MenuAdv _:
          e.Target = (UIElement) menuItemAdv;
          if (e.OriginalSource == menuItemAdv && menuItemAdv.IsSubMenuOpen)
          {
            flag = true;
            break;
          }
          break;
        default:
          e.Handled = true;
          break;
      }
    }
    else if (e.Scope == null)
    {
      if (e.Target != menuItemAdv && e.Target is MenuItemAdv)
      {
        flag = true;
      }
      else
      {
        for (DependencyObject reference = e.Source as DependencyObject; reference != null && reference != menuItemAdv; reference = VisualTreeHelper.GetParent(reference))
        {
          if (reference is UIElement container && ItemsControl.ItemsControlFromItemContainer((DependencyObject) container) == menuItemAdv)
          {
            flag = true;
            break;
          }
        }
      }
    }
    e.Handled = true;
    if (!flag)
      return;
    e.Scope = (object) menuItemAdv;
    e.Handled = true;
  }

  protected override void OnAccessKey(AccessKeyEventArgs e)
  {
    base.OnAccessKey(e);
    if (!e.IsMultiple)
    {
      if (this.Role.Equals((object) Role.TopLevelHeader) && this.Parent.IsTopLevelItem)
        this.Parent.CloseAllPopUps();
      this.EnterKeyAction();
      if (this.Role.Equals((object) Role.TopLevelHeader))
        this.Parent.IsTopLevelItem = true;
      else
        this.Parent.IsTopLevelItem = false;
    }
    else if (this.ParentMenuAdv != null)
    {
      this.CallVisualState(this, "MenuItemSelected");
    }
    else
    {
      if (this.ParentMenuItemAdv == null)
        return;
      this.CallVisualState(this, "SubMenuItemFocused");
    }
  }

  private void RegisterEvents()
  {
    if (this.CheckBoxPanel != null)
    {
      this.CheckBoxPanel.MouseLeave += new MouseEventHandler(this.CheckBoxPanel_MouseLeave);
      this.CheckBoxPanel.Click += new RoutedEventHandler(this.CheckBoxPanel_Click);
    }
    if (this.RadioButtonPanel != null)
    {
      this.RadioButtonPanel.MouseLeave += new MouseEventHandler(this.RadioButtonPanel_MouseLeave);
      this.RadioButtonPanel.Click += new RoutedEventHandler(this.RadioButtonPanel_Click);
    }
    this.ScrollerHeight = 0.0;
    if (this.PART_TopScroll != null)
    {
      this.ScrollerTick = new DispatcherTimer();
      this.ScrollerTick.Tick += new EventHandler(this.ScrollUp_Tick);
      this.PART_TopScroll.MouseEnter += new MouseEventHandler(this.PART_TopScroll_MouseEnter);
      this.PART_TopScroll.MouseLeave += new MouseEventHandler(this.PART_TopScroll_MouseLeave);
    }
    if (this.PART_BottomScroll != null)
    {
      this.PART_BottomScroll.MouseEnter += new MouseEventHandler(this.PART_BottomScroll_MouseEnter);
      this.PART_BottomScroll.MouseLeave += new MouseEventHandler(this.PART_BottomScroll_MouseLeave);
    }
    if (this.SubMenuItemPopUp == null)
      return;
    this.SubMenuItemPopUp.LayoutUpdated += new EventHandler(this.SubMenuItemPopUp_LayoutUpdated);
    this.PopUpClosing += new RoutedEventHandler(this.MenuItemAdv_PopUpClosing);
  }

  private void RadioButtonPanel_Click(object sender, RoutedEventArgs e)
  {
    if (this.StaysOpenOnClick || this.Items.Count > 0)
      return;
    this.Parent.CloseAllPopUps();
  }

  private void CheckBoxPanel_Click(object sender, RoutedEventArgs e)
  {
    if (!this.StaysOpenOnClick && this.Items.Count <= 0)
    {
      this.IsChecked = false;
      this.Parent.CloseAllPopUps();
    }
    else
    {
      if (!this.StaysOpenOnClick)
        return;
      this.CheckBoxPanel.IsChecked = new bool?(false);
    }
  }

  protected override bool IsItemItsOwnContainerOverride(object item)
  {
    return item is MenuItemAdv || item is MenuItemSeparator;
  }

  protected override DependencyObject GetContainerForItemOverride()
  {
    return (DependencyObject) new MenuItemAdv();
  }

  protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
  {
    if (element is MenuItemAdv menuItemAdv)
    {
      menuItemAdv.ParentMenuAdv = (MenuAdv) null;
      menuItemAdv.ParentMenuItemAdv = this;
      menuItemAdv.Parent = this.Parent;
      menuItemAdv.ParentMenu = this.ParentMenu;
    }
    this.ContainersToItems[element] = item;
    this.CheckGroupName(menuItemAdv);
    base.PrepareContainerForItemOverride((DependencyObject) menuItemAdv, item);
  }

  private void menuItemAdvGrid_MouseEnter(object sender, MouseEventArgs e)
  {
    if (this.Parent != null)
      this.Parent.IsItemMouseOver = true;
    if (this.ParentMenuAdv != null)
    {
      foreach (MenuItemAdv key in (IEnumerable<DependencyObject>) this.ParentMenuAdv.ContainersToItems.Keys)
      {
        if (key != this && key.IsEnabled)
        {
          key.IsSubMenuOpen = false;
          this.CallVisualState(key, "Normal");
        }
      }
      if ((this.ParentMenuAdv.ExpandMode == ExpandModes.ExpandOnMouseOver || this.ParentMenuAdv.firstClick) && this.Items.Count > 0 && this.ParentMenuAdv.mainWindow != null && this.ParentMenuAdv.mainWindow.IsActive)
        this.IsSubMenuOpen = true;
      if (this.IsSubMenuOpen && this.Parent.Orientation == Orientation.Horizontal)
        this.CallVisualState(this, "MenuItemSelected");
      else
        this.CallVisualState(this, "MenuItemFocused");
      if (this.ParentMenuAdv != null && this.ParentMenuAdv.mainWindow != null && this.ParentMenuAdv.mainWindow.IsActive)
        this.Focus();
    }
    else if (this.ParentMenuItemAdv != null)
    {
      foreach (DependencyObject key in (IEnumerable<DependencyObject>) this.ParentMenuItemAdv.ContainersToItems.Keys)
      {
        if (key is MenuItemAdv && key != this && (key as MenuItemAdv).IsEnabled)
        {
          (key as MenuItemAdv).IsSubMenuOpen = false;
          this.CallVisualState(key as MenuItemAdv, "Normal");
        }
        this.CallVisualState(this, "SubMenuItemFocused");
        if (!this.IsSubMenuOpen || this.Parent == null || !this.Parent.isMouseEnterpopup)
          this.Focus();
      }
      if (this.Items.Count > 0 && this.Parent != null)
      {
        this.IsSubMenuOpen = true;
        this.Parent.isMouseEnterpopup = false;
      }
      if (this.Parent == null && this.ParentMenuItemAdv != null)
        this.Parent = this.ParentMenuItemAdv.Parent;
      if (this.Parent != null)
      {
        this.Parent.IsExecuteonLostFocus = false;
        this.Parent.MouseOverItem = this;
      }
      if (this.ContainersToItems.Keys.Count > 0)
      {
        foreach (DependencyObject key in (IEnumerable<DependencyObject>) this.ContainersToItems.Keys)
        {
          if (key is MenuItemAdv && (key as MenuItemAdv).IsEnabled)
          {
            (key as MenuItemAdv).IsSubMenuOpen = false;
            this.CallVisualState(key as MenuItemAdv, "Normal");
          }
        }
      }
    }
    if (this.ParentMenuItemAdv == null || this._isExecute)
      return;
    bool flag = false;
    foreach (object obj in (IEnumerable) this.ParentMenuItemAdv.Items)
    {
      MenuItemAdv menuItemAdv = !(obj is MenuItemAdv) ? this.ParentMenuItemAdv.ItemContainerGenerator.ContainerFromItem(obj) as MenuItemAdv : obj as MenuItemAdv;
      if (menuItemAdv != null && !menuItemAdv.IsEnabled)
      {
        flag = true;
        break;
      }
    }
    if ((!flag || this._isExecute) && (this.Parent == null || !this.Parent.isMouseEnterpopup || this.IsSubMenuOpen) || this.ParentMenuItemAdv == null)
      return;
    this.MenuItemCanExecute(this.ParentMenuItemAdv);
    this._isExecute = true;
    if (this.Parent == null)
      return;
    this.Parent.isMouseEnterpopup = false;
  }

  protected override void OnMouseLeave(MouseEventArgs e)
  {
    this._isExecute = false;
    base.OnMouseLeave(e);
    if (this.ParentMenuItemAdv != null)
    {
      this.Parent.IsItemMouseOver = false;
      if (this.ParentMenuAdv != null)
        this.ParentMenuAdv.isMouseOver = false;
    }
    if (this.ParentMenuAdv != null && !this.ParentMenuAdv.firstClick && this.ParentMenuAdv.ExpandMode == ExpandModes.ExpandOnClick && this.IsEnabled)
    {
      this.CallVisualState(this, "Normal");
      this.Parent.IsAltKeyPressed = false;
    }
    if (this.IsSubMenuOpen)
      return;
    this.CallVisualState(this, "Normal");
  }

  internal IEnumerable<T> GetChildMenu<T>(DependencyObject depObj) where T : DependencyObject
  {
    if (depObj != null)
    {
      for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); ++i)
      {
        DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
        if (child != null && child is T)
          yield return (T) child;
        foreach (T childOfChild in this.GetChildMenu<T>(child))
          yield return childOfChild;
      }
    }
  }

  protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    base.OnPreviewMouseLeftButtonUp(e);
    if (this.ParentMenuAdv != null && this.ParentMenuItemAdv != null && this.ParentMenuAdv.ExpandMode == ExpandModes.ExpandOnClick)
    {
      foreach (MenuItemAdv key in (IEnumerable<DependencyObject>) this.ParentMenuAdv.ContainersToItems.Keys)
      {
        if (key.IsEnabled)
        {
          key.IsSubMenuOpen = false;
          this.CallVisualState(key, "Normal");
        }
      }
    }
    if (this.ParentMenuItemAdv != null)
    {
      if (this.Parent != null && this.Items.Count <= 0 && !this.StaysOpenOnClick)
      {
        this.Parent.CloseAllPopUps();
        this.IsSubMenuOpen = false;
        this.Parent.IsItemSelected = false;
        this.CallVisualState(this.ParentMenuItemAdv, "Normal");
      }
    }
    else
    {
      if (this.ParentMenuAdv != null)
      {
        this.ParentMenuAdv.IsItemSelected = true;
        foreach (MenuItemAdv key in (IEnumerable<DependencyObject>) this.ParentMenuAdv.ContainersToItems.Keys)
        {
          if (key != this && key.IsEnabled)
          {
            key.IsSubMenuOpen = false;
            this.CallVisualState(key, "Normal");
          }
          if (!key.IsEnabled)
            Syncfusion.Windows.VisualStateManager.GoToState((Control) key, "Disabled", false);
        }
      }
      if (this.IsSubMenuOpen && this.Parent.Orientation == Orientation.Horizontal)
        this.CallVisualState(this, "MenuItemSelected");
      else
        this.CallVisualState(this, "MenuItemFocused");
    }
    if (this.Role == Role.SubmenuHeader || this.Role == Role.SubmenuItem)
      this.IsPressed = false;
    if (this.Command != null)
    {
      if (this.Command is RoutedCommand command)
      {
        if (this.CommandTarget != null)
          command.Execute(this.CommandParameter, this.CommandTarget);
        else
          command.Execute(this.CommandParameter, (IInputElement) this);
      }
      else
        this.Command.Execute(this.CommandParameter);
    }
    if (MenuItemAdv.ClickEvent != null)
      this.RaiseEvent(new RoutedEventArgs(MenuItemAdv.ClickEvent));
    if (!this.IsCheckable || this.CheckIconType == CheckIconType.None)
      return;
    if (this.CheckIconType == CheckIconType.CheckBox)
      this.IsChecked = !this.IsChecked;
    else if (this.GroupName.Equals(string.Empty))
    {
      this.IsChecked = true;
    }
    else
    {
      this.IsChecked = true;
      this.UnCheck(this);
    }
  }

  protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    base.OnPreviewMouseLeftButtonDown(e);
    Window window = Window.GetWindow((DependencyObject) this);
    if (window != null)
    {
      foreach (MenuAdv menuAdv in this.GetChildMenu<MenuAdv>((DependencyObject) window))
      {
        if (menuAdv != null && menuAdv != this.ParentMenu)
          menuAdv.CloseAllPopUps();
      }
    }
    if (this.Role == Role.SubmenuHeader || this.Role == Role.SubmenuItem)
      this.IsPressed = true;
    if (this.ParentMenuAdv != null)
    {
      if (this.ParentMenuAdv.ExpandMode != ExpandModes.ExpandOnClick)
        return;
      if (this.Items.Count > 0 && !this.ParentMenuAdv.firstClick)
      {
        this.IsSubMenuOpen = true;
        this.ParentMenuAdv.firstClick = true;
      }
      else if (e.Source is MenuItemAdv)
      {
        if (!(e.Source is MenuItemAdv source) || source.ParentMenuItemAdv != null || this.mouseEnterOnPopup)
          return;
        this.ParentMenuAdv.firstClick = false;
        source.IsSubMenuOpen = false;
        this.CallVisualState(source, "Normal");
      }
      else
      {
        if (this.mouseEnterOnPopup || !(VisualUtils.FindAncestor((Visual) e.Source, typeof (MenuItemAdv)) is MenuItemAdv ancestor) || ancestor.ParentMenuItemAdv != null)
          return;
        this.ParentMenuAdv.firstClick = false;
        ancestor.IsSubMenuOpen = false;
        this.CallVisualState(ancestor, "Normal");
      }
    }
    else
    {
      if (this.ParentMenuItemAdv == null)
        return;
      foreach (DependencyObject key in (IEnumerable<DependencyObject>) this.ParentMenuItemAdv.ContainersToItems.Keys)
      {
        if (key is MenuItemAdv && key != this && (key as MenuItemAdv).IsEnabled)
        {
          (key as MenuItemAdv).IsSubMenuOpen = false;
          this.CallVisualState(key as MenuItemAdv, "Normal");
        }
        this.CallVisualState(this, "SubMenuItemFocused");
        this.Focus();
      }
      if (this.Items.Count <= 0 || this.Parent == null || this.Parent.ExpandMode != ExpandModes.ExpandOnClick)
        return;
      this.IsSubMenuOpen = true;
    }
  }

  protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    this.Focus();
    base.OnMouseLeftButtonUp(e);
  }

  protected override void OnPreviewKeyUp(KeyEventArgs e)
  {
    this.ParentMenu.accesscount = 0;
    if (this.ParentMenuAdv != null)
    {
      int num = this.ParentMenuAdv.ItemContainerGenerator.IndexFromContainer((DependencyObject) this);
      if (num == 0 || num == this.ParentMenuAdv.Items.Count - 1)
        this.CallVisualState(this, "MenuItemSelected");
    }
    base.OnPreviewKeyUp(e);
  }

  protected override void OnKeyUp(KeyEventArgs e)
  {
    this.ParentMenu.accesscount = 0;
    base.OnKeyUp(e);
  }

  protected override void OnPreviewKeyDown(KeyEventArgs e)
  {
    this.ParentMenu.accesscount = 0;
    base.OnPreviewKeyDown(e);
  }

  protected override void OnKeyDown(KeyEventArgs e)
  {
    switch (e.Key)
    {
      case Key.Tab:
        e.Handled = true;
        bool flag1 = false;
        if (this.ParentMenuItemAdv != null)
        {
          if (Keyboard.Modifiers == ModifierKeys.Shift)
          {
            if (this.ParentMenuAdv != null && this.ParentMenuAdv.Orientation == Orientation.Vertical)
              this.LeftKeyAction();
            else
              this.UpKeyAction();
          }
          else if (this.ParentMenuAdv != null && this.ParentMenuAdv.Orientation == Orientation.Vertical)
            this.RightKeyAction();
          else
            this.DownKeyAction();
          this.Parent.IsAltKeyPressed = true;
          break;
        }
        if (this.ParentMenuAdv != null)
        {
          if (KeyboardNavigation.GetTabNavigation((DependencyObject) this.ParentMenuAdv) == KeyboardNavigationMode.Continue || KeyboardNavigation.GetTabNavigation((DependencyObject) this.ParentMenuAdv) == KeyboardNavigationMode.Contained)
          {
            int num = this.ParentMenu.ItemContainerGenerator.IndexFromContainer((DependencyObject) this);
            if (Keyboard.Modifiers != ModifierKeys.Shift && num == this.ParentMenuAdv.Items.Count - 1 || Keyboard.Modifiers == ModifierKeys.Shift && num == 0)
            {
              flag1 = true;
              this.IsSubMenuOpen = false;
              this.CallVisualState(this, "Normal");
              e.Handled = false;
              this.ParentMenuAdv.firstClick = false;
            }
            else if (Keyboard.Modifiers == ModifierKeys.Shift && num > 0)
            {
              int index = num - 1;
              bool flag2 = false;
              for (; index >= 0; --index)
              {
                DependencyObject dependencyObject = this.ParentMenu.ItemContainerGenerator.ContainerFromIndex(index);
                if (dependencyObject is MenuItemAdv && (dependencyObject as MenuItemAdv).Visibility == Visibility.Visible && (dependencyObject as MenuItemAdv).IsEnabled)
                {
                  flag2 = true;
                  break;
                }
              }
              if (!flag2)
              {
                flag1 = true;
                this.IsSubMenuOpen = false;
                this.CallVisualState(this, "Normal");
                e.Handled = false;
                this.ParentMenuAdv.firstClick = false;
              }
            }
          }
          if (!flag1)
          {
            if (Keyboard.Modifiers != ModifierKeys.Shift && this.ParentMenuAdv != null)
              this.RightKeyAction();
            else
              this.LeftKeyAction();
            this.Parent.IsAltKeyPressed = true;
            break;
          }
          break;
        }
        break;
      case Key.Return:
        e.Handled = true;
        this.EnterKeyAction();
        break;
      case Key.Escape:
        e.Handled = true;
        this.EscapeKeyAction();
        break;
      case Key.Left:
        e.Handled = true;
        if (this.ParentMenuAdv != null && this.ParentMenuAdv.Orientation == Orientation.Vertical)
        {
          this.UpKeyAction();
        }
        else
        {
          this.IsRightLeftKeyPressed = true;
          this.LeftKeyAction();
        }
        this.Parent.IsAltKeyPressed = true;
        break;
      case Key.Up:
        e.Handled = true;
        if (this.ParentMenuAdv != null && this.ParentMenuAdv.Orientation == Orientation.Vertical)
          this.LeftKeyAction();
        else
          this.UpKeyAction();
        this.Parent.IsAltKeyPressed = true;
        break;
      case Key.Right:
        e.Handled = true;
        if (this.ParentMenuAdv != null && this.ParentMenuAdv.Orientation == Orientation.Vertical)
        {
          this.DownKeyAction();
        }
        else
        {
          this.IsRightLeftKeyPressed = true;
          this.RightKeyAction();
        }
        this.Parent.IsAltKeyPressed = true;
        break;
      case Key.Down:
        e.Handled = true;
        if (this.ParentMenuAdv != null && this.ParentMenuAdv.Orientation == Orientation.Vertical)
          this.RightKeyAction();
        else
          this.DownKeyAction();
        this.Parent.IsAltKeyPressed = true;
        break;
    }
    switch (e.SystemKey)
    {
      case Key.LeftAlt:
        this.AltKeyAction();
        break;
      case Key.RightAlt:
        this.AltKeyAction();
        break;
    }
    base.OnKeyDown(e);
  }

  internal void Dispose()
  {
    this.PopUpBorder = (Border) null;
    this.IconGrid = this.PopUpGrid = this.menuItemAdvGrid = (Grid) null;
    this.menuAdvStackPanel = (StackPanel) null;
    this.PART_ScrollViewer = (ScrollViewer) null;
    this.GestureTextBlock = (TextBlock) null;
    this.items = (IEnumerator<DependencyObject>) null;
    this.MenuItemBorder = (Border) null;
    this.Parent = this.ParentMenu = this.ParentMenuAdv = (MenuAdv) null;
    this.ParentMenuItemAdv = (MenuItemAdv) null;
    this.Icon = (object) null;
    this.GroupName = this.InputGestureText = (string) null;
    this.TopScrollButtonStyle = this.BottomScrollButtonStyle = this.CheckBoxStyle = this.RadioButtonStyle = (Style) null;
    if (this.ScrollerTick != null)
    {
      this.ScrollerTick.Tick -= new EventHandler(this.ScrollUp_Tick);
      this.ScrollerTick = (DispatcherTimer) null;
    }
    if (this.IconContent != null)
    {
      this.IconContent.Content = (object) null;
      this.IconContent = (ContentControl) null;
    }
    if (this.panel != null)
    {
      this.panel.LayoutUpdated -= new EventHandler(this.panel_LayoutUpdated);
      this.panel = (StackPanel) null;
    }
    if (this.CheckBoxPanel != null)
    {
      this.CheckBoxPanel.MouseLeave -= new MouseEventHandler(this.CheckBoxPanel_MouseLeave);
      this.CheckBoxPanel.Click -= new RoutedEventHandler(this.CheckBoxPanel_Click);
      this.CheckBoxPanel = (CheckBox) null;
    }
    if (this.RadioButtonPanel != null)
    {
      this.RadioButtonPanel.MouseLeave -= new MouseEventHandler(this.RadioButtonPanel_MouseLeave);
      this.RadioButtonPanel.Click -= new RoutedEventHandler(this.RadioButtonPanel_Click);
      this.RadioButtonPanel = (RadioButton) null;
    }
    if (this.PART_TopScroll != null)
    {
      this.PART_TopScroll.MouseEnter -= new MouseEventHandler(this.PART_TopScroll_MouseEnter);
      this.PART_TopScroll.MouseLeave -= new MouseEventHandler(this.PART_TopScroll_MouseLeave);
      this.PART_TopScroll = (Button) null;
    }
    if (this.PART_BottomScroll != null)
    {
      this.PART_BottomScroll.MouseEnter -= new MouseEventHandler(this.PART_BottomScroll_MouseEnter);
      this.PART_BottomScroll.MouseLeave -= new MouseEventHandler(this.PART_BottomScroll_MouseLeave);
      this.PART_BottomScroll = (Button) null;
    }
    if (this.SubMenuItemPopUp != null)
    {
      this.SubMenuItemPopUp.LayoutUpdated -= new EventHandler(this.SubMenuItemPopUp_LayoutUpdated);
      this.SubMenuItemPopUp.MouseEnter -= new MouseEventHandler(this.SubMenuItemPopUp_MouseEnter);
      this.SubMenuItemPopUp.MouseLeave -= new MouseEventHandler(this.SubMenuItemPopUp_MouseLeave);
      this.SubMenuItemPopUp = (Popup) null;
    }
    if (this.PopUpClosing != null)
      this.PopUpClosing -= new RoutedEventHandler(this.MenuItemAdv_PopUpClosing);
    this.Loaded -= new RoutedEventHandler(this.MenuItemAdv_Loaded);
    this.Unloaded -= new RoutedEventHandler(this.MenuItemAdv_Unloaded);
    this.IsEnabledChanged -= new DependencyPropertyChangedEventHandler(this.MenuItemAdv_IsEnabledChanged);
    if (this.ContainersToItems != null)
    {
      this.ContainersToItems.Clear();
      this.ContainersToItems = (IDictionary<DependencyObject, object>) null;
    }
    if (this.MenuItemBorder != null)
      this.MenuItemBorder.MouseEnter -= new MouseEventHandler(this.menuItemAdvGrid_MouseEnter);
    GC.SuppressFinalize((object) this);
  }

  private void MenuItemAdv_PopUpClosing(object sender, RoutedEventArgs e)
  {
    this.CallVisualState(this, "Normal");
    foreach (DependencyObject key in (IEnumerable<DependencyObject>) this.ContainersToItems.Keys)
    {
      if (key is MenuItemAdv && (key as MenuItemAdv).IsSubMenuOpen)
      {
        (key as MenuItemAdv).IsSubMenuOpen = false;
        if ((key as MenuItemAdv).PART_BottomScroll != null && (key as MenuItemAdv).PART_BottomScroll.Visibility == Visibility.Visible)
          (key as MenuItemAdv).PART_BottomScroll.Visibility = Visibility.Collapsed;
        if ((key as MenuItemAdv).PART_TopScroll != null && (key as MenuItemAdv).PART_TopScroll.Visibility == Visibility.Visible)
          (key as MenuItemAdv).PART_TopScroll.Visibility = Visibility.Collapsed;
      }
    }
  }

  private void RadioButtonPanel_MouseLeave(object sender, MouseEventArgs e)
  {
    Syncfusion.Windows.VisualStateManager.GoToState((Control) this.RadioButtonPanel, "MouseOver", false);
  }

  private void CheckBoxPanel_MouseLeave(object sender, MouseEventArgs e)
  {
    Syncfusion.Windows.VisualStateManager.GoToState((Control) this.CheckBoxPanel, "MouseOver", false);
  }

  private void MenuItemAdv_Loaded(object sender, RoutedEventArgs e)
  {
    if (this.ParentMenuItemAdv == null)
      this.isActiveScope = true;
    this.ChangeExtendButtonVisibility();
    this.ScrollerHeight = 0.0;
    if (this.ParentMenuItemAdv == null)
    {
      if (this.ParentMenuAdv != null && this.Items.Count > 0)
        this.Role = Role.TopLevelHeader;
      else if (this.ParentMenuAdv != null && this.Items.Count <= 0)
        this.Role = Role.TopLevelItem;
      if (this.ParentMenuAdv != null && !this.IsCheckable && this.Icon == null && this.IconGrid != null)
        this.IconGrid.Visibility = Visibility.Collapsed;
      if (this.ParentMenuAdv == null && !string.IsNullOrEmpty(this.InputGestureText) && this.GestureTextBlock != null)
        this.GestureTextBlock.Visibility = Visibility.Visible;
    }
    else
    {
      if (this.ParentMenuItemAdv != null && this.Items.Count > 0)
        this.Role = Role.SubmenuHeader;
      else if (this.ParentMenuItemAdv != null && this.Items.Count <= 0)
        this.Role = Role.SubmenuItem;
      foreach (DependencyObject key in (IEnumerable<DependencyObject>) this.ParentMenuItemAdv.ContainersToItems.Keys)
      {
        if (key is MenuItemAdv && (key as MenuItemAdv).InputGestureText != string.Empty)
          this.canGestureTextBlockVisible = true;
      }
      if (this.GestureTextBlock != null && this.canGestureTextBlockVisible)
        this.GestureTextBlock.Visibility = Visibility.Visible;
    }
    if (!this.IsEnabled)
      Syncfusion.Windows.VisualStateManager.GoToState((Control) this, "Disabled", false);
    if (this.MenuItemBorder != null)
      this.MenuItemBorder.MouseEnter += new MouseEventHandler(this.menuItemAdvGrid_MouseEnter);
    bool flag = true;
    if (this.ParentMenuAdv == null)
      return;
    using (IEnumerator<DependencyObject> enumerator = this.ParentMenuAdv.ContainersToItems.Keys.GetEnumerator())
    {
      if (enumerator.MoveNext())
      {
        DependencyObject current = enumerator.Current;
        if (current is MenuItemAdv)
        {
          if ((current as MenuItemAdv).IsSubMenuOpen)
            flag = false;
        }
      }
    }
    using (IEnumerator<DependencyObject> enumerator = this.ParentMenuAdv.ContainersToItems.Keys.GetEnumerator())
    {
      if (!enumerator.MoveNext())
        return;
      DependencyObject current = enumerator.Current;
      if (!(current is MenuItemAdv) || !flag || !(current as MenuItemAdv).IsEnabled || !this.Parent.FocusOnAlt)
        return;
      (current as MenuItemAdv).Focus();
      Syncfusion.Windows.VisualStateManager.GoToState((Control) (current as MenuItemAdv), "Normal", false);
      if ((current as MenuItemAdv).CheckBoxPanel != null)
        Syncfusion.Windows.VisualStateManager.GoToState((Control) (current as MenuItemAdv).CheckBoxPanel, "Normal", false);
      if ((current as MenuItemAdv).RadioButtonPanel != null)
        Syncfusion.Windows.VisualStateManager.GoToState((Control) (current as MenuItemAdv).RadioButtonPanel, "Normal", false);
      (current as MenuItemAdv).canFocused = false;
    }
  }

  protected void OnPanelWidthChanged(DependencyPropertyChangedEventArgs args)
  {
    if (args.OldValue == args.NewValue)
      return;
    this.HandlePopupOpen();
    this.SelectPopUpAnimation();
  }

  private void panel_LayoutUpdated(object sender, EventArgs e)
  {
    if (this.ParentMenuItemAdv != null)
    {
      if (this.ParentMenuItemAdv.PanelWidth == this.panel.ActualWidth || !this.ParentMenuItemAdv.IsSubMenuOpen || this.panel.ActualWidth == 0.0)
        return;
      this.ParentMenuItemAdv.PanelWidth = this.panel.ActualWidth;
      this.ParentMenuItemAdv.PanelHeight = this.panel.ActualHeight;
    }
    else
    {
      if (this.Parent == null || this.Parent.PanelWidth == this.ActualWidth || !this.IsSubMenuOpen || this.ActualWidth == 0.0)
        return;
      if (!this.SubMenuItemPopUp.IsOpen)
        this.SubMenuItemPopUp.IsOpen = true;
      this.Parent.GetMenuItem(this);
    }
  }

  private void SubMenuItemPopUp_LayoutUpdated(object sender, EventArgs e)
  {
    if (this.Parent != null && this.Parent.IsScrollEnabled)
    {
      this.ScrollerHeight = this.PART_ScrollViewer == null || this.PART_ScrollViewer.ScrollableHeight <= 0.0 || this.PART_ScrollViewer.ScrollableHeight == this.PART_ScrollViewer.ExtentHeight ? 0.0 : 10.0;
      if (this.ParentMenuItemAdv != null)
        this.ParentMenuItemAdv.ScrollabilityEnabled = true;
      if (this.ParentMenuItemAdv != null && this.ParentMenuItemAdv.ScrollabilityEnabled)
      {
        if (this.ParentMenuItemAdv.PART_ScrollViewer.ScrollableHeight - this.ParentMenuItemAdv.PART_ScrollViewer.VerticalOffset > 0.0)
        {
          this.ParentMenuItemAdv.PART_BottomScroll.Visibility = Visibility.Visible;
        }
        else
        {
          this.ParentMenuItemAdv.PART_BottomScroll.Visibility = Visibility.Collapsed;
          Syncfusion.Windows.VisualStateManager.GoToState((Control) this.ParentMenuItemAdv.PART_BottomScroll, "Normal", false);
        }
        if (this.ParentMenuItemAdv.PART_ScrollViewer.VerticalOffset > 0.0)
        {
          this.ParentMenuItemAdv.PART_TopScroll.Visibility = Visibility.Visible;
        }
        else
        {
          this.ParentMenuItemAdv.PART_TopScroll.Visibility = Visibility.Collapsed;
          Syncfusion.Windows.VisualStateManager.GoToState((Control) this.ParentMenuItemAdv.PART_TopScroll, "Normal", false);
        }
        if (this.ParentMenuItemAdv.PART_ScrollViewer.ScrollableHeight - this.ParentMenuItemAdv.PART_ScrollViewer.VerticalOffset > 0.0 && this.PopUpGrid.Height > 0.0)
          this.PopUpGrid.Height = -25.0;
        else if (this.ParentMenuItemAdv.PART_TopScroll.Visibility == Visibility.Visible && this.PopUpGrid.Height > 0.0)
          this.PopUpGrid.Height = -25.0;
        if (this.Parent.PopUpAnimationType == AnimationTypes.Slide && this.ParentMenuItemAdv.ParentMenuAdv != null && this.ParentMenuItemAdv.ParentMenuAdv.Orientation == Orientation.Horizontal && this.ParentMenuItemAdv.PART_ScrollViewer.VerticalOffset <= 0.0 && this.ParentMenuItemAdv.PART_ScrollViewer.ScrollableHeight - this.ParentMenuItemAdv.PART_ScrollViewer.VerticalOffset <= 0.0)
          this.ParentMenuItemAdv.SubMenuItemPopUp.VerticalOffset = -20.0;
      }
    }
    else if (this.ParentMenuItemAdv != null)
    {
      if (this.ParentMenuItemAdv.PART_TopScroll.Visibility == Visibility.Visible)
        this.ParentMenuItemAdv.PART_TopScroll.Visibility = Visibility.Collapsed;
      if (this.ParentMenuItemAdv.PART_BottomScroll.Visibility == Visibility.Visible)
        this.ParentMenuItemAdv.PART_BottomScroll.Visibility = Visibility.Collapsed;
    }
    if (this.isSubMenuItemsPopupOpenedThroughRightKey)
      this.RightKeyAction();
    if (this.isSubMenuItemsPopupOpenedThroughEnterKey)
      this.EnterKeyAction();
    if (this.isSubMenuItemsPopupOpenedThroughLeftKey)
      this.LeftKeyAction();
    if (!this.isSubMenuItemsPopupOpenedThroughDownKey)
      return;
    this.DownKeyAction();
  }

  private void ScrollUp_Tick(object sender, EventArgs e)
  {
    this.PART_ScrollViewer.ScrollToVerticalOffset(this.PART_ScrollViewer.VerticalOffset + this.scrollableValue);
  }

  private void PART_TopScroll_MouseLeave(object sender, MouseEventArgs e)
  {
    this.ScrollerTick.Stop();
    if (this.IsSubMenuOpen)
      return;
    Syncfusion.Windows.VisualStateManager.GoToState((Control) this.PART_TopScroll, "Normal", false);
  }

  private void PART_BottomScroll_MouseLeave(object sender, MouseEventArgs e)
  {
    this.ScrollerTick.Stop();
    this.Parent.IsItemMouseOver = false;
    if (this.IsSubMenuOpen)
      return;
    Syncfusion.Windows.VisualStateManager.GoToState((Control) this.PART_BottomScroll, "Normal", false);
  }

  private void PART_BottomScroll_MouseEnter(object sender, MouseEventArgs e)
  {
    this.ScrollerTick.Start();
    this.scrollableValue = 0.2;
    this.Parent.IsItemMouseOver = true;
    if (this.ContainersToItems.Keys.Count <= 0)
      return;
    foreach (DependencyObject key in (IEnumerable<DependencyObject>) this.ContainersToItems.Keys)
    {
      if (key is MenuItemAdv && (key as MenuItemAdv).IsEnabled)
      {
        (key as MenuItemAdv).IsSubMenuOpen = false;
        Syncfusion.Windows.VisualStateManager.GoToState((Control) (key as MenuItemAdv), "Normal", false);
        Syncfusion.Windows.VisualStateManager.GoToState((Control) (key as MenuItemAdv).CheckBoxPanel, "Normal", false);
        Syncfusion.Windows.VisualStateManager.GoToState((Control) (key as MenuItemAdv).RadioButtonPanel, "Normal", false);
      }
    }
  }

  private void PART_TopScroll_MouseEnter(object sender, MouseEventArgs e)
  {
    this.ScrollerTick.Start();
    this.scrollableValue = -0.2;
    this.Parent.IsItemMouseOver = true;
    if (this.ContainersToItems.Keys.Count <= 0)
      return;
    foreach (DependencyObject key in (IEnumerable<DependencyObject>) this.ContainersToItems.Keys)
    {
      if (key is MenuItemAdv && (key as MenuItemAdv).IsEnabled)
      {
        (key as MenuItemAdv).IsSubMenuOpen = false;
        Syncfusion.Windows.VisualStateManager.GoToState((Control) (key as MenuItemAdv), "Normal", false);
        Syncfusion.Windows.VisualStateManager.GoToState((Control) (key as MenuItemAdv).CheckBoxPanel, "Normal", false);
        Syncfusion.Windows.VisualStateManager.GoToState((Control) (key as MenuItemAdv).RadioButtonPanel, "Normal", false);
      }
    }
  }

  private static void OnIsSubMenuOpenChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    MenuItemAdv menuItemAdv = obj as MenuItemAdv;
    if (menuItemAdv.Parent != null && menuItemAdv.Parent != null)
      menuItemAdv.Parent.IsAllPopupClosed = false;
    if (menuItemAdv.SubMenuItemPopUp == null)
      return;
    if (menuItemAdv.IsSubMenuOpen)
    {
      menuItemAdv.SubMenuItemPopUp.IsOpen = true;
      menuItemAdv.OnOpening(new RoutedEventArgs());
      menuItemAdv.HandlePopupOpen();
      menuItemAdv.SelectPopUpAnimation();
      menuItemAdv.OnOpened(new RoutedEventArgs());
      if (menuItemAdv.ParentMenuAdv != null)
        menuItemAdv.ParentMenuAdv.IsMenuItemOpened = true;
      menuItemAdv.Parent.IsAltKeyPressed = true;
    }
    else
    {
      menuItemAdv.OnClosing(new RoutedEventArgs());
      menuItemAdv.SubMenuItemPopUp.IsOpen = false;
      menuItemAdv.IsBoundaryDetected = false;
      menuItemAdv.OnClosed(new RoutedEventArgs());
      if (menuItemAdv.ParentMenuAdv == null)
        return;
      menuItemAdv.ParentMenuAdv.IsMenuItemOpened = false;
    }
  }

  protected virtual void OnOpening(RoutedEventArgs e)
  {
    RoutedEventHandler popUpOpening = this.PopUpOpening;
    if (popUpOpening == null)
      return;
    popUpOpening((object) this, e);
  }

  protected virtual void OnOpened(RoutedEventArgs e)
  {
    RoutedEventHandler popUpOpened = this.PopUpOpened;
    foreach (object obj in (IEnumerable) this.Items)
    {
      if (obj is MenuItemAdv menuItemAdv)
        menuItemAdv.isActiveScope = true;
    }
    if (this.ParentMenuItemAdv != null)
    {
      foreach (object obj in (IEnumerable) this.ParentMenuItemAdv.Items)
      {
        if (obj is MenuItemAdv menuItemAdv)
          menuItemAdv.isActiveScope = false;
      }
    }
    else
    {
      foreach (object obj in (IEnumerable) this.ParentMenu.Items)
      {
        if (obj is MenuItemAdv menuItemAdv)
          menuItemAdv.isActiveScope = false;
      }
    }
    if (this.ParentMenu != null)
      this.ParentMenu.currentOpenMenu = this;
    if (popUpOpened == null)
      return;
    popUpOpened((object) this, e);
  }

  protected virtual void OnClosing(RoutedEventArgs e)
  {
    RoutedEventHandler popUpClosing = this.PopUpClosing;
    if (popUpClosing == null)
      return;
    popUpClosing((object) this, e);
  }

  protected virtual void OnClosed(RoutedEventArgs e)
  {
    RoutedEventHandler popUpClosed = this.PopUpClosed;
    if (this.ParentMenu != null)
    {
      this.ParentMenu.currentOpenMenu = (MenuItemAdv) null;
      if (this.ParentMenuItemAdv != null && this.ParentMenuItemAdv.IsSubMenuOpen)
      {
        this.ParentMenu.currentOpenMenu = this.ParentMenuItemAdv;
        this.CallVisualState(this, "Normal");
      }
    }
    if (popUpClosed != null)
      popUpClosed((object) this, e);
    foreach (object obj in (IEnumerable) this.Items)
    {
      if (obj is MenuItemAdv menuItemAdv)
        menuItemAdv.isActiveScope = false;
    }
    if (this.ParentMenuItemAdv != null)
    {
      foreach (object obj in (IEnumerable) this.ParentMenuItemAdv.Items)
      {
        if (obj is MenuItemAdv menuItemAdv)
          menuItemAdv.isActiveScope = true;
      }
    }
    else
    {
      foreach (object obj in (IEnumerable) this.ParentMenu.Items)
      {
        if (obj is MenuItemAdv menuItemAdv)
          menuItemAdv.isActiveScope = true;
      }
    }
  }

  private static void OnIsCheckablePropertyChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    if (!((MenuItemAdv) obj).IsCheckable)
    {
      ((MenuItemAdv) obj).RadioButtonVisibility = Visibility.Collapsed;
      ((MenuItemAdv) obj).CheckBoxVisibility = Visibility.Collapsed;
    }
    else
    {
      if (!((MenuItemAdv) obj).IsChecked)
        return;
      ((MenuItemAdv) obj).CheckBoxVisibility = ((MenuItemAdv) obj).CheckIconType != CheckIconType.RadioButton ? Visibility.Visible : Visibility.Collapsed;
      ((MenuItemAdv) obj).RadioButtonVisibility = ((MenuItemAdv) obj).CheckIconType == CheckIconType.RadioButton ? Visibility.Visible : Visibility.Collapsed;
    }
  }

  private static void OnCheckIconTypeChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    if (!((MenuItemAdv) obj).IsCheckable)
      return;
    if (((MenuItemAdv) obj).IsChecked)
    {
      ((MenuItemAdv) obj).CheckBoxVisibility = ((MenuItemAdv) obj).CheckIconType != CheckIconType.RadioButton ? Visibility.Visible : Visibility.Collapsed;
      ((MenuItemAdv) obj).RadioButtonVisibility = ((MenuItemAdv) obj).CheckIconType == CheckIconType.RadioButton ? Visibility.Visible : Visibility.Collapsed;
    }
    else
    {
      ((MenuItemAdv) obj).CheckBoxVisibility = Visibility.Collapsed;
      ((MenuItemAdv) obj).RadioButtonVisibility = Visibility.Collapsed;
    }
  }

  protected virtual void OnIsCheckedPropertyChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.IsChecked)
    {
      this.CheckBoxVisibility = this.CheckIconType != CheckIconType.RadioButton ? Visibility.Visible : Visibility.Collapsed;
      this.RadioButtonVisibility = this.CheckIconType == CheckIconType.RadioButton ? Visibility.Visible : Visibility.Collapsed;
      if (this.Checked == null)
        return;
      this.Checked((object) this, new RoutedEventArgs());
    }
    else
    {
      this.CheckBoxVisibility = Visibility.Collapsed;
      this.RadioButtonVisibility = Visibility.Collapsed;
      if (this.UnChecked == null)
        return;
      this.UnChecked((object) this, new RoutedEventArgs());
    }
  }

  internal void CallVisualState(MenuItemAdv Item, string state)
  {
    Syncfusion.Windows.VisualStateManager.GoToState((Control) Item, state, false);
    if (state == "Normal" && Item.CheckBoxPanel != null && Item.RadioButtonPanel != null)
    {
      Syncfusion.Windows.VisualStateManager.GoToState((Control) Item.CheckBoxPanel, "Normal", false);
      Syncfusion.Windows.VisualStateManager.GoToState((Control) Item.RadioButtonPanel, "Normal", false);
    }
    else
    {
      if (Item.CheckBoxPanel == null || Item.RadioButtonPanel == null)
        return;
      Syncfusion.Windows.VisualStateManager.GoToState((Control) Item.CheckBoxPanel, "MouseOver", false);
      Syncfusion.Windows.VisualStateManager.GoToState((Control) Item.RadioButtonPanel, "MouseOver", false);
    }
  }

  internal void UpKeyAction()
  {
    if (this.ParentMenuAdv != null)
    {
      if (this.Items.Count <= 0)
        return;
      int num = this.Items.Count - 1;
      this.items = this.ContainersToItems.Keys.GetEnumerator();
      this.items.Reset();
      for (int index = 0; index <= num; ++index)
        this.items.MoveNext();
      while (num >= 0 && this.items.Current is MenuItemSeparator)
      {
        --num;
        this.items.Reset();
        for (int index = 0; index <= num; ++index)
          this.items.MoveNext();
      }
      if (num >= this.Items.Count || !(this.items.Current is MenuItemAdv))
        return;
      this.CallVisualState((MenuItemAdv) this.items.Current, "SubMenuItemFocused");
      ((UIElement) this.items.Current).Focus();
    }
    else
    {
      if (this.ParentMenuItemAdv == null)
        return;
      int num1 = 0;
      this.items = this.ParentMenuItemAdv.ContainersToItems.Keys.GetEnumerator();
      this.items.Reset();
      this.items.MoveNext();
      while (this.items.Current is MenuItemSeparator || (MenuItemAdv) this.items.Current != this)
      {
        ++num1;
        this.items.MoveNext();
      }
      this.CallVisualState((MenuItemAdv) this.items.Current, "Normal");
      ((MenuItemAdv) this.items.Current).IsSubMenuOpen = false;
      if (num1 >= 0)
      {
        --num1;
        this.items.Reset();
        for (int index = 0; index <= num1; ++index)
          this.items.MoveNext();
      }
      while (num1 >= 0 && (this.items.Current is MenuItemSeparator || !((UIElement) this.items.Current).IsEnabled))
      {
        --num1;
        this.items.Reset();
        for (int index = 0; index <= num1; ++index)
          this.items.MoveNext();
      }
      if (num1 >= 0)
      {
        this.CallVisualState((MenuItemAdv) this.items.Current, "SubMenuItemFocused");
        ((UIElement) this.items.Current).Focus();
      }
      else
      {
        int num2 = this.ParentMenuItemAdv.Items.Count - 1;
        this.items.Reset();
        for (int index = 0; index <= num2; ++index)
          this.items.MoveNext();
        while (num2 >= 0 && (this.items.Current is MenuItemSeparator || !((UIElement) this.items.Current).IsEnabled))
        {
          --num2;
          this.items.Reset();
          for (int index = 0; index <= num2; ++index)
            this.items.MoveNext();
        }
        this.CallVisualState((MenuItemAdv) this.items.Current, "SubMenuItemFocused");
        ((UIElement) this.items.Current).Focus();
      }
    }
  }

  internal void DownKeyAction()
  {
    if (this.ParentMenuAdv != null)
    {
      if (this.Items.Count <= 0)
        return;
      this.IsSubMenuOpen = true;
      this.ParentMenuAdv.firstClick = true;
      this.isSubMenuItemsPopupOpenedThroughDownKey = true;
      foreach (DependencyObject key in (IEnumerable<DependencyObject>) this.ContainersToItems.Keys)
      {
        if (key is MenuItemAdv)
        {
          this.CallVisualState(this, "MenuItemSelected");
          (key as MenuItemAdv).Focus();
          this.CallVisualState(key as MenuItemAdv, "SubMenuItemFocused");
          this.isSubMenuItemsPopupOpenedThroughDownKey = false;
          break;
        }
      }
    }
    else
    {
      if (this.ParentMenuItemAdv == null)
        return;
      int num1 = 0;
      this.items = this.ParentMenuItemAdv.ContainersToItems.Keys.GetEnumerator();
      this.items.Reset();
      this.items.MoveNext();
      while (this.items.Current is MenuItemSeparator || (MenuItemAdv) this.items.Current != this)
      {
        this.items.MoveNext();
        ++num1;
      }
      this.CallVisualState((MenuItemAdv) this.items.Current, "Normal");
      ((MenuItemAdv) this.items.Current).IsSubMenuOpen = false;
      if (num1 < this.ParentMenuItemAdv.Items.Count)
      {
        this.items.MoveNext();
        ++num1;
      }
      while (this.items.Current != null && (this.items.Current is MenuItemSeparator || !((UIElement) this.items.Current).IsEnabled))
      {
        if (num1 + 1 < this.ParentMenuItemAdv.ContainersToItems.Keys.Count)
        {
          ++num1;
          this.items.MoveNext();
        }
        else
        {
          num1 = 0;
          this.items.Reset();
          this.items.MoveNext();
          for (; num1 < this.ParentMenuItemAdv.Items.Count && !((UIElement) this.items.Current).IsEnabled; ++num1)
            this.items.MoveNext();
          this.CallVisualState((MenuItemAdv) this.items.Current, "SubMenuItemFocused");
          ((UIElement) this.items.Current).Focus();
        }
      }
      if (num1 < this.ParentMenuItemAdv.Items.Count)
      {
        if (!(this.items.Current is MenuItemAdv))
          return;
        this.CallVisualState((MenuItemAdv) this.items.Current, "SubMenuItemFocused");
        ((UIElement) this.items.Current).Focus();
      }
      else
      {
        int num2 = 0;
        this.items.Reset();
        this.items.MoveNext();
        for (; num2 < this.ParentMenuItemAdv.Items.Count && !((UIElement) this.items.Current).IsEnabled; ++num2)
          this.items.MoveNext();
        this.CallVisualState((MenuItemAdv) this.items.Current, "SubMenuItemFocused");
        ((UIElement) this.items.Current).Focus();
      }
    }
  }

  internal void RightKeyAction()
  {
    if (this.ParentMenuAdv != null)
    {
      this.items = this.ParentMenuAdv.ContainersToItems.Keys.GetEnumerator();
      this.items.Reset();
      this.items.MoveNext();
      int num1 = 0;
      while (this.items.Current is MenuItemSeparator || this.items.Current != this)
      {
        this.items.MoveNext();
        ++num1;
      }
      this.CallVisualState(this, "Normal");
      if (((UIElement) this.items.Current).IsEnabled && ((UIElement) this.items.Current).Visibility == Visibility.Visible)
      {
        this.CallVisualState((MenuItemAdv) this.items.Current, "Normal");
        ((MenuItemAdv) this.items.Current).IsSubMenuOpen = false;
      }
      if (num1 < this.ParentMenuAdv.Items.Count)
      {
        this.items.MoveNext();
        ++num1;
      }
      while (num1 < this.ParentMenuAdv.ContainersToItems.Keys.Count && (this.items.Current is MenuItemSeparator || !((UIElement) this.items.Current).IsEnabled || ((UIElement) this.items.Current).Visibility != Visibility.Visible))
      {
        ++num1;
        this.items.MoveNext();
      }
      if (num1 <= this.ParentMenuAdv.ContainersToItems.Keys.Count)
      {
        if (num1 == this.ParentMenuAdv.ContainersToItems.Keys.Count)
        {
          this.items.Reset();
          this.items.MoveNext();
        }
        this.CallVisualState((MenuItemAdv) this.items.Current, "MenuItemSelected");
        ((UIElement) this.items.Current).Focus();
        if (this.ParentMenuAdv.ExpandMode != ExpandModes.ExpandOnMouseOver && !this.ParentMenuAdv.firstClick)
          return;
        ((MenuItemAdv) this.items.Current).IsSubMenuOpen = true;
        foreach (object menu in (IEnumerable) ((ItemsControl) this.items.Current).Items)
        {
          if (menu is MenuItemAdv)
            this.UpdateCanExecute(menu as MenuItemAdv);
        }
        if (this.ItemContainerGenerator.IndexFromContainer(this.items.Current) != this.Items.Count - 1)
          this.CallVisualState((MenuItemAdv) this.items.Current, "MenuItemSelected");
        if (!this.IsRightLeftKeyPressed)
          return;
        this.IsRightLeftKeyPressed = false;
        if (!((this.items.Current as MenuItemAdv).ItemContainerGenerator.ContainerFromIndex(0) is MenuItemAdv menuItemAdv))
          return;
        menuItemAdv.Focus();
        this.CallVisualState(menuItemAdv, "SubMenuItemFocused");
      }
      else
      {
        int num2 = 0;
        this.items.Reset();
        this.items.MoveNext();
        for (; num2 < this.ParentMenuAdv.ContainersToItems.Keys.Count && (this.items.Current is MenuItemSeparator || !((UIElement) this.items.Current).IsEnabled || ((UIElement) this.items.Current).Visibility != Visibility.Visible); ++num2)
          this.items.MoveNext();
        this.CallVisualState((MenuItemAdv) this.items.Current, "MenuItemSelected");
        ((UIElement) this.items.Current).Focus();
        if (this.ParentMenuAdv.ExpandMode != ExpandModes.ExpandOnMouseOver && !this.ParentMenuAdv.firstClick)
          return;
        ((MenuItemAdv) this.items.Current).IsSubMenuOpen = true;
        foreach (object menu in (IEnumerable) ((ItemsControl) this.items.Current).Items)
        {
          if (menu is MenuItemAdv)
            this.UpdateCanExecute(menu as MenuItemAdv);
        }
        ((MenuItemAdv) this.items.Current).isSubMenuItemsPopupOpenedThroughRightKey = true;
        this.CallVisualState((MenuItemAdv) this.items.Current, "MenuItemSelected");
        if (!this.IsRightLeftKeyPressed)
          return;
        this.IsRightLeftKeyPressed = false;
        ((this.items.Current as MenuItemAdv).ItemContainerGenerator.ContainerFromIndex(0) as MenuItemAdv).Focus();
        this.CallVisualState((this.items.Current as MenuItemAdv).ItemContainerGenerator.ContainerFromIndex(0) as MenuItemAdv, "SubMenuItemFocused");
      }
    }
    else
    {
      if (this.ParentMenuItemAdv == null)
        return;
      if (this.Items.Count > 0)
      {
        this.Focus();
        this.IsSubMenuOpen = true;
        foreach (object menu in (IEnumerable) this.Items)
        {
          if (menu is MenuItemAdv)
            this.UpdateCanExecute(menu as MenuItemAdv);
        }
        this.isSubMenuItemsPopupOpenedThroughRightKey = true;
        foreach (DependencyObject key in (IEnumerable<DependencyObject>) this.ContainersToItems.Keys)
        {
          if (key is MenuItemAdv && (key as MenuItemAdv).IsEnabled && (key as MenuItemAdv).Visibility == Visibility.Visible)
          {
            (key as MenuItemAdv).Focus();
            this.CallVisualState(key as MenuItemAdv, "SubMenuItemFocused");
            this.isSubMenuItemsPopupOpenedThroughRightKey = false;
            break;
          }
        }
      }
      else if (this.ParentMenuItemAdv.ParentMenuAdv != null)
      {
        this.CallVisualState(this, "Normal");
        this.ParentMenuItemAdv.IsRightLeftKeyPressed = true;
        this.ParentMenuItemAdv.RightKeyAction();
      }
      else
      {
        if (this.ParentMenuItemAdv == null || this.ParentMenuItemAdv.ParentMenuAdv != null)
          return;
        MenuItemAdv menuItemAdv = this;
        while (menuItemAdv.ParentMenuItemAdv != null & menuItemAdv.ParentMenuItemAdv.ParentMenuAdv == null)
        {
          MenuItemAdv parentMenuItemAdv = menuItemAdv.ParentMenuItemAdv;
          if (menuItemAdv.IsEnabled && menuItemAdv.Visibility == Visibility.Visible)
          {
            this.CallVisualState(menuItemAdv, "Normal");
            menuItemAdv.ParentMenuItemAdv.IsSubMenuOpen = false;
            menuItemAdv = parentMenuItemAdv;
          }
        }
        this.CallVisualState(menuItemAdv, "Normal");
        menuItemAdv.ParentMenuItemAdv.IsRightLeftKeyPressed = true;
        menuItemAdv.ParentMenuItemAdv.RightKeyAction();
      }
    }
  }

  internal void LeftKeyAction()
  {
    if (this.ParentMenuAdv != null)
    {
      this.items = this.ParentMenuAdv.ContainersToItems.Keys.GetEnumerator();
      this.items.Reset();
      this.items.MoveNext();
      int num1 = 0;
      while (this.items.Current is MenuItemSeparator || (MenuItemAdv) this.items.Current != this)
      {
        this.items.MoveNext();
        ++num1;
      }
      if (((UIElement) this.items.Current).IsEnabled && ((UIElement) this.items.Current).Visibility == Visibility.Visible)
      {
        this.CallVisualState((MenuItemAdv) this.items.Current, "MenuItemSelected");
        ((MenuItemAdv) this.items.Current).IsSubMenuOpen = false;
      }
      if (num1 >= 0)
      {
        --num1;
        this.items.Reset();
        for (int index = 0; index <= num1; ++index)
          this.items.MoveNext();
      }
      while (num1 >= 0 && (this.items.Current is MenuItemSeparator || !((UIElement) this.items.Current).IsEnabled || ((UIElement) this.items.Current).Visibility != Visibility.Visible))
      {
        --num1;
        this.items.Reset();
        for (int index = 0; index <= num1; ++index)
          this.items.MoveNext();
      }
      if (num1 >= 0)
      {
        if (!((UIElement) this.items.Current).IsEnabled || ((UIElement) this.items.Current).Visibility != Visibility.Visible)
          return;
        ((UIElement) this.items.Current).Focus();
        if (this.ParentMenuAdv.ExpandMode == ExpandModes.ExpandOnMouseOver || this.ParentMenuAdv.firstClick)
        {
          ((MenuItemAdv) this.items.Current).IsSubMenuOpen = true;
          if (this.IsRightLeftKeyPressed)
          {
            this.IsRightLeftKeyPressed = false;
            this.CallVisualState((MenuItemAdv) this.items.Current, "MenuItemSelected");
            if ((this.items.Current as MenuItemAdv).ItemContainerGenerator.ContainerFromIndex(0) is MenuItemAdv menuItemAdv)
            {
              menuItemAdv.Focus();
              this.CallVisualState(menuItemAdv, "SubMenuItemFocused");
            }
          }
        }
        this.CallVisualState((MenuItemAdv) this.items.Current, "MenuItemSelected");
      }
      else
      {
        int num2 = this.ParentMenuAdv.ContainersToItems.Keys.Count - 1;
        this.items.Reset();
        for (int index = 0; index <= num2; ++index)
          this.items.MoveNext();
        while (num2 >= 0 && (this.items.Current is MenuItemSeparator || !((UIElement) this.items.Current).IsEnabled || ((UIElement) this.items.Current).Visibility != Visibility.Visible))
        {
          --num2;
          this.items.Reset();
          for (int index = 0; index <= num2; ++index)
            this.items.MoveNext();
        }
        if (this.ParentMenuAdv.ExpandMode == ExpandModes.ExpandOnMouseOver || this.ParentMenuAdv.firstClick)
          ((MenuItemAdv) this.items.Current).IsSubMenuOpen = true;
        this.CallVisualState((MenuItemAdv) this.items.Current, "MenuItemSelected");
        ((UIElement) this.items.Current).Focus();
      }
    }
    else
    {
      if (this.ParentMenuItemAdv == null)
        return;
      if (this.ParentMenuItemAdv.ParentMenuAdv == null)
      {
        if (this.ParentMenuItemAdv.IsSubMenuOpen)
        {
          this.CallVisualState(this, "Normal");
          this.ParentMenuItemAdv.IsSubMenuOpen = false;
          this.CallVisualState(this.ParentMenuItemAdv, "SubMenuItemFocused");
          this.ParentMenuItemAdv.Focus();
        }
        else
          this.ParentMenuItemAdv.LeftKeyAction();
      }
      else
      {
        this.CallVisualState(this, "Normal");
        this.ParentMenuItemAdv.IsRightLeftKeyPressed = true;
        this.ParentMenuItemAdv.LeftKeyAction();
      }
    }
  }

  internal void EnterKeyAction()
  {
    if (!this.StaysOpenOnClick && this.Items.Count <= 0)
    {
      this.Parent.CloseAllPopUps();
      if (this.Parent.ExpandMode == ExpandModes.ExpandOnClick)
        this.Parent.firstClick = false;
    }
    else
    {
      if (this.Items.Count > 0)
      {
        this.CallVisualState(this, "SubMenuItemFocused");
        this.IsSubMenuOpen = true;
        foreach (object menu in (IEnumerable) this.Items)
        {
          if (menu is MenuItemAdv)
            this.UpdateCanExecute(menu as MenuItemAdv);
        }
      }
      if (this.ParentMenuAdv != null)
      {
        this.ParentMenuAdv.firstClick = true;
        if (this.IsSubMenuOpen && this.Parent.Orientation == Orientation.Horizontal)
          this.CallVisualState(this, "MenuItemSelected");
        else
          this.CallVisualState(this, "MenuItemFocused");
      }
      this.isSubMenuItemsPopupOpenedThroughEnterKey = true;
      foreach (DependencyObject key in (IEnumerable<DependencyObject>) this.ContainersToItems.Keys)
      {
        if (key is MenuItemAdv && (key as MenuItemAdv).IsEnabled && (key as MenuItemAdv).Visibility == Visibility.Visible)
        {
          (key as MenuItemAdv).Focus();
          this.CallVisualState(key as MenuItemAdv, "SubMenuItemFocused");
          this.isSubMenuItemsPopupOpenedThroughEnterKey = false;
          break;
        }
      }
    }
    if (this.Items.Count != 0)
      return;
    if (MenuItemAdv.ClickEvent != null)
      this.RaiseEvent(new RoutedEventArgs(MenuItemAdv.ClickEvent));
    if (this.IsCheckable && this.CheckIconType != CheckIconType.None)
    {
      if (this.CheckIconType == CheckIconType.CheckBox)
        this.IsChecked = !this.IsChecked;
      else if (this.GroupName.Equals(string.Empty))
      {
        this.IsChecked = true;
      }
      else
      {
        this.IsChecked = true;
        this.UnCheck(this);
      }
    }
    if (this.Command == null)
      return;
    if (this.Command is RoutedCommand command)
      command.Execute(this.CommandParameter, this.CommandTarget);
    else
      this.Command.Execute(this.CommandParameter);
  }

  internal void TabKeyAction()
  {
    if (this.ParentMenuAdv != null)
    {
      this.RightKeyAction();
    }
    else
    {
      if (this.ParentMenuItemAdv == null)
        return;
      this.DownKeyAction();
    }
  }

  internal void AltKeyAction()
  {
    if (!this.Parent.IsAltKeyPressed && this.Parent.FocusOnAlt)
    {
      foreach (DependencyObject key in (IEnumerable<DependencyObject>) this.Parent.ContainersToItems.Keys)
      {
        if (key is MenuItemAdv)
        {
          this.Parent.IsAltKeyPressed = true;
          (key as MenuItemAdv).Focus();
          (key as MenuItemAdv).canFocused = true;
          break;
        }
      }
    }
    else
    {
      this.Parent.IsAltKeyPressed = false;
      this.Parent.CloseAllPopUps();
    }
  }

  internal void EscapeKeyAction()
  {
    if (this.ParentMenuItemAdv != null)
    {
      this.CallVisualState(this, "Normal");
      this.ParentMenuItemAdv.IsSubMenuOpen = false;
      this.ParentMenuItemAdv.Focus();
      if (this.ParentMenuItemAdv.ParentMenuAdv != null)
        this.ParentMenuItemAdv.Parent.firstClick = false;
      this.CallVisualState(this.ParentMenuItemAdv, "SubMenuItemFocused");
    }
    else if (this.ParentMenuAdv != null && !this.IsSubMenuOpen)
    {
      this.CallVisualState(this, "MenuItemFocused");
      this.Parent.firstClick = false;
      this.Parent.IsAltKeyPressed = false;
    }
    else
    {
      if (this.ParentMenuAdv == null || !this.IsSubMenuOpen)
        return;
      this.IsSubMenuOpen = false;
    }
  }

  internal void HandlePopupOpen()
  {
    if (this.SubMenuItemPopUp == null)
      return;
    TransformGroup transformGroup = new TransformGroup();
    ScaleTransform scaleTransform = new ScaleTransform();
    TranslateTransform translateTransform = new TranslateTransform();
    transformGroup.Children.Add((Transform) scaleTransform);
    transformGroup.Children.Add((Transform) translateTransform);
    this.PopUpBorder.RenderTransform = (Transform) transformGroup;
    double primaryScreenWidth = SystemParameters.PrimaryScreenWidth;
    double primaryScreenHeight = SystemParameters.PrimaryScreenHeight;
    Point point = new Point();
    try
    {
      point = this.PointToScreen(new Point(0.0, 0.0));
    }
    catch
    {
    }
    this.SubMenuItemPopUp.VerticalOffset = 0.0;
    this.SubMenuItemPopUp.HorizontalOffset = 0.0;
    if (this.ParentMenuAdv != null && this.Parent.Orientation == Orientation.Horizontal)
    {
      this.SubMenuItemPopUp.PlacementTarget = (UIElement) this;
      if (primaryScreenHeight < point.Y + this.ActualHeight + this.PopUpBorder.ActualHeight && SystemParameters.PrimaryScreenWidth < SystemParameters.VirtualScreenWidth && (SystemParameters.WorkArea.X < point.X || SystemParameters.WorkArea.Y < point.Y))
        point.Y -= SystemParameters.PrimaryScreenHeight;
      if (this.PopUpBorder.ActualHeight > 0.0 && point.Y + this.ActualHeight + this.PopUpBorder.ActualHeight > primaryScreenHeight)
      {
        if (point.Y > primaryScreenHeight - this.ActualWidth - point.Y)
        {
          this.SubMenuItemPopUp.Placement = PlacementMode.Top;
          this.IsBoundaryDetected = true;
        }
        else
        {
          this.SubMenuItemPopUp.Placement = PlacementMode.Bottom;
          this.IsBoundaryDetected = false;
        }
      }
      else
      {
        this.SubMenuItemPopUp.Placement = PlacementMode.Bottom;
        this.IsBoundaryDetected = false;
      }
    }
    else
    {
      this.SubMenuItemPopUp.PlacementTarget = (UIElement) this;
      if (primaryScreenWidth < point.X + this.ActualWidth + this.PopUpBorder.ActualWidth && SystemParameters.PrimaryScreenWidth < SystemParameters.VirtualScreenWidth && (SystemParameters.WorkArea.X < point.X || SystemParameters.WorkArea.Y < point.Y))
        point.X -= SystemParameters.PrimaryScreenWidth;
      if (this.PopUpBorder.ActualWidth > 0.0 && point.X + this.ActualWidth + this.PopUpBorder.ActualWidth > primaryScreenWidth)
      {
        this.SubMenuItemPopUp.Placement = PlacementMode.Left;
        this.IsBoundaryDetected = true;
      }
      else
      {
        this.SubMenuItemPopUp.Placement = PlacementMode.Right;
        this.IsBoundaryDetected = false;
      }
    }
  }

  internal void SelectPopUpAnimation()
  {
    if (this.Parent == null)
      return;
    if (this.Parent.PopUpAnimationType == AnimationTypes.Fade)
    {
      Storyboard storyboard = new Storyboard();
      DoubleAnimationUsingKeyFrames element = new DoubleAnimationUsingKeyFrames();
      storyboard.Children.Add((Timeline) element);
      Storyboard.SetTarget((DependencyObject) element, (DependencyObject) this.PopUpBorder);
      Storyboard.SetTargetProperty((DependencyObject) element, new PropertyPath("(UIElement.Opacity)", new object[0]));
      SplineDoubleKeyFrame keyFrame1 = new SplineDoubleKeyFrame();
      keyFrame1.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.0));
      keyFrame1.Value = 0.5;
      element.KeyFrames.Add((DoubleKeyFrame) keyFrame1);
      SplineDoubleKeyFrame keyFrame2 = new SplineDoubleKeyFrame();
      keyFrame2.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.4));
      keyFrame2.Value = 1.0;
      element.KeyFrames.Add((DoubleKeyFrame) keyFrame2);
      storyboard.Begin();
    }
    else if (this.Parent.PopUpAnimationType == AnimationTypes.Scroll)
    {
      if (this.ParentMenuAdv != null && this.Parent.Orientation == Orientation.Horizontal)
      {
        Storyboard storyboard = new Storyboard();
        DoubleAnimationUsingKeyFrames element1 = new DoubleAnimationUsingKeyFrames();
        storyboard.Children.Add((Timeline) element1);
        Storyboard.SetTarget((DependencyObject) element1, (DependencyObject) this.PopUpBorder);
        Storyboard.SetTargetProperty((DependencyObject) element1, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)", new object[0]));
        SplineDoubleKeyFrame keyFrame3 = new SplineDoubleKeyFrame();
        keyFrame3.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.0));
        keyFrame3.Value = 0.0;
        element1.KeyFrames.Add((DoubleKeyFrame) keyFrame3);
        SplineDoubleKeyFrame keyFrame4 = new SplineDoubleKeyFrame();
        keyFrame4.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.3));
        keyFrame4.Value = 1.0;
        element1.KeyFrames.Add((DoubleKeyFrame) keyFrame4);
        PointAnimationUsingKeyFrames element2 = new PointAnimationUsingKeyFrames();
        storyboard.Children.Add((Timeline) element2);
        Storyboard.SetTarget((DependencyObject) element2, (DependencyObject) this.PopUpBorder);
        Storyboard.SetTargetProperty((DependencyObject) element2, new PropertyPath("(UIElement.RenderTransformOrigin)", new object[0]));
        SplinePointKeyFrame keyFrame5 = new SplinePointKeyFrame();
        if (!this.IsBoundaryDetected)
        {
          if (this.PopUpBorder.RenderTransformOrigin.X != 0.5 && this.PopUpBorder.RenderTransformOrigin.Y != 0.0)
            this.PopUpBorder.RenderTransformOrigin = new Point(0.5, 0.0);
          keyFrame5.Value = new Point(0.5, 0.0);
        }
        else
        {
          if (this.PopUpBorder.RenderTransformOrigin.X != 0.5 && this.PopUpBorder.RenderTransformOrigin.Y != 1.0)
            this.PopUpBorder.RenderTransformOrigin = new Point(0.5, 1.0);
          keyFrame5.Value = new Point(0.5, 1.0);
        }
        element2.KeyFrames.Add((PointKeyFrame) keyFrame5);
        storyboard.Begin();
      }
      else
      {
        Storyboard storyboard = new Storyboard();
        DoubleAnimationUsingKeyFrames element3 = new DoubleAnimationUsingKeyFrames();
        storyboard.Children.Add((Timeline) element3);
        Storyboard.SetTarget((DependencyObject) element3, (DependencyObject) this.PopUpBorder);
        Storyboard.SetTargetProperty((DependencyObject) element3, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)", new object[0]));
        SplineDoubleKeyFrame keyFrame6 = new SplineDoubleKeyFrame();
        keyFrame6.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.0));
        keyFrame6.Value = 0.0;
        element3.KeyFrames.Add((DoubleKeyFrame) keyFrame6);
        SplineDoubleKeyFrame keyFrame7 = new SplineDoubleKeyFrame();
        keyFrame7.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.3));
        keyFrame7.Value = 1.0;
        element3.KeyFrames.Add((DoubleKeyFrame) keyFrame7);
        PointAnimationUsingKeyFrames element4 = new PointAnimationUsingKeyFrames();
        storyboard.Children.Add((Timeline) element4);
        Storyboard.SetTarget((DependencyObject) element4, (DependencyObject) this.PopUpBorder);
        Storyboard.SetTargetProperty((DependencyObject) element4, new PropertyPath("(UIElement.RenderTransformOrigin)", new object[0]));
        SplinePointKeyFrame keyFrame8 = new SplinePointKeyFrame();
        if (!this.IsBoundaryDetected)
        {
          if (this.PopUpBorder.RenderTransformOrigin.X != 0.0 || this.PopUpBorder.RenderTransformOrigin.Y != 0.5)
            this.PopUpBorder.RenderTransformOrigin = new Point(0.0, 0.5);
          keyFrame8.Value = new Point(0.0, 0.5);
        }
        else
        {
          if (this.PopUpBorder.RenderTransformOrigin.X != 1.0 || this.PopUpBorder.RenderTransformOrigin.Y != 0.5)
            this.PopUpBorder.RenderTransformOrigin = new Point(1.0, 0.5);
          keyFrame8.Value = new Point(1.0, 0.5);
        }
        element4.KeyFrames.Add((PointKeyFrame) keyFrame8);
        storyboard.Begin();
      }
    }
    else
    {
      if (this.Parent.PopUpAnimationType != AnimationTypes.Slide)
        return;
      if (this.ParentMenuAdv != null & this.Parent.Orientation == Orientation.Horizontal)
      {
        Storyboard storyboard = new Storyboard();
        DoubleAnimation element = new DoubleAnimation();
        storyboard.Children.Add((Timeline) element);
        Storyboard.SetTarget((DependencyObject) element, (DependencyObject) this.SubMenuItemPopUp);
        Storyboard.SetTargetProperty((DependencyObject) element, new PropertyPath("VerticalOffset", new object[0]));
        element.BeginTime = new TimeSpan?(new TimeSpan(0, 0, 0));
        element.Duration = (Duration) new TimeSpan(0, 0, 0, 0, 100);
        element.From = this.IsBoundaryDetected ? new double?(15.0) : new double?(-15.0);
        element.To = new double?(0.0);
        storyboard.Begin();
      }
      else
      {
        Storyboard storyboard = new Storyboard();
        DoubleAnimation element = new DoubleAnimation();
        storyboard.Children.Add((Timeline) element);
        Storyboard.SetTarget((DependencyObject) element, (DependencyObject) this.SubMenuItemPopUp);
        Storyboard.SetTargetProperty((DependencyObject) element, new PropertyPath("HorizontalOffset", new object[0]));
        element.BeginTime = new TimeSpan?(new TimeSpan(0, 0, 0));
        element.Duration = (Duration) new TimeSpan(0, 0, 0, 0, 100);
        element.From = this.IsBoundaryDetected ? new double?(20.0) : new double?(-20.0);
        element.To = new double?(0.0);
        storyboard.Begin();
      }
    }
  }

  internal void ChangeExtendButtonVisibility()
  {
    this.ExtendButtonVisibility = this.ParentMenuItemAdv == null || this.Items.Count <= 0 ? Visibility.Collapsed : Visibility.Visible;
    if (this.ParentMenuAdv == null || this.ParentMenuAdv.Orientation != Orientation.Vertical || this.Items.Count <= 0)
      return;
    this.ExtendButtonVisibility = Visibility.Visible;
  }

  internal void CheckGroupName(MenuItemAdv menuitem)
  {
    for (int index1 = 0; index1 < this.Items.Count; ++index1)
    {
      if (this.Items[index1] is MenuItemAdv && this.Items[index1] == menuitem)
      {
        MenuItemAdv menuItemAdv1 = (MenuItemAdv) this.Items[index1];
        if (menuItemAdv1.IsCheckable && menuItemAdv1.CheckIconType == CheckIconType.RadioButton && !menuItemAdv1.GroupName.Equals(string.Empty))
        {
          for (int index2 = index1 - 1; index2 >= 0; --index2)
          {
            if (this.Items[index2] is MenuItemAdv)
            {
              MenuItemAdv menuItemAdv2 = this.Items[index2] as MenuItemAdv;
              if (menuItemAdv2.IsCheckable && menuItemAdv2.CheckIconType == CheckIconType.RadioButton && !menuItemAdv2.GroupName.Equals(string.Empty) && menuItemAdv1.GroupName.Equals(menuItemAdv2.GroupName) && menuItemAdv2.IsChecked)
                menuItemAdv1.IsChecked = false;
            }
          }
        }
      }
    }
  }

  private void UnCheck(MenuItemAdv menuitem)
  {
    if (this.ParentMenuItemAdv != null)
    {
      for (int index = 0; index < this.ParentMenuItemAdv.Items.Count; ++index)
      {
        if (this.ParentMenuItemAdv.Items[index] is MenuItemAdv)
        {
          MenuItemAdv menuItemAdv = (MenuItemAdv) this.ParentMenuItemAdv.Items[index];
          if (menuItemAdv != null && menuItemAdv != menuitem && !menuItemAdv.GroupName.Equals(string.Empty) && menuItemAdv.IsCheckable && menuItemAdv.CheckIconType == CheckIconType.RadioButton && menuItemAdv.GroupName.Equals(menuitem.GroupName))
            menuItemAdv.IsChecked = false;
        }
      }
    }
    else
    {
      for (int index = 0; index < this.ParentMenuItemAdv.Items.Count; ++index)
      {
        if (this.ParentMenuItemAdv.Items[index] is MenuItemAdv)
        {
          MenuItemAdv menuItemAdv = (MenuItemAdv) this.ParentMenuItemAdv.Items[index];
          if (menuItemAdv != null && menuItemAdv != menuitem && !menuItemAdv.GroupName.Equals(string.Empty) && menuItemAdv.IsCheckable && menuItemAdv.CheckIconType == CheckIconType.RadioButton)
            menuItemAdv.IsChecked = false;
        }
      }
    }
  }

  internal StackPanel GetStackPanel()
  {
    DependencyObject reference = (DependencyObject) this;
    while (true)
    {
      switch (reference)
      {
        case StackPanel _:
        case null:
          goto label_3;
        default:
          reference = VisualTreeHelper.GetParent(reference);
          continue;
      }
    }
label_3:
    return reference != null ? reference as StackPanel : (StackPanel) null;
  }

  [Description("Represents the Command of the MenuItem")]
  [Category("Common Properties")]
  public ICommand Command
  {
    get => (ICommand) this.GetValue(MenuItemAdv.CommandProperty);
    set => this.SetValue(MenuItemAdv.CommandProperty, (object) value);
  }

  [Description("Represents the Command Paramenter which has to be passed with the Command to execute it.")]
  [Category("Common Properties")]
  public object CommandParameter
  {
    get => this.GetValue(MenuItemAdv.CommandParameterProperty);
    set => this.SetValue(MenuItemAdv.CommandParameterProperty, value);
  }

  public IInputElement CommandTarget
  {
    get => (IInputElement) this.GetValue(MenuItemAdv.CommandTargetProperty);
    set => this.SetValue(MenuItemAdv.CommandTargetProperty, (object) value);
  }

  private static void CommandChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
  {
    ((MenuItemAdv) obj).OnCommandChanged((ICommand) e.OldValue, (ICommand) e.NewValue);
  }

  private void OnCommandChanged(ICommand oldCommand, ICommand newCommand)
  {
    if (oldCommand != null)
      this.UnhookCommand(oldCommand);
    if (newCommand == null)
      return;
    this.HookCommand(newCommand);
  }

  private void UnhookCommand(ICommand command)
  {
    EventHandler executeChangedHandler = this.CanExecuteChangedHandler;
    if (executeChangedHandler != null)
      command.CanExecuteChanged -= executeChangedHandler;
    this.UpdateCanExecute();
  }

  private void HookCommand(ICommand command)
  {
    EventHandler eventHandler = new EventHandler(this.OnCanExecuteChanged);
    this.CanExecuteChangedHandler = eventHandler;
    command.CanExecuteChanged += eventHandler;
    this.UpdateCanExecute();
  }

  private void OnCanExecuteChanged(object sender, EventArgs e) => this.UpdateCanExecute();

  private void UpdateCanExecute()
  {
    if (this.Command == null || !this.IsEnabled)
      return;
    if (ItemsControl.ItemsControlFromItemContainer((DependencyObject) this) is MenuItemAdv menuItemAdv)
      menuItemAdv._isExecute = true;
    if (menuItemAdv != null && !menuItemAdv.IsSubMenuOpen || this.Parent != null && this.Parent.IsExecuteonLostFocus || MenuItemAdv.CanExecuteCommandSource(this))
      return;
    Syncfusion.Windows.VisualStateManager.GoToState((Control) this, "Disabled", false);
  }

  private void UpdateCanExecute(MenuItemAdv menu)
  {
    if (menu.Command != null && menu.IsEnabled)
    {
      if (!(ItemsControl.ItemsControlFromItemContainer((DependencyObject) menu) is MenuItemAdv menuItemAdv) || menuItemAdv.IsSubMenuOpen)
      {
        if (!MenuItemAdv.CanExecuteCommandSource(menu))
          Syncfusion.Windows.VisualStateManager.GoToState((Control) menu, "Disabled", false);
        else
          Syncfusion.Windows.VisualStateManager.GoToState((Control) menu, "Normal", false);
      }
      else
        Syncfusion.Windows.VisualStateManager.GoToState((Control) menu, "Normal", false);
    }
    else
      Syncfusion.Windows.VisualStateManager.GoToState((Control) menu, "Normal", false);
  }

  private static bool CanExecuteCommandSource(MenuItemAdv commandSource)
  {
    ICommand command = commandSource.Command;
    if (command == null)
      return false;
    object commandParameter = commandSource.CommandParameter;
    IInputElement target = commandSource.CommandTarget;
    if (!(command is RoutedCommand routedCommand))
      return command.CanExecute(commandParameter);
    if (target == null)
      target = (IInputElement) commandSource;
    return routedCommand.CanExecute(commandParameter, target);
  }
}
