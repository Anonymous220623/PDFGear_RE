// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotAutoFilter
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

internal class PivotAutoFilter
{
  private string str_filterRange;
  private List<PivotFilterColumn> m_pivotFilterColumn = new List<PivotFilterColumn>();

  public string FilterRange
  {
    get => this.str_filterRange;
    set => this.str_filterRange = value;
  }

  public PivotFilterColumn this[int index]
  {
    get
    {
      if (this.m_pivotFilterColumn.Count > 0 && index < this.m_pivotFilterColumn.Count)
        return this.m_pivotFilterColumn[index];
      throw new ArgumentOutOfRangeException("Index");
    }
  }

  public int Count => this.m_pivotFilterColumn.Count;

  public void Add(PivotFilterColumn filterColumn) => this.m_pivotFilterColumn.Add(filterColumn);
}
