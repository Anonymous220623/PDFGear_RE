// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.Countable
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal static class Countable
{
  public static int Count<TSource>(IEnumerable<TSource> source)
  {
    switch (source)
    {
      case ICollection<TSource> sources:
        return sources.Count;
      case ICollection collection:
        return collection.Count;
      default:
        int num = 0;
        using (IEnumerator<TSource> enumerator = source.GetEnumerator())
        {
          while (enumerator.MoveNext())
            ++num;
        }
        return num;
    }
  }

  public static bool Any<TSource>(IEnumerable<TSource> source)
  {
    using (IEnumerator<TSource> enumerator = source.GetEnumerator())
    {
      if (enumerator.MoveNext())
        return true;
    }
    return false;
  }

  public static bool All<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
  {
    foreach (TSource source1 in source)
    {
      if (!predicate(source1))
        return false;
    }
    return true;
  }

  public static bool Any<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
  {
    foreach (TSource source1 in source)
    {
      if (predicate(source1))
        return true;
    }
    return false;
  }

  public static IEnumerable<TResult> OfType<TResult>(IEnumerable source)
  {
    return Countable.OfTypeIterator<TResult>(source);
  }

  private static IEnumerable<TResult> OfTypeIterator<TResult>(IEnumerable source)
  {
    foreach (object current in source)
    {
      if (current is TResult result)
        yield return result;
    }
  }

  public static TSource FirstOrDefault<TSource>(IEnumerable<TSource> source)
  {
    if (source is IList<TSource> sourceList)
    {
      if (sourceList.Count > 0)
        return sourceList[0];
    }
    else
    {
      using (IEnumerator<TSource> enumerator = source.GetEnumerator())
      {
        if (enumerator.MoveNext())
          return enumerator.Current;
      }
    }
    return default (TSource);
  }

  public static TSource FirstOrDefault<TSource>(
    IEnumerable<TSource> source,
    Func<TSource, bool> predicate)
  {
    if (source == null)
      throw new Exception("Null Reference");
    if (predicate == null)
      throw new Exception("Reference Exception");
    IEnumerator<TSource> enumerator = source.GetEnumerator();
    using (enumerator)
    {
      while (enumerator.MoveNext())
      {
        TSource current = enumerator.Current;
        if (predicate(current))
          return current;
      }
      return default (TSource);
    }
  }

  public static TSource First<TSource>(IEnumerable<TSource> source)
  {
    if (source is IList<TSource> sourceList)
    {
      if (sourceList.Count > 0)
        return sourceList[0];
    }
    else
    {
      using (IEnumerator<TSource> enumerator = source.GetEnumerator())
      {
        if (enumerator.MoveNext())
          return enumerator.Current;
      }
    }
    throw new Exception("Null Reference Exception");
  }

  public static TSource First<TSource>(IEnumerable<TSource> source, Func<TSource, bool> selector)
  {
    return Countable.First<TSource>(Countable.Where<TSource>(source, selector));
  }

  public static bool Contains<TSource>(IEnumerable<TSource> source, TSource value)
  {
    return source is ICollection<TSource> sources ? sources.Contains(value) : Countable.Contains<TSource>(source, value, (IEqualityComparer<TSource>) null);
  }

  public static bool Contains<TSource>(
    IEnumerable<TSource> source,
    TSource value,
    IEqualityComparer<TSource> comparer)
  {
    if (comparer == null)
      comparer = (IEqualityComparer<TSource>) EqualityComparer<TSource>.Default;
    foreach (TSource x in source)
    {
      if (comparer.Equals(x, value))
        return true;
    }
    return false;
  }

  public static TSource[] ToArray<TSource>(IEnumerable<TSource> source)
  {
    return new Buffer<TSource>(source).ToArray();
  }

  public static List<TSource> ToList<TSource>(IEnumerable<TSource> source)
  {
    return new List<TSource>(source);
  }

  public static IEnumerable<TResult> Empty<TResult>() => BlankEnumerable<TResult>.Instance;

  public static byte Max(IEnumerable<byte> source)
  {
    byte num1 = 0;
    foreach (byte num2 in source)
    {
      if ((int) num1 < (int) num2)
        num1 = num2;
    }
    return num1;
  }

  public static byte Max<TSource>(IEnumerable<TSource> source, Func<TSource, byte> selector)
  {
    return Countable.Max(Countable.Select<TSource, byte>(source, selector));
  }

  public static double Sum(IEnumerable<double> source)
  {
    double num1 = 0.0;
    foreach (double num2 in source)
      num1 += num2;
    return num1;
  }

  public static int Sum(IEnumerable<int> source)
  {
    if (source == null)
      throw new Exception("Null Reference");
    int num1 = 0;
    foreach (int num2 in source)
      num1 += num2;
    return num1;
  }

  public static int Sum<TSource>(IEnumerable<TSource> source, Func<TSource, int> selector)
  {
    return Countable.Sum(Countable.Select<TSource, int>(source, selector));
  }

  public static IEnumerable<TSource> Skip<TSource>(IEnumerable<TSource> source, int count)
  {
    IEnumerator<TSource> enumerator = source.GetEnumerator();
    int num;
    do
    {
      count = (num = count) - 1;
    }
    while (num > 0 && enumerator.MoveNext());
    while (enumerator.MoveNext())
      yield return enumerator.Current;
  }

  public static IEnumerable<TResult> Select<TSource, TResult>(
    IEnumerable<TSource> source,
    Func<TSource, TResult> selector)
  {
    foreach (TSource current in source)
      yield return selector(current);
  }

  public static IEnumerable<TSource> Where<TSource>(
    IEnumerable<TSource> source,
    Func<TSource, bool> predicate)
  {
    foreach (TSource current in source)
    {
      if (predicate(current))
        yield return current;
    }
  }

  public static IEnumerable<TSource> Take<TSource>(IEnumerable<TSource> source, int count)
  {
    return source != null ? Countable.TakeIterator<TSource>(source, count) : throw new Exception("Null Reference");
  }

  private static IEnumerable<TSource> TakeIterator<TSource>(IEnumerable<TSource> source, int count)
  {
    if (count > 0)
    {
      foreach (TSource current in source)
      {
        yield return current;
        int num = count - 1;
        int num2 = num;
        count = num;
        if (num2 == 0)
          break;
      }
    }
  }

  private static IEnumerable<TSource> ConcatIterator<TSource>(
    IEnumerable<TSource> first,
    IEnumerable<TSource> second)
  {
    foreach (TSource current in first)
      yield return current;
    foreach (TSource current2 in second)
      yield return current2;
  }

  public static TSource Last<TSource>(IEnumerable<TSource> source)
  {
    if (!(source is IList<TSource> sourceList))
    {
      IEnumerator<TSource> enumerator = source.GetEnumerator();
      using (enumerator)
      {
        TSource current;
        do
        {
          current = enumerator.Current;
        }
        while (enumerator.MoveNext());
        return current;
      }
    }
    int count = sourceList.Count;
    return count > 0 ? sourceList[count - 1] : throw new Exception("Null Reference");
  }

  public static IOrderedCountable<TSource> OrderBy<TSource, TKey>(
    IEnumerable<TSource> source,
    Func<TSource, TKey> keySelector)
  {
    return (IOrderedCountable<TSource>) new OrderedEnumerable<TSource, TKey>(source, keySelector, (IComparer<TKey>) null, false);
  }

  public static IOrderedCountable<TSource> OrderBy<TSource, TKey>(
    IEnumerable<TSource> source,
    Func<TSource, TKey> keySelector,
    IComparer<TKey> comparer)
  {
    return (IOrderedCountable<TSource>) new OrderedEnumerable<TSource, TKey>(source, keySelector, comparer, false);
  }

  public static TSource ElementAt<TSource>(IEnumerable<TSource> source, int index)
  {
    if (!(source is IList<TSource> sourceList) && index >= 0)
    {
      IEnumerator<TSource> enumerator = source.GetEnumerator();
      using (enumerator)
      {
        while (enumerator.MoveNext())
        {
          if (index == 0)
            return enumerator.Current;
          --index;
        }
      }
    }
    return sourceList[index];
  }

  public static TSource LastOrDefault<TSource>(IEnumerable<TSource> source)
  {
    TSource source1 = default (TSource);
    if (!(source is IList<TSource> sourceList))
    {
      IEnumerator<TSource> enumerator = source.GetEnumerator();
      using (enumerator)
      {
        if (!enumerator.MoveNext())
          return default (TSource);
        TSource current;
        do
        {
          current = enumerator.Current;
        }
        while (enumerator.MoveNext());
        return current;
      }
    }
    int count = sourceList.Count;
    return count > 0 ? sourceList[count - 1] : default (TSource);
  }

  public static TSource LastOrDefault<TSource>(
    IEnumerable<TSource> source,
    Func<TSource, bool> predicate)
  {
    TSource source1 = default (TSource);
    if (predicate == null)
      throw new Exception("Null Reference");
    foreach (TSource source2 in source)
    {
      if (predicate(source2))
        source1 = source2;
    }
    return source1;
  }

  public static TSource SingleOrDefault<TSource>(IEnumerable<TSource> source)
  {
    if (!(source is IList<TSource> sourceList))
    {
      IEnumerator<TSource> enumerator = source.GetEnumerator();
      TSource source1;
      using (enumerator)
        source1 = !enumerator.MoveNext() ? default (TSource) : enumerator.Current;
      return source1;
    }
    switch (sourceList.Count)
    {
      case 0:
        return default (TSource);
      case 1:
        return sourceList[0];
      default:
        throw new Exception("Null Reference");
    }
  }

  public static TSource SingleOrDefault<TSource>(
    IEnumerable<TSource> source,
    Func<TSource, bool> predicate)
  {
    TSource source1 = default (TSource);
    if (predicate == null)
      throw new Exception("Null Reference");
    long num = 0;
    foreach (TSource source2 in source)
    {
      if (predicate(source2))
      {
        source1 = source2;
        ++num;
      }
    }
    switch (num)
    {
      case 0:
        return default (TSource);
      case 1:
        return source1;
      default:
        throw new Exception("Null Reference");
    }
  }
}
