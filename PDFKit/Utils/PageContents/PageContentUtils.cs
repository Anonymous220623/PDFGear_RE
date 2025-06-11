// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PageContents.PageContentUtils
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace PDFKit.Utils.PageContents;

public static class PageContentUtils
{
  public static void GenerateContentAdvance(this PdfPage page, bool imagesOnly = false)
  {
    page.GenerateContentAdvance(new GenerateContentOptions()
    {
      ImagesOnly = imagesOnly
    });
  }

  public static void GenerateContentAdvance(this PdfPage page, GenerateContentOptions options)
  {
    if (page == null)
      return;
    if (options == null)
      options = new GenerateContentOptions();
    Dictionary<int, PageFormObjectUtils.InternalHeaderFooterData> dict = (Dictionary<int, PageFormObjectUtils.InternalHeaderFooterData>) null;
    if (options.KeepHeaderFooterData)
      dict = PageFormObjectUtils.GetHeaderFooterDataForGenerateContent(page);
    page.GenerateContent(options.ImagesOnly);
    page.ReloadPage();
    if (options.KeepHeaderFooterData)
      PageFormObjectUtils.ApplyHeaderFooterDataFromGenerateContent(page, dict);
    PageObjectHitTestHelper.GetHitTestCacheCore(page.Document).AddOrUpdateItemCore(page);
  }

  public static PdfTextObject[] UpdateTextObjectContent(
    PdfPage page,
    PdfTextObject textObject,
    string text)
  {
    return page == null || textObject == null ? Array.Empty<PdfTextObject>() : PdfTextObjectUtils.UpdateTextObjectContent(page, textObject, text);
  }

  public static PdfTextObject[] GetIntersectingTextObjects(PdfPage page, FS_RECTF rect)
  {
    if (page?.Document == null)
      return Array.Empty<PdfTextObject>();
    return (double) rect.Width <= 0.0 || (double) rect.Height <= 0.0 ? Array.Empty<PdfTextObject>() : ((IEnumerable<PdfTextObject>) PageContentUtils.GetIntersectingTextObjectsCore(page, rect)).Where<PdfTextObject>((Func<PdfTextObject, bool>) (c => PdfTextObjectUtils.ShouldRemoveTextObject(rect, c))).ToArray<PdfTextObject>();
  }

  internal static PdfTextObject[] GetIntersectingTextObjectsCore(PdfPage page, FS_RECTF rect)
  {
    if (page?.Document == null)
      return Array.Empty<PdfTextObject>();
    if ((double) rect.Width <= 0.0 || (double) rect.Height <= 0.0)
      return Array.Empty<PdfTextObject>();
    return PageObjectHitTestHelper.GetIntersectingObjects(page.Document, page.PageIndex, rect, new PageObjectTypes[1]
    {
      PageObjectTypes.PDFPAGE_TEXT
    }).Cast<PdfTextObject>().ToArray<PdfTextObject>();
  }

  public static RemoveIntersectingTextResult RemoveIntersectingText(
    System.Collections.Generic.IReadOnlyList<PdfTextObject> textObjects,
    FS_RECTF rect)
  {
    if (textObjects == null || textObjects.Count == 0 || textObjects.Any<PdfTextObject>((Func<PdfTextObject, bool>) (c => c?.Container == null)))
      return new RemoveIntersectingTextResult(false, (System.Collections.Generic.IReadOnlyList<PdfTextObject>) null, (System.Collections.Generic.IReadOnlyList<PdfTextObject>) null);
    return (double) rect.Width <= 0.0 || (double) rect.Height <= 0.0 ? new RemoveIntersectingTextResult(false, (System.Collections.Generic.IReadOnlyList<PdfTextObject>) null, (System.Collections.Generic.IReadOnlyList<PdfTextObject>) null) : PdfTextObjectUtils.RemoveIntersectingText(textObjects, rect);
  }

  public static System.Collections.Generic.IReadOnlyList<PdfTextObject> GetSelectedTextObject(
    PdfPage page,
    int selectRangeStartIndex,
    int selectRangeEndIndex,
    out PdfTextObject startObject,
    out int startObjectFirstCharIndex,
    out PdfTextObject endObject,
    out int endObjectLastCharIndex)
  {
    return PdfTextObjectUtils.PdfPageTextIndexHelper.GetSelectedTextObject(page, selectRangeStartIndex, selectRangeEndIndex, out startObject, out startObjectFirstCharIndex, out endObject, out endObjectLastCharIndex);
  }

  public static RemoveIntersectingTextResult RemoveSelectedText(
    System.Collections.Generic.IReadOnlyList<PdfTextObject> textObjects,
    PdfTextObject startObject,
    int startObjectFirstCharIndex,
    PdfTextObject endObject,
    int endObjectLastCharIndex)
  {
    return textObjects == null || textObjects.Count == 0 || textObjects.Any<PdfTextObject>((Func<PdfTextObject, bool>) (c => c?.Container == null)) ? new RemoveIntersectingTextResult(false, (System.Collections.Generic.IReadOnlyList<PdfTextObject>) null, (System.Collections.Generic.IReadOnlyList<PdfTextObject>) null) : PdfTextObjectUtils.PdfPageTextIndexHelper.RemoveSelectedText(textObjects, startObject, startObjectFirstCharIndex, endObject, endObjectLastCharIndex);
  }

  public static bool CanRemoveSelectedText(
    PdfPage page,
    int selectRangeStartIndex,
    int selectRangeEndIndex)
  {
    if (page == null || selectRangeStartIndex < 0 || selectRangeEndIndex < 0 || selectRangeStartIndex > selectRangeEndIndex)
      return false;
    int countChars = page.Text.CountChars;
    if (selectRangeStartIndex >= countChars || selectRangeEndIndex >= countChars)
      return false;
    for (int charIndexInPage = selectRangeStartIndex; charIndexInPage <= selectRangeEndIndex; ++charIndexInPage)
    {
      if (PdfTextObjectUtils.PdfPageTextIndexHelper.GetIntersectingTextObjects(page, charIndexInPage, out FS_RECTF _).Length != 0)
        return true;
    }
    return false;
  }

  public static WriteableBitmap GetPageObjectImage(
    PdfPage page,
    PdfPageObject pageObject,
    Color placeholderColor)
  {
    PdfDocument document = page?.Document;
    if (document == null)
      return (WriteableBitmap) null;
    PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(document);
    if (pdfControl == null)
      return (WriteableBitmap) null;
    try
    {
      FS_RECTF boundingBox = pageObject.BoundingBox;
      Rect clientRect;
      if ((double) boundingBox.Width <= 0.0 || (double) boundingBox.Height <= 0.0 || !pdfControl.TryGetClientRect(page.PageIndex, boundingBox, out clientRect))
        return (WriteableBitmap) null;
      DpiScale dpi = VisualTreeHelper.GetDpi((Visual) pdfControl);
      if (pageObject is PdfTextObject textObj)
        return PdfTextObjectUtils.GetTextObjectImage(page, textObj, clientRect, dpi.PixelsPerDip, placeholderColor);
    }
    catch
    {
    }
    return (WriteableBitmap) null;
  }
}
