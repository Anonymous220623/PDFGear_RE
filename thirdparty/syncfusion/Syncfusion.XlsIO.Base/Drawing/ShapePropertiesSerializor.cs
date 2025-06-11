// Decompiled with JetBrains decompiler
// Type: Syncfusion.Drawing.ShapePropertiesSerializor
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO;
using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Implementation.XmlReaders.Shapes;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
using Syncfusion.XlsIO.Implementation.XmlSerialization.Charts;
using Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes;
using Syncfusion.XlsIO.Interfaces;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.Drawing;

internal class ShapePropertiesSerializor
{
  private string attribute;
  private ShapeImplExt shape;
  private ShapeImpl parentShape;
  private int dpiX;
  private int dpiY;
  private string nameSpace;

  public ShapePropertiesSerializor(ShapeImplExt shape, string attribute)
  {
    this.shape = shape;
    this.attribute = attribute;
    if (shape.Worksheet != null)
    {
      this.dpiX = shape.Worksheet.AppImplementation.GetdpiX();
      this.dpiY = shape.Worksheet.AppImplementation.GetdpiY();
    }
    else if (shape.ParentSheet != null)
    {
      this.dpiX = shape.ParentSheet.AppImplementation.GetdpiX();
      this.dpiY = shape.ParentSheet.AppImplementation.GetdpiY();
    }
    else
    {
      this.dpiX = 96 /*0x60*/;
      this.dpiY = 96 /*0x60*/;
    }
    this.nameSpace = attribute == "xdr" ? "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing" : "http://schemas.openxmlformats.org/drawingml/2006/chartDrawing";
  }

  internal ShapePropertiesSerializor(AutoShapeImpl parentShape, string attribute)
  {
    this.parentShape = (ShapeImpl) parentShape;
    this.shape = parentShape.ShapeExt;
    this.attribute = attribute;
    if (this.shape.Worksheet != null)
    {
      this.dpiX = this.shape.Worksheet.AppImplementation.GetdpiX();
      this.dpiY = this.shape.Worksheet.AppImplementation.GetdpiY();
    }
    else if (this.shape.ParentSheet != null)
    {
      this.dpiX = this.shape.ParentSheet.AppImplementation.GetdpiX();
      this.dpiY = this.shape.ParentSheet.AppImplementation.GetdpiY();
    }
    else
    {
      this.dpiX = 96 /*0x60*/;
      this.dpiY = 96 /*0x60*/;
    }
    this.nameSpace = attribute == "xdr" ? "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing" : "http://schemas.openxmlformats.org/drawingml/2006/chartDrawing";
  }

  private void SerializeEffectProperties(XmlWriter xmlTextWriter)
  {
    Stream stream;
    if (!this.shape.PreservedElements.TryGetValue("Effect", out stream) || stream == null || stream.Length <= 0L)
      return;
    stream.Position = 0L;
    ShapeParser.WriteNodeFromStream(xmlTextWriter, stream);
  }

