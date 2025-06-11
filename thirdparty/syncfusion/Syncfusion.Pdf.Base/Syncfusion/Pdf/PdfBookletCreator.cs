// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfBookletCreator
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf;

public sealed class PdfBookletCreator
{
  private PdfBookletCreator()
  {
    throw new NotSupportedException("Instantination of BookletCreator class is not supported");
  }

  public static PdfDocument CreateBooklet(PdfLoadedDocument loadedDocument, SizeF pageSize)
  {
    return PdfBookletCreator.CreateBooklet(loadedDocument, pageSize, false);
  }

  public static void CreateBooklet(string from, string into, SizeF pageSize, bool twoSide)
  {
    if (from == null)
      throw new ArgumentNullException(nameof (from));
    if (from == string.Empty)
      throw new ArgumentOutOfRangeException(nameof (from), "Parameter can not be empty");
    if (into == null)
      throw new ArgumentNullException(nameof (into));
    if (into == string.Empty)
      throw new ArgumentOutOfRangeException(nameof (into), "Parameter can not be empty");
    if (pageSize == SizeF.Empty)
      throw new ArgumentOutOfRangeException(nameof (pageSize), "Parameter can not be empty");
    PdfDocument booklet = PdfBookletCreator.CreateBooklet(new PdfLoadedDocument(from), pageSize, twoSide);
    booklet.Save(into);
    booklet.Close();
  }

  public static void CreateBooklet(string from, string into, SizeF pageSize)
  {
    PdfBookletCreator.CreateBooklet(from, into, pageSize, false);
  }

  public static PdfDocument CreateBooklet(
    PdfLoadedDocument loadedDocument,
    SizeF pageSize,
    bool twoSide)
  {
    if (loadedDocument == null)
      throw new ArgumentNullException(nameof (loadedDocument));
    if (pageSize == SizeF.Empty)
      throw new ArgumentOutOfRangeException(nameof (pageSize), "Parameter can not be empty");
    SizeF size = new SizeF(pageSize.Width / 2f, pageSize.Height);
    PointF empty = PointF.Empty;
    PointF location = new PointF(size.Width, 0.0f);
    PdfDocument booklet = new PdfDocument();
    booklet.PageSettings.Margins.All = 0.0f;
    int count = loadedDocument.Pages.Count;
    PdfLoadedPageCollection pages = loadedDocument.Pages;
    int num = count / 2 + count % 2;
    bool flag = false;
    if (twoSide)
      flag = num % 2 == 0;
    for (int index1 = 0; index1 < num; ++index1)
    {
      booklet.PageSettings.Size = pageSize;
      if ((double) pageSize.Width > (double) pageSize.Height)
        booklet.PageSettings.Orientation = PdfPageOrientation.Landscape;
      PdfPage pdfPage = booklet.Pages.Add();
      int[] nextPair = PdfBookletCreator.GetNextPair(index1, count, twoSide);
      int index2 = !twoSide || !flag ? 0 : 1;
      int index3 = nextPair[index2];
      if (index3 >= 0)
      {
        PdfTemplate template = pages[index3].CreateTemplate();
        pdfPage.Graphics.DrawPdfTemplate(template, empty, size);
      }
      int index4 = flag ? 0 : 1;
      int index5 = nextPair[index4];
      if (index5 >= 0)
      {
        PdfTemplate template = pages[index5].CreateTemplate();
        pdfPage.Graphics.DrawPdfTemplate(template, location, size);
      }
    }
    return booklet;
  }

  public static PdfDocument CreateBooklet(
    PdfLoadedDocument loadedDocument,
    SizeF pageSize,
    bool twoSide,
    PdfMargins margin)
  {
    if (loadedDocument == null)
      throw new ArgumentNullException(nameof (loadedDocument));
    if (pageSize == SizeF.Empty)
      throw new ArgumentOutOfRangeException(nameof (pageSize), "Parameter can not be empty");
    SizeF size = new SizeF(pageSize.Width / 2f, pageSize.Height);
    PointF empty = PointF.Empty;
    PointF location = new PointF(size.Width, 0.0f);
    PdfDocument booklet = new PdfDocument();
    booklet.PageSettings.Margins = margin;
    int count = loadedDocument.Pages.Count;
    PdfLoadedPageCollection pages = loadedDocument.Pages;
    int num = count / 2 + count % 2;
    bool flag = false;
    if (twoSide)
      flag = num % 2 == 0;
    for (int index1 = 0; index1 < num; ++index1)
    {
      booklet.PageSettings.Size = pageSize;
      PdfPage pdfPage = booklet.Pages.Add();
      int[] nextPair = PdfBookletCreator.GetNextPair(index1, count, twoSide);
      int index2 = !twoSide || !flag ? 0 : 1;
      int index3 = nextPair[index2];
      if (index3 >= 0)
      {
        PdfTemplate template = pages[index3].CreateTemplate();
        pdfPage.Graphics.DrawPdfTemplate(template, empty, size);
      }
      int index4 = flag ? 0 : 1;
      int index5 = nextPair[index4];
      if (index5 >= 0)
      {
        PdfTemplate template = pages[index5].CreateTemplate();
        pdfPage.Graphics.DrawPdfTemplate(template, location, size);
      }
    }
    return booklet;
  }

  private static int[] GetNextPair(int index, int count, bool twoSide)
  {
    int[] nextPair = new int[2];
    int num = count - index - (count + 1) % 2;
    if (num == count)
      num = -1;
    if (twoSide && index % 2 > 0)
    {
      nextPair[1] = index;
      nextPair[0] = num;
    }
    else
    {
      nextPair[0] = index;
      nextPair[1] = num;
    }
    return nextPair;
  }
}
