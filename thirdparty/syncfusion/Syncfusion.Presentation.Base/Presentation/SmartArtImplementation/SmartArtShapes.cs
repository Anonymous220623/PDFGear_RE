// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SmartArtImplementation.SmartArtShapes
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.SmartArtImplementation;

internal class SmartArtShapes : ISmartArtShapes, IEnumerable<ISmartArtShape>, IEnumerable
{
  private List<ISmartArtShape> _list;
  private ISmartArtNode _parent;

  internal SmartArtShapes(ISmartArtNode smartArtNode)
  {
    this._parent = smartArtNode;
    this._list = new List<ISmartArtShape>();
  }

  public ISmartArtShape this[int index]
  {
    get
    {
      return this._list.Count != 0 && index < this._list.Count ? this._list[index] : (ISmartArtShape) null;
    }
  }

  internal void Add(SmartArtShape smartArtShape) => this._list.Add((ISmartArtShape) smartArtShape);

  public IEnumerator<ISmartArtShape> GetEnumerator()
  {
    return (IEnumerator<ISmartArtShape>) this._list.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._list.GetEnumerator();

  public ISmartArtShapes Clone()
  {
    SmartArtShapes smartArtShapes = (SmartArtShapes) this.MemberwiseClone();
    smartArtShapes._list = this.CloneShapeList();
    return (ISmartArtShapes) smartArtShapes;
  }

  private List<ISmartArtShape> CloneShapeList()
  {
    List<ISmartArtShape> smartArtShapeList = new List<ISmartArtShape>();
    foreach (SmartArtShape smartArtShape in this._list)
      smartArtShapeList.Add((ISmartArtShape) smartArtShape.Clone());
    return smartArtShapeList;
  }

  internal void SetParent(SmartArtNode smartArtNode)
  {
    this._parent = (ISmartArtNode) smartArtNode;
    foreach (SmartArtShape smartArtShape in this._list)
      smartArtShape.SetParent(smartArtNode.BasePoint.DataModel.ParentSmartArt);
  }
}
