// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Charts.ChartExSerializator
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Charts;
using Syncfusion.XlsIO.Implementation.XmlReaders.Shapes;
using Syncfusion.XlsIO.Implementation.XmlSerialization.Constants;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Interfaces.Charts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization.Charts;

internal class ChartExSerializator
{
  protected Dictionary<ExcelTickMark, string> s_dictTickMarkToAttributeValue = new Dictionary<ExcelTickMark, string>(4);

  internal ChartExSerializator()
  {
    this.s_dictTickMarkToAttributeValue.Add(ExcelTickMark.TickMark_None, "none");
    this.s_dictTickMarkToAttributeValue.Add(ExcelTickMark.TickMark_Inside, "in");
    this.s_dictTickMarkToAttributeValue.Add(ExcelTickMark.TickMark_Outside, "out");
    this.s_dictTickMarkToAttributeValue.Add(ExcelTickMark.TickMark_Cross, "cross");
  }

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
      ChartSerializatorCommon.SerializeFrameFormat(writer, (IChartFillBorder) chartArea, chart, chartArea.IsBorderCornersRound);
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
    if (chart.IsEmbeded)
      this.SerializePrinterSettings(writer, chart, relations);
    writer.WriteEndElement();
  }

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
    if (chart.HasTitle || chart.HasAutoTitle.HasValue && chart.HasAutoTitle.Value)
      this.SeriliazeChartTextArea(writer, chart.ChartTitleArea as ChartTextAreaImpl, chart, relations, 18.0, "title", chart.HasTitle, true);
    this.SerializePlotArea(writer, chart, relations);
    if (chart.HasLegend && chart.Series.Count > 0)
      this.SerializeLegend(writer, chart.Legend as ChartLegendImpl, chart);
    writer.WriteEndElement();
  }

  private void SerializePlotArea(XmlWriter writer, ChartImpl chart, RelationCollection relations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    ExcelChartType chartType = chart != null ? chart.ChartType : throw new ArgumentNullException(nameof (chart));
    bool treeMapOrSunBurst = chart.IsTreeMapOrSunBurst;
    writer.WriteStartElement("plotArea", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    writer.WriteStartElement("plotAreaRegion", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    if (chart.HasPlotArea)
    {
      writer.WriteStartElement("plotSurface", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      IChartFrameFormat plotArea = chart.PlotArea;
      ChartSerializatorCommon.SerializeFrameFormat(writer, (IChartFillBorder) plotArea, chart, chart.ChartArea.IsBorderCornersRound);
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
    if (chartType == ExcelChartType.Pareto)
      this.SerializeParetoSeries(writer, chart, chartType);
    writer.WriteEndElement();
    if (!treeMapOrSunBurst)
      this.SerializeAxes(writer, chart, chartType, relations);
    writer.WriteEndElement();
  }

  private void SerializeAxes(
    XmlWriter writer,
    ChartImpl chart,
    ExcelChartType chartType,
    RelationCollection relations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    this.SerializeAxis(writer, chart.PrimaryCategoryAxis as ChartValueAxisImpl, chart, relations, 0);
    if (chartType == ExcelChartType.Funnel)
      return;
    this.SerializeAxis(writer, chart.PrimaryValueAxis as ChartValueAxisImpl, chart, relations, 1);
    if (chartType != ExcelChartType.Pareto)
      return;
    this.SerializeAxis(writer, chart.SecondaryValueAxis as ChartValueAxisImpl, chart, relations, 2);
  }

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
      writer.WriteAttributeString("gapWidth", ChartSerializator.ToXmlString((object) num));
      writer.WriteEndElement();
    }
    else
    {
      writer.WriteStartElement("valScaling", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      if (!axis.IsAutoMin)
        writer.WriteAttributeString("min", ChartSerializator.ToXmlString((object) axis.MinimumValue));
      if (!axis.IsAutoMax)
        writer.WriteAttributeString("max", ChartSerializator.ToXmlString((object) axis.MaximumValue));
      if (!axis.IsAutoMinor)
        writer.WriteAttributeString("minorUnit", ChartSerializator.ToXmlString((object) axis.MinorUnit));
      if (!axis.IsAutoMajor)
        writer.WriteAttributeString("majorUnit", ChartSerializator.ToXmlString((object) axis.MajorUnit));
      writer.WriteEndElement();
    }
    this.SerializeAxisCommon(writer, axis, chart, relations, axisId);
    writer.WriteEndElement();
  }

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
      ChartSerializatorCommon.SerializeFrameFormat(writer, (IChartFillBorder) axis.MajorGridLines, chart, false);
      writer.WriteEndElement();
    }
    if (axis.HasMinorGridLines)
    {
      writer.WriteStartElement("minorGridlines", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      ChartSerializatorCommon.SerializeFrameFormat(writer, (IChartFillBorder) axis.MinorGridLines, chart, false);
      writer.WriteEndElement();
    }
    writer.WriteStartElement("majorTickMarks", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    writer.WriteAttributeString("type", this.s_dictTickMarkToAttributeValue[axis.MajorTickMark]);
    writer.WriteEndElement();
    writer.WriteStartElement("minorTickMarks", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    writer.WriteAttributeString("type", this.s_dictTickMarkToAttributeValue[axis.MinorTickMark]);
    writer.WriteEndElement();
    if (axis.TickLabelPosition != ExcelTickLabelPosition.TickLabelPosition_None)
      writer.WriteElementString("tickLabels", "http://schemas.microsoft.com/office/drawing/2014/chartex", "");
    if (axis.isNumber)
    {
      writer.WriteStartElement("numFmt", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      writer.WriteAttributeString("formatCode", axis.NumberFormat);
      writer.WriteAttributeString("sourceLinked", axis.IsSourceLinked ? "1" : "0");
      writer.WriteEndElement();
    }
    this.SerializeAxisShapeAndTextProperties(writer, axis, chart, relations);
  }

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
    if (axis.FrameFormat.HasInterior && axis.FrameFormat.Interior.Pattern != ExcelPattern.None)
    {
      if (axis.FrameFormat.Shadow.ShadowInnerPresets == Excel2007ChartPresetsInner.NoShadow && axis.FrameFormat.Shadow.ShadowOuterPresets == Excel2007ChartPresetsOuter.NoShadow && axis.FrameFormat.Shadow.ShadowPrespectivePresets == Excel2007ChartPresetsPrespective.NoShadow && (axis.Shadow.ShadowInnerPresets != Excel2007ChartPresetsInner.NoShadow || axis.Shadow.ShadowOuterPresets != Excel2007ChartPresetsOuter.NoShadow || axis.Shadow.ShadowPrespectivePresets != Excel2007ChartPresetsPrespective.NoShadow))
      {
        if (axis.Shadow.ShadowInnerPresets != Excel2007ChartPresetsInner.NoShadow)
          axis.FrameFormat.Shadow.ShadowInnerPresets = axis.Shadow.ShadowInnerPresets;
        else if (axis.Shadow.ShadowOuterPresets != Excel2007ChartPresetsOuter.NoShadow)
          axis.FrameFormat.Shadow.ShadowOuterPresets = axis.Shadow.ShadowOuterPresets;
        else if (axis.Shadow.ShadowPrespectivePresets != Excel2007ChartPresetsPrespective.NoShadow)
          axis.FrameFormat.Shadow.ShadowPrespectivePresets = axis.Shadow.ShadowPrespectivePresets;
        axis.FrameFormat.Shadow.Angle = axis.Shadow.Angle;
        axis.FrameFormat.Shadow.Blur = axis.Shadow.Blur;
        axis.FrameFormat.Shadow.Distance = axis.Shadow.Distance;
        axis.FrameFormat.Shadow.HasCustomShadowStyle = axis.Shadow.HasCustomShadowStyle;
        axis.FrameFormat.Shadow.ShadowColor = axis.Shadow.ShadowColor;
        if (axis.Shadow.ShadowInnerPresets == Excel2007ChartPresetsInner.NoShadow && axis.Shadow.Size != 0)
          axis.FrameFormat.Shadow.Size = axis.Shadow.Size;
        axis.FrameFormat.Shadow.Transparency = axis.Shadow.Transparency;
      }
      ChartSerializatorCommon.SerializeFrameFormat(writer, (IChartFillBorder) axis.FrameFormat, axis.ParentChart, false);
    }
    else if (axis.FrameFormat.HasLineProperties || (axis.Border as ChartBorderImpl).HasLineProperties && !(axis.Border as ChartBorderImpl).AutoFormat)
    {
      writer.WriteStartElement("spPr", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      ChartSerializatorCommon.SerializeLineProperties(writer, axis.Border, (IWorkbook) parentHolder.Workbook);
      if (axis.Shadow.ShadowInnerPresets != Excel2007ChartPresetsInner.NoShadow || axis.Shadow.ShadowOuterPresets != Excel2007ChartPresetsOuter.NoShadow || axis.Shadow.ShadowPrespectivePresets != Excel2007ChartPresetsPrespective.NoShadow)
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
    if (axis.HasDisplayUnitLabel && axis.DisplayUnit != ExcelChartDisplayUnit.None)
    {
      writer.WriteStartElement("units", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      if (axis.DisplayUnit != ExcelChartDisplayUnit.Custom)
        writer.WriteAttributeString("unit", ((Excel2007ChartDisplayUnit) axis.DisplayUnit).ToString());
      else if (axisId == 2)
        writer.WriteAttributeString("unit", Excel2007ChartDisplayUnit.percentage.ToString());
      if (axis.DisplayUnitLabel.Text != null)
        this.SeriliazeChartTextArea(writer, axis.DisplayUnitLabel as ChartTextAreaImpl, chart, relations, 10.0, "unitsLabel", true, false);
      writer.WriteEndElement();
    }
    else
    {
      if (axisId != 2)
        return;
      writer.WriteStartElement("units", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      writer.WriteAttributeString("unit", Excel2007ChartDisplayUnit.percentage.ToString());
      writer.WriteEndElement();
    }
  }

  private void SerializeParetoSeries(XmlWriter writer, ChartImpl chart, ExcelChartType chartType)
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
      ChartSerializatorCommon.SerializeFrameFormat(writer, (IChartFillBorder) serie.ParetoLineFormat, chart, false);
      writer.WriteEndElement();
    }
  }

  private void SerializeChartExSeries(
    XmlWriter writer,
    ChartSerieImpl serie,
    int serieDataIndex,
    ExcelChartType chartType,
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
      ChartSerializatorCommon.SerializeFrameFormat(writer, (IChartFillBorder) dataFormatOrNull, serie.ParentChart, false, true);
    this.SerializeDataPointsSettings(writer, serie);
    if (defaultDataPoint.HasDataLabels || (serie.DataPoints as ChartDataPointsCollection).CheckDPDataLabels())
      this.SerializeDataLabels(writer, serie, chart, relations);
    writer.WriteStartElement("dataId", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    writer.WriteAttributeString("val", serieDataIndex.ToString());
    writer.WriteEndElement();
    this.SerializeLayoutProperties(writer, serie, chart, relations);
    if (chartType == ExcelChartType.Pareto)
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
    ExcelChartType chartType = chart.ChartType;
    ChartSerieDataFormatImpl serieFormat = serie.SerieFormat as ChartSerieDataFormatImpl;
    bool histogramOrPareto = chart.IsHistogramOrPareto;
    writer.WriteStartElement("layoutPr", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    if (chartType == ExcelChartType.TreeMap)
    {
      writer.WriteStartElement("parentLabelLayout", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      writer.WriteAttributeString("val", serieFormat.TreeMapLabelOption.ToString().ToLower());
      writer.WriteEndElement();
    }
    if (chartType == ExcelChartType.BoxAndWhisker || chartType == ExcelChartType.WaterFall)
    {
      writer.WriteStartElement("visibility", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      if (chartType == ExcelChartType.WaterFall)
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
    if (chartType == ExcelChartType.BoxAndWhisker)
    {
      writer.WriteStartElement("statistics", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      writer.WriteAttributeString("quartileMethod", serieFormat.QuartileCalculationType == ExcelQuartileCalculation.InclusiveMedian ? "inclusive" : "exclusive");
      writer.WriteEndElement();
    }
    if (chartType == ExcelChartType.WaterFall)
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
          writer.WriteAttributeString("underflow", ChartSerializator.ToXmlString((object) histogramAxisFormat.UnderflowBinValue));
      }
      if (((int) histogramAxisFormat.FlagOptions & 16 /*0x10*/) == 16 /*0x10*/)
      {
        if (!histogramAxisFormat.IsNotAutomaticOverFlowValue)
          writer.WriteAttributeString("overflow", "auto");
        else
          writer.WriteAttributeString("overflow", ChartSerializator.ToXmlString((object) histogramAxisFormat.OverflowBinValue));
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
          writer.WriteAttributeString("val", ChartSerializator.ToXmlString((object) histogramAxisFormat.BinWidth));
          writer.WriteEndElement();
        }
      }
      writer.WriteEndElement();
    }
  }

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
    if (dataLabels1.Position != ExcelDataLabelPosition.Automatic && dataLabels1.Position != ExcelDataLabelPosition.Moved)
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
            if (dataLabels2.Position != ExcelDataLabelPosition.Automatic && dataLabels2.Position != ExcelDataLabelPosition.Moved)
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
    if (dataLabels.NumberFormat != null)
      this.SerializeNumFormat(writer, dataLabels);
    writer.WriteStartElement("visibility", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    if (dataLabels.IsValue)
      writer.WriteAttributeString("value", "1");
    if (dataLabels.IsSeriesName)
      writer.WriteAttributeString("seriesName", "1");
    if (dataLabels.IsCategoryName)
      writer.WriteAttributeString("categoryName", "1");
    writer.WriteEndElement();
    if (dataLabels.Delimiter != null)
      writer.WriteElementString("separator", "http://schemas.microsoft.com/office/drawing/2014/chartex", dataLabels.Delimiter);
    if (this.IsFrameFormatChanged(dataLabels.FrameFormat as ChartFrameFormatImpl))
      ChartSerializatorCommon.SerializeFrameFormat(writer, (IChartFillBorder) dataLabels.FrameFormat, parentChart, false);
    if (!dataLabels.ShowTextProperties)
      return;
    ChartSerializatorCommon.SerializeDefaultTextFormatting(writer, (IFont) dataLabels, (IWorkbook) parentChart.ParentWorkbook, 10.0, true, 0, Excel2007TextRotation.horz, "http://schemas.microsoft.com/office/drawing/2014/chartex", false, false);
  }

  private bool IsFrameFormatChanged(ChartFrameFormatImpl format)
  {
    return (format.HasInterior || format.HasLineProperties) && (!format.Interior.UseAutomaticFormat || !format.LineProperties.AutoFormat) || format.HasShadowProperties || format.Has3dProperties;
  }

  private void SerializeNumFormat(XmlWriter writer, ChartDataLabelsImpl dataLabels)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    bool flag = dataLabels != null ? dataLabels.IsSourceLinked : throw new ArgumentNullException(nameof (dataLabels));
    if (!(dataLabels.NumberFormat != "General") && flag)
      return;
    writer.WriteStartElement("numFmt", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    writer.WriteAttributeString("formatCode", dataLabels.NumberFormat);
    if (!flag)
      writer.WriteAttributeString("sourceLinked", Convert.ToInt16(flag).ToString());
    writer.WriteEndElement();
  }

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
            ChartSerializatorCommon.SerializeFrameFormat(writer, (IChartFillBorder) dataFormatOrNull, dataFormatOrNull.ParentChart, false);
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
      writer.WriteString(serie.NameRange.AddressGlobal);
      writer.WriteEndElement();
    }
    else
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
    ExcelChartType chartType,
    int serieIndex,
    bool isPareto)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (serie == null)
      throw new ArgumentNullException(nameof (serie));
    string str = ((Excel2016Charttype) chartType).ToString();
    if (chartType == ExcelChartType.Pareto)
      str = !isPareto ? Excel2016Charttype.clusteredColumn.ToString() : Excel2016Charttype.paretoLine.ToString();
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

  private void SerializeLegend(XmlWriter writer, ChartLegendImpl chartLegend, ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chartLegend == null)
      throw new ArgumentNullException(nameof (chartLegend));
    writer.WriteStartElement("legend", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    ExcelLegendPosition position1 = chartLegend.Position;
    ushort position2 = chartLegend.ChartExPosition;
    if (position1 != ExcelLegendPosition.NotDocked)
    {
      position2 = (ushort) 0;
      switch (position1)
      {
        case ExcelLegendPosition.Bottom:
          position2 |= (ushort) 72;
          break;
        case ExcelLegendPosition.Corner:
          position2 |= (ushort) 20;
          break;
        case ExcelLegendPosition.Top:
          position2 |= (ushort) 66;
          break;
        case ExcelLegendPosition.Right:
          position2 |= (ushort) 68;
          break;
        case ExcelLegendPosition.Left:
          position2 |= (ushort) 65;
          break;
      }
    }
    this.SerializeTextElementAttributes(writer, position2, chartLegend.IncludeInLayout);
    ChartSerializatorCommon.SerializeFrameFormat(writer, (IChartFillBorder) chartLegend.FrameFormat, chart, false);
    chartLegend.IsChartTextArea = true;
    if (((IInternalChartTextArea) chartLegend.TextArea).ParagraphType == ChartParagraphType.CustomDefault)
      ChartSerializatorCommon.SerializeDefaultTextFormatting(writer, (IFont) chartLegend.TextArea, (IWorkbook) chart.ParentWorkbook, 10.0, true, 0, Excel2007TextRotation.horz, "http://schemas.microsoft.com/office/drawing/2014/chartex", false, true);
    writer.WriteEndElement();
  }

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
      ChartSerializatorCommon.SerializeFrameFormat(writer, (IChartFillBorder) chartTextArea.FrameFormat, dataHolder, relations, false, false);
      if (!flag && chartTextArea.ParagraphType == ChartParagraphType.CustomDefault)
        ChartSerializatorCommon.SerializeDefaultTextFormatting(writer, (IFont) chartTextArea, (IWorkbook) chart.ParentWorkbook, defaultFontSize, true, 0, Excel2007TextRotation.horz, "http://schemas.microsoft.com/office/drawing/2014/chartex", true, false);
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
      ChartSerializatorCommon.SerializeRichText(writer, (IChartTextArea) chartTextArea, (IWorkbook) chart.ParentWorkbook, "rich", defaultFontSize);
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
    ExcelChartType chartType = chart != null ? chart.ChartType : throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("chartData", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    if (chart.ParentWorkbook.IsLoaded && chart.ChartExRelationId != null)
    {
      writer.WriteStartElement("externalData", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", chart.ChartExRelationId);
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
    ExcelChartType chartType)
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
    this.SerializeDimensionData(writer, serie.Values, serie.EnteredDirectlyValues, serie.IsRowWiseSeries, serie.FormatCode, false);
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
    this.SerializeDimensionData(writer, serie.CategoryLabels, serie.EnteredDirectlyCategoryLabels, serie.IsRowWiseCategory, serie.CategoriesFormatCode, true);
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private void SerializeDimensionData(
    XmlWriter writer,
    IRange range,
    object[] directValues,
    bool isInRow,
    string formatCode,
    bool isCategoryRange)
  {
    if (range != null)
    {
      writer.WriteStartElement("f", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      if (!(range is IRanges))
        writer.WriteAttributeString("dir", isInRow ? "row" : "col");
      if (range is NameImpl)
        writer.WriteString((range as NameImpl).Name);
      else
        writer.WriteString(range.AddressGlobal);
      writer.WriteEndElement();
    }
    if (directValues == null)
      return;
    writer.WriteStartElement("lvl", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    writer.WriteAttributeString("ptCount", directValues.Length.ToString());
    formatCode = formatCode == null || !(formatCode != string.Empty) ? "General" : formatCode;
    writer.WriteAttributeString(nameof (formatCode), formatCode);
    for (int index = 0; index < directValues.Length; ++index)
    {
      if (directValues[index] != null)
      {
        writer.WriteStartElement("pt", "http://schemas.microsoft.com/office/drawing/2014/chartex");
        writer.WriteAttributeString("idx", index.ToString());
        writer.WriteString(ChartSerializator.ToXmlString(directValues[index]));
        writer.WriteEndElement();
      }
    }
    writer.WriteEndElement();
  }

  private void SerializePrinterSettings(
    XmlWriter writer,
    ChartImpl chart,
    RelationCollection relations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    IChartPageSetup pageSetup = chart != null ? chart.PageSetup : throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("printSettings", "http://schemas.microsoft.com/office/drawing/2014/chartex");
    IPageSetupConstantsProvider constants = (IPageSetupConstantsProvider) new ChartPageSetupConstants(true);
    Excel2007Serializator.SerializePrintSettings(writer, (IPageSetupBase) pageSetup, constants, true);
    Excel2007Serializator.SerializeVmlHFShapesWorksheetPart(writer, (WorksheetBaseImpl) chart, constants, relations);
    chart.DataHolder.SerializeHeaderFooterImages((WorksheetBaseImpl) chart, relations);
    writer.WriteEndElement();
  }
}