  private void SerializeFillProperties(XmlWriter xmlTextWriter)
  {
    if (!this.shape.Fill.Visible)
    {
      xmlTextWriter.WriteStartElement("a", "noFill", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlTextWriter.WriteEndElement();
    }
    else if (this.shape.Logger.GetPreservedItem(PreservedFlag.Fill))
    {
      IInternalFill fill = (IInternalFill) this.shape.Fill;
      FileDataHolder holder = (FileDataHolder) null;
      RelationCollection relations = (RelationCollection) null;
      if (this.shape.Worksheet != null)
      {
        holder = this.shape.Worksheet.DataHolder.ParentHolder;
        relations = this.shape.Worksheet.DataHolder.DrawingsRelations;
      }
      else if (this.shape.ParentSheet != null)
      {
        holder = this.shape.ParentSheet.DataHolder.ParentHolder;
        relations = this.shape.Worksheet.DataHolder.DrawingsRelations;
      }
      if (holder == null)
        return;
      ChartSerializatorCommon.SerializeFill(xmlTextWriter, fill, holder, relations);
    }
    else
    {
      Stream stream;
      if (!this.shape.PreservedElements.TryGetValue("Fill", out stream) || stream == null || stream.Length <= 0L)
        return;
      stream.Position = 0L;
      ShapeParser.WriteNodeFromStream(xmlTextWriter, stream);
    }
  }

  private void SerializeGemoerty(XmlWriter xmlTextWriter)
  {
    if (!this.shape.IsCustomGeometry)
    {
      xmlTextWriter.WriteStartElement("a", "prstGeom", "http://schemas.openxmlformats.org/drawingml/2006/main");
      AutoShapeConstant autoShapeConstant = AutoShapeHelper.GetAutoShapeConstant(this.shape.AutoShapeType);
      if (autoShapeConstant != AutoShapeConstant.Index_187)
        xmlTextWriter.WriteAttributeString("prst", AutoShapeHelper.GetAutoShapeString(autoShapeConstant));
      else
        xmlTextWriter.WriteAttributeString("prst", "rect");
    }
    Stream stream;
    if (this.shape.PreservedElements.TryGetValue("avLst", out stream))
    {
      if (stream != null && stream.Length > 0L)
      {
        stream.Position = 0L;
        ShapeParser.WriteNodeFromStream(xmlTextWriter, stream);
      }
    }
    else
    {
      xmlTextWriter.WriteStartElement("a", "avLst", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlTextWriter.WriteEndElement();
    }
    if (this.shape.IsCustomGeometry)
      return;
    xmlTextWriter.WriteEndElement();
  }

  private void SerializeLineProperties(XmlWriter xmlTextWriter)
  {
    if (this.shape.Logger.GetPreservedItem(PreservedFlag.Line))
    {
      IShapeLineFormat line = (IShapeLineFormat) this.shape.Line;
      WorksheetImpl worksheet = this.shape.Worksheet;
      WorksheetBaseImpl parentSheet = this.shape.ParentSheet;
      if (worksheet != null)
      {
        DrawingShapeSerializator.SerializeLineSettings(xmlTextWriter, line, worksheet.Workbook);
      }
      else
      {
        if (parentSheet == null)
          return;
        DrawingShapeSerializator.SerializeLineSettings(xmlTextWriter, line, parentSheet.Workbook);
      }
    }
    else
    {
      Stream stream;
      if (!this.shape.PreservedElements.TryGetValue("Line", out stream) || stream == null || stream.Length <= 0L)
        return;
      stream.Position = 0L;
      ShapeParser.WriteNodeFromStream(xmlTextWriter, stream);
    }
  }

  private void SerializeScence3d(XmlWriter xmlTextWriter)
  {
    Stream stream;
    if (!this.shape.PreservedElements.TryGetValue("Scene3d", out stream) || stream == null || stream.Length <= 0L)
      return;
    stream.Position = 0L;
    ShapeParser.WriteNodeFromStream(xmlTextWriter, stream);
  }

  private void SerializeShape3d(XmlWriter xmlTextWriter)
  {
    Stream stream;
    if (!this.shape.PreservedElements.TryGetValue("Sp3d", out stream) || stream == null || stream.Length <= 0L)
      return;
    stream.Position = 0L;
    ShapeParser.WriteNodeFromStream(xmlTextWriter, stream);
  }

  private void SerializeTransformation(XmlWriter xmlTextWriter)
  {
    xmlTextWriter.WriteStartElement("a", "xfrm", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (this.shape.Rotation != 0.0 && this.shape.Rotation > 0.0)
      xmlTextWriter.WriteAttributeString("rot", Helper.ToString(this.shape.Rotation * 60000.0));
    if (this.shape.FlipHorizontal)
      xmlTextWriter.WriteAttributeString("flipH", this.shape.FlipHorizontal ? "1" : "0");
    if (this.shape.FlipVertical)
      xmlTextWriter.WriteAttributeString("flipV", this.shape.FlipVertical ? "1" : "0");
    double num1;
    double num2;
    double num3;
    double num4;
    if (this.parentShape != null && this.parentShape.Group != null)
    {
      num1 = (double) this.parentShape.ShapeFrame.OffsetX;
      num2 = (double) this.parentShape.ShapeFrame.OffsetY;
      num3 = (double) this.parentShape.ShapeFrame.OffsetCX;
      num4 = (double) this.parentShape.ShapeFrame.OffsetCY;
    }
    else
    {
      num1 = (double) this.shape.Coordinates.X;
      num2 = (double) this.shape.Coordinates.Y;
      num3 = (double) this.shape.Coordinates.Width;
      num4 = (double) this.shape.Coordinates.Height;
    }
    xmlTextWriter.WriteStartElement("a", "off", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlTextWriter.WriteAttributeString("x", num1.ToString());
    xmlTextWriter.WriteAttributeString("y", num2.ToString());
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteStartElement("a", "ext", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlTextWriter.WriteAttributeString("cx", num3.ToString());
    xmlTextWriter.WriteAttributeString("cy", num4.ToString());
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteEndElement();
  }

  internal void Write(XmlWriter xmlTextWriter)
  {
    xmlTextWriter.WriteStartElement(this.attribute, "spPr", this.nameSpace);
    this.SerializeTransformation(xmlTextWriter);
    this.SerializeGemoerty(xmlTextWriter);
    this.SerializeFillProperties(xmlTextWriter);
    this.SerializeLineProperties(xmlTextWriter);
    this.SerializeEffectProperties(xmlTextWriter);
    this.SerializeScence3d(xmlTextWriter);
    this.SerializeShape3d(xmlTextWriter);
    xmlTextWriter.WriteEndElement();
  }
}
