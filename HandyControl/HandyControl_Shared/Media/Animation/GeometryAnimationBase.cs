// Decompiled with JetBrains decompiler
// Type: HandyControl.Media.Animation.GeometryAnimationBase
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Globalization;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace HandyControl.Media.Animation;

public abstract class GeometryAnimationBase : AnimationTimeline
{
  protected GeometryAnimationBase()
  {
    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
  }

  public GeometryAnimationBase Clone() => (GeometryAnimationBase) base.Clone();

  public sealed override object GetCurrentValue(
    object defaultOriginValue,
    object defaultDestinationValue,
    AnimationClock animationClock)
  {
    if (defaultOriginValue == null)
      throw new ArgumentNullException(nameof (defaultOriginValue));
    if (defaultDestinationValue == null)
      throw new ArgumentNullException(nameof (defaultDestinationValue));
    return (object) this.GetCurrentValue((Geometry) defaultOriginValue, (Geometry) defaultDestinationValue, animationClock);
  }

  public override Type TargetPropertyType
  {
    get
    {
      this.ReadPreamble();
      return typeof (Geometry);
    }
  }

  public Geometry GetCurrentValue(
    Geometry defaultOriginValue,
    Geometry defaultDestinationValue,
    AnimationClock animationClock)
  {
    this.ReadPreamble();
    if (animationClock == null)
      throw new ArgumentNullException(nameof (animationClock));
    return animationClock.CurrentState == ClockState.Stopped ? defaultDestinationValue : this.GetCurrentValueCore(defaultOriginValue, defaultDestinationValue, animationClock);
  }

  protected abstract Geometry GetCurrentValueCore(
    Geometry defaultOriginValue,
    Geometry defaultDestinationValue,
    AnimationClock animationClock);
}
