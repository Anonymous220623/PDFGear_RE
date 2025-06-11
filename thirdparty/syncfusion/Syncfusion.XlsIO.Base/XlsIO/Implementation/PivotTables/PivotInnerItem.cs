// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotInnerItem
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

internal class PivotInnerItem
{
  private object m_parent;
  private string m_name;
  private int m_fieldIndex;
  private int m_valueIndex;
  private List<PivotInnerItem> m_items;
  private bool m_bSubtotal;

  internal object Parent => this.m_parent;

  internal string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  internal int FieldIndex
  {
    get => this.m_fieldIndex;
    set => this.m_fieldIndex = value;
  }

  internal int ValueIndex
  {
    get => this.m_valueIndex;
    set => this.m_valueIndex = value;
  }

  internal List<PivotInnerItem> Items
  {
    get => this.m_items;
    set => this.m_items = value;
  }

  internal bool IsSubtotal
  {
    get => this.m_bSubtotal;
    set => this.m_bSubtotal = value;
  }

  internal PivotInnerItem(string name, object parent)
  {
    this.m_name = name;
    this.m_parent = parent;
  }

  internal static int GetIndex(List<PivotInnerItem> pivotInnerItems, string name)
  {
    for (int index = 0; index < pivotInnerItems.Count; ++index)
    {
      if (pivotInnerItems[index].Name == name)
        return index;
    }
    return -1;
  }

  internal object Clone(object parent)
  {
    PivotInnerItem parent1 = (PivotInnerItem) this.MemberwiseClone();
    parent1.m_parent = parent;
    if (this.m_items != null)
    {
      for (int index = 0; index < this.m_items.Count; ++index)
        parent1.m_items.Add(this.m_items[index].Clone((object) parent1) as PivotInnerItem);
    }
    return (object) parent1;
  }
}
