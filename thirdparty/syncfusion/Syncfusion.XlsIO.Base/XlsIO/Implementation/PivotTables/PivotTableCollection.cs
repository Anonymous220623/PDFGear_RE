// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotTableCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

public class PivotTableCollection(IApplication application, object parent) : 
  CollectionBaseEx<object>(application, parent),
  ICloneParent,
  IEnumerable<PivotTableImpl>,
  IEnumerable,
  IPivotTables
{
  public IPivotTable this[int index]
  {
    get
    {
      return index >= 0 && index < this.Count ? (IPivotTable) this.InnerList[index] : throw new ArgumentOutOfRangeException(nameof (index));
    }
  }

  public IPivotTable this[string name]
  {
    get
    {
      IPivotTable pivotTable = (IPivotTable) null;
      foreach (IPivotTable inner in this.InnerList)
      {
        if (inner.Name == name)
        {
          pivotTable = inner;
          break;
        }
      }
      return pivotTable;
    }
  }

  public WorksheetImpl ParentWorksheet => this.FindParent(typeof (WorksheetImpl)) as WorksheetImpl;

  public int Parse(IList data, int iPos)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (iPos < 0 || iPos > data.Count - 1)
      throw new ArgumentOutOfRangeException(nameof (iPos), "Value cannot be less than 0 and greater than data.Length - 1");
    BiffRecordRaw biffRecordRaw = (BiffRecordRaw) data[iPos];
    while (biffRecordRaw.TypeCode == TBIFFRecord.PivotViewDefinition)
    {
      PivotTableImpl pivotTableImpl = new PivotTableImpl(this.Application, (object) this);
      iPos = pivotTableImpl.Parse(data, iPos);
      biffRecordRaw = (BiffRecordRaw) data[iPos];
      this.Add((object) pivotTableImpl);
    }
    return iPos;
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      ((PivotTableImpl) this.InnerList[index]).Serialize(records);
  }

  public void Add(PivotTableImpl table) => this.Add((object) table);

  public IPivotTable Add(string name, IRange location, IPivotCache cache)
  {
    PivotTableImpl table = new PivotTableImpl(this.Application, (object) this, cache.Index, location);
    table.Name = name;
    table.Cache.IsRefreshOnLoad = true;
    table.Cache.IsSaveData = true;
    PivotTableOptions options = table.Options as PivotTableOptions;
    options.IsAutoFormat = true;
    options.IsWHAutoFormat = true;
    this.Add(table);
    return (IPivotTable) table;
  }

  public new object Clone(object parent)
  {
    return (object) this.Clone(parent as WorksheetImpl, (Dictionary<string, string>) null);
  }

  public PivotTableCollection Clone(
    WorksheetImpl worksheet,
    Dictionary<string, string> hashWorksheetNames)
  {
    PivotTableCollection tables = new PivotTableCollection(worksheet.Application, (object) worksheet);
    WorkbookImpl parentWorkbook = worksheet.ParentWorkbook;
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      PivotTableImpl table = ((PivotTableImpl) this[index]).Clone(tables, hashWorksheetNames);
      tables.Add(table);
    }
    return tables;
  }

  public void Remove(string name)
  {
    int index = 0;
    PivotTableImpl pivotTable = (PivotTableImpl) null;
    foreach (PivotTableImpl inner in this.InnerList)
    {
      if (inner.Name.Equals(name))
      {
        pivotTable = inner;
        break;
      }
      ++index;
    }
    if (pivotTable == null)
      return;
    this.InnerList.RemoveAt(index);
    this.CleanAfterRemove(pivotTable);
  }

  public new void RemoveAt(int index) => this.Remove(this[index].Name);

  private void CleanAfterRemove(PivotTableImpl pivotTable)
  {
    WorkbookImpl workbook = pivotTable.Workbook;
    PivotCacheCollection pivotCaches = workbook.PivotCaches;
    workbook.RemoveCache(pivotTable.CacheIndex);
    pivotTable.ClearPivotRange();
  }

  internal void ClearWithoutCheck() => this.Clear();

  public IEnumerator<PivotTableImpl> GetEnumerator()
  {
    foreach (PivotTableImpl table in this.InnerList)
      yield return table;
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    foreach (PivotTableImpl table in this.InnerList)
      yield return (object) table;
  }
}
