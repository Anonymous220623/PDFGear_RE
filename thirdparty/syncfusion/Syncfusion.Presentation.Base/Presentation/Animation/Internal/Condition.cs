// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.Internal.Condition
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.Animation.Internal;

internal class Condition
{
  private RunTimeNodeTrigger runTimeNodeTrigger;
  private TargetElement target;
  private TimeNode timeNode;
  private int delay;
  private TriggerEvent triggerEvent;

  internal RunTimeNodeTrigger RunTimeNodeTrigger
  {
    get => this.runTimeNodeTrigger;
    set => this.runTimeNodeTrigger = value;
  }

  internal TargetElement Target
  {
    get => this.target;
    set => this.target = value;
  }

  internal TimeNode TimeNode
  {
    get => this.timeNode;
    set => this.timeNode = value;
  }

  internal int Delay
  {
    get => this.delay;
    set => this.delay = value;
  }

  internal TriggerEvent Event
  {
    get => this.triggerEvent;
    set => this.triggerEvent = value;
  }

  internal Condition Clone()
  {
    Condition condition = (Condition) this.MemberwiseClone();
    if (this.runTimeNodeTrigger != null)
      condition.runTimeNodeTrigger = this.runTimeNodeTrigger.Clone();
    if (this.target != null)
      condition.target = this.target.Clone();
    if (this.timeNode != null)
      condition.timeNode = this.timeNode.Clone();
    return condition;
  }
}
