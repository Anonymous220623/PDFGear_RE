// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.OrderedEnumerable`2
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class OrderedEnumerable<TElement, TKey> : SystemFontOrderedEnumerable<TElement>
{
  internal SystemFontOrderedEnumerable<TElement> parent;
  internal Func<TElement, TKey> keySelector;
  internal IComparer<TKey> comparer;
  internal bool descending;

  internal OrderedEnumerable(
    IEnumerable<TElement> source,
    Func<TElement, TKey> keySelector,
    IComparer<TKey> comparer,
    bool descending)
  {
    if (keySelector == null)
      throw new Exception(nameof (keySelector));
    this.source = source;
    this.parent = (SystemFontOrderedEnumerable<TElement>) null;
    this.keySelector = keySelector;
    this.comparer = comparer == null ? (IComparer<TKey>) Comparer<TKey>.Default : comparer;
    this.descending = descending;
  }

  internal override SystemFontEnumerableSorter<TElement> GetEnumerableSorter(
    SystemFontEnumerableSorter<TElement> next)
  {
    SystemFontEnumerableSorter<TElement> next1 = (SystemFontEnumerableSorter<TElement>) new EnumerableSorter<TElement, TKey>(this.keySelector, this.comparer, this.descending, next);
    if (this.parent != null)
      next1 = this.parent.GetEnumerableSorter(next1);
    return next1;
  }
}
