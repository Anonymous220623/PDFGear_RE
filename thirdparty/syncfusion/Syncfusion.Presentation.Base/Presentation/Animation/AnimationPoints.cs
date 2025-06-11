// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.AnimationPoints
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.Animation;

internal class AnimationPoints : IAnimationPoints, IEnumerable<IAnimationPoint>, IEnumerable
{
  private List<IAnimationPoint> pointsList = new List<IAnimationPoint>();

  public IAnimationPoint this[int index]
  {
    get
    {
      if (this.pointsList.Count <= index)
        throw new IndexOutOfRangeException("Index was out of range, value should be less than Points count");
      return this.pointsList[index];
    }
  }

  public int Count => this.pointsList.Count;

  public IEnumerator<IAnimationPoint> GetEnumerator()
  {
    return (IEnumerator<IAnimationPoint>) this.pointsList.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.pointsList.GetEnumerator();

  internal void Add(AnimationPoint point) => this.pointsList.Add((IAnimationPoint) point);

  internal void Clear() => this.pointsList.Clear();

  internal AnimationPoints Clone()
  {
    AnimationPoints animationPoints = (AnimationPoints) this.MemberwiseClone();
    animationPoints.pointsList = this.ClonePointsList();
    return animationPoints;
  }

  private List<IAnimationPoint> ClonePointsList()
  {
    List<IAnimationPoint> animationPointList = new List<IAnimationPoint>();
    foreach (AnimationPoint points in this.pointsList)
    {
      AnimationPoint animationPoint = points.Clone();
      animationPointList.Add((IAnimationPoint) animationPoint);
    }
    return animationPointList;
  }
}
