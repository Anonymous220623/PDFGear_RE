// Decompiled with JetBrains decompiler
// Type: Syncfusion.Drawing.ShapePropertiesSerializor
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart;
using Syncfusion.OfficeChart.Implementation.XmlReaders.Shapes;
using Syncfusion.OfficeChart.Implementation.XmlSerialization;
using Syncfusion.OfficeChart.Implementation.XmlSerialization.Charts;
using Syncfusion.OfficeChart.Implementation.XmlSerialization.Shapes;
using Syncfusion.OfficeChart.Interfaces;
using System.IO;
using System.Security;
using System.Xml;

#nullable disable
namespace Syncfusion.Drawing;

internal class ShapePropertiesSerializor
{
  private string attribute;
  private ShapeImplExt shape;
  private int dpiX;
  private int dpiY;

  public ShapePropertiesSerializor(ShapeImplExt shape, string attribute)
  {
    this.shape = shape;
    this.attribute = attribute;
    this.dpiX = shape.Worksheet.AppImplementation.GetdpiX();
    this.dpiY = shape.Worksheet.AppImplementation.GetdpiY();
  }

  private void SerializeEffectProperties(XmlWriter xmlTextWriter)
  {
    Stream stream;
    if (!this.shape.PreservedElements.TryGetValue("Effect", out stream) || stream == null || stream.Length <= 0L)
      return;
    stream.Position = 0L;
    ShapeParser.WriteNodeFromStream(xmlTextWriter, stream);
  }

  [SecurityCritical]
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
      FileDataHolder parentHolder = this.shape.Worksheet.DataHolder.ParentHolder;
      ChartSerializatorCommon.SerializeFill(xmlTextWriter, fill, parentHolder, this.shape.Relations);
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
    xmlTextWriter.WriteStartElement("a", "prstGeom", "http://schemas.openxmlformats.org/drawingml/2006/main");
    AutoShapeConstant autoShapeConstant = AutoShapeHelper.GetAutoShapeConstant(this.shape.AutoShapeType);
    if (autoShapeConstant != AutoShapeConstant.Index_187)
      xmlTextWriter.WriteAttributeString("prst", AutoShapeHelper.GetAutoShapeString(autoShapeConstant));
    else
      xmlTextWriter.WriteAttributeString("prst", "rect");
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
    xmlTextWriter.WriteEndElement();
  }

  private void SerializeLineProperties(XmlWriter xmlTextWriter)
  {
    if (this.shape.Logger.GetPreservedItem(PreservedFlag.Line))
    {
      IShapeLineFormat line = (IShapeLineFormat) this.shape.Line;
      DrawingShapeSerializator.SerializeLineSettings(xmlTextWriter, line, this.shape.Worksheet.Workbook);
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
      xmlTextWriter.WriteAttributeString("flipH", "1");
    if (this.shape.FlipVertical)
      xmlTextWriter.WriteAttributeString("flipV", "1");
    xmlTextWriter.WriteStartElement("a", "off", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlTextWriter.WriteAttributeString("x", "0");
    xmlTextWriter.WriteAttributeString("y", "0");
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteStartElement("a", "ext", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlTextWriter.WriteAttributeString("cx", "0");
    xmlTextWriter.WriteAttributeString("cy", "0");
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteEndElement();
  }

  [SecurityCritical]
  internal void Write(XmlWriter xmlTextWriter)
  {
    xmlTextWriter.WriteStartElement(this.attribute, "spPr", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
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
