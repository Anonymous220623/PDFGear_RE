// Decompiled with JetBrains decompiler
// Type: CommomLib.AppTheme.ThemeResourceObservableDictionary
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

#nullable disable
namespace CommomLib.AppTheme;

internal class ThemeResourceObservableDictionary : 
  IDictionary,
  ICollection,
  IEnumerable,
  IDictionary<string, ResourceDictionary>,
  ICollection<KeyValuePair<string, ResourceDictionary>>,
  IEnumerable<KeyValuePair<string, ResourceDictionary>>,
  INotifyCollectionChanged,
  INotifyPropertyChanged
{
  private const string CountString = "Count";
  private const string KeysString = "Keys";
  private const string ValuesString = "Values";
  private Dictionary<string, ResourceDictionary> internalDict;

  public ThemeResourceObservableDictionary()
  {
    this.internalDict = new Dictionary<string, ResourceDictionary>();
  }

  public ICollection<string> Keys
  {
    get => ((IDictionary<string, ResourceDictionary>) this.internalDict).Keys;
  }

  ICollection IDictionary.Keys => ((IDictionary) this.internalDict).Keys;

  public ICollection<ResourceDictionary> Values
  {
    get => ((IDictionary<string, ResourceDictionary>) this.internalDict).Values;
  }

  ICollection IDictionary.Values => ((IDictionary) this.internalDict).Values;

  public int Count => this.internalDict.Count;

  public bool IsReadOnly
  {
    get => ((ICollection<KeyValuePair<string, ResourceDictionary>>) this.internalDict).IsReadOnly;
  }

  bool IDictionary.IsFixedSize => ((IDictionary) this.internalDict).IsFixedSize;

  object ICollection.SyncRoot => ((ICollection) this.internalDict).SyncRoot;

  bool ICollection.IsSynchronized => ((ICollection) this.internalDict).IsSynchronized;

  object IDictionary.this[object key]
  {
    get => ((IDictionary) this.internalDict)[key];
    set => this.SetItem((string) key, (ResourceDictionary) value);
  }

  public ResourceDictionary this[string key]
  {
    get => this.internalDict[key];
    set => this.SetItem(key, value);
  }

  public bool ContainsKey(string key) => this.internalDict.ContainsKey(key);

  bool IDictionary.Contains(object key) => ((IDictionary) this.internalDict).Contains(key);

  public bool Contains(KeyValuePair<string, ResourceDictionary> item)
  {
    return ((ICollection<KeyValuePair<string, ResourceDictionary>>) this.internalDict).Contains(item);
  }

  public void Add(KeyValuePair<string, ResourceDictionary> item) => this.Add(item.Key, item.Value);

  void IDictionary.Add(object key, object value)
  {
    this.Add((string) key, (ResourceDictionary) value);
  }

  public bool Remove(KeyValuePair<string, ResourceDictionary> item)
  {
    ResourceDictionary x;
    return this.internalDict.TryGetValue(item.Key, out x) && EqualityComparer<ResourceDictionary>.Default.Equals(x, item.Value) && this.Remove(item.Key);
  }

  void IDictionary.Remove(object key) => this.Remove((string) key);

  public bool TryGetValue(string key, out ResourceDictionary value)
  {
    return this.internalDict.TryGetValue(key, out value);
  }

  public void CopyTo(KeyValuePair<string, ResourceDictionary>[] array, int arrayIndex)
  {
    ((ICollection<KeyValuePair<string, ResourceDictionary>>) this.internalDict).CopyTo(array, arrayIndex);
  }

  public IEnumerator<KeyValuePair<string, ResourceDictionary>> GetEnumerator()
  {
    return ((IEnumerable<KeyValuePair<string, ResourceDictionary>>) this.internalDict).GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) this.internalDict).GetEnumerator();

  IDictionaryEnumerator IDictionary.GetEnumerator()
  {
    return ((IDictionary) this.internalDict).GetEnumerator();
  }

  void ICollection.CopyTo(Array array, int index)
  {
    ((ICollection) this.internalDict).CopyTo(array, index);
  }

  private int IndexOfKey(string key, out KeyValuePair<string, ResourceDictionary> value)
  {
    int num = -1;
    lock (this.internalDict)
    {
      foreach (KeyValuePair<string, ResourceDictionary> keyValuePair in this.internalDict)
      {
        ++num;
        if (object.Equals((object) key, (object) keyValuePair.Key))
        {
          value = keyValuePair;
          return num;
        }
      }
    }
    value = new KeyValuePair<string, ResourceDictionary>();
    return -1;
  }

  public void Add(string key, ResourceDictionary value)
  {
    this.internalDict.Add(key, value);
    this.OnPropertyChanged("Count");
    this.OnPropertyChanged("Keys");
    this.OnPropertyChanged("Values");
    KeyValuePair<string, ResourceDictionary> changedItem;
    int index = this.IndexOfKey(key, out changedItem);
    this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, (object) changedItem, index));
  }

  private void SetItem(string key, ResourceDictionary value)
  {
    ResourceDictionary y;
    if (!this.internalDict.TryGetValue(key, out y))
      y = (ResourceDictionary) null;
    if (EqualityComparer<ResourceDictionary>.Default.Equals(value, y))
      return;
    this.internalDict[key] = value;
    this.OnPropertyChanged("Values");
    KeyValuePair<string, ResourceDictionary> newItem;
    int index = this.IndexOfKey(key, out newItem);
    this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, (object) newItem, (object) new KeyValuePair<string, ResourceDictionary>(key, y), index));
  }

  public bool Remove(string key)
  {
    KeyValuePair<string, ResourceDictionary> changedItem;
    int index = this.IndexOfKey(key, out changedItem);
    if (!this.internalDict.TryGetValue(key, out ResourceDictionary _) || !this.internalDict.Remove(key))
      return false;
    this.OnPropertyChanged("Count");
    this.OnPropertyChanged("Keys");
    this.OnPropertyChanged("Values");
    this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, (object) changedItem, index));
    return true;
  }

  public void Clear()
  {
    EventHandler resetRequested = this.ResetRequested;
    if (resetRequested != null)
      resetRequested((object) this, EventArgs.Empty);
    this.internalDict.Clear();
    this.OnPropertyChanged("Count");
    this.OnPropertyChanged("Keys");
    this.OnPropertyChanged("Values");
    this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
  }

  public event PropertyChangedEventHandler PropertyChanged;

  public event NotifyCollectionChangedEventHandler CollectionChanged;

  internal event EventHandler ResetRequested;

  private void OnPropertyChanged(string propertyName)
  {
    PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
    if (propertyChanged == null)
      return;
    propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
  }

  private void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
  {
    NotifyCollectionChangedEventHandler collectionChanged = this.CollectionChanged;
    if (collectionChanged == null)
      return;
    collectionChanged((object) this, args);
  }
}
