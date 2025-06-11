// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.PostScriptDict
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class PostScriptDict : 
  PostScriptObj,
  IDictionary<string, object>,
  ICollection<KeyValuePair<string, object>>,
  IEnumerable<KeyValuePair<string, object>>,
  IEnumerable
{
  private readonly Dictionary<string, object> collection;

  public ICollection<string> Keys => (ICollection<string>) this.collection.Keys;

  public ICollection<object> Values => (ICollection<object>) this.collection.Values;

  public object this[string key]
  {
    get => this.collection[key];
    set => this.collection[key] = value;
  }

  public int Count => this.collection.Count;

  public bool IsReadOnly => false;

  public PostScriptDict() => this.collection = new Dictionary<string, object>();

  public PostScriptDict(int capacity) => this.collection = new Dictionary<string, object>(capacity);

  public void Add(string key, object value) => this.collection.Add(key, value);

  public bool ContainsKey(string key) => this.collection.ContainsKey(key);

  public bool Remove(string key) => this.collection.Remove(key);

  public bool TryGetValue(string key, out object value)
  {
    return this.collection.TryGetValue(key, out value);
  }

  public void Add(KeyValuePair<string, object> item) => this.collection[item.Key] = item.Value;

  public void Clear() => this.collection.Clear();

  public bool Contains(KeyValuePair<string, object> item) => throw new NotImplementedException();

  public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
  {
    throw new NotImplementedException();
  }

  public bool Remove(KeyValuePair<string, object> item) => this.collection.Remove(item.Key);

  public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
  {
    return (IEnumerator<KeyValuePair<string, object>>) this.collection.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.collection.GetEnumerator();

  public T GetElementAs<T>(string key)
  {
    if (this.collection.ContainsKey(key))
      return (T) this.collection[key];
    return key == "middot" && this.collection.ContainsKey("periodcentered") ? (T) this.collection["periodcentered"] : (T) null;
  }
}
