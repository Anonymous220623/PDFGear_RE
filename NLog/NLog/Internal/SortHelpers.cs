// Decompiled with JetBrains decompiler
// Type: NLog.Internal.SortHelpers
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NLog.Internal;

internal static class SortHelpers
{
  public static Dictionary<TKey, List<TValue>> BucketSort<TValue, TKey>(
    this IEnumerable<TValue> inputs,
    SortHelpers.KeySelector<TValue, TKey> keySelector)
  {
    Dictionary<TKey, List<TValue>> dictionary = new Dictionary<TKey, List<TValue>>();
    foreach (TValue input in inputs)
    {
      TKey key = keySelector(input);
      List<TValue> objList;
      if (!dictionary.TryGetValue(key, out objList))
      {
        objList = new List<TValue>();
        dictionary.Add(key, objList);
      }
      objList.Add(input);
    }
    return dictionary;
  }

  public static SortHelpers.ReadOnlySingleBucketDictionary<TKey, IList<TValue>> BucketSort<TValue, TKey>(
    this IList<TValue> inputs,
    SortHelpers.KeySelector<TValue, TKey> keySelector)
  {
    return inputs.BucketSort<TValue, TKey>(keySelector, (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
  }

  public static SortHelpers.ReadOnlySingleBucketDictionary<TKey, IList<TValue>> BucketSort<TValue, TKey>(
    this IList<TValue> inputs,
    SortHelpers.KeySelector<TValue, TKey> keySelector,
    IEqualityComparer<TKey> keyComparer)
  {
    Dictionary<TKey, IList<TValue>> multiBucket = (Dictionary<TKey, IList<TValue>>) null;
    bool flag = false;
    TKey key1 = default (TKey);
    for (int index = 0; index < inputs.Count; ++index)
    {
      TKey key2 = keySelector(inputs[index]);
      if (!flag)
      {
        flag = true;
        key1 = key2;
      }
      else if (multiBucket == null)
      {
        if (!keyComparer.Equals(key1, key2))
          multiBucket = SortHelpers.CreateBucketDictionaryWithValue<TValue, TKey>(inputs, keyComparer, index, key1, key2);
      }
      else
      {
        IList<TValue> objList;
        if (!multiBucket.TryGetValue(key2, out objList))
        {
          objList = (IList<TValue>) new List<TValue>();
          multiBucket.Add(key2, objList);
        }
        objList.Add(inputs[index]);
      }
    }
    return multiBucket != null ? new SortHelpers.ReadOnlySingleBucketDictionary<TKey, IList<TValue>>(multiBucket, keyComparer) : new SortHelpers.ReadOnlySingleBucketDictionary<TKey, IList<TValue>>(new KeyValuePair<TKey, IList<TValue>>(key1, inputs), keyComparer);
  }

  private static Dictionary<TKey, IList<TValue>> CreateBucketDictionaryWithValue<TValue, TKey>(
    IList<TValue> inputs,
    IEqualityComparer<TKey> keyComparer,
    int currentIndex,
    TKey singleBucketKey,
    TKey keyValue)
  {
    Dictionary<TKey, IList<TValue>> dictionaryWithValue = new Dictionary<TKey, IList<TValue>>(keyComparer);
    List<TValue> objList1 = new List<TValue>(currentIndex);
    for (int index = 0; index < currentIndex; ++index)
      objList1.Add(inputs[index]);
    dictionaryWithValue[singleBucketKey] = (IList<TValue>) objList1;
    List<TValue> objList2 = new List<TValue>()
    {
      inputs[currentIndex]
    };
    dictionaryWithValue[keyValue] = (IList<TValue>) objList2;
    return dictionaryWithValue;
  }

  internal delegate TKey KeySelector<in TValue, out TKey>(TValue value);

  public struct ReadOnlySingleBucketDictionary<TKey, TValue> : 
    IDictionary<TKey, TValue>,
    ICollection<KeyValuePair<TKey, TValue>>,
    IEnumerable<KeyValuePair<TKey, TValue>>,
    IEnumerable
  {
    private KeyValuePair<TKey, TValue>? _singleBucket;
    private readonly Dictionary<TKey, TValue> _multiBucket;
    private readonly IEqualityComparer<TKey> _comparer;

    public IEqualityComparer<TKey> Comparer => this._comparer;

    public ReadOnlySingleBucketDictionary(KeyValuePair<TKey, TValue> singleBucket)
      : this(singleBucket, (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default)
    {
    }

    public ReadOnlySingleBucketDictionary(Dictionary<TKey, TValue> multiBucket)
      : this(multiBucket, (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default)
    {
    }

    public ReadOnlySingleBucketDictionary(
      KeyValuePair<TKey, TValue> singleBucket,
      IEqualityComparer<TKey> comparer)
    {
      this._comparer = comparer;
      this._multiBucket = (Dictionary<TKey, TValue>) null;
      this._singleBucket = new KeyValuePair<TKey, TValue>?(singleBucket);
    }

    public ReadOnlySingleBucketDictionary(
      Dictionary<TKey, TValue> multiBucket,
      IEqualityComparer<TKey> comparer)
    {
      this._comparer = comparer;
      this._multiBucket = multiBucket;
      this._singleBucket = new KeyValuePair<TKey, TValue>?(new KeyValuePair<TKey, TValue>());
    }

    public int Count
    {
      get
      {
        if (this._multiBucket != null)
          return this._multiBucket.Count;
        return this._singleBucket.HasValue ? 1 : 0;
      }
    }

    public ICollection<TKey> Keys
    {
      get
      {
        if (this._multiBucket != null)
          return (ICollection<TKey>) this._multiBucket.Keys;
        if (!this._singleBucket.HasValue)
          return (ICollection<TKey>) ArrayHelper.Empty<TKey>();
        return (ICollection<TKey>) new TKey[1]
        {
          this._singleBucket.Value.Key
        };
      }
    }

    public ICollection<TValue> Values
    {
      get
      {
        if (this._multiBucket != null)
          return (ICollection<TValue>) this._multiBucket.Values;
        if (!this._singleBucket.HasValue)
          return (ICollection<TValue>) ArrayHelper.Empty<TValue>();
        return (ICollection<TValue>) new TValue[1]
        {
          this._singleBucket.Value.Value
        };
      }
    }

    public bool IsReadOnly => true;

    public TValue this[TKey key]
    {
      get
      {
        if (this._multiBucket != null)
          return this._multiBucket[key];
        if (this._singleBucket.HasValue && this._comparer.Equals(this._singleBucket.Value.Key, key))
          return this._singleBucket.Value.Value;
        throw new KeyNotFoundException();
      }
      set => throw new NotSupportedException("Readonly");
    }

    public SortHelpers.ReadOnlySingleBucketDictionary<TKey, TValue>.Enumerator GetEnumerator()
    {
      if (this._multiBucket != null)
        return new SortHelpers.ReadOnlySingleBucketDictionary<TKey, TValue>.Enumerator(this._multiBucket);
      return this._singleBucket.HasValue ? new SortHelpers.ReadOnlySingleBucketDictionary<TKey, TValue>.Enumerator(this._singleBucket.Value) : new SortHelpers.ReadOnlySingleBucketDictionary<TKey, TValue>.Enumerator(new Dictionary<TKey, TValue>());
    }

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
    {
      return (IEnumerator<KeyValuePair<TKey, TValue>>) this.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public bool ContainsKey(TKey key)
    {
      if (this._multiBucket != null)
        return this._multiBucket.ContainsKey(key);
      return this._singleBucket.HasValue && this._comparer.Equals(this._singleBucket.Value.Key, key);
    }

    public void Add(TKey key, TValue value) => throw new NotSupportedException();

    public bool Remove(TKey key) => throw new NotSupportedException();

    public bool TryGetValue(TKey key, out TValue value)
    {
      if (this._multiBucket != null)
        return this._multiBucket.TryGetValue(key, out value);
      if (this._singleBucket.HasValue && this._comparer.Equals(this._singleBucket.Value.Key, key))
      {
        value = this._singleBucket.Value.Value;
        return true;
      }
      value = default (TValue);
      return false;
    }

    public void Add(KeyValuePair<TKey, TValue> item) => throw new NotSupportedException();

    public void Clear() => throw new NotSupportedException();

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
      if (this._multiBucket != null)
        return ((ICollection<KeyValuePair<TKey, TValue>>) this._multiBucket).Contains(item);
      return this._singleBucket.HasValue && this._comparer.Equals(this._singleBucket.Value.Key, item.Key) && EqualityComparer<TValue>.Default.Equals(this._singleBucket.Value.Value, item.Value);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
      if (this._multiBucket != null)
      {
        ((ICollection<KeyValuePair<TKey, TValue>>) this._multiBucket).CopyTo(array, arrayIndex);
      }
      else
      {
        if (!this._singleBucket.HasValue)
          return;
        array[arrayIndex] = this._singleBucket.Value;
      }
    }

    public bool Remove(KeyValuePair<TKey, TValue> item) => throw new NotSupportedException();

    public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDisposable, IEnumerator
    {
      private bool _singleBucketFirstRead;
      private KeyValuePair<TKey, TValue> _singleBucket;
      private readonly IEnumerator<KeyValuePair<TKey, TValue>> _multiBuckets;

      internal Enumerator(Dictionary<TKey, TValue> multiBucket)
      {
        this._singleBucketFirstRead = false;
        this._singleBucket = new KeyValuePair<TKey, TValue>();
        this._multiBuckets = (IEnumerator<KeyValuePair<TKey, TValue>>) multiBucket.GetEnumerator();
      }

      internal Enumerator(KeyValuePair<TKey, TValue> singleBucket)
      {
        this._singleBucketFirstRead = false;
        this._singleBucket = singleBucket;
        this._multiBuckets = (IEnumerator<KeyValuePair<TKey, TValue>>) null;
      }

      public KeyValuePair<TKey, TValue> Current
      {
        get
        {
          return this._multiBuckets != null ? new KeyValuePair<TKey, TValue>(this._multiBuckets.Current.Key, this._multiBuckets.Current.Value) : new KeyValuePair<TKey, TValue>(this._singleBucket.Key, this._singleBucket.Value);
        }
      }

      object IEnumerator.Current => (object) this.Current;

      public void Dispose()
      {
        if (this._multiBuckets == null)
          return;
        this._multiBuckets.Dispose();
      }

      public bool MoveNext()
      {
        if (this._multiBuckets != null)
          return this._multiBuckets.MoveNext();
        return !this._singleBucketFirstRead && (this._singleBucketFirstRead = true);
      }

      public void Reset()
      {
        if (this._multiBuckets != null)
          this._multiBuckets.Reset();
        else
          this._singleBucketFirstRead = false;
      }
    }
  }
}
