// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.CommandEffect
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.SlideImplementation;

#nullable disable
namespace Syncfusion.Presentation;

internal class CommandEffect : Behavior, ICommandEffect, IBehavior
{
  private string commandString;
  private IShape shapeTarget;
  private CommandEffectType type;

  public string CommandString
  {
    get => this.commandString;
    set => this.commandString = value;
  }

  public IShape ShapeTarget
  {
    get => this.shapeTarget;
    set => this.shapeTarget = value;
  }

  public CommandEffectType Type
  {
    get => this.type;
    set => this.type = value;
  }

  internal CommandEffect Clone(BaseSlide newBaseSlide)
  {
    CommandEffect newBehavior = (CommandEffect) this.MemberwiseClone();
    if (this.shapeTarget != null)
      newBehavior.shapeTarget = (IShape) (this.shapeTarget.Clone() as Shape);
    this.CloneBehavior((Behavior) newBehavior, newBaseSlide);
    return newBehavior;
  }
}
