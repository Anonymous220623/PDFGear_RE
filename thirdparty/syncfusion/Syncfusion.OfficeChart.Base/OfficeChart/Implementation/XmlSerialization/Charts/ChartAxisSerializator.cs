// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.XmlSerialization.Charts.ChartAxisSerializator
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Charts;
using Syncfusion.OfficeChart.Implementation.XmlReaders.Shapes;
using System;
using System.Collections.Generic;
using System.Security;
using System.Xml;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.XmlSerialization.Charts;

internal class ChartAxisSerializator
{
  public const int TextRotationMultiplier = 60000;
  private static Dictionary<OfficeTickLabelPosition, string> s_dictTickLabelToAttributeValue = new Dictionary<OfficeTickLabelPosition, string>(4);
  private static Dictionary<OfficeTickMark, string> s_dictTickMarkToAttributeValue = new Dictionary<OfficeTickMark, string>(4);

  static ChartAxisSerializator()
  {
    ChartAxisSerializator.s_dictTickLabelToAttributeValue.Add(OfficeTickLabelPosition.TickLabelPosition_High, "high");
    ChartAxisSerializator.s_dictTickLabelToAttributeValue.Add(OfficeTickLabelPosition.TickLabelPosition_Low, "low");
    ChartAxisSerializator.s_dictTickLabelToAttributeValue.Add(OfficeTickLabelPosition.TickLabelPosition_NextToAxis, "nextTo");
    ChartAxisSerializator.s_dictTickLabelToAttributeValue.Add(OfficeTickLabelPosition.TickLabelPosition_None, "none");
    ChartAxisSerializator.s_dictTickMarkToAttributeValue.Add(OfficeTickMark.TickMark_None, "none");
    ChartAxisSerializator.s_dictTickMarkToAttributeValue.Add(OfficeTickMark.TickMark_Inside, "in");
    ChartAxisSerializator.s_dictTickMarkToAttributeValue.Add(OfficeTickMark.TickMark_Outside, "out");
    ChartAxisSerializator.s_dictTickMarkToAttributeValue.Add(OfficeTickMark.TickMark_Cross, "cross");
  }

  [SecurityCritical]
  public void SerializeAxis(XmlWriter writer, IOfficeChartAxis axis, RelationCollection relations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (axis == null)
      return;
    switch (axis.AxisType)
    {
      case OfficeAxisType.Category:
        ChartCategoryAxisImpl axis1 = (ChartCategoryAxisImpl) axis;
        if (!axis1.IsChartBubbleOrScatter)
        {
          if (axis1.CategoryType == OfficeCategoryType.Time)
          {
            this.SerializeDateAxis(writer, axis1);
            break;
          }
          this.SerializeCategoryAxis(writer, axis1);
          break;
        }
        this.SerializeValueAxis(writer, (ChartValueAxisImpl) axis, relations);
        break;
      case OfficeAxisType.Value:
        this.SerializeValueAxis(writer, (ChartValueAxisImpl) axis, relations);
        break;
      case OfficeAxisType.Serie:
        this.SerializeSeriesAxis(writer, (ChartSeriesAxisImpl) axis);
        break;
      default:
        throw new NotSupportedException();
    }
  }

