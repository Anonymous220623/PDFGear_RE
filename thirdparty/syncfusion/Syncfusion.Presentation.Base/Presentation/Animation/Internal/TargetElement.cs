// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.Internal.TargetElement
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.Animation.Internal;

internal class TargetElement
{
  private ShapeTarget shapeTarget;
  private SlideTarget slideTarget;
  private InkTarget inkTarget;

  internal ShapeTarget ShapeTarget
  {
    get => this.shapeTarget;
    set => this.shapeTarget = value;
  }

  internal SlideTarget SlideTarget
  {
    get => this.slideTarget;
    set => this.slideTarget = value;
  }

  internal InkTarget InkTarget
  {
    get => this.inkTarget;
    set => this.inkTarget = value;
  }

  internal TargetElement Clone()
  {
    TargetElement targetElement = (TargetElement) this.MemberwiseClone();
    if (this.shapeTarget != null)
      targetElement.shapeTarget = this.shapeTarget.Clone();
    if (this.inkTarget != null)
      targetElement.inkTarget = this.inkTarget.Clone();
    return targetElement;
  }
}
