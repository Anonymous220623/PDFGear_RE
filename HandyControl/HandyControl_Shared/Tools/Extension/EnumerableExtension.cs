// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Extension.EnumerableExtension
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace HandyControl.Tools.Extension;

public static class EnumerableExtension
{
  public static IEnumerable<TSource> Do<TSource>(
    this IEnumerable<TSource> source,
    Action<TSource> predicate)
  {
    if (!(source is IList<TSource> sourceList1))
      sourceList1 = (IList<TSource>) source.ToList<TSource>();
    IList<TSource> sourceList2 = sourceList1;
    foreach (TSource source1 in (IEnumerable<TSource>) sourceList2)
      predicate(source1);
    return (IEnumerable<TSource>) sourceList2;
  }

  public static IEnumerable<TSource> DoWithIndex<TSource>(
    this IEnumerable<TSource> source,
    Action<TSource, int> predicate)
  {
    if (!(source is IList<TSource> sourceList1))
      sourceList1 = (IList<TSource>) source.ToList<TSource>();
    IList<TSource> sourceList2 = sourceList1;
    for (int index = 0; index < sourceList2.Count; ++index)
      predicate(sourceList2[index], index);
    return (IEnumerable<TSource>) sourceList2;
  }
}
