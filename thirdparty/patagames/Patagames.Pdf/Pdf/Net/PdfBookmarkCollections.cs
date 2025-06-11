// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfBookmarkCollections
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Properties;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>
/// Represents the collection of bookmarks insde a PDF document.
/// </summary>
public class PdfBookmarkCollections : IEnumerable<PdfBookmark>, IEnumerable
{
  private PdfDocument _doc;
  private ListObjectManager<IntPtr, PdfBookmark> _mgr = new ListObjectManager<IntPtr, PdfBookmark>();

  /// <summary>Gets parent bookmark.</summary>
  public PdfBookmark Parent { get; private set; }

  /// <summary>
  /// Initializes a new instance of the PdfBookmarkCollections class.
  /// </summary>
  /// <param name="document">Document which contains this collection of bookmarks.</param>
  /// <param name="parent">Parent bookmark</param>
  internal PdfBookmarkCollections(PdfDocument document, PdfBookmark parent)
  {
    this._doc = document != null ? document : throw new ArgumentNullException(nameof (document));
    this.Parent = parent;
  }

  /// <summary>
  /// Find a bookmark in the PDF document, using the bookmark title
  /// </summary>
  /// <param name="title">The string for the bookmark title to be searched. Can't be NULL or empty string.</param>
  /// <returns>The instance of PdfBookmark class represented a found bookmark item. Null if the title can't be found.</returns>
  /// <remarks>It always returns the first found bookmark if more than one bookmarks have the same title.</remarks>
  public PdfBookmark Find(string title)
  {
    if ((title ?? "").Trim() == "")
      throw new ArgumentException(Error.err0005, nameof (title));
    IntPtr num = Pdfium.FPDFBookmark_Find(this._doc.Handle, title);
    if (num == IntPtr.Zero)
      return (PdfBookmark) null;
    if (this._mgr.Contains(num))
      return this._mgr.Get(num);
    PdfBookmark pdfBookmark = new PdfBookmark(this._doc, num, this.Parent);
    this._mgr.Add(num, pdfBookmark);
    return pdfBookmark;
  }

  /// <summary>
  /// Inserts a bookmark into the collection at the specified index.
  /// </summary>
  /// <param name="Index">The zero-based index at which bookmark should be inserted.</param>
  /// <param name="Title">The title of the bookmark.</param>
  /// <param name="Page">The page to which the transition should be made when interacting with this bookmark.</param>
  /// <returns>The bookmark that has been inserted.</returns>
  public PdfBookmark InsertAt(int Index, string Title, PdfPage Page)
  {
    int count = this.Count;
    if (Index < 0 || Index > count)
      throw new ArgumentOutOfRangeException(nameof (Index), (object) Index, string.Format(Error.err0006, (object) 0, (object) count));
    PdfTypeDictionary prev;
    PdfTypeDictionary next;
    PdfTypeDictionary parent;
    this.GetDictionariesForInsert(Index, count, out prev, out next, out parent);
    PdfTypeDictionary dictionary = Page.Dictionary;
    this.CreateNewBookmark(parent, prev, next, Title, dictionary, (PdfAction) null, (PdfDestination) null);
    IntPtr zero = IntPtr.Zero;
    IntPtr handle = prev != null ? Pdfium.FPDFBookmark_GetNextSibling(this._doc.Handle, prev.Handle) : Pdfium.FPDFBookmark_GetFirstChild(this._doc.Handle, parent.Handle);
    if (handle == IntPtr.Zero)
      throw Pdfium.ProcessLastError();
    return new PdfBookmark(this._doc, handle, this.Parent);
  }

