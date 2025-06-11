// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.XmlSerialization.Charts.ChartParser
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.Compression;
using Syncfusion.OfficeChart.Implementation.Charts;
using Syncfusion.OfficeChart.Implementation.Collections;
using Syncfusion.OfficeChart.Implementation.Shapes;
using Syncfusion.OfficeChart.Implementation.XmlReaders;
using Syncfusion.OfficeChart.Implementation.XmlReaders.Shapes;
using Syncfusion.OfficeChart.Implementation.XmlSerialization.Constants;
using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Interfaces.Charts;
using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.Charts;
using Syncfusion.OfficeChart.Parser.Biff_Records.Formula;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.XmlSerialization.Charts;

internal class ChartParser
{
  internal const float DefaultTitleSize = 18f;
  private WorkbookImpl m_book;
  private double _appVersion = -1.0;
  internal ExcelEngine engine;

  internal IWorksheet Worksheet
  {
    get
    {
      if (this.engine == null)
      {
        this.engine = new ExcelEngine();
        this.engine.Excel.Workbooks.Create(1);
      }
      return this.engine.Excel.Workbooks[0].Worksheets[0];
    }
  }

  public ChartParser(WorkbookImpl book) => this.m_book = book;

  internal string ApplyNumberFormat(object value, string numberFormat)
  {
    RangeImpl rangeImpl = this.Worksheet["A1"] as RangeImpl;
    rangeImpl.Value2 = value;
    rangeImpl.NumberFormat = numberFormat;
    return rangeImpl.DisplayText;
  }

  public void ParseChart(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    double appVersion)
  {
    bool throwOnUnknownNames = this.m_book.ThrowOnUnknownNames;
    this.m_book.ThrowOnUnknownNames = false;
    this._appVersion = appVersion;
    this.ParseChart(reader, chart, relations);
    this.m_book.ThrowOnUnknownNames = throwOnUnknownNames;
  }

  internal static ChartColor ParseInvertSolidFillFormat(Stream stream, ChartSerieImpl serie)
  {
    ChartColor color = (ChartColor) null;
    XmlReader reader = UtilityMethods.CreateReader(stream);
    if (reader.LocalName == "invertSolidFillFmt")
    {
      reader.Read();
      while (reader.LocalName != "invertSolidFillFmt")
      {
        if (reader.LocalName == "solidFill")
        {
          color = (ChartColor) ColorExtension.Empty;
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
    IOfficeChartFrameFormat chartArea = chart.ChartArea;
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
            chart.HasChartArea = true;
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
            reader.Skip();
            continue;
          case "pivotSource":
            this.ParsePivotSource(reader, chart);
            continue;
          case "printSettings":
            this.ParsePrintSettings(reader, chart, relations);
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
          default:
            reader.Read();
            continue;
        }
      }
      else
        reader.Skip();
    }
    chart.DetectIsInRowOnParsing();
    if ((chart.DataRange as ChartDataRange).Range != null && chart.Categories.Count > 0)
    {
      IRange range = (IRange) null;
      chart.IsSeriesInRows = this.DetectIsInRow((chart.Series[0].Values as ChartDataRange).Range);
      this.GetSerieOrAxisRange((chart.DataRange as ChartDataRange).Range, chart.IsSeriesInRows, out range, 0);
      int CategoryLabelCount = 0;
      ChartDataRange categoryLabels = chart.Series[0].CategoryLabels as ChartDataRange;
      if (categoryLabels.Range != null)
        CategoryLabelCount = !chart.IsSeriesInRows ? categoryLabels.Range.LastColumn - categoryLabels.Range.Column : categoryLabels.Range.LastRow - categoryLabels.Range.Row;
      this.GetSerieOrAxisRange(range, !chart.IsSeriesInRows, out range, CategoryLabelCount);
      int count = range.Count / (chart.Series[0].Values as ChartDataRange).Range.Count;
      int num = (chart.Series[0].CategoryLabels as ChartDataRange).Range == null ? (chart.Series[0].Values as ChartDataRange).Range.Count : (chart.Series[0].CategoryLabels as ChartDataRange).Range.Count / (CategoryLabelCount + 1);
      for (int index = 0; index < num; ++index)
      {
        IRange categoryRange = ChartImpl.GetCategoryRange(range, out range, count, chart.IsSeriesInRows);
        (chart.Categories[index] as ChartCategory).CategoryLabelIRange = (chart.Series[0].CategoryLabels as ChartDataRange).Range;
        (chart.Categories[index] as ChartCategory).ValuesIRange = categoryRange;
        if ((chart.Categories[0].CategoryLabel as ChartDataRange).Range != null)
        {
          (chart.Categories[index] as ChartCategory).Name = (chart.Categories[0].CategoryLabel as ChartDataRange).Range.Cells[index].Text;
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
    else if (chart.Series != null && chart.Series.Count > 0 && chart.Series[0].Values != null && (chart.Series[0].CategoryLabels as ChartDataRange).Range != null && chart.Categories.Count > 0)
    {
      IRange range1 = (chart.Series[0].CategoryLabels as ChartDataRange).Range;
      int count = range1.Count;
      int row = range1.Row;
      int column = range1.Column;
      IRange range2 = (IRange) null;
      bool flag = false;
      if (range1 != null && range1.GetType() != typeof (ExternalRange) && range1.Worksheet != null)
      {
        flag = true;
        if (!chart.IsSeriesInRows && range1.LastRow >= row + chart.Categories.Count - 1)
          range2 = range1[row, column, row + chart.Categories.Count - 1, column];
        else if (range1.LastColumn >= column + chart.Categories.Count - 1)
          range2 = range1[row, column, row, column + chart.Categories.Count - 1];
      }
      for (int index = 0; index < chart.Categories.Count; ++index)
      {
        (chart.Categories[index] as ChartCategory).CategoryLabelIRange = range1;
        (chart.Categories[index] as ChartCategory).ValuesIRange = (chart.Series[0].Values as ChartDataRange).Range;
        if (flag && range2 != null && index < count)
          (chart.Categories[index] as ChartCategory).Name = range2.Cells[index].DisplayText;
      }
    }
    if (chart.Series.Count != 0 && (chart.Series[0] as ChartSerieImpl).FilteredValue != null)
      this.FindFilter(chart.Categories, (chart.Series[0] as ChartSerieImpl).FilteredValue, (chart.Series[0].Values as ChartDataRange).Range.AddressGlobal, chart.Series[0], chart.IsSeriesInRows);
    reader.Read();
    if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "chartSpace")
      ChartParserCommon.clear();
    ChartCategoryAxisImpl primaryCategoryAxis = chart.PrimaryCategoryAxis as ChartCategoryAxisImpl;
    if (primaryCategoryAxis.IsChartBubbleOrScatter)
      primaryCategoryAxis.SwapAxisValues();
    if (chart.IsSecondaryCategoryAxisAvail)
    {
      ChartCategoryAxisImpl secondaryCategoryAxis = chart.SecondaryCategoryAxis as ChartCategoryAxisImpl;
      if (secondaryCategoryAxis.IsChartBubbleOrScatter)
        secondaryCategoryAxis.SwapAxisValues();
    }
    if (!((chart.DataRange as ChartDataRange).SheetImpl.Name != (chart.ChartData as ChartData).SheetImpl.Name))
      return;
    (chart.ChartData as ChartData).SheetImpl = (chart.DataRange as ChartDataRange).SheetImpl;
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
              IOfficeChartDataLabels dataLabels = chart.CommonDataPointsCollection[index].DefaultDataPoint.DataLabels;
              FileDataHolder dataHolder = chart.ParentWorkbook.DataHolder;
              Excel2007Parser parser = dataHolder.Parser;
              this.ParseDataLabelSettings(reader, dataLabels, parser, dataHolder, relations, false);
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
    IOfficeChartDataLabels officeChartDataLabels = (IOfficeChartDataLabels) null;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "idx":
            int intValueTag = ChartParserCommon.ParseIntValueTag(reader);
            officeChartDataLabels = chart.CommonDataPointsCollection[index][intValueTag].DataLabels;
            (officeChartDataLabels as ChartDataLabelsImpl).ShowTextProperties = false;
            continue;
          case "layout":
            (officeChartDataLabels as ChartDataLabelsImpl).Layout = (IOfficeChartLayout) new ChartLayoutImpl(this.m_book.Application, (object) (officeChartDataLabels as ChartDataLabelsImpl), (object) chart);
            ChartParserCommon.ParseChartLayout(reader, (officeChartDataLabels as ChartDataLabelsImpl).Layout);
            continue;
          case "delete":
            bool boolValueTag = ChartParserCommon.ParseBoolValueTag(reader);
            (officeChartDataLabels as ChartDataLabelsImpl).IsDelete = boolValueTag;
            continue;
          default:
            FileDataHolder dataHolder = chart.ParentWorkbook.DataHolder;
            Excel2007Parser parser = dataHolder.Parser;
            this.ParseDataLabelSettings(reader, officeChartDataLabels, parser, dataHolder, relations, false);
            continue;
        }
      }
      else
        reader.Skip();
    }
    reader.Read();
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
        chartAxisImpl.Font = (IOfficeFont) ((CommonWrapper) chart.Font).Clone((object) chart);
        chartAxisImpl.IsChartFont = true;
        chartAxisImpl.IsDefaultTextSettings = true;
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
          reader.Skip();
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
          case "pivotOptions":
            this.ParsePivotOptions(reader, chart);
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

  internal void ParsePrintSettings(XmlReader reader, ChartImpl chart, RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "printSettings")
      throw new XmlException();
    ChartPageSetupConstants constants = new ChartPageSetupConstants();
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
            string str = reader.ReadElementContentAsString();
            chart.PreservedPivotSource = str;
            if (str != null)
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

