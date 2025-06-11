// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.SortedListEx`2
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf;

internal class SortedListEx<TKey, TValue> : 
  IDictionary<TKey, TValue>,
  ICollection<KeyValuePair<TKey, TValue>>,
  IEnumerable<KeyValuePair<TKey, TValue>>,
  IEnumerable,
  ICloneable
{
  private const int _defaultCapacity = 16 /*0x10*/;
  private TKey[] m_keys;
  private Dictionary<TKey, TValue> m_values;
  private int m_size;
  private int m_version;
  private IComparer<TKey> m_comparer;
  private SortedListEx<TKey, TValue>.KeyList m_keyList;
  private SortedListEx<TKey, TValue>.ValueList m_valueList;

  public virtual int Capacity
  {
    get => this.m_keys.Length;
    set
    {
      if (value == this.m_keys.Length)
        return;
      if (value < this.m_size)
        throw new ArgumentOutOfRangeException(nameof (value));
      if (value > 0)
      {
        TKey[] destinationArray = new TKey[value];
        if (this.m_size > 0)
          Array.Copy((Array) this.m_keys, 0, (Array) destinationArray, 0, this.m_size);
        this.m_keys = destinationArray;
      }
      else
        this.m_keys = new TKey[16 /*0x10*/];
    }
  }

  public virtual int Count => this.m_size;

  public virtual IList<TKey> Keys => this.GetKeyList();

  ICollection<TKey> IDictionary<TKey, TValue>.Keys => (ICollection<TKey>) this.GetKeyList();

  public virtual IList<TValue> Values => this.GetValueList();

  ICollection<TValue> IDictionary<TKey, TValue>.Values => (ICollection<TValue>) this.GetValueList();

  public virtual bool IsReadOnly => false;

  public virtual bool IsFixedSize => false;

  public virtual bool IsSynchronized => false;

  public virtual object SyncRoot => (object) this;

  public virtual TValue this[TKey key]
  {
    get => this.m_values[key];
    set
    {
      if ((object) key == null)
        throw new ArgumentNullException(nameof (key));
      if (this.m_values.ContainsKey(key))
        this.m_values[key] = value;
      else
        this.Add(key, value);
      ++this.m_version;
    }
  }

  public SortedListEx()
  {
    this.m_keys = new TKey[16 /*0x10*/];
    this.m_values = new Dictionary<TKey, TValue>(16 /*0x10*/);
    this.m_comparer = (IComparer<TKey>) Comparer<TKey>.Default;
  }

  public SortedListEx(int initialCapacity)
  {
    this.m_keys = initialCapacity >= 0 ? new TKey[initialCapacity] : throw new ArgumentOutOfRangeException(nameof (initialCapacity));
    this.m_values = new Dictionary<TKey, TValue>(initialCapacity);
    this.m_comparer = (IComparer<TKey>) Comparer<TKey>.Default;
  }

  public SortedListEx(IComparer<TKey> comparer)
    : this()
  {
    if (comparer == null)
      return;
    this.m_comparer = comparer;
  }

  public SortedListEx(IComparer<TKey> comparer, int capacity)
    : this(comparer)
  {
    this.Capacity = capacity;
  }

  public SortedListEx(IDictionary<TKey, TValue> d)
    : this(d, (IComparer<TKey>) null)
  {
  }

  public SortedListEx(IDictionary<TKey, TValue> d, IComparer<TKey> comparer)
    : this(comparer, d != null ? d.Count : 0)
  {
    if (d == null)
      throw new ArgumentNullException(nameof (d));
    d.Keys.CopyTo(this.m_keys, 0);
    this.m_values = new Dictionary<TKey, TValue>(d);
    Array.Sort<TKey>(this.m_keys, comparer);
    this.m_size = d.Count;
  }

  public static SortedListEx<TKey, TValue> Synchronized(SortedListEx<TKey, TValue> list)
  {
    return list != null ? (SortedListEx<TKey, TValue>) new SortedListEx<TKey, TValue>.SyncSortedListEx(list) : throw new ArgumentNullException(nameof (list));
  }

  public virtual void Add(TKey key, TValue value)
  {
    if ((object) key == null)
      throw new ArgumentNullException(nameof (key));
    if (this.m_values.ContainsKey(key))
      throw new ArgumentException("Duplicated");
    this.Insert(~Array.BinarySearch<TKey>(this.m_keys, 0, this.m_size, key, this.m_comparer), key, value);
  }

  public virtual void Add(KeyValuePair<TKey, TValue> pair) => this.Add(pair.Key, pair.Value);

  public virtual void Clear()
  {
    ++this.m_version;
    this.m_size = 0;
    this.m_keys = new TKey[16 /*0x10*/];
    this.m_values = new Dictionary<TKey, TValue>(16 /*0x10*/);
  }

  public virtual object Clone()
  {
    SortedListEx<TKey, TValue> sortedListEx = new SortedListEx<TKey, TValue>(this.m_size);
    Array.Copy((Array) this.m_keys, 0, (Array) sortedListEx.m_keys, 0, this.m_size);
    sortedListEx.m_values = new Dictionary<TKey, TValue>((IDictionary<TKey, TValue>) this.m_values);
    sortedListEx.m_size = this.m_size;
    sortedListEx.m_version = this.m_version;
    sortedListEx.m_comparer = this.m_comparer;
    return (object) sortedListEx;
  }

  public SortedListEx<TKey, TValue> CloneAll()
  {
    int count = this.Count;
    SortedListEx<TKey, TValue> sortedListEx = new SortedListEx<TKey, TValue>(count + 1);
    for (int index = 0; index < count; ++index)
    {
      TValue obj = (TValue) ((ICloneable) (object) this.GetByIndex(index)).Clone();
      sortedListEx.Add(this.GetKey(index), obj);
    }
    return sortedListEx;
  }

  public virtual bool Contains(TKey key) => this.m_values.ContainsKey(key);

  public virtual bool ContainsKey(TKey key) => this.m_values.ContainsKey(key);

  public virtual bool ContainsValue(TValue value) => this.m_values.ContainsValue(value);

  public virtual bool Contains(KeyValuePair<TKey, TValue> pair)
  {
    bool flag = false;
    if (this.ContainsKey(pair.Key))
      flag = pair.Value.Equals((object) this[pair.Key]);
    return flag;
  }

  public virtual void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    if (array.Rank != 1)
      throw new ArgumentException();
    if (arrayIndex < 0)
      throw new ArgumentOutOfRangeException(nameof (arrayIndex));
    if (array.Length - arrayIndex < this.Count)
      throw new ArgumentException();
    for (int index = 0; index < this.Count; ++index)
    {
      TKey key = this.m_keys[index];
      KeyValuePair<TKey, TValue> keyValuePair = new KeyValuePair<TKey, TValue>(key, this.m_values[key]);
      array.SetValue((object) keyValuePair, index + arrayIndex);
    }
  }

  public virtual TValue GetByIndex(int index)
  {
    if (index < 0 || index >= this.m_size)
      throw new ArgumentOutOfRangeException(nameof (index));
    return this.m_values[this.m_keys[index]];
  }

  public virtual TKey GetKey(int index)
  {
    return index >= 0 && index < this.m_size ? this.m_keys[index] : throw new ArgumentOutOfRangeException(nameof (index));
  }

  public virtual IList<TKey> GetKeyList()
  {
    if (this.m_keyList == null)
      this.m_keyList = new SortedListEx<TKey, TValue>.KeyList(this);
    return (IList<TKey>) this.m_keyList;
  }

  public virtual IList<TValue> GetValueList()
  {
    if (this.m_valueList == null)
      this.m_valueList = new SortedListEx<TKey, TValue>.ValueList(this);
    else
      this.m_valueList.UpdateValues();
    return (IList<TValue>) this.m_valueList;
  }

  public virtual int IndexOfKey(TKey key)
  {
    if ((object) key == null)
      throw new ArgumentNullException(nameof (key));
    int num = Array.BinarySearch<TKey>(this.m_keys, 0, this.m_size, key, this.m_comparer);
    return num < 0 ? -1 : num;
  }

  public virtual int IndexOfValue(TValue value)
  {
    IEnumerator<KeyValuePair<TKey, TValue>> enumerator = (IEnumerator<KeyValuePair<TKey, TValue>>) this.m_values.GetEnumerator();
    enumerator.Reset();
    while (enumerator.MoveNext())
    {
      if (enumerator.Current.Value.Equals((object) value))
        return Array.IndexOf<TKey>(this.m_keys, enumerator.Current.Key, 0, this.m_size);
    }
    return -1;
  }

  public virtual void RemoveAt(int index)
  {
    if (index < 0 || index >= this.m_size)
      throw new ArgumentOutOfRangeException(nameof (index));
    --this.m_size;
    TKey key = this.m_keys[index];
    if (index < this.m_size)
      Array.Copy((Array) this.m_keys, index + 1, (Array) this.m_keys, index, this.m_size - index);
    this.m_keys[this.m_size] = default (TKey);
    this.m_values.Remove(key);
    ++this.m_version;
  }

  public virtual bool Remove(TKey key)
  {
    int index = this.IndexOfKey(key);
    bool flag = false;
    if (index >= 0)
    {
      this.RemoveAt(index);
      flag = true;
    }
    return flag;
  }

  public virtual bool Remove(KeyValuePair<TKey, TValue> pair)
  {
    bool flag = false;
    if (this.Contains(pair) && pair.Value.Equals((object) this[pair.Key]))
    {
      this.Remove(pair.Key);
      flag = true;
    }
    return flag;
  }

  public virtual void SetByIndex(int index, TValue value)
  {
    if (index < 0 || index >= this.m_size)
      throw new ArgumentOutOfRangeException(nameof (index));
    this.m_values[this.m_keys[index]] = value;
    ++this.m_version;
  }

  public virtual void TrimToSize() => this.Capacity = this.m_size;

  public virtual IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
  {
    return (IEnumerator<KeyValuePair<TKey, TValue>>) new SortedListEx<TKey, TValue>.SortedListExEnumerator(this, 0, this.m_size);
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  public virtual bool TryGetValue(TKey key, out TValue value)
  {
    bool flag = false;
    if (this.ContainsKey(key))
    {
      value = this[key];
      flag = true;
    }
    else
      value = default (TValue);
    return flag;
  }

  private void Insert(int index, TKey key, TValue value)
  {
    if (this.m_size == this.m_keys.Length)
      this.EnsureCapacity(this.m_size + 1);
    if (index < this.m_size)
      Array.Copy((Array) this.m_keys, index, (Array) this.m_keys, index + 1, this.m_size - index);
    this.m_keys[index] = key;
    this.m_values[key] = value;
    ++this.m_size;
    ++this.m_version;
  }

  private void EnsureCapacity(int min)
  {
    int num = this.m_keys.Length == 0 ? 16 /*0x10*/ : this.m_keys.Length * 2;
    if (num < min)
      num = min;
    this.Capacity = num;
  }

  private class SyncSortedListEx : SortedListEx<TKey, TValue>
  {
    private SortedListEx<TKey, TValue> m_list;
    private object m_root;

    internal SyncSortedListEx(SortedListEx<TKey, TValue> list)
    {
      this.m_list = list;
      this.m_root = list.SyncRoot;
    }

    public override int Capacity
    {
      get
      {
        lock (this.m_root)
          return this.m_list.Capacity;
      }
    }

    public override int Count
    {
      get
      {
        lock (this.m_root)
          return this.m_list.Count;
      }
    }

    public override object SyncRoot => this.m_root;

    public override bool IsReadOnly => this.m_list.IsReadOnly;

    public override bool IsFixedSize => this.m_list.IsFixedSize;

    public override bool IsSynchronized => true;

    public override TValue this[TKey key]
    {
      get
      {
        lock (this.m_root)
          return this.m_list[key];
      }
      set
      {
        lock (this.m_root)
          this.m_list[key] = value;
      }
    }

    public override void Add(TKey key, TValue value)
    {
      lock (this.m_root)
        this.m_list.Add(key, value);
    }

    public override void Clear()
    {
      lock (this.m_root)
        this.m_list.Clear();
    }

    public override object Clone()
    {
      lock (this.m_root)
        return this.m_list.Clone();
    }

    public override bool Contains(TKey key)
    {
      lock (this.m_root)
        return this.m_list.Contains(key);
    }

    public override bool ContainsKey(TKey key)
    {
      lock (this.m_root)
        return this.m_list.ContainsKey(key);
    }

    public override bool ContainsValue(TValue value)
    {
      lock (this.m_root)
        return this.m_list.ContainsValue(value);
    }

    public override void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
    {
      lock (this.m_root)
        this.m_list.CopyTo(array, index);
    }

    public override TValue GetByIndex(int index)
    {
      lock (this.m_root)
        return this.m_list.GetByIndex(index);
    }

    public override IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
      lock (this.m_root)
        return this.m_list.GetEnumerator();
    }

    public override TKey GetKey(int index)
    {
      lock (this.m_root)
        return this.m_list.GetKey(index);
    }

    public override IList<TKey> GetKeyList()
    {
      lock (this.m_root)
        return this.m_list.GetKeyList();
    }

    public override IList<TValue> GetValueList()
    {
      lock (this.m_root)
        return this.m_list.GetValueList();
    }

    public override int IndexOfKey(TKey key)
    {
      lock (this.m_root)
        return this.m_list.IndexOfKey(key);
    }

    public override int IndexOfValue(TValue value)
    {
      lock (this.m_root)
        return this.m_list.IndexOfValue(value);
    }

    public override void RemoveAt(int index)
    {
      lock (this.m_root)
        this.m_list.RemoveAt(index);
    }

    public override bool Remove(TKey key)
    {
      lock (this.m_root)
        return this.m_list.Remove(key);
    }

    public override void SetByIndex(int index, TValue value)
    {
      lock (this.m_root)
        this.m_list.SetByIndex(index, value);
    }

    public override void TrimToSize()
    {
      lock (this.m_root)
        this.m_list.TrimToSize();
    }
  }

  private class SortedListExEnumerator : 
    IEnumerator<KeyValuePair<TKey, TValue>>,
    IDisposable,
    IEnumerator,
    ICloneable
  {
    private SortedListEx<TKey, TValue> m_sortedListEx;
    private TKey m_key;
    private TValue m_value;
    private int m_index;
    private int m_startIndex;
    private int m_endIndex;
    private int m_version;
    private bool m_current;
    private bool m_isDisposed;

    internal SortedListExEnumerator(SortedListEx<TKey, TValue> sortedListEx, int index, int count)
    {
      this.m_sortedListEx = sortedListEx;
      this.m_index = index;
      this.m_startIndex = index;
      this.m_endIndex = index + count;
      this.m_version = sortedListEx.m_version;
      this.m_current = false;
    }

    public object Clone() => this.MemberwiseClone();

    public virtual TKey Key
    {
      get
      {
        if (this.m_version != this.m_sortedListEx.m_version)
          throw new InvalidOperationException();
        if (!this.m_current)
          throw new InvalidOperationException();
        return this.m_key;
      }
    }

    public virtual bool MoveNext()
    {
      if (this.m_version != this.m_sortedListEx.m_version)
        throw new InvalidOperationException();
      if (this.m_index < this.m_endIndex)
      {
        this.m_key = this.m_sortedListEx.m_keys[this.m_index];
        this.m_value = this.m_sortedListEx.m_values[this.m_key];
        ++this.m_index;
        this.m_current = true;
        return true;
      }
      this.m_key = default (TKey);
      this.m_value = default (TValue);
      this.m_current = false;
      return false;
    }

    public virtual KeyValuePair<TKey, TValue> Current
    {
      get
      {
        if (!this.m_current)
          throw new InvalidOperationException();
        return new KeyValuePair<TKey, TValue>(this.m_key, this.m_value);
      }
    }

    public virtual TValue Value
    {
      get
      {
        if (this.m_version != this.m_sortedListEx.m_version)
          throw new InvalidOperationException();
        if (!this.m_current)
          throw new InvalidOperationException();
        return this.m_value;
      }
    }

    public virtual void Reset()
    {
      if (this.m_version != this.m_sortedListEx.m_version)
        throw new InvalidOperationException();
      this.m_index = this.m_startIndex;
      this.m_current = false;
      this.m_key = default (TKey);
      this.m_value = default (TValue);
    }

    public void Dispose()
    {
      if (this.m_isDisposed)
        return;
      this.m_isDisposed = true;
      this.m_sortedListEx = (SortedListEx<TKey, TValue>) null;
      this.m_current = false;
    }

    object IEnumerator.Current => (object) this.Current;
  }

  private class KeyList : IList<TKey>, ICollection<TKey>, IEnumerable<TKey>, IEnumerable
  {
    private SortedListEx<TKey, TValue> m_sortedListEx;

    internal KeyList(SortedListEx<TKey, TValue> sortedListEx) => this.m_sortedListEx = sortedListEx;

    public virtual int Count => this.m_sortedListEx.m_size;

    public virtual bool IsReadOnly => true;

    public virtual bool IsFixedSize => true;

    public virtual bool IsSynchronized => this.m_sortedListEx.IsSynchronized;

    public virtual object SyncRoot => this.m_sortedListEx.SyncRoot;

    public virtual void Add(TKey key) => throw new NotSupportedException();

    public virtual void Clear() => throw new NotSupportedException();

    public virtual bool Contains(TKey key) => this.m_sortedListEx.Contains(key);

    public virtual void CopyTo(TKey[] array, int arrayIndex)
    {
      if (array != null && array.Rank != 1)
        throw new ArgumentException();
      Array.Copy((Array) this.m_sortedListEx.m_keys, 0, (Array) array, arrayIndex, this.m_sortedListEx.Count);
    }

    public virtual void Insert(int index, TKey value) => throw new NotSupportedException();

    public virtual TKey this[int index]
    {
      get => this.m_sortedListEx.GetKey(index);
      set => throw new NotSupportedException();
    }

    public virtual IEnumerator<TKey> GetEnumerator()
    {
      for (int index = 0; index < this.m_sortedListEx.m_size; ++index)
      {
        TKey current = this.m_sortedListEx.m_keys[index];
        yield return current;
      }
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public virtual int IndexOf(TKey key)
    {
      if ((object) key == null)
        throw new ArgumentNullException(nameof (key));
      int num = Array.BinarySearch<TKey>(this.m_sortedListEx.m_keys, 0, this.m_sortedListEx.Count, key, this.m_sortedListEx.m_comparer);
      return num >= 0 ? num : -1;
    }

    public virtual bool Remove(TKey key) => throw new NotSupportedException();

    public virtual void RemoveAt(int index) => throw new NotSupportedException();
  }

  private class ValueList : IList<TValue>, ICollection<TValue>, IEnumerable<TValue>, IEnumerable
  {
    private SortedListEx<TKey, TValue> m_sortedListEx;
    private TValue[] m_values;

    internal ValueList(SortedListEx<TKey, TValue> sortedListEx)
    {
      this.m_sortedListEx = sortedListEx;
      this.UpdateValues();
    }

    public virtual void UpdateValues()
    {
      int count = this.m_sortedListEx.Count;
      this.m_values = new TValue[count];
      this.m_sortedListEx.m_values.Values.CopyTo(this.m_values, 0);
      TKey[] keyArray = new TKey[count];
      this.m_sortedListEx.m_values.Keys.CopyTo(keyArray, 0);
      Array.Sort<TKey, TValue>(keyArray, this.m_values, this.m_sortedListEx.m_comparer);
    }

    public virtual int Count => this.m_sortedListEx.m_size;

    public virtual bool IsReadOnly => true;

    public virtual bool IsFixedSize => true;

    public virtual bool IsSynchronized => this.m_sortedListEx.IsSynchronized;

    public virtual object SyncRoot => this.m_sortedListEx.SyncRoot;

    public virtual void Add(TValue value) => throw new NotSupportedException();

    public virtual void Clear() => throw new NotSupportedException();

    public virtual bool Contains(TValue value) => this.m_sortedListEx.ContainsValue(value);

    public virtual void CopyTo(TValue[] array, int arrayIndex)
    {
      if (array != null && array.Rank != 1)
        throw new ArgumentException();
      Array.Copy((Array) this.m_values, 0, (Array) array, arrayIndex, this.m_sortedListEx.Count);
    }

    public virtual void Insert(int index, TValue value) => throw new NotSupportedException();

    public virtual TValue this[int index]
    {
      get => this.m_sortedListEx.GetByIndex(index);
      set => this.m_sortedListEx.SetByIndex(index, value);
    }

    public virtual IEnumerator<TValue> GetEnumerator()
    {
      int index = 0;
      TKey[] keys = this.m_sortedListEx.m_keys;
      for (; index < this.m_sortedListEx.m_size; ++index)
      {
        TValue current = this.m_sortedListEx.m_values[keys[index]];
        yield return current;
      }
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public virtual int IndexOf(TValue value)
    {
      return Array.IndexOf<TValue>(this.m_values, value, 0, this.m_sortedListEx.Count);
    }

    public virtual bool Remove(TValue value) => throw new NotSupportedException();

    public virtual void RemoveAt(int index) => throw new NotSupportedException();
  }
}
