// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Charts.ChartAxisParser
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Compression;
using Syncfusion.XlsIO.Implementation.Charts;
using Syncfusion.XlsIO.Implementation.XmlReaders;
using Syncfusion.XlsIO.Implementation.XmlReaders.Shapes;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Interfaces.Charts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization.Charts;

public class ChartAxisParser
{
  public const string DefaultFont = "Calibri";
  public const float DefaultFontSize = 10f;
  private Dictionary<string, ExcelTickLabelPosition> s_dictTickLabelToAttributeValue = new Dictionary<string, ExcelTickLabelPosition>(4);
  internal Dictionary<string, ExcelTickMark> s_dictTickMarkToAttributeValue = new Dictionary<string, ExcelTickMark>(4);
  private WorkbookImpl m_book;

  public ChartAxisParser()
  {
    if (this.s_dictTickLabelToAttributeValue.Count != 0)
      return;
    this.s_dictTickLabelToAttributeValue.Add("high", ExcelTickLabelPosition.TickLabelPosition_High);
    this.s_dictTickLabelToAttributeValue.Add("low", ExcelTickLabelPosition.TickLabelPosition_Low);
    this.s_dictTickLabelToAttributeValue.Add("nextTo", ExcelTickLabelPosition.TickLabelPosition_NextToAxis);
    this.s_dictTickLabelToAttributeValue.Add("none", ExcelTickLabelPosition.TickLabelPosition_None);
    this.s_dictTickMarkToAttributeValue.Add("none", ExcelTickMark.TickMark_None);
    this.s_dictTickMarkToAttributeValue.Add("in", ExcelTickMark.TickMark_Inside);
    this.s_dictTickMarkToAttributeValue.Add("out", ExcelTickMark.TickMark_Outside);
    this.s_dictTickMarkToAttributeValue.Add("cross", ExcelTickMark.TickMark_Cross);
  }

  public ChartAxisParser(WorkbookImpl book)
    : this()
  {
    this.m_book = book;
    ChartParserCommon.SetWorkbook(book);
  }

