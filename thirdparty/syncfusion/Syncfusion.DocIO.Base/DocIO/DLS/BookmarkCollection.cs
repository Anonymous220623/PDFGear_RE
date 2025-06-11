// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.BookmarkCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class BookmarkCollection : CollectionImpl
{
  public Bookmark this[string name] => this.FindByName(name);

  public Bookmark this[int index] => this.InnerList[index] as Bookmark;

  internal BookmarkCollection(WordDocument doc)
    : base(doc, (OwnerHolder) doc)
  {
  }

  public Bookmark FindByName(string name)
  {
    name.Replace('-', '_');
    for (int index = 0; index < this.InnerList.Count; ++index)
    {
      Bookmark inner = this.InnerList[index] as Bookmark;
      if (inner.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
        return inner;
    }
    return (Bookmark) null;
  }

  public void RemoveAt(int index) => this.Remove(this.InnerList[index] as Bookmark);

  public void Remove(Bookmark bookmark)
  {
    this.InnerList.Remove((object) bookmark);
    BookmarkStart bookmarkStart = bookmark.BookmarkStart;
    BookmarkEnd bookmarkEnd = bookmark.BookmarkEnd;
    bookmarkStart?.RemoveSelf();
    bookmarkEnd?.RemoveSelf();
  }

  public void Clear()
  {
    while (this.InnerList.Count > 0)
      this.RemoveAt(this.InnerList.Count - 1);
  }

  internal void Add(Bookmark bookmark) => this.InnerList.Add((object) bookmark);

  internal void AttachBookmarkStart(BookmarkStart bookmarkStart)
  {
    if (this[bookmarkStart.Name] != null)
    {
      bookmarkStart.SetName(bookmarkStart.Name + Guid.NewGuid().ToString());
      bookmarkStart.RemoveSelf();
    }
    else
      this.Add(new Bookmark(bookmarkStart));
  }

  internal void AttachBookmarkEnd(BookmarkEnd bookmarkEnd)
  {
    Bookmark bookmark = this[bookmarkEnd.Name];
    if (bookmark == null)
      return;
    BookmarkEnd bookmarkEnd1 = bookmark.BookmarkEnd;
    if (bookmarkEnd1 != null)
    {
      bookmarkEnd.RemoveSelf();
      if (bookmark.BookmarkEnd != null)
        return;
      bookmark.SetEnd(bookmarkEnd1);
    }
    else
      bookmark.SetEnd(bookmarkEnd);
  }
}
