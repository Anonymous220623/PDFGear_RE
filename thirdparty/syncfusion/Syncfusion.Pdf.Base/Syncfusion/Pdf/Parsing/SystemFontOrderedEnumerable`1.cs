// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontOrderedEnumerable`1
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal abstract class SystemFontOrderedEnumerable<TElement> : 
  ISystemFontOrderedEnumerable<TElement>,
  IEnumerable<TElement>,
  IEnumerable
{
  internal IEnumerable<TElement> source;

  internal abstract SystemFontEnumerableSorter<TElement> GetEnumerableSorter(
    SystemFontEnumerableSorter<TElement> next);

  public IEnumerator<TElement> GetEnumerator()
  {
    SystemFontBuffer<TElement> buffer = new SystemFontBuffer<TElement>(this.source);
    if (buffer.count > 0)
    {
      SystemFontEnumerableSorter<TElement> enumerableSorter = this.GetEnumerableSorter((SystemFontEnumerableSorter<TElement>) null);
      int[] array = enumerableSorter.Sort(buffer.items, buffer.count);
      for (int i = 0; i < buffer.count; ++i)
        yield return buffer.items[array[i]];
    }
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  ISystemFontOrderedEnumerable<TElement> ISystemFontOrderedEnumerable<TElement>.CreateOrderedEnumerable<TKey>(
    Func<TElement, TKey> keySelector,
    IComparer<TKey> comparer,
    bool descending)
  {
    return (ISystemFontOrderedEnumerable<TElement>) new OrderedEnumerable<TElement, TKey>(this.source, keySelector, comparer, descending)
    {
      parent = this
    };
  }
}