  /// <summary>
  /// Removes the bookmark at the specified index of collection.
  /// </summary>
  /// <param name="Index">The zero-based index of the bookmark to remove.</param>
  public void DeleteAt(int Index)
  {
    int count = this.Count;
    if (Index < 0 || Index > count - 1)
      throw new ArgumentOutOfRangeException(nameof (Index), (object) Index, string.Format(Error.err0006, (object) 0, (object) (count - 1)));
    PdfTypeDictionary prev;
    PdfTypeDictionary next;
    PdfTypeDictionary parent;
    this.GetDictionariesForDelete(Index, count, out prev, out next, out parent);
    PdfIndirectList list = PdfIndirectList.FromPdfDocument(this._doc);
    PdfTypeNumber pdfTypeNumber1 = parent["Count"] as PdfTypeNumber;
    int intValue = pdfTypeNumber1.IntValue;
    int num1;
    if (next == null && prev == null)
    {
      parent.Remove("Last");
      parent.Remove("First");
      parent.Remove("Count");
    }
    else if (next != null && prev != null)
    {
      prev.SetIndirectAt("Next", list, (PdfTypeBase) next);
      next.SetIndirectAt("Prev", list, (PdfTypeBase) prev);
      PdfTypeNumber pdfTypeNumber2 = pdfTypeNumber1;
      int num2;
      if (intValue >= 0)
        num2 = num1 = intValue - 1;
      else
        num1 = num2 = intValue + 1;
      pdfTypeNumber2.IntValue = num2;
    }
    else if (next == null)
    {
      prev.Remove("Next");
      parent.SetIndirectAt("Last", list, (PdfTypeBase) prev);
      PdfTypeNumber pdfTypeNumber3 = pdfTypeNumber1;
      int num3;
      if (intValue >= 0)
        num3 = num1 = intValue - 1;
      else
        num1 = num3 = intValue + 1;
      pdfTypeNumber3.IntValue = num3;
    }
    else
    {
      if (prev != null)
        return;
      next.Remove("Prev");
      parent.SetIndirectAt("First", list, (PdfTypeBase) next);
      PdfTypeNumber pdfTypeNumber4 = pdfTypeNumber1;
      int num4;
      if (intValue >= 0)
        num4 = num1 = intValue - 1;
      else
        num1 = num4 = intValue + 1;
      pdfTypeNumber4.IntValue = num4;
    }
  }

  /// <summary>
  /// Inserts a bookmark with the specified action into the collection at the specified index.
  /// </summary>
  /// <param name="Index">The zero-based index at which bookmark should be inserted.</param>
  /// <param name="Title">The title of the bookmark.</param>
  /// <param name="action">The action to be performed by the bookmark.</param>
  /// <returns>The bookmark that has been inserted.</returns>
  public PdfBookmark InsertAt(int Index, string Title, PdfAction action)
  {
    int count = this.Count;
    if (Index < 0 || Index > count)
      throw new ArgumentOutOfRangeException(nameof (Index), (object) Index, string.Format(Error.err0006, (object) 0, (object) count));
    PdfTypeDictionary prev;
    PdfTypeDictionary next;
    PdfTypeDictionary parent;
    this.GetDictionariesForInsert(Index, count, out prev, out next, out parent);
    this.CreateNewBookmark(parent, prev, next, Title, (PdfTypeDictionary) null, action, (PdfDestination) null);
    IntPtr zero = IntPtr.Zero;
    IntPtr handle = prev != null ? Pdfium.FPDFBookmark_GetNextSibling(this._doc.Handle, prev.Handle) : Pdfium.FPDFBookmark_GetFirstChild(this._doc.Handle, parent.Handle);
    if (handle == IntPtr.Zero)
      throw Pdfium.ProcessLastError();
    return new PdfBookmark(this._doc, handle, this.Parent);
  }

  /// <summary>
  /// Inserts a bookmark with the specified destination into the collection at the specified index.
  /// </summary>
  /// <param name="Index">The zero-based index at which bookmark should be inserted.</param>
  /// <param name="Title">The title of the bookmark.</param>
  /// <param name="destination">The destination to be performed by the bookmark.</param>
  /// <returns>The bookmark that has been inserted.</returns>
  public PdfBookmark InsertAt(int Index, string Title, PdfDestination destination)
  {
    int count = this.Count;
    if (Index < 0 || Index > count)
      throw new ArgumentOutOfRangeException(nameof (Index), (object) Index, string.Format(Error.err0006, (object) 0, (object) count));
    PdfTypeDictionary prev;
    PdfTypeDictionary next;
    PdfTypeDictionary parent;
    this.GetDictionariesForInsert(Index, count, out prev, out next, out parent);
    this.CreateNewBookmark(parent, prev, next, Title, (PdfTypeDictionary) null, (PdfAction) null, destination);
    IntPtr zero = IntPtr.Zero;
    IntPtr handle = prev != null ? Pdfium.FPDFBookmark_GetNextSibling(this._doc.Handle, prev.Handle) : Pdfium.FPDFBookmark_GetFirstChild(this._doc.Handle, parent.Handle);
    if (handle == IntPtr.Zero)
      throw Pdfium.ProcessLastError();
    return new PdfBookmark(this._doc, handle, this.Parent);
  }