  public void ParseDateAxis(
    XmlReader reader,
    ChartCategoryAxisImpl axis,
    RelationCollection relations,
    ExcelChartType chartType,
    Excel2007Parser parser)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (axis == null)
      throw new ArgumentNullException(nameof (axis));
    if (reader.LocalName != "dateAx")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    axis.CategoryType = ExcelCategoryType.Time;
    this.ParseAxisCommon(reader, (ChartAxisImpl) axis, relations, chartType, parser, new ChartAxisParser.AxisTagsParser(this.DateAxisTagParsing));
    reader.Read();
  }

  public void ParseCategoryAxis(
    XmlReader reader,
    ChartCategoryAxisImpl axis,
    RelationCollection relations,
    ExcelChartType chartType,
    Excel2007Parser parser)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (axis == null)
      throw new ArgumentNullException(nameof (axis));
    if (reader.LocalName != "catAx")
      throw new XmlException(nameof (reader));
    reader.Read();
    axis.IsAutoMajor = true;
    axis.IsAutoMinor = true;
    axis.CategoryType = ExcelCategoryType.Category;
    this.ParseAxisCommon(reader, (ChartAxisImpl) axis, relations, chartType, parser, new ChartAxisParser.AxisTagsParser(this.CategoryAxisTagParsing));
    reader.Read();
  }

  public void ParseValueAxis(
    XmlReader reader,
    ChartValueAxisImpl valueAxis,
    RelationCollection relations,
    ExcelChartType chartType,
    Excel2007Parser parser)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (valueAxis == null)
      throw new ArgumentNullException(nameof (valueAxis));
    if (reader.LocalName != "valAx")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    this.ParseAxisCommon(reader, (ChartAxisImpl) valueAxis, relations, chartType, parser, new ChartAxisParser.AxisTagsParser(this.ValueAxisTagParsing));
    reader.Read();
  }

  public void ParseSeriesAxis(
    XmlReader reader,
    ChartSeriesAxisImpl seriesAxis,
    RelationCollection relations,
    ExcelChartType chartType,
    Excel2007Parser parser)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (seriesAxis == null)
      throw new ArgumentNullException(nameof (seriesAxis));
    if (reader.LocalName != "serAx")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    seriesAxis.AutoTickLabelSpacing = true;
    seriesAxis.AutoTickMarkSpacing = true;
    this.ParseAxisCommon(reader, (ChartAxisImpl) seriesAxis, relations, chartType, parser, new ChartAxisParser.AxisTagsParser(this.SeriesAxisTagParsing));
    reader.Read();
  }

  private void ParseAxisCommon(
    XmlReader reader,
    ChartAxisImpl axis,
    RelationCollection chartItemRelations,
    ExcelChartType chartType,
    Excel2007Parser parser,
    ChartAxisParser.AxisTagsParser unknownTagParser)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    ChartImpl chartImpl = axis != null ? axis.ParentChart : throw new ArgumentNullException(nameof (axis));
    FileDataHolder parentHolder = chartImpl.DataHolder.ParentHolder;
    RelationCollection relations = chartImpl.Relations;
    bool flag1 = true;
    ChartAxisScale chartAxisScale = (ChartAxisScale) null;
    axis.Visible = true;
    int num = -1;
    bool? nullable = new bool?();
    axis.Visible = true;
    axis.SetDefaultFont("Calibri", 10f);
    bool flag2 = false;
    MemoryStream memoryStream = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((Stream) memoryStream, Encoding.UTF8);
    writer.WriteStartElement("root");
    while (reader.NodeType != XmlNodeType.EndElement && flag1)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "axId":
            num = ChartParserCommon.ParseIntValueTag(reader);
            continue;
          case "scaling":
            chartAxisScale = this.ParseScaling(reader);
            continue;
          case "delete":
            nullable = new bool?(!ChartParserCommon.ParseBoolValueTag(reader));
            continue;
          case "axPos":
            string axisPosition1 = this.ParseAxisPosition(reader, axis);
            ChartAxisPos chartAxisPos = (ChartAxisPos) Enum.Parse(typeof (ChartAxisPos), axisPosition1, false);
            if ((chartType == ExcelChartType.Scatter_Markers || chartType == ExcelChartType.Bubble) && (axisPosition1 == "b" || axisPosition1 == "t"))
            {
              axis = (ChartAxisImpl) ((axis.IsPrimary ? axis.ParentChart.PrimaryCategoryAxis : axis.ParentChart.SecondaryCategoryAxis) as ChartValueAxisImpl);
              axis.Visible = true;
            }
            axis.AxisPosition = new ChartAxisPos?(chartAxisPos);
            continue;
          case "majorGridlines":
            axis.HasMajorGridLines = true;
            this.ParseGridlines(reader, axis.MajorGridLines, parentHolder, chartItemRelations);
            continue;
          case "minorGridlines":
            axis.HasMinorGridLines = true;
            this.ParseGridlines(reader, axis.MinorGridLines, parentHolder, chartItemRelations);
            continue;
          case "title":
            writer.WriteNode(reader, false);
            writer.Flush();
            flag2 = true;
            continue;
          case "numFmt":
            this.ParseNumberFormat(reader, axis);
            continue;
          case "majorTickMark":
            axis.MajorTickMark = this.ParseTickMark(reader);
            continue;
          case "minorTickMark":
            axis.MinorTickMark = this.ParseTickMark(reader);
            continue;
          case "tickLblPos":
            this.ParseTickLabel(reader, axis);
            continue;
          case "crossAx":
            this.ParseCrossAxis(reader, axis);
            continue;
          case "crosses":
            this.ParseCrossesTag(reader, axis);
            continue;
          case "crossesAt":
            if (ChartAxisSerializator.GetPairAxis(axis) is ChartValueAxisImpl pairAxis)
            {
              pairAxis.CrossesAt = ChartParserCommon.ParseDoubleValueTag(reader);
              continue;
            }
            (axis as ChartSeriesAxisImpl).CrossesAt = ChartParserCommon.ParseIntValueTag(reader);
            continue;
          case "spPr":
            ChartInteriorImpl interior = axis.FrameFormat.Interior as ChartInteriorImpl;
            interior.UseAutomaticFormat = false;
            ChartFillImpl fill = axis.FrameFormat.Fill as ChartFillImpl;
            IChartFillObjectGetter objectGetter = (IChartFillObjectGetter) new ChartFillObjectGetterAny(axis.FrameFormat.Border as ChartBorderImpl, interior, (IInternalFill) fill, axis.ShadowProperties as ShadowImpl, axis.FrameFormat.ThreeD as ThreeDFormatImpl);
            ChartParserCommon.ParseShapeProperties(reader, objectGetter, parentHolder, chartItemRelations);
            axis.AssignReference(axis.FrameFormat.Border);
            continue;
          case "txPr":
            axis.ParagraphType = ChartParagraphType.CustomDefault;
            Stream data = ShapeParser.ReadNodeAsStream(reader);
            data.Position = 0L;
            axis.TextStream = data;
            this.ParseTextSettings(UtilityMethods.CreateReader(data), axis, parser);
            continue;
          case "lblAlgn":
            switch (ChartParserCommon.ParseValueTag(reader))
            {
              case "l":
                axis.LabelAlign = AxisLabelAlignment.Left;
                continue;
              case "r":
                axis.LabelAlign = AxisLabelAlignment.Right;
                continue;
              default:
                axis.LabelAlign = AxisLabelAlignment.Center;
                continue;
            }
          default:
            if (unknownTagParser != null)
            {
              unknownTagParser(reader, axis, relations);
              continue;
            }
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    writer.WriteEndElement();
    writer.Flush();
    if (flag2)
    {
      IInternalChartTextArea titleArea = axis.TitleArea as IInternalChartTextArea;
      ChartParserCommon.ParseChartTitleElement((Stream) memoryStream, titleArea, parentHolder, relations, 10f);
      if (titleArea is ChartTextAreaImpl && !titleArea.HasTextRotation)
      {
        ChartAxisPos? axisPosition2 = axis.AxisPosition;
        if ((axisPosition2.GetValueOrDefault() != ChartAxisPos.l ? 0 : (axisPosition2.HasValue ? 1 : 0)) == 0)
        {
          ChartAxisPos? axisPosition3 = axis.AxisPosition;
          if ((axisPosition3.GetValueOrDefault() != ChartAxisPos.r ? 0 : (axisPosition3.HasValue ? 1 : 0)) == 0)
            goto label_42;
        }
        titleArea.TextRotationAngle = -90;
      }
    }
