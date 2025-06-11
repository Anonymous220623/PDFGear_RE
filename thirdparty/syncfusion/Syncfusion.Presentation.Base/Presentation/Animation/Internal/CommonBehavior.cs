// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.Internal.CommonBehavior
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.SlideImplementation;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.Animation.Internal;

internal class CommonBehavior
{
  private BehaviorAccumulateType accumulate;
  private BehaviorAdditiveType additive;
  private string by;
  private string from;
  private BehaviourOverrideType overrideType;
  private string runTimeContext;
  private string to;
  private BehaviorTransformType transformType;
  private List<AttributeName> attributeNameList;
  private CommonTimeNode commonTimeNode;
  private TargetElement target;

  internal BehaviorAccumulateType Accumulate
  {
    get => this.accumulate;
    set => this.accumulate = value;
  }

  internal BehaviorAdditiveType Additive
  {
    get => this.additive;
    set => this.additive = value;
  }

  internal string By
  {
    get => this.by;
    set => this.by = value;
  }

  internal string From
  {
    get => this.from;
    set => this.from = value;
  }

  internal BehaviourOverrideType Override
  {
    get => this.overrideType;
    set => this.overrideType = value;
  }

  internal string RunTimeContext
  {
    get => this.runTimeContext;
    set => this.runTimeContext = value;
  }

  internal string To
  {
    get => this.to;
    set => this.to = value;
  }

  internal BehaviorTransformType TransformType
  {
    get => this.transformType;
    set => this.transformType = value;
  }

  internal List<AttributeName> AttributeNameList
  {
    get => this.attributeNameList;
    set => this.attributeNameList = value;
  }

  internal CommonTimeNode CommonTimeNode
  {
    get => this.commonTimeNode;
    set => this.commonTimeNode = value;
  }

  internal TargetElement Target
  {
    get => this.target;
    set => this.target = value;
  }

  internal CommonBehavior Clone(BaseSlide newBaseSlide)
  {
    CommonBehavior commonBehavior = (CommonBehavior) this.MemberwiseClone();
    if (this.attributeNameList != null)
      commonBehavior.attributeNameList = this.CloneAttributeNameList();
    if (this.commonTimeNode != null)
      commonBehavior.commonTimeNode = this.commonTimeNode.Clone(newBaseSlide);
    if (this.target != null)
      commonBehavior.target = this.target.Clone();
    return commonBehavior;
  }

  private List<AttributeName> CloneAttributeNameList()
  {
    List<AttributeName> attributeNameList = new List<AttributeName>();
    foreach (AttributeName attributeName1 in this.attributeNameList)
    {
      AttributeName attributeName2 = attributeName1.Clone();
      attributeNameList.Add(attributeName2);
    }
    return attributeNameList;
  }
}
