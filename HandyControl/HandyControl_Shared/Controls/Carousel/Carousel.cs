// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.Carousel
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.Windows.Threading;

#nullable disable
namespace HandyControl.Controls;

[DefaultProperty("Items")]
[ContentProperty("Items")]
[TemplatePart(Name = "PART_PanelPage", Type = typeof (Panel))]
public class Carousel : SimpleItemsControl, IDisposable
{
  private const string ElementPanelPage = "PART_PanelPage";
  private bool _isDisposed;
  private Panel _panelPage;
  private bool _appliedTemplate;
  private int _pageIndex = -1;
  private RadioButton _selectedButton;
  private DispatcherTimer _updateTimer;
  private readonly List<double> _widthList = new List<double>();
  private readonly Dictionary<object, CarouselItem> _entryDic = new Dictionary<object, CarouselItem>();
  private bool _isRefresh;
  private IEnumerable _itemsSourceInternal;
  public static readonly DependencyProperty AutoRunProperty = DependencyProperty.Register(nameof (AutoRun), typeof (bool), typeof (Carousel), new PropertyMetadata(ValueBoxes.FalseBox, (PropertyChangedCallback) ((o, args) => ((Carousel) o).TimerSwitch((bool) args.NewValue))));
  public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register(nameof (Interval), typeof (TimeSpan), typeof (Carousel), new PropertyMetadata((object) TimeSpan.FromSeconds(2.0)));
  public static readonly DependencyProperty ExtendWidthProperty = DependencyProperty.Register(nameof (ExtendWidth), typeof (double), typeof (Carousel), new PropertyMetadata(ValueBoxes.Double0Box));
  public static readonly DependencyProperty IsCenterProperty = DependencyProperty.Register(nameof (IsCenter), typeof (bool), typeof (Carousel), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty PageButtonStyleProperty = DependencyProperty.Register(nameof (PageButtonStyle), typeof (Style), typeof (Carousel), new PropertyMetadata((object) null));

  public override void OnApplyTemplate()
  {
    this._appliedTemplate = false;
    this._panelPage?.RemoveHandler(ButtonBase.ClickEvent, (Delegate) new RoutedEventHandler(this.ButtonPages_OnClick));
    base.OnApplyTemplate();
    this._panelPage = this.GetTemplateChild("PART_PanelPage") as Panel;
    if (!this.CheckNull())
      return;
    this._panelPage.AddHandler(ButtonBase.ClickEvent, (Delegate) new RoutedEventHandler(this.ButtonPages_OnClick));
    this._appliedTemplate = true;
    this.Update();
  }

  private void Update()
  {
    this.TimerSwitch(this.AutoRun);
    this.UpdatePageButtons(this._pageIndex);
  }

  private bool CheckNull() => this._panelPage != null;

  public double ExtendWidth
  {
    get => (double) this.GetValue(Carousel.ExtendWidthProperty);
    set => this.SetValue(Carousel.ExtendWidthProperty, (object) value);
  }

  public bool IsCenter
  {
    get => (bool) this.GetValue(Carousel.IsCenterProperty);
    set => this.SetValue(Carousel.IsCenterProperty, ValueBoxes.BooleanBox(value));
  }

  public Style PageButtonStyle
  {
    get => (Style) this.GetValue(Carousel.PageButtonStyleProperty);
    set => this.SetValue(Carousel.PageButtonStyleProperty, (object) value);
  }

  public Carousel()
  {
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Prev, new ExecutedRoutedEventHandler(this.ButtonPrev_OnClick)));
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Next, new ExecutedRoutedEventHandler(this.ButtonNext_OnClick)));
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Selected, new ExecutedRoutedEventHandler(this.ButtonPages_OnClick)));
    this.Loaded += (RoutedEventHandler) ((s, e) => this.UpdatePageButtons());
    this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.Carousel_IsVisibleChanged);
  }

  ~Carousel() => this.Dispose();

  public void Dispose()
  {
    if (this._isDisposed)
      return;
    this.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(this.Carousel_IsVisibleChanged);
    this._updateTimer?.Stop();
    this._isDisposed = true;
    GC.SuppressFinalize((object) this);
  }

  private void Carousel_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
  {
    if (this._updateTimer == null)
      return;
    if (this.IsVisible)
    {
      this._updateTimer.Tick += new EventHandler(this.UpdateTimer_Tick);
      this._updateTimer.Start();
    }
    else
    {
      this._updateTimer.Stop();
      this._updateTimer.Tick -= new EventHandler(this.UpdateTimer_Tick);
    }
  }

  public bool AutoRun
  {
    get => (bool) this.GetValue(Carousel.AutoRunProperty);
    set => this.SetValue(Carousel.AutoRunProperty, ValueBoxes.BooleanBox(value));
  }

  public TimeSpan Interval
  {
    get => (TimeSpan) this.GetValue(Carousel.IntervalProperty);
    set => this.SetValue(Carousel.IntervalProperty, (object) value);
  }

  public int PageIndex
  {
    get => this._pageIndex;
    set
    {
      if (this.Items.Count == 0 || this._pageIndex == value)
        return;
      this._pageIndex = value >= 0 ? (value < this.Items.Count ? value : 0) : this.Items.Count - 1;
      this.UpdatePageButtons(this._pageIndex);
    }
  }

  private void TimerSwitch(bool run)
  {
    if (!this._appliedTemplate)
      return;
    if (this._updateTimer != null)
    {
      this._updateTimer.Tick -= new EventHandler(this.UpdateTimer_Tick);
      this._updateTimer.Stop();
      this._updateTimer = (DispatcherTimer) null;
    }
    if (!run)
      return;
    this._updateTimer = new DispatcherTimer()
    {
      Interval = this.Interval
    };
    this._updateTimer.Tick += new EventHandler(this.UpdateTimer_Tick);
    this._updateTimer.Start();
  }

  private void UpdateTimer_Tick(object sender, EventArgs e)
  {
    if (this.IsMouseOver)
      return;
    ++this.PageIndex;
  }

  public void UpdatePageButtons(int index = -1)
  {
    if (!this.CheckNull() || !this._appliedTemplate)
      return;
    int count = this.Items.Count;
    this._widthList.Clear();
    this._widthList.Add(0.0);
    double num = 0.0;
    foreach (FrameworkElement child in this.ItemsHost.Children)
    {
      child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
      num += child.DesiredSize.Width;
      this._widthList.Add(num);
    }
    this.ItemsHost.Width = this._widthList.Last<double>() + this.ExtendWidth;
    this._panelPage.Children.Clear();
    for (int index1 = 0; index1 < count; ++index1)
    {
      UIElementCollection children = this._panelPage.Children;
      RadioButton element = new RadioButton();
      element.Style = this.PageButtonStyle;
      children.Add((UIElement) element);
    }
    if (index == -1 && count > 0)
      index = 0;
    if (index < 0 || index >= count || !(this._panelPage.Children[index] is RadioButton child1))
      return;
    child1.IsChecked = new bool?(true);
    child1.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent, (object) child1));
    this.UpdateItemsPosition();
  }

  private void UpdateItemsPosition()
  {
    if (!this.CheckNull() || !this._appliedTemplate || this.Items.Count == 0)
      return;
    if (!this.IsCenter)
    {
      this.ItemsHost.BeginAnimation(FrameworkElement.MarginProperty, (AnimationTimeline) AnimationHelper.CreateAnimation(new Thickness(-this._widthList[this.PageIndex], 0.0, 0.0, 0.0)));
    }
    else
    {
      double width = this.ItemsHost.Children[this.PageIndex].DesiredSize.Width;
      this.ItemsHost.BeginAnimation(FrameworkElement.MarginProperty, (AnimationTimeline) AnimationHelper.CreateAnimation(new Thickness(-this._widthList[this.PageIndex] + (this.ActualWidth - width) / 2.0, 0.0, 0.0, 0.0)));
    }
  }

  protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
  {
    base.OnRenderSizeChanged(sizeInfo);
    this.UpdateItemsPosition();
  }

  private void ButtonPages_OnClick(object sender, RoutedEventArgs e)
  {
    if (!this.CheckNull())
      return;
    this._selectedButton = e.OriginalSource as RadioButton;
    int num = this._panelPage.Children.IndexOf((UIElement) this._selectedButton);
    if (num == -1)
      return;
    this.PageIndex = num;
  }

  private void ButtonPrev_OnClick(object sender, RoutedEventArgs e) => --this.PageIndex;

  private void ButtonNext_OnClick(object sender, RoutedEventArgs e) => ++this.PageIndex;

  protected override DependencyObject GetContainerForItemOverride()
  {
    return (DependencyObject) new CarouselItem();
  }

  protected override bool IsItemItsOwnContainerOverride(object item) => item is CarouselItem;

  private void ClearItems()
  {
    this.ItemsHost?.Children.Clear();
    this._entryDic.Clear();
  }

  private void RemoveItem(object item)
  {
    CarouselItem element;
    if (!this._entryDic.TryGetValue(item, out element))
      return;
    this.ItemsHost.Children.Remove((UIElement) element);
    this.Items.Remove(item);
    this._entryDic.Remove(item);
  }

  protected override void Refresh()
  {
    if (this.ItemsHost == null)
      return;
    this._entryDic.Clear();
    this._isRefresh = true;
    foreach (object obj in this.Items)
      this.AddItem(obj);
    this._isRefresh = false;
  }

  private void AddItem(object item) => this.InsertItem(this._entryDic.Count, item);

  private void InsertItem(int index, object item)
  {
    if (this.ItemsHost == null)
    {
      this.Items.Insert(index, item);
      this._entryDic.Add(item, (CarouselItem) null);
    }
    else
    {
      DependencyObject element1;
      if (this.IsItemItsOwnContainerOverride(item))
      {
        element1 = item as DependencyObject;
      }
      else
      {
        element1 = this.GetContainerForItemOverride();
        this.PrepareContainerForItemOverride(element1, item);
      }
      if (!(element1 is CarouselItem element2))
        return;
      element2.Style = this.ItemContainerStyle;
      this._entryDic[item] = element2;
      this.ItemsHost.Children.Insert(index, (UIElement) element2);
      if (!this.IsLoaded || this._isRefresh || this._itemsSourceInternal == null)
        return;
      this.Items.Insert(index, item);
    }
  }

  protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    if (this._itemsSourceInternal != null)
    {
      if (this._itemsSourceInternal is INotifyCollectionChanged itemsSourceInternal)
        itemsSourceInternal.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.InternalCollectionChanged);
      this.Items.Clear();
      this.ClearItems();
    }
    this._itemsSourceInternal = newValue;
    if (this._itemsSourceInternal == null)
      return;
    if (this._itemsSourceInternal is INotifyCollectionChanged itemsSourceInternal1)
      itemsSourceInternal1.CollectionChanged += new NotifyCollectionChangedEventHandler(this.InternalCollectionChanged);
    foreach (object obj in this._itemsSourceInternal)
      this.AddItem(obj);
  }

  private void InternalCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    if (this.ItemsHost == null)
      return;
    if (e.Action == NotifyCollectionChangedAction.Reset)
    {
      if (this._entryDic.Count == 0)
        return;
      this.ClearItems();
      this.Items.Clear();
    }
    else
    {
      if (e.OldItems != null)
      {
        foreach (object oldItem in (IEnumerable) e.OldItems)
          this.RemoveItem(oldItem);
      }
      if (e.NewItems == null)
        return;
      int num = 0;
      foreach (object newItem in (IEnumerable) e.NewItems)
        this.InsertItem(e.NewStartingIndex + num++, newItem);
    }
  }

  protected override void OnItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    if (this._itemsSourceInternal != null)
      return;
    this.InternalCollectionChanged(sender, e);
  }

  protected override void OnItemTemplateChanged(DependencyPropertyChangedEventArgs e)
  {
  }

  protected override void OnItemContainerStyleChanged(DependencyPropertyChangedEventArgs e)
  {
  }
}
