// Decompiled with JetBrains decompiler
// Type: Nito.Collections.CollectionHelpers
// Assembly: Nito.Collections.Deque, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AAACFF4B-3CF8-41B7-BD25-F9042A065111
// Assembly location: D:\PDFGear\bin\Nito.Collections.Deque.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable enable
namespace Nito.Collections;

internal static class CollectionHelpers
{
  public static IReadOnlyCollection<T> ReifyCollection<T>(IEnumerable<T> source)
  {
    switch (source)
    {
      case null:
        throw new ArgumentNullException(nameof (source));
      case IReadOnlyCollection<T> objs:
        return objs;
      case ICollection<T> collection1:
        return (IReadOnlyCollection<T>) new CollectionHelpers.CollectionWrapper<T>(collection1);
      case ICollection collection2:
        return (IReadOnlyCollection<T>) new CollectionHelpers.NongenericCollectionWrapper<T>(collection2);
      default:
        return (IReadOnlyCollection<T>) new List<T>(source);
    }
  }

  private sealed class NongenericCollectionWrapper<T> : 
    IReadOnlyCollection<T>,
    IEnumerable<T>,
    IEnumerable
  {
    private readonly ICollection _collection;

    public NongenericCollectionWrapper(ICollection collection)
    {
      this._collection = collection != null ? collection : throw new ArgumentNullException(nameof (collection));
    }

    public int Count => this._collection.Count;

    public IEnumerator<T> GetEnumerator()
    {
      foreach (T obj in (IEnumerable) this._collection)
        yield return obj;
    }

    IEnumerator IEnumerable.GetEnumerator() => this._collection.GetEnumerator();
  }

  private sealed class CollectionWrapper<T> : IReadOnlyCollection<T>, IEnumerable<T>, IEnumerable
  {
    private readonly ICollection<T> _collection;

    public CollectionWrapper(ICollection<T> collection)
    {
      this._collection = collection != null ? collection : throw new ArgumentNullException(nameof (collection));
    }

    public int Count => this._collection.Count;

    public IEnumerator<T> GetEnumerator() => this._collection.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._collection.GetEnumerator();
  }
}
