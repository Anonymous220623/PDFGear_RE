// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Bookmarks.BookmarkModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Patagames.Pdf.Net;
using pdfeditor.Controls;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;

#nullable disable
namespace pdfeditor.Models.Bookmarks;

public class BookmarkModel : ObservableObject, ITreeViewNode
{
  private string title;
  private BookmarkModel.NotifyResetList<BookmarkModel> children;
  private BookmarkModel parent;
  private bool isSelected;
  private bool isExpanded;
  private bool isHighlighted;

  public virtual string Title
  {
    get => this.title;
    private set => this.SetProperty<string>(ref this.title, value, nameof (Title));
  }

  public virtual int PageIndex { get; private set; }

  public virtual Point? Position { get; private set; }

  public System.Collections.Generic.IReadOnlyList<BookmarkModel> Children
  {
    get => (System.Collections.Generic.IReadOnlyList<BookmarkModel>) this.children;
  }

  public virtual PdfBookmark RawBookmark { get; private set; }

  ITreeViewNode ITreeViewNode.Parent
  {
    get => this.IsTopLevelModel ? (ITreeViewNode) null : (ITreeViewNode) this.Parent;
  }

  public virtual BookmarkModel Parent
  {
    get => this.parent;
    private set => this.SetProperty<BookmarkModel>(ref this.parent, value, nameof (Parent));
  }

  public bool IsTopLevelModel
  {
    get => this.Parent == null || this.Parent is BookmarkModel.RootBookmarkModel;
  }

  public virtual int Level
  {
    get
    {
      BookmarkModel parent = this.parent;
      return (parent != null ? parent.Level : -1) + 1;
    }
  }

  public virtual bool IsSelected
  {
    get => this.isSelected;
    set
    {
      if (value)
        this.ExpandToRoot();
      this.SetProperty<bool>(ref this.isSelected, value, nameof (IsSelected));
    }
  }

  public virtual bool IsExpanded
  {
    get => this.isExpanded;
    set => this.SetProperty<bool>(ref this.isExpanded, value, nameof (IsExpanded));
  }

  public virtual bool IsHighlighted
  {
    get => this.isHighlighted;
    set
    {
      if (!(value & this.SetProperty<bool>(ref this.isHighlighted, value, nameof (IsHighlighted))))
        return;
      this.ExpandToRoot();
    }
  }

  public event EventHandler ChildrenChanged;

  public virtual bool UpdateTitle(string newTitle)
  {
    newTitle = newTitle ?? string.Empty;
    if (!(newTitle != this.title))
      return false;
    this.RawBookmark.Title = newTitle;
    this.Title = newTitle;
    return true;
  }

  public void ExpandToRoot()
  {
    for (BookmarkModel parent = this.Parent; parent != null; parent = parent.Parent)
      parent.IsExpanded = true;
  }

  private void NotifyChildrenChanged(bool raiseCollectionEvent)
  {
    if (raiseCollectionEvent)
      this.children.NotifyReset();
    EventHandler childrenChanged = this.ChildrenChanged;
    if (childrenChanged != null)
      childrenChanged((object) this, EventArgs.Empty);
    for (BookmarkModel parent = this.Parent; parent != null; parent = parent.Parent)
      parent.NotifyChildrenChanged(false);
  }

  public BookmarkModel InsertChild(PdfDocument document, int index, BookmarkRecord record)
  {
    if (index < 0 || index > this.Children.Count)
      throw new ArgumentException(nameof (index));
    record.Index = index;
    PdfBookmarkCollections bookmarkCollection = this.GetRawBookmarkCollection(document);
    PdfBookmark bookmark = BookmarkRecordFactory.Insert(document, record, bookmarkCollection);
    if (bookmark == null)
      return (BookmarkModel) null;
    BookmarkModel bookmarkModel = BookmarkModel.Create(bookmark);
    this.children.Insert(index, bookmarkModel);
    bookmarkModel.Parent = this;
    this.NotifyChildrenChanged(true);
    return bookmarkModel;
  }

  public BookmarkRecord RemoveChild(PdfDocument document, BookmarkModel model)
  {
    if (model.Parent != this)
      return (BookmarkRecord) null;
    BookmarkRecord record = BookmarkRecordFactory.CreateRecord(document, model.RawBookmark);
    if (record == null || !this.children.Remove(model))
      return (BookmarkRecord) null;
    PdfBookmarkCollections bookmarkCollection = this.GetRawBookmarkCollection(document);
    BookmarkRecordFactory.Remove(document, model.RawBookmark, bookmarkCollection);
    model.Parent = (BookmarkModel) null;
    this.NotifyChildrenChanged(true);
    return record;
  }

  public BookmarkRecord RemoveChildAt(PdfDocument document, int index)
  {
    if (index < 0 || index >= this.children.Count)
      throw new ArgumentException(nameof (index));
    BookmarkModel child = this.Children[index];
    BookmarkRecord record = BookmarkRecordFactory.CreateRecord(document, child.RawBookmark);
    this.children.RemoveAt(index);
    PdfBookmarkCollections bookmarkCollection = this.GetRawBookmarkCollection(document);
    BookmarkRecordFactory.Remove(document, bookmarkCollection[record.Index], bookmarkCollection);
    child.Parent = (BookmarkModel) null;
    this.NotifyChildrenChanged(true);
    return record;
  }

  public virtual PdfBookmarkCollections GetRawBookmarkCollection(PdfDocument document)
  {
    return this.RawBookmark.Childs;
  }

