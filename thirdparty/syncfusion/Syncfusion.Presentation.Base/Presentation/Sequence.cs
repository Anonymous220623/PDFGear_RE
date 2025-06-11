// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Sequence
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Animation;
using Syncfusion.Presentation.Animation.Internal;
using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.Resource;
using Syncfusion.Presentation.SlideImplementation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace Syncfusion.Presentation;

internal class Sequence : ISequence, IEnumerable<IEffect>, IEnumerable
{
  private IShape shape;
  private List<IEffect> effectList = new List<IEffect>();
  private BaseSlide baseSlide;

  internal Sequence(BaseSlide slide) => this.baseSlide = slide;

  public IEffect this[int index]
  {
    get
    {
      if (this.effectList.Count <= index)
        throw new IndexOutOfRangeException("Index was out of range,value should be greater than effect list count");
      return this.effectList[index];
    }
    internal set => this.effectList[index] = value;
  }

  public int Count => this.effectList.Count;

  public IShape TriggerShape
  {
    get => this.shape;
    set => this.shape = value;
  }

  public IEffect AddEffect(
    IShape shape,
    EffectType effectType,
    EffectSubtype subtype,
    EffectTriggerType triggerType)
  {
    EffectPresetClassType presetClassType = effectType != EffectType.Custom ? AnimationConstant.GetEffectPresetClassType(effectType) : throw new Exception($"The EffectType {(object) effectType} is a not valid one");
    if (presetClassType == EffectPresetClassType.Path)
      subtype = EffectSubtype.None;
    else if (subtype == EffectSubtype.None || AnimationConstant.IsValidSubtype(effectType, subtype))
    {
      if (subtype == EffectSubtype.None)
        subtype = AnimationConstant.GetSubTypeWhileNone(effectType);
    }
    else
      throw new Exception($"{(object) subtype} subtype is not valid for the {(object) effectType} effect");
    return this.AddAnimation(shape, effectType, subtype, triggerType, presetClassType, -1, (IEffect) null);
  }

  public IEffect AddEffect(
    IShape shape,
    EffectType effectType,
    EffectSubtype subtype,
    EffectTriggerType triggerType,
    BuildType bldType)
  {
    EffectPresetClassType effectPresetClassType = AnimationConstant.GetEffectPresetClassType(effectType);
    if (effectPresetClassType == EffectPresetClassType.Path)
      subtype = EffectSubtype.None;
    else if (subtype == EffectSubtype.None || AnimationConstant.IsValidSubtype(effectType, subtype))
    {
      if (subtype == EffectSubtype.None)
        subtype = AnimationConstant.GetSubTypeWhileNone(effectType);
    }
    else
      throw new Exception($"{(object) subtype} subtype is not valid for the {(object) effectType} effect");
    IEffect effect = this.AddAnimation(shape, effectType, subtype, triggerType, effectPresetClassType, -1, (IEffect) null);
    this.AddEffectWithBuildType(shape, effectType, subtype, triggerType, bldType, effectPresetClassType, effect);
    return effect;
  }

  public void Clear()
  {
    if ((this.baseSlide.Timeline as Timeline).GetInteractiveSequences().Count == 0)
      this.baseSlide.IsAnimated = false;
    this.effectList.Clear();
  }

  public int GetCount(IShape shape)
  {
    List<IEffect> effectList = new List<IEffect>();
    int shapeId = (shape as Shape).ShapeId;
    foreach (Effect effect in this.effectList)
    {
      if (effect.ShapeID == shapeId)
        effectList.Add((IEffect) effect);
    }
    return effectList.Count;
  }

  public IEffect[] GetEffectsByShape(IShape shape)
  {
    List<IEffect> effectList = new List<IEffect>();
    int shapeId = (shape as Shape).ShapeId;
    foreach (Effect effect in this.effectList)
    {
      if (effect.ShapeID == shapeId)
        effectList.Add((IEffect) effect);
    }
    return effectList.ToArray();
  }

  public void Remove(IEffect item)
  {
    if (item == null)
      return;
    this.effectList.Remove(item);
  }

  public void RemoveAt(int index)
  {
    if (index >= this.effectList.Count)
      throw new Exception("Index must be less than effects count");
    this.effectList.RemoveAt(index);
  }

  public void RemoveByShape(IShape shape)
  {
    int shapeId = (shape as Shape).ShapeId;
    foreach (Effect effect in new List<IEffect>((IEnumerable<IEffect>) this.effectList))
    {
      if (effect.ShapeID == shapeId)
        this.effectList.Remove((IEffect) effect);
    }
  }

