// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.XmlSerialization.Charts.ChartExSerializator
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Charts;
using Syncfusion.OfficeChart.Implementation.XmlReaders.Shapes;
using Syncfusion.OfficeChart.Implementation.XmlSerialization.Constants;
using Syncfusion.OfficeChart.Interfaces.Charts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security;
using System.Xml;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.XmlSerialization.Charts;

internal class ChartExSerializator
{
  protected Dictionary<OfficeTickMark, string> s_dictTickMarkToAttributeValue = new Dictionary<OfficeTickMark, string>(4);

  internal ChartExSerializator()
  {
    this.s_dictTickMarkToAttributeValue.Add(OfficeTickMark.TickMark_None, "none");
    this.s_dictTickMarkToAttributeValue.Add(OfficeTickMark.TickMark_Inside, "in");
    this.s_dictTickMarkToAttributeValue.Add(OfficeTickMark.TickMark_Outside, "out");
    this.s_dictTickMarkToAttributeValue.Add(OfficeTickMark.TickMark_Cross, "cross");
  }

  [SecurityCritical]
  internal void SerializeChartEx(XmlWriter writer, ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    FileDataHolder parentHolder = chart.DataHolder.ParentHolder;
    RelationCollection relations = chart.Relations;
    writer.WriteStartElement("cx", "chartSpace", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    writer.WriteAttributeString("xmlns", "a", (string) null, "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("xmlns", "r", (string) null, "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
    this.SerializeChartExData(writer, chart);
    this.SerializeChartElement(writer, chart, relations);
    if (chart.HasChartArea && chart.ChartArea is ChartFrameFormatImpl chartArea && this.IsFrameFormatChanged(chartArea))
      ChartSerializatorCommon.SerializeFrameFormat(writer, (IOfficeChartFillBorder) chartArea, chart, chartArea.IsBorderCornersRound);
    if (chart.DefaultTextProperty != null)
    {
      chart.DefaultTextProperty.Position = 0L;
      ShapeParser.WriteNodeFromStream(writer, chart.DefaultTextProperty);
    }
    if (chart.m_colorMapOverrideStream != null)
    {
      chart.m_colorMapOverrideStream.Position = 0L;
      ShapeParser.WriteNodeFromStream(writer, (Stream) chart.m_colorMapOverrideStream);
    }
    writer.WriteEndElement();
  }

  [SecurityCritical]
  private void SerializeChartElement(
    XmlWriter writer,
    ChartImpl chart,
    RelationCollection relations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement(nameof (chart), "http://schemas.microsoft.com/office/drawing/2014/chartex");
    if (chart.HasTitle && chart.HasTitleInternal)
      this.SeriliazeChartTextArea(writer, chart.ChartTitleArea as ChartTextAreaImpl, chart, relations, 18.0, "title", chart.HasTitleInternal, true);
    this.SerializePlotArea(writer, chart, relations);
    if (chart.HasLegend && chart.Series.Count > 0)
      this.SerializeLegend(writer, chart.Legend as ChartLegendImpl, chart);
    writer.WriteEndElement();
  }

  [SecurityCritical]
  private void SerializePlotArea(XmlWriter writer, ChartImpl chart, RelationCollection relations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    OfficeChartType chartType = chart != null ? chart.ChartType : throw new ArgumentNullException(nameof (chart));
    bool treeMapOrSunBurst = chart.IsTreeMapOrSunBurst;
    writer.WriteStartElement("plotArea", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    writer.WriteStartElement("plotAreaRegion", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    if (chart.HasPlotArea)
    {
      writer.WriteStartElement("plotSurface", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      IOfficeChartFrameFormat plotArea = chart.PlotArea;
      ChartSerializatorCommon.SerializeFrameFormat(writer, (IOfficeChartFillBorder) plotArea, chart, chart.ChartArea.IsBorderCornersRound);
      writer.WriteEndElement();
    }
    if (chart.IsHistogramOrPareto && chart.Series.Count > 0)
    {
      HistogramAxisFormat axisFormatProperty1 = (chart.Series[0].SerieFormat as ChartSerieDataFormatImpl).HistogramAxisFormatProperty;
      HistogramAxisFormat axisFormatProperty2 = (chart.PrimaryCategoryAxis as ChartCategoryAxisImpl).HistogramAxisFormatProperty;
      if (axisFormatProperty1 != null && !axisFormatProperty1.Equals((object) axisFormatProperty2))
        axisFormatProperty1.Clone(axisFormatProperty2);
    }
    for (int index = 0; index < chart.Series.Count; ++index)
      this.SerializeChartExSeries(writer, chart.Series[index] as ChartSerieImpl, index, chartType, chart, relations);
    if (chartType == OfficeChartType.Pareto)
      this.SerializeParetoSeries(writer, chart, chartType);
    writer.WriteEndElement();
    if (!treeMapOrSunBurst)
      this.SerializeAxes(writer, chart, chartType, relations);
    writer.WriteEndElement();
  }

  [SecurityCritical]
  private void SerializeAxes(
    XmlWriter writer,
    ChartImpl chart,
    OfficeChartType chartType,
    RelationCollection relations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    this.SerializeAxis(writer, chart.PrimaryCategoryAxis as ChartValueAxisImpl, chart, relations, 0);
    if (chartType == OfficeChartType.Funnel)
      return;
    this.SerializeAxis(writer, chart.PrimaryValueAxis as ChartValueAxisImpl, chart, relations, 1);
    if (chartType != OfficeChartType.Pareto)
      return;
    this.SerializeAxis(writer, chart.PrimaryValueAxis as ChartValueAxisImpl, chart, relations, 2);
  }

  [SecurityCritical]
  private void SerializeAxis(
    XmlWriter writer,
    ChartValueAxisImpl axis,
    ChartImpl chart,
    RelationCollection relations,
    int axisId)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (axis == null)
      throw new ArgumentNullException(nameof (axis));
    writer.WriteStartElement(nameof (axis), "http://schemas.microsoft.com/office/drawing/2014/chartex");
    writer.WriteAttributeString("id", axisId.ToString());
    if (axis.Deleted)
      writer.WriteAttributeString("hidden", "1");
    if (axisId == 0)
    {
      writer.WriteStartElement("catScaling", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      double num = (double) chart.PrimaryFormats[0].GapWidth / 100.0;
      writer.WriteAttributeString("gapWidth", XmlConvert.ToString(num));
      writer.WriteEndElement();
    }
    else
    {
      writer.WriteStartElement("valScaling", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      if (!axis.IsAutoMin)
        writer.WriteAttributeString("min", XmlConvert.ToString(axis.MinimumValue));
      if (!axis.IsAutoMax)
        writer.WriteAttributeString("max", XmlConvert.ToString(axis.MaximumValue));
      if (!axis.IsAutoMinor)
        writer.WriteAttributeString("minorUnit", XmlConvert.ToString(axis.MinorUnit));
      if (!axis.IsAutoMajor)
        writer.WriteAttributeString("majorUnit", XmlConvert.ToString(axis.MajorUnit));
      writer.WriteEndElement();
    }
    this.SerializeAxisCommon(writer, axis, chart, relations, axisId);
    writer.WriteEndElement();
  }

  [SecurityCritical]
  private void SerializeAxisCommon(
    XmlWriter writer,
    ChartValueAxisImpl axis,
    ChartImpl chart,
    RelationCollection relations,
    int axisId)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (axis == null)
      throw new ArgumentNullException(nameof (axis));
    if (axis.HasAxisTitle)
      this.SeriliazeChartTextArea(writer, axis.TitleArea as ChartTextAreaImpl, chart, relations, 10.0, "title", true, false);
    this.SerializeDisplayUnit(writer, axis, chart, relations, axisId);
    if (axis.HasMajorGridLines)
    {
      writer.WriteStartElement("majorGridlines", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      ChartSerializatorCommon.SerializeFrameFormat(writer, (IOfficeChartFillBorder) axis.MajorGridLines, chart, false);
      writer.WriteEndElement();
    }
    if (axis.HasMinorGridLines)
    {
      writer.WriteStartElement("minorGridlines", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      ChartSerializatorCommon.SerializeFrameFormat(writer, (IOfficeChartFillBorder) axis.MinorGridLines, chart, false);
      writer.WriteEndElement();
    }
    writer.WriteStartElement("majorTickMarks", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    writer.WriteAttributeString("type", this.s_dictTickMarkToAttributeValue[axis.MajorTickMark]);
    writer.WriteEndElement();
    writer.WriteStartElement("minorTickMarks", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    writer.WriteAttributeString("type", this.s_dictTickMarkToAttributeValue[axis.MinorTickMark]);
    writer.WriteEndElement();
    if (axis.TickLabelPosition != OfficeTickLabelPosition.TickLabelPosition_None)
      writer.WriteElementString("tickLabels", "http://schemas.microsoft.com/office/drawing/2014/chartex", "");
    if (!axis.isNumber)
      return;
    writer.WriteStartElement("numFmt", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    writer.WriteAttributeString("formatCode", axis.NumberFormat);
    writer.WriteAttributeString("sourceLinked", axis.IsSourceLinked ? "1" : "0");
    writer.WriteEndElement();
  }

  [SecurityCritical]
  private void SerializeAxisShapeAndTextProperties(
    XmlWriter writer,
    ChartValueAxisImpl axis,
    ChartImpl chart,
    RelationCollection relations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (axis == null)
      throw new ArgumentNullException(nameof (axis));
    FileDataHolder parentHolder = chart.DataHolder.ParentHolder;
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
    else if (axis.FrameFormat.HasLineProperties || (axis.Border as ChartBorderImpl).HasLineProperties && !(axis.Border as ChartBorderImpl).AutoFormat)
    {
      writer.WriteStartElement("spPr", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      ChartSerializatorCommon.SerializeLineProperties(writer, axis.Border, (IWorkbook) parentHolder.Workbook);
      if (axis.Shadow.ShadowInnerPresets != Office2007ChartPresetsInner.NoShadow || axis.Shadow.ShadowOuterPresets != Office2007ChartPresetsOuter.NoShadow || axis.Shadow.ShadowPerspectivePresets != Office2007ChartPresetsPerspective.NoShadow)
        ChartSerializatorCommon.SerializeShadow(writer, axis.Shadow, axis.Shadow.HasCustomShadowStyle);
      writer.WriteEndElement();
    }
    if ((axis.ParagraphType != ChartParagraphType.CustomDefault || axis.TextStream == null) && axis.IsAutoTextRotation && axis.IsDefaultTextSettings)
      return;
    if (axis.IsDefaultTextSettings)
    {
      if (axis.TextStream == null)
        return;
      axis.TextStream.Position = 0L;
      ShapeParser.WriteNodeFromStream(writer, axis.TextStream);
    }
    else
      ChartSerializatorCommon.SerializeDefaultTextFormatting(writer, axis.Font, (IWorkbook) chart.ParentWorkbook, 10.0, axis.IsAutoTextRotation, axis.TextRotationAngle, axis.m_textRotation, "http://schemas.microsoft.com/office/drawing/2014/chartex", false, false);
  }

  [SecurityCritical]
  private void SerializeDisplayUnit(
    XmlWriter writer,
    ChartValueAxisImpl axis,
    ChartImpl chart,
    RelationCollection relations,
    int axisId)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (axis == null)
      throw new ArgumentNullException(nameof (axis));
    if (axis.HasDisplayUnitLabel && axis.DisplayUnit != OfficeChartDisplayUnit.None)
    {
      writer.WriteStartElement("units", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      if (axis.DisplayUnit != OfficeChartDisplayUnit.Custom)
        writer.WriteAttributeString("unit", ((Excel2007ChartDisplayUnit) axis.DisplayUnit).ToString());
      else if (axisId == 2)
        writer.WriteAttributeString("unit", Excel2007ChartDisplayUnit.hundreds.ToString());
      if (axis.DisplayUnitLabel.Text != null)
        this.SeriliazeChartTextArea(writer, axis.DisplayUnitLabel as ChartTextAreaImpl, chart, relations, 10.0, "unitsLabel", true, false);
      writer.WriteEndElement();
    }
    else
    {
      if (axisId != 2)
        return;
      writer.WriteStartElement("units", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      writer.WriteAttributeString("unit", Excel2007ChartDisplayUnit.hundreds.ToString());
      writer.WriteEndElement();
    }
  }

  [SecurityCritical]
  private void SerializeParetoSeries(XmlWriter writer, ChartImpl chart, OfficeChartType chartType)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    for (int index = 0; index < chart.Series.Count; ++index)
    {
      ChartSerieImpl serie = chart.Series[index] as ChartSerieImpl;
      writer.WriteStartElement("series", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      this.SerializeSerieAttributes(writer, serie, chartType, index, true);
      writer.WriteStartElement("axisId", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      writer.WriteAttributeString("val", "2");
      writer.WriteEndElement();
      ChartSerializatorCommon.SerializeFrameFormat(writer, (IOfficeChartFillBorder) serie.ParetoLineFormat, chart, false);
      writer.WriteEndElement();
    }
  }

  [SecurityCritical]
  private void SerializeChartExSeries(
    XmlWriter writer,
    ChartSerieImpl serie,
    int serieDataIndex,
    OfficeChartType chartType,
    ChartImpl chart,
    RelationCollection relations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (serie == null)
      throw new ArgumentNullException(nameof (serie));
    ChartDataPointImpl defaultDataPoint = (ChartDataPointImpl) serie.DataPoints.DefaultDataPoint;
    ChartSerieDataFormatImpl dataFormatOrNull = defaultDataPoint.DataFormatOrNull;
    writer.WriteStartElement("series", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    this.SerializeSerieAttributes(writer, serie, chartType, serieDataIndex, false);
    this.SerializeSeriesName(writer, serie);
    if (dataFormatOrNull != null)
      ChartSerializatorCommon.SerializeFrameFormat(writer, (IOfficeChartFillBorder) dataFormatOrNull, serie.ParentChart, false, true);
    this.SerializeDataPointsSettings(writer, serie);
    if (defaultDataPoint.HasDataLabels || (serie.DataPoints as ChartDataPointsCollection).CheckDPDataLabels())
      this.SerializeDataLabels(writer, serie, chart, relations);
    writer.WriteStartElement("dataId", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    writer.WriteAttributeString("val", serieDataIndex.ToString());
    writer.WriteEndElement();
    this.SerializeLayoutProperties(writer, serie, chart, relations);
    if (chartType == OfficeChartType.Pareto)
    {
      writer.WriteStartElement("axisId", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      writer.WriteAttributeString("val", "1");
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private void SerializeLayoutProperties(
    XmlWriter writer,
    ChartSerieImpl serie,
    ChartImpl chart,
    RelationCollection relations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (serie == null)
      throw new ArgumentNullException(nameof (serie));
    OfficeChartType chartType = chart.ChartType;
    ChartSerieDataFormatImpl serieFormat = serie.SerieFormat as ChartSerieDataFormatImpl;
    bool histogramOrPareto = chart.IsHistogramOrPareto;
    writer.WriteStartElement("layoutPr", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    if (chartType == OfficeChartType.TreeMap)
    {
      writer.WriteStartElement("parentLabelLayout", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      writer.WriteAttributeString("val", serieFormat.TreeMapLabelOption.ToString().ToLower());
      writer.WriteEndElement();
    }
    if (chartType == OfficeChartType.BoxAndWhisker || chartType == OfficeChartType.WaterFall)
    {
      writer.WriteStartElement("visibility", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      if (chartType == OfficeChartType.WaterFall)
      {
        writer.WriteAttributeString("connectorLines", serieFormat.ShowConnectorLines ? "1" : "0");
      }
      else
      {
        writer.WriteAttributeString("meanLine", serieFormat.ShowMeanLine ? "1" : "0");
        writer.WriteAttributeString("meanMarker", serieFormat.ShowMeanMarkers ? "1" : "0");
        writer.WriteAttributeString("outliers", serieFormat.ShowOutlierPoints ? "1" : "0");
        writer.WriteAttributeString("nonoutliers", serieFormat.ShowInnerPoints ? "1" : "0");
      }
      writer.WriteEndElement();
    }
    if (histogramOrPareto)
      this.SerializeBinningProperties(writer, serieFormat, chart);
    if (chartType == OfficeChartType.BoxAndWhisker)
    {
      writer.WriteStartElement("statistics", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      writer.WriteAttributeString("quartileMethod", serieFormat.QuartileCalculationType == QuartileCalculation.InclusiveMedian ? "inclusive" : "exclusive");
      writer.WriteEndElement();
    }
    if (chartType == OfficeChartType.WaterFall)
      this.SerializeSubTotalIndexes(writer, serie);
    writer.WriteEndElement();
  }

  private void SerializeSubTotalIndexes(XmlWriter writer, ChartSerieImpl serie)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    ChartDataPointsCollection pointsCollection = serie != null ? (ChartDataPointsCollection) serie.DataPoints : throw new ArgumentNullException(nameof (serie));
    if (pointsCollection.DeninedDPCount <= 0)
      return;
    List<int> intList = new List<int>(pointsCollection.DeninedDPCount);
    foreach (ChartDataPointImpl chartDataPointImpl in pointsCollection)
    {
      if ((!chartDataPointImpl.IsDefault || chartDataPointImpl.HasDataPoint) && chartDataPointImpl.SetAsTotal)
        intList.Add(chartDataPointImpl.Index);
    }
    if (intList.Count <= 0)
      return;
    writer.WriteStartElement("subtotals", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    for (int index = 0; index < intList.Count; ++index)
    {
      writer.WriteStartElement("idx", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      writer.WriteAttributeString("val", intList[index].ToString());
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private void SerializeBinningProperties(
    XmlWriter writer,
    ChartSerieDataFormatImpl dataFormat,
    ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (dataFormat == null)
      throw new ArgumentNullException(nameof (dataFormat));
    HistogramAxisFormat histogramAxisFormat = dataFormat.HistogramAxisFormatProperty != null ? dataFormat.HistogramAxisFormatProperty : (chart.PrimaryCategoryAxis as ChartCategoryAxisImpl).HistogramAxisFormatProperty;
    if (histogramAxisFormat.IsBinningByCategory)
    {
      writer.WriteElementString("aggregation", "http://schemas.microsoft.com/office/drawing/2014/chartex", string.Empty);
    }
    else
    {
      writer.WriteStartElement("binning", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      writer.WriteAttributeString("intervalClosed", histogramAxisFormat.IsIntervalClosedinLeft ? "l" : "r");
      if (((int) histogramAxisFormat.FlagOptions & 32 /*0x20*/) == 32 /*0x20*/)
      {
        if (!histogramAxisFormat.IsNotAutomaticUnderFlowValue)
          writer.WriteAttributeString("underflow", "auto");
        else
          writer.WriteAttributeString("underflow", XmlConvert.ToString(histogramAxisFormat.UnderflowBinValue));
      }
      if (((int) histogramAxisFormat.FlagOptions & 16 /*0x10*/) == 16 /*0x10*/)
      {
        if (!histogramAxisFormat.IsNotAutomaticOverFlowValue)
          writer.WriteAttributeString("overflow", "auto");
        else
          writer.WriteAttributeString("overflow", XmlConvert.ToString(histogramAxisFormat.OverflowBinValue));
      }
      if (!histogramAxisFormat.HasAutomaticBins)
      {
        if (((int) histogramAxisFormat.FlagOptions & 8) == 8)
        {
          writer.WriteStartElement("binCount", "http://schemas.microsoft.com/office/drawing/2014/chartex");
          writer.WriteAttributeString("val", histogramAxisFormat.NumberOfBins.ToString());
          writer.WriteEndElement();
        }
        else if (((int) histogramAxisFormat.FlagOptions & 4) == 4)
        {
          writer.WriteStartElement("binSize", "http://schemas.microsoft.com/office/drawing/2014/chartex");
          writer.WriteAttributeString("val", XmlConvert.ToString(histogramAxisFormat.BinWidth));
          writer.WriteEndElement();
        }
      }
      writer.WriteEndElement();
    }
  }

  [SecurityCritical]
  private void SerializeDataLabels(
    XmlWriter writer,
    ChartSerieImpl serie,
    ChartImpl chart,
    RelationCollection relations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    ChartImpl parentChart = serie != null ? serie.ParentChart : throw new ArgumentNullException(nameof (serie));
    ChartDataPointsCollection dataPoints = (ChartDataPointsCollection) serie.DataPoints;
    ChartDataLabelsImpl dataLabels1 = serie.DataPoints.DefaultDataPoint.DataLabels as ChartDataLabelsImpl;
    writer.WriteStartElement("dataLabels", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    if (dataLabels1.Position != OfficeDataLabelPosition.Automatic && dataLabels1.Position != OfficeDataLabelPosition.Moved)
    {
      Excel2007DataLabelPos position = (Excel2007DataLabelPos) dataLabels1.Position;
      writer.WriteAttributeString("pos", position.ToString());
    }
    this.SerializeDataLabelSettings(writer, dataLabels1, parentChart, relations);
    List<int> intList = (List<int>) null;
    if (dataPoints.DeninedDPCount > 0)
    {
      intList = new List<int>();
      foreach (ChartDataPointImpl chartDataPointImpl in dataPoints)
      {
        if (!chartDataPointImpl.IsDefault && chartDataPointImpl.HasDataLabels)
        {
          ChartDataLabelsImpl dataLabels2 = chartDataPointImpl.DataLabels as ChartDataLabelsImpl;
          if (dataLabels2.IsDelete)
          {
            intList.Add(chartDataPointImpl.Index);
          }
          else
          {
            writer.WriteStartElement("dataLabel", "http://schemas.microsoft.com/office/drawing/2014/chartex");
            writer.WriteAttributeString("idx", chartDataPointImpl.Index.ToString());
            if (dataLabels2.Position != OfficeDataLabelPosition.Automatic && dataLabels2.Position != OfficeDataLabelPosition.Moved)
            {
              Excel2007DataLabelPos position = (Excel2007DataLabelPos) dataLabels2.Position;
              writer.WriteAttributeString("pos", position.ToString());
            }
            this.SerializeDataLabelSettings(writer, dataLabels2, parentChart, relations);
            writer.WriteEndElement();
          }
        }
      }
    }
    if (intList != null)
    {
      for (int index = 0; index < intList.Count; ++index)
      {
        writer.WriteStartElement("dataLabelHidden", "http://schemas.microsoft.com/office/drawing/2014/chartex");
        writer.WriteAttributeString("idx", intList[index].ToString());
        writer.WriteEndElement();
      }
    }
    writer.WriteEndElement();
  }

  [SecurityCritical]
  private void SerializeDataLabelSettings(
    XmlWriter writer,
    ChartDataLabelsImpl dataLabels,
    ChartImpl parentChart,
    RelationCollection relations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (dataLabels == null)
      throw new ArgumentNullException(nameof (dataLabels));
    writer.WriteStartElement("visibility", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    if (dataLabels.IsValue)
      writer.WriteAttributeString("value", "1");
    if (dataLabels.IsSeriesName)
      writer.WriteAttributeString("seriesName", "1");
    if (dataLabels.IsCategoryName || parentChart.ChartType == OfficeChartType.TreeMap || parentChart.ChartType == OfficeChartType.SunBurst)
      writer.WriteAttributeString("categoryName", "1");
    writer.WriteEndElement();
    if (dataLabels.Delimiter != null)
      writer.WriteElementString("separator", "http://schemas.microsoft.com/office/drawing/2014/chartex", dataLabels.Delimiter);
    if (!this.IsFrameFormatChanged(dataLabels.FrameFormat as ChartFrameFormatImpl))
      return;
    ChartSerializatorCommon.SerializeFrameFormat(writer, (IOfficeChartFillBorder) dataLabels.FrameFormat, parentChart, false);
  }

  private bool IsFrameFormatChanged(ChartFrameFormatImpl format)
  {
    return (format.HasInterior || format.HasLineProperties) && (!format.Interior.UseAutomaticFormat || !format.LineProperties.AutoFormat) || format.HasShadowProperties || format.Has3dProperties;
  }

  private void SerializeNumFormat(XmlWriter writer, ChartDataLabelsImpl dataLabels)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (dataLabels == null)
      throw new ArgumentNullException(nameof (dataLabels));
    writer.WriteStartElement("numFmt", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    writer.WriteAttributeString("formatCode", dataLabels.NumberFormat);
    bool isSourceLinked = dataLabels.IsSourceLinked;
    if (!isSourceLinked)
      writer.WriteAttributeString("sourceLinked", Convert.ToInt16(isSourceLinked).ToString());
    writer.WriteEndElement();
  }

  [SecurityCritical]
  private void SerializeDataPointsSettings(XmlWriter writer, ChartSerieImpl serie)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    ChartDataPointsCollection pointsCollection = serie != null ? (ChartDataPointsCollection) serie.DataPoints : throw new ArgumentNullException(nameof (serie));
    if (pointsCollection.DeninedDPCount <= 0)
      return;
    foreach (ChartDataPointImpl chartDataPointImpl in pointsCollection)
    {
      if (!chartDataPointImpl.IsDefault || chartDataPointImpl.HasDataPoint)
      {
        ChartSerieDataFormatImpl dataFormatOrNull = chartDataPointImpl.DataFormatOrNull;
        if (dataFormatOrNull != null && (dataFormatOrNull.IsFormatted || dataFormatOrNull.IsParsed))
        {
          writer.WriteStartElement("dataPt", "http://schemas.microsoft.com/office/drawing/2014/chartex");
          writer.WriteAttributeString("idx", chartDataPointImpl.Index.ToString());
          if (dataFormatOrNull.HasInterior || dataFormatOrNull.HasShadowProperties || dataFormatOrNull.HasLineProperties)
            ChartSerializatorCommon.SerializeFrameFormat(writer, (IOfficeChartFillBorder) dataFormatOrNull, dataFormatOrNull.ParentChart, false);
          writer.WriteEndElement();
        }
      }
    }
  }

  private void SerializeSeriesName(XmlWriter writer, ChartSerieImpl serie)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (serie == null)
      throw new ArgumentNullException(nameof (serie));
    if (serie.IsDefaultName || (serie.NameRange == null || serie.NameRange is ExternalRange) && (serie.Name == null || !(serie.Name != "")))
      return;
    writer.WriteStartElement("tx", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    writer.WriteStartElement("txData", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    if (serie.NameRange != null && !(serie.NameRange is ExternalRange))
    {
      writer.WriteStartElement("f", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      if ((serie.NameRange as ChartDataRange).Range != null)
        writer.WriteString((serie.NameRange as ChartDataRange).Range.AddressGlobal);
      writer.WriteEndElement();
    }
    if (!string.IsNullOrEmpty(serie.Name))
    {
      writer.WriteStartElement("v", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      writer.WriteString(serie.Name);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private void SerializeSerieAttributes(
    XmlWriter writer,
    ChartSerieImpl serie,
    OfficeChartType chartType,
    int serieIndex,
    bool isPareto)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (serie == null)
      throw new ArgumentNullException(nameof (serie));
    string str = chartType.ToString().ToLowerInvariant();
    switch (chartType)
    {
      case OfficeChartType.Pareto:
        str = !isPareto ? "clusteredColumn" : "paretoLine";
        break;
      case OfficeChartType.Histogram:
        str = "clusteredColumn";
        break;
      case OfficeChartType.BoxAndWhisker:
        str = "boxWhisker";
        break;
    }
    writer.WriteAttributeString("layoutId", str);
    if (isPareto && serie.IsParetoLineHidden)
      writer.WriteAttributeString("hidden", "1");
    if (!isPareto && serie.IsSeriesHidden)
      writer.WriteAttributeString("hidden", "1");
    if (isPareto)
      writer.WriteAttributeString("ownerIdx", serieIndex.ToString());
    if (isPareto)
    {
      if (serie.ParetoLineFormatIndex == -1)
        return;
      writer.WriteAttributeString("formatIdx", serie.ParetoLineFormatIndex.ToString());
    }
    else
    {
      if (serie.Number == -1)
        return;
      writer.WriteAttributeString("formatIdx", serie.Number.ToString());
    }
  }

  [SecurityCritical]
  private void SerializeLegend(XmlWriter writer, ChartLegendImpl chartLegend, ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chartLegend == null)
      throw new ArgumentNullException(nameof (chartLegend));
    writer.WriteStartElement("legend", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    OfficeLegendPosition position1 = chartLegend.Position;
    ushort position2 = chartLegend.ChartExPosition;
    if (position1 != OfficeLegendPosition.NotDocked)
    {
      position2 = (ushort) 0;
      switch (position1)
      {
        case OfficeLegendPosition.Bottom:
          position2 |= (ushort) 72;
          break;
        case OfficeLegendPosition.Corner:
          position2 |= (ushort) 20;
          break;
        case OfficeLegendPosition.Top:
          position2 |= (ushort) 66;
          break;
        case OfficeLegendPosition.Right:
          position2 |= (ushort) 68;
          break;
        case OfficeLegendPosition.Left:
          position2 |= (ushort) 65;
          break;
      }
    }
    this.SerializeTextElementAttributes(writer, position2, chartLegend.IncludeInLayout);
    ChartSerializatorCommon.SerializeFrameFormat(writer, (IOfficeChartFillBorder) chartLegend.FrameFormat, chart, false);
    chartLegend.IsChartTextArea = true;
    if (((IInternalOfficeChartTextArea) chartLegend.TextArea).ParagraphType == ChartParagraphType.CustomDefault)
      ChartSerializatorCommon.SerializeDefaultTextFormatting(writer, (IOfficeFont) chartLegend.TextArea, (IWorkbook) chart.ParentWorkbook, 10.0, true, 0, Excel2007TextRotation.horz, "http://schemas.microsoft.com/office/drawing/2014/chartex", false, true);
    writer.WriteEndElement();
  }

  [SecurityCritical]
  private void SeriliazeChartTextArea(
    XmlWriter writer,
    ChartTextAreaImpl chartTextArea,
    ChartImpl chart,
    RelationCollection relations,
    double defaultFontSize,
    string parentElement,
    bool isNotAuto,
    bool isChartTitle)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chartTextArea == null)
      throw new ArgumentNullException(nameof (chartTextArea));
    writer.WriteStartElement(parentElement, "http://schemas.microsoft.com/office/drawing/2014/chartex");
    bool flag = false;
    if (isChartTitle)
      this.SerializeTextElementAttributes(writer, chart.ChartExTitlePosition, chart.ChartTitleIncludeInLayout);
    if (isNotAuto)
    {
      chartTextArea.ShowBoldProperties = true;
      FileDataHolder dataHolder = chart.ParentWorkbook.DataHolder;
      if (chartTextArea.HasText)
        flag = this.SerializeTextAreaText(writer, chartTextArea, chart, defaultFontSize);
      ChartSerializatorCommon.SerializeFrameFormat(writer, (IOfficeChartFillBorder) chartTextArea.FrameFormat, dataHolder, relations, false, false);
      if (!flag && chartTextArea.ParagraphType == ChartParagraphType.CustomDefault)
        ChartSerializatorCommon.SerializeDefaultTextFormatting(writer, (IOfficeFont) chartTextArea, (IWorkbook) chart.ParentWorkbook, defaultFontSize, true, 0, Excel2007TextRotation.horz, "http://schemas.microsoft.com/office/drawing/2014/chartex", true, false);
    }
    writer.WriteEndElement();
  }

  private void SerializeTextElementAttributes(XmlWriter writer, ushort position, bool isLayout)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteAttributeString("overlay", isLayout ? "1" : "0");
    if (position == (ushort) 0)
    {
      writer.WriteAttributeString("align", ChartExPositionAlignment.ctr.ToString());
      writer.WriteAttributeString("pos", ChartExSidePosition.t.ToString());
    }
    else
    {
      ushort num1 = 240 /*0xF0*/;
      ChartExPositionAlignment positionAlignment = (ChartExPositionAlignment) ((int) position & (int) num1);
      ushort num2 = 15;
      ChartExSidePosition chartExSidePosition = (ChartExSidePosition) ((int) position & (int) num2);
      writer.WriteAttributeString("align", positionAlignment.ToString());
      writer.WriteAttributeString("pos", chartExSidePosition.ToString());
    }
  }

  private bool SerializeTextAreaText(
    XmlWriter writer,
    ChartTextAreaImpl chartTextArea,
    ChartImpl chart,
    double defaultFontSize)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chartTextArea == null)
      throw new ArgumentNullException(nameof (chartTextArea));
    bool flag = false;
    writer.WriteStartElement("tx", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    if (chartTextArea.ChartAlRuns != null && chartTextArea.ChartAlRuns.Runs != null && chartTextArea.ChartAlRuns.Runs.Length > 0)
    {
      ChartSerializatorCommon.SerializeRichText(writer, (IOfficeChartTextArea) chartTextArea, (IWorkbook) chart.ParentWorkbook, "rich", defaultFontSize);
      flag = true;
    }
    else
    {
      writer.WriteStartElement("txData", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      if (chartTextArea.IsFormula)
      {
        writer.WriteStartElement("f", "http://schemas.microsoft.com/office/drawing/2014/chartex");
        writer.WriteString(chartTextArea.Text);
        writer.WriteEndElement();
      }
      else
      {
        writer.WriteStartElement("v", "http://schemas.microsoft.com/office/drawing/2014/chartex");
        writer.WriteString(chartTextArea.Text);
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
    return flag;
  }

  private void SerializeChartExData(XmlWriter writer, ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    OfficeChartType chartType = chart != null ? chart.ChartType : throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("chartData", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    string relationId = chart.Relations.GenerateRelationId();
    chart.Relations[relationId] = new Relation("", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/package");
    if (chart.ParentWorkbook.IsLoaded && chart.ChartExRelationId != null)
    {
      writer.WriteStartElement("externalData", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", relationId);
      if (chart.AutoUpdate.HasValue)
      {
        string str = chart.AutoUpdate.Value ? "1" : "0";
        writer.WriteAttributeString("autoUpdate", "http://schemas.microsoft.com/office/drawing/2014/chartex", str);
      }
      writer.WriteEndElement();
    }
    for (int index = 0; index < chart.Series.Count; ++index)
      this.SerializeIndividualChartSerieData(writer, chart.Series[index] as ChartSerieImpl, index, chartType);
    writer.WriteEndElement();
  }

  private void SerializeIndividualChartSerieData(
    XmlWriter writer,
    ChartSerieImpl serie,
    int index,
    OfficeChartType chartType)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (serie == null)
      throw new ArgumentNullException(nameof (serie));
    bool treeMapOrSunBurst = serie.InnerChart.IsTreeMapOrSunBurst;
    writer.WriteStartElement("data", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    writer.WriteAttributeString("id", index.ToString());
    writer.WriteStartElement("numDim", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    string str = treeMapOrSunBurst ? "size" : "val";
    writer.WriteAttributeString("type", str);
    this.SerializeDimensionData(writer, serie.Values, serie.EnteredDirectlyValues, serie.IsRowWiseSeries, serie.FormatCode, false, treeMapOrSunBurst);
    writer.WriteEndElement();
    if (serie.CategoryLabels == null && serie.EnteredDirectlyCategoryLabels != null)
    {
      if (serie.EnteredDirectlyCategoryLabels.Length > 0 && serie.EnteredDirectlyCategoryLabels[0] is string)
        writer.WriteStartElement("strDim", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      else
        writer.WriteStartElement("numDim", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    }
    else
      writer.WriteStartElement("strDim", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    writer.WriteAttributeString("type", "cat");
    this.SerializeDimensionData(writer, serie.CategoryLabels, serie.EnteredDirectlyCategoryLabels, serie.IsRowWiseCategory, serie.CategoriesFormatCode, true, treeMapOrSunBurst);
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private void SerializeDimensionData(
    XmlWriter writer,
    IOfficeDataRange range,
    object[] directValues,
    bool isInRow,
    string formatCode,
    bool isCategoryRange,
    bool isTreeMaporSunburst)
  {
    if (range != null)
    {
      writer.WriteStartElement("f", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      if (!(range is IRanges))
        writer.WriteAttributeString("dir", isInRow ? "row" : "col");
      if (range is NameImpl)
        writer.WriteString((range as NameImpl).Name);
      else if ((range as ChartDataRange).Range != null)
        writer.WriteString((range as ChartDataRange).Range.AddressGlobal);
      writer.WriteEndElement();
    }
    if (directValues == null && (range as ChartDataRange).Range == null)
      return;
    int index1 = 0;
    IRange range1 = (range as ChartDataRange).Range;
    IWorksheet worksheet = range1.Worksheet;
    if (isTreeMaporSunburst && isCategoryRange && !isInRow)
    {
      for (int column = range1.Column; column <= range1.LastColumn; ++column)
      {
        int num = 0;
        int count = (range as ChartDataRange).Range.Columns[column - 1].Count;
        writer.WriteStartElement("lvl", "http://schemas.microsoft.com/office/drawing/2014/chartex");
        if (directValues != null)
          writer.WriteAttributeString("ptCount", count.ToString());
        else if (range != null)
          writer.WriteAttributeString("ptCount", count.ToString());
        formatCode = formatCode == null || !(formatCode != string.Empty) ? "General" : formatCode;
        if (!isCategoryRange)
          writer.WriteAttributeString(nameof (formatCode), formatCode);
        if (directValues != null)
        {
          int length = directValues.Length;
          for (int index2 = 0; index2 < count; ++index2)
          {
            if (index1 < length && directValues[index1] != null)
            {
              writer.WriteStartElement("cx", "pt", (string) null);
              writer.WriteAttributeString("idx", num.ToString());
              writer.WriteString(ChartSerializator.ToXmlString(directValues[index1]));
              writer.WriteEndElement();
            }
            ++num;
            ++index1;
          }
          writer.WriteEndElement();
        }
        else if ((range as ChartDataRange).Range != null)
        {
          for (int row = 2; row <= count; ++row)
          {
            string text = this.ObjectValueToString((object) worksheet.Range[row, column].DisplayText);
            if (text != null)
            {
              writer.WriteStartElement("cx", "pt", (string) null);
              writer.WriteAttributeString("idx", num.ToString());
              writer.WriteString(text);
              writer.WriteEndElement();
            }
            ++index1;
            ++num;
          }
          writer.WriteEndElement();
        }
      }
    }
    else
    {
      int count = (range as ChartDataRange).Range.Count;
      writer.WriteStartElement("lvl", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      if (directValues != null)
        writer.WriteAttributeString("ptCount", directValues.Length.ToString());
      else if (range != null)
        writer.WriteAttributeString("ptCount", count.ToString());
      formatCode = formatCode == null || !(formatCode != string.Empty) ? "General" : formatCode;
      if (!isCategoryRange)
        writer.WriteAttributeString(nameof (formatCode), formatCode);
      if (directValues != null)
      {
        for (int index3 = 0; index3 < directValues.Length; ++index3)
        {
          if (directValues[index3] != null)
          {
            writer.WriteStartElement("cx", "pt", (string) null);
            writer.WriteAttributeString("idx", index3.ToString());
            writer.WriteString(ChartSerializator.ToXmlString(directValues[index3]));
            writer.WriteEndElement();
          }
        }
        writer.WriteEndElement();
      }
      else
      {
        if ((range as ChartDataRange).Range == null)
          return;
        for (; index1 < count; ++index1)
        {
          string text = this.ObjectValueToString(worksheet.Range[range1.Cells[index1].AddressLocal].Value2);
          if (text != null)
          {
            writer.WriteStartElement("cx", "pt", (string) null);
            writer.WriteAttributeString("idx", index1.ToString());
            writer.WriteString(text);
            writer.WriteEndElement();
          }
        }
        writer.WriteEndElement();
      }
    }
  }

  private string ObjectValueToString(object value)
  {
    switch (value)
    {
      case string _:
        return value.ToString();
      case int _:
        return value.ToString();
      case float num1:
        return num1.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      case double num2:
        return num2.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      case int _:
        return value.ToString();
      case Decimal num3:
        return num3.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      default:
        return value.ToString();
    }
  }

  private void SerializePrinterSettings(
    XmlWriter writer,
    ChartImpl chart,
    RelationCollection relations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    IOfficeChartPageSetup pageSetup = chart != null ? chart.PageSetup : throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("printSettings", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    IPageSetupConstantsProvider constants = (IPageSetupConstantsProvider) new ChartPageSetupConstants();
    Excel2007Serializator.SerializePrintSettings(writer, (IPageSetupBase) pageSetup, constants, true);
    Excel2007Serializator.SerializeVmlHFShapesWorksheetPart(writer, (WorksheetBaseImpl) chart, constants, relations);
    chart.DataHolder.SerializeHeaderFooterImages((WorksheetBaseImpl) chart, relations);
    writer.WriteEndElement();
  }
}
