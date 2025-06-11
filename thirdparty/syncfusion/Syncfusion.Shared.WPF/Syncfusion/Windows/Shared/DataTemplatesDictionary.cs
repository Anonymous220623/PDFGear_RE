// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.DataTemplatesDictionary
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class DataTemplatesDictionary : Hashtable, INotifyCollectionChanged
{
  public override void Add(object key, object value)
  {
    if (!(key is string s))
      throw new NotSupportedException("The key must be valid a string representation of DateTime value.");
    if (!(value is DataTemplateItem dataTemplateItem))
      throw new NotSupportedException("Collection is used for DataTemplateItem items only.");
    DateTime result;
    if (!DateTime.TryParse(s, out result))
      throw new ArgumentException("The key must be valid a string representation of DateTime value.");
    if (dataTemplateItem.Template == null)
      throw new ArgumentException("Template value must be of DataTemplate type only.");
    dataTemplateItem.Date = result;
    base.Add((object) result, (object) dataTemplateItem);
    this.NotifyCollectionChanged(NotifyCollectionChangedAction.Add, dataTemplateItem);
  }

  public override void Remove(object key)
  {
    DataTemplateItem dataTemplateItem = (DataTemplateItem) base[key];
    base.Remove(key);
    this.NotifyCollectionChanged(NotifyCollectionChangedAction.Remove, dataTemplateItem);
  }

  public override void Clear()
  {
    base.Clear();
    this.NotifyCollectionChanged(NotifyCollectionChangedAction.Reset, (DataTemplateItem) null);
  }

  public override object this[object key]
  {
    get => this.ContainsKey(key) ? (object) ((DataTemplateItem) base[key]).Template : (object) null;
    set
    {
      if (!(value is DataTemplate dataTemplate))
        return;
      ((DataTemplateItem) base[key]).Template = dataTemplate;
      this.NotifyCollectionChanged(NotifyCollectionChangedAction.Add, (DataTemplateItem) base[key]);
    }
  }

  public event NotifyCollectionChangedEventHandler CollectionChanged;

  private void NotifyCollectionChanged(NotifyCollectionChangedAction action, DataTemplateItem item)
  {
    if (this.CollectionChanged == null)
      return;
    this.CollectionChanged((object) this, new NotifyCollectionChangedEventArgs(action, (object) item));
  }
}