  [SecurityCritical]
  private void SerializeDateAxis(XmlWriter writer, ChartCategoryAxisImpl axis)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (axis == null)
      throw new ArgumentNullException(nameof (axis));
    writer.WriteStartElement("dateAx", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    this.SerializeAxisCommon(writer, (ChartAxisImpl) axis);
    if (axis.CategoryType == OfficeCategoryType.Automatic)
      ChartSerializatorCommon.SerializeBoolValueTag(writer, "auto", true);
    else
      ChartSerializatorCommon.SerializeBoolValueTag(writer, "auto", false);
    string str1 = axis.LabelAlign == AxisLabelAlignment.Left ? "l" : (axis.LabelAlign == AxisLabelAlignment.Right ? "r" : "ctr");
    ChartSerializatorCommon.SerializeValueTag(writer, "lblAlgn", str1);
    ChartSerializatorCommon.SerializeValueTag(writer, "lblOffset", axis.Offset.ToString());
    if (!axis.IsAutoMajor)
      ChartSerializatorCommon.SerializeValueTag(writer, "majorUnit", XmlConvert.ToString(axis.MajorUnit));
    if (!axis.MajorUnitScaleIsAuto || (axis.ParentChart.Workbook as WorkbookImpl).IsConverted)
    {
      string str2 = this.ConvertDateUnitToString(axis.MajorUnitScale);
      ChartSerializatorCommon.SerializeValueTag(writer, "majorTimeUnit", str2);
    }
    if (!axis.IsAutoMinor)
      ChartSerializatorCommon.SerializeValueTag(writer, "minorUnit", XmlConvert.ToString(axis.MinorUnit));
    if (!axis.MinorUnitScaleIsAuto || (axis.ParentChart.Workbook as WorkbookImpl).IsConverted)
    {
      string str3 = this.ConvertDateUnitToString(axis.MinorUnitScale);
      ChartSerializatorCommon.SerializeValueTag(writer, "minorTimeUnit", str3);
    }
    if (!axis.BaseUnitIsAuto)
    {
      string str4 = this.ConvertDateUnitToString(axis.BaseUnit);
      ChartSerializatorCommon.SerializeValueTag(writer, "baseTimeUnit", str4);
    }
    writer.WriteEndElement();
  }

  private string ConvertDateUnitToString(OfficeChartBaseUnit baseUnit)
  {
    return baseUnit.ToString().ToLower() + (object) 's';
  }

  [SecurityCritical]
  private void SerializeCategoryAxis(XmlWriter writer, ChartCategoryAxisImpl axis)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (axis == null)
      throw new ArgumentNullException(nameof (axis));
    writer.WriteStartElement("catAx", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    this.SerializeAxisCommon(writer, (ChartAxisImpl) axis);
    if (axis.CategoryType == OfficeCategoryType.Automatic)
      ChartSerializatorCommon.SerializeBoolValueTag(writer, "auto", true);
    else
      ChartSerializatorCommon.SerializeBoolValueTag(writer, "auto", false);
    string str = axis.LabelAlign == AxisLabelAlignment.Left ? "l" : (axis.LabelAlign == AxisLabelAlignment.Right ? "r" : "ctr");
    ChartSerializatorCommon.SerializeValueTag(writer, "lblAlgn", str);
    ChartSerializatorCommon.SerializeValueTag(writer, "lblOffset", axis.Offset.ToString());
    ChartSerializatorCommon.SerializeBoolValueTag(writer, "noMultiLvlLbl", axis.NoMultiLevelLabel);
    if (!axis.IsAutoMajor || !axis.AutoTickLabelSpacing)
      ChartSerializatorCommon.SerializeValueTag(writer, "tickLblSkip", axis.TickLabelSpacing.ToString());
    if (!axis.IsAutoMinor || !axis.AutoTickMarkSpacing)
      ChartSerializatorCommon.SerializeValueTag(writer, "tickMarkSkip", axis.TickMarkSpacing.ToString());
    writer.WriteEndElement();
  }

