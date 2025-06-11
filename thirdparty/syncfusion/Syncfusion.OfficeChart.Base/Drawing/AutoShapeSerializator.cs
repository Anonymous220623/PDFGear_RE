// Decompiled with JetBrains decompiler
// Type: Syncfusion.Drawing.AutoShapeSerializator
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart;
using System;
using System.Security;
using System.Xml;

#nullable disable
namespace Syncfusion.Drawing;

internal class AutoShapeSerializator
{
  private AnchorType anchorType;
  private string attribute;
  private string chartNameSpace = "http://schemas.openxmlformats.org/drawingml/2006/chartDrawing";
  private ShapeImplExt shape;
  private int resolution;

  internal AutoShapeSerializator(ShapeImplExt shape)
  {
    this.shape = shape;
    this.attribute = "xdr";
    this.anchorType = shape.AnchorType;
    this.resolution = shape.Worksheet.AppImplementation.GetdpiX();
  }

  private void SerializeAbsoluteAnchor(XmlWriter xmlTextWriter)
  {
    int offsetValue1 = this.shape.ClientAnchor.Width;
    int offsetValue2 = this.shape.ClientAnchor.Height;
    if (offsetValue1 < 911)
    {
      offsetValue1 = 900;
      offsetValue2 = 600;
    }
    int emu1 = Helper.ConvertOffsetToEMU(offsetValue1, this.resolution);
    int emu2 = Helper.ConvertOffsetToEMU(offsetValue2, this.resolution);
    xmlTextWriter.WriteStartElement("xdr", "pos", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    xmlTextWriter.WriteAttributeString("x", "0");
    xmlTextWriter.WriteAttributeString("y", "0");
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteStartElement("xdr", "ext", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
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
    xmlTextWriter.WriteStartElement("xdr", "clientData", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    xmlTextWriter.WriteEndElement();
  }

  [SecurityCritical]
  private void SerializeConnector(XmlWriter xmlTextWriter)
  {
    new ShapePropertiesSerializor(this.shape, this.attribute).Write(xmlTextWriter);
    new ShapeStyle(this.shape, this.attribute).Write(xmlTextWriter);
  }

  [SecurityCritical]
  private void SerializeGenralShapes(XmlWriter xmlTextWriter)
  {
    new ShapePropertiesSerializor(this.shape, this.attribute).Write(xmlTextWriter);
    new ShapeStyle(this.shape, this.attribute).Write(xmlTextWriter);
    new TextBody(this.shape, this.attribute).Write(xmlTextWriter);
  }

  private void SerializeGraphicFrame(XmlWriter xmlTextWriter)
  {
    throw new NotImplementedException();
  }

  private void SerializeGroupShapes(XmlWriter xmlTextWriter) => throw new NotImplementedException();

  private void SerializeOneCellAnchor(XmlWriter xmlTextWriter)
  {
    int leftColumn = this.shape.ClientAnchor.LeftColumn;
    int leftColumnOffset = this.shape.ClientAnchor.LeftColumnOffset;
    int columnOffset = this.shape.ClientAnchor.CalculateColumnOffset(leftColumn, 0, leftColumn, leftColumnOffset);
    int topRow = this.shape.ClientAnchor.TopRow;
    int topRowOffset = this.shape.ClientAnchor.TopRowOffset;
    int rowOffset = this.shape.ClientAnchor.CalculateRowOffset(topRow, 0, topRow, topRowOffset);
    int width = this.shape.ClientAnchor.Width;
    int height = this.shape.ClientAnchor.Height;
    int emu1 = Helper.ConvertOffsetToEMU(width, this.resolution);
    int emu2 = Helper.ConvertOffsetToEMU(height, this.resolution);
    xmlTextWriter.WriteStartElement("xdr", "from", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    xmlTextWriter.WriteStartElement("xdr", "col", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    xmlTextWriter.WriteString(Helper.ToString(leftColumn));
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteStartElement("xdr", "colOff", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    xmlTextWriter.WriteString(Helper.ToString(Helper.ConvertOffsetToEMU(columnOffset, this.resolution)));
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteStartElement("xdr", "row", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    xmlTextWriter.WriteString(Helper.ToString(topRow));
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteStartElement("xdr", "rowOff", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    xmlTextWriter.WriteString(Helper.ToString(Helper.ConvertOffsetToEMU(rowOffset, this.resolution)));
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteStartElement("xdr", "ext", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    xmlTextWriter.WriteAttributeString("cx", Helper.ToString(emu1));
    xmlTextWriter.WriteAttributeString("cy", Helper.ToString(emu2));
    xmlTextWriter.WriteEndElement();
  }

  [SecurityCritical]
  private void SerializePicture(XmlWriter xmlTextWriter)
  {
    new ShapePropertiesSerializor(this.shape, this.attribute).Write(xmlTextWriter);
    new ShapeStyle(this.shape, this.attribute).Write(xmlTextWriter);
  }

  private void SerializeRelSizeAnchor(XmlWriter xmlTextWriter)
  {
    throw new NotImplementedException();
  }

  [SecurityCritical]
  private void SerializeShapeChoices(XmlWriter xmlTextWriter)
  {
    ExcelAutoShapeType shapeType = this.shape.ShapeType;
    xmlTextWriter.WriteStartElement(this.attribute, shapeType.ToString(), "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
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
    int leftColumn = this.shape.ClientAnchor.LeftColumn;
    int leftColumnOffset = this.shape.ClientAnchor.LeftColumnOffset;
    int columnOffset1 = this.shape.ClientAnchor.CalculateColumnOffset(leftColumn, 0, leftColumn, leftColumnOffset);
    int topRow = this.shape.ClientAnchor.TopRow;
    int topRowOffset = this.shape.ClientAnchor.TopRowOffset;
    int rowOffset1 = this.shape.ClientAnchor.CalculateRowOffset(topRow, 0, topRow, topRowOffset);
    int rightColumn = this.shape.ClientAnchor.RightColumn;
    int rightColumnOffset = this.shape.ClientAnchor.RightColumnOffset;
    int columnOffset2 = this.shape.ClientAnchor.CalculateColumnOffset(rightColumn, 0, rightColumn, rightColumnOffset);
    int bottomRow = this.shape.ClientAnchor.BottomRow;
    int bottomRowOffset = this.shape.ClientAnchor.BottomRowOffset;
    int rowOffset2 = this.shape.ClientAnchor.CalculateRowOffset(bottomRow, 0, bottomRow, bottomRowOffset);
    xmlTextWriter.WriteStartElement("xdr", "from", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    xmlTextWriter.WriteStartElement("xdr", "col", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    xmlTextWriter.WriteString(Helper.ToString(leftColumn));
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteStartElement("xdr", "colOff", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    xmlTextWriter.WriteString(Helper.ToString(Helper.ConvertOffsetToEMU(columnOffset1, this.resolution)));
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteStartElement("xdr", "row", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    xmlTextWriter.WriteString(Helper.ToString(topRow));
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteStartElement("xdr", "rowOff", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    xmlTextWriter.WriteString(Helper.ToString(Helper.ConvertOffsetToEMU(rowOffset1, this.resolution)));
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteStartElement("xdr", "to", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    xmlTextWriter.WriteStartElement("xdr", "col", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    xmlTextWriter.WriteString(Helper.ToString(rightColumn));
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteStartElement("xdr", "colOff", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    xmlTextWriter.WriteString(Helper.ToString(Helper.ConvertOffsetToEMU(columnOffset2, this.resolution)));
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteStartElement("xdr", "row", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    xmlTextWriter.WriteString(Helper.ToString(bottomRow));
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteStartElement("xdr", "rowOff", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    xmlTextWriter.WriteString(Helper.ToString(Helper.ConvertOffsetToEMU(rowOffset2, this.resolution)));
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteEndElement();
  }

  [SecurityCritical]
  internal void Write(XmlWriter xmlTextWriter)
  {
    switch (this.anchorType)
    {
      case AnchorType.Absolute:
        xmlTextWriter.WriteStartElement("xdr", "absoluteAnchor", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
        break;
      case AnchorType.RelSize:
        xmlTextWriter.WriteStartElement("cdr:relSizeAnchor");
        xmlTextWriter.WriteAttributeString("xmlns:cdr", this.chartNameSpace);
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
    if (!isGeneralShape)
      return;
    string textLink = this.shape.TextLink;
    if (textLink == null)
      return;
    xmlTextWriter.WriteAttributeString("textlink", textLink);
  }
}
