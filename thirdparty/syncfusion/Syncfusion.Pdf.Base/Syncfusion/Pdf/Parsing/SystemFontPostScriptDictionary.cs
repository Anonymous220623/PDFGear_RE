// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontPostScriptDictionary
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontPostScriptDictionary : 
  SystemFontPostScriptObject,
  IDictionary<string, object>,
  ICollection<KeyValuePair<string, object>>,
  IEnumerable<KeyValuePair<string, object>>,
  IEnumerable
{
  private readonly Dictionary<string, object> store;

  public ICollection<string> Keys => (ICollection<string>) this.store.Keys;

  public ICollection<object> Values => (ICollection<object>) this.store.Values;

  public object this[string key]
  {
    get => this.store[key];
    set => this.store[key] = value;
  }

  public int Count => this.store.Count;

  public bool IsReadOnly => false;

  public SystemFontPostScriptDictionary() => this.store = new Dictionary<string, object>();

  public SystemFontPostScriptDictionary(int capacity)
  {
    this.store = new Dictionary<string, object>(capacity);
  }

  public void Add(string key, object value) => this.store.Add(key, value);

  public bool ContainsKey(string key) => this.store.ContainsKey(key);

  public bool Remove(string key) => this.store.Remove(key);

  public bool TryGetValue(string key, out object value) => this.store.TryGetValue(key, out value);

  public void Add(KeyValuePair<string, object> item) => this.store[item.Key] = item.Value;

  public void Clear() => this.store.Clear();

  public bool Contains(KeyValuePair<string, object> item) => throw new NotImplementedException();

  public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
  {
    throw new NotImplementedException();
  }

  public bool Remove(KeyValuePair<string, object> item) => this.store.Remove(item.Key);

  public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
  {
    return (IEnumerator<KeyValuePair<string, object>>) this.store.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.store.GetEnumerator();

  public T GetElementAs<T>(string key) => (T) this.store[key];
}
