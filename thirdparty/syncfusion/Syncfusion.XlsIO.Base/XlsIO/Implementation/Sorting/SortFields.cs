// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Sorting.SortFields
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using System;
using System.Collections;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Sorting;

internal class SortFields : CollectionBaseEx<ISortField>, ISortFields, IEnumerable
{
  public SortFields(IApplication application, object parent)
    : base(application, parent)
  {
  }

  internal SortFields(IApplication application, IWorksheet parent)
    : base(application, (object) parent)
  {
  }

  public ISortField Add(int key, SortOn sortBasedOn, OrderBy orderBy)
  {
    SortField sortField = new SortField(this);
    sortField.Key = key;
    sortField.SortOn = sortBasedOn;
    sortField.Order = orderBy;
    if ((sortBasedOn == SortOn.Values ? (this.FindByKey(key) == -1 ? 1 : 0) : 1) != 0)
      this.Add((ISortField) sortField);
    return (ISortField) sortField;
  }

  public void Remove(ISortField sortField) => this.Remove(sortField.Key);

  public void Remove(int key)
  {
    int byKey = this.FindByKey(key);
    if (byKey == -1)
      throw new ArgumentOutOfRangeException("Key Not found");
    this.RemoveAt(byKey);
  }

  internal void RemoveLast(int key)
  {
    int lastByKey = this.FindLastByKey(key);
    if (lastByKey == -1)
      throw new ArgumentOutOfRangeException("Key Not found");
    this.RemoveAt(lastByKey);
  }

  internal void SetPriority(SortField sortField, int priority)
  {
    int byKey = this.FindByKey(sortField.Key);
    if (byKey != -1)
      this.RemoveAt(byKey);
    this.Insert(priority, (ISortField) sortField);
  }

  internal int FindByKey(int key)
  {
    int byKey = 0;
    foreach (ISortField sortField in (CollectionBase<ISortField>) this)
    {
      if (sortField.Key == key)
        return byKey;
      ++byKey;
    }
    return -1;
  }

  internal int FindLastByKey(int key)
  {
    for (int i = this.Count - 1; i >= 0; --i)
    {
      if (this[i].Key == key)
        return i;
    }
    return -1;
  }
}
