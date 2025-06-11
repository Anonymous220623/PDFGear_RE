// Decompiled with JetBrains decompiler
// Type: Syncfusion.Drawing.ShapeNonVisual
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart;
using System.Xml;

#nullable disable
namespace Syncfusion.Drawing;

internal class ShapeNonVisual
{
  private string attribute;
  private string drawingPros = "cNvPr";
  private string drawingShapeProps;
  private string nonVisual;
  private ShapeImplExt shape;

  public ShapeNonVisual(ShapeImplExt shape, string attribute)
  {
    this.shape = shape;
    this.attribute = attribute;
  }

  private void InitializeShapeType(ExcelAutoShapeType shapeType)
  {
    switch (shapeType)
    {
      case ExcelAutoShapeType.sp:
        this.nonVisual = "nvSpPr";
        this.drawingShapeProps = "cNvSpPr";
        break;
      case ExcelAutoShapeType.grpSp:
        this.nonVisual = "nvGrpSpPr";
        this.drawingShapeProps = "cNvGrpSpPr";
        break;
      case ExcelAutoShapeType.graphicFrame:
        this.nonVisual = "nvGraphicFramePr";
        this.drawingShapeProps = "cNvGraphicFramePr";
        break;
      case ExcelAutoShapeType.cxnSp:
        this.nonVisual = "nvCxnSpPr";
        this.drawingShapeProps = "cNvCxnSpPr";
        break;
      case ExcelAutoShapeType.pic:
        this.nonVisual = "nvPicPr";
        this.drawingShapeProps = "cNvPicPr";
        break;
    }
  }

  private void SerializeNonVisualDrawingProps(XmlWriter xmlTextWriter)
  {
    int shapeId = this.shape.ShapeID;
    xmlTextWriter.WriteStartElement(this.attribute, this.drawingPros, "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    xmlTextWriter.WriteAttributeString("id", shapeId.ToString());
    if (this.shape.Name != null && this.shape.Name.Length > 0)
      xmlTextWriter.WriteAttributeString("name", this.shape.Name);
    else
      xmlTextWriter.WriteAttributeString("name", $"{this.shape.AutoShapeType.ToString()} {shapeId}");
    if (this.shape.Description != null && this.shape.Description.Length > 0)
      xmlTextWriter.WriteAttributeString("descr", this.shape.Description);
    if (this.shape.IsHidden)
      xmlTextWriter.WriteAttributeString("hidden", "1");
    if (this.shape.Title != null && this.shape.Title.Length > 0)
      xmlTextWriter.WriteAttributeString("title", this.shape.Title);
    xmlTextWriter.WriteEndElement();
  }

  private void SerializeNonVisualDrawingShapeProps(XmlWriter xmlTextWriter)
  {
    xmlTextWriter.WriteStartElement(this.attribute, this.drawingShapeProps, "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    xmlTextWriter.WriteEndElement();
  }

  internal void Write(XmlWriter xmlTextWriter)
  {
    this.InitializeShapeType(this.shape.ShapeType);
    xmlTextWriter.WriteStartElement(this.attribute, this.nonVisual, "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    this.SerializeNonVisualDrawingProps(xmlTextWriter);
    this.SerializeNonVisualDrawingShapeProps(xmlTextWriter);
    xmlTextWriter.WriteEndElement();
  }
}
