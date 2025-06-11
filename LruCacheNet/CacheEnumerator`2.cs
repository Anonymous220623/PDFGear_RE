// Decompiled with JetBrains decompiler
// Type: LruCacheNet.CacheEnumerator`2
// Assembly: LruCacheNet, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0571EC11-8C09-445B-9A07-07D97ABEC1CB
// Assembly location: D:\PDFGear\bin\LruCacheNet.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace LruCacheNet;

public class CacheEnumerator<TKey, TValue> : 
  IEnumerator<KeyValuePair<TKey, TValue>>,
  IEnumerator,
  IDisposable
{
  private Node<TKey, TValue> _head;
  private Node<TKey, TValue> _current;
  private LruCache<TKey, TValue> _cache;
  private bool _collectionChanged;

  public CacheEnumerator(LruCache<TKey, TValue> cache, Node<TKey, TValue> head)
  {
    if (head == null)
      throw new ArgumentException("Head can't be null");
    if (cache != null)
    {
      this._cache = cache;
      cache.CollectionChanged += new EventHandler(this.Cache_CollectionChanged);
    }
    this._head = head;
    this._current = (Node<TKey, TValue>) null;
  }

  public CacheEnumerator(Node<TKey, TValue> head)
    : this((LruCache<TKey, TValue>) null, head)
  {
  }

  public KeyValuePair<TKey, TValue> Current
  {
    get
    {
      if (this._head == null)
        throw new ObjectDisposedException(nameof (CacheEnumerator<TKey, TValue>));
      if (this._collectionChanged)
        throw new InvalidOperationException("Collection has changed");
      return new KeyValuePair<TKey, TValue>(this._current.Key, this._current.Value);
    }
  }

  object IEnumerator.Current => (object) this.Current;

  public void Dispose()
  {
    this._head = (Node<TKey, TValue>) null;
    this._current = (Node<TKey, TValue>) null;
    if (this._cache != null)
      this._cache.CollectionChanged -= new EventHandler(this.Cache_CollectionChanged);
    this._cache = (LruCache<TKey, TValue>) null;
  }

  public bool MoveNext()
  {
    if (this._head == null)
      throw new ObjectDisposedException(nameof (CacheEnumerator<TKey, TValue>));
    if (this._collectionChanged)
      throw new InvalidOperationException("Collection has changed");
    if (this._current == null)
    {
      this._current = this._head;
      return true;
    }
    if (this._current.Next == null)
      return false;
    this._current = this._current.Next;
    return true;
  }

  public void Reset()
  {
    if (this._collectionChanged)
      throw new InvalidOperationException("Collection has changed");
    this._current = (Node<TKey, TValue>) null;
  }

  private void Cache_CollectionChanged(object sender, EventArgs e)
  {
    this._collectionChanged = true;
  }
}
