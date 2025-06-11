// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.Internal.InternaTimeLine
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.SlideImplementation;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.Animation.Internal;

internal class InternaTimeLine
{
  private BaseSlide _baseSlide;
  private List<object> timeNodeList;
  private List<Build> buildList;

  internal InternaTimeLine(BaseSlide baseSlide)
  {
    this._baseSlide = baseSlide;
    this.timeNodeList = new List<object>();
    this.buildList = new List<Build>();
  }

  internal List<object> TimeNodeList
  {
    get => this.timeNodeList;
    set => this.timeNodeList = value;
  }

  internal List<Build> BuildList
  {
    get => this.buildList;
    set => this.buildList = value;
  }

  internal void Update()
  {
    this.UpdateBuildList();
    uint cTnId = 0;
    CommonTimeNode rootElements = AnimationConstant.CreateRootElements(this.timeNodeList, ref cTnId);
    if ((this._baseSlide.Timeline as Timeline).GetMainSequence().Count > 0)
      AnimationConstant.UpdateMainSequence(rootElements, ref cTnId, this._baseSlide);
    if ((this._baseSlide.Timeline as Timeline).GetInteractiveSequences().Count <= 0)
      return;
    AnimationConstant.UpdateInteractiveSequences(rootElements, ref cTnId, this._baseSlide);
  }

  private void UpdateBuildList()
  {
    List<Build> buildList = new List<Build>();
    Sequence mainSequence = (this._baseSlide.Timeline as Timeline).GetMainSequence() as Sequence;
    Sequences interactiveSequences = (this._baseSlide.Timeline as Timeline).GetInteractiveSequences() as Sequences;
    foreach (Shape shape in (IEnumerable<ISlideItem>) this._baseSlide.Shapes)
    {
      int grpId = -1;
      this.AddBuildList(buildList, mainSequence, ref grpId, shape);
      foreach (Sequence sequence in interactiveSequences)
        this.AddBuildList(buildList, sequence, ref grpId, shape);
    }
    this._baseSlide.Timing.BuildList = buildList;
  }

  private void AddBuildList(List<Build> buildList, Sequence sequence, ref int grpId, Shape shape)
  {
    foreach (Effect effect in sequence.GetEffectsByShape((IShape) shape))
    {
      effect.GroupID = ++grpId;
      BuildType buildType = effect.BuildType;
      Build build = new Build();
      build.BuildElements = new List<object>();
      BuildParagraph buildParagraph = new BuildParagraph();
      buildParagraph.AnimateBackground = true;
      buildParagraph.ShapeId = shape.ShapeId.ToString();
      buildParagraph.GroupId = (uint) grpId;
      if (buildType == BuildType.AsOneObject)
      {
        buildParagraph.BuildType = ParagraphBuildType.None;
        build.BuildElements.Add((object) buildParagraph);
        buildList.Add(build);
      }
      else if (this.IsNewBuild(buildList, shape.ShapeId, buildType))
      {
        switch (buildType)
        {
          case BuildType.AllParagraphsAtOnce:
            buildParagraph.BuildType = ParagraphBuildType.AllAtOnce;
            break;
          case BuildType.ByLevelParagraphs1:
            buildParagraph.BuildType = ParagraphBuildType.Paragraph;
            break;
          case BuildType.ByLevelParagraphs2:
            buildParagraph.BuildType = ParagraphBuildType.Paragraph;
            buildParagraph.BuildLevel = 2U;
            break;
          case BuildType.ByLevelParagraphs3:
            buildParagraph.BuildType = ParagraphBuildType.Paragraph;
            buildParagraph.BuildLevel = 3U;
            break;
          case BuildType.ByLevelParagraphs4:
            buildParagraph.BuildType = ParagraphBuildType.Paragraph;
            buildParagraph.BuildLevel = 4U;
            break;
          case BuildType.ByLevelParagraphs5:
            buildParagraph.BuildType = ParagraphBuildType.Paragraph;
            buildParagraph.BuildLevel = 5U;
            break;
        }
        build.BuildElements.Add((object) buildParagraph);
        buildList.Add(build);
      }
    }
  }

  private bool IsNewBuild(List<Build> buildList, int shapeId, BuildType buildType)
  {
    foreach (Build build in buildList)
    {
      foreach (object buildElement in build.BuildElements)
      {
        if (buildElement is BuildParagraph && (buildElement as BuildParagraph).ShapeId == shapeId.ToString() && ((buildElement as BuildParagraph).BuildType == ParagraphBuildType.AllAtOnce && buildType == BuildType.AllParagraphsAtOnce || (buildElement as BuildParagraph).BuildType == ParagraphBuildType.Paragraph))
          return false;
      }
    }
    return true;
  }

  internal InternaTimeLine Clone(BaseSlide newBaseSlide)
  {
    InternaTimeLine internaTimeLine = (InternaTimeLine) this.MemberwiseClone();
    internaTimeLine._baseSlide = newBaseSlide;
    if (internaTimeLine.timeNodeList != null)
      internaTimeLine.timeNodeList = this.CloneTimeNodeList(newBaseSlide);
    if (internaTimeLine.buildList != null)
      internaTimeLine.buildList = this.CloneBuildList();
    return internaTimeLine;
  }

  private List<object> CloneTimeNodeList(BaseSlide newBaseSlide)
  {
    List<object> objectList = new List<object>();
    foreach (object timeNode in this.timeNodeList)
    {
      object obj = (object) null;
      switch (timeNode)
      {
        case SequenceTimeNode _:
          obj = (object) (timeNode as SequenceTimeNode).Clone(newBaseSlide);
          break;
        case ParallelTimeNode _:
          obj = (object) (timeNode as ParallelTimeNode).Clone(newBaseSlide);
          break;
        case PropertyEffect _:
          obj = (object) (timeNode as PropertyEffect).Clone(newBaseSlide);
          break;
        case ColorEffect _:
          obj = (object) (timeNode as ColorEffect).Clone(newBaseSlide);
          break;
        case FilterEffect _:
          obj = (object) (timeNode as FilterEffect).Clone(newBaseSlide);
          break;
        case MotionEffect _:
          obj = (object) (timeNode as MotionEffect).Clone(newBaseSlide);
          break;
        case RotationEffect _:
          obj = (object) (timeNode as RotationEffect).Clone(newBaseSlide);
          break;
        case ScaleEffect _:
          obj = (object) (timeNode as ScaleEffect).Clone(newBaseSlide);
          break;
        case CommandEffect _:
          obj = (object) (timeNode as CommandEffect).Clone(newBaseSlide);
          break;
        case SetEffect _:
          obj = (object) (timeNode as SetEffect).Clone(newBaseSlide);
          break;
      }
      objectList.Add(obj);
    }
    return objectList;
  }

  private List<Build> CloneBuildList()
  {
    List<Build> buildList = new List<Build>();
    foreach (Build build1 in this.buildList)
    {
      Build build2 = build1.CloneBuild();
      buildList.Add(build2);
    }
    return buildList;
  }
}
