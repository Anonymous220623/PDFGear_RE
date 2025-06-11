// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.XmlSerialization.Shapes.BitmapShapeSerializator
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Shapes;
using System;
using System.Xml;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.XmlSerialization.Shapes;

internal class BitmapShapeSerializator : DrawingShapeSerializator
{
  public override void Serialize(
    XmlWriter writer,
    ShapeImpl shape,
    WorksheetDataHolder holder,
    RelationCollection vmlRelations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (!(shape is BitmapShapeImpl bitmapShapeImpl))
      throw new ArgumentOutOfRangeException("picture");
    FileDataHolder parentHolder = holder.ParentHolder;
    string relationId = this.SerializePictureFile(holder, bitmapShapeImpl, vmlRelations);
    bool flag = !(shape.Worksheet is WorksheetImpl);
    string localName;
    string str;
    if (flag)
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
    if (!flag)
      writer.WriteAttributeString("editAs", DrawingShapeSerializator.GetEditAsValue((ShapeImpl) bitmapShapeImpl));
    this.SerializeAnchorPoint(writer, "from", shape.LeftColumn, shape.LeftColumnOffset, shape.TopRow, shape.TopRowOffset, shape.Worksheet, str);
    this.SerializeAnchorPoint(writer, "to", shape.RightColumn, shape.RightColumnOffset, shape.BottomRow, shape.BottomRowOffset, shape.Worksheet, str);
    this.SerializePicture(writer, bitmapShapeImpl, relationId, holder, str);
    writer.WriteEndElement();
  }

  private void SerializePicture(
    XmlWriter writer,
    BitmapShapeImpl picture,
    string relationId,
    WorksheetDataHolder holder,
    string mainNamespace)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (picture == null)
      throw new ArgumentNullException(nameof (picture));
    if (relationId == null || relationId.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (relationId));
    writer.WriteStartElement("pic", mainNamespace);
    string macro = picture.Macro;
    if (macro != null)
      writer.WriteAttributeString("macro", macro);
    this.SerializeNonVisualProperties(writer, picture, holder, mainNamespace);
    this.SerializeBlipFill(writer, picture, relationId, mainNamespace);
    this.SerializeShapeProperties(writer, picture, mainNamespace);
    writer.WriteEndElement();
    if (!(mainNamespace == "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing"))
      return;
    writer.WriteStartElement("clientData", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    writer.WriteEndElement();
  }

  private void SerializeShapeProperties(
    XmlWriter writer,
    BitmapShapeImpl picture,
    string mainNamespace)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (picture == null)
      throw new ArgumentNullException(nameof (picture));
    if (picture.ShapePropertiesStream != null)
    {
      Excel2007Serializator.SerializeStream(writer, picture.ShapePropertiesStream, (string) null);
    }
    else
    {
      writer.WriteStartElement("spPr", mainNamespace);
      DrawingShapeSerializator.SerializeForm(writer, "http://schemas.openxmlformats.org/drawingml/2006/main", "http://schemas.openxmlformats.org/drawingml/2006/main", 0, 1, 2076450, 1557338, (IShape) picture);
      this.SerializePresetGeometry(writer);
      writer.WriteEndElement();
    }
  }

  private void SerializeNonVisualProperties(
    XmlWriter writer,
    BitmapShapeImpl picture,
    WorksheetDataHolder holder,
    string mainNamespace)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (picture == null)
      throw new ArgumentNullException(nameof (picture));
    writer.WriteStartElement("nvPicPr", mainNamespace);
    this.SerializeNVCanvasProperties(writer, (ShapeImpl) picture, holder, mainNamespace);
    this.SerializeNVPictureCanvasProperties(writer, picture, mainNamespace);
    writer.WriteEndElement();
  }

  private void SerializeNVPictureCanvasProperties(
    XmlWriter writer,
    BitmapShapeImpl picture,
    string mainNamespace)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (picture == null)
      throw new ArgumentNullException(nameof (picture));
    writer.WriteStartElement("cNvPicPr", mainNamespace);
    writer.WriteStartElement("picLocks", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("noChangeAspect", "1");
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private void SerializeBlipFill(
    XmlWriter writer,
    BitmapShapeImpl picture,
    string relationId,
    string mainNamespace)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (picture == null)
      throw new ArgumentNullException(nameof (picture));
    if (relationId == null || relationId.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (relationId));
    writer.WriteStartElement("blipFill", mainNamespace);
    writer.WriteStartElement("blip", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("embed", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", relationId);
    if (picture.BlipSubNodesStream != null)
      Excel2007Serializator.SerializeStream(writer, picture.BlipSubNodesStream, "root");
    writer.WriteEndElement();
    if (picture.SourceRectStream != null)
    {
      Excel2007Serializator.SerializeStream(writer, picture.SourceRectStream, "root");
    }
    else
    {
      writer.WriteStartElement("srcRect", "http://schemas.openxmlformats.org/drawingml/2006/main");
      if (picture.CropBottomOffset != 0)
        writer.WriteAttributeString("b", picture.CropBottomOffset.ToString());
      if (picture.CropLeftOffset != 0)
        writer.WriteAttributeString("l", picture.CropLeftOffset.ToString());
      if (picture.CropRightOffset != 0)
        writer.WriteAttributeString("r", picture.CropRightOffset.ToString());
      if (picture.CropTopOffset != 0)
        writer.WriteAttributeString("t", picture.CropTopOffset.ToString());
      writer.WriteEndElement();
    }
    writer.WriteStartElement("stretch", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteElementString("fillRect", "http://schemas.openxmlformats.org/drawingml/2006/main", string.Empty);
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  public override void SerializeShapeType(XmlWriter writer, Type shapeType)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public string SerializePictureFile(
    WorksheetDataHolder holder,
    BitmapShapeImpl picture,
    RelationCollection vmlRelations)
  {
    if (holder == null)
      throw new ArgumentNullException(nameof (holder));
    if (picture == null)
      throw new ArgumentNullException(nameof (picture));
    RelationCollection drawingsRelations = holder.DrawingsRelations;
    string empty = string.Empty;
    string id = (string) null;
    string str;
    if (picture.BlipId > 0U)
    {
      str = '/'.ToString() + holder.ParentHolder.GetImageItemName((int) picture.BlipId - 1);
      id = drawingsRelations.FindRelationByTarget(str);
    }
    else
      str = "NULL";
    if (id == null)
    {
      id = drawingsRelations.GenerateRelationId();
      drawingsRelations[id] = new Relation(str, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image");
    }
    return id;
  }
}
