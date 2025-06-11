// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotFieldItemsCollections
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

internal class PivotFieldItemsCollections : IPivotFieldItems
{
  private List<PivotFieldItem> m_pivotFilterItem = new List<PivotFieldItem>();

  public IPivotFieldItem this[int index]
  {
    get
    {
      if (this.m_pivotFilterItem.Count > 0 && index < this.m_pivotFilterItem.Count)
        return (IPivotFieldItem) this.m_pivotFilterItem[index];
      throw new ArgumentOutOfRangeException("Index");
    }
  }

  public int Count => this.m_pivotFilterItem.Count;

  public IPivotFieldItem this[string FilterText]
  {
    get
    {
      if (this.m_pivotFilterItem.Count > 0)
      {
        foreach (PivotFieldItem pivotFieldItem in this.m_pivotFilterItem)
        {
          if (pivotFieldItem.Text != null && pivotFieldItem.Text.Equals(FilterText))
            return (IPivotFieldItem) pivotFieldItem;
        }
      }
      return (IPivotFieldItem) null;
    }
  }

  public void Add(object Parent, string ItemValue, string text)
  {
    this.m_pivotFilterItem.Add(new PivotFieldItem()
    {
      Parent = Parent as PivotFieldImpl,
      Name = ItemValue,
      Text = text
    });
  }

  internal void Add(object Parent, string ItemValue, PivotItemOptions itemOption)
  {
    this.m_pivotFilterItem.Add(new PivotFieldItem()
    {
      Parent = Parent as PivotFieldImpl,
      ItemOptions = itemOption,
      Name = ItemValue
    });
  }

  internal int GetPosition(PivotFieldItem item)
  {
    return this.m_pivotFilterItem.IndexOf(this[item.Name] as PivotFieldItem);
  }

  internal void SetPosition(PivotFieldItem item, int index)
  {
    this.m_pivotFilterItem.Remove(this[item.Name] as PivotFieldItem);
    this.m_pivotFilterItem.Insert(index, item);
  }

  internal void Clear() => this.m_pivotFilterItem.Clear();
}
