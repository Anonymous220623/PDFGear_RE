// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes.GroupShapeSerializator
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Drawing;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Implementation.XmlReaders.Shapes;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes;

internal class GroupShapeSerializator : DrawingShapeSerializator
{
  private string attribute;
  private string chartNameSpace = "http://schemas.openxmlformats.org/drawingml/2006/chartDrawing";
  private GroupShapeImpl shape;
  private int resolution;
  private string nameSpace;

  internal GroupShapeSerializator(GroupShapeImpl shape)
  {
    this.shape = shape;
    this.attribute = shape.Worksheet is WorksheetImpl ? "xdr" : "cdr";
    this.nameSpace = shape.Worksheet is WorksheetImpl ? "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing" : "http://schemas.openxmlformats.org/drawingml/2006/chartDrawing";
    if (shape.Worksheet != null)
      this.resolution = shape.Worksheet.AppImplementation.GetdpiX();
    else if (shape.Worksheet != null)
      this.resolution = shape.Worksheet.AppImplementation.GetdpiX();
    else
      this.resolution = 96 /*0x60*/;
  }

  internal void SerializeAncor(XmlWriter writer)
  {
    string localName;
    string str;
    if (!(this.shape.Worksheet is WorksheetImpl))
    {
      localName = "relSizeAnchor";
      str = "http://schemas.openxmlformats.org/drawingml/2006/chartDrawing";
    }
    else
    {
      localName = "twoCellAnchor";
      str = "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing";
    }
    writer.WriteStartElement(localName, str);
    this.SerializeAnchorPoint(writer, "from", this.shape.LeftColumn, this.shape.LeftColumnOffset, this.shape.TopRow, this.shape.TopRowOffset, this.shape.Worksheet, str);
    this.SerializeAnchorPoint(writer, "to", this.shape.RightColumn, this.shape.RightColumnOffset, this.shape.BottomRow, this.shape.BottomRowOffset, this.shape.Worksheet, str);
  }

  internal void SerializeTransformation(XmlWriter xmlTextWriter)
  {
    xmlTextWriter.WriteStartElement("a", "xfrm", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (this.shape.ShapeRotation != 0 && this.shape.ShapeRotation > 0)
      xmlTextWriter.WriteAttributeString("rot", Helper.ToString(this.shape.ShapeRotation * 60000));
    if (this.shape.FlipHorizontal)
      xmlTextWriter.WriteAttributeString("flipH", this.shape.FlipHorizontal ? "1" : "0");
    if (this.shape.FlipVertical)
      xmlTextWriter.WriteAttributeString("flipV", this.shape.FlipVertical ? "1" : "0");
    xmlTextWriter.WriteStartElement("a", "off", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlTextWriter.WriteAttributeString("x", this.shape.ShapeFrame.OffsetX.ToString());
    xmlTextWriter.WriteAttributeString("y", this.shape.ShapeFrame.OffsetY.ToString());
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteStartElement("a", "ext", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlTextWriter.WriteAttributeString("cx", this.shape.ShapeFrame.OffsetCX.ToString());
    xmlTextWriter.WriteAttributeString("cy", this.shape.ShapeFrame.OffsetCY.ToString());
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteStartElement("a", "chOff", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlTextWriter.WriteAttributeString("x", this.shape.ShapeFrame.ChOffsetX.ToString());
    xmlTextWriter.WriteAttributeString("y", this.shape.ShapeFrame.ChOffsetY.ToString());
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteStartElement("a", "chExt", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlTextWriter.WriteAttributeString("cx", this.shape.ShapeFrame.ChOffsetCX.ToString());
    xmlTextWriter.WriteAttributeString("cy", this.shape.ShapeFrame.ChOffsetCY.ToString());
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteEndElement();
  }

  internal void SerializeGroupShapeNVProps(XmlWriter writer)
  {
    writer.WriteStartElement(this.attribute, "nvGrpSpPr", this.nameSpace);
    this.SerializeNonVisualDrawingProps(writer);
    this.SerializeNonVisualDrawingShapeProps(writer);
    writer.WriteEndElement();
  }

  internal void SerializeNonVisualDrawingProps(XmlWriter writer)
  {
    int shapeId = this.shape.ShapeId;
    writer.WriteStartElement(this.attribute, "cNvPr", this.nameSpace);
    writer.WriteAttributeString("id", shapeId.ToString());
    ShapeImpl shapeImpl = (ShapeImpl) null;
    IShapes shapes = this.shape.Worksheet.Shapes;
    if (this.shape.Name != null && this.shape.Name.Length > 0)
    {
      writer.WriteAttributeString("name", this.shape.Name);
      if (!string.IsNullOrEmpty(this.shape.AlternativeText))
        writer.WriteAttributeString("descr", this.shape.AlternativeText);
      foreach (IShape shape in (IEnumerable) shapes)
      {
        if (this.shape.ShapeId == shape.Id)
        {
          shapeImpl = shape as ShapeImpl;
          break;
        }
      }
    }
    if (shapeImpl != null && shapeImpl.IsHyperlink)
    {
      writer.WriteStartElement("hlinkClick", "http://schemas.openxmlformats.org/drawingml/2006/main");
      IHyperLink hyperlink = shapeImpl.Hyperlink;
      string str1 = hyperlink.Address;
      if (hyperlink.Type == ExcelHyperLinkType.File && !str1.StartsWith("..") && str1.Contains(":\\") && !str1.StartsWith("file:///") || hyperlink.Type == ExcelHyperLinkType.Unc)
        str1 = "file:///" + str1;
      if (hyperlink.Type != ExcelHyperLinkType.Workbook)
        str1 = str1?.Replace(" ", "%20");
      bool isExternal = hyperlink.Type != ExcelHyperLinkType.Workbook;
      if (!isExternal)
        str1 = str1.StartsWith("#") ? str1 : "#" + str1;
      string str2 = this.shape.Worksheet.DataHolder.DrawingsRelations.FindRelationByTarget(str1) ?? this.shape.Worksheet.DataHolder.DrawingsRelations.Add(new Relation(str1, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/hyperlink", isExternal));
      writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", str2);
      if (!string.IsNullOrEmpty(hyperlink.ScreenTip))
        writer.WriteAttributeString("tooltip", hyperlink.ScreenTip);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  internal void SerializeNonVisualDrawingShapeProps(XmlWriter writer)
  {
    writer.WriteStartElement(this.attribute, "cNvGrpSpPr", this.nameSpace);
    writer.WriteEndElement();
  }

  private void SerializePreserveStream(
    XmlWriter xmlTextWriter,
    Dictionary<string, Stream> PreservedElements,
    string elementName)
  {
    Stream stream;
    if (!PreservedElements.TryGetValue(elementName, out stream) || stream == null || stream.Length <= 0L)
      return;
    stream.Position = 0L;
    ShapeParser.WriteNodeFromStream(xmlTextWriter, stream);
  }

  internal void SerializeGroupShapeProperties(
    XmlWriter xmlTextWriter,
    GroupShapeImpl groupShape,
    FileDataHolder holder,
    RelationCollection relations)
  {
    xmlTextWriter.WriteStartElement(this.attribute, "grpSpPr", this.nameSpace);
    this.SerializeTransformation(xmlTextWriter);
    this.SerializeFill(xmlTextWriter, (ShapeImpl) groupShape, holder, relations);
    this.SerializePreserveStream(xmlTextWriter, this.shape.PreservedElements, "Scene3d");
    this.SerializePreserveStream(xmlTextWriter, this.shape.PreservedElements, "Sp3d");
    xmlTextWriter.WriteEndElement();
  }
}
