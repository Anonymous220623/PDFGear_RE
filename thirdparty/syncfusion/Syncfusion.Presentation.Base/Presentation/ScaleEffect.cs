// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.ScaleEffect
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.SlideImplementation;
using System.Drawing;

#nullable disable
namespace Syncfusion.Presentation;

internal class ScaleEffect : Behavior, IScaleEffect, IBehavior
{
  private PointF by;
  private PointF from;
  private PointF to;
  private bool? zoomContent;

  public ScaleEffect()
  {
    this.by = new PointF();
    this.by.X = float.NaN;
    this.by.Y = float.NaN;
    this.from = new PointF();
    this.from.X = float.NaN;
    this.from.Y = float.NaN;
    this.to = new PointF();
    this.to.X = float.NaN;
    this.to.Y = float.NaN;
  }

  public PointF By
  {
    get => this.by;
    set => this.by = value;
  }

  public PointF From
  {
    get => this.from;
    set => this.from = value;
  }

  public PointF To
  {
    get => this.to;
    set => this.to = value;
  }

  public bool? ZoomContent
  {
    get => this.zoomContent;
    set => this.zoomContent = value;
  }

  internal ScaleEffect Clone(BaseSlide newBaseSlide)
  {
    ScaleEffect newBehavior = (ScaleEffect) this.MemberwiseClone();
    this.CloneBehavior((Behavior) newBehavior, newBaseSlide);
    return newBehavior;
  }
}
