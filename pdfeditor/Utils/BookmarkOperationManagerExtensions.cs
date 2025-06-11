// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.BookmarkOperationManagerExtensions
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf.Net;
using pdfeditor.Models.Bookmarks;
using pdfeditor.Models.Operations;
using pdfeditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable disable
namespace pdfeditor.Utils;

public static class BookmarkOperationManagerExtensions
{
  public static async Task<bool> RemoveBookmarkAsync(
    this OperationManager operationManager,
    PdfDocument document,
    BookmarkModel bookmark,
    string tag = "")
  {
    if (operationManager == null || document == null || bookmark == null || bookmark.Parent == null)
      return false;
    BookmarkModel parent = bookmark.Parent;
    BookmarkOperationManagerExtensions.BookmarkAccessor oldParentAccessor = new BookmarkOperationManagerExtensions.BookmarkAccessor(parent);
    int oldIndex = -1;
    for (int index = 0; index < parent.Children.Count; ++index)
    {
      if (parent.Children[index] == bookmark)
      {
        oldIndex = index;
        break;
      }
    }
    if (oldIndex >= 0)
    {
      BookmarkRecord oldRecord = parent.RemoveChild(document, bookmark);
      if (oldRecord != null)
      {
        await operationManager.AddOperationAsync((Action<PdfDocument>) (doc => oldParentAccessor.GetTarget(doc).InsertChild(doc, oldIndex, oldRecord)?.ExpandToRoot()), (Action<PdfDocument>) (doc => oldParentAccessor.GetTarget(doc).RemoveChildAt(doc, oldIndex)), tag);
        return true;
      }
    }
    return false;
  }

  public static async Task<BookmarkModel> InsertBookmarkAsync(
    this OperationManager operationManager,
    PdfDocument document,
    BookmarkModel parent,
    BookmarkRecord record,
    string tag = "")
  {
    if (operationManager == null || document == null || record == null || parent == null)
      return (BookmarkModel) null;
    int insertIndex = record.Index;
    if (insertIndex < 0 || insertIndex > parent.Children.Count)
      return (BookmarkModel) null;
    BookmarkModel result = parent.InsertChild(document, insertIndex, record);
    if (result == null)
      throw new ArgumentException("result");
    BookmarkOperationManagerExtensions.BookmarkAccessor newParentAccessor = new BookmarkOperationManagerExtensions.BookmarkAccessor(parent);
    BookmarkRecord newRecord = BookmarkRecordFactory.CreateRecord(document, result.RawBookmark);
    await operationManager.AddOperationAsync((Action<PdfDocument>) (doc => newParentAccessor.GetTarget(doc).RemoveChildAt(doc, insertIndex)), (Action<PdfDocument>) (doc => newParentAccessor.GetTarget(doc).InsertChild(doc, insertIndex, newRecord)?.ExpandToRoot()), tag);
    return result;
  }