  private IntPtr GetByIndex(int index)
  {
    IntPtr bookmark = Pdfium.FPDFBookmark_GetFirstChild(this._doc.Handle, this.Parent == null ? IntPtr.Zero : this.Parent.Handle);
    int num = 0;
    for (; bookmark != IntPtr.Zero; bookmark = Pdfium.FPDFBookmark_GetNextSibling(this._doc.Handle, bookmark))
    {
      if (num++ == index)
        return bookmark;
    }
    return IntPtr.Zero;
  }

  private int GetCount()
  {
    IntPtr bookmark = Pdfium.FPDFBookmark_GetFirstChild(this._doc.Handle, this.Parent == null ? IntPtr.Zero : this.Parent.Handle);
    int count = 0;
    for (; bookmark != IntPtr.Zero; bookmark = Pdfium.FPDFBookmark_GetNextSibling(this._doc.Handle, bookmark))
      ++count;
    return count;
  }

  private void GetDictionariesForInsert(
    int Index,
    int count,
    out PdfTypeDictionary prev,
    out PdfTypeDictionary next,
    out PdfTypeDictionary parent)
  {
    prev = next = parent = (PdfTypeDictionary) null;
    if (this.Parent != null)
      parent = this.Parent.Dictionary;
    else if (this._doc.Root.ContainsKey("Outlines"))
    {
      parent = (this._doc.Root["Outlines"] as PdfTypeIndirect).Direct as PdfTypeDictionary;
    }
    else
    {
      parent = PdfTypeDictionary.Create();
      parent["Type"] = (PdfTypeBase) PdfTypeName.Create("Outlines");
      PdfIndirectList list = PdfIndirectList.FromPdfDocument(this._doc);
      list.Add((PdfTypeBase) parent);
      this._doc.Root.SetIndirectAt("Outlines", list, (PdfTypeBase) parent);
    }
    if (parent == null)
      throw Pdfium.ProcessLastError(536870929U /*0x20000011*/);
    prev = Index == 0 ? (PdfTypeDictionary) null : PdfTypeDictionary.Create(this[Index - 1].Handle);
    next = Index == count ? (PdfTypeDictionary) null : PdfTypeDictionary.Create(this[Index].Handle);
  }

