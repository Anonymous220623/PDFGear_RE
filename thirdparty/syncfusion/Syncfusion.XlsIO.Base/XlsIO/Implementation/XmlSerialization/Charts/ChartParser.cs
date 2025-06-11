// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Charts.ChartParser
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Compression;
using Syncfusion.XlsIO.Implementation.Charts;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Implementation.XmlReaders;
using Syncfusion.XlsIO.Implementation.XmlReaders.Shapes;
using Syncfusion.XlsIO.Implementation.XmlSerialization.Constants;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Interfaces.Charts;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Charts;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization.Charts;

public class ChartParser
{
  internal const float DefaultTitleSize = 18f;
  internal const string EndParaTag = "endParaRPr";
  private WorkbookImpl m_book;

  public ChartParser(WorkbookImpl book)
  {
    this.m_book = book;
    ChartParserCommon.SetWorkbook(this.m_book);
  }

  public void ParseChart(XmlReader reader, ChartImpl chart, RelationCollection relations)
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
    while (reader.NodeType != XmlNodeType.EndElement && reader.LocalName != "chartSpace")
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case nameof (chart):
            this.ParseChartElement(reader, chart, relations);
            continue;
          case "roundedCorners":
            chart.HasPlotArea = true;
            chart.ChartArea.IsBorderCornersRound = ChartParserCommon.ParseBoolValueTag(reader);
            continue;
          case "spPr":
            IChartFillObjectGetter objectGetter = (IChartFillObjectGetter) new ChartFillObjectGetterAny(chartArea.Border as ChartBorderImpl, chartArea.Interior as ChartInteriorImpl, chartArea.Fill as IInternalFill, chartArea.Shadow as ShadowImpl, chartArea.ThreeD as ThreeDFormatImpl);
            ChartParserCommon.ParseShapeProperties(reader, objectGetter, chart.ParentWorkbook.DataHolder, relations);
            continue;
          case "style":
            chart.Style = ChartParserCommon.ParseIntValueTag(reader);
            continue;
          case "userShapes":
            this.ParseUserShapes(reader, chart, relations);
            continue;
          case "pivotSource":
            this.ParsePivotSource(reader, chart);
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
          case "extLst":
            this.ParseExtensionList(reader, chart);
            continue;
          case "AlternateContent":
            Stream stream = ShapeParser.ReadNodeAsStream(reader);
            int style = chart.Style;
            this.ParseStyleIdFromAlternateContent(stream, chart);
            stream.Position = 0L;
            if (style == chart.Style)
            {
              chart.AlternateContent = stream;
              continue;
            }
            continue;
          case "txPr":
            this.ParseDefaultTextProperties(reader, chart);
            continue;
          case "lang":
            chart.m_lang = ChartParserCommon.ParseValueTag(reader);
            continue;
          default:
            reader.Read();
            continue;
        }
      }
      else
        reader.Skip();
    }
    chart.DetectIsInRowOnParsing();
    if (chart.DataRange != null && chart.Categories.Count > 0)
    {
      IRange range = (IRange) null;
      chart.IsSeriesInRows = this.DetectIsInRow(chart.Series[0].Values);
      this.GetSerieOrAxisRange(chart.DataRange, chart.IsSeriesInRows, out range, 0);
      int CategoryLabelCount = 0;
      if (chart.Series[0].CategoryLabels != null)
        CategoryLabelCount = !chart.IsSeriesInRows ? chart.Series[0].CategoryLabels.LastColumn - chart.Series[0].CategoryLabels.Column : chart.Series[0].CategoryLabels.LastRow - chart.Series[0].CategoryLabels.Row;
      this.GetSerieOrAxisRange(range, !chart.IsSeriesInRows, out range, CategoryLabelCount);
      int count = range.Count / chart.Series[0].Values.Count;
      int num1 = 0;
      int num2 = chart.Series[0].CategoryLabels == null ? chart.Series[0].Values.Count : (num1 = chart.Series[0].CategoryLabels.Count / (CategoryLabelCount + 1));
      for (int index = 0; index < num2; ++index)
      {
        IRange categoryRange = ChartImpl.GetCategoryRange(range, out range, count, chart.IsSeriesInRows);
        (chart.Categories[index] as ChartCategory).CategoryLabel = chart.Series[0].CategoryLabels;
        (chart.Categories[index] as ChartCategory).Values = categoryRange;
        if (chart.Categories[0].CategoryLabel != null)
        {
          (chart.Categories[index] as ChartCategory).Name = chart.Categories[0].CategoryLabel.Cells[index].Text;
        }
        else
        {
          (chart.Categories[index] as ChartCategory).Name = (index + 1).ToString();
          if (chart.Legend != null && chart.Legend.LegendEntries != null && chart.Legend.LegendEntries.Count > index && chart.Legend.LegendEntries[index].TextArea != null)
          {
            chart.Legend.LegendEntries[index].TextArea.Text = (chart.Categories[index] as ChartCategory).Name;
            chart.Legend.LegendEntries[index].IsFormatted = false;
          }
        }
      }
    }
    else if (chart.Series != null && chart.Series.Count > 0 && chart.Series[0].Values != null && chart.Series[0].CategoryLabels != null && chart.Categories.Count > 0)
      chart.UpdateChartCategoriesByRange();
    if (chart.Series.Count != 0 && (chart.Series[0] as ChartSerieImpl).FilteredValue != null)
      this.FindFilter(chart.Categories, (chart.Series[0] as ChartSerieImpl).FilteredValue, chart.Series[0].Values.AddressGlobal, chart.Series[0], chart.IsSeriesInRows);
    if (chart.CommonDataPointsCollection != null)
    {
      foreach (ChartSerieImpl chartSerieImpl in (CollectionBase<IChartSerie>) chart.Series)
      {
        if (!(chartSerieImpl.DataPoints.DefaultDataPoint as ChartDataPointImpl).HasDataLabels)
        {
          ChartDataLabelsImpl dataLabels1 = chartSerieImpl.DataPoints.DefaultDataPoint.DataLabels as ChartDataLabelsImpl;
          if (chart.CommonDataPointsCollection.ContainsKey(chartSerieImpl.ChartGroup))
          {
            ChartDataLabelsImpl dataLabels2 = chart.CommonDataPointsCollection[chartSerieImpl.ChartGroup].DefaultDataPoint.DataLabels as ChartDataLabelsImpl;
            dataLabels1.IsCategoryName = dataLabels2.IsCategoryName;
            dataLabels1.IsPercentage = dataLabels2.IsPercentage;
            dataLabels1.IsLegendKey = dataLabels2.IsLegendKey;
            dataLabels1.IsValue = dataLabels2.IsValue;
            dataLabels1.IsSeriesName = dataLabels2.IsSeriesName;
            dataLabels1.ShowTextProperties = false;
          }
        }
      }
    }
    reader.Read();
    foreach (ChartFormatImpl primaryFormat in (CollectionBase<ChartFormatImpl>) chart.PrimaryFormats)
    {
      if (primaryFormat.SerieFormat.TypeCode == TBIFFRecord.ChartRadar || primaryFormat.SerieFormat.TypeCode == TBIFFRecord.ChartRadarArea)
        primaryFormat.HasRadarAxisLabels = chart.PrimaryCategoryAxis.TickLabelPosition != ExcelTickLabelPosition.TickLabelPosition_None;
    }
    if (chart.IsSecondaryCategoryAxisAvail)
    {
      foreach (ChartFormatImpl secondaryFormat in (CollectionBase<ChartFormatImpl>) chart.SecondaryFormats)
      {
        if (secondaryFormat.SerieFormat.TypeCode == TBIFFRecord.ChartRadar || secondaryFormat.SerieFormat.TypeCode == TBIFFRecord.ChartRadarArea)
          secondaryFormat.HasRadarAxisLabels = chart.SecondaryCategoryAxis.TickLabelPosition != ExcelTickLabelPosition.TickLabelPosition_None;
      }
    }
    ChartCategoryAxisImpl primaryCategoryAxis = chart.PrimaryCategoryAxis as ChartCategoryAxisImpl;
    if (primaryCategoryAxis.IsChartBubbleOrScatter)
      primaryCategoryAxis.SwapAxisValues();
    if (!chart.IsSecondaryCategoryAxisAvail)
      return;
    ChartCategoryAxisImpl secondaryCategoryAxis = chart.SecondaryCategoryAxis as ChartCategoryAxisImpl;
    if (!secondaryCategoryAxis.IsChartBubbleOrScatter)
      return;
    secondaryCategoryAxis.SwapAxisValues();
  }

  private void ParseStyleIdFromAlternateContent(Stream stream, ChartImpl chart)
  {
    XmlReader reader = UtilityMethods.CreateReader(stream);
    if (!(reader.LocalName == "AlternateContent"))
      return;
    reader.Read();
    Excel2007Parser.SkipWhiteSpaces(reader);
    while (reader.NodeType != XmlNodeType.None)
    {
      if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "style")
      {
        chart.Style = ChartParserCommon.ParseIntValueTag(reader);
        break;
      }
      if (reader.NodeType == XmlNodeType.EndElement && (reader.LocalName == "Choice" || reader.LocalName == "AlternateContent"))
        break;
      reader.Read();
    }
  }

  internal void ParseDefaultTextProperties(XmlReader reader, ChartImpl chart)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "txPr")
      throw new XmlException("Unexpected tag name");
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    chart.DefaultTextProperty = ShapeParser.ReadNodeAsStream(reader);
    chart.DefaultTextProperty.Position = 0L;
    XmlReader reader1 = UtilityMethods.CreateReader(chart.DefaultTextProperty);
    if (reader1.LocalName != "txPr")
      throw new XmlException();
    if (!reader1.IsEmptyElement)
    {
      reader1.Read();
      while (reader1.NodeType != XmlNodeType.EndElement)
      {
        if (reader1.NodeType == XmlNodeType.Element)
        {
          switch (reader1.LocalName)
          {
            case "bodyPr":
              this.ParseChartBodyProperties(reader1, chart);
              continue;
            case "p":
              this.ParserChartParagraphs(reader1, chart);
              continue;
            default:
              reader1.Skip();
              continue;
          }
        }
        else
          reader1.Skip();
      }
    }
    reader1.Read();
  }

  private void ParseChartBodyProperties(XmlReader reader, ChartImpl chart)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "bodyPr")
      throw new XmlException();
    reader.MoveToElement();
    reader.Skip();
  }

  private void ParserChartParagraphs(XmlReader reader, ChartImpl chart)
  {
    FileDataHolder parentHolder = chart.DataHolder.ParentHolder;
    while (!(reader.LocalName == "p") || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "defRPr")
      {
        TextSettings paragraphProperties = ChartParserCommon.ParseDefaultParagraphProperties(reader, parentHolder.Parser);
        ChartParserCommon.CopyDefaultSettings(chart.Font as IInternalFont, paragraphProperties);
        this.CheckDefaultTextSettings(chart);
        if (chart.HasChartTitle)
          ChartParserCommon.CopyChartTitleDefaultSettings(chart.ChartTitleArea as ChartTextAreaImpl, paragraphProperties);
        if (chart.PrimaryValueAxis != null && chart.PrimaryValueAxis.Title != null)
          ChartParserCommon.CopyChartTitleDefaultSettings(chart.PrimaryValueAxis.TitleArea as ChartTextAreaImpl, paragraphProperties);
        if (chart.PrimaryCategoryAxis != null && chart.PrimaryCategoryAxis.Title != null)
          ChartParserCommon.CopyChartTitleDefaultSettings(chart.PrimaryCategoryAxis.TitleArea as ChartTextAreaImpl, paragraphProperties);
      }
      else
        reader.Read();
    }
  }

  private void CheckDefaultTextSettings(ChartImpl chart)
  {
    foreach (ChartAxisImpl chartAxisImpl in new List<ChartAxisImpl>()
    {
      (ChartAxisImpl) chart.PrimaryCategoryAxis,
      (ChartAxisImpl) chart.PrimaryValueAxis,
      (ChartAxisImpl) chart.SecondaryValueAxis,
      (ChartAxisImpl) chart.SecondaryCategoryAxis
    })
    {
      if (chartAxisImpl != null && chartAxisImpl.IsDefaultTextSettings)
      {
        chartAxisImpl.IsChartFont = true;
        chartAxisImpl.Font = (IFont) ((CommonWrapper) chart.Font).Clone((object) chart);
        chartAxisImpl.IsChartFont = false;
      }
    }
  }

  private void ParseExtensionList(XmlReader reader, ChartImpl chart)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.IsEmptyElement)
    {
      reader.Read();
    }
    else
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "ext":
              this.ParseExtension(reader, chart);
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          Excel2007Parser.SkipWhiteSpaces(reader);
      }
      reader.Read();
    }
  }

  private void ParseExtension(XmlReader reader, ChartImpl chart)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "pivotSource":
            this.ParsePivotSource(reader, chart);
            continue;
          case "pivotOptions":
            this.ParsePivotOptions(reader, chart);
            continue;
          case "pivotOptions16":
            this.ParsePivotOptions16(reader, chart);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        Excel2007Parser.SkipWhiteSpaces(reader);
    }
    reader.Read();
  }

  private void ParsePivotOptions(XmlReader reader, ChartImpl chart)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "pivotOptions")
      throw new XmlException("Unexpected tag name");
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "dropZoneCategories":
            if (reader.MoveToAttribute("val"))
            {
              chart.ShowAxisFieldButtons = true;
              continue;
            }
            continue;
          case "dropZoneData":
            if (reader.MoveToAttribute("val"))
            {
              chart.ShowValueFieldButtons = true;
              continue;
            }
            continue;
          case "dropZoneFilter":
            if (reader.MoveToAttribute("val"))
            {
              chart.ShowReportFilterFieldButtons = true;
              continue;
            }
            continue;
          case "dropZoneSeries":
            if (reader.MoveToAttribute("val"))
            {
              chart.ShowLegendFieldButtons = true;
              continue;
            }
            continue;
          case "dropZonesVisible":
            if (reader.MoveToAttribute("val"))
            {
              chart.ShowAllFieldButtons = true;
              continue;
            }
            continue;
          default:
            reader.Read();
            continue;
        }
      }
      else
        reader.Read();
    }
    if (!(reader.LocalName == "pivotOptions"))
      return;
    reader.Read();
  }

  private void ParsePivotOptions16(XmlReader reader, ChartImpl chart)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "pivotOptions16")
      throw new XmlException("Unexpected tag name");
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "showExpandCollapseFieldButtons":
            if (reader.MoveToAttribute("val"))
            {
              chart.ShowExpandCollapseFieldButtons = true;
              continue;
            }
            continue;
          default:
            reader.Read();
            continue;
        }
      }
      else
        reader.Read();
    }
    if (!(reader.LocalName == "pivotOptions16"))
      return;
    reader.Read();
  }

  internal void ParsePrintSettings(XmlReader reader, ChartImpl chart, RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "printSettings")
      throw new XmlException();
    ChartPageSetupConstants constants = new ChartPageSetupConstants(this is ChartExParser);
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      IPageSetupBase pageSetup = (IPageSetupBase) chart.PageSetup;
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "printOptions":
              Excel2007Parser.ParsePrintOptions(reader, pageSetup);
              continue;
            case "pageMargins":
              Excel2007Parser.ParsePageMargins(reader, pageSetup, (IPageSetupConstantsProvider) constants);
              continue;
            case "pageSetup":
              Excel2007Parser.ParsePageSetup(reader, (PageSetupBaseImpl) pageSetup);
              continue;
            case "headerFooter":
              Excel2007Parser.ParseHeaderFooter(reader, (PageSetupBaseImpl) pageSetup);
              continue;
            case "legacyDrawingHF":
              Excel2007Parser.ParseLegacyDrawingHF(reader, (WorksheetBaseImpl) chart, relations);
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

  private void ParsePivotSource(XmlReader reader, ChartImpl chart)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "pivotSource")
      throw new XmlException("Unexpected xml tag");
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "name":
            string pivotSourceName = reader.ReadElementContentAsString();
            chart.PivotSource = this.GetPivotTable(chart.Workbook, pivotSourceName);
            chart.PreservedPivotSource = pivotSourceName;
            if (pivotSourceName != null)
            {
              chart.ShowAllFieldButtons = false;
              chart.ShowAxisFieldButtons = false;
              chart.ShowLegendFieldButtons = false;
              chart.ShowReportFilterFieldButtons = false;
              chart.ShowValueFieldButtons = false;
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
  }

  private IPivotTable GetPivotTable(IWorkbook book, string pivotSourceName)
  {
    int length = pivotSourceName.LastIndexOf('!');
    if (length < 0)
      return (IPivotTable) null;
    string name = pivotSourceName.Substring(length + 1);
    pivotSourceName = pivotSourceName.Substring(0, length);
    int num = pivotSourceName.IndexOf(']');
    if (num < 0)
      return (IPivotTable) null;
    string sheetName1 = pivotSourceName.Substring(num + 1);
    IWorksheet worksheet = book.Worksheets[sheetName1];
    IPivotTable pivotTable = (IPivotTable) null;
    if (worksheet != null)
      pivotTable = worksheet.PivotTables[name];
    else if (sheetName1.Contains(string.Empty))
    {
      string sheetName2 = sheetName1.Replace(" ", "_");
      if (book.Worksheets[sheetName2] != null)
        pivotTable = book.Worksheets[sheetName2].PivotTables[name];
    }
    return pivotTable;
  }

  private void ParseUserShapes(XmlReader reader, ChartImpl chart, RelationCollection relations)
  {
    reader.MoveToAttribute("id", (chart.Workbook as WorkbookImpl).IsStrict ? "http://purl.oclc.org/ooxml/officeDocument/relationships" : "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
    string id = reader.Value;
    Dictionary<string, object> dictItemsToRemove = new Dictionary<string, object>();
    Relation relation = relations[id];
    chart.DataHolder.ParseDrawings((WorksheetBaseImpl) chart, relation, dictItemsToRemove, true);
  }

  private void ParseChartElement(XmlReader reader, ChartImpl chart, RelationCollection relations)
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
    bool? nullable = new bool?();
    MemoryStream memoryStream = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((Stream) memoryStream, Encoding.UTF8);
    writer.WriteStartElement("root");
    Chart3DRecord chart3D = (Chart3DRecord) null;
    chart.PlotVisibleOnly = false;
    if (chart.m_themeOverrideStream != null)
    {
      chart.ThemeColors = chart.ParentWorkbook.DataHolder.Parser.ParseThemeOverideColors(chart);
      parentHolder.Parser.CurrentChartThemeColors = chart.ThemeColors;
    }
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "view3D":
            if (!reader.IsEmptyElement)
            {
              chart3D = this.ParseView3D(reader, chart);
              continue;
            }
            reader.Skip();
            continue;
          case "plotArea":
            this.ParsePlotArea(reader, chart, relations, parentHolder.Parser);
            continue;
          case "legend":
            chart.HasLegend = true;
            this.ParseLegend(reader, chart.Legend as ChartLegendImpl, chart, relations);
            continue;
          case "floor":
            this.ParseSurface(reader, chart.Floor, parentHolder, relations);
            continue;
          case "sideWall":
            this.ParseSurface(reader, chart.SideWall, parentHolder, relations);
            continue;
          case "backWall":
            this.ParseSurface(reader, chart.Walls, parentHolder, relations);
            continue;
          case "dispBlanksAs":
            if (reader.MoveToAttribute("val"))
            {
              chart.DisplayBlanksAs = (ExcelChartPlotEmpty) Enum.Parse(typeof (Excel2007ChartPlotEmpty), reader.Value.ToString().ToLower());
              continue;
            }
            chart.DisplayBlanksAs = ExcelChartPlotEmpty.Zero;
            reader.Skip();
            continue;
          case "title":
            writer.WriteNode(reader, false);
            writer.Flush();
            flag = true;
            continue;
          case "autoTitleDeleted":
            nullable = new bool?(this.ParseAutoTitleDeleted(reader, chart));
            continue;
          case "pivotFmts":
            this.ParsePivotFormats(reader, chart);
            continue;
          case "plotVisOnly":
            chart.PlotVisibleOnly = ChartParserCommon.ParseBoolValueTag(reader);
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
      ChartTextAreaImpl chartTitleArea = chart.ChartTitleArea as ChartTextAreaImpl;
      ChartParserCommon.ParseChartTitleElement((Stream) memoryStream, (IInternalChartTextArea) chartTitleArea, parentHolder, relations, 18f);
      chartTitleArea.IsTitleElement = true;
      if (chartTitleArea.Text == null)
        chartTitleArea.TextRecord.IsAutoText = true;
    }
    if (nullable.HasValue)
    {
      chart.HasAutoTitle = nullable;
      if (flag && !chart.HasTitle)
        (chart.ChartTitleArea as ChartTextAreaImpl).TextRecord.IsDeleted = nullable.Value;
    }
    reader.Read();
    this.Set3DSettings(chart, chart3D);
    if (parentHolder.Parser.CurrentChartThemeColors == null)
      return;
    parentHolder.Parser.CurrentChartThemeColors = (Dictionary<string, Color>) null;
  }

  internal void TryParsePositioningValues(
    XmlReader reader,
    out bool? isOverlay,
    out ushort position)
  {
    isOverlay = new bool?();
    position = (ushort) 0;
    if (reader.MoveToAttribute("align"))
    {
      ChartExPositionAlignment positionAlignment = (ChartExPositionAlignment) Enum.Parse(typeof (ChartExPositionAlignment), reader.Value, false);
      position |= (ushort) positionAlignment;
    }
    if (reader.MoveToAttribute("pos"))
    {
      ChartExSidePosition chartExSidePosition = (ChartExSidePosition) Enum.Parse(typeof (ChartExSidePosition), reader.Value, false);
      position |= (ushort) chartExSidePosition;
    }
    if (reader.MoveToAttribute("overlay"))
      isOverlay = new bool?(XmlConvertExtension.ToBoolean(reader.Value));
    reader.MoveToElement();
  }

  private bool ParseAutoTitleDeleted(XmlReader reader, ChartImpl chart)
  {
    bool autoTitleDeleted = false;
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.MoveToAttribute("val"))
      autoTitleDeleted = XmlConvertExtension.ToBoolean(reader.Value);
    reader.Read();
    return autoTitleDeleted;
  }

  private void ParsePivotFormats(XmlReader reader, ChartImpl chart)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    chart.PivotFormatsStream = ShapeParser.ReadNodeAsStream(reader);
  }

  private void Set3DSettings(ChartImpl chart, Chart3DRecord chart3D)
  {
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (chart3D == (Chart3DRecord) null || chart.Series.Count == 0)
      return;
    if (!chart3D.IsDefaultElevation)
      chart.Elevation = (int) chart3D.ElevationAngle;
    chart.HeightPercent = (int) chart3D.Height;
    chart.AutoScaling = chart3D.IsAutoScaled;
    if (!chart3D.IsDefaultRotation && chart.IsChartPie && chart.IsChart3D)
      chart.ChartFormat.FirstSliceAngle = (int) chart3D.RotationAngle;
    else if (!chart3D.IsDefaultRotation)
      chart.Rotation = (int) chart3D.RotationAngle;
    chart.DepthPercent = (int) chart3D.Depth;
    chart.Perspective = (int) chart3D.DistanceFromEye;
    chart.RightAngleAxes = chart3D.IsPerspective;
  }

  internal void ParseLegend(
    XmlReader reader,
    ChartLegendImpl legend,
    ChartImpl chart,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (legend == null)
      throw new ArgumentNullException(nameof (legend));
    bool flag = !(reader.LocalName != nameof (legend)) ? reader.IsEmptyElement : throw new XmlException("Unexpected xml tag.");
    if (this is ChartExParser)
    {
      bool? isOverlay = new bool?();
      ushort position = 0;
      this.TryParsePositioningValues(reader, out isOverlay, out position);
      if (isOverlay.HasValue)
        legend.IncludeInLayout = isOverlay.Value;
      if (position != (ushort) 0)
        legend.ChartExPosition = position;
      legend.Position = this.GetChartLegendPosition(position);
    }
    Excel2007Parser parser = chart.ParentWorkbook.DataHolder.Parser;
    legend.IsChartTextArea = true;
    legend.TextArea.FontName = "Calibri";
    legend.TextArea.Size = 10.0;
    legend.IsChartTextArea = false;
    if (!flag)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "legendPos":
              string valueTag = ChartParserCommon.ParseValueTag(reader);
              legend.Position = (ExcelLegendPosition) Enum.Parse(typeof (Excel2007LegendPosition), valueTag, false);
              continue;
            case "legendEntry":
              this.ParseLegendEntry(reader, (IChartLegend) legend, parser);
              continue;
            case "txPr":
              legend.IsChartTextArea = true;
              IInternalChartTextArea textArea = legend.TextArea as IInternalChartTextArea;
              this.ParseDefaultTextFormatting(reader, textArea, parser);
              legend.IsChartTextArea = false;
              continue;
            case "spPr":
              IChartFrameFormat frameFormat = legend.FrameFormat;
              ChartFillObjectGetterAny objectGetter = new ChartFillObjectGetterAny(frameFormat.Border as ChartBorderImpl, frameFormat.Interior as ChartInteriorImpl, frameFormat.Fill as IInternalFill, frameFormat.Shadow as ShadowImpl, frameFormat.ThreeD as ThreeDFormatImpl);
              FileDataHolder dataHolder = chart.ParentWorkbook.DataHolder;
              ChartParserCommon.ParseShapeProperties(reader, (IChartFillObjectGetter) objectGetter, dataHolder, relations);
              continue;
            case "layout":
              legend.Layout = (IChartLayout) new ChartLayoutImpl(this.m_book.Application, (object) legend, (object) chart);
              ChartParserCommon.ParseChartLayout(reader, legend.Layout);
              continue;
            case "overlay":
              legend.IncludeInLayout = !ChartParserCommon.ParseBoolValueTag(reader);
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

  private ExcelLegendPosition GetChartLegendPosition(ushort position)
  {
    ExcelLegendPosition chartLegendPosition = ExcelLegendPosition.NotDocked;
    ushort num1 = 240 /*0xF0*/;
    ChartExPositionAlignment positionAlignment = (ChartExPositionAlignment) ((int) position & (int) num1);
    ushort num2 = 15;
    ChartExSidePosition chartExSidePosition = (ChartExSidePosition) ((int) position & (int) num2);
    switch (positionAlignment)
    {
      case ChartExPositionAlignment.min:
        if (chartExSidePosition == ChartExSidePosition.r)
        {
          chartLegendPosition = ExcelLegendPosition.Corner;
          break;
        }
        break;
      case ChartExPositionAlignment.ctr:
        switch (chartExSidePosition)
        {
          case ChartExSidePosition.l:
            chartLegendPosition = ExcelLegendPosition.Left;
            break;
          case ChartExSidePosition.t:
            chartLegendPosition = ExcelLegendPosition.Top;
            break;
          case ChartExSidePosition.r:
            chartLegendPosition = ExcelLegendPosition.Right;
            break;
          case ChartExSidePosition.b:
            chartLegendPosition = ExcelLegendPosition.Bottom;
            break;
        }
        break;
    }
    return chartLegendPosition;
  }

  private void ParseLegendEntry(XmlReader reader, IChartLegend legend, Excel2007Parser parser)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (legend == null)
      throw new ArgumentNullException(nameof (legend));
    if (reader.LocalName != "legendEntry")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    int iIndex = 0;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "idx":
            iIndex = int.Parse(ChartParserCommon.ParseValueTag(reader));
            continue;
          case "delete":
            bool boolValueTag = ChartParserCommon.ParseBoolValueTag(reader);
            legend.LegendEntries[iIndex].IsDeleted = boolValueTag;
            continue;
          case "txPr":
            IInternalChartTextArea textArea = legend.LegendEntries[iIndex].TextArea as IInternalChartTextArea;
            this.ParseDefaultTextFormatting(reader, textArea, parser);
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

  private Chart3DRecord ParseView3D(XmlReader reader, ChartImpl chart)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "view3D")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    Chart3DRecord record = (Chart3DRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Chart3D);
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "rotX":
            string valueTag1 = ChartParserCommon.ParseValueTag(reader);
            record.ElevationAngle = short.Parse(valueTag1);
            continue;
          case "hPercent":
            record.IsAutoScaled = false;
            string valueTag2 = ChartParserCommon.ParseValueTag(reader);
            record.Height = ushort.Parse(valueTag2);
            continue;
          case "rotY":
            if (reader.MoveToAttribute("val"))
            {
              string valueTag3 = ChartParserCommon.ParseValueTag(reader);
              record.RotationAngle = ushort.Parse(valueTag3);
              continue;
            }
            reader.Skip();
            continue;
          case "depthPercent":
            string valueTag4 = ChartParserCommon.ParseValueTag(reader);
            record.Depth = ushort.Parse(valueTag4);
            continue;
          case "rAngAx":
            string valueTag5 = ChartParserCommon.ParseValueTag(reader);
            record.IsPerspective = XmlConvertExtension.ToBoolean(valueTag5);
            continue;
          case "perspective":
            string valueTag6 = ChartParserCommon.ParseValueTag(reader);
            record.DistanceFromEye = (ushort) (int.Parse(valueTag6) / 2);
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
    return record;
  }

  private void ParseErrorBars(
    XmlReader reader,
    ChartSerieImpl series,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    if (reader.LocalName != "errBars")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    IChartErrorBars errorBars = (IChartErrorBars) null;
    WorkbookImpl parentBook = series.ParentBook;
    FileDataHolder dataHolder = parentBook.DataHolder;
    object[] values = (object[]) null;
    ChartErrorBarsImpl chartErrorBarsImpl = (ChartErrorBarsImpl) null;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "errDir":
            if (ChartParserCommon.ParseValueTag(reader) == "x")
            {
              series.HasErrorBarsX = true;
              errorBars = series.ErrorBarsX;
            }
            else
            {
              series.HasErrorBarsY = true;
              errorBars = series.ErrorBarsY;
            }
            chartErrorBarsImpl = errorBars as ChartErrorBarsImpl;
            continue;
          case "errBarType":
            string valueTag = ChartParserCommon.ParseValueTag(reader);
            if (errorBars == null)
            {
              series.HasErrorBarsY = true;
              errorBars = series.ErrorBarsY;
            }
            errorBars.Include = (ExcelErrorBarInclude) Enum.Parse(typeof (ExcelErrorBarInclude), valueTag, true);
            continue;
          case "errValType":
            Excel2007ErrorBarType excel2007ErrorBarType = (Excel2007ErrorBarType) Enum.Parse(typeof (Excel2007ErrorBarType), ChartParserCommon.ParseValueTag(reader), false);
            errorBars.Type = (ExcelErrorBarType) excel2007ErrorBarType;
            continue;
          case "noEndCap":
            errorBars.HasCap = !ChartParserCommon.ParseBoolValueTag(reader);
            continue;
          case "plus":
            IRange errorBarRange1 = this.ParseErrorBarRange(reader, (IWorkbook) parentBook, out values, errorBars);
            if (!((ChartErrorBarsImpl) errorBars).IsPlusNumberLiteral && errorBars.Include != ExcelErrorBarInclude.Minus)
              errorBars.PlusRange = errorBarRange1;
            if (chartErrorBarsImpl == null & errorBars != null)
              chartErrorBarsImpl = errorBars as ChartErrorBarsImpl;
            chartErrorBarsImpl.PlusRangeValues = values;
            continue;
          case "minus":
            IRange errorBarRange2 = this.ParseErrorBarRange(reader, (IWorkbook) parentBook, out values, errorBars);
            if (!((ChartErrorBarsImpl) errorBars).IsMinusNumberLiteral && errorBars.Include != ExcelErrorBarInclude.Plus)
              errorBars.MinusRange = errorBarRange2;
            if (chartErrorBarsImpl == null & errorBars != null)
              chartErrorBarsImpl = errorBars as ChartErrorBarsImpl;
            chartErrorBarsImpl.MinusRangeValues = values;
            continue;
          case "val":
            errorBars.NumberValue = ChartParserCommon.ParseDoubleValueTag(reader);
            continue;
          case "spPr":
            ChartFillObjectGetterAny objectGetter = new ChartFillObjectGetterAny(errorBars.Border as ChartBorderImpl, (ChartInteriorImpl) null, (IInternalFill) null, errorBars.Shadow as ShadowImpl, errorBars.Chart3DOptions as ThreeDFormatImpl);
            ChartParserCommon.ParseShapeProperties(reader, (IChartFillObjectGetter) objectGetter, dataHolder, relations);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    this.CheckCustomErrorBarType(errorBars);
    reader.Read();
  }

  private void CheckCustomErrorBarType(IChartErrorBars errorBars)
  {
    if (errorBars.Type != ExcelErrorBarType.Custom || ((ChartErrorBarsImpl) errorBars).IsPlusNumberLiteral && ((ChartErrorBarsImpl) errorBars).IsMinusNumberLiteral || errorBars.MinusRange == null || errorBars.PlusRange == null)
      return;
    errorBars.Include = ExcelErrorBarInclude.Both;
  }

  private IRange ParseErrorBarRange(
    XmlReader reader,
    IWorkbook book,
    out object[] values,
    IChartErrorBars errorBars)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    bool flag = false;
    if (reader.LocalName == "plus")
      flag = true;
    reader.Read();
    string formula = (string) null;
    values = (object[]) null;
    string formatCode = (string) null;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "numRef")
        formula = this.ParseNumReference(reader, out values);
      else if (reader.LocalName == "numLit")
      {
        if (flag)
          ((ChartErrorBarsImpl) errorBars).IsPlusNumberLiteral = true;
        else
          ((ChartErrorBarsImpl) errorBars).IsMinusNumberLiteral = true;
        values = this.ParseDirectlyEnteredValues(reader, out formatCode);
      }
    }
    reader.Read();
    return formula != null ? ChartParser.GetRange(book as WorkbookImpl, formula) : (IRange) null;
  }

  private void ParseTrendlines(
    XmlReader reader,
    ChartSerieImpl series,
    RelationCollection relations,
    Excel2007Parser parser)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    if (parser == null)
      throw new ArgumentException(nameof (parser));
