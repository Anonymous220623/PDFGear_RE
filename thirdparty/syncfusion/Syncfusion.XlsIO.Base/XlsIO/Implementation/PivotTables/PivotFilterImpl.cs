// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotFilterImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

internal class PivotFilterImpl : IPivotFilter
{
  private string str_value;
  private PivotFilterCollections m_parent;

  internal PivotFilterImpl(PivotFilterCollections pivotFilterCollections)
  {
    this.m_parent = pivotFilterCollections;
  }

  public string Value1
  {
    get => this.str_value;
    set
    {
      if (!string.IsNullOrEmpty(value))
      {
        PivotFieldImpl parent = this.m_parent.Parent as PivotFieldImpl;
        this.UpdateFields(parent, parent.PivotTable, value);
        IWorkbook workbook = (IWorkbook) parent.m_table.Workbook;
        if (parent.CacheFieldUpdated && workbook != null)
        {
          for (int Index = 0; Index < workbook.Worksheets.Count; ++Index)
          {
            for (int index = 0; index < workbook.Worksheets[Index].PivotTables.Count; ++index)
            {
              if (workbook.Worksheets[Index].PivotTables[index] is PivotTableImpl pivotTable && parent.m_table.CacheIndex.Equals(pivotTable.CacheIndex))
                this.UpdateFields(pivotTable.Fields[parent.Name] as PivotFieldImpl, pivotTable, value);
            }
          }
        }
      }
      this.str_value = value;
    }
  }

  private void UpdateFields(PivotFieldImpl pivotField, PivotTableImpl table, string value)
  {
    PivotCacheImpl cache = table.Cache;
    PivotCacheFieldImpl cacheField = pivotField.CacheField;
    if (pivotField.CacheFieldUpdated && cacheField.Items.Contains((object) value) || !(cache.SourceRange is RangeImpl))
      return;
    IRange column = cache.SourceRange.Columns[table.Fields.IndexOf(pivotField)];
    cacheField.ItemRange = column.Worksheet[column.Row + 1, column.Column, column.LastRow, column.LastColumn];
    cacheField.Fill(column.Worksheet, column.Row, column.LastRow, column.Column);
    pivotField.LoadPivotItems(cacheField);
    pivotField.CacheFieldUpdated = true;
  }
}
