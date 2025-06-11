// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.Internal.CommonTimeNode
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.SlideImplementation;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.Animation.Internal;

internal class CommonTimeNode
{
  private BaseSlide baseSlide;
  private int acceleration;
  private bool afterEffect;
  private bool autoReverse;
  private int buildLevel;
  private List<object> childTimeNodeList;
  private int deceleration;
  private bool display;
  private string duration;
  private List<Condition> endConditionList;
  private string eventFilter;
  private TimeNodeFill fill;
  private float groupId = float.NaN;
  private uint id;
  private bool nodePlaceholder;
  private TimeNodeType nodeType;
  private EffectPresetClassType presetClass;
  private float presetId = float.NaN;
  private float presetSubtype = float.NaN;
  private string repeatCount;
  private string repeatDuration;
  private EffectRestartType restart;
  private int speed;
  private List<Condition> startConditionList;
  private List<object> subTimeNodeList;
  private TimeNodeSync syncBehavior;
  private string timeFilter;
  private Condition endSync;
  private Iterate iterate;

  internal BaseSlide BaseSlide
  {
    get => this.baseSlide;
    set => this.baseSlide = value;
  }

  internal int Acceleration
  {
    get => this.acceleration;
    set => this.acceleration = value;
  }

  internal bool AfterEffect
  {
    get => this.afterEffect;
    set => this.afterEffect = value;
  }

  internal bool AutoReverse
  {
    get => this.autoReverse;
    set => this.autoReverse = value;
  }

  internal int BuildLevel
  {
    get => this.buildLevel;
    set => this.buildLevel = value;
  }

  internal List<object> ChildTimeNodeList
  {
    get => this.childTimeNodeList;
    set => this.childTimeNodeList = value;
  }

  internal int Deceleration
  {
    get => this.deceleration;
    set => this.deceleration = value;
  }

  internal bool Display
  {
    get => this.display;
    set => this.display = value;
  }

  internal string Duration
  {
    get => this.duration;
    set => this.duration = value;
  }

  internal List<Condition> EndConditionList
  {
    get => this.endConditionList;
    set => this.endConditionList = value;
  }

  internal string EventFilter
  {
    get => this.eventFilter;
    set => this.eventFilter = value;
  }

  internal TimeNodeFill Fill
  {
    get => this.fill;
    set => this.fill = value;
  }

  internal float GroupId
  {
    get => this.groupId;
    set => this.groupId = value;
  }

  internal uint Id
  {
    get => this.id;
    set => this.id = value;
  }

  internal bool NodePlaceholder
  {
    get => this.nodePlaceholder;
    set => this.nodePlaceholder = value;
  }

  internal TimeNodeType NodeType
  {
    get => this.nodeType;
    set => this.nodeType = value;
  }

  internal EffectPresetClassType PresetClass
  {
    get => this.presetClass;
    set => this.presetClass = value;
  }

  internal float PresetId
  {
    get => this.presetId;
    set => this.presetId = value;
  }

  internal float PresetSubtype
  {
    get => this.presetSubtype;
    set => this.presetSubtype = value;
  }

  internal string RepeatCount
  {
    get => this.repeatCount;
    set => this.repeatCount = value;
  }

  internal string RepeatDuration
  {
    get => this.repeatDuration;
    set => this.repeatDuration = value;
  }

  internal EffectRestartType Restart
  {
    get => this.restart;
    set => this.restart = value;
  }

  internal int Speed
  {
    get => this.speed;
    set => this.speed = value;
  }

  internal List<Condition> StartConditionList
  {
    get => this.startConditionList;
    set => this.startConditionList = value;
  }

  internal List<object> SubTimeNodeList
  {
    get => this.subTimeNodeList;
    set => this.subTimeNodeList = value;
  }

  internal TimeNodeSync SyncBehavior
  {
    get => this.syncBehavior;
    set => this.syncBehavior = value;
  }

  internal string TimeFilter
  {
    get => this.timeFilter;
    set => this.timeFilter = value;
  }

  internal Condition EndSync
  {
    get => this.endSync;
    set => this.endSync = value;
  }

  internal Iterate Iterate
  {
    get => this.iterate;
    set => this.iterate = value;
  }

  internal CommonTimeNode Clone(BaseSlide newBaseSlide)
  {
    CommonTimeNode commonTimeNode = (CommonTimeNode) this.MemberwiseClone();
    commonTimeNode.baseSlide = newBaseSlide;
    if (this.childTimeNodeList != null)
      commonTimeNode.childTimeNodeList = this.CloneTimeNodeList(newBaseSlide);
    if (this.endConditionList != null)
      commonTimeNode.endConditionList = this.CloneConditionList(this.endConditionList);
    if (this.startConditionList != null)
      commonTimeNode.startConditionList = this.CloneConditionList(this.startConditionList);
    if (this.endSync != null)
      commonTimeNode.endSync = this.endSync.Clone();
    return commonTimeNode;
  }

  private List<object> CloneTimeNodeList(BaseSlide newBaseSlide)
  {
    List<object> objectList = new List<object>();
    foreach (object childTimeNode in this.childTimeNodeList)
    {
      object obj = (object) null;
      switch (childTimeNode)
      {
        case SequenceTimeNode _:
          obj = (object) (childTimeNode as SequenceTimeNode).Clone(newBaseSlide);
          break;
        case ParallelTimeNode _:
          obj = (object) (childTimeNode as ParallelTimeNode).Clone(newBaseSlide);
          break;
        case PropertyEffect _:
          obj = (object) (childTimeNode as PropertyEffect).Clone(newBaseSlide);
          break;
        case ColorEffect _:
          obj = (object) (childTimeNode as ColorEffect).Clone(newBaseSlide);
          break;
        case FilterEffect _:
          obj = (object) (childTimeNode as FilterEffect).Clone(newBaseSlide);
          break;
        case MotionEffect _:
          obj = (object) (childTimeNode as MotionEffect).Clone(newBaseSlide);
          break;
        case RotationEffect _:
          obj = (object) (childTimeNode as RotationEffect).Clone(newBaseSlide);
          break;
        case ScaleEffect _:
          obj = (object) (childTimeNode as ScaleEffect).Clone(newBaseSlide);
          break;
        case CommandEffect _:
          obj = (object) (childTimeNode as CommandEffect).Clone(newBaseSlide);
          break;
        case SetEffect _:
          obj = (object) (childTimeNode as SetEffect).Clone(newBaseSlide);
          break;
      }
      objectList.Add(obj);
    }
    return objectList;
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
