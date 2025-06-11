// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Bookmarks.BookmarkTreeView
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Models.Bookmarks;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Controls.Bookmarks;

internal sealed partial class BookmarkTreeView : TreeView
{
  public static readonly DependencyProperty CanDragItemsProperty = DependencyProperty.Register(nameof (CanDragItems), typeof (bool), typeof (BookmarkTreeView), new PropertyMetadata((object) false));

  static BookmarkTreeView()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (BookmarkTreeView), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (BookmarkTreeView)));
  }

  public override void OnApplyTemplate() => base.OnApplyTemplate();

  public bool CanDragItems
  {
    get => (bool) this.GetValue(BookmarkTreeView.CanDragItemsProperty);
    set => this.SetValue(BookmarkTreeView.CanDragItemsProperty, (object) value);
  }

  protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
  {
    base.OnPreviewMouseDown(e);
    if (e.ChangedButton != MouseButton.Left || !(e.OriginalSource is ScrollViewer))
      return;
    this.Dispatcher.BeginInvoke(DispatcherPriority.Input, (Delegate) (() =>
    {
      if (!(this.SelectedItem is BookmarkModel selectedItem2))
        return;
      selectedItem2.IsSelected = false;
    }));
  }

  protected override bool IsItemItsOwnContainerOverride(object item)
  {
    return item is BookmarkTreeViewItem;
  }

  protected override DependencyObject GetContainerForItemOverride()
  {
    return (DependencyObject) new BookmarkTreeViewItem();
  }

  private BookmarkControl ParentBookmarkControl
  {
    get
    {
      FrameworkElement reference = (FrameworkElement) this;
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
}
