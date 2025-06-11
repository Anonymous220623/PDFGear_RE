// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotTableFields
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using System;
using System.Collections;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

public class PivotTableFields : CollectionBaseEx<PivotFieldImpl>, IPivotFields, IEnumerable
{
  private PivotTableImpl m_table;

  IPivotField IPivotFields.this[int index]
  {
    get
    {
      return index >= 0 && index < this.Count ? (IPivotField) this.InnerList[index] : throw new ArgumentOutOfRangeException(nameof (index));
    }
  }

  public IPivotField this[string name]
  {
    get
    {
      IPivotField pivotField1 = (IPivotField) null;
      int i = 0;
      for (int count = this.Count; i < count; ++i)
      {
        IPivotField pivotField2 = (IPivotField) this[i];
        if (pivotField2.Name == name)
        {
          pivotField1 = pivotField2;
          break;
        }
      }
      return pivotField1;
    }
  }

  public PivotTableFields(IApplication application, object parent)
    : base(application, parent)
  {
    this.m_table = this.FindParent(typeof (PivotTableImpl)) as PivotTableImpl;
    if (this.m_table == null)
      throw new ArgumentException(nameof (parent));
  }

  public PivotTableFields(PivotTableImpl table)
    : this(table.Application, (object) table)
  {
    PivotCacheFieldsCollection cacheFields = table.Cache.CacheFields;
    int i = 0;
    for (int count = cacheFields.Count; i < count; ++i)
      this.Add(cacheFields[i], table.Workbook);
  }

  private void Add(PivotCacheFieldImpl cacheField, WorkbookImpl book)
  {
    this.Add(new PivotFieldImpl(cacheField, this.m_table));
  }

  public override object Clone(object parent)
  {
    PivotTableFields pivotTableFields = parent is PivotTableImpl table ? new PivotTableFields(table) : throw new ArgumentException(nameof (parent));
    return base.Clone(parent);
  }
}
