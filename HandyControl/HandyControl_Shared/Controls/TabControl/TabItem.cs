// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.TabItem
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools;
using HandyControl.Tools.Extension;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace HandyControl.Controls;

public class TabItem : System.Windows.Controls.TabItem
{
  private const int AnimationSpeed = 150;
  private static bool ItemIsDragging;
  private bool _isWaiting;
  private Point _dragPoint;
  private int _mouseDownIndex;
  private double _mouseDownOffsetX;
  private Point _mouseDownPoint;
  private double _maxMoveRight;
  private double _maxMoveLeft;
  private const double WaitLength = 20.0;
  private bool _isDragging;
  private bool _isDragged;
  private int _currentIndex;
  private double _scrollHorizontalOffset;
  private TabPanel _tabPanel;
  public static readonly DependencyProperty ShowCloseButtonProperty = TabControl.ShowCloseButtonProperty.AddOwner(typeof (TabItem));
  public static readonly DependencyProperty ShowContextMenuProperty = TabControl.ShowContextMenuProperty.AddOwner(typeof (TabItem), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(TabItem.OnShowContextMenuChanged)));
  public static readonly DependencyProperty MenuProperty = DependencyProperty.Register(nameof (Menu), typeof (ContextMenu), typeof (TabItem), new PropertyMetadata((object) null, new PropertyChangedCallback(TabItem.OnMenuChanged)));
  public static readonly RoutedEvent ClosingEvent = EventManager.RegisterRoutedEvent("Closing", RoutingStrategy.Bubble, typeof (EventHandler), typeof (TabItem));
  public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent("Closed", RoutingStrategy.Bubble, typeof (EventHandler), typeof (TabItem));

  public double ItemWidth { get; internal set; }

  internal double TargetOffsetX { get; set; }

  internal TabPanel TabPanel
  {
    get
    {
      if (this._tabPanel == null && this.TabControlParent != null)
        this._tabPanel = this.TabControlParent.HeaderPanel;
      return this._tabPanel;
    }
    set => this._tabPanel = value;
  }

  internal int CurrentIndex
  {
    get => this._currentIndex;
    set
    {
      if (this._currentIndex == value || value < 0)
        return;
      int currentIndex = this._currentIndex;
      this._currentIndex = value;
      this.UpdateItemOffsetX(currentIndex);
    }
  }

  public bool ShowCloseButton
  {
    get => (bool) this.GetValue(TabItem.ShowCloseButtonProperty);
    set => this.SetValue(TabItem.ShowCloseButtonProperty, ValueBoxes.BooleanBox(value));
  }

  public static void SetShowCloseButton(DependencyObject element, bool value)
  {
    element.SetValue(TabItem.ShowCloseButtonProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetShowCloseButton(DependencyObject element)
  {
    return (bool) element.GetValue(TabItem.ShowCloseButtonProperty);
  }

  private static void OnShowContextMenuChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    TabItem tabItem = (TabItem) d;
    if (tabItem.Menu == null)
      return;
    bool newValue = (bool) e.NewValue;
    tabItem.Menu.IsEnabled = newValue;
    tabItem.Menu.Show(newValue);
  }

  public bool ShowContextMenu
  {
    get => (bool) this.GetValue(TabItem.ShowContextMenuProperty);
    set => this.SetValue(TabItem.ShowContextMenuProperty, ValueBoxes.BooleanBox(value));
  }

  public static void SetShowContextMenu(DependencyObject element, bool value)
  {
    element.SetValue(TabItem.ShowContextMenuProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetShowContextMenu(DependencyObject element)
  {
    return (bool) element.GetValue(TabItem.ShowContextMenuProperty);
  }

  private static void OnMenuChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((TabItem) d).OnMenuChanged(e.NewValue as ContextMenu);
  }

  private void OnMenuChanged(ContextMenu menu)
  {
    if (!this.IsLoaded || menu == null)
      return;
    TabControl tabControlParent = this.TabControlParent;
    if (tabControlParent == null)
      return;
    object obj = tabControlParent.ItemContainerGenerator.ItemFromContainer((DependencyObject) this);
    menu.DataContext = obj;
    menu.SetBinding(UIElement.IsEnabledProperty, (BindingBase) new Binding(TabItem.ShowContextMenuProperty.Name)
    {
      Source = (object) this
    });
    menu.SetBinding(UIElement.VisibilityProperty, (BindingBase) new Binding(TabItem.ShowContextMenuProperty.Name)
    {
      Source = (object) this,
      Converter = HandyControl.Tools.ResourceHelper.GetResourceInternal<IValueConverter>("Boolean2VisibilityConverter")
    });
  }

  public ContextMenu Menu
  {
    get => (ContextMenu) this.GetValue(TabItem.MenuProperty);
    set => this.SetValue(TabItem.MenuProperty, (object) value);
  }

  private void UpdateItemOffsetX(int oldIndex)
  {
    if (!this._isDragging || this.CurrentIndex >= this.TabPanel.ItemDic.Count)
      return;
    TabItem tabItem = this.TabPanel.ItemDic[this.CurrentIndex];
    tabItem.CurrentIndex -= this.CurrentIndex - oldIndex;
    double targetOffsetX = tabItem.TargetOffsetX;
    double resultX = targetOffsetX + (double) (oldIndex - this.CurrentIndex) * this.ItemWidth;
    this.TabPanel.ItemDic[this.CurrentIndex] = this;
    this.TabPanel.ItemDic[tabItem.CurrentIndex] = tabItem;
    tabItem.CreateAnimation(targetOffsetX, resultX);
  }

  public TabItem()
  {
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Close, (ExecutedRoutedEventHandler) ((s, e) => this.Close())));
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.CloseAll, (ExecutedRoutedEventHandler) ((s, e) => this.TabControlParent.CloseAllItems())));
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.CloseOther, (ExecutedRoutedEventHandler) ((s, e) => this.TabControlParent.CloseOtherItems(this)), (CanExecuteRoutedEventHandler) ((s, e) => e.CanExecute = this.TabControlParent.Items.Count > 1)));
    this.Loaded += (RoutedEventHandler) ((s, e) => this.OnMenuChanged(this.Menu));
  }

  private TabControl TabControlParent
  {
    get => ItemsControl.ItemsControlFromItemContainer((DependencyObject) this) as TabControl;
  }

  protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
  {
    base.OnMouseRightButtonDown(e);
    if (!this.IsMouseOverHeader(e))
      return;
    this.IsSelected = true;
    this.Focus();
  }

  protected override void OnHeaderChanged(object oldHeader, object newHeader)
  {
    base.OnHeaderChanged(oldHeader, newHeader);
    if (this.TabPanel == null)
      return;
    this.TabPanel.ForceUpdate = true;
    this.InvalidateMeasure();
    this.TabPanel.ForceUpdate = true;
  }

  internal void Close()
  {
    TabControl tabControlParent = this.TabControlParent;
    if (tabControlParent == null)
      return;
    object source = tabControlParent.ItemContainerGenerator.ItemFromContainer((DependencyObject) this);
    CancelRoutedEventArgs e = new CancelRoutedEventArgs(TabItem.ClosingEvent, source);
    this.RaiseEvent((RoutedEventArgs) e);
    if (e.Cancel)
      return;
    this.TabPanel.SetValue(TabPanel.FluidMoveDurationPropertyKey, (object) (tabControlParent.IsAnimationEnabled ? new Duration(TimeSpan.FromMilliseconds(200.0)) : new Duration(TimeSpan.FromMilliseconds(1.0))));
    tabControlParent.IsInternalAction = true;
    this.RaiseEvent(new RoutedEventArgs(TabItem.ClosedEvent, source));
    tabControlParent.GetActualList()?.Remove(source);
  }

  protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    base.OnMouseLeftButtonDown(e);
    if (!this.IsMouseOverHeader(e))
      return;
    TabControl tabControlParent = this.TabControlParent;
    if (tabControlParent == null || !tabControlParent.IsDraggable || TabItem.ItemIsDragging || this._isDragging)
      return;
    tabControlParent.UpdateScroll();
    this.TabPanel.SetValue(TabPanel.FluidMoveDurationPropertyKey, (object) new Duration(TimeSpan.FromSeconds(0.0)));
    this._mouseDownOffsetX = this.RenderTransform.Value.OffsetX;
    this._scrollHorizontalOffset = tabControlParent.GetHorizontalOffset();
    this._mouseDownIndex = this.CalLocationIndex(this.TranslatePoint(new Point(), (UIElement) tabControlParent).X + this._scrollHorizontalOffset);
    this._maxMoveLeft = (double) -(this._mouseDownIndex - this.CalLocationIndex(this._scrollHorizontalOffset)) * this.ItemWidth;
    this._maxMoveRight = tabControlParent.ActualWidth - this.ActualWidth + this._maxMoveLeft;
    this._isDragging = true;
    TabItem.ItemIsDragging = true;
    this._isWaiting = true;
    this._dragPoint = e.GetPosition((IInputElement) tabControlParent);
    this._dragPoint = new Point(this._dragPoint.X + this._scrollHorizontalOffset, this._dragPoint.Y);
    this._mouseDownPoint = this._dragPoint;
    this.CaptureMouse();
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    base.OnMouseMove(e);
    if (!TabItem.ItemIsDragging || !this._isDragging)
      return;
    TabControl tabControlParent = this.TabControlParent;
    if (tabControlParent == null)
      return;
    this.CurrentIndex = this.CalLocationIndex(this.TranslatePoint(new Point(), (UIElement) tabControlParent).X + this._scrollHorizontalOffset);
    Point point = e.GetPosition((IInputElement) tabControlParent);
    point = new Point(point.X + this._scrollHorizontalOffset, point.Y);
    double num1 = point.X - this._dragPoint.X;
    double num2 = point.X - this._mouseDownPoint.X;
    if (Math.Abs(num1) <= 20.0 && this._isWaiting)
      return;
    this._isWaiting = false;
    this._isDragged = true;
    double offsetX = num1 + this.RenderTransform.Value.OffsetX;
    if (num2 < this._maxMoveLeft)
      offsetX = this._maxMoveLeft + this._mouseDownOffsetX;
    else if (num2 > this._maxMoveRight)
      offsetX = this._maxMoveRight + this._mouseDownOffsetX;
    this.RenderTransform = (Transform) new TranslateTransform(offsetX, 0.0);
    this._dragPoint = point;
  }

  protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    base.OnMouseLeftButtonUp(e);
    this.ReleaseMouseCapture();
    if (this._isDragged)
    {
      TabControl tabControlParent = this.TabControlParent;
      if (tabControlParent == null)
        return;
      double left = this.TranslatePoint(new Point(), (UIElement) tabControlParent).X + this._scrollHorizontalOffset;
      int index = this.CalLocationIndex(left);
      double num = (double) index * this.ItemWidth;
      double offsetX = this.RenderTransform.Value.OffsetX;
      this.CreateAnimation(offsetX, offsetX - left + num, index);
    }
    this._isDragging = false;
    TabItem.ItemIsDragging = false;
    this._isDragged = false;
  }

  protected override void OnMouseDown(MouseButtonEventArgs e)
  {
    if (e == null || e.ChangedButton != MouseButton.Middle || e.ButtonState != MouseButtonState.Pressed || !this.TabControlParent.CanBeClosedByMiddleButton || !this.IsMouseOverHeader(e) || !this.ShowCloseButton && !this.ShowContextMenu)
      return;
    this.Close();
  }

  internal void CreateAnimation(double offsetX, double resultX, int index = -1)
  {
    TabControl parent = this.TabControlParent;
    this.TargetOffsetX = resultX;
    if (!parent.IsAnimationEnabled)
    {
      AnimationCompleted();
    }
    else
    {
      DoubleAnimation animation = AnimationHelper.CreateAnimation(resultX, 150.0);
      animation.FillBehavior = FillBehavior.Stop;
      animation.Completed += (EventHandler) ((s1, e1) => AnimationCompleted());
      TranslateTransform translateTransform = new TranslateTransform(offsetX, 0.0);
      this.RenderTransform = (Transform) translateTransform;
      translateTransform.BeginAnimation(TranslateTransform.XProperty, (AnimationTimeline) animation, HandoffBehavior.Compose);
    }

    void AnimationCompleted()
    {
      this.RenderTransform = (Transform) new TranslateTransform(resultX, 0.0);
      if (index == -1)
        return;
      IList actualList = parent.GetActualList();
      if (actualList == null)
        return;
      object obj = parent.ItemContainerGenerator.ItemFromContainer((DependencyObject) this);
      if (obj == null)
        return;
      this.TabPanel.CanUpdate = false;
      parent.IsInternalAction = true;
      actualList.Remove(obj);
      parent.IsInternalAction = true;
      actualList.Insert(index, obj);
      this._tabPanel.SetValue(TabPanel.FluidMoveDurationPropertyKey, (object) new Duration(TimeSpan.FromMilliseconds(0.0)));
      this.TabPanel.CanUpdate = true;
      this.TabPanel.ForceUpdate = true;
      this.TabPanel.Measure(new Size(this.TabPanel.DesiredSize.Width, this.ActualHeight));
      this.TabPanel.ForceUpdate = false;
      this.Focus();
      this.IsSelected = true;
      if (this.IsMouseCaptured)
        return;
      parent.SetCurrentValue(Selector.SelectedIndexProperty, (object) this._currentIndex);
    }
  }

  private int CalLocationIndex(double left)
  {
    if (this._isWaiting)
      return this.CurrentIndex;
    int num1 = this.TabControlParent.Items.Count - 1;
    int num2 = (int) (left / this.ItemWidth);
    int num3 = left % this.ItemWidth / this.ItemWidth > 0.5 ? num2 + 1 : num2;
    return num3 <= num1 ? num3 : num1;
  }

  private bool IsMouseOverHeader(MouseButtonEventArgs e)
  {
    return VisualTreeHelper.HitTest((Visual) this, e.GetPosition((IInputElement) this)) != null;
  }

  public event EventHandler Closing
  {
    add => this.AddHandler(TabItem.ClosingEvent, (Delegate) value);
    remove => this.RemoveHandler(TabItem.ClosingEvent, (Delegate) value);
  }

  public event EventHandler Closed
  {
    add => this.AddHandler(TabItem.ClosedEvent, (Delegate) value);
    remove => this.RemoveHandler(TabItem.ClosedEvent, (Delegate) value);
  }
}
