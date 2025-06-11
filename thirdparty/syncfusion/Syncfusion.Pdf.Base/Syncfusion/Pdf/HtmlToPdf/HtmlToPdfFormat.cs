// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.HtmlToPdf.HtmlToPdfFormat
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Graphics.Images.Metafiles;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.HtmlToPdf;

internal class HtmlToPdfFormat
{
  private bool m_boundsSet;
  private RectangleF m_paginateBounds;
  private PdfLayoutType m_layout;
  private PdfLayoutBreakType m_break;
  private bool m_splitTextLines;
  private bool m_splitImages;
  internal float TotalPageLayoutSize;
  internal int PageCount;
  internal int PageNumber;
  internal double TotalPageSize;
  private ImageRegionManager m_imageRegionManager = new ImageRegionManager();
  private TextRegionManager m_textRegionManager = new TextRegionManager();
  private ImageRegionManager m_formRegionManager = new ImageRegionManager();
  private ArrayList m_htmlHyperlinksCollection = new ArrayList();
  private List<HtmlInternalLink> m_htmlInternalLinksCollection = new List<HtmlInternalLink>();

  public bool SplitTextLines
  {
    get => this.m_splitTextLines;
    set => this.m_splitTextLines = value;
  }

  public bool SplitImages
  {
    get => this.m_splitImages;
    set => this.m_splitImages = value;
  }

  public PdfLayoutType Layout
  {
    get => this.m_layout;
    set => this.m_layout = value;
  }

  public PdfLayoutBreakType Break
  {
    get => this.m_break;
    set => this.m_break = value;
  }

  public RectangleF PaginateBounds
  {
    get => this.m_paginateBounds;
    set
    {
      this.m_paginateBounds = value;
      this.m_boundsSet = true;
    }
  }

  internal TextRegionManager TextRegionManager
  {
    get => this.m_textRegionManager;
    set => this.m_textRegionManager = value;
  }

  internal ArrayList HtmlHyperlinksCollection
  {
    get => this.m_htmlHyperlinksCollection;
    set => this.m_htmlHyperlinksCollection = value;
  }

  internal List<HtmlInternalLink> HtmlInternalLinksCollection
  {
    get => this.m_htmlInternalLinksCollection;
    set => this.m_htmlInternalLinksCollection = value;
  }

  internal ImageRegionManager ImageRegionManager
  {
    get => this.m_imageRegionManager;
    set => this.m_imageRegionManager = value;
  }

  internal ImageRegionManager FormRegionManager
  {
    get => this.m_formRegionManager;
    set => this.m_formRegionManager = value;
  }

  internal bool UsePaginateBounds => this.m_boundsSet;
}
