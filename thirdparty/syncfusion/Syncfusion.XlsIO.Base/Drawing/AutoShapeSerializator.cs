// Decompiled with JetBrains decompiler
// Type: Syncfusion.Drawing.AutoShapeSerializator
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO;
using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
using System;
using System.Xml;

#nullable disable
namespace Syncfusion.Drawing;

internal class AutoShapeSerializator
{
  private AnchorType anchorType;
  private string attribute;
  private string chartNameSpace = "http://schemas.openxmlformats.org/drawingml/2006/chartDrawing";
  private AutoShapeImpl parentShape;
  private ShapeImplExt shape;
  private int resolution;
  private string nameSpace;

  internal AutoShapeSerializator(ShapeImplExt shape)
  {
    this.shape = shape;
    this.attribute = this.shape.ParentSheet is WorksheetImpl ? "xdr" : "cdr";
    this.nameSpace = this.shape.ParentSheet is WorksheetImpl ? "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing" : "http://schemas.openxmlformats.org/drawingml/2006/chartDrawing";
    this.anchorType = shape.AnchorType;
    if (shape.Worksheet != null)
      this.resolution = shape.Worksheet.AppImplementation.GetdpiX();
    else if (shape.ParentSheet != null)
      this.resolution = shape.ParentSheet.AppImplementation.GetdpiX();
    else
      this.resolution = 96 /*0x60*/;
  }

  internal AutoShapeSerializator(AutoShapeImpl parentShape)
  {
    this.parentShape = parentShape;
    this.shape = parentShape.ShapeExt;
    this.attribute = this.parentShape.Worksheet is WorksheetImpl ? "xdr" : "cdr";
    this.nameSpace = this.parentShape.Worksheet is WorksheetImpl ? "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing" : "http://schemas.openxmlformats.org/drawingml/2006/chartDrawing";
    this.anchorType = this.shape.AnchorType;
    if (this.shape.Worksheet != null)
      this.resolution = this.shape.Worksheet.AppImplementation.GetdpiX();
    else if (this.shape.ParentSheet != null)
      this.resolution = this.shape.ParentSheet.AppImplementation.GetdpiX();
    else
      this.resolution = 96 /*0x60*/;
  }

  private void SerializeAbsoluteAnchor(XmlWriter xmlTextWriter)
  {
    int offsetValue1 = this.parentShape.Width;
    int offsetValue2 = this.parentShape.Height;
    if (offsetValue1 < 911)
    {
      offsetValue1 = 900;
      offsetValue2 = 600;
    }
    int emu1 = Helper.ConvertOffsetToEMU(offsetValue1, this.resolution);
    int emu2 = Helper.ConvertOffsetToEMU(offsetValue2, this.resolution);
    xmlTextWriter.WriteStartElement(this.attribute, "pos", this.nameSpace);
    xmlTextWriter.WriteAttributeString("x", "0");
    xmlTextWriter.WriteAttributeString("y", "0");
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteStartElement(this.attribute, "ext", this.nameSpace);
    xmlTextWriter.WriteAttributeString("cx", Helper.ToString(emu1));
    xmlTextWriter.WriteAttributeString("cy", Helper.ToString(emu2));
    xmlTextWriter.WriteEndElement();
  }

  private void SerializeAnchorPosition(XmlWriter xmlTextWriter)
  {
    switch (this.anchorType)
    {
      case AnchorType.Absolute:
        this.SerializeAbsoluteAnchor(xmlTextWriter);
        break;
      case AnchorType.RelSize:
        this.SerializeRelSizeAnchor(xmlTextWriter);
        break;
      case AnchorType.OneCell:
        this.SerializeOneCellAnchor(xmlTextWriter);
        break;
      default:
        this.SerializeTwoCellAnchor(xmlTextWriter);
        break;
    }
  }

