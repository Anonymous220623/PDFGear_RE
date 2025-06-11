// Decompiled with JetBrains decompiler
// Type: NLog.Internal.DictionaryAdapter`2
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NLog.Internal;

internal class DictionaryAdapter<TKey, TValue> : IDictionary, ICollection, IEnumerable
{
  private readonly IDictionary<TKey, TValue> _implementation;

  public DictionaryAdapter(IDictionary<TKey, TValue> implementation)
  {
    this._implementation = implementation;
  }

  public ICollection Values
  {
    get => (ICollection) new List<TValue>((IEnumerable<TValue>) this._implementation.Values);
  }

  public int Count => this._implementation.Count;

  public bool IsSynchronized => false;

  public object SyncRoot => (object) this._implementation;

  public bool IsFixedSize => false;

  public bool IsReadOnly => this._implementation.IsReadOnly;

  public ICollection Keys
  {
    get => (ICollection) new List<TKey>((IEnumerable<TKey>) this._implementation.Keys);
  }

  public object this[object key]
  {
    get
    {
      TValue obj;
      return this._implementation.TryGetValue((TKey) key, out obj) ? (object) obj : (object) null;
    }
    set => this._implementation[(TKey) key] = (TValue) value;
  }

  public void Add(object key, object value) => this._implementation.Add((TKey) key, (TValue) value);

  public void Clear() => this._implementation.Clear();

  public bool Contains(object key) => this._implementation.ContainsKey((TKey) key);

  public System.Collections.IDictionaryEnumerator GetEnumerator()
  {
    return (System.Collections.IDictionaryEnumerator) new DictionaryAdapter<TKey, TValue>.MyEnumerator(this._implementation.GetEnumerator());
  }

  public void Remove(object key) => this._implementation.Remove((TKey) key);

  public void CopyTo(Array array, int index) => throw new NotImplementedException();

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  private class MyEnumerator : System.Collections.IDictionaryEnumerator, IEnumerator
  {
    private readonly IEnumerator<KeyValuePair<TKey, TValue>> _wrapped;

    public MyEnumerator(IEnumerator<KeyValuePair<TKey, TValue>> wrapped) => this._wrapped = wrapped;

    public DictionaryEntry Entry
    {
      get
      {
        KeyValuePair<TKey, TValue> current = this._wrapped.Current;
        __Boxed<TKey> key = (object) current.Key;
        current = this._wrapped.Current;
        __Boxed<TValue> local = (object) current.Value;
        return new DictionaryEntry((object) key, (object) local);
      }
    }

    public object Key => (object) this._wrapped.Current.Key;

    public object Value => (object) this._wrapped.Current.Value;

    public object Current => (object) this.Entry;

    public bool MoveNext() => this._wrapped.MoveNext();

    public void Reset() => this._wrapped.Reset();
  }
}
