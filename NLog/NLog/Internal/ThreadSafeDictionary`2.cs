// Decompiled with JetBrains decompiler
// Type: NLog.Internal.ThreadSafeDictionary`2
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace NLog.Internal;

[DebuggerDisplay("Count = {Count}")]
internal class ThreadSafeDictionary<TKey, TValue> : 
  IDictionary<TKey, TValue>,
  ICollection<KeyValuePair<TKey, TValue>>,
  IEnumerable<KeyValuePair<TKey, TValue>>,
  IEnumerable
{
  private readonly object _lockObject = new object();
  private readonly IEqualityComparer<TKey> _comparer;
  private Dictionary<TKey, TValue> _dict;
  private Dictionary<TKey, TValue> _dictReadOnly;

  public ThreadSafeDictionary()
    : this((IEqualityComparer<TKey>) EqualityComparer<TKey>.Default)
  {
  }

  public ThreadSafeDictionary(IEqualityComparer<TKey> comparer)
  {
    this._comparer = comparer;
    this._dict = new Dictionary<TKey, TValue>(this._comparer);
  }

  public ThreadSafeDictionary(ThreadSafeDictionary<TKey, TValue> source)
  {
    this._comparer = source._comparer;
    this._dict = source.GetReadOnlyDict();
    this.GetWritableDict();
  }

  public TValue this[TKey key]
  {
    get => this.GetReadOnlyDict()[key];
    set
    {
      lock (this._lockObject)
        this.GetWritableDict()[key] = value;
    }
  }

  public ICollection<TKey> Keys => (ICollection<TKey>) this.GetReadOnlyDict().Keys;

  public ICollection<TValue> Values => (ICollection<TValue>) this.GetReadOnlyDict().Values;

  public int Count => this.GetReadOnlyDict().Count;

  public bool IsReadOnly => false;

  public void Add(TKey key, TValue value)
  {
    lock (this._lockObject)
      this.GetWritableDict().Add(key, value);
  }

  public void Add(KeyValuePair<TKey, TValue> item)
  {
    lock (this._lockObject)
      this.GetWritableDict().Add(item.Key, item.Value);
  }

  public void Clear()
  {
    lock (this._lockObject)
      this.GetWritableDict(true);
  }

  public bool Contains(KeyValuePair<TKey, TValue> item)
  {
    return this.GetReadOnlyDict().Contains<KeyValuePair<TKey, TValue>>(item);
  }

  public bool ContainsKey(TKey key) => this.GetReadOnlyDict().ContainsKey(key);

  public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
  {
    ((ICollection<KeyValuePair<TKey, TValue>>) this.GetReadOnlyDict()).CopyTo(array, arrayIndex);
  }

  public void CopyFrom(IDictionary<TKey, TValue> source)
  {
    if (this == source || source == null || source.Count <= 0)
      return;
    lock (this._lockObject)
    {
      IDictionary<TKey, TValue> writableDict = this.GetWritableDict();
      foreach (KeyValuePair<TKey, TValue> keyValuePair in (IEnumerable<KeyValuePair<TKey, TValue>>) source)
        writableDict[keyValuePair.Key] = keyValuePair.Value;
    }
  }

  public bool Remove(TKey key)
  {
    lock (this._lockObject)
      return this.GetWritableDict().Remove(key);
  }

  public bool Remove(KeyValuePair<TKey, TValue> item)
  {
    lock (this._lockObject)
      return ((ICollection<KeyValuePair<TKey, TValue>>) this.GetWritableDict()).Remove(item);
  }

  public bool TryGetValue(TKey key, out TValue value)
  {
    return this.GetReadOnlyDict().TryGetValue(key, out value);
  }

  IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
  {
    return (IEnumerator<KeyValuePair<TKey, TValue>>) this.GetReadOnlyDict().GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetReadOnlyDict().GetEnumerator();

  public ThreadSafeDictionary<TKey, TValue>.Enumerator GetEnumerator()
  {
    return new ThreadSafeDictionary<TKey, TValue>.Enumerator(this.GetReadOnlyDict().GetEnumerator());
  }

  private Dictionary<TKey, TValue> GetReadOnlyDict()
  {
    Dictionary<TKey, TValue> readOnlyDict = this._dictReadOnly;
    if (readOnlyDict == null)
    {
      lock (this._lockObject)
        readOnlyDict = this._dictReadOnly = this._dict;
    }
    return readOnlyDict;
  }

  private IDictionary<TKey, TValue> GetWritableDict(bool clearDictionary = false)
  {
    Dictionary<TKey, TValue> writableDict = new Dictionary<TKey, TValue>(clearDictionary ? 0 : this._dict.Count + 1, this._comparer);
    if (!clearDictionary)
    {
      foreach (KeyValuePair<TKey, TValue> keyValuePair in this._dict)
        writableDict[keyValuePair.Key] = keyValuePair.Value;
    }
    this._dict = writableDict;
    this._dictReadOnly = (Dictionary<TKey, TValue>) null;
    return (IDictionary<TKey, TValue>) writableDict;
  }

  public struct Enumerator(Dictionary<TKey, TValue>.Enumerator enumerator) : 
    IEnumerator<KeyValuePair<TKey, TValue>>,
    IDisposable,
    IEnumerator
  {
    private Dictionary<TKey, TValue>.Enumerator _enumerator = enumerator;

    public KeyValuePair<TKey, TValue> Current => this._enumerator.Current;

    object IEnumerator.Current => (object) this._enumerator.Current;

    public void Dispose() => this._enumerator.Dispose();

    public bool MoveNext() => this._enumerator.MoveNext();

    void IEnumerator.Reset() => ((IEnumerator) this._enumerator).Reset();
  }
}
