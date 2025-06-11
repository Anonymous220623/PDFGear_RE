// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.StylesDictionary
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
public class StylesDictionary : Hashtable, INotifyCollectionChanged
{
  public override void Add(object key, object value)
  {
    if (!(key is string s))
      throw new NotSupportedException("The key must be valid a string representation of DateTime value.");
    if (!(value is StyleItem styleItem))
      throw new NotSupportedException("Collection is used for StyleItem items only.");
    DateTime result;
    if (!DateTime.TryParse(s, out result))
      throw new ArgumentException(" The key must be valid a string representation of DateTime value.");
    if (styleItem.Style == null)
      throw new ArgumentException("Style value must be of Style type only.");
    styleItem.Date = result;
    base.Add((object) result, (object) styleItem);
    this.NotifyCollectionChanged(NotifyCollectionChangedAction.Add, styleItem);
  }

  public override void Remove(object key)
  {
    StyleItem styleItem = (StyleItem) base[key];
    base.Remove(key);
    this.NotifyCollectionChanged(NotifyCollectionChangedAction.Remove, styleItem);
  }

  public override void Clear()
  {
    base.Clear();
    this.NotifyCollectionChanged(NotifyCollectionChangedAction.Reset, (StyleItem) null);
  }

  public override object this[object key]
  {
    get => this.ContainsKey(key) ? (object) ((StyleItem) base[key]).Style : (object) null;
    set
    {
      if (!(value is Style style))
        return;
      ((StyleItem) base[key]).Style = style;
      this.NotifyCollectionChanged(NotifyCollectionChangedAction.Add, (StyleItem) base[key]);
    }
  }

  public event NotifyCollectionChangedEventHandler CollectionChanged;

  private void NotifyCollectionChanged(NotifyCollectionChangedAction action, StyleItem item)
  {
    if (this.CollectionChanged == null)
      return;
    this.CollectionChanged((object) this, new NotifyCollectionChangedEventArgs(action, (object) item));
  }
}
