// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Bookmark
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class Bookmark
{
  private BookmarkStart m_bkmkStart;
  private BookmarkEnd m_bkmkEnd;

  public string Name => this.m_bkmkStart.Name;

  public BookmarkStart BookmarkStart => this.m_bkmkStart;

  public BookmarkEnd BookmarkEnd => this.m_bkmkEnd;

  public short FirstColumn
  {
    get => this.BookmarkStart != null ? this.BookmarkStart.ColumnFirst : (short) -1;
    set
    {
      if (this.BookmarkStart == null)
        return;
      this.BookmarkStart.ColumnFirst = value;
    }
  }

  public short LastColumn
  {
    get => this.BookmarkStart != null ? this.BookmarkStart.ColumnLast : (short) -1;
    set
    {
      if (this.BookmarkStart == null)
        return;
      this.BookmarkStart.ColumnLast = value;
    }
  }

  public Bookmark(BookmarkStart start)
    : this(start, (BookmarkEnd) null)
  {
  }

  public Bookmark(BookmarkStart start, BookmarkEnd end)
  {
    this.m_bkmkStart = start;
    this.m_bkmkEnd = end;
  }

  internal void SetStart(BookmarkStart start) => this.m_bkmkStart = start;

  internal void SetEnd(BookmarkEnd end)
  {
    if (!this.HasValidPosition(end))
      return;
    this.m_bkmkEnd = end;
  }

  private bool HasValidPosition(BookmarkEnd bookmarkEnd)
  {
    InlineContentControl owner1 = bookmarkEnd == null || bookmarkEnd.Owner == null ? (InlineContentControl) null : bookmarkEnd.Owner as InlineContentControl;
    if (owner1 != null && owner1.ContentControlProperties.Type == ContentControlType.Text)
    {
      InlineContentControl owner2 = this.BookmarkStart == null || this.BookmarkStart.Owner == null ? (InlineContentControl) null : this.BookmarkStart.Owner as InlineContentControl;
      string message = $"Cannot add Bookmark with name \"{bookmarkEnd.Name}\" at invalid positions in the Content Control";
      if (owner2 == null || owner2.ContentControlProperties.Type != ContentControlType.RichText && owner2.ContentControlProperties.Type != ContentControlType.Text && owner2.ContentControlProperties.Type != ContentControlType.BuildingBlockGallery && owner2.ContentControlProperties.Type != ContentControlType.RepeatingSection)
        throw new Exception(message);
      for (int index = owner2.ParagraphItems.IndexOf((IEntity) this.BookmarkStart); index >= 0; --index)
      {
        switch ((Entity) owner2.ParagraphItems[index])
        {
          case BookmarkStart _:
          case BookmarkEnd _:
          case EditableRangeStart _:
          case EditableRangeEnd _:
            continue;
          default:
            throw new Exception(message);
        }
      }
    }
    return true;
  }
}
