// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.OfficeMathMatrixRows
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

#nullable disable
namespace Syncfusion.Office;

internal class OfficeMathMatrixRows : 
  CollectionImpl,
  IOfficeMathMatrixRows,
  ICollectionBase,
  IOfficeMathEntity
{
  public IOfficeMathMatrixRow this[int index]
  {
    get => (IOfficeMathMatrixRow) (this.InnerList[index] as OfficeMathMatrixRow);
  }

  public new void Remove(IOfficeMathEntity item)
  {
    if (item is OfficeMathMatrixRow)
    {
      OfficeMathMatrixRow officeMathMatrixRow = item as OfficeMathMatrixRow;
      OfficeMathMatrix ownerMathEntity = this.OwnerMathEntity as OfficeMathMatrix;
      if (officeMathMatrixRow.RowIndex > -1)
        ownerMathEntity.RemoveMatrixItems(0, officeMathMatrixRow.RowIndex, ownerMathEntity.Columns.Count - 1, officeMathMatrixRow.RowIndex);
    }
    base.Remove(item);
  }

  public IOfficeMathMatrixRow Add(int index)
  {
    OfficeMathMatrixRow officeMathMatrixRow = new OfficeMathMatrixRow(this.OwnerMathEntity);
    this.m_innerList.Insert(index, (object) officeMathMatrixRow);
    officeMathMatrixRow.OnRowAdded();
    return (IOfficeMathMatrixRow) officeMathMatrixRow;
  }

  public IOfficeMathMatrixRow Add()
  {
    OfficeMathMatrixRow officeMathMatrixRow = new OfficeMathMatrixRow(this.OwnerMathEntity);
    this.m_innerList.Add((object) officeMathMatrixRow);
    officeMathMatrixRow.OnRowAdded();
    return (IOfficeMathMatrixRow) officeMathMatrixRow;
  }

  internal void CloneItemsTo(OfficeMathMatrixRows items)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      OfficeMathMatrixRow officeMathMatrixRow = (this[index] as OfficeMathMatrixRow).Clone(items.OwnerMathEntity);
      items.Add((object) officeMathMatrixRow);
    }
  }

  internal OfficeMathMatrixRows(IOfficeMathEntity owner)
    : base(owner)
  {
  }
}