  private void SerializeClientData(XmlWriter xmlTextWriter)
  {
    xmlTextWriter.WriteStartElement("clientData", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    Excel2007Serializator.SerializeAttribute(xmlTextWriter, "fLocksWithSheet", this.shape.LockWithSheet, true);
    Excel2007Serializator.SerializeAttribute(xmlTextWriter, "fPrintsWithSheet", this.shape.PrintWithSheet, true);
    xmlTextWriter.WriteEndElement();
  }

  private void SerializeConnector(XmlWriter xmlTextWriter)
  {
    if (this.parentShape != null && this.parentShape.Group != null)
      new ShapePropertiesSerializor(this.parentShape, this.attribute).Write(xmlTextWriter);
    else
      new ShapePropertiesSerializor(this.shape, this.attribute).Write(xmlTextWriter);
    new ShapeStyle(this.shape, this.attribute).Write(xmlTextWriter, "style");
  }

  private void SerializeGenralShapes(XmlWriter xmlTextWriter)
  {
    if (this.parentShape != null && this.parentShape.Group != null)
      new ShapePropertiesSerializor(this.parentShape, this.attribute).Write(xmlTextWriter);
    else
      new ShapePropertiesSerializor(this.shape, this.attribute).Write(xmlTextWriter);
    new ShapeStyle(this.shape, this.attribute).Write(xmlTextWriter, "style");
    new TextBody(this.shape, this.attribute).Write(xmlTextWriter);
  }

  private void SerializeGraphicFrame(XmlWriter xmlTextWriter)
  {
    throw new NotImplementedException();
  }

  private void SerializeGroupShapes(XmlWriter xmlTextWriter) => throw new NotImplementedException();

  private void SerializeOneCellAnchor(XmlWriter xmlTextWriter)
  {
    this.SerializeColRowAnchor(xmlTextWriter, "from", this.parentShape.LeftColumn, this.parentShape.LeftColumnOffset, this.parentShape.TopRow, this.parentShape.TopRowOffset, this.shape.ParentSheet, "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    xmlTextWriter.WriteStartElement("xdr", "ext", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    xmlTextWriter.WriteAttributeString("cx", ApplicationImpl.ConvertFromPixel((double) this.parentShape.Width, MeasureUnits.EMU).ToString());
    xmlTextWriter.WriteAttributeString("cy", ApplicationImpl.ConvertFromPixel((double) this.parentShape.Height, MeasureUnits.EMU).ToString());
    xmlTextWriter.WriteEndElement();
  }

  private void SerializePicture(XmlWriter xmlTextWriter)
  {
    if (this.parentShape != null && this.parentShape.Group != null)
      new ShapePropertiesSerializor(this.parentShape, this.attribute).Write(xmlTextWriter);
    else
      new ShapePropertiesSerializor(this.shape, this.attribute).Write(xmlTextWriter);
    new ShapeStyle(this.shape, this.attribute).Write(xmlTextWriter, "style");
  }

  private void SerializeRelSizeAnchor(XmlWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (this.shape.ParentSheet is WorksheetImpl)
    {
      this.SerializeColRowAnchor(writer, "from", this.parentShape.LeftColumn, this.parentShape.LeftColumnOffset, this.parentShape.TopRow, this.parentShape.TopRowOffset, this.shape.ParentSheet, "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
      this.SerializeColRowAnchor(writer, "to", this.parentShape.RightColumn, this.parentShape.RightColumnOffset, this.parentShape.BottomRow, this.parentShape.BottomRowOffset, this.shape.ParentSheet, "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    }
    else
    {
      this.SerializeXYAnchor(writer, "from", this.parentShape.LeftColumn, this.parentShape.TopRow, "http://schemas.openxmlformats.org/drawingml/2006/chartDrawing");
      this.SerializeXYAnchor(writer, "to", this.parentShape.RightColumn, this.parentShape.BottomRow, "http://schemas.openxmlformats.org/drawingml/2006/chartDrawing");
    }
  }

  internal void SerializeShapeChoices(XmlWriter xmlTextWriter)
  {
    ExcelAutoShapeType shapeType = this.shape.ShapeType;
    xmlTextWriter.WriteStartElement(this.attribute, shapeType.ToString(), this.nameSpace);
    if (shapeType != ExcelAutoShapeType.sp)
      this.WriteShapeAttributes(xmlTextWriter, false);
    else
      this.WriteShapeAttributes(xmlTextWriter, true);
    this.SerializeShapeNonVisual(xmlTextWriter);
    switch (shapeType)
    {
      case ExcelAutoShapeType.sp:
        this.SerializeGenralShapes(xmlTextWriter);
        break;
      case ExcelAutoShapeType.grpSp:
        this.SerializeGroupShapes(xmlTextWriter);
        break;
      case ExcelAutoShapeType.graphicFrame:
        this.SerializeGraphicFrame(xmlTextWriter);
        break;
      case ExcelAutoShapeType.cxnSp:
        this.SerializeConnector(xmlTextWriter);
        break;
      case ExcelAutoShapeType.pic:
        this.SerializePicture(xmlTextWriter);
        break;
    }
    xmlTextWriter.WriteEndElement();
  }

  private void SerializeShapeNonVisual(XmlWriter xmlTextWriter)
  {
    new ShapeNonVisual(this.shape, this.attribute).Write(xmlTextWriter);
  }

  private void SerializeTwoCellAnchor(XmlWriter xmlTextWriter)
  {
    this.SerializeColRowAnchor(xmlTextWriter, "from", this.parentShape.LeftColumn, this.parentShape.LeftColumnOffset, this.parentShape.TopRow, this.parentShape.TopRowOffset, this.shape.ParentSheet, "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    this.SerializeColRowAnchor(xmlTextWriter, "to", this.parentShape.RightColumn, this.parentShape.RightColumnOffset, this.parentShape.BottomRow, this.parentShape.BottomRowOffset, this.shape.ParentSheet, "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
  }

  internal void Write(XmlWriter xmlTextWriter)
  {
    switch (this.anchorType)
    {
      case AnchorType.Absolute:
        xmlTextWriter.WriteStartElement("xdr", "absoluteAnchor", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
        break;
      case AnchorType.RelSize:
        xmlTextWriter.WriteStartElement("cdr", "relSizeAnchor", this.chartNameSpace);
        break;
      case AnchorType.OneCell:
        xmlTextWriter.WriteStartElement("xdr", "oneCellAnchor", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
        break;
      default:
        xmlTextWriter.WriteStartElement("xdr", "twoCellAnchor", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
        break;
    }
    if (this.shape.ClientAnchor.Placement != PlacementType.MoveAndSize)
    {
      string placementType = Helper.GetPlacementType(this.shape.ClientAnchor.Placement);
      xmlTextWriter.WriteAttributeString("editAs", placementType);
    }
    this.SerializeAnchorPosition(xmlTextWriter);
    this.SerializeShapeChoices(xmlTextWriter);
    this.SerializeClientData(xmlTextWriter);
    xmlTextWriter.WriteEndElement();
  }

  private void WriteShapeAttributes(XmlWriter xmlTextWriter, bool isGeneralShape)
  {
    string macro = this.shape.Macro;
    if (macro != null)
      xmlTextWriter.WriteAttributeString("macro", macro);
    if (this.shape.Published)
      xmlTextWriter.WriteAttributeString("fPublished", "1");
    if (isGeneralShape)
    {
      string textLink = this.shape.TextLink;
      if (textLink != null)
        xmlTextWriter.WriteAttributeString("textlink", textLink);
    }
    bool locksText = this.shape.LocksText;
    Excel2007Serializator.SerializeAttribute(xmlTextWriter, "fLocksText", this.shape.LocksText, true);
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
    string str1 = XmlConvert.ToString((double) column / 1000.0);
    writer.WriteElementString("x", drawingsNamespace, str1);
    if (row > 1000)
      row = 1000;
    string str2 = XmlConvert.ToString((double) row / 1000.0);
    writer.WriteElementString("y", drawingsNamespace, str2);
    writer.WriteEndElement();
  }
}
