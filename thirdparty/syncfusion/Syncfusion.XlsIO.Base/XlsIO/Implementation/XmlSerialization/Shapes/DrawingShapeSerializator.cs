// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes.DrawingShapeSerializator
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Implementation.XmlSerialization.Charts;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing;
using System;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes;

public class DrawingShapeSerializator : ShapeSerializator
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
    else if (shape.IsAbsoluteAnchor)
      writer.WriteStartElement("absoluteAnchor", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    else
      writer.WriteStartElement("twoCellAnchor", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    if (shape.IsAbsoluteAnchor)
    {
      writer.WriteStartElement("pos", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
      int num1 = (int) ApplicationImpl.ConvertFromPixel((double) shape.Left, MeasureUnits.EMU);
      writer.WriteAttributeString("x", num1.ToString());
      int num2 = (int) ApplicationImpl.ConvertFromPixel((double) shape.Top, MeasureUnits.EMU);
      writer.WriteAttributeString("y", num2.ToString());
      writer.WriteEndElement();
      writer.WriteStartElement("ext", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
      int num3 = (int) ApplicationImpl.ConvertFromPixel((double) shape.Width, MeasureUnits.EMU);
      writer.WriteAttributeString("cx", num3.ToString());
      num3 = (int) ApplicationImpl.ConvertFromPixel((double) shape.Height, MeasureUnits.EMU);
      writer.WriteAttributeString("cy", num3.ToString());
      writer.WriteEndElement();
    }
    else
      this.SerializeAnchorPoint(writer, "from", shape.LeftColumn, shape.LeftColumnOffset, shape.TopRow, shape.TopRowOffset, shape.Worksheet, "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    if (clientAnchor.OneCellAnchor)
    {
      writer.WriteStartElement("ext", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
      int num4 = (int) ApplicationImpl.ConvertFromPixel((double) shape.Width, MeasureUnits.EMU);
      writer.WriteAttributeString("cx", num4.ToString());
      int num5 = (int) ApplicationImpl.ConvertFromPixel((double) shape.Height, MeasureUnits.EMU);
      writer.WriteAttributeString("cy", num5.ToString());
      writer.WriteEndElement();
    }
    else if (!shape.IsAbsoluteAnchor)
      this.SerializeAnchorPoint(writer, "to", shape.RightColumn, shape.RightColumnOffset, shape.BottomRow, shape.BottomRowOffset, shape.Worksheet, "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    this.SerializeXmlDataStream(writer, xmlDataStream);
    if (!shape.EnableAlternateContent)
      return;
    Excel2007Serializator.WriteAlternateContentFooter(writer);
  }

  internal void SerializeXmlDataStream(XmlWriter writer, Stream dataStream)
  {
    dataStream.Position = 0L;
    XmlReader reader = UtilityMethods.CreateReader(dataStream);
    writer.WriteNode(reader, false);
    writer.WriteElementString("clientData", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing", string.Empty);
    writer.WriteEndElement();
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
      this.SerializeColRowAnchor(writer, tagName, column, columnOffset, row, rowOffset, sheet, drawingsNamespace, true);
    else
      this.SerializeXYAnchor(writer, tagName, column, row, drawingsNamespace);
  }

  internal void SerializeColRowAnchor(
    XmlWriter writer,
    string tagName,
    int column,
    int columnOffset,
    int row,
    int rowOffset,
    WorksheetBaseImpl sheet,
    string drawingsNamespace,
    bool isNameSpace)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (tagName == null || tagName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (tagName));
    WorksheetImpl worksheetImpl = sheet as WorksheetImpl;
    double num1 = worksheetImpl != null ? (double) worksheetImpl.GetColumnWidthInPixels(column) : 1.0;
    if (num1 == 0.0)
      num1 = (double) worksheetImpl.GetHiddenColumnWidthInPixels(column);
    double num2 = Math.Round(ApplicationImpl.ConvertFromPixel(num1 * (double) columnOffset / 1024.0, MeasureUnits.EMU));
    --column;
    double num3 = worksheetImpl != null ? (double) worksheetImpl.GetRowHeightInPixels(row) : 1.0;
    if (num3 == 0.0)
      num3 = (double) worksheetImpl.GetHiddenRowHeightInPixels(row);
    double num4 = (double) (int) ApplicationImpl.ConvertFromPixel(Math.Round(num3 * (double) rowOffset / 256.0, 1), MeasureUnits.EMU);
    --row;
    if (isNameSpace)
      writer.WriteStartElement(tagName, drawingsNamespace);
    else
      writer.WriteStartElement(tagName);
    writer.WriteElementString("col", drawingsNamespace, column.ToString());
    writer.WriteElementString("colOff", drawingsNamespace, ((int) num2).ToString());
    writer.WriteElementString(nameof (row), drawingsNamespace, row.ToString());
    writer.WriteElementString("rowOff", drawingsNamespace, ((int) num4).ToString());
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
    if (shape is TextBoxShapeImpl && (shape as TextBoxShapeImpl).FlipVertical)
      writer.WriteAttributeString("flipV", "1");
    if (shape is TextBoxShapeImpl && (shape as TextBoxShapeImpl).FlipHorizontal)
      writer.WriteAttributeString("flipH", "1");
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

  internal static void SerializeForm(
    XmlWriter writer,
    string xmlOuterNamespace,
    string xmlInnerNamespace,
    BitmapShapeImpl picture)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartElement("xfrm", xmlOuterNamespace);
    if (picture.ShapeRotation != 0)
      writer.WriteAttributeString("rot", (picture.ShapeRotation * 60000).ToString());
    if (picture.FlipHorizontal)
      writer.WriteAttributeString("flipH", "1");
    if (picture.FlipVertical)
      writer.WriteAttributeString("flipV", "1");
    double offsetX;
    double offsetY;
    double num1;
    double num2;
    if (picture.Group != null)
    {
      offsetX = (double) picture.ShapeFrame.OffsetX;
      offsetY = (double) picture.ShapeFrame.OffsetY;
      num1 = (double) picture.ShapeFrame.OffsetCX;
      num2 = (double) picture.ShapeFrame.OffsetCY;
    }
    else
    {
      offsetX = (double) picture.OffsetX;
      offsetY = (double) picture.OffsetY;
      num1 = (double) picture.ExtentsX;
      num2 = (double) picture.ExtentsX;
    }
    writer.WriteStartElement("off", xmlInnerNamespace);
    writer.WriteAttributeString("x", offsetX.ToString());
    writer.WriteAttributeString("y", offsetY.ToString());
    writer.WriteEndElement();
    writer.WriteStartElement("ext", xmlInnerNamespace);
    writer.WriteAttributeString("cx", num1.ToString());
    writer.WriteAttributeString("cy", num2.ToString());
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
      IHyperLink hyperlink = shape.Hyperlink;
      string target = hyperlink.Address;
      if (hyperlink.Type == ExcelHyperLinkType.File && !target.StartsWith("..") && target.Contains(":\\") && !target.StartsWith("file:///") || hyperlink.Type == ExcelHyperLinkType.Unc)
        target = "file:///" + target;
      if (hyperlink.Type != ExcelHyperLinkType.Workbook)
        target = target?.Replace(" ", "%20");
      bool isExternal = hyperlink.Type != ExcelHyperLinkType.Workbook;
      if (!isExternal)
        target = target.StartsWith("#") ? target : "#" + target;
      if (target != null)
      {
        Relation relation = new Relation(target, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/hyperlink", isExternal);
        string str = holder.DrawingsRelations.Add(relation);
        writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", str);
      }
      else
        writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", string.Empty);
      if (!string.IsNullOrEmpty(hyperlink.ScreenTip))
        writer.WriteAttributeString("tooltip", hyperlink.ScreenTip);
      writer.WriteEndElement();
    }
    if (shape.NvPrExtLstStream != null)
      Excel2007Serializator.SerializeStream(writer, shape.NvPrExtLstStream, "root");
    writer.WriteEndElement();
  }

  private void SerializeNonVisualDrawingShapeProps(XmlWriter writer, string drawingsNamespace)
  {
    writer.WriteStartElement("xdr", "cNvSpPr", drawingsNamespace);
    writer.WriteEndElement();
  }

  protected void SerializePresetGeometry(XmlWriter writer, string type)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartElement("prstGeom", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("prst", !string.IsNullOrEmpty(type) ? type : "rect");
    writer.WriteStartElement("avLst", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  internal void SerializeCustomGeometry(XmlWriter writer, ShapeImpl shape)
  {
    Stream data;
    if (!shape.PreservedElements.TryGetValue("custGeom", out data) || data == null || data.Length <= 0L)
      return;
    Excel2007Serializator.SerializeStream(writer, data, "root");
  }

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
    else
    {
      writer.WriteStartElement("noFill", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteEndElement();
    }
    if (shape.HasLineFormat && !shape.IsGroup)
    {
      IShapeLineFormat line = shape.Line;
      DrawingShapeSerializator.SerializeLineSettings(writer, line, (IWorkbook) holder.Workbook);
    }
    else
    {
      if (shape.IsGroup)
        return;
      writer.WriteStartElement("ln", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteElementString("noFill", "http://schemas.openxmlformats.org/drawingml/2006/main", string.Empty);
      writer.WriteEndElement();
    }
  }

  internal static void SerializeLineSettings(
    XmlWriter writer,
    IShapeLineFormat line,
    IWorkbook book)
  {
    writer.WriteStartElement("ln", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if ((line as ShapeLineFormatImpl).IsWidthExist)
      writer.WriteAttributeString("w", (line.Weight * 12700.0).ToString());
    Excel2007ShapeLineStyle style = (Excel2007ShapeLineStyle) line.Style;
    writer.WriteAttributeString("cmpd", style.ToString());
    if (line.Weight >= 0.0 && line.Visible)
      ChartSerializatorCommon.SerializeSolidFill(writer, (ColorObject) line.ForeColor, false, book, 1.0 - line.Transparency, (ShapeLineFormatImpl) line);
    else
      writer.WriteElementString("noFill", "http://schemas.openxmlformats.org/drawingml/2006/main", string.Empty);
    ShapeLineFormatImpl shapeLineFormatImpl = line as ShapeLineFormatImpl;
    if (line.DashStyle != ExcelShapeDashLineStyle.Solid)
    {
      string str = (book.Application as ApplicationImpl).StringEnum.LineDashTypeEnumToXml[line.DashStyle];
      if (str != null && str.Length != 0)
        ChartSerializatorCommon.SerializeValueTag(writer, "prstDash", "http://schemas.openxmlformats.org/drawingml/2006/main", str);
    }
    if (shapeLineFormatImpl.HasBorderJoin)
      DrawingShapeSerializator.SerializeBorderJoinType(writer, shapeLineFormatImpl);
    DrawingShapeSerializator.SerializeArrowProperties(writer, shapeLineFormatImpl, true);
    DrawingShapeSerializator.SerializeArrowProperties(writer, shapeLineFormatImpl, false);
    writer.WriteEndElement();
  }

  private static void SerializeBorderJoinType(XmlWriter writer, ShapeLineFormatImpl lineFormat)
  {
    switch (lineFormat.JoinType)
    {
      case Excel2007BorderJoinType.Round:
        writer.WriteElementString("round", "http://schemas.openxmlformats.org/drawingml/2006/main", string.Empty);
        break;
      case Excel2007BorderJoinType.Bevel:
        writer.WriteElementString("bevel", "http://schemas.openxmlformats.org/drawingml/2006/main", string.Empty);
        break;
      case Excel2007BorderJoinType.Mitter:
        writer.WriteStartElement("miter", "http://schemas.openxmlformats.org/drawingml/2006/main");
        if (lineFormat.MiterLim != 0)
          writer.WriteAttributeString("lim", lineFormat.MiterLim.ToString());
        writer.WriteEndElement();
        break;
    }
  }

  private static void SerializeArrowProperties(
    XmlWriter writer,
    ShapeLineFormatImpl line,
    bool isHead)
  {
    string localName;
    ExcelShapeArrowStyle excelShapeArrowStyle;
    ExcelShapeArrowWidth excelShapeArrowWidth;
    ExcelShapeArrowLength shapeArrowLength;
    if (isHead)
    {
      localName = "headEnd";
      excelShapeArrowStyle = line.BeginArrowHeadStyle;
      excelShapeArrowWidth = line.BeginArrowheadWidth;
      shapeArrowLength = line.BeginArrowheadLength;
    }
    else
    {
      localName = "tailEnd";
      excelShapeArrowStyle = line.EndArrowHeadStyle;
      excelShapeArrowWidth = line.EndArrowheadWidth;
      shapeArrowLength = line.EndArrowheadLength;
    }
    writer.WriteStartElement(localName, "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("type", DrawingShapeSerializator.GetArrowStyle(excelShapeArrowStyle));
    writer.WriteAttributeString("w", DrawingShapeSerializator.GetArrowWidth(excelShapeArrowWidth));
    writer.WriteAttributeString("len", DrawingShapeSerializator.GetArrowLength(shapeArrowLength));
    writer.WriteEndElement();
  }

  private static string GetArrowLength(ExcelShapeArrowLength obj4)
  {
    switch (obj4)
    {
      case ExcelShapeArrowLength.ArrowHeadShort:
        return "sm";
      case ExcelShapeArrowLength.ArrowHeadMedium:
        return "med";
      case ExcelShapeArrowLength.ArrowHeadLong:
        return "lg";
      default:
        return "med";
    }
  }

  private static string GetArrowWidth(ExcelShapeArrowWidth obj3)
  {
    switch (obj3)
    {
      case ExcelShapeArrowWidth.ArrowHeadNarrow:
        return "sm";
      case ExcelShapeArrowWidth.ArrowHeadMedium:
        return "med";
      case ExcelShapeArrowWidth.ArrowHeadWide:
        return "lg";
      default:
        return "med";
    }
  }

  private static string GetArrowStyle(ExcelShapeArrowStyle obj2)
  {
    switch (obj2)
    {
      case ExcelShapeArrowStyle.LineNoArrow:
        return "none";
      case ExcelShapeArrowStyle.LineArrow:
        return "triangle";
      case ExcelShapeArrowStyle.LineArrowStealth:
        return "stealth";
      case ExcelShapeArrowStyle.LineArrowDiamond:
        return "diamond";
      case ExcelShapeArrowStyle.LineArrowOval:
        return "oval";
      case ExcelShapeArrowStyle.LineArrowOpen:
        return "arrow";
      default:
        return "none";
    }
  }

  internal void SerializeChildShape(
    XmlWriter writer,
    ShapeImpl shape,
    WorksheetDataHolder holder,
    RelationCollection relations,
    string drawingsNamespace,
    string prefix)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (shape == null)
      throw new ArgumentNullException("textBox");
    writer.WriteStartElement(prefix, "sp", drawingsNamespace);
    writer.WriteStartElement(prefix, "nvSpPr", drawingsNamespace);
    this.SerializeNVCanvasProperties(writer, shape, holder, drawingsNamespace);
    this.SerializeNonVisualDrawingShapeProps(writer, drawingsNamespace);
    writer.WriteEndElement();
    this.SerializeShapeProperties(writer, shape, holder, relations, drawingsNamespace);
    if (shape is TextBoxShapeBase)
      TextBoxSerializator.SerializeRichText(writer, drawingsNamespace, shape as TextBoxShapeBase);
    writer.WriteEndElement();
  }

  internal void SerializeShapeProperties(
    XmlWriter writer,
    ShapeImpl shape,
    WorksheetDataHolder holder,
    RelationCollection relations,
    string drawingsNamespace)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (shape == null)
      throw new ArgumentNullException("textBox");
    writer.WriteStartElement("spPr", drawingsNamespace);
    int offsetX = (int) shape.ShapeFrame.OffsetX;
    int offsetY = (int) shape.ShapeFrame.OffsetY;
    int offsetCx = (int) shape.ShapeFrame.OffsetCX;
    int offsetCy = (int) shape.ShapeFrame.OffsetCY;
    DrawingShapeSerializator.SerializeForm(writer, "http://schemas.openxmlformats.org/drawingml/2006/main", "http://schemas.openxmlformats.org/drawingml/2006/main", offsetX, offsetY, offsetCx, offsetCy, (IShape) shape);
    if (shape.IsCustomGeometry)
      this.SerializeCustomGeometry(writer, shape);
    else
      this.SerializePresetGeometry(writer, shape.PresetGeometry);
    Stream data;
    if (shape.PreservedElements.TryGetValue("solidFill", out data))
    {
      if (data != null && data.Length > 0L)
        Excel2007Serializator.SerializeStream(writer, data, "root");
    }
    else if (shape.HasFill)
      this.SerializeFill(writer, shape, holder, relations);
    else if (shape.IsCustomGeometry)
    {
      writer.WriteStartElement("grpFill", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }
}
