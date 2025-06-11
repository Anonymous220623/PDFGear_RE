// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.PivotAreaReferences
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.PivotTables;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

internal class PivotAreaReferences
{
  private List<PivotAreaReference> m_pivotAreaReferences;

  internal List<PivotAreaReference> AreaReferences => this.m_pivotAreaReferences;

  internal int Count => this.m_pivotAreaReferences.Count;

  internal PivotAreaReference this[int index] => this.m_pivotAreaReferences[index];

  public PivotAreaReferences() => this.m_pivotAreaReferences = new List<PivotAreaReference>();

  internal void AddReference(PivotAreaReference reference)
  {
    bool flag = false;
    for (int index1 = 0; index1 < this.m_pivotAreaReferences.Count; ++index1)
    {
      if (reference.FieldIndex == this.m_pivotAreaReferences[index1].FieldIndex)
      {
        foreach (int index2 in reference.Indexes)
        {
          if (!this.m_pivotAreaReferences[index1].Indexes.Contains(index2))
            this.m_pivotAreaReferences[index1].Indexes.Add(index2);
        }
        flag = true;
        break;
      }
    }
    if (flag)
      return;
    this.Add(reference);
  }

  internal void Add(PivotAreaReference reference) => this.m_pivotAreaReferences.Add(reference);

  internal void SetReferences(List<PivotAreaReference> references)
  {
    this.m_pivotAreaReferences = references;
  }

  internal new bool Equals(object obj)
  {
    if (!(obj is PivotAreaReferences pivotAreaReferences))
      return false;
    bool flag1 = this.m_pivotAreaReferences.Count == pivotAreaReferences.AreaReferences.Count;
    if (this.m_pivotAreaReferences.Count == 0 || !flag1)
      return flag1;
    bool flag2 = false;
    for (int index1 = 0; index1 < pivotAreaReferences.AreaReferences.Count; ++index1)
    {
      flag2 = false;
      PivotAreaReference areaReference = pivotAreaReferences.AreaReferences[index1];
      for (int index2 = 0; index2 < this.m_pivotAreaReferences.Count; ++index2)
      {
        flag2 = areaReference.Equals((object) this.m_pivotAreaReferences[index2]);
        if (flag2)
          break;
      }
      if (!flag2)
        break;
    }
    return flag2;
  }
}
