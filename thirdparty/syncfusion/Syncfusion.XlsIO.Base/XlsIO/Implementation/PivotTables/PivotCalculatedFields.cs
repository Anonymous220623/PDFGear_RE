// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotCalculatedFields
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

public class PivotCalculatedFields : List<PivotFieldImpl>, IPivotCalculatedFields
{
  private PivotTableImpl m_pivotTable;

  public PivotCalculatedFields(PivotTableImpl pivotTable) => this.m_pivotTable = pivotTable;

  IPivotField IPivotCalculatedFields.Add(string name, string formula)
  {
    PivotCacheFieldImpl cacheField = this.m_pivotTable.Cache.CacheFields.AddNewField(name, formula);
    PivotFieldImpl field = new PivotFieldImpl(cacheField, this.m_pivotTable);
    this.m_pivotTable.AddPivotField(PivotAxisTypes.None, field, true);
    string str = "Sum of " + (object) this.m_pivotTable.DataFields.Count;
    field.CanDragToColumn = false;
    field.CanDragToPage = false;
    field.CanDragToRow = false;
    field.Axis = PivotAxisTypes.None;
    this.m_pivotTable.SetChanged(true);
    this.UpdatePivotFields(this.m_pivotTable, cacheField, field);
    return (IPivotField) field;
  }

  private void UpdatePivotFields(
    PivotTableImpl table,
    PivotCacheFieldImpl cacheField,
    PivotFieldImpl field)
  {
    int count1 = table.Workbook.Worksheets.Count;
    for (int Index = 0; Index < count1; ++Index)
    {
      int count2 = this.m_pivotTable.Workbook.Worksheets[Index].PivotTables.Count;
      if (count2 > 0)
      {
        for (int index = 0; index < count2; ++index)
        {
          PivotTableImpl pivotTable = this.m_pivotTable.Workbook.Worksheets[Index].PivotTables[index] as PivotTableImpl;
          if (pivotTable.CacheIndex == this.m_pivotTable.CacheIndex && !pivotTable.Fields.Contains(field))
            pivotTable.AddPivotField(PivotAxisTypes.None, new PivotFieldImpl(cacheField, pivotTable)
            {
              IsDataField = false
            }, false);
        }
      }
    }
  }

  public IPivotField this[int index]
  {
    get
    {
      return index >= 0 && index < this.Count ? (IPivotField) base[index] : throw new ArgumentOutOfRangeException(nameof (index));
    }
  }

  IPivotField IPivotCalculatedFields.this[string name]
  {
    get
    {
      foreach (PivotFieldImpl pivotFieldImpl in (List<PivotFieldImpl>) this)
      {
        if (pivotFieldImpl.Name == name)
          return (IPivotField) pivotFieldImpl;
      }
      return (IPivotField) null;
    }
  }
}
