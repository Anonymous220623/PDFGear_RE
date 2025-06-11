// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.FilterEffect
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Animation.Internal;
using Syncfusion.Presentation.SlideImplementation;

#nullable disable
namespace Syncfusion.Presentation;

internal class FilterEffect : Behavior, IFilterEffect, IBehavior
{
  private FilterEffectRevealType reveal;
  private SubtypeFilterEffect subtype;
  private FilterEffectType type;
  private Values animationValues;
  private string propertyEffect;

  public FilterEffectRevealType Reveal
  {
    get => this.reveal;
    set => this.reveal = value;
  }

  public SubtypeFilterEffect Subtype
  {
    get => this.subtype;
    set => this.subtype = value;
  }

  public FilterEffectType Type
  {
    get => this.type;
    set => this.type = value;
  }

  internal Values AnimationValues
  {
    get => this.animationValues;
    set => this.animationValues = value;
  }

  internal string PropEffect
  {
    get => this.propertyEffect;
    set => this.propertyEffect = value;
  }

  internal FilterEffect Clone(BaseSlide newBaseSlide)
  {
    FilterEffect newBehavior = (FilterEffect) this.MemberwiseClone();
    if (this.animationValues != null)
      newBehavior.animationValues = this.animationValues.Clone();
    this.CloneBehavior((Behavior) newBehavior, newBaseSlide);
    return newBehavior;
  }
}