label_9:
    while (reader.LocalName == "trendline")
    {
      this.ParseTrendline(reader, series, relations, parser);
      while (true)
      {
        if (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.Element)
          reader.Read();
        else
          goto label_9;
      }
    }
  }

  private void ParseTrendline(
    XmlReader reader,
    ChartSerieImpl series,
    RelationCollection relations,
    Excel2007Parser parser)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    if (parser == null)
      throw new ArgumentException(nameof (parser));
    if (reader.LocalName != "trendline")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    IChartTrendLine trendline = series.TrendLines.Add();
    FileDataHolder dataHolder = series.ParentBook.DataHolder;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "name":
            trendline.Name = reader.ReadElementContentAsString();
            continue;
          case "spPr":
            ChartFillObjectGetterAny objectGetter = new ChartFillObjectGetterAny(trendline.Border as ChartBorderImpl, (ChartInteriorImpl) null, (IInternalFill) null, trendline.Shadow as ShadowImpl, trendline.Chart3DOptions as ThreeDFormatImpl);
            ChartParserCommon.ParseShapeProperties(reader, (IChartFillObjectGetter) objectGetter, dataHolder, relations);
            continue;
          case "trendlineType":
            Excel2007TrendlineType excel2007TrendlineType = (Excel2007TrendlineType) Enum.Parse(typeof (Excel2007TrendlineType), ChartParserCommon.ParseValueTag(reader), false);
            trendline.Type = (ExcelTrendLineType) excel2007TrendlineType;
            continue;
          case "order":
          case "period":
            trendline.Order = ChartParserCommon.ParseIntValueTag(reader);
            continue;
          case "forward":
            trendline.Forward = ChartParserCommon.ParseDoubleValueTag(reader);
            continue;
          case "backward":
            trendline.Backward = ChartParserCommon.ParseDoubleValueTag(reader);
            continue;
          case "intercept":
            trendline.Intercept = ChartParserCommon.ParseDoubleValueTag(reader);
            continue;
          case "dispRSqr":
            trendline.DisplayRSquared = ChartParserCommon.ParseBoolValueTag(reader);
            continue;
          case "dispEq":
            trendline.DisplayEquation = ChartParserCommon.ParseBoolValueTag(reader);
            continue;
          case "trendlineLbl":
            this.ParseTrendlineLabel(reader, trendline, parser, dataHolder, relations);
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

  private void ParseTrendlineLabel(
    XmlReader reader,
    IChartTrendLine trendline,
    Excel2007Parser parser,
    FileDataHolder dataHolder,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (trendline == null)
      throw new ArgumentNullException(nameof (trendline));
    if (reader.LocalName != "trendlineLbl")
      throw new XmlException("Unexpected xml tag.");
    if (parser == null)
      throw new ArgumentException(nameof (parser));
    IChartTextArea textArea = trendline is ChartTrendLineImpl ? (trendline as ChartTrendLineImpl).TrendLineTextArea : throw new ArgumentException("trendLine");
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "tx":
            textArea.Text = string.Empty;
            ChartParserCommon.ParseTextAreaText(reader, textArea as IInternalChartTextArea, parser, new float?(10f));
            continue;
          case "layout":
            textArea.Layout = (IChartLayout) new ChartLayoutImpl(this.m_book.Application, (object) trendline.DataLabel, trendline.DataLabel.Parent);
            ChartParserCommon.ParseChartLayout(reader, textArea.Layout);
            continue;
          case "spPr":
            IChartFrameFormat frameFormat = textArea.FrameFormat;
            IChartFillObjectGetter objectGetter = (IChartFillObjectGetter) new ChartFillObjectGetterAny(frameFormat.Border as ChartBorderImpl, frameFormat.Interior as ChartInteriorImpl, frameFormat.Fill as IInternalFill, frameFormat.Shadow as ShadowImpl, frameFormat.ThreeD as ThreeDFormatImpl);
            ChartParserCommon.ParseShapeProperties(reader, objectGetter, dataHolder, relations);
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

  private void ParseSurface(
    XmlReader reader,
    IChartWallOrFloor surface,
    FileDataHolder dataHolder,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (surface == null)
      throw new ArgumentNullException(nameof (surface));
    ((ChartWallOrFloorImpl) surface).HasShapeProperties = false;
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "spPr":
              ((ChartWallOrFloorImpl) surface).HasShapeProperties = true;
              IChartFillObjectGetter objectGetter = (IChartFillObjectGetter) new ChartFillObjectGetterAny(surface.LineProperties as ChartBorderImpl, surface.Interior as ChartInteriorImpl, surface.Fill as IInternalFill, surface.Shadow as ShadowImpl, surface.ThreeD as ThreeDFormatImpl);
              ChartParserCommon.ParseShapeProperties(reader, objectGetter, dataHolder, relations);
              continue;
            case "thickness":
              string valueTag1 = ChartParserCommon.ParseValueTag(reader);
              ((ChartWallOrFloorImpl) surface).Thickness = (uint) int.Parse(valueTag1);
              continue;
            case "pictureOptions":
              reader.Read();
              int num = reader.LocalName == "pictureFormat" ? 1 : 0;
              string valueTag2 = ChartParserCommon.ParseValueTag(reader);
              if (valueTag2 == ExcelChartPictureType.stack.ToString())
                ((ChartWallOrFloorImpl) surface).PictureUnit = ExcelChartPictureType.stack;
              else if (valueTag2 == ExcelChartPictureType.stackScale.ToString())
              {
                ((ChartWallOrFloorImpl) surface).PictureUnit = ExcelChartPictureType.stackScale;
                reader.Read();
                if (reader.LocalName == "pictureStackUnit")
                {
                  string valueTag3 = ChartParserCommon.ParseValueTag(reader);
                  ((ChartWallOrFloorImpl) surface).PictureStackUnit = Convert.ToDouble(valueTag3);
                }
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

  private void ParsePlotArea(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    Excel2007Parser parser)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    Stream data = !(reader.LocalName != "plotArea") ? ShapeParser.ReadNodeAsStream(reader) : throw new XmlException("Unexpected xml tag");
    data.Position = 0L;
    reader = UtilityMethods.CreateReader(data);
    reader.Read();
    this.ParsePlotAreaAxes(reader, chart, relations, parser);
    data.Position = 0L;
    reader = UtilityMethods.CreateReader(data);
    reader.Read();
    this.ParsePlotAreaGeneral(reader, chart, relations, parser);
  }

  private void ParsePlotAreaGeneral(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    Excel2007Parser parser)
  {
    IChartFrameFormat plotArea1 = chart.PlotArea;
    FileDataHolder parentHolder = chart.DataHolder.ParentHolder;
    Dictionary<int, int> dictSeriesAxis = new Dictionary<int, int>();
    bool flag = chart.PlotArea != null && chart.ChartArea.IsBorderCornersRound;
    chart.HasPlotArea = false;
    ExcelChartType chartType = ExcelChartType.Column_Clustered;
    List<int> intList = (List<int>) null;
    while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "layout":
            if (chart.PlotArea == null)
              chart.PlotArea = (IChartFrameFormat) new ChartPlotAreaImpl(this.m_book.Application, (object) chart);
            chart.PlotArea.Layout = (IChartLayout) new ChartLayoutImpl(this.m_book.Application, (object) chart.PlotArea, (object) chart);
            ChartParserCommon.ParseChartLayout(reader, chart.PlotArea.Layout);
            continue;
          case "dTable":
            this.ParseDataTable(reader, chart);
            continue;
          case "spPr":
            chart.HasPlotArea = true;
            IChartFrameFormat plotArea2 = chart.PlotArea;
            chart.ChartArea.IsBorderCornersRound = flag;
            ChartFillObjectGetterAny objectGetter = new ChartFillObjectGetterAny(plotArea2.Border as ChartBorderImpl, plotArea2.Interior as ChartInteriorImpl, plotArea2.Fill as IInternalFill, plotArea2.Shadow as ShadowImpl, plotArea2.ThreeD as ThreeDFormatImpl);
            ChartParserCommon.ParseShapeProperties(reader, (IChartFillObjectGetter) objectGetter, parentHolder, relations);
            continue;
          case "barChart":
            this.ParseBarChart(reader, chart, relations, dictSeriesAxis, out chartType);
            continue;
          case "bar3DChart":
            this.ParseBar3DChart(reader, chart, relations, dictSeriesAxis, out chartType);
            continue;
          case "areaChart":
            this.ParseAreaChart(reader, chart, relations, dictSeriesAxis, out chartType);
            continue;
          case "area3DChart":
            this.ParseArea3DChart(reader, chart, relations, dictSeriesAxis, out chartType);
            continue;
          case "lineChart":
            if (intList == null)
              intList = new List<int>();
            this.ParseLineChart(reader, chart, relations, dictSeriesAxis, parser, out chartType, intList);
            continue;
          case "line3DChart":
            this.ParseLine3DChart(reader, chart, relations, dictSeriesAxis, parser, out chartType);
            continue;
          case "bubbleChart":
          case "bubble3D":
            this.ParseBubbleChart(reader, chart, relations, dictSeriesAxis, out chartType);
            continue;
          case "surfaceChart":
            this.ParseSurfaceChart(reader, chart, relations, dictSeriesAxis, out chartType);
            continue;
          case "surface3DChart":
            this.ParseSurfaceChart(reader, chart, relations, dictSeriesAxis, out chartType);
            continue;
          case "radarChart":
            this.ParseRadarChart(reader, chart, relations, dictSeriesAxis, parser, out chartType);
            continue;
          case "scatterChart":
            this.ParseScatterChart(reader, chart, relations, dictSeriesAxis, parser, out chartType);
            continue;
          case "pieChart":
            this.ParsePieChart(reader, chart, relations, dictSeriesAxis, out chartType);
            continue;
          case "pie3DChart":
            this.ParsePie3DChart(reader, chart, relations, dictSeriesAxis, out chartType);
            continue;
          case "doughnutChart":
            this.ParseDoughnutChart(reader, chart, relations, dictSeriesAxis, out chartType);
            continue;
          case "ofPieChart":
            this.ParseOfPieChart(reader, chart, relations, dictSeriesAxis, out chartType);
            continue;
          case "stockChart":
            this.ParseStockChart(reader, chart, relations, dictSeriesAxis, parser);
            chart.IsStock = true;
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    ChartSeriesCollection series1 = (ChartSeriesCollection) chart.Series;
    ChartSeriesCollection source = (chart.Series as ChartSeriesCollection).Clone(chart.Series.Parent) as ChartSeriesCollection;
    chart.ChartSerieGroupsBeforesorting = source.Where<IChartSerie>((Func<IChartSerie, bool>) (x => !x.IsFiltered)).GroupBy<IChartSerie, int, IChartSerie>((Func<IChartSerie, int>) (x => (x as ChartSerieImpl).ChartGroup), (Func<IChartSerie, IChartSerie>) (x => x));
    series1.ResortSeries(dictSeriesAxis, intList);
    if (chart.Series.Count > 0 && chart.Series[0].SerieType == ExcelChartType.Bubble)
      chart.CheckIsBubble3D();
    if (chart.CommonDataPointsCollection != null && chart.CommonDataPointsCollection.Count > 0)
      this.ChangeKeyToChartGroup(chart);
    if (chart.Series != null && chart.Series.Count > 0)
    {
      for (int index = 0; index < chart.Series.Count; ++index)
      {
        ChartSerieImpl series2 = chart.Series[index] as ChartSerieImpl;
        if (series2.DropLinesStream != null)
        {
          XmlReader reader1 = UtilityMethods.CreateReader(series2.DropLinesStream);
          this.ParseLines(reader1, chart, series2, reader1.LocalName);
          reader1.Close();
          series2.DropLinesStream.Dispose();
          series2.DropLinesStream = (Stream) null;
        }
      }
    }
    if (chart.Series.Count != 0 || chart.HasPivotSource || chart.ChartType == chartType)
      return;
    chart.ChartType = chartType;
  }

  private void ParsePlotAreaAxes(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    Excel2007Parser parser)
  {
    ChartAxisParser chartAxisParser = new ChartAxisParser(this.m_book);
    IChartFrameFormat plotArea = chart.PlotArea;
    FileDataHolder parentHolder = chart.DataHolder.ParentHolder;
    Dictionary<int, int> dictionary = new Dictionary<int, int>();
    int num = 0;
    ExcelChartType chartType = chart.ChartType;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "valAx":
            bool bPrimary1 = num <= 1;
            chart.CreateNecessaryAxes(bPrimary1);
            ChartValueAxisImpl valueAxis = bPrimary1 ? (ChartValueAxisImpl) chart.PrimaryValueAxis : (ChartValueAxisImpl) chart.SecondaryValueAxis;
            chartAxisParser.ParseValueAxis(reader, valueAxis, relations, chartType, parser);
            ++num;
            continue;
          case "serAx":
            chart.CreateNecessaryAxes(true);
            ChartSeriesAxisImpl seriesAxis = (ChartSeriesAxisImpl) chart.PrimarySerieAxis ?? chart.CreatePrimarySeriesAxis();
            chartAxisParser.ParseSeriesAxis(reader, seriesAxis, relations, chartType, parser);
            continue;
          case "catAx":
            bool bPrimary2 = num <= 1;
            chart.CreateNecessaryAxes(bPrimary2);
            ChartCategoryAxisImpl axis1 = num <= 1 ? (ChartCategoryAxisImpl) chart.PrimaryCategoryAxis : (ChartCategoryAxisImpl) chart.SecondaryCategoryAxis;
            chartAxisParser.ParseCategoryAxis(reader, axis1, relations, chartType, parser);
            ++num;
            continue;
          case "dateAx":
            bool bPrimary3 = num <= 1;
            chart.CreateNecessaryAxes(bPrimary3);
            ChartCategoryAxisImpl axis2 = bPrimary3 ? (ChartCategoryAxisImpl) chart.PrimaryCategoryAxis : (ChartCategoryAxisImpl) chart.SecondaryCategoryAxis;
            chartAxisParser.ParseDateAxis(reader, axis2, relations, chartType, parser);
            ++num;
            continue;
          case "bubbleChart":
            chartType = ExcelChartType.Bubble;
            reader.Skip();
            continue;
          case "scatterChart":
            chartType = ExcelChartType.Scatter_Markers;
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

  private void ParseBarChart(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    Dictionary<int, int> dictSeriesAxis,
    out ExcelChartType chartType)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "barChart")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    List<ChartSerieImpl> lstSeries = new List<ChartSerieImpl>();
    string shape = (string) null;
    IChartSerie barChartShared = (IChartSerie) this.ParseBarChartShared(reader, chart, relations, false, lstSeries, out shape, out chartType);
    int? nullable1 = new int?();
    int? nullable2 = new int?();
    bool secondary = false;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "gapWidth":
            string valueTag1 = ChartParserCommon.ParseValueTag(reader);
            nullable1 = new int?(int.Parse(valueTag1));
            if (barChartShared != null)
            {
              (barChartShared as ChartSerieImpl).GapWidth = nullable1.Value;
              (barChartShared as ChartSerieImpl).ShowGapWidth = true;
            }
            chart.GapWidth = int.Parse(valueTag1);
            continue;
          case "overlap":
            if (reader.MoveToAttribute("val"))
            {
              string valueTag2 = ChartParserCommon.ParseValueTag(reader);
              nullable2 = new int?(int.Parse(valueTag2));
              if (barChartShared != null)
                (barChartShared as ChartSerieImpl).Overlap = nullable2.Value;
              chart.OverLap = int.Parse(valueTag2);
              int? nullable3 = nullable2;
              if ((nullable3.GetValueOrDefault() != 100 ? 0 : (nullable3.HasValue ? 1 : 0)) != 0)
              {
                nullable2 = new int?(-65436);
                continue;
              }
              continue;
            }
            reader.Skip();
            continue;
          case "serLines":
            this.ParseLines(reader, chart, barChartShared as ChartSerieImpl, reader.LocalName);
            continue;
          case "axId":
            if (this.ParseAxisId(reader, lstSeries, dictSeriesAxis))
            {
              secondary = true;
              continue;
            }
            continue;
          case "extLst":
            this.ParseFilteredSeries(reader, chart, relations, true, barChartShared.SerieType, secondary);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    IChartFormat commonSerieOptions = barChartShared?.SerieFormat.CommonSerieOptions;
    if (nullable1.HasValue && barChartShared != null)
      commonSerieOptions.GapWidth = nullable1.Value;
    if (nullable2.HasValue && barChartShared != null)
      commonSerieOptions.Overlap = nullable2.Value;
    if (nullable1.HasValue && commonSerieOptions == null && !chart.HasPivotSource)
    {
      if (chart.ChartFormat.FormatRecordType != TBIFFRecord.ChartBar)
        chart.ChartFormat.ChangeSerieType(chartType, false);
      chart.ChartFormat.GapWidth = nullable1.Value;
    }
    reader.Read();
  }

  private IChartSerie ParseFilteredSeries(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    bool is3D,
    ExcelChartType SeriesType,
    bool secondary)
  {
    FileDataHolder parentHolder = chart.DataHolder.ParentHolder;
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    string empty1 = string.Empty;
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    string empty2 = string.Empty;
    if (reader.LocalName != "extLst")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    ChartSerieImpl filteredSeries = (ChartSerieImpl) null;
    if (reader.LocalName == "ext")
    {
      reader.Read();
      while (reader.LocalName != "extLst" && reader.NodeType != XmlNodeType.EndElement)
      {
        reader.Read();
        if (reader.LocalName == "ser")
        {
          switch (SeriesType)
          {
            case ExcelChartType.Line:
            case ExcelChartType.Line_Stacked:
            case ExcelChartType.Line_Stacked_100:
            case ExcelChartType.Line_Markers:
            case ExcelChartType.Line_Markers_Stacked:
            case ExcelChartType.Line_Markers_Stacked_100:
            case ExcelChartType.Line_3D:
              filteredSeries = this.ParseLineSeries(reader, chart, SeriesType, relations, parentHolder.Parser);
              break;
            case ExcelChartType.Pie:
            case ExcelChartType.Pie_3D:
            case ExcelChartType.PieOfPie:
            case ExcelChartType.Pie_Exploded:
            case ExcelChartType.Pie_Exploded_3D:
            case ExcelChartType.Pie_Bar:
              filteredSeries = this.ParsePieSeries(reader, chart, SeriesType, relations);
              break;
            case ExcelChartType.Scatter_Markers:
            case ExcelChartType.Scatter_SmoothedLine_Markers:
            case ExcelChartType.Scatter_SmoothedLine:
            case ExcelChartType.Scatter_Line_Markers:
            case ExcelChartType.Scatter_Line:
              filteredSeries = this.ParseScatterSeries(reader, chart, SeriesType, relations, parentHolder.Parser);
              break;
            case ExcelChartType.Area:
            case ExcelChartType.Area_Stacked:
            case ExcelChartType.Area_Stacked_100:
            case ExcelChartType.Area_3D:
            case ExcelChartType.Area_Stacked_3D:
            case ExcelChartType.Area_Stacked_100_3D:
              filteredSeries = this.ParseAreaSeries(reader, chart, SeriesType, relations, !secondary);
              break;
            case ExcelChartType.Radar:
            case ExcelChartType.Radar_Markers:
            case ExcelChartType.Radar_Filled:
              filteredSeries = this.ParseRadarSeries(reader, chart, SeriesType, relations, parentHolder.Parser);
              break;
            case ExcelChartType.Surface_3D:
            case ExcelChartType.Surface_NoColor_3D:
            case ExcelChartType.Surface_Contour:
            case ExcelChartType.Surface_NoColor_Contour:
              filteredSeries = this.ParseSurfaceSeries(reader, chart, SeriesType, relations);
              break;
            case ExcelChartType.Bubble:
            case ExcelChartType.Bubble_3D:
              filteredSeries = this.ParseBubbleSeries(reader, chart, relations);
              break;
            default:
              filteredSeries = this.ParseBarSeries(reader, chart, SeriesType, relations);
              break;
          }
        }
        if (secondary)
          filteredSeries.UsePrimaryAxis = !secondary;
        filteredSeries.IsFiltered = true;
        reader.Skip();
      }
      reader.Read();
      reader.Read();
    }
    return (IChartSerie) filteredSeries;
  }

  private bool ParseAxisId(
    XmlReader reader,
    List<ChartSerieImpl> lstSeries,
    Dictionary<int, int> dictSeriesAxis)
  {
    int intValueTag = ChartParserCommon.ParseIntValueTag(reader);
    int count = lstSeries.Count;
    bool axisId1 = false;
    if (count > 0)
    {
      ChartImpl parentChart = lstSeries[0].ParentChart;
      int axisId2 = (parentChart.PrimaryValueAxis as ChartAxisImpl).AxisId;
      int axisId3 = (parentChart.PrimaryCategoryAxis as ChartAxisImpl).AxisId;
      axisId1 = intValueTag != axisId2 && intValueTag != axisId3;
      for (int index1 = 0; index1 < count; ++index1)
      {
        int index2 = lstSeries[index1].Index;
        dictSeriesAxis[index2] = intValueTag;
      }
    }
    lstSeries.Clear();
    return axisId1;
  }

  internal void ParseBar3DChart(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    Dictionary<int, int> dictSeriesAxis,
    out ExcelChartType chartType)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "bar3DChart")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    List<ChartSerieImpl> lstSeries = new List<ChartSerieImpl>();
    string shape = (string) null;
    ChartSerieImpl barChartShared = this.ParseBarChartShared(reader, chart, relations, true, lstSeries, out shape, out chartType);
    int? nullable = new int?();
    if (shape != null && barChartShared != null)
      this.ParseBarShape(shape, barChartShared.GetCommonSerieFormat().SerieDataFormat);
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element && barChartShared != null)
      {
        switch (reader.LocalName)
        {
          case "gapWidth":
            nullable = new int?(int.Parse(ChartParserCommon.ParseValueTag(reader)));
            chart.GapWidth = nullable.Value;
            chart.ShowGapWidth = true;
            continue;
          case "gapDepth":
            chart.GapDepth = ChartParserCommon.ParseIntValueTag(reader);
            continue;
          case "axId":
            this.ParseAxisId(reader, lstSeries, dictSeriesAxis);
            continue;
          case "extLst":
            this.ParseFilteredSeries(reader, chart, relations, true, barChartShared.SerieType, false);
            continue;
          case "shape":
            string valueTag = ChartParserCommon.ParseValueTag(reader);
            if (barChartShared != null)
            {
              this.ParseBarShape(valueTag, barChartShared.GetCommonSerieFormat().SerieDataFormat);
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
    if (barChartShared != null && nullable.HasValue)
      barChartShared.SerieFormat.CommonSerieOptions.GapWidth = nullable.Value;
    else if (nullable.HasValue && !chart.HasPivotSource)
    {
      if (chart.ChartFormat.FormatRecordType != TBIFFRecord.ChartBar)
        chart.ChartFormat.ChangeSerieType(chartType, false);
      chart.ChartFormat.GapWidth = nullable.Value;
    }
    foreach (ChartSerieImpl chartSerieImpl in (CollectionBase<IChartSerie>) chart.Series)
    {
      if (!(chartSerieImpl.SerieFormat as ChartSerieDataFormatImpl).HasBarShape)
      {
        chartSerieImpl.SerieFormat.BarShapeBase = chartSerieImpl.GetCommonSerieFormat().SerieDataFormat.BarShapeBase;
        chartSerieImpl.SerieFormat.BarShapeTop = chartSerieImpl.GetCommonSerieFormat().SerieDataFormat.BarShapeTop;
      }
    }
    reader.Read();
  }

  private void ParseBarShape(XmlReader reader, ChartSerieImpl firstSeries)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (firstSeries == null)
      throw new ArgumentNullException(nameof (firstSeries));
    this.ParseBarShape(ChartParserCommon.ParseValueTag(reader), firstSeries.SerieFormat);
  }

  private void ParseBarShape(string value, IChartSerieDataFormat dataFormat)
  {
    if (value == null)
      return;
    if (dataFormat == null)
      throw new ArgumentNullException(nameof (dataFormat));
    switch (value)
    {
      case "cone":
        dataFormat.BarShapeTop = ExcelTopFormat.Sharp;
        dataFormat.BarShapeBase = ExcelBaseFormat.Circle;
        break;
      case "pyramid":
        dataFormat.BarShapeTop = ExcelTopFormat.Sharp;
        dataFormat.BarShapeBase = ExcelBaseFormat.Rectangle;
        break;
      case "coneToMax":
        dataFormat.BarShapeTop = ExcelTopFormat.Trunc;
        dataFormat.BarShapeBase = ExcelBaseFormat.Circle;
        break;
      case "pyramidToMax":
        dataFormat.BarShapeTop = ExcelTopFormat.Trunc;
        dataFormat.BarShapeBase = ExcelBaseFormat.Rectangle;
        break;
      case "cylinder":
        dataFormat.BarShapeTop = ExcelTopFormat.Straight;
        dataFormat.BarShapeBase = ExcelBaseFormat.Circle;
        break;
      case "box":
        dataFormat.BarShapeTop = ExcelTopFormat.Straight;
        dataFormat.BarShapeBase = ExcelBaseFormat.Rectangle;
        break;
      default:
        throw new XmlException();
    }
  }

  private ChartSerieImpl ParseBarChartShared(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    bool is3D,
    List<ChartSerieImpl> lstSeries,
    out string shape,
    out ExcelChartType chartType)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    chartType = chart != null ? chart.ChartType : throw new ArgumentNullException(nameof (chart));
    bool flag1 = true;
    ChartSerieImpl series = (ChartSerieImpl) null;
    string str1 = (string) null;
    bool flag2 = false;
    string str2 = (string) null;
    shape = (string) null;
    Stream data1 = (Stream) null;
    MemoryStream data2 = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((Stream) data2, Encoding.UTF8);
    writer.WriteStartElement("root");
    int? nullable = new int?();
    while (flag1)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "barDir":
            str1 = ChartParserCommon.ParseValueTag(reader);
            continue;
          case "grouping":
            str2 = ChartParserCommon.ParseValueTag(reader);
            continue;
          case "varyColors":
            flag2 = ChartParserCommon.ParseBoolValueTag(reader);
            continue;
          case nameof (shape):
            shape = ChartParserCommon.ParseValueTag(reader);
            continue;
          case "ser":
            writer.WriteNode(reader, false);
            writer.Flush();
            continue;
          case "dLbls":
            if (!reader.IsEmptyElement)
            {
              data1 = ShapeParser.ReadNodeAsStream(reader);
              continue;
            }
            reader.Skip();
            continue;
          case "gapWidth":
            if (data2.Length == 0L && !chart.HasPivotSource)
            {
              nullable = new int?(int.Parse(ChartParserCommon.ParseValueTag(reader)));
              chart.GapWidth = nullable.Value;
              chart.ShowGapWidth = true;
              continue;
            }
            flag1 = false;
            continue;
          default:
            if (data2.Length == 0L && !chart.HasPivotSource)
            {
              ExcelChartType pivotBarSeriesType = this.GetPivotBarSeriesType(str1, str2, shape, is3D);
              this.ParseFilterSecondaryAxis(reader, pivotBarSeriesType, is3D, lstSeries, chart, relations, ref series);
            }
            flag1 = false;
            continue;
        }
      }
      else
        reader.Skip();
    }
    writer.WriteEndElement();
    writer.Flush();
    data2.Position = 0L;
    XmlReader reader1 = UtilityMethods.CreateReader((Stream) data2);
    if (!reader1.IsEmptyElement)
    {
      reader1.Read();
      this.ParseSeries(reader1, str1, str2, shape, is3D, lstSeries, chart, relations, ref series);
      if (series != null)
      {
        series.Grouping = str2;
        series.SerieFormat.CommonSerieOptions.IsVaryColor = flag2;
      }
    }
    reader1.Close();
    writer.Close();
    if (data1 != null && series != null)
    {
      data1.Position = 0L;
      XmlReader reader2 = UtilityMethods.CreateReader(data1);
      this.ParseDataLabels(reader2, chart, relations, series.Number);
      reader2.Close();
    }
    if (chart.HasPivotSource)
      chart.PivotChartType = chart.ChartType == ExcelChartType.Combination_Chart ? ExcelChartType.Combination_Chart : this.GetPivotBarSeriesType(str1, str2, shape, is3D);
    else if (series == null && chart.ChartFormat != (ChartFormatImpl) null)
    {
      chartType = this.GetPivotBarSeriesType(str1, str2, shape, is3D);
      chart.ChartFormat.IsVaryColor = flag2;
      if (nullable.HasValue)
        chart.ChartFormat.GapWidth = nullable.Value;
    }
    return series;
  }

  private ChartSerieImpl ParseFilterSecondaryAxis(
    XmlReader reader,
    ExcelChartType seriesType,
    bool is3D,
    List<ChartSerieImpl> lstSeries,
    ChartImpl chart,
    RelationCollection relations,
    ref ChartSerieImpl series)
  {
    ChartSerieImpl filterSecondaryAxis = (ChartSerieImpl) null;
    Dictionary<int, int> dictionary = new Dictionary<int, int>();
    IChartSerie chartSerie = (IChartSerie) null;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "overlap":
            string s = "0";
            if (reader.MoveToAttribute("val"))
              s = ChartParserCommon.ParseValueTag(reader);
            else
              reader.Skip();
            chart.OverLap = int.Parse(s);
            continue;
          case "axId":
            reader.Skip();
            continue;
          case "extLst":
            chartSerie = this.ParseFilteredSeries(reader, chart, relations, true, seriesType, true);
            chartSerie.UsePrimaryAxis = false;
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
    }
    series = chartSerie as ChartSerieImpl;
    return filterSecondaryAxis;
  }

  private void ParseSeries(
    XmlReader reader,
    string strDirection,
    string strGrouping,
    string shape,
    bool is3D,
    List<ChartSerieImpl> lstSeries,
    ChartImpl chart,
    RelationCollection relations,
    ref ChartSerieImpl series)
  {
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "ser")
      {
        ExcelChartType pivotBarSeriesType = this.GetPivotBarSeriesType(strDirection, strGrouping, shape, is3D);
        ChartSerieImpl barSeries = this.ParseBarSeries(reader, chart, pivotBarSeriesType, relations);
        if (series == null)
          series = barSeries;
        lstSeries.Add(barSeries);
      }
      else
        reader.Read();
    }
  }

  private void FindFilter(
    IChartCategories categories,
    string filteredcategory,
    string fullreference,
    IChartSerie series1,
    bool isseries)
  {
    IRange range1 = this.FindRange(series1, fullreference);
    int start = isseries ? range1.Column : range1.Row;
    int end = isseries ? range1.LastColumn : range1.LastRow;
    filteredcategory = filteredcategory.Trim('(');
    filteredcategory = filteredcategory.Trim(')');
    string[] strArray = filteredcategory.Split(',');
    int[] categories_length = new int[range1.Count];
    int index1 = 0;
    for (int index2 = 0; index2 < strArray.Length; ++index2)
    {
      IRange range2 = this.FindRange(series1, strArray[index2]);
      int num1 = isseries ? range2.Column : range2.Row;
      int num2 = isseries ? range2.LastColumn : range2.LastRow;
      for (int index3 = num1; index3 <= num2; ++index3)
      {
        categories_length[index1] = index3;
        ++index1;
      }
    }
    int catLenIndex = 0;
    int catIndex = 0;
    if (range1 is RangesCollection rangesCollection)
    {
      for (int index4 = 0; index4 < rangesCollection.InnerList.Count; ++index4)
        this.FilterCategories(isseries ? rangesCollection.InnerList[index4].Column : rangesCollection.InnerList[index4].Row, isseries ? rangesCollection.InnerList[index4].LastColumn : rangesCollection.InnerList[index4].LastRow, categories_length, categories, ref catLenIndex, ref catIndex);
    }
    else
      this.FilterCategories(start, end, categories_length, categories, ref catLenIndex, ref catIndex);
  }

  private void FilterCategories(
    int start,
    int end,
    int[] categories_length,
    IChartCategories categories,
    ref int catLenIndex,
    ref int catIndex)
  {
    for (int index = start; index <= end; ++index)
    {
      if (index != categories_length[catLenIndex] && categories_length[catLenIndex] != 0)
        categories[catIndex].IsFiltered = true;
      if (index == categories_length[catLenIndex] && categories_length[catLenIndex] != 0)
        ++catLenIndex;
      else
        categories[catIndex].IsFiltered = true;
      ++catIndex;
    }
  }

  private IRange FindRange(IChartSerie series1, string strValue)
  {
    IRange range = (IRange) null;
    bool flag1 = true;
    bool flag2 = false;
    bool flag3 = false;
    ChartSerieImpl chartSerieImpl = series1 as ChartSerieImpl;
    WorkbookImpl parentBook = chartSerieImpl.ParentBook;
    if (strValue != null)
    {
      range = ChartParser.GetRange(chartSerieImpl.ParentBook, strValue);
      switch (range)
      {
        case ExternalRange _:
          (range as ExternalRange).IsNumReference = flag1;
          (range as ExternalRange).IsStringReference = flag2;
          (range as ExternalRange).IsMultiReference = flag3;
          break;
        case RangeImpl _:
          (range as RangeImpl).IsNumReference = flag1;
          (range as RangeImpl).IsStringReference = flag2;
          (range as RangeImpl).IsMultiReference = flag3;
          break;
        case NameImpl _:
          (range as NameImpl).IsNumReference = flag1;
          (range as NameImpl).IsStringReference = flag2;
          (range as NameImpl).IsMultiReference = flag3;
          break;
      }
    }
    return range;
  }

  public IRange GetSerieOrAxisRange(
    IRange range,
    bool bIsInRow,
    out IRange serieRange,
    int CategoryLabelCount)
  {
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    int num1 = bIsInRow ? range.Row : range.Column;
    int num2 = bIsInRow ? range.LastRow : range.LastColumn;
    int num3 = bIsInRow ? range.Column : range.Row;
    int num4 = bIsInRow ? range.LastColumn : range.LastRow;
    int num5 = -1;
    bool flag = false;
    for (int index = num3; index < num4 && (!flag || CategoryLabelCount != 0); ++index)
    {
      IRange range1 = bIsInRow ? range.Worksheet[num2, index] : range.Worksheet[index, num2];
      flag = range1.HasNumber || range1.IsBlank || range1.HasFormula;
      if (!flag)
        num5 = index;
      if (CategoryLabelCount != 0)
        --CategoryLabelCount;
    }
    if (num5 == -1)
    {
      serieRange = range;
      return (IRange) null;
    }
    IRange serieOrAxisRange = bIsInRow ? range.Worksheet[num1, num3, num2, num5] : range.Worksheet[num3, num1, num5, num2];
    serieRange = bIsInRow ? range.Worksheet[range.Row, serieOrAxisRange.LastColumn + 1, range.LastRow, range.LastColumn] : range.Worksheet[serieOrAxisRange.LastRow + 1, range.Column, range.LastRow, range.LastColumn];
    return serieOrAxisRange;
  }

  private bool DetectIsInRow(IRange range)
  {
    return range == null || range.LastRow - range.Row <= range.LastColumn - range.Column;
  }

  private ExcelChartType GetBarSeriesType(
    string direction,
    string grouping,
    bool is3D,
    string shape)
  {
    string str1;
    if (!is3D)
    {
      str1 = direction == "bar" ? "Bar" : "Column";
    }
    else
    {
      str1 = shape == "box" ? "Column" : "Cone";
      if (direction == "bar")
        str1 = $"{str1}{(object) '_'}Bar";
    }
    string str2;
    switch (grouping)
    {
      case "clustered":
        str2 = str1 + "_Clustered";
        break;
      case "percentStacked":
        str2 = str1 + "_Stacked_100";
        break;
      case "stacked":
        str2 = str1 + "_Stacked";
        break;
      default:
        str2 = !is3D ? str1 + "_Clustered" : str1 + "_Clustered_3D";
        break;
    }
    return (ExcelChartType) Enum.Parse(typeof (ExcelChartType), str2, false);
  }

  private ExcelChartType GetPivotBarSeriesType(
    string direction,
    string grouping,
    string shape,
    bool is3D)
  {
    string[] array = new string[3]
    {
      "Cone".ToLower(),
      "Cylinder".ToLower(),
      "Pyramid".ToLower()
    };
    if (shape != null)
      shape = shape.ToLower();
    string str1;
    if (shape != null)
    {
      if (Array.IndexOf<string>(array, shape) == -1)
      {
        str1 = direction == "bar" ? "Bar" : "Column";
      }
      else
      {
        str1 = shape;
        if (direction == "bar")
          str1 += "_Bar";
      }
    }
    else
    {
      string str2;
      switch (direction)
      {
        case "bar":
          str2 = shape == null ? "Bar" : shape + "_Bar";
          break;
        case "col":
          if (shape != null)
          {
            str2 = shape;
            break;
          }
          goto default;
        default:
          str2 = "Column";
          break;
      }
      str1 = str2;
    }
    string str3;
    switch (grouping)
    {
      case "clustered":
        str3 = !is3D || shape != null && Array.IndexOf<string>(array, shape) != -1 ? str1 + "_Clustered" : str1 + "_Clustered_3D";
        break;
      case "standard":
        str3 = !is3D || shape != null && Array.IndexOf<string>(array, shape) != -1 ? str1 + "_Clustered_3D" : str1 + "_3D";
        break;
      case "percentStacked":
        str3 = !is3D || shape != null && Array.IndexOf<string>(array, shape) != -1 ? str1 + "_Stacked_100" : str1 + "_Stacked_100_3D";
        break;
      case "stacked":
        str3 = !is3D || shape != null && Array.IndexOf<string>(array, shape) != -1 ? str1 + "_Stacked" : str1 + "_Stacked_3D";
        break;
      default:
        str3 = !is3D ? str1 + "_Clustered" : str1 + "_Clustered_3D";
        break;
    }
    return (ExcelChartType) Enum.Parse(typeof (ExcelChartType), str3, true);
  }

  private ExcelChartType GetAreaSeriesType(string grouping, bool is3D)
  {
    string str = "Area";
    switch (grouping)
    {
      case "percentStacked":
        str += "_Stacked_100";
        break;
      case "stacked":
        str += "_Stacked";
        break;
    }
    if (is3D)
      str += "_3D";
    return (ExcelChartType) Enum.Parse(typeof (ExcelChartType), str, false);
  }

  private ExcelChartType GetLineSeriesType(string grouping, bool is3D)
  {
    string str = "Line";
    ExcelChartType lineSeriesType;
    if (!is3D)
    {
      switch (grouping)
      {
        case "percentStacked":
          str += "_Stacked_100";
          break;
        case "stacked":
          str += "_Stacked";
          break;
      }
      lineSeriesType = (ExcelChartType) Enum.Parse(typeof (ExcelChartType), str, false);
    }
    else
      lineSeriesType = ExcelChartType.Line_3D;
    return lineSeriesType;
  }

  private void ParseArea3DChart(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    Dictionary<int, int> dictSeriesAxis,
    out ExcelChartType chartType)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "area3DChart")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    List<ChartSerieImpl> lstSeries = new List<ChartSerieImpl>();
    this.ParseAreaChartCommon(reader, chart, true, relations, lstSeries, true, out chartType);
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "gapDepth":
            chart.GapDepth = ChartParserCommon.ParseIntValueTag(reader);
            continue;
          case "axId":
            this.ParseAxisId(reader, lstSeries, dictSeriesAxis);
            continue;
          case "extLst":
            this.ParseFilteredSeries(reader, chart, relations, true, chart.ChartType, false);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Read();
    }
    reader.Read();
  }

  private void ParseAreaChart(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    Dictionary<int, int> dictSeriesAxis,
    out ExcelChartType chartType)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "areaChart")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    bool secondary = false;
    List<ChartSerieImpl> lstSeries = new List<ChartSerieImpl>();
    bool axisType = this.GetAxisType(chart, ref reader);
    this.ParseAreaChartCommon(reader, chart, false, relations, lstSeries, !axisType, out chartType);
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "axId")
      {
        if (this.ParseAxisId(reader, lstSeries, dictSeriesAxis))
          secondary = true;
      }
      else if (reader.LocalName == "extLst")
      {
        if (chart.Series[0] != null)
          this.ParseFilteredSeries(reader, chart, relations, true, chart.Series[0].SerieType, secondary);
      }
      else
        reader.Skip();
    }
  }

  private bool GetAxisType(ChartImpl chart, ref XmlReader reader)
  {
    MemoryStream data = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((Stream) data, Encoding.UTF8);
    writer.WriteStartElement("root");
    bool axisType = false;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "axId")
      {
        string localName = reader.LocalName;
        int intValueTag = ChartParserCommon.ParseIntValueTag(reader);
        int axisId1 = (chart.PrimaryValueAxis as ChartAxisImpl).AxisId;
        int axisId2 = (chart.PrimaryCategoryAxis as ChartAxisImpl).AxisId;
        axisType = intValueTag != axisId1 && intValueTag != axisId2;
        ChartSerializatorCommon.SerializeValueTag(writer, localName, intValueTag.ToString());
      }
      else if (reader.NodeType == XmlNodeType.Element)
        writer.WriteNode(reader, false);
      else
        reader.Skip();
    }
    writer.WriteEndElement();
    writer.Flush();
    reader.Read();
    data.Position = 0L;
    reader = UtilityMethods.CreateReader((Stream) data);
    reader.Read();
    return axisType;
  }

  private void ParseAreaChartCommon(
    XmlReader reader,
    ChartImpl chart,
    bool b3D,
    RelationCollection relations,
    List<ChartSerieImpl> lstSeries,
    bool isPrimary,
    out ExcelChartType chartType)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    bool flag1 = true;
    string grouping = (string) null;
    bool flag2 = false;
    ChartSerieImpl series = (ChartSerieImpl) null;
    chartType = ExcelChartType.Area;
    while (reader.NodeType != XmlNodeType.EndElement && flag1)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "grouping":
            grouping = reader.MoveToAttribute("val") ? reader.Value : "standard";
            reader.Read();
            continue;
          case "varyColors":
            flag2 = ChartParserCommon.ParseBoolValueTag(reader);
            continue;
          case "ser":
            ExcelChartType areaSeriesType = this.GetAreaSeriesType(grouping, b3D);
            series = this.ParseAreaSeries(reader, chart, areaSeriesType, relations, isPrimary);
            series.SerieFormat.CommonSerieOptions.IsVaryColor = flag2;
            lstSeries.Add(series);
            continue;
          case "dLbls":
            if (!reader.IsEmptyElement && series != null)
            {
              this.ParseDataLabels(reader, chart, relations, series.Number);
              continue;
            }
            reader.Skip();
            continue;
          case "dropLines":
            this.ParseLines(reader, chart, series, reader.LocalName);
            continue;
          default:
            if (lstSeries.Count != 0)
            {
              flag1 = false;
              continue;
            }
            ExcelChartType lineSeriesType = this.GetLineSeriesType(grouping, b3D);
            this.ParseFilterSecondaryAxis(reader, lineSeriesType, b3D, lstSeries, chart, relations, ref series);
            flag1 = false;
            continue;
        }
      }
      else
        reader.Read();
    }
    if (chart.HasPivotSource)
    {
      chart.PivotChartType = chart.ChartType == ExcelChartType.Combination_Chart ? ExcelChartType.Combination_Chart : this.GetAreaSeriesType(grouping, b3D);
    }
    else
    {
      if (series != null || !(chart.ChartFormat != (ChartFormatImpl) null))
        return;
      chartType = this.GetAreaSeriesType(grouping, b3D);
      chart.ChartFormat.IsVaryColor = flag2;
    }
  }

  private ChartSerieImpl ParseLineChartCommon(
    XmlReader reader,
    ChartImpl chart,
    bool is3D,
    RelationCollection relations,
    List<ChartSerieImpl> lstSeries,
    Excel2007Parser parser,
    out ExcelChartType chartType)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    bool flag1 = true;
    string grouping = (string) null;
    bool flag2 = false;
    ChartSerieImpl lineChartCommon = (ChartSerieImpl) null;
    ChartSerieImpl series = (ChartSerieImpl) null;
    chartType = ExcelChartType.Line;
    bool flag3 = false;
    while (reader.NodeType != XmlNodeType.EndElement && flag1)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "grouping":
            grouping = reader.MoveToAttribute("val") ? reader.Value : "standard";
            reader.Read();
            continue;
          case "varyColors":
            flag2 = ChartParserCommon.ParseBoolValueTag(reader);
            continue;
          case "ser":
            ExcelChartType lineSeriesType1 = this.GetLineSeriesType(grouping, is3D);
            series = this.ParseLineSeries(reader, chart, lineSeriesType1, relations, parser);
            series.SerieFormat.CommonSerieOptions.IsVaryColor = flag2;
            lstSeries.Add(series);
            if (lineChartCommon == null)
            {
              lineChartCommon = series;
              continue;
            }
            continue;
          case "dLbls":
            if (!reader.IsEmptyElement && series != null)
            {
              this.ParseDataLabels(reader, chart, relations, series.Number);
              continue;
            }
            reader.Skip();
            continue;
          case "dropLines":
            if (lineChartCommon != null)
            {
              lineChartCommon.DropLinesStream = ShapeParser.ReadNodeAsStream(reader, true);
              continue;
            }
            reader.Skip();
            continue;
          case "marker":
            if (lstSeries.Count == 0)
            {
              flag3 = ChartParserCommon.ParseBoolValueTag(reader);
              continue;
            }
            flag1 = false;
            continue;
          case "smooth":
            if (lstSeries.Count == 0)
            {
              ChartParserCommon.ParseBoolValueTag(reader);
              continue;
            }
            flag1 = false;
            continue;
          default:
            if (lstSeries.Count != 0)
            {
              flag1 = false;
              continue;
            }
            ExcelChartType lineSeriesType2 = this.GetLineSeriesType(grouping, is3D);
            this.ParseFilterSecondaryAxis(reader, lineSeriesType2, is3D, lstSeries, chart, relations, ref series);
            flag1 = false;
            continue;
        }
      }
      else
        reader.Skip();
    }
    if (chart.HasPivotSource)
      chart.PivotChartType = chart.ChartType == ExcelChartType.Combination_Chart ? ExcelChartType.Combination_Chart : this.GetLineSeriesType(grouping, is3D);
    else if (series == null && chart.ChartFormat != (ChartFormatImpl) null)
    {
      chartType = this.GetLineSeriesType(grouping, is3D);
      if (flag3)
        chartType = (ExcelChartType) Enum.Parse(typeof (ExcelChartType), chartType.ToString().Insert(4, "_Markers"), false);
      chart.ChartFormat.IsVaryColor = flag2;
    }
    return lineChartCommon;
  }

  private void ParseLine3DChart(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    Dictionary<int, int> dictSeriesAxis,
    Excel2007Parser parser,
    out ExcelChartType chartType)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "line3DChart")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    List<ChartSerieImpl> lstSeries = new List<ChartSerieImpl>();
    this.ParseLineChartCommon(reader, chart, true, relations, lstSeries, parser, out chartType);
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "gapDepth")
        chart.GapDepth = ChartParserCommon.ParseIntValueTag(reader);
      else if (reader.LocalName == "axId")
        this.ParseAxisId(reader, lstSeries, dictSeriesAxis);
      else if (reader.LocalName == "extLst")
        this.ParseFilteredSeries(reader, chart, relations, true, ExcelChartType.Line_3D, false);
      else
        reader.Skip();
    }
    reader.Read();
  }

  private void ParseLineChart(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    Dictionary<int, int> dictSeriesAxis,
    Excel2007Parser parser,
    out ExcelChartType chartType,
    List<int> markerArray)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    reader.Read();
    List<ChartSerieImpl> lstSeries = new List<ChartSerieImpl>();
    ChartSerieImpl lineChartCommon = this.ParseLineChartCommon(reader, chart, false, relations, lstSeries, parser, out chartType);
    bool secondary = false;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "upDownBars":
            this.ParseUpDownBars(reader, chart, lineChartCommon, relations);
            continue;
          case "marker":
            if (ChartParserCommon.ParseBoolValueTag(reader) && lineChartCommon != null)
            {
              ChartMarkerFormatRecord markerFormat = ((ChartSerieDataFormatImpl) lineChartCommon.SerieFormat).MarkerFormat;
              if (!lineChartCommon.SerieType.ToString().Contains("_Markers"))
              {
                markerArray.Add(lineChartCommon.Index);
                continue;
              }
              continue;
            }
            continue;
          case "hiLowLines":
          case "dropLines":
            this.ParseLines(reader, chart, lineChartCommon, reader.LocalName);
            continue;
          case "axId":
            if (this.ParseAxisId(reader, lstSeries, dictSeriesAxis))
            {
              secondary = true;
              continue;
            }
            continue;
          case "extLst":
            this.ParseFilteredSeries(reader, chart, relations, true, lineChartCommon.SerieType, secondary);
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

  private void ParseBubbleChart(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    Dictionary<int, int> dictSeriesAxis,
    out ExcelChartType chartType)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "bubbleChart")
      throw new XmlException("Unexpected xml tag.");
    chartType = ExcelChartType.Bubble;
    reader.Read();
    bool flag1 = false;
    ChartSerieImpl chartSerieImpl = (ChartSerieImpl) null;
    List<ChartSerieImpl> lstSeries = new List<ChartSerieImpl>();
    int num = 0;
    bool flag2 = false;
    ExcelBubbleSize excelBubbleSize = ExcelBubbleSize.Area;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "varyColors":
            flag1 = ChartParserCommon.ParseBoolValueTag(reader);
            continue;
          case "ser":
            chartSerieImpl = this.ParseBubbleSeries(reader, chart, relations);
            chartSerieImpl.SerieFormat.CommonSerieOptions.IsVaryColor = flag1;
            lstSeries.Add(chartSerieImpl);
            continue;
          case "bubbleScale":
            num = ChartParserCommon.ParseIntValueTag(reader);
            if (chartSerieImpl != null)
            {
              chartSerieImpl.SerieFormat.CommonSerieOptions.BubbleScale = num;
              continue;
            }
            continue;
          case "showNegBubbles":
            flag2 = ChartParserCommon.ParseBoolValueTag(reader);
            if (chartSerieImpl != null)
            {
              chartSerieImpl.SerieFormat.CommonSerieOptions.ShowNegativeBubbles = flag2;
              continue;
            }
            continue;
          case "sizeRepresents":
            excelBubbleSize = ChartParserCommon.ParseValueTag(reader) == "area" ? ExcelBubbleSize.Area : ExcelBubbleSize.Width;
            if (chartSerieImpl != null)
            {
              chartSerieImpl.SerieFormat.CommonSerieOptions.SizeRepresents = excelBubbleSize;
              continue;
            }
            continue;
          case "axId":
            this.ParseAxisId(reader, lstSeries, dictSeriesAxis);
            continue;
          case "extLst":
            this.ParseFilteredSeries(reader, chart, relations, false, chartSerieImpl.SerieType, false);
            continue;
          case "dLbls":
            if (!reader.IsEmptyElement && chartSerieImpl != null)
            {
              this.ParseDataLabels(reader, chart, relations, chartSerieImpl.Number);
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
    if (chartSerieImpl == null && chart.ChartFormat != (ChartFormatImpl) null)
    {
      chart.ChartType = chartType;
      if (chart.ChartFormat.FormatRecordType != TBIFFRecord.ChartScatter)
        chart.ChartFormat.ChangeSerieType(chartType, false);
      chart.ChartFormat.BubbleScale = num;
      chart.ChartFormat.ShowNegativeBubbles = flag2;
      chart.ChartFormat.SizeRepresents = excelBubbleSize;
      chart.ChartFormat.IsVaryColor = flag1;
    }
    reader.Read();
  }

  private void ParseSurfaceChart(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    Dictionary<int, int> dictSeriesAxis,
    out ExcelChartType chartType)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    bool is3D;
    if (reader.LocalName == "surfaceChart")
    {
      is3D = false;
    }
    else
    {
      if (!(reader.LocalName == "surface3DChart"))
        throw new XmlException("Unexpected xml tag.");
      is3D = true;
    }
    reader.Read();
    List<ChartSerieImpl> lstSeries = new List<ChartSerieImpl>();
    this.ParseSurfaceCommon(reader, chart, is3D, relations, lstSeries, out chartType);
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "axId")
        this.ParseAxisId(reader, lstSeries, dictSeriesAxis);
      else if (reader.LocalName == "extLst")
        this.ParseFilteredSeries(reader, chart, relations, is3D, chart.ChartType, false);
      else
        reader.Skip();
    }
    reader.Skip();
  }

  private void ParseSurfaceCommon(
    XmlReader reader,
    ChartImpl chart,
    bool is3D,
    RelationCollection relations,
    List<ChartSerieImpl> lstSeries,
    out ExcelChartType chartType)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    ExcelChartType excelChartType = ExcelChartType.Surface_3D;
    bool flag1 = true;
    bool bWireframe = false;
    bool flag2 = false;
    chartType = ExcelChartType.Surface_3D;
    while (reader.NodeType != XmlNodeType.EndElement && flag1)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "wireframe":
            bWireframe = ChartParserCommon.ParseBoolValueTag(reader);
            continue;
          case "ser":
            excelChartType = this.GetSurfaceSeriesType(bWireframe, is3D);
            ChartSerieImpl surfaceSeries = this.ParseSurfaceSeries(reader, chart, excelChartType, relations);
            lstSeries.Add(surfaceSeries);
            flag2 = true;
            continue;
          case "bandFmts":
            this.ParseBandFormats(reader, chart);
            continue;
          case "extLst":
            this.ParseFilteredSeries(reader, chart, relations, true, excelChartType, false);
            flag2 = true;
            continue;
          default:
            flag1 = false;
            continue;
        }
      }
      else
        reader.Skip();
    }
    if (chart.HasPivotSource)
    {
      chart.PivotChartType = chart.ChartType == ExcelChartType.Combination_Chart ? ExcelChartType.Combination_Chart : this.GetSurfaceSeriesType(bWireframe, is3D);
    }
    else
    {
      if (flag2)
        return;
      chartType = this.GetSurfaceSeriesType(bWireframe, is3D);
    }
  }

  private void ParseBandFormats(XmlReader reader, ChartImpl chart)
  {
    chart.PreservedBandFormats = ShapeParser.ReadNodeAsStream(reader);
  }

  private ExcelChartType GetSurfaceSeriesType(bool bWireframe, bool is3D)
  {
    return !bWireframe ? (is3D ? ExcelChartType.Surface_3D : ExcelChartType.Surface_Contour) : (is3D ? ExcelChartType.Surface_NoColor_3D : ExcelChartType.Surface_NoColor_Contour);
  }

  private void ParseRadarChart(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    Dictionary<int, int> dictSeriesAxis,
    Excel2007Parser parser,
    out ExcelChartType chartType)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "radarChart")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    ExcelChartType excelChartType = ExcelChartType.Radar;
    bool flag = false;
    List<ChartSerieImpl> lstSeries = new List<ChartSerieImpl>();
    bool secondary = false;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "radarStyle":
            string valueTag = ChartParserCommon.ParseValueTag(reader);
            excelChartType = (ExcelChartType) Enum.Parse(typeof (Excel2007RadarStyle), valueTag, false);
            chart.RadarStyle = valueTag;
            continue;
          case "varyColors":
            flag = ChartParserCommon.ParseBoolValueTag(reader);
            continue;
          case "ser":
            if (lstSeries.Count == 1 && lstSeries[0].SerieType != excelChartType)
            {
              excelChartType = lstSeries[0].SerieType;
              chart.m_bIsRadarTypeChanged = true;
            }
            ChartSerieImpl radarSeries = this.ParseRadarSeries(reader, chart, excelChartType, relations, parser);
            lstSeries.Add(radarSeries);
            if (flag)
            {
              radarSeries.SerieFormat.CommonSerieOptions.IsVaryColor = true;
              continue;
            }
            continue;
          case "axId":
            if (this.ParseAxisId(reader, lstSeries, dictSeriesAxis))
            {
              secondary = true;
              continue;
            }
            continue;
          case "extLst":
            this.ParseFilteredSeries(reader, chart, relations, false, excelChartType, secondary);
            continue;
          case "dLbls":
            if (!reader.IsEmptyElement && lstSeries.Count > 0)
            {
              this.ParseDataLabels(reader, chart, relations, lstSeries[0].Number);
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
    if (chart.HasPivotSource)
      chart.PivotChartType = chart.ChartType == ExcelChartType.Combination_Chart ? ExcelChartType.Combination_Chart : excelChartType;
    chartType = excelChartType;
    if (lstSeries.Count == 0 && chart.ChartFormat != (ChartFormatImpl) null)
      chart.ChartFormat.IsVaryColor = flag;
    if (chart.m_bIsRadarTypeChanged)
      chart.m_bIsRadarTypeChanged = false;
    reader.Read();
  }

  private void ParseScatterChart(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    Dictionary<int, int> dictSeriesAxis,
    Excel2007Parser parser,
    out ExcelChartType chartType)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "scatterChart")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    ExcelChartType excelChartType = ExcelChartType.Scatter_Markers;
    bool flag = true;
    bool secondary = false;
    ChartSerieImpl series = (ChartSerieImpl) null;
    List<ChartSerieImpl> lstSeries = new List<ChartSerieImpl>();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "scatterStyle":
            excelChartType = (ExcelChartType) Enum.Parse(typeof (Excel2007ScatterStyle), ChartParserCommon.ParseValueTag(reader), false);
            continue;
          case "varyColors":
            flag = ChartParserCommon.ParseBoolValueTag(reader);
            continue;
          case "ser":
            series = this.ParseScatterSeries(reader, chart, excelChartType, relations, parser);
            lstSeries.Add(series);
            if (flag)
            {
              series.SerieFormat.CommonSerieOptions.IsVaryColor = true;
              continue;
            }
            continue;
          case "upDownBars":
            this.ParseUpDownBars(reader, chart, series, relations);
            continue;
          case "axId":
            if (this.ParseAxisId(reader, lstSeries, dictSeriesAxis))
            {
              secondary = true;
              continue;
            }
            continue;
          case "extLst":
            this.ParseFilteredSeries(reader, chart, relations, false, excelChartType, secondary);
            continue;
          case "dLbls":
            if (!reader.IsEmptyElement && lstSeries.Count > 0)
            {
              this.ParseDataLabels(reader, chart, relations, lstSeries[0].Number);
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
    chartType = excelChartType;
    if (lstSeries.Count == 0 && chart.ChartFormat != (ChartFormatImpl) null)
      chart.ChartFormat.IsVaryColor = flag;
    reader.Read();
  }

  private void ParsePieChart(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    Dictionary<int, int> dictSeriesAxis,
    out ExcelChartType chartType)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "pieChart")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    List<ChartSerieImpl> lstSeries = new List<ChartSerieImpl>();
    ChartSerieImpl pieCommon = this.ParsePieCommon(reader, chart, ExcelChartType.Pie, relations, lstSeries);
    chartType = ExcelChartType.Pie;
    int num = 0;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element && pieCommon != null)
      {
        switch (reader.LocalName)
        {
          case "firstSliceAng":
            if (reader.MoveToAttribute("val"))
              num = ChartParserCommon.ParseIntValueTag(reader);
            else
              reader.Skip();
            if (pieCommon != null)
            {
              pieCommon.SerieFormat.CommonSerieOptions.FirstSliceAngle = num;
              continue;
            }
            continue;
          case "axId":
            this.ParseAxisId(reader, lstSeries, dictSeriesAxis);
            continue;
          case "extLst":
            this.ParseFilteredSeries(reader, chart, relations, false, pieCommon.SerieType, false);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    if (!chart.HasPivotSource && pieCommon == null && chart.ChartFormat != (ChartFormatImpl) null)
    {
      if (chart.ChartFormat.FormatRecordType != TBIFFRecord.ChartPie)
        chart.ChartFormat.ChangeSerieType(chartType, false);
      chart.PrimaryFormats[0].FirstSliceAngle = num;
    }
    reader.Read();
  }

  private void ParsePie3DChart(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    Dictionary<int, int> dictSeriesAxis,
    out ExcelChartType chartType)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "pie3DChart")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    List<ChartSerieImpl> lstSeries = new List<ChartSerieImpl>();
    chartType = ExcelChartType.Pie_3D;
    this.ParsePieCommon(reader, chart, ExcelChartType.Pie_3D, relations, lstSeries);
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "axId")
        this.ParseAxisId(reader, lstSeries, dictSeriesAxis);
      else if (reader.LocalName == "extLst")
        this.ParseFilteredSeries(reader, chart, relations, true, ExcelChartType.Pie_3D, false);
      else
        reader.Skip();
    }
    reader.Read();
  }

  private void ParseOfPieChart(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    Dictionary<int, int> dictSeriesAxis,
    out ExcelChartType chartType)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "ofPieChart")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    ExcelChartType excelChartType = ExcelChartType.PieOfPie;
    ChartSerieImpl chartSerieImpl = (ChartSerieImpl) null;
    List<ChartSerieImpl> lstSeries = new List<ChartSerieImpl>();
    int? nullable = new int?();
    Excel2007SplitType excel2007SplitType = Excel2007SplitType.pos;
    int num1 = 0;
    int num2 = 0;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "ofPieType":
            excelChartType = ChartParserCommon.ParseValueTag(reader) == "pie" ? ExcelChartType.PieOfPie : ExcelChartType.Pie_Bar;
            continue;
          case "varyColors":
          case "ser":
          case "dLbls":
            chartSerieImpl = this.ParsePieCommon(reader, chart, excelChartType, relations, lstSeries);
            continue;
          case "gapWidth":
            nullable = new int?(ChartParserCommon.ParseIntValueTag(reader));
            continue;
          case "splitType":
            excel2007SplitType = (Excel2007SplitType) Enum.Parse(typeof (Excel2007SplitType), ChartParserCommon.ParseValueTag(reader), false);
            if (chartSerieImpl != null)
            {
              chartSerieImpl.SerieFormat.CommonSerieOptions.SplitType = (ExcelSplitType) excel2007SplitType;
              continue;
            }
            continue;
          case "splitPos":
            num1 = ChartParserCommon.ParseIntValueTag(reader);
            if (chartSerieImpl != null)
            {
              chartSerieImpl.SerieFormat.CommonSerieOptions.SplitValue = num1;
              continue;
            }
            continue;
          case "secondPieSize":
            num2 = ChartParserCommon.ParseIntValueTag(reader);
            if (chartSerieImpl != null)
            {
              chartSerieImpl.SerieFormat.CommonSerieOptions.PieSecondSize = num2;
              continue;
            }
            continue;
          case "axId":
            this.ParseAxisId(reader, lstSeries, dictSeriesAxis);
            continue;
          case "serLines":
            if (!reader.IsEmptyElement)
            {
              reader.Read();
              reader.Read();
              while (reader.NodeType != XmlNodeType.EndElement)
              {
                if (reader.NodeType == XmlNodeType.Element)
                {
                  switch (reader.LocalName)
                  {
                    case "ln":
                      if (chartSerieImpl != null)
                      {
                        ChartBorderImpl pieSeriesLine = chartSerieImpl.SerieFormat.CommonSerieOptions.PieSeriesLine as ChartBorderImpl;
                        ChartParserCommon.ParseLineProperties(reader, pieSeriesLine, chartSerieImpl.ParentBook.DataHolder.Parser);
                        continue;
                      }
                      if (!chart.HasPivotSource && chart.ChartFormat != (ChartFormatImpl) null)
                      {
                        if (chart.ChartFormat.FormatRecordType != TBIFFRecord.ChartBoppop)
                          chart.ChartFormat.ChangeSerieType(excelChartType, false);
                        ChartBorderImpl pieSeriesLine = chart.ChartFormat.PieSeriesLine as ChartBorderImpl;
                        ChartParserCommon.ParseLineProperties(reader, pieSeriesLine, chart.ParentWorkbook.DataHolder.Parser);
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
              if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "spPr")
                reader.Read();
              reader.Read();
              continue;
            }
            reader.Skip();
            continue;
          case "extLst":
            this.ParseFilteredSeries(reader, chart, relations, false, chartSerieImpl.SerieType, false);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    if (nullable.HasValue && chartSerieImpl != null)
      chartSerieImpl.SerieFormat.CommonSerieOptions.GapWidth = nullable.Value;
    else if (!chart.HasPivotSource && chart.ChartFormat != (ChartFormatImpl) null)
    {
      if (chart.ChartFormat.FormatRecordType != TBIFFRecord.ChartBoppop)
        chart.ChartFormat.ChangeSerieType(excelChartType, false);
      chart.ChartFormat.GapWidth = nullable.Value;
      chart.ChartFormat.SplitValue = num1;
      chart.ChartFormat.PieSecondSize = num2;
      chart.ChartFormat.SplitType = (ExcelSplitType) excel2007SplitType;
    }
    if (chart.HasPivotSource)
      chart.PivotChartType = chart.ChartType == ExcelChartType.Combination_Chart ? ExcelChartType.Combination_Chart : excelChartType;
    chartType = excelChartType;
    reader.Read();
  }

  private void ParseStockChart(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    Dictionary<int, int> dictSeriesAxis,
    Excel2007Parser parser)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "stockChart")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    ChartSerieImpl series = (ChartSerieImpl) null;
    List<ChartSerieImpl> lstSeries = new List<ChartSerieImpl>();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "ser":
            series = this.ParseLineSeries(reader, chart, ExcelChartType.Line, relations, parser);
            lstSeries.Add(series);
            continue;
          case "hiLowLines":
          case "dropLines":
            this.ParseLines(reader, chart, series, reader.LocalName);
            continue;
          case "upDownBars":
            this.ParseUpDownBars(reader, chart, series, relations);
            continue;
          case "axId":
            this.ParseAxisId(reader, lstSeries, dictSeriesAxis);
            continue;
          case "extLst":
            this.ParseFilteredSeries(reader, chart, relations, true, series.SerieType, false);
            continue;
          case "dLbls":
            if (!reader.IsEmptyElement && series != null)
            {
              this.ParseDataLabels(reader, chart, relations, series.Number);
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
    ChartFormatImpl chartFormatImpl = series != null ? chart.PrimaryFormats[series.ChartGroup] : (chart.PrimaryFormats.ContainsIndex(0) ? chart.PrimaryFormats[0] : (ChartFormatImpl) null);
    if (chartFormatImpl != (ChartFormatImpl) null)
      chartFormatImpl.InitializeStockFormat();
    reader.Read();
  }

  private void ParseLines(
    XmlReader reader,
    ChartImpl chart,
    ChartSerieImpl series,
    string lineStyle)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    ChartFormatImpl parent = series == null ? ((series.SerieFormat.CommonSerieOptions as ChartFormatCollection).ContainsIndex(0) ? (series.SerieFormat.CommonSerieOptions as ChartFormatCollection)[0] : (ChartFormatImpl) null) : (ChartFormatImpl) series.SerieFormat.CommonSerieOptions;
    if (parent != (ChartFormatImpl) null)
    {
      switch (lineStyle)
      {
        case "hiLowLines":
          parent.HasHighLowLines = true;
          break;
        case "dropLines":
          parent.HasDropLines = true;
          break;
        case "serLines":
          parent.HasSeriesLines = true;
          break;
      }
      if (!reader.IsEmptyElement)
      {
        reader.Read();
        reader.Read();
        while (reader.NodeType != XmlNodeType.EndElement)
        {
          if (reader.NodeType == XmlNodeType.Element)
          {
            switch (reader.LocalName)
            {
              case "ln":
                ChartBorderImpl border = new ChartBorderImpl(chart.Application, (object) parent);
                ChartParserCommon.ParseLineProperties(reader, border, series.ParentBook.DataHolder.Parser);
                switch (lineStyle)
                {
                  case "hiLowLines":
                    parent.HighLowLines = (IChartBorder) border;
                    continue;
                  case "dropLines":
                    parent.DropLines = (IChartBorder) border;
                    continue;
                  case "serLines":
                    parent.PieSeriesLine = (IChartBorder) border;
                    continue;
                  default:
                    continue;
                }
              default:
                reader.Skip();
                continue;
            }
          }
          else
            reader.Skip();
        }
        if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "spPr")
          reader.Read();
        reader.Read();
      }
      else
        reader.Skip();
    }
    else
      reader.Skip();
  }

  private void ParseDoughnutChart(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    Dictionary<int, int> dictSeriesAxis,
    out ExcelChartType chartType)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "doughnutChart")
      throw new XmlException("Unexpected xml tag.");
    chartType = ExcelChartType.Doughnut;
    reader.Read();
    List<ChartSerieImpl> lstSeries = new List<ChartSerieImpl>();
    IChartFormat chartFormat = (IChartFormat) null;
    ChartSerieImpl pieCommon = this.ParsePieCommon(reader, chart, ExcelChartType.Doughnut, relations, lstSeries);
    if (pieCommon == null && !chart.HasPivotSource)
    {
      chart.ChartType = ExcelChartType.Doughnut;
      chart.PrimaryFormats[0].ChangeSerieType(chartType, false);
    }
    if (reader.NodeType != XmlNodeType.EndElement)
      chartFormat = pieCommon != null ? pieCommon.SerieFormat.CommonSerieOptions : (chart.ChartFormat != (ChartFormatImpl) null ? (IChartFormat) chart.ChartFormat : (IChartFormat) null);
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element && chartFormat != null)
      {
        switch (reader.LocalName)
        {
          case "firstSliceAng":
            if (reader.MoveToAttribute("val"))
            {
              chartFormat.FirstSliceAngle = ChartParserCommon.ParseIntValueTag(reader);
              continue;
            }
            chartFormat.FirstSliceAngle = 0;
            reader.Skip();
            continue;
          case "holeSize":
            chartFormat.DoughnutHoleSize = ChartParserCommon.ParseIntValueTag(reader);
            continue;
          case "axId":
            this.ParseAxisId(reader, lstSeries, dictSeriesAxis);
            continue;
          case "extLst":
            this.ParseFilteredSeries(reader, chart, relations, true, pieCommon.SerieType, false);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    if (chart.HasPivotSource)
      chart.PivotChartType = chart.ChartType == ExcelChartType.Combination_Chart ? ExcelChartType.Combination_Chart : ExcelChartType.Doughnut;
    reader.Read();
  }

  private ChartSerieImpl ParsePieCommon(
    XmlReader reader,
    ChartImpl chart,
    ExcelChartType seriesType,
    RelationCollection relations,
    List<ChartSerieImpl> lstSeries)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    bool flag1 = true;
    ChartSerieImpl pieCommon = (ChartSerieImpl) null;
    bool flag2 = false;
    while (reader.NodeType != XmlNodeType.EndElement && flag1)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "varyColors":
            flag2 = ChartParserCommon.ParseBoolValueTag(reader);
            continue;
          case "ser":
            pieCommon = this.ParsePieSeries(reader, chart, seriesType, relations);
            pieCommon.SerieFormat.CommonSerieOptions.IsVaryColor = flag2;
            lstSeries.Add(pieCommon);
            continue;
          case "dLbls":
            if (pieCommon != null && !reader.IsEmptyElement)
            {
              this.ParseDataLabels(reader, chart, relations, pieCommon.Number);
              continue;
            }
            reader.Skip();
            continue;
          default:
            flag1 = false;
            continue;
        }
      }
      else
        reader.Skip();
    }
    if (chart.HasPivotSource)
      chart.PivotChartType = chart.ChartType == ExcelChartType.Combination_Chart ? ExcelChartType.Combination_Chart : seriesType;
    else if (pieCommon == null && chart.ChartFormat != (ChartFormatImpl) null)
      chart.PrimaryFormats[0].IsVaryColor = flag2;
    return pieCommon;
  }

  internal void ParseDataLabels(
    XmlReader reader,
    ChartSerieImpl series,
    RelationCollection relations,
    bool isChartExSeries)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    List<int> intList = (List<int>) null;
    if (isChartExSeries)
    {
      if (reader.LocalName != "dataLabels")
        throw new XmlException("Unexpected xml tag.");
      if (reader.MoveToAttribute("pos"))
        series.DataPoints.DefaultDataPoint.DataLabels.Position = (ExcelDataLabelPosition) Enum.Parse(typeof (Excel2007DataLabelPos), reader.Value, false);
      series.DataPoints.DefaultDataPoint.DataLabels.FrameFormat.Interior.UseAutomaticFormat = true;
      series.DataPoints.DefaultDataPoint.DataLabels.FrameFormat.Border.AutoFormat = true;
    }
    else if (reader.LocalName != "dLbls")
      throw new XmlException("Unexpected xml tag.");
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "dLbl":
            case "dataLabel":
              this.ParseDataLabel(reader, series, relations, isChartExSeries);
              continue;
            case "dataLabelHidden":
              if (reader.MoveToAttribute("idx"))
              {
                if (intList == null)
                  intList = new List<int>();
                intList.Add(XmlConvertExtension.ToInt32(reader.Value));
              }
              reader.Skip();
              continue;
            default:
              IChartDataLabels dataLabels = series.DataPoints.DefaultDataPoint.DataLabels;
              FileDataHolder dataHolder = series.ParentBook.DataHolder;
              Excel2007Parser parser = dataHolder.Parser;
              if (isChartExSeries)
                dataLabels.IsLegendKey = false;
              this.ParseDataLabelSettings(reader, dataLabels, parser, dataHolder, relations, isChartExSeries);
              if (series.ParentBook.Version != ExcelVersion.Excel2007)
              {
                (dataLabels as ChartDataLabelsImpl).IsEnableDataLabels = true;
                continue;
              }
              continue;
          }
        }
        else
          reader.Skip();
      }
    }
    if (isChartExSeries && intList != null)
    {
      foreach (int index in intList)
        (series.DataPoints[index].DataLabels as ChartDataLabelsImpl).IsDelete = true;
      intList.Clear();
    }
    reader.Read();
  }

  private void ParseDataLabel(
    XmlReader reader,
    ChartSerieImpl series,
    RelationCollection relations,
    bool isChartExSeries)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    IChartDataLabels chartDataLabels = (IChartDataLabels) null;
    if (isChartExSeries)
    {
      if (reader.LocalName != "dataLabel")
        throw new XmlException("Unexpeced xml tag.");
      if (reader.MoveToAttribute("idx"))
      {
        int int32 = XmlConvertExtension.ToInt32(reader.Value);
        chartDataLabels = series.DataPoints[int32].DataLabels;
        (chartDataLabels as ChartDataLabelsImpl).ShowTextProperties = false;
      }
      if (chartDataLabels != null && reader.MoveToAttribute("pos"))
        chartDataLabels.Position = (ExcelDataLabelPosition) Enum.Parse(typeof (Excel2007DataLabelPos), reader.Value, false);
    }
    else if (reader.LocalName != "dLbl")
      throw new XmlException("Unexpeced xml tag.");
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "idx":
            int intValueTag = ChartParserCommon.ParseIntValueTag(reader);
            chartDataLabels = series.DataPoints[intValueTag].DataLabels;
            (chartDataLabels as ChartDataLabelsImpl).ShowTextProperties = false;
            continue;
          case "layout":
            (chartDataLabels as ChartDataLabelsImpl).Layout = (IChartLayout) new ChartLayoutImpl(this.m_book.Application, (object) (chartDataLabels as ChartDataLabelsImpl), series.Parent);
            ChartParserCommon.ParseChartLayout(reader, (chartDataLabels as ChartDataLabelsImpl).Layout);
            continue;
          case "delete":
            bool boolValueTag = ChartParserCommon.ParseBoolValueTag(reader);
            (chartDataLabels as ChartDataLabelsImpl).IsDelete = boolValueTag;
            continue;
          default:
            FileDataHolder dataHolder = series.ParentBook.DataHolder;
            Excel2007Parser parser = dataHolder.Parser;
            this.ParseDataLabelSettings(reader, chartDataLabels, parser, dataHolder, relations, isChartExSeries);
            continue;
        }
      }
      else
        reader.Skip();
    }
    reader.Read();
  }

  private void ParseDataLabelSettings(
    XmlReader reader,
    IChartDataLabels dataLabels,
    Excel2007Parser parser,
    FileDataHolder holder,
    RelationCollection relations,
    bool isChartExSeries)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (dataLabels == null)
      throw new ArgumentNullException(nameof (dataLabels));
    (dataLabels as IInternalChartTextArea).Size = 10.0;
    ChartDataLabelsImpl dataLabels1 = dataLabels as ChartDataLabelsImpl;
    dataLabels1.ShowTextProperties = false;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (reader.LocalName == "dataLabel" || reader.LocalName == "dLbl" || reader.LocalName == "dataLabelHidden")
          break;
        switch (reader.LocalName)
        {
          case "dLblPos":
            if (reader.MoveToAttribute("val"))
            {
              Excel2007DataLabelPos excel2007DataLabelPos = (Excel2007DataLabelPos) Enum.Parse(typeof (Excel2007DataLabelPos), reader.Value, false);
              dataLabels.Position = (ExcelDataLabelPosition) excel2007DataLabelPos;
              reader.Read();
              continue;
            }
            reader.Skip();
            continue;
          case "showLegendKey":
            if (!dataLabels1.m_bHasLegendKeyOption)
            {
              dataLabels.IsLegendKey = ChartParserCommon.ParseBoolValueTag(reader);
              continue;
            }
            reader.Skip();
            continue;
          case "showLeaderLines":
            bool boolValueTag1 = ChartParserCommon.ParseBoolValueTag(reader);
            dataLabels.ShowLeaderLines = boolValueTag1;
            continue;
          case "extLst":
            this.ParseDataLabelsExtensionList(reader, dataLabels1, holder, relations);
            continue;
          case "showVal":
            if (!dataLabels1.m_bHasValueOption)
            {
              dataLabels.IsValue = ChartParserCommon.ParseBoolValueTag(reader);
              continue;
            }
            reader.Skip();
            continue;
          case "showCatName":
            if (!dataLabels1.m_bHasCategoryOption)
            {
              dataLabels.IsCategoryName = ChartParserCommon.ParseBoolValueTag(reader);
              continue;
            }
            reader.Skip();
            continue;
          case "showPercent":
            if (!dataLabels1.m_bHasPercentageOption)
            {
              dataLabels.IsPercentage = ChartParserCommon.ParseBoolValueTag(reader);
              continue;
            }
            reader.Skip();
            continue;
          case "showBubbleSize":
            if (!dataLabels1.m_bHasBubbleSizeOption)
            {
              dataLabels.IsBubbleSize = ChartParserCommon.ParseBoolValueTag(reader);
              continue;
            }
            reader.Skip();
            continue;
          case "showSerName":
            if (!dataLabels1.m_bHasSeriesOption)
            {
              dataLabels.IsSeriesName = ChartParserCommon.ParseBoolValueTag(reader);
              continue;
            }
            reader.Skip();
            continue;
          case "visibility":
            this.ParseChartDataLabelVisibility(reader, dataLabels1);
            continue;
          case "separator":
            dataLabels.Delimiter = reader.ReadElementContentAsString();
            continue;
          case "txPr":
            IInternalChartTextArea textFormatting = dataLabels as IInternalChartTextArea;
            this.ParseDefaultTextFormatting(reader, textFormatting, parser);
            dataLabels1.ShowTextProperties = true;
            IChartDataPoint parent = (IChartDataPoint) dataLabels1.Parent;
            if (parent.IsDefault && (parent.DataLabels as ChartDataLabelsImpl).TextArea.Text != null)
            {
              ChartTextAreaImpl textArea = (parent.DataLabels as ChartDataLabelsImpl).TextArea;
              IEnumerator enumerator = ((ChartDataPointsCollection) parent.Parent).GetEnumerator();
              try
              {
                while (enumerator.MoveNext())
                {
                  ChartDataPointImpl current = (ChartDataPointImpl) enumerator.Current;
                  if (current.HasDataLabels && (current.DataLabels as ChartDataLabelsImpl).ParagraphType != ChartParagraphType.CustomDefault)
                    (current.DataLabels as ChartDataLabelsImpl).TextArea = textArea;
                }
                continue;
              }
              finally
              {
                if (enumerator is IDisposable disposable)
                  disposable.Dispose();
              }
            }
            else
              continue;
          case "spPr":
            IChartFillObjectGetter objectGetter = (IChartFillObjectGetter) new ChartFillObjectGetterAny(dataLabels.FrameFormat.Border as ChartBorderImpl, dataLabels.FrameFormat.Interior as ChartInteriorImpl, dataLabels.FrameFormat.Fill as IInternalFill, dataLabels.FrameFormat.Shadow as ShadowImpl, dataLabels.FrameFormat.ThreeD as ThreeDFormatImpl);
            ChartParserCommon.ParseShapeProperties(reader, objectGetter, holder, relations);
            continue;
          case "numFmt":
            ChartParserCommon.ParseNumberFormat(reader, dataLabels);
            continue;
          case "delete":
            bool boolValueTag2 = ChartParserCommon.ParseBoolValueTag(reader);
            dataLabels1.IsDelete = boolValueTag2;
            continue;
          default:
            if (isChartExSeries)
            {
              reader.Skip();
              continue;
            }
            ChartParserCommon.ParseTextAreaTag(reader, dataLabels as IInternalChartTextArea, relations, holder, new float?(10f));
            continue;
        }
      }
      else
        reader.Skip();
    }
  }

  internal void ParseDataLabelsExtensionList(
    XmlReader reader,
    ChartDataLabelsImpl dataLabels,
    FileDataHolder holder,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (dataLabels == null)
      throw new ArgumentNullException(nameof (dataLabels));
    if (reader.LocalName != "extLst")
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
            case "ext":
              this.ParseDataLabelsExtension(reader, dataLabels, holder, relations);
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

  private void ParseDataLabelsExtension(
    XmlReader reader,
    ChartDataLabelsImpl dataLabels,
    FileDataHolder holder,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (dataLabels == null)
      throw new ArgumentNullException("table");
    if (reader.LocalName != "ext")
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
            case "showLeaderLines":
              dataLabels.Serie.HasLeaderLines = ChartParserCommon.ParseBoolValueTag(reader);
              continue;
            case "leaderLines":
              this.ParseLeaderLines(reader, dataLabels, holder, relations);
              continue;
            case "showDataLabelsRange":
              dataLabels.IsValueFromCells = ChartParserCommon.ParseBoolValueTag(reader);
              continue;
            case "layout":
              ChartParserCommon.ParseChartLayout(reader, dataLabels.Layout);
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

  private void ParseLeaderLines(
    XmlReader reader,
    ChartDataLabelsImpl dataLabels,
    FileDataHolder holder,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (dataLabels == null)
      throw new ArgumentNullException("table");
    if (reader.LocalName != "leaderLines")
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
            case "spPr":
              IChartFillObjectGetter objectGetter = (IChartFillObjectGetter) new ChartFillObjectGetterAny(dataLabels.Serie.LeaderLines as ChartBorderImpl, (ChartInteriorImpl) null, (IInternalFill) null, (ShadowImpl) null, (ThreeDFormatImpl) null);
              ChartParserCommon.ParseShapeProperties(reader, objectGetter, holder, relations);
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

  internal virtual void ParseChartDataLabelVisibility(
    XmlReader reader,
    ChartDataLabelsImpl dataLabels)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (dataLabels == null)
      throw new ArgumentNullException("data labels");
    if (reader.LocalName != "visibility")
      throw new XmlException("Unexpeced xml tag.");
    reader.Skip();
  }

  private ChartSerieImpl ParseBarSeries(
    XmlReader reader,
    ChartImpl chart,
    ExcelChartType seriesType,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    ChartSerieImpl barSeries = (ChartSerieImpl) chart.Series.Add(seriesType);
    this.ParseSeriesCommonWithoutEnd(reader, barSeries, relations);
    object[] values;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "dLbls":
            this.ParseDataLabels(reader, barSeries, relations, false);
            continue;
          case "trendline":
            this.ParseTrendlines(reader, barSeries, relations, chart.DataHolder.ParentHolder.Parser);
            continue;
          case "errBars":
            this.ParseErrorBars(reader, barSeries, relations);
            continue;
          case "cat":
            barSeries.CategoryLabels = this.ParseSeriesValues(reader, barSeries, out values, false);
            if (values != null)
            {
              barSeries.EnteredDirectlyCategoryLabels = values;
              continue;
            }
            continue;
          case "val":
            barSeries.Values = this.ParseSeriesValues(reader, barSeries, out values, true);
            if (values != null && barSeries.Values == null)
            {
              barSeries.EnteredDirectlyValues = values;
              continue;
            }
            continue;
          case "dPt":
            this.ParseDataPoint(reader, barSeries, relations);
            continue;
          case "extLst":
            this.ParseFilteredSeriesOrCategoryName(reader, barSeries);
            continue;
          case "shape":
            this.ParseBarShape(reader, barSeries);
            (barSeries.SerieFormat as ChartSerieDataFormatImpl).HasBarShape = true;
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
    return barSeries;
  }

  private void ParseFilteredSeriesOrCategoryName(XmlReader reader, ChartSerieImpl series)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "extLst")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "ext":
            this.ParseExtension(reader, series);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    if (!(reader.LocalName != "ser"))
      return;
    reader.Read();
  }

  private void ParseExtension(XmlReader reader, ChartSerieImpl series)
  {
    if (reader == null)
      throw new ArgumentException(nameof (reader));
    if (series == null)
      throw new ArgumentException("Series");
    while (reader.LocalName != "extLst")
    {
      if (reader.LocalName == "ext" && reader.NodeType != XmlNodeType.EndElement)
      {
        reader.Read();
        if (reader.LocalName == "invertSolidFillFmt")
          series.m_invertFillFormatStream = ShapeParser.ReadNodeAsStream(reader);
        else if (reader.LocalName == "filteredSeriesTitle")
        {
          reader.Read();
          this.ParseSeriesText(reader, series);
          series.ParentChart.SeriesNameLevel = ExcelSeriesNameLevel.SeriesNameLevelNone;
          reader.Read();
        }
        else if (reader.LocalName == "filteredCategoryTitle")
        {
          reader.Read();
          object[] values;
          series.CategoryLabels = this.ParseSeriesValues(reader, series, out values, false);
          if (values != null)
            series.EnteredDirectlyCategoryLabels = values;
          series.ParentChart.CategoryLabelLevel = ExcelCategoriesLabelLevel.CategoriesLabelLevelNone;
          reader.Read();
        }
        else if (reader.LocalName == "datalabelsRange")
          this.ParseDatalabelsRange(reader, series);
      }
      else
        reader.Read();
    }
  }

  private void ParseDatalabelsRange(XmlReader reader, ChartSerieImpl series)
  {
    if (reader == null)
      throw new ArgumentException("Xmlreader");
    if (series == null)
      throw new ArgumentException("Series");
    string formula = (string) null;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "f":
            formula = reader.ReadElementContentAsString();
            continue;
          case "dlblRangeCache":
            this.ParseDatalabelRangeCache(reader, series);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
    }
    WorkbookImpl parentBook = series.ParentBook;
    if (parentBook != null && !string.IsNullOrEmpty(formula))
      series.DataPoints.DefaultDataPoint.DataLabels.ValueFromCellsRange = ChartParser.GetRange(parentBook, formula);
    reader.Read();
  }

  private void ParseDatalabelRangeCache(XmlReader reader, ChartSerieImpl series)
  {
    if (reader == null)
      throw new ArgumentException("XmlReader");
    reader.Read();
    Dictionary<int, object> list = new Dictionary<int, object>();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "pt":
            this.AddNumericPoint(reader, list);
            continue;
          case "ptCount":
            if (reader.MoveToAttribute("val"))
              XmlConvertExtension.ToInt32(reader.Value);
            reader.Read();
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
    }
    if (series != null && list != null)
      series.DataLabelCellsValues = list;
    reader.Read();
  }

  private ChartSerieImpl ParseSurfaceSeries(
    XmlReader reader,
    ChartImpl chart,
    ExcelChartType seriesType,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    ChartSerieImpl series = (ChartSerieImpl) chart.Series.Add(seriesType);
    this.ParseSeriesCommonWithoutEnd(reader, series, relations);
    object[] values;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "cat":
            series.CategoryLabels = this.ParseSeriesValues(reader, series, out values, false);
            if (values != null)
            {
              series.EnteredDirectlyCategoryLabels = values;
              continue;
            }
            continue;
          case "val":
            series.Values = this.ParseSeriesValues(reader, series, out values, true);
            if (values != null && series.Values == null)
            {
              series.EnteredDirectlyValues = values;
              continue;
            }
            continue;
          case "extLst":
            this.ParseFilteredSeriesOrCategoryName(reader, series);
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
    return series;
  }

  private ChartSerieImpl ParsePieSeries(
    XmlReader reader,
    ChartImpl chart,
    ExcelChartType seriesType,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    ChartSerieImpl series = (ChartSerieImpl) chart.Series.Add(seriesType);
    this.ParseSeriesCommonWithoutEnd(reader, series, relations);
    object[] values;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "explosion":
            series.SerieFormat.Percent = ChartParserCommon.ParseIntValueTag(reader);
            continue;
          case "dLbls":
            this.ParseDataLabels(reader, series, relations, false);
            continue;
          case "trendline":
            this.ParseTrendlines(reader, series, relations, chart.DataHolder.ParentHolder.Parser);
            continue;
          case "errBars":
            this.ParseErrorBars(reader, series, relations);
            continue;
          case "cat":
            series.CategoryLabels = this.ParseSeriesValues(reader, series, out values, false);
            if (values != null)
            {
              series.EnteredDirectlyCategoryLabels = values;
              continue;
            }
            continue;
          case "val":
            series.Values = this.ParseSeriesValues(reader, series, out values, true);
            if (values != null && series.Values == null)
            {
              series.EnteredDirectlyValues = values;
              continue;
            }
            continue;
          case "dPt":
            this.ParseDataPoint(reader, series, relations);
            continue;
          case "extLst":
            this.ParseFilteredSeriesOrCategoryName(reader, series);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        Excel2007Parser.SkipWhiteSpaces(reader);
    }
    reader.Read();
    return series;
  }

  private ChartSerieImpl ParseLineSeries(
    XmlReader reader,
    ChartImpl chart,
    ExcelChartType seriesType,
    RelationCollection relations,
    Excel2007Parser parser)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    ChartSerieImpl series = (ChartSerieImpl) chart.Series.Add(seriesType);
    series.SerieFormat.LineProperties.LineWeight = ExcelChartLineWeight.Hairline;
    series.SerieFormat.LineProperties.AutoFormat = true;
    this.ParseSeriesCommonWithoutEnd(reader, series, relations);
    FileDataHolder dataHolder = series.ParentBook.DataHolder;
    object[] values;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "spPr":
            ChartFillObjectGetterAny objectGetter = new ChartFillObjectGetterAny(series.SerieFormat.LineProperties as ChartBorderImpl, (ChartInteriorImpl) null, (IInternalFill) null, series.SerieFormat.Shadow as ShadowImpl, series.SerieFormat.ThreeD as ThreeDFormatImpl);
            ChartParserCommon.ParseShapeProperties(reader, (IChartFillObjectGetter) objectGetter, dataHolder, relations);
            continue;
          case "smooth":
            bool boolValueTag = ChartParserCommon.ParseBoolValueTag(reader);
            ChartSerieDataFormatImpl serieFormat = (ChartSerieDataFormatImpl) series.SerieFormat;
            if (serieFormat != null)
            {
              serieFormat.IsSmoothedLine = boolValueTag;
              continue;
            }
            continue;
          case "marker":
            this.ParseMarker(reader, series, parser);
            continue;
          case "dLbls":
            this.ParseDataLabels(reader, series, relations, false);
            continue;
          case "trendline":
            this.ParseTrendlines(reader, series, relations, chart.DataHolder.ParentHolder.Parser);
            continue;
          case "errBars":
            this.ParseErrorBars(reader, series, relations);
            continue;
          case "cat":
            series.CategoryLabels = this.ParseSeriesValues(reader, series, out values, false);
            if (values != null)
            {
              series.EnteredDirectlyCategoryLabels = values;
              continue;
            }
            continue;
          case "val":
            series.Values = this.ParseSeriesValues(reader, series, out values, true);
            if (values != null && series.Values == null)
            {
              series.EnteredDirectlyValues = values;
              continue;
            }
            continue;
          case "dPt":
            this.ParseDataPoint(reader, series, relations);
            continue;
          case "extLst":
            this.ParseFilteredSeriesOrCategoryName(reader, series);
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
    return series;
  }

  private ChartSerieImpl ParseScatterSeries(
    XmlReader reader,
    ChartImpl chart,
    ExcelChartType seriesType,
    RelationCollection relations,
    Excel2007Parser parser)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    ChartSerieImpl series = (ChartSerieImpl) chart.Series.Add(seriesType);
    series.SerieFormat.LineProperties.LineWeight = ExcelChartLineWeight.Hairline;
    series.SerieFormat.LineProperties.AutoFormat = true;
    this.ParseSeriesCommonWithoutEnd(reader, series, relations);
    object[] values;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "spPr":
            ChartSerieDataFormatImpl serieFormat = (ChartSerieDataFormatImpl) series.SerieFormat;
            FileDataHolder dataHolder = series.ParentBook.DataHolder;
            ChartFillObjectGetter objectGetter = new ChartFillObjectGetter(serieFormat);
            ChartParserCommon.ParseShapeProperties(reader, (IChartFillObjectGetter) objectGetter, dataHolder, relations);
            continue;
          case "marker":
            this.ParseMarker(reader, series, parser);
            continue;
          case "dLbls":
            this.ParseDataLabels(reader, series, relations, false);
            continue;
          case "trendline":
            this.ParseTrendlines(reader, series, relations, parser);
            continue;
          case "errBars":
            this.ParseErrorBars(reader, series, relations);
            continue;
          case "xVal":
            series.CategoryLabels = this.ParseSeriesValues(reader, series, out values, false);
            if (values != null)
            {
              series.EnteredDirectlyCategoryLabels = values;
              continue;
            }
            continue;
          case "yVal":
            series.Values = this.ParseSeriesValues(reader, series, out values, true);
            if (values != null)
            {
              series.EnteredDirectlyValues = values;
              continue;
            }
            continue;
          case "smooth":
            bool boolValueTag = ChartParserCommon.ParseBoolValueTag(reader);
            ((ChartSerieDataFormatImpl) series.SerieFormat).IsSmoothedLine = boolValueTag;
            continue;
          case "dPt":
            this.ParseDataPoint(reader, series, relations);
            continue;
          case "extLst":
            this.ParseFilteredSeriesOrCategoryName(reader, series);
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
    return series;
  }

  private ChartSerieImpl ParseRadarSeries(
    XmlReader reader,
    ChartImpl chart,
    ExcelChartType seriesType,
    RelationCollection relations,
    Excel2007Parser parser)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    ChartSerieImpl series = (ChartSerieImpl) chart.Series.Add(seriesType);
    series.SerieFormat.LineProperties.LineWeight = ExcelChartLineWeight.Hairline;
    series.SerieFormat.LineProperties.AutoFormat = true;
    this.ParseSeriesCommonWithoutEnd(reader, series, relations);
    object[] values;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "marker":
            this.ParseMarker(reader, series, parser);
            continue;
          case "dLbls":
            this.ParseDataLabels(reader, series, relations, false);
            continue;
          case "cat":
            series.CategoryLabels = this.ParseSeriesValues(reader, series, out values, false);
            if (values != null)
            {
              series.EnteredDirectlyCategoryLabels = values;
              continue;
            }
            continue;
          case "val":
            series.Values = this.ParseSeriesValues(reader, series, out values, true);
            if (values != null && series.Values == null)
            {
              series.EnteredDirectlyValues = values;
              continue;
            }
            continue;
          case "dPt":
            this.ParseDataPoint(reader, series, relations);
            continue;
          case "extLst":
            this.ParseFilteredSeriesOrCategoryName(reader, series);
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
    return series;
  }

  private ChartSerieImpl ParseBubbleSeries(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "ser")
      throw new XmlException("Unexpected xml tag.");
    ChartSerieImpl series = (ChartSerieImpl) chart.Series.Add(ExcelChartType.Bubble);
    this.ParseSeriesCommonWithoutEnd(reader, series, relations);
    object[] values;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "dLbls":
            this.ParseDataLabels(reader, series, relations, false);
            continue;
          case "trendline":
            this.ParseTrendlines(reader, series, relations, chart.DataHolder.ParentHolder.Parser);
            continue;
          case "errBars":
            this.ParseErrorBars(reader, series, relations);
            continue;
          case "xVal":
            series.CategoryLabels = this.ParseSeriesValues(reader, series, out values, false);
            if (values != null)
            {
              series.EnteredDirectlyCategoryLabels = values;
              continue;
            }
            continue;
          case "yVal":
            series.Values = this.ParseSeriesValues(reader, series, out values, true);
            if (values != null)
            {
              series.EnteredDirectlyValues = values;
              continue;
            }
            continue;
          case "bubbleSize":
            series.Bubbles = this.ParseSeriesValues(reader, series, out values, false);
            if (values != null)
            {
              series.EnteredDirectlyBubbles = values;
              continue;
            }
            continue;
          case "bubble3D":
            if (ChartParserCommon.ParseBoolValueTag(reader))
            {
              series.SerieFormat.Is3DBubbles = true;
              continue;
            }
            continue;
          case "dPt":
            this.ParseDataPoint(reader, series, relations);
            continue;
          case "extLst":
            this.ParseFilteredSeriesOrCategoryName(reader, series);
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
    return series;
  }

  private ChartSerieImpl ParseAreaSeries(
    XmlReader reader,
    ChartImpl chart,
    ExcelChartType seriesType,
    RelationCollection relations,
    bool isPrimary)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    ExcelChartType serieType = chart != null ? chart.ChartType : throw new ArgumentNullException(nameof (chart));
    if (serieType == seriesType || serieType == ExcelChartType.Combination_Chart)
      serieType = seriesType;
    ChartSerieImpl series = (ChartSerieImpl) chart.Series.Add(serieType);
    if (!isPrimary & series.ParentSeries.Count > 1)
      series.UsePrimaryAxis = isPrimary;
    if (serieType != seriesType)
      series.SerieType = seriesType;
    this.ParseSeriesCommonWithoutEnd(reader, series, relations);
    object[] values;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "dLbls":
            this.ParseDataLabels(reader, series, relations, false);
            continue;
          case "trendline":
            this.ParseTrendlines(reader, series, relations, chart.DataHolder.ParentHolder.Parser);
            continue;
          case "errBars":
            this.ParseErrorBars(reader, series, relations);
            continue;
          case "cat":
            series.CategoryLabels = this.ParseSeriesValues(reader, series, out values, false);
            if (values != null)
            {
              series.EnteredDirectlyCategoryLabels = values;
              continue;
            }
            continue;
          case "val":
            series.Values = this.ParseSeriesValues(reader, series, out values, true);
            if (values != null && series.Values == null)
            {
              series.EnteredDirectlyValues = values;
              continue;
            }
            continue;
          case "dPt":
            this.ParseDataPoint(reader, series, relations);
            continue;
          case "extLst":
            this.ParseFilteredSeriesOrCategoryName(reader, series);
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
    return series;
  }

  private void ParseSeriesCommonWithoutEnd(
    XmlReader reader,
    ChartSerieImpl series,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    if (reader.LocalName != "ser")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    bool flag = true;
    while (reader.NodeType != XmlNodeType.EndElement && flag)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "idx":
            string valueTag1 = ChartParserCommon.ParseValueTag(reader);
            series.Number = int.Parse(valueTag1);
            continue;
          case "order":
            string valueTag2 = ChartParserCommon.ParseValueTag(reader);
            series.Index = int.Parse(valueTag2);
            continue;
          case "tx":
            this.ParseSeriesText(reader, series);
            continue;
          case "spPr":
            this.ParseSeriesProperties(reader, series, relations);
            continue;
          case "invertIfNegative":
            if (reader.MoveToAttribute("val"))
            {
              series.InvertIfNegative = new bool?(XmlConvertExtension.ToBoolean(reader.Value));
              continue;
            }
            continue;
          default:
            flag = false;
            continue;
        }
      }
      else
        reader.Skip();
    }
  }

  private void ParseSeriesText(XmlReader reader, ChartSerieImpl series)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    if (reader.LocalName != "tx")
      throw new XmlException("Unexpected xml tag.");
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (series.IsDefaultName && reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "v":
              series.Name = reader.ReadElementContentAsString();
              continue;
            case "strRef":
              string nameCache = (string) null;
              string stringReference = this.ParseStringReference(reader, ref nameCache);
              if (!this.IsNullOrWhiteSpace(stringReference))
                series.Name = "=" + stringReference;
              series.NameCache = nameCache;
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

  internal bool IsNullOrWhiteSpace(string text)
  {
    if (text == null)
      return true;
    foreach (char c in text)
    {
      if (!char.IsWhiteSpace(c))
        return false;
    }
    return true;
  }

  internal void ParseDataPoint(
    XmlReader reader,
    ChartSerieImpl series,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    if (reader.LocalName != "dPt" && reader.LocalName != "dataPt")
      throw new XmlException();
    if (!reader.IsEmptyElement)
    {
      FileDataHolder dataHolder = series.ParentChart.ParentWorkbook.DataHolder;
      bool flag = this is ChartExParser;
      IChartDataPoint chartDataPoint = (IChartDataPoint) null;
      if (flag && reader.MoveToAttribute("idx"))
      {
        chartDataPoint = series.DataPoints[XmlConvertExtension.ToInt32(reader.Value)];
        (chartDataPoint as ChartDataPointImpl).HasDataPoint = true;
      }
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "idx":
              int index = int.Parse(ChartParserCommon.ParseValueTag(reader));
              chartDataPoint = series.DataPoints[index];
              (chartDataPoint as ChartDataPointImpl).HasDataPoint = true;
              continue;
            case "spPr":
              ChartSerieDataFormatImpl serieDataFormatImpl = chartDataPoint != null ? chartDataPoint.DataFormat as ChartSerieDataFormatImpl : throw new XmlException();
              serieDataFormatImpl.HasLineProperties = true;
              serieDataFormatImpl.HasInterior = true;
              serieDataFormatImpl.IsParsed = true;
              ChartFillObjectGetterAny objectGetter = new ChartFillObjectGetterAny(serieDataFormatImpl.LineProperties as ChartBorderImpl, serieDataFormatImpl.Interior as ChartInteriorImpl, serieDataFormatImpl.Fill as IInternalFill, serieDataFormatImpl.Shadow as ShadowImpl, serieDataFormatImpl.ThreeD as ThreeDFormatImpl);
              ChartParserCommon.ParseShapeProperties(reader, (IChartFillObjectGetter) objectGetter, dataHolder, relations);
              continue;
            case "marker":
              ChartSerieDataFormatImpl dataFormat1 = chartDataPoint.DataFormat as ChartSerieDataFormatImpl;
              this.ParseMarker(reader, (IChartSerieDataFormat) dataFormat1, dataHolder.Parser);
              chartDataPoint.IsDefaultmarkertype = true;
              dataFormat1.IsParsed = true;
              continue;
            case "invertIfNegative":
              if (reader.MoveToAttribute("val"))
              {
                ChartSerieDataFormatImpl dataFormat2 = chartDataPoint.DataFormat as ChartSerieDataFormatImpl;
                if (dataFormat2.IsSupportFill)
                {
                  series.ParentChart.IsParsed = true;
                  (dataFormat2.Fill as ChartFillImpl).InvertIfNegative = XmlConvertExtension.ToBoolean(reader.Value);
                  if (dataFormat2.Interior != null)
                    (dataFormat2.Interior as ChartInteriorImpl).SwapColorsOnNegative = XmlConvertExtension.ToBoolean(reader.Value);
                  series.ParentChart.IsParsed = false;
                  dataFormat2.IsParsed = true;
                  continue;
                }
                continue;
              }
              continue;
            case "bubble3D":
              (chartDataPoint as ChartDataPointImpl).Bubble3D = ChartParserCommon.ParseBoolValueTag(reader);
              (chartDataPoint.DataFormat as ChartSerieDataFormatImpl).IsParsed = true;
              continue;
            case "explosion":
              (chartDataPoint as ChartDataPointImpl).Explosion = ChartParserCommon.ParseIntValueTag(reader);
              (chartDataPoint.DataFormat as ChartSerieDataFormatImpl).IsParsed = true;
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          reader.Read();
      }
    }
    reader.Read();
  }

  private IRange ParseSeriesValues(
    XmlReader reader,
    ChartSerieImpl series,
    out object[] values,
    bool isValueAxis)
  {
    ChartImpl parentChart = series.ParentChart;
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    reader.Read();
    string formula = (string) null;
    string Filteredcategory = (string) null;
    string filteredvalue = (string) null;
    values = (object[]) null;
    bool flag1 = false;
    bool flag2 = false;
    bool flag3 = false;
    string formatCode = (string) null;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "numRef":
            flag1 = true;
            formula = this.ParseNumReference(reader, out values, out filteredvalue, out formatCode);
            if (formula != null)
            {
              if (formula.Split(',').Length > 1)
                series.NumRefFormula = formula;
            }
            if (values != null)
            {
              if (isValueAxis)
              {
                series.EnteredDirectlyValues = values;
                series.FormatCode = formatCode;
              }
              else
              {
                series.EnteredDirectlyCategoryLabels = values;
                series.CategoriesFormatCode = formatCode;
              }
            }
            series.FilteredValue = filteredvalue;
            if (filteredvalue != null)
            {
              isValueAxis = true;
              continue;
            }
            continue;
          case "strRef":
            flag2 = true;
            formula = this.ParseStringReference(reader, out Filteredcategory, out values, parentChart);
            if (formula != null)
            {
              if (formula.Split(',').Length > 1)
                series.StrRefFormula = formula;
            }
            series.FilteredCategory = Filteredcategory;
            continue;
          case "multiLvlStrRef":
            flag3 = true;
            formula = this.ParseMultiLevelStringReference(reader, series);
            if (formula != null)
            {
              series.MulLvlStrRefFormula = formula;
              continue;
            }
            continue;
          case "numLit":
            values = this.ParseDirectlyEnteredValues(reader, out formatCode);
            if (isValueAxis)
            {
              series.FormatCode = formatCode;
              continue;
            }
            series.CategoriesFormatCode = formatCode;
            continue;
          case "strLit":
            values = this.ParseDirectlyEnteredValues(reader, out formatCode);
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
    if (formula != null && formula.StartsWith("(") && formula.EndsWith(")"))
      formula = formula.Substring(1, formula.Length - 2);
    IRange seriesValues = (IRange) null;
    if (formula != null)
    {
      seriesValues = ChartParser.GetRange(series.ParentBook, formula);
      switch (seriesValues)
      {
        case null:
          if (flag1 && series.NumRefFormula == null)
          {
            series.NumRefFormula = formula;
            break;
          }
          if (flag2 && series.StrRefFormula == null)
          {
            series.StrRefFormula = formula;
            break;
          }
          if (flag3 && series.MulLvlStrRefFormula == null)
          {
            series.MulLvlStrRefFormula = formula;
            break;
          }
          break;
        case ExternalRange _:
          (seriesValues as ExternalRange).IsNumReference = flag1;
          (seriesValues as ExternalRange).IsStringReference = flag2;
          (seriesValues as ExternalRange).IsMultiReference = flag3;
          break;
        case RangeImpl _:
          (seriesValues as RangeImpl).IsNumReference = flag1;
          (seriesValues as RangeImpl).IsStringReference = flag2;
          (seriesValues as RangeImpl).IsMultiReference = flag3;
          break;
        case NameImpl _:
          (seriesValues as NameImpl).IsNumReference = flag1;
          (seriesValues as NameImpl).IsStringReference = flag2;
          (seriesValues as NameImpl).IsMultiReference = flag3;
          break;
      }
    }
    if (seriesValues != null && isValueAxis)
    {
      int count = parentChart.Categories.Count;
      if (count < seriesValues.Count)
      {
        for (int index = count; index < seriesValues.Count; ++index)
          (parentChart.Categories as ChartCategoryCollection).Add();
      }
    }
    return seriesValues;
  }

  internal static IRange GetRange(WorkbookImpl workbook, string formula)
  {
    IWorksheet worksheet = workbook.Worksheets.Count > 0 ? workbook.Worksheets[0] : (IWorksheet) null;
    return ChartParser.GetRange(workbook, formula, worksheet);
  }

  internal static IRange GetRange(WorkbookImpl workbook, string formula, IWorksheet worksheet)
  {
    Ptg[] ptgArray = workbook.FormulaUtil.ParseString(formula);
    IRange range1 = (IRange) null;
    if (ptgArray.Length == 1)
    {
      if (!(ptgArray[0] is IRangeGetter rangeGetter))
        return (IRange) null;
      range1 = rangeGetter.GetRange((IWorkbook) workbook, worksheet);
    }
    else
    {
      IRange range2 = (ptgArray[0] as IRangeGetter).GetRange((IWorkbook) workbook, worksheet);
      if (range2 != null)
      {
        IRanges ranges = range2.Worksheet.CreateRangesCollection();
        ranges.Add(range2);
        for (int index = 1; index < ptgArray.Length; ++index)
        {
          if (!ptgArray[index].IsOperation && ptgArray[index] is IRangeGetter rangeGetter)
          {
            IRange range3 = rangeGetter.GetRange((IWorkbook) workbook, worksheet);
            if (range3 == null || ranges.Worksheet != range3.Worksheet)
            {
              ranges = (IRanges) null;
              break;
            }
            ranges.Add(range3);
          }
        }
        range1 = ranges == null || ranges.Count < 1 ? (IRange) null : (IRange) ranges;
      }
    }
    return range1;
  }

  private string ParseNumReference(XmlReader reader, out object[] values)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "numRef")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    string numReference = (string) null;
    values = (object[]) null;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "f":
            if (numReference == null)
            {
              numReference = reader.ReadElementContentAsString();
              continue;
            }
            reader.Skip();
            continue;
          case "numCache":
            string formatCode = (string) null;
            values = this.ParseDirectlyEnteredValues(reader, out formatCode);
            if (values.Length == 0)
            {
              values = (object[]) null;
              continue;
            }
            continue;
          case "extLst":
            reader.Read();
            if (reader.LocalName == "ext")
            {
              reader.Read();
              if (reader.LocalName == "fullRef")
              {
                reader.Read();
                if (reader.LocalName == "sqref")
                {
                  numReference = reader.ReadElementContentAsString();
                  reader.Skip();
                }
                reader.Skip();
              }
              if (reader.LocalName == "formulaRef")
              {
                reader.Read();
                if (reader.LocalName == "sqref")
                {
                  numReference = reader.ReadElementContentAsString();
                  reader.Skip();
                }
                reader.Skip();
              }
              reader.Read();
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
    return numReference;
  }

  private string ParseNumReference(
    XmlReader reader,
    out object[] values,
    out string filteredvalue,
    out string formatCode)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "numRef")
      throw new XmlException("Unexpected xml tag.");
    formatCode = (string) null;
    reader.Read();
    string numReference = (string) null;
    filteredvalue = (string) null;
    values = (object[]) null;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "f":
            if (numReference == null)
            {
              numReference = reader.ReadElementContentAsString();
              continue;
            }
            filteredvalue = reader.ReadElementContentAsString();
            continue;
          case "numCache":
            values = this.ParseDirectlyEnteredValues(reader, out formatCode);
            if (values.Length == 0)
            {
              values = (object[]) null;
              continue;
            }
            continue;
          case "extLst":
            reader.Read();
            if (reader.LocalName == "ext")
            {
              reader.Read();
              if (reader.LocalName == "fullRef")
              {
                reader.Read();
                if (reader.LocalName == "sqref")
                {
                  numReference = reader.ReadElementContentAsString();
                  reader.Skip();
                }
                if (reader.LocalName != "formulaRef")
                  reader.Skip();
              }
              if (reader.LocalName == "formulaRef")
              {
                reader.Read();
                if (reader.LocalName == "sqref")
                {
                  if (numReference == null)
                    numReference = reader.ReadElementContentAsString();
                  else
                    filteredvalue = reader.ReadElementContentAsString();
                  reader.Skip();
                }
                reader.Skip();
              }
              reader.Read();
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
    return numReference;
  }

  private string ParseStringReference(XmlReader reader, ref string nameCache)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "strRef")
      throw new XmlException("Unexpected xml tag.");
    string formatCode = (string) null;
    reader.Read();
    string stringReference = (string) null;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "f":
            if (stringReference == null)
            {
              stringReference = reader.ReadElementContentAsString();
              continue;
            }
            reader.Skip();
            continue;
          case "extLst":
            reader.Read();
            if (reader.LocalName == "ext")
            {
              reader.Read();
              if (reader.LocalName == "fullRef")
              {
                reader.Read();
                if (reader.LocalName == "sqref")
                {
                  stringReference = reader.ReadElementContentAsString();
                  reader.Skip();
                }
                reader.Skip();
              }
              if (reader.LocalName == "formulaRef")
              {
                reader.Read();
                if (reader.LocalName == "sqref")
                {
                  stringReference = reader.ReadElementContentAsString();
                  reader.Skip();
                }
                reader.Skip();
              }
              reader.Read();
              continue;
            }
            continue;
          case "strCache":
            object[] directlyEnteredValues = this.ParseDirectlyEnteredValues(reader, out formatCode);
            nameCache = directlyEnteredValues.Length > 0 ? directlyEnteredValues[0].ToString() : (string) null;
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
    return stringReference;
  }

  private string ParseStringReference(
    XmlReader reader,
    out string Filteredcategory,
    out object[] values,
    ChartImpl chart)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "strRef")
      throw new XmlException("Unexpected xml tag.");
    Filteredcategory = string.Empty;
    reader.Read();
    string stringReference = (string) null;
    values = (object[]) null;
    bool flag = false;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "f":
            if (stringReference == null)
            {
              stringReference = reader.ReadElementContentAsString();
              if (chart.Application.IsChartCacheEnabled && chart.CategoryFormula == null)
              {
                chart.CategoryFormula = stringReference;
                flag = true;
                continue;
              }
              continue;
            }
            Filteredcategory = reader.ReadElementContentAsString();
            continue;
          case "strCache":
            if (chart.Application.IsChartCacheEnabled)
            {
              if (stringReference == null || chart.CategoryFormula != stringReference || flag)
              {
                string formatCode = (string) null;
                values = this.ParseDirectlyEnteredValues(reader, out formatCode);
                if (chart.CategoryLabelValues == null && values.Length > 0)
                  chart.CategoryLabelValues = values;
                if (values.Length == 0)
                {
                  values = (object[]) null;
                  continue;
                }
                continue;
              }
              values = (object[]) null;
              reader.Skip();
              continue;
            }
            reader.Skip();
            continue;
          case "extLst":
            reader.Read();
            if (reader.LocalName == "ext")
            {
              reader.Read();
              if (reader.LocalName == "fullRef")
              {
                reader.Read();
                if (reader.LocalName == "sqref")
                {
                  stringReference = reader.ReadElementContentAsString();
                  reader.Skip();
                }
                if (reader.LocalName != "formulaRef")
                  reader.Skip();
              }
              if (reader.LocalName == "formulaRef")
              {
                reader.Read();
                if (reader.LocalName == "sqref")
                {
                  if (stringReference == null)
                    stringReference = reader.ReadElementContentAsString();
                  else
                    Filteredcategory = reader.ReadElementContentAsString();
                  reader.Skip();
                }
                reader.Skip();
              }
              reader.Read();
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
    return stringReference;
  }

  private string ParseMultiLevelStringReference(XmlReader reader, ChartSerieImpl series)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "multiLvlStrRef")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    string levelStringReference = (string) null;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "f":
            levelStringReference = reader.ReadElementContentAsString();
            continue;
          case "multiLvlStrCache":
            series.MultiLevelStrCache = this.ParseMultiLevelStringCache(reader, series, "multiLvlStrCache");
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
    return levelStringReference;
  }

  private Dictionary<int, object[]> ParseMultiLevelStringCache(
    XmlReader reader,
    ChartSerieImpl series,
    string tagName)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    int index = 0;
    int key = 0;
    reader.Read();
    Dictionary<int, object[]> levelStringCache = new Dictionary<int, object[]>();
    List<object> list = new List<object>();
    bool flag = true;
    if (reader.NodeType == XmlNodeType.EndElement)
      return levelStringCache;
    while (flag)
    {
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
                  list.Insert(index, (object) null);
                  ++index;
                }
                while (index != num && num > index);
              }
              ++index;
              continue;
            case "ptCount":
              if (reader.MoveToAttribute("val"))
              {
                series.PointCount = XmlConvertExtension.ToInt32(reader.Value);
                continue;
              }
              continue;
            case "lvl":
              reader.Read();
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          reader.Skip();
      }
      if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName.Equals("multiLvlStrCache"))
        flag = false;
      if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName.Equals("lvl"))
      {
        levelStringCache.Add(key, list.ToArray());
        list.Clear();
        index = 0;
        ++key;
        reader.Read();
      }
    }
    reader.Read();
    return levelStringCache;
  }

  private void ParseMarker(XmlReader reader, ChartSerieImpl series, Excel2007Parser parser)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    if (reader.LocalName != "marker")
      throw new XmlException("Unexpected xml tag.");
    IChartSerieDataFormat serieFormat = series.SerieFormat;
    this.ParseMarker(reader, serieFormat, parser);
  }

  private void ParseMarker(
    XmlReader reader,
    IChartSerieDataFormat dataFormat,
    Excel2007Parser parser)
  {
    bool flag = true;
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "symbol":
              string valueTag = ChartParserCommon.ParseValueTag(reader);
              if (valueTag != "picture")
              {
                Excel2007ChartMarkerType excel2007ChartMarkerType = (Excel2007ChartMarkerType) Enum.Parse(typeof (Excel2007ChartMarkerType), valueTag, false);
                dataFormat.MarkerStyle = (ExcelChartMarkerType) excel2007ChartMarkerType;
                (dataFormat as ChartSerieDataFormatImpl).HasMarkerProperties = true;
                flag = false;
                continue;
              }
              continue;
            case "size":
              dataFormat.MarkerSize = ChartParserCommon.ParseIntValueTag(reader);
              flag = false;
              continue;
            case "spPr":
              flag = false;
              this.ParseMarkerFill(reader, dataFormat, parser);
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
    ((ChartSerieDataFormatImpl) dataFormat).MarkerFormat.IsAutoColor = flag;
    reader.Read();
  }

  private void ParseMarkerFill(
    XmlReader reader,
    IChartSerieDataFormat serieDataFormat,
    Excel2007Parser parser)
  {
    string localName = reader.LocalName;
    bool flag = true;
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      ChartSerieDataFormatImpl format = serieDataFormat as ChartSerieDataFormatImpl;
      while (reader.NodeType != XmlNodeType.EndElement && reader.LocalName != localName)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "solidFill":
              ChartParserCommon.ParseSolidFill(reader, parser, format.MarkerBackColorObject);
              format.MarkerFormat.FillColorIndex = (ushort) format.MarkerBackColorObject.GetIndexed((IWorkbook) this.m_book);
              flag = false;
              continue;
            case "gradFill":
              GradientStops gradientFill = ChartParserCommon.ParseGradientFill(reader, parser);
              ChartMarkerFormatRecord markerFormat = format.MarkerFormat;
              markerFormat.IsAutoColor = false;
              markerFormat.IsNotShowInt = false;
              markerFormat.IsNotShowBrd = false;
              format.MarkerBackColorObject.CopyFrom(gradientFill[0].ColorObject, true);
              format.MarkerGradient = gradientFill;
              flag = false;
              continue;
            case "ln":
              format.MarkerFormat.FlagOptions |= (byte) 2;
              format.MarkerFormat.IsNotShowBrd = !this.ParseMarkerLine(reader, format.MarkerForeColorObject, parser, format);
              continue;
            case "noFill":
              format.MarkerFormat.IsNotShowInt = true;
              reader.Read();
              continue;
            case "effectLst":
              format.EffectListStream = ShapeParser.ReadNodeAsStream(reader);
              format.EffectListStream.Position = 0L;
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
    (serieDataFormat as ChartSerieDataFormatImpl).IsAutoMarkerColor = flag;
  }

  private bool ParseMarkerLine(
    XmlReader reader,
    ColorObject color,
    Excel2007Parser parser,
    ChartSerieDataFormatImpl format)
  {
    bool markerLine = false;
    int Alpha = 100000;
    Stream data = ShapeParser.ReadNodeAsStream(reader);
    data.Position = 0L;
    reader = UtilityMethods.CreateReader(data);
    format.MarkerLineStream = data;
    if (reader.MoveToAttribute("w"))
    {
      format.MarkerLineWidth = (double) int.Parse(reader.Value) / 12700.0;
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
            case "solidFill":
              ChartParserCommon.ParseSolidFill(reader, parser, color, out Alpha);
              format.MarkerTransparency = (double) Alpha / 100000.0;
              markerLine = true;
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
    return markerLine;
  }

  private void ParseUpDownBars(
    XmlReader reader,
    ChartImpl chart,
    ChartSerieImpl series,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "upDownBars")
      throw new XmlException("Unexpected xml tag.");
    ChartFormatImpl chartFormatImpl = (ChartFormatImpl) null;
    if (series != null)
      chartFormatImpl = (ChartFormatImpl) series.SerieFormat.CommonSerieOptions;
    else if (chart.PrimaryFormats.ContainsIndex(0))
      chartFormatImpl = chart.PrimaryFormats[0];
    FileDataHolder dataHolder = chart.ParentWorkbook.DataHolder;
    if (chartFormatImpl != (ChartFormatImpl) null)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "gapWidth":
              chartFormatImpl.FirstDropBar.Gap = ChartParserCommon.ParseIntValueTag(reader);
              continue;
            case "upBars":
              IChartDropBar firstDropBar = chartFormatImpl.FirstDropBar;
              this.ParseDropBar(reader, firstDropBar, dataHolder, relations);
              continue;
            case "downBars":
              IChartDropBar secondDropBar = chartFormatImpl.SecondDropBar;
              this.ParseDropBar(reader, secondDropBar, dataHolder, relations);
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

  private void ParseDropBar(
    XmlReader reader,
    IChartDropBar dropBar,
    FileDataHolder dataHolder,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (dropBar == null)
      throw new ArgumentNullException(nameof (dropBar));
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "spPr")
        {
          ChartFillObjectGetterAny objectGetter = new ChartFillObjectGetterAny(dropBar.LineProperties as ChartBorderImpl, dropBar.Interior as ChartInteriorImpl, dropBar.Fill as IInternalFill, dropBar.Shadow as ShadowImpl, dropBar.ThreeD as ThreeDFormatImpl);
          ChartParserCommon.ParseShapeProperties(reader, (IChartFillObjectGetter) objectGetter, dataHolder, relations);
        }
        else
          reader.Skip();
      }
    }
    reader.Read();
  }

  private void ParseDataTable(XmlReader reader, ChartImpl chart)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "dTable")
      throw new XmlException("Unexpected xml tag.");
    Excel2007Parser parser = chart.ParentWorkbook.DataHolder.Parser;
    chart.HasDataTable = true;
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      IChartDataTable dataTable = chart.DataTable;
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "showHorzBorder":
              dataTable.HasHorzBorder = ChartParserCommon.ParseBoolValueTag(reader);
              continue;
            case "showVertBorder":
              dataTable.HasVertBorder = ChartParserCommon.ParseBoolValueTag(reader);
              continue;
            case "showOutline":
              dataTable.HasBorders = ChartParserCommon.ParseBoolValueTag(reader);
              continue;
            case "showKeys":
              dataTable.ShowSeriesKeys = ChartParserCommon.ParseBoolValueTag(reader);
              continue;
            case "txPr":
              IInternalChartTextArea textArea = dataTable.TextArea as IInternalChartTextArea;
              this.ParseDefaultTextFormatting(reader, textArea, parser);
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

  internal void ParseSeriesProperties(
    XmlReader reader,
    ChartSerieImpl series,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    if (reader.LocalName != "spPr")
      throw new XmlException("Unexpected xml tag");
    ChartSerieDataFormatImpl serieFormat = (ChartSerieDataFormatImpl) series.SerieFormat;
    FileDataHolder parentHolder = series.ParentChart.DataHolder.ParentHolder;
    ChartFillObjectGetter objectGetter = new ChartFillObjectGetter(serieFormat);
    ChartParserCommon.ParseShapeProperties(reader, (IChartFillObjectGetter) objectGetter, parentHolder, relations);
  }

  private void ParseDefaultTextFormatting(
    XmlReader reader,
    IInternalChartTextArea textFormatting,
    Excel2007Parser parser)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textFormatting == null)
      throw new ArgumentNullException(nameof (textFormatting));
    if (reader.LocalName != "txPr")
      throw new XmlException("Unexpected xml tag");
    textFormatting.ParagraphType = ChartParagraphType.CustomDefault;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "bodyPr":
            if (reader.MoveToAttribute("rot"))
              textFormatting.TextRotationAngle = XmlConvertExtension.ToInt32(reader.Value) / 60000;
            reader.Skip();
            continue;
          case "defRPr":
            ChartParserCommon.ParseParagraphRunProperites(reader, textFormatting, parser, (TextSettings) null);
            while (reader.LocalName != "txPr")
              reader.Read();
            continue;
          case "p":
            while (reader.NodeType != XmlNodeType.EndElement && reader.LocalName != "txPr" && reader.LocalName != "defRPr")
              reader.Read();
            if (reader.LocalName == "defRPr")
            {
              if (!(this is ChartExParser) || !(textFormatting.Parent is ChartLegendImpl))
              {
                if (reader.IsEmptyElement && textFormatting is ChartTextAreaImpl)
                  (textFormatting as ChartTextAreaImpl).isEmptyDefPara = true;
                ChartParserCommon.ParseParagraphRunProperites(reader, textFormatting, parser, (TextSettings) null);
              }
              while (reader.LocalName != "txPr")
              {
                if (reader.LocalName == "endParaRPr" && reader.MoveToAttribute("lang") && this.m_book.Loading && this.m_book.Version != ExcelVersion.Excel97to2003 && !textFormatting.Font.HasLatin)
                {
                  textFormatting.Font.Language = reader.Value;
                  FontImpl fontImpl = (FontImpl) null;
                  if (this.m_book.MinorFonts.TryGetValue("latin", out fontImpl))
                    textFormatting.FontName = fontImpl.FontName;
                }
                if (this is ChartExParser && (reader.LocalName == "endParaRPr" || reader.MoveToElement() && reader.LocalName == "endParaRPr") && textFormatting.Parent is ChartLegendImpl && textFormatting != null)
                {
                  textFormatting.Font.Size = 9.0;
                  ChartParserCommon.ParseParagraphRunProperites(reader, textFormatting, parser, (TextSettings) null);
                }
                reader.Read();
              }
              continue;
            }
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Read();
    }
    reader.Read();
  }

  private object[] ParseDirectlyEnteredValues(XmlReader reader, out string formatCode)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    bool flag = false;
    int num1 = 0;
    if (reader.LocalName == "strLit" || reader.LocalName == "strCache")
      flag = true;
    formatCode = (string) null;
    reader.Read();
    List<object> list = new List<object>();
    int index1 = 0;
    if (reader.NodeType == XmlNodeType.EndElement)
      return list.ToArray();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "pt":
            int num2 = this.AddNumericPoint(reader, list);
            if (num2 != -1 && num2 > index1)
            {
              do
              {
                if (flag)
                  list.Insert(index1, (object) "");
                else
                  list.Insert(index1, (object) null);
                ++index1;
              }
              while (index1 != num2 && num2 > index1);
            }
            ++index1;
            continue;
          case "ptCount":
            if (reader.MoveToAttribute("val"))
            {
              num1 = XmlConvertExtension.ToInt32(reader.Value);
              continue;
            }
            continue;
          case nameof (formatCode):
            if (!reader.IsEmptyElement && !flag)
            {
              formatCode = reader.ReadElementContentAsString();
              formatCode = formatCode;
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
    if (flag && list.Count == 0 && num1 > 0)
    {
      for (int index2 = 0; index2 < num1; ++index2)
        list.Add((object) "");
    }
    reader.Read();
    return list.ToArray();
  }

  internal int AddNumericPoint(XmlReader reader, List<object> list)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (list == null)
      throw new ArgumentNullException(nameof (list));
    if (reader.LocalName != "pt")
      throw new XmlException();
    int num = -1;
    if (reader.MoveToAttribute("idx"))
      num = XmlConvertExtension.ToInt32(reader.Value);
    if (this is ChartExParser)
    {
      reader.MoveToElement();
      list.Add(this.ReadXmlValue(reader));
      Excel2007Parser.SkipWhiteSpaces(reader);
    }
    else
    {
      if (!reader.IsEmptyElement)
      {
        reader.Read();
        while (reader.NodeType != XmlNodeType.EndElement)
        {
          if (reader.NodeType == XmlNodeType.Element)
          {
            switch (reader.LocalName)
            {
              case "v":
                list.Add(this.ReadXmlValue(reader));
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
    return num;
  }

  internal int AddNumericPoint(XmlReader reader, Dictionary<int, object> list)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (list == null)
      throw new ArgumentNullException(nameof (list));
    if (reader.LocalName != "pt")
      throw new XmlException();
    int key = -1;
    if (reader.MoveToAttribute("idx"))
      key = XmlConvertExtension.ToInt32(reader.Value);
    if (this is ChartExParser)
    {
      reader.MoveToElement();
      list.Add(key, this.ReadXmlValue(reader));
      Excel2007Parser.SkipWhiteSpaces(reader);
    }
    else
    {
      if (!reader.IsEmptyElement)
      {
        reader.Read();
        while (reader.NodeType != XmlNodeType.EndElement)
        {
          if (reader.NodeType == XmlNodeType.Element)
          {
            switch (reader.LocalName)
            {
              case "v":
                list.Add(key, this.ReadXmlValue(reader));
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
    return key;
  }

  private object ReadXmlValue(XmlReader reader)
  {
    string s = reader != null ? reader.ReadElementContentAsString() : throw new ArgumentNullException(nameof (reader));
    NumberStyles style = NumberStyles.Any;
    double result;
    return !double.TryParse(s, style, (IFormatProvider) CultureInfo.InvariantCulture, out result) ? (object) s : (object) result;
  }

  private void ChangeKeyToChartGroup(ChartImpl chart)
  {
    Dictionary<int, ChartDataPointsCollection> dictionary = new Dictionary<int, ChartDataPointsCollection>();
    foreach (int key1 in chart.CommonDataPointsCollection.Keys)
    {
      int key2 = 0;
      for (int index = 0; index < chart.Series.Count; ++index)
      {
        ChartSerieImpl chartSerieImpl = chart.Series[index] as ChartSerieImpl;
        if (chartSerieImpl.Number == key1)
        {
          key2 = chartSerieImpl.ChartGroup;
          break;
        }
      }
      if (!dictionary.ContainsKey(key2))
        dictionary.Add(key2, chart.CommonDataPointsCollection[key1]);
    }
    chart.CommonDataPointsCollection = dictionary;
  }

  private void ParseDataLabels(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    int index)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "dLbls")
      throw new XmlException("Unexpected xml tag.");
    if (chart.CommonDataPointsCollection == null)
      chart.CommonDataPointsCollection = new Dictionary<int, ChartDataPointsCollection>();
    if (!chart.CommonDataPointsCollection.ContainsKey(index))
    {
      ChartDataPointsCollection pointsCollection = new ChartDataPointsCollection(chart.Application, (object) chart);
      chart.CommonDataPointsCollection.Add(index, pointsCollection);
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "dLbl":
              this.ParseDataLabel(reader, chart, relations, index);
              continue;
            default:
              IChartDataLabels dataLabels = chart.CommonDataPointsCollection[index].DefaultDataPoint.DataLabels;
              FileDataHolder dataHolder = chart.ParentWorkbook.DataHolder;
              Excel2007Parser parser = dataHolder.Parser;
              this.ParseDataLabelSettings(reader, dataLabels, parser, dataHolder, relations, false);
              if (chart.ParentWorkbook.Version != ExcelVersion.Excel2007)
              {
                (dataLabels as ChartDataLabelsImpl).IsEnableDataLabels = true;
                continue;
              }
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

  private void ParseDataLabel(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    int index)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException("series");
    if (reader.LocalName != "dLbl")
      throw new XmlException("Unexpeced xml tag.");
    reader.Read();
    IChartDataLabels chartDataLabels = (IChartDataLabels) null;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "idx":
            int intValueTag = ChartParserCommon.ParseIntValueTag(reader);
            chartDataLabels = chart.CommonDataPointsCollection[index][intValueTag].DataLabels;
            (chartDataLabels as ChartDataLabelsImpl).ShowTextProperties = false;
            continue;
          case "layout":
            (chartDataLabels as ChartDataLabelsImpl).Layout = (IChartLayout) new ChartLayoutImpl(this.m_book.Application, (object) (chartDataLabels as ChartDataLabelsImpl), (object) chart);
            ChartParserCommon.ParseChartLayout(reader, (chartDataLabels as ChartDataLabelsImpl).Layout);
            continue;
          case "delete":
            bool boolValueTag = ChartParserCommon.ParseBoolValueTag(reader);
            (chartDataLabels as ChartDataLabelsImpl).IsDelete = boolValueTag;
            continue;
          default:
            FileDataHolder dataHolder = chart.ParentWorkbook.DataHolder;
            Excel2007Parser parser = dataHolder.Parser;
            this.ParseDataLabelSettings(reader, chartDataLabels, parser, dataHolder, relations, false);
            continue;
        }
      }
      else
        reader.Skip();
    }
    reader.Read();
  }

  internal static ColorObject ParseInvertSolidFillFormat(Stream stream, ChartSerieImpl serie)
  {
    ColorObject color = (ColorObject) null;
    XmlReader reader = UtilityMethods.CreateReader(stream);
    if (reader.LocalName == "invertSolidFillFmt")
    {
      reader.Read();
      while (reader.LocalName != "invertSolidFillFmt")
      {
        if (reader.LocalName == "solidFill")
        {
          color = (ColorObject) ColorExtension.Empty;
          ChartParserCommon.ParseSolidFill(reader, serie.ParentChart.ParentWorkbook.DataHolder.Parser, color);
          break;
        }
        if (reader.LocalName == "spPr")
          reader.Read();
        else
          reader.Skip();
      }
    }
    return color;
  }
}
