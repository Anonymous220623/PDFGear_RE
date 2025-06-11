// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.RotationEffect
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.SlideImplementation;

#nullable disable
namespace Syncfusion.Presentation;

internal class RotationEffect : Behavior, IRotationEffect, IBehavior
{
  private float by = float.NaN;
  private float from = float.NaN;
  private float to = float.NaN;

  public float By
  {
    get => this.by;
    set => this.by = value;
  }

  public float From
  {
    get => this.from;
    set => this.from = value;
  }

  public float To
  {
    get => this.to;
    set => this.to = value;
  }

  internal RotationEffect Clone(BaseSlide newBaseSlide)
  {
    RotationEffect newBehavior = (RotationEffect) this.MemberwiseClone();
    this.CloneBehavior((Behavior) newBehavior, newBaseSlide);
    return newBehavior;
  }
}
