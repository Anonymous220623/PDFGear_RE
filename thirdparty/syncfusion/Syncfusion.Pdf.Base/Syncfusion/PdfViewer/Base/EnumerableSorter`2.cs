// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.EnumerableSorter`2
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class EnumerableSorter<TElement, TKey> : CountableSortingHelper<TElement>
{
  internal Func<TElement, TKey> keySelector;
  internal IComparer<TKey> comparer;
  internal bool descending;
  internal CountableSortingHelper<TElement> next;
  internal TKey[] keys;

  internal EnumerableSorter(
    Func<TElement, TKey> keySelector,
    IComparer<TKey> comparer,
    bool descending,
    CountableSortingHelper<TElement> next)
  {
    this.keySelector = keySelector;
    this.comparer = comparer;
    this.descending = descending;
    this.next = next;
  }

  internal override int CompareKeys(int index1, int index2)
  {
    int num = this.comparer.Compare(this.keys[index1], this.keys[index2]);
    return num != 0 ? (this.descending ? -num : num) : (this.next != null ? this.next.CompareKeys(index1, index2) : index1 - index2);
  }

  internal override void ComputeKeys(TElement[] elements, int count)
  {
    this.keys = new TKey[count];
    for (int index = 0; index < count; ++index)
      this.keys[index] = this.keySelector(elements[index]);
    if (this.next == null)
      return;
    this.next.ComputeKeys(elements, count);
  }
}
