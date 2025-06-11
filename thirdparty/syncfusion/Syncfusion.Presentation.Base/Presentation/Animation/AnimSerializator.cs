// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.AnimSerializator
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Animation.Internal;
using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.SlideImplementation;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

#nullable disable
namespace Syncfusion.Presentation.Animation;

internal class AnimSerializator
{
  private const string PPrefix = "p";
  private const string APrefix = "a";

  internal static void SerializeAnimation(XmlWriter xmlWriter, BaseSlide slide)
  {
    if (!slide.IsAnimated)
      return;
    if (slide.Timing.TimeNodeList == null && ((slide.GetSlideTimeLine() as Timeline).GetInteractiveSequences().Count > 0 || (slide.GetSlideTimeLine() as Timeline).GetMainSequence().Count > 0))
    {
      slide.Timing.TimeNodeList = new List<object>();
      slide.Timing.Update();
    }
    if (slide.Timing.TimeNodeList == null)
      return;
    AnimSerializator.SerializeAnimationXML(xmlWriter, slide.Timing);
  }

  private static void SerializeAnimationXML(XmlWriter writer, InternaTimeLine timing)
  {
    writer.WriteStartElement("p", nameof (timing), "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (timing.TimeNodeList.Count > 0)
      AnimSerializator.SerializeTimeNodeList(writer, timing.TimeNodeList);
    if (timing.BuildList.Count > 0)
      AnimSerializator.SerializeBuildList(writer, timing.BuildList);
    writer.WriteEndElement();
  }

  private static void SerializeTimeNodeList(XmlWriter writer, List<object> timeNodeList)
  {
    writer.WriteStartElement("p", "tnLst", "http://schemas.openxmlformats.org/presentationml/2006/main");
    AnimSerializator.SerializeTimeNodePropertiesList(writer, timeNodeList);
    writer.WriteEndElement();
  }

  private static void SerializeTimeNodePropertiesList(XmlWriter writer, List<object> timeNodeList)
  {
    foreach (object timeNode in timeNodeList)
    {
      switch (timeNode)
      {
        case ParallelTimeNode _:
          AnimSerializator.SerializeParallelTimeNode(writer, (ParallelTimeNode) timeNode);
          continue;
        case SequenceTimeNode _:
          AnimSerializator.SerializeSequenceTimeNode(writer, (SequenceTimeNode) timeNode);
          continue;
        case PropertyEffect _:
          AnimSerializator.SerializeAnimPropertyEffect(writer, (PropertyEffect) timeNode);
          continue;
        case ColorEffect _:
          AnimSerializator.SerializeColorEffect(writer, (ColorEffect) timeNode);
          continue;
        case FilterEffect _:
          AnimSerializator.SerializeAnimFilterEffect(writer, (FilterEffect) timeNode);
          continue;
        case MotionEffect _:
          AnimSerializator.SerializeAnimMotionEffect(writer, (MotionEffect) timeNode);
          continue;
        case RotationEffect _:
          AnimSerializator.SerializeAnimationRotationEffect(writer, (RotationEffect) timeNode);
          continue;
        case ScaleEffect _:
          AnimSerializator.SerializeScaleEffect(writer, (ScaleEffect) timeNode);
          continue;
        case CommandEffect _:
          AnimSerializator.SerializeCommandEffect(writer, (CommandEffect) timeNode);
          continue;
        case SetEffect _:
          AnimSerializator.SerializeSetEffect(writer, (SetEffect) timeNode);
          continue;
        default:
          continue;
      }
    }
  }

  private static void SerializeBuildList(XmlWriter writer, List<Build> buildList)
  {
    writer.WriteStartElement("p", "bldLst", "http://schemas.openxmlformats.org/presentationml/2006/main");
    foreach (Build build in buildList)
      AnimSerializator.SerializeBuildParagraph(writer, build.BuildElements);
    writer.WriteEndElement();
  }

  private static void SerializeBuildParagraph(XmlWriter writer, List<object> buildParagraph)
  {
    foreach (object diagramValues in buildParagraph)
    {
      switch (diagramValues)
      {
        case BuildParagraph _:
          AnimSerializator.SerializeBuildParagraph2(writer, (BuildParagraph) diagramValues);
          continue;
        case BuildDiagram _:
          AnimSerializator.SerializeBuildDiagram(writer, (BuildDiagram) diagramValues);
          continue;
        case BuildGraphics _:
          AnimSerializator.SerializeBuildGraphics(writer, (BuildGraphics) diagramValues);
          continue;
        case BuildOleChart _:
          AnimSerializator.SerializeBuildChart(writer, (BuildOleChart) diagramValues);
          continue;
        default:
          continue;
      }
    }
  }

  private static void SerializeBuildParagraph2(XmlWriter writer, BuildParagraph buildParagraph)
  {
    writer.WriteStartElement("p", "bldP", "http://schemas.openxmlformats.org/presentationml/2006/main");
    writer.WriteAttributeString("spid", buildParagraph.ShapeId);
    writer.WriteAttributeString("grpId", buildParagraph.GroupId.ToString());
    if (buildParagraph.AnimateBackground)
      writer.WriteAttributeString("animBg", "1");
    else
      writer.WriteAttributeString("animBg", "0");
    if (buildParagraph.AutoAdvanceTime != 0)
      writer.WriteAttributeString("advAuto", buildParagraph.AutoAdvanceTime.ToString());
    if (buildParagraph.AutoUpdateAnimBackground)
      writer.WriteAttributeString("autoUpdateAnimBg", buildParagraph.AutoUpdateAnimBackground.ToString());
    if (buildParagraph.BuildLevel != 0U)
      writer.WriteAttributeString("bldLvl", buildParagraph.BuildLevel.ToString());
    if (buildParagraph.BuildType != ParagraphBuildType.None)
      writer.WriteAttributeString("build", AnimationConstant.GetParagraphBuildTypeString(buildParagraph.BuildType));
    if (buildParagraph.ExpandUI)
      writer.WriteAttributeString("uiExpand", "1");
    if (buildParagraph.Reverse)
      writer.WriteAttributeString("rev", buildParagraph.Reverse.ToString());
    if (buildParagraph.TemplateList != null)
      AnimSerializator.SerializeTemplateEffects(writer, buildParagraph.TemplateList);
    writer.WriteEndElement();
  }

  private static void SerializeTemplateEffects(XmlWriter writer, List<TemplateEffects> templates)
  {
    writer.WriteStartElement("p", "tmplLst", "http://schemas.openxmlformats.org/presentationml/2006/main");
    foreach (TemplateEffects template in templates)
    {
      writer.WriteStartElement("p", "tmpl", "http://schemas.openxmlformats.org/presentationml/2006/main");
      writer.WriteAttributeString("lvl", template.Template.Level.ToString());
      if (template.Template.TimeNodeList != null)
        AnimSerializator.SerializeTimeNodeList(writer, template.Template.TimeNodeList);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private static void SerializeBuildDiagram(XmlWriter writer, BuildDiagram diagramValues)
  {
    writer.WriteStartElement("p", "bldDgm", "http://schemas.openxmlformats.org/presentationml/2006/main");
    writer.WriteAttributeString("spid", diagramValues.ShapeId.ToString());
    writer.WriteAttributeString("grpId", diagramValues.GroupId.ToString());
    if (diagramValues.UiExpand)
      writer.WriteAttributeString("uiExpand", "1");
    if (diagramValues.Build != DiagramBuildType.None)
      writer.WriteAttributeString("bld", AnimationConstant.GetDiagramBuildTypeString(diagramValues.Build));
    writer.WriteEndElement();
  }

  private static void SerializeBuildGraphics(XmlWriter writer, BuildGraphics buildGraphics)
  {
    writer.WriteStartElement("p", "bldGraphic", "http://schemas.openxmlformats.org/presentationml/2006/main");
    writer.WriteAttributeString("spid", buildGraphics.ShapeId.ToString());
    writer.WriteAttributeString("grpId", buildGraphics.GroupId.ToString());
    if (buildGraphics.IsUiExpand)
      writer.WriteAttributeString("uiExpand", "1");
    if (buildGraphics.BuildAsOne != null)
    {
      writer.WriteStartElement("p", "bldAsOne", "http://schemas.openxmlformats.org/presentationml/2006/main");
      writer.WriteEndElement();
    }
    if (buildGraphics.BuildSubElements != null)
      AnimSerializator.SerializeBuildGraphicsSubElements(writer, buildGraphics.BuildSubElements);
    writer.WriteEndElement();
  }

  private static void SerializeBuildGraphicsSubElements(
    XmlWriter writer,
    BuildSubElements buildSubElements)
  {
    writer.WriteStartElement("p", "bldSub", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (buildSubElements.Chart != null)
    {
      writer.WriteStartElement("a", "bldChart", "http://schemas.openxmlformats.org/drawingml/2006/main");
      if (buildSubElements.Chart.AnimateBackground)
        writer.WriteAttributeString("animBg", "1");
      else
        writer.WriteAttributeString("animBg", "0");
      if (buildSubElements.Chart.Build != OleChartBuildType.None)
        writer.WriteAttributeString("bld", AnimationConstant.GetOleChartBuildTypeString(buildSubElements.Chart.Build));
      writer.WriteEndElement();
    }
    if (buildSubElements.Diagram != null)
    {
      writer.WriteStartElement("a", "bldDgm", "http://schemas.openxmlformats.org/drawingml/2006/main");
      if (buildSubElements.Diagram.BuildType != AnimationBuildType.None)
        writer.WriteAttributeString("bld", AnimationConstant.GetAnimationDiagramBuildTypeString(buildSubElements.Diagram.BuildType));
      if (buildSubElements.Diagram.IsReverseAnimation)
        writer.WriteAttributeString("rev", buildSubElements.Diagram.IsReverseAnimation.ToString());
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private static void SerializeBuildChart(XmlWriter writer, BuildOleChart buildOleChart)
  {
    writer.WriteStartElement("p", "bldOleChart", "http://schemas.openxmlformats.org/presentationml/2006/main");
    writer.WriteAttributeString("spid", buildOleChart.ShapeId.ToString());
    writer.WriteAttributeString("grpId", buildOleChart.GroupId.ToString());
    if (buildOleChart.UiExpand)
      writer.WriteAttributeString("uiExpand", "1");
    if (buildOleChart.Build != OleChartBuildType.None)
      writer.WriteAttributeString("bld", AnimationConstant.GetOleChartBuildTypeString(buildOleChart.Build));
    if (buildOleChart.AnimateBackground)
      writer.WriteAttributeString("animBg", buildOleChart.AnimateBackground.ToString());
    else
      writer.WriteAttributeString("animBg", "0");
    writer.WriteEndElement();
  }

  private static void SerializeParallelTimeNode(XmlWriter writer, ParallelTimeNode parallelTimeNode)
  {
    writer.WriteStartElement("p", "par", "http://schemas.openxmlformats.org/presentationml/2006/main");
    AnimSerializator.SerializeCommonTimeNode(writer, parallelTimeNode.CommonTimeNode);
    writer.WriteEndElement();
  }

  private static void SerializeCommonTimeNode(XmlWriter writer, CommonTimeNode commonTimeNode)
  {
    writer.WriteStartElement("p", "cTn", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (commonTimeNode.Acceleration != 0)
      writer.WriteAttributeString("accel", commonTimeNode.Acceleration.ToString());
    if (commonTimeNode.AfterEffect)
      writer.WriteAttributeString("afterEffect", "1");
    if (commonTimeNode.AutoReverse)
      writer.WriteAttributeString("autoRev", "1");
    if (commonTimeNode.BuildLevel != 0)
      writer.WriteAttributeString("bldLvl", commonTimeNode.BuildLevel.ToString());
    if (commonTimeNode.Deceleration != 0)
      writer.WriteAttributeString("decel", commonTimeNode.Deceleration.ToString());
    if (commonTimeNode.Display)
      writer.WriteAttributeString("display", "1");
    if (commonTimeNode.Duration != null)
      writer.WriteAttributeString("dur", commonTimeNode.Duration);
    if (commonTimeNode.EventFilter != null)
      writer.WriteAttributeString("evtFilter", commonTimeNode.EventFilter.ToString());
    if (commonTimeNode.Fill != TimeNodeFill.None)
      writer.WriteAttributeString("fill", AnimationConstant.GetTimeNodeFillString(commonTimeNode.Fill));
    if ((double) commonTimeNode.GroupId != -1.0 && !float.IsNaN(commonTimeNode.GroupId))
      writer.WriteAttributeString("grpId", commonTimeNode.GroupId.ToString());
    if (commonTimeNode.Id != 0U)
      writer.WriteAttributeString("id", commonTimeNode.Id.ToString());
    if (commonTimeNode.NodePlaceholder)
      writer.WriteAttributeString("nodePh", "1");
    if (commonTimeNode.NodeType != TimeNodeType.None)
      writer.WriteAttributeString("nodeType", AnimationConstant.GetTimeNodeTypeString(commonTimeNode.NodeType));
    if (commonTimeNode.PresetClass != EffectPresetClassType.None)
      writer.WriteAttributeString("presetClass", AnimationConstant.GetPresetClassString(commonTimeNode.PresetClass));
    if (!float.IsNaN(commonTimeNode.PresetId))
      writer.WriteAttributeString("presetID", commonTimeNode.PresetId.ToString());
    if ((double) commonTimeNode.PresetSubtype != -1.0 && !float.IsNaN(commonTimeNode.PresetSubtype))
      writer.WriteAttributeString("presetSubtype", commonTimeNode.PresetSubtype.ToString());
    if (commonTimeNode.RepeatCount != null)
      writer.WriteAttributeString("repeatCount", commonTimeNode.RepeatCount);
    if (commonTimeNode.RepeatDuration != null)
      writer.WriteAttributeString("repeatDur", commonTimeNode.RepeatDuration);
    if (commonTimeNode.Restart != EffectRestartType.NotDefined)
      writer.WriteAttributeString("restart", AnimationConstant.GetRestartTypeString(commonTimeNode.Restart));
    if (commonTimeNode.Speed != 0)
      writer.WriteAttributeString("spd", commonTimeNode.Speed.ToString());
    if (commonTimeNode.SyncBehavior != TimeNodeSync.None)
      writer.WriteAttributeString("syncBehavior", commonTimeNode.SyncBehavior.ToString());
    if (commonTimeNode.TimeFilter != null)
      writer.WriteAttributeString("tmFilter", commonTimeNode.TimeFilter);
    if (commonTimeNode.StartConditionList != null)
      AnimSerializator.SerializeStartConditionsList(writer, commonTimeNode.StartConditionList);
    if (commonTimeNode.EndConditionList != null)
      AnimSerializator.SerializeEndConditionsList(writer, commonTimeNode.EndConditionList);
    if (commonTimeNode.EndSync != null)
      AnimSerializator.SerializeEndSync(writer, commonTimeNode.EndSync);
    if (commonTimeNode.Iterate != null)
      AnimSerializator.SerializeIterateProperty(writer, commonTimeNode.Iterate);
    if (commonTimeNode.ChildTimeNodeList != null)
      AnimSerializator.SerializeChildTimeNodeList(writer, commonTimeNode.ChildTimeNodeList);
    if (commonTimeNode.SubTimeNodeList != null && commonTimeNode.SubTimeNodeList.Count > 0)
    {
      writer.WriteStartElement("p", "subTnLst", "http://schemas.openxmlformats.org/presentationml/2006/main");
      AnimSerializator.SerializeTimeNodePropertiesList(writer, commonTimeNode.SubTimeNodeList);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private static void SerializeStartConditionsList(
    XmlWriter writer,
    List<Condition> startConditionList)
  {
    writer.WriteStartElement("p", "stCondLst", "http://schemas.openxmlformats.org/presentationml/2006/main");
    AnimSerializator.SerializeConditionList(writer, startConditionList);
    writer.WriteEndElement();
  }

  private static void SerializeEndConditionsList(XmlWriter writer, List<Condition> endConditionList)
  {
    writer.WriteStartElement("p", "endCondLst", "http://schemas.openxmlformats.org/presentationml/2006/main");
    AnimSerializator.SerializeConditionList(writer, endConditionList);
    writer.WriteEndElement();
  }

  private static void SerializeEndSync(XmlWriter writer, Condition endSyncCondition)
  {
    writer.WriteStartElement("p", "endSync", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (endSyncCondition.Delay == -1)
      writer.WriteAttributeString("delay", "indefinite");
    else
      writer.WriteAttributeString("delay", endSyncCondition.Delay.ToString());
    if (endSyncCondition.Event != TriggerEvent.None)
      writer.WriteAttributeString("evt", AnimationConstant.GetTriggerEventString(endSyncCondition.Event));
    if (endSyncCondition.RunTimeNodeTrigger != null)
      AnimSerializator.SerializeRunTimeNodeTrigger(writer, endSyncCondition.RunTimeNodeTrigger);
    if (endSyncCondition.TimeNode != null)
      AnimSerializator.SerializeConditionTimeNode(writer, endSyncCondition.TimeNode);
    if (endSyncCondition.Target != null)
      AnimSerializator.SerializeTargetElement(writer, endSyncCondition.Target);
    writer.WriteEndElement();
  }

  private static void SerializeIterateProperty(XmlWriter writer, Iterate iterate)
  {
    writer.WriteStartElement("p", nameof (iterate), "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (iterate.Backwards)
      writer.WriteAttributeString("backwards", "1");
    if (iterate.Type != IterateType.None)
      writer.WriteAttributeString("type", iterate.Type.ToString().ToLower());
    if (iterate.TimeAbsolute != null)
    {
      writer.WriteStartElement("p", "tmAbs", "http://schemas.openxmlformats.org/presentationml/2006/main");
      if (iterate.TimeAbsolute.Value == -1)
        writer.WriteAttributeString("val", "indefinite");
      else
        writer.WriteAttributeString("val", iterate.TimeAbsolute.Value.ToString());
      writer.WriteEndElement();
    }
    if (iterate.TimePercantage != null)
    {
      writer.WriteStartElement("p", "tmPct", "http://schemas.openxmlformats.org/presentationml/2006/main");
      writer.WriteAttributeString("val", iterate.TimePercantage.Value);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private static void SerializeChildTimeNodeList(XmlWriter writer, List<object> childTimeNodeList)
  {
    writer.WriteStartElement("p", "childTnLst", "http://schemas.openxmlformats.org/presentationml/2006/main");
    foreach (object childTimeNode in childTimeNodeList)
    {
      switch (childTimeNode)
      {
        case ParallelTimeNode _:
          AnimSerializator.SerializeParallelTimeNode(writer, (ParallelTimeNode) childTimeNode);
          continue;
        case SequenceTimeNode _:
          AnimSerializator.SerializeSequenceTimeNode(writer, (SequenceTimeNode) childTimeNode);
          continue;
        case PropertyEffect _:
          AnimSerializator.SerializeAnimPropertyEffect(writer, (PropertyEffect) childTimeNode);
          continue;
        case ColorEffect _:
          AnimSerializator.SerializeColorEffect(writer, (ColorEffect) childTimeNode);
          continue;
        case FilterEffect _:
          AnimSerializator.SerializeAnimFilterEffect(writer, (FilterEffect) childTimeNode);
          continue;
        case MotionEffect _:
          AnimSerializator.SerializeAnimMotionEffect(writer, (MotionEffect) childTimeNode);
          continue;
        case RotationEffect _:
          AnimSerializator.SerializeAnimationRotationEffect(writer, (RotationEffect) childTimeNode);
          continue;
        case ScaleEffect _:
          AnimSerializator.SerializeScaleEffect(writer, (ScaleEffect) childTimeNode);
          continue;
        case CommandEffect _:
          AnimSerializator.SerializeCommandEffect(writer, (CommandEffect) childTimeNode);
          continue;
        case SetEffect _:
          AnimSerializator.SerializeSetEffect(writer, (SetEffect) childTimeNode);
          continue;
        default:
          continue;
      }
    }
    writer.WriteEndElement();
  }

  private static void SerializeSequenceTimeNode(XmlWriter writer, SequenceTimeNode sequenceTimeNode)
  {
    writer.WriteStartElement("p", "seq", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (sequenceTimeNode.Concurrent)
      writer.WriteAttributeString("concurrent", "1");
    if (sequenceTimeNode.NextAction != NextAction.None)
      writer.WriteAttributeString("nextAc", NextAction.Seek.ToString().ToLower());
    if (sequenceTimeNode.PreviousAction != PreviousAction.None)
      writer.WriteAttributeString("prevAc", AnimationConstant.GetPreviousActionString(PreviousAction.SkipTimed));
    if (sequenceTimeNode.CommonTimeNode != null)
      AnimSerializator.SerializeCommonTimeNode(writer, sequenceTimeNode.CommonTimeNode);
    if (sequenceTimeNode.PreviousConditionList != null)
    {
      writer.WriteStartElement("p", "prevCondLst", "http://schemas.openxmlformats.org/presentationml/2006/main");
      AnimSerializator.SerializeConditionList(writer, sequenceTimeNode.PreviousConditionList);
      writer.WriteEndElement();
    }
    if (sequenceTimeNode.NextConditionList != null)
    {
      writer.WriteStartElement("p", "nextCondLst", "http://schemas.openxmlformats.org/presentationml/2006/main");
      AnimSerializator.SerializeConditionList(writer, sequenceTimeNode.NextConditionList);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private static void SerializeConditionList(XmlWriter writer, List<Condition> conditionsList)
  {
    foreach (Condition conditions in conditionsList)
    {
      writer.WriteStartElement("p", "cond", "http://schemas.openxmlformats.org/presentationml/2006/main");
      if (conditions.Delay == -1)
        writer.WriteAttributeString("delay", "indefinite");
      else
        writer.WriteAttributeString("delay", conditions.Delay.ToString());
      if (conditions.Event != TriggerEvent.None)
        writer.WriteAttributeString("evt", AnimationConstant.GetTriggerEventString(conditions.Event));
      if (conditions.RunTimeNodeTrigger != null)
        AnimSerializator.SerializeRunTimeNodeTrigger(writer, conditions.RunTimeNodeTrigger);
      if (conditions.TimeNode != null)
        AnimSerializator.SerializeConditionTimeNode(writer, conditions.TimeNode);
      if (conditions.Target != null)
        AnimSerializator.SerializeTargetElement(writer, conditions.Target);
      writer.WriteEndElement();
    }
  }

  private static void SerializeRunTimeNodeTrigger(
    XmlWriter writer,
    RunTimeNodeTrigger runTimeNodeTrigger)
  {
    writer.WriteStartElement("p", "rtn", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (runTimeNodeTrigger.Val != TriggerRuntimeNode.None)
      writer.WriteAttributeString("val", runTimeNodeTrigger.Val.ToString().ToLower());
    writer.WriteEndElement();
  }

  private static void SerializeConditionTimeNode(XmlWriter writer, TimeNode conditionTimeNode)
  {
    writer.WriteStartElement("p", "tn", "http://schemas.openxmlformats.org/presentationml/2006/main");
    writer.WriteAttributeString("val", conditionTimeNode.Val.ToString());
    writer.WriteEndElement();
  }

  private static void SerializeTargetElement(XmlWriter writer, TargetElement targetElement)
  {
    writer.WriteStartElement("p", "tgtEl", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (targetElement.ShapeTarget != null)
      AnimSerializator.SerializeTarget(writer, targetElement.ShapeTarget);
    if (targetElement.SlideTarget != null)
    {
      writer.WriteStartElement("p", "sldTgt", "http://schemas.openxmlformats.org/presentationml/2006/main");
      writer.WriteEndElement();
    }
    if (targetElement.InkTarget != null)
    {
      writer.WriteStartElement("p", "inkTgt", "http://schemas.openxmlformats.org/presentationml/2006/main");
      if (string.IsNullOrEmpty(targetElement.InkTarget.ShapeId))
        writer.WriteAttributeString("spid", targetElement.InkTarget.ShapeId);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private static void SerializeTarget(XmlWriter writer, ShapeTarget shapeTarget)
  {
    writer.WriteStartElement("p", "spTgt", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (!string.IsNullOrEmpty(shapeTarget.ShapeId))
      writer.WriteAttributeString("spid", shapeTarget.ShapeId);
    if (shapeTarget.Background != null)
    {
      writer.WriteStartElement("p", "bg", "http://schemas.openxmlformats.org/presentationml/2006/main");
      writer.WriteEndElement();
    }
    if (shapeTarget.GraphicElement != null)
      AnimSerializator.SerializeTargetGraphicElement(writer, shapeTarget.GraphicElement);
    if (shapeTarget.OleChartElement != null)
    {
      writer.WriteStartElement("p", "oleChartEl", "http://schemas.openxmlformats.org/presentationml/2006/main");
      writer.WriteAttributeString("lvl", shapeTarget.OleChartElement.Level.ToString());
      if (shapeTarget.OleChartElement.Type != ChartSubelementType.None)
        writer.WriteAttributeString("type", AnimationConstant.GetTargetOleChartSubElementString(shapeTarget.OleChartElement.Type));
      writer.WriteEndElement();
    }
    if (shapeTarget.SubShape != null)
    {
      writer.WriteStartElement("p", "subSp", "http://schemas.openxmlformats.org/presentationml/2006/main");
      if (string.IsNullOrEmpty(shapeTarget.SubShape.ShapeId))
        writer.WriteAttributeString("spid", shapeTarget.SubShape.ShapeId);
      writer.WriteEndElement();
    }
    if (shapeTarget.TextElement != null)
    {
      writer.WriteStartElement("p", "txEl", "http://schemas.openxmlformats.org/presentationml/2006/main");
      if (shapeTarget.TextElement.CharacterRange != null)
      {
        writer.WriteStartElement("p", "charRg", "http://schemas.openxmlformats.org/presentationml/2006/main");
        writer.WriteAttributeString("st", shapeTarget.TextElement.CharacterRange.Start.ToString());
        writer.WriteAttributeString("end", shapeTarget.TextElement.CharacterRange.End.ToString());
        writer.WriteEndElement();
      }
      if (shapeTarget.TextElement.ParagraphRange != null)
      {
        writer.WriteStartElement("p", "pRg", "http://schemas.openxmlformats.org/presentationml/2006/main");
        writer.WriteAttributeString("st", shapeTarget.TextElement.ParagraphRange.Start.ToString());
        writer.WriteAttributeString("end", shapeTarget.TextElement.ParagraphRange.End.ToString());
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private static void SerializeTargetGraphicElement(
    XmlWriter writer,
    GraphicElement targetGraphicElement)
  {
    writer.WriteStartElement("p", "graphicEl", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (targetGraphicElement.GraphicChart != null)
    {
      writer.WriteStartElement("a", "chart", "http://schemas.openxmlformats.org/drawingml/2006/main");
      if (targetGraphicElement.GraphicChart.AnimationBuildStep != AnimationBuildStep.None)
        writer.WriteAttributeString("bldStep", AnimationConstant.GetTargetChartAnimationBuildStepString(targetGraphicElement.GraphicChart.AnimationBuildStep));
      writer.WriteAttributeString("categoryIdx", targetGraphicElement.GraphicChart.CategoryIndex.ToString());
      writer.WriteAttributeString("seriesIdx", targetGraphicElement.GraphicChart.SeriesIndex.ToString());
      writer.WriteEndElement();
    }
    if (targetGraphicElement.GraphicDiagram != null)
    {
      writer.WriteStartElement("a", "dgm", "http://schemas.openxmlformats.org/drawingml/2006/main");
      if (targetGraphicElement.GraphicDiagram.AnimationBuildStepDiagram != AnimationBuildStepDiagram.None)
        writer.WriteAttributeString("bldStep", targetGraphicElement.GraphicDiagram.AnimationBuildStepDiagram.ToString());
      writer.WriteAttributeString("id", Serializator.GetGuidString(targetGraphicElement.GraphicDiagram.Id));
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private static void SerializeScaleEffect(XmlWriter writer, ScaleEffect effect)
  {
    writer.WriteStartElement("p", "animScale", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (effect.ZoomContent.HasValue)
    {
      bool? zoomContent = effect.ZoomContent;
      if ((zoomContent.GetValueOrDefault() ? 1 : (!zoomContent.HasValue ? 1 : 0)) != 0)
        writer.WriteAttributeString("zoomContents", AnimationConstant.GetNullableBoolString(effect.ZoomContent));
    }
    if (effect.CommonBehavior != null)
      AnimSerializator.SerializeCommonBehavior(writer, effect.CommonBehavior);
    if ((double) effect.From.X >= 0.0 || (double) effect.From.Y >= 0.0)
    {
      writer.WriteStartElement("p", "from", "http://schemas.openxmlformats.org/presentationml/2006/main");
      AnimSerializator.SerializePointFValues(writer, effect.From);
      writer.WriteEndElement();
    }
    if ((double) effect.By.X >= 0.0 || (double) effect.By.Y >= 0.0)
    {
      writer.WriteStartElement("p", "by", "http://schemas.openxmlformats.org/presentationml/2006/main");
      AnimSerializator.SerializePointFValues(writer, effect.By);
      writer.WriteEndElement();
    }
    if ((double) effect.To.X >= 0.0 || (double) effect.To.Y >= 0.0)
    {
      writer.WriteStartElement("p", "to", "http://schemas.openxmlformats.org/presentationml/2006/main");
      AnimSerializator.SerializePointFValues(writer, effect.To);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private static void SerializeCommonBehavior(XmlWriter writer, CommonBehavior behavior)
  {
    writer.WriteStartElement("p", "cBhvr", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (behavior.Accumulate != BehaviorAccumulateType.None)
      writer.WriteAttributeString("accumulate", AnimationConstant.GetBehaviorTypeString(behavior.Accumulate));
    if (behavior.Additive != BehaviorAdditiveType.None && behavior.Additive != BehaviorAdditiveType.NotDefined)
      writer.WriteAttributeString("additive", AnimationConstant.GetBehaviorAdditiveString(behavior.Additive));
    if (!string.IsNullOrEmpty(behavior.By))
      writer.WriteAttributeString("by", behavior.By);
    if (!string.IsNullOrEmpty(behavior.From))
      writer.WriteAttributeString("from", behavior.From);
    if (behavior.Override != BehaviourOverrideType.None)
      writer.WriteAttributeString("override", AnimationConstant.GetBehaviorOverrideTypeString(behavior.Override));
    if (!string.IsNullOrEmpty(behavior.RunTimeContext))
      writer.WriteAttributeString("rctx", behavior.RunTimeContext);
    if (!string.IsNullOrEmpty(behavior.To))
      writer.WriteAttributeString("to", behavior.To);
    if (behavior.TransformType != BehaviorTransformType.None)
      writer.WriteAttributeString("xfrmType", AnimationConstant.GetBehaviorTransformTypeString(behavior.TransformType));
    if (behavior.CommonTimeNode != null)
      AnimSerializator.SerializeCommonTimeNode(writer, behavior.CommonTimeNode);
    if (behavior.Target != null)
      AnimSerializator.SerializeTargetElement(writer, behavior.Target);
    if (behavior.AttributeNameList != null)
      AnimSerializator.SerializeAttributeList(writer, behavior.AttributeNameList);
    writer.WriteEndElement();
  }

  private static void SerializeAttributeList(XmlWriter writer, List<AttributeName> attributeList)
  {
    writer.WriteStartElement("p", "attrNameLst", "http://schemas.openxmlformats.org/presentationml/2006/main");
    foreach (AttributeName attribute in attributeList)
    {
      writer.WriteStartElement("p", "attrName", "http://schemas.openxmlformats.org/presentationml/2006/main");
      writer.WriteString(attribute.Name);
      writer.WriteEndElement();
    }
    if (attributeList.Count == 0)
    {
      writer.WriteStartElement("p", "attrName", "http://schemas.openxmlformats.org/presentationml/2006/main");
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private static void SerializePointFValues(XmlWriter writer, PointF pointValues)
  {
    writer.WriteAttributeString("x", (pointValues.X * 1000f).ToString());
    writer.WriteAttributeString("y", (pointValues.Y * 1000f).ToString());
  }

  private static void SerializeAnimationRotationEffect(XmlWriter writer, RotationEffect effect)
  {
    writer.WriteStartElement("p", "animRot", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (!float.IsNaN(effect.By))
      writer.WriteAttributeString("by", ((int) effect.By * 60000).ToString());
    if (!float.IsNaN(effect.From))
      writer.WriteAttributeString("from", ((int) effect.From * 60000).ToString());
    if (!float.IsNaN(effect.To))
      writer.WriteAttributeString("to", ((int) effect.To * 60000).ToString());
    if (effect.CommonBehavior != null)
      AnimSerializator.SerializeCommonBehavior(writer, effect.CommonBehavior);
    writer.WriteEndElement();
  }

  private static void SerializeCommandEffect(XmlWriter writer, CommandEffect effect)
  {
    writer.WriteStartElement("p", "cmd", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (!string.IsNullOrEmpty(effect.CommandString))
      writer.WriteAttributeString("cmd", effect.CommandString);
    if (effect.Type != CommandEffectType.NotDefined)
      writer.WriteAttributeString("type", effect.Type.ToString().ToLower());
    if (effect.CommonBehavior != null)
      AnimSerializator.SerializeCommonBehavior(writer, effect.CommonBehavior);
    writer.WriteEndElement();
  }

  private static void SerializeAnimPropertyEffect(XmlWriter writer, PropertyEffect effect)
  {
    writer.WriteStartElement("p", "anim", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (!string.IsNullOrEmpty(effect.To))
      writer.WriteAttributeString("to", effect.To);
    if (effect.CalcMode != PropertyCalcModeType.NotDefined)
      writer.WriteAttributeString("calcmode", AnimationConstant.GetCalcPropertyTypeString(effect.CalcMode));
    if (!string.IsNullOrEmpty(effect.By))
      writer.WriteAttributeString("by", effect.By);
    if (!string.IsNullOrEmpty(effect.From))
      writer.WriteAttributeString("from", effect.From);
    if (effect.ValueType != PropertyValueType.NotDefined)
      writer.WriteAttributeString("valueType", AnimationConstant.GetPropertyValueString(effect.ValueType));
    if (effect.CommonBehavior != null)
      AnimSerializator.SerializeCommonBehavior(writer, effect.CommonBehavior);
    if (effect.TAVList != null && effect.TAVList.Count > 0)
      AnimSerializator.SerializeTimeanimatedValueList(writer, effect.TAVList);
    writer.WriteEndElement();
  }

  private static void SerializeTimeanimatedValueList(
    XmlWriter writer,
    List<TimeAnimateValue> timeValues)
  {
    writer.WriteStartElement("p", "tavLst", "http://schemas.openxmlformats.org/presentationml/2006/main");
    foreach (TimeAnimateValue timeValue in timeValues)
    {
      writer.WriteStartElement("p", "tav", "http://schemas.openxmlformats.org/presentationml/2006/main");
      if (!string.IsNullOrEmpty(timeValue.Formula))
        writer.WriteAttributeString("fmla", timeValue.Formula);
      if (timeValue.Time >= 0)
        writer.WriteAttributeString("tm", timeValue.Time.ToString());
      if (timeValue.TimeValue != null)
      {
        writer.WriteStartElement("p", "val", "http://schemas.openxmlformats.org/presentationml/2006/main");
        AnimSerializator.SerializeValuesProperty(writer, timeValue.TimeValue);
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private static void SerializeSetEffect(XmlWriter writer, SetEffect effect)
  {
    writer.WriteStartElement("p", "set", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (effect.CommonBehavior != null)
      AnimSerializator.SerializeCommonBehavior(writer, effect.CommonBehavior);
    if (effect.InternalTo != null)
    {
      writer.WriteStartElement("p", "to", "http://schemas.openxmlformats.org/presentationml/2006/main");
      AnimSerializator.SerializeValuesProperty(writer, effect.InternalTo);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private static void SerializeValuesProperty(XmlWriter writer, Values value)
  {
    if (value.Bool.HasValue)
    {
      writer.WriteStartElement("p", "boolVal", "http://schemas.openxmlformats.org/presentationml/2006/main");
      writer.WriteAttributeString("val", value.Bool.ToString().ToLower());
      writer.WriteEndElement();
    }
    if (value.Int.HasValue)
    {
      writer.WriteStartElement("p", "intVal", "http://schemas.openxmlformats.org/presentationml/2006/main");
      writer.WriteAttributeString("val", value.Int.ToString());
      writer.WriteEndElement();
    }
    if (value.Float.HasValue)
    {
      writer.WriteStartElement("p", "fltVal", "http://schemas.openxmlformats.org/presentationml/2006/main");
      writer.WriteAttributeString("val", Helper.ToString(value.Float.Value));
      writer.WriteEndElement();
    }
    if (value.String != null)
    {
      writer.WriteStartElement("p", "strVal", "http://schemas.openxmlformats.org/presentationml/2006/main");
      writer.WriteAttributeString("val", value.String);
      writer.WriteEndElement();
    }
    if (value.Color == null)
      return;
    writer.WriteStartElement("p", "clrVal", "http://schemas.openxmlformats.org/presentationml/2006/main");
    AnimSerializator.SerializeColorValues(writer, value.Color);
    writer.WriteEndElement();
  }

  private static string GetPointsTypesValues(string pointsTypesValues, MotionPath path)
  {
    for (int index = 0; index < path.Count; ++index)
    {
      MotionCmdPath motionCmdPath = path[index] as MotionCmdPath;
      pointsTypesValues += (string) (object) AnimationConstant.GetMotionPathPointsTypesString(motionCmdPath.PointsType);
    }
    return pointsTypesValues.Trim();
  }

  private static string GetMotionPathValue(MotionPath path)
  {
    string motionPathValue = "";
    for (int index = 0; index < path.Count; ++index)
    {
      MotionCmdPath motionCmdPath = path[index] as MotionCmdPath;
      switch (motionCmdPath.CommandType)
      {
        case MotionCommandPathType.MoveTo:
          motionPathValue = !motionCmdPath.IsRelative ? motionPathValue + " M" : motionPathValue + " m";
          break;
        case MotionCommandPathType.LineTo:
          motionPathValue = !motionCmdPath.IsRelative ? motionPathValue + " L" : motionPathValue + " l";
          break;
        case MotionCommandPathType.CurveTo:
          motionPathValue = !motionCmdPath.IsRelative ? motionPathValue + " C" : motionPathValue + " c";
          break;
        case MotionCommandPathType.CloseLoop:
          motionPathValue = !motionCmdPath.IsRelative ? motionPathValue + " Z" : motionPathValue + " z";
          break;
        case MotionCommandPathType.End:
          motionPathValue = !motionCmdPath.IsRelative ? motionPathValue + " E" : motionPathValue + " e";
          break;
      }
      if (motionCmdPath.Points != null)
      {
        foreach (PointF point in motionCmdPath.Points)
          motionPathValue = $"{$"{motionPathValue} {Helper.ToString(point.X)}"} {Helper.ToString(point.Y)}";
      }
    }
    return motionPathValue;
  }

  private static void SerializeAnimMotionEffect(XmlWriter writer, MotionEffect effect)
  {
    string pointsTypesValues = "";
    writer.WriteStartElement("p", "animMotion", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (effect.Origin != MotionOriginType.NotDefined)
      writer.WriteAttributeString("origin", effect.Origin.ToString().ToLower());
    if (effect.Path != null)
    {
      string motionPathValue = AnimSerializator.GetMotionPathValue(effect.Path as MotionPath);
      writer.WriteAttributeString("path", motionPathValue);
    }
    if (effect.PathEditMode != MotionPathEditMode.NotDefined)
      writer.WriteAttributeString("pathEditMode", effect.PathEditMode.ToString().ToLower());
    writer.WriteAttributeString("ptsTypes", AnimSerializator.GetPointsTypesValues(pointsTypesValues, effect.Path as MotionPath));
    if ((double) effect.Angle >= 0.0)
      writer.WriteAttributeString("rAng", effect.Angle.ToString());
    if (effect.CommonBehavior != null)
      AnimSerializator.SerializeCommonBehavior(writer, effect.CommonBehavior);
    if ((double) effect.By.X >= 0.0 || (double) effect.By.Y >= 0.0)
    {
      writer.WriteStartElement("p", "by", "http://schemas.openxmlformats.org/presentationml/2006/main");
      AnimSerializator.SerializePointFValues(writer, effect.By);
      writer.WriteEndElement();
    }
    if ((double) effect.From.X >= 0.0 || (double) effect.From.Y >= 0.0)
    {
      writer.WriteStartElement("p", "from", "http://schemas.openxmlformats.org/presentationml/2006/main");
      AnimSerializator.SerializePointFValues(writer, effect.From);
      writer.WriteEndElement();
    }
    if ((double) effect.To.X >= 0.0 || (double) effect.To.Y >= 0.0)
    {
      writer.WriteStartElement("p", "to", "http://schemas.openxmlformats.org/presentationml/2006/main");
      AnimSerializator.SerializePointFValues(writer, effect.To);
      writer.WriteEndElement();
    }
    if ((double) effect.RotationCenter.X >= 0.0 || (double) effect.RotationCenter.Y >= 0.0)
    {
      writer.WriteStartElement("p", "rCtr", "http://schemas.openxmlformats.org/presentationml/2006/main");
      AnimSerializator.SerializePointFValues(writer, effect.RotationCenter);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private static void SerializeAnimColorValues(
    XmlWriter writer,
    ColorEffect effect,
    ColorObject colorObject)
  {
    BaseSlide baseSlide = effect.CommonBehavior.CommonTimeNode.BaseSlide;
    Syncfusion.Presentation.Presentation presentation = baseSlide.Presentation;
    if (colorObject.ColorType == ColorType.Theme)
    {
      writer.WriteStartElement("a", "schemeClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
      MasterSlide masterSlide1 = (MasterSlide) null;
      LayoutSlide layoutSlide = (LayoutSlide) null;
      if (baseSlide == null)
      {
        if (presentation != null && presentation.Masters != null && presentation.Masters.Count != 0)
          masterSlide1 = presentation.Masters[0] as MasterSlide;
      }
      else
      {
        IMasterSlide masterSlide2 = (IMasterSlide) null;
        if (baseSlide is Slide)
        {
          Slide slide = (Slide) baseSlide;
          if (!((BaseSlide) slide.LayoutSlide.MasterSlide).IsDisposed)
            masterSlide2 = slide.LayoutSlide.MasterSlide;
        }
        else if (baseSlide is MasterSlide && !baseSlide.IsDisposed)
          masterSlide1 = (MasterSlide) baseSlide;
        else if (baseSlide is LayoutSlide)
          layoutSlide = (LayoutSlide) baseSlide;
        else if (baseSlide is HandoutMaster)
          masterSlide2 = presentation != null || baseSlide.Presentation == null ? presentation.Masters[0] : baseSlide.Presentation.Masters[0];
        if (masterSlide2 != null)
          masterSlide1 = (MasterSlide) masterSlide2;
      }
      if (masterSlide1 != null)
        writer.WriteAttributeString("val", Helper.GetThemeStringFromIndex(colorObject.GetColorString(), masterSlide1));
      else
        writer.WriteAttributeString("val", Helper.GetThemeStringFromIndex(colorObject.GetColorString(), layoutSlide));
    }
    else
    {
      writer.WriteStartElement("a", "srgbClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
      string colorName;
      if (colorObject.ColorType == ColorType.RGB)
      {
        colorName = Helper.GetColorName(ColorExtension.FromArgb(colorObject.GetColorInt()));
      }
      else
      {
        colorObject.UpdateColorObject((object) presentation);
        colorName = Helper.GetColorName((IColor) colorObject);
      }
      writer.WriteAttributeString("val", colorName);
    }
    writer.WriteEndElement();
  }

  private static void SerializeColorEffect(XmlWriter writer, ColorEffect effect)
  {
    writer.WriteStartElement("p", "animClr", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (effect.ColorSpace != ColorSpace.NotDefined)
      writer.WriteAttributeString("clrSpc", effect.ColorSpace.ToString().ToLower());
    if (effect.Direction != ColorDirection.NotDefined)
      writer.WriteAttributeString("dir", AnimationConstant.GetColorDirectionString(effect.Direction));
    if (effect.CommonBehavior != null)
      AnimSerializator.SerializeCommonBehavior(writer, effect.CommonBehavior);
    if (!float.IsNaN(effect.By.Value0) || !float.IsNaN(effect.By.Value1) || !float.IsNaN(effect.By.Value2))
    {
      writer.WriteStartElement("p", "by", "http://schemas.openxmlformats.org/presentationml/2006/main");
      AnimSerializator.SerializeColorByValues(writer, effect);
      writer.WriteEndElement();
    }
    if (effect.From != null)
    {
      writer.WriteStartElement("p", "from", "http://schemas.openxmlformats.org/presentationml/2006/main");
      AnimSerializator.SerializeAnimColorValues(writer, effect, effect.GetFromColorObject());
      writer.WriteEndElement();
    }
    if (effect.To != null)
    {
      writer.WriteStartElement("p", "to", "http://schemas.openxmlformats.org/presentationml/2006/main");
      AnimSerializator.SerializeAnimColorValues(writer, effect, effect.GetToColorObject());
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private static void SerializeFilterProperty(XmlWriter writer, FilterEffect effect)
  {
    string str = effect.Type.ToString().ToLower();
    if (effect.Subtype != SubtypeFilterEffect.None)
    {
      string effectSubTypeString = AnimationConstant.GetFilterEffectSubTypeString(effect.Subtype);
      str = $"{str}({effectSubTypeString})";
    }
    writer.WriteAttributeString("filter", str);
  }

  private static void SerializeAnimFilterEffect(XmlWriter writer, FilterEffect effect)
  {
    writer.WriteStartElement("p", "animEffect", "http://schemas.openxmlformats.org/presentationml/2006/main");
    AnimSerializator.SerializeFilterProperty(writer, effect);
    if (effect.Reveal != FilterEffectRevealType.None)
      writer.WriteAttributeString("transition", AnimationConstant.GetRevealTypeFilterEffectString(effect.Reveal));
    if (!string.IsNullOrEmpty(effect.PropEffect))
      writer.WriteAttributeString("prLst", effect.PropEffect);
    if (effect.CommonBehavior != null)
      AnimSerializator.SerializeCommonBehavior(writer, effect.CommonBehavior);
    if (effect.AnimationValues != null)
    {
      writer.WriteStartElement("p", "progress", "http://schemas.openxmlformats.org/presentationml/2006/main");
      AnimSerializator.SerializeValuesProperty(writer, effect.AnimationValues);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private static void SerializeColorByValues(XmlWriter writer, ColorEffect effect)
  {
    if (effect.ColorSpace == ColorSpace.HSL)
    {
      writer.WriteStartElement("p", "hsl", "http://schemas.openxmlformats.org/presentationml/2006/main");
      writer.WriteAttributeString("h", ((float.IsNaN(effect.By.Value0) ? 0 : (int) effect.By.Value0) * 60000).ToString());
      writer.WriteAttributeString("s", ((float.IsNaN(effect.By.Value1) ? 0 : (int) effect.By.Value1) * 10000).ToString());
      writer.WriteAttributeString("l", ((float.IsNaN(effect.By.Value2) ? 0 : (int) effect.By.Value2) * 10000).ToString());
      writer.WriteEndElement();
    }
    else
    {
      if (effect.ColorSpace != ColorSpace.RGB)
        return;
      writer.WriteStartElement("p", "rgb", "http://schemas.openxmlformats.org/presentationml/2006/main");
      writer.WriteAttributeString("b", (float.IsNaN(effect.By.Value0) ? 0 : (int) effect.By.Value0).ToString());
      writer.WriteAttributeString("g", (float.IsNaN(effect.By.Value1) ? 0 : (int) effect.By.Value1).ToString());
      writer.WriteAttributeString("r", (float.IsNaN(effect.By.Value2) ? 0 : (int) effect.By.Value2).ToString());
      writer.WriteEndElement();
    }
  }

  private static void SerializeColorValues(XmlWriter writer, ColorValues colorValue)
  {
    if (colorValue.HSLColorValue != null)
    {
      writer.WriteStartElement("a", "hslClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
      if ((double) colorValue.HSLColorValue.Hue != 0.0)
        writer.WriteAttributeString("h", colorValue.HSLColorValue.Hue.ToString());
      if ((double) colorValue.HSLColorValue.Saturation != 0.0)
        writer.WriteAttributeString("s", colorValue.HSLColorValue.Saturation.ToString());
      if ((double) colorValue.HSLColorValue.Luminance != 0.0)
        writer.WriteAttributeString("l", colorValue.HSLColorValue.Luminance.ToString());
      writer.WriteEndElement();
    }
    if (colorValue.PresetColorValue != null)
    {
      writer.WriteStartElement("a", "prstClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
      if (!string.IsNullOrEmpty(colorValue.PresetColorValue.Value))
        writer.WriteAttributeString("val", colorValue.PresetColorValue.Value);
      writer.WriteEndElement();
    }
    if (colorValue.SchemeColorValue != null)
    {
      writer.WriteStartElement("a", "schemeClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
      if (!string.IsNullOrEmpty(colorValue.SchemeColorValue.Value))
        writer.WriteAttributeString("val", colorValue.SchemeColorValue.Value);
      writer.WriteEndElement();
    }
    if (colorValue.SCrgbColorValue != null)
    {
      writer.WriteStartElement("a", "scrgbClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
      if (!string.IsNullOrEmpty(colorValue.SCrgbColorValue.Blue))
        writer.WriteAttributeString("b", colorValue.SCrgbColorValue.Blue);
      if (!string.IsNullOrEmpty(colorValue.SCrgbColorValue.Green))
        writer.WriteAttributeString("g", colorValue.SCrgbColorValue.Green);
      if (!string.IsNullOrEmpty(colorValue.SCrgbColorValue.Red))
        writer.WriteAttributeString("r", colorValue.SCrgbColorValue.Red);
      writer.WriteEndElement();
    }
    if (colorValue.SrgbColorValue != null)
    {
      writer.WriteStartElement("a", "srgbClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
      if (!string.IsNullOrEmpty(colorValue.SrgbColorValue.Value))
        writer.WriteAttributeString("val", colorValue.SrgbColorValue.Value);
      writer.WriteEndElement();
    }
    if (colorValue.SystemColor == null)
      return;
    writer.WriteStartElement("a", "sysClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (!string.IsNullOrEmpty(colorValue.SystemColor.Value))
      writer.WriteAttributeString("val", colorValue.SystemColor.Value);
    writer.WriteEndElement();
  }
}
