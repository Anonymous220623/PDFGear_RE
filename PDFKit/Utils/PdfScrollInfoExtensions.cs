// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PdfScrollInfoExtensions
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using System;
using System.Windows;

#nullable disable
namespace PDFKit.Utils;

public static class PdfScrollInfoExtensions
{
  public static (int startPage, int endPage) GetVisiblePageRange(
    this IPdfScrollInfoInternal scrollInfo)
  {
    if (scrollInfo == null || scrollInfo.Document == null)
      return (-1, -1);
    Rect rect1 = new Rect(0.0, 0.0, scrollInfo.ViewportWidth, scrollInfo.ViewportHeight);
    int count = scrollInfo.Document.Pages.Count;
    int currentIndex = scrollInfo.Document.Pages.CurrentIndex;
    int index1 = currentIndex;
    int index2 = currentIndex;
    int startPage = scrollInfo.StartPage;
    int endPage = scrollInfo.EndPage;
    int num = 0;
    for (; index1 >= startPage; --index1)
    {
      Rect rect2 = scrollInfo.CalcActualRect(index1);
      if (!rect1.IntersectsWith(rect2))
        ++num;
      else
        num = 0;
      if (num > 3)
        break;
    }
    int val1_1 = index1 + num;
    for (; index2 <= endPage; ++index2)
    {
      Rect rect3 = scrollInfo.CalcActualRect(index2);
      if (!rect1.IntersectsWith(rect3))
        ++num;
      else
        num = 0;
      if (num > 3)
        break;
    }
    int val2 = index2 - num;
    int val1_2 = Math.Min(val1_1, val2);
    int val1_3 = Math.Max(val1_2, val2);
    return (Math.Max(val1_2, scrollInfo.StartPage), Math.Min(val1_3, scrollInfo.EndPage));
  }

  public static bool TryGetPagePoint(
    this IPdfScrollInfoInternal scrollInfo,
    int pageIndex,
    Point clientPoint,
    out Point pagePoint)
  {
    pagePoint = new Point();
    if (scrollInfo == null)
      return false;
    int startPage = scrollInfo.StartPage;
    int endPage = scrollInfo.EndPage;
    if (pageIndex < startPage || pageIndex > endPage)
      return false;
    pagePoint = scrollInfo.ClientToPage(pageIndex, clientPoint);
    return true;
  }

  public static bool TryGetPageRect(
    this IPdfScrollInfoInternal scrollInfo,
    int pageIndex,
    Rect clientRect,
    out FS_RECTF pageRect)
  {
    pageRect = new FS_RECTF();
    if (scrollInfo == null)
      return false;
    int startPage = scrollInfo.StartPage;
    int endPage = scrollInfo.EndPage;
    if (pageIndex < startPage || pageIndex > endPage)
      return false;
    pageRect = scrollInfo.ClientToPageRect(clientRect, pageIndex);
    return true;
  }

  public static bool TryGetClientPoint(
    this IPdfScrollInfoInternal scrollInfo,
    int pageIndex,
    Point pagePoint,
    out Point clientPoint)
  {
    clientPoint = new Point();
    if (scrollInfo == null)
      return false;
    int startPage = scrollInfo.StartPage;
    int endPage = scrollInfo.EndPage;
    if (pageIndex < startPage || pageIndex > endPage)
      return false;
    clientPoint = scrollInfo.PageToClient(pageIndex, pagePoint);
    return true;
  }

  public static bool TryGetClientRect(
    this IPdfScrollInfoInternal scrollInfo,
    int pageIndex,
    FS_RECTF pageRect,
    out Rect clientRect)
  {
    clientRect = new Rect();
    if (scrollInfo == null)
      return false;
    int startPage = scrollInfo.StartPage;
    int endPage = scrollInfo.EndPage;
    if (pageIndex < startPage || pageIndex > endPage)
      return false;
    clientRect = scrollInfo.PageToClientRect(pageRect, pageIndex);
    return true;
  }

  public static PdfControl GetPdfControl(this IPdfScrollInfoInternal obj)
  {
    return PdfControl.GetPdfControl(obj);
  }
}