  private void ParseUserShapes(XmlReader reader, ChartImpl chart, RelationCollection relations)
  {
    reader.MoveToAttribute("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
    string id = reader.Value;
    Dictionary<string, object> dictItemsToRemove = new Dictionary<string, object>();
    Relation relation = relations[id];
    chart.DataHolder.ParseDrawings((WorksheetBaseImpl) chart, relation, dictItemsToRemove);
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
    MemoryStream data = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((Stream) data, Encoding.UTF8);
    writer.WriteStartElement("root");
    Chart3DRecord chart3D = (Chart3DRecord) null;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "view3D":
            chart3D = this.ParseView3D(reader, chart);
            continue;
          case "plotArea":
            this.ParsePlotArea(reader, chart, relations, parentHolder.Parser);
            continue;
          case "legend":
            chart.HasLegend = true;
            this.ParseLegend(reader, chart.Legend, chart, relations);
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
              chart.DisplayBlanksAs = (OfficeChartPlotEmpty) Enum.Parse(typeof (Excel2007ChartPlotEmpty), reader.Value.ToString().ToLower());
              continue;
            }
            continue;
          case "title":
            writer.WriteNode(reader, false);
            writer.Flush();
            flag = true;
            continue;
          case "autoTitleDeleted":
            this.ParseAutoTitleDeleted(reader, chart);
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
      IInternalOfficeChartTextArea chartTitleArea = chart.ChartTitleArea as IInternalOfficeChartTextArea;
      data.Position = 0L;
      XmlReader reader1 = UtilityMethods.CreateReader((Stream) data);
      reader1.Read();
      if (reader1.LocalName == "title")
      {
        reader1.Read();
        while (reader1.NodeType != XmlNodeType.EndElement)
        {
          if (reader1.NodeType == XmlNodeType.Element)
          {
            switch (reader1.LocalName)
            {
              case "txPr":
                ((ChartTextAreaImpl) chartTitleArea).ParagraphType = ChartParagraphType.CustomDefault;
                ChartParserCommon.ParseDefaultTextFormatting(reader1, chartTitleArea, parentHolder.Parser, new double?(18.0));
                ((ChartTextAreaImpl) chartTitleArea).IsTextParsed = true;
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
      data.Position = 0L;
      XmlReader reader2 = UtilityMethods.CreateReader((Stream) data);
      reader2.Read();
      if (reader2.LocalName == "title" && !reader2.IsEmptyElement)
      {
        ChartParserCommon.SetWorkbook(this.m_book);
        ChartParserCommon.ParseTextArea(reader2, chartTitleArea, parentHolder, relations, new float?(18f));
        chart.HasTitle = true;
      }
      reader2.Close();
      reader1.Close();
      data.Dispose();
    }
    reader.Read();
    this.Set3DSettings(chart, chart3D);
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

  private void ParseAutoTitleDeleted(XmlReader reader, ChartImpl chart)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.MoveToAttribute("val"))
      chart.HasAutoTitle = new bool?(XmlConvertExtension.ToBoolean(reader.Value));
    reader.Read();
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
    if (!chart3D.IsDefaultRotation)
      chart.Rotation = (int) chart3D.RotationAngle;
    chart.DepthPercent = (int) chart3D.Depth;
    chart.Perspective = (int) chart3D.DistanceFromEye;
    chart.RightAngleAxes = chart3D.IsPerspective;
  }

  internal void ParseLegend(
    XmlReader reader,
    IOfficeChartLegend legend,
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
        (legend as ChartLegendImpl).ChartExPosition = position;
      legend.Position = this.GetChartLegendPosition(position);
    }
    reader.Read();
    Excel2007Parser parser = chart.ParentWorkbook.DataHolder.Parser;
    (legend as ChartLegendImpl).IsChartTextArea = true;
    legend.TextArea.FontName = "Calibri";
    legend.TextArea.Size = 10.0;
    (legend as ChartLegendImpl).IsChartTextArea = false;
    if (!flag)
    {
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "legendPos":
              string valueTag = ChartParserCommon.ParseValueTag(reader);
              legend.Position = (OfficeLegendPosition) Enum.Parse(typeof (Excel2007LegendPosition), valueTag, false);
              continue;
            case "legendEntry":
              this.ParseLegendEntry(reader, legend, parser);
              continue;
            case "txPr":
              ChartLegendImpl chartLegendImpl = legend as ChartLegendImpl;
              chartLegendImpl.IsChartTextArea = true;
              IInternalOfficeChartTextArea textArea = legend.TextArea as IInternalOfficeChartTextArea;
              this.ParseDefaultTextFormatting(reader, textArea, parser);
              if (chartLegendImpl.TextArea.Size != 10.0 || (textArea as ChartTextAreaImpl).ShowSizeProperties)
                chartLegendImpl.IsDefaultTextSettings = false;
              chartLegendImpl.IsChartTextArea = false;
              continue;
            case "spPr":
              IOfficeChartFrameFormat frameFormat = legend.FrameFormat;
              ChartFillObjectGetterAny objectGetter = new ChartFillObjectGetterAny(frameFormat.Border as ChartBorderImpl, frameFormat.Interior as ChartInteriorImpl, frameFormat.Fill as IInternalFill, frameFormat.Shadow as ShadowImpl, frameFormat.ThreeD as ThreeDFormatImpl);
              FileDataHolder dataHolder = chart.ParentWorkbook.DataHolder;
              ChartParserCommon.ParseShapeProperties(reader, (IChartFillObjectGetter) objectGetter, dataHolder, relations);
              continue;
            case "layout":
              (legend as ChartLegendImpl).Layout = (IOfficeChartLayout) new ChartLayoutImpl(this.m_book.Application, (object) (legend as ChartLegendImpl), (object) chart);
              ChartParserCommon.ParseChartLayout(reader, (legend as ChartLegendImpl).Layout);
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

  private OfficeLegendPosition GetChartLegendPosition(ushort position)
  {
    OfficeLegendPosition chartLegendPosition = OfficeLegendPosition.NotDocked;
    ushort num1 = 240 /*0xF0*/;
    ChartExPositionAlignment positionAlignment = (ChartExPositionAlignment) ((int) position & (int) num1);
    ushort num2 = 15;
    ChartExSidePosition chartExSidePosition = (ChartExSidePosition) ((int) position & (int) num2);
    switch (positionAlignment)
    {
      case ChartExPositionAlignment.min:
        if (chartExSidePosition == ChartExSidePosition.r)
        {
          chartLegendPosition = OfficeLegendPosition.Corner;
          break;
        }
        break;
      case ChartExPositionAlignment.ctr:
        switch (chartExSidePosition)
        {
          case ChartExSidePosition.l:
            chartLegendPosition = OfficeLegendPosition.Left;
            break;
          case ChartExSidePosition.t:
            chartLegendPosition = OfficeLegendPosition.Top;
            break;
          case ChartExSidePosition.r:
            chartLegendPosition = OfficeLegendPosition.Right;
            break;
          case ChartExSidePosition.b:
            chartLegendPosition = OfficeLegendPosition.Bottom;
            break;
        }
        break;
    }
    return chartLegendPosition;
  }

  internal static IRange GetRange(WorkbookImpl workbook, string formula)
  {
    FormulaUtil formulaUtil = workbook.DataHolder.Parser.FormulaUtil;
    if (!ChartImpl.TryAndModifyToValidFormula(formula))
      return (IRange) null;
    Ptg[] ptgArray = formulaUtil.ParseString(formula);
    IWorksheet worksheet = workbook.Worksheets.Count > 0 ? workbook.Worksheets[0] : (IWorksheet) null;
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
        IRanges rangesCollection = range2.Worksheet.CreateRangesCollection();
        rangesCollection.Add(range2);
        for (int index = 1; index < ptgArray.Length; ++index)
        {
          if (!ptgArray[index].IsOperation && ptgArray[index] is IRangeGetter rangeGetter)
          {
            IRange range3 = rangeGetter.GetRange((IWorkbook) workbook, worksheet);
            if (range3 != null)
              rangesCollection.Add(range3);
          }
        }
        range1 = rangesCollection.Count < 1 ? (IRange) null : (IRange) rangesCollection;
      }
    }
    return range1;
  }

  private void ParseLegendEntry(
    XmlReader reader,
    IOfficeChartLegend legend,
    Excel2007Parser parser)
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
            IInternalOfficeChartTextArea textArea = legend.LegendEntries[iIndex].TextArea as IInternalOfficeChartTextArea;
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
            string valueTag3 = ChartParserCommon.ParseValueTag(reader);
            record.RotationAngle = ushort.Parse(valueTag3);
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
    IOfficeChartErrorBars errorBars = (IOfficeChartErrorBars) null;
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
            errorBars.Include = (OfficeErrorBarInclude) Enum.Parse(typeof (OfficeErrorBarInclude), valueTag, true);
            continue;
          case "errValType":
            Excel2007ErrorBarType excel2007ErrorBarType = (Excel2007ErrorBarType) Enum.Parse(typeof (Excel2007ErrorBarType), ChartParserCommon.ParseValueTag(reader), false);
            errorBars.Type = (OfficeErrorBarType) excel2007ErrorBarType;
            continue;
          case "noEndCap":
            errorBars.HasCap = !ChartParserCommon.ParseBoolValueTag(reader);
            continue;
          case "plus":
            IRange errorBarRange1 = this.ParseErrorBarRange(reader, (IWorkbook) parentBook, out values, errorBars, series, "plus");
            if (!((ChartErrorBarsImpl) errorBars).IsPlusNumberLiteral && errorBars.Include != OfficeErrorBarInclude.Minus)
              errorBars.PlusRange = (series.ParentChart.ChartData as ChartData)[errorBarRange1];
            if (chartErrorBarsImpl == null & errorBars != null)
              chartErrorBarsImpl = errorBars as ChartErrorBarsImpl;
            chartErrorBarsImpl.PlusRangeValues = values;
            continue;
          case "minus":
            IRange errorBarRange2 = this.ParseErrorBarRange(reader, (IWorkbook) parentBook, out values, errorBars, series, "minus");
            if (!((ChartErrorBarsImpl) errorBars).IsMinusNumberLiteral && errorBars.Include != OfficeErrorBarInclude.Plus)
              errorBars.MinusRange = (series.ParentChart.ChartData as ChartData)[errorBarRange2];
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

  private void CheckCustomErrorBarType(IOfficeChartErrorBars errorBars)
  {
    if (errorBars.Type != OfficeErrorBarType.Custom || ((ChartErrorBarsImpl) errorBars).IsPlusNumberLiteral && ((ChartErrorBarsImpl) errorBars).IsMinusNumberLiteral || errorBars.MinusRange == null || errorBars.MinusRange == null)
      return;
    errorBars.Include = OfficeErrorBarInclude.Both;
  }

  private IRange ParseErrorBarRange(
    XmlReader reader,
    IWorkbook book,
    out object[] values,
    IOfficeChartErrorBars errorBars,
    ChartSerieImpl series,
    string tagName)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    bool flag = false;
    if (reader.LocalName == "plus")
      flag = true;
    reader.Read();
    string strFormula = (string) null;
    values = (object[]) null;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "numRef")
        strFormula = this.ParseNumReference(reader, out values, series, tagName);
      else if (reader.LocalName == "numLit")
      {
        if (flag)
          ((ChartErrorBarsImpl) errorBars).IsPlusNumberLiteral = true;
        else
          ((ChartErrorBarsImpl) errorBars).IsMinusNumberLiteral = true;
        string empty = string.Empty;
        values = this.ParseDirectlyEnteredValues(reader, series, "numLit");
      }
    }
    reader.Read();
    if (strFormula == null)
      return (IRange) null;
    FormulaUtil formulaUtil = (book as WorkbookImpl).DataHolder.Parser.FormulaUtil;
    return !ChartImpl.TryAndModifyToValidFormula(strFormula) ? (IRange) null : (formulaUtil.ParseString(strFormula)[0] as IRangeGetter).GetRange(book, book.Worksheets[0]);
  }

  private void ParseTrendlines(
    XmlReader reader,
    ChartSerieImpl series,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (series == null)
      throw new ArgumentNullException(nameof (series));
label_7:
    while (reader.LocalName == "trendline")
    {
      this.ParseTrendline(reader, series, relations);
      while (true)
      {
        if (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.Element)
          reader.Read();
        else
          goto label_7;
      }
    }
  }

  private void ParseTrendline(
    XmlReader reader,
    ChartSerieImpl series,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    if (reader.LocalName != "trendline")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    IOfficeChartTrendLine trendline = series.TrendLines.Add();
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
            trendline.Type = (OfficeTrendLineType) excel2007TrendlineType;
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
            this.ParseTrendlineLabel(reader, trendline);
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

  private void ParseTrendlineLabel(XmlReader reader, IOfficeChartTrendLine trendline)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (trendline == null)
      throw new ArgumentNullException(nameof (trendline));
    if (reader.LocalName != "trendlineLbl")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
        reader.Skip();
      else
        reader.Skip();
    }
    reader.Read();
  }

  private void ParseSurface(
    XmlReader reader,
    IOfficeChartWallOrFloor surface,
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
              string valueTag = ChartParserCommon.ParseValueTag(reader);
              ((ChartWallOrFloorImpl) surface).Thickness = (uint) int.Parse(valueTag);
              continue;
            case "pictureOptions":
              reader.Read();
              int num = reader.LocalName == "pictureFormat" ? 1 : 0;
              if (ChartParserCommon.ParseValueTag(reader) == OfficeChartPictureType.stack.ToString())
                ((ChartWallOrFloorImpl) surface).PictureUnit = OfficeChartPictureType.stack;
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
    IOfficeChartFrameFormat plotArea1 = chart.PlotArea;
    FileDataHolder parentHolder = chart.DataHolder.ParentHolder;
    Dictionary<int, int> dictSeriesAxis = new Dictionary<int, int>();
    bool flag = chart.PlotArea != null && chart.PlotArea.IsBorderCornersRound;
    chart.HasPlotArea = false;
    List<int> intList = (List<int>) null;
    int num1 = 0;
    int num2 = 0;
    while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (reader.LocalName == "catAx")
          ++num1;
        else if (reader.LocalName == "valAx")
          ++num2;
        switch (reader.LocalName)
        {
          case "layout":
            if (chart.PlotArea == null)
              chart.PlotArea = (IOfficeChartFrameFormat) new ChartPlotAreaImpl(this.m_book.Application, (object) chart);
            chart.PlotArea.Layout = (IOfficeChartLayout) new ChartLayoutImpl(this.m_book.Application, (object) chart.PlotArea, (object) chart);
            ChartParserCommon.ParseChartLayout(reader, chart.PlotArea.Layout);
            continue;
          case "dTable":
            this.ParseDataTable(reader, chart);
            continue;
          case "spPr":
            chart.HasPlotArea = true;
            IOfficeChartFrameFormat plotArea2 = chart.PlotArea;
            chart.PlotArea.IsBorderCornersRound = flag;
            ChartFillObjectGetterAny objectGetter = new ChartFillObjectGetterAny(plotArea2.Border as ChartBorderImpl, plotArea2.Interior as ChartInteriorImpl, plotArea2.Fill as IInternalFill, plotArea2.Shadow as ShadowImpl, plotArea2.ThreeD as ThreeDFormatImpl);
            ChartParserCommon.ParseShapeProperties(reader, (IChartFillObjectGetter) objectGetter, parentHolder, relations);
            continue;
          case "barChart":
            this.ParseBarChart(reader, chart, relations, dictSeriesAxis);
            continue;
          case "bar3DChart":
            this.ParseBar3DChart(reader, chart, relations, dictSeriesAxis);
            continue;
          case "areaChart":
            this.ParseAreaChart(reader, chart, relations, dictSeriesAxis);
            continue;
          case "area3DChart":
            this.ParseArea3DChart(reader, chart, relations, dictSeriesAxis);
            continue;
          case "lineChart":
            if (intList == null)
              intList = new List<int>();
            this.ParseLineChart(reader, chart, relations, dictSeriesAxis, parser, intList);
            continue;
          case "line3DChart":
            this.ParseLine3DChart(reader, chart, relations, dictSeriesAxis, parser);
            continue;
          case "bubbleChart":
          case "bubble3D":
            this.ParseBubbleChart(reader, chart, relations, dictSeriesAxis);
            continue;
          case "surfaceChart":
            this.ParseSurfaceChart(reader, chart, relations, dictSeriesAxis);
            continue;
          case "surface3DChart":
            this.ParseSurfaceChart(reader, chart, relations, dictSeriesAxis);
            continue;
          case "radarChart":
            this.ParseRadarChart(reader, chart, relations, dictSeriesAxis, parser);
            continue;
          case "scatterChart":
            this.ParseScatterChart(reader, chart, relations, dictSeriesAxis, parser);
            continue;
          case "pieChart":
            this.ParsePieChart(reader, chart, relations, dictSeriesAxis);
            continue;
          case "pie3DChart":
            this.ParsePie3DChart(reader, chart, relations, dictSeriesAxis);
            continue;
          case "doughnutChart":
            this.ParseDoughnutChart(reader, chart, relations, dictSeriesAxis);
            continue;
          case "ofPieChart":
            this.ParseOfPieChart(reader, chart, relations, dictSeriesAxis);
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
    if (num1 > 1)
      chart.IsPrimarySecondaryCategory = true;
    if (num2 > 1)
      chart.IsPrimarySecondaryValue = true;
    ChartSeriesCollection series1 = (ChartSeriesCollection) chart.Series;
    ChartSeriesCollection source = (chart.Series as ChartSeriesCollection).Clone(chart.Series.Parent) as ChartSeriesCollection;
    chart.ChartSerieGroupsBeforesorting = source.Where<IOfficeChartSerie>((Func<IOfficeChartSerie, bool>) (x => !x.IsFiltered)).GroupBy<IOfficeChartSerie, int, IOfficeChartSerie>((Func<IOfficeChartSerie, int>) (x => (x as ChartSerieImpl).ChartGroup), (Func<IOfficeChartSerie, IOfficeChartSerie>) (x => x));
    series1.ResortSeries(dictSeriesAxis, intList);
    if (chart.Series.Count > 0 && chart.Series[0].SerieType == OfficeChartType.Bubble)
      chart.CheckIsBubble3D();
    if (chart.CommonDataPointsCollection != null && chart.CommonDataPointsCollection.Count > 0)
      this.ChangeKeyToChartGroup(chart);
    if (chart.Series == null || chart.Series.Count <= 0)
      return;
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

  private void ParsePlotAreaAxes(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    Excel2007Parser parser)
  {
    ChartAxisParser chartAxisParser = new ChartAxisParser(this.m_book);
    IOfficeChartFrameFormat plotArea = chart.PlotArea;
    FileDataHolder parentHolder = chart.DataHolder.ParentHolder;
    Dictionary<int, int> dictionary = new Dictionary<int, int>();
    int num1 = 0;
    OfficeChartType chartType = chart.ChartType;
    int num2 = 0;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "valAx":
            bool bPrimary1 = num1 <= 1;
            chart.CreateNecessaryAxes(bPrimary1);
            ChartValueAxisImpl valueAxis = bPrimary1 ? (ChartValueAxisImpl) chart.PrimaryValueAxis : (ChartValueAxisImpl) chart.SecondaryValueAxis;
            if (num1 > num2 * 2 - 1 && chartType == OfficeChartType.Scatter_Markers)
              chartType = OfficeChartType.Combination_Chart;
            chartAxisParser.ParseValueAxis(reader, valueAxis, relations, chartType, parser);
            ++num1;
            continue;
          case "serAx":
            chart.CreateNecessaryAxes(true);
            ChartSeriesAxisImpl seriesAxis = (ChartSeriesAxisImpl) chart.PrimarySerieAxis ?? chart.CreatePrimarySeriesAxis();
            chartAxisParser.ParseSeriesAxis(reader, seriesAxis, relations, chartType, parser);
            continue;
          case "catAx":
            bool bPrimary2 = num1 <= 1;
            chart.CreateNecessaryAxes(bPrimary2);
            ChartCategoryAxisImpl axis1 = num1 <= 1 ? (ChartCategoryAxisImpl) chart.PrimaryCategoryAxis : (ChartCategoryAxisImpl) chart.SecondaryCategoryAxis;
            chartAxisParser.ParseCategoryAxis(reader, axis1, relations, chartType, parser);
            ++num1;
            continue;
          case "dateAx":
            bool bPrimary3 = num1 <= 1;
            chart.CreateNecessaryAxes(bPrimary3);
            ChartCategoryAxisImpl axis2 = bPrimary3 ? (ChartCategoryAxisImpl) chart.PrimaryCategoryAxis : (ChartCategoryAxisImpl) chart.SecondaryCategoryAxis;
            chartAxisParser.ParseDateAxis(reader, axis2, relations, chartType, parser);
            ++num1;
            continue;
          case "bubbleChart":
            chartType = OfficeChartType.Bubble;
            reader.Skip();
            continue;
          case "scatterChart":
            chartType = OfficeChartType.Scatter_Markers;
            ++num2;
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
    Dictionary<int, int> dictSeriesAxis)
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
    IOfficeChartSerie barChartShared = (IOfficeChartSerie) this.ParseBarChartShared(reader, chart, relations, false, lstSeries, out shape);
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
    IOfficeChartFormat commonSerieOptions = barChartShared?.SerieFormat.CommonSerieOptions;
    if (nullable1.HasValue && barChartShared != null)
      commonSerieOptions.GapWidth = nullable1.Value;
    if (nullable2.HasValue && barChartShared != null)
      commonSerieOptions.Overlap = nullable2.Value;
    reader.Read();
  }

  private IOfficeChartSerie ParseFilteredSeries(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    bool is3D,
    OfficeChartType SeriesType,
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
            case OfficeChartType.Line:
            case OfficeChartType.Line_Stacked:
            case OfficeChartType.Line_Stacked_100:
            case OfficeChartType.Line_Markers:
            case OfficeChartType.Line_Markers_Stacked:
            case OfficeChartType.Line_Markers_Stacked_100:
            case OfficeChartType.Line_3D:
              filteredSeries = this.ParseLineSeries(reader, chart, SeriesType, relations, parentHolder.Parser);
              break;
            case OfficeChartType.Pie:
            case OfficeChartType.Pie_3D:
            case OfficeChartType.PieOfPie:
            case OfficeChartType.Pie_Exploded:
            case OfficeChartType.Pie_Exploded_3D:
            case OfficeChartType.Pie_Bar:
              filteredSeries = this.ParsePieSeries(reader, chart, SeriesType, relations);
              break;
            case OfficeChartType.Scatter_Markers:
            case OfficeChartType.Scatter_SmoothedLine_Markers:
            case OfficeChartType.Scatter_SmoothedLine:
            case OfficeChartType.Scatter_Line_Markers:
            case OfficeChartType.Scatter_Line:
              filteredSeries = this.ParseScatterSeries(reader, chart, SeriesType, relations, parentHolder.Parser);
              break;
            case OfficeChartType.Area:
            case OfficeChartType.Area_Stacked:
            case OfficeChartType.Area_Stacked_100:
            case OfficeChartType.Area_3D:
            case OfficeChartType.Area_Stacked_3D:
            case OfficeChartType.Area_Stacked_100_3D:
              filteredSeries = this.ParseAreaSeries(reader, chart, SeriesType, relations, !secondary);
              break;
            case OfficeChartType.Radar:
            case OfficeChartType.Radar_Markers:
            case OfficeChartType.Radar_Filled:
              filteredSeries = this.ParseRadarSeries(reader, chart, SeriesType, relations, parentHolder.Parser);
              break;
            case OfficeChartType.Surface_3D:
            case OfficeChartType.Surface_NoColor_3D:
            case OfficeChartType.Surface_Contour:
            case OfficeChartType.Surface_NoColor_Contour:
              filteredSeries = this.ParseSurfaceSeries(reader, chart, SeriesType, relations);
              break;
            case OfficeChartType.Bubble:
            case OfficeChartType.Bubble_3D:
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
    return (IOfficeChartSerie) filteredSeries;
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

  public void ParseBar3DChart(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    Dictionary<int, int> dictSeriesAxis)
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
    ChartSerieImpl barChartShared = this.ParseBarChartShared(reader, chart, relations, true, lstSeries, out shape);
    int? nullable = new int?();
    if (shape != null)
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
            if (reader.MoveToAttribute("val"))
            {
              chart.GapDepth = ChartParserCommon.ParseIntValueTag(reader);
              continue;
            }
            reader.Skip();
            continue;
          case "axId":
            this.ParseAxisId(reader, lstSeries, dictSeriesAxis);
            continue;
          case "extLst":
            this.ParseFilteredSeries(reader, chart, relations, true, barChartShared.SerieType, false);
            continue;
          case "shape":
            this.ParseBarShape(ChartParserCommon.ParseValueTag(reader), chart.ChartFormat.SerieDataFormat);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    if (nullable.HasValue)
      barChartShared.SerieFormat.CommonSerieOptions.GapWidth = nullable.Value;
    foreach (ChartSerieImpl chartSerieImpl in (CollectionBase<IOfficeChartSerie>) chart.Series)
    {
      if (!chartSerieImpl.HasColumnShape)
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

  private void ParseBarShape(string value, IOfficeChartSerieDataFormat dataFormat)
  {
    if (value == null)
      return;
    if (dataFormat == null)
      throw new ArgumentNullException(nameof (dataFormat));
    switch (value)
    {
      case "cone":
        dataFormat.BarShapeTop = OfficeTopFormat.Sharp;
        dataFormat.BarShapeBase = OfficeBaseFormat.Circle;
        break;
      case "pyramid":
        dataFormat.BarShapeTop = OfficeTopFormat.Sharp;
        dataFormat.BarShapeBase = OfficeBaseFormat.Rectangle;
        break;
      case "coneToMax":
        dataFormat.BarShapeTop = OfficeTopFormat.Trunc;
        dataFormat.BarShapeBase = OfficeBaseFormat.Circle;
        break;
      case "pyramidToMax":
        dataFormat.BarShapeTop = OfficeTopFormat.Trunc;
        dataFormat.BarShapeBase = OfficeBaseFormat.Rectangle;
        break;
      case "cylinder":
        dataFormat.BarShapeTop = OfficeTopFormat.Straight;
        dataFormat.BarShapeBase = OfficeBaseFormat.Circle;
        break;
      case "box":
        dataFormat.BarShapeTop = OfficeTopFormat.Straight;
        dataFormat.BarShapeBase = OfficeBaseFormat.Rectangle;
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
    out string shape)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    bool flag1 = true;
    ChartSerieImpl series = (ChartSerieImpl) null;
    string str1 = (string) null;
    bool flag2 = this._appVersion == 0.0 || this._appVersion > 12.0;
    string str2 = (string) null;
    shape = (string) null;
    MemoryStream data1 = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((Stream) data1, Encoding.UTF8);
    writer.WriteStartElement("root");
    Stream data2 = (Stream) null;
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
              data2 = ShapeParser.ReadNodeAsStream(reader);
              continue;
            }
            reader.Skip();
            continue;
          case "gapWidth":
            if (data1.Length == 0L)
            {
              nullable = new int?(int.Parse(ChartParserCommon.ParseValueTag(reader)));
              chart.GapWidth = nullable.Value;
              chart.ShowGapWidth = true;
              continue;
            }
            flag1 = false;
            continue;
          case "overlap":
            reader.Skip();
            continue;
          default:
            if (!is3D && data1.Length == 0L)
            {
              OfficeChartType pivotBarSeriesType = this.GetPivotBarSeriesType(str1, str2, shape, is3D);
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
    data1.Position = 0L;
    XmlReader reader1 = UtilityMethods.CreateReader((Stream) data1);
    if (!reader1.IsEmptyElement)
    {
      reader1.Read();
      this.ParseSeries(reader1, str1, str2, shape, is3D, lstSeries, chart, relations, ref series);
      if (series != null)
        series.Grouping = str2;
      series.SerieFormat.CommonSerieOptions.IsVaryColor = flag2;
    }
    reader1.Close();
    writer.Close();
    if (data2 != null && series != null)
    {
      data2.Position = 0L;
      XmlReader reader2 = UtilityMethods.CreateReader(data2);
      this.ParseDataLabels(reader2, chart, relations, series.Number);
      reader2.Close();
    }
    return series;
  }

  private ChartSerieImpl ParseFilterSecondaryAxis(
    XmlReader reader,
    OfficeChartType seriesType,
    bool is3D,
    List<ChartSerieImpl> lstSeries,
    ChartImpl chart,
    RelationCollection relations,
    ref ChartSerieImpl series)
  {
    ChartSerieImpl filterSecondaryAxis = (ChartSerieImpl) null;
    int? nullable = new int?();
    Dictionary<int, int> dictionary = new Dictionary<int, int>();
    IOfficeChartSerie officeChartSerie = (IOfficeChartSerie) null;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "gapWidth":
            nullable = new int?(int.Parse(ChartParserCommon.ParseValueTag(reader)));
            chart.GapWidth = nullable.Value;
            chart.ShowGapWidth = true;
            continue;
          case "overlap":
            string valueTag = ChartParserCommon.ParseValueTag(reader);
            chart.OverLap = int.Parse(valueTag);
            continue;
          case "axId":
            reader.Skip();
            continue;
          case "extLst":
            officeChartSerie = this.ParseFilteredSeries(reader, chart, relations, true, seriesType, true);
            officeChartSerie.UsePrimaryAxis = false;
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
    }
    series = officeChartSerie as ChartSerieImpl;
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
        OfficeChartType pivotBarSeriesType = this.GetPivotBarSeriesType(strDirection, strGrouping, shape, is3D);
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
    IOfficeChartCategories categories,
    string filteredcategory,
    string fullreference,
    IOfficeChartSerie series1,
    bool isseries)
  {
    IRange range1 = this.FindRange(series1, fullreference);
    int num1 = isseries ? range1.Column : range1.Row;
    int num2 = isseries ? range1.LastColumn : range1.LastRow;
    filteredcategory = filteredcategory.Trim('(');
    filteredcategory = filteredcategory.Trim(')');
    string[] strArray = filteredcategory.Split(',');
    int[] numArray = new int[range1.Count];
    int index1 = 0;
    for (int index2 = 0; index2 < strArray.Length; ++index2)
    {
      IRange range2 = this.FindRange(series1, strArray[index2]);
      int num3 = isseries ? range2.Column : range2.Row;
      int num4 = isseries ? range2.LastColumn : range2.LastRow;
      for (int index3 = num3; index3 <= num4; ++index3)
      {
        numArray[index1] = index3;
        ++index1;
      }
    }
    int index4 = 0;
    int index5 = 0;
    for (int index6 = num1; index6 <= num2; ++index6)
    {
      if (index6 != numArray[index4] && numArray[index4] != 0)
        categories[index5].IsFiltered = true;
      if (index6 == numArray[index4] && numArray[index4] != 0)
        ++index4;
      else
        categories[index5].IsFiltered = true;
      ++index5;
    }
  }

  private IRange FindRange(IOfficeChartSerie series1, string strValue)
  {
    IRange range = (IRange) null;
    bool flag1 = true;
    bool flag2 = false;
    bool flag3 = false;
    ChartSerieImpl chartSerieImpl = series1 as ChartSerieImpl;
    WorkbookImpl parentBook1 = chartSerieImpl.ParentBook;
    if (strValue != null)
    {
      WorkbookImpl parentBook2 = chartSerieImpl.ParentBook;
      FormulaUtil formulaUtil = parentBook2.DataHolder.Parser.FormulaUtil;
      if (!ChartImpl.TryAndModifyToValidFormula(strValue))
        return (IRange) null;
      range = (formulaUtil.ParseString(strValue)[0] as IRangeGetter).GetRange((IWorkbook) parentBook2, parentBook2.Worksheets[0]);
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
      IRange range1 = bIsInRow ? range[num2, index] : range[index, num2];
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
    IRange serieOrAxisRange = bIsInRow ? range[num1, num3, num2, num5] : range[num3, num1, num5, num2];
    serieRange = bIsInRow ? range[range.Row, serieOrAxisRange.LastColumn + 1, range.LastRow, range.LastColumn] : range[serieOrAxisRange.LastRow + 1, range.Column, range.LastRow, range.LastColumn];
    return serieOrAxisRange;
  }

  private bool DetectIsInRow(IRange range)
  {
    return range == null || range.LastRow - range.Row <= range.LastColumn - range.Column;
  }

  private OfficeChartType GetBarSeriesType(
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
    return (OfficeChartType) Enum.Parse(typeof (OfficeChartType), str2, false);
  }

  private OfficeChartType GetPivotBarSeriesType(
    string direction,
    string grouping,
    string shape,
    bool is3D)
  {
    string str1 = (string) null;
    string[] array = new string[3]
    {
      "Cone",
      "Cylinder",
      "Pyramid"
    };
    if (shape != null)
    {
      if (Array.IndexOf<string>(array, shape) == -1)
        str1 = direction == "bar" ? "Bar" : "Column";
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
    return (OfficeChartType) Enum.Parse(typeof (OfficeChartType), str3, true);
  }

  private OfficeChartType GetAreaSeriesType(string grouping, bool is3D)
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
    return (OfficeChartType) Enum.Parse(typeof (OfficeChartType), str, false);
  }

  private OfficeChartType GetLineSeriesType(string grouping, bool is3D)
  {
    string str = "Line";
    OfficeChartType lineSeriesType;
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
      lineSeriesType = (OfficeChartType) Enum.Parse(typeof (OfficeChartType), str, false);
    }
    else
      lineSeriesType = OfficeChartType.Line_3D;
    return lineSeriesType;
  }

  private void ParseArea3DChart(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    Dictionary<int, int> dictSeriesAxis)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "area3DChart")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    List<ChartSerieImpl> lstSeries = new List<ChartSerieImpl>();
    this.ParseAreaChartCommon(reader, chart, true, relations, lstSeries, true);
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
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
    Dictionary<int, int> dictSeriesAxis)
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
    this.ParseAreaChartCommon(reader, chart, false, relations, lstSeries, !axisType);
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "axId")
      {
        if (this.ParseAxisId(reader, lstSeries, dictSeriesAxis))
          secondary = true;
      }
      else if (reader.LocalName == "extLst")
      {
        if (chart.Series.Count > 0 && chart.Series[0] != null)
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
    bool isPrimary)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    bool flag = true;
    string grouping = (string) null;
    ChartSerieImpl series = (ChartSerieImpl) null;
    while (reader.NodeType != XmlNodeType.EndElement && flag)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "grouping":
            grouping = ChartParserCommon.ParseValueTag(reader);
            continue;
          case "varyColors":
            ChartParserCommon.ParseBoolValueTag(reader);
            continue;
          case "ser":
            OfficeChartType areaSeriesType = this.GetAreaSeriesType(grouping, b3D);
            series = this.ParseAreaSeries(reader, chart, areaSeriesType, relations, isPrimary);
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
              flag = false;
              continue;
            }
            OfficeChartType lineSeriesType = this.GetLineSeriesType(grouping, b3D);
            this.ParseFilterSecondaryAxis(reader, lineSeriesType, b3D, lstSeries, chart, relations, ref series);
            flag = false;
            continue;
        }
      }
      else
        reader.Read();
    }
  }

  private ChartSerieImpl ParseLineChartCommon(
    XmlReader reader,
    ChartImpl chart,
    bool is3D,
    RelationCollection relations,
    List<ChartSerieImpl> lstSeries,
    Excel2007Parser parser)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    bool flag = true;
    string grouping = (string) null;
    ChartSerieImpl lineChartCommon = (ChartSerieImpl) null;
    ChartSerieImpl series = (ChartSerieImpl) null;
    while (reader.NodeType != XmlNodeType.EndElement && flag)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "grouping":
            grouping = ChartParserCommon.ParseValueTag(reader);
            continue;
          case "varyColors":
            ChartParserCommon.ParseBoolValueTag(reader);
            continue;
          case "ser":
            OfficeChartType lineSeriesType1 = this.GetLineSeriesType(grouping, is3D);
            series = this.ParseLineSeries(reader, chart, lineSeriesType1, relations, parser);
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
          default:
            if (lstSeries.Count != 0)
            {
              flag = false;
              continue;
            }
            if (reader.LocalName == "axId")
            {
              reader.Skip();
              continue;
            }
            OfficeChartType lineSeriesType2 = this.GetLineSeriesType(grouping, is3D);
            this.ParseFilterSecondaryAxis(reader, lineSeriesType2, is3D, lstSeries, chart, relations, ref series);
            flag = false;
            continue;
        }
      }
      else
        reader.Skip();
    }
    return lineChartCommon;
  }

  private void ParseLine3DChart(
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
    if (reader.LocalName != "line3DChart")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    List<ChartSerieImpl> lstSeries = new List<ChartSerieImpl>();
    this.ParseLineChartCommon(reader, chart, true, relations, lstSeries, parser);
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "axId")
        this.ParseAxisId(reader, lstSeries, dictSeriesAxis);
      else if (reader.LocalName == "extLst")
        this.ParseFilteredSeries(reader, chart, relations, true, OfficeChartType.Line_3D, false);
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
    List<int> markerArray)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    ++chart.LineChartCount;
    reader.Read();
    List<ChartSerieImpl> lstSeries = new List<ChartSerieImpl>();
    ChartSerieImpl lineChartCommon = this.ParseLineChartCommon(reader, chart, false, relations, lstSeries, parser);
    bool secondary = false;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "upDownBars":
            this.ParseUpDownBars(reader, lineChartCommon, relations);
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
    Dictionary<int, int> dictSeriesAxis)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "bubbleChart")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    ChartSerieImpl chartSerieImpl = (ChartSerieImpl) null;
    List<ChartSerieImpl> lstSeries = new List<ChartSerieImpl>();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "varyColors":
            ChartParserCommon.ParseBoolValueTag(reader);
            continue;
          case "ser":
            chartSerieImpl = this.ParseBubbleSeries(reader, chart, relations);
            lstSeries.Add(chartSerieImpl);
            continue;
          case "bubbleScale":
            int intValueTag = ChartParserCommon.ParseIntValueTag(reader);
            chartSerieImpl.SerieFormat.CommonSerieOptions.BubbleScale = intValueTag;
            continue;
          case "showNegBubbles":
            bool boolValueTag = ChartParserCommon.ParseBoolValueTag(reader);
            chartSerieImpl.SerieFormat.CommonSerieOptions.ShowNegativeBubbles = boolValueTag;
            continue;
          case "sizeRepresents":
            string valueTag = ChartParserCommon.ParseValueTag(reader);
            chartSerieImpl.SerieFormat.CommonSerieOptions.SizeRepresents = valueTag == "area" ? ChartBubbleSize.Area : ChartBubbleSize.Width;
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
    reader.Read();
  }

  private void ParseSurfaceChart(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    Dictionary<int, int> dictSeriesAxis)
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
    this.ParseSurfaceCommon(reader, chart, is3D, relations, lstSeries);
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
    List<ChartSerieImpl> lstSeries)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    OfficeChartType officeChartType = OfficeChartType.Surface_3D;
    bool flag = true;
    bool bWireframe = false;
    while (reader.NodeType != XmlNodeType.EndElement && flag)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "wireframe":
            bWireframe = ChartParserCommon.ParseBoolValueTag(reader);
            continue;
          case "ser":
            officeChartType = this.GetSurfaceSeriesType(bWireframe, is3D);
            ChartSerieImpl surfaceSeries = this.ParseSurfaceSeries(reader, chart, officeChartType, relations);
            lstSeries.Add(surfaceSeries);
            continue;
          case "bandFmts":
            this.ParseBandFormats(reader, chart);
            continue;
          case "extLst":
            this.ParseFilteredSeries(reader, chart, relations, true, officeChartType, false);
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

  private void ParseBandFormats(XmlReader reader, ChartImpl chart)
  {
    chart.PreservedBandFormats = ShapeParser.ReadNodeAsStream(reader);
  }

  private OfficeChartType GetSurfaceSeriesType(bool bWireframe, bool is3D)
  {
    return !bWireframe ? (is3D ? OfficeChartType.Surface_3D : OfficeChartType.Surface_Contour) : (is3D ? OfficeChartType.Surface_NoColor_3D : OfficeChartType.Surface_NoColor_Contour);
  }

  private void ParseRadarChart(
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
    if (reader.LocalName != "radarChart")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    OfficeChartType officeChartType = OfficeChartType.Radar;
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
            officeChartType = (OfficeChartType) Enum.Parse(typeof (Excel2007RadarStyle), valueTag, false);
            chart.RadarStyle = valueTag;
            continue;
          case "varyColors":
            flag = ChartParserCommon.ParseBoolValueTag(reader);
            continue;
          case "ser":
            ChartSerieImpl radarSeries = this.ParseRadarSeries(reader, chart, officeChartType, relations, parser);
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
            this.ParseFilteredSeries(reader, chart, relations, false, officeChartType, secondary);
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
    reader.Read();
  }

  private void ParseScatterChart(
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
    if (reader.LocalName != "scatterChart")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    OfficeChartType officeChartType = OfficeChartType.Scatter_Markers;
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
            officeChartType = (OfficeChartType) Enum.Parse(typeof (Excel2007ScatterStyle), ChartParserCommon.ParseValueTag(reader), false);
            continue;
          case "varyColors":
            flag = ChartParserCommon.ParseBoolValueTag(reader);
            continue;
          case "ser":
            series = this.ParseScatterSeries(reader, chart, officeChartType, relations, parser);
            lstSeries.Add(series);
            if (flag)
            {
              series.SerieFormat.CommonSerieOptions.IsVaryColor = true;
              continue;
            }
            continue;
          case "upDownBars":
            this.ParseUpDownBars(reader, series, relations);
            continue;
          case "axId":
            if (this.ParseAxisId(reader, lstSeries, dictSeriesAxis))
            {
              secondary = true;
              continue;
            }
            continue;
          case "extLst":
            this.ParseFilteredSeries(reader, chart, relations, false, officeChartType, secondary);
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
    reader.Read();
  }

  private void ParsePieChart(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    Dictionary<int, int> dictSeriesAxis)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "pieChart")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    List<ChartSerieImpl> lstSeries = new List<ChartSerieImpl>();
    ChartSerieImpl pieCommon = this.ParsePieCommon(reader, chart, OfficeChartType.Pie, relations, lstSeries);
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element && pieCommon != null)
      {
        switch (reader.LocalName)
        {
          case "firstSliceAng":
            pieCommon.SerieFormat.CommonSerieOptions.FirstSliceAngle = ChartParserCommon.ParseIntValueTag(reader);
            continue;
          case "axId":
            this.ParseAxisId(reader, lstSeries, dictSeriesAxis);
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

  private void ParsePie3DChart(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    Dictionary<int, int> dictSeriesAxis)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "pie3DChart")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    List<ChartSerieImpl> lstSeries = new List<ChartSerieImpl>();
    this.ParsePieCommon(reader, chart, OfficeChartType.Pie_3D, relations, lstSeries);
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "axId")
        this.ParseAxisId(reader, lstSeries, dictSeriesAxis);
      else
        reader.Skip();
    }
    reader.Read();
  }

  private void ParseOfPieChart(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    Dictionary<int, int> dictSeriesAxis)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "ofPieChart")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    OfficeChartType seriesType = OfficeChartType.PieOfPie;
    ChartSerieImpl chartSerieImpl = (ChartSerieImpl) null;
    List<ChartSerieImpl> lstSeries = new List<ChartSerieImpl>();
    int? nullable = new int?();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "ofPieType":
            seriesType = ChartParserCommon.ParseValueTag(reader) == "pie" ? OfficeChartType.PieOfPie : OfficeChartType.Pie_Bar;
            continue;
          case "varyColors":
          case "ser":
          case "dLbls":
            chartSerieImpl = this.ParsePieCommon(reader, chart, seriesType, relations, lstSeries);
            continue;
          case "gapWidth":
            nullable = new int?(ChartParserCommon.ParseIntValueTag(reader));
            continue;
          case "splitType":
            Excel2007SplitType excel2007SplitType = (Excel2007SplitType) Enum.Parse(typeof (Excel2007SplitType), ChartParserCommon.ParseValueTag(reader), false);
            chartSerieImpl.SerieFormat.CommonSerieOptions.SplitType = (OfficeSplitType) excel2007SplitType;
            continue;
          case "splitPos":
            chartSerieImpl.SerieFormat.CommonSerieOptions.SplitValue = ChartParserCommon.ParseIntValueTag(reader);
            continue;
          case "secondPieSize":
            chartSerieImpl.SerieFormat.CommonSerieOptions.PieSecondSize = ChartParserCommon.ParseIntValueTag(reader);
            continue;
          case "axId":
            this.ParseAxisId(reader, lstSeries, dictSeriesAxis);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    if (nullable.HasValue)
      chartSerieImpl.SerieFormat.CommonSerieOptions.GapWidth = nullable.Value;
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
            series = this.ParseLineSeries(reader, chart, OfficeChartType.Line, relations, parser);
            lstSeries.Add(series);
            continue;
          case "hiLowLines":
          case "dropLines":
            this.ParseLines(reader, chart, series, reader.LocalName);
            continue;
          case "upDownBars":
            this.ParseUpDownBars(reader, series, relations);
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
                    parent.HighLowLines = (IOfficeChartBorder) border;
                    continue;
                  case "dropLines":
                    parent.DropLines = (IOfficeChartBorder) border;
                    continue;
                  case "serLines":
                    parent.PieSeriesLine = (IOfficeChartBorder) border;
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
    Dictionary<int, int> dictSeriesAxis)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "doughnutChart")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    List<ChartSerieImpl> lstSeries = new List<ChartSerieImpl>();
    IOfficeChartFormat officeChartFormat = (IOfficeChartFormat) null;
    ChartSerieImpl pieCommon = this.ParsePieCommon(reader, chart, OfficeChartType.Doughnut, relations, lstSeries);
    if (pieCommon != null)
      officeChartFormat = reader.NodeType != XmlNodeType.EndElement ? pieCommon.SerieFormat.CommonSerieOptions : (IOfficeChartFormat) null;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element && pieCommon != null)
      {
        switch (reader.LocalName)
        {
          case "firstSliceAng":
            officeChartFormat.FirstSliceAngle = ChartParserCommon.ParseIntValueTag(reader);
            continue;
          case "holeSize":
            officeChartFormat.DoughnutHoleSize = ChartParserCommon.ParseIntValueTag(reader);
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
    reader.Read();
  }

  private ChartSerieImpl ParsePieCommon(
    XmlReader reader,
    ChartImpl chart,
    OfficeChartType seriesType,
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
    if (isChartExSeries)
    {
      if (reader.LocalName != "dataLabels")
        throw new XmlException("Unexpected xml tag.");
      if (reader.MoveToAttribute("pos"))
        series.DataPoints.DefaultDataPoint.DataLabels.Position = (OfficeDataLabelPosition) Enum.Parse(typeof (Excel2007DataLabelPos), reader.Value, false);
      series.DataPoints.DefaultDataPoint.DataLabels.FrameFormat.Interior.UseAutomaticFormat = true;
      series.DataPoints.DefaultDataPoint.DataLabels.FrameFormat.Border.AutoFormat = true;
    }
    else if (reader.LocalName != "dLbls")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "dLbl":
            this.ParseDataLabel(reader, series, relations, isChartExSeries);
            continue;
          default:
            IOfficeChartDataLabels dataLabels = series.DataPoints.DefaultDataPoint.DataLabels;
            FileDataHolder dataHolder = series.ParentBook.DataHolder;
            Excel2007Parser parser = dataHolder.Parser;
            if (isChartExSeries)
              dataLabels.IsLegendKey = false;
            this.ParseDataLabelSettings(reader, dataLabels, parser, dataHolder, relations, false);
            continue;
        }
      }
      else
        reader.Skip();
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
    IOfficeChartDataLabels officeChartDataLabels = (IOfficeChartDataLabels) null;
    if (isChartExSeries)
    {
      if (reader.LocalName != "dataLabel")
        throw new XmlException("Unexpeced xml tag.");
      if (reader.MoveToAttribute("idx"))
      {
        int int32 = XmlConvertExtension.ToInt32(reader.Value);
        officeChartDataLabels = series.DataPoints[int32].DataLabels;
        (officeChartDataLabels as ChartDataLabelsImpl).ShowTextProperties = false;
      }
      if (officeChartDataLabels != null && reader.MoveToAttribute("pos"))
        officeChartDataLabels.Position = (OfficeDataLabelPosition) Enum.Parse(typeof (Excel2007DataLabelPos), reader.Value, false);
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
            officeChartDataLabels = series.DataPoints[intValueTag].DataLabels;
            (officeChartDataLabels as ChartDataLabelsImpl).ShowTextProperties = false;
            continue;
          case "layout":
            (officeChartDataLabels as ChartDataLabelsImpl).Layout = (IOfficeChartLayout) new ChartLayoutImpl(this.m_book.Application, (object) (officeChartDataLabels as ChartDataLabelsImpl), series.Parent);
            ChartParserCommon.ParseChartLayout(reader, (officeChartDataLabels as ChartDataLabelsImpl).Layout);
            continue;
          case "delete":
            bool boolValueTag = ChartParserCommon.ParseBoolValueTag(reader);
            (officeChartDataLabels as ChartDataLabelsImpl).IsDelete = boolValueTag;
            continue;
          default:
            FileDataHolder dataHolder = series.ParentBook.DataHolder;
            Excel2007Parser parser = dataHolder.Parser;
            this.ParseDataLabelSettings(reader, officeChartDataLabels, parser, dataHolder, relations, isChartExSeries);
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
    IOfficeChartDataLabels dataLabels,
    Excel2007Parser parser,
    FileDataHolder holder,
    RelationCollection relations,
    bool isChartExSeries)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (dataLabels == null)
      throw new ArgumentNullException(nameof (dataLabels));
    (dataLabels as IInternalOfficeChartTextArea).Size = 10.0;
    ChartDataLabelsImpl dataLabels1 = dataLabels as ChartDataLabelsImpl;
    dataLabels1.ShowTextProperties = false;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "dLblPos":
            Excel2007DataLabelPos excel2007DataLabelPos = (Excel2007DataLabelPos) Enum.Parse(typeof (Excel2007DataLabelPos), ChartParserCommon.ParseValueTag(reader), false);
            dataLabels.Position = (OfficeDataLabelPosition) excel2007DataLabelPos;
            continue;
          case "showLegendKey":
            if (!(dataLabels as ChartDataLabelsImpl).m_bHasLegendKeyOption)
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
            if (!(dataLabels as ChartDataLabelsImpl).m_bHasValueOption)
            {
              dataLabels.IsValue = ChartParserCommon.ParseBoolValueTag(reader);
              continue;
            }
            reader.Skip();
            continue;
          case "showCatName":
            if (!(dataLabels as ChartDataLabelsImpl).m_bHasCategoryOption)
            {
              dataLabels.IsCategoryName = ChartParserCommon.ParseBoolValueTag(reader);
              continue;
            }
            reader.Skip();
            continue;
          case "showPercent":
            if (!(dataLabels as ChartDataLabelsImpl).m_bHasPercentageOption)
            {
              dataLabels.IsPercentage = ChartParserCommon.ParseBoolValueTag(reader);
              continue;
            }
            reader.Skip();
            continue;
          case "showBubbleSize":
            if (!(dataLabels as ChartDataLabelsImpl).m_bHasBubbleSizeOption)
            {
              dataLabels.IsBubbleSize = ChartParserCommon.ParseBoolValueTag(reader);
              continue;
            }
            reader.Skip();
            continue;
          case "showSerName":
            if (!(dataLabels as ChartDataLabelsImpl).m_bHasSeriesOption)
            {
              dataLabels.IsSeriesName = ChartParserCommon.ParseBoolValueTag(reader);
              continue;
            }
            reader.Skip();
            continue;
          case "separator":
            dataLabels.Delimiter = reader.ReadElementContentAsString();
            continue;
          case "txPr":
            IInternalOfficeChartTextArea textFormatting = dataLabels as IInternalOfficeChartTextArea;
            this.ParseDefaultTextFormatting(reader, textFormatting, parser);
            (dataLabels as ChartDataLabelsImpl).ShowTextProperties = true;
            IOfficeChartDataPoint parent = (IOfficeChartDataPoint) (dataLabels as ChartDataLabelsImpl).Parent;
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
          case "numFmt":
            ChartParserCommon.ParseNumberFormat(reader, dataLabels);
            continue;
          case "delete":
            bool boolValueTag2 = ChartParserCommon.ParseBoolValueTag(reader);
            (dataLabels as ChartDataLabelsImpl).IsDelete = boolValueTag2;
            continue;
          case "spPr":
            IChartFillObjectGetter objectGetter = (IChartFillObjectGetter) new ChartFillObjectGetterAny(dataLabels.FrameFormat.Border as ChartBorderImpl, dataLabels.FrameFormat.Interior as ChartInteriorImpl, dataLabels.FrameFormat.Fill as IInternalFill, dataLabels.FrameFormat.Shadow as ShadowImpl, dataLabels.FrameFormat.ThreeD as ThreeDFormatImpl);
            ChartParserCommon.ParseShapeProperties(reader, objectGetter, holder, relations);
            (dataLabels as ChartDataLabelsImpl).ShowTextProperties = true;
            continue;
          default:
            if (isChartExSeries)
            {
              dataLabels.IsCategoryName = false;
              dataLabels.IsValue = true;
              reader.Skip();
              continue;
            }
            ChartParserCommon.ParseTextAreaTag(reader, dataLabels as IInternalOfficeChartTextArea, relations, holder, new float?(10f));
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

  private ChartSerieImpl ParseBarSeries(
    XmlReader reader,
    ChartImpl chart,
    OfficeChartType seriesType,
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
          case "dLbls":
            this.ParseDataLabels(reader, series, relations, false);
            continue;
          case "trendline":
            this.ParseTrendlines(reader, series, relations);
            continue;
          case "shape":
            this.ParseBarShape(ChartParserCommon.ParseValueTag(reader), series.SerieFormat);
            series.HasColumnShape = true;
            continue;
          case "errBars":
            this.ParseErrorBars(reader, series, relations);
            continue;
          case "cat":
            series.CategoryLabelsIRange = this.ParseSeriesValues(reader, series, out values, false, "cat");
            if (values != null && (series.CategoryLabelsIRange == null || series.CategoryLabelsIRange.Count < values.Length))
              series.IsValidCategoryRange = false;
            if (values != null)
            {
              series.EnteredDirectlyCategoryLabels = values;
              continue;
            }
            continue;
          case "val":
            series.ValuesIRange = this.ParseSeriesValues(reader, series, out values, true, "val");
            if (values != null && (series.ValuesIRange == null || series.ValuesIRange.Count < values.Length))
              series.IsValidValueRange = false;
            if (values != null && series.ValuesIRange == null)
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

  private void ParseFilteredSeriesOrCategoryName(XmlReader reader, ChartSerieImpl series)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "extLst")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    if (reader.NodeType != XmlNodeType.Element)
      Excel2007Parser.SkipWhiteSpaces(reader);
    if (!(reader.LocalName == "ext"))
      return;
    while (reader.LocalName != "extLst")
    {
      if (reader.LocalName == "ext" && reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.LocalName == "ext" && reader.LocalName != "ext")
          series.extentsStream = ShapeParser.ReadNodeAsStream(reader);
        reader.Read();
        if (reader.LocalName == "invertSolidFillFmt")
          series.m_invertFillFormatStream = ShapeParser.ReadNodeAsStream(reader);
        if (reader.LocalName == "filteredCategoryTitle" || reader.LocalName == "filteredSeriesTitle")
          reader.Read();
        if (reader.LocalName == "tx")
        {
          this.ParseSeriesText(reader, series);
          series.ParentChart.SeriesNameLevel = OfficeSeriesNameLevel.SeriesNameLevelNone;
          reader.Skip();
          reader.Read();
        }
        else if (reader.LocalName == "cat")
        {
          object[] values;
          series.CategoryLabelsIRange = this.ParseSeriesValues(reader, series, out values, false, "cat");
          if (values != null)
            series.EnteredDirectlyCategoryLabels = values;
          series.ParentChart.CategoryLabelLevel = OfficeCategoriesLabelLevel.CategoriesLabelLevelNone;
          reader.Skip();
          reader.Read();
        }
        else if (reader.LocalName == "datalabelsRange")
          this.ParseDatalabelsRange(reader, series);
      }
      else
        reader.Read();
    }
    reader.Read();
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
      (series.DataPoints.DefaultDataPoint.DataLabels as ChartDataLabelsImpl).ValueFromCellsIRange = ChartParser.GetRange(parentBook, formula);
    reader.Read();
  }

  private void ParseDatalabelRangeCache(XmlReader reader, ChartSerieImpl series)
  {
    if (reader == null)
      throw new ArgumentException("XmlReader");
    reader.Read();
    string formatCode = (string) null;
    Dictionary<int, object> list = new Dictionary<int, object>();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "pt":
            this.AddNumericPoint(reader, list, true, ref formatCode);
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
    OfficeChartType seriesType,
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
            series.CategoryLabelsIRange = this.ParseSeriesValues(reader, series, out values, false, "cat");
            if (values != null && (series.CategoryLabelsIRange == null || series.CategoryLabelsIRange.Count < values.Length))
              series.IsValidCategoryRange = false;
            if (values != null)
            {
              series.EnteredDirectlyCategoryLabels = values;
              continue;
            }
            continue;
          case "val":
            series.ValuesIRange = this.ParseSeriesValues(reader, series, out values, true, "val");
            if (values != null && (series.ValuesIRange == null || series.ValuesIRange.Count < values.Length))
              series.IsValidValueRange = false;
            if (values != null && series.ValuesIRange == null)
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
    OfficeChartType seriesType,
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
            this.ParseTrendlines(reader, series, relations);
            continue;
          case "errBars":
            this.ParseErrorBars(reader, series, relations);
            continue;
          case "cat":
            series.CategoryLabelsIRange = this.ParseSeriesValues(reader, series, out values, false, "cat");
            if (values != null && (series.CategoryLabelsIRange == null || series.CategoryLabelsIRange.Count < values.Length))
              series.IsValidCategoryRange = false;
            if (values != null)
            {
              series.EnteredDirectlyCategoryLabels = values;
              continue;
            }
            continue;
          case "val":
            series.ValuesIRange = this.ParseSeriesValues(reader, series, out values, true, "val");
            if (values != null && (series.ValuesIRange == null || series.ValuesIRange.Count < values.Length))
              series.IsValidValueRange = false;
            if (values != null && series.ValuesIRange == null)
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

  private ChartSerieImpl ParseLineSeries(
    XmlReader reader,
    ChartImpl chart,
    OfficeChartType seriesType,
    RelationCollection relations,
    Excel2007Parser parser)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    ChartSerieImpl series = (ChartSerieImpl) chart.Series.Add(seriesType);
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
            this.ParseTrendlines(reader, series, relations);
            continue;
          case "errBars":
            this.ParseErrorBars(reader, series, relations);
            continue;
          case "cat":
            series.CategoryLabelsIRange = this.ParseSeriesValues(reader, series, out values, false, "cat");
            if (values != null && (series.CategoryLabelsIRange == null || series.CategoryLabelsIRange.Count < values.Length))
              series.IsValidCategoryRange = false;
            if (values != null)
            {
              series.EnteredDirectlyCategoryLabels = values;
              continue;
            }
            continue;
          case "val":
            series.ValuesIRange = this.ParseSeriesValues(reader, series, out values, true, "val");
            if (values != null && (series.ValuesIRange == null || series.ValuesIRange.Count < values.Length))
              series.IsValidValueRange = false;
            if (values != null && series.ValuesIRange == null)
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
    OfficeChartType seriesType,
    RelationCollection relations,
    Excel2007Parser parser)
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
            this.ParseTrendlines(reader, series, relations);
            continue;
          case "errBars":
            this.ParseErrorBars(reader, series, relations);
            continue;
          case "xVal":
            series.CategoryLabelsIRange = this.ParseSeriesValues(reader, series, out values, false, "cat");
            NameImpl categoryLabelsIrange = series.CategoryLabelsIRange as NameImpl;
            if (series.CategoriesFormatCode != null && series.CategoryLabelsIRange != null && !(series.CategoryLabelsIRange is ExternalRange) && (categoryLabelsIrange != null && categoryLabelsIrange.RefersToRange != null || series.CategoryLabelsIRange is RangeImpl))
              series.CategoryLabelsIRange.NumberFormat = series.CategoriesFormatCode;
            if (values != null && (series.CategoryLabelsIRange == null || series.CategoryLabelsIRange.Count < values.Length))
              series.IsValidCategoryRange = false;
            if (values != null)
            {
              series.EnteredDirectlyCategoryLabels = values;
              continue;
            }
            continue;
          case "yVal":
            series.ValuesIRange = this.ParseSeriesValues(reader, series, out values, false, "val");
            NameImpl valuesIrange = series.ValuesIRange as NameImpl;
            if (series.FormatCode != null && series.ValuesIRange != null && !(series.ValuesIRange is ExternalRange) && (valuesIrange != null && valuesIrange.RefersToRange != null || series.ValuesIRange is RangeImpl))
              series.ValuesIRange.NumberFormat = series.FormatCode;
            if (values != null && (series.ValuesIRange == null || series.ValuesIRange.Count < values.Length))
              series.IsValidValueRange = false;
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
    OfficeChartType seriesType,
    RelationCollection relations,
    Excel2007Parser parser)
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
          case "marker":
            this.ParseMarker(reader, series, parser);
            continue;
          case "dLbls":
            this.ParseDataLabels(reader, series, relations, false);
            continue;
          case "cat":
            series.CategoryLabelsIRange = this.ParseSeriesValues(reader, series, out values, false, "cat");
            if (series.CategoriesFormatCode != null && series.CategoryLabelsIRange != null && !(series.CategoryLabelsIRange is ExternalRange))
              series.CategoryLabelsIRange.NumberFormat = series.CategoriesFormatCode;
            if (values != null && (series.CategoryLabelsIRange == null || series.CategoryLabelsIRange.Count < values.Length))
              series.IsValidCategoryRange = false;
            if (values != null)
            {
              series.EnteredDirectlyCategoryLabels = values;
              continue;
            }
            continue;
          case "val":
            series.ValuesIRange = this.ParseSeriesValues(reader, series, out values, true, "val");
            if (series.FormatCode != null && series.ValuesIRange != null && !(series.ValuesIRange is ExternalRange))
              series.ValuesIRange.NumberFormat = series.FormatCode;
            if (values != null && (series.ValuesIRange == null || series.ValuesIRange.Count < values.Length))
              series.IsValidValueRange = false;
            if (values != null && series.ValuesIRange == null)
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
    ChartSerieImpl series = (ChartSerieImpl) chart.Series.Add(OfficeChartType.Bubble);
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
            this.ParseTrendlines(reader, series, relations);
            continue;
          case "errBars":
            this.ParseErrorBars(reader, series, relations);
            continue;
          case "xVal":
            series.CategoryLabelsIRange = this.ParseSeriesValues(reader, series, out values, false, "xVal");
            if (values != null && (series.CategoryLabelsIRange == null || series.CategoryLabelsIRange.Count < values.Length))
              series.IsValidCategoryRange = false;
            if (values != null)
            {
              series.EnteredDirectlyCategoryLabels = values;
              continue;
            }
            continue;
          case "yVal":
            series.ValuesIRange = this.ParseSeriesValues(reader, series, out values, false, "yVal");
            if (values != null && (series.ValuesIRange == null || series.ValuesIRange.Count < values.Length))
              series.IsValidValueRange = false;
            if (values != null)
            {
              series.EnteredDirectlyValues = values;
              continue;
            }
            continue;
          case "bubbleSize":
            series.BubblesIRange = this.ParseSeriesValues(reader, series, out values, false, "bubbleSize");
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
    OfficeChartType seriesType,
    RelationCollection relations,
    bool isPrimary)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    OfficeChartType serieType = chart != null ? chart.ChartType : throw new ArgumentNullException(nameof (chart));
    if (serieType == seriesType || serieType == OfficeChartType.Combination_Chart)
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
            this.ParseTrendlines(reader, series, relations);
            continue;
          case "errBars":
            this.ParseErrorBars(reader, series, relations);
            continue;
          case "cat":
            series.CategoryLabelsIRange = this.ParseSeriesValues(reader, series, out values, false, "cat");
            if (values != null && (series.CategoryLabelsIRange == null || series.CategoryLabelsIRange.Count < values.Length))
              series.IsValidCategoryRange = false;
            if (values != null)
            {
              series.EnteredDirectlyCategoryLabels = values;
              continue;
            }
            continue;
          case "val":
            series.ValuesIRange = this.ParseSeriesValues(reader, series, out values, true, "val");
            if (values != null && (series.ValuesIRange == null || series.ValuesIRange.Count < values.Length))
              series.IsValidValueRange = false;
            if (values != null && series.ValuesIRange == null)
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
              series.SetInvertIfNegative(XmlConvertExtension.ToBoolean(reader.Value));
              continue;
            }
            reader.Skip();
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
              string stringReference = this.ParseStringReference(reader, series);
              if (!string.IsNullOrEmpty(stringReference))
              {
                series.Name = "=" + stringReference;
                if (!string.IsNullOrEmpty(series.Name) && series.Name[0] == '=')
                  series.Name = series.SerieName;
              }
              string str = series.SerieName != null ? series.SerieName : series.Name;
              if (series.NameRangeIRange == null)
              {
                series.Name = series.SerieName;
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
    reader.Read();
  }

  private void ParseDirectlyEnteredStringValue(XmlReader reader, ChartSerieImpl series)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    reader.Read();
    List<object> objectList = new List<object>();
    if (reader.NodeType != XmlNodeType.EndElement)
    {
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "pt":
              series.SerieName = this.AddStringPoint(reader);
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

  private string AddStringPoint(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "pt")
      throw new XmlException();
    string str = (string) null;
    if (reader.MoveToAttribute("idx"))
      XmlConvertExtension.ToInt32(reader.Value);
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
              str = reader.ReadElementContentAsString();
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
    return str;
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
    if (reader.LocalName != "dPt")
      throw new XmlException();
    if (!reader.IsEmptyElement)
    {
      FileDataHolder dataHolder = series.ParentChart.ParentWorkbook.DataHolder;
      bool flag = this is ChartExParser;
      IOfficeChartDataPoint officeChartDataPoint = (IOfficeChartDataPoint) null;
      if (flag && reader.MoveToAttribute("idx"))
      {
        officeChartDataPoint = series.DataPoints[XmlConvertExtension.ToInt32(reader.Value)];
        (officeChartDataPoint as ChartDataPointImpl).HasDataPoint = true;
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
              officeChartDataPoint = series.DataPoints[index];
              (officeChartDataPoint as ChartDataPointImpl).HasDataPoint = true;
              continue;
            case "spPr":
              ChartSerieDataFormatImpl serieDataFormatImpl = officeChartDataPoint != null ? officeChartDataPoint.DataFormat as ChartSerieDataFormatImpl : throw new XmlException();
              serieDataFormatImpl.HasLineProperties = true;
              serieDataFormatImpl.HasInterior = true;
              serieDataFormatImpl.IsParsed = true;
              ChartFillObjectGetterAny objectGetter = new ChartFillObjectGetterAny(serieDataFormatImpl.LineProperties as ChartBorderImpl, serieDataFormatImpl.Interior as ChartInteriorImpl, serieDataFormatImpl.Fill as IInternalFill, serieDataFormatImpl.Shadow as ShadowImpl, serieDataFormatImpl.ThreeD as ThreeDFormatImpl);
              ChartParserCommon.ParseShapeProperties(reader, (IChartFillObjectGetter) objectGetter, dataHolder, relations);
              serieDataFormatImpl.IsDataPointColorParsed = true;
              continue;
            case "marker":
              ChartSerieDataFormatImpl dataFormat1 = officeChartDataPoint.DataFormat as ChartSerieDataFormatImpl;
              this.ParseMarker(reader, (IOfficeChartSerieDataFormat) dataFormat1, dataHolder.Parser);
              officeChartDataPoint.IsDefaultmarkertype = true;
              dataFormat1.IsParsed = true;
              continue;
            case "invertIfNegative":
              if (reader.MoveToAttribute("val"))
              {
                ChartSerieDataFormatImpl dataFormat2 = officeChartDataPoint.DataFormat as ChartSerieDataFormatImpl;
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
              (officeChartDataPoint as ChartDataPointImpl).Bubble3D = ChartParserCommon.ParseBoolValueTag(reader);
              (officeChartDataPoint.DataFormat as ChartSerieDataFormatImpl).IsParsed = true;
              continue;
            case "explosion":
              (officeChartDataPoint as ChartDataPointImpl).Explosion = ChartParserCommon.ParseIntValueTag(reader);
              (officeChartDataPoint.DataFormat as ChartSerieDataFormatImpl).IsParsed = true;
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
    bool isValueAxis,
    string tagName)
  {
    ChartImpl parentChart = series.ParentChart;
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    reader.Read();
    string strFormula = (string) null;
    string Filteredcategory = (string) null;
    string filteredvalue = (string) null;
    values = (object[]) null;
    bool flag1 = false;
    bool flag2 = false;
    bool flag3 = false;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "numRef":
            flag1 = true;
            strFormula = this.ParseNumReference(reader, out values, out filteredvalue, series, parentChart, tagName);
            if (strFormula != null)
            {
              if (strFormula.Split(',').Length > 1)
                series.NumRefFormula = strFormula;
            }
            if (values != null && tagName != "bubbleSize" && tagName != "cat")
              series.EnteredDirectlyValues = values;
            series.FilteredValue = filteredvalue;
            continue;
          case "strRef":
            flag2 = true;
            strFormula = this.ParseStringReference(reader, out Filteredcategory, out values, parentChart, series, tagName);
            if (strFormula != null)
            {
              if (strFormula.Split(',').Length > 1)
                series.StrRefFormula = strFormula;
            }
            series.FilteredCategory = Filteredcategory;
            continue;
          case "multiLvlStrRef":
            flag3 = true;
            strFormula = this.ParseMultiLevelStringReference(reader, series);
            if (strFormula != null)
            {
              series.MulLvlStrRefFormula = strFormula;
              continue;
            }
            continue;
          case "numLit":
            values = this.ParseDirectlyEnteredValues(reader, series, tagName);
            continue;
          case "strLit":
            values = this.ParseDirectlyEnteredValues(reader, series, tagName);
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
    if (strFormula != null && strFormula.StartsWith("(") && strFormula.EndsWith(")"))
      strFormula = strFormula.Substring(1, strFormula.Length - 2);
    IWorksheet worksheet = series.ParentBook.Worksheets[0];
    IRange seriesValues = (IRange) null;
    if (strFormula != null)
    {
      WorkbookImpl parentBook = series.ParentBook;
      FormulaUtil formulaUtil = parentBook.DataHolder.Parser.FormulaUtil;
      if (!ChartImpl.TryAndModifyToValidFormula(strFormula))
        return (IRange) null;
      seriesValues = (formulaUtil.ParseString(strFormula)[0] as IRangeGetter).GetRange((IWorkbook) parentBook, parentBook.Worksheets[0]);
      switch (seriesValues)
      {
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

  private string ParseNumReference(
    XmlReader reader,
    out object[] values,
    ChartSerieImpl series,
    string tagName)
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
            values = this.ParseDirectlyEnteredValues(reader, series, tagName);
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
    ChartSerieImpl series,
    ChartImpl chart,
    string tagName)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "numRef")
      throw new XmlException("Unexpected xml tag.");
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
              if (chart.CategoryFormula == null && !string.IsNullOrEmpty(numReference))
              {
                chart.CategoryFormula = numReference;
                continue;
              }
              continue;
            }
            filteredvalue = reader.ReadElementContentAsString();
            continue;
          case "numCache":
            values = this.ParseDirectlyEnteredValues(reader, series, tagName);
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

  private string ParseStringReference(XmlReader reader, ChartSerieImpl series)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "strRef")
      throw new XmlException("Unexpected xml tag.");
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
          case "strCache":
            this.ParseDirectlyEnteredStringValue(reader, series);
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
    ChartImpl chart,
    ChartSerieImpl series,
    string tagName)
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
    ChartDataRange categoryLabels = chart.Series[0].CategoryLabels as ChartDataRange;
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
              if (chart.CategoryFormula == null && !string.IsNullOrEmpty(stringReference))
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
            if (chart.CategoryFormula != stringReference || flag || stringReference == null || categoryLabels != null && categoryLabels.Range is NameImpl)
            {
              values = this.ParseDirectlyEnteredValues(reader, series, tagName);
              if (chart.CategoryLabelValues == null)
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
    while (!reader.LocalName.Equals("multiLvlStrRef"))
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
    reader.Read();
    Dictionary<int, object[]> levelStringCache = new Dictionary<int, object[]>();
    List<object> list = new List<object>();
    int key1 = 0;
    bool isString = true;
    if (reader.NodeType == XmlNodeType.EndElement)
      return levelStringCache;
    while (isString)
    {
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "pt":
              string formatCode = (string) null;
              int key2 = this.AddNumericPoint(reader, list, isString, ref formatCode);
              if (!string.IsNullOrEmpty(formatCode) && tagName == "val")
                series.FormatValueCodes.Add(key2, formatCode);
              else if (!string.IsNullOrEmpty(formatCode) && tagName == "cat")
                series.FormatCategoryCodes.Add(key2, formatCode);
              if (key2 != -1 && key2 > index)
              {
                do
                {
                  list.Insert(index, (object) null);
                  ++index;
                }
                while (index != key2 && key2 > index);
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
        isString = false;
      if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName.Equals("lvl"))
      {
        levelStringCache.Add(key1, list.ToArray());
        list.Clear();
        index = 0;
        ++key1;
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
    IOfficeChartSerieDataFormat serieFormat = series.SerieFormat;
    this.ParseMarker(reader, serieFormat, parser);
  }

  private void ParseMarker(
    XmlReader reader,
    IOfficeChartSerieDataFormat dataFormat,
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
              Excel2007ChartMarkerType excel2007ChartMarkerType = (Excel2007ChartMarkerType) Enum.Parse(typeof (Excel2007ChartMarkerType), ChartParserCommon.ParseValueTag(reader), false);
              dataFormat.MarkerStyle = (OfficeChartMarkerType) excel2007ChartMarkerType;
              (dataFormat as ChartSerieDataFormatImpl).HasMarkerProperties = true;
              flag = (dataFormat as ChartSerieDataFormatImpl).IsAutoMarker;
              continue;
            case "size":
              dataFormat.MarkerSize = ChartParserCommon.ParseIntValueTag(reader);
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
    IOfficeChartSerieDataFormat serieDataFormat,
    Excel2007Parser parser)
  {
    string localName = reader.LocalName;
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
              continue;
            case "gradFill":
              GradientStops gradientFill = ChartParserCommon.ParseGradientFill(reader, parser);
              ChartMarkerFormatRecord markerFormat = format.MarkerFormat;
              markerFormat.IsAutoColor = false;
              markerFormat.IsNotShowInt = false;
              markerFormat.IsNotShowBrd = false;
              format.MarkerBackColorObject.CopyFrom(gradientFill[0].ColorObject, true);
              format.MarkerGradient = gradientFill;
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
  }

  private bool ParseMarkerLine(
    XmlReader reader,
    ChartColor color,
    Excel2007Parser parser,
    ChartSerieDataFormatImpl format)
  {
    bool markerLine = false;
    int Alpha = 100000;
    Stream data = ShapeParser.ReadNodeAsStream(reader);
    data.Position = 0L;
    reader = UtilityMethods.CreateReader(data);
    format.MarkerLineStream = data;
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
    ChartSerieImpl series,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    if (reader.LocalName != "upDownBars")
      throw new XmlException("Unexpected xml tag.");
    ChartFormatImpl commonSerieOptions = (ChartFormatImpl) series.SerieFormat.CommonSerieOptions;
    FileDataHolder dataHolder = series.ParentBook.DataHolder;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "upBars":
            IOfficeChartDropBar firstDropBar = commonSerieOptions.FirstDropBar;
            this.ParseDropBar(reader, firstDropBar, dataHolder, relations);
            continue;
          case "downBars":
            IOfficeChartDropBar secondDropBar = commonSerieOptions.SecondDropBar;
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

  private void ParseDropBar(
    XmlReader reader,
    IOfficeChartDropBar dropBar,
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
      IOfficeChartDataTable dataTable = chart.DataTable;
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
              IInternalOfficeChartTextArea textArea = dataTable.TextArea as IInternalOfficeChartTextArea;
              this.ParseDefaultTextFormatting(reader, textArea, parser);
              continue;
            case "spPr":
              (dataTable as ChartDataTableImpl).HasShapeProperties = true;
              (dataTable as ChartDataTableImpl).shapeStream = chart.ParentWorkbook.DataHolder.Parser.ReadSingleNodeIntoStream(reader);
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
    IInternalOfficeChartTextArea textFormatting,
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
                ChartParserCommon.ParseParagraphRunProperites(reader, textFormatting, parser, (TextSettings) null);
              while (reader.LocalName != "txPr")
              {
                if (reader.NodeType != XmlNodeType.EndElement)
                {
                  switch (reader.LocalName)
                  {
                    case "endParaRPr":
                      if (this is ChartExParser && textFormatting.Parent is ChartLegendImpl && textFormatting != null)
                      {
                        textFormatting.Font.Size = 9.0;
                        ChartParserCommon.ParseParagraphRunProperites(reader, textFormatting, parser, (TextSettings) null);
                      }
                      reader.Read();
                      continue;
                    default:
                      reader.Read();
                      continue;
                  }
                }
                else
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

  private object[] ParseDirectlyEnteredValues(
    XmlReader reader,
    ChartSerieImpl series,
    string tagName)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    bool isString = false;
    int num = 0;
    if (reader.LocalName == "strLit" || reader.LocalName == "strCache")
      isString = true;
    int index1 = 0;
    reader.Read();
    List<object> list = new List<object>();
    if (reader.NodeType == XmlNodeType.EndElement)
      return list.ToArray();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "formatCode":
            if (tagName == "cat" || tagName == "xVal")
            {
              series.CategoriesFormatCode = reader.ReadElementContentAsString();
              continue;
            }
            series.FormatCode = reader.ReadElementContentAsString();
            continue;
          case "pt":
            string formatCode = (string) null;
            int key = this.AddNumericPoint(reader, list, isString, ref formatCode);
            if (!string.IsNullOrEmpty(formatCode) && tagName == "val")
              series.FormatValueCodes.Add(key, formatCode);
            else if (!string.IsNullOrEmpty(formatCode) && tagName == "cat")
              series.FormatCategoryCodes.Add(key, formatCode);
            if (key != -1 && key > index1)
            {
              do
              {
                if (isString)
                  list.Insert(index1, (object) "");
                else
                  list.Insert(index1, (object) null);
                ++index1;
              }
              while (index1 != key && key > index1);
            }
            ++index1;
            continue;
          case "ptCount":
            if (reader.MoveToAttribute("val"))
            {
              num = XmlConvertExtension.ToInt32(reader.Value);
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
    if (isString && list.Count == 0 && num > 0)
    {
      for (int index2 = 0; index2 < num; ++index2)
        list.Add((object) "");
    }
    reader.Read();
    return list.ToArray();
  }

  internal int AddNumericPoint(
    XmlReader reader,
    List<object> list,
    bool isString,
    ref string formatCode)
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
    if (reader.MoveToAttribute(nameof (formatCode)))
      formatCode = reader.Value;
    if (this is ChartExParser)
    {
      reader.MoveToElement();
      list.Add(this.ReadXmlValue(reader, isString));
      Excel2007Parser.SkipWhiteSpaces(reader);
    }
    else if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "v":
              list.Add(this.ReadXmlValue(reader, isString));
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
    return num;
  }

  internal int AddNumericPoint(
    XmlReader reader,
    Dictionary<int, object> list,
    bool isString,
    ref string formatCode)
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
    if (reader.MoveToAttribute(nameof (formatCode)))
      formatCode = reader.Value;
    if (this is ChartExParser)
    {
      reader.MoveToElement();
      list.Add(key, this.ReadXmlValue(reader, isString));
      Excel2007Parser.SkipWhiteSpaces(reader);
    }
    else if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "v":
              list.Add(key, this.ReadXmlValue(reader, isString));
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
    return key;
  }

  private object ReadXmlValue(XmlReader reader, bool isString)
  {
    string s = reader != null ? reader.ReadElementContentAsString() : throw new ArgumentNullException(nameof (reader));
    return isString ? (object) s : (object) XmlConvertExtension.ToDouble(s);
  }
}
