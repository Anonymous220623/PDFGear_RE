// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.XmlSerialization.Charts.ChartSerializator
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.Drawing;
using Syncfusion.OfficeChart.Implementation.Charts;
using Syncfusion.OfficeChart.Implementation.XmlReaders.Shapes;
using Syncfusion.OfficeChart.Implementation.XmlSerialization.Constants;
using Syncfusion.OfficeChart.Implementation.XmlSerialization.Shapes;
using Syncfusion.OfficeChart.Interfaces.Charts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Security;
using System.Xml;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.XmlSerialization.Charts;

internal class ChartSerializator
{
  public const int DefaultExtentX = 8666049;
  public const int DefaultExtentY = 6293304;
  public int categoryFilter;
  public bool findFilter;
  private bool m_isChartExFallBack;
  private double _appVersion = -1.0;

  internal ChartSerializator(bool value) => this.m_isChartExFallBack = value;

  public ChartSerializator()
  {
  }

  [SecurityCritical]
  internal void SerializeChart(
    XmlWriter writer,
    ChartImpl chart,
    string chartItemName,
    double appVersion)
  {
    this._appVersion = appVersion;
    this.SerializeChart(writer, chart, chartItemName);
  }

  [SecurityCritical]
  internal void SerializeChart(XmlWriter writer, ChartImpl chart, string chartItemName)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("c", "chartSpace", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteAttributeString("xmlns", "a", (string) null, "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("xmlns", "r", (string) null, "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
    ChartSerializatorCommon.SerializeBoolValueTag(writer, "date1904", chart.Workbook.Date1904);
    if (chart.HasChartArea && chart.ChartArea.IsBorderCornersRound)
      ChartSerializatorCommon.SerializeBoolValueTag(writer, "roundedCorners", true);
    else
      ChartSerializatorCommon.SerializeBoolValueTag(writer, "roundedCorners", false);
    if (chart.AlternateContent != null)
    {
      chart.AlternateContent.Position = 0L;
      ShapeParser.WriteNodeFromStream(writer, chart.AlternateContent);
    }
    else if ((this._appVersion != -1.0 && this._appVersion <= 12.0 || chart.Style >= 101 && chart.Style <= 148) && chart.AlternateContent == null)
    {
      writer.WriteStartElement("mc", "AlternateContent", "http://schemas.openxmlformats.org/markup-compatibility/2006");
      writer.WriteStartElement("mc", "Choice", (string) null);
      writer.WriteAttributeString("Requires", "c14");
      writer.WriteAttributeString("xmlns", "c14", (string) null, "http://schemas.microsoft.com/office/drawing/2007/8/2/chart");
      writer.WriteStartElement("c14", "style", (string) null);
      writer.WriteAttributeString("val", chart.Style < 101 || chart.Style > 148 ? "102" : chart.Style.ToString());
      writer.WriteEndElement();
      writer.WriteEndElement();
      writer.WriteStartElement("mc", "Fallback", (string) null);
      ChartSerializatorCommon.SerializeValueTag(writer, "style", chart.Style < 101 || chart.Style > 148 ? "2" : (chart.Style - 100).ToString());
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
    if (chart.Style > 0 && chart.Style <= 48 /*0x30*/)
      ChartSerializatorCommon.SerializeValueTag(writer, "style", chart.Style.ToString());
    if (chart.InnerProtection != OfficeSheetProtection.None)
      writer.WriteElementString("protection", "http://schemas.openxmlformats.org/drawingml/2006/chart", string.Empty);
    writer.WriteStartElement(nameof (chart), "http://schemas.openxmlformats.org/drawingml/2006/chart");
    FileDataHolder parentHolder = chart.DataHolder.ParentHolder;
    RelationCollection relations = chart.Relations;
    bool flag = false;
    if (chart.HasAutoTitle.HasValue)
      flag = chart.HasAutoTitle.Value;
    if (flag && chart.ChartTitle != null && chart.ChartTitle != string.Empty)
      flag = false;
    if (chart.HasTitle && chart.HasTitleInternal && !flag)
      ChartSerializatorCommon.SerializeTextArea(writer, chart.ChartTitleArea, chart.ParentWorkbook, relations, 18.0);
    if (chart.HasAutoTitle.HasValue)
    {
      writer.WriteStartElement("autoTitleDeleted", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      writer.WriteAttributeString("val", flag ? "1" : "0");
      writer.WriteEndElement();
    }
    else if (!chart.Loading && chart.Series.Count == 1)
    {
      writer.WriteStartElement("autoTitleDeleted", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      writer.WriteAttributeString("val", "0");
      writer.WriteEndElement();
    }
    this.SerializePivotFormats(writer, chart);
    if (chart.Series.Count > 0)
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
    if (chart.HasLegend)
      this.SerializeLegend(writer, chart.Legend, chart);
    if (chart.PlotVisibleOnly || (chart.Workbook as WorkbookImpl).IsCreated)
      ChartSerializatorCommon.SerializeBoolValueTag(writer, "plotVisOnly", chart.PlotVisibleOnly);
    Excel2007ChartPlotEmpty displayBlanksAs = (Excel2007ChartPlotEmpty) chart.DisplayBlanksAs;
    ChartSerializatorCommon.SerializeValueTag(writer, "dispBlanksAs", displayBlanksAs.ToString());
    writer.WriteEndElement();
    if (chart.HasChartArea)
    {
      IOfficeChartFrameFormat chartArea = chart.ChartArea;
      if (chartArea != null)
        ChartSerializatorCommon.SerializeFrameFormat(writer, (IOfficeChartFillBorder) chartArea, chart, chartArea.IsBorderCornersRound);
    }
    this.SerializeDefaultTextProperties(writer, chart);
    string relationId = chart.Relations.GenerateRelationId();
    chart.Relations[relationId] = new Relation("", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/package");
    writer.WriteStartElement("c", "externalData", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", relationId);
    writer.WriteStartElement("c", "autoUpdate", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteAttributeString("val", "0");
    writer.WriteEndElement();
    writer.WriteEndElement();
    this.SerializeShapes(writer, chart, chartItemName);
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

  [SecurityCritical]
  private void SerializeShapes(XmlWriter writer, ChartImpl chart, string chartItemName)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (!this.m_isChartExFallBack && chart.Shapes.Count - chart.VmlShapesCount <= 0)
    {
      if (!chart.RelationPreservedStreamCollection.ContainsKey("http://schemas.openxmlformats.org/officeDocument/2006/relationships/chartUserShapes"))
        return;
      string relationId = chart.Relations.GenerateRelationId();
      chart.Relations[relationId] = new Relation("", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/chartUserShapes");
      writer.WriteStartElement("userShapes", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", relationId);
      writer.WriteEndElement();
    }
    else
    {
      WorksheetDataHolder dataHolder = chart.DataHolder;
      RelationCollection relations = chart.Relations;
      string id = dataHolder.DrawingsId;
      if (id == null)
      {
        dataHolder.DrawingsId = id = relations.GenerateRelationId();
        relations[id] = (Relation) null;
      }
      if (chart.DataHolder.SerializeDrawings((WorksheetBaseImpl) chart, relations, ref id, "application/vnd.openxmlformats-officedocument.drawingml.chartshapes+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/chartUserShapes"))
      {
        writer.WriteStartElement("userShapes", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", id);
        writer.WriteEndElement();
      }
      else if (this.m_isChartExFallBack)
      {
        chart.DataHolder.SerializeChartExFallbackShape((WorksheetBaseImpl) chart, relations, ref id, chartItemName, "application/vnd.openxmlformats-officedocument.drawingml.chartshapes+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/chartUserShapes");
        writer.WriteStartElement("userShapes", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", id);
        writer.WriteEndElement();
      }
      else
        relations.Remove(id);
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
    writer.WriteStartElement("printSettings", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    IPageSetupConstantsProvider constants = (IPageSetupConstantsProvider) new ChartPageSetupConstants();
    Excel2007Serializator.SerializePrintSettings(writer, (IPageSetupBase) pageSetup, constants, true);
    Excel2007Serializator.SerializeVmlHFShapesWorksheetPart(writer, (WorksheetBaseImpl) chart, constants, relations);
    chart.DataHolder.SerializeHeaderFooterImages((WorksheetBaseImpl) chart, relations);
    writer.WriteEndElement();
  }

  [SecurityCritical]
  private void SerializeLegend(XmlWriter writer, IOfficeChartLegend legend, ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (legend == null)
      throw new ArgumentNullException(nameof (legend));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement(nameof (legend), "http://schemas.openxmlformats.org/drawingml/2006/chart");
    OfficeLegendPosition position = legend.Position;
    if (position != OfficeLegendPosition.NotDocked)
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
    ChartSerializatorCommon.SerializeFrameFormat(writer, (IOfficeChartFillBorder) legend.FrameFormat, chart, false);
    (legend as ChartLegendImpl).IsChartTextArea = true;
    if (((IInternalOfficeChartTextArea) legend.TextArea).ParagraphType == ChartParagraphType.CustomDefault)
      this.SerializeDefaultTextFormatting(writer, legend.TextArea, workbook, 10.0);
    writer.WriteEndElement();
  }

  private void SerializeLegendEntry(
    XmlWriter writer,
    IOfficeChartLegendEntry legendEntry,
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
    string str1 = chart.PivotChartType == OfficeChartType.Surface_3D || chart.PivotChartType == OfficeChartType.Surface_NoColor_3D ? "15" : "90";
    string str2 = chart.PivotChartType == OfficeChartType.Surface_3D || chart.PivotChartType == OfficeChartType.Surface_NoColor_3D ? "20" : "0";
    string str3 = chart.PivotChartType == OfficeChartType.Surface_3D || chart.PivotChartType == OfficeChartType.Surface_NoColor_3D ? "30" : "0";
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
    if (!chartFormat.IsDefaultElevation)
      ChartSerializatorCommon.SerializeValueTag(writer, "rotX", chart.Elevation.ToString());
    if (!chart.AutoScaling)
      ChartSerializatorCommon.SerializeValueTag(writer, "hPercent", chart.HeightPercent.ToString());
    if (!chartFormat.IsDefaultRotation)
      ChartSerializatorCommon.SerializeValueTag(writer, "rotY", chart.Rotation.ToString());
    ChartSerializatorCommon.SerializeValueTag(writer, "depthPercent", chart.DepthPercent.ToString());
    ChartSerializatorCommon.SerializeBoolValueTag(writer, "rAngAx", chart.RightAngleAxes);
    ChartSerializatorCommon.SerializeValueTag(writer, "perspective", (chart.Perspective * 2).ToString());
    writer.WriteEndElement();
  }

  private void SerializeErrorBars(
    XmlWriter writer,
    IOfficeChartErrorBars errorBars,
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
    OfficeErrorBarInclude include = errorBars.Include;
    string lower = include.ToString().ToLower();
    ChartSerializatorCommon.SerializeValueTag(writer, "errBarType", lower);
    Excel2007ErrorBarType type = (Excel2007ErrorBarType) errorBars.Type;
    ChartSerializatorCommon.SerializeValueTag(writer, "errValType", type.ToString());
    ChartErrorBarsImpl chartErrorBarsImpl = errorBars as ChartErrorBarsImpl;
    ChartSerializatorCommon.SerializeBoolValueTag(writer, "noEndCap", !errorBars.HasCap);
    if (type == Excel2007ErrorBarType.cust)
    {
      if ((include == OfficeErrorBarInclude.Plus || include == OfficeErrorBarInclude.Both) && (errorBars.PlusRange != null || chartErrorBarsImpl.PlusRangeValues != null))
      {
        writer.WriteStartElement("plus", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        if (!chartErrorBarsImpl.IsPlusNumberLiteral)
          this.SerializeNumReference(writer, (errorBars.PlusRange as ChartDataRange).Range, chartErrorBarsImpl.PlusRangeValues, series, "plus");
        else
          this.SerializeDirectlyEntered(writer, chartErrorBarsImpl.PlusRangeValues, false);
        writer.WriteEndElement();
      }
      if ((include == OfficeErrorBarInclude.Minus || include == OfficeErrorBarInclude.Both) && (errorBars.MinusRange != null || chartErrorBarsImpl.MinusRangeValues != null))
      {
        writer.WriteStartElement("minus", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        if (!chartErrorBarsImpl.IsMinusNumberLiteral)
          this.SerializeNumReference(writer, (errorBars.MinusRange as ChartDataRange).Range, chartErrorBarsImpl.MinusRangeValues, series, "minus");
        else
          this.SerializeDirectlyEntered(writer, chartErrorBarsImpl.MinusRangeValues, false);
        writer.WriteEndElement();
      }
    }
    ChartSerializatorCommon.SerializeValueTag(writer, "val", XmlConvert.ToString(errorBars.NumberValue));
    IOfficeChartBorder border = errorBars.Border;
    if (border != null && !border.AutoFormat)
    {
      writer.WriteStartElement("spPr", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      ChartSerializatorCommon.SerializeLineProperties(writer, border, book);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private void SerializeTrendlines(
    XmlWriter writer,
    IOfficeChartTrendLines trendlines,
    IWorkbook book)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (trendlines == null)
      return;
    int iIndex = 0;
    for (int count = trendlines.Count; iIndex < count; ++iIndex)
      this.SerializeTrendline(writer, trendlines[iIndex], book);
  }

  private void SerializeTrendline(
    XmlWriter writer,
    IOfficeChartTrendLine trendline,
    IWorkbook book)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (trendline == null)
      throw new ArgumentNullException(nameof (trendline));
    writer.WriteStartElement(nameof (trendline), "http://schemas.openxmlformats.org/drawingml/2006/chart");
    string name = trendline.Name;
    if (name != null && !trendline.NameIsAuto)
      writer.WriteElementString("name", "http://schemas.openxmlformats.org/drawingml/2006/chart", name);
    IOfficeChartBorder border = trendline.Border;
    if (border != null && !border.AutoFormat)
    {
      writer.WriteStartElement("spPr", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      ChartSerializatorCommon.SerializeLineProperties(writer, trendline.Border, book);
      writer.WriteEndElement();
    }
    Excel2007TrendlineType type = (Excel2007TrendlineType) trendline.Type;
    ChartSerializatorCommon.SerializeValueTag(writer, "trendlineType", type.ToString());
    string tagName = (string) null;
    if (trendline.Type == OfficeTrendLineType.Polynomial)
      tagName = "order";
    else if (trendline.Type == OfficeTrendLineType.Moving_Average)
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
      this.SerializeTrendlineLabel(writer, trendline.DataLabel);
    writer.WriteEndElement();
  }

  private void SerializeTrendlineLabel(XmlWriter writer, IOfficeChartTextArea dataLabelFormat)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (dataLabelFormat == null)
      throw new ArgumentNullException(nameof (dataLabelFormat));
    writer.WriteStartElement("trendlineLbl", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteElementString("layout", "http://schemas.openxmlformats.org/drawingml/2006/chart", string.Empty);
    writer.WriteStartElement("numFmt", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteAttributeString("formatCode", "General");
    writer.WriteAttributeString("sourceLinked", "0");
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  [SecurityCritical]
  private void SerializeSurface(
    XmlWriter writer,
    IOfficeChartWallOrFloor surface,
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
      ChartSerializatorCommon.SerializeFrameFormat(writer, (IOfficeChartFillBorder) surface, chart, false);
    if (chartWallOrFloorImpl.PictureUnit == OfficeChartPictureType.stack)
    {
      writer.WriteStartElement("pictureOptions", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteStartElement("pictureFormat", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteAttributeString("val", chartWallOrFloorImpl.PictureUnit.ToString());
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  [SecurityCritical]
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
    if (num == 0)
    {
      if (count == 0 && chart.IsChartCleared && !chart.IsChartBar)
      {
        writer.WriteStartElement(Helper.GetOfficeChartType(chart.ChartType), "http://schemas.openxmlformats.org/drawingml/2006/chart");
      }
      else
      {
        writer.WriteStartElement("barChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        ChartSerializatorCommon.SerializeValueTag(writer, "barDir", "col");
        ChartSerializatorCommon.SerializeValueTag(writer, "grouping", "clustered");
      }
      ChartAxisImpl primaryCategoryAxis = (ChartAxisImpl) chart.PrimaryCategoryAxis;
      ChartSerializatorCommon.SerializeValueTag(writer, "axId", primaryCategoryAxis.AxisId.ToString());
      chart.SerializedAxisIds.Add(primaryCategoryAxis.AxisId);
      ChartAxisImpl primaryValueAxis = (ChartAxisImpl) chart.PrimaryValueAxis;
      ChartSerializatorCommon.SerializeValueTag(writer, "axId", primaryValueAxis.AxisId.ToString());
      chart.SerializedAxisIds.Add(primaryValueAxis.AxisId);
      writer.WriteEndElement();
    }
    this.SerializeAxes(writer, chart, relations);
    this.SerializeDataTable(writer, chart);
    if (chart.HasPlotArea)
    {
      IOfficeChartFrameFormat plotArea = chart.PlotArea;
      ChartSerializatorCommon.SerializeFrameFormat(writer, (IOfficeChartFillBorder) plotArea, chart, plotArea.IsBorderCornersRound);
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

  [SecurityCritical]
  private int SerializeBarChart(XmlWriter writer, ChartImpl chart, ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("barChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    int num1 = this.SerializeBarChartShared(writer, chart, firstSeries);
    IOfficeChartFormat commonSerieOptions = firstSeries.SerieFormat.CommonSerieOptions;
    int num2 = 0;
    int num3 = 0;
    bool flag1 = false;
    bool flag2 = false;
    if ((chart.Workbook as WorkbookImpl).IsCreated)
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
    if (commonSerieOptions.HasSeriesLines && (chart.ChartType == OfficeChartType.Bar_Stacked || chart.ChartType == OfficeChartType.Bar_Stacked_100))
    {
      IOfficeChartBorder pieSeriesLine = commonSerieOptions.PieSeriesLine;
      writer.WriteStartElement("serLines", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      writer.WriteStartElement("spPr", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      ChartSerializatorCommon.SerializeLineProperties(writer, pieSeriesLine, chart.Workbook);
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
    if (chart.IsPrimarySecondaryCategory && chart.IsClustered)
      firstSeries.UsePrimaryAxis = true;
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
    ChartSerializatorCommon.SerializeValueTag(writer, "axId", primarySerieAxis.AxisId.ToString());
  }

  private void SerializeBar3DChart(XmlWriter writer, ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("bar3DChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    string str1 = chart.PivotChartType.ToString();
    string str2 = str1.Contains("Bar") ? "bar" : "col";
    ChartSerializatorCommon.SerializeValueTag(writer, "barDir", str2);
    this.SerializeChartGrouping(writer, chart);
    if (!str1.Contains("Cone") && !str1.Contains("Cylinder") && !str1.Contains("Pyramid"))
      return;
    OfficeBaseFormat baseFormat;
    OfficeTopFormat topFormat;
    switch (chart.PivotChartType)
    {
      case OfficeChartType.Cylinder_Clustered:
      case OfficeChartType.Cylinder_Stacked:
      case OfficeChartType.Cylinder_Stacked_100:
      case OfficeChartType.Cylinder_Bar_Clustered:
      case OfficeChartType.Cylinder_Bar_Stacked:
      case OfficeChartType.Cylinder_Bar_Stacked_100:
      case OfficeChartType.Cylinder_Clustered_3D:
        baseFormat = OfficeBaseFormat.Circle;
        topFormat = OfficeTopFormat.Straight;
        break;
      case OfficeChartType.Cone_Clustered:
      case OfficeChartType.Cone_Stacked:
      case OfficeChartType.Cone_Stacked_100:
      case OfficeChartType.Cone_Bar_Clustered:
      case OfficeChartType.Cone_Bar_Stacked:
      case OfficeChartType.Cone_Bar_Stacked_100:
      case OfficeChartType.Cone_Clustered_3D:
        baseFormat = OfficeBaseFormat.Circle;
        topFormat = OfficeTopFormat.Sharp;
        break;
      case OfficeChartType.Pyramid_Clustered:
      case OfficeChartType.Pyramid_Stacked:
      case OfficeChartType.Pyramid_Stacked_100:
      case OfficeChartType.Pyramid_Bar_Clustered:
      case OfficeChartType.Pyramid_Bar_Stacked:
      case OfficeChartType.Pyramid_Bar_Stacked_100:
      case OfficeChartType.Pyramid_Clustered_3D:
        baseFormat = OfficeBaseFormat.Rectangle;
        topFormat = OfficeTopFormat.Sharp;
        break;
      default:
        throw new ArgumentException("type");
    }
    this.SerializeBarShape(writer, baseFormat, topFormat);
  }

  [SecurityCritical]
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
    this.SerializeBarShape(writer, chart, firstSeries, false);
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
    OfficeBaseFormat baseFormat,
    OfficeTopFormat topFormat)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    string str = (string) null;
    switch (topFormat)
    {
      case OfficeTopFormat.Straight:
        str = baseFormat == OfficeBaseFormat.Circle ? "cylinder" : "box";
        break;
      case OfficeTopFormat.Sharp:
        str = baseFormat == OfficeBaseFormat.Circle ? "cone" : "pyramid";
        break;
      case OfficeTopFormat.Trunc:
        str = baseFormat == OfficeBaseFormat.Circle ? "coneToMax" : "pyramidToMax";
        break;
    }
    ChartSerializatorCommon.SerializeValueTag(writer, "shape", str);
  }

  private void SerializeBarShape(
    XmlWriter writer,
    ChartImpl chart,
    ChartSerieImpl firstSeries,
    bool isSerieCalled)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    IOfficeChartSerieDataFormat chartSerieDataFormat = !isSerieCalled ? chart.ChartFormat.SerieDataFormat : firstSeries.SerieFormat;
    OfficeTopFormat barShapeTop = chartSerieDataFormat.BarShapeTop;
    OfficeBaseFormat barShapeBase = chartSerieDataFormat.BarShapeBase;
    string str = (string) null;
    switch (barShapeTop)
    {
      case OfficeTopFormat.Straight:
        str = barShapeBase == OfficeBaseFormat.Circle ? "cylinder" : "box";
        break;
      case OfficeTopFormat.Sharp:
        str = barShapeBase == OfficeBaseFormat.Circle ? "cone" : "pyramid";
        break;
      case OfficeTopFormat.Trunc:
        str = barShapeBase == OfficeBaseFormat.Circle ? "coneToMax" : "pyramidToMax";
        break;
    }
    ChartSerializatorCommon.SerializeValueTag(writer, "shape", str);
  }

  [SecurityCritical]
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
    this.SerializeChartGrouping(writer, firstSeries);
    this.SerializeVaryColors(writer, firstSeries);
    int num = this.SerializeChartSeries(writer, chart, firstSeries, new ChartSerializator.SerializeSeriesDelegate(this.SerializeBarSeries));
    if (chart.CommonDataPointsCollection != null && chart.CommonDataPointsCollection.ContainsKey(firstSeries.ChartGroup))
      this.SerializeDataLabels(writer, chart, chart.CommonDataPointsCollection[firstSeries.ChartGroup]);
    return num;
  }

  [SecurityCritical]
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
    IList<IOfficeChartSerie> additionOrder = (IList<IOfficeChartSerie>) (chart.Series as ChartSeriesCollection).AdditionOrder;
    IOfficeChartSeries series1 = chart.Series;
    IList<IOfficeChartSerie> arrOrderedSeries = additionOrder.Count == chart.Series.Count ? additionOrder : (IList<IOfficeChartSerie>) chart.Series;
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

  [SecurityCritical]
  private void SerializeFilteredSeries(XmlWriter writer, ChartSerieImpl series)
  {
    string seriesType = this.GetSeriesType(series.StartType);
    writer.WriteStartElement("c15", seriesType, "http://schemas.microsoft.com/office/drawing/2012/chart");
    writer.WriteStartElement("c15", "ser", "http://schemas.microsoft.com/office/drawing/2012/chart");
    this.SerializeFilterSeries(writer, series);
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  [SecurityCritical]
  private void SerializeFilterSeries(XmlWriter writer, ChartSerieImpl series)
  {
    writer.WriteStartElement("c", "idx", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteAttributeString("val", series.Number.ToString());
    writer.WriteEndElement();
    writer.WriteStartElement("c", "order", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteAttributeString("val", series.Index.ToString());
    writer.WriteEndElement();
    if (series.ParentChart.SeriesNameLevel == OfficeSeriesNameLevel.SeriesNameLevelAll)
      this.SerializeFilteredText(writer, series);
    this.SerializeSeriesCommonWithoutEnd(writer, series, true);
    int percent = series.SerieFormat.Percent;
    if (percent != 0)
      ChartSerializatorCommon.SerializeValueTag(writer, "explosion", percent.ToString());
    if ((series.DataPoints.DefaultDataPoint as ChartDataPointImpl).HasDataLabels || (series.DataPoints as ChartDataPointsCollection).CheckDPDataLabels())
      this.SerializeDataLabels(writer, series.DataPoints.DefaultDataPoint.DataLabels, series);
    this.SerializeTrendlines(writer, series.TrendLines, (IWorkbook) series.ParentBook);
    this.SerializeErrorBars(writer, series);
    if (series.ParentChart.CategoryLabelLevel == OfficeCategoriesLabelLevel.CategoriesLabelLevelAll)
      this.SerializeFilteredCategory(writer, series, this.FindFiltered(series));
    this.SerializeFilteredValues(writer, series, this.FindFiltered(series));
    if (series.SerieType == OfficeChartType.Bubble || series.SerieType == OfficeChartType.Bubble_3D)
      this.SerializeSeriesValues(writer, series.BubblesIRange, series.EnteredDirectlyBubbles, "bubbleSize", series);
    if (series.ParentChart.CategoryLabelLevel == OfficeCategoriesLabelLevel.CategoriesLabelLevelAll && series.ParentChart.SeriesNameLevel == OfficeSeriesNameLevel.SeriesNameLevelAll)
      return;
    writer.WriteStartElement("extLst", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    if (series.ParentChart.SeriesNameLevel != OfficeSeriesNameLevel.SeriesNameLevelAll)
      this.SerializeFilteredSeriesOrCategoryName(writer, series, true);
    if (series.CategoryLabelsIRange != null && series.ParentChart.CategoryLabelLevel != OfficeCategoriesLabelLevel.CategoriesLabelLevelAll)
      this.SerializeFilteredSeriesOrCategoryName(writer, series, false);
    writer.WriteEndElement();
  }

  private void SerializeFilteredText(XmlWriter writer, ChartSerieImpl series)
  {
    if (series.IsDefaultName)
      return;
    if (series.ParentChart.SeriesNameLevel != OfficeSeriesNameLevel.SeriesNameLevelAll)
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
    if (series.CategoryLabelsIRange == null)
      return;
    if (series.SerieType == OfficeChartType.Bubble || series.SerieType == OfficeChartType.Bubble_3D || series.StartType.Contains("Scatter"))
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
      if (series.CategoryLabelsIRange != null)
        str = series.CategoryLabelsIRange.AddressGlobal;
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
    if (series.SerieType == OfficeChartType.Bubble || series.SerieType == OfficeChartType.Bubble_3D || series.StartType.Contains("Scatter"))
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
      if (series.ValuesIRange != null)
        str = series.ValuesIRange.AddressGlobal;
      writer.WriteElementString("sqref", "http://schemas.microsoft.com/office/drawing/2012/chart", str);
      writer.WriteEndElement();
    }
    else
      this.SerializeFilteredFullReference(writer, series, true);
    writer.WriteEndElement();
    writer.WriteEndElement();
    if (!series.IsFiltered && categoryfilter)
      writer.WriteElementString("f", "http://schemas.openxmlformats.org/drawingml/2006/chart", series.FilteredValue);
    writer.WriteElementString("numCache", "http://schemas.openxmlformats.org/drawingml/2006/chart", string.Empty);
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
    IList<IOfficeChartSerie> arrOrderedSeries,
    IOfficeChartSeries arrSeries)
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
      if (series.ValuesIRange != null)
        str = series.ValuesIRange.AddressGlobal;
    }
    else
      str = series.CategoryLabelsIRange.AddressGlobal;
    writer.WriteElementString("sqref", "http://schemas.microsoft.com/office/drawing/2012/chart", str);
    writer.WriteEndElement();
  }

  private void SerializeChartGrouping(XmlWriter writer, ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    OfficeChartType chartType = firstSeries != null ? firstSeries.SerieType : throw new ArgumentNullException(nameof (firstSeries));
    string str = !ChartImpl.GetIsClustered(chartType) ? (!ChartImpl.GetIs100(chartType) ? (!ChartImpl.GetIsStacked(chartType) ? "standard" : "stacked") : "percentStacked") : "clustered";
    ChartSerializatorCommon.SerializeValueTag(writer, "grouping", str);
  }

  private void SerializeChartGrouping(XmlWriter writer, ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    OfficeChartType chartType = chart != null ? chart.PivotChartType : throw new ArgumentNullException("firstSeries");
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

  [SecurityCritical]
  private int SerializeArea3DChart(XmlWriter writer, ChartImpl chart, ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("area3DChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    int num = this.SerializeAreaChartCommon(writer, chart, firstSeries);
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

  [SecurityCritical]
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

  [SecurityCritical]
  private int SerializeAreaChartCommon(
    XmlWriter writer,
    ChartImpl chart,
    ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    ChartFormatImpl commonSerieOptions = (ChartFormatImpl) firstSeries.SerieFormat.CommonSerieOptions;
    this.SerializeChartGrouping(writer, firstSeries);
    this.SerializeVaryColors(writer, firstSeries);
    int num = this.SerializeChartSeries(writer, chart, firstSeries, new ChartSerializator.SerializeSeriesDelegate(this.SerializeAreaSeries));
    if (chart.CommonDataPointsCollection != null && chart.CommonDataPointsCollection.ContainsKey(firstSeries.ChartGroup))
      this.SerializeDataLabels(writer, chart, chart.CommonDataPointsCollection[firstSeries.ChartGroup]);
    if (commonSerieOptions.HasDropLines)
    {
      IOfficeChartBorder dropLines = commonSerieOptions.DropLines;
      writer.WriteStartElement("dropLines", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      writer.WriteStartElement("spPr", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      ChartSerializatorCommon.SerializeLineProperties(writer, dropLines, chart.Workbook);
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
    return num;
  }

  [SecurityCritical]
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
      this.SerializeChartGrouping(writer, firstSeries);
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

  [SecurityCritical]
  private int SerializeLine3DChart(XmlWriter writer, ChartImpl chart, ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("line3DChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    int num = this.SerializeLineChartCommon(writer, chart, firstSeries);
    IOfficeChartFormat commonSerieOptions = firstSeries.SerieFormat.CommonSerieOptions;
    if (commonSerieOptions.HasDropLines)
    {
      IOfficeChartBorder dropLines = commonSerieOptions.DropLines;
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

  [SecurityCritical]
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
    if (firstSeries != null && firstSeries.DropLinesStream != null)
    {
      firstSeries.DropLinesStream.Position = 0L;
      ShapeParser.WriteNodeFromStream(writer, firstSeries.DropLinesStream, true);
    }
    if (serieFormat.IsMarker)
      ChartSerializatorCommon.SerializeValueTag(writer, "marker", "1");
    this.SerializeBarAxisId(writer, chart, firstSeries);
    writer.WriteEndElement();
    return num;
  }

  [SecurityCritical]
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
    IOfficeChartFormat commonSerieOptions = firstSeries.SerieFormat.CommonSerieOptions;
    int bubbleScale = commonSerieOptions.BubbleScale;
    if (bubbleScale != 100)
      ChartSerializatorCommon.SerializeValueTag(writer, "bubbleScale", bubbleScale.ToString());
    if (commonSerieOptions.ShowNegativeBubbles)
      ChartSerializatorCommon.SerializeValueTag(writer, "showNegBubbles", "1");
    string str = commonSerieOptions.SizeRepresents == ChartBubbleSize.Area ? "area" : "w";
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
    OfficeChartType pivotChartType = chart.PivotChartType;
    if (pivotChartType != OfficeChartType.Surface_NoColor_3D && pivotChartType != OfficeChartType.Surface_NoColor_Contour)
      return;
    ChartSerializatorCommon.SerializeValueTag(writer, "wireframe", "1");
  }

  [SecurityCritical]
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
    OfficeChartType pivotChartType = chart.PivotChartType;
    if (pivotChartType != OfficeChartType.Surface_NoColor_3D && pivotChartType != OfficeChartType.Surface_NoColor_Contour)
      return;
    ChartSerializatorCommon.SerializeValueTag(writer, "wireframe", "1");
  }

  [SecurityCritical]
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

  [SecurityCritical]
  private int SerializeSurfaceCommon(XmlWriter writer, ChartImpl chart, ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    OfficeChartType serieType = firstSeries.SerieType;
    if (serieType == OfficeChartType.Surface_NoColor_3D || serieType == OfficeChartType.Surface_NoColor_Contour)
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

  [SecurityCritical]
  private int SerializeMainChartTypeTag(XmlWriter writer, ChartImpl chart, int groupIndex)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    IOfficeChartSeries officeChartSeries = chart != null ? chart.Series : throw new ArgumentNullException(nameof (chart));
    ChartSerieImpl firstSeries = (ChartSerieImpl) null;
    int index = 0;
    for (int count = officeChartSeries.Count; index < count; ++index)
    {
      ChartSerieImpl chartSerieImpl = (ChartSerieImpl) officeChartSeries[index];
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
        case OfficeChartType.Column_Clustered:
        case OfficeChartType.Column_Stacked:
        case OfficeChartType.Column_Stacked_100:
        case OfficeChartType.Bar_Clustered:
        case OfficeChartType.Bar_Stacked:
        case OfficeChartType.Bar_Stacked_100:
          num = this.SerializeBarChart(writer, chart, firstSeries);
          break;
        case OfficeChartType.Column_Clustered_3D:
        case OfficeChartType.Column_Stacked_3D:
        case OfficeChartType.Column_Stacked_100_3D:
        case OfficeChartType.Column_3D:
        case OfficeChartType.Bar_Clustered_3D:
        case OfficeChartType.Bar_Stacked_3D:
        case OfficeChartType.Bar_Stacked_100_3D:
        case OfficeChartType.Cylinder_Clustered:
        case OfficeChartType.Cylinder_Stacked:
        case OfficeChartType.Cylinder_Stacked_100:
        case OfficeChartType.Cylinder_Bar_Clustered:
        case OfficeChartType.Cylinder_Bar_Stacked:
        case OfficeChartType.Cylinder_Bar_Stacked_100:
        case OfficeChartType.Cylinder_Clustered_3D:
        case OfficeChartType.Cone_Clustered:
        case OfficeChartType.Cone_Stacked:
        case OfficeChartType.Cone_Stacked_100:
        case OfficeChartType.Cone_Bar_Clustered:
        case OfficeChartType.Cone_Bar_Stacked:
        case OfficeChartType.Cone_Bar_Stacked_100:
        case OfficeChartType.Cone_Clustered_3D:
        case OfficeChartType.Pyramid_Clustered:
        case OfficeChartType.Pyramid_Stacked:
        case OfficeChartType.Pyramid_Stacked_100:
        case OfficeChartType.Pyramid_Bar_Clustered:
        case OfficeChartType.Pyramid_Bar_Stacked:
        case OfficeChartType.Pyramid_Bar_Stacked_100:
        case OfficeChartType.Pyramid_Clustered_3D:
          num = this.SerializeBar3DChart(writer, chart, firstSeries);
          break;
        case OfficeChartType.Line:
        case OfficeChartType.Line_Stacked:
        case OfficeChartType.Line_Stacked_100:
        case OfficeChartType.Line_Markers:
        case OfficeChartType.Line_Markers_Stacked:
        case OfficeChartType.Line_Markers_Stacked_100:
          num = this.SerializeLineChart(writer, chart, firstSeries);
          break;
        case OfficeChartType.Line_3D:
          num = this.SerializeLine3DChart(writer, chart, firstSeries);
          break;
        case OfficeChartType.Pie:
        case OfficeChartType.Pie_Exploded:
          num = this.SerializePieChart(writer, chart, firstSeries);
          break;
        case OfficeChartType.Pie_3D:
        case OfficeChartType.Pie_Exploded_3D:
          num = this.SerializePie3DChart(writer, chart, firstSeries);
          break;
        case OfficeChartType.PieOfPie:
        case OfficeChartType.Pie_Bar:
          num = this.SerializeOfPieChart(writer, chart, firstSeries);
          break;
        case OfficeChartType.Scatter_Markers:
        case OfficeChartType.Scatter_SmoothedLine_Markers:
        case OfficeChartType.Scatter_SmoothedLine:
        case OfficeChartType.Scatter_Line_Markers:
        case OfficeChartType.Scatter_Line:
          num = this.SerializeScatterChart(writer, chart, firstSeries);
          break;
        case OfficeChartType.Area:
        case OfficeChartType.Area_Stacked:
        case OfficeChartType.Area_Stacked_100:
          num = this.SerializeAreaChart(writer, chart, firstSeries);
          break;
        case OfficeChartType.Area_3D:
        case OfficeChartType.Area_Stacked_3D:
        case OfficeChartType.Area_Stacked_100_3D:
          num = this.SerializeArea3DChart(writer, chart, firstSeries);
          break;
        case OfficeChartType.Doughnut:
        case OfficeChartType.Doughnut_Exploded:
          num = this.SerializeDoughnutChart(writer, chart, firstSeries);
          break;
        case OfficeChartType.Radar:
        case OfficeChartType.Radar_Markers:
        case OfficeChartType.Radar_Filled:
          num = this.SerializeRadarChart(writer, chart, firstSeries);
          break;
        case OfficeChartType.Surface_3D:
        case OfficeChartType.Surface_NoColor_3D:
          num = this.SerializeSurface3DChart(writer, chart, firstSeries);
          break;
        case OfficeChartType.Surface_Contour:
        case OfficeChartType.Surface_NoColor_Contour:
          num = this.SerializeSurfaceChart(writer, chart, firstSeries);
          break;
        case OfficeChartType.Bubble:
        case OfficeChartType.Bubble_3D:
          num = this.SerializeBubbleChart(writer, chart, firstSeries);
          break;
        case OfficeChartType.Stock_HighLowClose:
        case OfficeChartType.Stock_OpenHighLowClose:
        case OfficeChartType.Stock_VolumeHighLowClose:
        case OfficeChartType.Stock_VolumeOpenHighLowClose:
          num = this.SerializeStockChart(writer, chart, firstSeries);
          break;
      }
    }
    return num;
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

  [SecurityCritical]
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
      this.SerializeVaryColors(writer, firstSeries);
    }
    int num = this.SerializeChartSeries(writer, chart, firstSeries, new ChartSerializator.SerializeSeriesDelegate(this.SerializeRadarSeries));
    if (chart.CommonDataPointsCollection != null && chart.CommonDataPointsCollection.ContainsKey(firstSeries.ChartGroup))
      this.SerializeDataLabels(writer, chart, chart.CommonDataPointsCollection[firstSeries.ChartGroup]);
    this.SerializeBarAxisId(writer, chart, firstSeries);
    writer.WriteEndElement();
    return num;
  }

  [SecurityCritical]
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

  [SecurityCritical]
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

  [SecurityCritical]
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
    string str = chart.PivotChartType == OfficeChartType.PieOfPie ? "pie" : "bar";
    ChartSerializatorCommon.SerializeValueTag(writer, "ofPieType", str);
    ChartSerializatorCommon.SerializeBoolValueTag(writer, "varyColors", true);
  }

  [SecurityCritical]
  private int SerializeOfPieChart(XmlWriter writer, ChartImpl chart, ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("ofPieChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    string str = firstSeries.SerieType == OfficeChartType.PieOfPie ? "pie" : "bar";
    ChartSerializatorCommon.SerializeValueTag(writer, "ofPieType", str);
    int num = this.SerializePieCommon(writer, chart, firstSeries);
    IOfficeChartFormat commonSerieOptions = firstSeries.SerieFormat.CommonSerieOptions;
    ChartSerializatorCommon.SerializeValueTag(writer, "gapWidth", commonSerieOptions.GapWidth.ToString());
    int splitValue = commonSerieOptions.SplitValue;
    if (splitValue != 0)
    {
      Excel2007SplitType splitType = (Excel2007SplitType) commonSerieOptions.SplitType;
      ChartSerializatorCommon.SerializeValueTag(writer, "splitType", splitType.ToString());
      ChartSerializatorCommon.SerializeValueTag(writer, "splitPos", splitValue.ToString());
    }
    ChartSerializatorCommon.SerializeValueTag(writer, "secondPieSize", commonSerieOptions.PieSecondSize.ToString());
    writer.WriteElementString("serLines", "http://schemas.openxmlformats.org/drawingml/2006/chart", string.Empty);
    writer.WriteEndElement();
    return num;
  }

  [SecurityCritical]
  private int SerializeStockChart(XmlWriter writer, ChartImpl chart, ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("stockChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    IOfficeChartSeries series1 = chart.Series;
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
      IOfficeChartBorder border = (IOfficeChartBorder) null;
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

  private void SerializeHiLowLineProperties(
    XmlWriter writer,
    ChartFormatImpl format,
    ChartImpl chart)
  {
    if (format.IsChartLineFormat)
    {
      writer.WriteStartElement("hiLowLines", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      writer.WriteStartElement("spPr", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      ChartSerializatorCommon.SerializeLineProperties(writer, format.HighLowLineProperties, chart.Workbook);
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
    else
      writer.WriteElementString("hiLowLines", "http://schemas.openxmlformats.org/drawingml/2006/chart", string.Empty);
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

  [SecurityCritical]
  private int SerializeDoughnutChart(XmlWriter writer, ChartImpl chart, ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    writer.WriteStartElement("doughnutChart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    int num = this.SerializePieCommon(writer, chart, firstSeries);
    IOfficeChartFormat commonSerieOptions = firstSeries.SerieFormat.CommonSerieOptions;
    int firstSliceAngle = commonSerieOptions.FirstSliceAngle;
    ChartSerializatorCommon.SerializeValueTag(writer, "firstSliceAng", firstSliceAngle.ToString());
    int doughnutHoleSize = commonSerieOptions.DoughnutHoleSize;
    ChartSerializatorCommon.SerializeValueTag(writer, "holeSize", doughnutHoleSize.ToString());
    writer.WriteEndElement();
    return num;
  }

  [SecurityCritical]
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

  [SecurityCritical]
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

  [SecurityCritical]
  private void SerializeDataLabels(
    XmlWriter writer,
    IOfficeChartDataLabels dataLabels,
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
    this.SerializeDataLabelSettings(writer, dataLabels, parentChart, true);
    writer.WriteEndElement();
  }

  private void SerializeNumFormat(XmlWriter writer, ChartDataLabelsImpl dataLabels)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (dataLabels == null)
      throw new ArgumentNullException(nameof (dataLabels));
    writer.WriteStartElement("numFmt", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteAttributeString("formatCode", dataLabels.NumberFormat);
    writer.WriteAttributeString("sourceLinked", Convert.ToInt16(dataLabels.IsSourceLinked).ToString());
    writer.WriteEndElement();
  }

  [SecurityCritical]
  private void SerializeDataLabel(
    XmlWriter writer,
    IOfficeChartDataLabels dataLabels,
    int index,
    ChartImpl chart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (dataLabels == null)
      throw new ArgumentNullException(nameof (dataLabels));
    writer.WriteStartElement("dLbl", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    ChartSerializatorCommon.SerializeValueTag(writer, "idx", index.ToString());
    if (dataLabels is ChartDataLabelsImpl)
    {
      IOfficeChartLayout layout = (dataLabels as ChartDataLabelsImpl).Layout;
      if (layout != null)
      {
        IOfficeChartManualLayout manualLayout = layout.ManualLayout;
        if (manualLayout != null && (manualLayout.LayoutTarget != LayoutTargets.auto || manualLayout.LeftMode != LayoutModes.auto || manualLayout.TopMode != LayoutModes.auto || manualLayout.Left != 0.0 || manualLayout.Top != 0.0 || manualLayout.WidthMode != LayoutModes.auto || manualLayout.HeightMode != LayoutModes.auto || manualLayout.Width != 0.0 || manualLayout.Height != 0.0))
          ChartSerializatorCommon.SerializeLayout(writer, (object) (dataLabels as ChartDataLabelsImpl));
      }
    }
    IInternalOfficeChartTextArea textArea = dataLabels as IInternalOfficeChartTextArea;
    if (!string.IsNullOrEmpty(textArea.Text))
    {
      WorkbookImpl parentWorkbook = chart.ParentWorkbook;
      if (textArea is ChartTextAreaImpl && (textArea as ChartTextAreaImpl).Layout != null)
        ChartSerializatorCommon.SerializeLayout(writer, (object) (textArea as ChartTextAreaImpl));
      ChartSerializatorCommon.SerializeTextAreaText(writer, (IOfficeChartTextArea) textArea, (IWorkbook) parentWorkbook, 10.0);
    }
    this.SerializeDataLabelSettings(writer, dataLabels, chart, true);
    writer.WriteEndElement();
  }

  [SecurityCritical]
  private void SerializeDataLabelSettings(
    XmlWriter writer,
    IOfficeChartDataLabels dataLabels,
    ChartImpl chart,
    bool SerializeLeaderLines)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    ChartDataLabelsImpl chartDataLabelsImpl1 = dataLabels != null ? dataLabels as ChartDataLabelsImpl : throw new ArgumentNullException(nameof (dataLabels));
    ChartDataPointImpl parent1 = dataLabels.Parent as ChartDataPointImpl;
    if (chartDataLabelsImpl1.ShowTextProperties)
    {
      ChartSerializatorCommon.SerializeFrameFormat(writer, (IOfficeChartFillBorder) dataLabels.FrameFormat, chart, false);
      if (((ChartDataLabelsImpl) dataLabels).ParagraphType == ChartParagraphType.CustomDefault)
        this.SerializeDefaultTextFormatting(writer, (IOfficeChartTextArea) dataLabels, chart.Workbook, 10.0);
    }
    else if ((dataLabels as ChartDataLabelsImpl).IsDelete)
      ChartSerializatorCommon.SerializeBoolValueTag(writer, "delete", true);
    OfficeDataLabelPosition position = dataLabels.Position;
    switch (position)
    {
      case OfficeDataLabelPosition.Automatic:
      case OfficeDataLabelPosition.Moved:
        if (chart.Workbook.Version == OfficeVersion.Excel2007)
        {
          if ((dataLabels as ChartDataLabelsImpl).m_bHasLegendKeyOption || dataLabels.IsLegendKey || chart.DestinationType == OfficeChartType.Scatter_Line_Markers)
            ChartSerializatorCommon.SerializeBoolValueTag(writer, "showLegendKey", dataLabels.IsLegendKey);
          if ((dataLabels as ChartDataLabelsImpl).m_bHasValueOption || dataLabels.IsValue || chart.DestinationType == OfficeChartType.Scatter_Line_Markers)
            ChartSerializatorCommon.SerializeBoolValueTag(writer, "showVal", dataLabels.IsValue);
          if ((dataLabels as ChartDataLabelsImpl).m_bHasCategoryOption || dataLabels.IsCategoryName || chart.DestinationType == OfficeChartType.Scatter_Line_Markers)
            ChartSerializatorCommon.SerializeBoolValueTag(writer, "showCatName", dataLabels.IsCategoryName);
          if ((dataLabels as ChartDataLabelsImpl).m_bHasSeriesOption || dataLabels.IsSeriesName || chart.DestinationType == OfficeChartType.Scatter_Line_Markers)
            ChartSerializatorCommon.SerializeBoolValueTag(writer, "showSerName", dataLabels.IsSeriesName);
          if ((dataLabels as ChartDataLabelsImpl).m_bHasPercentageOption || dataLabels.IsPercentage || chart.DestinationType == OfficeChartType.Scatter_Line_Markers)
            ChartSerializatorCommon.SerializeBoolValueTag(writer, "showPercent", dataLabels.IsPercentage);
          if ((dataLabels as ChartDataLabelsImpl).m_bHasBubbleSizeOption || dataLabels.IsBubbleSize || chart.DestinationType == OfficeChartType.Scatter_Line_Markers)
            ChartSerializatorCommon.SerializeBoolValueTag(writer, "showBubbleSize", dataLabels.IsBubbleSize);
        }
        else
        {
          ChartDataLabelsImpl chartDataLabelsImpl2 = dataLabels as ChartDataLabelsImpl;
          if (chartDataLabelsImpl2.m_bHasLegendKeyOption || chartDataLabelsImpl2.m_bHasValueOption || chartDataLabelsImpl2.m_bHasCategoryOption || chartDataLabelsImpl2.m_bHasSeriesOption || chartDataLabelsImpl2.m_bHasPercentageOption || chartDataLabelsImpl2.m_bHasBubbleSizeOption || chart.DestinationType == OfficeChartType.Scatter_Line_Markers)
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
        if (parent1.IsDefault && (SerializeLeaderLines || chartDataLabelsImpl1.CheckSerieIsPie || chart.IsChartPie))
        {
          ChartSerieImpl parent2 = parent1.IsDefault ? (parent1.Parent as ChartDataPointsCollection).Parent as ChartSerieImpl : (ChartSerieImpl) null;
          ChartSerializatorCommon.SerializeBoolValueTag(writer, "showLeaderLines", dataLabels.ShowLeaderLines);
          if (parent2 == null)
            break;
          writer.WriteStartElement("c", "extLst", (string) null);
          writer.WriteStartElement("c", "ext", (string) null);
          writer.WriteAttributeString("uri", "{CE6537A1-D6FC-4f65-9D91-7224C49458BB}");
          writer.WriteAttributeString("xmlns", "c15", (string) null, "http://schemas.microsoft.com/office/drawing/2012/chart");
          if (chartDataLabelsImpl1.IsValueFromCells)
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
        if (!chartDataLabelsImpl1.IsValueFromCells)
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
        goto case OfficeDataLabelPosition.Automatic;
    }
  }

  private void SerializeDefaultTextFormatting(
    XmlWriter writer,
    IOfficeChartTextArea textFormatting,
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
    ChartSerializatorCommon.SerializeParagraphRunProperites(writer, (IOfficeFont) textFormatting, "defRPr", book, defaultFontSize);
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

  [SecurityCritical]
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
      if (series.ParentChart.CategoryLabelLevel == OfficeCategoriesLabelLevel.CategoriesLabelLevelAll)
        this.SerializeSeriesCategory(writer, series);
      this.SerializeSeriesValues(writer, series);
    }
    else
    {
      if (series.ParentChart.CategoryLabelLevel == OfficeCategoriesLabelLevel.CategoriesLabelLevelAll)
        this.SerializeFilteredCategory(writer, series, this.findFilter);
      this.SerializeFilteredValues(writer, series, this.findFilter);
    }
    if (series.ParentChart.Loading && series.HasColumnShape || series.SerieFormat != null)
      this.SerializeBarShape(writer, series.ParentChart, series, true);
    if (series.ParentChart.CategoryLabelLevel == OfficeCategoriesLabelLevel.CategoriesLabelLevelAll && series.ParentChart.SeriesNameLevel == OfficeSeriesNameLevel.SeriesNameLevelAll)
    {
      bool? invertIfNegative = series.GetInvertIfNegative();
      if ((!invertIfNegative.GetValueOrDefault() ? 0 : (invertIfNegative.HasValue ? 1 : 0)) == 0 && !series.DataPoints.DefaultDataPoint.DataLabels.IsValueFromCells)
        goto label_31;
    }
    writer.WriteStartElement("extLst", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    if (series.ParentChart.CategoryLabelLevel != OfficeCategoriesLabelLevel.CategoriesLabelLevelAll || series.ParentChart.SeriesNameLevel != OfficeSeriesNameLevel.SeriesNameLevelAll)
    {
      if (series.ParentChart.SeriesNameLevel != OfficeSeriesNameLevel.SeriesNameLevelAll)
        this.SerializeFilteredSeriesOrCategoryName(writer, series, true);
      if (series.ParentChart.CategoryLabelLevel != OfficeCategoriesLabelLevel.CategoriesLabelLevelAll)
        this.SerializeFilteredSeriesOrCategoryName(writer, series, false);
    }
    else
    {
      bool? invertIfNegative1 = series.GetInvertIfNegative();
      if ((!invertIfNegative1.GetValueOrDefault() ? 0 : (invertIfNegative1.HasValue ? 1 : 0)) != 0)
      {
        Stream fillFormatStream = series.m_invertFillFormatStream;
        bool? invertIfNegative2 = series.GetInvertIfNegative();
        if ((!invertIfNegative2.GetValueOrDefault() ? 0 : (invertIfNegative2.HasValue ? 1 : 0)) != 0 && series.SerieFormat.Fill.FillType == OfficeFillType.SolidColor)
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

  private void SeriealizeValuesFromCellsRange(XmlWriter writer, ChartSerieImpl series)
  {
    IRange valueFromCellsIrange = (series.DataPoints.DefaultDataPoint.DataLabels as ChartDataLabelsImpl).ValueFromCellsIRange;
    if (!series.DataPoints.DefaultDataPoint.DataLabels.IsValueFromCells || valueFromCellsIrange == null)
      return;
    writer.WriteStartElement("c", "ext", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteAttributeString("uri", "{02D57815-91ED-43cb-92C2-25804820EDAC}");
    writer.WriteAttributeString("xmlns", "c15", (string) null, "http://schemas.microsoft.com/office/drawing/2012/chart");
    writer.WriteStartElement("c15", "datalabelsRange", (string) null);
    writer.WriteStartElement("c15", "f", (string) null);
    writer.WriteString(valueFromCellsIrange.AddressGlobal);
    writer.WriteEndElement();
    this.serializeDataLabelRangeCache(writer, series.DataLabelCellsValues);
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private void serializeDataLabelRangeCache(XmlWriter writer, Dictionary<int, object> values)
  {
    if (values.Count <= 0)
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

  private bool FindFiltered(ChartSerieImpl series)
  {
    IOfficeChartCategories categories = series.ParentChart.Categories;
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
    if (!(parentChart.Categories[0] as ChartCategory).Filter_customize)
      return;
    WorksheetBaseImpl worksheet1 = (parentChart.DataRange as ChartDataRange).Range.Worksheet as WorksheetBaseImpl;
    IOfficeChartSeries series1 = parentChart.Series;
    IWorksheet worksheet2 = worksheet1 as IWorksheet;
    IOfficeChartCategories categories = (IOfficeChartCategories) (parentChart.Categories as ChartCategoryCollection);
    int count1 = series.ValuesIRange.Count;
    int count2 = categories.Count;
    int row1 = 0;
    int lastRow = 0;
    string str1 = string.Empty;
    int column = 0;
    int lastColumn = 0;
    int num = 0;
    for (int index1 = 0; index1 < (categories[0].Values as ChartDataRange).Range.Count; ++index1)
    {
      for (int index2 = 0; index2 < categories.Count; ++index2)
      {
        if (!categories[index2].IsFiltered && row1 == 0)
        {
          row1 = (categories[index2].Values as ChartDataRange).Range.Cells[index1].Row;
          column = (categories[index2].Values as ChartDataRange).Range.Cells[index1].Column;
        }
        else if (row1 != 0 && categories[index2].IsFiltered)
        {
          lastColumn = (categories[index2].Values as ChartDataRange).Range.Cells[index1].Column;
          lastRow = (categories[index2].Values as ChartDataRange).Range.Cells[index1].Row;
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
        lastColumn = (categories[categories.Count - 1].Values as ChartDataRange).Range.Cells[index1].Column;
        int row2 = (categories[categories.Count - 1].Values as ChartDataRange).Range.Cells[index1].Row;
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

  private void UpdateCategoryLabel(ChartSerieImpl series)
  {
    ChartImpl parentChart = series.ParentChart;
    if (!(parentChart.Categories[0] as ChartCategory).Filter_customize || series.CategoryLabelsIRange == null)
      return;
    WorksheetBaseImpl worksheet1 = (parentChart.DataRange as ChartDataRange).Range.Worksheet as WorksheetBaseImpl;
    IOfficeChartSeries series1 = parentChart.Series;
    IWorksheet worksheet2 = worksheet1 as IWorksheet;
    IOfficeChartCategories categories = (IOfficeChartCategories) (parentChart.Categories as ChartCategoryCollection);
    int count1 = series.ValuesIRange.Count;
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
        row1 = (categories[index].CategoryLabel as ChartDataRange).Range.Cells[index].Row;
        column1 = (categories[index].CategoryLabel as ChartDataRange).Range.Cells[index].Column;
      }
      else if (row1 != 0 && categories[index].IsFiltered)
      {
        lastColumn = (categories[index].CategoryLabel as ChartDataRange).Range.Cells[index].Column;
        lastRow = (categories[index].CategoryLabel as ChartDataRange).Range.Cells[index].Row;
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
      int column2 = (categories[categories.Count - 1].CategoryLabel as ChartDataRange).Range.Cells[categories.Count - 1].Column;
      int row2 = (categories[categories.Count - 1].CategoryLabel as ChartDataRange).Range.Cells[categories.Count - 1].Row;
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
      if (series.CategoryLabelsIRange == null)
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

  private void SerializeFilteredCategoryName(XmlWriter writer, ChartSerieImpl series)
  {
    if (series.CategoryLabelsIRange == null)
      return;
    writer.WriteStartElement("c15", "cat", "http://schemas.microsoft.com/office/drawing/2012/chart");
    if (series.CategoryLabelsIRange.HasString)
      writer.WriteStartElement("c", "strRef", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    else if (series.CategoryLabelsIRange.HasNumber || series.CategoryLabelsIRange.HasDateTime)
      writer.WriteStartElement("c", "numRef", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteStartElement("c", "extLst", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteStartElement("c", "ext", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteAttributeString("uri", "{02D57815-91ED-43cb-92C2-25804820EDAC}");
    writer.WriteStartElement("c15", "formulaRef", "http://schemas.microsoft.com/office/drawing/2012/chart");
    writer.WriteElementString("sqref", "http://schemas.microsoft.com/office/drawing/2012/chart", series.CategoryLabelsIRange.AddressGlobal);
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
    if (series.CategoryLabelsIRange.HasString)
    {
      writer.WriteStartElement("c", "strCache", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      this.SerializeCategoryTagCacheValues(writer, series, false);
    }
    else if (series.CategoryLabelsIRange.HasNumber || series.CategoryLabelsIRange.HasDateTime)
    {
      writer.WriteStartElement("c", "numCache", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      if (series.CategoriesFormatCode != null)
        writer.WriteElementString("formatCode", "http://schemas.openxmlformats.org/drawingml/2006/chart", series.CategoriesFormatCode);
      else
        writer.WriteElementString("formatCode", "http://schemas.openxmlformats.org/drawingml/2006/chart", series.ParentChart.PrimaryCategoryAxis.NumberFormat);
      this.SerializeCategoryTagCacheValues(writer, series, true);
    }
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  [SecurityCritical]
  private void SerializePieSeries(XmlWriter writer, ChartSerieImpl series)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    this.SerializeSeriesCommonWithoutEnd(writer, series, false);
    int percent = series.SerieFormat.Percent;
    if (percent != 0 || series.ParentSeries[0].Equals((object) series))
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
      if (series.ParentChart.CategoryLabelLevel == OfficeCategoriesLabelLevel.CategoriesLabelLevelAll)
        this.SerializeSeriesCategory(writer, series);
      this.SerializeSeriesValues(writer, series);
    }
    else
    {
      if (series.ParentChart.CategoryLabelLevel == OfficeCategoriesLabelLevel.CategoriesLabelLevelAll)
        this.SerializeFilteredCategory(writer, series, this.findFilter);
      this.SerializeFilteredValues(writer, series, this.findFilter);
    }
    if (series.ParentChart.CategoryLabelLevel != OfficeCategoriesLabelLevel.CategoriesLabelLevelAll || series.ParentChart.SeriesNameLevel != OfficeSeriesNameLevel.SeriesNameLevelAll || series.DataPoints.DefaultDataPoint.DataLabels.IsValueFromCells)
    {
      writer.WriteStartElement("extLst", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      if (series.ParentChart.CategoryLabelLevel != OfficeCategoriesLabelLevel.CategoriesLabelLevelAll || series.ParentChart.SeriesNameLevel != OfficeSeriesNameLevel.SeriesNameLevelAll)
      {
        if (series.ParentChart.SeriesNameLevel != OfficeSeriesNameLevel.SeriesNameLevelAll)
          this.SerializeFilteredSeriesOrCategoryName(writer, series, true);
        if (series.ParentChart.CategoryLabelLevel != OfficeCategoriesLabelLevel.CategoriesLabelLevelAll)
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

  [SecurityCritical]
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
      if (series.ParentChart.CategoryLabelLevel == OfficeCategoriesLabelLevel.CategoriesLabelLevelAll)
        this.SerializeSeriesCategory(writer, series);
      this.SerializeSeriesValues(writer, series);
    }
    else
    {
      if (series.ParentChart.CategoryLabelLevel == OfficeCategoriesLabelLevel.CategoriesLabelLevelAll)
        this.SerializeFilteredCategory(writer, series, this.findFilter);
      this.SerializeFilteredValues(writer, series, this.findFilter);
    }
    if (series.ParentChart.CategoryLabelLevel != OfficeCategoriesLabelLevel.CategoriesLabelLevelAll || series.ParentChart.SeriesNameLevel != OfficeSeriesNameLevel.SeriesNameLevelAll || series.DataPoints.DefaultDataPoint.DataLabels.IsValueFromCells)
    {
      writer.WriteStartElement("extLst", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      if (series.ParentChart.CategoryLabelLevel != OfficeCategoriesLabelLevel.CategoriesLabelLevelAll || series.ParentChart.SeriesNameLevel != OfficeSeriesNameLevel.SeriesNameLevelAll)
      {
        if (series.ParentChart.SeriesNameLevel != OfficeSeriesNameLevel.SeriesNameLevelAll)
          this.SerializeFilteredSeriesOrCategoryName(writer, series, true);
        if (series.ParentChart.CategoryLabelLevel != OfficeCategoriesLabelLevel.CategoriesLabelLevelAll)
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

  [SecurityCritical]
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
    if (series.ParentChart.CategoryLabelLevel != OfficeCategoriesLabelLevel.CategoriesLabelLevelAll || series.ParentChart.SeriesNameLevel != OfficeSeriesNameLevel.SeriesNameLevelAll || series.DataPoints.DefaultDataPoint.DataLabels.IsValueFromCells)
    {
      writer.WriteStartElement("extLst", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      if (series.ParentChart.CategoryLabelLevel != OfficeCategoriesLabelLevel.CategoriesLabelLevelAll || series.ParentChart.SeriesNameLevel != OfficeSeriesNameLevel.SeriesNameLevelAll)
      {
        if (series.ParentChart.SeriesNameLevel != OfficeSeriesNameLevel.SeriesNameLevelAll)
          this.SerializeFilteredSeriesOrCategoryName(writer, series, true);
        if (series.ParentChart.CategoryLabelLevel != OfficeCategoriesLabelLevel.CategoriesLabelLevelAll)
          this.SerializeFilteredSeriesOrCategoryName(writer, series, false);
      }
      else if (series.DataPoints.DefaultDataPoint.DataLabels.IsValueFromCells)
        this.SeriealizeValuesFromCellsRange(writer, series);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  [SecurityCritical]
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
      if (series.ParentChart.CategoryLabelLevel == OfficeCategoriesLabelLevel.CategoriesLabelLevelAll)
        this.SerializeSeriesCategory(writer, series);
      this.SerializeSeriesValues(writer, series);
    }
    else
    {
      if (series.ParentChart.CategoryLabelLevel == OfficeCategoriesLabelLevel.CategoriesLabelLevelAll)
        this.SerializeFilteredCategory(writer, series, this.findFilter);
      this.SerializeFilteredValues(writer, series, this.findFilter);
    }
    if (series.ParentChart.CategoryLabelLevel != OfficeCategoriesLabelLevel.CategoriesLabelLevelAll || series.ParentChart.SeriesNameLevel != OfficeSeriesNameLevel.SeriesNameLevelAll || series.DataPoints.DefaultDataPoint.DataLabels.IsValueFromCells)
    {
      writer.WriteStartElement("extLst", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      if (series.ParentChart.CategoryLabelLevel != OfficeCategoriesLabelLevel.CategoriesLabelLevelAll || series.ParentChart.SeriesNameLevel != OfficeSeriesNameLevel.SeriesNameLevelAll)
      {
        if (series.ParentChart.SeriesNameLevel != OfficeSeriesNameLevel.SeriesNameLevelAll)
          this.SerializeFilteredSeriesOrCategoryName(writer, series, true);
        if (series.ParentChart.CategoryLabelLevel != OfficeCategoriesLabelLevel.CategoriesLabelLevelAll)
          this.SerializeFilteredSeriesOrCategoryName(writer, series, false);
      }
      if (series.DataPoints.DefaultDataPoint.DataLabels.IsValueFromCells)
        this.SeriealizeValuesFromCellsRange(writer, series);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  [SecurityCritical]
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
    this.SerializeSeriesValues(writer, series.BubblesIRange, series.EnteredDirectlyBubbles, "bubbleSize", series);
    if (series.SerieType == OfficeChartType.Bubble_3D)
      ChartSerializatorCommon.SerializeValueTag(writer, "bubble3D", "1");
    if (series.ParentChart.CategoryLabelLevel != OfficeCategoriesLabelLevel.CategoriesLabelLevelAll || series.ParentChart.SeriesNameLevel != OfficeSeriesNameLevel.SeriesNameLevelAll || series.DataPoints.DefaultDataPoint.DataLabels.IsValueFromCells)
    {
      writer.WriteStartElement("extLst", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      if (series.ParentChart.CategoryLabelLevel != OfficeCategoriesLabelLevel.CategoriesLabelLevelAll || series.ParentChart.SeriesNameLevel != OfficeSeriesNameLevel.SeriesNameLevelAll)
      {
        if (series.ParentChart.SeriesNameLevel != OfficeSeriesNameLevel.SeriesNameLevelAll)
          this.SerializeFilteredSeriesOrCategoryName(writer, series, true);
        if (series.ParentChart.CategoryLabelLevel != OfficeCategoriesLabelLevel.CategoriesLabelLevelAll)
          this.SerializeFilteredSeriesOrCategoryName(writer, series, false);
      }
      if (series.DataPoints.DefaultDataPoint.DataLabels.IsValueFromCells)
        this.SeriealizeValuesFromCellsRange(writer, series);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  [SecurityCritical]
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
      if (series.ParentChart.CategoryLabelLevel == OfficeCategoriesLabelLevel.CategoriesLabelLevelAll)
        this.SerializeSeriesCategory(writer, series);
      this.SerializeSeriesValues(writer, series);
    }
    else
    {
      if (series.ParentChart.CategoryLabelLevel == OfficeCategoriesLabelLevel.CategoriesLabelLevelAll)
        this.SerializeFilteredCategory(writer, series, this.findFilter);
      this.SerializeFilteredValues(writer, series, this.findFilter);
    }
    if (series.ParentChart.CategoryLabelLevel != OfficeCategoriesLabelLevel.CategoriesLabelLevelAll || series.ParentChart.SeriesNameLevel != OfficeSeriesNameLevel.SeriesNameLevelAll || series.DataPoints.DefaultDataPoint.DataLabels.IsValueFromCells)
    {
      writer.WriteStartElement("extLst", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      if (series.ParentChart.CategoryLabelLevel != OfficeCategoriesLabelLevel.CategoriesLabelLevelAll || series.ParentChart.SeriesNameLevel != OfficeSeriesNameLevel.SeriesNameLevelAll)
      {
        if (series.ParentChart.SeriesNameLevel != OfficeSeriesNameLevel.SeriesNameLevelAll)
          this.SerializeFilteredSeriesOrCategoryName(writer, series, true);
        if (series.ParentChart.CategoryLabelLevel != OfficeCategoriesLabelLevel.CategoriesLabelLevelAll)
          this.SerializeFilteredSeriesOrCategoryName(writer, series, false);
      }
      if (series.DataPoints.DefaultDataPoint.DataLabels.IsValueFromCells)
        this.SeriealizeValuesFromCellsRange(writer, series);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  [SecurityCritical]
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
      if (!series.IsDefaultName && series.ParentChart.SeriesNameLevel == OfficeSeriesNameLevel.SeriesNameLevelAll)
      {
        string nameOrFormula = series.NameOrFormula;
        writer.WriteStartElement("tx", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        if (nameOrFormula != null && nameOrFormula.Length > 0 && nameOrFormula[0] == '=')
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
      ChartSerializatorCommon.SerializeFrameFormat(writer, (IOfficeChartFillBorder) dataFormatOrNull, series.ParentChart, false, true);
    if (series.GetInvertIfNegative().HasValue)
    {
      bool? invertIfNegative = series.GetInvertIfNegative();
      string str = (!invertIfNegative.GetValueOrDefault() ? 0 : (invertIfNegative.HasValue ? 1 : 0)) != 0 ? "1" : "0";
      ChartSerializatorCommon.SerializeValueTag(writer, "invertIfNegative", str);
    }
    this.SerializeMarker(writer, series);
    ChartDataPointsCollection dataPoints = (ChartDataPointsCollection) series.DataPoints;
    if (dataPoints.DeninedDPCount <= 0)
      return;
    foreach (ChartDataPointImpl dataPoint in dataPoints)
    {
      if (!dataPoint.IsDefault || dataPoint.HasDataPoint)
        this.SerializeDataPoint(writer, dataPoint, series);
    }
  }

  [SecurityCritical]
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
      ChartSerializatorCommon.SerializeFrameFormat(writer, (IOfficeChartFillBorder) series.SerieFormat, series.ParentChart, false);
    else if (!serieDataFormatImpl.IsDefault && (serieDataFormatImpl.IsDataPointColorParsed || !serieDataFormatImpl.IsParsed))
      ChartSerializatorCommon.SerializeFrameFormat(writer, (IOfficeChartFillBorder) serieDataFormatImpl, serieDataFormatImpl.ParentChart, false);
    if (dataPoint.IsDefaultmarkertype)
      this.SerializeMarker(writer, serieDataFormatImpl);
    writer.WriteEndElement();
  }

  private void SerializeSeriesCategory(XmlWriter writer, ChartSerieImpl series, string tagName)
  {
    this.SerializeSeriesValues(writer, series.CategoryLabelsIRange, series.EnteredDirectlyCategoryLabels, tagName, series);
  }

  private void SerializeSeriesCategory(XmlWriter writer, ChartSerieImpl series)
  {
    this.SerializeSeriesValues(writer, series.CategoryLabelsIRange, series.EnteredDirectlyCategoryLabels, "cat", series);
  }

  private void SerializeSeriesValues(XmlWriter writer, ChartSerieImpl series)
  {
    this.SerializeSeriesValues(writer, series.ValuesIRange, series.EnteredDirectlyValues, "val", series);
  }

  private void SerializeSeriesValues(XmlWriter writer, ChartSerieImpl series, string tagName)
  {
    this.SerializeSeriesValues(writer, series.ValuesIRange, series.EnteredDirectlyValues, tagName, series);
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
      if ((workbookImpl.IsCreated || workbookImpl.IsConverted) && series.MulLvlStrRefFormula == null)
        this.SerializeNormalReference(writer, range, values, tagName, series);
      else if (workbookImpl.IsLoaded || workbookImpl.IsWorkbookOpening || series.MulLvlStrRefFormula != null)
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
          this.SerializeMultiLevelStringReference(writer, range, values, series);
        else if (values != null)
          this.SerializeDirectlyEntered(writer, values, false);
      }
      else if (range != null && tagName != "cat")
        this.SerializeReference(writer, range, values, series, tagName);
      else if (range != null && tagName == "cat")
        this.SerializeMultiLevelStringReference(writer, range, values, series);
    }
    else if (values != null)
      this.SerializeDirectlyEntered(writer, values, false);
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
    int num;
    switch (range)
    {
      case null:
      case ExternalRange _:
        num = 0;
        break;
      default:
        num = range.HasDateTime ? 1 : 0;
        break;
    }
    bool flag = num != 0;
    if (range != null && (range is RangeImpl && (range as RangeImpl).IsNumReference || (!(tagName != "cat") || !(range is RangeImpl) || (range as RangeImpl).IsStringReference ? (flag ? 1 : 0) : 1) != 0 || tagName == "cat" && (series.CategoriesFormatCode != null && (series.CategoriesFormatCode.EndsWith("%") || series.CategoriesFormatCode.ToLowerInvariant().EndsWith("y")) || !(range is ExternalRange) && range.NumberFormat != null && range.NumberFormat.ToLowerInvariant().EndsWith("y"))))
      this.SerializeReference(writer, range, values, series, tagName);
    else if (range != null && (tagName == "cat" || range is RangeImpl && (range as RangeImpl).IsStringReference))
    {
      this.SerializeStringReference(writer, range, series, tagName);
    }
    else
    {
      if (values == null)
        return;
      this.SerializeDirectlyEntered(writer, values, false);
    }
  }

  private void SerializeReference(
    XmlWriter writer,
    IRange range,
    object[] rangeValues,
    ChartSerieImpl series,
    string tagName)
  {
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    ChartImpl parentChart = series.ParentChart;
    bool flag = false;
    if (tagName != "yVal" && tagName != "val")
    {
      if (rangeValues != null)
        this.IsStringValue(rangeValues);
      flag = range.HasString;
      if (!flag && range is RangeImpl)
        flag = (range as RangeImpl).IsSingleCellContainsString;
    }
    if (flag && tagName != "yVal" && tagName != "val")
      this.SerializeStringReference(writer, range, series, tagName);
    else
      this.SerializeNumReference(writer, range, rangeValues, series, tagName);
  }

  private bool GetStringReference(IRange range)
  {
    WorkbookImpl workbook = range.Worksheet.Workbook as WorkbookImpl;
    if (workbook.IsCreated || workbook.IsConverted)
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

  private bool IsStringValue(object[] rangeValues)
  {
    if (rangeValues != null)
    {
      for (int index = 0; index < rangeValues.Length; ++index)
      {
        if (rangeValues[index] is string)
          return true;
      }
    }
    return false;
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
    if (series.NumRefFormula != null)
      str = series.NumRefFormula;
    writer.WriteStartElement("numRef", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    if (series.InnerChart.IsAddCopied)
      writer.WriteElementString("f", "http://schemas.openxmlformats.org/drawingml/2006/chart", "[]" + str);
    else
      writer.WriteElementString("f", "http://schemas.openxmlformats.org/drawingml/2006/chart", str);
    if (rangeValues == null)
    {
      writer.WriteStartElement("numCache", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      if (tagName == "cat" || tagName == "xVal")
      {
        if (series.CategoriesFormatCode != null)
          writer.WriteElementString("formatCode", "http://schemas.openxmlformats.org/drawingml/2006/chart", series.CategoriesFormatCode);
        else
          writer.WriteElementString("formatCode", "http://schemas.openxmlformats.org/drawingml/2006/chart", series.ParentChart.PrimaryCategoryAxis.NumberFormat);
        this.SerializeCategoryTagCacheValues(writer, series, true);
      }
      else
      {
        if (series.FormatCode != null)
          writer.WriteElementString("formatCode", "http://schemas.openxmlformats.org/drawingml/2006/chart", series.FormatCode);
        else if (!(series.ValuesIRange is ExternalRange))
          writer.WriteElementString("formatCode", "http://schemas.openxmlformats.org/drawingml/2006/chart", series.ValuesIRange.NumberFormat);
        this.SerializeNumCacheValues(writer, series, tagName);
        writer.WriteEndElement();
      }
    }
    else
      this.SerializeDirectlyEntered(writer, rangeValues, series, tagName);
    writer.WriteEndElement();
  }

  private void SerializeNumCacheValues(XmlWriter writer, ChartSerieImpl series, string tagName)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    int key = 0;
    string s = string.Empty;
    object[] objArray;
    IRange range1;
    if (tagName == "bubbleSize")
    {
      objArray = series.EnteredDirectlyBubbles;
      range1 = series.BubblesIRange;
    }
    else
    {
      objArray = series.EnteredDirectlyValues;
      range1 = series.ValuesIRange;
    }
    if (objArray != null)
    {
      ChartSerializatorCommon.SerializeValueTag(writer, "ptCount", range1.Count.ToString());
      for (; key < objArray.Length; ++key)
      {
        if (series.EnteredDirectlyValues[key] != null)
        {
          string str = this.ObjectValueToString(objArray[key]);
          if (str.Length != 0 && !char.IsWhiteSpace(str.ToCharArray()[0]))
          {
            writer.WriteStartElement("pt", "http://schemas.openxmlformats.org/drawingml/2006/chart");
            writer.WriteAttributeString("idx", key.ToString());
            if (series.FormatValueCodes.Count > 0 && series.FormatValueCodes.ContainsKey(key))
              writer.WriteAttributeString("formatCode", series.FormatValueCodes[key]);
            writer.WriteElementString("v", "http://schemas.openxmlformats.org/drawingml/2006/chart", str);
            writer.WriteEndElement();
          }
        }
      }
    }
    else
    {
      IWorksheet worksheet = range1.Worksheet;
      ChartSerializatorCommon.SerializeValueTag(writer, "ptCount", range1.Count.ToString());
      for (; key < range1.Count; ++key)
      {
        if (!(range1 is ExternalRange))
        {
          IRange range2 = worksheet.Range[range1.Cells[key].AddressLocal];
          if (range2.HasDateTime)
            s = ChartSerializator.ToXmlString((object) range2.DateTime.ToOADate());
          else if (range2.HasFormulaDateTime)
            s = ChartSerializator.ToXmlString((object) range2.FormulaDateTime.ToOADate());
          else if (range2.HasNumber)
            s = this.ObjectValueToString(range2.Value2);
          else if (range2.HasFormulaNumberValue)
          {
            s = this.ObjectValueToString((object) range2.FormulaNumberValue);
          }
          else
          {
            s = this.ObjectValueToString(range2.Value2);
            if (!double.TryParse(s, out double _))
              s = "";
          }
        }
        if (s.Length != 0 && !char.IsWhiteSpace(s.ToCharArray()[0]))
        {
          writer.WriteStartElement("pt", "http://schemas.openxmlformats.org/drawingml/2006/chart");
          writer.WriteAttributeString("idx", key.ToString());
          if (series.FormatValueCodes.Count > 0 && series.FormatValueCodes.ContainsKey(key))
            writer.WriteAttributeString("formatCode", series.FormatValueCodes[key]);
          writer.WriteElementString("v", "http://schemas.openxmlformats.org/drawingml/2006/chart", s);
          writer.WriteEndElement();
        }
      }
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
    if (!hasSeriesName && series.StrRefFormula != null)
      range = series.StrRefFormula;
    writer.WriteStartElement("strRef", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteElementString("f", "http://schemas.openxmlformats.org/drawingml/2006/chart", range);
    writer.WriteStartElement("strCache", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    if (tagName == "cat" || tagName == "xVal")
      this.SerializeCategoryTagCacheValues(writer, series, false);
    else
      this.SerializeTextTagCacheValues(writer, series);
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
    if (!string.IsNullOrEmpty(series.Name))
      writer.WriteElementString("v", "http://schemas.openxmlformats.org/drawingml/2006/chart", series.Name);
    else
      writer.WriteElementString("v", "http://schemas.openxmlformats.org/drawingml/2006/chart", series.SerieName);
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private void SerializeCategoryTagCacheValues(
    XmlWriter writer,
    ChartSerieImpl series,
    bool Numberformat)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    int key = 0;
    ChartImpl parentChart = series.ParentChart;
    IRange categoryLabelsIrange = series.CategoryLabelsIRange;
    if (series.EnteredDirectlyCategoryLabels != null || parentChart.CategoryLabelValues != null)
    {
      if (series.EnteredDirectlyCategoryLabels == null)
      {
        ChartSerializatorCommon.SerializeValueTag(writer, "ptCount", categoryLabelsIrange.Count.ToString());
        for (; key < parentChart.CategoryLabelValues.Length; ++key)
        {
          string xmlString = ChartSerializator.ToXmlString(parentChart.CategoryLabelValues[key]);
          if (xmlString != "")
          {
            writer.WriteStartElement("pt", "http://schemas.openxmlformats.org/drawingml/2006/chart");
            writer.WriteAttributeString("idx", key.ToString());
            if (series.FormatCategoryCodes.Count > 0 && series.FormatCategoryCodes.ContainsKey(key))
              writer.WriteAttributeString("formatCode", series.FormatCategoryCodes[key]);
            writer.WriteElementString("v", "http://schemas.openxmlformats.org/drawingml/2006/chart", xmlString);
            writer.WriteEndElement();
          }
        }
      }
      else
      {
        ChartSerializatorCommon.SerializeValueTag(writer, "ptCount", categoryLabelsIrange.Count.ToString());
        for (; key < series.EnteredDirectlyCategoryLabels.Length; ++key)
        {
          string xmlString = ChartSerializator.ToXmlString(series.EnteredDirectlyCategoryLabels[key]);
          if (xmlString != "")
          {
            writer.WriteStartElement("pt", "http://schemas.openxmlformats.org/drawingml/2006/chart");
            writer.WriteAttributeString("idx", key.ToString());
            if (series.FormatCategoryCodes.Count > 0 && series.FormatCategoryCodes.ContainsKey(key))
              writer.WriteAttributeString("formatCode", series.FormatCategoryCodes[key]);
            writer.WriteElementString("v", "http://schemas.openxmlformats.org/drawingml/2006/chart", xmlString);
            writer.WriteEndElement();
          }
        }
      }
    }
    else
    {
      int count = categoryLabelsIrange.Count;
      int row = categoryLabelsIrange.Row;
      int column = categoryLabelsIrange.Column;
      IWorksheet worksheet = categoryLabelsIrange.Worksheet;
      ChartSerializatorCommon.SerializeValueTag(writer, "ptCount", count.ToString());
      bool hasDateTime = categoryLabelsIrange.HasDateTime;
      for (; key < count; ++key)
      {
        string str = !categoryLabelsIrange.Cells[key].HasDateTime || !hasDateTime || categoryLabelsIrange.Cells[key].NumberFormat == null || !categoryLabelsIrange.Cells[key].NumberFormat.ToLowerInvariant().EndsWith("y") ? (!categoryLabelsIrange.Cells[key].HasDateTime ? (!categoryLabelsIrange.Cells[key].HasFormulaDateTime ? (!worksheet.Range[categoryLabelsIrange.Cells[key].AddressLocal].HasFormulaStringValue ? this.ObjectValueToString(worksheet.Range[categoryLabelsIrange.Cells[key].AddressLocal].Value2) : this.ObjectValueToString((object) worksheet.Range[categoryLabelsIrange.Cells[key].AddressLocal].FormulaStringValue)) : categoryLabelsIrange.Cells[key].FormulaNumberValue.ToString()) : (!Numberformat ? ChartSerializator.ToXmlString((object) categoryLabelsIrange.Cells[key].DisplayText) : ChartSerializator.ToXmlString((object) categoryLabelsIrange.Cells[key].Number))) : ChartSerializator.ToXmlString((object) categoryLabelsIrange.Cells[key].Number);
        if (str != "")
        {
          writer.WriteStartElement("pt", "http://schemas.openxmlformats.org/drawingml/2006/chart");
          writer.WriteAttributeString("idx", key.ToString());
          if (series.FormatCategoryCodes.Count > 0 && series.FormatCategoryCodes.ContainsKey(key))
            writer.WriteAttributeString("formatCode", series.FormatCategoryCodes[key]);
          writer.WriteElementString("v", "http://schemas.openxmlformats.org/drawingml/2006/chart", str);
          writer.WriteEndElement();
        }
      }
    }
    writer.WriteEndElement();
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

  private void SerializeMultiLevelStringReference(
    XmlWriter writer,
    IRange range,
    object[] rangeValues,
    ChartSerieImpl series)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    string str = range is ICombinedRange combinedRange ? combinedRange.AddressGlobal2007 : range.AddressGlobal;
    if (series.MultiLevelStrCache.Count == 0 && rangeValues != null)
    {
      writer.WriteStartElement("numRef", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      this.SerializeDirectlyEntered(writer, rangeValues, true);
    }
    else
    {
      writer.WriteStartElement("multiLvlStrRef", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      writer.WriteElementString("f", "http://schemas.openxmlformats.org/drawingml/2006/chart", str);
      this.SerializeMultiLevelStringCache(writer, series, range);
    }
    writer.WriteEndElement();
  }

  private void SerializeMultiLevelStringCache(
    XmlWriter writer,
    ChartSerieImpl series,
    IRange range)
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
    else if (range != null)
    {
      int num1 = range.LastRow - range.Row;
      int num2 = 0;
      ChartSerializatorCommon.SerializeValueTag(writer, "ptCount", num1.ToString());
      bool hasDateTime = range.HasDateTime;
      for (int lastColumn = range.LastColumn; lastColumn >= range.Column; --lastColumn)
      {
        writer.WriteStartElement("lvl", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        for (int row = range.Row; row <= range.LastRow; ++row)
        {
          IRange range1 = range[row, lastColumn];
          string text = !range1.HasDateTime || !hasDateTime || range1.NumberFormat == null || !range1.NumberFormat.ToLowerInvariant().EndsWith("y") ? (!range1.HasDateTime ? (!range1.HasFormulaDateTime ? (!range1.HasFormulaStringValue ? this.ObjectValueToString(range1.Value2) : this.ObjectValueToString((object) range1.FormulaStringValue)) : range1.DisplayText) : ChartSerializator.ToXmlString((object) range1.Number)) : ChartSerializator.ToXmlString((object) range1.Number);
          if (text != "")
          {
            writer.WriteStartElement("pt", "http://schemas.openxmlformats.org/drawingml/2006/chart");
            writer.WriteAttributeString("idx", num2.ToString());
            writer.WriteStartElement("v", "http://schemas.openxmlformats.org/drawingml/2006/chart");
            writer.WriteString(text);
            writer.WriteEndElement();
            writer.WriteEndElement();
          }
          ++num2;
        }
        writer.WriteEndElement();
        num2 = 0;
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

  [SecurityCritical]
  private void SerializeAxes(XmlWriter writer, ChartImpl chart, RelationCollection relations)
  {
    ChartAxisSerializator axisSerializator = new ChartAxisSerializator();
    if (chart.IsCategoryAxisAvail && Array.IndexOf<int>(chart.SerializedAxisIds.ToArray(), (chart.PrimaryCategoryAxis as ChartAxisImpl).AxisId) >= 0)
      axisSerializator.SerializeAxis(writer, (IOfficeChartAxis) chart.PrimaryCategoryAxis, relations);
    if (chart.IsValueAxisAvail && Array.IndexOf<int>(chart.SerializedAxisIds.ToArray(), (chart.PrimaryValueAxis as ChartAxisImpl).AxisId) >= 0)
      axisSerializator.SerializeAxis(writer, (IOfficeChartAxis) chart.PrimaryValueAxis, relations);
    if (chart.IsSecondaryCategoryAxisAvail && Array.IndexOf<int>(chart.SerializedAxisIds.ToArray(), (chart.SecondaryCategoryAxis as ChartAxisImpl).AxisId) >= 0)
      axisSerializator.SerializeAxis(writer, (IOfficeChartAxis) chart.SecondaryCategoryAxis, relations);
    if (chart.IsSecondaryValueAxisAvail && Array.IndexOf<int>(chart.SerializedAxisIds.ToArray(), (chart.SecondaryValueAxis as ChartAxisImpl).AxisId) >= 0)
      axisSerializator.SerializeAxis(writer, (IOfficeChartAxis) chart.SecondaryValueAxis, relations);
    if (!chart.IsSeriesAxisAvail)
      return;
    axisSerializator.SerializeAxis(writer, (IOfficeChartAxis) chart.PrimarySerieAxis, relations);
  }

  [SecurityCritical]
  private void SerializePivotAxes(XmlWriter writer, ChartImpl chart, RelationCollection relations)
  {
    ChartAxisSerializator axisSerializator = new ChartAxisSerializator();
    if (chart.IsCategoryAxisAvail)
      axisSerializator.SerializeAxis(writer, (IOfficeChartAxis) chart.PrimaryCategoryAxis, relations);
    if (chart.IsValueAxisAvail)
      axisSerializator.SerializeAxis(writer, (IOfficeChartAxis) chart.PrimaryValueAxis, relations);
    if (!chart.IsPivotChart3D)
      return;
    axisSerializator.SerializeAxis(writer, (IOfficeChartAxis) chart.PrimarySerieAxis, relations);
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
            ChartSerializatorCommon.SerializeSolidFill(writer, (ChartColor) serieFormat.MarkerBackgroundColor, false, parentWorkbook, alphavalue);
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
      ChartSerializatorCommon.SerializeValueTag(writer, "symbol", "none");
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
      ChartSerializatorCommon.SerializeSolidFill(writer, (ChartColor) color, false, book, transparency);
    else
      writer.WriteElementString("noFill", "http://schemas.openxmlformats.org/drawingml/2006/main", string.Empty);
    writer.WriteEndElement();
  }

  [SecurityCritical]
  private void SerializeUpDownBars(XmlWriter writer, ChartImpl chart, ChartSerieImpl firstSeries)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    ChartFormatImpl commonSerieOptions = (ChartFormatImpl) firstSeries.SerieFormat.CommonSerieOptions;
    if (!commonSerieOptions.IsDropBar)
      return;
    IOfficeChartDropBar firstDropBar = commonSerieOptions.FirstDropBar;
    IOfficeChartDropBar secondDropBar = commonSerieOptions.SecondDropBar;
    writer.WriteStartElement("upDownBars", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    ChartSerializatorCommon.SerializeValueTag(writer, "gapWidth", firstDropBar.Gap.ToString());
    this.SerializeDropBar(writer, firstDropBar, "upBars", chart);
    this.SerializeDropBar(writer, secondDropBar, "downBars", chart);
    writer.WriteEndElement();
  }

  [SecurityCritical]
  private void SerializeDropBar(
    XmlWriter writer,
    IOfficeChartDropBar dropBar,
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
    ChartSerializatorCommon.SerializeFrameFormat(writer, (IOfficeChartFillBorder) dropBar, chart, false);
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
    if ((chart.ParentWorkbook.HasMacros || chart.HasCodeName) && codeName != null && codeName.Length > 0)
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
    Excel2007Serializator.SerializeVmlHFShapesWorksheetPart(writer, (WorksheetBaseImpl) chart, (IPageSetupConstantsProvider) new ChartPageSetupConstants(), (RelationCollection) null);
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
    writer.WriteAttributeString("x", XmlConvert.ToString(num1));
    writer.WriteAttributeString("y", XmlConvert.ToString(num2));
    writer.WriteEndElement();
    writer.WriteStartElement("ext", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    if (chart.Width <= 0.0)
      writer.WriteAttributeString("cx", 8666049.ToString());
    else
      writer.WriteAttributeString("cx", XmlConvert.ToString(chart.EMUWidth));
    if (chart.Height <= 0.0)
      writer.WriteAttributeString("cy", 6293304.ToString());
    else
      writer.WriteAttributeString("cy", XmlConvert.ToString(chart.EMUHeight));
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
    IOfficeChartDataTable dataTable = chart.DataTable;
    writer.WriteStartElement("dTable", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    ChartSerializatorCommon.SerializeBoolValueTag(writer, "showHorzBorder", dataTable.HasHorzBorder);
    ChartSerializatorCommon.SerializeBoolValueTag(writer, "showVertBorder", dataTable.HasVertBorder);
    ChartSerializatorCommon.SerializeBoolValueTag(writer, "showOutline", dataTable.HasBorders);
    ChartSerializatorCommon.SerializeBoolValueTag(writer, "showKeys", dataTable.ShowSeriesKeys);
    if ((dataTable as ChartDataTableImpl).HasShapeProperties)
      ShapeParser.WriteNodeFromStream(writer, (Stream) (dataTable as ChartDataTableImpl).shapeStream);
    if (((IInternalOfficeChartTextArea) dataTable.TextArea).ParagraphType == ChartParagraphType.CustomDefault)
    {
      WorkbookImpl parentWorkbook = ((dataTable as ChartDataTableImpl).Parent as ChartImpl).ParentWorkbook;
      this.SerializeDefaultTextFormatting(writer, dataTable.TextArea, (IWorkbook) parentWorkbook, 10.0);
    }
    writer.WriteEndElement();
  }

  private void SerializeDirectlyEntered(XmlWriter writer, object[] values, bool isCache)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (values == null)
      throw new ArgumentNullException(nameof (values));
    string localName = !isCache ? (values[0] is string ? "strLit" : "numLit") : (values[0] is string ? "strCache" : "numCache");
    writer.WriteStartElement(localName, "http://schemas.openxmlformats.org/drawingml/2006/chart");
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

  private void SerializeDirectlyEntered(
    XmlWriter writer,
    object[] values,
    ChartSerieImpl series,
    string tagname)
  {
    string localName = values[0] is string ? "strCache" : "numCache";
    writer.WriteStartElement(localName, "http://schemas.openxmlformats.org/drawingml/2006/chart");
    if (series.CategoriesFormatCode != null && tagname == "cat")
      writer.WriteElementString("formatCode", "http://schemas.openxmlformats.org/drawingml/2006/chart", series.CategoriesFormatCode);
    else
      writer.WriteElementString("formatCode", "http://schemas.openxmlformats.org/drawingml/2006/chart", series.FormatCode);
    ChartSerializatorCommon.SerializeValueTag(writer, "ptCount", values.Length.ToString());
    for (int index = 0; index < values.Length; ++index)
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
      case null:
        return "";
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

  private delegate void SerializeSeriesDelegate(XmlWriter writer, ChartSerieImpl series);
}
