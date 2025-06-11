// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.PropertyEffect
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Animation;
using Syncfusion.Presentation.Animation.Internal;
using Syncfusion.Presentation.SlideImplementation;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation;

internal class PropertyEffect : Behavior, IPropertyEffect, IBehavior
{
  private string by;
  private PropertyCalcModeType calcMode;
  private string from;
  private IAnimationPoints points;
  private string to;
  private PropertyValueType valueType;
  private List<TimeAnimateValue> tAVList;

  public string By
  {
    get => this.by;
    set => this.by = value;
  }

  public PropertyCalcModeType CalcMode
  {
    get => this.calcMode;
    set => this.calcMode = value;
  }

  public string From
  {
    get => this.from;
    set => this.from = value;
  }

  public IAnimationPoints Points
  {
    get => this.points;
    set => this.points = value;
  }

  public string To
  {
    get => this.to;
    set => this.to = value;
  }

  public PropertyValueType ValueType
  {
    get => this.valueType;
    set => this.valueType = value;
  }

  internal List<TimeAnimateValue> TAVList
  {
    get => this.tAVList;
    set => this.tAVList = value;
  }

  internal PropertyEffect Clone(BaseSlide newBaseSlide)
  {
    PropertyEffect newBehavior = (PropertyEffect) this.MemberwiseClone();
    if (this.points != null)
      newBehavior.points = (IAnimationPoints) (this.points as AnimationPoints).Clone();
    if (this.tAVList != null)
      newBehavior.tAVList = this.CloneTAVList();
    this.CloneBehavior((Behavior) newBehavior, newBaseSlide);
    return newBehavior;
  }

  private List<TimeAnimateValue> CloneTAVList()
  {
    List<TimeAnimateValue> timeAnimateValueList = new List<TimeAnimateValue>();
    foreach (TimeAnimateValue tAv in this.tAVList)
    {
      TimeAnimateValue timeAnimateValue = tAv.Clone();
      timeAnimateValueList.Add(timeAnimateValue);
    }
    return timeAnimateValueList;
  }
}
