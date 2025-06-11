// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.CollectionEnumerator`1
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Wrappers;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf;

internal class CollectionEnumerator<T> : IEnumerator<T>, IDisposable, IEnumerator
{
  private int _index = -1;
  private IList<T> _list;
  private Patagames.Pdf.Net.IReadOnlyList<T> _readOnlyList;
  private PdfNameTreeCollection _nameTree;
  private PdfAttachmentCollection _attachments;
  private PdfBookmarkCollections _bookmarks;
  private IntPtr _currentBookmark;
  private IntPtr _doc;

  private int _count
  {
    get
    {
      if (this._list != null)
        return this._list.Count;
      if (this._readOnlyList != null)
        return this._readOnlyList.Count;
      if ((PdfWrapper) this._nameTree != (PdfWrapper) null)
        return this._nameTree.Count;
      if (this._attachments != null)
        return this._attachments.Count;
      return this._bookmarks != null ? this._bookmarks.Count : 0;
    }
  }

  private T _item
  {
    get
    {
      if (this._list != null)
        return this._list[this._index];
      if (this._readOnlyList != null)
        return this._readOnlyList[this._index];
      if ((PdfWrapper) this._nameTree != (PdfWrapper) null)
        return (T) this._nameTree[this._index];
      if (this._attachments != null)
        return (T) this._attachments[this._index];
      return this._bookmarks != null ? (T) this._bookmarks[this._index] : default (T);
    }
  }

  public CollectionEnumerator(IList<T> collection)
  {
    this._list = collection;
    this._readOnlyList = (Patagames.Pdf.Net.IReadOnlyList<T>) null;
    this._nameTree = (PdfNameTreeCollection) null;
    this._attachments = (PdfAttachmentCollection) null;
    this._bookmarks = (PdfBookmarkCollections) null;
  }

  public CollectionEnumerator(Patagames.Pdf.Net.IReadOnlyList<T> collection)
  {
    this._list = (IList<T>) null;
    this._readOnlyList = collection;
    this._nameTree = (PdfNameTreeCollection) null;
    this._attachments = (PdfAttachmentCollection) null;
    this._bookmarks = (PdfBookmarkCollections) null;
  }

  public CollectionEnumerator(PdfNameTreeCollection collection)
  {
    this._list = (IList<T>) null;
    this._readOnlyList = (Patagames.Pdf.Net.IReadOnlyList<T>) null;
    this._nameTree = collection;
    this._attachments = (PdfAttachmentCollection) null;
    this._bookmarks = (PdfBookmarkCollections) null;
  }

  public CollectionEnumerator(PdfAttachmentCollection collection)
  {
    this._list = (IList<T>) null;
    this._readOnlyList = (Patagames.Pdf.Net.IReadOnlyList<T>) null;
    this._nameTree = (PdfNameTreeCollection) null;
    this._attachments = collection;
    this._bookmarks = (PdfBookmarkCollections) null;
  }

  public CollectionEnumerator(PdfBookmarkCollections collection, IntPtr doc)
  {
    this._list = (IList<T>) null;
    this._readOnlyList = (Patagames.Pdf.Net.IReadOnlyList<T>) null;
    this._nameTree = (PdfNameTreeCollection) null;
    this._attachments = (PdfAttachmentCollection) null;
    this._bookmarks = collection;
    this._doc = doc;
  }

  public T Current
  {
    get
    {
      if (this._bookmarks != null && this._currentBookmark == IntPtr.Zero)
        throw new InvalidOperationException();
      if (this._bookmarks != null && this._currentBookmark != IntPtr.Zero)
        return (T) this._bookmarks[this._currentBookmark];
      if (this._index < 0 || this._index >= this._count)
        throw new InvalidOperationException();
      return this._item;
    }
  }

  object IEnumerator.Current => (object) this.Current;

  public void Dispose()
  {
  }

  public bool MoveNext()
  {
    if (this._bookmarks != null && this._currentBookmark == IntPtr.Zero)
    {
      this._currentBookmark = Pdfium.FPDFBookmark_GetFirstChild(this._doc, this._bookmarks.Parent == null ? IntPtr.Zero : this._bookmarks.Parent.Handle);
      return this._currentBookmark != IntPtr.Zero;
    }
    if (this._bookmarks != null && this._currentBookmark != IntPtr.Zero)
    {
      this._currentBookmark = Pdfium.FPDFBookmark_GetNextSibling(this._doc, this._currentBookmark);
      return this._currentBookmark != IntPtr.Zero;
    }
    if (this._index < this._count - 1)
    {
      ++this._index;
      return true;
    }
    this._index = this._count;
    return false;
  }

  public void Reset()
  {
    this._currentBookmark = IntPtr.Zero;
    this._index = -1;
  }
}
