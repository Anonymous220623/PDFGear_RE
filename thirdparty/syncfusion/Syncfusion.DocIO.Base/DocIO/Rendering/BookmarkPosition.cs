// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.Rendering.BookmarkPosition
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.Rendering;

internal class BookmarkPosition
{
  private RectangleF m_bounds;
  private int m_pageNumber;
  private int m_bookmarkStyle;
  private string m_bookmarkName;

  internal int BookmarkStyle
  {
    get => this.m_bookmarkStyle;
    set => this.m_bookmarkStyle = value;
  }

  public RectangleF Bounds
  {
    get => this.m_bounds;
    set => this.m_bounds = value;
  }

  public int PageNumber
  {
    get => this.m_pageNumber;
    set => this.m_pageNumber = value;
  }

  public string BookmarkName
  {
    get => this.m_bookmarkName;
    set => this.m_bookmarkName = value;
  }

  public BookmarkPosition(string bookmarkName, int pageNumber, RectangleF bounds)
  {
    this.BookmarkName = bookmarkName;
    this.PageNumber = pageNumber;
    this.Bounds = bounds;
  }

  internal BookmarkPosition(string bookmarkName, int pageNumber, RectangleF bounds, int level)
    : this(bookmarkName, pageNumber, bounds)
  {
    this.m_bookmarkStyle = level + 1;
  }
}
