// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.OfficeMaths
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;

#nullable disable
namespace Syncfusion.Office;

internal class OfficeMaths : CollectionImpl, IOfficeMaths, ICollectionBase, IOfficeMathEntity
{
  public IOfficeMath this[int index] => (IOfficeMath) this.InnerList[index];

  internal OfficeMaths(IOfficeMathEntity owner)
    : base(owner)
  {
  }

  public IOfficeMath Add(int index)
  {
    this.OnBeforeInsert();
    OfficeMath officeMath = new OfficeMath(this.OwnerMathEntity);
    this.m_innerList.Insert(index, (object) officeMath);
    return (IOfficeMath) officeMath;
  }

  public IOfficeMath Add()
  {
    this.OnBeforeInsert();
    OfficeMath officeMath = new OfficeMath(this.OwnerMathEntity);
    this.m_innerList.Add((object) officeMath);
    return (IOfficeMath) officeMath;
  }

  internal void Add(OfficeMath item)
  {
    this.OnBeforeInsert();
    this.Add((object) item);
  }

  private void OnBeforeInsert()
  {
    if (this.OwnerMathEntity is IOfficeMathMatrixRow || this.OwnerMathEntity is IOfficeMathMatrixColumn)
      throw new NotSupportedException("New arguments cannot be added directly into matrix rows or columns.");
  }

  internal void CloneItemsTo(OfficeMaths items)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      OfficeMath officeMath = (this[index] as OfficeMath).CloneImpl(items.OwnerMathEntity);
      items.InnerList.Add((object) officeMath);
    }
  }

  internal override void Close()
  {
    if (this.m_innerList != null)
    {
      for (int index = 0; index < this.m_innerList.Count; ++index)
        (this.m_innerList[index] as OfficeMath).Close();
    }
    base.Close();
  }
}
