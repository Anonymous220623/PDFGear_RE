// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PdfPagePreviewGridView
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using pdfeditor.Models.Thumbnails;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Controls;

public partial class PdfPagePreviewGridView : PdfPagePreviewListView
{
  private DispatcherTimer dragTimer;
  private bool itemDraging;
  private bool dragingContinuousRange;
  private object dragStartItem;
  private int dragStartItemIdx = -1;
  private object dragEndItem;
  private int dragEndItemIdx = -1;
  private Point lastLeftBtnDownPos;
  private DateTime lastLeftBtnDownTime;
  private Point lastLeftBtnClickPos;
  private DateTime lastClickTime;
  private Border Bd;
  private ScrollViewer ScrollViewer;
  private ContentPresenter DragInfo;
  private FrameworkElement InsertPlaceholder;
  private Rect leftBounds;
  private Rect rightBounds;
  private object leftItem;
  private int leftItemIdx;
  private object rightItem;
  private int rightItemIdx;

  static PdfPagePreviewGridView()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (PdfPagePreviewGridView), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (PdfPagePreviewGridView)));
  }

  public PdfPagePreviewGridView()
  {
    this.Loaded += new RoutedEventHandler(this.PdfPagePreviewGridView_Loaded);
    this.Unloaded += new RoutedEventHandler(this.PdfPagePreviewGridView_Unloaded);
    this.dragTimer = new DispatcherTimer()
    {
      Interval = TimeSpan.FromMilliseconds(100.0)
    };
    this.dragTimer.Tick += new EventHandler(this.DragTimer_Tick);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    if (this.DragInfo != null)
      this.DragInfo.SizeChanged -= new SizeChangedEventHandler(this.DragInfo_SizeChanged);
    this.Bd = this.GetTemplateChild("Bd") as Border;
    this.ScrollViewer = this.GetTemplateChild("ScrollViewer") as ScrollViewer;
    this.DragInfo = this.GetTemplateChild("DragInfo") as ContentPresenter;
    this.InsertPlaceholder = this.GetTemplateChild("InsertPlaceholder") as FrameworkElement;
    if (this.DragInfo != null)
      this.DragInfo.SizeChanged += new SizeChangedEventHandler(this.DragInfo_SizeChanged);
    VisualStateManager.GoToState((FrameworkElement) this, "NotDraging", true);
  }

  protected override void OnMouseDown(MouseButtonEventArgs e)
  {
    base.OnMouseDown(e);
    if (e.OriginalSource != this.Bd && !(e.OriginalSource is ScrollViewer))
      return;
    this.UnselectAll();
    if (!(this.GetTemplateChild("ScrollViewer") is ScrollViewer templateChild) || !(templateChild.Content is ItemsPresenter content) || VisualTreeHelper.GetChildrenCount((DependencyObject) content) <= 0 || !(VisualTreeHelper.GetChild((DependencyObject) content, 0) is Panel child))
      return;
    foreach (FrameworkElement control in child.Children.OfType<FrameworkElement>())
      VisualStateManager.GoToState(control, "FocusBorderInvisible", true);
  }

  private void PdfPagePreviewGridView_Loaded(object sender, RoutedEventArgs e)
  {
    if (this.IsVisible)
    {
      if (this.ItemsSource is PdfPageEditList itemsSource && itemsSource.SelectedItems.Count > 0)
        this.ScrollIntoView((object) itemsSource.SelectedItems.First<PdfPageEditListModel>());
    }
    else
      this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.PdfPagePreviewGridView_IsVisibleChanged);
    if (DesignerProperties.GetIsInDesignMode((DependencyObject) this))
      return;
    StrongReferenceMessenger.Default.Unregister<ValueChangedMessage<(int, int)>, string>((object) this, "MESSAGE_PAGE_EDITOR_SELECT_INDEX");
    StrongReferenceMessenger.Default.Register<ValueChangedMessage<(int, int)>, string>((object) this, "MESSAGE_PAGE_EDITOR_SELECT_INDEX", new MessageHandler<object, ValueChangedMessage<(int, int)>>(this.OnSelectIndexChangeNotified));
  }

  private void DragInfo_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    if (!(((UIElement) sender).RenderTransform is TranslateTransform translateTransform1))
    {
      translateTransform1 = new TranslateTransform();
      ((UIElement) sender).RenderTransform = (Transform) translateTransform1;
    }
    TranslateTransform translateTransform2 = translateTransform1;
    Size newSize = e.NewSize;
    double num1 = -newSize.Width / 2.0;
    translateTransform2.X = num1;
    TranslateTransform translateTransform3 = translateTransform1;
    newSize = e.NewSize;
    double num2 = -newSize.Height / 2.0;
    translateTransform3.Y = num2;
  }

  private void PdfPagePreviewGridView_Unloaded(object sender, RoutedEventArgs e)
  {
    if (DesignerProperties.GetIsInDesignMode((DependencyObject) this))
      return;
    StrongReferenceMessenger.Default.Unregister<ValueChangedMessage<(int, int)>, string>((object) this, "MESSAGE_PAGE_EDITOR_SELECT_INDEX");
  }

  private void OnSelectIndexChangeNotified(
    object recipient,
    ValueChangedMessage<(int startPage, int endPage)> message)
  {
    (int num, int endPage) = message.Value;
    IList list = this.ItemsSource as IList;
    if (list == null || num < 0 || num >= list.Count || endPage < 0 || endPage >= list.Count)
      return;
    this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) (() =>
    {
      if (!this.IsLoaded)
        return;
      for (int index = 0; index < list.Count; ++index)
        ((Collection<PdfPageEditListModel>) list)[index].Selected = index >= num && index <= endPage;
      this.ScrollIntoView(list[num]);
    }));
  }

  private void PdfPagePreviewGridView_IsVisibleChanged(
    object sender,
    DependencyPropertyChangedEventArgs e)
  {
    if (!this.IsVisible)
      return;
    this.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(this.PdfPagePreviewGridView_IsVisibleChanged);
    if (this.SelectedItem == null)
      return;
    this.ScrollIntoView(this.SelectedItem);
  }

  public new void UnselectAll()
  {
    base.UnselectAll();
    if (!(this.ItemsSource is PdfPageEditList itemsSource))
      return;
    itemsSource.AllItemSelected = new bool?(false);
  }

  public new void SelectAll()
  {
    base.SelectAll();
    if (!(this.ItemsSource is PdfPageEditList itemsSource))
      return;
    itemsSource.AllItemSelected = new bool?(true);
  }

  protected override double ViewportThreshold => 40.0;

  protected override DependencyObject GetContainerForItemOverride()
  {
    return (DependencyObject) new PdfPagePreviewGridViewItem();
  }

  protected override bool IsItemItsOwnContainerOverride(object item)
  {
    return item is PdfPagePreviewGridViewItem;
  }

  protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
  {
    base.PrepareContainerForItemOverride(element, item);
    if (!(element is PdfPagePreviewGridViewItem control))
      return;
    this.SelectedItems.Remove(item);
    Binding binding = new Binding("Selected")
    {
      Mode = BindingMode.TwoWay
    };
    control.SetBinding(ListBoxItem.IsSelectedProperty, (BindingBase) binding);
    VisualStateManager.GoToState((FrameworkElement) control, "FocusBorderInvisible", true);
  }

  protected override void ClearContainerForItemOverride(DependencyObject element, object item)
  {
    base.ClearContainerForItemOverride(element, item);
    if (!(element is PdfPagePreviewGridViewItem previewGridViewItem))
      return;
    BindingOperations.ClearBinding((DependencyObject) previewGridViewItem, ListBoxItem.IsSelectedProperty);
    VisualStateManager.GoToState((FrameworkElement) previewGridViewItem, "FocusBorderInvisible", true);
  }

  internal void OnItemsDragStart(PdfPagePreviewGridViewItem dragContainer)
  {
    if (Mouse.LeftButton != MouseButtonState.Pressed)
      return;
    this.itemDraging = true;
    if (!this.itemDraging)
      return;
    bool flag = true;
    PdfPageEditListModel[] array = (this.ItemsSource as PdfPageEditList).SelectedItems.ToArray<PdfPageEditListModel>();
    if (array.Length == this.Items.Count)
    {
      this.itemDraging = false;
      this.ReleaseMouseCapture();
    }
    PdfPagePreviewGridViewItem.draging = this.itemDraging;
    if (!this.itemDraging)
      return;
    if (array.Length == 1)
    {
      this.dragStartItem = (object) array[0];
      this.dragEndItem = (object) array[0];
      this.dragStartItemIdx = this.Items.IndexOf(this.dragStartItem);
      this.dragEndItemIdx = this.dragStartItemIdx;
    }
    else
    {
      object obj1 = (object) null;
      int num1 = -1;
      object obj2 = (object) null;
      int num2 = -1;
      HashSet<object> objectSet = new HashSet<object>((IEnumerable<object>) ((IEnumerable<PdfPageEditListModel>) array).Distinct<PdfPageEditListModel>());
      for (int index = 0; index < this.Items.Count; ++index)
      {
        object obj3 = this.Items[index];
        if (objectSet.Contains(obj3))
        {
          if (num1 == -1 || index < num1)
          {
            num1 = index;
            obj1 = obj3;
          }
          if (index > num2)
          {
            num2 = index;
            obj2 = obj3;
          }
        }
      }
      if (num2 - num1 + 1 != objectSet.Count)
        flag = false;
      this.dragStartItem = obj1;
      this.dragStartItemIdx = num1;
      this.dragEndItem = obj2;
      this.dragEndItemIdx = num2;
    }
    this.dragingContinuousRange = flag;
    this.UpdateDragPosition(Mouse.GetPosition((IInputElement) this));
    VisualStateManager.GoToState((FrameworkElement) this, "Draging", true);
    PdfPagePreviewGridViewItemDragStartEventArgs e = new PdfPagePreviewGridViewItemDragStartEventArgs((FrameworkElement) dragContainer, (object[]) array);
    EventHandler<PdfPagePreviewGridViewItemDragStartEventArgs> itemsDragStart = this.ItemsDragStart;
    if (itemsDragStart != null)
      itemsDragStart((object) this, e);
    this.DragInfo.Content = e.UIOverride;
  }

  private void OnItemsDragCompleted(bool cancel)
  {
    VisualStateManager.GoToState((FrameworkElement) this, "NotDraging", true);
    this.itemDraging = false;
    this.dragTimer.Stop();
    if (this.DragInfo != null)
      this.DragInfo.Content = (object) null;
    PdfPagePreviewGridViewItem.draging = false;
    object beforeItem = this.leftItem;
    object afterItem = this.rightItem;
    PdfPageEditListModel[] dragItems = (this.ItemsSource as PdfPageEditList).SelectedItems.ToArray<PdfPageEditListModel>();
    int leftItemIdx = this.leftItemIdx;
    int rightItemIdx = this.rightItemIdx;
    bool dragingContinuousRange = this.dragingContinuousRange;
    int dragStartItemIdx = this.dragStartItemIdx;
    int dragEndItemIdx = this.dragEndItemIdx;
    this.leftItem = (object) null;
    this.leftItemIdx = -1;
    this.leftBounds = Rect.Empty;
    this.rightItem = (object) null;
    this.rightItemIdx = -1;
    this.rightBounds = Rect.Empty;
    this.dragingContinuousRange = false;
    this.dragStartItem = (object) null;
    this.dragEndItem = (object) null;
    this.dragStartItemIdx = -1;
    this.dragEndItemIdx = -1;
    bool reordered = true;
    if (cancel)
    {
      beforeItem = (object) null;
      afterItem = (object) null;
      dragItems = (PdfPageEditListModel[]) null;
      dragingContinuousRange = false;
      reordered = false;
    }
    else if (leftItemIdx == -1 && rightItemIdx == -1)
      reordered = false;
    else if (dragingContinuousRange && (leftItemIdx != -1 && leftItemIdx >= dragStartItemIdx && leftItemIdx <= dragEndItemIdx || rightItemIdx != -1 && rightItemIdx >= dragStartItemIdx && rightItemIdx <= dragEndItemIdx))
      reordered = false;
    PdfPagePreviewGridViewItemDragCompletedEventArgs e = new PdfPagePreviewGridViewItemDragCompletedEventArgs(beforeItem, afterItem, (object[]) dragItems, dragingContinuousRange, reordered);
    EventHandler<PdfPagePreviewGridViewItemDragCompletedEventArgs> itemsDragCompleted = this.ItemsDragCompleted;
    if (itemsDragCompleted == null)
      return;
    itemsDragCompleted((object) this, e);
  }

  protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    base.OnPreviewMouseLeftButtonDown(e);
    if (!(this.ScrollViewer?.Content is FrameworkElement content) || VisualTreeHelper.GetChildrenCount((DependencyObject) content) <= 0 || !(VisualTreeHelper.GetChild((DependencyObject) content, 0) is Panel child) || !child.IsMouseOver)
      return;
    this.lastLeftBtnDownPos = e.GetPosition((IInputElement) this);
    this.lastLeftBtnDownTime = DateTime.UtcNow;
  }

  protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    base.OnPreviewMouseLeftButtonUp(e);
    this.ReleaseMouseCapture();
    Point lastLeftBtnDownPos = this.lastLeftBtnDownPos;
    DateTime lastLeftBtnDownTime = this.lastLeftBtnDownTime;
    EventHandler<PdfPagePreviewGridViewItemEventArgs> itemClick1 = this.ItemClick;
    EventHandler<PdfPagePreviewGridViewItemEventArgs> itemDoubleClick1 = this.ItemDoubleClick;
    this.lastLeftBtnDownPos = new Point();
    this.lastLeftBtnDownTime = new DateTime();
    Point position = e.GetPosition((IInputElement) this);
    DateTime utcNow = DateTime.UtcNow;
    Point lastLeftBtnClickPos = this.lastLeftBtnClickPos;
    if (itemClick1 == null && itemDoubleClick1 == null)
      return;
    TimeSpan timeSpan = utcNow - lastLeftBtnDownTime;
    if (timeSpan.TotalMilliseconds >= 300.0 || Math.Abs(lastLeftBtnDownPos.X - position.X) >= 10.0 || Math.Abs(lastLeftBtnDownPos.Y - position.Y) >= 10.0)
      return;
    Panel panel = (Panel) null;
    if (this.ScrollViewer?.Content is FrameworkElement content && VisualTreeHelper.GetChildrenCount((DependencyObject) content) > 0)
      panel = VisualTreeHelper.GetChild((DependencyObject) content, 0) as Panel;
    if (panel != null)
    {
      FrameworkElement[] array = panel.Children.OfType<FrameworkElement>().ToArray<FrameworkElement>();
      for (int index = 0; index < array.Length; ++index)
      {
        Rect layoutSlot = LayoutInformation.GetLayoutSlot(array[index]);
        if (!layoutSlot.IsEmpty && layoutSlot.Contains(position))
        {
          timeSpan = utcNow - this.lastClickTime;
          if (timeSpan.TotalMilliseconds < 500.0 && Math.Abs(position.X - lastLeftBtnClickPos.X) < 10.0 && Math.Abs(position.Y - lastLeftBtnClickPos.Y) < 10.0)
          {
            this.lastLeftBtnClickPos = new Point();
            this.lastClickTime = new DateTime();
            EventHandler<PdfPagePreviewGridViewItemEventArgs> itemDoubleClick2 = this.ItemDoubleClick;
            if (itemDoubleClick2 == null)
              return;
            itemDoubleClick2((object) this, new PdfPagePreviewGridViewItemEventArgs(array[index].DataContext));
            return;
          }
          this.lastLeftBtnClickPos = position;
          this.lastClickTime = utcNow;
          EventHandler<PdfPagePreviewGridViewItemEventArgs> itemClick2 = this.ItemClick;
          if (itemClick2 == null)
            return;
          itemClick2((object) this, new PdfPagePreviewGridViewItemEventArgs(array[index].DataContext));
          return;
        }
      }
    }
    this.lastClickTime = new DateTime();
  }

  protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
  {
    base.OnPreviewMouseRightButtonDown(e);
    this.OnItemsDragCompleted(true);
    this.ReleaseMouseCapture();
  }

  protected override void OnPreviewKeyDown(KeyEventArgs e)
  {
    base.OnPreviewKeyDown(e);
    if (e.Key != Key.Escape || !this.itemDraging)
      return;
    e.Handled = true;
    this.OnItemsDragCompleted(true);
    this.ReleaseMouseCapture();
  }

  protected override void OnLostMouseCapture(MouseEventArgs e)
  {
    base.OnLostMouseCapture(e);
    if (e.Source != this || !this.itemDraging)
      return;
    Point position = Mouse.GetPosition((IInputElement) this);
    this.OnItemsDragCompleted(position.X <= 0.0 || position.X >= this.ActualWidth || position.Y <= 0.0 || position.Y >= this.ActualHeight);
  }

  protected override void OnPreviewMouseMove(MouseEventArgs e)
  {
    base.OnPreviewMouseMove(e);
    if (Mouse.LeftButton != MouseButtonState.Pressed || !this.itemDraging)
      return;
    Point position = e.GetPosition((IInputElement) this);
    this.UpdateDragPosition(position);
    if (position.Y < 40.0 || position.Y > this.ActualHeight - 40.0)
    {
      if (this.dragTimer.IsEnabled)
        return;
      this.dragTimer.Start();
    }
    else
      this.dragTimer.Stop();
  }

  private void UpdateDragPosition(Point mousePos)
  {
    Panel panel = (Panel) null;
    if (this.ScrollViewer.Content is FrameworkElement content && VisualTreeHelper.GetChildrenCount((DependencyObject) content) > 0)
      panel = VisualTreeHelper.GetChild((DependencyObject) content, 0) as Panel;
    FrameworkElement[] array = panel.Children.OfType<FrameworkElement>().ToArray<FrameworkElement>();
    Rect? nullable1 = new Rect?();
    for (int index = 0; index < array.Length; ++index)
    {
      Rect layoutSlot = LayoutInformation.GetLayoutSlot(array[index]);
      if (layoutSlot.IsEmpty)
      {
        this.leftBounds = Rect.Empty;
        this.rightBounds = Rect.Empty;
        this.leftItem = (object) null;
        this.rightItem = (object) null;
        break;
      }
      Point point = new Point(layoutSlot.X + layoutSlot.Width / 2.0, layoutSlot.Y + layoutSlot.Height / 2.0);
      if (layoutSlot.Contains(mousePos))
      {
        if (mousePos.X < point.X)
        {
          if (nullable1.HasValue && nullable1.Value.Top + nullable1.Value.Height / 2.0 > layoutSlot.Top)
          {
            this.leftItem = array[index - 1].DataContext;
            this.leftBounds = nullable1.Value;
          }
          else
          {
            this.leftItem = (object) null;
            this.leftBounds = Rect.Empty;
          }
          this.rightItem = array[index].DataContext;
          this.rightBounds = layoutSlot;
          break;
        }
        Rect? nullable2 = new Rect?();
        if (index < array.Length - 1)
        {
          nullable2 = new Rect?(LayoutInformation.GetLayoutSlot(array[index + 1]));
          if (nullable2.Value.IsEmpty)
            nullable2 = new Rect?();
        }
        if (nullable2.HasValue && layoutSlot.Top + layoutSlot.Height / 2.0 > nullable2.Value.Top)
        {
          this.rightItem = array[index + 1].DataContext;
          this.rightBounds = nullable2.Value;
        }
        else
        {
          this.rightItem = (object) null;
          this.rightBounds = Rect.Empty;
        }
        this.leftItem = array[index].DataContext;
        this.leftBounds = layoutSlot;
        break;
      }
      nullable1 = new Rect?(layoutSlot);
    }
    this.leftItemIdx = this.leftItem != null ? this.Items.IndexOf(this.leftItem) : -1;
    this.rightItemIdx = this.rightItem != null ? this.Items.IndexOf(this.rightItem) : -1;
    if (this.DragInfo != null)
    {
      Canvas.SetLeft((UIElement) this.DragInfo, mousePos.X);
      Canvas.SetTop((UIElement) this.DragInfo, mousePos.Y);
    }
    if (this.InsertPlaceholder == null)
      return;
    if (this.leftItem == null && this.rightItem == null)
      this.InsertPlaceholder.Visibility = Visibility.Collapsed;
    else if (this.dragingContinuousRange)
    {
      if (this.leftItemIdx != -1 && this.leftItemIdx >= this.dragStartItemIdx && this.leftItemIdx <= this.dragEndItemIdx || this.rightItemIdx != -1 && this.rightItemIdx >= this.dragStartItemIdx && this.rightItemIdx <= this.dragEndItemIdx)
        this.InsertPlaceholder.Visibility = Visibility.Collapsed;
      else
        this.InsertPlaceholder.Visibility = Visibility.Visible;
    }
    else
      this.InsertPlaceholder.Visibility = Visibility.Visible;
    Rect rect = this.leftBounds;
    if (rect.IsEmpty)
      rect = this.rightBounds;
    this.InsertPlaceholder.Height = Math.Max(50.0, rect.Height - 50.0);
    double length = rect.Top + (rect.Height - this.InsertPlaceholder.Height) / 2.0;
    Canvas.SetLeft((UIElement) this.InsertPlaceholder, this.leftItem == null || this.rightItem == null ? (this.leftItem == null ? this.rightBounds.Left - 4.0 : this.leftBounds.Right + 4.0) : this.leftBounds.Right + (this.rightBounds.Left - this.leftBounds.Right) / 2.0 + 6.0);
    Canvas.SetTop((UIElement) this.InsertPlaceholder, length);
  }

  private void DragTimer_Tick(object sender, EventArgs e)
  {
    ScrollViewer scrollViewer = this.ScrollViewer;
    if (scrollViewer == null)
    {
      this.dragTimer.Stop();
    }
    else
    {
      Point p = Mouse.GetPosition((IInputElement) this);
      if (p.Y < 40.0)
      {
        double num = 300.0 * (40.0 - Math.Max(0.0, p.Y)) / 40.0;
        double offset = scrollViewer.VerticalOffset - num;
        scrollViewer.ScrollToVerticalOffset(offset);
        scrollViewer.UpdateLayout();
        this.Dispatcher.BeginInvoke(DispatcherPriority.Render, (Delegate) (() => this.UpdateDragPosition(p)));
      }
      else if (p.Y > this.ActualHeight - 40.0)
      {
        double num = 300.0 * (40.0 - Math.Max(0.0, this.ActualHeight - p.Y)) / 40.0;
        double offset = scrollViewer.VerticalOffset + num;
        scrollViewer.ScrollToVerticalOffset(offset);
        scrollViewer.UpdateLayout();
        this.Dispatcher.BeginInvoke(DispatcherPriority.Render, (Delegate) (() => this.UpdateDragPosition(p)));
      }
      else
        this.dragTimer.Stop();
    }
  }

  public event EventHandler<PdfPagePreviewGridViewItemDragStartEventArgs> ItemsDragStart;

  public event EventHandler<PdfPagePreviewGridViewItemDragCompletedEventArgs> ItemsDragCompleted;

  public event EventHandler<PdfPagePreviewGridViewItemEventArgs> ItemClick;

  public event EventHandler<PdfPagePreviewGridViewItemEventArgs> ItemDoubleClick;
}
