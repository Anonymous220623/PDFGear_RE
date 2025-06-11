// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SetEffect
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Animation.Internal;
using Syncfusion.Presentation.SlideImplementation;

#nullable disable
namespace Syncfusion.Presentation;

internal class SetEffect : Behavior, ISetEffect, IBehavior
{
  private Values internalTo;

  public object To
  {
    get
    {
      if (this.internalTo.Bool.HasValue)
        return (object) this.internalTo.Bool.ToString();
      if (this.internalTo.Int.HasValue)
        return (object) this.internalTo.Int.ToString();
      if (this.internalTo.Float.HasValue)
        return (object) this.internalTo.Float.ToString();
      return this.internalTo.Color != null ? (object) this.internalTo.Color.ToString() : (object) this.internalTo.String;
    }
    set
    {
      this.InternalTo = new Values();
      switch (value)
      {
        case string _:
          this.InternalTo.String = value as string;
          break;
        case int _:
          this.InternalTo.Int = value as int?;
          break;
        case float _:
          this.InternalTo.Float = value as float?;
          break;
        case bool _:
          this.InternalTo.Bool = value as bool?;
          break;
      }
    }
  }

  internal Values InternalTo
  {
    get => this.internalTo;
    set => this.internalTo = value;
  }

  internal SetEffect Clone(BaseSlide newBaseSlide)
  {
    SetEffect newBehavior = (SetEffect) this.MemberwiseClone();
    if (this.internalTo != null)
      newBehavior.internalTo = this.internalTo.Clone();
    this.CloneBehavior((Behavior) newBehavior, newBaseSlide);
    return newBehavior;
  }
}
