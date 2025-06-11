// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Rendering.PageResult
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS.Rendering;

internal class PageResult
{
  private Image m_image;
  private List<Dictionary<string, RectangleF>> m_hyperLinks;
  private List<Dictionary<string, DocumentLayouter.BookmarkHyperlink>> m_bookmarkHyperlinks;

  public Image PageImage
  {
    get => this.m_image;
    set => this.m_image = value;
  }

  public List<Dictionary<string, RectangleF>> Hyperlinks
  {
    get => this.m_hyperLinks;
    set => this.m_hyperLinks = value;
  }

  public List<Dictionary<string, DocumentLayouter.BookmarkHyperlink>> BookmarkHyperlinks
  {
    get => this.m_bookmarkHyperlinks;
    set => this.m_bookmarkHyperlinks = value;
  }

  public PageResult()
  {
  }

  public PageResult(
    Image image,
    List<Dictionary<string, RectangleF>> hyperlinks,
    List<Dictionary<string, DocumentLayouter.BookmarkHyperlink>> bookmarkHyperlinks)
  {
    this.m_image = image;
    this.m_hyperLinks = hyperlinks;
    this.m_bookmarkHyperlinks = bookmarkHyperlinks;
  }
}