  [SecurityCritical]
  private void SerializeValueAxis(
    XmlWriter writer,
    ChartValueAxisImpl valueAxis,
    RelationCollection relations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (valueAxis == null)
      throw new ArgumentNullException(nameof (valueAxis));
    writer.WriteStartElement("valAx", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    this.SerializeAxisCommon(writer, (ChartAxisImpl) valueAxis);
    ChartImpl parentChart = valueAxis.ParentChart;
    string str = (valueAxis.IsPrimary ? parentChart.PrimaryCategoryAxis : parentChart.SecondaryCategoryAxis).IsBetween ? "between" : "midCat";
    ChartSerializatorCommon.SerializeValueTag(writer, "crossBetween", str);
    if (!valueAxis.IsAutoMajor)
      ChartSerializatorCommon.SerializeValueTag(writer, "majorUnit", XmlConvert.ToString(valueAxis.MajorUnit));
    if (!valueAxis.IsAutoMinor)
      ChartSerializatorCommon.SerializeValueTag(writer, "minorUnit", XmlConvert.ToString(valueAxis.MinorUnit));
    this.SerializeDisplayUnit(writer, valueAxis, relations);
    writer.WriteEndElement();
  }

  [SecurityCritical]
  private void SerializeSeriesAxis(XmlWriter writer, ChartSeriesAxisImpl seriesAxis)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (seriesAxis == null)
      throw new ArgumentNullException(nameof (seriesAxis));
    writer.WriteStartElement("serAx", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    this.SerializeAxisCommon(writer, (ChartAxisImpl) seriesAxis);
    if (!seriesAxis.AutoTickLabelSpacing)
      ChartSerializatorCommon.SerializeValueTag(writer, "tickLblSkip", seriesAxis.TickLabelSpacing.ToString());
    if (!seriesAxis.AutoTickMarkSpacing)
      ChartSerializatorCommon.SerializeValueTag(writer, "tickMarkSkip", seriesAxis.TickMarkSpacing.ToString());
    writer.WriteEndElement();
  }

  [SecurityCritical]
  private void SerializeAxisCommon(XmlWriter writer, ChartAxisImpl axis)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    ChartImpl chartImpl = axis != null ? axis.ParentChart : throw new ArgumentNullException(nameof (axis));
    FileDataHolder parentHolder = chartImpl.DataHolder.ParentHolder;
    RelationCollection relations = chartImpl.Relations;
    ChartSerializatorCommon.SerializeValueTag(writer, "axId", axis.AxisId.ToString());
    this.SerializeScaling(writer, axis);
    ChartSerializatorCommon.SerializeBoolValueTag(writer, "delete", axis.Deleted);
    this.SerializeAxisPosition(writer, axis);
    this.SerializeGridlines(writer, axis);
    if (axis.HasAxisTitle)
      ChartSerializatorCommon.SerializeTextArea(writer, axis.TitleArea, chartImpl.ParentWorkbook, relations, 10.0);
    bool isPrimary = axis.IsPrimary;
    if (axis.isNumber)
      this.SerializeNumberFormat(writer, axis);
    else if (axis.AxisType == OfficeAxisType.Value && (isPrimary ? axis.ParentChart.PrimaryFormats : axis.ParentChart.SecondaryFormats).IsPercentStackedAxis)
    {
      writer.WriteStartElement("numFmt", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      writer.WriteAttributeString("formatCode", "0%");
      writer.WriteAttributeString("sourceLinked", "1");
      writer.WriteEndElement();
    }
    this.SerializeTickMark(writer, "majorTickMark", axis.MajorTickMark);
    this.SerializeTickMark(writer, "minorTickMark", axis.MinorTickMark);
    this.SerializeTickLabel(writer, axis);
    if (axis.FrameFormat.HasInterior && axis.FrameFormat.Interior.Pattern != OfficePattern.None)
    {
      if (axis.FrameFormat.Shadow.ShadowInnerPresets == Office2007ChartPresetsInner.NoShadow && axis.FrameFormat.Shadow.ShadowOuterPresets == Office2007ChartPresetsOuter.NoShadow && axis.FrameFormat.Shadow.ShadowPerspectivePresets == Office2007ChartPresetsPerspective.NoShadow && (axis.Shadow.ShadowInnerPresets != Office2007ChartPresetsInner.NoShadow || axis.Shadow.ShadowOuterPresets != Office2007ChartPresetsOuter.NoShadow || axis.Shadow.ShadowPerspectivePresets != Office2007ChartPresetsPerspective.NoShadow))
      {
        if (axis.Shadow.ShadowInnerPresets != Office2007ChartPresetsInner.NoShadow)
          axis.FrameFormat.Shadow.ShadowInnerPresets = axis.Shadow.ShadowInnerPresets;
        else if (axis.Shadow.ShadowOuterPresets != Office2007ChartPresetsOuter.NoShadow)
          axis.FrameFormat.Shadow.ShadowOuterPresets = axis.Shadow.ShadowOuterPresets;
        else if (axis.Shadow.ShadowPerspectivePresets != Office2007ChartPresetsPerspective.NoShadow)
          axis.FrameFormat.Shadow.ShadowPerspectivePresets = axis.Shadow.ShadowPerspectivePresets;
        axis.FrameFormat.Shadow.Angle = axis.Shadow.Angle;
        axis.FrameFormat.Shadow.Blur = axis.Shadow.Blur;
        axis.FrameFormat.Shadow.Distance = axis.Shadow.Distance;
        axis.FrameFormat.Shadow.HasCustomShadowStyle = axis.Shadow.HasCustomShadowStyle;
        axis.FrameFormat.Shadow.ShadowColor = axis.Shadow.ShadowColor;
        if (axis.Shadow.ShadowInnerPresets == Office2007ChartPresetsInner.NoShadow && axis.Shadow.Size != 0)
          axis.FrameFormat.Shadow.Size = axis.Shadow.Size;
        axis.FrameFormat.Shadow.Transparency = axis.Shadow.Transparency;
      }
      ChartSerializatorCommon.SerializeFrameFormat(writer, (IOfficeChartFillBorder) axis.FrameFormat, axis.ParentChart, false);
    }
    else if ((axis.FrameFormat.HasLineProperties || (axis.Border as ChartBorderImpl).HasLineProperties) && !axis.Border.AutoFormat)
    {
      writer.WriteStartElement("spPr", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      ChartSerializatorCommon.SerializeLineProperties(writer, axis.Border, (IWorkbook) parentHolder.Workbook);
      if (axis.Shadow.ShadowInnerPresets != Office2007ChartPresetsInner.NoShadow || axis.Shadow.ShadowOuterPresets != Office2007ChartPresetsOuter.NoShadow || axis.Shadow.ShadowPerspectivePresets != Office2007ChartPresetsPerspective.NoShadow)
        ChartSerializatorCommon.SerializeShadow(writer, axis.Shadow, axis.Shadow.HasCustomShadowStyle);
      writer.WriteEndElement();
    }
    if (axis.ParagraphType == ChartParagraphType.CustomDefault || !axis.IsAutoTextRotation)
      this.SerializeTextSettings(writer, axis);
    this.SerializeCrossAxis(writer, axis);
    string str = "autoZero";
    string tagName = "crosses";
    if (ChartAxisSerializator.GetPairAxis(axis) is ChartValueAxisImpl pairAxis)
    {
      if (pairAxis.IsMaxCross)
        str = "max";
      else if (!pairAxis.IsAutoCross)
      {
        str = XmlConvert.ToString(pairAxis.CrossesAt);
        tagName = "crossesAt";
      }
    }
    ChartSerializatorCommon.SerializeValueTag(writer, tagName, str);
  }

  public static IOfficeChartAxis GetPairAxis(ChartAxisImpl axis)
  {
    IOfficeChartAxis pairAxis = (IOfficeChartAxis) null;
    if (axis != null)
    {
      ChartImpl parentChart = axis.ParentChart;
      switch (axis.AxisType)
      {
        case OfficeAxisType.Category:
          pairAxis = axis.IsPrimary ? (IOfficeChartAxis) parentChart.PrimaryValueAxis : (IOfficeChartAxis) parentChart.SecondaryValueAxis;
          break;
        case OfficeAxisType.Value:
          pairAxis = axis.IsPrimary ? (IOfficeChartAxis) parentChart.PrimaryCategoryAxis : (IOfficeChartAxis) parentChart.SecondaryCategoryAxis;
          break;
      }
    }
    return pairAxis;
  }

  private void SerializeCrossAxis(XmlWriter writer, ChartAxisImpl axis)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    ChartImpl chartImpl = axis != null ? axis.ParentChart : throw new ArgumentNullException(nameof (axis));
    bool isPrimary = axis.IsPrimary;
    ChartAxisImpl chartAxisImpl;
    switch (axis.AxisType)
    {
      case OfficeAxisType.Category:
        chartAxisImpl = isPrimary ? (ChartAxisImpl) chartImpl.PrimaryValueAxis : (ChartAxisImpl) chartImpl.SecondaryValueAxis;
        break;
      case OfficeAxisType.Value:
        chartAxisImpl = isPrimary ? (ChartAxisImpl) chartImpl.PrimaryCategoryAxis : (ChartAxisImpl) chartImpl.SecondaryCategoryAxis;
        break;
      case OfficeAxisType.Serie:
        chartAxisImpl = (ChartAxisImpl) chartImpl.PrimaryValueAxis;
        break;
      default:
        throw new InvalidOperationException();
    }
    ChartSerializatorCommon.SerializeValueTag(writer, "crossAx", chartAxisImpl.AxisId.ToString());
  }

  private void SerializeTickMark(XmlWriter writer, string tagName, OfficeTickMark tickMark)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (tagName == null || tagName.Length == 0)
      throw new ArgumentException(nameof (tagName));
    string str = ChartAxisSerializator.s_dictTickMarkToAttributeValue[tickMark];
    ChartSerializatorCommon.SerializeValueTag(writer, tagName, str);
  }

