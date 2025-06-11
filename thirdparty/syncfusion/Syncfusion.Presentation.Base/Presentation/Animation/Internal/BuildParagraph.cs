// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.Internal.BuildParagraph
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.Animation.Internal;

internal class BuildParagraph : Build
{
  private bool animateBackground;
  private uint groupId;
  private string shapeId;
  private List<TemplateEffects> templateList;
  private int autoAdvanceTime;
  private bool autoUpdateAnimBackground;
  private uint buildLevel;
  private ParagraphBuildType buildType;
  private bool reverse;
  private bool expandUI;

  internal bool AnimateBackground
  {
    get => this.animateBackground;
    set => this.animateBackground = value;
  }

  internal uint GroupId
  {
    get => this.groupId;
    set => this.groupId = value;
  }

  internal string ShapeId
  {
    get => this.shapeId;
    set => this.shapeId = value;
  }

  internal List<TemplateEffects> TemplateList
  {
    get => this.templateList;
    set => this.templateList = value;
  }

  internal int AutoAdvanceTime
  {
    get => this.autoAdvanceTime;
    set => this.autoAdvanceTime = value;
  }

  internal bool AutoUpdateAnimBackground
  {
    get => this.autoUpdateAnimBackground;
    set => this.autoUpdateAnimBackground = value;
  }

  internal uint BuildLevel
  {
    get => this.buildLevel;
    set => this.buildLevel = value;
  }

  internal ParagraphBuildType BuildType
  {
    get => this.buildType;
    set => this.buildType = value;
  }

  internal bool Reverse
  {
    get => this.reverse;
    set => this.reverse = value;
  }

  internal bool ExpandUI
  {
    get => this.expandUI;
    set => this.expandUI = value;
  }

  internal BuildParagraph Clone()
  {
    BuildParagraph buildParagraph = (BuildParagraph) this.MemberwiseClone();
    if (this.templateList != null)
      buildParagraph.templateList = this.CloneTemplateList();
    return buildParagraph;
  }

  private List<TemplateEffects> CloneTemplateList()
  {
    List<TemplateEffects> templateEffectsList = new List<TemplateEffects>();
    foreach (TemplateEffects template in this.templateList)
    {
      TemplateEffects templateEffects = template.Clone();
      templateEffectsList.Add(templateEffects);
    }
    return templateEffectsList;
  }
}
