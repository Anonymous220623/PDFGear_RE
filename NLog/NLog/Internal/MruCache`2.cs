// Decompiled with JetBrains decompiler
// Type: NLog.Internal.MruCache`2
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System.Collections.Generic;

#nullable disable
namespace NLog.Internal;

internal class MruCache<TKey, TValue>
{
  private readonly Dictionary<TKey, MruCache<TKey, TValue>.MruCacheItem> _dictionary;
  private readonly int _maxCapacity;
  private long _currentVersion;

  public MruCache(int maxCapacity)
  {
    this._maxCapacity = maxCapacity;
    this._dictionary = new Dictionary<TKey, MruCache<TKey, TValue>.MruCacheItem>(this._maxCapacity / 4);
    this._currentVersion = 1L;
  }

  public bool TryAddValue(TKey key, TValue value)
  {
    lock (this._dictionary)
    {
      MruCache<TKey, TValue>.MruCacheItem mruCacheItem;
      if (this._dictionary.TryGetValue(key, out mruCacheItem))
      {
        long currentVersion = this._currentVersion;
        if (mruCacheItem.Version != currentVersion || !EqualityComparer<TValue>.Default.Equals(value, mruCacheItem.Value))
          this._dictionary[key] = new MruCache<TKey, TValue>.MruCacheItem(value, currentVersion, false);
        return false;
      }
      if (this._dictionary.Count >= this._maxCapacity)
      {
        ++this._currentVersion;
        this.PruneCache();
      }
      this._dictionary.Add(key, new MruCache<TKey, TValue>.MruCacheItem(value, this._currentVersion, true));
      return true;
    }
  }

  private void PruneCache()
  {
    long num1 = this._currentVersion - 2L;
    long num2 = 1;
    List<TKey> keyList = new List<TKey>((int) ((double) this._dictionary.Count / 2.5));
    for (int index = 0; index < 3; ++index)
    {
      long num3 = this._currentVersion - 5L;
      if (index != 0)
      {
        if (index == 1)
          num3 = this._currentVersion - 10L;
      }
      else
        num3 = this._currentVersion - (long) (int) ((double) this._maxCapacity / 1.5);
      if (num3 < num2)
        num3 = num2;
      num2 = long.MaxValue;
      foreach (KeyValuePair<TKey, MruCache<TKey, TValue>.MruCacheItem> keyValuePair in this._dictionary)
      {
        long version = keyValuePair.Value.Version;
        if (version <= num3 || keyValuePair.Value.Virgin && (index != 0 || version < num1))
        {
          keyList.Add(keyValuePair.Key);
          if ((double) (this._dictionary.Count - keyList.Count) < (double) this._maxCapacity / 1.5)
          {
            index = 3;
            break;
          }
        }
        else if (version < num2)
          num2 = version;
      }
    }
    foreach (TKey key in keyList)
      this._dictionary.Remove(key);
    if (this._dictionary.Count < this._maxCapacity)
      return;
    this._dictionary.Clear();
  }

  public bool TryGetValue(TKey key, out TValue value)
  {
    MruCache<TKey, TValue>.MruCacheItem mruCacheItem;
    try
    {
      if (!this._dictionary.TryGetValue(key, out mruCacheItem))
      {
        value = default (TValue);
        return false;
      }
    }
    catch
    {
      mruCacheItem = new MruCache<TKey, TValue>.MruCacheItem();
    }
    if (mruCacheItem.Version != this._currentVersion || mruCacheItem.Virgin)
    {
      lock (this._dictionary)
      {
        long version = this._currentVersion;
        if (this._dictionary.TryGetValue(key, out mruCacheItem))
        {
          if (mruCacheItem.Version == version)
          {
            if (!mruCacheItem.Virgin)
              goto label_15;
          }
          if (mruCacheItem.Virgin)
            version = ++this._currentVersion;
          this._dictionary[key] = new MruCache<TKey, TValue>.MruCacheItem(mruCacheItem.Value, version, false);
        }
        else
        {
          value = default (TValue);
          return false;
        }
      }
    }
label_15:
    value = mruCacheItem.Value;
    return true;
  }

  private struct MruCacheItem(TValue value, long version, bool virgin)
  {
    public readonly TValue Value = value;
    public readonly long Version = version;
    public readonly bool Virgin = virgin;
  }
}
