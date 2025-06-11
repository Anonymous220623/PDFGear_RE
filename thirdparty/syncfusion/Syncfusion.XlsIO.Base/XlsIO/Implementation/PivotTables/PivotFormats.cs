// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotFormats
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

internal class PivotFormats
{
  private PivotTableImpl m_parent;
  private List<PivotFormat> m_pivotFormats;

  internal PivotTableImpl Parent => this.m_parent;

  internal PivotFormat this[int index]
  {
    get
    {
      if (index < 0 || index >= this.m_pivotFormats.Count)
        throw new ArgumentOutOfRangeException($"index is {index}, Count is {this.m_pivotFormats.Count}");
      return this.m_pivotFormats[index];
    }
  }

  internal int Count => this.m_pivotFormats.Count;

  internal List<PivotFormat> Formats => this.m_pivotFormats;

  public PivotFormats(PivotTableImpl parent)
  {
    this.m_parent = parent;
    this.m_pivotFormats = new List<PivotFormat>();
  }

  internal void Add(PivotFormat pivotFormat) => this.m_pivotFormats.Add(pivotFormat);

  internal void AddRange(List<PivotFormat> pivotFormats)
  {
    this.m_pivotFormats.AddRange((IEnumerable<PivotFormat>) pivotFormats);
  }

  internal bool Contains(PivotFormat pivotFormat)
  {
    bool flag = false;
    foreach (PivotFormat pivotFormat1 in this.m_pivotFormats)
    {
      flag = pivotFormat.PivotArea.Equals((object) pivotFormat1.PivotArea) && pivotFormat.PivotArea.References.Equals((object) pivotFormat1.PivotArea.References);
      if (flag)
        break;
    }
    return flag;
  }

  internal int IndexOf(PivotFormat pivotFormat)
  {
    for (int index = 0; index < this.m_pivotFormats.Count; ++index)
    {
      PivotFormat pivotFormat1 = this.m_pivotFormats[index];
      if (pivotFormat.PivotArea.Equals((object) pivotFormat1.PivotArea) && pivotFormat.PivotArea.References.Equals((object) pivotFormat1.PivotArea.References))
        return index;
    }
    return -1;
  }

  internal object Clone(PivotTableImpl parent)
  {
    PivotFormats parent1 = new PivotFormats(parent);
    for (int index = 0; index < this.Formats.Count; ++index)
    {
      PivotFormat pivotFormat = (PivotFormat) this[index].Clone(parent1);
      parent1.Add(pivotFormat);
    }
    return (object) parent1;
  }
}