  private PdfTypeDictionary CreateNewBookmark(
    PdfTypeDictionary parent,
    PdfTypeDictionary prev,
    PdfTypeDictionary next,
    string title,
    PdfTypeDictionary page,
    PdfAction action,
    PdfDestination destination)
  {
    PdfTypeDictionary newBookmark = PdfTypeDictionary.Create();
    PdfIndirectList list = PdfIndirectList.FromPdfDocument(this._doc);
    list.Add((PdfTypeBase) newBookmark);
    if (prev != null)
    {
      prev.SetIndirectAt("Next", list, (PdfTypeBase) newBookmark);
      newBookmark.SetIndirectAt("Prev", list, (PdfTypeBase) prev);
    }
    if (next != null)
    {
      next.SetIndirectAt("Prev", list, (PdfTypeBase) newBookmark);
      newBookmark.SetIndirectAt("Next", list, (PdfTypeBase) next);
    }
    if (prev == null && next == null)
    {
      parent.SetIndirectAt("Last", list, (PdfTypeBase) newBookmark);
      parent.SetIndirectAt("First", list, (PdfTypeBase) newBookmark);
      parent["Count"] = (PdfTypeBase) PdfTypeNumber.Create(-1);
    }
    else if (prev != null && next != null)
    {
      if (!parent.ContainsKey("Count"))
        parent["Count"] = (PdfTypeBase) PdfTypeNumber.Create(0);
      PdfTypeNumber pdfTypeNumber = parent["Count"] as PdfTypeNumber;
      int intValue = pdfTypeNumber.IntValue;
      int num1;
      int num2;
      if (intValue >= 0)
        num2 = num1 = intValue + 1;
      else
        num1 = num2 = intValue - 1;
      pdfTypeNumber.IntValue = num2;
    }
    else if (prev == null)
    {
      parent.SetIndirectAt("First", list, (PdfTypeBase) newBookmark);
      if (!parent.ContainsKey("Count"))
        parent["Count"] = (PdfTypeBase) PdfTypeNumber.Create(0);
      PdfTypeNumber pdfTypeNumber = parent["Count"] as PdfTypeNumber;
      int intValue = pdfTypeNumber.IntValue;
      int num3;
      int num4;
      if (intValue >= 0)
        num4 = num3 = intValue + 1;
      else
        num3 = num4 = intValue - 1;
      pdfTypeNumber.IntValue = num4;
    }
    else if (next == null)
    {
      parent.SetIndirectAt("Last", list, (PdfTypeBase) newBookmark);
      if (!parent.ContainsKey("Count"))
        parent["Count"] = (PdfTypeBase) PdfTypeNumber.Create(0);
      PdfTypeNumber pdfTypeNumber = parent["Count"] as PdfTypeNumber;
      int intValue = pdfTypeNumber.IntValue;
      int num5;
      int num6;
      if (intValue >= 0)
        num6 = num5 = intValue + 1;
      else
        num5 = num6 = intValue - 1;
      pdfTypeNumber.IntValue = num6;
    }
    newBookmark.SetIndirectAt("Parent", list, (PdfTypeBase) parent);
    newBookmark["Title"] = (PdfTypeBase) PdfTypeString.Create(title, true);
    if (page != null)
    {
      PdfTypeArray pdfTypeArray = PdfTypeArray.Create();
      newBookmark["Dest"] = (PdfTypeBase) pdfTypeArray;
      pdfTypeArray.AddIndirect(list, (PdfTypeBase) page);
      pdfTypeArray.AddName("XYZ");
      pdfTypeArray.Add((PdfTypeBase) PdfTypeNull.Create());
      pdfTypeArray.Add((PdfTypeBase) PdfTypeNull.Create());
      pdfTypeArray.Add((PdfTypeBase) PdfTypeNull.Create());
    }
    else if (action != null)
    {
      if (action.Dictionary.ObjectNumber == 0 && Pdfium.FPDFOBJ_GetParentObj(action.Dictionary.Handle) != IntPtr.Zero)
        throw new ArgumentException(string.Format(Error.err0067, (object) "pdf action", (object) "object"));
      list.Add((PdfTypeBase) action.Dictionary);
      newBookmark.SetIndirectAt("A", list, (PdfTypeBase) action.Dictionary);
    }
    else
      newBookmark["Dest"] = destination != null ? destination.GetForInsert(this._doc) : throw new NotImplementedException();
    return newBookmark;
  }

  private void GetDictionariesForDelete(
    int Index,
    int count,
    out PdfTypeDictionary prev,
    out PdfTypeDictionary next,
    out PdfTypeDictionary parent)
  {
    prev = next = parent = (PdfTypeDictionary) null;
    if (this.Parent != null)
      parent = this.Parent.Dictionary;
    else if (this._doc.Root.ContainsKey("Outlines"))
      parent = (this._doc.Root["Outlines"] as PdfTypeIndirect).Direct as PdfTypeDictionary;
    if (parent == null)
      throw Pdfium.ProcessLastError(536870929U /*0x20000011*/);
    prev = Index == 0 ? (PdfTypeDictionary) null : PdfTypeDictionary.Create(this[Index - 1].Handle);
    next = Index == count - 1 ? (PdfTypeDictionary) null : PdfTypeDictionary.Create(this[Index + 1].Handle);
  }

  /// <summary>
  /// Gets the <see cref="T:Patagames.Pdf.Net.PdfBookmark" /> with the specified handle
  /// </summary>
  /// <param name="handle">Pdfium SDK handle that the bookmark is bound to</param>
  /// <returns>The <see cref="T:Patagames.Pdf.Net.PdfBookmark" /> at the specified index.</returns>
  public PdfBookmark this[IntPtr handle]
  {
    get
    {
      if (this._mgr.Contains(handle))
        return this._mgr.Get(handle);
      PdfBookmark pdfBookmark = new PdfBookmark(this._doc, handle, this.Parent);
      this._mgr.Add(handle, pdfBookmark);
      return pdfBookmark;
    }
  }

