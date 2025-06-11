// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.Internal.ShapeTarget
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.Animation.Internal;

internal class ShapeTarget
{
  private string shapeId;
  private BackGround background;
  private GraphicElement graphicElement;
  private OleChartElement oleChartElement;
  private SubShape subShape;
  private TextElement textElement;

  internal string ShapeId
  {
    get => this.shapeId;
    set => this.shapeId = value;
  }

  internal BackGround Background
  {
    get => this.background;
    set => this.background = value;
  }

  internal GraphicElement GraphicElement
  {
    get => this.graphicElement;
    set => this.graphicElement = value;
  }

  internal OleChartElement OleChartElement
  {
    get => this.oleChartElement;
    set => this.oleChartElement = value;
  }

  internal SubShape SubShape
  {
    get => this.subShape;
    set => this.subShape = value;
  }

  internal TextElement TextElement
  {
    get => this.textElement;
    set => this.textElement = value;
  }

  internal ShapeTarget Clone()
  {
    ShapeTarget shapeTarget = (ShapeTarget) this.MemberwiseClone();
    if (this.graphicElement != null)
      shapeTarget.graphicElement = this.graphicElement.Clone();
    if (this.oleChartElement != null)
      shapeTarget.oleChartElement = this.oleChartElement.Clone();
    if (this.subShape != null)
      shapeTarget.subShape = this.subShape.Clone();
    if (this.textElement != null)
      shapeTarget.textElement = this.textElement.Clone();
    return shapeTarget;
  }
}
