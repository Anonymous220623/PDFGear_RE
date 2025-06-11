// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.OfficeMathBreaks
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

#nullable disable
namespace Syncfusion.Office;

internal class OfficeMathBreaks : 
  CollectionImpl,
  IOfficeMathBreaks,
  ICollectionBase,
  IOfficeMathEntity
{
  internal OfficeMathBreaks(IOfficeMathEntity owner)
    : base(owner)
  {
  }

  public IOfficeMathBreak Add(int index)
  {
    OfficeMathBreak officeMathBreak = new OfficeMathBreak(this.OwnerMathEntity);
    this.m_innerList.Insert(index, (object) officeMathBreak);
    return (IOfficeMathBreak) officeMathBreak;
  }

  public IOfficeMathBreak Add()
  {
    OfficeMathBreak officeMathBreak = new OfficeMathBreak(this.OwnerMathEntity);
    this.m_innerList.Add((object) officeMathBreak);
    return (IOfficeMathBreak) officeMathBreak;
  }

  public IOfficeMathBreak this[int index]
  {
    get => (IOfficeMathBreak) (this.InnerList[index] as OfficeMathBreak);
  }

  internal void CloneItemsTo(OfficeMathBreaks items)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      OfficeMathBreak officeMathBreak = (this[index] as OfficeMathBreak).Clone();
      officeMathBreak.SetOwner(items.OwnerMathEntity);
      items.Add((object) officeMathBreak);
    }
  }

  internal override void Close()
  {
    if (this.m_innerList != null)
    {
      for (int index = 0; index < this.m_innerList.Count; ++index)
        (this.m_innerList[index] as OfficeMathBreak).Close();
    }
    base.Close();
  }
}