  /// <summary>
  /// Gets or sets the <see cref="T:Patagames.Pdf.Net.PdfBookmark" /> at the specified index
  /// </summary>
  /// <param name="index">The zero-based index of the element to get.</param>
  /// <returns>The <see cref="T:Patagames.Pdf.Net.PdfBookmark" /> at the specified index.</returns>
  public PdfBookmark this[int index]
  {
    get
    {
      IntPtr num = index >= 0 && index < this.Count ? this.GetByIndex(index) : throw new ArgumentOutOfRangeException();
      if (num == IntPtr.Zero)
        throw Pdfium.ProcessLastError();
      if (this._mgr.Contains(num))
        return this._mgr.Get(num);
      PdfBookmark pdfBookmark = new PdfBookmark(this._doc, num, this.Parent);
      this._mgr.Add(num, pdfBookmark);
      return pdfBookmark;
    }
  }

  /// <summary>
  /// Gets the number of <see cref="T:Patagames.Pdf.Net.PdfBookmark" /> contained in the collection.
  /// </summary>
  public int Count => this.GetCount();

  /// <summary>
  /// Gets a value indicating whether the collection is read-only.
  /// </summary>
  public bool IsReadOnly => false;

  /// <summary>
  /// Removes all <see cref="T:Patagames.Pdf.Net.PdfBookmark" /> from the collection
  /// </summary>
  public void Clear()
  {
    if (!this._doc.Root.ContainsKey("Outlines"))
      return;
    this._doc.Root.Remove("Outlines");
  }

  /// <summary>
  /// Determines whether the collection contains a specific <see cref="T:Patagames.Pdf.Net.PdfBookmark" />.
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Net.PdfBookmark" /> to locate in the collection.</param>
  /// <returns>true if <see cref="T:Patagames.Pdf.Net.PdfBookmark" /> is found in the collection; otherwise, false.</returns>
  public bool Contains(PdfBookmark item) => this.IndexOf(item) >= 0;

  /// <summary>
  /// Copies the elements of the collections to an System.Array, starting at a particular System.Array index.
  /// </summary>
  /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from collection. The System.Array must have zero-based indexing.</param>
  /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
  public void CopyTo(PdfBookmark[] array, int arrayIndex)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    if (arrayIndex < 0)
      throw new ArgumentOutOfRangeException();
    foreach (PdfBookmark pdfBookmark in this)
    {
      if (arrayIndex > array.Length - 1)
        break;
      array[arrayIndex++] = pdfBookmark;
    }
  }

  /// <summary>
  /// Returns an enumerator that iterates through the collection.
  /// </summary>
  /// <returns>A System.Collections.Generic.IEnumerator that can be used to iterate through the collection.</returns>
  public IEnumerator<PdfBookmark> GetEnumerator()
  {
    return (IEnumerator<PdfBookmark>) new CollectionEnumerator<PdfBookmark>(this, this._doc.Handle);
  }

  /// <summary>
  /// Determines the index of a specific  <see cref="T:Patagames.Pdf.Net.PdfBookmark" /> in the collection.
  /// </summary>
  /// <param name="item">The  <see cref="T:Patagames.Pdf.Net.PdfBookmark" /> to locate in the collection</param>
  /// <returns>The index of  <see cref="T:Patagames.Pdf.Net.PdfBookmark" /> if found in the collection; otherwise, -1.</returns>
  public int IndexOf(PdfBookmark item)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      if (this[index].Handle == item.Handle)
        return index;
    }
    return -1;
  }

  /// <summary>
  /// Removes the first occurrence of a specific <see cref="T:Patagames.Pdf.Net.PdfBookmark" /> from the collection.
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Net.PdfBookmark" /> to remove from the collection.</param>
  /// <returns>
  /// true if <see cref="T:Patagames.Pdf.Net.PdfBookmark" /> was successfully removed from the collection;
  /// otherwise, false. This method also returns false if <see cref="T:Patagames.Pdf.Net.PdfBookmark" /> is not found in the
  /// original collection.
  /// </returns>
  public bool Remove(PdfBookmark item)
  {
    int Index = this.IndexOf(item);
    if (Index < 0)
      return false;
    this.DeleteAt(Index);
    return true;
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
}
