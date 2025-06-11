// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.Timing
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;

#nullable disable
namespace Syncfusion.Presentation.Animation;

internal class Timing : ITiming
{
  private float accelarate;
  private bool autoReverse;
  private float decelerate;
  private float duration = float.NaN;
  private float repeatCount;
  private float repeatDuration = float.NaN;
  private EffectRestartType restart;
  private float speed;
  private float triggerDelayTime = float.NaN;
  private EffectTriggerType triggerType;
  private TimeNodeFill fill;

  public float Accelerate
  {
    get => this.accelarate;
    set
    {
      this.accelarate = (double) value <= 1.0 && (double) value >= 0.0 ? value : throw new Exception("Value should be with in the range of 0 to 1");
    }
  }

  public bool AutoReverse
  {
    get => this.autoReverse;
    set => this.autoReverse = value;
  }

  public float Decelerate
  {
    get => this.decelerate;
    set
    {
      this.decelerate = (double) value <= 1.0 && (double) value >= 0.0 ? value : throw new Exception("Value should be with in the range of 0 to 1");
    }
  }

  public float Duration
  {
    get => this.duration;
    set => this.duration = value;
  }

  public float RepeatCount
  {
    get => this.repeatCount;
    set => this.repeatCount = value;
  }

  public float RepeatDuration
  {
    get => this.repeatDuration;
    set => this.repeatDuration = value;
  }

  public EffectRestartType Restart
  {
    get => this.restart;
    set => this.restart = value;
  }

  public float Speed
  {
    get => this.speed;
    set => this.speed = value;
  }

  public float TriggerDelayTime
  {
    get => this.triggerDelayTime;
    set => this.triggerDelayTime = value;
  }

  public EffectTriggerType TriggerType
  {
    get => this.triggerType;
    set => this.triggerType = value;
  }

  internal TimeNodeFill Fill
  {
    get => this.fill;
    set => this.fill = value;
  }

  internal Timing Clone() => (Timing) this.MemberwiseClone();
}
