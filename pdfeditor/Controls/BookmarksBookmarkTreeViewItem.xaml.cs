// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Bookmarks.BookmarkTreeViewItem
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf.Net;
using pdfeditor.Models.Bookmarks;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Controls.Bookmarks;

internal sealed partial class BookmarkTreeViewItem : TreeViewItem
{
  private static WeakReference<BookmarkTreeViewItem.DragDropDataModel> draggingModel;
  private ColumnDefinition MarginColumn;
  private Grid LayoutRoot;
  private Border BackgroundBorder;
  private Border ContentBorder;
  private ToggleButton Expander;
  private TextBlock toolTipText;
  private Border toolTipContent;
  public static readonly DependencyProperty IsHighlightedProperty = DependencyProperty.Register(nameof (IsHighlighted), typeof (bool), typeof (BookmarkTreeViewItem), new PropertyMetadata((object) false));
  private DateTime lastClickCollapseExpanderTime;
  private Point? lastMouseDownPosition;

  static BookmarkTreeViewItem()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (BookmarkTreeViewItem), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (BookmarkTreeViewItem)));
    EventManager.RegisterClassHandler(typeof (BookmarkTreeViewItem), FrameworkElement.RequestBringIntoViewEvent, (Delegate) new RequestBringIntoViewEventHandler(BookmarkTreeViewItem.OnRequestBringIntoView));
  }

  public BookmarkTreeViewItem()
  {
    this.DataContextChanged += new DependencyPropertyChangedEventHandler(this.BookmarkTreeViewItem_DataContextChanged);
    this.Loaded += (RoutedEventHandler) ((s, a) =>
    {
      VisualStateManager.GoToState((FrameworkElement) this, "DropIndicatorInvisible", true);
      this.UpdateDraggingState(false);
    });
    Border border = new Border();
    TextBlock textBlock1 = new TextBlock();
    textBlock1.TextWrapping = TextWrapping.Wrap;
    TextBlock textBlock2 = textBlock1;
    this.toolTipText = textBlock1;
    border.Child = (UIElement) textBlock2;
    border.MaxWidth = 320.0;
    this.toolTipContent = border;
  }

  public bool IsHighlighted
  {
    get => (bool) this.GetValue(BookmarkTreeViewItem.IsHighlightedProperty);
    set => this.SetValue(BookmarkTreeViewItem.IsHighlightedProperty, (object) value);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    if (this.ContentBorder != null)
    {
      this.ContentBorder.MouseDown -= new MouseButtonEventHandler(this.Control_MouseDown);
      this.ContentBorder.ToolTipOpening -= new ToolTipEventHandler(this.Control_ToolTipOpening);
      SetToolTip((UIElement) this.ContentBorder, false);
    }
    if (this.BackgroundBorder != null)
    {
      this.BackgroundBorder.MouseDown -= new MouseButtonEventHandler(this.Control_MouseDown);
      this.BackgroundBorder.ToolTipOpening -= new ToolTipEventHandler(this.Control_ToolTipOpening);
      SetToolTip((UIElement) this.BackgroundBorder, false);
    }
    if (this.Expander != null)
      this.Expander.PreviewMouseLeftButtonDown -= new MouseButtonEventHandler(this.Expander_PreviewMouseLeftButtonDown);
    this.LayoutRoot = this.GetTemplateChild("LayoutRoot") as Grid;
    this.MarginColumn = this.GetTemplateChild("MarginColumn") as ColumnDefinition;
    this.BackgroundBorder = this.GetTemplateChild("BackgroundBorder") as Border;
    this.ContentBorder = this.GetTemplateChild("Bd") as Border;
    this.Expander = this.GetTemplateChild("Expander") as ToggleButton;
    if (this.ContentBorder != null)
    {
      this.ContentBorder.MouseDown += new MouseButtonEventHandler(this.Control_MouseDown);
      this.ContentBorder.ToolTipOpening += new ToolTipEventHandler(this.Control_ToolTipOpening);
      SetToolTip((UIElement) this.ContentBorder, true);
    }
    if (this.BackgroundBorder != null)
    {
      this.BackgroundBorder.MouseDown += new MouseButtonEventHandler(this.Control_MouseDown);
      this.BackgroundBorder.ToolTipOpening += new ToolTipEventHandler(this.Control_ToolTipOpening);
      SetToolTip((UIElement) this.BackgroundBorder, true);
    }
    if (this.Expander != null)
      this.Expander.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.Expander_PreviewMouseLeftButtonDown);
    this.UpdateBackgroundSize();
    this.UpdateDraggingState(false);
    VisualStateManager.GoToState((FrameworkElement) this, "DropIndicatorInvisible", false);

    void SetToolTip(UIElement element, bool enable)
    {
      ToolTipService.SetToolTip((DependencyObject) element, enable ? (object) this.toolTipContent : (object) (Border) null);
      ToolTipService.SetPlacement((DependencyObject) element, PlacementMode.Mouse);
    }
  }

  private void UpdateDraggingState(bool useTransitions)
  {
    BookmarkTreeViewItem.DragDropDataModel target;
    if (this.DataContext is BookmarkModel dataContext && BookmarkTreeViewItem.draggingModel != null && BookmarkTreeViewItem.draggingModel.TryGetTarget(out target) && dataContext == target.Bookmark)
      VisualStateManager.GoToState((FrameworkElement) this, "Dragging", useTransitions);
    else
      VisualStateManager.GoToState((FrameworkElement) this, "NotDragging", useTransitions);
  }

  private void Control_MouseDown(object sender, MouseButtonEventArgs e)
  {
    if (e.ChangedButton != MouseButton.Left || e.ClickCount != 2)
      return;
    e.Handled = true;
    if (!(this.DataContext is BookmarkModel dataContext))
      return;
    BookmarkRenameDialog.Create(dataContext).ShowDialog();
  }

  private void Control_ToolTipOpening(object sender, ToolTipEventArgs e)
  {
    e.Handled = true;
    if (!(this.ContentBorder?.Child is FrameworkElement child) || !(this.DataContext is BookmarkModel dataContext))
      return;
    ItemsControl container = (ItemsControl) this;
    do
      ;
    while ((container = ItemsControl.ItemsControlFromItemContainer((DependencyObject) container)) is BookmarkTreeViewItem);
    if (!(container is BookmarkTreeView _parent) || !IsOverflow((FrameworkElement) _parent, child))
      return;
    this.toolTipText.Text = dataContext.Title ?? string.Empty;
    e.Handled = false;

    static bool IsOverflow(FrameworkElement _parent, FrameworkElement _child)
    {
      return !new Rect(0.0, 0.0, _parent.ActualWidth, _parent.ActualHeight).Contains(_child.TransformToVisual((Visual) _parent).TransformBounds(new Rect(0.0, 0.0, _child.ActualWidth, _child.ActualHeight)));
    }
  }

  private void Expander_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    bool? isChecked = ((ToggleButton) sender).IsChecked;
    if (!isChecked.HasValue || !isChecked.GetValueOrDefault())
      return;
    this.lastClickCollapseExpanderTime = DateTime.Now;
  }

  internal bool ShouldUpdatePageIndex
  {
    get => (DateTime.Now - this.lastClickCollapseExpanderTime).TotalMilliseconds > 800.0;
  }

  protected override void OnMouseDown(MouseButtonEventArgs e)
  {
    if (e.ChangedButton == MouseButton.Left && this.CaptureMouse())
    {
      e.Handled = true;
      this.lastMouseDownPosition = new Point?(e.GetPosition((IInputElement) this));
    }
    else
    {
      base.OnMouseDown(e);
      this.lastMouseDownPosition = new Point?();
    }
  }

  protected override void OnMouseUp(MouseButtonEventArgs e)
  {
    base.OnMouseUp(e);
    Point? mouseDownPosition = this.lastMouseDownPosition;
    this.lastMouseDownPosition = new Point?();
    if (!this.IsMouseCaptured)
      return;
    e.Handled = true;
    this.ReleaseMouseCapture();
    Point position = e.GetPosition((IInputElement) this);
    if (!mouseDownPosition.HasValue || Math.Abs(mouseDownPosition.Value.X - position.X) >= 10.0 || Math.Abs(mouseDownPosition.Value.Y - position.Y) >= 10.0 || !(this.DataContext is BookmarkModel dataContext))
      return;
    if (!dataContext.IsSelected)
      dataContext.IsSelected = true;
    else if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
    {
      if (!dataContext.IsSelected)
        return;
      dataContext.IsSelected = false;
    }
    else
    {
      BookmarkControl parentBookmarkControl = this.ParentBookmarkControl;
      if (parentBookmarkControl?.PdfViewer == null)
        return;
      int? currentIndex = parentBookmarkControl?.PdfViewer?.CurrentIndex;
      int pageIndex = dataContext.PageIndex;
      if (currentIndex.GetValueOrDefault() == pageIndex & currentIndex.HasValue)
        return;
      parentBookmarkControl.UpdateSelectedPageIndex();
    }
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    base.OnMouseMove(e);
    Point? mouseDownPosition = this.lastMouseDownPosition;
    bool? canDragItems = this.ParentTreeView?.CanDragItems;
    if (!canDragItems.HasValue || !canDragItems.GetValueOrDefault() || !(this.DataContext is BookmarkModel) || !mouseDownPosition.HasValue || !this.IsMouseCaptured || (e.LeftButton & MouseButtonState.Pressed) == MouseButtonState.Released)
      return;
    Point position = e.GetPosition((IInputElement) this);
    if (Math.Abs(mouseDownPosition.Value.X - position.X) < 10.0 && Math.Abs(mouseDownPosition.Value.Y - position.Y) < 10.0)
      return;
    e.Handled = true;
    this.DoDragDropOperation(mouseDownPosition.Value, false);
  }

  protected override void OnLostStylusCapture(StylusEventArgs e)
  {
    base.OnLostStylusCapture(e);
    BookmarkTreeViewItem.DragDropDataModel target;
    if (BookmarkTreeViewItem.draggingModel != null && BookmarkTreeViewItem.draggingModel.TryGetTarget(out target) && target.BookmarkControl == null)
      BookmarkTreeViewItem.ResetCachedDraggingModel();
    this.UpdateDraggingState(true);
  }

  protected override void OnStylusSystemGesture(StylusSystemGestureEventArgs e)
  {
    base.OnStylusSystemGesture(e);
    bool? canDragItems = this.ParentTreeView?.CanDragItems;
    if (!canDragItems.HasValue || !canDragItems.GetValueOrDefault())
      return;
    if (e.SystemGesture == SystemGesture.HoldEnter && this.CaptureStylus())
    {
      ScrollViewer parentScrollViewer = this.ParentScrollViewer;
      if (this.ContentBorder != null && parentScrollViewer != null)
      {
        e.Handled = true;
        Point position1 = e.GetPosition((IInputElement) this.ContentBorder);
        if (position1.Y >= 0.0 && position1.Y <= this.ContentBorder.ActualHeight)
        {
          parentScrollViewer.PanningMode = PanningMode.None;
          Point position2 = e.GetPosition((IInputElement) this);
          BookmarkTreeViewItem.draggingModel = new WeakReference<BookmarkTreeViewItem.DragDropDataModel>(new BookmarkTreeViewItem.DragDropDataModel()
          {
            Bookmark = this.DataContext as BookmarkModel,
            TreeViewScrollViewer = parentScrollViewer,
            IsTouching = true,
            DragStartPoint = position2
          });
          e.Handled = true;
        }
      }
    }
    else if (e.SystemGesture == SystemGesture.RightDrag)
    {
      BookmarkTreeViewItem.DragDropDataModel target;
      if (BookmarkTreeViewItem.draggingModel != null && BookmarkTreeViewItem.draggingModel.TryGetTarget(out target))
      {
        this.CaptureStylus();
        e.Handled = true;
        this.DoDragDropOperation(target.DragStartPoint, true);
      }
    }
    else
    {
      this.ReleaseStylusCapture();
      BookmarkTreeViewItem.ResetCachedDraggingModel();
    }
    this.UpdateDraggingState(true);
  }

  protected override void OnDragEnter(DragEventArgs e)
  {
    base.OnDragEnter(e);
    this.ProcessDragEvent(e);
  }

  protected override void OnDragOver(DragEventArgs e)
  {
    base.OnDragOver(e);
    BookmarkTreeViewItem.DragDropProcessResult dropProcessResult = this.ProcessDragEvent(e);
    if ((e.Effects & DragDropEffects.Scroll) != DragDropEffects.None && dropProcessResult.ScrollOffset != 0.0 && dropProcessResult.BookmarkTreeViewScrollViewer != null)
    {
      double val1 = dropProcessResult.BookmarkTreeViewScrollViewer.VerticalOffset + dropProcessResult.ScrollOffset;
      dropProcessResult.BookmarkTreeViewScrollViewer.ScrollToVerticalOffset(Math.Max(Math.Min(val1, dropProcessResult.BookmarkTreeViewScrollViewer.ScrollableHeight), 0.0));
    }
    if ((e.Effects & DragDropEffects.Move) != DragDropEffects.None && dropProcessResult.Model != null)
    {
      switch (dropProcessResult.InsertPosition)
      {
        case BookmarkTreeViewItem.DragDropProcessResult.InsertPositionEnum.InsertToChildren:
          VisualStateManager.GoToState((FrameworkElement) this, "DropIndicatorForChildren", true);
          break;
        case BookmarkTreeViewItem.DragDropProcessResult.InsertPositionEnum.InsertAsPreviousSibling:
          VisualStateManager.GoToState((FrameworkElement) this, "DropIndicatorForPreviousSibling", true);
          break;
        case BookmarkTreeViewItem.DragDropProcessResult.InsertPositionEnum.InsertAsNextSibling:
          VisualStateManager.GoToState((FrameworkElement) this, "DropIndicatorForNextSibling", true);
          break;
      }
    }
    else
      VisualStateManager.GoToState((FrameworkElement) this, "DropIndicatorInvisible", true);
  }

  protected override void OnPreviewDragLeave(DragEventArgs e)
  {
    base.OnPreviewDragLeave(e);
    VisualStateManager.GoToState((FrameworkElement) this, "DropIndicatorInvisible", true);
  }

  protected override async void OnDrop(DragEventArgs e)
  {
    BookmarkTreeViewItem control = this;
    // ISSUE: reference to a compiler-generated method
    control.\u003C\u003En__0(e);
    VisualStateManager.GoToState((FrameworkElement) control, "DropIndicatorInvisible", true);
    if (!(control.DataContext is BookmarkModel dataContext1))
      return;
    BookmarkTreeViewItem.DragDropProcessResult dropProcessResult = control.ProcessDragEvent(e);
    BookmarkControl bookmarkControl = dropProcessResult.Model.BookmarkControl;
    if (dropProcessResult.Model != null && bookmarkControl.PdfViewer?.DataContext is MainViewModel dataContext2)
    {
      bool selected = dropProcessResult.Model.Bookmark.IsSelected;
      BookmarkModel newParent = (BookmarkModel) null;
      int insertIndex = -1;
      if (dropProcessResult.InsertPosition == BookmarkTreeViewItem.DragDropProcessResult.InsertPositionEnum.InsertToChildren)
      {
        newParent = dataContext1;
        insertIndex = 0;
      }
      else if (dropProcessResult.InsertPosition == BookmarkTreeViewItem.DragDropProcessResult.InsertPositionEnum.InsertAsPreviousSibling)
      {
        newParent = dataContext1.Parent;
        insertIndex = IndexOf(newParent.Children, dataContext1);
        if (insertIndex == -1)
          insertIndex = 0;
      }
      else if (dropProcessResult.InsertPosition == BookmarkTreeViewItem.DragDropProcessResult.InsertPositionEnum.InsertAsNextSibling)
      {
        newParent = dataContext1.Parent;
        int num = IndexOf(newParent.Children, dataContext1);
        insertIndex = num != -1 ? num + 1 : newParent.Children.Count;
      }
      if (newParent != null)
      {
        PdfDocument document = bookmarkControl.PdfViewer.Document;
        bookmarkControl.IsHitTestVisible = false;
        try
        {
          BookmarkModel bookmarkModel = await dataContext2.OperationManager.MoveBookmarkAsync(document, dropProcessResult.Model.Bookmark, newParent, insertIndex);
          if (bookmarkModel != null)
          {
            if (selected)
              bookmarkControl.SelectedItem = bookmarkModel;
          }
        }
        finally
        {
          bookmarkControl.IsHitTestVisible = true;
        }
      }
    }
    bookmarkControl = (BookmarkControl) null;

    static int IndexOf(System.Collections.Generic.IReadOnlyList<BookmarkModel> _bookmarks, BookmarkModel _bookmark)
    {
      if (_bookmarks == null)
        return -1;
      for (int index = 0; index < _bookmarks.Count; ++index)
      {
        if (_bookmarks[index] == _bookmark)
          return index;
      }
      return -1;
    }
  }

  private BookmarkTreeViewItem.DragDropProcessResult ProcessDragEvent(DragEventArgs e)
  {
    e.Handled = true;
    if (e.Data.GetDataPresent(typeof (BookmarkTreeViewItem.DragDropDataModel)))
    {
      BookmarkTreeViewItem.DragDropDataModel dragDropDataModel = (BookmarkTreeViewItem.DragDropDataModel) e.Data.GetData(typeof (BookmarkTreeViewItem.DragDropDataModel));
      double num = 0.0;
      ScrollViewer viewScrollViewer = dragDropDataModel.TreeViewScrollViewer;
      Point? nullable1 = new Point?();
      Point? nullable2 = new Point?();
      BookmarkTreeViewItem._POINT lpPoint;
      if (this.ContentBorder != null && BookmarkTreeViewItem.GetCursorPos(out lpPoint))
      {
        nullable1 = new Point?(this.PointFromScreen(new Point((double) lpPoint.X, (double) lpPoint.Y)));
        nullable2 = new Point?(this.TransformToVisual((Visual) this.ContentBorder).Transform(nullable1.Value));
        if (nullable2.Value.Y >= 0.0 && nullable2.Value.Y <= this.ContentBorder.ActualHeight)
        {
          Point point = this.TransformToVisual((Visual) viewScrollViewer).Transform(nullable1.Value);
          if (point.Y < 50.0 && viewScrollViewer.ContentVerticalOffset > 0.0)
          {
            e.Effects = DragDropEffects.Scroll;
            num = -20.0;
          }
          else if (point.Y > viewScrollViewer.ActualHeight - 50.0 && viewScrollViewer.ContentVerticalOffset < viewScrollViewer.ScrollableHeight)
          {
            e.Effects = DragDropEffects.Scroll;
            num = 20.0;
          }
        }
        else
          dragDropDataModel = (BookmarkTreeViewItem.DragDropDataModel) null;
      }
      if (dragDropDataModel != null)
      {
        BookmarkTreeViewItem.DragDropProcessResult.InsertPositionEnum insertPositionEnum = BookmarkTreeViewItem.DragDropProcessResult.InsertPositionEnum.InsertAsNextSibling;
        if (dragDropDataModel.BookmarkControl != null && BookmarkTreeViewItem.AllowDropToItem(dragDropDataModel.Bookmark, this.DataContext as BookmarkModel))
        {
          e.Effects = DragDropEffects.Move;
          System.Collections.Generic.IReadOnlyList<BookmarkModel> children = dragDropDataModel.BookmarkControl.Bookmarks.Children;
          if ((children != null ? children.FirstOrDefault<BookmarkModel>() : (BookmarkModel) null) == this.DataContext && nullable2.HasValue && nullable2.Value.Y < this.ContentBorder.ActualHeight / 5.0 * 2.0)
            insertPositionEnum = BookmarkTreeViewItem.DragDropProcessResult.InsertPositionEnum.InsertAsPreviousSibling;
          else if (this.ContentBorder != null && nullable1.HasValue)
          {
            if (dragDropDataModel.IsTouching)
            {
              if (this.TransformToVisual((Visual) dragDropDataModel.TreeViewScrollViewer).Transform(nullable1.Value).X > dragDropDataModel.TreeViewScrollViewer.ActualWidth / 2.0)
                insertPositionEnum = BookmarkTreeViewItem.DragDropProcessResult.InsertPositionEnum.InsertToChildren;
            }
            else if (this.TransformToVisual((Visual) this.ContentBorder).Transform(nullable1.Value).X > 20.0)
              insertPositionEnum = BookmarkTreeViewItem.DragDropProcessResult.InsertPositionEnum.InsertToChildren;
          }
        }
        else
          e.Effects = DragDropEffects.None;
        if (num != 0.0)
          e.Effects |= DragDropEffects.Scroll;
        return new BookmarkTreeViewItem.DragDropProcessResult()
        {
          Model = dragDropDataModel,
          ScrollOffset = num,
          BookmarkTreeViewScrollViewer = viewScrollViewer,
          MousePositionForItem = nullable1,
          InsertPosition = insertPositionEnum
        };
      }
    }
    e.Effects = DragDropEffects.None;
    return new BookmarkTreeViewItem.DragDropProcessResult();
  }

  private static bool AllowDropToItem(BookmarkModel draggingModel, BookmarkModel currentModel)
  {
    if (draggingModel == null || currentModel == null || draggingModel == currentModel)
      return false;
    for (BookmarkModel parent = currentModel.Parent; parent != null; parent = parent.Parent)
    {
      if (parent == draggingModel)
        return false;
    }
    return true;
  }

  private bool DoDragDropOperation(Point startPoint, bool isTouching)
  {
    BookmarkTreeViewItem.DragDropDataModel target;
    if (BookmarkTreeViewItem.draggingModel != null && BookmarkTreeViewItem.draggingModel.TryGetTarget(out target) && target.BookmarkControl == null)
      BookmarkTreeViewItem.ResetCachedDraggingModel();
    if (BookmarkTreeViewItem.draggingModel != null || !(this.DataContext is BookmarkModel dataContext))
      return false;
    ScrollViewer parentScrollViewer = this.ParentScrollViewer;
    BookmarkControl parentBookmarkControl = this.ParentBookmarkControl;
    if (parentScrollViewer == null || parentBookmarkControl == null)
      return false;
    if (isTouching)
      parentScrollViewer.PanningMode = PanningMode.None;
    target = new BookmarkTreeViewItem.DragDropDataModel()
    {
      Bookmark = dataContext,
      DragStartPoint = startPoint,
      ContainerSize = new Size(this.ActualWidth, this.ActualHeight),
      TreeViewScrollViewer = parentScrollViewer,
      BookmarkControl = parentBookmarkControl,
      IsTouching = isTouching
    };
    BookmarkTreeViewItem.draggingModel = new WeakReference<BookmarkTreeViewItem.DragDropDataModel>(target);
    this.UpdateDraggingState(true);
    int num = (int) DragDrop.DoDragDrop((DependencyObject) this, (object) target, DragDropEffects.Move);
    this.OnDropCompleted(target);
    return true;
  }

  private void OnDropCompleted(
    BookmarkTreeViewItem.DragDropDataModel dragDropModel)
  {
    dragDropModel.TreeViewScrollViewer.PanningMode = PanningMode.VerticalFirst;
    BookmarkTreeViewItem.ResetCachedDraggingModel();
    FrameworkElement reference = (FrameworkElement) dragDropModel.TreeViewScrollViewer;
    while (true)
    {
      switch (reference)
      {
        case null:
        case BookmarkTreeView _:
          goto label_3;
        default:
          reference = (reference.Parent ?? VisualTreeHelper.GetParent((DependencyObject) reference)) as FrameworkElement;
          continue;
      }
    }
label_3:
    if (reference != null && ((TreeView) reference).TreeViewItemFromElement((ITreeViewNode) dragDropModel.Bookmark) is BookmarkTreeViewItem bookmarkTreeViewItem)
      bookmarkTreeViewItem.UpdateDraggingState(true);
    if (!dragDropModel.IsTouching)
      return;
    this.ReleaseStylusCapture();
  }

  private static void ResetCachedDraggingModel()
  {
    WeakReference<BookmarkTreeViewItem.DragDropDataModel> draggingModel = BookmarkTreeViewItem.draggingModel;
    BookmarkTreeViewItem.draggingModel = (WeakReference<BookmarkTreeViewItem.DragDropDataModel>) null;
    BookmarkTreeViewItem.DragDropDataModel target;
    if (draggingModel == null || !draggingModel.TryGetTarget(out target) || target.TreeViewScrollViewer == null)
      return;
    target.TreeViewScrollViewer.PanningMode = PanningMode.VerticalFirst;
  }

  private void BookmarkTreeViewItem_DataContextChanged(
    object sender,
    DependencyPropertyChangedEventArgs e)
  {
    this.UpdateBackgroundSize();
  }

  private void UpdateBackgroundSize()
  {
    if (this.MarginColumn == null || this.BackgroundBorder == null || !(this.DataContext is BookmarkModel dataContext))
      return;
    int level = dataContext.Level;
    double num = 0.0;
    if (level > 0)
      num = (double) (19 * level);
    this.MarginColumn.MaxWidth = num;
    this.MarginColumn.Width = new GridLength(num, GridUnitType.Pixel);
  }

  protected override bool IsItemItsOwnContainerOverride(object item)
  {
    return item is BookmarkTreeViewItem;
  }

  protected override DependencyObject GetContainerForItemOverride()
  {
    return (DependencyObject) new BookmarkTreeViewItem();
  }

  private static void OnRequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
  {
    if (e.TargetObject != sender && !(e.TargetObject is ContentPresenter) && !(e.TargetObject is TextBox))
      return;
    BookmarkTreeViewItem container = sender as BookmarkTreeViewItem;
    if (container == null || container.LayoutRoot == null)
      return;
    if (container.LayoutRoot != null)
    {
      e.Handled = true;
      container.LayoutRoot.BringIntoView();
    }
    else
      container.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) (() => container.LayoutRoot?.BringIntoView()));
  }

  private BookmarkTreeView ParentTreeView
  {
    get
    {
      ItemsControl container = (ItemsControl) this;
      while (true)
      {
        switch (container)
        {
          case null:
          case BookmarkTreeView _:
            goto label_3;
          default:
            container = ItemsControl.ItemsControlFromItemContainer((DependencyObject) container);
            continue;
        }
      }
label_3:
      return container as BookmarkTreeView;
    }
  }

  private ScrollViewer ParentScrollViewer
  {
    get
    {
      BookmarkTreeView parentTreeView = this.ParentTreeView;
      return parentTreeView != null && VisualTreeHelper.GetChildrenCount((DependencyObject) parentTreeView) > 0 && VisualTreeHelper.GetChild((DependencyObject) parentTreeView, 0) is FrameworkElement child && child.FindName("_tv_scrollviewer_") is ScrollViewer name ? name : (ScrollViewer) null;
    }
  }

  private BookmarkControl ParentBookmarkControl
  {
    get
    {
      FrameworkElement reference = (FrameworkElement) this.ParentScrollViewer;
      while (true)
      {
        switch (reference)
        {
          case null:
          case BookmarkControl _:
            goto label_3;
          default:
            reference = (reference.Parent ?? VisualTreeHelper.GetParent((DependencyObject) reference)) as FrameworkElement;
            continue;
        }
      }
label_3:
      return reference as BookmarkControl;
    }
  }

  [DllImport("user32.dll")]
  private static extern bool GetCursorPos(out BookmarkTreeViewItem._POINT lpPoint);

  private struct _POINT
  {
    public int X;
    public int Y;
  }

  private struct DragDropProcessResult
  {
    public BookmarkTreeViewItem.DragDropDataModel Model;
    public ScrollViewer BookmarkTreeViewScrollViewer;
    public double ScrollOffset;
    public Point? MousePositionForItem;
    public BookmarkTreeViewItem.DragDropProcessResult.InsertPositionEnum InsertPosition;

    public enum InsertPositionEnum
    {
      InsertToChildren,
      InsertAsPreviousSibling,
      InsertAsNextSibling,
    }
  }

  private class DragDropDataModel
  {
    public BookmarkModel Bookmark { get; set; }

    public Point DragStartPoint { get; set; }

    public Size ContainerSize { get; set; }

    public BookmarkControl BookmarkControl { get; set; }

    public ScrollViewer TreeViewScrollViewer { get; set; }

    public bool IsTouching { get; set; }
  }
}
