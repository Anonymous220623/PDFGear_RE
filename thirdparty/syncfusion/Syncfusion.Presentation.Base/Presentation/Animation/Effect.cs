// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.Effect
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Animation.Internal;
using Syncfusion.Presentation.Drawing;
using System;

#nullable disable
namespace Syncfusion.Presentation.Animation;

internal class Effect : IEffect
{
  private Syncfusion.Presentation.Animation.Behaviors behaviors;
  private EffectPresetClassType presetClassType;
  private ISequence sequence;
  private ITiming timing;
  private EffectType type;
  private EffectSubtype effectSubType;
  private int shapeId = -1;
  private int index = -1;
  private Iterate iterate;
  private BuildType buildType;
  private int grpId;

  internal Effect(int indx)
  {
    this.index = indx;
    this.behaviors = new Syncfusion.Presentation.Animation.Behaviors();
    this.timing = (ITiming) new Syncfusion.Presentation.Animation.Timing();
  }

  public IBehaviors Behaviors
  {
    get => (IBehaviors) this.behaviors;
    set => this.behaviors = value as Syncfusion.Presentation.Animation.Behaviors;
  }

  public EffectPresetClassType PresetClassType
  {
    get => this.presetClassType;
    set
    {
      if (this.presetClassType == EffectPresetClassType.Entrance && value == EffectPresetClassType.Exit || this.presetClassType == EffectPresetClassType.Exit && value == EffectPresetClassType.Entrance)
      {
        Syncfusion.Presentation.Sequence sequence = this.sequence as Syncfusion.Presentation.Sequence;
        IShape shapeWithId = (IShape) sequence.BaseSlide.GetShapeWithId(sequence.BaseSlide.Shapes as Shapes, this.ShapeID);
        if (value == EffectPresetClassType.Exit && this.type == EffectType.Zoom && (this.effectSubType == EffectSubtype.InCenter || this.effectSubType == EffectSubtype.OutBottom))
          throw new Exception($"{(object) this.effectSubType} subtype is not valid for the {(object) this.type} effect");
        if (value == EffectPresetClassType.Exit && this.type == EffectType.Stretch)
          this.effectSubType = EffectSubtype.Across;
        sequence.AddAnimation(shapeWithId, this.type, this.effectSubType, this.Timing.TriggerType, value, -1, (IEffect) this);
      }
      else
        this.presetClassType = this.presetClassType == value || value == EffectPresetClassType.None ? value : throw new Exception($"{(object) value} is not a valid class type for the effect {(object) this.type}");
    }
  }

  public ISequence Sequence
  {
    get => this.sequence;
    set => this.sequence = value;
  }

  public EffectSubtype Subtype
  {
    get => this.effectSubType;
    set
    {
      if (this.effectSubType == value)
        return;
      Syncfusion.Presentation.Sequence sequence = this.sequence as Syncfusion.Presentation.Sequence;
      IShape shapeWithId = (IShape) sequence.BaseSlide.GetShapeWithId(sequence.BaseSlide.Shapes as Shapes, this.ShapeID);
      sequence.AddEffect(shapeWithId, this.type, value, this.timing.TriggerType, this.presetClassType, (IEffect) this);
    }
  }

  public ITiming Timing
  {
    get => this.timing;
    set => this.timing = value;
  }

  public EffectType Type
  {
    get => this.type;
    set
    {
      if (this.type == value)
        return;
      Syncfusion.Presentation.Sequence sequence = this.sequence as Syncfusion.Presentation.Sequence;
      IShape shapeWithId = (IShape) sequence.BaseSlide.GetShapeWithId(sequence.BaseSlide.Shapes as Shapes, this.ShapeID);
      EffectPresetClassType effectPresetClassType = AnimationConstant.GetEffectPresetClassType(value);
      sequence.AddEffect(shapeWithId, value, EffectSubtype.None, this.timing.TriggerType, effectPresetClassType, (IEffect) this);
    }
  }

  public BuildType BuildType
  {
    get
    {
      if (this.buildType == BuildType.AsOneObject)
        this.UpdateBuildType();
      return this.buildType;
    }
    internal set => this.buildType = value;
  }

  internal int ShapeID
  {
    get
    {
      this.UpdateShapeID();
      return this.shapeId;
    }
    set => this.shapeId = value;
  }

  internal int GroupID
  {
    get => this.grpId;
    set => this.grpId = value;
  }

  internal Iterate Iterate
  {
    get => this.iterate;
    set => this.iterate = value;
  }

  internal void SetPresetClassType(EffectPresetClassType presetClass)
  {
    this.presetClassType = presetClass;
  }

  internal void SeType(EffectType effectType) => this.type = effectType;

  internal void SetSubType(EffectSubtype subType) => this.effectSubType = subType;

  private void UpdateShapeID()
  {
    if (this.Behaviors.Count <= 0)
      return;
    Behavior behavior = this.Behaviors[0] as Behavior;
    if (behavior.TargetElement == null)
      return;
    TargetElement targetElement = behavior.TargetElement;
    if (targetElement.ShapeTarget == null)
      return;
    this.ShapeID = int.Parse(targetElement.ShapeTarget.ShapeId);
  }

  private void UpdateBuildType()
  {
    foreach (Build build in (this.sequence as Syncfusion.Presentation.Sequence).BaseSlide.Timing.BuildList)
    {
      foreach (object buildElement in build.BuildElements)
      {
        if (buildElement is BuildParagraph)
        {
          BuildParagraph buildParagraph = buildElement as BuildParagraph;
          if (int.Parse(buildParagraph.ShapeId) == this.ShapeID)
          {
            if (buildParagraph.BuildType == ParagraphBuildType.None)
              this.BuildType = BuildType.AsOneObject;
            else if (buildParagraph.BuildType == ParagraphBuildType.AllAtOnce)
              this.BuildType = BuildType.AllParagraphsAtOnce;
            else if (buildParagraph.BuildType == ParagraphBuildType.Paragraph)
            {
              switch (buildParagraph.BuildLevel)
              {
                case 0:
                case 1:
                  this.BuildType = BuildType.ByLevelParagraphs1;
                  continue;
                case 2:
                  this.BuildType = BuildType.ByLevelParagraphs2;
                  continue;
                case 3:
                  this.BuildType = BuildType.ByLevelParagraphs3;
                  continue;
                case 4:
                  this.BuildType = BuildType.ByLevelParagraphs4;
                  continue;
                case 5:
                  this.BuildType = BuildType.ByLevelParagraphs5;
                  continue;
                default:
                  continue;
              }
            }
          }
        }
      }
    }
  }

  internal Effect Clone(Syncfusion.Presentation.Sequence newSequence)
  {
    Effect effect = (Effect) this.MemberwiseClone();
    effect.sequence = (ISequence) newSequence;
    effect.timing = (ITiming) (this.timing as Syncfusion.Presentation.Animation.Timing).Clone();
    if (this.iterate != null)
      effect.iterate = this.iterate.Clone();
    effect.behaviors = this.behaviors.Clone(newSequence.BaseSlide);
    return effect;
  }
}
