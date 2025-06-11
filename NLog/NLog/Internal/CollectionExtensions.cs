// Decompiled with JetBrains decompiler
// Type: NLog.Internal.CollectionExtensions
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;

#nullable disable
namespace NLog.Internal;

internal static class CollectionExtensions
{
  [NotNull]
  public static IList<TItem> Filter<TItem, TState>(
    [NotNull] this IList<TItem> items,
    TState state,
    Func<TItem, TState, bool> filter)
  {
    bool flag = false;
    IList<TItem> objList1 = (IList<TItem>) null;
    for (int index1 = 0; index1 < items.Count; ++index1)
    {
      TItem obj = items[index1];
      if (filter(obj, state))
      {
        if (flag && objList1 == null)
          objList1 = (IList<TItem>) new List<TItem>();
        objList1?.Add(obj);
      }
      else
      {
        if (!flag && index1 > 0)
        {
          objList1 = (IList<TItem>) new List<TItem>();
          for (int index2 = 0; index2 < index1; ++index2)
            objList1.Add(items[index2]);
        }
        flag = true;
      }
    }
    IList<TItem> objList2 = objList1;
    if (objList2 != null)
      return objList2;
    return !flag ? items : (IList<TItem>) ArrayHelper.Empty<TItem>();
  }
}
