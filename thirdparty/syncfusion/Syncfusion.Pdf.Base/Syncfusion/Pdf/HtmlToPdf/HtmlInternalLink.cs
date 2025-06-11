// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.HtmlToPdf.HtmlInternalLink
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Interactive;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.HtmlToPdf;

internal class HtmlInternalLink
{
  private const int maxHeaderLevel = 6;
  private string m_href;
  private string m_sourcePageNumber;
  private RectangleF m_bounds;
  private string m_headerTagLevel;
  private string m_id;
  private string m_headerContent;
  private int m_destinationPageNumber;
  private PointF m_destination;
  private PdfPageBase m_destinationPage;
  private float m_tocXcoordinate;
  private float m_tocRectangleHeight;
  private int m_tocPagecount;
  private float m_bottomMargin;

  internal string Href
  {
    get => this.m_href;
    set => this.m_href = value;
  }

  internal string SourcePageNumber
  {
    get => this.m_sourcePageNumber;
    set => this.m_sourcePageNumber = value;
  }

  internal RectangleF Bounds
  {
    get => this.m_bounds;
    set => this.m_bounds = value;
  }

  internal string HeaderTagLevel
  {
    get => this.m_headerTagLevel;
    set => this.m_headerTagLevel = value;
  }

  internal string ID
  {
    get => this.m_id;
    set => this.m_id = value;
  }

  internal string HeaderContent
  {
    get => this.m_headerContent;
    set => this.m_headerContent = value;
  }

  internal int DestinationPageNumber
  {
    get => this.m_destinationPageNumber;
    set => this.m_destinationPageNumber = value;
  }

  internal PdfPageBase DestinationPage
  {
    get => this.m_destinationPage;
    set => this.m_destinationPage = value;
  }

  internal PointF Destination
  {
    get => this.m_destination;
    set => this.m_destination = value;
  }

  internal float TocXcoordinate
  {
    get => this.m_tocXcoordinate;
    set => this.m_tocXcoordinate = value;
  }

  internal float TocRectHeight
  {
    get => this.m_tocRectangleHeight;
    set => this.m_tocRectangleHeight = value;
  }

  internal int TocPageCount
  {
    get => this.m_tocPagecount;
    set => this.m_tocPagecount = value;
  }

  internal void AddBookmark(
    PdfPage page,
    PdfDocument lDoc,
    List<HtmlInternalLink> internalLinkCollection)
  {
    PdfBookmark[] bookmarkCollection = new PdfBookmark[internalLinkCollection.Count];
    int num1 = 6;
    int num2 = 0;
    int index1 = 0;
    HtmlInternalLink internalLink1 = internalLinkCollection[index1];
    for (int index2 = 0; index2 < internalLinkCollection.Count; ++index2)
    {
      HtmlInternalLink internalLink2 = internalLinkCollection[index2];
      if (internalLink2.HeaderTagLevel != null)
      {
        int num3 = int.Parse(internalLink2.HeaderTagLevel.Split('H')[1]);
        if (num3 <= num1)
        {
          PdfBookmark pdfBookmark = page.Document.Bookmarks.Add(internalLink2.HeaderContent);
          pdfBookmark.Destination = new PdfDestination((PdfPageBase) lDoc.Pages[internalLink2.DestinationPageNumber + this.TocPageCount - 1]);
          pdfBookmark.Destination.Location = new PointF(internalLink2.Destination.X, internalLink2.Destination.Y);
          num1 = num3;
          num2 = num3;
          bookmarkCollection[index2] = pdfBookmark;
        }
        else
        {
          int prevIndex1 = 0;
          if (num3 > num2)
          {
            this.AddChildBookmark(index2, lDoc, bookmarkCollection, prevIndex1, internalLinkCollection);
            num2 = num3;
          }
          else
          {
            string headerTagLevel;
            char[] chArray;
            do
            {
              HtmlInternalLink internalLink3;
              do
              {
                ++prevIndex1;
                internalLink3 = internalLinkCollection[index2 - prevIndex1];
              }
              while (internalLink3.HeaderTagLevel == null);
              headerTagLevel = internalLink3.HeaderTagLevel;
              chArray = new char[1]{ 'H' };
            }
            while (int.Parse(headerTagLevel.Split(chArray)[1]) >= num3);
            int prevIndex2 = prevIndex1 - 1;
            this.AddChildBookmark(index2, lDoc, bookmarkCollection, prevIndex2, internalLinkCollection);
            num2 = num3;
          }
        }
      }
    }
  }

  private void AddChildBookmark(
    int index,
    PdfDocument lDoc,
    PdfBookmark[] bookmarkCollection,
    int prevIndex,
    List<HtmlInternalLink> internalLinkCollection)
  {
    HtmlInternalLink internalLink = internalLinkCollection[index];
    PdfBookmark pdfBookmark = bookmarkCollection[index - (prevIndex + 1)].Add(internalLink.HeaderContent);
    pdfBookmark.Destination = new PdfDestination((PdfPageBase) lDoc.Pages[internalLink.DestinationPageNumber + this.TocPageCount - 1]);
    pdfBookmark.Destination.Location = new PointF(internalLink.Destination.X, internalLink.Destination.Y);
    bookmarkCollection[index] = pdfBookmark;
  }
}
