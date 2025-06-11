// Decompiled with JetBrains decompiler
// Type: LruCacheNet.LruCache`2
// Assembly: LruCacheNet, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0571EC11-8C09-445B-9A07-07D97ABEC1CB
// Assembly location: D:\PDFGear\bin\LruCacheNet.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace LruCacheNet;

public class LruCache<TKey, TValue> : 
  IDictionary<TKey, TValue>,
  ICollection<KeyValuePair<TKey, TValue>>,
  IEnumerable<KeyValuePair<TKey, TValue>>,
  IEnumerable
{
  private const int DefaultCacheSize = 1000;
  private const int MinimumCacheSize = 2;
  private Dictionary<TKey, Node<TKey, TValue>> _data;
  private Node<TKey, TValue> _head;
  private Node<TKey, TValue> _tail;
  private int _cacheSize;
  private object _lock;
  private LruCache<TKey, TValue>.UpdateDataMethod _updateMethod;
  private LruCache<TKey, TValue>.CreateCopyMethod _createMethod;

  public LruCache()
    : this(1000)
  {
  }

  public LruCache(int capacity)
  {
    this._cacheSize = capacity >= 2 ? capacity : throw new ArgumentException("Cache size must be at least 2", nameof (capacity));
    this._data = new Dictionary<TKey, Node<TKey, TValue>>();
    this._head = (Node<TKey, TValue>) null;
    this._tail = (Node<TKey, TValue>) null;
    this._lock = new object();
  }

  public event EventHandler CollectionChanged;

  public int Count
  {
    get
    {
      lock (this._lock)
        return this._data.Count;
    }
  }

  public int Capacity => this._cacheSize;

  public ICollection<TKey> Keys
  {
    get
    {
      lock (this._lock)
        return (ICollection<TKey>) this._data.Keys;
    }
  }

  public ICollection<TValue> Values
  {
    get
    {
      lock (this._lock)
        return (ICollection<TValue>) this.ToList();
    }
  }

  public bool IsReadOnly => false;

  public TValue this[TKey key]
  {
    get => this.Get(key);
    set => this.AddOrUpdate(key, value);
  }

  public void SetUpdateMethod(LruCache<TKey, TValue>.UpdateDataMethod method)
  {
    this._updateMethod = method;
  }

  public void SetCopyMethod(LruCache<TKey, TValue>.CreateCopyMethod method)
  {
    this._createMethod = method;
  }

  public void AddOrUpdate(TKey key, TValue data)
  {
    if ((object) key == null)
      throw new ArgumentException("Key cannot be null", nameof (key));
    if ((object) data == null)
      throw new ArgumentException("Data cannot be null", nameof (data));
    lock (this._lock)
    {
      Node<TKey, TValue> node;
      if (this._data.TryGetValue(key, out node))
      {
        this.MoveNodeUp(node);
        if (this._updateMethod != null)
        {
          if (!this._updateMethod(node.Value, data))
            return;
          EventHandler collectionChanged = this.CollectionChanged;
          if (collectionChanged == null)
            return;
          collectionChanged((object) this, new EventArgs());
        }
        else
        {
          node.Value = data;
          EventHandler collectionChanged = this.CollectionChanged;
          if (collectionChanged == null)
            return;
          collectionChanged((object) this, new EventArgs());
        }
      }
      else
        this.AddItem(key, data);
    }
  }

  public bool Refresh(TKey key)
  {
    lock (this._lock)
    {
      Node<TKey, TValue> node;
      if (!this._data.TryGetValue(key, out node))
        return false;
      this.MoveNodeUp(node);
      return true;
    }
  }

  public TValue Peek(TKey key)
  {
    lock (this._lock)
    {
      Node<TKey, TValue> node;
      this._data.TryGetValue(key, out node);
      return node != null ? node.Value : throw new KeyNotFoundException();
    }
  }

  public bool TryPeek(TKey key, out TValue data)
  {
    lock (this._lock)
    {
      Node<TKey, TValue> node;
      if (this._data.TryGetValue(key, out node))
      {
        data = node.Value;
        return true;
      }
      data = default (TValue);
      return false;
    }
  }

  public void Clear()
  {
    lock (this._lock)
    {
      this._data.Clear();
      this._head = (Node<TKey, TValue>) null;
      this._tail = (Node<TKey, TValue>) null;
      EventHandler collectionChanged = this.CollectionChanged;
      if (collectionChanged == null)
        return;
      collectionChanged((object) this, new EventArgs());
    }
  }

  public List<TValue> ToList()
  {
    lock (this._lock)
    {
      List<TValue> list = new List<TValue>(this.Count);
      for (Node<TKey, TValue> node = this._head; node != null; node = node.Next)
        list.Add(node.Value);
      return list;
    }
  }

  public TValue Get(TKey key)
  {
    lock (this._lock)
    {
      Node<TKey, TValue> node;
      if (!this._data.TryGetValue(key, out node))
        throw new KeyNotFoundException();
      this.MoveNodeUp(node);
      return node.Value;
    }
  }

  public bool TryGetValue(TKey key, out TValue data)
  {
    lock (this._lock)
    {
      Node<TKey, TValue> node;
      if (this._data.TryGetValue(key, out node))
      {
        data = node.Value;
        this.MoveNodeUp(node);
        return true;
      }
      data = default (TValue);
      return false;
    }
  }

  public bool Remove(TKey key)
  {
    lock (this._lock)
    {
      Node<TKey, TValue> node;
      if (!this._data.TryGetValue(key, out node))
        return false;
      this.RemoveNodeFromList(node);
      return true;
    }
  }

  public bool ContainsKey(TKey key)
  {
    lock (this._lock)
      return this._data.ContainsKey(key);
  }

  public void Add(TKey key, TValue value)
  {
    lock (this._lock)
    {
      if (this._data.ContainsKey(key))
        throw new ArgumentException("Key already exists in cache");
      this.AddItem(key, value);
    }
  }

  public void Add(KeyValuePair<TKey, TValue> item)
  {
    lock (this._lock)
      this.Add(item.Key, item.Value);
  }

  public bool Contains(KeyValuePair<TKey, TValue> item)
  {
    lock (this._lock)
    {
      Node<TKey, TValue> node;
      return this._data.TryGetValue(item.Key, out node) && node.Value.Equals((object) item.Value);
    }
  }

  public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
  {
    lock (this._lock)
    {
      if (array.Length < arrayIndex + this.Count)
        throw new IndexOutOfRangeException("Cache will not fit in array");
      int num = 0;
      for (Node<TKey, TValue> node = this._head; node != null; node = node.Next)
      {
        array[arrayIndex + num] = new KeyValuePair<TKey, TValue>(node.Key, node.Value);
        ++num;
      }
    }
  }

  public bool Remove(KeyValuePair<TKey, TValue> item)
  {
    lock (this._lock)
    {
      Node<TKey, TValue> node;
      if (!this._data.TryGetValue(item.Key, out node) || !node.Value.Equals((object) item.Value))
        return false;
      this.RemoveNodeFromList(node);
      return true;
    }
  }

  public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
  {
    lock (this._lock)
      return (IEnumerator<KeyValuePair<TKey, TValue>>) new CacheEnumerator<TKey, TValue>(this, this._head);
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    lock (this._lock)
      return (IEnumerator) this.GetEnumerator();
  }

  private void RemoveNodeFromList(Node<TKey, TValue> node)
  {
    this._data.Remove(node.Key);
    if (node.Previous != null)
      node.Previous.Next = node.Next;
    if (node.Next != null)
      node.Next.Previous = node.Previous;
    if (node == this._head)
      this._head = node.Next;
    if (node == this._tail)
      this._tail = node.Previous;
    node.Previous = (Node<TKey, TValue>) null;
    node.Next = (Node<TKey, TValue>) null;
    EventHandler collectionChanged = this.CollectionChanged;
    if (collectionChanged == null)
      return;
    collectionChanged((object) this, new EventArgs());
  }

  private void MoveNodeUp(Node<TKey, TValue> node)
  {
    if (node == this._head)
      return;
    if (node.Previous != null)
    {
      if (node == this._tail)
        this._tail = node.Previous;
      node.Previous.Next = node.Next;
    }
    if (node.Next != null)
      node.Next.Previous = node.Previous;
    node.Next = this._head;
    this._head.Previous = node;
    node.Previous = (Node<TKey, TValue>) null;
    this._head = node;
    EventHandler collectionChanged = this.CollectionChanged;
    if (collectionChanged == null)
      return;
    collectionChanged((object) this, new EventArgs());
  }

  private void AddItem(TKey key, TValue value)
  {
    lock (this._lock)
    {
      TValue data = this._createMethod == null ? value : this._createMethod(value);
      Node<TKey, TValue> node = new Node<TKey, TValue>(key, data);
      this._data[key] = node;
      if (this._head == null)
      {
        this._head = node;
        this._tail = node;
      }
      else
      {
        this._head.Previous = node;
        node.Next = this._head;
        this._head = node;
        if (this.Count > this._cacheSize)
          this.RemoveNodeFromList(this._tail);
      }
      EventHandler collectionChanged = this.CollectionChanged;
      if (collectionChanged == null)
        return;
      collectionChanged((object) this, new EventArgs());
    }
  }

  public delegate bool UpdateDataMethod(TValue cachedData, TValue newData);

  public delegate TValue CreateCopyMethod(TValue data);
}
