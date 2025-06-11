// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.GroupShape
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.SlideImplementation;

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class GroupShape : Shape, IGroupShape, ISlideItem
{
  private Syncfusion.Presentation.Drawing.Shapes _collection;
  private string _groupName;

  internal GroupShape(string groupName, BaseSlide baseSlide)
    : base(ShapeType.GrpSp, baseSlide)
  {
    this._collection = new Syncfusion.Presentation.Drawing.Shapes(this, baseSlide);
    this._groupName = groupName;
  }

  internal string GroupName
  {
    get => this._groupName;
    set => this._groupName = value;
  }

  public IShapes Shapes => (IShapes) this._collection;

  internal ISlideItem this[int index] => this._collection[index];

  internal Shape[] GetGroupedShapes()
  {
    if (this._collection == null)
      return (Shape[]) null;
    Shape[] groupedShapes = new Shape[this._collection.Count];
    for (int index = 0; index < this._collection.Count; ++index)
      groupedShapes[index] = (Shape) this._collection[index];
    return groupedShapes;
  }

  internal void Add(Shape shape)
  {
    shape.SetGroupShape(this);
    this._collection.Add(shape);
  }

  internal void ResetCollection() => this._collection = new Syncfusion.Presentation.Drawing.Shapes(this.BaseSlide);

  internal override void Close()
  {
    base.Close();
    this.Clear();
  }

  internal void Clear()
  {
    if (this._collection == null)
      return;
    this._collection.Close();
    this._collection = (Syncfusion.Presentation.Drawing.Shapes) null;
  }

  public override ISlideItem Clone()
  {
    GroupShape groupShape = (GroupShape) this.MemberwiseClone();
    groupShape._collection = this._collection.Clone();
    groupShape._collection.SetParent(groupShape);
    this.Clone((Shape) groupShape);
    return (ISlideItem) groupShape;
  }

  internal void SetParentForGroupChild(BaseSlide baseSlide)
  {
    foreach (Shape shape in this._collection)
      shape.SetParent(baseSlide);
  }
}
