// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.ColorEffect
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.SlideImplementation;

#nullable disable
namespace Syncfusion.Presentation;

internal class ColorEffect : Behavior, IColorEffect, IBehavior
{
  private IColorOffset by;
  private ColorSpace colorSpace;
  private ColorDirection direction;
  private ColorObject from;
  private ColorObject to;

  public ColorEffect() => this.by = (IColorOffset) new ColorOffset();

  public IColorOffset By
  {
    get => this.by;
    set => this.by = value;
  }

  public ColorSpace ColorSpace
  {
    get => this.colorSpace;
    set => this.colorSpace = value;
  }

  public ColorDirection Direction
  {
    get => this.direction;
    set => this.direction = value;
  }

  public IColor From
  {
    get
    {
      if (this.from != null)
        this.from.UpdateColorObject((object) this.CommonBehavior.CommonTimeNode.BaseSlide.Presentation);
      return (IColor) this.from;
    }
    set => this.from = value as ColorObject;
  }

  public IColor To
  {
    get
    {
      if (this.to != null)
        this.to.UpdateColorObject((object) this.CommonBehavior.CommonTimeNode.BaseSlide.Presentation);
      return (IColor) this.to;
    }
    set => this.to = value as ColorObject;
  }

  internal ColorObject GetToColorObject() => this.to;

  internal ColorObject GetFromColorObject() => this.from;

  internal ColorEffect Clone(BaseSlide newBaseSlide)
  {
    ColorEffect newBehavior = (ColorEffect) this.MemberwiseClone();
    if (this.by != null)
      newBehavior.by = (IColorOffset) (this.by as ColorOffset).Clone();
    if (this.from != null)
      newBehavior.from = this.from.CloneColorObject();
    if (this.to != null)
      newBehavior.to = this.to.CloneColorObject();
    this.CloneBehavior((Behavior) newBehavior, newBaseSlide);
    return newBehavior;
  }
}
