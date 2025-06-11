// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.TabControl
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools.Extension;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_OverflowButton", Type = typeof (ContextMenuToggleButton))]
[TemplatePart(Name = "PART_HeaderPanel", Type = typeof (TabPanel))]
[TemplatePart(Name = "PART_OverflowScrollviewer", Type = typeof (ScrollViewer))]
[TemplatePart(Name = "PART_ScrollButtonLeft", Type = typeof (ButtonBase))]
[TemplatePart(Name = "PART_ScrollButtonRight", Type = typeof (ButtonBase))]
[TemplatePart(Name = "PART_HeaderBorder", Type = typeof (Border))]
public class TabControl : System.Windows.Controls.TabControl
{
  private const string OverflowButtonKey = "PART_OverflowButton";
  private const string HeaderPanelKey = "PART_HeaderPanel";
  private const string OverflowScrollviewer = "PART_OverflowScrollviewer";
  private const string ScrollButtonLeft = "PART_ScrollButtonLeft";
  private const string ScrollButtonRight = "PART_ScrollButtonRight";
  private const string HeaderBorder = "PART_HeaderBorder";
  private ContextMenuToggleButton _buttonOverflow;
  private ScrollViewer _scrollViewerOverflow;
  private ButtonBase _buttonScrollLeft;
  private ButtonBase _buttonScrollRight;
  private Border _headerBorder;
  internal bool IsInternalAction;
  public static readonly DependencyProperty IsAnimationEnabledProperty = DependencyProperty.Register(nameof (IsAnimationEnabled), typeof (bool), typeof (TabControl), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty IsDraggableProperty = DependencyProperty.Register(nameof (IsDraggable), typeof (bool), typeof (TabControl), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty ShowCloseButtonProperty = DependencyProperty.RegisterAttached(nameof (ShowCloseButton), typeof (bool), typeof (TabControl), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty ShowContextMenuProperty = DependencyProperty.RegisterAttached(nameof (ShowContextMenu), typeof (bool), typeof (TabControl), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty CanBeClosedByMiddleButtonProperty = DependencyProperty.Register(nameof (CanBeClosedByMiddleButton), typeof (bool), typeof (TabControl), new PropertyMetadata(ValueBoxes.TrueBox));
  public static readonly DependencyProperty IsTabFillEnabledProperty = DependencyProperty.Register(nameof (IsTabFillEnabled), typeof (bool), typeof (TabControl), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty TabItemWidthProperty = DependencyProperty.Register(nameof (TabItemWidth), typeof (double), typeof (TabControl), new PropertyMetadata((object) 200.0));
  public static readonly DependencyProperty TabItemHeightProperty = DependencyProperty.Register(nameof (TabItemHeight), typeof (double), typeof (TabControl), new PropertyMetadata((object) 30.0));
  public static readonly DependencyProperty IsScrollableProperty = DependencyProperty.Register(nameof (IsScrollable), typeof (bool), typeof (TabControl), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty ShowOverflowButtonProperty = DependencyProperty.Register(nameof (ShowOverflowButton), typeof (bool), typeof (TabControl), new PropertyMetadata(ValueBoxes.TrueBox));
  public static readonly DependencyProperty ShowScrollButtonProperty = DependencyProperty.Register(nameof (ShowScrollButton), typeof (bool), typeof (TabControl), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty OverflowMenuDisplayMemberPathProperty = DependencyProperty.Register(nameof (OverflowMenuDisplayMemberPath), typeof (string), typeof (TabControl), new PropertyMetadata((object) null));
  private int _itemShowCount;

  internal TabPanel HeaderPanel { get; private set; }

  public bool IsAnimationEnabled
  {
    get => (bool) this.GetValue(TabControl.IsAnimationEnabledProperty);
    set => this.SetValue(TabControl.IsAnimationEnabledProperty, ValueBoxes.BooleanBox(value));
  }

  public bool IsDraggable
  {
    get => (bool) this.GetValue(TabControl.IsDraggableProperty);
    set => this.SetValue(TabControl.IsDraggableProperty, ValueBoxes.BooleanBox(value));
  }

  public static void SetShowCloseButton(DependencyObject element, bool value)
  {
    element.SetValue(TabControl.ShowCloseButtonProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetShowCloseButton(DependencyObject element)
  {
    return (bool) element.GetValue(TabControl.ShowCloseButtonProperty);
  }

  public bool ShowCloseButton
  {
    get => (bool) this.GetValue(TabControl.ShowCloseButtonProperty);
    set => this.SetValue(TabControl.ShowCloseButtonProperty, ValueBoxes.BooleanBox(value));
  }

  public static void SetShowContextMenu(DependencyObject element, bool value)
  {
    element.SetValue(TabControl.ShowContextMenuProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetShowContextMenu(DependencyObject element)
  {
    return (bool) element.GetValue(TabControl.ShowContextMenuProperty);
  }

  public bool ShowContextMenu
  {
    get => (bool) this.GetValue(TabControl.ShowContextMenuProperty);
    set => this.SetValue(TabControl.ShowContextMenuProperty, ValueBoxes.BooleanBox(value));
  }

  public bool CanBeClosedByMiddleButton
  {
    get => (bool) this.GetValue(TabControl.CanBeClosedByMiddleButtonProperty);
    set
    {
      this.SetValue(TabControl.CanBeClosedByMiddleButtonProperty, ValueBoxes.BooleanBox(value));
    }
  }

  public bool IsTabFillEnabled
  {
    get => (bool) this.GetValue(TabControl.IsTabFillEnabledProperty);
    set => this.SetValue(TabControl.IsTabFillEnabledProperty, ValueBoxes.BooleanBox(value));
  }

  public double TabItemWidth
  {
    get => (double) this.GetValue(TabControl.TabItemWidthProperty);
    set => this.SetValue(TabControl.TabItemWidthProperty, (object) value);
  }

  public double TabItemHeight
  {
    get => (double) this.GetValue(TabControl.TabItemHeightProperty);
    set => this.SetValue(TabControl.TabItemHeightProperty, (object) value);
  }

  public bool IsScrollable
  {
    get => (bool) this.GetValue(TabControl.IsScrollableProperty);
    set => this.SetValue(TabControl.IsScrollableProperty, ValueBoxes.BooleanBox(value));
  }

  public bool ShowOverflowButton
  {
    get => (bool) this.GetValue(TabControl.ShowOverflowButtonProperty);
    set => this.SetValue(TabControl.ShowOverflowButtonProperty, ValueBoxes.BooleanBox(value));
  }

  public bool ShowScrollButton
  {
    get => (bool) this.GetValue(TabControl.ShowScrollButtonProperty);
    set => this.SetValue(TabControl.ShowScrollButtonProperty, ValueBoxes.BooleanBox(value));
  }

  public string OverflowMenuDisplayMemberPath
  {
    get => (string) this.GetValue(TabControl.OverflowMenuDisplayMemberPathProperty);
    set => this.SetValue(TabControl.OverflowMenuDisplayMemberPathProperty, (object) value);
  }

  protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
  {
    base.OnItemsChanged(e);
    if (this.HeaderPanel == null)
    {
      this.IsInternalAction = false;
    }
    else
    {
      this.UpdateOverflowButton();
      if (this.IsInternalAction)
      {
        this.IsInternalAction = false;
      }
      else
      {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
          for (int index = 0; index < this.Items.Count; ++index)
          {
            if (!(this.ItemContainerGenerator.ContainerFromIndex(index) is TabItem tabItem))
              return;
            tabItem.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            tabItem.TabPanel = this.HeaderPanel;
          }
        }
        this._headerBorder?.InvalidateMeasure();
        this.IsInternalAction = false;
      }
    }
  }

  public override void OnApplyTemplate()
  {
    if (this._buttonOverflow != null)
    {
      if (this._buttonOverflow.Menu != null)
      {
        this._buttonOverflow.Menu.Closed -= new RoutedEventHandler(this.Menu_Closed);
        this._buttonOverflow.Menu = (ContextMenu) null;
      }
      this._buttonOverflow.Click -= new RoutedEventHandler(this.ButtonOverflow_Click);
    }
    if (this._buttonScrollLeft != null)
      this._buttonScrollLeft.Click -= new RoutedEventHandler(this.ButtonScrollLeft_Click);
    if (this._buttonScrollRight != null)
      this._buttonScrollRight.Click -= new RoutedEventHandler(this.ButtonScrollRight_Click);
    base.OnApplyTemplate();
    this.HeaderPanel = this.GetTemplateChild("PART_HeaderPanel") as TabPanel;
    if (this.IsTabFillEnabled)
      return;
    this._buttonOverflow = this.GetTemplateChild("PART_OverflowButton") as ContextMenuToggleButton;
    this._scrollViewerOverflow = this.GetTemplateChild("PART_OverflowScrollviewer") as ScrollViewer;
    this._buttonScrollLeft = this.GetTemplateChild("PART_ScrollButtonLeft") as ButtonBase;
    this._buttonScrollRight = this.GetTemplateChild("PART_ScrollButtonRight") as ButtonBase;
    this._headerBorder = this.GetTemplateChild("PART_HeaderBorder") as Border;
    if (this._buttonScrollLeft != null)
      this._buttonScrollLeft.Click += new RoutedEventHandler(this.ButtonScrollLeft_Click);
    if (this._buttonScrollRight != null)
      this._buttonScrollRight.Click += new RoutedEventHandler(this.ButtonScrollRight_Click);
    if (this._buttonOverflow == null)
      return;
    ContextMenu contextMenu = new ContextMenu()
    {
      Placement = PlacementMode.Bottom,
      PlacementTarget = (UIElement) this._buttonOverflow
    };
    contextMenu.Closed += new RoutedEventHandler(this.Menu_Closed);
    this._buttonOverflow.Menu = contextMenu;
    this._buttonOverflow.Click += new RoutedEventHandler(this.ButtonOverflow_Click);
  }

  protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
  {
    base.OnRenderSizeChanged(sizeInfo);
    this.UpdateOverflowButton();
  }

  private void UpdateOverflowButton()
  {
    if (this.IsTabFillEnabled)
      return;
    double actualWidth = this.ActualWidth;
    double tabItemWidth = this.TabItemWidth;
    this._itemShowCount = (int) (actualWidth / tabItemWidth);
    ContextMenuToggleButton buttonOverflow = this._buttonOverflow;
    if (buttonOverflow == null)
      return;
    buttonOverflow.Show(this.Items.Count > 0 && (double) this.Items.Count * tabItemWidth >= actualWidth && this.ShowOverflowButton);
  }

  private void Menu_Closed(object sender, RoutedEventArgs e)
  {
    this._buttonOverflow.IsChecked = new bool?(false);
  }

  private void ButtonScrollRight_Click(object sender, RoutedEventArgs e)
  {
    this._scrollViewerOverflow.ScrollToHorizontalOffsetWithAnimation(Math.Min(this._scrollViewerOverflow.CurrentHorizontalOffset + this.TabItemWidth, this._scrollViewerOverflow.ScrollableWidth));
  }

  private void ButtonScrollLeft_Click(object sender, RoutedEventArgs e)
  {
    this._scrollViewerOverflow.ScrollToHorizontalOffsetWithAnimation(Math.Max(this._scrollViewerOverflow.CurrentHorizontalOffset - this.TabItemWidth, 0.0));
  }

  private void ButtonOverflow_Click(object sender, RoutedEventArgs e)
  {
    bool? isChecked = this._buttonOverflow.IsChecked;
    bool flag = true;
    if (!(isChecked.GetValueOrDefault() == flag & isChecked.HasValue))
      return;
    this._buttonOverflow.Menu.Items.Clear();
    for (int index = 0; index < this.Items.Count; ++index)
    {
      TabItem item = this.ItemContainerGenerator.ContainerFromIndex(index) as TabItem;
      if (item != null)
      {
        MenuItem menuItem = new MenuItem();
        menuItem.HeaderStringFormat = this.ItemStringFormat;
        menuItem.IsChecked = item.IsSelected;
        menuItem.IsCheckable = true;
        menuItem.IsEnabled = item.IsEnabled;
        MenuItem newItem = menuItem;
        if (item.DataContext != null)
        {
          if (this.ItemTemplate == null)
            newItem.SetBinding(HeaderedItemsControl.HeaderProperty, (BindingBase) new Binding(this.DisplayMemberPath)
            {
              Source = item.DataContext
            });
          else
            newItem.SetBinding(HeaderedItemsControl.HeaderProperty, (BindingBase) new Binding(this.OverflowMenuDisplayMemberPath)
            {
              Source = item.DataContext
            });
        }
        else
          newItem.SetBinding(HeaderedItemsControl.HeaderProperty, (BindingBase) new Binding(HeaderedItemsControl.HeaderProperty.Name)
          {
            Source = (object) item
          });
        newItem.Click += (RoutedEventHandler) ((_param1, _param2) =>
        {
          this._buttonOverflow.IsChecked = new bool?(false);
          IList actualList = this.GetActualList();
          if (actualList == null)
            return;
          object obj = this.ItemContainerGenerator.ItemFromContainer((DependencyObject) item);
          if (obj == null)
            return;
          if (actualList.IndexOf(obj) >= this._itemShowCount)
          {
            actualList.Remove(obj);
            actualList.Insert(0, obj);
            this.HeaderPanel.SetValue(TabPanel.FluidMoveDurationPropertyKey, (object) (this.IsAnimationEnabled ? new Duration(TimeSpan.FromMilliseconds(200.0)) : new Duration(TimeSpan.FromMilliseconds(0.0))));
            this.HeaderPanel.ForceUpdate = true;
            this.HeaderPanel.Measure(new Size(this.HeaderPanel.DesiredSize.Width, this.ActualHeight));
            this.HeaderPanel.ForceUpdate = false;
            this.SetCurrentValue(Selector.SelectedIndexProperty, ValueBoxes.Int0Box);
          }
          item.IsSelected = true;
        });
        this._buttonOverflow.Menu.Items.Add((object) newItem);
      }
    }
  }

  internal double GetHorizontalOffset()
  {
    ScrollViewer scrollViewerOverflow = this._scrollViewerOverflow;
    return scrollViewerOverflow == null ? 0.0 : scrollViewerOverflow.CurrentHorizontalOffset;
  }

  internal void UpdateScroll()
  {
    ScrollViewer scrollViewerOverflow = this._scrollViewerOverflow;
    if (scrollViewerOverflow == null)
      return;
    MouseWheelEventArgs e = new MouseWheelEventArgs(Mouse.PrimaryDevice, Environment.TickCount, 0);
    e.RoutedEvent = UIElement.MouseWheelEvent;
    // ISSUE: explicit non-virtual call
    __nonvirtual (scrollViewerOverflow.RaiseEvent((RoutedEventArgs) e));
  }

  internal void CloseAllItems() => this.CloseOtherItems((TabItem) null);

  internal void CloseOtherItems(TabItem currentItem)
  {
    object objB = currentItem != null ? this.ItemContainerGenerator.ItemFromContainer((DependencyObject) currentItem) : (object) null;
    IList actualList = this.GetActualList();
    if (actualList == null)
      return;
    this.IsInternalAction = true;
    for (int index = 0; index < this.Items.Count; ++index)
    {
      object obj = actualList[index];
      if (!object.Equals(obj, objB) && obj != null)
      {
        CancelRoutedEventArgs e = new CancelRoutedEventArgs(TabItem.ClosingEvent, obj);
        if (this.ItemContainerGenerator.ContainerFromItem(obj) is TabItem tabItem)
        {
          tabItem.RaiseEvent((RoutedEventArgs) e);
          if (!e.Cancel)
          {
            tabItem.RaiseEvent(new RoutedEventArgs(TabItem.ClosedEvent, obj));
            actualList.Remove(obj);
            --index;
          }
        }
      }
    }
    this.SetCurrentValue(Selector.SelectedIndexProperty, (object) (this.Items.Count == 0 ? -1 : 0));
  }

  internal IList GetActualList()
  {
    return this.ItemsSource == null ? (IList) this.Items : this.ItemsSource as IList;
  }

  protected override bool IsItemItsOwnContainerOverride(object item) => item is TabItem;

  protected override DependencyObject GetContainerForItemOverride()
  {
    return (DependencyObject) new TabItem();
  }
}
