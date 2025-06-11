// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PdfPageRange
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace PDFKit.Utils;

public struct PdfPageRange : 
  IEquatable<PdfPageRange>,
  IComparable<PdfPageRange>,
  IEnumerable<int>,
  IEnumerable
{
  public PdfPageRange(int pageIndex)
    : this(pageIndex, pageIndex)
  {
  }

  public PdfPageRange(int from, int to)
  {
    if (from < 0)
      throw new ArgumentException(nameof (from));
    this.From = to >= from ? from : throw new ArgumentException(nameof (to));
    this.To = to;
  }

  public int From { get; }

  public int To { get; }

  public int Count => this.To - this.From + 1;

  public int CompareTo(PdfPageRange other) => this.From.CompareTo(other.From);

  public bool Equals(PdfPageRange other) => this.From == other.From && this.To == other.To;

  public override bool Equals(object obj) => obj is PdfPageRange other && this.Equals(other);

  public override int GetHashCode() => HashCode.Combine<int, int>(this.From, this.To);

  public override string ToString() => this.ToString(1);

  public string ToString(int baseIndex)
  {
    if (this.From == this.To)
      return $"{this.From + baseIndex}";
    if (this.From < this.To)
      return $"{this.From + baseIndex}-{this.To + baseIndex}";
    throw new ArgumentException("To");
  }

  public static bool operator ==(PdfPageRange left, PdfPageRange right) => left.Equals(right);

  public static bool operator !=(PdfPageRange left, PdfPageRange right) => !left.Equals(right);

  public IEnumerator<int> GetEnumerator()
  {
    for (int i = this.From; i <= this.To; ++i)
      yield return i;
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    for (int i = this.From; i <= this.To; ++i)
      yield return (object) i;
  }

  public static bool TryParsePageRange(
    string range,
    PdfPageRange.PageRangeParseOptions options,
    out IReadOnlyList<PdfPageRange> pageRanges,
    out int errorCharIndex)
  {
    pageRanges = (IReadOnlyList<PdfPageRange>) null;
    errorCharIndex = -1;
    if (string.IsNullOrEmpty(range))
      return false;
    List<PdfPageRange> list = new List<PdfPageRange>();
    PageRangeReader pageRangeReader = new PageRangeReader(range);
    int num1 = options.PageCount + options.BaseIndex - 1;
    if (options.PageCount == 0)
      num1 = int.MaxValue;
    int from = -1;
    int to = -1;
    bool isTo = false;
    int num2 = -1;
    while (pageRangeReader.HasMore)
    {
      (PageRangeTokenType type, int value, int startIdx) = pageRangeReader.GetNextToken();
      num2 = startIdx;
      switch (type)
      {
        case PageRangeTokenType.Number:
          if (!isTo)
          {
            if (from == -1)
            {
              if (value < options.BaseIndex || value > num1)
              {
                errorCharIndex = startIdx;
                return false;
              }
              from = value;
              break;
            }
            errorCharIndex = startIdx;
            return false;
          }
          if (to == -1)
          {
            if (value < options.BaseIndex || value > num1)
            {
              errorCharIndex = startIdx;
              return false;
            }
            if (value < from)
            {
              errorCharIndex = startIdx;
              return false;
            }
            to = value;
            break;
          }
          errorCharIndex = startIdx;
          return false;
        case PageRangeTokenType.Dash:
          if (from == -1 | isTo || to != -1)
          {
            errorCharIndex = startIdx;
            return false;
          }
          isTo = true;
          break;
        case PageRangeTokenType.Comma:
          if (!Complete())
          {
            errorCharIndex = startIdx;
            return false;
          }
          break;
        default:
          if (pageRangeReader.HasMore)
          {
            errorCharIndex = startIdx;
            return false;
          }
          break;
      }
    }
    if (!Complete())
    {
      errorCharIndex = num2;
      return false;
    }
    list.Sort();
    pageRanges = (IReadOnlyList<PdfPageRange>) list;
    return true;

    bool Complete()
    {
      if (from == -1)
        return false;
      if (to == -1)
      {
        if (isTo)
          return false;
        list.Add(new PdfPageRange(from - options.BaseIndex, from - options.BaseIndex));
      }
      else if (from < to)
      {
        list.Add(new PdfPageRange(from - options.BaseIndex, to - options.BaseIndex));
      }
      else
      {
        if (from != to)
          return false;
        list.Add(new PdfPageRange(from - options.BaseIndex, from - options.BaseIndex));
      }
      from = -1;
      to = -1;
      isTo = false;
      return true;
    }
  }

  public struct PageRangeParseOptions
  {
    public static PdfPageRange.PageRangeParseOptions StartFromPage0
    {
      get
      {
        return new PdfPageRange.PageRangeParseOptions()
        {
          BaseIndex = 0
        };
      }
    }

    public static PdfPageRange.PageRangeParseOptions StartFromPage1
    {
      get
      {
        return new PdfPageRange.PageRangeParseOptions()
        {
          BaseIndex = 1
        };
      }
    }

    public int PageCount { get; set; }

    public int BaseIndex { get; set; }
  }
}
