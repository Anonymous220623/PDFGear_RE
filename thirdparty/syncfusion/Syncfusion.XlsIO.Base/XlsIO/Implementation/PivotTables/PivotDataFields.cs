// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotDataFields
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

public class PivotDataFields(IApplication application, object parent) : 
  CollectionBaseEx<PivotDataField>(application, parent),
  IPivotDataFields
{
  IPivotDataField IPivotDataFields.this[int index] => (IPivotDataField) this.List[index];

  public IPivotDataField Add(IPivotField field, string name, PivotSubtotalTypes subtotal)
  {
    PivotDataField pivotDataField = new PivotDataField(name, subtotal, field as PivotFieldImpl);
    this.Add(pivotDataField);
    if (this.Parent is PivotTableImpl parent && !parent.Workbook.Loading && this.List.Count == 2)
      parent.ColFieldsOrder.Add(-2);
    return (IPivotDataField) pivotDataField;
  }
}
