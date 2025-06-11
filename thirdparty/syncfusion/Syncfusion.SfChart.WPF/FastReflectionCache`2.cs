// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.FastReflectionCache`2
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

internal abstract class FastReflectionCache<TKey, TValue> : IFastReflectionCache<TKey, TValue> where TKey : class
{
  private Dictionary<TKey, TValue> m_cache = new Dictionary<TKey, TValue>();

  public TValue Get(TKey key)
  {
    TValue obj1 = default (TValue);
    if (this.m_cache.TryGetValue(key, out obj1))
      return obj1;
    bool lockTaken = false;
    object obj2;
    try
    {
      Monitor.Enter((object) (__Boxed<TKey>) (obj2 = (object) key), ref lockTaken);
      if (!this.m_cache.TryGetValue(key, out obj1))
      {
        obj1 = this.Create(key);
        this.m_cache[key] = obj1;
      }
    }
    finally
    {
      if (lockTaken)
        Monitor.Exit(obj2);
    }
    return obj1;
  }

  protected abstract TValue Create(TKey key);
}
