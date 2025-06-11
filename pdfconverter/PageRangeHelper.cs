// Decompiled with JetBrains decompiler
// Type: pdfconverter.PageRangeHelper
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace pdfconverter;

public static class PageRangeHelper
{
  public static bool TryParsePageRange(string range, out int[] pageIndexes, out int errorCharIndex)
  {
    pageIndexes = (int[]) null;
    int[][] pageIndexes1;
    if (!PageRangeHelper.TryParsePageRangeCore(range, out pageIndexes1, out errorCharIndex))
      return false;
    pageIndexes = ((IEnumerable<int[]>) pageIndexes1).SelectMany<int[], int>((Func<int[], IEnumerable<int>>) (c => (IEnumerable<int>) c)).Distinct<int>().OrderBy<int, int>((Func<int, int>) (c => c)).ToArray<int>();
    return true;
  }

  public static bool TryParsePageRange2(
    string range,
    out int[][] pageIndexes,
    out int errorCharIndex)
  {
    return PageRangeHelper.TryParsePageRangeCore(range, out pageIndexes, out errorCharIndex);
  }

  private static bool TryParsePageRangeCore(
    string range,
    out int[][] pageIndexes,
    out int errorCharIndex)
  {
    pageIndexes = (int[][]) null;
    errorCharIndex = -1;
    if (string.IsNullOrEmpty(range))
      return false;
    List<List<int>> list = new List<List<int>>();
    PageRangeHelper.PageRangeReader pageRangeReader = new PageRangeHelper.PageRangeReader(range);
    int from = -1;
    int to = -1;
    bool isTo = false;
    int num = -1;
    while (pageRangeReader.HasMore)
    {
      (PageRangeHelper.PageRangeTokenType type, string str, int startIdx) = pageRangeReader.GetNextToken();
      num = startIdx;
      switch (type)
      {
        case PageRangeHelper.PageRangeTokenType.Number:
          if (!isTo)
          {
            if (from == -1)
            {
              if (!int.TryParse(str, out from))
              {
                errorCharIndex = startIdx;
                return false;
              }
              continue;
            }
            errorCharIndex = startIdx;
            return false;
          }
          if (to == -1)
          {
            if (!int.TryParse(str, out to))
            {
              errorCharIndex = startIdx;
              return false;
            }
            if (to < from)
            {
              errorCharIndex = startIdx;
              return false;
            }
            continue;
          }
          errorCharIndex = startIdx;
          return false;
        case PageRangeHelper.PageRangeTokenType.Dash:
          if (from == -1 | isTo || to != -1)
          {
            errorCharIndex = startIdx;
            return false;
          }
          isTo = true;
          continue;
        case PageRangeHelper.PageRangeTokenType.Comma:
          if (!Complete())
          {
            errorCharIndex = startIdx;
            return false;
          }
          continue;
        default:
          if (pageRangeReader.HasMore)
          {
            errorCharIndex = startIdx;
            return false;
          }
          continue;
      }
    }
    if (!Complete())
    {
      errorCharIndex = num;
      return false;
    }
    pageIndexes = list.Select<List<int>, int[]>((Func<List<int>, int[]>) (c => c.OrderBy<int, int>((Func<int, int>) (x => x)).ToArray<int>())).ToArray<int[]>();
    return true;

    bool Complete()
    {
      if (from == -1)
        return false;
      if (to == -1)
      {
        if (isTo)
          return false;
        list.Add(new List<int>() { from - 1 });
      }
      else if (from < to)
      {
        list.Add(new List<int>(Enumerable.Range(from - 1, to - from + 1)));
      }
      else
      {
        if (from != to)
          return false;
        list.Add(new List<int>() { from - 1 });
      }
      from = -1;
      to = -1;
      isTo = false;
      return true;
    }
  }

  public static string ConvertToRange(this IEnumerable<int> pageIndexes)
  {
    return pageIndexes.ConvertToRange(out int[] _);
  }

  public static string ConvertToRange(
    this IEnumerable<int> pageIndexes,
    out int[] sortedPageIndexes)
  {
    sortedPageIndexes = (int[]) null;
    if (pageIndexes == null)
      return string.Empty;
    int[] array = pageIndexes.ToArray<int>();
    if (array.Length == 0)
      return string.Empty;
    Array.Sort<int>(array);
    sortedPageIndexes = array;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(array[0] + 1);
    bool flag = false;
    for (int index = 1; index < array.Length; ++index)
    {
      if (array[index] != array[index - 1])
      {
        if (array[index] - 1 == array[index - 1])
        {
          flag = true;
        }
        else
        {
          if (flag)
          {
            stringBuilder.Append('-').Append(array[index - 1] + 1);
            flag = false;
          }
          stringBuilder.Append(',').Append(array[index] + 1);
        }
      }
    }
    if (flag)
      stringBuilder.Append('-').Append(array[array.Length - 1] + 1);
    return stringBuilder.ToString();
  }

  private class PageRangeReader
  {
    private readonly string pageRange;
    private int curIdx;
    private StringBuilder sb;
    private PageRangeHelper.PageRangeTokenType curType;

    public PageRangeReader(string pageRange)
    {
      this.pageRange = !string.IsNullOrEmpty(pageRange) ? pageRange : throw new ArgumentException(nameof (pageRange));
      this.sb = new StringBuilder();
      this.curType = PageRangeHelper.PageRangeTokenType.None;
    }

    public bool HasMore => this.curIdx < this.pageRange.Length;

    public (PageRangeHelper.PageRangeTokenType type, string value, int startIdx) GetNextToken()
    {
      int curIdx = this.curIdx;
      for (; this.curIdx < this.pageRange.Length; ++this.curIdx)
      {
        char ch = this.pageRange[this.curIdx];
        if (ch >= '0' && ch <= '9')
        {
          if (this.curType == PageRangeHelper.PageRangeTokenType.None)
            this.curType = PageRangeHelper.PageRangeTokenType.Number;
          this.sb.Append(ch);
        }
        else
        {
          switch (ch)
          {
            case ' ':
              if (this.curType != PageRangeHelper.PageRangeTokenType.Number)
              {
                ++curIdx;
                continue;
              }
              goto label_17;
            case ',':
              if (this.curType == PageRangeHelper.PageRangeTokenType.None)
              {
                this.curType = PageRangeHelper.PageRangeTokenType.Comma;
                this.sb.Append(ch);
                ++this.curIdx;
                goto label_17;
              }
              if (this.curType == PageRangeHelper.PageRangeTokenType.Number || this.curType == PageRangeHelper.PageRangeTokenType.Dash)
                goto label_17;
              continue;
            case '-':
              if (this.curType == PageRangeHelper.PageRangeTokenType.None)
              {
                this.curType = PageRangeHelper.PageRangeTokenType.Dash;
                this.sb.Append(ch);
                ++this.curIdx;
                goto label_17;
              }
              if (this.curType == PageRangeHelper.PageRangeTokenType.Number || this.curType == PageRangeHelper.PageRangeTokenType.Comma)
                goto label_17;
              continue;
            default:
              this.curType = PageRangeHelper.PageRangeTokenType.None;
              this.sb.Length = 0;
              goto label_17;
          }
        }
      }
label_17:
      int curType = (int) this.curType;
      string str1 = this.sb.ToString();
      this.curType = PageRangeHelper.PageRangeTokenType.None;
      this.sb.Length = 0;
      string str2 = str1;
      int num = curIdx;
      return ((PageRangeHelper.PageRangeTokenType) curType, str2, num);
    }
  }

  private enum PageRangeTokenType
  {
    None,
    Number,
    Dash,
    Comma,
  }
}
