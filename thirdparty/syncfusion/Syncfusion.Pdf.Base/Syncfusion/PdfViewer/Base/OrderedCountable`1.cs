// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.OrderedCountable`1
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal abstract class OrderedCountable<TElement> : 
  IOrderedCountable<TElement>,
  IEnumerable<TElement>,
  IEnumerable
{
  internal IEnumerable<TElement> source;

  internal abstract CountableSortingHelper<TElement> GetEnumerableSorter(
    CountableSortingHelper<TElement> next);

  public IEnumerator<TElement> GetEnumerator()
  {
    Buffer<TElement> buffer = new Buffer<TElement>(this.source);
    if (buffer.count > 0)
    {
      CountableSortingHelper<TElement> enumerableSorter = this.GetEnumerableSorter((CountableSortingHelper<TElement>) null);
      int[] array = enumerableSorter.Sort(buffer.items, buffer.count);
      for (int i = 0; i < buffer.count; ++i)
        yield return buffer.items[array[i]];
    }
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  IOrderedCountable<TElement> IOrderedCountable<TElement>.CreateOrderedEnumerable<TKey>(
    Func<TElement, TKey> keySelector,
    IComparer<TKey> comparer,
    bool descending)
  {
    return (IOrderedCountable<TElement>) new OrderedEnumerable<TElement, TKey>(this.source, keySelector, comparer, descending)
    {
      parent = this
    };
  }
}
