// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Charts.ChartExParser
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Compression;
using Syncfusion.XlsIO.Implementation.Charts;
using Syncfusion.XlsIO.Implementation.XmlReaders;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Interfaces.Charts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization.Charts;

internal class ChartExParser(WorkbookImpl book) : ChartParser(book)
{
  internal void ParseChartEx(XmlReader reader, ChartImpl chart, RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.LocalName != "chartSpace")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    IChartFrameFormat chartArea = chart.ChartArea;
    chartArea.Interior.UseAutomaticFormat = true;
    chartArea.Border.AutoFormat = true;
    Dictionary<int, ChartExDataCache> dictionary1 = (Dictionary<int, ChartExDataCache>) null;
    Dictionary<int, int> dictionary2 = (Dictionary<int, int>) null;
    while (reader.NodeType != XmlNodeType.EndElement && reader.LocalName != "chartSpace")
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "chartData":
            dictionary1 = this.ParseChartExData(reader, chart, relations);
            continue;
          case nameof (chart):
            dictionary2 = this.ParseChartExElement(reader, chart, relations);
            continue;
          case "spPr":
            IChartFillObjectGetter objectGetter = (IChartFillObjectGetter) new ChartFillObjectGetterAny(chartArea.Border as ChartBorderImpl, chartArea.Interior as ChartInteriorImpl, chartArea.Fill as IInternalFill, chartArea.Shadow as ShadowImpl, chartArea.ThreeD as ThreeDFormatImpl);
            ChartParserCommon.ParseShapeProperties(reader, objectGetter, chart.ParentWorkbook.DataHolder, relations);
            continue;
          case "txPr":
            this.ParseDefaultTextProperties(reader, chart);
            continue;
          case "printSettings":
            this.ParsePrintSettings(reader, chart, relations);
            continue;
          case "clrMapOvr":
            MemoryStream data = new MemoryStream();
            XmlWriter writer = UtilityMethods.CreateWriter((Stream) data, Encoding.UTF8);
            writer.WriteNode(reader, false);
            writer.Flush();
            chart.m_colorMapOverrideStream = data;
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    if (dictionary1 == null || dictionary2 == null)
      return;
    foreach (int key1 in dictionary2.Keys)
    {
      int key2 = dictionary2[key1];
      dictionary1[key2].CopyProperties(chart.Series[key1] as ChartSerieImpl, chart.ParentWorkbook);
    }
    dictionary1.Clear();
  }

  private Dictionary<int, ChartExDataCache> ParseChartExData(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "chartData")
      throw new XmlException("Unexpected xml tag.");
    Dictionary<int, ChartExDataCache> chartExData = new Dictionary<int, ChartExDataCache>();
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "data":
            KeyValuePair<int, ChartExDataCache> chartExDataCache = this.TryParseChartExDataCache(reader, chart, relations);
            if (chartExDataCache.Value != null)
            {
              chartExData.Add(chartExDataCache.Key, chartExDataCache.Value);
              continue;
            }
            continue;
          case "externalData":
            this.ParseExternalDataAttributes(reader, chart);
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
    return chartExData;
  }

  private void ParseExternalDataAttributes(XmlReader reader, ChartImpl chart)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "externalData")
      throw new XmlException("Unexpected xml tag.");
    if (reader.MoveToAttribute("id", (chart.Workbook as WorkbookImpl).IsStrict ? "http://purl.oclc.org/ooxml/officeDocument/relationships" : "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
      chart.ChartExRelationId = reader.Value;
    if (reader.MoveToAttribute("autoUpdate", "http://schemas.microsoft.com/office/drawing/2014/chartex"))
      chart.AutoUpdate = new bool?(XmlConvertExtension.ToBoolean(reader.Value));
    reader.Skip();
    reader.Read();
  }

  private KeyValuePair<int, ChartExDataCache> TryParseChartExDataCache(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "data")
      throw new XmlException("Unexpected xml tag.");
    KeyValuePair<int, ChartExDataCache> chartExDataCache = new KeyValuePair<int, ChartExDataCache>();
    if (reader.MoveToAttribute("id"))
    {
      ChartExDataCache cache = new ChartExDataCache();
      chartExDataCache = new KeyValuePair<int, ChartExDataCache>(XmlConvertExtension.ToInt32(reader.Value), cache);
      reader.MoveToElement();
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "numDim":
              this.ParseDimensionData(reader, cache);
              continue;
            case "strDim":
              this.ParseDimensionData(reader, cache);
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
    else
      reader.Skip();
    reader.Read();
    return chartExDataCache;
  }

  private void ParseDimensionData(XmlReader reader, ChartExDataCache cache)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (cache == null)
      throw new ArgumentNullException(nameof (cache));
    if (reader.LocalName != "strDim" && reader.LocalName != "numDim")
      throw new XmlException("Unexpected xml tag.");
    bool isCategoryValues = false;
    if (reader.MoveToAttribute("type"))
    {
      if (reader.Value == "cat")
        isCategoryValues = true;
      reader.MoveToElement();
    }
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "f":
              if (reader.MoveToAttribute("dir"))
              {
                if (reader.Value.ToLower() == "row")
                {
                  if (isCategoryValues)
                    cache.IsRowWiseCategory = true;
                  else
                    cache.IsRowWiseSeries = true;
                }
                reader.MoveToElement();
              }
              string str = reader.ReadElementContentAsString();
              if (isCategoryValues)
                cache.CategoryFormula = str;
              else
                cache.SeriesFormula = str;
              Excel2007Parser.SkipWhiteSpaces(reader);
              if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "f")
              {
                reader.Read();
                continue;
              }
              continue;
            case "lvl":
              this.ParseChartExLevelElement(reader, cache, isCategoryValues);
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
    }
    else
      reader.Skip();
  }

  private void ParseChartExLevelElement(
    XmlReader reader,
    ChartExDataCache cache,
    bool isCategoryValues)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (cache == null)
      throw new ArgumentNullException(nameof (cache));
    if (reader.LocalName != "lvl")
      throw new XmlException("Unexpected xml tag.");
    int index = 0;
    List<object> list = new List<object>();
    if (reader.MoveToAttribute("formatCode"))
    {
      if (isCategoryValues)
        cache.CategoriesFormatCode = reader.Value;
      else
        cache.SeriesFormatCode = reader.Value;
    }
    if (reader.MoveToAttribute("ptCount"))
      XmlConvertExtension.ToInt32(reader.Value);
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "pt":
              int num = this.AddNumericPoint(reader, list);
              if (num != -1 && num > index)
              {
                do
                {
                  if (isCategoryValues)
                    list.Insert(index, (object) "");
                  else
                    list.Insert(index, (object) null);
                  ++index;
                }
                while (index != num && num > index);
                continue;
              }
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
    else
      reader.Skip();
    if (isCategoryValues)
      cache.CategoryValues = list.ToArray();
    else
      cache.SeriesValues = list.ToArray();
    reader.Read();
  }

  private Dictionary<int, int> ParseChartExElement(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != nameof (chart))
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    FileDataHolder parentHolder = chart.DataHolder.ParentHolder;
    bool flag = false;
    Dictionary<int, int> chartExElement = (Dictionary<int, int>) null;
    MemoryStream memoryStream = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((Stream) memoryStream, Encoding.UTF8);
    writer.WriteStartElement("root");
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "plotArea":
            chartExElement = this.ParseChartExPlotArea(reader, chart, relations, parentHolder.Parser);
            continue;
          case "legend":
            chart.HasLegend = true;
            this.ParseLegend(reader, chart.Legend as ChartLegendImpl, chart, relations);
            continue;
          case "title":
            bool? isOverlay = new bool?(false);
            ushort position = 0;
            this.TryParsePositioningValues(reader, out isOverlay, out position);
            if (isOverlay.HasValue)
              chart.ChartTitleIncludeInLayout = isOverlay.Value;
            if (position != (ushort) 0)
              chart.ChartExTitlePosition = position;
            reader.MoveToElement();
            writer.WriteNode(reader, false);
            writer.Flush();
            flag = true;
            chart.HasAutoTitle = new bool?(true);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    writer.WriteEndElement();
    writer.Flush();
    if (flag)
    {
      chart.ChartTitleArea.Bold = false;
      ChartParserCommon.ParseChartTitleElement((Stream) memoryStream, chart.ChartTitleArea as IInternalChartTextArea, parentHolder, relations, 18f);
    }
    reader.Read();
    return chartExElement;
  }

  private Dictionary<int, int> ParseChartExPlotArea(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    Excel2007Parser excel2007Parser)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "plotArea")
      throw new XmlException("Unexpected xml tag");
    reader.Read();
    chart.HasPlotArea = false;
    bool flag = chart.PlotArea != null && chart.ChartArea.IsBorderCornersRound;
    FileDataHolder parentHolder = chart.DataHolder.ParentHolder;
    int secondaryAxisId = -1;
    List<int> hashCodeList = new List<int>(3);
    ChartExAxisParser axisParser = (ChartExAxisParser) null;
    Dictionary<int, int> chartExPlotArea = (Dictionary<int, int>) null;
    while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "spPr":
            if (!chart.HasPlotArea)
            {
              chart.HasPlotArea = true;
              IChartFrameFormat plotArea = chart.PlotArea;
              chart.ChartArea.IsBorderCornersRound = flag;
              ChartFillObjectGetterAny objectGetter = new ChartFillObjectGetterAny(plotArea.Border as ChartBorderImpl, plotArea.Interior as ChartInteriorImpl, plotArea.Fill as IInternalFill, plotArea.Shadow as ShadowImpl, plotArea.ThreeD as ThreeDFormatImpl);
              ChartParserCommon.ParseShapeProperties(reader, (IChartFillObjectGetter) objectGetter, parentHolder, relations);
              continue;
            }
            reader.Skip();
            continue;
          case "plotAreaRegion":
            chartExPlotArea = this.ParsePlotAreaRegion(reader, chart, relations, excel2007Parser, out secondaryAxisId);
            continue;
          case "axis":
            if (axisParser == null)
              axisParser = new ChartExAxisParser(chart.ParentWorkbook);
            int chartExAxes = this.ParseChartExAxes(reader, secondaryAxisId, hashCodeList, axisParser, chart, parentHolder, excel2007Parser, relations);
            if (chartExAxes != -1 && hashCodeList.Count < 3)
            {
              hashCodeList.Add(chartExAxes);
              continue;
            }
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
    return chartExPlotArea;
  }

  private Dictionary<int, int> ParsePlotAreaRegion(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    Excel2007Parser excel2007Parser,
    out int secondaryAxisId)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "plotAreaRegion")
      throw new XmlException("Unexpected xml tag");
    reader.Read();
    secondaryAxisId = -1;
    int count = chart.Series.Count;
    Dictionary<int, int> plotAreaRegion = new Dictionary<int, int>();
    while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "series":
            int secondaryAxisId1 = -1;
            int chartExSeries = this.ParseChartExSeries(reader, chart, relations, excel2007Parser, out secondaryAxisId1);
            if (secondaryAxisId != secondaryAxisId1)
              secondaryAxisId = secondaryAxisId1;
            if (count < chart.Series.Count)
            {
              plotAreaRegion.Add(count, chartExSeries);
              ++count;
              continue;
            }
            continue;
          case "plotSurface":
            if (!reader.IsEmptyElement)
            {
              this.ParsePlotSurface(reader, chart, relations, excel2007Parser);
              continue;
            }
            reader.Skip();
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    if (chart.Series.Count > 0 && chart.IsHistogramOrPareto && (chart.Series[0].SerieFormat as ChartSerieDataFormatImpl).HistogramAxisFormatProperty != null)
      (chart.PrimaryCategoryAxis as ChartCategoryAxisImpl).HistogramAxisFormatProperty.Clone((chart.Series[0].SerieFormat as ChartSerieDataFormatImpl).HistogramAxisFormatProperty);
    reader.Read();
    return plotAreaRegion;
  }

  private int ParseChartExSeries(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    Excel2007Parser excel2007Parser,
    out int secondaryAxisId)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "series")
      throw new XmlException("Unexpected xml tag");
    ChartFrameFormatImpl paretoLineFormat = (ChartFrameFormatImpl) null;
    ChartSerieImpl seriesFromAttributes = this.TryParseSeriesFromAttributes(reader, chart, out paretoLineFormat);
    secondaryAxisId = -1;
    int chartExSeries = 0;
    if (seriesFromAttributes != null)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "tx":
              if (!reader.IsEmptyElement)
              {
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                  if (reader.LocalName == "txData")
                  {
                    string formula = (string) null;
                    seriesFromAttributes.Name = ChartParserCommon.ParseFormulaOrValue(reader, out formula);
                    if (formula != null)
                      seriesFromAttributes.Name = !(ChartParser.GetRange(chart.ParentWorkbook, formula) is IName range) || range.RefersToRange == null ? "=" + formula : "=" + range.RefersToRange.AddressGlobal;
                    reader.Read();
                  }
                  else
                    reader.Read();
                  if (reader.LocalName == "tx" && reader.NodeType == XmlNodeType.EndElement)
                  {
                    reader.Read();
                    break;
                  }
                }
                continue;
              }
              reader.Skip();
              continue;
            case "spPr":
              this.ParseSeriesProperties(reader, seriesFromAttributes, relations);
              continue;
            case "dataPt":
              this.ParseDataPoint(reader, seriesFromAttributes, relations);
              continue;
            case "dataLabels":
              this.ParseDataLabels(reader, seriesFromAttributes, relations, true);
              continue;
            case "dataId":
              if (reader.MoveToAttribute("val"))
                chartExSeries = XmlConvertExtension.ToInt32(reader.Value);
              reader.Skip();
              continue;
            case "layoutPr":
              this.ParseChartExSeriesLayoutProperties(reader, seriesFromAttributes, relations);
              continue;
            case "axisId":
              int num = -1;
              if (reader.MoveToAttribute("val"))
                num = XmlConvertExtension.ToInt32(reader.Value);
              if (num != -1)
              {
                (chart.PrimaryValueAxis as ChartAxisImpl).AxisId = num;
                if (paretoLineFormat != null)
                  secondaryAxisId = num;
              }
              reader.Skip();
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
    }
    else if (paretoLineFormat != null)
      secondaryAxisId = this.ParseParetoLineFormat(reader, paretoLineFormat, chart.ParentWorkbook.DataHolder, relations);
    else
      reader.Skip();
    return chartExSeries;
  }

  private int ParseParetoLineFormat(
    XmlReader reader,
    ChartFrameFormatImpl paretoLineFormat,
    FileDataHolder fileDataHolder,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (paretoLineFormat == null)
      throw new ArgumentNullException("Series Data Format");
    if (reader.LocalName != "series")
      throw new XmlException("Unexpected xml tag");
    reader.Read();
    int paretoLineFormat1 = -1;
    while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "spPr":
            ChartFillObjectGetterAny objectGetter = new ChartFillObjectGetterAny(paretoLineFormat.Border as ChartBorderImpl, paretoLineFormat.Interior as ChartInteriorImpl, paretoLineFormat.Fill as IInternalFill, paretoLineFormat.Shadow as ShadowImpl, paretoLineFormat.ThreeD as ThreeDFormatImpl);
            ChartParserCommon.ParseShapeProperties(reader, (IChartFillObjectGetter) objectGetter, fileDataHolder, relations);
            continue;
          case "axisId":
            if (reader.MoveToAttribute("val"))
              paretoLineFormat1 = XmlConvertExtension.ToInt32(reader.Value);
            reader.Skip();
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
    return paretoLineFormat1;
  }

  private ChartSerieImpl TryParseSeriesFromAttributes(
    XmlReader reader,
    ChartImpl chart,
    out ChartFrameFormatImpl paretoLineFormat)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "series")
      throw new XmlException("Unexpected xml tag");
    ChartSerieImpl seriesFromAttributes = (ChartSerieImpl) null;
    paretoLineFormat = (ChartFrameFormatImpl) null;
    if (reader.MoveToAttribute("layoutId"))
    {
      if (reader.Value == "regionMap")
        return (ChartSerieImpl) null;
      Excel2016Charttype serieType = (Excel2016Charttype) Enum.Parse(typeof (Excel2016Charttype), reader.Value, true);
      if (serieType != Excel2016Charttype.paretoLine)
      {
        seriesFromAttributes = chart.Series.Add((ExcelChartType) serieType) as ChartSerieImpl;
        seriesFromAttributes.Number = -1;
        if (serieType == Excel2016Charttype.clusteredColumn)
          (seriesFromAttributes.SerieFormat as ChartSerieDataFormatImpl).HistogramAxisFormatProperty = new HistogramAxisFormat();
      }
    }
    if (reader.MoveToAttribute("ownerIdx") && seriesFromAttributes == null)
    {
      int int32 = XmlConvertExtension.ToInt32(reader.Value);
      chart.Series[int32].SerieType = ExcelChartType.Pareto;
      paretoLineFormat = chart.Series[int32].ParetoLineFormat as ChartFrameFormatImpl;
    }
    if (reader.MoveToAttribute("formatIdx"))
    {
      if (seriesFromAttributes != null)
        seriesFromAttributes.Number = XmlConvertExtension.ToInt32(reader.Value);
      else if (paretoLineFormat != null)
        (paretoLineFormat.Parent as ChartSerieImpl).ParetoLineFormatIndex = XmlConvertExtension.ToInt32(reader.Value);
    }
    if (reader.MoveToAttribute("hidden"))
    {
      if (seriesFromAttributes != null)
        seriesFromAttributes.IsSeriesHidden = XmlConvertExtension.ToBoolean(reader.Value);
      else if (paretoLineFormat != null)
        (paretoLineFormat.Parent as ChartSerieImpl).IsParetoLineHidden = XmlConvertExtension.ToBoolean(reader.Value);
    }
    reader.MoveToElement();
    return seriesFromAttributes;
  }

  private void ParseChartExSeriesLayoutProperties(
    XmlReader reader,
    ChartSerieImpl series,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    if (reader.LocalName != "layoutPr")
      throw new XmlException("Unexpected xml tag");
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      ChartSerieDataFormatImpl serieFormat = series.SerieFormat as ChartSerieDataFormatImpl;
      while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "parentLabelLayout":
              if (reader.MoveToAttribute("val"))
                serieFormat.TreeMapLabelOption = (ExcelTreeMapLabelOption) Enum.Parse(typeof (ExcelTreeMapLabelOption), reader.Value, true);
              reader.Skip();
              continue;
            case "visibility":
              this.ParseChartSeriesVisibility(reader, serieFormat);
              continue;
            case "aggregation":
              serieFormat.IsBinningByCategory = true;
              reader.Skip();
              continue;
            case "binning":
              this.ParseSeriesBinningProperties(reader, serieFormat);
              continue;
            case "statistics":
              if (reader.MoveToAttribute("quartileMethod"))
              {
                serieFormat.QuartileCalculationType = !(reader.Value == "inclusive") ? ExcelQuartileCalculation.ExclusiveMedian : ExcelQuartileCalculation.InclusiveMedian;
                continue;
              }
              continue;
            case "subtotals":
              if (!reader.IsEmptyElement)
              {
                reader.Read();
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                  if (reader.LocalName == "idx" && reader.MoveToAttribute("val"))
                  {
                    int int32 = XmlConvertExtension.ToInt32(reader.Value);
                    series.DataPoints[int32].SetAsTotal = true;
                    reader.Skip();
                  }
                  else
                    reader.Skip();
                  if (reader.LocalName == "subtotals" && reader.NodeType == XmlNodeType.EndElement)
                  {
                    reader.Read();
                    break;
                  }
                }
                continue;
              }
              reader.Skip();
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

  private void ParseSeriesBinningProperties(XmlReader reader, ChartSerieDataFormatImpl dataFormat)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (dataFormat == null)
      throw new ArgumentNullException("Serie Data Format");
    if (reader.LocalName != "binning")
      throw new XmlException("Unexpeced xml tag.");
    if (reader.MoveToAttribute("underflow"))
    {
      if (reader.Value != "auto")
        dataFormat.UnderflowBinValue = XmlConvertExtension.ToDouble(reader.Value);
      else
        dataFormat.HistogramAxisFormatProperty.IsNotAutomaticUnderFlowValue = false;
    }
    if (reader.MoveToAttribute("overflow"))
    {
      if (reader.Value != "auto")
        dataFormat.OverflowBinValue = XmlConvertExtension.ToDouble(reader.Value);
      else
        dataFormat.HistogramAxisFormatProperty.IsNotAutomaticOverFlowValue = false;
    }
    if (reader.MoveToAttribute("intervalClosed") && reader.Value == "l")
      dataFormat.IsIntervalClosedinLeft = true;
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "binSize":
              if (reader.MoveToAttribute("val"))
                dataFormat.BinWidth = XmlConvertExtension.ToDouble(reader.Value);
              reader.Skip();
              continue;
            case "binCount":
              if (reader.MoveToAttribute("val"))
                dataFormat.NumberOfBins = XmlConvertExtension.ToInt32(reader.Value);
              reader.Skip();
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

  private void ParseChartSeriesVisibility(XmlReader reader, ChartSerieDataFormatImpl dataFormat)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (dataFormat == null)
      throw new ArgumentNullException("Serie Data Format");
    if (reader.LocalName != "visibility")
      throw new XmlException("Unexpeced xml tag.");
    if (reader.MoveToAttribute("connectorLines"))
      dataFormat.ShowConnectorLines = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("meanLine"))
      dataFormat.ShowMeanLine = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("meanMarker"))
      dataFormat.ShowMeanMarkers = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("outliers"))
      dataFormat.ShowOutlierPoints = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("nonoutliers"))
      dataFormat.ShowInnerPoints = XmlConvertExtension.ToBoolean(reader.Value);
    reader.Skip();
  }

  private void ParsePlotSurface(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    Excel2007Parser excel2007Parser)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "plotSurface")
      throw new XmlException("Unexpected xml tag");
    bool flag = chart.PlotArea != null && chart.ChartArea.IsBorderCornersRound;
    FileDataHolder parentHolder = chart.DataHolder.ParentHolder;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "spPr":
            chart.HasPlotArea = true;
            IChartFrameFormat plotArea = chart.PlotArea;
            chart.ChartArea.IsBorderCornersRound = flag;
            ChartFillObjectGetterAny objectGetter = new ChartFillObjectGetterAny(plotArea.Border as ChartBorderImpl, plotArea.Interior as ChartInteriorImpl, plotArea.Fill as IInternalFill, plotArea.Shadow as ShadowImpl, plotArea.ThreeD as ThreeDFormatImpl);
            ChartParserCommon.ParseShapeProperties(reader, (IChartFillObjectGetter) objectGetter, parentHolder, relations);
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
  }

  private int ParseChartExAxes(
    XmlReader reader,
    int secondaryAxisId,
    List<int> hashCodeList,
    ChartExAxisParser axisParser,
    ChartImpl chart,
    FileDataHolder dataHolder,
    Excel2007Parser excel2007parser,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "axis")
      throw new XmlException("Unexpected xml tag");
    bool? axisIsHidden = new bool?();
    int? axisId = new int?();
    int chartExAxes = -1;
    axisParser.ParseAxisCommonAttributes(reader, out axisIsHidden, out axisId);
    if (!reader.IsEmptyElement)
    {
      MemoryStream data = new MemoryStream();
      XmlWriter writer = UtilityMethods.CreateWriter((Stream) data, Encoding.UTF8);
      writer.WriteNode(reader, false);
      writer.Flush();
      data.Position = 0L;
      XmlReader reader1 = UtilityMethods.CreateReader((Stream) data);
      reader1.Read();
      ChartAxisImpl axisFromReader = this.TryParseAxisFromReader(reader1, axisParser, chart, axisId, secondaryAxisId);
      if (axisFromReader != null && !hashCodeList.Contains(axisFromReader.GetHashCode()))
      {
        if (axisIsHidden.HasValue && axisIsHidden.Value)
          axisFromReader.Deleted = true;
        data.Position = 0L;
        reader1 = UtilityMethods.CreateReader((Stream) data);
        axisParser.ParseChartExAxis(reader1, axisFromReader, chart, relations, excel2007parser, dataHolder);
        chartExAxes = axisFromReader.GetHashCode();
      }
      reader1.Close();
      writer.Close();
      data.Dispose();
    }
    return chartExAxes;
  }

  private ChartAxisImpl TryParseAxisFromReader(
    XmlReader axisReader,
    ChartExAxisParser axisParser,
    ChartImpl chart,
    int? currentAxisId,
    int secondaryAxisId)
  {
    if (axisReader == null)
      throw new ArgumentNullException("reader");
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    ChartAxisImpl axis = (ChartAxisImpl) null;
    while (axisReader.NodeType != XmlNodeType.EndElement)
    {
      if (axisReader.NodeType == XmlNodeType.Element)
      {
        switch (axisReader.LocalName)
        {
          case "valScaling":
            axis = !currentAxisId.HasValue || currentAxisId.Value != secondaryAxisId ? chart.PrimaryValueAxis as ChartAxisImpl : chart.SecondaryValueAxis as ChartAxisImpl;
            axisParser.ParseAxisAttributes(axisReader, axis, true);
            axisReader.Skip();
            continue;
          case "catScaling":
            axis = chart.PrimaryCategoryAxis as ChartAxisImpl;
            axisParser.ParseAxisAttributes(axisReader, axis, false);
            axisReader.Skip();
            continue;
          default:
            axisReader.Skip();
            continue;
        }
      }
      else
        axisReader.Skip();
    }
    return axis;
  }

  internal override void ParseChartDataLabelVisibility(
    XmlReader reader,
    ChartDataLabelsImpl dataLabels)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (dataLabels == null)
      throw new ArgumentNullException("data labels");
    if (reader.LocalName != "visibility")
      throw new XmlException("Unexpeced xml tag.");
    dataLabels.IsSeriesName = dataLabels.IsValue = dataLabels.IsCategoryName = false;
    if (reader.MoveToAttribute("seriesName"))
      dataLabels.IsSeriesName = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("categoryName"))
      dataLabels.IsCategoryName = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("value"))
      dataLabels.IsValue = XmlConvertExtension.ToBoolean(reader.Value);
    reader.Skip();
  }
}
