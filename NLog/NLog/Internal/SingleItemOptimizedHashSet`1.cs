// Decompiled with JetBrains decompiler
// Type: NLog.Internal.SingleItemOptimizedHashSet`1
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace NLog.Internal;

internal struct SingleItemOptimizedHashSet<T> : ICollection<T>, IEnumerable<T>, IEnumerable
{
  private readonly T _singleItem;
  private HashSet<T> _hashset;
  private readonly IEqualityComparer<T> _comparer;

  private IEqualityComparer<T> Comparer
  {
    get => this._comparer ?? (IEqualityComparer<T>) EqualityComparer<T>.Default;
  }

  public int Count
  {
    get
    {
      HashSet<T> hashset = this._hashset;
      if (hashset != null)
        return __nonvirtual (hashset.Count);
      return !EqualityComparer<T>.Default.Equals(this._singleItem, default (T)) ? 1 : 0;
    }
  }

  public bool IsReadOnly => false;

  public SingleItemOptimizedHashSet(
    T singleItem,
    SingleItemOptimizedHashSet<T> existing,
    IEqualityComparer<T> comparer = null)
  {
    this._comparer = existing._comparer ?? comparer ?? (IEqualityComparer<T>) EqualityComparer<T>.Default;
    if (existing._hashset != null)
    {
      this._hashset = new HashSet<T>((IEnumerable<T>) existing._hashset, this._comparer);
      this._hashset.Add(singleItem);
      this._singleItem = default (T);
    }
    else if (existing.Count == 1)
    {
      this._hashset = new HashSet<T>(this._comparer);
      this._hashset.Add(existing._singleItem);
      this._hashset.Add(singleItem);
      this._singleItem = default (T);
    }
    else
    {
      this._hashset = (HashSet<T>) null;
      this._singleItem = singleItem;
    }
  }

  public void Add(T item)
  {
    if (this._hashset != null)
    {
      this._hashset.Add(item);
    }
    else
    {
      HashSet<T> objSet = new HashSet<T>(this.Comparer);
      if (this.Count != 0)
        objSet.Add(this._singleItem);
      objSet.Add(item);
      this._hashset = objSet;
    }
  }

  public void Clear()
  {
    if (this._hashset != null)
      this._hashset.Clear();
    else
      this._hashset = new HashSet<T>(this.Comparer);
  }

  public bool Contains(T item)
  {
    if (this._hashset != null)
      return this._hashset.Contains(item);
    return this.Count == 1 && this.Comparer.Equals(this._singleItem, item);
  }

  public bool Remove(T item)
  {
    if (this._hashset != null)
      return this._hashset.Remove(item);
    this._hashset = new HashSet<T>(this.Comparer);
    return this.Comparer.Equals(this._singleItem, item);
  }

  public void CopyTo(T[] array, int arrayIndex)
  {
    if (this._hashset != null)
    {
      this._hashset.CopyTo(array, arrayIndex);
    }
    else
    {
      if (this.Count != 1)
        return;
      array[arrayIndex] = this._singleItem;
    }
  }

  public IEnumerator<T> GetEnumerator()
  {
    return this._hashset != null ? (IEnumerator<T>) this._hashset.GetEnumerator() : this.SingleItemEnumerator();
  }

  private IEnumerator<T> SingleItemEnumerator()
  {
    if (this.Count != 0)
      yield return this._singleItem;
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  public struct SingleItemScopedInsert : IDisposable
  {
    private readonly T _singleItem;
    private readonly HashSet<T> _hashset;

    public SingleItemScopedInsert(
      T singleItem,
      ref SingleItemOptimizedHashSet<T> existing,
      bool forceHashSet,
      IEqualityComparer<T> comparer)
    {
      this._singleItem = singleItem;
      if (existing._hashset != null)
      {
        existing._hashset.Add(singleItem);
        this._hashset = existing._hashset;
      }
      else if (forceHashSet)
      {
        existing = new SingleItemOptimizedHashSet<T>(singleItem, existing, comparer);
        existing.Add(singleItem);
        this._hashset = existing._hashset;
      }
      else
      {
        existing = new SingleItemOptimizedHashSet<T>(singleItem, existing, comparer);
        this._hashset = (HashSet<T>) null;
      }
    }

    public void Dispose()
    {
      if (this._hashset == null)
        return;
      this._hashset.Remove(this._singleItem);
    }
  }

  public sealed class ReferenceEqualityComparer : IEqualityComparer<T>
  {
    public static readonly SingleItemOptimizedHashSet<T>.ReferenceEqualityComparer Default = new SingleItemOptimizedHashSet<T>.ReferenceEqualityComparer();

    bool IEqualityComparer<T>.Equals(T x, T y) => (object) x == (object) y;

    int IEqualityComparer<T>.GetHashCode(T obj) => RuntimeHelpers.GetHashCode((object) obj);
  }
}
