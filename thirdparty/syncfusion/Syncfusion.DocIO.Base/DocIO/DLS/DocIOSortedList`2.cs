// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.DocIOSortedList`2
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class DocIOSortedList<TKey, TValue> : TypedSortedListEx<TKey, TValue> where TKey : IComparable
{
  public DocIOSortedList()
  {
  }

  public DocIOSortedList(IComparer<TKey> comparer)
    : base(comparer)
  {
  }

  public DocIOSortedList(int count)
    : base(count)
  {
  }

  public DocIOSortedList(IDictionary<TKey, TValue> dictionary)
    : base(dictionary)
  {
  }
}
