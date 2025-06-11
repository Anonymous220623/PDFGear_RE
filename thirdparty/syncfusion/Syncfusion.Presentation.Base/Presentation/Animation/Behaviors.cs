// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.Behaviors
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.SlideImplementation;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.Animation;

internal class Behaviors : IBehaviors, IEnumerable<IBehavior>, IEnumerable
{
  private List<IBehavior> behaviorList;

  public Behaviors() => this.behaviorList = new List<IBehavior>();

  public int Count => this.behaviorList.Count;

  public IBehavior this[int index]
  {
    get
    {
      if (this.behaviorList.Count <= index)
        throw new IndexOutOfRangeException("Index was out of range, value should be less than Behaviors count");
      return this.behaviorList[index];
    }
  }

  public void Add(IBehavior behavior)
  {
    if (behavior == null)
      return;
    this.behaviorList.Add(behavior);
  }

  public void Clear() => this.behaviorList.Clear();

  public int IndexOf(IBehavior item) => this.behaviorList.IndexOf(item);

  public IEnumerator<IBehavior> GetEnumerator()
  {
    return (IEnumerator<IBehavior>) this.behaviorList.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.behaviorList.GetEnumerator();

  internal Behaviors Clone(BaseSlide newBaseSlide)
  {
    Behaviors behaviors = (Behaviors) this.MemberwiseClone();
    behaviors.behaviorList = this.CloneBehaviorList(newBaseSlide);
    return behaviors;
  }

  private List<IBehavior> CloneBehaviorList(BaseSlide newBaseSlide)
  {
    List<IBehavior> behaviorList = new List<IBehavior>();
    foreach (Behavior behavior1 in this.behaviorList)
    {
      Behavior behavior2 = (Behavior) null;
      switch (behavior1)
      {
        case SetEffect _:
          behavior2 = (Behavior) (behavior1 as SetEffect).Clone(newBaseSlide);
          break;
        case ScaleEffect _:
          behavior2 = (Behavior) (behavior1 as ScaleEffect).Clone(newBaseSlide);
          break;
        case RotationEffect _:
          behavior2 = (Behavior) (behavior1 as RotationEffect).Clone(newBaseSlide);
          break;
        case ColorEffect _:
          behavior2 = (Behavior) (behavior1 as ColorEffect).Clone(newBaseSlide);
          break;
        case MotionEffect _:
          behavior2 = (Behavior) (behavior1 as MotionEffect).Clone(newBaseSlide);
          break;
        case PropertyEffect _:
          behavior2 = (Behavior) (behavior1 as PropertyEffect).Clone(newBaseSlide);
          break;
        case FilterEffect _:
          behavior2 = (Behavior) (behavior1 as FilterEffect).Clone(newBaseSlide);
          break;
        case CommandEffect _:
          behavior2 = (Behavior) (behavior1 as CommandEffect).Clone(newBaseSlide);
          break;
      }
      behaviorList.Add((IBehavior) behavior2);
    }
    return behaviorList;
  }
}
