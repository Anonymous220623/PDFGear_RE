// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.AnimParser
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Animation.Internal;
using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.SlideImplementation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

#nullable disable
namespace Syncfusion.Presentation.Animation;

internal class AnimParser
{
  internal static void ParseAnimation(XmlReader reader, BaseSlide slide)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (!reader.IsEmptyElement)
    {
      string localName = reader.LocalName;
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "tnLst":
              slide.IsAnimated = true;
              AnimParser.ParseTimeNodeList(reader, slide.Timing.TimeNodeList, slide);
              continue;
            case "bldLst":
              AnimParser.ParseBuildList(reader, slide.Timing.BuildList, slide);
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          reader.Skip();
      }
    }
    reader.Read();
  }

  private static void ParseParallelTimeNodeList(
    XmlReader reader,
    ParallelTimeNode parallelTimeNode,
    BaseSlide slide)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "cTn":
            CommonTimeNode commonTimeNode = new CommonTimeNode();
            AnimParser.ParseCommonTimeList(reader, commonTimeNode, slide);
            parallelTimeNode.CommonTimeNode = commonTimeNode;
            break;
        }
        reader.Read();
      }
      else
        reader.Skip();
    }
  }

  private static void ParseCommonTimeList(
    XmlReader reader,
    CommonTimeNode commonTimeNode,
    BaseSlide slide)
  {
    commonTimeNode.BaseSlide = slide;
    commonTimeNode.GroupId = -1f;
    commonTimeNode.PresetSubtype = -1f;
    if (reader.MoveToAttribute("dur"))
      commonTimeNode.Duration = reader.Value;
    if (reader.MoveToAttribute("presetID"))
      commonTimeNode.PresetId = (float) XmlConvert.ToInt32(reader.Value);
    if (reader.MoveToAttribute("accel"))
      commonTimeNode.Acceleration = XmlConvert.ToInt32(reader.Value);
    if (reader.MoveToAttribute("afterEffect"))
      commonTimeNode.AfterEffect = XmlConvert.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("autoRev"))
      commonTimeNode.AutoReverse = XmlConvert.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("bldLvl"))
      commonTimeNode.BuildLevel = XmlConvert.ToInt32(reader.Value);
    if (reader.MoveToAttribute("decel"))
      commonTimeNode.Deceleration = XmlConvert.ToInt32(reader.Value);
    if (reader.MoveToAttribute("display"))
      commonTimeNode.Display = XmlConvert.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("evtFilter"))
      commonTimeNode.EventFilter = reader.Value;
    if (reader.MoveToAttribute("fill"))
      commonTimeNode.Fill = AnimationConstant.GetTimeNodeFillValue(reader.Value);
    if (reader.MoveToAttribute("grpId"))
      commonTimeNode.GroupId = (float) XmlConvert.ToInt32(reader.Value);
    if (reader.MoveToAttribute("id"))
      commonTimeNode.Id = XmlConvert.ToUInt32(reader.Value);
    if (reader.MoveToAttribute("nodePh"))
      commonTimeNode.NodePlaceholder = XmlConvert.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("nodeType"))
      commonTimeNode.NodeType = AnimationConstant.GetTimeNode(reader.Value);
    if (reader.MoveToAttribute("presetClass"))
      commonTimeNode.PresetClass = AnimationConstant.GetPresetClassType(reader.Value);
    if (reader.MoveToAttribute("presetSubtype"))
      commonTimeNode.PresetSubtype = (float) XmlConvert.ToInt32(reader.Value);
    if (reader.MoveToAttribute("repeatCount"))
      commonTimeNode.RepeatCount = reader.Value;
    if (reader.MoveToAttribute("repeatDur"))
      commonTimeNode.RepeatDuration = reader.Value;
    if (reader.MoveToAttribute("restart"))
      commonTimeNode.Restart = AnimationConstant.GetRestartType(reader.Value);
    if (reader.MoveToAttribute("spd"))
      commonTimeNode.Speed = XmlConvert.ToInt32(reader.Value);
    if (reader.MoveToAttribute("syncBehavior"))
      commonTimeNode.SyncBehavior = AnimationConstant.GetTimeNodeSyncValue(reader.Value);
    if (reader.MoveToAttribute("tmFilter"))
      commonTimeNode.TimeFilter = reader.Value;
    reader.MoveToElement();
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "stCondLst":
            List<Condition> conditions1 = new List<Condition>();
            AnimParser.ParseConditionList(reader, conditions1);
            commonTimeNode.StartConditionList = conditions1;
            break;
          case "endCondLst":
            List<Condition> conditions2 = new List<Condition>();
            AnimParser.ParseConditionList(reader, conditions2);
            commonTimeNode.EndConditionList = conditions2;
            break;
          case "childTnLst":
            List<object> timeNodeList = new List<object>();
            AnimParser.ParseTimeNodeList(reader, timeNodeList, slide);
            commonTimeNode.ChildTimeNodeList = timeNodeList;
            break;
          case "endSync":
            commonTimeNode.EndSync = new Condition();
            if (reader.IsEmptyElement)
            {
              AnimParser.ParseNormalConditionsList(reader, commonTimeNode.EndSync);
              break;
            }
            AnimParser.ParseConditionsList(reader, commonTimeNode.EndSync);
            break;
          case "subTnLst":
            commonTimeNode.SubTimeNodeList = new List<object>();
            AnimParser.ParseTimeNodeList(reader, commonTimeNode.SubTimeNodeList, slide);
            break;
          case "iterate":
            commonTimeNode.Iterate = new Iterate();
            AnimParser.ParseIteration(reader, commonTimeNode.Iterate);
            break;
        }
        reader.Read();
      }
      else
        reader.Skip();
    }
  }

  private static void ParseIteration(XmlReader reader, Iterate iterate)
  {
    if (reader.MoveToAttribute("backwards"))
      iterate.Backwards = XmlConvert.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("type"))
      iterate.Type = AnimationConstant.GetIterateType(reader.Value);
    reader.MoveToElement();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "tmAbs":
            iterate.TimeAbsolute = new TimeAbsolute();
            if (reader.MoveToAttribute("val"))
              iterate.TimeAbsolute.Value = !(reader.Value == "indefinite") ? XmlConvert.ToInt32(reader.Value) : -1;
            reader.MoveToElement();
            break;
          case "tmPct":
            iterate.TimePercantage = new TimePercentage();
            if (reader.MoveToAttribute("val"))
              iterate.TimePercantage.Value = reader.Value;
            reader.MoveToElement();
            break;
        }
        reader.Read();
      }
      else
        reader.Skip();
    }
  }

  private static void ParseConditionList(XmlReader reader, List<Condition> conditions)
  {
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "cond":
            Condition condition = new Condition();
            if (reader.IsEmptyElement)
              AnimParser.ParseNormalConditionsList(reader, condition);
            else
              AnimParser.ParseConditionsList(reader, condition);
            conditions.Add(condition);
            break;
        }
        reader.Read();
      }
      else
        reader.Skip();
    }
  }

  private static void ParseConditionsList(XmlReader reader, Condition condition)
  {
    if (reader.MoveToAttribute("delay"))
      condition.Delay = !(reader.Value == "indefinite") ? XmlConvert.ToInt32(reader.Value) : -1;
    if (reader.MoveToAttribute("evt"))
      condition.Event = AnimationConstant.GetTriggerEvent(reader.Value);
    reader.MoveToElement();
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "rtn":
            condition.RunTimeNodeTrigger = new RunTimeNodeTrigger();
            if (reader.MoveToAttribute("val"))
              condition.RunTimeNodeTrigger.Val = AnimationConstant.GetTriggerRuntimeNode(reader.Value);
            reader.MoveToElement();
            break;
          case "tgtEl":
            condition.Target = AnimParser.ParseTargetElement(reader);
            break;
          case "tn":
            condition.TimeNode = new TimeNode();
            if (reader.MoveToAttribute("val"))
            {
              condition.TimeNode.Val = XmlConvert.ToUInt32(reader.Value);
              break;
            }
            break;
        }
        reader.Read();
      }
      else
        reader.Skip();
    }
  }

  private static void ParseNormalConditionsList(XmlReader reader, Condition condition)
  {
    if (reader.MoveToAttribute("delay"))
      condition.Delay = !(reader.Value == "indefinite") ? XmlConvert.ToInt32(reader.Value) : -1;
    if (reader.MoveToAttribute("evt"))
      condition.Event = AnimationConstant.GetTriggerEvent(reader.Value);
    reader.MoveToElement();
  }

  private static TargetElement ParseTargetElement(XmlReader reader)
  {
    string localName = reader.LocalName;
    reader.Read();
    TargetElement targetElement = new TargetElement();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "spTgt":
            targetElement.ShapeTarget = new ShapeTarget();
            AnimParser.ParseShapeTarget(reader, targetElement.ShapeTarget);
            break;
          case "sldTgt":
            targetElement.SlideTarget = new SlideTarget();
            break;
          case "inkTgt":
            targetElement.InkTarget = new InkTarget();
            if (reader.MoveToAttribute("spid"))
              targetElement.InkTarget.ShapeId = reader.Value;
            reader.MoveToElement();
            break;
        }
        reader.Read();
      }
      else
        reader.Skip();
    }
    return targetElement;
  }

  private static void ParseShapeTarget(XmlReader reader, ShapeTarget shapeTarget)
  {
    if (reader.MoveToAttribute("spid"))
      shapeTarget.ShapeId = reader.Value;
    reader.MoveToElement();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "bg":
            shapeTarget.Background = new BackGround();
            break;
          case "graphicEl":
            shapeTarget.GraphicElement = new GraphicElement();
            AnimParser.ParseGraphicTargetElement(reader, shapeTarget.GraphicElement);
            break;
          case "oleChartEl":
            shapeTarget.OleChartElement = new OleChartElement();
            if (reader.MoveToAttribute("lvl"))
              shapeTarget.OleChartElement.Level = XmlConvert.ToUInt32(reader.Value);
            if (reader.MoveToAttribute("type"))
              shapeTarget.OleChartElement.Type = AnimationConstant.GetTargetOleChartSubElement(reader.Value);
            reader.MoveToElement();
            break;
          case "subSp":
            shapeTarget.SubShape = new SubShape();
            if (reader.MoveToAttribute("spid"))
              shapeTarget.SubShape.ShapeId = reader.Value;
            reader.MoveToElement();
            break;
          case "txEl":
            shapeTarget.TextElement = new TextElement();
            AnimParser.ParseTargetTextElement(reader, shapeTarget.TextElement);
            break;
        }
        reader.Read();
      }
      else
        reader.Skip();
    }
  }

  private static void ParseGraphicTargetElement(XmlReader reader, GraphicElement graphicElement)
  {
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "chart":
            graphicElement.GraphicChart = new Chart();
            if (reader.MoveToAttribute("bldStep"))
              graphicElement.GraphicChart.AnimationBuildStep = AnimationConstant.GetTargetChartAnimationBuildStep(reader.Value);
            if (reader.MoveToAttribute("categoryIdx"))
              graphicElement.GraphicChart.CategoryIndex = XmlConvert.ToInt32(reader.Value);
            if (reader.MoveToAttribute("seriesIdx"))
              graphicElement.GraphicChart.SeriesIndex = XmlConvert.ToInt32(reader.Value);
            reader.MoveToElement();
            break;
          case "dgm":
            graphicElement.GraphicDiagram = new Diagram();
            if (reader.MoveToAttribute("bldStep"))
              graphicElement.GraphicDiagram.AnimationBuildStepDiagram = AnimationConstant.GetTargetDiagramBuildStep(reader.Value);
            if (reader.MoveToAttribute("id"))
              graphicElement.GraphicDiagram.Id = XmlConvert.ToGuid(reader.Value);
            reader.MoveToElement();
            break;
        }
        reader.Read();
      }
      else
        reader.Skip();
    }
  }

  private static void ParseTargetTextElement(XmlReader reader, TextElement textElement)
  {
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "charRg":
            textElement.CharacterRange = new RangeValues();
            if (reader.MoveToAttribute("end"))
              textElement.CharacterRange.End = XmlConvert.ToUInt32(reader.Value);
            if (reader.MoveToAttribute("st"))
              textElement.CharacterRange.Start = XmlConvert.ToUInt32(reader.Value);
            reader.MoveToElement();
            break;
          case "pRg":
            textElement.ParagraphRange = new RangeValues();
            if (reader.MoveToAttribute("end"))
              textElement.ParagraphRange.End = XmlConvert.ToUInt32(reader.Value);
            if (reader.MoveToAttribute("st"))
              textElement.ParagraphRange.Start = XmlConvert.ToUInt32(reader.Value);
            reader.MoveToElement();
            break;
        }
        reader.Read();
      }
      else
        reader.Skip();
    }
  }

  internal static List<object> GetAnimationInternalDOM(
    XmlReader reader,
    EffectPresetClassType presetClassType,
    EffectType effectType,
    EffectSubtype subtype,
    BaseSlide slide)
  {
    List<object> timeNodeList = new List<object>();
    reader.ReadToFollowing("p:" + presetClassType.ToString().ToLower());
    reader.ReadToFollowing("p:" + effectType.ToString().ToLower());
    reader.ReadToFollowing("p:" + subtype.ToString().ToLower());
    AnimParser.ParseTimeNodeList(reader, timeNodeList, slide);
    return timeNodeList;
  }

  internal static void ParseTimeNodeList(
    XmlReader reader,
    List<object> timeNodeList,
    BaseSlide slide)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "seq":
            SequenceTimeNode sequenceTime = new SequenceTimeNode();
            AnimParser.ParseSequenceTimeNode(reader, sequenceTime, slide);
            timeNodeList.Add((object) sequenceTime);
            break;
          case "par":
            ParallelTimeNode parallelTimeNode = new ParallelTimeNode();
            AnimParser.ParseParallelTimeNodeList(reader, parallelTimeNode, slide);
            timeNodeList.Add((object) parallelTimeNode);
            break;
          case "anim":
            PropertyEffect propertyEffect = new PropertyEffect();
            AnimParser.ParseAnimBehaviorEffect(reader, propertyEffect, slide);
            timeNodeList.Add((object) propertyEffect);
            break;
          case "animClr":
            ColorEffect colorEffect = new ColorEffect();
            AnimParser.ParseAnimColorEffect(reader, colorEffect, slide);
            timeNodeList.Add((object) colorEffect);
            break;
          case "animEffect":
            FilterEffect filterEffect = new FilterEffect();
            AnimParser.ParseAnimEffect(reader, filterEffect, slide);
            timeNodeList.Add((object) filterEffect);
            break;
          case "animMotion":
            MotionEffect motionEffect = new MotionEffect();
            AnimParser.ParseAnimMotionBehaviorEffect(reader, motionEffect, slide);
            timeNodeList.Add((object) motionEffect);
            break;
          case "animRot":
            RotationEffect rotationEffect = new RotationEffect();
            AnimParser.ParseAnimRotBehaviorEffect(reader, rotationEffect, slide);
            timeNodeList.Add((object) rotationEffect);
            break;
          case "animScale":
            ScaleEffect scaleEffect = new ScaleEffect();
            AnimParser.ParseAnimScaleBehaviorEffect(reader, scaleEffect, slide);
            timeNodeList.Add((object) scaleEffect);
            break;
          case "cmd":
            CommandEffect commandEffect = new CommandEffect();
            AnimParser.ParseCmdBehaviorEffect(reader, commandEffect, slide);
            timeNodeList.Add((object) commandEffect);
            break;
          case "set":
            SetEffect setEffect = new SetEffect();
            AnimParser.ParseSetBehaviorEffect(reader, setEffect, slide);
            timeNodeList.Add((object) setEffect);
            break;
        }
        reader.Read();
      }
      else
        reader.Skip();
    }
  }

  private static void ParseSequenceTimeNode(
    XmlReader reader,
    SequenceTimeNode sequenceTime,
    BaseSlide slide)
  {
    if (reader.MoveToAttribute("concurrent"))
      sequenceTime.Concurrent = XmlConvert.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("nextAc"))
      sequenceTime.NextAction = AnimationConstant.GetAction(reader.Value);
    if (reader.MoveToAttribute("prevAc"))
      sequenceTime.PreviousAction = AnimationConstant.GetPreviousAction(reader.Value);
    reader.MoveToElement();
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "cTn":
            CommonTimeNode commonTimeNode = new CommonTimeNode();
            AnimParser.ParseCommonTimeList(reader, commonTimeNode, slide);
            sequenceTime.CommonTimeNode = commonTimeNode;
            break;
          case "nextCondLst":
            List<Condition> conditions1 = new List<Condition>();
            AnimParser.ParseConditionList(reader, conditions1);
            sequenceTime.NextConditionList = conditions1;
            break;
          case "prevCondLst":
            List<Condition> conditions2 = new List<Condition>();
            AnimParser.ParseConditionList(reader, conditions2);
            sequenceTime.PreviousConditionList = conditions2;
            break;
        }
        reader.Read();
      }
      else
        reader.Skip();
    }
  }

  private static void ParseAnimBehaviorEffect(
    XmlReader reader,
    PropertyEffect propertyEffect,
    BaseSlide slide)
  {
    if (reader.MoveToAttribute("by"))
      propertyEffect.By = reader.Value;
    if (reader.MoveToAttribute("calcmode"))
      propertyEffect.CalcMode = AnimationConstant.GetCalcPropertyType(reader.Value);
    if (reader.MoveToAttribute("from"))
      propertyEffect.From = reader.Value;
    if (reader.MoveToAttribute("to"))
      propertyEffect.To = reader.Value;
    if (reader.MoveToAttribute("valueType"))
      propertyEffect.ValueType = AnimationConstant.GetPropertyValue(reader.Value);
    reader.MoveToElement();
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "cBhvr":
            propertyEffect.CommonBehavior = new CommonBehavior();
            AnimParser.ParseCommonBehavior(reader, propertyEffect.CommonBehavior, slide);
            break;
          case "tavLst":
            List<TimeAnimateValue> timeAnimatedList = new List<TimeAnimateValue>();
            AnimParser.ParseTimeAnimatedValueList(reader, timeAnimatedList);
            propertyEffect.TAVList = timeAnimatedList;
            break;
        }
        reader.Read();
      }
      else
        reader.Skip();
    }
  }

  private static void ParseTimeAnimatedValueList(
    XmlReader reader,
    List<TimeAnimateValue> timeAnimatedList)
  {
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        TimeAnimateValue timeAnimatedList1 = new TimeAnimateValue();
        AnimParser.ParseTimeAnimatedValue(reader, timeAnimatedList1);
        timeAnimatedList.Add(timeAnimatedList1);
        reader.Read();
      }
      else
        reader.Skip();
    }
  }

  private static void ParseTimeAnimatedValue(XmlReader reader, TimeAnimateValue timeAnimatedList)
  {
    if (reader.MoveToAttribute("fmla"))
      timeAnimatedList.Formula = reader.Value;
    if (reader.MoveToAttribute("tm"))
      timeAnimatedList.Time = !(reader.Value == "indefinite") ? XmlConvert.ToInt32(reader.Value) : -1;
    timeAnimatedList.TimeValue = new Values();
    reader.MoveToElement();
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "val":
            Values values = new Values();
            AnimParser.GetTimeAnimatedValuesList(reader, values);
            timeAnimatedList.TimeValue = values;
            break;
        }
        reader.Read();
      }
      else
        reader.Skip();
    }
  }

  private static void GetTimeAnimatedValuesList(XmlReader reader, Values value)
  {
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "boolVal":
            if (reader.MoveToAttribute("val"))
            {
              value.Bool = new bool?(XmlConvert.ToBoolean(reader.Value));
              break;
            }
            break;
          case "clrVal":
            value.Color = new ColorValues();
            AnimParser.ParseColorObject(reader, value.Color);
            break;
          case "fltVal":
            if (reader.MoveToAttribute("val"))
            {
              value.Float = new float?(reader.ReadContentAsFloat());
              break;
            }
            break;
          case "intVal":
            if (reader.MoveToAttribute("val"))
            {
              value.Int = new int?(XmlConvert.ToInt32(reader.Value));
              break;
            }
            break;
          case "strVal":
            if (reader.MoveToAttribute("val"))
            {
              value.String = reader.Value;
              break;
            }
            break;
        }
        reader.Read();
      }
      else
        reader.Skip();
    }
  }

  private static void ParseByColorValues(XmlReader reader, ColorOffset by)
  {
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "hsl":
            if (reader.MoveToAttribute("h"))
              by.Value0 = (float) XmlConvert.ToInt32(reader.Value) / 60000f;
            if (reader.MoveToAttribute("s"))
              by.Value1 = (float) XmlConvert.ToInt32(reader.Value) / 10000f;
            if (reader.MoveToAttribute("l"))
              by.Value2 = (float) XmlConvert.ToInt32(reader.Value) / 10000f;
            reader.MoveToElement();
            break;
          case "rgb":
            if (reader.MoveToAttribute("r"))
              by.Value0 = (float) XmlConvert.ToInt32(reader.Value);
            if (reader.MoveToAttribute("g"))
              by.Value1 = (float) XmlConvert.ToInt32(reader.Value);
            if (reader.MoveToAttribute("b"))
              by.Value2 = (float) XmlConvert.ToInt32(reader.Value);
            reader.MoveToElement();
            break;
        }
        reader.Read();
      }
      else
        reader.Skip();
    }
  }

  private static void ParseAnimColorEffect(
    XmlReader reader,
    ColorEffect colorEffect,
    BaseSlide slide)
  {
    if (reader.MoveToAttribute("clrSpc"))
      colorEffect.ColorSpace = AnimationConstant.GetColorSpaceValue(reader.Value);
    if (reader.MoveToAttribute("dir"))
      colorEffect.Direction = AnimationConstant.GetColorDirectionValue(reader.Value);
    reader.MoveToElement();
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "by":
            colorEffect.By = (IColorOffset) new ColorOffset();
            AnimParser.ParseByColorValues(reader, colorEffect.By as ColorOffset);
            break;
          case "cBhvr":
            colorEffect.CommonBehavior = new CommonBehavior();
            AnimParser.ParseCommonBehavior(reader, colorEffect.CommonBehavior, slide);
            colorEffect.TargetElement = colorEffect.CommonBehavior.Target;
            break;
          case "from":
            IMasterSlide masterSlide1 = (IMasterSlide) null;
            BaseSlide baseSlide1 = colorEffect.CommonBehavior.CommonTimeNode.BaseSlide;
            switch (baseSlide1)
            {
              case Slide _:
                masterSlide1 = (baseSlide1 as Slide).LayoutSlide.MasterSlide;
                break;
              case LayoutSlide _:
                masterSlide1 = (baseSlide1 as LayoutSlide).MasterSlide;
                break;
              case MasterSlide _:
                masterSlide1 = (IMasterSlide) (baseSlide1 as MasterSlide);
                break;
            }
            if (masterSlide1 != null)
            {
              colorEffect.From = (IColor) DrawingParser.ParseColorChoice(reader, masterSlide1 as MasterSlide);
              break;
            }
            break;
          case "to":
            IMasterSlide masterSlide2 = (IMasterSlide) null;
            BaseSlide baseSlide2 = colorEffect.CommonBehavior.CommonTimeNode.BaseSlide;
            switch (baseSlide2)
            {
              case Slide _:
                masterSlide2 = (baseSlide2 as Slide).LayoutSlide.MasterSlide;
                break;
              case LayoutSlide _:
                masterSlide2 = (baseSlide2 as LayoutSlide).MasterSlide;
                break;
              case MasterSlide _:
                masterSlide2 = (IMasterSlide) (baseSlide2 as MasterSlide);
                break;
            }
            if (masterSlide2 != null)
            {
              colorEffect.To = (IColor) DrawingParser.ParseColorChoice(reader, masterSlide2 as MasterSlide);
              break;
            }
            break;
        }
        if (reader.NodeType != XmlNodeType.Whitespace && reader.NodeType != XmlNodeType.EndElement)
          reader.Read();
      }
      else
        reader.Skip();
    }
  }

  private static void ParseColorObject(XmlReader reader, ColorValues color)
  {
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "hslClr":
            color.HSLColorValue = new HSLColor();
            AnimParser.ParseHSLColorValues(reader, color.HSLColorValue);
            break;
          case "prstClr":
            color.PresetColorValue = new PresetColor();
            if (reader.MoveToAttribute("val"))
            {
              color.PresetColorValue.Value = reader.Value;
              break;
            }
            break;
          case "schemeClr":
            color.SchemeColorValue = new PresetColor();
            if (reader.MoveToAttribute("val"))
            {
              color.SchemeColorValue.Value = reader.Value;
              break;
            }
            break;
          case "scrgbClr":
            color.SCrgbColorValue = new RGBColors();
            AnimParser.ParseRGBColorValues(reader, color.SCrgbColorValue);
            break;
          case "srgbClr":
            color.SrgbColorValue = new PresetColor();
            if (reader.MoveToAttribute("val"))
            {
              color.SrgbColorValue.Value = reader.Value;
              break;
            }
            break;
          case "sysClr":
            color.SystemColor = new PresetColor();
            if (reader.MoveToAttribute("val"))
            {
              color.SystemColor.Value = reader.Value;
              break;
            }
            break;
        }
        reader.Read();
      }
      else
        reader.Skip();
    }
  }

  private static void ParseRGBColorValues(XmlReader reader, RGBColors rgbValues)
  {
    if (reader.MoveToAttribute("r"))
      rgbValues.Red = reader.Value;
    if (reader.MoveToAttribute("g"))
      rgbValues.Green = reader.Value;
    if (reader.MoveToAttribute("b"))
      rgbValues.Blue = reader.Value;
    reader.MoveToElement();
  }

  private static void ParseHSLColorValues(XmlReader reader, HSLColor hslValues)
  {
    if (reader.MoveToAttribute("hue"))
      hslValues.Hue = (float) XmlConvert.ToInt32(reader.Value);
    if (reader.MoveToAttribute("sat"))
      hslValues.Saturation = (float) XmlConvert.ToInt32(reader.Value);
    if (reader.MoveToAttribute("lum"))
      hslValues.Luminance = (float) XmlConvert.ToInt32(reader.Value);
    reader.MoveToElement();
  }

  private static void ParseAnimEffect(XmlReader reader, FilterEffect filterEffect, BaseSlide slide)
  {
    if (reader.MoveToAttribute("filter"))
    {
      filterEffect.Type = AnimationConstant.GetFilterEffectType(reader.Value);
      if (reader.Value.Contains("("))
        AnimationConstant.SetFilterEffectSubType(reader.Value, filterEffect);
    }
    if (reader.MoveToAttribute("prLst"))
      filterEffect.PropEffect = reader.Value;
    if (reader.MoveToAttribute("transition"))
      filterEffect.Reveal = AnimationConstant.GetRevealTypeFilterEffect(reader.Value);
    reader.MoveToElement();
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "cBhvr":
            filterEffect.CommonBehavior = new CommonBehavior();
            AnimParser.ParseCommonBehavior(reader, filterEffect.CommonBehavior, slide);
            break;
          case "progress":
            filterEffect.AnimationValues = new Values();
            AnimParser.GetTimeAnimatedValuesList(reader, filterEffect.AnimationValues);
            break;
        }
        reader.Read();
      }
      else
        reader.Skip();
    }
  }

  private static void ParseAnimMotionBehaviorEffect(
    XmlReader reader,
    MotionEffect motionEffect,
    BaseSlide slide)
  {
    string pointsTypes = "";
    string attribute1 = reader.GetAttribute("origin");
    if (attribute1 != null)
    {
      motionEffect.Origin = AnimationConstant.GetMotionOriginType(attribute1);
      reader.MoveToElement();
    }
    string attribute2 = reader.GetAttribute("pathEditMode");
    if (attribute2 != null)
    {
      motionEffect.PathEditMode = AnimationConstant.GetMotionPathEditMode(attribute2);
      reader.MoveToElement();
    }
    if (reader.MoveToAttribute("ptsTypes"))
      pointsTypes = reader.Value;
    if (reader.MoveToAttribute("path"))
    {
      string pathValues = reader.Value;
      motionEffect.Path = (IMotionPath) new MotionPath();
      if (pathValues != null)
        AnimParser.ParseMotionPath(motionEffect.Path as MotionPath, pathValues, pointsTypes);
    }
    if (reader.MoveToAttribute("rAng"))
      motionEffect.Angle = (float) XmlConvert.ToInt32(reader.Value);
    reader.MoveToElement();
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      switch (reader.LocalName)
      {
        case "by":
          motionEffect.By = AnimParser.ParsePointFValues(reader);
          break;
        case "cBhvr":
          motionEffect.CommonBehavior = new CommonBehavior();
          AnimParser.ParseCommonBehavior(reader, motionEffect.CommonBehavior, slide);
          break;
        case "from":
          motionEffect.From = AnimParser.ParsePointFValues(reader);
          break;
        case "rCtr":
          motionEffect.RotationCenter = AnimParser.ParsePointFValues(reader);
          break;
        case "to":
          motionEffect.To = AnimParser.ParsePointFValues(reader);
          break;
      }
      reader.Read();
    }
  }

  private static void ParseMotionPath(MotionPath path, string pathValues, string pointsTypes)
  {
    string[] all = Array.FindAll<string>(pathValues.Split(' '), new Predicate<string>(AnimationConstant.IsNotNullOrEmpty));
    char[] charArray = pointsTypes.Replace(" ", string.Empty).ToCharArray();
    for (int index = 0; index < all.Length; ++index)
    {
      MotionCmdPath motionPath = (MotionCmdPath) null;
      switch (all[index])
      {
        case "M":
          motionPath = new MotionCmdPath();
          motionPath.SetCommandType(MotionCommandPathType.MoveTo);
          motionPath.IsRelative = false;
          PointF[] pointFArray1 = new PointF[1]
          {
            new PointF(Helper.ToFloat(all[index + 1].ToString()), Helper.ToFloat(all[index + 2].ToString()))
          };
          motionPath.Points = pointFArray1;
          index += 2;
          break;
        case "L":
          motionPath = new MotionCmdPath();
          motionPath.SetCommandType(MotionCommandPathType.LineTo);
          motionPath.IsRelative = false;
          PointF[] pointFArray2 = new PointF[1]
          {
            new PointF(Helper.ToFloat(all[index + 1].ToString()), Helper.ToFloat(all[index + 2].ToString()))
          };
          motionPath.Points = pointFArray2;
          index += 2;
          break;
        case "C":
          motionPath = new MotionCmdPath();
          motionPath.SetCommandType(MotionCommandPathType.CurveTo);
          motionPath.IsRelative = false;
          PointF[] pointFArray3 = new PointF[3]
          {
            new PointF(Helper.ToFloat(all[index + 1].ToString()), Helper.ToFloat(all[index + 2].ToString())),
            new PointF(Helper.ToFloat(all[index + 3].ToString()), Helper.ToFloat(all[index + 4].ToString())),
            new PointF(Helper.ToFloat(all[index + 5].ToString()), Helper.ToFloat(all[index + 6].ToString()))
          };
          motionPath.Points = pointFArray3;
          index += 6;
          break;
        case "Z":
          motionPath = new MotionCmdPath();
          motionPath.SetCommandType(MotionCommandPathType.CloseLoop);
          motionPath.IsRelative = false;
          break;
        case "E":
          motionPath = new MotionCmdPath();
          motionPath.SetCommandType(MotionCommandPathType.End);
          motionPath.IsRelative = false;
          break;
        case "m":
          motionPath = new MotionCmdPath();
          motionPath.SetCommandType(MotionCommandPathType.MoveTo);
          motionPath.IsRelative = true;
          PointF[] pointFArray4 = new PointF[1]
          {
            new PointF(Helper.ToFloat(all[index + 1].ToString()), Helper.ToFloat(all[index + 2].ToString()))
          };
          motionPath.Points = pointFArray4;
          index += 2;
          break;
        case "l":
          motionPath = new MotionCmdPath();
          motionPath.SetCommandType(MotionCommandPathType.LineTo);
          motionPath.IsRelative = true;
          PointF[] pointFArray5 = new PointF[1]
          {
            new PointF(Helper.ToFloat(all[index + 1].ToString()), Helper.ToFloat(all[index + 2].ToString()))
          };
          motionPath.Points = pointFArray5;
          index += 2;
          break;
        case "c":
          motionPath = new MotionCmdPath();
          motionPath.SetCommandType(MotionCommandPathType.CurveTo);
          motionPath.IsRelative = true;
          PointF[] pointFArray6 = new PointF[3]
          {
            new PointF(Helper.ToFloat(all[index + 1].ToString()), Helper.ToFloat(all[index + 2].ToString())),
            new PointF(Helper.ToFloat(all[index + 3].ToString()), Helper.ToFloat(all[index + 4].ToString())),
            new PointF(Helper.ToFloat(all[index + 5].ToString()), Helper.ToFloat(all[index + 6].ToString()))
          };
          motionPath.Points = pointFArray6;
          index += 6;
          break;
        case "z":
          motionPath = new MotionCmdPath();
          motionPath.SetCommandType(MotionCommandPathType.CloseLoop);
          motionPath.IsRelative = true;
          break;
        case "e":
          motionPath = new MotionCmdPath();
          motionPath.SetCommandType(MotionCommandPathType.End);
          motionPath.IsRelative = true;
          break;
      }
      if (charArray.Length > path.Count && motionPath != null && motionPath.CommandType != MotionCommandPathType.End)
        motionPath.PointsType = AnimationConstant.GetMotionPathPointsTypes(charArray[path.Count]);
      if (motionPath != null)
        path.Add((IMotionCmdPath) motionPath);
    }
  }

  private static void ParseAnimRotBehaviorEffect(
    XmlReader reader,
    RotationEffect rotationEffect,
    BaseSlide slide)
  {
    if (reader.MoveToAttribute("by"))
      rotationEffect.By = (float) (XmlConvert.ToInt32(reader.Value) / 60000);
    if (reader.MoveToAttribute("from"))
      rotationEffect.From = (float) (XmlConvert.ToInt32(reader.Value) / 60000);
    if (reader.MoveToAttribute("to"))
      rotationEffect.To = (float) (XmlConvert.ToInt32(reader.Value) / 60000);
    reader.MoveToElement();
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "cBhvr":
            rotationEffect.CommonBehavior = new CommonBehavior();
            AnimParser.ParseCommonBehavior(reader, rotationEffect.CommonBehavior, slide);
            break;
        }
        reader.Read();
      }
      else
        reader.Skip();
    }
  }

  private static void ParseAnimScaleBehaviorEffect(
    XmlReader reader,
    ScaleEffect scaleEffect,
    BaseSlide slide)
  {
    if (reader.MoveToAttribute("zoomContents"))
      scaleEffect.ZoomContent = AnimationConstant.GetNullableBoolValue(reader.Value);
    reader.MoveToElement();
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "by":
            scaleEffect.By = AnimParser.ParsePointFValues(reader);
            break;
          case "cBhvr":
            scaleEffect.CommonBehavior = new CommonBehavior();
            AnimParser.ParseCommonBehavior(reader, scaleEffect.CommonBehavior, slide);
            break;
          case "from":
            scaleEffect.From = AnimParser.ParsePointFValues(reader);
            break;
          case "to":
            scaleEffect.To = AnimParser.ParsePointFValues(reader);
            break;
        }
        reader.Read();
      }
      else
        reader.Skip();
    }
  }

  private static PointF ParsePointFValues(XmlReader reader)
  {
    PointF pointFvalues = new PointF();
    if (reader.MoveToAttribute("x"))
      pointFvalues.X = reader.ReadContentAsFloat() / 1000f;
    if (reader.MoveToAttribute("y"))
      pointFvalues.Y = reader.ReadContentAsFloat() / 1000f;
    return pointFvalues;
  }

  private static void ParseCmdBehaviorEffect(
    XmlReader reader,
    CommandEffect commandEffect,
    BaseSlide slide)
  {
    if (reader.MoveToAttribute("cmd"))
      commandEffect.CommandString = reader.Value;
    if (reader.MoveToAttribute("type"))
      commandEffect.Type = AnimationConstant.GetCommandEffectType(reader.Value);
    reader.MoveToElement();
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "cBhvr":
            commandEffect.CommonBehavior = new CommonBehavior();
            AnimParser.ParseCommonBehavior(reader, commandEffect.CommonBehavior, slide);
            break;
        }
        reader.Read();
      }
      else
        reader.Skip();
    }
  }

  private static void ParseSetBehaviorEffect(
    XmlReader reader,
    SetEffect setEffect,
    BaseSlide slide)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "cBhvr":
            setEffect.CommonBehavior = new CommonBehavior();
            AnimParser.ParseCommonBehavior(reader, setEffect.CommonBehavior, slide);
            break;
          case "to":
            setEffect.InternalTo = new Values();
            AnimParser.GetTimeAnimatedValuesList(reader, setEffect.InternalTo);
            break;
        }
        reader.Read();
      }
      else
        reader.Skip();
    }
  }

  private static void ParseCommonBehavior(
    XmlReader reader,
    CommonBehavior commonBehavior,
    BaseSlide slide)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.MoveToAttribute("accumulate"))
      commonBehavior.Accumulate = AnimationConstant.GetBehaviorType(reader.Value);
    if (reader.MoveToAttribute("additive"))
      commonBehavior.Additive = AnimationConstant.GetBehaviorAdditiveType(reader.Value);
    if (reader.MoveToAttribute("by"))
      commonBehavior.By = reader.Value;
    if (reader.MoveToAttribute("from"))
      commonBehavior.From = reader.Value;
    if (reader.MoveToAttribute("override"))
      commonBehavior.Override = AnimationConstant.GetBehaviorOverrideType(reader.Value);
    if (reader.MoveToAttribute("rctx"))
      commonBehavior.RunTimeContext = reader.Value;
    if (reader.MoveToAttribute("to"))
      commonBehavior.To = reader.Value;
    if (reader.MoveToAttribute("xfrmType"))
      commonBehavior.TransformType = AnimationConstant.GetBehaviorTransformType(reader.Value);
    reader.MoveToElement();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "attrNameLst":
            commonBehavior.AttributeNameList = new List<AttributeName>();
            AnimParser.ParseAttributeNameList(reader, commonBehavior.AttributeNameList);
            break;
          case "cTn":
            CommonTimeNode commonTimeNode = new CommonTimeNode();
            if (reader.IsEmptyElement)
              AnimParser.ParseCommonTimingValues(reader, commonTimeNode, slide);
            else
              AnimParser.ParseCommonTimeList(reader, commonTimeNode, slide);
            commonBehavior.CommonTimeNode = commonTimeNode;
            break;
          case "tgtEl":
            commonBehavior.Target = AnimParser.ParseTargetElement(reader);
            break;
        }
        reader.Read();
      }
      else
        reader.Skip();
    }
  }

  private static void ParseCommonTimingValues(
    XmlReader reader,
    CommonTimeNode timeValue,
    BaseSlide slide)
  {
    timeValue.BaseSlide = slide;
    timeValue.GroupId = -1f;
    timeValue.PresetSubtype = -1f;
    if (reader.MoveToAttribute("dur"))
      timeValue.Duration = reader.Value;
    if (reader.MoveToAttribute("presetID"))
      timeValue.PresetId = (float) XmlConvert.ToInt32(reader.Value);
    if (reader.MoveToAttribute("accel"))
      timeValue.Acceleration = XmlConvert.ToInt32(reader.Value);
    if (reader.MoveToAttribute("afterEffect"))
      timeValue.AfterEffect = XmlConvert.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("autoRev"))
      timeValue.AutoReverse = XmlConvert.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("bldLvl"))
      timeValue.BuildLevel = XmlConvert.ToInt32(reader.Value);
    if (reader.MoveToAttribute("decel"))
      timeValue.Deceleration = XmlConvert.ToInt32(reader.Value);
    if (reader.MoveToAttribute("display"))
      timeValue.Display = XmlConvert.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("evtFilter"))
      timeValue.EventFilter = reader.Value;
    if (reader.MoveToAttribute("fill"))
      timeValue.Fill = AnimationConstant.GetTimeNodeFillValue(reader.Value);
    if (reader.MoveToAttribute("grpId"))
      timeValue.GroupId = (float) XmlConvert.ToInt32(reader.Value);
    if (reader.MoveToAttribute("id"))
      timeValue.Id = XmlConvert.ToUInt32(reader.Value);
    if (reader.MoveToAttribute("nodePh"))
      timeValue.NodePlaceholder = XmlConvert.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("nodeType"))
      timeValue.NodeType = AnimationConstant.GetTimeNode(reader.Value);
    if (reader.MoveToAttribute("presetClass"))
      timeValue.PresetClass = AnimationConstant.GetPresetClassType(reader.Value);
    if (reader.MoveToAttribute("presetSubtype"))
      timeValue.PresetSubtype = (float) XmlConvert.ToInt32(reader.Value);
    if (reader.MoveToAttribute("repeatCount"))
      timeValue.RepeatCount = reader.Value;
    if (reader.MoveToAttribute("repeatDur"))
      timeValue.RepeatDuration = reader.Value;
    if (reader.MoveToAttribute("restart"))
      timeValue.Restart = AnimationConstant.GetRestartType(reader.Value);
    if (reader.MoveToAttribute("spd"))
      timeValue.Speed = XmlConvert.ToInt32(reader.Value);
    if (reader.MoveToAttribute("syncBehavior"))
      timeValue.SyncBehavior = AnimationConstant.GetTimeNodeSyncValue(reader.Value);
    if (!reader.MoveToAttribute("tmFilter"))
      return;
    timeValue.TimeFilter = reader.Value;
  }

  private static void ParseAttributeNameList(
    XmlReader reader,
    List<AttributeName> attributeNameList)
  {
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (!reader.Value.Contains("\n") && !string.IsNullOrEmpty(reader.Value))
        attributeNameList.Add(new AttributeName()
        {
          Name = reader.Value
        });
      reader.Read();
    }
  }

  private static void ParseBuildList(XmlReader reader, List<Build> buildList, BaseSlide slide)
  {
    Build build = new Build();
    build.BuildElements = new List<object>();
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      string localName1 = reader.LocalName;
      reader.Read();
      while (!(localName1 == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "bldP":
              BuildParagraph buildParagraph = new BuildParagraph();
              if (reader.MoveToAttribute("spid"))
                buildParagraph.ShapeId = reader.Value;
              if (reader.MoveToAttribute("grpId"))
                buildParagraph.GroupId = XmlConvert.ToUInt32(reader.Value);
              if (reader.MoveToAttribute("animBg"))
                buildParagraph.AnimateBackground = XmlConvert.ToBoolean(reader.Value);
              if (reader.MoveToAttribute("advAuto"))
                buildParagraph.AutoAdvanceTime = XmlConvert.ToInt32(reader.Value);
              if (reader.MoveToAttribute("autoUpdateAnimBg"))
                buildParagraph.AutoUpdateAnimBackground = XmlConvert.ToBoolean(reader.Value);
              if (reader.MoveToAttribute("bldLvl"))
                buildParagraph.BuildLevel = XmlConvert.ToUInt32(reader.Value);
              if (reader.MoveToAttribute("build"))
                buildParagraph.BuildType = AnimationConstant.GetParagraphBuildType(reader.Value);
              if (reader.MoveToAttribute("rev"))
                buildParagraph.Reverse = XmlConvert.ToBoolean(reader.Value);
              if (reader.MoveToAttribute("uiExpand"))
                buildParagraph.ExpandUI = XmlConvert.ToBoolean(reader.Value);
              reader.MoveToElement();
              if (!reader.IsEmptyElement)
              {
                reader.Read();
                string localName2 = reader.LocalName;
                while (!(localName2 == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
                {
                  if (reader.NodeType == XmlNodeType.Element)
                  {
                    switch (reader.LocalName)
                    {
                      case "tmplLst":
                        buildParagraph.TemplateList = new List<TemplateEffects>();
                        AnimParser.ParseTemplateEffects(reader, buildParagraph.TemplateList, slide);
                        break;
                    }
                    reader.Read();
                  }
                  else
                    reader.Skip();
                }
              }
              build.BuildElements.Add((object) buildParagraph);
              break;
            case "bldDgm":
              BuildDiagram buildDiagram = new BuildDiagram();
              if (reader.MoveToAttribute("bld"))
                buildDiagram.Build = AnimationConstant.GetDiagramBuildType(reader.Value);
              if (reader.MoveToAttribute("grpId"))
                buildDiagram.GroupId = XmlConvert.ToUInt32(reader.Value);
              if (reader.MoveToAttribute("spid"))
                buildDiagram.ShapeId = XmlConvert.ToUInt32(reader.Value);
              if (reader.MoveToAttribute("uiExpand"))
                buildDiagram.UiExpand = XmlConvert.ToBoolean(reader.Value);
              build.BuildElements.Add((object) buildDiagram);
              break;
            case "bldGraphic":
              BuildGraphics buildGraphic = new BuildGraphics();
              AnimParser.ParseBuildGraphicElement(reader, buildGraphic);
              build.BuildElements.Add((object) buildGraphic);
              break;
            case "bldOleChart":
              BuildOleChart buildOleChart = new BuildOleChart();
              if (reader.MoveToAttribute("bld"))
                buildOleChart.Build = AnimationConstant.GetOleChartBuildType(reader.Value);
              if (reader.MoveToAttribute("grpId"))
                buildOleChart.GroupId = XmlConvert.ToUInt32(reader.Value);
              if (reader.MoveToAttribute("spid"))
                buildOleChart.ShapeId = XmlConvert.ToUInt32(reader.Value);
              if (reader.MoveToAttribute("uiExpand"))
                buildOleChart.UiExpand = XmlConvert.ToBoolean(reader.Value);
              build.BuildElements.Add((object) buildOleChart);
              break;
            default:
              reader.Skip();
              break;
          }
          reader.Read();
        }
        else
          reader.Skip();
      }
      buildList.Add(build);
    }
    reader.Read();
  }

  private static void ParseBuildGraphicElement(XmlReader reader, BuildGraphics buildGraphic)
  {
    if (reader.MoveToAttribute("grpId"))
      buildGraphic.GroupId = XmlConvert.ToUInt32(reader.Value);
    if (reader.MoveToAttribute("spid"))
      buildGraphic.ShapeId = XmlConvert.ToUInt32(reader.Value);
    if (reader.MoveToAttribute("uiExpand"))
      buildGraphic.IsUiExpand = XmlConvert.ToBoolean(reader.Value);
    reader.MoveToElement();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "bldAsOne":
            buildGraphic.BuildAsOne = new BuildAsOne();
            break;
          case "bldSub":
            buildGraphic.BuildSubElements = new BuildSubElements();
            AnimParser.ParseBuildSubElements(reader, buildGraphic.BuildSubElements);
            break;
        }
        reader.Read();
      }
      else
        reader.Skip();
    }
  }

  private static void ParseBuildSubElements(XmlReader reader, BuildSubElements buildSubElement)
  {
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "bldChart":
            buildSubElement.Chart = new BuildSubElementChart();
            if (reader.MoveToAttribute("animBg"))
              buildSubElement.Chart.AnimateBackground = XmlConvert.ToBoolean(reader.Value);
            if (reader.MoveToAttribute("bld"))
            {
              buildSubElement.Chart.Build = AnimationConstant.GetOleChartBuildType(reader.Value);
              break;
            }
            break;
          case "bldDgm":
            buildSubElement.Diagram = new BuildSubElementDiagram();
            if (reader.MoveToAttribute("rev"))
              buildSubElement.Diagram.IsReverseAnimation = XmlConvert.ToBoolean(reader.Value);
            if (reader.MoveToAttribute("bld"))
            {
              buildSubElement.Diagram.BuildType = AnimationConstant.GetAnimationDiagramBuildType(reader.Value);
              break;
            }
            break;
        }
        reader.Read();
      }
      else
        reader.Skip();
    }
  }

  private static void ParseTemplateEffects(
    XmlReader reader,
    List<TemplateEffects> templateEffects,
    BaseSlide slide)
  {
    if (reader.IsEmptyElement)
      return;
    reader.Read();
    string localName = reader.LocalName;
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "tmpl":
            TemplateEffects templateEffects1 = new TemplateEffects();
            AnimParser.ParseTemplate(reader, templateEffects1.Template, slide);
            templateEffects.Add(templateEffects1);
            break;
        }
        reader.Read();
      }
      else
        reader.Skip();
    }
  }

  private static void ParseTemplate(XmlReader reader, Template template, BaseSlide slide)
  {
    if (reader.MoveToAttribute("lvl"))
      template.Level = XmlConvert.ToUInt32(reader.Value);
    if (reader.IsEmptyElement)
      return;
    reader.Read();
    string localName = reader.LocalName;
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "tnLst":
            template.TimeNodeList = new List<object>();
            AnimParser.ParseTimeNodeList(reader, template.TimeNodeList, slide);
            continue;
          default:
            continue;
        }
      }
      else
        reader.Skip();
    }
  }
}
