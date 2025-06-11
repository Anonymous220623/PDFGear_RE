// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.Ribbon
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools.Extension;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_TabHeaderItemsControl", Type = typeof (ItemsControl))]
[TemplatePart(Name = "PART_RootPanel", Type = typeof (Panel))]
[TemplatePart(Name = "PART_ContentPanel", Type = typeof (Panel))]
public class Ribbon : Selector
{
  private const string TabHeaderItemsControl = "PART_TabHeaderItemsControl";
  private const string RootPanel = "PART_RootPanel";
  private const string ContentPanel = "PART_ContentPanel";
  private ItemsControl _tabHeaderItemsControl;
  private Panel _rootPanel;
  private Panel _contentPanel;
  private System.Windows.Window _window;
  private readonly ObservableCollection<object> _tabHeaderItemsSource = new ObservableCollection<object>();
  internal static readonly DependencyProperty RibbonProperty = DependencyProperty.RegisterAttached(nameof (Ribbon), typeof (Ribbon), typeof (Ribbon), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(nameof (IsDropDownOpen), typeof (bool), typeof (Ribbon), new PropertyMetadata(ValueBoxes.TrueBox, new PropertyChangedCallback(Ribbon.OnIsDropDownOpenChanged)));
  public static readonly DependencyProperty IsMinimizedProperty = DependencyProperty.Register(nameof (IsMinimized), typeof (bool), typeof (Ribbon), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty ContentHeightProperty = DependencyProperty.Register(nameof (ContentHeight), typeof (double), typeof (Ribbon), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty PrefixContentProperty = DependencyProperty.Register(nameof (PrefixContent), typeof (object), typeof (Ribbon), new PropertyMetadata((object) null));
  public static readonly DependencyProperty PostfixContentProperty = DependencyProperty.Register(nameof (PostfixContent), typeof (object), typeof (Ribbon), new PropertyMetadata((object) null));

  internal ItemsControl RibbonTabHeaderItemsControl => this._tabHeaderItemsControl;

  public Ribbon()
  {
    Ribbon.SetRibbon((DependencyObject) this, this);
    this.Loaded += new RoutedEventHandler(this.Ribbon_Loaded);
    this.Unloaded += new RoutedEventHandler(this.Ribbon_Unloaded);
    this.ItemContainerGenerator.StatusChanged += new EventHandler(this.OnItemContainerGeneratorStatusChanged);
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Switch, new ExecutedRoutedEventHandler(this.IsMinimizedSwitchButton_OnClick)));
  }

  internal static void SetRibbon(DependencyObject element, Ribbon value)
  {
    element.SetValue(Ribbon.RibbonProperty, (object) value);
  }

  internal static Ribbon GetRibbon(DependencyObject element)
  {
    return (Ribbon) element.GetValue(Ribbon.RibbonProperty);
  }

  private static void OnIsDropDownOpenChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Ribbon) d).OnIsDropDownOpenChanged((bool) e.NewValue);
  }

  private void OnIsDropDownOpenChanged(bool isDropDownOpen)
  {
    if (this._contentPanel == null)
      return;
    this.SwitchCurrentTabContentVisibility(isDropDownOpen);
    this._contentPanel.Show(isDropDownOpen);
  }

  public bool IsDropDownOpen
  {
    get => (bool) this.GetValue(Ribbon.IsDropDownOpenProperty);
    set => this.SetValue(Ribbon.IsDropDownOpenProperty, (object) value);
  }

  public bool IsMinimized
  {
    get => (bool) this.GetValue(Ribbon.IsMinimizedProperty);
    set => this.SetValue(Ribbon.IsMinimizedProperty, (object) value);
  }

  public double ContentHeight
  {
    get => (double) this.GetValue(Ribbon.ContentHeightProperty);
    set => this.SetValue(Ribbon.ContentHeightProperty, (object) value);
  }

  public object PrefixContent
  {
    get => this.GetValue(Ribbon.PrefixContentProperty);
    set => this.SetValue(Ribbon.PrefixContentProperty, value);
  }

  public object PostfixContent
  {
    get => this.GetValue(Ribbon.PostfixContentProperty);
    set => this.SetValue(Ribbon.PostfixContentProperty, value);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this._tabHeaderItemsControl = this.GetTemplateChild("PART_TabHeaderItemsControl") as ItemsControl;
    ItemsControl headerItemsControl = this._tabHeaderItemsControl;
    if (headerItemsControl != null && headerItemsControl.ItemsSource == null)
      this._tabHeaderItemsControl.ItemsSource = (IEnumerable) this._tabHeaderItemsSource;
    this._rootPanel = this.GetTemplateChild("PART_RootPanel") as Panel;
    this._contentPanel = this.GetTemplateChild("PART_ContentPanel") as Panel;
    if (this.IsMinimized)
      this._rootPanel.SetCurrentValue(FrameworkElement.HeightProperty, (object) this._tabHeaderItemsControl.ActualHeight);
    if (this.IsDropDownOpen)
      return;
    this._contentPanel.SetCurrentValue(FrameworkElement.HeightProperty, (object) 0.0);
  }

  internal void ResetSelection()
  {
    this.SelectedIndex = -1;
    this.InitializeSelection();
  }

  internal void NotifyMouseClickedOnTabHeader(RibbonTabHeader tabHeader, MouseButtonEventArgs e)
  {
    if (this._tabHeaderItemsControl == null)
      return;
    int num = this._tabHeaderItemsControl.ItemContainerGenerator.IndexFromContainer((DependencyObject) tabHeader);
    if (e.ClickCount == 1)
    {
      int selectedIndex = this.SelectedIndex;
      if (selectedIndex < 0 || selectedIndex != num)
      {
        this.SelectedIndex = num;
        if (!this.IsMinimized)
          return;
        this.IsDropDownOpen = true;
      }
      else
      {
        if (!this.IsMinimized)
          return;
        this.IsDropDownOpen = !this.IsDropDownOpen;
        if (this.IsDropDownOpen)
          return;
        this.SelectedIndex = -1;
      }
    }
    else
    {
      if (e.ClickCount != 2)
        return;
      this.IsMinimized = !this.IsMinimized;
      this.IsDropDownOpen = !this.IsMinimized;
      if (this.IsMinimized && !this.IsDropDownOpen)
        this.SelectedIndex = -1;
      else
        this.SelectedIndex = num;
    }
  }

  internal void NotifyTabHeaderChanged()
  {
    if (this.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
      return;
    this.RefreshHeaderCollection();
  }

  protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
  {
    base.OnItemsChanged(e);
    if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace || e.Action == NotifyCollectionChangedAction.Reset)
      this.InitializeSelection();
    if ((this.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated || e.Action != NotifyCollectionChangedAction.Move) && e.Action != NotifyCollectionChangedAction.Remove)
      return;
    this.RefreshHeaderCollection();
  }

  protected override DependencyObject GetContainerForItemOverride()
  {
    return (DependencyObject) new RibbonTab();
  }

  protected override bool IsItemItsOwnContainerOverride(object item) => item is RibbonTab;

  protected override void OnSelectionChanged(SelectionChangedEventArgs e)
  {
    base.OnSelectionChanged(e);
    IList addedItems = e.AddedItems;
    if (addedItems == null || addedItems.Count <= 0)
      return;
    this.SelectedItem = e.AddedItems[0];
  }

  private void OnPreviewMouseButton(MouseButtonEventArgs e)
  {
    if (this.InputHitTest(e.GetPosition((IInputElement) this)) != null || !this.IsMinimized || !this.IsDropDownOpen)
      return;
    this.IsDropDownOpen = false;
    this.SelectedIndex = -1;
  }

  private void Ribbon_Loaded(object sender, RoutedEventArgs e)
  {
    this._window = System.Windows.Window.GetWindow((DependencyObject) this);
    if (this._window == null)
      return;
    this._window.Deactivated += new EventHandler(this.Window_Deactivated);
    this._window.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.Window_PreviewMouseLeftButtonDown);
    this._window.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(this.Window_PreviewMouseLeftButtonUp);
    this._window.PreviewMouseRightButtonDown += new MouseButtonEventHandler(this.Window_PreviewMouseRightButtonDown);
    this._window.PreviewMouseRightButtonUp += new MouseButtonEventHandler(this.Window_PreviewMouseRightButtonUp);
  }

  private void Ribbon_Unloaded(object sender, RoutedEventArgs e)
  {
    if (this._window == null)
      return;
    this._window.Deactivated -= new EventHandler(this.Window_Deactivated);
  }

  private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    this.OnPreviewMouseButton(e);
  }

  private void Window_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    this.OnPreviewMouseButton(e);
  }

  private void Window_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
  {
    this.OnPreviewMouseButton(e);
  }

  private void Window_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
  {
    this.OnPreviewMouseButton(e);
  }

  private void Window_Deactivated(object sender, EventArgs e)
  {
    if (!this.IsMinimized || !this.IsDropDownOpen)
      return;
    this.IsDropDownOpen = false;
    this.SelectedIndex = -1;
  }

  private void IsMinimizedSwitchButton_OnClick(object sender, ExecutedRoutedEventArgs e)
  {
    this.IsDropDownOpen = !this.IsMinimized;
    if (this.IsDropDownOpen)
      return;
    this.SelectedIndex = -1;
  }

  private void OnItemContainerGeneratorStatusChanged(object sender, EventArgs e)
  {
    if (this.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
      return;
    this.InitializeSelection();
    this.RefreshHeaderCollection();
  }

  private int GetFirstVisibleTabIndex()
  {
    int count = this.Items.Count;
    for (int index = 0; index < count; ++index)
    {
      if (this.ItemContainerGenerator.ContainerFromIndex(index) is RibbonTab ribbonTab && ribbonTab.IsVisible)
        return index;
    }
    return -1;
  }

  private void SwitchCurrentTabContentVisibility(bool isVisible)
  {
    this.GetCurrentTab()?.SwitchContentVisibility(isVisible);
  }

  private RibbonTab GetCurrentTab()
  {
    int selectedIndex = this.SelectedIndex;
    if (selectedIndex == -1)
      return (RibbonTab) null;
    return this.ItemContainerGenerator.ContainerFromIndex(selectedIndex) is RibbonTab ribbonTab ? ribbonTab : (RibbonTab) null;
  }

  private void InitializeSelection()
  {
    if (!this.IsDropDownOpen)
    {
      this.SelectedIndex = -1;
    }
    else
    {
      if (this.SelectedIndex >= 0 || this.Items.Count <= 0)
        return;
      int firstVisibleTabIndex = this.GetFirstVisibleTabIndex();
      if (firstVisibleTabIndex < 0)
        return;
      this.SelectedIndex = firstVisibleTabIndex;
    }
  }

  private void RefreshHeaderCollection()
  {
    int count1 = this.Items.Count;
    for (int index = 0; index < count1; ++index)
    {
      object obj = (object) null;
      if (this.ItemContainerGenerator.ContainerFromIndex(index) is RibbonTab ribbonTab)
        obj = ribbonTab.Header;
      if (obj == null)
        obj = (object) string.Empty;
      if (index >= this._tabHeaderItemsSource.Count)
        this._tabHeaderItemsSource.Add(obj);
      else
        this._tabHeaderItemsSource[index] = obj;
    }
    int count2 = this._tabHeaderItemsSource.Count;
    for (int index = 0; index < count2 - count1; ++index)
      this._tabHeaderItemsSource.RemoveAt(count1);
  }
}
