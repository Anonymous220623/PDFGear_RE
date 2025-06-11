// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.GroupShapesCollection
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.SlideImplementation;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class GroupShapesCollection : IGroupShapes, IEnumerable<IGroupShape>, IEnumerable
{
  private BaseSlide _baseSlide;

  internal GroupShapesCollection(BaseSlide baseSlide) => this._baseSlide = baseSlide;

  public IGroupShape AddGroupShape(double left, double top, double width, double height)
  {
    return this._baseSlide.Shapes.AddGroupShape(left, top, width, height);
  }

  public int IndexOf(IGroupShape groupShape) => this.GetList().IndexOf(groupShape);

  public void Remove(IGroupShape groupShape)
  {
    ((Shapes) this._baseSlide.Shapes).Remove((ISlideItem) groupShape);
  }

  public void RemoveAt(int index) => this.Remove(this[index]);

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetList().GetEnumerator();

  public IEnumerator<IGroupShape> GetEnumerator()
  {
    return (IEnumerator<IGroupShape>) this.GetList().GetEnumerator();
  }

  public IGroupShape this[int index] => this.GetList()[index];

  public int Count => this.GetList().Count;

  internal List<IGroupShape> GetList()
  {
    List<IGroupShape> list = new List<IGroupShape>();
    foreach (IShape shape in (IEnumerable<ISlideItem>) this._baseSlide.Shapes)
    {
      if (shape.SlideItemType == SlideItemType.GroupShape)
        list.Add((IGroupShape) shape);
    }
    return list;
  }
}
