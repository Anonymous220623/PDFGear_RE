// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes.ChartShapeSerializator
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Compression.Zip;
using Syncfusion.XlsIO.Implementation.Charts;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Implementation.XmlReaders.Shapes;
using Syncfusion.XlsIO.Implementation.XmlSerialization.Charts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes;

public class ChartShapeSerializator : DrawingShapeSerializator
{
  public const string ChartItemPath = "/xl/charts/chart{0}.xml";
  internal const string ChartExItemPath = "/xl/charts/chartEx{0}.xml";
  internal const string ColorsItemPath = "/xl/charts/colors{0}.xml";
  internal const string StyleItemPath = "/xl/charts/style{0}.xml";

  public override void Serialize(
    XmlWriter writer,
    ShapeImpl shape,
    WorksheetDataHolder holder,
    RelationCollection vmlRelations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (shape == null)
      throw new ArgumentNullException(nameof (shape));
    if (holder == null)
      throw new ArgumentNullException(nameof (holder));
    ChartShapeImpl chart = (ChartShapeImpl) shape;
    ChartImpl chartObject = chart.ChartObject;
    if (chartObject.Relations.Count != 0)
    {
      foreach (KeyValuePair<string, Relation> relation in chartObject.Relations)
      {
        if (relation.Value.Target.Contains("drawing"))
        {
          chartObject.Relations.Remove(relation.Key);
          break;
        }
      }
    }
    if (chartObject.DataHolder == null && holder != null)
      chartObject.DataHolder = holder;
    string chartFileName;
    string strRelationId = this.SerializeChartFile(holder, chartObject, out chartFileName);
    bool flag = !(shape.Worksheet is WorksheetImpl);
    if (!shape.IsAbsoluteAnchor && !flag)
    {
      writer.WriteStartElement("twoCellAnchor", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
      writer.WriteAttributeString("editAs", DrawingShapeSerializator.GetEditAsValue(shape));
      this.SerializeAnchorPoint(writer, "from", shape.LeftColumn, shape.LeftColumnOffset, shape.TopRow, shape.TopRowOffset, shape.Worksheet, "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
      this.SerializeAnchorPoint(writer, "to", shape.RightColumn, shape.RightColumnOffset, shape.BottomRow, shape.BottomRowOffset, shape.Worksheet, "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
      this.SerializeChartProperties(writer, chart, strRelationId, holder, false);
      writer.WriteEndElement();
    }
    else if (flag)
    {
      string localName = "relSizeAnchor";
      string str = "http://schemas.openxmlformats.org/drawingml/2006/chartDrawing";
      writer.WriteStartElement(localName, str);
      writer.WriteAttributeString("editAs", DrawingShapeSerializator.GetEditAsValue(shape));
      this.SerializeAnchorPoint(writer, "from", shape.LeftColumn, shape.LeftColumnOffset, shape.TopRow, shape.TopRowOffset, shape.Worksheet, str);
      this.SerializeAnchorPoint(writer, "to", shape.RightColumn, shape.RightColumnOffset, shape.BottomRow, shape.BottomRowOffset, shape.Worksheet, str);
      this.SerializeChartProperties(writer, chart, strRelationId, holder, true);
      writer.WriteEndElement();
    }
    else
      ChartSerializator.SerializeAbsoluteAnchorChart(writer, chartObject, strRelationId, false);
    holder.SerializeRelations(chartObject.Relations, chartFileName.Substring(1), holder, (WorksheetBaseImpl) chartObject);
  }

  internal string SerializeChartFile(
    WorksheetDataHolder holder,
    ChartImpl chart,
    out string chartFileName)
  {
    if (holder == null)
      throw new ArgumentNullException(nameof (holder));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    MemoryStream newDataStream = new MemoryStream();
    StreamWriter data = new StreamWriter((Stream) newDataStream);
    XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) data);
    bool flag = ChartImpl.IsChartExSerieType(chart.ChartType);
    string type = flag ? "http://schemas.microsoft.com/office/2014/relationships/chartEx" : "http://schemas.openxmlformats.org/officeDocument/2006/relationships/chart";
    string str = flag ? "application/vnd.ms-office.chartex+xml" : "application/vnd.openxmlformats-officedocument.drawingml.chart+xml";
    chartFileName = !flag ? ChartShapeSerializator.GetChartFileName(holder, chart) : ChartShapeSerializator.GetChartExFileName(holder, chart);
    FileDataHolder parentHolder = holder.ParentHolder;
    if (chart.DataHolder == null || chart.IsWorkSheetDataholder)
      parentHolder.CreateDataHolder((WorksheetBaseImpl) chart, chartFileName);
    if (flag)
      new ChartExSerializator().SerializeChartEx(writer, chart);
    else
      new ChartSerializator().SerializeChart(writer, chart, chartFileName);
    writer.Flush();
    data.Flush();
    string itemName = UtilityMethods.RemoveFirstCharUnsafe(chartFileName);
    parentHolder.ChartAndTableItemsToRemove.Add(itemName);
    parentHolder.Archive.UpdateItem(itemName, (Stream) newDataStream, true, FileAttributes.Archive);
    string relationId = holder.DrawingsRelations.GenerateRelationId();
    holder.DrawingsRelations[relationId] = new Relation(chartFileName, type);
    parentHolder.OverriddenContentTypes[chartFileName] = str;
    holder.ChartRelationsToRemove.Add(relationId);
    return relationId;
  }

  public static string GetChartFileName(WorksheetDataHolder holder, ChartImpl chart)
  {
    if (holder == null)
      throw new ArgumentNullException(nameof (holder));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    int num = ++holder.ParentHolder.LastChartIndex;
    string empty = string.Empty;
    string chartFileName;
    ZipArchive archive;
    string itemName;
    do
    {
      chartFileName = $"/xl/charts/chart{num}.xml";
      ++num;
      archive = holder.ParentHolder.Archive;
      itemName = chartFileName.TrimStart('/');
    }
    while (archive[itemName] != null);
    return chartFileName;
  }

  internal static string GetChartExFileName(WorksheetDataHolder holder, ChartImpl chart)
  {
    if (holder == null)
      throw new ArgumentNullException(nameof (holder));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    return $"/xl/charts/chartEx{++holder.ParentHolder.LastChartExIndex}.xml";
  }

  internal void SerializeChartProperties(
    XmlWriter writer,
    ChartShapeImpl chart,
    string strRelationId,
    WorksheetDataHolder holder,
    bool isGroupShape)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (strRelationId == null || strRelationId.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (strRelationId));
    bool flag = !(chart.Worksheet is WorksheetImpl);
    if (holder == null)
      throw new ArgumentNullException(nameof (holder));
    bool isChartEx = ChartImpl.IsChartExSerieType(chart.ChartType);
    if (isChartEx)
    {
      writer.WriteStartElement("mc", "AlternateContent", "http://schemas.openxmlformats.org/markup-compatibility/2006");
      writer.WriteAttributeString("xmlns", "mc", (string) null, "http://schemas.openxmlformats.org/markup-compatibility/2006");
      writer.WriteStartElement("mc", "Choice", "http://schemas.openxmlformats.org/markup-compatibility/2006");
      writer.WriteAttributeString("xmlns", "cx1", (string) null, "http://schemas.microsoft.com/office/drawing/2015/9/8/chartex");
      writer.WriteAttributeString("Requires", "cx1");
    }
    string str = !flag ? "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing" : "http://schemas.openxmlformats.org/drawingml/2006/chartDrawing";
    writer.WriteStartElement("graphicFrame", str);
    writer.WriteAttributeString("macro", chart.OnAction);
    this.SerializeNonVisualGraphicFrameProperties(writer, chart, holder);
    if (!isGroupShape || isChartEx)
      DrawingShapeSerializator.SerializeForm(writer, str, "http://schemas.openxmlformats.org/drawingml/2006/main", 0, 0, 0, 0);
    else
      DrawingShapeSerializator.SerializeForm(writer, str, "http://schemas.openxmlformats.org/drawingml/2006/main", (int) chart.ShapeFrame.OffsetX, (int) chart.ShapeFrame.OffsetY, (int) chart.ShapeFrame.OffsetCX, (int) chart.ShapeFrame.OffsetCY);
    this.SerializeGraphics(writer, chart, strRelationId, isChartEx);
    writer.WriteEndElement();
    if (!isGroupShape && !isChartEx)
      writer.WriteElementString("clientData", str, string.Empty);
    if (!isChartEx)
      return;
    writer.WriteEndElement();
    writer.WriteStartElement("mc", "Fallback", "http://schemas.openxmlformats.org/markup-compatibility/2006");
    holder.SerializeChartExFallBackShapeContent(writer, false);
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteElementString("clientData", str, string.Empty);
  }

