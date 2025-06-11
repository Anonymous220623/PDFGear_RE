// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SmartArtImplementation.SmartArtShape
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;

#nullable disable
namespace Syncfusion.Presentation.SmartArtImplementation;

internal class SmartArtShape : Shape, ISmartArtShape, IShape, ISlideItem
{
  private SmartArt _parentSmartArt;

  internal SmartArtShape(SmartArt smartArt)
    : base(ShapeType.Drawing, smartArt.BaseSlide)
  {
    this._parentSmartArt = smartArt;
  }

  internal SmartArtShape(SmartArt smartArt, ShapeType shapeType)
    : base(shapeType, smartArt.BaseSlide)
  {
    this._parentSmartArt = smartArt;
  }

  internal SmartArt ParentSmartArt => this._parentSmartArt;

  public override ISlideItem Clone()
  {
    SmartArtShape smartArtShape = (SmartArtShape) this.MemberwiseClone();
    this.Clone((Shape) smartArtShape);
    return (ISlideItem) smartArtShape;
  }

  internal void SetParent(SmartArt smartArt)
  {
    this.SetParent(smartArt.BaseSlide);
    this._parentSmartArt = smartArt;
  }

  internal override void Close()
  {
    base.Close();
    this._parentSmartArt = (SmartArt) null;
  }
}
