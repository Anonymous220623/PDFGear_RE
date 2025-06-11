// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.OfficeMathMatrixColumns
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

#nullable disable
namespace Syncfusion.Office;

internal class OfficeMathMatrixColumns : 
  CollectionImpl,
  IOfficeMathMatrixColumns,
  ICollectionBase,
  IOfficeMathEntity
{
  public IOfficeMathMatrixColumn this[int index]
  {
    get => (IOfficeMathMatrixColumn) (this.InnerList[index] as OfficeMathMatrixColumn);
  }

  public IOfficeMathMatrixColumn Add(int index)
  {
    OfficeMathMatrixColumn mathMatrixColumn = new OfficeMathMatrixColumn(this.OwnerMathEntity);
    this.m_innerList.Insert(index, (object) mathMatrixColumn);
    mathMatrixColumn.OnColumnAdded();
    return (IOfficeMathMatrixColumn) mathMatrixColumn;
  }

  public IOfficeMathMatrixColumn Add()
  {
    OfficeMathMatrixColumn mathMatrixColumn = new OfficeMathMatrixColumn(this.OwnerMathEntity);
    this.m_innerList.Add((object) mathMatrixColumn);
    mathMatrixColumn.OnColumnAdded();
    return (IOfficeMathMatrixColumn) mathMatrixColumn;
  }

  internal void CloneItemsTo(OfficeMathMatrixColumns items)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      OfficeMathMatrixColumn mathMatrixColumn = (this[index] as OfficeMathMatrixColumn).Clone(items.OwnerMathEntity);
      mathMatrixColumn.SetOwner(items.OwnerMathEntity);
      items.Add((object) mathMatrixColumn);
    }
  }

  public new void Remove(IOfficeMathEntity item)
  {
    if (item is OfficeMathMatrixColumn)
    {
      OfficeMathMatrixColumn mathMatrixColumn = item as OfficeMathMatrixColumn;
      OfficeMathMatrix ownerMathEntity = this.OwnerMathEntity as OfficeMathMatrix;
      if (mathMatrixColumn.ColumnIndex > -1)
        ownerMathEntity.RemoveMatrixItems(mathMatrixColumn.ColumnIndex, 0, mathMatrixColumn.ColumnIndex, ownerMathEntity.Rows.Count - 1);
    }
    base.Remove(item);
  }

  internal OfficeMathMatrixColumns(IOfficeMathEntity owner)
    : base(owner)
  {
  }
}