  public static async Task<BookmarkModel> MoveBookmarkAsync(
    this OperationManager operationManager,
    PdfDocument document,
    BookmarkModel bookmark,
    BookmarkModel newParent,
    int insertIndex,
    string tag = "")
  {
    if (bookmark == null || newParent == null || insertIndex < 0 || insertIndex > newParent.Children.Count)
      return (BookmarkModel) null;
    if (bookmark.Parent == newParent && insertIndex < newParent.Children.Count && newParent.Children[insertIndex] == bookmark)
      return bookmark;
    BookmarkModel parent = bookmark.Parent;
    BookmarkOperationManagerExtensions.BookmarkAccessor oldParentAccessor = new BookmarkOperationManagerExtensions.BookmarkAccessor(parent);
    int oldIndex = -1;
    for (int index = 0; index < parent.Children.Count; ++index)
    {
      if (parent.Children[index] == bookmark)
      {
        oldIndex = index;
        break;
      }
    }
    if (oldIndex >= 0)
    {
      BookmarkRecord oldRecord = parent.RemoveChild(document, bookmark);
      if (oldRecord != null)
      {
        if (newParent == parent && insertIndex > oldIndex)
          insertIndex--;
        BookmarkModel result = newParent.InsertChild(document, insertIndex, oldRecord);
        if (result == null)
          throw new ArgumentException("result");
        BookmarkOperationManagerExtensions.BookmarkAccessor newParentAccessor = new BookmarkOperationManagerExtensions.BookmarkAccessor(newParent);
        BookmarkRecord newRecord = BookmarkRecordFactory.CreateRecord(document, result.RawBookmark);
        result.ExpandToRoot();
        await operationManager.AddOperationAsync((Action<PdfDocument>) (doc =>
        {
          newParentAccessor.GetTarget(doc).RemoveChildAt(doc, insertIndex);
          oldParentAccessor.GetTarget(doc).InsertChild(doc, oldIndex, oldRecord)?.ExpandToRoot();
        }), (Action<PdfDocument>) (doc =>
        {
          oldParentAccessor.GetTarget(doc).RemoveChildAt(doc, oldIndex);
          newParentAccessor.GetTarget(doc).InsertChild(doc, insertIndex, newRecord)?.ExpandToRoot();
        }), tag);
        return result;
      }
    }
    return (BookmarkModel) null;
  }

  public static async Task<bool> UpdateBookmarkTitleAsync(
    this OperationManager operationManager,
    PdfDocument document,
    BookmarkModel bookmark,
    string newTitle,
    string tag = "")
  {
    newTitle = newTitle ?? string.Empty;
    string oldTitle = bookmark?.Title ?? "";
    if (string.Equals(newTitle, oldTitle))
      return false;
    bool? nullable = bookmark?.UpdateTitle(newTitle);
    if (!nullable.HasValue || !nullable.GetValueOrDefault())
      return false;
    BookmarkOperationManagerExtensions.BookmarkAccessor parentAccessor = new BookmarkOperationManagerExtensions.BookmarkAccessor(bookmark);
    await operationManager.AddOperationAsync((Action<PdfDocument>) (doc => parentAccessor.GetTarget(doc)?.UpdateTitle(oldTitle)), (Action<PdfDocument>) (doc => parentAccessor.GetTarget(doc)?.UpdateTitle(newTitle)), tag);
    return true;
  }

  private static BookmarkModel GetRootBookmarkModel(PdfDocument doc)
  {
    return !(PDFKit.PdfControl.GetPdfControl(doc)?.DataContext is MainViewModel dataContext) ? (BookmarkModel) null : dataContext.Bookmarks;
  }

  private class BookmarkAccessor
  {
    private List<int> indexList = new List<int>();

    public BookmarkAccessor(BookmarkModel bookmark)
    {
      for (BookmarkModel bookmarkModel = bookmark; bookmarkModel != null && bookmarkModel.Level != -1; bookmarkModel = bookmarkModel.Parent)
      {
        int count = this.indexList.Count;
        for (int index = 0; index < bookmarkModel.Parent.Children.Count && count == this.indexList.Count; ++index)
        {
          if (bookmarkModel.Parent.Children[index] == bookmarkModel)
            this.indexList.Add(index);
        }
        if (count == this.indexList.Count)
          throw new ArgumentException(nameof (bookmark));
      }
    }

    public BookmarkModel GetTarget(BookmarkModel rootModel)
    {
      BookmarkModel target = rootModel.Level == -1 ? rootModel : throw new ArgumentException(nameof (rootModel));
      for (int index = this.indexList.Count - 1; index >= 0; --index)
        target = target.Children[this.indexList[index]];
      return target;
    }

    public BookmarkModel GetTarget(PdfDocument document)
    {
      return this.GetTarget(BookmarkOperationManagerExtensions.GetRootBookmarkModel(document));
    }
  }
}
