// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Charts.ChartSerializator
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Charts;
using Syncfusion.XlsIO.Implementation.PivotTables;
using Syncfusion.XlsIO.Implementation.XmlReaders.Shapes;
using Syncfusion.XlsIO.Implementation.XmlSerialization.Constants;
using Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Interfaces.Charts;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization.Charts;

public class ChartSerializator
{
  public const int DefaultExtentX = 8666049;
  public const int DefaultExtentY = 6293304;
  public int categoryFilter;
  public bool findFilter;
  private bool m_isChartExFallBack;

  internal ChartSerializator(bool value) => this.m_isChartExFallBack = value;

  public ChartSerializator()
  {
  }

  public void SerializeChart(XmlWriter writer, ChartImpl chart, string chartItemName)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("c", "chartSpace", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteAttributeString("xmlns", "a", (string) null, "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("xmlns", "r", (string) null, "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
    if (chart.HasPlotArea && chart.ChartArea.IsBorderCornersRound)
      ChartSerializatorCommon.SerializeBoolValueTag(writer, "roundedCorners", true);
    else
      ChartSerializatorCommon.SerializeBoolValueTag(writer, "roundedCorners", false);
    if (chart.m_lang != null)
      ChartSerializatorCommon.SerializeValueTag(writer, "lang", chart.m_lang);
    if (chart.AlternateContent != null)
    {
      chart.AlternateContent.Position = 0L;
      ShapeParser.WriteNodeFromStream(writer, chart.AlternateContent);
    }
    if (chart.Style > 0 && chart.Style <= 48 /*0x30*/)
      ChartSerializatorCommon.SerializeValueTag(writer, "style", chart.Style.ToString());
    else if (chart.AlternateContent == null && chart.Style > 100 && chart.Style <= 148)
    {
      writer.WriteStartElement("mc", "AlternateContent", "http://schemas.openxmlformats.org/markup-compatibility/2006");
      writer.WriteAttributeString("xmlns", "mc", (string) null, "http://schemas.openxmlformats.org/markup-compatibility/2006");
      writer.WriteStartElement("mc", "Choice", "http://schemas.openxmlformats.org/markup-compatibility/2006");
      writer.WriteAttributeString("xmlns", "c14", (string) null, "http://schemas.microsoft.com/office/drawing/2007/8/2/chart");
      writer.WriteAttributeString("Requires", "c14");
      ChartSerializatorCommon.SerializeValueTag(writer, "style", "http://schemas.microsoft.com/office/drawing/2007/8/2/chart", chart.Style.ToString());
      writer.WriteEndElement();
      writer.WriteStartElement("mc", "Fallback", "http://schemas.openxmlformats.org/markup-compatibility/2006");
      ChartSerializatorCommon.SerializeValueTag(writer, "style", (chart.Style - 100).ToString());
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
    if (chart.m_colorMapOverrideStream != null)
    {
      chart.m_colorMapOverrideStream.Position = 0L;
      ShapeParser.WriteNodeFromStream(writer, (Stream) chart.m_colorMapOverrideStream);
    }
    this.SerializePivotSource(writer, chart);
    if (chart.InnerProtection != ExcelSheetProtection.None)
      writer.WriteElementString("protection", "http://schemas.openxmlformats.org/drawingml/2006/chart", string.Empty);
    writer.WriteStartElement(nameof (chart), "http://schemas.openxmlformats.org/drawingml/2006/chart");
    FileDataHolder parentHolder = chart.DataHolder.ParentHolder;
    RelationCollection relations = chart.Relations;
    bool flag = false;
    if (chart.HasAutoTitle.HasValue)
      flag = chart.HasAutoTitle.Value;
    if (flag && chart.ChartTitle != null && chart.ChartTitle != string.Empty)
      flag = false;
    if (chart.HasTitle && !flag)
      ChartSerializatorCommon.SerializeTextArea(writer, chart.ChartTitleArea, chart.ParentWorkbook, relations, 18.0);
    else if (chart.IsTitleAreaInitialized && (chart.ChartTitleArea as ChartTextAreaImpl).TextRecord.IsAutoText && !flag)
    {
      writer.WriteStartElement("title", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      ChartSerializatorCommon.SerializeValueTag(writer, "overlay", (chart.ChartTitleArea as ChartTextAreaImpl).Overlay ? "1" : "0");
      writer.WriteEndElement();
    }
    if (chart.HasAutoTitle.HasValue)
    {
      writer.WriteStartElement("autoTitleDeleted", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      writer.WriteAttributeString("val", flag ? "1" : "0");
      writer.WriteEndElement();
    }
    this.SerializePivotFormats(writer, chart);
    if (chart.Series.Count > 0 || !chart.HasPivotSource)
      this.SerializeView3D(writer, chart);
    else
      this.SerializePivotView3D(writer, chart);
    if (chart.SupportWallsAndFloor)
    {
      if (chart.HasFloor)
        this.SerializeSurface(writer, chart.Floor, "floor", chart);
      if (chart.HasWalls && chart.ParentWorkbook.BeginVersion == 1)
      {
        this.SerializeSurface(writer, chart.Walls, "sideWall", chart);
        this.SerializeSurface(writer, chart.Walls, "backWall", chart);
      }
      else
      {
        this.SerializeSurface(writer, chart.SideWall, "sideWall", chart);
        this.SerializeSurface(writer, chart.Walls, "backWall", chart);
      }
    }
    this.SerializePlotArea(writer, chart, relations);
    if (chart.HasLegend && (chart.HasPivotSource ? 1 : (chart.Series.Count > 0 ? 1 : 0)) != 0)
      this.SerializeLegend(writer, chart.Legend, chart);
    if (chart.ShowPlotVisible || (chart.Workbook as WorkbookImpl).IsCreated)
      ChartSerializatorCommon.SerializeBoolValueTag(writer, "plotVisOnly", chart.PlotVisibleOnly);
    Excel2007ChartPlotEmpty displayBlanksAs = (Excel2007ChartPlotEmpty) chart.DisplayBlanksAs;
    ChartSerializatorCommon.SerializeValueTag(writer, "dispBlanksAs", displayBlanksAs.ToString());
    writer.WriteEndElement();
    if (chart.HasChartArea)
    {
      IChartFrameFormat chartArea = chart.ChartArea;
      if (chartArea != null)
        ChartSerializatorCommon.SerializeFrameFormat(writer, (IChartFillBorder) chartArea, chart, chartArea.IsBorderCornersRound);
    }
    this.SerializeDefaultTextProperties(writer, chart);
    if (chart.IsEmbeded)
      this.SerializePrinterSettings(writer, chart, relations);
    this.SerializeShapes(writer, chart, chartItemName);
    this.SerializePivotOptions(writer, chart);
    if (chart.ShowExpandCollapseFieldButtons)
      this.SerializePivotOptions16(writer, chart);
    writer.WriteEndElement();
  }

  private void SerializeDefaultTextProperties(XmlWriter writer, ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    Stream stream = chart != null ? chart.DefaultTextProperty : throw new ArgumentNullException(nameof (chart));
    if (stream == null)
      return;
    stream.Position = 0L;
    ShapeParser.WriteNodeFromStream(writer, stream);
  }

  private void SerializePivotOptions(XmlWriter writer, ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (!chart.HasPivotSource)
      return;
    writer.WriteStartElement("c", "extLst", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteStartElement("c", "ext", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteAttributeString("uri", "{781A3756-C4B2-4CAC-9D66-4F8BD8637D16}");
    writer.WriteAttributeString("xmlns", "c14", (string) null, "http://schemas.microsoft.com/office/drawing/2007/8/2/chart");
    writer.WriteStartElement("c14", "pivotOptions", "http://schemas.microsoft.com/office/drawing/2007/8/2/chart");
    if (chart.ShowReportFilterFieldButtons)
    {
      writer.WriteStartElement("c14", "dropZoneFilter", "http://schemas.microsoft.com/office/drawing/2007/8/2/chart");
      writer.WriteAttributeString("val", "1");
      writer.WriteEndElement();
    }
    if (chart.ShowAxisFieldButtons)
    {
      writer.WriteStartElement("c14", "dropZoneCategories", "http://schemas.microsoft.com/office/drawing/2007/8/2/chart");
      writer.WriteAttributeString("val", "1");
      writer.WriteEndElement();
    }
    if (chart.ShowValueFieldButtons)
    {
      writer.WriteStartElement("c14", "dropZoneData", "http://schemas.microsoft.com/office/drawing/2007/8/2/chart");
      writer.WriteAttributeString("val", "1");
      writer.WriteEndElement();
    }
    if (chart.ShowLegendFieldButtons)
    {
      writer.WriteStartElement("c14", "dropZoneSeries", "http://schemas.microsoft.com/office/drawing/2007/8/2/chart");
      writer.WriteAttributeString("val", "1");
      writer.WriteEndElement();
    }
    if (chart.ShowAllFieldButtons)
    {
      writer.WriteStartElement("c14", "dropZonesVisible", "http://schemas.microsoft.com/office/drawing/2007/8/2/chart");
      writer.WriteAttributeString("val", "1");
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
    writer.WriteEndElement();
    if (chart.ShowExpandCollapseFieldButtons)
      return;
    writer.WriteEndElement();
  }

  private void SerializePivotOptions16(XmlWriter writer, ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (!chart.HasPivotSource)
      return;
    writer.WriteStartElement("c", "ext", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteAttributeString("uri", "{E28EC0CA-F0BB-4C9C-879D-F8772B89E7AC}");
    writer.WriteAttributeString("xmlns", "c16", (string) null, "http://schemas.microsoft.com/office/drawing/2014/chart");
    writer.WriteStartElement("c16", "pivotOptions16", "http://schemas.microsoft.com/office/drawing/2014/chart");
    writer.WriteStartElement("c16", "showExpandCollapseFieldButtons", "http://schemas.microsoft.com/office/drawing/2014/chart");
    writer.WriteAttributeString("val", "1");
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private void SerializePivotFormats(XmlWriter writer, ChartImpl chart)
  {
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    Stream pivotFormatsStream = chart.PivotFormatsStream;
    if (pivotFormatsStream == null)
      return;
    pivotFormatsStream.Position = 0L;
    ShapeParser.WriteNodeFromStream(writer, pivotFormatsStream);
  }

  private void SerializePivotSource(XmlWriter writer, ChartImpl chart)
  {
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    IPivotTable pivotSource = chart.PivotSource;
    string text = chart.PreservedPivotSource;
    if (pivotSource != null)
      text = ChartSerializator.GetPivotSource(pivotSource);
    if (text == null)
      return;
    writer.WriteStartElement("c", "pivotSource", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteStartElement("c", "name", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteString(text);
    writer.WriteEndElement();
    writer.WriteStartElement("c", "fmtId", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteAttributeString("val", chart.FormatId.ToString());
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private static string GetPivotSource(IPivotTable pivotTable)
  {
    IWorksheet worksheet = pivotTable != null ? (IWorksheet) (pivotTable as PivotTableImpl).Worksheet : throw new ArgumentNullException(nameof (pivotTable));
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("[0]");
    stringBuilder.Append(worksheet.Name);
    stringBuilder.Append('!');
    stringBuilder.Append(pivotTable.Name);
    return stringBuilder.ToString();
  }

  private void SerializeShapes(XmlWriter writer, ChartImpl chart, string chartItemName)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (!this.m_isChartExFallBack && chart.Shapes.Count - chart.VmlShapesCount <= 0)
      return;
    WorksheetDataHolder dataHolder = chart.DataHolder;
    RelationCollection relations = chart.Relations;
    string relationId = !chart.IsWorkSheetDataholder ? dataHolder.DrawingsId : (string) null;
    if (relationId == null)
    {
      if (relations.Count > 0)
        relations.FindRelationByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/chartUserShapes", out relationId);
      if (relationId == null)
      {
        dataHolder.DrawingsId = relationId = relations.GenerateRelationId();
        relations[relationId] = (Relation) null;
      }
    }
    if (chart.DataHolder.SerializeDrawings((WorksheetBaseImpl) chart, relations, ref relationId, "application/vnd.openxmlformats-officedocument.drawingml.chartshapes+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/chartUserShapes"))
    {
      writer.WriteStartElement("userShapes", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", relationId);
      writer.WriteEndElement();
    }
    else if (this.m_isChartExFallBack)
    {
      chart.DataHolder.SerializeChartExFallbackShape((WorksheetBaseImpl) chart, relations, ref relationId, chartItemName, "application/vnd.openxmlformats-officedocument.drawingml.chartshapes+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/chartUserShapes");
      writer.WriteStartElement("userShapes", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", relationId);
      writer.WriteEndElement();
    }
    else
      relations.Remove(relationId);
  }

  private void SerializePrinterSettings(
    XmlWriter writer,
    ChartImpl chart,
    RelationCollection relations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    IChartPageSetup pageSetup = chart != null ? chart.PageSetup : throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("printSettings", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    IPageSetupConstantsProvider constants = (IPageSetupConstantsProvider) new ChartPageSetupConstants(false);
    Excel2007Serializator.SerializePrintSettings(writer, (IPageSetupBase) pageSetup, constants, true);
    Excel2007Serializator.SerializeVmlHFShapesWorksheetPart(writer, (WorksheetBaseImpl) chart, constants, relations);
    chart.DataHolder.SerializeHeaderFooterImages((WorksheetBaseImpl) chart, relations);
    writer.WriteEndElement();
  }

  private void SerializeLegend(XmlWriter writer, IChartLegend legend, ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (legend == null)
      throw new ArgumentNullException(nameof (legend));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement(nameof (legend), "http://schemas.openxmlformats.org/drawingml/2006/chart");
    ExcelLegendPosition position = legend.Position;
    if (position != ExcelLegendPosition.NotDocked)
    {
      Excel2007LegendPosition excel2007LegendPosition = (Excel2007LegendPosition) position;
      ChartSerializatorCommon.SerializeValueTag(writer, "legendPos", excel2007LegendPosition.ToString());
    }
    IChartLegendEntries legendEntries = legend.LegendEntries;
    IWorkbook workbook = chart.Workbook;
    int num = 0;
    for (int count = legendEntries.Count; num < count; ++num)
      this.SerializeLegendEntry(writer, legendEntries[num], num, workbook);
    if ((legend as ChartLegendImpl).Layout != null)
      ChartSerializatorCommon.SerializeLayout(writer, (object) (legend as ChartLegendImpl));
    if (!legend.IncludeInLayout)
      ChartSerializatorCommon.SerializeValueTag(writer, "overlay", "1");
    else
      ChartSerializatorCommon.SerializeValueTag(writer, "overlay", "0");
    ChartSerializatorCommon.SerializeFrameFormat(writer, (IChartFillBorder) legend.FrameFormat, chart, false);
    (legend as ChartLegendImpl).IsChartTextArea = true;
    if (((IInternalChartTextArea) legend.TextArea).ParagraphType == ChartParagraphType.CustomDefault)
      this.SerializeDefaultTextFormatting(writer, legend.TextArea, workbook, 10.0);
    writer.WriteEndElement();
  }

  private void SerializeLegendEntry(
    XmlWriter writer,
    IChartLegendEntry legendEntry,
    int index,
    IWorkbook book)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (legendEntry == null)
      throw new ArgumentNullException(nameof (legendEntry));
    if (!legendEntry.IsDeleted && !legendEntry.IsFormatted)
      return;
    writer.WriteStartElement(nameof (legendEntry), "http://schemas.openxmlformats.org/drawingml/2006/chart");
    ChartSerializatorCommon.SerializeValueTag(writer, "idx", index.ToString());
    if (legendEntry.IsDeleted)
      ChartSerializatorCommon.SerializeBoolValueTag(writer, "delete", true);
    if (legendEntry.IsFormatted)
      this.SerializeDefaultTextFormatting(writer, legendEntry.TextArea, book, 10.0);
    writer.WriteEndElement();
  }

  private void SerializePivotView3D(XmlWriter writer, ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (!chart.IsPivotChart3D)
      return;
    string str1 = chart.PivotChartType == ExcelChartType.Surface_3D || chart.PivotChartType == ExcelChartType.Surface_NoColor_3D ? "15" : "90";
    string str2 = chart.PivotChartType == ExcelChartType.Surface_3D || chart.PivotChartType == ExcelChartType.Surface_NoColor_3D ? "20" : "0";
    string str3 = chart.PivotChartType == ExcelChartType.Surface_3D || chart.PivotChartType == ExcelChartType.Surface_NoColor_3D ? "30" : "0";
    writer.WriteStartElement("view3D", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    ChartSerializatorCommon.SerializeValueTag(writer, "rotX", str1);
    ChartSerializatorCommon.SerializeValueTag(writer, "rotY", str2);
    ChartSerializatorCommon.SerializeBoolValueTag(writer, "rAngAx", false);
    ChartSerializatorCommon.SerializeValueTag(writer, "perspective", str3);
    writer.WriteEndElement();
  }

  private void SerializeView3D(XmlWriter writer, ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (!chart.IsChart3D)
      return;
    writer.WriteStartElement("view3D", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    ChartFormatImpl chartFormat = chart.ChartFormat;
    if (chartFormat != (ChartFormatImpl) null)
    {
      if (!chartFormat.IsDefaultElevation)
        ChartSerializatorCommon.SerializeValueTag(writer, "rotX", chart.Elevation.ToString());
      if (!chart.AutoScaling)
        ChartSerializatorCommon.SerializeValueTag(writer, "hPercent", chart.HeightPercent.ToString());
      if (!chartFormat.IsDefaultRotation)
        ChartSerializatorCommon.SerializeValueTag(writer, "rotY", chart.Rotation.ToString());
      ChartSerializatorCommon.SerializeValueTag(writer, "depthPercent", chart.DepthPercent.ToString());
      ChartSerializatorCommon.SerializeBoolValueTag(writer, "rAngAx", chart.RightAngleAxes);
      ChartSerializatorCommon.SerializeValueTag(writer, "perspective", (chart.Perspective * 2).ToString());
    }
    writer.WriteEndElement();
  }

  private void SerializeErrorBars(
    XmlWriter writer,
    IChartErrorBars errorBars,
    string direction,
    IWorkbook book,
    ChartSerieImpl series)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (errorBars == null)
      return;
    if (direction == null || direction.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (direction));
    writer.WriteStartElement("errBars", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    if (ChartFormatImpl.GetStartSerieType(series.SerieType) != "Bar")
      ChartSerializatorCommon.SerializeValueTag(writer, "errDir", direction);
    ExcelErrorBarInclude include = errorBars.Include;
    string lower = include.ToString().ToLower();
    ChartSerializatorCommon.SerializeValueTag(writer, "errBarType", lower);
    Excel2007ErrorBarType type = (Excel2007ErrorBarType) errorBars.Type;
    ChartSerializatorCommon.SerializeValueTag(writer, "errValType", type.ToString());
    ChartErrorBarsImpl chartErrorBarsImpl = errorBars as ChartErrorBarsImpl;
    ChartSerializatorCommon.SerializeBoolValueTag(writer, "noEndCap", !errorBars.HasCap);
    if (type == Excel2007ErrorBarType.cust)
    {
      if ((include == ExcelErrorBarInclude.Plus || include == ExcelErrorBarInclude.Both) && (errorBars.PlusRange != null || chartErrorBarsImpl.PlusRangeValues != null))
      {
        writer.WriteStartElement("plus", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        if (!chartErrorBarsImpl.IsPlusNumberLiteral)
          this.SerializeNumReference(writer, errorBars.PlusRange, chartErrorBarsImpl.PlusRangeValues, series, "");
        else
          this.SerializeDirectlyEntered(writer, chartErrorBarsImpl.PlusRangeValues, false, (string) null);
        writer.WriteEndElement();
      }
      if ((include == ExcelErrorBarInclude.Minus || include == ExcelErrorBarInclude.Both) && (errorBars.MinusRange != null || chartErrorBarsImpl.MinusRangeValues != null))
      {
        writer.WriteStartElement("minus", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        if (!chartErrorBarsImpl.IsMinusNumberLiteral)
          this.SerializeNumReference(writer, errorBars.MinusRange, chartErrorBarsImpl.MinusRangeValues, series, "");
        else
          this.SerializeDirectlyEntered(writer, chartErrorBarsImpl.MinusRangeValues, false, (string) null);
        writer.WriteEndElement();
      }
    }
    ChartSerializatorCommon.SerializeValueTag(writer, "val", XmlConvert.ToString(errorBars.NumberValue));
    IChartBorder border = errorBars.Border;
    bool flag1 = border != null && !border.AutoFormat;
    bool flag2 = errorBars.Shadow.ShadowInnerPresets != Excel2007ChartPresetsInner.NoShadow || errorBars.Shadow.ShadowOuterPresets != Excel2007ChartPresetsOuter.NoShadow || errorBars.Shadow.ShadowPrespectivePresets != Excel2007ChartPresetsPrespective.NoShadow;
    if (flag1 || flag2)
    {
      writer.WriteStartElement("spPr", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      if (flag1)
        ChartSerializatorCommon.SerializeLineProperties(writer, border, book);
      if (flag2)
        ChartSerializatorCommon.SerializeShadow(writer, errorBars.Shadow, errorBars.Shadow.HasCustomShadowStyle);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private void SerializeTrendlines(XmlWriter writer, IChartTrendLines trendlines, IWorkbook book)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (trendlines == null)
      return;
    int iIndex = 0;
    for (int count = trendlines.Count; iIndex < count; ++iIndex)
      this.SerializeTrendline(writer, trendlines[iIndex], book);
  }

  private void SerializeTrendline(XmlWriter writer, IChartTrendLine trendline, IWorkbook book)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (trendline == null)
      throw new ArgumentNullException(nameof (trendline));
    writer.WriteStartElement(nameof (trendline), "http://schemas.openxmlformats.org/drawingml/2006/chart");
    string name = trendline.Name;
    if (name != null && !trendline.NameIsAuto)
      writer.WriteElementString("name", "http://schemas.openxmlformats.org/drawingml/2006/chart", name);
    IChartBorder border = trendline.Border;
    if (border != null && !border.AutoFormat)
    {
      writer.WriteStartElement("spPr", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      ChartSerializatorCommon.SerializeLineProperties(writer, trendline.Border, book);
      writer.WriteEndElement();
    }
    Excel2007TrendlineType type = (Excel2007TrendlineType) trendline.Type;
    ChartSerializatorCommon.SerializeValueTag(writer, "trendlineType", type.ToString());
    string tagName = (string) null;
    if (trendline.Type == ExcelTrendLineType.Polynomial)
      tagName = "order";
    else if (trendline.Type == ExcelTrendLineType.Moving_Average)
      tagName = "period";
    if (tagName != null)
      ChartSerializatorCommon.SerializeValueTag(writer, tagName, trendline.Order.ToString());
    ChartSerializatorCommon.SerializeValueTag(writer, "forward", XmlConvert.ToString(trendline.Forward));
    ChartSerializatorCommon.SerializeValueTag(writer, "backward", XmlConvert.ToString(trendline.Backward));
    if (!trendline.InterceptIsAuto)
      ChartSerializatorCommon.SerializeValueTag(writer, "intercept", XmlConvert.ToString(trendline.Intercept));
    ChartSerializatorCommon.SerializeBoolValueTag(writer, "dispRSqr", trendline.DisplayRSquared);
    ChartSerializatorCommon.SerializeBoolValueTag(writer, "dispEq", trendline.DisplayEquation);
    if (trendline.DisplayRSquared || trendline.DisplayEquation)
      this.SerializeTrendlineLabel(writer, trendline.DataLabel, book as WorkbookImpl, (trendline as ChartTrendLineImpl).TrendLineTextArea, trendline as ChartTrendLineImpl);
    writer.WriteEndElement();
  }

  private void SerializeTrendlineLabel(
    XmlWriter writer,
    IChartTextArea dataLabelFormat,
    WorkbookImpl book,
    IChartTextArea trendlineTextArea,
    ChartTrendLineImpl trendline)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (book == null)
      throw new ArgumentException(nameof (book));
    if (dataLabelFormat == null)
      throw new ArgumentNullException(nameof (dataLabelFormat));
    if (trendline == null)
      throw new ArgumentException(nameof (trendline));
    writer.WriteStartElement("trendlineLbl", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    ChartSerializatorCommon.SerializeLayout(writer, (object) trendlineTextArea);
    if (!string.IsNullOrEmpty(trendlineTextArea.Text))
      ChartSerializatorCommon.SerializeTextAreaText(writer, trendlineTextArea, (IWorkbook) book, 10.0);
    writer.WriteStartElement("numFmt", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteAttributeString("formatCode", "General");
    writer.WriteAttributeString("sourceLinked", "0");
    writer.WriteEndElement();
    IChartBorder trendLineBorder = trendline.TrendLineBorder;
    ChartSerializatorCommon.SerializeFrameFormat(writer, (IChartFillBorder) trendlineTextArea.FrameFormat, book.DataHolder, (RelationCollection) null, false, false);
    writer.WriteEndElement();
  }

  private void SerializeSurface(
    XmlWriter writer,
    IChartWallOrFloor surface,
    string mainTagName,
    ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (surface == null)
      throw new ArgumentNullException(nameof (surface));
    if (mainTagName == null || mainTagName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (mainTagName));
    ChartWallOrFloorImpl chartWallOrFloorImpl = (ChartWallOrFloorImpl) surface;
    writer.WriteStartElement(mainTagName, "http://schemas.openxmlformats.org/drawingml/2006/chart");
    if (chartWallOrFloorImpl.Thickness != uint.MaxValue || (chart.Workbook as WorkbookImpl).IsConverted)
      ChartSerializatorCommon.SerializeValueTag(writer, "thickness", chartWallOrFloorImpl.Thickness.ToString());
    if (chartWallOrFloorImpl.HasShapeProperties || (chart.Workbook as WorkbookImpl).IsConverted)
      ChartSerializatorCommon.SerializeFrameFormat(writer, (IChartFillBorder) surface, chart, false);
    if (chartWallOrFloorImpl.PictureUnit != ExcelChartPictureType.stretch)
    {
      writer.WriteStartElement("pictureOptions", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteStartElement("pictureFormat", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteAttributeString("val", chartWallOrFloorImpl.PictureUnit.ToString());
      writer.WriteEndElement();
      if (chartWallOrFloorImpl.PictureUnit == ExcelChartPictureType.stackScale)
      {
        writer.WriteStartElement("pictureStackUnit", "http://schemas.openxmlformats.org/drawingml/2006/main");
        writer.WriteAttributeString("val", chartWallOrFloorImpl.PictureStackUnit == 0.0 ? "1" : XmlConvert.ToString(chartWallOrFloorImpl.PictureStackUnit));
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private void SerializePlotArea(XmlWriter writer, ChartImpl chart, RelationCollection relations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("plotArea", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    if (chart.PlotArea != null && chart.PlotArea.Layout != null)
      ChartSerializatorCommon.SerializeLayout(writer, (object) chart.PlotArea);
    int count = chart.Series.Count;
    int num = 0;
    int groupIndex = 0;
    while (num != count)
    {
      num += this.SerializeMainChartTypeTag(writer, chart, groupIndex);
      ++groupIndex;
    }
    if (num == 0 && !chart.HasPivotSource)
      this.SerializeEmptyChart(writer, chart);
    else if (count == 0 && chart.HasPivotSource)
    {
      this.SerializePivotPlotArea(writer, chart, relations);
      return;
    }
    this.SerializeAxes(writer, chart, relations);
    this.SerializeDataTable(writer, chart);
    if (chart.HasPlotArea)
    {
      IChartFrameFormat plotArea = chart.PlotArea;
      ChartSerializatorCommon.SerializeFrameFormat(writer, (IChartFillBorder) plotArea, chart, chart.ChartArea.IsBorderCornersRound);
    }
    writer.WriteEndElement();
  }

  private void SerializeEmptyChart(XmlWriter writer, ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (!(chart.ChartFormat != (ChartFormatImpl) null))
      return;
    ChartFormatImpl chartFormat = chart.ChartFormat;
    switch (chartFormat.FormatRecordType)
    {
      case TBIFFRecord.ChartBar:
        if (chartFormat.Is3D)
          writer.WriteStartElement("bar3DChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        else
          writer.WriteStartElement("barChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        string str1 = chart.IsChartBar ? "bar" : "col";
        ChartSerializatorCommon.SerializeValueTag(writer, "barDir", str1);
        this.SerializeChartGrouping(writer, chart.ChartType);
        ChartSerializatorCommon.SerializeBoolValueTag(writer, "varyColors", chartFormat.IsVaryColor);
        this.SerializeEmptyChartDataLabels(writer);
        ChartSerializatorCommon.SerializeValueTag(writer, "gapWidth", chartFormat.GapWidth.ToString());
        this.SerializeBarShapeFromType(writer, chart.ChartType);
        break;
      case TBIFFRecord.ChartLine:
        if (chartFormat.Is3D)
          writer.WriteStartElement("line3DChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        else
          writer.WriteStartElement("lineChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        this.SerializeChartGrouping(writer, chart.ChartType);
        ChartSerializatorCommon.SerializeBoolValueTag(writer, "varyColors", chartFormat.IsVaryColor);
        this.SerializeEmptyChartDataLabels(writer);
        if (!chartFormat.Is3D)
        {
          ChartSerializatorCommon.SerializeBoolValueTag(writer, "smooth", false);
          ChartSerializatorCommon.SerializeBoolValueTag(writer, "marker", chart.ChartType.ToString().Contains("_Markers"));
          break;
        }
        break;
      case TBIFFRecord.ChartPie:
        if (chartFormat.DoughnutHoleSize == 0)
        {
          if (chartFormat.Is3D)
            writer.WriteStartElement("pie3DChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
          else
            writer.WriteStartElement("pieChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        }
        else
          writer.WriteStartElement("doughnutChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        ChartSerializatorCommon.SerializeBoolValueTag(writer, "varyColors", chartFormat.IsVaryColor);
        this.SerializeEmptyChartDataLabels(writer);
        if (chart.ChartType != ExcelChartType.Pie_3D)
        {
          ChartSerializatorCommon.SerializeValueTag(writer, "firstSliceAng", chartFormat.FirstSliceAngle.ToString());
          if (chartFormat.DoughnutHoleSize != 0)
          {
            ChartSerializatorCommon.SerializeValueTag(writer, "holeSize", chartFormat.DoughnutHoleSize.ToString());
            break;
          }
          break;
        }
        break;
      case TBIFFRecord.ChartArea:
        if (chartFormat.Is3D)
          writer.WriteStartElement("area3DChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        else
          writer.WriteStartElement("areaChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        this.SerializeChartGrouping(writer, chart.ChartType);
        ChartSerializatorCommon.SerializeBoolValueTag(writer, "varyColors", chartFormat.IsVaryColor);
        this.SerializeEmptyChartDataLabels(writer);
        break;
      case TBIFFRecord.ChartScatter:
        if (chartFormat.IsBubbles)
        {
          writer.WriteStartElement("bubbleChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
          ChartSerializatorCommon.SerializeBoolValueTag(writer, "varyColors", chartFormat.IsVaryColor);
          this.SerializeEmptyChartDataLabels(writer);
          ChartSerializatorCommon.SerializeValueTag(writer, "bubbleScale", chartFormat.BubbleScale.ToString());
          ChartSerializatorCommon.SerializeValueTag(writer, "showNegBubbles", chartFormat.ShowNegativeBubbles ? "1" : "0");
          break;
        }
        writer.WriteStartElement("scatterChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        Excel2007ScatterStyle chartType1 = (Excel2007ScatterStyle) chart.ChartType;
        ChartSerializatorCommon.SerializeValueTag(writer, "scatterStyle", chartType1.ToString());
        ChartSerializatorCommon.SerializeBoolValueTag(writer, "varyColors", chartFormat.IsVaryColor);
        this.SerializeEmptyChartDataLabels(writer);
        break;
      case TBIFFRecord.ChartRadar:
      case TBIFFRecord.ChartRadarArea:
        writer.WriteStartElement("radarChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        Excel2007RadarStyle chartType2 = (Excel2007RadarStyle) chart.ChartType;
        ChartSerializatorCommon.SerializeValueTag(writer, "radarStyle", chartType2.ToString());
        ChartSerializatorCommon.SerializeBoolValueTag(writer, "varyColors", chartFormat.IsVaryColor);
        this.SerializeEmptyChartDataLabels(writer);
        break;
      case TBIFFRecord.ChartSurface:
        if (chart.ChartType == ExcelChartType.Surface_3D || chart.ChartType == ExcelChartType.Surface_NoColor_3D)
          writer.WriteStartElement("surface3DChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        else
          writer.WriteStartElement("surfaceChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        bool flag = chart.ChartType == ExcelChartType.Surface_NoColor_3D || chart.ChartType == ExcelChartType.Surface_NoColor_Contour;
        ChartSerializatorCommon.SerializeValueTag(writer, "wireframe", flag ? "1" : "0");
        writer.WriteElementString("bandFmts", "http://schemas.openxmlformats.org/drawingml/2006/chart", "");
        break;
      case TBIFFRecord.ChartBoppop:
        writer.WriteStartElement("ofPieChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        string str2 = chart.ChartType == ExcelChartType.PieOfPie ? "pie" : "bar";
        ChartSerializatorCommon.SerializeValueTag(writer, "ofPieType", str2);
        ChartSerializatorCommon.SerializeBoolValueTag(writer, "varyColors", chartFormat.IsVaryColor);
        this.SerializeEmptyChartDataLabels(writer);
        ChartSerializatorCommon.SerializeValueTag(writer, "gapWidth", chart.PrimaryFormats[0].GapWidth.ToString());
        ChartSerializatorCommon.SerializeValueTag(writer, "secondPieSize", chart.PrimaryFormats[0].PieSecondSize.ToString());
        IChartBorder pieSeriesLine = chart.PrimaryFormats[0].PieSeriesLine;
        writer.WriteStartElement("serLines", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        writer.WriteStartElement("spPr", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        ChartSerializatorCommon.SerializeLineProperties(writer, pieSeriesLine, (IWorkbook) chart.ParentWorkbook);
        writer.WriteEndElement();
        writer.WriteEndElement();
        break;
      default:
        writer.WriteStartElement("barChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        ChartSerializatorCommon.SerializeValueTag(writer, "barDir", "col");
        ChartSerializatorCommon.SerializeValueTag(writer, "grouping", "clustered");
        this.SerializeEmptyChartDataLabels(writer);
        break;
    }
    if (chartFormat.FormatRecordType != TBIFFRecord.ChartPie && chartFormat.FormatRecordType != TBIFFRecord.ChartBoppop)
    {
      ChartAxisImpl primaryCategoryAxis = (ChartAxisImpl) chart.PrimaryCategoryAxis;
      ChartSerializatorCommon.SerializeValueTag(writer, "axId", primaryCategoryAxis.AxisId.ToString());
      chart.SerializedAxisIds.Add(primaryCategoryAxis.AxisId);
      ChartAxisImpl primaryValueAxis = (ChartAxisImpl) chart.PrimaryValueAxis;
      ChartSerializatorCommon.SerializeValueTag(writer, "axId", primaryValueAxis.AxisId.ToString());
      chart.SerializedAxisIds.Add(primaryValueAxis.AxisId);
      if (chart.IsSeriesAxisAvail)
      {
        ChartAxisImpl primarySerieAxis = (ChartAxisImpl) chart.PrimarySerieAxis;
        if (primarySerieAxis != null)
          ChartSerializatorCommon.SerializeValueTag(writer, "axId", primarySerieAxis.AxisId.ToString());
      }
    }
    writer.WriteEndElement();
  }

  private void SerializeEmptyChartDataLabels(XmlWriter writer)
  {
    writer.WriteStartElement("dLbls", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    ChartSerializatorCommon.SerializeBoolValueTag(writer, "showLegendKey", false);
    ChartSerializatorCommon.SerializeBoolValueTag(writer, "showVal", false);
    ChartSerializatorCommon.SerializeBoolValueTag(writer, "showCatName", false);
    ChartSerializatorCommon.SerializeBoolValueTag(writer, "showSerName", false);
    ChartSerializatorCommon.SerializeBoolValueTag(writer, "showPercent", false);
    ChartSerializatorCommon.SerializeBoolValueTag(writer, "showBubbleSize", false);
    writer.WriteEndElement();
  }

  private void SerializePivotPlotArea(
    XmlWriter writer,
    ChartImpl chart,
    RelationCollection relations)
  {
    this.SerializeMainChartTypeTag(writer, chart);
    string str = chart.PivotChartType.ToString();
    bool flag1 = str.Contains("Pie");
    bool flag2 = str.Contains("Doughnut");
    if (!flag1 && !flag2)
    {
      ChartAxisImpl primaryCategoryAxis = (ChartAxisImpl) chart.PrimaryCategoryAxis;
      ChartSerializatorCommon.SerializeValueTag(writer, "axId", primaryCategoryAxis.AxisId.ToString());
      ChartAxisImpl primaryValueAxis = (ChartAxisImpl) chart.PrimaryValueAxis;
      ChartSerializatorCommon.SerializeValueTag(writer, "axId", primaryValueAxis.AxisId.ToString());
      if (str.Contains("Surface"))
      {
        ChartAxisImpl primarySerieAxis = (ChartAxisImpl) chart.PrimarySerieAxis;
        ChartSerializatorCommon.SerializeValueTag(writer, "axId", primarySerieAxis.AxisId.ToString());
      }
    }
    writer.WriteEndElement();
    if (!flag1 && !flag2)
    {
      this.SerializePivotAxes(writer, chart, relations);
      this.SerializeDataTable(writer, chart);
    }
    if (chart.HasPlotArea)
    {
      IChartFrameFormat plotArea = chart.PlotArea;
      ChartSerializatorCommon.SerializeFrameFormat(writer, (IChartFillBorder) plotArea, chart, chart.ChartArea.IsBorderCornersRound);
    }
    writer.WriteEndElement();
  }

  private void SerializeBarChart(XmlWriter writer, ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("barChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    string str = chart.PivotChartType.ToString().Contains("Bar") ? "bar" : "col";
    ChartSerializatorCommon.SerializeValueTag(writer, "barDir", str);
    this.SerializeChartGrouping(writer, chart);
    ChartSerializatorCommon.SerializeValueTag(writer, "overlap", chart.OverLap.ToString());
  }

  private int SerializeBarChart(XmlWriter writer, ChartImpl chart, ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("barChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    int num1 = this.SerializeBarChartShared(writer, chart, firstSeries);
    IChartFormat commonSerieOptions = firstSeries.SerieFormat.CommonSerieOptions;
    int num2 = 0;
    int num3 = 0;
    bool flag1 = false;
    bool flag2 = false;
    if (!(chart.Workbook as WorkbookImpl).IsCreated)
    {
      if (firstSeries.GapWidth != 0 || firstSeries.Overlap != 0)
      {
        num2 = firstSeries.GapWidth;
        num3 = firstSeries.Overlap;
        flag1 = firstSeries.ShowGapWidth;
        flag2 = true;
      }
      if (chart.GapWidth != commonSerieOptions.GapWidth || chart.OverLap != commonSerieOptions.Overlap)
      {
        num2 = commonSerieOptions.GapWidth;
        num3 = commonSerieOptions.Overlap;
      }
    }
    if (num3 == 0 & !flag2)
      num3 = commonSerieOptions.Overlap;
    if (num2 == 0 & !flag1)
      num2 = commonSerieOptions.GapWidth;
    ChartSerializatorCommon.SerializeValueTag(writer, "gapWidth", num2.ToString());
    if (num3 == -65436)
      num3 = 100;
    if (num3 != 0)
      ChartSerializatorCommon.SerializeValueTag(writer, "overlap", num3.ToString());
    if (commonSerieOptions.HasSeriesLines && (chart.ChartType == ExcelChartType.Bar_Stacked || chart.ChartType == ExcelChartType.Bar_Stacked_100))
    {
      IChartBorder pieSeriesLine = commonSerieOptions.PieSeriesLine;
      writer.WriteStartElement("serLines", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      writer.WriteStartElement("spPr", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      ChartSerializatorCommon.SerializeLineProperties(writer, pieSeriesLine, chart.Workbook);
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
    this.SerializeBarAxisId(writer, chart, firstSeries);
    writer.WriteEndElement();
    return num1;
  }

  private void SerializeBarAxisId(XmlWriter writer, ChartImpl chart, ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    bool flag = firstSeries != null ? firstSeries.UsePrimaryAxis : throw new ArgumentNullException(nameof (firstSeries));
    ChartAxisImpl chartAxisImpl1 = flag ? (ChartAxisImpl) chart.PrimaryCategoryAxis : (ChartAxisImpl) chart.SecondaryCategoryAxis;
    if (chartAxisImpl1 == null)
      throw new ArgumentNullException("axis");
    ChartSerializatorCommon.SerializeValueTag(writer, "axId", chartAxisImpl1.AxisId.ToString());
    chart.SerializedAxisIds.Add(chartAxisImpl1.AxisId);
    ChartAxisImpl chartAxisImpl2 = flag ? (ChartAxisImpl) chart.PrimaryValueAxis : (ChartAxisImpl) chart.SecondaryValueAxis;
    ChartSerializatorCommon.SerializeValueTag(writer, "axId", chartAxisImpl2.AxisId.ToString());
    chart.SerializedAxisIds.Add(chartAxisImpl2.AxisId);
    if (!chart.IsSeriesAxisAvail)
      return;
    ChartAxisImpl primarySerieAxis = (ChartAxisImpl) chart.PrimarySerieAxis;
    if (primarySerieAxis == null)
      return;
    ChartSerializatorCommon.SerializeValueTag(writer, "axId", primarySerieAxis.AxisId.ToString());
  }

  private void SerializeBar3DChart(XmlWriter writer, ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("bar3DChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    string str = chart.PivotChartType.ToString().Contains("Bar") ? "bar" : "col";
    ChartSerializatorCommon.SerializeValueTag(writer, "barDir", str);
    this.SerializeChartGrouping(writer, chart);
    this.SerializeBarShapeFromType(writer, chart.PivotChartType);
  }

  private void SerializeBarShapeFromType(XmlWriter writer, ExcelChartType type)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    string str = type.ToString();
    if (!str.Contains("Cone") && !str.Contains("Cylinder") && !str.Contains("Pyramid"))
      return;
    ExcelBaseFormat baseFormat;
    ExcelTopFormat topFormat;
    switch (type)
    {
      case ExcelChartType.Cylinder_Clustered:
      case ExcelChartType.Cylinder_Stacked:
      case ExcelChartType.Cylinder_Stacked_100:
      case ExcelChartType.Cylinder_Bar_Clustered:
      case ExcelChartType.Cylinder_Bar_Stacked:
      case ExcelChartType.Cylinder_Bar_Stacked_100:
      case ExcelChartType.Cylinder_Clustered_3D:
        baseFormat = ExcelBaseFormat.Circle;
        topFormat = ExcelTopFormat.Straight;
        break;
      case ExcelChartType.Cone_Clustered:
      case ExcelChartType.Cone_Stacked:
      case ExcelChartType.Cone_Stacked_100:
      case ExcelChartType.Cone_Bar_Clustered:
      case ExcelChartType.Cone_Bar_Stacked:
      case ExcelChartType.Cone_Bar_Stacked_100:
      case ExcelChartType.Cone_Clustered_3D:
        baseFormat = ExcelBaseFormat.Circle;
        topFormat = ExcelTopFormat.Sharp;
        break;
      case ExcelChartType.Pyramid_Clustered:
      case ExcelChartType.Pyramid_Stacked:
      case ExcelChartType.Pyramid_Stacked_100:
      case ExcelChartType.Pyramid_Bar_Clustered:
      case ExcelChartType.Pyramid_Bar_Stacked:
      case ExcelChartType.Pyramid_Bar_Stacked_100:
      case ExcelChartType.Pyramid_Clustered_3D:
        baseFormat = ExcelBaseFormat.Rectangle;
        topFormat = ExcelTopFormat.Sharp;
        break;
      default:
        throw new ArgumentException(nameof (type));
    }
    this.SerializeBarShape(writer, baseFormat, topFormat);
  }

  private int SerializeBar3DChart(XmlWriter writer, ChartImpl chart, ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("bar3DChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    int num = this.SerializeBarChartShared(writer, chart, firstSeries);
    int gapWidth = firstSeries.SerieFormat.CommonSerieOptions.GapWidth;
    ChartSerializatorCommon.SerializeValueTag(writer, "gapWidth", gapWidth.ToString());
    this.SerializeGapDepth(writer, chart);
    this.SerializeBarShape(writer, firstSeries.GetCommonSerieFormat().SerieDataFormat);
    this.SerializeBarAxisId(writer, chart, firstSeries);
    writer.WriteEndElement();
    return num;
  }

  private void SerializeGapDepth(XmlWriter writer, ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    ChartSerializatorCommon.SerializeValueTag(writer, "gapDepth", chart.GapDepth.ToString());
  }

  private void SerializeBarShape(
    XmlWriter writer,
    ExcelBaseFormat baseFormat,
    ExcelTopFormat topFormat)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    string str = (string) null;
    switch (topFormat)
    {
      case ExcelTopFormat.Straight:
        str = baseFormat == ExcelBaseFormat.Circle ? "cylinder" : "box";
        break;
      case ExcelTopFormat.Sharp:
        str = baseFormat == ExcelBaseFormat.Circle ? "cone" : "pyramid";
        break;
      case ExcelTopFormat.Trunc:
        str = baseFormat == ExcelBaseFormat.Circle ? "coneToMax" : "pyramidToMax";
        break;
    }
    ChartSerializatorCommon.SerializeValueTag(writer, "shape", str);
  }

  private void SerializeBarShape(XmlWriter writer, IChartSerieDataFormat dataFormat)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    ExcelTopFormat barShapeTop = dataFormat.BarShapeTop;
    ExcelBaseFormat barShapeBase = dataFormat.BarShapeBase;
    string str = (string) null;
    switch (barShapeTop)
    {
      case ExcelTopFormat.Straight:
        str = barShapeBase == ExcelBaseFormat.Circle ? "cylinder" : "box";
        break;
      case ExcelTopFormat.Sharp:
        str = barShapeBase == ExcelBaseFormat.Circle ? "cone" : "pyramid";
        break;
      case ExcelTopFormat.Trunc:
        str = barShapeBase == ExcelBaseFormat.Circle ? "coneToMax" : "pyramidToMax";
        break;
    }
    ChartSerializatorCommon.SerializeValueTag(writer, "shape", str);
  }

  private int SerializeBarChartShared(
    XmlWriter writer,
    ChartImpl chart,
    ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    string str = firstSeries.SerieType.ToString().Contains("Bar") ? "bar" : "col";
    ChartSerializatorCommon.SerializeValueTag(writer, "barDir", str);
    if (firstSeries == null)
      throw new ArgumentNullException(nameof (firstSeries));
    this.SerializeChartGrouping(writer, firstSeries.SerieType);
    this.SerializeVaryColors(writer, firstSeries);
    int num = this.SerializeChartSeries(writer, chart, firstSeries, new ChartSerializator.SerializeSeriesDelegate(this.SerializeBarSeries));
    if (chart.CommonDataPointsCollection != null && chart.CommonDataPointsCollection.ContainsKey(firstSeries.ChartGroup))
      this.SerializeDataLabels(writer, chart, chart.CommonDataPointsCollection[firstSeries.ChartGroup]);
    return num;
  }

  private int SerializeChartSeries(
    XmlWriter writer,
    ChartImpl chart,
    ChartSerieImpl firstSeries,
    ChartSerializator.SerializeSeriesDelegate serializator)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (firstSeries == null)
      throw new ArgumentNullException(nameof (firstSeries));
    if (serializator == null)
      throw new ArgumentNullException(nameof (serializator));
    int chartGroup = firstSeries.ChartGroup;
    IList<IChartSerie> additionOrder = (IList<IChartSerie>) (chart.Series as ChartSeriesCollection).AdditionOrder;
    IChartSeries series1 = chart.Series;
    IList<IChartSerie> arrOrderedSeries = additionOrder.Count == chart.Series.Count ? additionOrder : (IList<IChartSerie>) chart.Series;
    int seriesIndex = this.GetSeriesIndex(firstSeries, arrOrderedSeries, series1);
    firstSeries = arrOrderedSeries[seriesIndex] as ChartSerieImpl;
    List<ChartSerieImpl> chartSerieImplList = new List<ChartSerieImpl>();
    if (!firstSeries.IsFiltered)
      serializator(writer, firstSeries);
    else
      chartSerieImplList.Add(firstSeries);
    int num = 1;
    int index1 = seriesIndex + 1;
    for (int count = arrOrderedSeries.Count; index1 < count; ++index1)
    {
      ChartSerieImpl series2 = (ChartSerieImpl) arrOrderedSeries[index1];
      if (series2.ChartGroup == chartGroup)
      {
        if (!series2.IsFiltered)
          serializator(writer, series2);
        else
          chartSerieImplList.Add(series2);
        ++num;
      }
    }
    if (chartSerieImplList.Count != 0)
    {
      writer.WriteStartElement("extLst", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      writer.WriteStartElement("c", "ext", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      writer.WriteAttributeString("uri", "{02D57815-91ED-43cb-92C2-25804820EDAC}");
      writer.WriteAttributeString("xmlns", "c15", (string) null, "http://schemas.microsoft.com/office/drawing/2012/chart");
      if (this.categoryFilter == 0)
      {
        this.findFilter = this.FindFiltered(chartSerieImplList[0]);
        ++this.categoryFilter;
        if (this.findFilter)
        {
          this.UpdateCategoryLabel(chartSerieImplList[0]);
          this.UpdateFilteredValuesRange(chartSerieImplList[0]);
        }
      }
      for (int index2 = 0; index2 < chartSerieImplList.Count; ++index2)
        this.SerializeFilteredSeries(writer, chartSerieImplList[index2]);
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
    return num;
  }

  private void SerializeFilteredSeries(XmlWriter writer, ChartSerieImpl series)
  {
    string seriesType = this.GetSeriesType(series.StartType);
    writer.WriteStartElement("c15", seriesType, "http://schemas.microsoft.com/office/drawing/2012/chart");
    writer.WriteStartElement("c15", "ser", "http://schemas.microsoft.com/office/drawing/2012/chart");
    this.SerializeFilterSeries(writer, series);
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private void SerializeFilterSeries(XmlWriter writer, ChartSerieImpl series)
  {
    writer.WriteStartElement("c", "idx", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteAttributeString("val", series.Number.ToString());
    writer.WriteEndElement();
    writer.WriteStartElement("c", "order", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteAttributeString("val", series.Index.ToString());
    writer.WriteEndElement();
    if (series.ParentChart.SeriesNameLevel == ExcelSeriesNameLevel.SeriesNameLevelAll)
      this.SerializeFilteredText(writer, series);
    this.SerializeSeriesCommonWithoutEnd(writer, series, true);
    int percent = series.SerieFormat.Percent;
    if (percent != 0)
      ChartSerializatorCommon.SerializeValueTag(writer, "explosion", percent.ToString());
    if ((series.DataPoints.DefaultDataPoint as ChartDataPointImpl).HasDataLabels || (series.DataPoints as ChartDataPointsCollection).CheckDPDataLabels())
      this.SerializeDataLabels(writer, series.DataPoints.DefaultDataPoint.DataLabels, series);
    this.SerializeTrendlines(writer, series.TrendLines, (IWorkbook) series.ParentBook);
    this.SerializeErrorBars(writer, series);
    if (series.ParentChart.CategoryLabelLevel == ExcelCategoriesLabelLevel.CategoriesLabelLevelAll)
      this.SerializeFilteredCategory(writer, series, this.FindFiltered(series));
    this.SerializeFilteredValues(writer, series, this.FindFiltered(series));
    if (series.SerieType == ExcelChartType.Bubble || series.SerieType == ExcelChartType.Bubble_3D)
      this.SerializeSeriesValues(writer, series.Bubbles, series.EnteredDirectlyBubbles, "bubbleSize", series);
    if (series.ParentChart.CategoryLabelLevel == ExcelCategoriesLabelLevel.CategoriesLabelLevelAll && series.ParentChart.SeriesNameLevel == ExcelSeriesNameLevel.SeriesNameLevelAll)
      return;
    writer.WriteStartElement("extLst", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    if (series.ParentChart.SeriesNameLevel != ExcelSeriesNameLevel.SeriesNameLevelAll)
      this.SerializeFilteredSeriesOrCategoryName(writer, series, true);
    if (series.CategoryLabels != null && series.ParentChart.CategoryLabelLevel != ExcelCategoriesLabelLevel.CategoriesLabelLevelAll)
      this.SerializeFilteredSeriesOrCategoryName(writer, series, false);
    writer.WriteEndElement();
  }

  private void SerializeFilteredText(XmlWriter writer, ChartSerieImpl series)
  {
    if (series.IsDefaultName)
      return;
    if (series.ParentChart.SeriesNameLevel != ExcelSeriesNameLevel.SeriesNameLevelAll)
      writer.WriteStartElement("c", "tx", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    else
      writer.WriteStartElement("c15", "tx", "http://schemas.microsoft.com/office/drawing/2012/chart");
    string nameOrFormula = series.NameOrFormula;
    if (nameOrFormula.Length > 0 && nameOrFormula[0] == '=')
    {
      this.SerializeFiltedStringReference(writer, nameOrFormula, series);
    }
    else
    {
      writer.WriteStartElement("v", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      writer.WriteString(nameOrFormula);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private void SerializeFiltedStringReference(
    XmlWriter writer,
    string range,
    ChartSerieImpl series)
  {
    writer.WriteStartElement("c", "strRef", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteStartElement("c", "extLst", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteStartElement("c", "ext", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteAttributeString("uri", "{02D57815-91ED-43cb-92C2-25804820EDAC}");
    writer.WriteStartElement("c15", "formulaRef", "http://schemas.microsoft.com/office/drawing/2012/chart");
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    if (range[0] == '=')
      range = UtilityMethods.RemoveFirstCharUnsafe(range);
    if (series.StrRefFormula != null)
      range = series.StrRefFormula;
    writer.WriteElementString("sqref", "http://schemas.microsoft.com/office/drawing/2012/chart", range);
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private void SerializeFilteredCategory(
    XmlWriter writer,
    ChartSerieImpl series,
    bool categoryfilter)
  {
    if (series.CategoryLabels == null)
      return;
    if (series.SerieType == ExcelChartType.Bubble || series.SerieType == ExcelChartType.Bubble_3D || series.StartType.Contains("Scatter"))
      writer.WriteStartElement("c", "xVal", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    else
      writer.WriteStartElement("c", "cat", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteStartElement("c", "strRef", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteStartElement("c", "extLst", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteStartElement("c", "ext", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteAttributeString("uri", "{02D57815-91ED-43cb-92C2-25804820EDAC}");
    if (categoryfilter && series.IsFiltered)
    {
      this.SerializeFilteredFullReference(writer, series, false);
      writer.WriteStartElement("c15", "formulaRef", "http://schemas.microsoft.com/office/drawing/2012/chart");
      writer.WriteElementString("sqref", "http://schemas.microsoft.com/office/drawing/2012/chart", series.FilteredCategory);
      writer.WriteEndElement();
    }
    else if (series.IsFiltered)
    {
      writer.WriteStartElement("c15", "formulaRef", "http://schemas.microsoft.com/office/drawing/2012/chart");
      string str = (string) null;
      if (series.CategoryLabels != null)
        str = series.CategoryLabels.AddressGlobal;
      writer.WriteElementString("sqref", "http://schemas.microsoft.com/office/drawing/2012/chart", str);
      writer.WriteEndElement();
    }
    else
      this.SerializeFilteredFullReference(writer, series, false);
    writer.WriteEndElement();
    writer.WriteEndElement();
    if (!series.IsFiltered && categoryfilter)
      writer.WriteElementString("f", "http://schemas.openxmlformats.org/drawingml/2006/chart", series.FilteredCategory);
    writer.WriteElementString("strCache", "http://schemas.openxmlformats.org/drawingml/2006/chart", string.Empty);
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private void SerializeFilteredValues(
    XmlWriter writer,
    ChartSerieImpl series,
    bool categoryfilter)
  {
    if (series.SerieType == ExcelChartType.Bubble || series.SerieType == ExcelChartType.Bubble_3D || series.StartType.Contains("Scatter"))
      writer.WriteStartElement("c", "yVal", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    else
      writer.WriteStartElement("c", "val", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteStartElement("c", "numRef", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteStartElement("c", "extLst", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteStartElement("c", "ext", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteAttributeString("uri", "{02D57815-91ED-43cb-92C2-25804820EDAC}");
    if (categoryfilter && series.IsFiltered)
    {
      this.SerializeFilteredFullReference(writer, series, true);
      writer.WriteStartElement("c15", "formulaRef", "http://schemas.microsoft.com/office/drawing/2012/chart");
      writer.WriteElementString("sqref", "http://schemas.microsoft.com/office/drawing/2012/chart", series.FilteredValue);
      writer.WriteEndElement();
    }
    else if (series.IsFiltered)
    {
      writer.WriteStartElement("c15", "formulaRef", "http://schemas.microsoft.com/office/drawing/2012/chart");
      string str = (string) null;
      if (series.Values != null)
        str = series.Values.AddressGlobal;
      writer.WriteElementString("sqref", "http://schemas.microsoft.com/office/drawing/2012/chart", str);
      writer.WriteEndElement();
    }
    else
      this.SerializeFilteredFullReference(writer, series, true);
    writer.WriteEndElement();
    writer.WriteEndElement();
    if (!series.IsFiltered && categoryfilter)
      writer.WriteElementString("f", "http://schemas.openxmlformats.org/drawingml/2006/chart", series.FilteredValue);
    writer.WriteStartElement("c", "numCache", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    this.SerializeNumCacheValues(writer, series);
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  public string GetSeriesType(string Series)
  {
    string[] strArray = new string[8]
    {
      "filteredBarSeries",
      "filteredAreaSeries",
      "filteredLineSeries",
      "filteredPieSeries",
      "filteredRadarSeries",
      "filteredScatterSeries",
      "filteredSurfaceSeries",
      "filteredBubbleSeries"
    };
    int index = 0;
    while (strArray.Length > index && !strArray[index].Contains(Series))
      ++index;
    return strArray[index % 8];
  }

  private int GetSeriesIndex(
    ChartSerieImpl firstSeries,
    IList<IChartSerie> arrOrderedSeries,
    IChartSeries arrSeries)
  {
    int seriesIndex = -1;
    if (arrSeries == arrOrderedSeries)
    {
      seriesIndex = firstSeries.Index;
    }
    else
    {
      int index = 0;
      for (int count = arrOrderedSeries.Count; index < count; ++index)
      {
        if ((arrOrderedSeries[index] as ChartSerieImpl).ChartGroup == firstSeries.ChartGroup)
        {
          seriesIndex = index;
          break;
        }
      }
    }
    return seriesIndex;
  }

  private void SerializeFilteredFullReference(
    XmlWriter writer,
    ChartSerieImpl series,
    bool catorval)
  {
    writer.WriteStartElement("c15", "fullRef", "http://schemas.microsoft.com/office/drawing/2012/chart");
    string str = (string) null;
    if (catorval)
    {
      if (series.Values != null)
        str = series.Values.AddressGlobal;
    }
    else
      str = series.CategoryLabels.AddressGlobal;
    writer.WriteElementString("sqref", "http://schemas.microsoft.com/office/drawing/2012/chart", str);
    writer.WriteEndElement();
  }

  private void SerializeChartGrouping(XmlWriter writer, ExcelChartType seriesType)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    string str = !ChartImpl.GetIsClustered(seriesType) ? (!ChartImpl.GetIs100(seriesType) ? (!ChartImpl.GetIsStacked(seriesType) ? "standard" : "stacked") : "percentStacked") : "clustered";
    ChartSerializatorCommon.SerializeValueTag(writer, "grouping", str);
  }

  private void SerializeChartGrouping(XmlWriter writer, ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    ExcelChartType chartType = chart != null ? chart.PivotChartType : throw new ArgumentNullException("firstSeries");
    string str = !ChartImpl.GetIsClustered(chartType) ? (!ChartImpl.GetIs100(chartType) ? (!ChartImpl.GetIsStacked(chartType) ? "standard" : "stacked") : "percentStacked") : "clustered";
    ChartSerializatorCommon.SerializeValueTag(writer, "grouping", str);
  }

  private void SerializeArea3DChart(XmlWriter writer, ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("area3DChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    this.SerializeChartGrouping(writer, chart);
  }

  private int SerializeArea3DChart(XmlWriter writer, ChartImpl chart, ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("area3DChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    int num = this.SerializeAreaChartCommon(writer, chart, firstSeries);
    this.SerializeGapDepth(writer, chart);
    this.SerializeBarAxisId(writer, chart, firstSeries);
    writer.WriteEndElement();
    return num;
  }

  private void SerializeAreaChart(XmlWriter writer, ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("areaChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    this.SerializeChartGrouping(writer, chart);
  }

  private int SerializeAreaChart(XmlWriter writer, ChartImpl chart, ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("areaChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    int num = this.SerializeAreaChartCommon(writer, chart, firstSeries);
    this.SerializeBarAxisId(writer, chart, firstSeries);
    writer.WriteEndElement();
    return num;
  }

  private int SerializeAreaChartCommon(
    XmlWriter writer,
    ChartImpl chart,
    ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (firstSeries == null)
      throw new ArgumentNullException(nameof (firstSeries));
    ChartFormatImpl commonSerieOptions = (ChartFormatImpl) firstSeries.SerieFormat.CommonSerieOptions;
    this.SerializeChartGrouping(writer, firstSeries.SerieType);
    this.SerializeVaryColors(writer, firstSeries);
    int num = this.SerializeChartSeries(writer, chart, firstSeries, new ChartSerializator.SerializeSeriesDelegate(this.SerializeAreaSeries));
    if (chart.CommonDataPointsCollection != null && chart.CommonDataPointsCollection.ContainsKey(firstSeries.ChartGroup))
      this.SerializeDataLabels(writer, chart, chart.CommonDataPointsCollection[firstSeries.ChartGroup]);
    if (commonSerieOptions.HasDropLines)
    {
      IChartBorder dropLines = commonSerieOptions.DropLines;
      writer.WriteStartElement("dropLines", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      writer.WriteStartElement("spPr", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      ChartSerializatorCommon.SerializeLineProperties(writer, dropLines, chart.Workbook);
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
    return num;
  }

  private int SerializeLineChartCommon(
    XmlWriter writer,
    ChartImpl chart,
    ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (!chart.IsChartStock)
    {
      if (firstSeries == null)
        throw new ArgumentNullException(nameof (firstSeries));
      this.SerializeChartGrouping(writer, firstSeries.SerieType);
      this.SerializeVaryColors(writer, firstSeries);
    }
    int num = this.SerializeChartSeries(writer, chart, firstSeries, new ChartSerializator.SerializeSeriesDelegate(this.SerializeLineSeries));
    if (chart.CommonDataPointsCollection != null && chart.CommonDataPointsCollection.ContainsKey(firstSeries.ChartGroup))
      this.SerializeDataLabels(writer, chart, chart.CommonDataPointsCollection[firstSeries.ChartGroup]);
    return num;
  }

  private void SerializeLine3DChart(XmlWriter writer, ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("line3DChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    this.SerializeChartGrouping(writer, chart);
    ChartSerializatorCommon.SerializeValueTag(writer, "marker", "1");
  }

  private int SerializeLine3DChart(XmlWriter writer, ChartImpl chart, ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("line3DChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    int num = this.SerializeLineChartCommon(writer, chart, firstSeries);
    this.SerializeGapDepth(writer, chart);
    IChartFormat commonSerieOptions = firstSeries.SerieFormat.CommonSerieOptions;
    if (commonSerieOptions.HasDropLines)
    {
      IChartBorder dropLines = commonSerieOptions.DropLines;
      writer.WriteStartElement("dropLines", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      writer.WriteStartElement("spPr", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      ChartSerializatorCommon.SerializeLineProperties(writer, dropLines, chart.Workbook);
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
    this.SerializeBarAxisId(writer, chart, firstSeries);
    writer.WriteEndElement();
    return num;
  }

  private void SerializeLineChart(XmlWriter writer, ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    string localName = "lineChart";
    writer.WriteStartElement(localName, "http://schemas.openxmlformats.org/drawingml/2006/chart");
    this.SerializeChartGrouping(writer, chart);
    ChartSerializatorCommon.SerializeValueTag(writer, "marker", "1");
  }

  private int SerializeLineChart(XmlWriter writer, ChartImpl chart, ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    string localName = chart.IsChartStock ? "stockChart" : "lineChart";
    writer.WriteStartElement(localName, "http://schemas.openxmlformats.org/drawingml/2006/chart");
    int num = this.SerializeLineChartCommon(writer, chart, firstSeries);
    ChartSerieDataFormatImpl serieFormat = (ChartSerieDataFormatImpl) firstSeries.SerieFormat;
    ChartFormatImpl commonSerieOptions = (ChartFormatImpl) firstSeries.SerieFormat.CommonSerieOptions;
    if (commonSerieOptions.IsChartChartLine)
    {
      if (commonSerieOptions.HasHighLowLines)
      {
        if (commonSerieOptions.IsChartLineFormat)
        {
          writer.WriteStartElement("hiLowLines", "http://schemas.openxmlformats.org/drawingml/2006/chart");
          writer.WriteStartElement("spPr", "http://schemas.openxmlformats.org/drawingml/2006/chart");
          ChartSerializatorCommon.SerializeLineProperties(writer, commonSerieOptions.HighLowLines, chart.Workbook);
          writer.WriteEndElement();
          writer.WriteEndElement();
        }
        else
          writer.WriteElementString("hiLowLines", "http://schemas.openxmlformats.org/drawingml/2006/chart", string.Empty);
      }
      if (commonSerieOptions.HasDropLines)
      {
        if (commonSerieOptions.IsChartLineFormat)
        {
          writer.WriteStartElement("dropLines", "http://schemas.openxmlformats.org/drawingml/2006/chart");
          writer.WriteStartElement("spPr", "http://schemas.openxmlformats.org/drawingml/2006/chart");
          ChartSerializatorCommon.SerializeLineProperties(writer, commonSerieOptions.DropLines, chart.Workbook);
          writer.WriteEndElement();
          writer.WriteEndElement();
        }
        else
          writer.WriteElementString("dropLines", "http://schemas.openxmlformats.org/drawingml/2006/chart", string.Empty);
      }
    }
    this.SerializeUpDownBars(writer, chart, firstSeries);
    if (serieFormat.IsMarker)
      ChartSerializatorCommon.SerializeValueTag(writer, "marker", "1");
    this.SerializeBarAxisId(writer, chart, firstSeries);
    writer.WriteEndElement();
    return num;
  }

  private int SerializeBubbleChart(XmlWriter writer, ChartImpl chart, ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("bubbleChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    this.SerializeVaryColors(writer, firstSeries);
    int num = this.SerializeChartSeries(writer, chart, firstSeries, new ChartSerializator.SerializeSeriesDelegate(this.SerializeBubbleSeries));
    if (chart.CommonDataPointsCollection != null && chart.CommonDataPointsCollection.ContainsKey(firstSeries.ChartGroup))
      this.SerializeDataLabels(writer, chart, chart.CommonDataPointsCollection[firstSeries.ChartGroup]);
    IChartFormat commonSerieOptions = firstSeries.SerieFormat.CommonSerieOptions;
    int bubbleScale = commonSerieOptions.BubbleScale;
    if (bubbleScale != 100)
      ChartSerializatorCommon.SerializeValueTag(writer, "bubbleScale", bubbleScale.ToString());
    if (commonSerieOptions.ShowNegativeBubbles)
      ChartSerializatorCommon.SerializeValueTag(writer, "showNegBubbles", "1");
    string str = commonSerieOptions.SizeRepresents == ExcelBubbleSize.Area ? "area" : "w";
    ChartSerializatorCommon.SerializeValueTag(writer, "sizeRepresents", str);
    this.SerializeBarAxisId(writer, chart, firstSeries);
    writer.WriteEndElement();
    return num;
  }

  private void SerializeSurfaceChart(XmlWriter writer, ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("surfaceChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    ExcelChartType pivotChartType = chart.PivotChartType;
    if (pivotChartType != ExcelChartType.Surface_NoColor_3D && pivotChartType != ExcelChartType.Surface_NoColor_Contour)
      return;
    ChartSerializatorCommon.SerializeValueTag(writer, "wireframe", "1");
  }

  private int SerializeSurfaceChart(XmlWriter writer, ChartImpl chart, ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("surfaceChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    int num = this.SerializeSurfaceCommon(writer, chart, firstSeries);
    this.SerializeBarAxisId(writer, chart, firstSeries);
    writer.WriteEndElement();
    return num;
  }

  private void SerializeSurface3DChart(XmlWriter writer, ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("surface3DChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    ExcelChartType pivotChartType = chart.PivotChartType;
    if (pivotChartType != ExcelChartType.Surface_NoColor_3D && pivotChartType != ExcelChartType.Surface_NoColor_Contour)
      return;
    ChartSerializatorCommon.SerializeValueTag(writer, "wireframe", "1");
  }

  private int SerializeSurface3DChart(
    XmlWriter writer,
    ChartImpl chart,
    ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("surface3DChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    int num = this.SerializeSurfaceCommon(writer, chart, firstSeries);
    this.SerializeBarAxisId(writer, chart, firstSeries);
    writer.WriteEndElement();
    return num;
  }

  private int SerializeSurfaceCommon(XmlWriter writer, ChartImpl chart, ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    ExcelChartType serieType = firstSeries.SerieType;
    if (serieType == ExcelChartType.Surface_NoColor_3D || serieType == ExcelChartType.Surface_NoColor_Contour)
      ChartSerializatorCommon.SerializeValueTag(writer, "wireframe", "1");
    int num = this.SerializeChartSeries(writer, chart, firstSeries, new ChartSerializator.SerializeSeriesDelegate(this.SerializeBarSeries));
    this.SerializeBandFormats(writer, chart);
    return num;
  }

  private void SerializeBandFormats(XmlWriter writer, ChartImpl chart)
  {
    Stream preservedBandFormats = chart.PreservedBandFormats;
    if (preservedBandFormats == null || preservedBandFormats.Length <= 0L)
      return;
    preservedBandFormats.Position = 0L;
    ShapeParser.WriteNodeFromStream(writer, preservedBandFormats);
  }

  private int SerializeMainChartTypeTag(XmlWriter writer, ChartImpl chart, int groupIndex)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    IChartSeries chartSeries = chart != null ? chart.Series : throw new ArgumentNullException(nameof (chart));
    ChartSerieImpl firstSeries = (ChartSerieImpl) null;
    int index = 0;
    for (int count = chartSeries.Count; index < count; ++index)
    {
      ChartSerieImpl chartSerieImpl = (ChartSerieImpl) chartSeries[index];
      if (chartSerieImpl.ChartGroup == groupIndex)
      {
        firstSeries = chartSerieImpl;
        break;
      }
    }
    int num = 0;
    if (firstSeries != null)
    {
      switch (firstSeries.SerieType)
      {
        case ExcelChartType.Column_Clustered:
        case ExcelChartType.Column_Stacked:
        case ExcelChartType.Column_Stacked_100:
        case ExcelChartType.Bar_Clustered:
        case ExcelChartType.Bar_Stacked:
        case ExcelChartType.Bar_Stacked_100:
          num = this.SerializeBarChart(writer, chart, firstSeries);
          break;
        case ExcelChartType.Column_Clustered_3D:
        case ExcelChartType.Column_Stacked_3D:
        case ExcelChartType.Column_Stacked_100_3D:
        case ExcelChartType.Column_3D:
        case ExcelChartType.Bar_Clustered_3D:
        case ExcelChartType.Bar_Stacked_3D:
        case ExcelChartType.Bar_Stacked_100_3D:
        case ExcelChartType.Cylinder_Clustered:
        case ExcelChartType.Cylinder_Stacked:
        case ExcelChartType.Cylinder_Stacked_100:
        case ExcelChartType.Cylinder_Bar_Clustered:
        case ExcelChartType.Cylinder_Bar_Stacked:
        case ExcelChartType.Cylinder_Bar_Stacked_100:
        case ExcelChartType.Cylinder_Clustered_3D:
        case ExcelChartType.Cone_Clustered:
        case ExcelChartType.Cone_Stacked:
        case ExcelChartType.Cone_Stacked_100:
        case ExcelChartType.Cone_Bar_Clustered:
        case ExcelChartType.Cone_Bar_Stacked:
        case ExcelChartType.Cone_Bar_Stacked_100:
        case ExcelChartType.Cone_Clustered_3D:
        case ExcelChartType.Pyramid_Clustered:
        case ExcelChartType.Pyramid_Stacked:
        case ExcelChartType.Pyramid_Stacked_100:
        case ExcelChartType.Pyramid_Bar_Clustered:
        case ExcelChartType.Pyramid_Bar_Stacked:
        case ExcelChartType.Pyramid_Bar_Stacked_100:
        case ExcelChartType.Pyramid_Clustered_3D:
          num = this.SerializeBar3DChart(writer, chart, firstSeries);
          break;
        case ExcelChartType.Line:
        case ExcelChartType.Line_Stacked:
        case ExcelChartType.Line_Stacked_100:
        case ExcelChartType.Line_Markers:
        case ExcelChartType.Line_Markers_Stacked:
        case ExcelChartType.Line_Markers_Stacked_100:
          num = this.SerializeLineChart(writer, chart, firstSeries);
          break;
        case ExcelChartType.Line_3D:
          num = this.SerializeLine3DChart(writer, chart, firstSeries);
          break;
        case ExcelChartType.Pie:
        case ExcelChartType.Pie_Exploded:
          num = this.SerializePieChart(writer, chart, firstSeries);
          break;
        case ExcelChartType.Pie_3D:
        case ExcelChartType.Pie_Exploded_3D:
          num = this.SerializePie3DChart(writer, chart, firstSeries);
          break;
        case ExcelChartType.PieOfPie:
        case ExcelChartType.Pie_Bar:
          num = this.SerializeOfPieChart(writer, chart, firstSeries);
          break;
        case ExcelChartType.Scatter_Markers:
        case ExcelChartType.Scatter_SmoothedLine_Markers:
        case ExcelChartType.Scatter_SmoothedLine:
        case ExcelChartType.Scatter_Line_Markers:
        case ExcelChartType.Scatter_Line:
          num = this.SerializeScatterChart(writer, chart, firstSeries);
          break;
        case ExcelChartType.Area:
        case ExcelChartType.Area_Stacked:
        case ExcelChartType.Area_Stacked_100:
          num = this.SerializeAreaChart(writer, chart, firstSeries);
          break;
        case ExcelChartType.Area_3D:
        case ExcelChartType.Area_Stacked_3D:
        case ExcelChartType.Area_Stacked_100_3D:
          num = this.SerializeArea3DChart(writer, chart, firstSeries);
          break;
        case ExcelChartType.Doughnut:
        case ExcelChartType.Doughnut_Exploded:
          num = this.SerializeDoughnutChart(writer, chart, firstSeries);
          break;
        case ExcelChartType.Radar:
        case ExcelChartType.Radar_Markers:
        case ExcelChartType.Radar_Filled:
          num = this.SerializeRadarChart(writer, chart, firstSeries);
          break;
        case ExcelChartType.Surface_3D:
        case ExcelChartType.Surface_NoColor_3D:
          num = this.SerializeSurface3DChart(writer, chart, firstSeries);
          break;
        case ExcelChartType.Surface_Contour:
        case ExcelChartType.Surface_NoColor_Contour:
          num = this.SerializeSurfaceChart(writer, chart, firstSeries);
          break;
        case ExcelChartType.Bubble:
        case ExcelChartType.Bubble_3D:
          num = this.SerializeBubbleChart(writer, chart, firstSeries);
          break;
        case ExcelChartType.Stock_HighLowClose:
        case ExcelChartType.Stock_OpenHighLowClose:
        case ExcelChartType.Stock_VolumeHighLowClose:
        case ExcelChartType.Stock_VolumeOpenHighLowClose:
          num = this.SerializeStockChart(writer, chart, firstSeries);
          break;
      }
    }
    return num;
  }

  private void SerializeMainChartTypeTag(XmlWriter writer, ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (!chart.HasPivotSource)
      return;
    switch (chart.PivotChartType)
    {
      case ExcelChartType.Column_Clustered:
      case ExcelChartType.Column_Stacked:
      case ExcelChartType.Column_Stacked_100:
      case ExcelChartType.Bar_Clustered:
      case ExcelChartType.Bar_Stacked:
      case ExcelChartType.Bar_Stacked_100:
        this.SerializeBarChart(writer, chart);
        break;
      case ExcelChartType.Column_Clustered_3D:
      case ExcelChartType.Column_Stacked_3D:
      case ExcelChartType.Column_Stacked_100_3D:
      case ExcelChartType.Column_3D:
      case ExcelChartType.Bar_Clustered_3D:
      case ExcelChartType.Bar_Stacked_3D:
      case ExcelChartType.Bar_Stacked_100_3D:
      case ExcelChartType.Cylinder_Clustered:
      case ExcelChartType.Cylinder_Stacked:
      case ExcelChartType.Cylinder_Stacked_100:
      case ExcelChartType.Cylinder_Bar_Clustered:
      case ExcelChartType.Cylinder_Bar_Stacked:
      case ExcelChartType.Cylinder_Bar_Stacked_100:
      case ExcelChartType.Cylinder_Clustered_3D:
      case ExcelChartType.Cone_Clustered:
      case ExcelChartType.Cone_Stacked:
      case ExcelChartType.Cone_Stacked_100:
      case ExcelChartType.Cone_Bar_Clustered:
      case ExcelChartType.Cone_Bar_Stacked:
      case ExcelChartType.Cone_Bar_Stacked_100:
      case ExcelChartType.Cone_Clustered_3D:
      case ExcelChartType.Pyramid_Clustered:
      case ExcelChartType.Pyramid_Stacked:
      case ExcelChartType.Pyramid_Stacked_100:
      case ExcelChartType.Pyramid_Bar_Clustered:
      case ExcelChartType.Pyramid_Bar_Stacked:
      case ExcelChartType.Pyramid_Bar_Stacked_100:
      case ExcelChartType.Pyramid_Clustered_3D:
        this.SerializeBar3DChart(writer, chart);
        break;
      case ExcelChartType.Line:
      case ExcelChartType.Line_Stacked:
      case ExcelChartType.Line_Stacked_100:
      case ExcelChartType.Line_Markers:
      case ExcelChartType.Line_Markers_Stacked:
      case ExcelChartType.Line_Markers_Stacked_100:
        this.SerializeLineChart(writer, chart);
        break;
      case ExcelChartType.Line_3D:
        this.SerializeLine3DChart(writer, chart);
        break;
      case ExcelChartType.Pie:
      case ExcelChartType.Pie_Exploded:
        this.SerializePieChart(writer, chart);
        break;
      case ExcelChartType.Pie_3D:
      case ExcelChartType.Pie_Exploded_3D:
        this.SerializePie3DChart(writer, chart);
        break;
      case ExcelChartType.PieOfPie:
      case ExcelChartType.Pie_Bar:
        this.SerializeOfPieChart(writer, chart);
        break;
      case ExcelChartType.Scatter_Markers:
      case ExcelChartType.Scatter_SmoothedLine_Markers:
      case ExcelChartType.Scatter_SmoothedLine:
      case ExcelChartType.Scatter_Line_Markers:
      case ExcelChartType.Scatter_Line:
        writer.WriteStartElement("scatterChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        break;
      case ExcelChartType.Area:
      case ExcelChartType.Area_Stacked:
      case ExcelChartType.Area_Stacked_100:
        this.SerializeAreaChart(writer, chart);
        break;
      case ExcelChartType.Area_3D:
      case ExcelChartType.Area_Stacked_3D:
      case ExcelChartType.Area_Stacked_100_3D:
        this.SerializeArea3DChart(writer, chart);
        break;
      case ExcelChartType.Doughnut:
      case ExcelChartType.Doughnut_Exploded:
        this.SerializeDoughnutChart(writer, chart);
        break;
      case ExcelChartType.Radar:
      case ExcelChartType.Radar_Markers:
      case ExcelChartType.Radar_Filled:
        this.SerializeRadarChart(writer, chart);
        break;
      case ExcelChartType.Surface_3D:
      case ExcelChartType.Surface_NoColor_3D:
        this.SerializeSurface3DChart(writer, chart);
        break;
      case ExcelChartType.Surface_Contour:
      case ExcelChartType.Surface_NoColor_Contour:
        this.SerializeSurfaceChart(writer, chart);
        break;
      case ExcelChartType.Bubble:
      case ExcelChartType.Bubble_3D:
        writer.WriteStartElement("bubbleChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        break;
    }
  }

  private void SerializeRadarChart(XmlWriter writer, ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("radarChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    Excel2007RadarStyle pivotChartType = (Excel2007RadarStyle) chart.PivotChartType;
    ChartSerializatorCommon.SerializeValueTag(writer, "radarStyle", pivotChartType.ToString());
  }

  private int SerializeRadarChart(XmlWriter writer, ChartImpl chart, ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("radarChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    if (chart.RadarStyle != null)
    {
      ChartSerializatorCommon.SerializeValueTag(writer, "radarStyle", chart.RadarStyle);
    }
    else
    {
      Excel2007RadarStyle serieType = (Excel2007RadarStyle) firstSeries.SerieType;
      ChartSerializatorCommon.SerializeValueTag(writer, "radarStyle", serieType.ToString());
    }
    this.SerializeVaryColors(writer, firstSeries);
    int num = this.SerializeChartSeries(writer, chart, firstSeries, new ChartSerializator.SerializeSeriesDelegate(this.SerializeRadarSeries));
    if (chart.CommonDataPointsCollection != null && chart.CommonDataPointsCollection.ContainsKey(firstSeries.ChartGroup))
      this.SerializeDataLabels(writer, chart, chart.CommonDataPointsCollection[firstSeries.ChartGroup]);
    this.SerializeBarAxisId(writer, chart, firstSeries);
    writer.WriteEndElement();
    return num;
  }

  private int SerializeScatterChart(XmlWriter writer, ChartImpl chart, ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("scatterChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    Excel2007ScatterStyle serieType = (Excel2007ScatterStyle) firstSeries.SerieType;
    ChartSerializatorCommon.SerializeValueTag(writer, "scatterStyle", serieType.ToString());
    this.SerializeVaryColors(writer, firstSeries);
    int num = this.SerializeChartSeries(writer, chart, firstSeries, new ChartSerializator.SerializeSeriesDelegate(this.SerializeScatterSeries));
    if (chart.CommonDataPointsCollection != null && chart.CommonDataPointsCollection.ContainsKey(firstSeries.ChartGroup))
      this.SerializeDataLabels(writer, chart, chart.CommonDataPointsCollection[firstSeries.ChartGroup]);
    this.SerializeUpDownBars(writer, chart, firstSeries);
    this.SerializeBarAxisId(writer, chart, firstSeries);
    writer.WriteEndElement();
    return num;
  }

  private void SerializePieChart(XmlWriter writer, ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("pieChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    ChartSerializatorCommon.SerializeBoolValueTag(writer, "varyColors", true);
  }

  private int SerializePieChart(XmlWriter writer, ChartImpl chart, ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("pieChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    int num = this.SerializePieCommon(writer, chart, firstSeries);
    int firstSliceAngle = firstSeries.SerieFormat.CommonSerieOptions.FirstSliceAngle;
    ChartSerializatorCommon.SerializeValueTag(writer, "firstSliceAng", firstSliceAngle.ToString());
    writer.WriteEndElement();
    return num;
  }

  private void SerializePie3DChart(XmlWriter writer, ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("pie3DChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    ChartSerializatorCommon.SerializeBoolValueTag(writer, "varyColors", true);
  }

  private int SerializePie3DChart(XmlWriter writer, ChartImpl chart, ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("pie3DChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    int num = this.SerializePieCommon(writer, chart, firstSeries);
    writer.WriteEndElement();
    return num;
  }

  private void SerializeOfPieChart(XmlWriter writer, ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("ofPieChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    string str = chart.PivotChartType == ExcelChartType.PieOfPie ? "pie" : "bar";
    ChartSerializatorCommon.SerializeValueTag(writer, "ofPieType", str);
    ChartSerializatorCommon.SerializeBoolValueTag(writer, "varyColors", true);
  }

  private int SerializeOfPieChart(XmlWriter writer, ChartImpl chart, ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("ofPieChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    string str = firstSeries.SerieType == ExcelChartType.PieOfPie ? "pie" : "bar";
    ChartSerializatorCommon.SerializeValueTag(writer, "ofPieType", str);
    int num = this.SerializePieCommon(writer, chart, firstSeries);
    IWorkbook workbook = chart.Workbook;
    IChartFormat commonSerieOptions = firstSeries.SerieFormat.CommonSerieOptions;
    ChartSerializatorCommon.SerializeValueTag(writer, "gapWidth", commonSerieOptions.GapWidth.ToString());
    int splitValue = commonSerieOptions.SplitValue;
    IChartBorder pieSeriesLine = commonSerieOptions.PieSeriesLine;
    if (splitValue != 0)
    {
      Excel2007SplitType splitType = (Excel2007SplitType) commonSerieOptions.SplitType;
      ChartSerializatorCommon.SerializeValueTag(writer, "splitType", splitType.ToString());
      ChartSerializatorCommon.SerializeValueTag(writer, "splitPos", splitValue.ToString());
    }
    ChartSerializatorCommon.SerializeValueTag(writer, "secondPieSize", commonSerieOptions.PieSecondSize.ToString());
    writer.WriteStartElement("serLines", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteStartElement("spPr", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    ChartSerializatorCommon.SerializeLineProperties(writer, pieSeriesLine, workbook);
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
    return num;
  }

  private int SerializeStockChart(XmlWriter writer, ChartImpl chart, ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("stockChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    IChartSeries series1 = chart.Series;
    int index = 0;
    for (int count = series1.Count; index < count; ++index)
    {
      ChartSerieImpl series2 = (ChartSerieImpl) series1[index];
      this.SerializeLineSeries(writer, series2);
    }
    if (chart.CommonDataPointsCollection != null && chart.CommonDataPointsCollection.ContainsKey(firstSeries.ChartGroup))
      this.SerializeDataLabels(writer, chart, chart.CommonDataPointsCollection[firstSeries.ChartGroup]);
    ChartFormatImpl primaryFormat = chart.PrimaryFormats[0];
    if (primaryFormat.IsChartChartLine)
    {
      string localName = "";
      IChartBorder border = (IChartBorder) null;
      if (primaryFormat.HasHighLowLines)
      {
        localName = "hiLowLines";
        border = primaryFormat.HighLowLines;
      }
      else if (primaryFormat.HasDropLines)
      {
        localName = "dropLines";
        border = primaryFormat.DropLines;
      }
      if (primaryFormat.IsChartLineFormat)
      {
        writer.WriteStartElement(localName, "http://schemas.openxmlformats.org/drawingml/2006/chart");
        writer.WriteStartElement("spPr", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        ChartSerializatorCommon.SerializeLineProperties(writer, border, chart.Workbook);
        writer.WriteEndElement();
        writer.WriteEndElement();
      }
      else
        writer.WriteElementString(localName, "http://schemas.openxmlformats.org/drawingml/2006/chart", string.Empty);
    }
    this.SerializeUpDownBars(writer, chart, firstSeries);
    this.SerializeBarAxisId(writer, chart, firstSeries);
    writer.WriteEndElement();
    return series1.Count;
  }

  private void SerializeDoughnutChart(XmlWriter writer, ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("doughnutChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    ChartSerializatorCommon.SerializeBoolValueTag(writer, "varyColors", true);
    ChartSerializatorCommon.SerializeValueTag(writer, "holeSize", "50");
  }

  private int SerializeDoughnutChart(XmlWriter writer, ChartImpl chart, ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("doughnutChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    int num = this.SerializePieCommon(writer, chart, firstSeries);
    IChartFormat commonSerieOptions = firstSeries.SerieFormat.CommonSerieOptions;
    int firstSliceAngle = commonSerieOptions.FirstSliceAngle;
    ChartSerializatorCommon.SerializeValueTag(writer, "firstSliceAng", firstSliceAngle.ToString());
    int doughnutHoleSize = commonSerieOptions.DoughnutHoleSize;
    ChartSerializatorCommon.SerializeValueTag(writer, "holeSize", doughnutHoleSize.ToString());
    writer.WriteEndElement();
    return num;
  }

  private int SerializePieCommon(XmlWriter writer, ChartImpl chart, ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    this.SerializeVaryColors(writer, firstSeries);
    int num = this.SerializeChartSeries(writer, chart, firstSeries, new ChartSerializator.SerializeSeriesDelegate(this.SerializePieSeries));
    if (chart.CommonDataPointsCollection != null && chart.CommonDataPointsCollection.ContainsKey(firstSeries.ChartGroup))
      this.SerializeDataLabels(writer, chart, chart.CommonDataPointsCollection[firstSeries.ChartGroup]);
    return num;
  }

  private void SerializeDataLabels(
    XmlWriter writer,
    IChartDataLabels dataLabels,
    ChartSerieImpl series)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (dataLabels == null)
      throw new ArgumentNullException(nameof (dataLabels));
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    writer.WriteStartElement("dLbls", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    ChartImpl parentChart = series.ParentChart;
    ChartDataPointsCollection dataPoints = (ChartDataPointsCollection) series.DataPoints;
    if (dataPoints.DeninedDPCount > 0)
    {
      foreach (ChartDataPointImpl chartDataPointImpl in dataPoints)
      {
        if (!chartDataPointImpl.IsDefault && chartDataPointImpl.HasDataLabels)
          this.SerializeDataLabel(writer, chartDataPointImpl.DataLabels, chartDataPointImpl.Index, parentChart);
      }
    }
    ChartDataLabelsImpl dataLabels1 = series.DataPoints.DefaultDataPoint.DataLabels as ChartDataLabelsImpl;
    if (dataLabels1.NumberFormat != null)
      this.SerializeNumFormat(writer, dataLabels1);
    this.SerializeDataLabelSettings(writer, dataLabels, parentChart, parentChart.Workbook.Version > ExcelVersion.Excel2010);
    writer.WriteEndElement();
  }

  private void SerializeNumFormat(XmlWriter writer, ChartDataLabelsImpl dataLabels)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    bool flag = dataLabels != null ? dataLabels.IsSourceLinked : throw new ArgumentNullException(nameof (dataLabels));
    if (!(dataLabels.NumberFormat != "General") && flag)
      return;
    writer.WriteStartElement("numFmt", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteAttributeString("formatCode", dataLabels.NumberFormat);
    if (!flag)
      writer.WriteAttributeString("sourceLinked", Convert.ToInt16(flag).ToString());
    writer.WriteEndElement();
  }

  private void SerializeDataLabel(
    XmlWriter writer,
    IChartDataLabels dataLabels,
    int index,
    ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (dataLabels == null)
      throw new ArgumentNullException(nameof (dataLabels));
    writer.WriteStartElement("dLbl", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    ChartSerializatorCommon.SerializeValueTag(writer, "idx", index.ToString());
    ChartDataLabelsImpl dataLabels1 = dataLabels as ChartDataLabelsImpl;
    if (dataLabels1.NumberFormat != null)
      this.SerializeNumFormat(writer, dataLabels1);
    if (dataLabels is ChartDataLabelsImpl)
    {
      IChartLayout layout = (dataLabels as ChartDataLabelsImpl).Layout;
      if (layout != null)
      {
        IChartManualLayout manualLayout = layout.ManualLayout;
        if (manualLayout != null && (manualLayout.LayoutTarget != LayoutTargets.auto || manualLayout.LeftMode != LayoutModes.auto || manualLayout.TopMode != LayoutModes.auto || manualLayout.Left != 0.0 || manualLayout.Top != 0.0 || manualLayout.WidthMode != LayoutModes.auto || manualLayout.HeightMode != LayoutModes.auto || manualLayout.Width != 0.0 || manualLayout.Height != 0.0))
          ChartSerializatorCommon.SerializeLayout(writer, (object) (dataLabels as ChartDataLabelsImpl));
      }
    }
    IInternalChartTextArea textArea = dataLabels as IInternalChartTextArea;
    if (!string.IsNullOrEmpty(textArea.Text))
    {
      WorkbookImpl parentWorkbook = chart.ParentWorkbook;
      if (textArea is ChartTextAreaImpl && (textArea as ChartTextAreaImpl).Layout != null)
        ChartSerializatorCommon.SerializeLayout(writer, (object) (textArea as ChartTextAreaImpl));
      ChartSerializatorCommon.SerializeTextAreaText(writer, (IChartTextArea) textArea, (IWorkbook) parentWorkbook, 10.0);
    }
    this.SerializeDataLabelSettings(writer, dataLabels, chart, chart.Workbook.Version > ExcelVersion.Excel2010);
    writer.WriteEndElement();
  }

  private void SerializeDataLabelSettings(
    XmlWriter writer,
    IChartDataLabels dataLabels,
    ChartImpl chart,
    bool serializeLeaderLines)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    ChartDataLabelsImpl chartDataLabelsImpl = dataLabels != null ? dataLabels as ChartDataLabelsImpl : throw new ArgumentNullException(nameof (dataLabels));
    ChartDataPointImpl parent1 = dataLabels.Parent as ChartDataPointImpl;
    if (chartDataLabelsImpl.ShowTextProperties)
    {
      ChartSerializatorCommon.SerializeFrameFormat(writer, (IChartFillBorder) dataLabels.FrameFormat, chart, false);
      if (((ChartDataLabelsImpl) dataLabels).ParagraphType == ChartParagraphType.CustomDefault)
        this.SerializeDefaultTextFormatting(writer, (IChartTextArea) dataLabels, chart.Workbook, 10.0);
    }
    else if (chartDataLabelsImpl.IsDelete)
      ChartSerializatorCommon.SerializeBoolValueTag(writer, "delete", true);
    ExcelDataLabelPosition position = dataLabels.Position;
    switch (position)
    {
      case ExcelDataLabelPosition.Automatic:
      case ExcelDataLabelPosition.Moved:
        if (!chartDataLabelsImpl.IsDelete || parent1.IsDefault)
        {
          if (chart.Workbook.Version == ExcelVersion.Excel2007)
          {
            if (dataLabels.IsLegendKey)
              ChartSerializatorCommon.SerializeBoolValueTag(writer, "showLegendKey", dataLabels.IsLegendKey);
            if (dataLabels.IsValue)
              ChartSerializatorCommon.SerializeBoolValueTag(writer, "showVal", dataLabels.IsValue);
            if (dataLabels.IsCategoryName)
              ChartSerializatorCommon.SerializeBoolValueTag(writer, "showCatName", dataLabels.IsCategoryName);
            if (dataLabels.IsSeriesName)
              ChartSerializatorCommon.SerializeBoolValueTag(writer, "showSerName", dataLabels.IsSeriesName);
            if (dataLabels.IsPercentage)
              ChartSerializatorCommon.SerializeBoolValueTag(writer, "showPercent", dataLabels.IsPercentage);
            if (dataLabels.IsBubbleSize)
              ChartSerializatorCommon.SerializeBoolValueTag(writer, "showBubbleSize", dataLabels.IsBubbleSize);
          }
          else
          {
            ChartSerializatorCommon.SerializeBoolValueTag(writer, "showLegendKey", dataLabels.IsLegendKey);
            ChartSerializatorCommon.SerializeBoolValueTag(writer, "showVal", dataLabels.IsValue);
            ChartSerializatorCommon.SerializeBoolValueTag(writer, "showCatName", dataLabels.IsCategoryName);
            ChartSerializatorCommon.SerializeBoolValueTag(writer, "showSerName", dataLabels.IsSeriesName);
            ChartSerializatorCommon.SerializeBoolValueTag(writer, "showPercent", dataLabels.IsPercentage);
            ChartSerializatorCommon.SerializeBoolValueTag(writer, "showBubbleSize", dataLabels.IsBubbleSize);
          }
        }
        string delimiter = dataLabels.Delimiter;
        if (delimiter != null)
          writer.WriteElementString("separator", "http://schemas.openxmlformats.org/drawingml/2006/chart", delimiter);
        if (parent1.IsDefault && (serializeLeaderLines || chartDataLabelsImpl.CheckSerieIsPie || chart.IsChartPie))
        {
          ChartSerieImpl parent2 = parent1.IsDefault ? (parent1.Parent as ChartDataPointsCollection).Parent as ChartSerieImpl : (ChartSerieImpl) null;
          ChartSerializatorCommon.SerializeBoolValueTag(writer, "showLeaderLines", dataLabels.ShowLeaderLines);
          if (parent2 == null)
            break;
          writer.WriteStartElement("c", "extLst", (string) null);
          writer.WriteStartElement("c", "ext", (string) null);
          writer.WriteAttributeString("uri", "{CE6537A1-D6FC-4f65-9D91-7224C49458BB}");
          writer.WriteAttributeString("xmlns", "c15", (string) null, "http://schemas.microsoft.com/office/drawing/2012/chart");
          if (chartDataLabelsImpl.IsValueFromCells)
          {
            writer.WriteStartElement("c15", "showDataLabelsRange", (string) null);
            writer.WriteAttributeString("val", "1");
            writer.WriteEndElement();
          }
          writer.WriteStartElement("c15", "showLeaderLines", (string) null);
          writer.WriteAttributeString("val", !parent2.HasLeaderLines ? "0" : "1");
          writer.WriteEndElement();
          if (parent2.HasLeaderLines)
          {
            writer.WriteStartElement("c15", "leaderLines", (string) null);
            writer.WriteStartElement("spPr", "http://schemas.openxmlformats.org/drawingml/2006/chart");
            FileDataHolder parentHolder = chart.DataHolder.ParentHolder;
            ChartSerializatorCommon.SerializeLineProperties(writer, parent2.LeaderLines, (IWorkbook) parentHolder.Workbook);
            writer.WriteEndElement();
            writer.WriteEndElement();
          }
          writer.WriteEndElement();
          writer.WriteEndElement();
          break;
        }
        if (!chartDataLabelsImpl.IsValueFromCells)
          break;
        writer.WriteStartElement("c", "extLst", (string) null);
        writer.WriteStartElement("c", "ext", (string) null);
        writer.WriteAttributeString("uri", "{CE6537A1-D6FC-4f65-9D91-7224C49458BB}");
        writer.WriteAttributeString("xmlns", "c15", (string) null, "http://schemas.microsoft.com/office/drawing/2012/chart");
        writer.WriteStartElement("c15", "showDataLabelsRange", (string) null);
        writer.WriteAttributeString("val", "1");
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement();
        break;
      default:
        Excel2007DataLabelPos excel2007DataLabelPos = (Excel2007DataLabelPos) position;
        ChartSerializatorCommon.SerializeValueTag(writer, "dLblPos", excel2007DataLabelPos.ToString());
        goto case ExcelDataLabelPosition.Automatic;
    }
  }

  private void SerializeDefaultTextFormatting(
    XmlWriter writer,
    IChartTextArea textFormatting,
    IWorkbook book,
    double defaultFontSize)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (book == null)
      throw new ArgumentNullException(nameof (book));
    if (textFormatting == null)
      return;
    writer.WriteStartElement("txPr", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteStartElement("bodyPr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (textFormatting.TextRotationAngle != 0)
    {
      int num = textFormatting.TextRotationAngle * 60000;
      writer.WriteAttributeString("rot", num.ToString());
    }
    writer.WriteEndElement();
    writer.WriteStartElement("lstStyle", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteEndElement();
    writer.WriteStartElement("p", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteStartElement("pPr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    ChartSerializatorCommon.SerializeParagraphRunProperites(writer, (IFont) textFormatting, "defRPr", book, defaultFontSize);
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private void SerializeVaryColors(XmlWriter writer, ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (firstSeries == null)
      throw new ArgumentNullException(nameof (firstSeries));
    bool isVaryColor = firstSeries.SerieFormat.CommonSerieOptions.IsVaryColor;
    ChartSerializatorCommon.SerializeBoolValueTag(writer, "varyColors", isVaryColor);
  }

  private void SerializeBarSeries(XmlWriter writer, ChartSerieImpl series)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    this.SerializeSeriesCommonWithoutEnd(writer, series, series.IsFiltered);
    if ((series.DataPoints.DefaultDataPoint as ChartDataPointImpl).HasDataLabels || (series.DataPoints as ChartDataPointsCollection).CheckDPDataLabels())
      this.SerializeDataLabels(writer, series.DataPoints.DefaultDataPoint.DataLabels, series);
    this.SerializeTrendlines(writer, series.TrendLines, (IWorkbook) series.ParentBook);
    this.SerializeErrorBars(writer, series);
    if (this.categoryFilter == 0)
    {
      this.findFilter = this.FindFiltered(series);
      ++this.categoryFilter;
      if (this.findFilter)
      {
        this.UpdateCategoryLabel(series);
        this.UpdateFilteredValuesRange(series);
      }
    }
    if (!this.findFilter)
    {
      if (series.ParentChart.CategoryLabelLevel == ExcelCategoriesLabelLevel.CategoriesLabelLevelAll)
        this.SerializeSeriesCategory(writer, series);
      this.SerializeSeriesValues(writer, series);
    }
    else
    {
      if (series.ParentChart.CategoryLabelLevel == ExcelCategoriesLabelLevel.CategoriesLabelLevelAll)
        this.SerializeFilteredCategory(writer, series, this.findFilter);
      this.SerializeFilteredValues(writer, series, this.findFilter);
    }
    if (series.ParentChart.IsChart3D && (series.SerieFormat as ChartSerieDataFormatImpl).HasBarShape)
      this.SerializeBarShape(writer, series.SerieFormat);
    if (series.ParentChart.CategoryLabelLevel == ExcelCategoriesLabelLevel.CategoriesLabelLevelAll && series.ParentChart.SeriesNameLevel == ExcelSeriesNameLevel.SeriesNameLevelAll)
    {
      bool? invertIfNegative = series.InvertIfNegative;
      if ((!invertIfNegative.GetValueOrDefault() ? 0 : (invertIfNegative.HasValue ? 1 : 0)) == 0 && !series.DataPoints.DefaultDataPoint.DataLabels.IsValueFromCells)
        goto label_31;
    }
    writer.WriteStartElement("extLst", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    if (series.ParentChart.CategoryLabelLevel != ExcelCategoriesLabelLevel.CategoriesLabelLevelAll || series.ParentChart.SeriesNameLevel != ExcelSeriesNameLevel.SeriesNameLevelAll)
    {
      if (series.ParentChart.SeriesNameLevel != ExcelSeriesNameLevel.SeriesNameLevelAll)
        this.SerializeFilteredSeriesOrCategoryName(writer, series, true);
      if (series.ParentChart.CategoryLabelLevel != ExcelCategoriesLabelLevel.CategoriesLabelLevelAll)
        this.SerializeFilteredSeriesOrCategoryName(writer, series, false);
    }
    else
    {
      bool? invertIfNegative1 = series.InvertIfNegative;
      if ((!invertIfNegative1.GetValueOrDefault() ? 0 : (invertIfNegative1.HasValue ? 1 : 0)) != 0)
      {
        Stream fillFormatStream = series.m_invertFillFormatStream;
        bool? invertIfNegative2 = series.InvertIfNegative;
        if ((!invertIfNegative2.GetValueOrDefault() ? 0 : (invertIfNegative2.HasValue ? 1 : 0)) != 0 && series.SerieFormat.Fill.FillType == ExcelFillType.SolidColor)
        {
          writer.WriteStartElement("c", "ext", "http://schemas.openxmlformats.org/drawingml/2006/chart");
          writer.WriteAttributeString("uri", "{6F2FDCE9-48DA-4B69-8628-5D25D57E5C99}");
          writer.WriteAttributeString("xmlns", "c14", (string) null, "http://schemas.microsoft.com/office/drawing/2007/8/2/chart");
          this.SerializeInvertIfNegativeColor(writer, series);
          writer.WriteEndElement();
        }
      }
    }
    if (series.DataPoints.DefaultDataPoint.DataLabels.IsValueFromCells)
      this.SeriealizeValuesFromCellsRange(writer, series);
    writer.WriteEndElement();
label_31:
    writer.WriteEndElement();
  }

  private void SerializeInvertIfNegativeColor(XmlWriter writer, ChartSerieImpl series)
  {
    writer.WriteStartElement("c14", "invertSolidFillFmt", (string) null);
    writer.WriteStartElement("c14", "spPr", (string) null);
    writer.WriteAttributeString("xmlns", "c14", (string) null, "http://schemas.microsoft.com/office/drawing/2007/8/2/chart");
    ChartSerializatorCommon.SerializeSolidFill(writer, series.InvertIfNegativeColor, false, (IWorkbook) series.ParentBook, 1.0 - series.SerieFormat.Fill.Transparency);
    if (series.SerieFormat.HasLineProperties)
      ChartSerializatorCommon.SerializeLineProperties(writer, series.SerieFormat.LineProperties, (IWorkbook) series.ParentBook);
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private bool FindFiltered(ChartSerieImpl series)
  {
    IChartCategories categories = series.ParentChart.Categories;
    for (int index = 0; index < categories.Count; ++index)
    {
      if (categories[index].IsFiltered)
        return true;
    }
    return false;
  }

  private void UpdateFilteredValuesRange(ChartSerieImpl series)
  {
    ChartImpl parentChart = series.ParentChart;
    if (!this.HasCategoryFilters(parentChart))
      return;
    WorksheetBaseImpl worksheet1 = parentChart.DataRange.Worksheet as WorksheetBaseImpl;
    IChartSeries series1 = parentChart.Series;
    IWorksheet worksheet2 = worksheet1 as IWorksheet;
    IChartCategories categories = (IChartCategories) (parentChart.Categories as ChartCategoryCollection);
    int count1 = series.Values.Count;
    int count2 = categories.Count;
    int row1 = 0;
    int lastRow = 0;
    string str1 = string.Empty;
    int column = 0;
    int lastColumn = 0;
    int num = 0;
    for (int index1 = 0; index1 < categories[0].Values.Count; ++index1)
    {
      for (int index2 = 0; index2 < categories.Count; ++index2)
      {
        if (!categories[index2].IsFiltered && row1 == 0)
        {
          row1 = categories[index2].Values.Cells[index1].Row;
          column = categories[index2].Values.Cells[index1].Column;
        }
        else if (row1 != 0 && categories[index2].IsFiltered)
        {
          lastColumn = categories[index2].Values.Cells[index1].Column;
          lastRow = categories[index2].Values.Cells[index1].Row;
        }
        if (row1 != 0 && lastRow != 0)
        {
          IRange range = row1 != lastRow ? worksheet2.Range[row1, column, lastRow - 1, lastColumn] : worksheet2.Range[row1, column, lastRow, lastColumn - 1];
          str1 = str1 == string.Empty ? "(" + range.AddressGlobal : $"{str1},{range.AddressGlobal}";
          ++num;
          row1 = 0;
          lastRow = 0;
        }
      }
      if (row1 != 0)
      {
        lastColumn = categories[categories.Count - 1].Values.Cells[index1].Column;
        int row2 = categories[categories.Count - 1].Values.Cells[index1].Row;
        IRange range;
        if (row1 == row2)
        {
          range = worksheet2.Range[row1, column, row2, lastColumn];
          row1 = 0;
          lastRow = 0;
        }
        else
        {
          range = worksheet2.Range[row1, column, row2, lastColumn];
          row1 = 0;
          lastRow = 0;
        }
        str1 = str1 == string.Empty ? "(" + range.AddressGlobal : $"{str1},{range.AddressGlobal}";
        ++num;
      }
      string str2 = str1 + ")";
      (series1[index1] as ChartSerieImpl).FilteredValue = str2.Replace("'", "");
      str1 = string.Empty;
    }
  }

  private bool HasCategoryFilters(ChartImpl chart)
  {
    foreach (ChartCategory category in (IEnumerable<IChartCategory>) chart.Categories)
    {
      if (category.Filter_customize)
        return true;
    }
    return false;
  }

  private void UpdateCategoryLabel(ChartSerieImpl series)
  {
    ChartImpl parentChart = series.ParentChart;
    if (!this.HasCategoryFilters(parentChart) || series.CategoryLabels == null)
      return;
    WorksheetBaseImpl worksheet1 = parentChart.DataRange.Worksheet as WorksheetBaseImpl;
    IChartSeries series1 = parentChart.Series;
    IWorksheet worksheet2 = worksheet1 as IWorksheet;
    IChartCategories categories = (IChartCategories) (parentChart.Categories as ChartCategoryCollection);
    int count1 = series.Values.Count;
    int count2 = categories.Count;
    int row1 = 0;
    int lastRow = 0;
    string str1 = string.Empty;
    int column1 = 0;
    int lastColumn = 0;
    int num1 = 0;
    for (int index = 0; index < categories.Count; ++index)
    {
      if (!categories[index].IsFiltered && row1 == 0)
      {
        row1 = categories[index].CategoryLabel.Cells[index].Row;
        column1 = categories[index].CategoryLabel.Cells[index].Column;
      }
      else if (row1 != 0 && categories[index].IsFiltered)
      {
        lastColumn = categories[index].CategoryLabel.Cells[index].Column;
        lastRow = categories[index].CategoryLabel.Cells[index].Row;
      }
      if (row1 != 0 && lastRow != 0)
      {
        IRange range = row1 != lastRow ? worksheet2.Range[row1, column1, lastRow - 1, lastColumn] : worksheet2.Range[row1, column1, lastRow, lastColumn - 1];
        str1 = str1 == string.Empty ? "(" + range.AddressGlobal : $"{str1},{range.AddressGlobal}";
        ++num1;
        row1 = 0;
        lastRow = 0;
      }
    }
    if (row1 != 0)
    {
      int column2 = categories[categories.Count - 1].CategoryLabel.Cells[categories.Count - 1].Column;
      int row2 = categories[categories.Count - 1].CategoryLabel.Cells[categories.Count - 1].Row;
      IRange range;
      int num2;
      int num3;
      if (row1 == row2)
      {
        range = worksheet2.Range[row1, column1, row2, column2];
        num2 = 0;
        num3 = 0;
      }
      else
      {
        range = worksheet2.Range[row1, column1, row2, column2];
        num2 = 0;
        num3 = 0;
      }
      str1 = str1 == string.Empty ? "(" + range.AddressGlobal : $"{str1},{range.AddressGlobal}";
      int num4 = num1 + 1;
    }
    string str2 = (str1 + ")").Replace("'", "");
    for (int index = 0; index < series1.Count; ++index)
      (series1[index] as ChartSerieImpl).FilteredCategory = str2;
  }

  private void SerializeFilteredSeriesOrCategoryName(
    XmlWriter writer,
    ChartSerieImpl series,
    bool seriesOrcategory)
  {
    if (seriesOrcategory && !series.IsDefaultName)
    {
      writer.WriteStartElement("c", "ext", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      writer.WriteAttributeString("uri", "{02D57815-91ED-43cb-92C2-25804820EDAC}");
      writer.WriteAttributeString("xmlns", "c15", (string) null, "http://schemas.microsoft.com/office/drawing/2012/chart");
      writer.WriteStartElement("c15", "filteredSeriesTitle", "http://schemas.microsoft.com/office/drawing/2012/chart");
      this.SerializeFilteredText(writer, series);
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
    else
    {
      if (series.CategoryLabels == null)
        return;
      writer.WriteStartElement("c", "ext", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      writer.WriteAttributeString("uri", "{02D57815-91ED-43cb-92C2-25804820EDAC}");
      writer.WriteAttributeString("xmlns", "c15", (string) null, "http://schemas.microsoft.com/office/drawing/2012/chart");
      writer.WriteStartElement("c15", "filteredCategoryTitle", "http://schemas.microsoft.com/office/drawing/2012/chart");
      this.SerializeFilteredCategoryName(writer, series);
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
  }

  private void SeriealizeValuesFromCellsRange(XmlWriter writer, ChartSerieImpl series)
  {
    IRange valueFromCellsRange = series.DataPoints.DefaultDataPoint.DataLabels.ValueFromCellsRange;
    if (!series.DataPoints.DefaultDataPoint.DataLabels.IsValueFromCells || valueFromCellsRange == null)
      return;
    writer.WriteStartElement("c", "ext", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteAttributeString("uri", "{02D57815-91ED-43cb-92C2-25804820EDAC}");
    writer.WriteAttributeString("xmlns", "c15", (string) null, "http://schemas.microsoft.com/office/drawing/2012/chart");
    writer.WriteStartElement("c15", "datalabelsRange", (string) null);
    writer.WriteStartElement("c15", "f", (string) null);
    writer.WriteString(valueFromCellsRange.AddressGlobal);
    writer.WriteEndElement();
    this.serializeDataLabelRangeCache(writer, series.DataLabelCellsValues);
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private void serializeDataLabelRangeCache(XmlWriter writer, Dictionary<int, object> values)
  {
    if (values == null || values.Count <= 0)
      return;
    writer.WriteStartElement("c15", "dlblRangeCache", (string) null);
    int count = values.Count;
    ChartSerializatorCommon.SerializeValueTag(writer, "ptCount", count.ToString());
    if (count > 0)
    {
      foreach (KeyValuePair<int, object> keyValuePair in values)
      {
        if (keyValuePair.Value != null)
        {
          writer.WriteStartElement("pt", "http://schemas.openxmlformats.org/drawingml/2006/chart");
          writer.WriteAttributeString("idx", keyValuePair.Key.ToString());
          writer.WriteStartElement("v", "http://schemas.openxmlformats.org/drawingml/2006/chart");
          writer.WriteString(ChartSerializator.ToXmlString(keyValuePair.Value));
          writer.WriteEndElement();
          writer.WriteEndElement();
        }
      }
    }
    writer.WriteEndElement();
  }

  private void SerializeFilteredCategoryName(XmlWriter writer, ChartSerieImpl series)
  {
    if (series.CategoryLabels == null)
      return;
    writer.WriteStartElement("c15", "cat", "http://schemas.microsoft.com/office/drawing/2012/chart");
    writer.WriteStartElement("c", "strRef", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteStartElement("c", "extLst", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteStartElement("c", "ext", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteAttributeString("uri", "{02D57815-91ED-43cb-92C2-25804820EDAC}");
    writer.WriteStartElement("c15", "formulaRef", "http://schemas.microsoft.com/office/drawing/2012/chart");
    writer.WriteElementString("sqref", "http://schemas.microsoft.com/office/drawing/2012/chart", series.CategoryLabels.AddressGlobal);
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private void SerializePieSeries(XmlWriter writer, ChartSerieImpl series)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    this.SerializeSeriesCommonWithoutEnd(writer, series, false);
    int percent = series.SerieFormat.Percent;
    bool flag = series.SerieType == ExcelChartType.Pie_Exploded || series.SerieType == ExcelChartType.Pie_Exploded_3D;
    if (percent != 0 || series.ParentSeries[0].Equals((object) series) || flag)
    {
      if (percent == 0)
        percent = series.GetCommonSerieFormat().SerieDataFormat.Percent;
      ChartSerializatorCommon.SerializeValueTag(writer, "explosion", percent.ToString());
    }
    if ((series.DataPoints.DefaultDataPoint as ChartDataPointImpl).HasDataLabels || (series.DataPoints as ChartDataPointsCollection).CheckDPDataLabels())
      this.SerializeDataLabels(writer, series.DataPoints.DefaultDataPoint.DataLabels, series);
    this.SerializeTrendlines(writer, series.TrendLines, (IWorkbook) series.ParentBook);
    this.SerializeErrorBars(writer, series);
    if (this.categoryFilter == 0)
    {
      this.findFilter = this.FindFiltered(series);
      ++this.categoryFilter;
      if (this.findFilter)
      {
        this.UpdateCategoryLabel(series);
        this.UpdateFilteredValuesRange(series);
      }
    }
    if (!this.findFilter)
    {
      if (series.ParentChart.CategoryLabelLevel == ExcelCategoriesLabelLevel.CategoriesLabelLevelAll)
        this.SerializeSeriesCategory(writer, series);
      this.SerializeSeriesValues(writer, series);
    }
    else
    {
      if (series.ParentChart.CategoryLabelLevel == ExcelCategoriesLabelLevel.CategoriesLabelLevelAll)
        this.SerializeFilteredCategory(writer, series, this.findFilter);
      this.SerializeFilteredValues(writer, series, this.findFilter);
    }
    if (series.ParentChart.CategoryLabelLevel != ExcelCategoriesLabelLevel.CategoriesLabelLevelAll || series.ParentChart.SeriesNameLevel != ExcelSeriesNameLevel.SeriesNameLevelAll || series.DataPoints.DefaultDataPoint.DataLabels.IsValueFromCells)
    {
      writer.WriteStartElement("extLst", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      if (series.ParentChart.CategoryLabelLevel != ExcelCategoriesLabelLevel.CategoriesLabelLevelAll || series.ParentChart.SeriesNameLevel != ExcelSeriesNameLevel.SeriesNameLevelAll)
      {
        if (series.ParentChart.SeriesNameLevel != ExcelSeriesNameLevel.SeriesNameLevelAll)
          this.SerializeFilteredSeriesOrCategoryName(writer, series, true);
        if (series.ParentChart.CategoryLabelLevel != ExcelCategoriesLabelLevel.CategoriesLabelLevelAll)
          this.SerializeFilteredSeriesOrCategoryName(writer, series, false);
      }
      if (series.DataPoints.DefaultDataPoint.DataLabels.IsValueFromCells)
        this.SeriealizeValuesFromCellsRange(writer, series);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private void SerializeErrorBars(XmlWriter writer, ChartSerieImpl series)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    IWorkbook book = series != null ? (IWorkbook) series.ParentBook : throw new ArgumentNullException(nameof (series));
    if (series.HasErrorBarsX)
      this.SerializeErrorBars(writer, series.ErrorBarsX, "x", book, series);
    if (!series.HasErrorBarsY)
      return;
    this.SerializeErrorBars(writer, series.ErrorBarsY, "y", book, series);
  }

  private void SerializeLineSeries(XmlWriter writer, ChartSerieImpl series)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    this.SerializeSeriesCommonWithoutEnd(writer, series, false);
    if ((series.DataPoints.DefaultDataPoint as ChartDataPointImpl).HasDataLabels || (series.DataPoints as ChartDataPointsCollection).CheckDPDataLabels())
      this.SerializeDataLabels(writer, series.DataPoints.DefaultDataPoint.DataLabels, series);
    this.SerializeTrendlines(writer, series.TrendLines, (IWorkbook) series.ParentBook);
    this.SerializeErrorBars(writer, series);
    if (this.categoryFilter == 0)
    {
      this.findFilter = this.FindFiltered(series);
      ++this.categoryFilter;
      if (this.findFilter)
      {
        this.UpdateCategoryLabel(series);
        this.UpdateFilteredValuesRange(series);
      }
    }
    if (!this.findFilter)
    {
      if (series.ParentChart.CategoryLabelLevel == ExcelCategoriesLabelLevel.CategoriesLabelLevelAll)
        this.SerializeSeriesCategory(writer, series);
      this.SerializeSeriesValues(writer, series);
    }
    else
    {
      if (series.ParentChart.CategoryLabelLevel == ExcelCategoriesLabelLevel.CategoriesLabelLevelAll)
        this.SerializeFilteredCategory(writer, series, this.findFilter);
      this.SerializeFilteredValues(writer, series, this.findFilter);
    }
    if (series.ParentChart.CategoryLabelLevel != ExcelCategoriesLabelLevel.CategoriesLabelLevelAll || series.ParentChart.SeriesNameLevel != ExcelSeriesNameLevel.SeriesNameLevelAll || series.DataPoints.DefaultDataPoint.DataLabels.IsValueFromCells)
    {
      writer.WriteStartElement("extLst", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      if (series.ParentChart.CategoryLabelLevel != ExcelCategoriesLabelLevel.CategoriesLabelLevelAll || series.ParentChart.SeriesNameLevel != ExcelSeriesNameLevel.SeriesNameLevelAll)
      {
        if (series.ParentChart.SeriesNameLevel != ExcelSeriesNameLevel.SeriesNameLevelAll)
          this.SerializeFilteredSeriesOrCategoryName(writer, series, true);
        if (series.ParentChart.CategoryLabelLevel != ExcelCategoriesLabelLevel.CategoriesLabelLevelAll)
          this.SerializeFilteredSeriesOrCategoryName(writer, series, false);
      }
      if (series.DataPoints.DefaultDataPoint.DataLabels.IsValueFromCells)
        this.SeriealizeValuesFromCellsRange(writer, series);
      writer.WriteEndElement();
    }
    ChartSerieDataFormatImpl serieFormat = (ChartSerieDataFormatImpl) series.SerieFormat;
    ChartSerializatorCommon.SerializeBoolValueTag(writer, "smooth", serieFormat.IsSmoothed);
    writer.WriteEndElement();
  }

  private void SerializeScatterSeries(XmlWriter writer, ChartSerieImpl series)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    this.SerializeSeriesCommonWithoutEnd(writer, series, false);
    if ((series.DataPoints.DefaultDataPoint as ChartDataPointImpl).HasDataLabels || (series.DataPoints as ChartDataPointsCollection).CheckDPDataLabels())
      this.SerializeDataLabels(writer, series.DataPoints.DefaultDataPoint.DataLabels, series);
    this.SerializeTrendlines(writer, series.TrendLines, (IWorkbook) series.ParentBook);
    this.SerializeErrorBars(writer, series);
    this.SerializeSeriesCategory(writer, series, "xVal");
    this.SerializeSeriesValues(writer, series, "yVal");
    ChartSerieDataFormatImpl serieFormat = (ChartSerieDataFormatImpl) series.SerieFormat;
    ChartSerializatorCommon.SerializeBoolValueTag(writer, "smooth", serieFormat.IsSmoothed);
    if (series.ParentChart.CategoryLabelLevel != ExcelCategoriesLabelLevel.CategoriesLabelLevelAll || series.ParentChart.SeriesNameLevel != ExcelSeriesNameLevel.SeriesNameLevelAll || series.DataPoints.DefaultDataPoint.DataLabels.IsValueFromCells)
    {
      writer.WriteStartElement("extLst", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      if (series.ParentChart.CategoryLabelLevel != ExcelCategoriesLabelLevel.CategoriesLabelLevelAll || series.ParentChart.SeriesNameLevel != ExcelSeriesNameLevel.SeriesNameLevelAll)
      {
        if (series.ParentChart.SeriesNameLevel != ExcelSeriesNameLevel.SeriesNameLevelAll)
          this.SerializeFilteredSeriesOrCategoryName(writer, series, true);
        if (series.ParentChart.CategoryLabelLevel != ExcelCategoriesLabelLevel.CategoriesLabelLevelAll)
          this.SerializeFilteredSeriesOrCategoryName(writer, series, false);
      }
      if (series.DataPoints.DefaultDataPoint.DataLabels.IsValueFromCells)
        this.SeriealizeValuesFromCellsRange(writer, series);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private void SerializeRadarSeries(XmlWriter writer, ChartSerieImpl series)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    this.SerializeSeriesCommonWithoutEnd(writer, series, false);
    if ((series.DataPoints.DefaultDataPoint as ChartDataPointImpl).HasDataLabels || (series.DataPoints as ChartDataPointsCollection).CheckDPDataLabels())
      this.SerializeDataLabels(writer, series.DataPoints.DefaultDataPoint.DataLabels, series);
    if (this.categoryFilter == 0)
    {
      this.findFilter = this.FindFiltered(series);
      ++this.categoryFilter;
      if (this.findFilter)
      {
        this.UpdateCategoryLabel(series);
        this.UpdateFilteredValuesRange(series);
      }
    }
    if (!this.findFilter)
    {
      if (series.ParentChart.CategoryLabelLevel == ExcelCategoriesLabelLevel.CategoriesLabelLevelAll)
        this.SerializeSeriesCategory(writer, series);
      this.SerializeSeriesValues(writer, series);
    }
    else
    {
      if (series.ParentChart.CategoryLabelLevel == ExcelCategoriesLabelLevel.CategoriesLabelLevelAll)
        this.SerializeFilteredCategory(writer, series, this.findFilter);
      this.SerializeFilteredValues(writer, series, this.findFilter);
    }
    if (series.ParentChart.CategoryLabelLevel != ExcelCategoriesLabelLevel.CategoriesLabelLevelAll || series.ParentChart.SeriesNameLevel != ExcelSeriesNameLevel.SeriesNameLevelAll || series.DataPoints.DefaultDataPoint.DataLabels.IsValueFromCells)
    {
      writer.WriteStartElement("extLst", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      if (series.ParentChart.CategoryLabelLevel != ExcelCategoriesLabelLevel.CategoriesLabelLevelAll || series.ParentChart.SeriesNameLevel != ExcelSeriesNameLevel.SeriesNameLevelAll)
      {
        if (series.ParentChart.SeriesNameLevel != ExcelSeriesNameLevel.SeriesNameLevelAll)
          this.SerializeFilteredSeriesOrCategoryName(writer, series, true);
        if (series.ParentChart.CategoryLabelLevel != ExcelCategoriesLabelLevel.CategoriesLabelLevelAll)
          this.SerializeFilteredSeriesOrCategoryName(writer, series, false);
      }
      if (series.DataPoints.DefaultDataPoint.DataLabels.IsValueFromCells)
        this.SeriealizeValuesFromCellsRange(writer, series);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private void SerializeBubbleSeries(XmlWriter writer, ChartSerieImpl series)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    this.SerializeSeriesCommonWithoutEnd(writer, series, false);
    if ((series.DataPoints.DefaultDataPoint as ChartDataPointImpl).HasDataLabels || (series.DataPoints as ChartDataPointsCollection).CheckDPDataLabels())
      this.SerializeDataLabels(writer, series.DataPoints.DefaultDataPoint.DataLabels, series);
    this.SerializeTrendlines(writer, series.TrendLines, (IWorkbook) series.ParentBook);
    this.SerializeErrorBars(writer, series);
    this.SerializeSeriesCategory(writer, series, "xVal");
    this.SerializeSeriesValues(writer, series, "yVal");
    this.SerializeSeriesValues(writer, series.Bubbles, series.EnteredDirectlyBubbles, "bubbleSize", series);
    if (series.SerieType == ExcelChartType.Bubble_3D)
      ChartSerializatorCommon.SerializeValueTag(writer, "bubble3D", "1");
    if (series.ParentChart.CategoryLabelLevel != ExcelCategoriesLabelLevel.CategoriesLabelLevelAll || series.ParentChart.SeriesNameLevel != ExcelSeriesNameLevel.SeriesNameLevelAll || series.DataPoints.DefaultDataPoint.DataLabels.IsValueFromCells)
    {
      writer.WriteStartElement("extLst", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      if (series.ParentChart.CategoryLabelLevel != ExcelCategoriesLabelLevel.CategoriesLabelLevelAll || series.ParentChart.SeriesNameLevel != ExcelSeriesNameLevel.SeriesNameLevelAll)
      {
        if (series.ParentChart.SeriesNameLevel != ExcelSeriesNameLevel.SeriesNameLevelAll)
          this.SerializeFilteredSeriesOrCategoryName(writer, series, true);
        if (series.ParentChart.CategoryLabelLevel != ExcelCategoriesLabelLevel.CategoriesLabelLevelAll)
          this.SerializeFilteredSeriesOrCategoryName(writer, series, false);
      }
      if (series.DataPoints.DefaultDataPoint.DataLabels.IsValueFromCells)
        this.SeriealizeValuesFromCellsRange(writer, series);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private void SerializeAreaSeries(XmlWriter writer, ChartSerieImpl series)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    this.SerializeSeriesCommonWithoutEnd(writer, series, false);
    if ((series.DataPoints.DefaultDataPoint as ChartDataPointImpl).HasDataLabels || (series.DataPoints as ChartDataPointsCollection).CheckDPDataLabels())
      this.SerializeDataLabels(writer, series.DataPoints.DefaultDataPoint.DataLabels, series);
    this.SerializeTrendlines(writer, series.TrendLines, (IWorkbook) series.ParentBook);
    this.SerializeErrorBars(writer, series);
    if (this.categoryFilter == 0)
    {
      this.findFilter = this.FindFiltered(series);
      ++this.categoryFilter;
      if (this.findFilter)
      {
        this.UpdateCategoryLabel(series);
        this.UpdateFilteredValuesRange(series);
      }
    }
    if (!this.findFilter)
    {
      if (series.ParentChart.CategoryLabelLevel == ExcelCategoriesLabelLevel.CategoriesLabelLevelAll)
        this.SerializeSeriesCategory(writer, series);
      this.SerializeSeriesValues(writer, series);
    }
    else
    {
      if (series.ParentChart.CategoryLabelLevel == ExcelCategoriesLabelLevel.CategoriesLabelLevelAll)
        this.SerializeFilteredCategory(writer, series, this.findFilter);
      this.SerializeFilteredValues(writer, series, this.findFilter);
    }
    if (series.ParentChart.CategoryLabelLevel != ExcelCategoriesLabelLevel.CategoriesLabelLevelAll || series.ParentChart.SeriesNameLevel != ExcelSeriesNameLevel.SeriesNameLevelAll || series.DataPoints.DefaultDataPoint.DataLabels.IsValueFromCells)
    {
      writer.WriteStartElement("extLst", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      if (series.ParentChart.CategoryLabelLevel != ExcelCategoriesLabelLevel.CategoriesLabelLevelAll || series.ParentChart.SeriesNameLevel != ExcelSeriesNameLevel.SeriesNameLevelAll)
      {
        if (series.ParentChart.SeriesNameLevel != ExcelSeriesNameLevel.SeriesNameLevelAll)
          this.SerializeFilteredSeriesOrCategoryName(writer, series, true);
        if (series.ParentChart.CategoryLabelLevel != ExcelCategoriesLabelLevel.CategoriesLabelLevelAll)
          this.SerializeFilteredSeriesOrCategoryName(writer, series, false);
      }
      if (series.DataPoints.DefaultDataPoint.DataLabels.IsValueFromCells)
        this.SeriealizeValuesFromCellsRange(writer, series);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private void SerializeSeriesCommonWithoutEnd(
    XmlWriter writer,
    ChartSerieImpl series,
    bool isFiltered)
  {
    string tagName = (string) null;
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    if (!isFiltered)
    {
      writer.WriteStartElement("ser", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      ChartSerializatorCommon.SerializeValueTag(writer, "idx", series.Number.ToString());
      ChartSerializatorCommon.SerializeValueTag(writer, "order", series.Index.ToString());
      series.CheckLimits();
      if (!series.IsDefaultName && series.ParentChart.SeriesNameLevel == ExcelSeriesNameLevel.SeriesNameLevelAll)
      {
        string nameOrFormula = series.NameOrFormula;
        writer.WriteStartElement("tx", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        if (nameOrFormula.Length > 0 && nameOrFormula[0] == '=')
        {
          this.SerializeStringReference(writer, nameOrFormula, series, true, tagName);
        }
        else
        {
          writer.WriteStartElement("v", "http://schemas.openxmlformats.org/drawingml/2006/chart");
          writer.WriteString(nameOrFormula);
          writer.WriteEndElement();
        }
        writer.WriteEndElement();
      }
    }
    ChartSerieDataFormatImpl dataFormatOrNull = ((ChartDataPointImpl) series.DataPoints.DefaultDataPoint).DataFormatOrNull;
    if (dataFormatOrNull != null)
      ChartSerializatorCommon.SerializeFrameFormat(writer, (IChartFillBorder) dataFormatOrNull, series.ParentChart, false, true);
    if (series.InvertIfNegative.HasValue)
    {
      bool? invertIfNegative = series.InvertIfNegative;
      string str = (!invertIfNegative.GetValueOrDefault() ? 0 : (invertIfNegative.HasValue ? 1 : 0)) != 0 ? "1" : "0";
      ChartSerializatorCommon.SerializeValueTag(writer, "invertIfNegative", str);
    }
    this.SerializeMarker(writer, series);
    ChartDataPointsCollection dataPoints = (ChartDataPointsCollection) series.DataPoints;
    bool flag = true;
    if (dataPoints.DeninedDPCount > 1)
    {
      foreach (IChartDataPoint chartDataPoint in dataPoints)
      {
        if (!chartDataPoint.IsDefault && !(series.SerieFormat as ChartSerieDataFormatImpl).CompareFormat(chartDataPoint.DataFormat))
          flag = false;
      }
    }
    else
      flag = false;
    if (dataPoints.DeninedDPCount <= 0 || flag)
      return;
    foreach (ChartDataPointImpl dataPoint in dataPoints)
    {
      if (!dataPoint.IsDefault || dataPoint.HasDataPoint)
        this.SerializeDataPoint(writer, dataPoint, series);
    }
  }

  private void SerializeDataPoint(
    XmlWriter writer,
    ChartDataPointImpl dataPoint,
    ChartSerieImpl series)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    ChartSerieDataFormatImpl serieDataFormatImpl = dataPoint != null ? dataPoint.DataFormatOrNull : throw new ArgumentNullException(nameof (dataPoint));
    if (serieDataFormatImpl == null || !serieDataFormatImpl.IsFormatted && !serieDataFormatImpl.IsParsed)
      return;
    string str = series.SerieType.ToString();
    writer.WriteStartElement("dPt", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    ChartSerializatorCommon.SerializeValueTag(writer, "idx", dataPoint.Index.ToString());
    ChartSerializatorCommon.SerializeBoolValueTag(writer, "bubble3D", dataPoint.Bubble3D);
    if (dataPoint.HasExplosion)
      ChartSerializatorCommon.SerializeValueTag(writer, "explosion", dataPoint.Explosion.ToString());
    if (serieDataFormatImpl.IsSupportFill && serieDataFormatImpl.HasInterior && (str.IndexOf("Column") != -1 || str.IndexOf("Bar") != -1 || str.IndexOf("Pyramid") != -1 || str.IndexOf("Cylinder") != -1 || str.IndexOf("Cone") != -1))
      ChartSerializatorCommon.SerializeBoolValueTag(writer, "invertIfNegative", serieDataFormatImpl.Interior.SwapColorsOnNegative);
    else if (serieDataFormatImpl.IsSupportFill && series.ParentChart.IsParsed && (str.IndexOf("Column") != -1 || str.IndexOf("Bar") != -1 || str.IndexOf("Pyramid") != -1 || str.IndexOf("Cylinder") != -1 || str.IndexOf("Cone") != -1))
      ChartSerializatorCommon.SerializeBoolValueTag(writer, "invertIfNegative", (dataPoint.DataFormat.Fill as ChartFillImpl).InvertIfNegative);
    if (series.ParentChart.IsChartPie && !series.SerieFormat.LineProperties.AutoFormat && !series.SerieFormat.Interior.UseAutomaticFormat && (series.SerieFormat.HasInterior || series.SerieFormat.HasShadowProperties || series.SerieFormat.HasLineProperties))
      ChartSerializatorCommon.SerializeFrameFormat(writer, (IChartFillBorder) series.SerieFormat, series.ParentChart, false);
    else if (!serieDataFormatImpl.IsDefault)
      ChartSerializatorCommon.SerializeFrameFormat(writer, (IChartFillBorder) serieDataFormatImpl, serieDataFormatImpl.ParentChart, false);
    if (dataPoint.IsDefaultmarkertype)
      this.SerializeMarker(writer, serieDataFormatImpl);
    writer.WriteEndElement();
  }

  private void SerializeSeriesCategory(XmlWriter writer, ChartSerieImpl series, string tagName)
  {
    this.SerializeSeriesValues(writer, series.CategoryLabels, series.EnteredDirectlyCategoryLabels, tagName, series);
  }

  private void SerializeSeriesCategory(XmlWriter writer, ChartSerieImpl series)
  {
    this.SerializeSeriesValues(writer, series.CategoryLabels, series.EnteredDirectlyCategoryLabels, "cat", series);
  }

  private void SerializeSeriesValues(XmlWriter writer, ChartSerieImpl series)
  {
    this.SerializeSeriesValues(writer, series.Values, series.EnteredDirectlyValues, "val", series);
  }

  private void SerializeSeriesValues(XmlWriter writer, ChartSerieImpl series, string tagName)
  {
    this.SerializeSeriesValues(writer, series.Values, series.EnteredDirectlyValues, tagName, series);
  }

  private void SerializeSeriesValues(
    XmlWriter writer,
    IRange range,
    object[] values,
    string tagName,
    ChartSerieImpl series)
  {
    if (range == null && values == null && series.NumRefFormula == null && series.StrRefFormula == null && series.MulLvlStrRefFormula == null)
      return;
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (tagName == null || tagName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (tagName));
    WorkbookImpl workbookImpl = (WorkbookImpl) null;
    if (range != null && range.Worksheet != null)
      workbookImpl = range.Worksheet.Workbook as WorkbookImpl;
    writer.WriteStartElement(tagName, "http://schemas.openxmlformats.org/drawingml/2006/chart");
    if (workbookImpl != null)
    {
      bool flag1 = false;
      bool flag2 = false;
      switch (range)
      {
        case ExternalRange _:
          flag1 = (range as ExternalRange).IsNumReference || (range as ExternalRange).IsStringReference;
          flag2 = (range as ExternalRange).IsMultiReference;
          break;
        case RangeImpl _:
          flag1 = (range as RangeImpl).IsNumReference || (range as RangeImpl).IsStringReference;
          flag2 = (range as RangeImpl).IsMultiReference;
          break;
        case NameImpl _:
          flag1 = (range as NameImpl).IsNumReference || (range as NameImpl).IsStringReference;
          flag2 = (range as NameImpl).IsMultiReference;
          break;
      }
      if (range != null && !flag1 && !flag2)
        this.SerializeNormalReference(writer, range, values, tagName, series);
      else if (range != null && flag1)
        this.SerializeReference(writer, range, values, series, tagName);
      else if (range != null && flag2)
        this.SerializeMultiLevelStringReference(writer, range, values, series, tagName);
      else if (values != null)
      {
        string formatCode = (string) null;
        switch (tagName)
        {
          case "xVal":
          case "cat":
            formatCode = series.CategoriesFormatCode;
            break;
          case "yVal":
          case "val":
            formatCode = series.FormatCode;
            break;
        }
        this.SerializeDirectlyEntered(writer, values, false, formatCode);
      }
    }
    else if (range != null && tagName != "cat")
      this.SerializeReference(writer, range, values, series, tagName);
    else if (range != null && tagName == "cat")
      this.SerializeMultiLevelStringReference(writer, range, values, series, tagName);
    else if (values != null)
    {
      string formatCode = (string) null;
      switch (tagName)
      {
        case "xVal":
        case "cat":
          formatCode = series.CategoriesFormatCode;
          break;
        case "yVal":
        case "val":
          formatCode = series.FormatCode;
          break;
      }
      this.SerializeDirectlyEntered(writer, values, false, tagName, formatCode);
    }
    else if (series.StrRefFormula != null)
      this.SerializeFormula(writer, "strRef", series.StrRefFormula);
    else if (series.NumRefFormula != null)
      this.SerializeFormula(writer, "numRef", series.NumRefFormula);
    else if (series.MulLvlStrRefFormula != null)
      this.SerializeFormula(writer, "multiLvlStrRef", series.MulLvlStrRefFormula);
    writer.WriteEndElement();
  }

  private void SerializeNormalReference(
    XmlWriter writer,
    IRange range,
    object[] values,
    string tagName,
    ChartSerieImpl series)
  {
    if (range != null && tagName != "cat")
      this.SerializeReference(writer, range, values, series, tagName);
    else if (range != null && tagName == "cat")
    {
      this.SerializeStringReference(writer, range, series, tagName);
    }
    else
    {
      if (values == null)
        return;
      string formatCode = (string) null;
      switch (tagName)
      {
        case "xVal":
        case "cat":
          formatCode = series.CategoriesFormatCode;
          break;
        case "yVal":
        case "val":
          formatCode = series.FormatCode;
          break;
      }
      this.SerializeDirectlyEntered(writer, values, false, formatCode);
    }
  }

  private void SerializeReference(
    XmlWriter writer,
    IRange range,
    object[] rangeValues,
    ChartSerieImpl series,
    string tagName)
  {
    bool flag = range != null ? range.HasString : throw new ArgumentNullException(nameof (range));
    if (range.Worksheet != null)
      flag = this.GetStringReference(range);
    if (flag && tagName != "yVal" && tagName != "val")
      this.SerializeStringReference(writer, range, series, tagName);
    else
      this.SerializeNumReference(writer, range, rangeValues, series, tagName);
  }

  private bool GetStringReference(IRange range)
  {
    if (range.Worksheet.Workbook is WorkbookImpl workbook && (workbook.IsCreated || workbook.IsConverted))
      return range.HasString;
    switch (range)
    {
      case ExternalRange _:
        return (range as ExternalRange).IsStringReference;
      case RangeImpl _:
        return (range as RangeImpl).IsStringReference;
      case NameImpl _:
        return (range as NameImpl).IsStringReference;
      default:
        return false;
    }
  }

  private void SerializeNumReference(
    XmlWriter writer,
    IRange range,
    object[] rangeValues,
    ChartSerieImpl series,
    string tagName)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    string str = range is ICombinedRange combinedRange ? combinedRange.AddressGlobal2007 : range.AddressGlobal;
    if (combinedRange != null && !series.InnerChart.IsAddCopied && !(combinedRange is ExternalRange))
      rangeValues = (object[]) null;
    writer.WriteStartElement("numRef", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    if (series.InnerChart.IsAddCopied)
      writer.WriteElementString("f", "http://schemas.openxmlformats.org/drawingml/2006/chart", "[]" + str);
    else
      writer.WriteElementString("f", "http://schemas.openxmlformats.org/drawingml/2006/chart", str);
    if (rangeValues == null)
    {
      IRange values = series.Values;
      IWorksheet worksheet;
      if (values != null && values.AddressLocal != null && values.Worksheet.Application != null && (worksheet = values.Worksheet).Workbook.Worksheets[worksheet.Name] != null && values.Application.IsChartCacheEnabled)
      {
        writer.WriteStartElement("numCache", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        writer.WriteElementString("formatCode", "http://schemas.openxmlformats.org/drawingml/2006/chart", series.Values.NumberFormat);
        this.SerializeNumCacheValues(writer, series);
        writer.WriteEndElement();
      }
      else
        writer.WriteElementString("numCache", "http://schemas.openxmlformats.org/drawingml/2006/chart", string.Empty);
    }
    else
    {
      string formatCode = (string) null;
      switch (tagName)
      {
        case "xVal":
        case "cat":
          formatCode = series.CategoriesFormatCode;
          break;
        case "yVal":
        case "val":
          formatCode = series.FormatCode;
          break;
      }
      this.SerializeDirectlyEntered(writer, rangeValues, true, formatCode);
    }
    writer.WriteEndElement();
  }

  private void SerializeNumCacheValues(XmlWriter writer, ChartSerieImpl series)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    int index = 0;
    if (series.EnteredDirectlyValues != null)
    {
      ChartSerializatorCommon.SerializeValueTag(writer, "ptCount", series.EnteredDirectlyValues.Length.ToString());
      for (; index < series.EnteredDirectlyValues.Length; ++index)
      {
        if (series.EnteredDirectlyValues[index] != null)
        {
          string xmlString = ChartSerializator.ToXmlString(series.EnteredDirectlyValues[index]);
          writer.WriteStartElement("pt", "http://schemas.openxmlformats.org/drawingml/2006/chart");
          writer.WriteAttributeString("idx", index.ToString());
          writer.WriteElementString("v", "http://schemas.openxmlformats.org/drawingml/2006/chart", xmlString);
          writer.WriteEndElement();
        }
      }
    }
    else
    {
      IRange values = series.Values;
      IWorksheet worksheet = values.Worksheet;
      worksheet.EnableSheetCalculations();
      ChartSerializatorCommon.SerializeValueTag(writer, "ptCount", values.Count.ToString());
      for (; index < values.Count && !(values.Worksheet is ExternWorksheetImpl); ++index)
      {
        string calculatedValue = worksheet.Range[values.Cells[index].AddressLocal].CalculatedValue;
        double result;
        if (calculatedValue != null && double.TryParse(calculatedValue, out result))
        {
          writer.WriteStartElement("pt", "http://schemas.openxmlformats.org/drawingml/2006/chart");
          writer.WriteAttributeString("idx", index.ToString());
          writer.WriteElementString("v", "http://schemas.openxmlformats.org/drawingml/2006/chart", XmlConvert.ToString(result));
          writer.WriteEndElement();
        }
      }
      worksheet.DisableSheetCalculations();
    }
  }

  private void SerializeStringReference(
    XmlWriter writer,
    IRange range,
    ChartSerieImpl series,
    string tagName)
  {
    string range1 = range is ICombinedRange combinedRange ? combinedRange.AddressGlobal2007 : range.AddressGlobal;
    this.SerializeStringReference(writer, range1, series, false, tagName);
  }

  private void SerializeStringReference(
    XmlWriter writer,
    string range,
    ChartSerieImpl series,
    bool hasSeriesName,
    string tagName)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    if (range[0] == '=')
      range = UtilityMethods.RemoveFirstCharUnsafe(range);
    writer.WriteStartElement("strRef", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteElementString("f", "http://schemas.openxmlformats.org/drawingml/2006/chart", range);
    if (series.Application.IsChartCacheEnabled)
    {
      writer.WriteStartElement("strCache", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      if (tagName == "cat")
        this.SerializeCategoryTagCacheValues(writer, series);
      else
        this.SerializeTextTagCacheValues(writer, series);
    }
    else
      writer.WriteElementString("strCache", "http://schemas.openxmlformats.org/drawingml/2006/chart", string.Empty);
    writer.WriteEndElement();
  }

  private void SerializeTextTagCacheValues(XmlWriter writer, ChartSerieImpl series)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    int num1 = 0;
    int num2 = 1;
    ChartSerializatorCommon.SerializeValueTag(writer, "ptCount", num2.ToString());
    writer.WriteStartElement("pt", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteAttributeString("idx", num1.ToString());
    if (series.Name != null)
      writer.WriteElementString("v", "http://schemas.openxmlformats.org/drawingml/2006/chart", series.Name);
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private void SerializeCategoryTagCacheValues(XmlWriter writer, ChartSerieImpl series)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    int index = 0;
    ChartImpl parentChart = series.ParentChart;
    if (series.EnteredDirectlyCategoryLabels != null || parentChart.CategoryLabelValues != null)
    {
      if (series.EnteredDirectlyCategoryLabels == null)
      {
        ChartSerializatorCommon.SerializeValueTag(writer, "ptCount", parentChart.CategoryLabelValues.Length.ToString());
        for (; index < parentChart.CategoryLabelValues.Length; ++index)
        {
          writer.WriteStartElement("pt", "http://schemas.openxmlformats.org/drawingml/2006/chart");
          writer.WriteAttributeString("idx", index.ToString());
          writer.WriteElementString("v", "http://schemas.openxmlformats.org/drawingml/2006/chart", ChartSerializator.ToXmlString(parentChart.CategoryLabelValues[index]));
          writer.WriteEndElement();
        }
      }
      else
      {
        ChartSerializatorCommon.SerializeValueTag(writer, "ptCount", series.EnteredDirectlyCategoryLabels.Length.ToString());
        for (; index < series.EnteredDirectlyCategoryLabels.Length; ++index)
        {
          writer.WriteStartElement("pt", "http://schemas.openxmlformats.org/drawingml/2006/chart");
          writer.WriteAttributeString("idx", index.ToString());
          writer.WriteElementString("v", "http://schemas.openxmlformats.org/drawingml/2006/chart", ChartSerializator.ToXmlString(series.EnteredDirectlyCategoryLabels[index]));
          writer.WriteEndElement();
        }
      }
    }
    else
    {
      IRange categoryLabels = series.CategoryLabels;
      int count = categoryLabels.Count;
      int row = categoryLabels.Row;
      int column = categoryLabels.Column;
      IWorksheet worksheet = categoryLabels.Worksheet;
      ChartSerializatorCommon.SerializeValueTag(writer, "ptCount", count.ToString());
      for (; index < count; ++index)
      {
        string text = worksheet.GetText(row + index, column);
        writer.WriteStartElement("pt", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        writer.WriteAttributeString("idx", index.ToString());
        writer.WriteElementString("v", "http://schemas.openxmlformats.org/drawingml/2006/chart", text);
        writer.WriteEndElement();
      }
    }
    writer.WriteEndElement();
  }

  private void SerializeMultiLevelStringReference(
    XmlWriter writer,
    IRange range,
    object[] rangeValues,
    ChartSerieImpl series,
    string tagName)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    string str = range is ICombinedRange combinedRange ? combinedRange.AddressGlobal2007 : range.AddressGlobal;
    writer.WriteStartElement("multiLvlStrRef", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteElementString("f", "http://schemas.openxmlformats.org/drawingml/2006/chart", str);
    this.SerializeMultiLevelStringCache(writer, series);
    writer.WriteEndElement();
  }

  private void SerializeMultiLevelStringCache(XmlWriter writer, ChartSerieImpl series)
  {
    int count = series.MultiLevelStrCache.Count;
    writer.WriteStartElement("multiLvlStrCache", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    if (count != 0)
    {
      int num = series.PointCount;
      ChartSerializatorCommon.SerializeValueTag(writer, "ptCount", num.ToString());
      for (int key = 0; key < count; ++key)
      {
        writer.WriteStartElement("lvl", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        object[] objArray = series.MultiLevelStrCache[key];
        num = series.MultiLevelStrCache[key].Length;
        for (int index = 0; index < num; ++index)
        {
          if (objArray[index] != null)
          {
            writer.WriteStartElement("pt", "http://schemas.openxmlformats.org/drawingml/2006/chart");
            writer.WriteAttributeString("idx", index.ToString());
            writer.WriteStartElement("v", "http://schemas.openxmlformats.org/drawingml/2006/chart");
            writer.WriteString(ChartSerializator.ToXmlString(objArray[index]));
            writer.WriteEndElement();
            writer.WriteEndElement();
          }
        }
        writer.WriteEndElement();
      }
    }
    writer.WriteEndElement();
  }

  private void SerializeFormula(XmlWriter writer, string tag, string formula)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (tag == null)
      throw new ArgumentNullException(nameof (tag));
    if (formula == null)
      throw new ArgumentNullException(nameof (formula));
    writer.WriteStartElement(tag, "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteElementString("f", "http://schemas.openxmlformats.org/drawingml/2006/chart", formula);
    writer.WriteEndElement();
  }

  private void SerializeAxes(XmlWriter writer, ChartImpl chart, RelationCollection relations)
  {
    ChartAxisSerializator axisSerializator = new ChartAxisSerializator();
    if (chart.IsCategoryAxisAvail && Array.IndexOf<int>(chart.SerializedAxisIds.ToArray(), (chart.PrimaryCategoryAxis as ChartAxisImpl).AxisId) >= 0)
      axisSerializator.SerializeAxis(writer, (IChartAxis) chart.PrimaryCategoryAxis, relations);
    if (chart.IsValueAxisAvail && Array.IndexOf<int>(chart.SerializedAxisIds.ToArray(), (chart.PrimaryValueAxis as ChartAxisImpl).AxisId) >= 0)
      axisSerializator.SerializeAxis(writer, (IChartAxis) chart.PrimaryValueAxis, relations);
    if (chart.IsSecondaryCategoryAxisAvail && Array.IndexOf<int>(chart.SerializedAxisIds.ToArray(), (chart.SecondaryCategoryAxis as ChartAxisImpl).AxisId) >= 0)
      axisSerializator.SerializeAxis(writer, (IChartAxis) chart.SecondaryCategoryAxis, relations);
    if (chart.IsSecondaryValueAxisAvail && Array.IndexOf<int>(chart.SerializedAxisIds.ToArray(), (chart.SecondaryValueAxis as ChartAxisImpl).AxisId) >= 0)
      axisSerializator.SerializeAxis(writer, (IChartAxis) chart.SecondaryValueAxis, relations);
    if (!chart.IsSeriesAxisAvail)
      return;
    axisSerializator.SerializeAxis(writer, (IChartAxis) chart.PrimarySerieAxis, relations);
  }

  private void SerializePivotAxes(XmlWriter writer, ChartImpl chart, RelationCollection relations)
  {
    ChartAxisSerializator axisSerializator = new ChartAxisSerializator();
    if (chart.IsCategoryAxisAvail)
      axisSerializator.SerializeAxis(writer, (IChartAxis) chart.PrimaryCategoryAxis, relations);
    if (chart.IsValueAxisAvail)
      axisSerializator.SerializeAxis(writer, (IChartAxis) chart.PrimaryValueAxis, relations);
    if (!chart.IsPivotChart3D)
      return;
    axisSerializator.SerializeAxis(writer, (IChartAxis) chart.PrimarySerieAxis, relations);
  }

  private void SerializeMarker(XmlWriter writer, ChartSerieImpl series)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    ChartSerieDataFormatImpl serieFormat = series != null ? (ChartSerieDataFormatImpl) series.SerieFormat : throw new ArgumentNullException(nameof (series));
    this.SerializeMarker(writer, serieFormat);
  }

  private void SerializeMarker(XmlWriter writer, ChartSerieDataFormatImpl serieFormat)
  {
    if (serieFormat.IsMarkerSupported && serieFormat.IsMarker)
    {
      if (serieFormat.IsAutoMarker)
        return;
      writer.WriteStartElement("marker", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      Excel2007ChartMarkerType markerStyle = (Excel2007ChartMarkerType) serieFormat.MarkerStyle;
      ChartSerializatorCommon.SerializeValueTag(writer, "symbol", markerStyle.ToString());
      ChartSerializatorCommon.SerializeValueTag(writer, "size", serieFormat.MarkerSize.ToString());
      if (!serieFormat.MarkerFormat.IsAutoColor)
      {
        writer.WriteStartElement("spPr", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        IWorkbook parentWorkbook = (IWorkbook) serieFormat.ParentChart.ParentWorkbook;
        if ((serieFormat.IsMarkerChanged ? 1 : (!serieFormat.MarkerFormat.IsNotShowInt ? 1 : 0)) != 0)
        {
          if (serieFormat.MarkerGradient != null)
          {
            new GradientSerializator().Serialize(writer, serieFormat.MarkerGradient, parentWorkbook);
          }
          else
          {
            double num = serieFormat.IsSupportFill ? serieFormat.Fill.Transparency : 0.0;
            double alphavalue = serieFormat.MarkerBackgroundColor == Color.Transparent ? 0.0 : 1.0 - num;
            if (!serieFormat.IsAutoMarkerColor)
              ChartSerializatorCommon.SerializeSolidFill(writer, (ColorObject) serieFormat.MarkerBackgroundColor, false, parentWorkbook, alphavalue);
          }
        }
        else if (serieFormat.MarkerFormat.IsNotShowInt)
          writer.WriteElementString("noFill", "http://schemas.openxmlformats.org/drawingml/2006/main", string.Empty);
        if (serieFormat.MarkerLineStream != null)
        {
          serieFormat.MarkerLineStream.Position = 0L;
          ShapeParser.WriteNodeFromStream(writer, serieFormat.MarkerLineStream);
        }
        else if (serieFormat.MarkerFormat.HasLineProperties)
          ChartSerializator.SerializeLineSettings(writer, serieFormat.MarkerForegroundColor, parentWorkbook, serieFormat.MarkerFormat.IsNotShowBrd, serieFormat.MarkerTransparency);
        if (serieFormat.EffectListStream != null && !serieFormat.IsMarkerChanged)
          ShapeParser.WriteNodeFromStream(writer, serieFormat.EffectListStream);
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }
    else
    {
      if (!serieFormat.IsMarkerSupported || !serieFormat.HasMarkerProperties)
        return;
      writer.WriteStartElement("marker", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      ChartSerializatorCommon.SerializeValueTag(writer, "symbol", Excel2007ChartMarkerType.none.ToString());
      writer.WriteEndElement();
    }
  }

  internal static void SerializeLineSettings(XmlWriter writer, Color color, IWorkbook book)
  {
    ChartSerializator.SerializeLineSettings(writer, color, book, false);
  }

  internal static void SerializeLineSettings(
    XmlWriter writer,
    Color color,
    IWorkbook book,
    bool bNoFill)
  {
    ChartSerializator.SerializeLineSettings(writer, color, book, bNoFill, 1.0);
  }

  internal static void SerializeLineSettings(
    XmlWriter writer,
    Color color,
    IWorkbook book,
    bool bNoFill,
    double transparency)
  {
    writer.WriteStartElement("ln", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (!bNoFill)
      ChartSerializatorCommon.SerializeSolidFill(writer, (ColorObject) color, false, book, transparency);
    else
      writer.WriteElementString("noFill", "http://schemas.openxmlformats.org/drawingml/2006/main", string.Empty);
    writer.WriteEndElement();
  }

  private void SerializeUpDownBars(XmlWriter writer, ChartImpl chart, ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    ChartFormatImpl commonSerieOptions = (ChartFormatImpl) firstSeries.SerieFormat.CommonSerieOptions;
    if (!commonSerieOptions.IsDropBar)
      return;
    IChartDropBar firstDropBar = commonSerieOptions.FirstDropBar;
    IChartDropBar secondDropBar = commonSerieOptions.SecondDropBar;
    writer.WriteStartElement("upDownBars", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    ChartSerializatorCommon.SerializeValueTag(writer, "gapWidth", firstDropBar.Gap.ToString());
    this.SerializeDropBar(writer, firstDropBar, "upBars", chart);
    this.SerializeDropBar(writer, secondDropBar, "downBars", chart);
    writer.WriteEndElement();
  }

  private void SerializeDropBar(
    XmlWriter writer,
    IChartDropBar dropBar,
    string tagName,
    ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (dropBar == null)
      throw new ArgumentNullException(nameof (dropBar));
    if (tagName == null || tagName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (tagName));
    writer.WriteStartElement(tagName, "http://schemas.openxmlformats.org/drawingml/2006/chart");
    ChartSerializatorCommon.SerializeFrameFormat(writer, (IChartFillBorder) dropBar, chart, false);
    writer.WriteEndElement();
  }

  internal void SerializeChartsheet(XmlWriter writer, ChartImpl chart, string drawingRelation)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (drawingRelation == null || drawingRelation.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (drawingRelation));
    writer.WriteStartDocument();
    writer.WriteStartElement("chartsheet", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
    writer.WriteAttributeString("xmlns", "r", (string) null, "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
    writer.WriteStartElement("sheetPr");
    string codeName = chart.CodeName;
    if (chart.HasCodeName && codeName != null && codeName.Length > 0)
      writer.WriteAttributeString("codeName", codeName);
    writer.WriteEndElement();
    writer.WriteStartElement("sheetViews");
    writer.WriteStartElement("sheetView");
    Excel2007Serializator.SerializeAttribute(writer, "zoomScale", chart.Zoom, 100);
    writer.WriteAttributeString("workbookViewId", "0");
    Excel2007Serializator.SerializeAttribute(writer, "zoomToFit", chart.ZoomToFit, false);
    writer.WriteEndElement();
    writer.WriteEndElement();
    Excel2007Serializator serializator = chart.ParentWorkbook.DataHolder.Serializator;
    serializator.SerializeSheetProtection(writer, (WorksheetBaseImpl) chart);
    IPageSetupConstantsProvider constants = (IPageSetupConstantsProvider) new WorksheetPageSetupConstants();
    Excel2007Serializator.SerializePageMargins(writer, (IPageSetupBase) chart.PageSetup, constants);
    Excel2007Serializator.SerializePageSetup(writer, (IPageSetupBase) chart.PageSetup, constants);
    Excel2007Serializator.SerializeHeaderFooter(writer, (IPageSetupBase) chart.PageSetupBase, constants);
    writer.WriteStartElement("drawing");
    writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", drawingRelation);
    writer.WriteEndElement();
    serializator.SerializeVmlShapesWorksheetPart(writer, (WorksheetBaseImpl) chart);
    Excel2007Serializator.SerializeVmlHFShapesWorksheetPart(writer, (WorksheetBaseImpl) chart, (IPageSetupConstantsProvider) new ChartPageSetupConstants(false), (RelationCollection) null);
    serializator.SerilizeBackgroundImage(writer, (WorksheetBaseImpl) chart);
    writer.WriteEndElement();
  }

  public void SerializeChartsheetDrawing(XmlWriter writer, ChartImpl chart, string strRelationId)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartDocument(true);
    writer.WriteStartElement("xdr", "wsDr", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    writer.WriteAttributeString("xmlns", "a", (string) null, "http://schemas.openxmlformats.org/drawingml/2006/main");
    ChartSerializator.SerializeAbsoluteAnchorChart(writer, chart, strRelationId, true);
    writer.WriteEndElement();
  }

  internal static void SerializeAbsoluteAnchorChart(
    XmlWriter writer,
    ChartImpl chart,
    string strRelationId,
    bool isForChartSheet)
  {
    writer.WriteStartElement("absoluteAnchor", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    writer.WriteStartElement("pos", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    double num1 = (chart.Workbook as WorkbookImpl).IsConverted ? 0.0 : chart.XPos;
    double num2 = (chart.Workbook as WorkbookImpl).IsConverted ? 0.0 : chart.YPos;
    writer.WriteAttributeString("x", ChartSerializator.ToXmlString((object) num1));
    writer.WriteAttributeString("y", ChartSerializator.ToXmlString((object) num2));
    writer.WriteEndElement();
    writer.WriteStartElement("ext", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    if (chart.Width <= 0.0)
      writer.WriteAttributeString("cx", 8666049.ToString());
    else
      writer.WriteAttributeString("cx", ChartSerializator.ToXmlString((object) chart.EMUWidth));
    if (chart.Height <= 0.0)
      writer.WriteAttributeString("cy", 6293304.ToString());
    else
      writer.WriteAttributeString("cy", ChartSerializator.ToXmlString((object) chart.EMUHeight));
    writer.WriteEndElement();
    bool flag = ChartImpl.IsChartExSerieType(chart.ChartType);
    if (flag && isForChartSheet)
    {
      writer.WriteStartElement("mc", "AlternateContent", "http://schemas.openxmlformats.org/markup-compatibility/2006");
      writer.WriteAttributeString("xmlns", "mc", (string) null, "http://schemas.openxmlformats.org/markup-compatibility/2006");
      writer.WriteStartElement("mc", "Choice", "http://schemas.openxmlformats.org/markup-compatibility/2006");
      writer.WriteAttributeString("xmlns", "cx1", (string) null, "http://schemas.microsoft.com/office/drawing/2015/9/8/chartex");
      writer.WriteAttributeString("Requires", "cx1");
    }
    writer.WriteStartElement("graphicFrame", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    writer.WriteAttributeString("macro", string.Empty);
    writer.WriteStartElement("nvGraphicFramePr", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    writer.WriteStartElement("cNvPr", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    writer.WriteAttributeString("id", "2");
    writer.WriteAttributeString("name", chart.Name);
    writer.WriteEndElement();
    writer.WriteStartElement("cNvGraphicFramePr", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    writer.WriteStartElement("graphicFrameLocks", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("noGrp", "1");
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
    DrawingShapeSerializator.SerializeForm(writer, "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing", "http://schemas.openxmlformats.org/drawingml/2006/main", 0, 0, 0, 0);
    writer.WriteStartElement("graphic", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteStartElement("graphicData", "http://schemas.openxmlformats.org/drawingml/2006/main");
    string[] strArray = strRelationId.Split(';');
    if (flag && isForChartSheet)
    {
      writer.WriteAttributeString("uri", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      writer.WriteStartElement("cx", nameof (chart), "http://schemas.microsoft.com/office/drawing/2014/chartex");
      writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", strArray[0]);
    }
    else
    {
      writer.WriteAttributeString("uri", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      writer.WriteStartElement("c", nameof (chart), "http://schemas.openxmlformats.org/drawingml/2006/chart");
      writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", strArray[0]);
    }
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
    if (flag && isForChartSheet)
    {
      writer.WriteEndElement();
      ChartSerializator.SerializeChartExFallBackContentForChartSheet(writer, strArray[1]);
      writer.WriteEndElement();
    }
    writer.WriteElementString("clientData", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing", string.Empty);
    writer.WriteEndElement();
  }

  private static void SerializeChartExFallBackContentForChartSheet(
    XmlWriter writer,
    string relationId)
  {
    writer.WriteStartElement("mc", "Fallback", "http://schemas.openxmlformats.org/markup-compatibility/2006");
    writer.WriteStartElement("graphicFrame", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    writer.WriteAttributeString("macro", string.Empty);
    writer.WriteStartElement("nvGraphicFramePr", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    writer.WriteStartElement("cNvPr", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    writer.WriteAttributeString("id", "0");
    writer.WriteAttributeString("name", "");
    writer.WriteEndElement();
    writer.WriteStartElement("cNvGraphicFramePr", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    writer.WriteStartElement("graphicFrameLocks", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("noGrp", "1");
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
    DrawingShapeSerializator.SerializeForm(writer, "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing", "http://schemas.openxmlformats.org/drawingml/2006/main", 0, 0, 0, 0);
    writer.WriteStartElement("graphic", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteStartElement("graphicData", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("uri", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteStartElement("c", "chart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", relationId);
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private void SerializeDataTable(XmlWriter writer, ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (!chart.HasDataTable)
      return;
    ChartDataTableImpl dataTable = chart.DataTable as ChartDataTableImpl;
    writer.WriteStartElement("dTable", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    ChartSerializatorCommon.SerializeBoolValueTag(writer, "showHorzBorder", dataTable.HasHorzBorder);
    ChartSerializatorCommon.SerializeBoolValueTag(writer, "showVertBorder", dataTable.HasVertBorder);
    ChartSerializatorCommon.SerializeBoolValueTag(writer, "showOutline", dataTable.HasBorders);
    ChartSerializatorCommon.SerializeBoolValueTag(writer, "showKeys", dataTable.ShowSeriesKeys);
    if (dataTable.HasTextArea && ((IInternalChartTextArea) dataTable.TextArea).ParagraphType == ChartParagraphType.CustomDefault)
    {
      WorkbookImpl parentWorkbook = (dataTable.Parent as ChartImpl).ParentWorkbook;
      this.SerializeDefaultTextFormatting(writer, dataTable.TextArea, (IWorkbook) parentWorkbook, 10.0);
    }
    writer.WriteEndElement();
  }

  private void SerializeDirectlyEntered(
    XmlWriter writer,
    object[] values,
    bool isCache,
    string formatCode)
  {
    this.SerializeDirectlyEntered(writer, values, isCache, (string) null, formatCode);
  }

  private void SerializeDirectlyEntered(
    XmlWriter writer,
    object[] values,
    bool isCache,
    string tagName,
    string formatCode)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (values == null)
      throw new ArgumentNullException(nameof (values));
    string localName;
    if (isCache)
    {
      string str;
      switch (tagName)
      {
        case "cat":
        case "xVal":
          str = "strCache";
          break;
        default:
          if (!(values[0] is string) || !(values[0] as string != "#N/A"))
          {
            str = "numCache";
            break;
          }
          goto case "cat";
      }
      localName = str;
    }
    else
    {
      string str;
      switch (tagName)
      {
        case "cat":
        case "xVal":
          str = "strLit";
          break;
        default:
          if (!(values[0] is string) || !(values[0] as string != "#N/A"))
          {
            str = "numLit";
            break;
          }
          goto case "cat";
      }
      localName = str;
    }
    writer.WriteStartElement(localName, "http://schemas.openxmlformats.org/drawingml/2006/chart");
    if (localName.StartsWith("num") && (tagName == "xVal" || tagName == "cat" || tagName == "yVal" || tagName == "val") && formatCode != null && formatCode != string.Empty)
      writer.WriteElementString(nameof (formatCode), "http://schemas.openxmlformats.org/drawingml/2006/chart", formatCode);
    int length = values.Length;
    ChartSerializatorCommon.SerializeValueTag(writer, "ptCount", length.ToString());
    for (int index = 0; index < length; ++index)
    {
      if (values[index] != null)
      {
        writer.WriteStartElement("pt", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        writer.WriteAttributeString("idx", index.ToString());
        writer.WriteStartElement("v", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        writer.WriteString(ChartSerializator.ToXmlString(values[index]));
        writer.WriteEndElement();
        writer.WriteEndElement();
      }
    }
    writer.WriteEndElement();
  }

  internal static string ToXmlString(object value)
  {
    string xmlString;
    switch (value)
    {
      case double num1:
        xmlString = XmlConvert.ToString(num1);
        break;
      case float num2:
        xmlString = XmlConvert.ToString(num2);
        break;
      case Decimal num3:
        xmlString = XmlConvert.ToString(num3);
        break;
      default:
        xmlString = value.ToString();
        break;
    }
    return xmlString;
  }

  private void SerializeDataLabels(
    XmlWriter writer,
    ChartImpl parentChart,
    ChartDataPointsCollection chartDataPointsCollection)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartElement("dLbls", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    ChartDataPointsCollection pointsCollection = chartDataPointsCollection;
    if (pointsCollection.DeninedDPCount > 0)
    {
      foreach (ChartDataPointImpl chartDataPointImpl in pointsCollection)
      {
        if (!chartDataPointImpl.IsDefault && chartDataPointImpl.HasDataLabels)
          this.SerializeDataLabel(writer, chartDataPointImpl.DataLabels, chartDataPointImpl.Index, parentChart);
      }
    }
    ChartDataLabelsImpl dataLabels = pointsCollection.DefaultDataPoint.DataLabels as ChartDataLabelsImpl;
    if (dataLabels.NumberFormat != null)
      this.SerializeNumFormat(writer, dataLabels);
    this.SerializeDataLabelSettings(writer, pointsCollection.DefaultDataPoint.DataLabels, parentChart, true);
    writer.WriteEndElement();
  }

  private delegate void SerializeSeriesDelegate(XmlWriter writer, ChartSerieImpl series);
}
