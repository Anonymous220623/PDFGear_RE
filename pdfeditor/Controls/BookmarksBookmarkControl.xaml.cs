// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Bookmarks.BookmarkControl
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using Patagames.Pdf.Net;
using pdfeditor.Models.Bookmarks;
using pdfeditor.Utils;
using PDFKit;
using PDFKit.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Controls.Bookmarks;

public sealed partial class BookmarkControl : Control
{
  private BookmarkModel bookmarkSource;
  private IReadOnlyDictionary<int, System.Collections.Generic.IReadOnlyList<BookmarkModel>> bookmarkLeaves;
  private BookmarkModel lastHighlightBookmark;
  private BookmarkTreeView bookmarkTreeView;
  private BookmarkControl.ScrollViewerHelper pdfViewerScrollHelper;
  private static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof (SelectedItem), typeof (BookmarkModel), typeof (BookmarkControl), new PropertyMetadata((object) null, (PropertyChangedCallback) ((s, a) =>
  {
    if (a.NewValue == a.OldValue || !(s is BookmarkControl bookmarkControl2))
      return;
    BookmarkModel bookmarkModel = (BookmarkModel) null;
    if (a.NewValue is BookmarkModel newValue2 && bookmarkControl2.bookmarkSource != null)
      bookmarkModel = BookmarkControl.FindBookmarkModel(bookmarkControl2.bookmarkSource.Children, newValue2?.RawBookmark?.Handle.GetValueOrDefault());
    if (bookmarkModel != null)
    {
      bookmarkModel.IsSelected = true;
      bookmarkControl2.ScrollIntoViewAsync(bookmarkModel);
    }
    else if (bookmarkControl2.BookmarkTreeView?.SelectedItem is BookmarkModel selectedItem2)
      selectedItem2.IsSelected = false;
    BookmarkTreeView bookmarkTreeView = bookmarkControl2.BookmarkTreeView;
    if (!((bookmarkTreeView != null ? bookmarkTreeView.TreeViewItemFromElement((ITreeViewNode) bookmarkModel) : (TreeViewItem) null) is BookmarkTreeViewItem bookmarkTreeViewItem2) || bookmarkTreeViewItem2.ShouldUpdatePageIndex)
      bookmarkControl2.UpdateSelectedPageIndex();
    bookmarkControl2.OnSelectionChanged(bookmarkModel);
  })));
  public static readonly DependencyProperty BookmarksProperty = DependencyProperty.Register(nameof (Bookmarks), typeof (BookmarkModel), typeof (BookmarkControl), new PropertyMetadata((object) null, new PropertyChangedCallback(BookmarkControl.OnBookmarksPropertyChanged)));
  public static readonly DependencyProperty PdfViewerProperty = DependencyProperty.Register(nameof (PdfViewer), typeof (PdfViewer), typeof (BookmarkControl), new PropertyMetadata((object) null, new PropertyChangedCallback(BookmarkControl.OnPdfViewerPropertyChanged)));

  static BookmarkControl()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (BookmarkControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (BookmarkControl)));
  }

  private BookmarkTreeView BookmarkTreeView
  {
    get => this.bookmarkTreeView;
    set
    {
      if (this.bookmarkTreeView == value)
        return;
      if (this.bookmarkTreeView != null)
        this.bookmarkTreeView.SelectedItemChanged -= new RoutedPropertyChangedEventHandler<object>(this.BookmarkTreeView_SelectedItemChanged);
      this.bookmarkTreeView = value;
      if (this.bookmarkTreeView != null)
      {
        this.bookmarkTreeView.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(this.BookmarkTreeView_SelectedItemChanged);
        this.bookmarkTreeView.ItemsSource = (IEnumerable) this.bookmarkSource?.Children;
      }
      this.UpdateHighlightBookmark();
    }
  }

  private void BookmarkTreeView_SelectedItemChanged(
    object sender,
    RoutedPropertyChangedEventArgs<object> e)
  {
    this.SelectedItem = e.NewValue as BookmarkModel;
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.BookmarkTreeView = this.GetTemplateChild("PART_BookmarkTreeView") as BookmarkTreeView;
  }

  private void UpdateBookmarks()
  {
    try
    {
      IntPtr? handle = this.lastHighlightBookmark?.RawBookmark?.Handle;
      if (this.lastHighlightBookmark != null)
        this.lastHighlightBookmark.IsHighlighted = false;
      this.lastHighlightBookmark = (BookmarkModel) null;
      if (this.Bookmarks == null)
      {
        this.bookmarkLeaves = (IReadOnlyDictionary<int, System.Collections.Generic.IReadOnlyList<BookmarkModel>>) null;
        this.bookmarkSource = (BookmarkModel) null;
      }
      else
      {
        BookmarkModel bookmarkSource = this.bookmarkSource;
        if (bookmarkSource != null)
          WeakEventManager<BookmarkModel, EventArgs>.RemoveHandler(bookmarkSource, "ChildrenChanged", new EventHandler<EventArgs>(this.Bookmarks_ChildrenChanged));
        this.bookmarkSource = this.Bookmarks;
        if (this.bookmarkSource != null)
          WeakEventManager<BookmarkModel, EventArgs>.AddHandler(this.bookmarkSource, "ChildrenChanged", new EventHandler<EventArgs>(this.Bookmarks_ChildrenChanged));
        this.UpdateBookmarkLeaves();
        if (this.bookmarkSource != bookmarkSource)
          this.UpdateExpandState(this.bookmarkSource.Children, bookmarkSource?.Children);
        if (handle.HasValue)
        {
          BookmarkModel bookmarkModel = BookmarkControl.FindBookmarkModel(this.bookmarkSource.Children, handle.Value);
          if (bookmarkModel != null)
          {
            this.lastHighlightBookmark = bookmarkModel;
            bookmarkModel.IsHighlighted = true;
          }
        }
      }
      if (this.BookmarkTreeView == null)
        return;
      this.BookmarkTreeView.ItemsSource = (IEnumerable) this.bookmarkSource?.Children;
    }
    catch (OperationCanceledException ex)
    {
      this.bookmarkLeaves = (IReadOnlyDictionary<int, System.Collections.Generic.IReadOnlyList<BookmarkModel>>) null;
      this.bookmarkSource = (BookmarkModel) null;
    }
  }

  private void Bookmarks_ChildrenChanged(object sender, EventArgs e) => this.UpdateBookmarkLeaves();

  private void UpdateBookmarkLeaves()
  {
    try
    {
      if (this.bookmarkSource != null)
      {
        this.bookmarkLeaves = this.CreateLeaves(this.bookmarkSource.Children);
        return;
      }
    }
    catch
    {
    }
    this.bookmarkLeaves = (IReadOnlyDictionary<int, System.Collections.Generic.IReadOnlyList<BookmarkModel>>) null;
    this.UpdateHighlightBookmark();
  }

  private void UpdateExpandState(
    System.Collections.Generic.IReadOnlyList<BookmarkModel> bookmarks,
    System.Collections.Generic.IReadOnlyList<BookmarkModel> oldBookmarks)
  {
    if (bookmarks == null || bookmarks.Count == 0 || oldBookmarks == null || oldBookmarks.Count == 0)
      return;
    Dictionary<IntPtr, (bool, System.Collections.Generic.IReadOnlyList<BookmarkModel>)> dictionary = oldBookmarks.ToDictionary<BookmarkModel, IntPtr, (bool, System.Collections.Generic.IReadOnlyList<BookmarkModel>)>((Func<BookmarkModel, IntPtr>) (c => c.RawBookmark.Handle), (Func<BookmarkModel, (bool, System.Collections.Generic.IReadOnlyList<BookmarkModel>)>) (c => (c.IsExpanded, c.Children)));
    foreach (BookmarkModel bookmark in (IEnumerable<BookmarkModel>) bookmarks)
    {
      (bool, System.Collections.Generic.IReadOnlyList<BookmarkModel>) tuple;
      if (dictionary.TryGetValue(bookmark.RawBookmark.Handle, out tuple))
      {
        bookmark.IsExpanded = tuple.Item1;
        if (bookmark.Children != null && bookmark.Children.Count > 0)
          this.UpdateExpandState(bookmark.Children, tuple.Item2);
      }
    }
  }

  public void ExpandAll() => this.SetAllIsExpanded(true);

  public void CollapseAll() => this.SetAllIsExpanded(false);

  private void SetAllIsExpanded(bool isExpanded)
  {
    if (this.bookmarkSource == null)
      return;
    HashSet<BookmarkModel> _expandedItems = new HashSet<BookmarkModel>();
    BookmarkModel selectedItem = this.BookmarkTreeView.SelectedItem as BookmarkModel;
    if (!isExpanded)
    {
      for (BookmarkModel bookmarkModel = selectedItem; bookmarkModel != null; bookmarkModel = bookmarkModel.Parent)
        _expandedItems.Add(bookmarkModel);
    }
    foreach (BookmarkModel child in (IEnumerable<BookmarkModel>) this.bookmarkSource.Children)
      SetIsExpandedCore(child, isExpanded, _expandedItems);
    if (selectedItem == null)
      return;
    this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) (() => this.ScrollIntoViewAsync(selectedItem)));

    static void SetIsExpandedCore(
      BookmarkModel _model,
      bool _isExpanded,
      HashSet<BookmarkModel> _expandedItems)
    {
      if (_model == null || _model.Children == null || _model.Children.Count <= 0)
        return;
      if (_isExpanded || !_expandedItems.Contains(_model))
        _model.IsExpanded = _isExpanded;
      foreach (BookmarkModel child in (IEnumerable<BookmarkModel>) _model.Children)
        SetIsExpandedCore(child, _isExpanded, _expandedItems);
    }
  }

  public BookmarkModel SelectedItem
  {
    get => (BookmarkModel) this.GetValue(BookmarkControl.SelectedItemProperty);
    set => this.SetValue(BookmarkControl.SelectedItemProperty, (object) value);
  }

  public BookmarkModel Bookmarks
  {
    get => (BookmarkModel) this.GetValue(BookmarkControl.BookmarksProperty);
    set => this.SetValue(BookmarkControl.BookmarksProperty, (object) value);
  }

  private static async void OnBookmarksPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue == e.OldValue || !(d is BookmarkControl bookmarkControl))
      return;
    bookmarkControl.UpdateBookmarks();
    if (bookmarkControl.UpdateHighlightBookmark() || bookmarkControl.lastHighlightBookmark == null)
      return;
    await bookmarkControl.ScrollIntoViewAsync(bookmarkControl.lastHighlightBookmark);
  }

  public PdfViewer PdfViewer
  {
    get => (PdfViewer) this.GetValue(BookmarkControl.PdfViewerProperty);
    set => this.SetValue(BookmarkControl.PdfViewerProperty, (object) value);
  }

  private static void OnPdfViewerPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is BookmarkControl bookmarkControl) || object.Equals(e.NewValue, e.OldValue))
      return;
    bookmarkControl.UpdatePdfViewer();
    bookmarkControl.UpdateHighlightBookmark();
  }

  public event BookmarkControlSelectionChangedEventHandler SelectionChanged;

  private void OnSelectionChanged(BookmarkModel bookmark)
  {
    BookmarkControlSelectionChangedEventHandler selectionChanged = this.SelectionChanged;
    if (selectionChanged == null)
      return;
    selectionChanged(this, new BookmarkControlSelectionChangedEventArgs(bookmark));
  }

  private void UpdatePdfViewer()
  {
    this.pdfViewerScrollHelper?.Dispose();
    this.pdfViewerScrollHelper = (BookmarkControl.ScrollViewerHelper) null;
    if (this.PdfViewer == null)
      return;
    this.pdfViewerScrollHelper = new BookmarkControl.ScrollViewerHelper((IScrollInfo) this.PdfViewer);
    // ISSUE: method pointer
    this.pdfViewerScrollHelper.DelayScrollChanged += new EventHandler((object) this, __methodptr(\u003CUpdatePdfViewer\u003Eg__ScrollViewerHelper_DelayScrollChanged\u007C37_0));
  }

  private bool UpdateHighlightBookmark()
  {
    if (this.BookmarkTreeView == null || this.pdfViewerScrollHelper?.ScrollViewer == null)
      return false;
    PdfViewer pdfViewer = this.PdfViewer;
    if (pdfViewer == null || this.Bookmarks == null)
      return false;
    BookmarkModel model = (BookmarkModel) null;
    double actualHeight = pdfViewer.ActualHeight;
    if (this.bookmarkLeaves != null && this.bookmarkLeaves.Count > 0)
    {
      (int startPage, int endPage) = pdfViewer.GetVisiblePageRange();
      double d1 = double.NaN;
      BookmarkModel bookmarkModel = (BookmarkModel) null;
      double d2 = double.NaN;
      for (int index1 = startPage; index1 <= endPage; ++index1)
      {
        System.Collections.Generic.IReadOnlyList<BookmarkModel> bookmarkModelList;
        if (this.bookmarkLeaves.TryGetValue(index1, out bookmarkModelList))
        {
          bool flag = false;
          for (int index2 = 0; index2 < bookmarkModelList.Count; ++index2)
          {
            Point? position = bookmarkModelList[index2].Position;
            if (position.HasValue)
            {
              position = bookmarkModelList[index2].Position;
              Point pt = position.Value;
              Point client = pdfViewer.PageToClient(index1, pt);
              double num = client.X * client.X + client.Y * client.Y;
              if (num < d1 || double.IsNaN(d1))
              {
                d1 = num;
                model = bookmarkModelList[index2];
              }
            }
            else if (!flag)
            {
              PdfPage page = pdfViewer.Document.Pages[index1];
              FS_SIZEF effectiveSize = page.GetEffectiveSize(page.Rotation);
              Point client1 = pdfViewer.PageToClient(index1, new Point(0.0, 0.0));
              Point client2 = pdfViewer.PageToClient(index1, new Point((double) effectiveSize.Width, (double) effectiveSize.Height));
              Point point = new Point(Math.Min(client1.X, client2.X), Math.Min(client1.Y, client2.Y));
              double num = point.X * point.X + point.Y * point.Y;
              if (num < d2 || double.IsNaN(d2))
              {
                d2 = num;
                bookmarkModel = bookmarkModelList[index2];
              }
            }
          }
        }
      }
      if (model == null)
        model = bookmarkModel;
    }
    if (this.lastHighlightBookmark == model)
      return false;
    if (model != null)
    {
      if (this.lastHighlightBookmark != null)
        this.lastHighlightBookmark.IsHighlighted = false;
      this.lastHighlightBookmark = model;
      model.IsHighlighted = true;
      this.ScrollIntoViewAsync(model);
    }
    return true;
  }

  internal void UpdateSelectedPageIndex()
  {
    PdfViewer pdfViewer = this.PdfViewer;
    if (pdfViewer == null || this.Bookmarks == null || this.BookmarkTreeView == null || !(this.BookmarkTreeView.SelectedItem is BookmarkModel selectedItem) || selectedItem.PageIndex < 0)
      return;
    bool flag = false;
    Point? position = selectedItem.Position;
    if (position.HasValue)
    {
      position = selectedItem.Position;
      Point pagePoint = position.Value;
      double width;
      double height;
      Pdfium.FPDF_GetPageSizeByIndex(pdfViewer.Document.Handle, selectedItem.PageIndex, out width, out height);
      if (Math.Abs(pagePoint.X) < width && Math.Abs(pagePoint.Y) < height)
      {
        flag = true;
        pdfViewer.ScrollToPoint(selectedItem.PageIndex, pagePoint);
      }
    }
    if (flag)
      return;
    pdfViewer.ScrollToPage(selectedItem.PageIndex);
  }

  private async Task ScrollIntoViewAsync(BookmarkModel model)
  {
    if (this.BookmarkTreeView == null)
      return;
    await this.BookmarkTreeView.ScrollIntoViewAsync((ITreeViewNode) model, ScrollIntoViewOrientation.Vertical).ConfigureAwait(false);
  }

  public void ScrollIntoView(BookmarkModel item) => this.ScrollIntoViewAsync(item);

  private static BookmarkModel FindBookmarkModel(
    System.Collections.Generic.IReadOnlyList<BookmarkModel> bookmarks,
    IntPtr bookmarkHandle)
  {
    if (bookmarkHandle == IntPtr.Zero)
      return (BookmarkModel) null;
    foreach (BookmarkModel bookmark in (IEnumerable<BookmarkModel>) bookmarks)
    {
      BookmarkModel bookmarkModel;
      if (bookmark != null)
      {
        IntPtr? handle = bookmark.RawBookmark?.Handle;
        IntPtr num = bookmarkHandle;
        if ((handle.HasValue ? (handle.GetValueOrDefault() == num ? 1 : 0) : 0) != 0)
        {
          bookmarkModel = bookmark;
          goto label_8;
        }
      }
      bookmarkModel = BookmarkControl.FindBookmarkModel(bookmark.Children, bookmarkHandle);
label_8:
      if (bookmarkModel != null)
        return bookmarkModel;
    }
    return (BookmarkModel) null;
  }

  private IReadOnlyDictionary<int, System.Collections.Generic.IReadOnlyList<BookmarkModel>> CreateLeaves(
    System.Collections.Generic.IReadOnlyList<BookmarkModel> bookmarks)
  {
    return (IReadOnlyDictionary<int, System.Collections.Generic.IReadOnlyList<BookmarkModel>>) new SortedDictionary<int, System.Collections.Generic.IReadOnlyList<BookmarkModel>>((IDictionary<int, System.Collections.Generic.IReadOnlyList<BookmarkModel>>) bookmarks.SelectMany<BookmarkModel, BookmarkModel>((Func<BookmarkModel, IEnumerable<BookmarkModel>>) (c => Flatten(c))).GroupBy<BookmarkModel, int>((Func<BookmarkModel, int>) (c => c.PageIndex)).Select<IGrouping<int, BookmarkModel>, (int, System.Collections.Generic.IReadOnlyList<BookmarkModel>)>((Func<IGrouping<int, BookmarkModel>, (int, System.Collections.Generic.IReadOnlyList<BookmarkModel>)>) (c => (c.Key, (System.Collections.Generic.IReadOnlyList<BookmarkModel>) c.OrderByDescending<BookmarkModel, int>((Func<BookmarkModel, int>) (x => x.Level)).ToList<BookmarkModel>()))).Where<(int, System.Collections.Generic.IReadOnlyList<BookmarkModel>)>((Func<(int, System.Collections.Generic.IReadOnlyList<BookmarkModel>), bool>) (c => c.value != null)).ToDictionary<(int, System.Collections.Generic.IReadOnlyList<BookmarkModel>), int, System.Collections.Generic.IReadOnlyList<BookmarkModel>>((Func<(int, System.Collections.Generic.IReadOnlyList<BookmarkModel>), int>) (c => c.key), (Func<(int, System.Collections.Generic.IReadOnlyList<BookmarkModel>), System.Collections.Generic.IReadOnlyList<BookmarkModel>>) (c => c.value)));

    static IEnumerable<BookmarkModel> Flatten(BookmarkModel _model)
    {
      if (_model == null)
        return Enumerable.Empty<BookmarkModel>();
      return _model.Children != null && _model.Children.Count > 0 ? ((IEnumerable<BookmarkModel>) new BookmarkModel[1]
      {
        _model
      }).Concat<BookmarkModel>(_model.Children.SelectMany<BookmarkModel, BookmarkModel>((Func<BookmarkModel, IEnumerable<BookmarkModel>>) (c => Flatten(c)))) : (IEnumerable<BookmarkModel>) new BookmarkModel[1]
      {
        _model
      };
    }
  }

  private Task ApplyScrollPositionSnapshotAsync(BookmarkControl.ScrollPositionSnapshot snapshot)
  {
    // ISSUE: unable to decompile the method.
  }

  private BookmarkControl.ScrollPositionSnapshot TakeScrollPositionSnapshot()
  {
    if (this.BookmarkTreeView == null || VisualTreeHelper.GetChildrenCount((DependencyObject) this.BookmarkTreeView) == 0)
      return (BookmarkControl.ScrollPositionSnapshot) null;
    ScrollViewer scrollViewer = (VisualTreeHelper.GetChild((DependencyObject) this.BookmarkTreeView, 0) is FrameworkElement child1 ? child1.FindName("_tv_scrollviewer_") : (object) null) as ScrollViewer;
    if (scrollViewer == null)
      return (BookmarkControl.ScrollPositionSnapshot) null;
    FrameworkElement topElement = (FrameworkElement) null;
    Rect? topBounds = new Rect?();
    FindTopElement((ItemsControl) this.BookmarkTreeView);
    return topBounds.HasValue && topElement?.DataContext is BookmarkModel dataContext1 && dataContext1.RawBookmark != null && dataContext1.RawBookmark.Handle != IntPtr.Zero ? new BookmarkControl.ScrollPositionSnapshot(dataContext1.RawBookmark.Handle, topBounds.Value) : (BookmarkControl.ScrollPositionSnapshot) null;

    void FindTopElement(ItemsControl _itemsControl)
    {
      Panel itemsControlPanel = GetItemsControlPanel(_itemsControl);
      if (itemsControlPanel == null)
        return;
      foreach (BookmarkTreeViewItem _itemsControl1 in itemsControlPanel.Children.OfType<BookmarkTreeViewItem>())
      {
        if (_itemsControl1.DataContext is BookmarkModel dataContext2)
        {
          Rect rect = _itemsControl1.TransformToVisual((Visual) scrollViewer).TransformBounds(new Rect(0.0, 0.0, _itemsControl1.ActualWidth, _itemsControl1.ActualHeight));
          if (rect.Top > 0.0 && (topBounds.HasValue && topBounds.Value.Top > rect.Top || !topBounds.HasValue))
          {
            topBounds = new Rect?(rect);
            topElement = (FrameworkElement) _itemsControl1;
          }
          if (dataContext2.Children.Count > 0)
            FindTopElement((ItemsControl) _itemsControl1);
        }
      }
    }

    static Panel GetItemsControlPanel(ItemsControl _itemsControl)
    {
      if (_itemsControl == null)
        return (Panel) null;
      if (!(VisualTreeHelper.GetChild((DependencyObject) _itemsControl, 0) is FrameworkElement child2))
        return (Panel) null;
      if (!(child2.FindName("ItemsHost") is ItemsPresenter reference))
        reference = (child2.FindName("_tv_scrollviewer_") as ScrollViewer).Content as ItemsPresenter;
      return reference != null && VisualTreeHelper.GetChildrenCount((DependencyObject) reference) > 0 ? VisualTreeHelper.GetChild((DependencyObject) reference, 0) as Panel : (Panel) null;
    }
  }

  public bool CheckIfCollapseAll()
  {
    return this.bookmarkSource.Children.All<BookmarkModel>((Func<BookmarkModel, bool>) (item => this.CheckIfAllExpanded(item)));
  }

  private bool CheckIfAllExpanded(BookmarkModel item)
  {
    if (!item.IsExpanded && item.Children.Count != 0)
      return false;
    if (item.Children != null && item.Children.Count > 0)
    {
      foreach (BookmarkModel child in (IEnumerable<BookmarkModel>) item.Children)
      {
        if (!this.CheckIfAllExpanded(child))
          return false;
      }
    }
    return true;
  }

  private class ScrollPositionSnapshot
  {
    public ScrollPositionSnapshot(IntPtr rawBookmarkHandle, Rect itemBounds)
    {
      if (rawBookmarkHandle == IntPtr.Zero)
        throw new ArgumentException(nameof (rawBookmarkHandle));
      if (itemBounds.IsEmpty)
        throw new ArgumentException(nameof (itemBounds));
      this.RawBookmarkHandle = rawBookmarkHandle;
      this.ItemBounds = itemBounds;
      this.CreateTime = DateTime.UtcNow;
    }

    public DateTime CreateTime { get; }

    public IntPtr RawBookmarkHandle { get; }

    public Rect ItemBounds { get; }

    public bool IsValid => (DateTime.UtcNow - this.CreateTime).TotalSeconds < 10.0;
  }

  private class ScrollViewerHelper : IDisposable
  {
    private IScrollInfo scrollInfo;
    private ScrollViewer scrollViewer;
    private DispatcherTimer delayTimer;
    private DateTime lastScrollTime;

    public ScrollViewerHelper(IScrollInfo scrollInfo)
    {
      this.scrollInfo = scrollInfo;
      this.EnsureScrollViewer(false);
      if (!(scrollInfo is FrameworkElement frameworkElement))
        return;
      frameworkElement.Loaded += new RoutedEventHandler(this.ScrollInfo_Loaded);
      frameworkElement.Unloaded += new RoutedEventHandler(this.ScrollInfo_Unloaded);
    }

    public ScrollViewer ScrollViewer => this.scrollViewer;

    private void ScrollInfo_Loaded(object sender, RoutedEventArgs e)
    {
      if (!((FrameworkElement) sender).IsLoaded)
        return;
      this.RemoveScrollViewer();
      this.EnsureScrollViewer(true);
    }

    private void ScrollInfo_Unloaded(object sender, RoutedEventArgs e)
    {
      if (((FrameworkElement) sender).IsLoaded)
        return;
      this.RemoveScrollViewer();
    }

    private void EnsureScrollViewer(bool raiseLoaded)
    {
      if (this.scrollInfo == null || this.scrollViewer != null)
        return;
      this.scrollViewer = this.scrollInfo?.ScrollOwner;
      if (this.scrollViewer == null)
        return;
      if (this.delayTimer == null)
      {
        this.delayTimer = new DispatcherTimer(DispatcherPriority.Normal)
        {
          Interval = TimeSpan.FromMilliseconds(500.0)
        };
        this.delayTimer.Tick += (EventHandler) ((s, a) =>
        {
          ((DispatcherTimer) s).Stop();
          if (this.scrollViewer == null)
            return;
          EventHandler delayScrollChanged = this.DelayScrollChanged;
          if (delayScrollChanged == null)
            return;
          delayScrollChanged((object) this.scrollViewer, EventArgs.Empty);
        });
      }
      this.scrollViewer.ScrollChanged -= new ScrollChangedEventHandler(this.ScrollViewer_ScrollChanged);
      this.scrollViewer.ScrollChanged += new ScrollChangedEventHandler(this.ScrollViewer_ScrollChanged);
      this.scrollViewer.Unloaded -= new RoutedEventHandler(this.ScrollViewer_Unloaded);
      this.scrollViewer.Unloaded += new RoutedEventHandler(this.ScrollViewer_Unloaded);
      if (this.scrollViewer.IsLoaded)
      {
        if (!raiseLoaded)
          return;
        EventHandler scrollViewerLoaded = this.ScrollViewerLoaded;
        if (scrollViewerLoaded != null)
          scrollViewerLoaded((object) this.scrollViewer, EventArgs.Empty);
        EventHandler delayScrollChanged = this.DelayScrollChanged;
        if (delayScrollChanged == null)
          return;
        delayScrollChanged((object) this.scrollViewer, EventArgs.Empty);
      }
      else
      {
        this.scrollViewer.Loaded -= new RoutedEventHandler(this.ScrollViewer_Loaded);
        this.scrollViewer.Loaded += new RoutedEventHandler(this.ScrollViewer_Loaded);
      }
    }

    private void RemoveScrollViewer()
    {
      ScrollViewer scrollViewer = this.scrollViewer;
      if (scrollViewer == null)
        return;
      this.scrollViewer = (ScrollViewer) null;
      this.delayTimer?.Stop();
      scrollViewer.Loaded -= new RoutedEventHandler(this.ScrollViewer_Loaded);
      scrollViewer.Unloaded -= new RoutedEventHandler(this.ScrollViewer_Unloaded);
      scrollViewer.ScrollChanged -= new ScrollChangedEventHandler(this.ScrollViewer_ScrollChanged);
    }

    private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
    {
      if (this.scrollViewer == null)
        return;
      DateTime now = DateTime.Now;
      ScrollChangedEventHandler scrollChanged = this.ScrollChanged;
      if (scrollChanged != null)
        scrollChanged(sender, e);
      if (!this.delayTimer.IsEnabled || (now - this.lastScrollTime).TotalMilliseconds > 500.0)
      {
        this.lastScrollTime = now;
        EventHandler delayScrollChanged = this.DelayScrollChanged;
        if (delayScrollChanged != null)
          delayScrollChanged((object) this.scrollViewer, EventArgs.Empty);
      }
      this.delayTimer.Stop();
      this.delayTimer.Start();
    }

    private void ScrollViewer_Unloaded(object sender, RoutedEventArgs e)
    {
      this.RemoveScrollViewer();
    }

    private void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
    {
      ((FrameworkElement) sender).Loaded -= new RoutedEventHandler(this.ScrollViewer_Loaded);
      if (this.scrollViewer == null)
        return;
      EventHandler scrollViewerLoaded = this.ScrollViewerLoaded;
      if (scrollViewerLoaded != null)
        scrollViewerLoaded((object) this.scrollViewer, EventArgs.Empty);
      EventHandler delayScrollChanged = this.DelayScrollChanged;
      if (delayScrollChanged == null)
        return;
      delayScrollChanged((object) this.scrollViewer, EventArgs.Empty);
    }

    public event EventHandler ScrollViewerLoaded;

    public event ScrollChangedEventHandler ScrollChanged;

    public event EventHandler DelayScrollChanged;

    public void Dispose()
    {
      if (this.scrollInfo is FrameworkElement scrollInfo)
      {
        scrollInfo.Loaded -= new RoutedEventHandler(this.ScrollInfo_Loaded);
        scrollInfo.Unloaded -= new RoutedEventHandler(this.ScrollInfo_Unloaded);
      }
      this.scrollInfo = (IScrollInfo) null;
      this.delayTimer?.Stop();
      this.delayTimer = (DispatcherTimer) null;
      this.ScrollChanged = (ScrollChangedEventHandler) null;
      this.ScrollViewerLoaded = (EventHandler) null;
      this.DelayScrollChanged = (EventHandler) null;
      this.RemoveScrollViewer();
    }
  }
}
