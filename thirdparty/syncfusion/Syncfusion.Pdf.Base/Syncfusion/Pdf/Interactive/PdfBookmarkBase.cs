// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfBookmarkBase
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfBookmarkBase : IPdfWrapper, IEnumerable
{
  private System.Collections.Generic.List<PdfBookmarkBase> m_list = new System.Collections.Generic.List<PdfBookmarkBase>();
  private PdfDictionary m_dictionary = new PdfDictionary();
  private PdfCrossTable m_crossTable = new PdfCrossTable();
  private System.Collections.Generic.List<PdfBookmark> bookmark;
  private System.Collections.Generic.List<PdfBookmarkBase> m_booklist;
  private bool m_isExpanded;
  private int parentIndex = -1;
  internal System.Collections.Generic.List<long> m_bookmarkReference = new System.Collections.Generic.List<long>();

  internal PdfBookmarkBase()
  {
  }

  internal PdfBookmarkBase(PdfDictionary dictionary, PdfCrossTable crossTable)
  {
    this.m_dictionary = dictionary;
    if (crossTable == null)
      return;
    this.m_crossTable = crossTable;
  }

  public int Count
  {
    get
    {
      if (!(this.m_crossTable.Document is PdfLoadedDocument) || this.m_booklist != null)
        return this.List.Count;
      this.m_booklist = new System.Collections.Generic.List<PdfBookmarkBase>();
      for (int index = 0; index < this.List.Count; ++index)
        this.m_booklist.Add(this.List[index]);
      return this.List.Count;
    }
  }

  public PdfBookmark this[int index] => this.List[index] as PdfBookmark;

  internal virtual System.Collections.Generic.List<PdfBookmarkBase> List => this.m_list;

  internal PdfDictionary Dictionary => this.m_dictionary;

  internal PdfCrossTable CrossTable => this.m_crossTable;

  internal bool IsExpanded
  {
    get
    {
      if (!this.Dictionary.ContainsKey("Count"))
        return this.m_isExpanded;
      return (this.Dictionary["Count"] as PdfNumber).IntValue >= 0;
    }
    set
    {
      this.m_isExpanded = value;
      if (this.Count <= 0)
        return;
      this.m_dictionary.SetNumber("Count", !this.m_isExpanded ? -this.List.Count : this.List.Count);
    }
  }

  public PdfBookmark Add(string title)
  {
    if (title == null)
      throw new ArgumentNullException(nameof (title));
    PdfBookmark previous = this.Count < 1 ? (PdfBookmark) null : this[this.Count - 1];
    PdfBookmark pdfBookmark = new PdfBookmark(title, this, previous, (PdfBookmark) null);
    if (previous != null)
      previous.Next = pdfBookmark;
    this.List.Add((PdfBookmarkBase) pdfBookmark);
    this.UpdateFields();
    return pdfBookmark;
  }

  public bool Contains(PdfBookmark outline) => this.List.Contains((PdfBookmarkBase) outline);

  public void Remove(string title)
  {
    if (title == null)
      throw new ArgumentNullException(nameof (title));
    int index1 = -1;
    if (this.bookmark == null || this.bookmark.Count < this.List.Count)
    {
      this.bookmark = new System.Collections.Generic.List<PdfBookmark>();
      System.Collections.Generic.Dictionary<PdfPageBase, object> dictionary = (System.Collections.Generic.Dictionary<PdfPageBase, object>) null;
      if (this.m_crossTable.Document is PdfLoadedDocument)
        dictionary = (this.m_crossTable.Document as PdfLoadedDocument).CreateBookmarkDestinationDictionary();
      for (int index2 = 0; index2 < this.List.Count; ++index2)
      {
        if (!(this.List[index2] is PdfBookmark))
          throw new Exception("bookmark");
        this.bookmark.Add(this.List[index2] as PdfBookmark);
        if (this.List[index2].List.Count != 0)
        {
          for (int index3 = 0; index3 < this.List[index2].List.Count; ++index3)
            this.bookmark.Add(this.List[index2].List[index3] as PdfBookmark);
        }
      }
      if (dictionary != null)
      {
        this.bookmark = new System.Collections.Generic.List<PdfBookmark>();
        foreach (System.Collections.Generic.List<object> objectList in dictionary.Values)
        {
          foreach (PdfBookmark pdfBookmark in objectList)
            this.bookmark.Add(pdfBookmark);
        }
      }
      if (this.m_booklist == null || this.m_booklist.Count < this.List.Count)
      {
        this.m_booklist = new System.Collections.Generic.List<PdfBookmarkBase>();
        for (int index4 = 0; index4 < this.bookmark.Count; ++index4)
          this.m_booklist.Add((PdfBookmarkBase) this.bookmark[index4]);
      }
    }
    for (int index5 = 0; index5 < this.bookmark.Count; ++index5)
    {
      if (this.bookmark[index5] is PdfLoadedBookmark)
      {
        if ((this.bookmark[index5] as PdfLoadedBookmark).Title.Equals(title))
        {
          index1 = index5;
          break;
        }
      }
      else if (this.bookmark[index5] != null && this.bookmark[index5].Title.Equals(title))
      {
        index1 = index5;
        break;
      }
    }
    for (int index6 = 0; index6 < this.m_booklist.Count; ++index6)
    {
      PdfLoadedBookmark pdfLoadedBookmark = this.m_booklist[index6] as PdfLoadedBookmark;
      PdfBookmark pdfBookmark = this.m_booklist[index6] as PdfBookmark;
      if (pdfLoadedBookmark != null)
      {
        if (pdfLoadedBookmark.Title.Equals(title))
        {
          this.parentIndex = index6;
          break;
        }
      }
      else if (pdfBookmark != null && pdfBookmark.Title.Equals(title))
      {
        this.parentIndex = index6;
        break;
      }
    }
    int count = this.m_booklist.Count;
    this.RemoveAt(index1);
    if (this.parentIndex >= this.m_list.Count || this.parentIndex >= this.m_booklist.Count || count != this.m_booklist.Count)
      return;
    this.m_booklist.RemoveAt(this.parentIndex);
    this.m_list.RemoveAt(this.parentIndex);
  }

  public void RemoveAt(int index)
  {
    if (this.bookmark == null || this.bookmark.Count < this.List.Count)
    {
      this.bookmark = new System.Collections.Generic.List<PdfBookmark>();
      System.Collections.Generic.Dictionary<PdfPageBase, object> dictionary = (System.Collections.Generic.Dictionary<PdfPageBase, object>) null;
      if (this.m_crossTable.Document is PdfLoadedDocument)
        dictionary = (this.m_crossTable.Document as PdfLoadedDocument).CreateBookmarkDestinationDictionary();
      for (int index1 = 0; index1 < this.List.Count; ++index1)
      {
        if (!(this.List[index1] is PdfBookmark))
          throw new Exception("bookmark");
        this.bookmark.Add(this.List[index1] as PdfBookmark);
      }
      if (dictionary != null)
      {
        foreach (System.Collections.Generic.List<object> objectList in dictionary.Values)
        {
          foreach (PdfBookmark pdfBookmark in objectList)
          {
            if (!this.bookmark.Contains(pdfBookmark))
              this.bookmark.Add(pdfBookmark);
          }
        }
      }
      if (this.m_booklist == null || this.m_booklist.Count < this.List.Count)
      {
        this.m_booklist = new System.Collections.Generic.List<PdfBookmarkBase>();
        for (int index2 = 0; index2 < this.bookmark.Count; ++index2)
          this.m_booklist.Add((PdfBookmarkBase) this.bookmark[index2]);
      }
    }
    if (index < 0 || index >= this.bookmark.Count)
      throw new ArgumentOutOfRangeException();
    if (index >= this.List.Count && index >= this.bookmark.Count)
      throw new ArgumentOutOfRangeException();
    if (this.bookmark[index] != null)
    {
      PdfBookmark pdfBookmark = this.bookmark[index];
      if (index == 0)
      {
        if (pdfBookmark.Dictionary.ContainsKey("Next"))
          this.m_dictionary.SetProperty("First", pdfBookmark.Dictionary["Next"]);
        else if (!pdfBookmark.Dictionary.ContainsKey("Prev"))
        {
          if (this.List.Count > 1)
          {
            this.m_dictionary.SetProperty("First", pdfBookmark.Dictionary["First"]);
          }
          else
          {
            this.m_dictionary.Remove("First");
            this.m_dictionary.Remove("Last");
          }
        }
        else
          this.m_dictionary.SetProperty("First", pdfBookmark.Dictionary["Next"]);
      }
      else if (pdfBookmark.Parent != null && pdfBookmark.Previous == null && pdfBookmark.Next != null)
      {
        pdfBookmark.Parent.Dictionary.SetProperty("First", pdfBookmark.Dictionary["Next"]);
        pdfBookmark.Next.Dictionary.Remove("Prev");
      }
      else if (pdfBookmark.Parent != null && pdfBookmark.Previous != null && pdfBookmark.Next != null)
      {
        pdfBookmark.Previous.Dictionary.SetProperty("Next", pdfBookmark.Dictionary["Next"]);
        PdfReferenceHolder pointer = pdfBookmark.Dictionary["Next"] as PdfReferenceHolder;
        if (pointer != (PdfReferenceHolder) null && this.m_crossTable.CrossTable != null)
          (this.m_crossTable.GetObject((IPdfPrimitive) pointer) as PdfDictionary).SetProperty("Prev", pdfBookmark.Dictionary["Prev"]);
      }
      else if (pdfBookmark.Parent != null && pdfBookmark.Previous != null && pdfBookmark.Next == null)
      {
        pdfBookmark.Previous.Dictionary.Remove("Next");
        pdfBookmark.Parent.Dictionary.SetProperty("Last", pdfBookmark.Dictionary["Prev"]);
      }
      else
        pdfBookmark.Parent.Dictionary.Remove("First");
    }
    else if (this.bookmark[index] is PdfLoadedBookmark)
    {
      PdfLoadedBookmark pdfLoadedBookmark = this.bookmark[index] as PdfLoadedBookmark;
      if (index == 0)
      {
        if (pdfLoadedBookmark.Dictionary.ContainsKey("Next"))
          this.m_dictionary.SetProperty("First", pdfLoadedBookmark.Dictionary["Next"]);
        else if (!pdfLoadedBookmark.Dictionary.ContainsKey("Prev"))
        {
          if (this.List.Count > 1)
          {
            this.m_dictionary.SetProperty("First", pdfLoadedBookmark.Dictionary["First"]);
          }
          else
          {
            this.m_dictionary.Remove("First");
            this.m_dictionary.Remove("Last");
          }
        }
        else
          this.m_dictionary.SetProperty("First", pdfLoadedBookmark.Dictionary["Next"]);
      }
      else if (pdfLoadedBookmark.Parent != null && pdfLoadedBookmark.Previous == null && pdfLoadedBookmark.Next != null)
      {
        pdfLoadedBookmark.Parent.Dictionary.SetProperty("First", pdfLoadedBookmark.Dictionary["Next"]);
        pdfLoadedBookmark.Next.Dictionary.Remove("Prev");
      }
      else if (pdfLoadedBookmark.Parent != null && pdfLoadedBookmark.Previous != null && pdfLoadedBookmark.Next != null)
      {
        pdfLoadedBookmark.Previous.Dictionary.SetProperty("Next", pdfLoadedBookmark.Dictionary["Next"]);
        PdfReferenceHolder pointer = pdfLoadedBookmark.Dictionary["Next"] as PdfReferenceHolder;
        if (pointer != (PdfReferenceHolder) null)
          (this.m_crossTable.GetObject((IPdfPrimitive) pointer) as PdfDictionary).SetProperty("Prev", pdfLoadedBookmark.Dictionary["Prev"]);
      }
      else if (pdfLoadedBookmark.Parent != null && pdfLoadedBookmark.Previous != null && pdfLoadedBookmark.Next == null)
      {
        pdfLoadedBookmark.Previous.Dictionary.Remove("Next");
        pdfLoadedBookmark.Parent.Dictionary.SetProperty("Last", pdfLoadedBookmark.Dictionary["Prev"]);
      }
      else
        pdfLoadedBookmark.Parent.Dictionary.Remove("First");
    }
    if (index < this.m_list.Count && index < this.m_booklist.Count)
    {
      this.m_list.RemoveAt(index);
      this.m_booklist.RemoveAt(index);
    }
    this.UpdateFields();
    this.bookmark.RemoveAt(index);
  }

  public void Clear()
  {
    if (this.CrossTable != null && this.CrossTable.Document != null)
    {
      for (int index = this.Count - 1; index >= 0; --index)
        this.RemoveAt(index);
    }
    this.List.Clear();
    if (this.m_booklist == null)
      return;
    this.m_booklist.Clear();
  }

  public PdfBookmark Insert(int index, string title)
  {
    if (title == null)
      throw new ArgumentNullException(nameof (title));
    if (index < 0 || index > this.Count)
      throw new IndexOutOfRangeException();
    if (title == null)
      throw new ArgumentNullException(nameof (title));
    PdfBookmark pdfBookmark;
    if (index == this.Count)
    {
      pdfBookmark = this.Add(title);
    }
    else
    {
      PdfBookmark next = this[index];
      PdfBookmark previous = index == 0 ? (PdfBookmark) null : this[index - 1];
      pdfBookmark = new PdfBookmark(title, this, previous, next);
      this.List.Insert(index, (PdfBookmarkBase) pdfBookmark);
      if (previous != null)
        previous.Next = pdfBookmark;
      next.Previous = pdfBookmark;
      this.UpdateFields();
    }
    return pdfBookmark;
  }

  private void GetBookmarkCollection(System.Collections.Generic.List<PdfBookmark> pageBookmarks, System.Collections.Generic.List<PdfBookmark> bookmarks)
  {
    if (pageBookmarks == null)
      return;
    foreach (object pageBookmark in pageBookmarks)
      bookmarks.Add(pageBookmark as PdfBookmark);
  }

  internal void Dispose()
  {
    this.List.Clear();
    if (this.m_booklist == null)
      return;
    this.m_booklist.Clear();
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.List.GetEnumerator();

  internal void ReproduceTree()
  {
    PdfLoadedBookmark pdfLoadedBookmark = this.GetFirstBookMark(this);
    for (bool flag = pdfLoadedBookmark != null; flag && pdfLoadedBookmark.Dictionary != null; flag = pdfLoadedBookmark != null)
    {
      pdfLoadedBookmark.SetParent(this);
      this.m_list.Add((PdfBookmarkBase) pdfLoadedBookmark);
      pdfLoadedBookmark = pdfLoadedBookmark.Next as PdfLoadedBookmark;
    }
  }

  private void UpdateFields()
  {
    if (this.Count > 0)
    {
      this.m_dictionary.SetNumber("Count", this.IsExpanded || !this.Dictionary.ContainsKey("Count") ? this.List.Count : -this.List.Count);
      this.m_dictionary.SetProperty("First", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this[0]));
      this.m_dictionary.SetProperty("Last", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this[this.Count - 1]));
    }
    else
      this.m_dictionary.Clear();
    this.m_dictionary.Modify();
  }

  private PdfLoadedBookmark GetFirstBookMark(PdfBookmarkBase bookmark)
  {
    PdfLoadedBookmark firstBookMark = (PdfLoadedBookmark) null;
    PdfDictionary dictionary = bookmark.Dictionary;
    if (dictionary.ContainsKey("First"))
      firstBookMark = new PdfLoadedBookmark(this.CrossTable.GetObject(dictionary["First"]) as PdfDictionary, this.CrossTable);
    return firstBookMark;
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;
}