  private void SerializeTickLabel(XmlWriter writer, ChartAxisImpl axis)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (axis == null)
      throw new ArgumentNullException(nameof (axis));
    string str = ChartAxisSerializator.s_dictTickLabelToAttributeValue[axis.TickLabelPosition];
    ChartSerializatorCommon.SerializeValueTag(writer, "tickLblPos", str);
  }

  private void SerializeNumberFormat(XmlWriter writer, ChartAxisImpl axis)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (axis == null)
      throw new ArgumentNullException(nameof (axis));
    writer.WriteStartElement("numFmt", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteAttributeString("formatCode", axis.NumberFormat);
    bool isSourceLinked = axis.IsSourceLinked;
    Excel2007Serializator.SerializeAttribute(writer, "sourceLinked", isSourceLinked, !isSourceLinked);
    writer.WriteEndElement();
  }

  private void SerializeGridlines(XmlWriter writer, ChartAxisImpl axis)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (axis == null)
      throw new ArgumentNullException(nameof (axis));
    WorkbookImpl parentWorkbook = axis.ParentChart.ParentWorkbook;
    if (axis.HasMajorGridLines)
      this.SerializeGridlines(writer, axis.MajorGridLines, "majorGridlines", (IWorkbook) parentWorkbook);
    if (!axis.HasMinorGridLines)
      return;
    this.SerializeGridlines(writer, axis.MinorGridLines, "minorGridlines", (IWorkbook) parentWorkbook);
  }

  private void SerializeGridlines(
    XmlWriter writer,
    IOfficeChartGridLine gridLines,
    string tagName,
    IWorkbook book)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (gridLines == null)
      throw new ArgumentNullException(nameof (gridLines));
    if (tagName == null || tagName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (tagName));
    writer.WriteStartElement(tagName, "http://schemas.openxmlformats.org/drawingml/2006/chart");
    IOfficeChartBorder border = gridLines.Border;
    if (border != null && !border.AutoFormat)
    {
      writer.WriteStartElement("spPr", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      ChartSerializatorCommon.SerializeLineProperties(writer, border, book);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private void SerializeAxisPosition(XmlWriter writer, ChartAxisImpl axis)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (axis == null)
      throw new ArgumentNullException("valueAxis");
    string str = axis.AxisPosition.HasValue ? axis.AxisPosition.ToString() : (string) null;
    bool isPrimary = axis.IsPrimary;
    bool isBarChartAxes = (isPrimary ? axis.ParentChart.PrimaryFormats : axis.ParentChart.SecondaryFormats).IsBarChartAxes;
    if (str == null)
    {
      switch (axis.AxisType)
      {
        case OfficeAxisType.Category:
          str = isPrimary ? (isBarChartAxes ? "l" : "b") : (isBarChartAxes ? "r" : "t");
          break;
        case OfficeAxisType.Value:
          str = isPrimary ? (isBarChartAxes ? "b" : "l") : (isBarChartAxes ? "t" : "r");
          break;
        case OfficeAxisType.Serie:
          str = isPrimary ? "b" : "t";
          break;
      }
    }
    if (str == null)
      return;
    ChartSerializatorCommon.SerializeValueTag(writer, "axPos", str);
  }

  private void SerializeScaling(XmlWriter writer, ChartAxisImpl axis)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (axis == null)
      throw new ArgumentNullException(nameof (axis));
    writer.WriteStartElement("scaling", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    if (axis is ChartValueAxisImpl chartValueAxisImpl && chartValueAxisImpl.IsLogScale)
      ChartSerializatorCommon.SerializeValueTag(writer, "logBase", XmlConvert.ToString(10));
    string str = axis.ReversePlotOrder ? "maxMin" : "minMax";
    ChartSerializatorCommon.SerializeValueTag(writer, "orientation", str);
    if (chartValueAxisImpl != null)
    {
      if (!chartValueAxisImpl.IsAutoMax)
        ChartSerializatorCommon.SerializeValueTag(writer, "max", XmlConvert.ToString(chartValueAxisImpl.MaximumValue));
      if (!chartValueAxisImpl.IsAutoMin)
        ChartSerializatorCommon.SerializeValueTag(writer, "min", XmlConvert.ToString(chartValueAxisImpl.MinimumValue));
    }
    writer.WriteEndElement();
  }

  [SecurityCritical]
  private void SerializeDisplayUnit(
    XmlWriter writer,
    ChartValueAxisImpl axis,
    RelationCollection relations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    OfficeChartDisplayUnit displayUnit = axis.DisplayUnit;
    if (displayUnit == OfficeChartDisplayUnit.None)
      return;
    bool displayUnitLabel1 = axis.HasDisplayUnitLabel;
    writer.WriteStartElement("dispUnits", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    if (displayUnit != OfficeChartDisplayUnit.Custom)
    {
      string str = ((Excel2007ChartDisplayUnit) displayUnit).ToString();
      ChartSerializatorCommon.SerializeValueTag(writer, "builtInUnit", str);
    }
    else
    {
      string str = XmlConvert.ToString(axis.DisplayUnitCustom);
      ChartSerializatorCommon.SerializeValueTag(writer, "custUnit", str);
    }
    if (displayUnitLabel1)
    {
      ChartImpl parentChart = axis.ParentChart;
      WorkbookImpl parentWorkbook = parentChart.ParentWorkbook;
      IOfficeChartTextArea displayUnitLabel2 = axis.DisplayUnitLabel;
      ChartTextAreaImpl chartTextAreaImpl = displayUnitLabel2 as ChartTextAreaImpl;
      writer.WriteStartElement("dispUnitsLbl", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      ChartSerializatorCommon.SerializeFrameFormat(writer, (IOfficeChartFillBorder) displayUnitLabel2.FrameFormat, parentChart, false);
      if (chartTextAreaImpl.ParagraphType == ChartParagraphType.CustomDefault)
        this.SerializeTextSettings(writer, (IWorkbook) parentWorkbook, (IOfficeFont) displayUnitLabel2, !chartTextAreaImpl.HasTextRotation, chartTextAreaImpl.TextRotationAngle, axis.LabelTextAlign);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private void SerializeTextSettings(XmlWriter writer, ChartAxisImpl axis)
  {
    if (axis == null)
      throw new ArgumentNullException(nameof (axis));
    if (axis.IsDefaultTextSettings)
    {
      if (axis.TextStream == null)
        return;
      axis.TextStream.Position = 0L;
      ShapeParser.WriteNodeFromStream(writer, axis.TextStream);
    }
    else
      this.SerializeTextSettings(writer, (IWorkbook) axis.ParentChart.ParentWorkbook, axis.Font, axis.IsAutoTextRotation, axis.TextRotationAngle, axis.LabelTextAlign);
  }

  private void SerializeTextSettings(
    XmlWriter writer,
    IWorkbook book,
    IOfficeFont font,
    bool isAutoTextRotation,
    int rotationAngle,
    AxisLabelAlignment labelTextAlign)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartElement("txPr", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteStartElement("bodyPr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (!isAutoTextRotation)
    {
      int num = rotationAngle * 60000;
      writer.WriteAttributeString("rot", num.ToString());
    }
    writer.WriteEndElement();
    writer.WriteStartElement("p", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteStartElement("pPr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    string str1;
    switch (labelTextAlign)
    {
      case AxisLabelAlignment.Left:
        str1 = "l";
        break;
      case AxisLabelAlignment.Right:
        str1 = "r";
        break;
      default:
        str1 = "ctr";
        break;
    }
    string str2 = str1;
    writer.WriteAttributeString("algn", str2);
    ChartSerializatorCommon.SerializeParagraphRunProperites(writer, font, "defRPr", book, 10.0);
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
  }
}
