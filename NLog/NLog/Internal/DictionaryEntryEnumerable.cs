// Decompiled with JetBrains decompiler
// Type: NLog.Internal.DictionaryEntryEnumerable
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NLog.Internal;

internal struct DictionaryEntryEnumerable(IDictionary dictionary) : 
  IEnumerable<DictionaryEntry>,
  IEnumerable
{
  private readonly IDictionary _dictionary = dictionary;

  public DictionaryEntryEnumerable.DictionaryEntryEnumerator GetEnumerator()
  {
    IDictionary dictionary = this._dictionary;
    return new DictionaryEntryEnumerable.DictionaryEntryEnumerator((dictionary != null ? (dictionary.Count > 0 ? 1 : 0) : 0) != 0 ? this._dictionary : (IDictionary) null);
  }

  IEnumerator<DictionaryEntry> IEnumerable<DictionaryEntry>.GetEnumerator()
  {
    return (IEnumerator<DictionaryEntry>) this.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  internal struct DictionaryEntryEnumerator : IEnumerator<DictionaryEntry>, IDisposable, IEnumerator
  {
    private readonly System.Collections.IDictionaryEnumerator _entryEnumerator;

    public DictionaryEntry Current => this._entryEnumerator.Entry;

    public DictionaryEntryEnumerator(IDictionary dictionary)
    {
      this._entryEnumerator = dictionary?.GetEnumerator();
    }

    object IEnumerator.Current => (object) this.Current;

    public void Dispose()
    {
      if (!(this._entryEnumerator is IDisposable entryEnumerator))
        return;
      entryEnumerator.Dispose();
    }

    public bool MoveNext()
    {
      System.Collections.IDictionaryEnumerator entryEnumerator = this._entryEnumerator;
      return entryEnumerator != null && entryEnumerator.MoveNext();
    }

    public void Reset() => this._entryEnumerator?.Reset();
  }
}