label_42:
    axis.AxisId = num;
    if (nullable.HasValue)
      axis.Deleted = !nullable.Value;
    else if (axis.Deleted)
      axis.Deleted = false;
    chartAxisScale?.CopyTo(axis as IScalable);
  }

  internal void ParseTextSettings(XmlReader reader, ChartAxisImpl axis, Excel2007Parser parser)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (axis == null)
      throw new ArgumentNullException(nameof (axis));
    if (reader.LocalName != "txPr")
      throw new XmlException();
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "bodyPr":
              this.ParseBodyProperties(reader, axis);
              continue;
            case "p":
              this.ParseAxisParagraphs(reader, axis, parser);
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

  private void ParseAxisParagraphs(XmlReader reader, ChartAxisImpl axis, Excel2007Parser parser)
  {
    while (!(reader.LocalName == "p") || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "defRPr")
      {
        bool flag = false;
        if (reader.IsEmptyElement)
          flag = true;
        TextSettings paragraphProperties = ChartParserCommon.ParseDefaultParagraphProperties(reader, parser, new float?(10f));
        ChartParserCommon.CopyDefaultSettings(axis.Font as IInternalFont, paragraphProperties);
        if (flag)
          axis.IsDefaultTextSettings = true;
      }
      else
        reader.Read();
    }
    reader.Read();
  }

  private void ParseBodyProperties(XmlReader reader, ChartAxisImpl axis)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (axis == null)
      throw new ArgumentNullException(nameof (axis));
    if (reader.LocalName != "bodyPr")
      throw new XmlException();
    if (reader.MoveToAttribute("rot"))
      axis.TextRotationAngle = XmlConvertExtension.ToInt32(reader.Value) / 60000;
    if (reader.MoveToAttribute("vert"))
      axis.m_textRotation = this.ParseTextRotation(reader);
    reader.MoveToElement();
    reader.Skip();
  }

  private Excel2007TextRotation ParseTextRotation(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "vert")
      throw new XmlException("Unexpected xml tag.");
    Excel2007TextRotation textRotation;
    switch (reader.Value)
    {
      case "wordArtVert":
        textRotation = Excel2007TextRotation.wordArtVert;
        break;
      case "vert":
        textRotation = Excel2007TextRotation.vert;
        break;
      case "vert270":
        textRotation = Excel2007TextRotation.vert270;
        break;
      case "eaVert":
        textRotation = Excel2007TextRotation.vert;
        break;
      case "mongolianVert":
        textRotation = Excel2007TextRotation.mongolianVert;
        break;
      case "wordArtVertRtl":
        textRotation = Excel2007TextRotation.wordArtVertRtl;
        break;
      default:
        textRotation = Excel2007TextRotation.horz;
        break;
    }
    return textRotation;
  }

  private void ParseCrossesTag(XmlReader reader, ChartAxisImpl axis)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (axis == null)
      throw new ArgumentNullException(nameof (axis));
    if (reader.LocalName != "crosses")
      throw new XmlException("Unexpected xml tag.");
    if (!(ChartParserCommon.ParseValueTag(reader) == "max"))
      return;
    (ChartAxisSerializator.GetPairAxis(axis) as ChartValueAxisImpl).IsMaxCross = true;
  }

  private void ParseCrossAxis(XmlReader reader, ChartAxisImpl axis)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (axis == null)
      throw new ArgumentNullException(nameof (axis));
    if (reader.LocalName != "crossAx")
      throw new XmlException("Unexpected xml tag.");
    reader.Skip();
  }

  private ExcelTickMark ParseTickMark(XmlReader reader)
  {
    return reader != null ? this.s_dictTickMarkToAttributeValue[ChartParserCommon.ParseValueTag(reader)] : throw new ArgumentNullException(nameof (reader));
  }

  private void ParseTickLabel(XmlReader reader, ChartAxisImpl axis)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (axis == null)
      throw new ArgumentNullException(nameof (axis));
    string key = !(reader.LocalName != "tickLblPos") ? ChartParserCommon.ParseValueTag(reader) : throw new XmlException("Unexpected xml tag.");
    axis.TickLabelPosition = this.s_dictTickLabelToAttributeValue[key];
  }

  internal void ParseNumberFormat(XmlReader reader, ChartAxisImpl axis)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (axis == null)
      throw new ArgumentNullException(nameof (axis));
    if (reader.LocalName != "numFmt")
      throw new XmlException("Unexpected xml tag.");
    if (reader.MoveToAttribute("formatCode"))
      axis.NumberFormat = reader.Value;
    if (reader.MoveToAttribute("sourceLinked"))
      axis.IsSourceLinked = XmlConvertExtension.ToBoolean(reader.Value);
    reader.Read();
  }

  internal void ParseGridlines(
    XmlReader reader,
    IChartGridLine gridLines,
    FileDataHolder dataHolder,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (gridLines == null)
      throw new ArgumentNullException(nameof (gridLines));
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "spPr")
        {
          ChartFillObjectGetterAny objectGetter = new ChartFillObjectGetterAny(gridLines.Border as ChartBorderImpl, (ChartInteriorImpl) null, (IInternalFill) null, gridLines.Shadow as ShadowImpl, gridLines.ThreeD as ThreeDFormatImpl);
          ChartParserCommon.ParseShapeProperties(reader, (IChartFillObjectGetter) objectGetter, dataHolder, relations);
        }
        else
          reader.Skip();
      }
    }
    reader.Read();
  }

  private string ParseAxisPosition(XmlReader reader, ChartAxisImpl axis)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (axis == null)
      throw new ArgumentNullException("valueAxis");
    return ChartParserCommon.ParseValueTag(reader);
  }

  private ChartAxisScale ParseScaling(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "scaling")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    ChartAxisScale scaling = new ChartAxisScale();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "logBase":
            reader.Read();
            scaling.LogScale = new bool?(true);
            continue;
          case "orientation":
            string str = "minMax";
            if (reader.MoveToAttribute("val"))
              str = ChartParserCommon.ParseValueTag(reader);
            else
              reader.Skip();
            if (str == "maxMin")
            {
              scaling.Reversed = new bool?(true);
              continue;
            }
            continue;
          case "max":
            scaling.MaximumValue = new double?(ChartParserCommon.ParseDoubleValueTag(reader));
            continue;
          case "min":
            scaling.MinimumValue = new double?(ChartParserCommon.ParseDoubleValueTag(reader));
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    reader.Read();
    return scaling;
  }

  private void ParseDisplayUnit(
    XmlReader reader,
    ChartValueAxisImpl valueAxis,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (valueAxis == null)
      throw new ArgumentNullException(nameof (valueAxis));
    if (reader.LocalName != "dispUnits")
      throw new XmlException();
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "builtInUnit":
              this.ParseBuiltInDisplayUnit(reader, valueAxis);
              continue;
            case "dispUnitsLbl":
              valueAxis.HasDisplayUnitLabel = true;
              IInternalChartTextArea displayUnitLabel = valueAxis.DisplayUnitLabel as IInternalChartTextArea;
              FileDataHolder dataHolder = valueAxis.ParentChart.ParentWorkbook.DataHolder;
              ChartParserCommon.ParseTextArea(reader, displayUnitLabel, dataHolder, relations);
              continue;
            case "custUnit":
              valueAxis.DisplayUnitCustom = ChartParserCommon.ParseDoubleValueTag(reader);
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

  private void ParseBuiltInDisplayUnit(XmlReader reader, ChartValueAxisImpl valueAxis)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (valueAxis == null)
      throw new ArgumentNullException(nameof (valueAxis));
    Excel2007ChartDisplayUnit chartDisplayUnit = (Excel2007ChartDisplayUnit) Enum.Parse(typeof (Excel2007ChartDisplayUnit), ChartParserCommon.ParseValueTag(reader), false);
    valueAxis.DisplayUnit = (ExcelChartDisplayUnit) chartDisplayUnit;
  }

  private void CategoryAxisTagParsing(
    XmlReader reader,
    ChartAxisImpl axis,
    RelationCollection relations)
  {
    if (reader.NodeType == XmlNodeType.Element)
    {
      ChartCategoryAxisImpl categoryAxisImpl = axis as ChartCategoryAxisImpl;
      switch (reader.LocalName)
      {
        case "lblOffset":
          if (reader.MoveToAttribute("val"))
          {
            categoryAxisImpl.Offset = ChartParserCommon.ParseIntValueTag(reader);
            break;
          }
          categoryAxisImpl.Offset = 100;
          reader.Skip();
          break;
        case "tickLblSkip":
          categoryAxisImpl.TickLabelSpacing = ChartParserCommon.ParseIntValueTag(reader);
          (axis as ChartCategoryAxisImpl).AutoTickLabelSpacing = false;
          break;
        case "tickMarkSkip":
          categoryAxisImpl.TickMarkSpacing = ChartParserCommon.ParseIntValueTag(reader);
          break;
        case "auto":
          categoryAxisImpl.CategoryType = ChartParserCommon.ParseBoolValueTag(reader) ? ExcelCategoryType.Automatic : ExcelCategoryType.Category;
          break;
        case "majorUnit":
          categoryAxisImpl.MajorUnit = ChartParserCommon.ParseDoubleValueTag(reader);
          break;
        case "minorUnit":
          categoryAxisImpl.MinorUnit = ChartParserCommon.ParseDoubleValueTag(reader);
          break;
        case "noMultiLvlLbl":
          categoryAxisImpl.NoMultiLevelLabel = ChartParserCommon.ParseBoolValueTag(reader);
          categoryAxisImpl.m_showNoMultiLvlLbl = true;
          break;
        default:
          reader.Skip();
          break;
      }
    }
    else
      reader.Skip();
  }

  private void DateAxisTagParsing(
    XmlReader reader,
    ChartAxisImpl axis,
    RelationCollection relations)
  {
    if (reader.NodeType == XmlNodeType.Element)
    {
      ChartCategoryAxisImpl categoryAxisImpl = axis as ChartCategoryAxisImpl;
      switch (reader.LocalName)
      {
        case "lblOffset":
          if (reader.MoveToAttribute("val"))
          {
            categoryAxisImpl.Offset = ChartParserCommon.ParseIntValueTag(reader);
            break;
          }
          categoryAxisImpl.Offset = 100;
          reader.Skip();
          break;
        case "majorUnit":
          categoryAxisImpl.MajorUnit = ChartParserCommon.ParseDoubleValueTag(reader);
          break;
        case "minorUnit":
          categoryAxisImpl.MinorUnit = ChartParserCommon.ParseDoubleValueTag(reader);
          break;
        case "minorTimeUnit":
          string valueTag1 = ChartParserCommon.ParseValueTag(reader);
          ((ChartCategoryAxisImpl) axis).MinorUnitScale = this.GetChartBaseUnitFromString(valueTag1);
          ((ChartCategoryAxisImpl) axis).MinorUnitScaleIsAuto = false;
          break;
        case "majorTimeUnit":
          string valueTag2 = ChartParserCommon.ParseValueTag(reader);
          ((ChartCategoryAxisImpl) axis).MajorUnitScale = this.GetChartBaseUnitFromString(valueTag2);
          ((ChartCategoryAxisImpl) axis).MajorUnitScaleIsAuto = false;
          break;
        case "baseTimeUnit":
          string valueTag3 = ChartParserCommon.ParseValueTag(reader);
          ((ChartCategoryAxisImpl) axis).BaseUnit = this.GetChartBaseUnitFromString(valueTag3);
          ((ChartCategoryAxisImpl) axis).BaseUnitIsAuto = false;
          break;
        default:
          reader.Skip();
          break;
      }
    }
    else
      reader.Skip();
  }

  private void ValueAxisTagParsing(
    XmlReader reader,
    ChartAxisImpl axis,
    RelationCollection relations)
  {
    if (reader.NodeType == XmlNodeType.Element)
    {
      ChartValueAxisImpl valueAxis = axis as ChartValueAxisImpl;
      switch (reader.LocalName)
      {
        case "crossBetween":
          string valueTag = ChartParserCommon.ParseValueTag(reader);
          ChartImpl parentChart = valueAxis.ParentChart;
          (valueAxis.IsPrimary ? parentChart.PrimaryCategoryAxis : parentChart.SecondaryCategoryAxis).IsBetween = valueTag == "between";
          break;
        case "majorUnit":
          valueAxis.SetMajorUnit(ChartParserCommon.ParseDoubleValueTag(reader));
          break;
        case "minorUnit":
          valueAxis.SetMinorUnit(ChartParserCommon.ParseDoubleValueTag(reader));
          break;
        case "dispUnits":
          this.ParseDisplayUnit(reader, valueAxis, relations);
          break;
        default:
          reader.Skip();
          break;
      }
    }
    else
      reader.Skip();
  }

  private void SeriesAxisTagParsing(
    XmlReader reader,
    ChartAxisImpl axis,
    RelationCollection relations)
  {
    if (reader.NodeType == XmlNodeType.Element)
    {
      ChartSeriesAxisImpl chartSeriesAxisImpl = axis as ChartSeriesAxisImpl;
      switch (reader.LocalName)
      {
        case "tickLblSkip":
          chartSeriesAxisImpl.TickLabelSpacing = ChartParserCommon.ParseIntValueTag(reader);
          (axis as ChartSeriesAxisImpl).AutoTickLabelSpacing = false;
          break;
        case "tickMarkSkip":
          chartSeriesAxisImpl.TickMarkSpacing = ChartParserCommon.ParseIntValueTag(reader);
          break;
        default:
          reader.Skip();
          break;
      }
    }
    else
      reader.Skip();
  }

  private ExcelChartBaseUnit GetChartBaseUnitFromString(string baseUnitScale)
  {
    baseUnitScale = this.PrepareBaseUnitScale(baseUnitScale);
    return (ExcelChartBaseUnit) Enum.Parse(typeof (ExcelChartBaseUnit), baseUnitScale, false);
  }

  private string PrepareBaseUnitScale(string baseUnitScale)
  {
    baseUnitScale = this.RemoveCharUnSafeAtLast(baseUnitScale);
    return char.ToUpper(baseUnitScale[0]).ToString() + baseUnitScale.Substring(1);
  }

  private string RemoveCharUnSafeAtLast(string baseUnitScale)
  {
    return baseUnitScale.Substring(0, baseUnitScale.Length - 1);
  }

  private delegate void AxisTagsParser(
    XmlReader reader,
    ChartAxisImpl axis,
    RelationCollection relations);
}
