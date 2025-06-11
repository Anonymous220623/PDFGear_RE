// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Charts.ChartExAxisParser
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
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization.Charts;

internal class ChartExAxisParser(WorkbookImpl book) : ChartAxisParser(book)
{
  internal void ParseChartExAxis(
    XmlReader reader,
    ChartAxisImpl axis,
    ChartImpl chart,
    RelationCollection relations,
    Excel2007Parser parser,
    FileDataHolder dataHolder)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (axis == null)
      throw new ArgumentNullException(nameof (axis));
    if (reader.LocalName != nameof (axis))
      throw new XmlException("Unexpected xml tag");
    reader.Read();
    axis.Visible = true;
    axis.SetDefaultFont("Calibri", 10f);
    axis.MajorTickMark = ExcelTickMark.TickMark_None;
    axis.MinorTickMark = ExcelTickMark.TickMark_None;
    bool flag1 = false;
    bool flag2 = false;
    MemoryStream memoryStream = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((Stream) memoryStream, Encoding.UTF8);
    writer.WriteStartElement("root");
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "title":
            writer.WriteNode(reader, false);
            writer.WriteEndElement();
            writer.Flush();
            flag1 = true;
            continue;
          case "units":
            this.ParseDisplayUnits(reader, axis as ChartValueAxisImpl, relations);
            continue;
          case "majorGridlines":
            axis.HasMajorGridLines = true;
            this.ParseGridlines(reader, axis.MajorGridLines, dataHolder, relations);
            continue;
          case "minorGridlines":
            axis.HasMinorGridLines = true;
            this.ParseGridlines(reader, axis.MinorGridLines, dataHolder, relations);
            continue;
          case "majorTickMarks":
            axis.MajorTickMark = this.ParseTickMark(reader);
            continue;
          case "minorTickMarks":
            axis.MinorTickMark = this.ParseTickMark(reader);
            continue;
          case "spPr":
            if (!reader.IsEmptyElement)
            {
              ChartInteriorImpl interior = axis.FrameFormat.Interior as ChartInteriorImpl;
              interior.UseAutomaticFormat = false;
              ChartFillImpl fill = axis.FrameFormat.Fill as ChartFillImpl;
              IChartFillObjectGetter objectGetter = (IChartFillObjectGetter) new ChartFillObjectGetterAny(axis.FrameFormat.Border as ChartBorderImpl, interior, (IInternalFill) fill, axis.ShadowProperties as ShadowImpl, axis.FrameFormat.ThreeD as ThreeDFormatImpl);
              ChartParserCommon.ParseShapeProperties(reader, objectGetter, dataHolder, relations);
              axis.AssignReference(axis.FrameFormat.Border);
              continue;
            }
            reader.Skip();
            continue;
          case "txPr":
            axis.ParagraphType = ChartParagraphType.CustomDefault;
            Stream data = ShapeParser.ReadNodeAsStream(reader);
            data.Position = 0L;
            axis.TextStream = data;
            this.ParseTextSettings(UtilityMethods.CreateReader(data), axis, parser);
            continue;
          case "tickLabels":
            flag2 = true;
            reader.Skip();
            continue;
          case "numFmt":
            this.ParseNumberFormat(reader, axis);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    if (flag1)
    {
      IInternalChartTextArea titleArea = axis.TitleArea as IInternalChartTextArea;
      titleArea.Bold = false;
      ChartParserCommon.ParseChartTitleElement((Stream) memoryStream, titleArea, dataHolder, relations, 10f);
    }
    if (flag2)
      return;
    axis.TickLabelPosition = ExcelTickLabelPosition.TickLabelPosition_None;
  }

  private void ParseDisplayUnits(
    XmlReader reader,
    ChartValueAxisImpl valueAxis,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (valueAxis == null)
      throw new ArgumentNullException(nameof (valueAxis));
    if (reader.LocalName != "units")
      throw new XmlException();
    valueAxis.HasDisplayUnitLabel = true;
    if (reader.MoveToAttribute("unit"))
    {
      Excel2007ChartDisplayUnit chartDisplayUnit = (Excel2007ChartDisplayUnit) Enum.Parse(typeof (Excel2007ChartDisplayUnit), reader.Value, false);
      if (chartDisplayUnit == Excel2007ChartDisplayUnit.percentage)
      {
        valueAxis.DisplayUnit = ExcelChartDisplayUnit.Custom;
        valueAxis.NumberFormat = "0%";
        valueAxis.IsSourceLinked = false;
      }
      else
        valueAxis.DisplayUnit = (ExcelChartDisplayUnit) chartDisplayUnit;
    }
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      MemoryStream memoryStream = new MemoryStream();
      XmlWriter writer = UtilityMethods.CreateWriter((Stream) memoryStream, Encoding.UTF8);
      writer.WriteStartElement("root");
      writer.WriteNode(reader, false);
      writer.WriteEndElement();
      writer.Flush();
      IInternalChartTextArea displayUnitLabel = valueAxis.DisplayUnitLabel as IInternalChartTextArea;
      FileDataHolder dataHolder = valueAxis.ParentChart.ParentWorkbook.DataHolder;
      ChartParserCommon.ParseChartTitleElement((Stream) memoryStream, displayUnitLabel, dataHolder, relations, 10f);
    }
    else
      reader.Skip();
  }

  internal void ParseAxisCommonAttributes(
    XmlReader reader,
    out bool? axisIsHidden,
    out int? axisId)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "axis")
      throw new XmlException("Unexpected xml tag");
    axisId = new int?();
    axisIsHidden = new bool?();
    if (reader.MoveToAttribute("hidden"))
      axisIsHidden = new bool?(XmlConvertExtension.ToBoolean(reader.Value));
    if (reader.MoveToAttribute("id"))
      axisId = new int?(XmlConvertExtension.ToInt32(reader.Value));
    reader.MoveToElement();
  }

  internal void ParseAxisAttributes(XmlReader reader, ChartAxisImpl axis, bool isValueAxis)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "valScaling" && reader.LocalName != "catScaling")
      throw new XmlException("Unexpected xml tag");
    if (axis == null)
      throw new ArgumentNullException(nameof (axis));
    if (isValueAxis)
    {
      ChartValueAxisImpl chartValueAxisImpl = axis as ChartValueAxisImpl;
      chartValueAxisImpl.IsAutoMax = chartValueAxisImpl.IsAutoMin = true;
      chartValueAxisImpl.IsAutoMinor = chartValueAxisImpl.IsAutoMajor = true;
      if (reader.MoveToAttribute("min") && reader.Value != "auto")
        chartValueAxisImpl.MinimumValue = XmlConvertExtension.ToDouble(reader.Value);
      if (reader.MoveToAttribute("max") && reader.Value != "auto")
        chartValueAxisImpl.MaximumValue = XmlConvertExtension.ToDouble(reader.Value);
      if (reader.MoveToAttribute("minorUnit") && reader.Value != "auto")
        chartValueAxisImpl.MinorUnit = XmlConvertExtension.ToDouble(reader.Value);
      if (reader.MoveToAttribute("majorUnit") && reader.Value != "auto")
        chartValueAxisImpl.MajorUnit = XmlConvertExtension.ToDouble(reader.Value);
    }
    else if (reader.MoveToAttribute("gapWidth") && reader.Value != "auto")
    {
      double num = XmlConvertExtension.ToDouble(reader.Value);
      axis.ParentChart.PrimaryFormats[0].GapWidth = (int) Math.Round(num * 100.0);
    }
    reader.MoveToElement();
  }

  private ExcelTickMark ParseTickMark(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    ExcelTickMark tickMark = ExcelTickMark.TickMark_Cross;
    if (reader.MoveToAttribute("type"))
      tickMark = this.s_dictTickMarkToAttributeValue[reader.Value];
    return tickMark;
  }
}
