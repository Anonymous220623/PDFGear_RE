// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotCustomFilters
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

internal class PivotCustomFilters
{
  private bool m_bHasAnd;
  private List<PivotCustomFilter> m_pivotCustomFilter = new List<PivotCustomFilter>();

  public bool HasAnd
  {
    get => this.m_bHasAnd;
    set => this.m_bHasAnd = value;
  }

  public PivotCustomFilter this[int index]
  {
    get
    {
      if (this.m_pivotCustomFilter.Count > 0 && index < this.m_pivotCustomFilter.Count)
        return this.m_pivotCustomFilter[index];
      throw new ArgumentOutOfRangeException("Index");
    }
  }

  public int Count => this.m_pivotCustomFilter.Count;

  public void Add(PivotCustomFilter customFilter) => this.m_pivotCustomFilter.Add(customFilter);
}
