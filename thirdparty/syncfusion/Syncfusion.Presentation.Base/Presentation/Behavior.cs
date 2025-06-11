// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Behavior
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Animation.Internal;
using Syncfusion.Presentation.SlideImplementation;

#nullable disable
namespace Syncfusion.Presentation;

internal abstract class Behavior : IBehavior
{
  private bool? accumulate = new bool?();
  private BehaviorAdditiveType additive = BehaviorAdditiveType.NotDefined;
  private IBehaviorProperties properties = (IBehaviorProperties) new BehaviorProperties();
  private ITiming timing = (ITiming) new Syncfusion.Presentation.Animation.Timing();
  private TargetElement targetElement;
  private CommonBehavior commonBehavior;

  public bool? Accumulate
  {
    get => this.accumulate;
    set => this.accumulate = value;
  }

  public BehaviorAdditiveType Additive
  {
    get => this.additive;
    set => this.additive = value;
  }

  public IBehaviorProperties Properties => this.properties;

  public ITiming Timing
  {
    get => this.timing;
    set => this.timing = value;
  }

  internal TargetElement TargetElement
  {
    get => this.targetElement;
    set => this.targetElement = value;
  }

  internal CommonBehavior CommonBehavior
  {
    get => this.commonBehavior;
    set => this.commonBehavior = value;
  }

  internal Behavior CloneBehavior(Behavior newBehavior, BaseSlide newBaseSlide)
  {
    newBehavior.properties = (IBehaviorProperties) (this.properties as BehaviorProperties).Clone();
    newBehavior.timing = (ITiming) (this.timing as Syncfusion.Presentation.Animation.Timing).Clone();
    if (this.targetElement != null)
      newBehavior.targetElement = this.targetElement.Clone();
    if (this.commonBehavior != null)
      newBehavior.commonBehavior = this.commonBehavior.Clone(newBaseSlide);
    return newBehavior;
  }
}
