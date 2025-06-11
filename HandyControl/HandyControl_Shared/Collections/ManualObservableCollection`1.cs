// Decompiled with JetBrains decompiler
// Type: HandyControl.Collections.ManualObservableCollection`1
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

#nullable disable
namespace HandyControl.Collections;

[Serializable]
public class ManualObservableCollection<T> : ObservableCollection<T>
{
  private const string CountString = "Count";
  private const string IndexerName = "Item[]";
  private int _oldCount;
  private bool _canNotify = true;

  public bool CanNotify
  {
    get => this._canNotify;
    set
    {
      this._canNotify = value;
      if (value)
      {
        if (this._oldCount != this.Count)
          this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
        this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
        this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
      }
      else
        this._oldCount = this.Count;
    }
  }

  public ManualObservableCollection()
  {
  }

  public ManualObservableCollection(List<T> list)
    : base(list != null ? new List<T>(list.Count) : list)
  {
    this.CopyFrom((IEnumerable<T>) list);
  }

  public ManualObservableCollection(IEnumerable<T> collection)
  {
    if (collection == null)
      throw new ArgumentNullException(nameof (collection));
    this.CopyFrom(collection);
  }

  protected override void OnPropertyChanged(PropertyChangedEventArgs e)
  {
    if (!this.CanNotify)
      return;
    base.OnPropertyChanged(e);
  }

  protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
  {
    if (!this.CanNotify)
      return;
    base.OnCollectionChanged(e);
  }

  private void CopyFrom(IEnumerable<T> collection)
  {
    IList<T> items = this.Items;
    if (collection == null)
      return;
    foreach (T obj in collection)
      items.Add(obj);
  }
}
