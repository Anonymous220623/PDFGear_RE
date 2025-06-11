// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.MotionEffect
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.SlideImplementation;
using System.Drawing;

#nullable disable
namespace Syncfusion.Presentation;

internal class MotionEffect : Behavior, IMotionEffect, IBehavior
{
  private float angle = float.NaN;
  private PointF by;
  private PointF from;
  private MotionOriginType origin;
  private IMotionPath path;
  private MotionPathEditMode pathEditMode;
  private PointF rotationCenter;
  private PointF to;

  public MotionEffect()
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
    this.rotationCenter = new PointF();
    this.rotationCenter.X = float.NaN;
    this.rotationCenter.Y = float.NaN;
  }

  public float Angle
  {
    get => this.angle;
    set => this.angle = value;
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

  public MotionOriginType Origin
  {
    get => this.origin;
    set => this.origin = value;
  }

  public IMotionPath Path
  {
    get => this.path;
    set => this.path = value;
  }

  public MotionPathEditMode PathEditMode
  {
    get => this.pathEditMode;
    set => this.pathEditMode = value;
  }

  public PointF RotationCenter
  {
    get => this.rotationCenter;
    set => this.rotationCenter = value;
  }

  public PointF To
  {
    get => this.to;
    set => this.to = value;
  }

  internal MotionEffect Clone(BaseSlide newBaseSlide)
  {
    MotionEffect newBehavior = (MotionEffect) this.MemberwiseClone();
    if (this.path != null)
      newBehavior.path = (IMotionPath) (this.path as MotionPath).Clone();
    this.CloneBehavior((Behavior) newBehavior, newBaseSlide);
    return newBehavior;
  }
}
