// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ChildShapeCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class ChildShapeCollection : CollectionImpl
{
  public ChildShape this[int index] => this.InnerList[index] as ChildShape;

  internal ChildShapeCollection(WordDocument doc)
    : base(doc, (OwnerHolder) doc)
  {
  }

  public void Add(ChildShape childShape)
  {
    this.InnerList.Add((object) childShape);
    if (this.Document.IsOpening || childShape.skipPositionUpdate || childShape.Owner is ChildGroupShape)
      return;
    this.UpdatePositionForGroupShapeAndChildShape(childShape);
  }

  internal void UpdatePositionForGroupShapeAndChildShape(ChildShape childShape)
  {
    childShape.GetOwnerGroupShape().UpdatePositionForGroupShapeAndChildShape();
  }

  public void RemoveAt(int index) => this.InnerList.RemoveAt(index);

  public void Clear()
  {
    while (this.InnerList.Count > 0)
      this.RemoveAt(this.InnerList.Count - 1);
  }

  public void Remove(ChildShape childShape) => this.InnerList.Remove((object) childShape);
}
