// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.XmlSerialization.Shapes.DrawingShapeSerializator
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Shapes;
using Syncfusion.OfficeChart.Implementation.XmlSerialization.Charts;
using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing;
using System;
using System.IO;
using System.Security;
using System.Xml;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.XmlSerialization.Shapes;

internal class DrawingShapeSerializator : ShapeSerializator
{
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
    Stream xmlDataStream = shape.XmlDataStream;
    if (xmlDataStream == null || xmlDataStream.Length <= 0L)
      return;
    MsofbtClientAnchor clientAnchor = shape.ClientAnchor;
    if (shape.EnableAlternateContent)
      Excel2007Serializator.WriteAlternateContentHeader(writer);
    if (clientAnchor.OneCellAnchor)
      writer.WriteStartElement("oneCellAnchor", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    else
      writer.WriteStartElement("twoCellAnchor", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    this.SerializeAnchorPoint(writer, "from", shape.LeftColumn, shape.LeftColumnOffset, shape.TopRow, shape.TopRowOffset, shape.Worksheet, "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    if (clientAnchor.OneCellAnchor)
    {
      writer.WriteStartElement("ext", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
      int num1 = (int) ApplicationImpl.ConvertFromPixel((double) shape.Width, MeasureUnits.EMU);
      writer.WriteAttributeString("cx", num1.ToString());
      int num2 = (int) ApplicationImpl.ConvertFromPixel((double) shape.Height, MeasureUnits.EMU);
      writer.WriteAttributeString("cy", num2.ToString());
      writer.WriteEndElement();
    }
    else
      this.SerializeAnchorPoint(writer, "to", shape.RightColumn, shape.RightColumnOffset, shape.BottomRow, shape.BottomRowOffset, shape.Worksheet, "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    xmlDataStream.Position = 0L;
    XmlReader reader = UtilityMethods.CreateReader(xmlDataStream);
    writer.WriteNode(reader, false);
    writer.WriteElementString("clientData", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing", string.Empty);
    writer.WriteEndElement();
    if (!shape.EnableAlternateContent)
      return;
    Excel2007Serializator.WriteAlternateContentFooter(writer);
  }

  public override void SerializeShapeType(XmlWriter writer, Type shapeType)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public static string GetEditAsValue(ShapeImpl shape)
  {
    if (shape == null)
      throw new ArgumentNullException(nameof (shape));
    return !shape.IsMoveWithCell ? "absolute" : (!shape.IsSizeWithCell ? "oneCell" : "twoCell");
  }

  internal void SerializeAnchorPoint(
    XmlWriter writer,
    string tagName,
    int column,
    int columnOffset,
    int row,
    int rowOffset,
    WorksheetBaseImpl sheet,
    string drawingsNamespace)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (tagName == null || tagName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (tagName));
    if (sheet is WorksheetImpl)
      this.SerializeColRowAnchor(writer, tagName, column, columnOffset, row, rowOffset, sheet, drawingsNamespace);
    else
      this.SerializeXYAnchor(writer, tagName, column, row, drawingsNamespace);
  }

  private void SerializeColRowAnchor(
    XmlWriter writer,
    string tagName,
    int column,
    int columnOffset,
    int row,
    int rowOffset,
    WorksheetBaseImpl sheet,
    string drawingsNamespace)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (tagName == null || tagName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (tagName));
    WorksheetImpl worksheetImpl = sheet as WorksheetImpl;
    double num1 = Math.Round(ApplicationImpl.ConvertFromPixel((worksheetImpl != null ? (double) worksheetImpl.GetColumnWidthInPixels(column) : 1.0) * (double) columnOffset / 1024.0, MeasureUnits.EMU));
    --column;
    double num2 = (double) (int) ApplicationImpl.ConvertFromPixel(Math.Round((worksheetImpl != null ? (double) worksheetImpl.GetRowHeightInPixels(row) : 1.0) * (double) rowOffset / 256.0, 1), MeasureUnits.EMU);
    --row;
    writer.WriteStartElement(tagName, drawingsNamespace);
    writer.WriteElementString("col", drawingsNamespace, column.ToString());
    writer.WriteElementString("colOff", drawingsNamespace, ((int) num1).ToString());
    writer.WriteElementString(nameof (row), drawingsNamespace, row.ToString());
    writer.WriteElementString("rowOff", drawingsNamespace, ((int) num2).ToString());
    writer.WriteEndElement();
  }

  private void SerializeXYAnchor(
    XmlWriter writer,
    string tagName,
    int column,
    int row,
    string drawingsNamespace)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (tagName == null || tagName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (tagName));
    writer.WriteStartElement(tagName, drawingsNamespace);
    if (column > 1000)
      column = 1000;
    string coordinateValue1 = this.GetCoordinateValue(column);
    writer.WriteElementString("x", drawingsNamespace, coordinateValue1);
    if (row > 1000)
      row = 1000;
    string coordinateValue2 = this.GetCoordinateValue(row);
    writer.WriteElementString("y", drawingsNamespace, coordinateValue2);
    writer.WriteEndElement();
  }

  private string GetCoordinateValue(int coordinate)
  {
    return XmlConvert.ToString((double) coordinate / 1000.0);
  }

  public static void SerializeForm(
    XmlWriter writer,
    string xmlOuterNamespace,
    string xmlInnerNamespace,
    int x,
    int y,
    int cx,
    int cy)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartElement("xfrm", xmlOuterNamespace);
    writer.WriteStartElement("off", xmlInnerNamespace);
    writer.WriteAttributeString(nameof (x), x.ToString());
    writer.WriteAttributeString(nameof (y), y.ToString());
    writer.WriteEndElement();
    writer.WriteStartElement("ext", xmlInnerNamespace);
    writer.WriteAttributeString(nameof (cx), cx.ToString());
    writer.WriteAttributeString(nameof (cy), cy.ToString());
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  public static void SerializeForm(
    XmlWriter writer,
    string xmlOuterNamespace,
    string xmlInnerNamespace,
    int x,
    int y,
    int cx,
    int cy,
    IShape shape)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartElement("xfrm", xmlOuterNamespace);
    if (shape.ShapeRotation != 0)
      writer.WriteAttributeString("rot", (shape.ShapeRotation * 60000).ToString());
    writer.WriteStartElement("off", xmlInnerNamespace);
    writer.WriteAttributeString(nameof (x), x.ToString());
    writer.WriteAttributeString(nameof (y), y.ToString());
    writer.WriteEndElement();
    writer.WriteStartElement("ext", xmlInnerNamespace);
    writer.WriteAttributeString(nameof (cx), cx.ToString());
    writer.WriteAttributeString(nameof (cy), cy.ToString());
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  protected void SerializeNVCanvasProperties(
    XmlWriter writer,
    ShapeImpl shape,
    WorksheetDataHolder holder,
    string drawingsNamespace)
  {
    writer.WriteStartElement("cNvPr", drawingsNamespace);
    int shapeId = shape.ShapeId;
    writer.WriteAttributeString("id", shapeId.ToString());
    string name = shape.Name;
    writer.WriteAttributeString("name", name);
    string alternativeText = shape.AlternativeText;
    if (alternativeText != null && alternativeText.Length > 0)
      writer.WriteAttributeString("descr", alternativeText);
    if (!shape.IsShapeVisible)
      writer.WriteAttributeString("hidden", "1");
    if (shape.IsHyperlink)
    {
      writer.WriteStartElement("hlinkClick", "http://schemas.openxmlformats.org/drawingml/2006/main");
      string str = holder.DrawingsRelations.Add(shape.ImageRelation);
      writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", str);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  protected void SerializePresetGeometry(XmlWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartElement("prstGeom", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("prst", "rect");
    writer.WriteStartElement("avLst", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  [SecurityCritical]
  protected void SerializeFill(
    XmlWriter writer,
    ShapeImpl shape,
    FileDataHolder holder,
    RelationCollection relations)
  {
    if (shape.HasFill)
    {
      IInternalFill fill = (IInternalFill) shape.Fill;
      if (fill.Visible)
      {
        ChartSerializatorCommon.SerializeFill(writer, fill, holder, relations);
      }
      else
      {
        writer.WriteStartElement("noFill", "http://schemas.openxmlformats.org/drawingml/2006/main");
        writer.WriteEndElement();
      }
    }
    if (!shape.HasLineFormat)
      return;
    IShapeLineFormat line = shape.Line;
    DrawingShapeSerializator.SerializeLineSettings(writer, line, (IWorkbook) holder.Workbook);
  }

  internal static void SerializeLineSettings(
    XmlWriter writer,
    IShapeLineFormat line,
    IWorkbook book)
  {
    writer.WriteStartElement("ln", "http://schemas.openxmlformats.org/drawingml/2006/main");
    int num = (int) (line.Weight * 12700.0);
    writer.WriteAttributeString("w", num.ToString());
    Excel2007ShapeLineStyle style = (Excel2007ShapeLineStyle) line.Style;
    writer.WriteAttributeString("cmpd", style.ToString());
    if (line.Weight >= 0.0 && line.Visible)
      ChartSerializatorCommon.SerializeSolidFill(writer, (ChartColor) line.ForeColor, false, book, 1.0 - line.Transparency);
    else
      writer.WriteElementString("noFill", "http://schemas.openxmlformats.org/drawingml/2006/main", string.Empty);
    ShapeLineFormatImpl line1 = line as ShapeLineFormatImpl;
    if (line1.IsRound)
      writer.WriteElementString("round", "http://schemas.openxmlformats.org/drawingml/2006/main", string.Empty);
    if (line.DashStyle != OfficeShapeDashLineStyle.Solid)
    {
      string str = ((book as WorksheetImpl).Application as ApplicationImpl).StringEnum.LineDashTypeEnumToXml[line.DashStyle];
      if (str != null && str.Length != 0)
        ChartSerializatorCommon.SerializeValueTag(writer, "prstDash", "http://schemas.openxmlformats.org/drawingml/2006/main", str);
    }
    DrawingShapeSerializator.SerializeArrowProperties(writer, line1, true);
    DrawingShapeSerializator.SerializeArrowProperties(writer, line1, false);
    writer.WriteEndElement();
  }

  private static void SerializeArrowProperties(
    XmlWriter writer,
    ShapeLineFormatImpl line,
    bool isHead)
  {
    string localName;
    OfficeShapeArrowStyle officeShapeArrowStyle;
    OfficeShapeArrowWidth officeShapeArrowWidth;
    OfficeShapeArrowLength shapeArrowLength;
    if (isHead)
    {
      localName = "headEnd";
      officeShapeArrowStyle = line.BeginArrowHeadStyle;
      officeShapeArrowWidth = line.BeginArrowheadWidth;
      shapeArrowLength = line.BeginArrowheadLength;
    }
    else
    {
      localName = "tailEnd";
      officeShapeArrowStyle = line.EndArrowHeadStyle;
      officeShapeArrowWidth = line.EndArrowheadWidth;
      shapeArrowLength = line.EndArrowheadLength;
    }
    writer.WriteStartElement(localName, "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("type", DrawingShapeSerializator.GetArrowStyle(officeShapeArrowStyle));
    writer.WriteAttributeString("w", DrawingShapeSerializator.GetArrowWidth(officeShapeArrowWidth));
    writer.WriteAttributeString("len", DrawingShapeSerializator.GetArrowLength(shapeArrowLength));
    writer.WriteEndElement();
  }

  private static string GetArrowLength(OfficeShapeArrowLength obj4)
  {
    switch (obj4)
    {
      case OfficeShapeArrowLength.ArrowHeadShort:
        return "sm";
      case OfficeShapeArrowLength.ArrowHeadMedium:
        return "med";
      case OfficeShapeArrowLength.ArrowHeadLong:
        return "lg";
      default:
        return "med";
    }
  }

  private static string GetArrowWidth(OfficeShapeArrowWidth obj3)
  {
    switch (obj3)
    {
      case OfficeShapeArrowWidth.ArrowHeadNarrow:
        return "sm";
      case OfficeShapeArrowWidth.ArrowHeadMedium:
        return "med";
      case OfficeShapeArrowWidth.ArrowHeadWide:
        return "lg";
      default:
        return "med";
    }
  }

  private static string GetArrowStyle(OfficeShapeArrowStyle obj2)
  {
    switch (obj2)
    {
      case OfficeShapeArrowStyle.LineNoArrow:
        return "none";
      case OfficeShapeArrowStyle.LineArrow:
        return "triangle";
      case OfficeShapeArrowStyle.LineArrowStealth:
        return "stealth";
      case OfficeShapeArrowStyle.LineArrowDiamond:
        return "diamond";
      case OfficeShapeArrowStyle.LineArrowOval:
        return "oval";
      case OfficeShapeArrowStyle.LineArrowOpen:
        return "arrow";
      default:
        return "none";
    }
  }
}