  private void SerializeGraphics(
    XmlWriter writer,
    ChartShapeImpl chart,
    string strRelationId,
    bool isChartEx)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (strRelationId == null || strRelationId.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (strRelationId));
    writer.WriteStartElement("graphic", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteStartElement("graphicData", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (isChartEx)
    {
      writer.WriteAttributeString("uri", "http://schemas.microsoft.com/office/drawing/2014/chartex");
      writer.WriteStartElement("cx", nameof (chart), "http://schemas.microsoft.com/office/drawing/2014/chartex");
    }
    else
    {
      writer.WriteAttributeString("uri", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      writer.WriteStartElement("c", nameof (chart), "http://schemas.openxmlformats.org/drawingml/2006/chart");
    }
    writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", strRelationId);
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  internal void SerializeSlicerGraphics(XmlWriter writer, ChartShapeImpl shape)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (shape == null)
      throw new ArgumentNullException("chart");
    writer.WriteStartElement("graphic", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteStartElement("graphicData", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("uri", "http://schemas.microsoft.com/office/drawing/2010/slicer");
    shape.GraphicFrameStream.Position = 0L;
    ShapeParser.WriteNodeFromStream(writer, shape.GraphicFrameStream);
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  internal void SerializeNonVisualGraphicFrameProperties(
    XmlWriter writer,
    ChartShapeImpl chart,
    WorksheetDataHolder holder)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    string str = chart.Worksheet is WorksheetImpl ? "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing" : "http://schemas.openxmlformats.org/drawingml/2006/chartDrawing";
    writer.WriteStartElement("nvGraphicFramePr", str);
    this.SerializeNVCanvasProperties(writer, (ShapeImpl) chart, holder, str);
    writer.WriteElementString("cNvGraphicFramePr", str, string.Empty);
    writer.WriteEndElement();
  }
}
