// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.TypedSortedListEx`2
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class TypedSortedListEx<TKey, TValue> : 
  IDictionary<TKey, TValue>,
  ICollection<KeyValuePair<TKey, TValue>>,
  IEnumerable<KeyValuePair<TKey, TValue>>,
  IDictionary,
  ICollection,
  IEnumerable
  where TKey : IComparable
{
  private const int DefaultCapacity = 16 /*0x10*/;
  private TKey[] m_arrKeys;
  private Dictionary<TKey, TValue> m_dicValues;
  private int m_iSize;
  private int m_iVersion;
  private IComparer<TKey> m_comparer;
  private TypedSortedListEx<TKey, TValue>.KeyList m_listKeys;
  private TypedSortedListEx<TKey, TValue>.ValueList m_lstValues;

  public virtual int Capacity
  {
    get => this.m_arrKeys.Length;
    set
    {
      if (value == this.m_arrKeys.Length)
        return;
      if (value < this.m_iSize)
        throw new ArgumentOutOfRangeException(nameof (value));
      if (value > 0)
      {
        TKey[] destinationArray = new TKey[value];
        if (this.m_iSize > 0)
          Array.Copy((Array) this.m_arrKeys, 0, (Array) destinationArray, 0, this.m_iSize);
        this.m_arrKeys = destinationArray;
      }
      else
        this.m_arrKeys = new TKey[16 /*0x10*/];
    }
  }

  public virtual int Count => this.m_iSize;

  public virtual IList<TKey> Keys => this.GetKeyList();

  public virtual IList<TValue> Values => this.GetValueList();

  public virtual bool IsReadOnly => false;

  public virtual bool IsFixedSize => false;

  public virtual bool IsSynchronized => false;

  public virtual object SyncRoot => (object) this;

  public virtual TValue this[TKey key]
  {
    get
    {
      TValue obj;
      this.m_dicValues.TryGetValue(key, out obj);
      return obj;
    }
    set
    {
      if ((object) key == null)
        throw new ArgumentNullException(nameof (key));
      if (this.m_dicValues.ContainsKey(key))
        this.m_dicValues[key] = value;
      else
        this.Add(key, value);
      ++this.m_iVersion;
    }
  }

  public TypedSortedListEx()
  {
    this.m_arrKeys = new TKey[16 /*0x10*/];
    this.m_dicValues = new Dictionary<TKey, TValue>(16 /*0x10*/);
    this.m_comparer = (IComparer<TKey>) Comparer<TKey>.Default;
  }

  public TypedSortedListEx(int initialCapacity)
  {
    this.m_arrKeys = initialCapacity >= 0 ? new TKey[initialCapacity] : throw new ArgumentOutOfRangeException(nameof (initialCapacity));
    this.m_dicValues = new Dictionary<TKey, TValue>(initialCapacity);
    this.m_comparer = (IComparer<TKey>) Comparer<TKey>.Default;
  }

  public TypedSortedListEx(IComparer<TKey> comparer)
    : this()
  {
    if (comparer == null)
      return;
    this.m_comparer = comparer;
  }

  public TypedSortedListEx(IComparer<TKey> comparer, int capacity)
    : this(comparer)
  {
    this.Capacity = capacity;
  }

  public TypedSortedListEx(IDictionary<TKey, TValue> d)
    : this(d, (IComparer<TKey>) null)
  {
  }

  public TypedSortedListEx(IDictionary<TKey, TValue> d, IComparer<TKey> comparer)
    : this(comparer, d != null ? d.Count : 0)
  {
    if (d == null)
      throw new ArgumentNullException(nameof (d));
    d.Keys.CopyTo(this.m_arrKeys, 0);
    this.m_dicValues = new Dictionary<TKey, TValue>(d);
    Array.Sort<TKey>(this.m_arrKeys, comparer);
    this.m_iSize = d.Count;
  }

  public static TypedSortedListEx<TKey, TValue> Synchronized(TypedSortedListEx<TKey, TValue> list)
  {
    if (list == null)
      throw new ArgumentNullException(nameof (list));
    throw new NotImplementedException();
  }

  public virtual void Add(TKey key, TValue value)
  {
    if ((object) key == null)
      throw new ArgumentNullException(nameof (key));
    if (this.m_dicValues.ContainsKey(key))
      throw new ArgumentException("Duplicated");
    this.Insert(~Array.BinarySearch<TKey>(this.m_arrKeys, 0, this.m_iSize, key, this.m_comparer), key, value);
  }

  public virtual void Clear()
  {
    ++this.m_iVersion;
    this.m_iSize = 0;
    this.m_arrKeys = new TKey[16 /*0x10*/];
    this.m_dicValues = new Dictionary<TKey, TValue>(16 /*0x10*/);
  }

  public virtual object Clone()
  {
    TypedSortedListEx<TKey, TValue> typedSortedListEx = new TypedSortedListEx<TKey, TValue>(this.m_iSize);
    Array.Copy((Array) this.m_arrKeys, 0, (Array) typedSortedListEx.m_arrKeys, 0, this.m_iSize);
    typedSortedListEx.m_dicValues = new Dictionary<TKey, TValue>((IDictionary<TKey, TValue>) this.m_dicValues);
    typedSortedListEx.m_iSize = this.m_iSize;
    typedSortedListEx.m_iVersion = this.m_iVersion;
    typedSortedListEx.m_comparer = this.m_comparer;
    return (object) typedSortedListEx;
  }

  public TypedSortedListEx<TKey, TValue> CloneAll()
  {
    int count = this.Count;
    TypedSortedListEx<TKey, TValue> typedSortedListEx = (TypedSortedListEx<TKey, TValue>) this.MemberwiseClone();
    typedSortedListEx.m_arrKeys = new TKey[count];
    typedSortedListEx.m_dicValues = new Dictionary<TKey, TValue>(count);
    typedSortedListEx.m_listKeys = (TypedSortedListEx<TKey, TValue>.KeyList) null;
    typedSortedListEx.m_lstValues = (TypedSortedListEx<TKey, TValue>.ValueList) null;
    typedSortedListEx.m_iSize = 0;
    for (int index = 0; index < count; ++index)
    {
      TKey key = this.GetKey(index);
      TValue obj = this.m_dicValues[key];
      if (obj is ICloneable cloneable)
        obj = (TValue) cloneable.Clone();
      typedSortedListEx.Add(key, obj);
    }
    return typedSortedListEx;
  }

  public virtual bool Contains(TKey key) => this.m_dicValues.ContainsKey(key);

  public virtual bool ContainsKey(TKey key) => this.m_dicValues.ContainsKey(key);

  public virtual bool ContainsValue(TValue value) => this.m_dicValues.ContainsValue(value);

  public virtual void CopyTo(Array array, int arrayIndex)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    if (array.Rank != 1)
      throw new ArgumentException(nameof (array));
    if (arrayIndex < 0)
      throw new ArgumentOutOfRangeException(nameof (arrayIndex));
    if (array.Length - arrayIndex < this.Count)
      throw new ArgumentException();
    for (int index = 0; index < this.Count; ++index)
    {
      KeyValuePair<TKey, TValue> keyValuePair = new KeyValuePair<TKey, TValue>(this.m_arrKeys[index], this.m_dicValues[this.m_arrKeys[index]]);
      array.SetValue((object) keyValuePair, index + arrayIndex);
    }
  }

  public virtual TValue GetByIndex(int index)
  {
    if (index < 0 || index >= this.m_iSize)
      throw new ArgumentOutOfRangeException(nameof (index));
    return this.m_dicValues[this.m_arrKeys[index]];
  }

  public virtual TValue GetByName(string name)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    string[] array = new string[this.m_arrKeys.Length];
    this.m_arrKeys.CopyTo((Array) array, 0);
    int index = Array.IndexOf<string>(array, name);
    if (index < 0 || index >= this.m_arrKeys.Length)
      throw new ArgumentOutOfRangeException("index");
    return this.m_dicValues[this.m_arrKeys[index]];
  }

  public virtual TKey GetKey(int index)
  {
    return index >= 0 && index < this.m_iSize ? this.m_arrKeys[index] : throw new ArgumentOutOfRangeException(nameof (index));
  }

  public virtual IList<TKey> GetKeyList()
  {
    if (this.m_listKeys == null)
      this.m_listKeys = new TypedSortedListEx<TKey, TValue>.KeyList(this);
    return (IList<TKey>) this.m_listKeys;
  }

  public virtual IList<TValue> GetValueList()
  {
    if (this.m_lstValues == null)
      this.m_lstValues = new TypedSortedListEx<TKey, TValue>.ValueList(this);
    else
      this.m_lstValues.UpdateValues();
    return (IList<TValue>) this.m_lstValues;
  }

  public virtual int IndexOfKey(TKey key)
  {
    if ((object) key == null)
      throw new ArgumentNullException(nameof (key));
    int num = Array.BinarySearch<TKey>(this.m_arrKeys, 0, this.m_iSize, key, this.m_comparer);
    return num < 0 ? -1 : num;
  }

  public virtual int IndexOfValue(TValue value)
  {
    object obj = (object) null;
    IDictionaryEnumerator enumerator = (IDictionaryEnumerator) this.m_dicValues.GetEnumerator();
    enumerator.Reset();
    while (enumerator.MoveNext())
    {
      if (enumerator.Value.Equals((object) value))
      {
        obj = enumerator.Key;
        break;
      }
    }
    return obj == null ? -1 : Array.IndexOf((Array) this.m_arrKeys, obj, 0, this.m_iSize);
  }

  public virtual void RemoveAt(int index)
  {
    if (index < 0 || index >= this.m_iSize)
      throw new ArgumentOutOfRangeException(nameof (index));
    --this.m_iSize;
    TKey arrKey = this.m_arrKeys[index];
    if (index < this.m_iSize)
      Array.Copy((Array) this.m_arrKeys, index + 1, (Array) this.m_arrKeys, index, this.m_iSize - index);
    this.m_arrKeys[this.m_iSize] = default (TKey);
    this.m_dicValues.Remove(arrKey);
    ++this.m_iVersion;
  }

  public virtual bool Remove(TKey key)
  {
    int index = this.IndexOfKey(key);
    bool flag;
    if (index >= 0)
    {
      this.RemoveAt(index);
      flag = true;
    }
    else
      flag = false;
    return flag;
  }

  public virtual void SetByIndex(int index, TValue value)
  {
    if (index < 0 || index >= this.m_iSize)
      throw new ArgumentOutOfRangeException(nameof (index));
    this.m_dicValues[this.m_arrKeys[index]] = value;
    ++this.m_iVersion;
  }

  public virtual void TrimToSize() => this.Capacity = this.m_iSize;

  IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();

  public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
  {
    return (IEnumerator<KeyValuePair<TKey, TValue>>) new TypedSortedListEx<TKey, TValue>.TypedSortedListExEnumerator(this, 0, this.m_iSize);
  }

  private void Insert(int index, TKey key, TValue value)
  {
    if (this.m_iSize == this.m_arrKeys.Length)
      this.EnsureCapacity(this.m_iSize + 1);
    if (index < this.m_iSize)
      Array.Copy((Array) this.m_arrKeys, index, (Array) this.m_arrKeys, index + 1, this.m_iSize - index);
    this.m_arrKeys[index] = key;
    this.m_dicValues[key] = value;
    ++this.m_iSize;
    ++this.m_iVersion;
  }

  private void EnsureCapacity(int min)
  {
    int num = this.m_arrKeys.Length == 0 ? 16 /*0x10*/ : this.m_arrKeys.Length * 2;
    if (num < min)
      num = min;
    this.Capacity = num;
  }

  public void Add(object key, object value) => this.Add((TKey) key, (TValue) value);

  public bool Contains(object key) => key is TKey key1 && this.ContainsKey(key1);

  IDictionaryEnumerator IDictionary.GetEnumerator()
  {
    return ((IDictionary) this.m_dicValues).GetEnumerator();
  }

  ICollection IDictionary.Keys => (ICollection) this.m_dicValues.Keys;

  public void Remove(object key)
  {
    if (!(key is TKey key1))
      return;
    this.Remove(key1);
  }

  ICollection IDictionary.Values => (ICollection) this.m_dicValues.Values;

  public object this[object key]
  {
    get => (object) (key is TKey key1 ? this[key1] : default (TValue));
    set => this[(TKey) key] = (TValue) value;
  }

  ICollection<TKey> IDictionary<TKey, TValue>.Keys => (ICollection<TKey>) this.Keys;

  public bool TryGetValue(TKey key, out TValue value)
  {
    return this.m_dicValues.TryGetValue(key, out value);
  }

  ICollection<TValue> IDictionary<TKey, TValue>.Values
  {
    get => (ICollection<TValue>) this.m_dicValues.Values;
  }

  public void Add(KeyValuePair<TKey, TValue> item) => this.Add(item.Key, item.Value);

  public bool Contains(KeyValuePair<TKey, TValue> item)
  {
    TValue obj;
    return this.TryGetValue(item.Key, out obj) && obj.Equals((object) item.Value);
  }

  public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
  {
    ((ICollection<KeyValuePair<TKey, TValue>>) this.m_dicValues).CopyTo(array, arrayIndex);
  }

  public bool Remove(KeyValuePair<TKey, TValue> item) => this.Remove(item.Key);

  [Serializable]
  private class TypedSortedListExEnumerator : 
    IEnumerator<KeyValuePair<TKey, TValue>>,
    IDisposable,
    IEnumerator,
    ICloneable
  {
    private TypedSortedListEx<TKey, TValue> m_list;
    private TKey m_key;
    private TValue m_value;
    private int m_iIndex;
    private int m_iStartIndex;
    private int m_iEndIndex;
    private int m_iVersion;
    private bool m_bCurrent;

    internal TypedSortedListExEnumerator(
      TypedSortedListEx<TKey, TValue> list,
      int index,
      int count)
    {
      this.m_list = list;
      this.m_iIndex = index;
      this.m_iStartIndex = index;
      this.m_iEndIndex = index + count;
      this.m_iVersion = this.m_list.m_iVersion;
      this.m_bCurrent = false;
    }

    public void Dispose()
    {
    }

    public object Clone() => this.MemberwiseClone();

    public virtual TKey Key
    {
      get
      {
        if (this.m_iVersion != this.m_list.m_iVersion)
          throw new InvalidOperationException();
        if (!this.m_bCurrent)
          throw new InvalidOperationException();
        return this.m_key;
      }
    }

    public virtual bool MoveNext()
    {
      if (this.m_iVersion != this.m_list.m_iVersion)
        throw new InvalidOperationException();
      if (this.m_iIndex < this.m_iEndIndex)
      {
        this.m_key = this.m_list.m_arrKeys[this.m_iIndex];
        this.m_value = this.m_list.m_dicValues[this.m_key];
        ++this.m_iIndex;
        this.m_bCurrent = true;
        return true;
      }
      this.m_key = default (TKey);
      this.m_value = default (TValue);
      this.m_bCurrent = false;
      return false;
    }

    public virtual KeyValuePair<TKey, TValue> Entry
    {
      get
      {
        if (this.m_iVersion != this.m_list.m_iVersion)
          throw new InvalidOperationException();
        if (!this.m_bCurrent)
          throw new InvalidOperationException();
        return new KeyValuePair<TKey, TValue>(this.m_key, this.m_value);
      }
    }

    public virtual KeyValuePair<TKey, TValue> Current
    {
      get
      {
        if (!this.m_bCurrent)
          throw new InvalidOperationException();
        return new KeyValuePair<TKey, TValue>(this.m_key, this.m_value);
      }
    }

    object IEnumerator.Current
    {
      get
      {
        if (!this.m_bCurrent)
          throw new InvalidOperationException();
        return (object) new KeyValuePair<TKey, TValue>(this.m_key, this.m_value);
      }
    }

    public virtual object Value
    {
      get
      {
        if (this.m_iVersion != this.m_list.m_iVersion)
          throw new InvalidOperationException();
        if (!this.m_bCurrent)
          throw new InvalidOperationException();
        return (object) this.m_value;
      }
    }

    public virtual void Reset()
    {
      if (this.m_iVersion != this.m_list.m_iVersion)
        throw new InvalidOperationException();
      this.m_iIndex = this.m_iStartIndex;
      this.m_bCurrent = false;
      this.m_key = default (TKey);
      this.m_value = default (TValue);
    }
  }

  private class KeysEnumerator : IEnumerator<TKey>, IDisposable, IEnumerator
  {
    private TypedSortedListEx<TKey, TValue> m_list;
    private int m_iIndex = -1;
    private int m_iVersion;

    public KeysEnumerator(TypedSortedListEx<TKey, TValue> list)
    {
      this.m_list = list != null ? list : throw new ArgumentNullException(nameof (list));
      this.m_iVersion = this.m_list.m_iVersion;
    }

    public TKey Current
    {
      get
      {
        if (this.m_iVersion != this.m_list.m_iVersion)
          throw new InvalidOperationException("Parent collection was changed");
        if (this.m_iIndex < 0 || this.m_iIndex >= this.m_list.m_iSize)
          throw new InvalidOperationException();
        return this.m_list.m_arrKeys[this.m_iIndex];
      }
    }

    public void Dispose()
    {
    }

    object IEnumerator.Current
    {
      get
      {
        if (this.m_iIndex < 0 || this.m_iIndex >= this.m_list.m_iSize)
          throw new InvalidOperationException();
        if (this.m_iVersion != this.m_list.m_iVersion)
          throw new InvalidOperationException("Parent collection was changed");
        return (object) this.m_list.m_arrKeys[this.m_iIndex];
      }
    }

    public bool MoveNext()
    {
      if (this.m_iVersion != this.m_list.m_iVersion)
        throw new InvalidOperationException("Parent collection was changed");
      if (this.m_iIndex < 0)
        this.m_iIndex = 0;
      else
        ++this.m_iIndex;
      if (this.m_iIndex >= this.m_list.m_iSize)
        this.m_iIndex = -1;
      return this.m_iIndex >= 0;
    }

    public void Reset()
    {
      if (this.m_iVersion != this.m_list.m_iVersion)
        throw new InvalidOperationException("Parent collection was changed");
      this.m_iIndex = -1;
    }
  }

  [Serializable]
  private class KeyList : IList<TKey>, ICollection<TKey>, IEnumerable<TKey>, IEnumerable
  {
    private TypedSortedListEx<TKey, TValue> m_list;

    internal KeyList(TypedSortedListEx<TKey, TValue> list) => this.m_list = list;

    public virtual int Count => this.m_list.m_iSize;

    public virtual bool IsReadOnly => true;

    public virtual bool IsFixedSize => true;

    public virtual bool IsSynchronized => this.m_list.IsSynchronized;

    public virtual object SyncRoot => this.m_list.SyncRoot;

    public void Add(TKey key) => throw new NotSupportedException();

    public virtual void Clear() => throw new NotSupportedException();

    public virtual bool Contains(TKey key) => this.m_list.ContainsKey(key);

    public virtual void CopyTo(TKey[] array, int arrayIndex)
    {
      if (array == null)
        throw new ArgumentException(nameof (array));
      Array.Copy((Array) this.m_list.m_arrKeys, 0, (Array) array, arrayIndex, this.m_list.Count);
    }

    public virtual void CopyTo(Array array, int arrayIndex)
    {
      if (array == null)
        throw new ArgumentNullException(nameof (array));
      if (array.Rank != 1)
        throw new ArgumentException(nameof (array));
      Array.Copy((Array) this.m_list.m_arrKeys, 0, array, arrayIndex, this.m_list.Count);
    }

    public virtual void Insert(int index, TKey value) => throw new NotSupportedException();

    public virtual TKey this[int index]
    {
      get => this.m_list.GetKey(index);
      set => throw new NotSupportedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) new TypedSortedListEx<TKey, TValue>.KeysEnumerator(this.m_list);
    }

    public IEnumerator<TKey> GetEnumerator()
    {
      return (IEnumerator<TKey>) new TypedSortedListEx<TKey, TValue>.KeysEnumerator(this.m_list);
    }

    public virtual int IndexOf(TKey key)
    {
      if ((object) key == null)
        throw new ArgumentNullException(nameof (key));
      int num = Array.BinarySearch<TKey>(this.m_list.m_arrKeys, 0, this.m_list.Count, key, this.m_list.m_comparer);
      return num < 0 ? -1 : num;
    }

    public bool Remove(TKey key) => throw new NotSupportedException();

    public virtual void RemoveAt(int index) => throw new NotSupportedException();
  }

  [Serializable]
  private class ValueList : IList<TValue>, ICollection<TValue>, IEnumerable<TValue>, IEnumerable
  {
    private TypedSortedListEx<TKey, TValue> m_list;
    private TValue[] vals;

    internal ValueList(TypedSortedListEx<TKey, TValue> list)
    {
      this.m_list = list;
      this.UpdateValues();
    }

    public virtual void UpdateValues()
    {
      int count = this.m_list.Count;
      this.vals = new TValue[count];
      this.m_list.m_dicValues.Values.CopyTo(this.vals, 0);
      TKey[] keyArray = new TKey[count];
      this.m_list.m_dicValues.Keys.CopyTo(keyArray, 0);
      Array.Sort<TKey, TValue>(keyArray, this.vals, this.m_list.m_comparer);
    }

    public virtual int Count => this.m_list.m_iSize;

    public virtual bool IsReadOnly => true;

    public virtual bool IsFixedSize => true;

    public virtual bool IsSynchronized => this.m_list.IsSynchronized;

    public virtual object SyncRoot => this.m_list.SyncRoot;

    public virtual void Add(TValue value) => throw new NotSupportedException();

    public virtual void Clear() => throw new NotSupportedException();

    public virtual bool Contains(TValue value) => this.m_list.ContainsValue(value);

    public virtual void CopyTo(TValue[] array, int arrayIndex)
    {
      if (array == null)
        throw new ArgumentNullException(nameof (array));
      if (array.Rank != 1)
        throw new ArgumentException("arrray");
      Array.Copy((Array) this.vals, 0, (Array) array, arrayIndex, this.m_list.Count);
    }

    public virtual void CopyTo(Array array, int arrayIndex)
    {
      if (array != null && array.Rank != 1)
        throw new ArgumentException();
      Array.Copy((Array) this.vals, 0, array, arrayIndex, this.m_list.Count);
    }

    public virtual void Insert(int index, TValue value) => throw new NotSupportedException();

    public virtual TValue this[int index]
    {
      get => this.m_list.GetByIndex(index);
      set => this.m_list.SetByIndex(index, value);
    }

    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();

    public virtual IEnumerator<TValue> GetEnumerator()
    {
      return ((IEnumerable<TValue>) this.vals).GetEnumerator();
    }

    public virtual int IndexOf(TValue value)
    {
      return Array.IndexOf<TValue>(this.vals, value, 0, this.m_list.Count);
    }

    public virtual bool Remove(TValue value) => throw new NotSupportedException();

    public virtual void RemoveAt(int index) => throw new NotSupportedException();
  }
}
