// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.Internal.SequenceTimeNode
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.SlideImplementation;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.Animation.Internal;

internal class SequenceTimeNode
{
  private CommonTimeNode commonTimeNode;
  private List<Condition> nextConditionList;
  private List<Condition> previousConditionList;
  private bool concurrent;
  private NextAction nextAction;
  private PreviousAction previousAction;

  internal CommonTimeNode CommonTimeNode
  {
    get => this.commonTimeNode;
    set => this.commonTimeNode = value;
  }

  internal List<Condition> NextConditionList
  {
    get => this.nextConditionList;
    set => this.nextConditionList = value;
  }

  internal List<Condition> PreviousConditionList
  {
    get => this.previousConditionList;
    set => this.previousConditionList = value;
  }

  internal bool Concurrent
  {
    get => this.concurrent;
    set => this.concurrent = value;
  }

  internal NextAction NextAction
  {
    get => this.nextAction;
    set => this.nextAction = value;
  }

  internal PreviousAction PreviousAction
  {
    get => this.previousAction;
    set => this.previousAction = value;
  }

  internal SequenceTimeNode Clone(BaseSlide newBaseSlide)
  {
    SequenceTimeNode sequenceTimeNode = (SequenceTimeNode) this.MemberwiseClone();
    if (this.commonTimeNode != null)
      sequenceTimeNode.commonTimeNode = this.commonTimeNode.Clone(newBaseSlide);
    if (this.nextConditionList != null)
      sequenceTimeNode.nextConditionList = this.CloneConditionList(this.nextConditionList);
    if (this.previousConditionList != null)
      sequenceTimeNode.previousConditionList = this.CloneConditionList(this.nextConditionList);
    return sequenceTimeNode;
  }

  private List<Condition> CloneConditionList(List<Condition> list)
  {
    List<Condition> conditionList = new List<Condition>();
    foreach (Condition condition1 in list)
    {
      Condition condition2 = condition1.Clone();
      conditionList.Add(condition2);
    }
    return conditionList;
  }
}