  public void Insert(int index, IEffect effect)
  {
    if (index > this.effectList.Count)
      throw new Exception("Index must be less than or equal to Effects count");
    effect = (IEffect) (effect as Effect).Clone(this);
    this.effectList.Insert(index, effect);
  }

  public IEnumerator<IEffect> GetEnumerator()
  {
    return (IEnumerator<IEffect>) this.effectList.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.effectList.GetEnumerator();

  internal void AddEffect(IEffect effect) => this.effectList.Add(effect);

  internal IEffect AddEffect(
    IShape shape,
    EffectType effectType,
    EffectSubtype subtype,
    EffectTriggerType triggerType,
    EffectPresetClassType presetClass,
    IEffect effect)
  {
    if (presetClass == EffectPresetClassType.Path)
      subtype = EffectSubtype.None;
    else if (subtype == EffectSubtype.None || AnimationConstant.IsValidSubtype(effectType, subtype))
    {
      if (subtype == EffectSubtype.None)
        subtype = AnimationConstant.GetSubTypeWhileNone(effectType);
    }
    else
      throw new Exception($"{(object) subtype} subtype is not valid for the {(object) effectType} effect");
    effect = this.AddAnimation(shape, effectType, subtype, triggerType, presetClass, -1, effect);
    return effect;
  }

  internal List<IEffect> GetEffectList() => this.effectList;

  internal BaseSlide BaseSlide => this.baseSlide;

  internal IEffect AddAnimation(
    IShape shape,
    EffectType effectType,
    EffectSubtype subtype,
    EffectTriggerType triggerType,
    EffectPresetClassType presetClassType,
    int paraRange,
    IEffect effect)
  {
    BaseSlide baseSlide = (shape as Shape).BaseSlide;
    foreach (object obj in AnimParser.GetAnimationInternalDOM(XmlReader.Create(ResourceManager.GetStreamFromResource("AnimationEffects", ".xml")), presetClassType, effectType, subtype, baseSlide))
    {
      if (obj is ParallelTimeNode)
      {
        AnimationConstant.UpdateEffects((obj as ParallelTimeNode).CommonTimeNode, this, effect as Effect);
        if (effect == null)
          effect = this[this.Count - 1];
        this.ReplaceCurrentAnimShape(shape as Shape, triggerType, effect, paraRange);
      }
    }
    baseSlide.IsAnimated = true;
    return effect;
  }

  private void ReplaceCurrentAnimShape(
    Shape shape,
    EffectTriggerType triggerType,
    IEffect effect,
    int paraRange)
  {
    effect.Timing.TriggerType = triggerType;
    foreach (Behavior behavior in (IEnumerable<IBehavior>) effect.Behaviors)
    {
      if (behavior.CommonBehavior.Target != null)
      {
        ShapeTarget shapeTarget = behavior.CommonBehavior.Target.ShapeTarget;
        if (shapeTarget != null)
        {
          shapeTarget.ShapeId = shape.ShapeId.ToString();
          (effect as Effect).ShapeID = shape.ShapeId;
          if (paraRange >= 0)
          {
            shapeTarget.TextElement = new TextElement();
            shapeTarget.TextElement.ParagraphRange = new RangeValues();
            shapeTarget.TextElement.ParagraphRange.Start = (uint) paraRange;
            shapeTarget.TextElement.ParagraphRange.End = (uint) paraRange;
          }
        }
      }
    }
  }

  private void AddEffectWithBuildType(
    IShape shape,
    EffectType effectType,
    EffectSubtype subtype,
    EffectTriggerType triggerType,
    BuildType bldType,
    EffectPresetClassType presetClass,
    IEffect effect)
  {
    IEffect effect1;
    switch (bldType)
    {
      case BuildType.AllParagraphsAtOnce:
        (effect as Effect).BuildType = BuildType.AllParagraphsAtOnce;
        IParagraphs paragraphs1 = shape.TextBody.Paragraphs;
        for (int index = 0; index < paragraphs1.Count; ++index)
        {
          if (string.IsNullOrEmpty(paragraphs1[index].Text))
            (this.AddAnimation(shape, effectType, subtype, EffectTriggerType.WithPrevious, presetClass, index, (IEffect) null) as Effect).BuildType = BuildType.AllParagraphsAtOnce;
        }
        break;
      case BuildType.ByLevelParagraphs1:
        (effect as Effect).BuildType = BuildType.ByLevelParagraphs1;
        IParagraphs paragraphs2 = shape.TextBody.Paragraphs;
        effect1 = (IEffect) null;
        for (int index = 0; index < paragraphs2.Count; ++index)
        {
          if (!string.IsNullOrEmpty(paragraphs2[index].Text))
            ((paragraphs2[index].IndentLevelNumber >= 1 ? this.AddAnimation(shape, effectType, subtype, EffectTriggerType.WithPrevious, presetClass, index, (IEffect) null) : this.AddAnimation(shape, effectType, subtype, EffectTriggerType.OnClick, presetClass, index, (IEffect) null)) as Effect).BuildType = BuildType.ByLevelParagraphs1;
        }
        break;
      case BuildType.ByLevelParagraphs2:
        (effect as Effect).BuildType = BuildType.ByLevelParagraphs2;
        IParagraphs paragraphs3 = shape.TextBody.Paragraphs;
        effect1 = (IEffect) null;
        for (int index = 0; index < paragraphs3.Count; ++index)
        {
          if (!string.IsNullOrEmpty(paragraphs3[index].Text))
            ((paragraphs3[index].IndentLevelNumber >= 2 ? this.AddAnimation(shape, effectType, subtype, EffectTriggerType.WithPrevious, presetClass, index, (IEffect) null) : this.AddAnimation(shape, effectType, subtype, EffectTriggerType.OnClick, presetClass, index, (IEffect) null)) as Effect).BuildType = BuildType.ByLevelParagraphs2;
        }
        break;
      case BuildType.ByLevelParagraphs3:
        (effect as Effect).BuildType = BuildType.ByLevelParagraphs3;
        IParagraphs paragraphs4 = shape.TextBody.Paragraphs;
        effect1 = (IEffect) null;
        for (int index = 0; index < paragraphs4.Count; ++index)
        {
          if (!string.IsNullOrEmpty(paragraphs4[index].Text))
            ((paragraphs4[index].IndentLevelNumber >= 3 ? this.AddAnimation(shape, effectType, subtype, EffectTriggerType.WithPrevious, presetClass, index, (IEffect) null) : this.AddAnimation(shape, effectType, subtype, EffectTriggerType.OnClick, presetClass, index, (IEffect) null)) as Effect).BuildType = BuildType.ByLevelParagraphs3;
        }
        break;
      case BuildType.ByLevelParagraphs4:
        (effect as Effect).BuildType = BuildType.ByLevelParagraphs4;
        IParagraphs paragraphs5 = shape.TextBody.Paragraphs;
        effect1 = (IEffect) null;
        for (int index = 0; index < paragraphs5.Count; ++index)
        {
          if (!string.IsNullOrEmpty(paragraphs5[index].Text))
            ((paragraphs5[index].IndentLevelNumber >= 4 ? this.AddAnimation(shape, effectType, subtype, EffectTriggerType.WithPrevious, presetClass, index, (IEffect) null) : this.AddAnimation(shape, effectType, subtype, EffectTriggerType.OnClick, presetClass, index, (IEffect) null)) as Effect).BuildType = BuildType.ByLevelParagraphs4;
        }
        break;
      case BuildType.ByLevelParagraphs5:
        (effect as Effect).BuildType = BuildType.ByLevelParagraphs5;
        IParagraphs paragraphs6 = shape.TextBody.Paragraphs;
        effect1 = (IEffect) null;
        for (int index = 0; index < paragraphs6.Count; ++index)
        {
          if (!string.IsNullOrEmpty(paragraphs6[index].Text))
            ((paragraphs6[index].IndentLevelNumber >= 5 ? this.AddAnimation(shape, effectType, subtype, EffectTriggerType.WithPrevious, presetClass, index, (IEffect) null) : this.AddAnimation(shape, effectType, subtype, EffectTriggerType.OnClick, presetClass, index, (IEffect) null)) as Effect).BuildType = BuildType.ByLevelParagraphs5;
        }
        break;
    }
  }

  internal Sequence Clone(BaseSlide newBaseSlide)
  {
    Sequence newSequence = (Sequence) this.MemberwiseClone();
    newSequence.baseSlide = newBaseSlide;
    if (this.shape != null)
      newSequence.TriggerShape = (IShape) (this.shape.Clone() as Shape);
    newSequence.effectList = this.CloneEffectList(newSequence);
    return newSequence;
  }

  private List<IEffect> CloneEffectList(Sequence newSequence)
  {
    List<IEffect> effectList = new List<IEffect>();
    foreach (Effect effect1 in this.effectList)
    {
      IEffect effect2 = (IEffect) effect1.Clone(newSequence);
      effectList.Add(effect2);
    }
    return effectList;
  }
}
