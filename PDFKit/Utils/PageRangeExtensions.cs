// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PageRangeExtensions
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace PDFKit.Utils;

public static class PageRangeExtensions
{
  public static IEnumerable<int> GetPagesEnumerable(this IEnumerable<PdfPageRange> pageRanges)
  {
    return pageRanges == null ? Enumerable.Empty<int>() : (IEnumerable<int>) pageRanges.SelectMany<PdfPageRange, int>((Func<PdfPageRange, IEnumerable<int>>) (c => (IEnumerable<int>) c)).Distinct<int>().OrderBy<int, int>((Func<int, int>) (c => c));
  }

  public static string ToPageRangeString(this IEnumerable<PdfPageRange> pageRanges, int baseIndex)
  {
    return pageRanges == null ? string.Empty : PageRangeExtensions.ToPageRangeString(pageRanges, baseIndex, true);
  }

  public static string ToPageRangeString(
    IEnumerable<PdfPageRange> pageRanges,
    int baseIndex,
    bool sort)
  {
    if (pageRanges == null)
      return string.Empty;
    StringBuilder stringBuilder = (StringBuilder) null;
    if (sort)
      pageRanges = (IEnumerable<PdfPageRange>) pageRanges.GetPagesEnumerable().ToPageRange();
    foreach (PdfPageRange pageRange in pageRanges)
    {
      if (stringBuilder == null)
        stringBuilder = new StringBuilder();
      else
        stringBuilder.Append(',');
      stringBuilder.Append(pageRange.ToString(baseIndex));
    }
    return stringBuilder != null ? stringBuilder.ToString() : string.Empty;
  }

  public static string ToPageRangeString(this IEnumerable<int> pageIndexes, int baseIndex)
  {
    return pageIndexes == null ? string.Empty : PageRangeExtensions.ToPageRangeString((IEnumerable<PdfPageRange>) pageIndexes.ToPageRange(), baseIndex, false);
  }

  public static IReadOnlyList<PdfPageRange> ToPageRange(this IEnumerable<int> pageIndexes)
  {
    if (pageIndexes == null)
      return (IReadOnlyList<PdfPageRange>) Array.Empty<PdfPageRange>();
    IEnumerable<int> ints = pageIndexes.OrderBy<int, int>((Func<int, int>) (c => c)).Distinct<int>();
    List<PdfPageRange> pdfPageRangeList = new List<PdfPageRange>();
    int to = -1;
    int num1 = -1;
    bool flag = false;
    foreach (int num2 in ints)
    {
      if (num2 >= 0)
      {
        if (num1 == -1)
        {
          num1 = num2;
          to = num2;
        }
        else if (num2 != to)
        {
          if (num2 - 1 == to)
          {
            flag = true;
            to = num2;
          }
          else
          {
            if (flag)
              pdfPageRangeList.Add(new PdfPageRange(num1, to));
            else
              pdfPageRangeList.Add(new PdfPageRange(num1));
            num1 = num2;
            to = num2;
            flag = false;
          }
        }
      }
    }
    if (flag)
      pdfPageRangeList.Add(new PdfPageRange(num1, to));
    else
      pdfPageRangeList.Add(new PdfPageRange(num1));
    return (IReadOnlyList<PdfPageRange>) ((object) pdfPageRangeList ?? (object) Array.Empty<PdfPageRange>());
  }

  public static IEnumerable<IReadOnlyList<PdfPageRange>> SplitBy(
    this IEnumerable<int> pageIndexes,
    int limit)
  {
    foreach (IGrouping<int, int> span in pageIndexes.OrderBy<int, int>((Func<int, int>) (c => c)).GroupBy<int, int>((Func<int, int>) (c => c / limit)))
      yield return span.ToPageRange();
  }
}
