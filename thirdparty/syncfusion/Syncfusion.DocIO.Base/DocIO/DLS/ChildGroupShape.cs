// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ChildGroupShape
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Layouting;
using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class ChildGroupShape : ChildShape, IEntity, ILeafWidget, IWidget
{
  private ChildShapeCollection m_childShapes;
  private float m_extentXValue;
  private float m_extentYValue;
  private float m_offsetXValue;
  private float m_offsetYValue;
  private long shapeId;
  private bool m_hasPictureItem;

  internal long GenerateShapeID() => this.shapeId++;

  internal float OffsetXValue
  {
    get => this.m_offsetXValue;
    set => this.m_offsetXValue = value;
  }

  internal float OffsetYValue
  {
    get => this.m_offsetYValue;
    set => this.m_offsetYValue = value;
  }

  internal float ExtentXValue
  {
    get => this.m_extentXValue;
    set => this.m_extentXValue = value;
  }

  internal float ExtentYValue
  {
    get => this.m_extentYValue;
    set => this.m_extentYValue = value;
  }

  internal bool HasPictureItem
  {
    get => this.m_hasPictureItem;
    set => this.m_hasPictureItem = value;
  }

  internal ChildShapeCollection ChildShapes
  {
    get
    {
      if (this.m_childShapes == null)
        this.m_childShapes = new ChildShapeCollection(this.Document);
      return this.m_childShapes;
    }
    set => this.m_childShapes = value;
  }

  public override EntityType EntityType => EntityType.ChildGroupShape;

  internal void Add(ParagraphItem childShape)
  {
    if (!this.Document.IsOpening && childShape.EntityType != EntityType.ChildShape && childShape.GetTextWrappingStyle() == TextWrappingStyle.Inline)
      throw new Exception("Inline objects cannot be grouped");
    GroupShape groupShape = new GroupShape((IWordDocument) this.Document);
    switch (childShape.EntityType)
    {
      case EntityType.Picture:
        ChildShape childShape1 = groupShape.ConvertPictureToChildShape(childShape as WPicture);
        childShape1.SetOwner((OwnerHolder) this);
        this.ChildShapes.Add(childShape1);
        break;
      case EntityType.TextBox:
        ChildShape childShape2 = groupShape.ConvertTextboxToChildShape(childShape as WTextBox);
        childShape2.SetOwner((OwnerHolder) this);
        this.ChildShapes.Add(childShape2);
        break;
      case EntityType.Chart:
        ChildShape childShape3 = groupShape.ConvertChartToChildShape(childShape as WChart);
        childShape3.SetOwner((OwnerHolder) this);
        this.ChildShapes.Add(childShape3);
        break;
      case EntityType.AutoShape:
        ChildShape childShape4 = groupShape.ConvertShapeToChildShape(childShape as Shape);
        childShape4.SetOwner((OwnerHolder) this);
        this.ChildShapes.Add(childShape4);
        break;
      case EntityType.GroupShape:
        ChildShape childGroupShape = (ChildShape) groupShape.ConvertGroupShapeToChildGroupShape(childShape as GroupShape);
        childGroupShape.SetOwner((OwnerHolder) this);
        this.ChildShapes.Add(childGroupShape);
        break;
      case EntityType.ChildShape:
        childShape.SetOwner((OwnerHolder) this);
        this.ChildShapes.Add(childShape as ChildShape);
        break;
      case EntityType.ChildGroupShape:
        childShape.SetOwner((OwnerHolder) this);
        this.ChildShapes.Add((ChildShape) (childShape as ChildGroupShape));
        break;
      default:
        throw new InvalidOperationException($"Cannot add object of type {childShape.EntityType} to Group Shape");
    }
  }

  internal ChildGroupShape(IWordDocument doc)
    : base(doc)
  {
  }

  protected override object CloneImpl()
  {
    ChildGroupShape owner = (ChildGroupShape) base.CloneImpl();
    owner.ChildShapes = new ChildShapeCollection(this.m_doc);
    foreach (Entity childShape in (CollectionImpl) this.ChildShapes)
    {
      Entity entity = childShape.Clone();
      entity.SetOwner((OwnerHolder) owner);
      if (entity is ChildGroupShape)
        owner.ChildShapes.Add((ChildShape) (entity as ChildGroupShape));
      else
        owner.ChildShapes.Add(entity as ChildShape);
    }
    return (object) owner;
  }

  internal override void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
    base.CloneRelationsTo(doc, nextOwner);
    int index = 0;
    for (int count = this.ChildShapes.Count; index < count; ++index)
      this.ChildShapes[index].CloneRelationsTo(doc, nextOwner);
  }
}