  public static BookmarkModel Create(PdfDocument document)
  {
    BookmarkModel.RootBookmarkModel rootBookmarkModel = document != null ? new BookmarkModel.RootBookmarkModel(document) : throw new ArgumentException(nameof (document));
    if (document.Bookmarks != null)
    {
      IEnumerable<BookmarkModel> items = document.Bookmarks.Select<PdfBookmark, BookmarkModel>((Func<PdfBookmark, BookmarkModel>) (c => CreateCore(c)));
      rootBookmarkModel.children = new BookmarkModel.NotifyResetList<BookmarkModel>(items);
      for (int index = 0; index < rootBookmarkModel.children.Count; ++index)
        rootBookmarkModel.children[index].parent = (BookmarkModel) rootBookmarkModel;
    }
    else
      rootBookmarkModel.children = new BookmarkModel.NotifyResetList<BookmarkModel>();
    return (BookmarkModel) rootBookmarkModel;

    static BookmarkModel CreateCore(PdfBookmark bookmark)
    {
      if (bookmark == null)
        throw new ArgumentException(nameof (bookmark));
      Point? nullable = new Point?();
      if (bookmark.Destination != null)
      {
        float? left = bookmark.Destination.Left;
        float? top = bookmark.Destination.Top;
        if (left.HasValue || top.HasValue)
          nullable = new Point?(new Point((double) left.GetValueOrDefault(), (double) top.GetValueOrDefault()));
      }
      BookmarkModel bookmarkModel1 = new BookmarkModel();
      bookmarkModel1.Title = bookmark.Title ?? string.Empty;
      PdfDestination destination = bookmark.Destination;
      bookmarkModel1.PageIndex = destination != null ? destination.PageIndex : 0;
      bookmarkModel1.Position = nullable;
      bookmarkModel1.RawBookmark = bookmark;
      BookmarkModel core = bookmarkModel1;
      BookmarkModel.NotifyResetList<BookmarkModel> child = BookmarkModel.CreateChild(bookmark);
      foreach (BookmarkModel bookmarkModel2 in (List<BookmarkModel>) child)
        bookmarkModel2.Parent = core;
      core.children = child;
      return core;
    }
  }

  public static BookmarkModel Create(PdfBookmark bookmark)
  {
    if (bookmark == null)
      throw new ArgumentException(nameof (bookmark));
    Point? nullable = new Point?();
    if (bookmark.Destination != null)
    {
      float? left = bookmark.Destination.Left;
      float? top = bookmark.Destination.Top;
      if (left.HasValue || top.HasValue)
        nullable = new Point?(new Point((double) left.GetValueOrDefault(), (double) top.GetValueOrDefault()));
    }
    BookmarkModel bookmarkModel1 = new BookmarkModel();
    bookmarkModel1.Title = bookmark.Title ?? string.Empty;
    PdfDestination destination = bookmark.Destination;
    bookmarkModel1.PageIndex = destination != null ? destination.PageIndex : 0;
    bookmarkModel1.Position = nullable;
    bookmarkModel1.RawBookmark = bookmark;
    BookmarkModel bookmarkModel2 = bookmarkModel1;
    BookmarkModel.NotifyResetList<BookmarkModel> child = BookmarkModel.CreateChild(bookmark);
    foreach (BookmarkModel bookmarkModel3 in (List<BookmarkModel>) child)
      bookmarkModel3.Parent = bookmarkModel2;
    bookmarkModel2.children = child;
    return bookmarkModel2;
  }

  private static BookmarkModel.NotifyResetList<BookmarkModel> CreateChild(PdfBookmark bookmark)
  {
    if (bookmark == null)
      throw new ArgumentException(nameof (bookmark));
    return bookmark.Childs != null && bookmark.Childs.Count > 0 ? new BookmarkModel.NotifyResetList<BookmarkModel>(bookmark.Childs.Select<PdfBookmark, BookmarkModel>((Func<PdfBookmark, BookmarkModel>) (c => BookmarkModel.Create(c)))) : new BookmarkModel.NotifyResetList<BookmarkModel>();
  }

  private class NotifyResetList<T> : List<T>, INotifyCollectionChanged
  {
    public NotifyResetList()
    {
    }

    public NotifyResetList(IEnumerable<T> items)
      : base(items)
    {
    }

    public event NotifyCollectionChangedEventHandler CollectionChanged;

    public void NotifyReset()
    {
      NotifyCollectionChangedEventHandler collectionChanged = this.CollectionChanged;
      if (collectionChanged == null)
        return;
      collectionChanged((object) this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
  }

  private class RootBookmarkModel : BookmarkModel
  {
    private readonly PdfDocument document;

    public RootBookmarkModel(PdfDocument document) => this.document = document;

    public override bool IsExpanded => throw new NotImplementedException(nameof (IsExpanded));

    public override bool IsHighlighted => throw new NotImplementedException(nameof (IsHighlighted));

    public override bool IsSelected => throw new NotImplementedException(nameof (IsSelected));

    public override int PageIndex => throw new NotImplementedException(nameof (PageIndex));

    public override PdfBookmark RawBookmark
    {
      get => throw new NotImplementedException(nameof (RawBookmark));
    }

    public override string Title => throw new NotImplementedException(nameof (Title));

    public override BookmarkModel Parent => (BookmarkModel) null;

    public override Point? Position => new Point?();

    public override int Level => -1;

    public override bool UpdateTitle(string newTitle)
    {
      throw new NotImplementedException(nameof (UpdateTitle));
    }

    public override PdfBookmarkCollections GetRawBookmarkCollection(PdfDocument document)
    {
      return document.Bookmarks;
    }
  }
}
