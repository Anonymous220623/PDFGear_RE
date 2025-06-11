// Decompiled with JetBrains decompiler
// Type: Syncfusion.Drawing.ShapeNonVisual
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
using System.Collections;
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
  private string nameSpace;

  public ShapeNonVisual(ShapeImplExt shape, string attribute)
  {
    this.shape = shape;
    this.attribute = attribute;
    this.nameSpace = attribute == "xdr" ? "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing" : "http://schemas.openxmlformats.org/drawingml/2006/chartDrawing";
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
    xmlTextWriter.WriteStartElement(this.attribute, this.drawingPros, this.nameSpace);
    xmlTextWriter.WriteAttributeString("id", shapeId.ToString());
    ShapeImpl shapeImpl = (ShapeImpl) null;
    if (this.shape.Name != null && this.shape.Name.Length > 0)
    {
      xmlTextWriter.WriteAttributeString("name", this.shape.Name);
      foreach (IShape shape1 in (IEnumerable) this.shape.ParentSheet.Shapes)
      {
        if (this.shape.ShapeID == shape1.Id)
        {
          shapeImpl = shape1 as ShapeImpl;
          break;
        }
        if (shape1 is GroupShapeImpl)
        {
          foreach (IShape shape2 in (shape1 as GroupShapeImpl).Items)
          {
            if (this.shape.ShapeID == shape2.Id)
            {
              shapeImpl = shape2 as ShapeImpl;
              break;
            }
          }
        }
      }
    }
    else
    {
      string str = $"{this.shape.AutoShapeType.ToString()} {shapeId}";
      xmlTextWriter.WriteAttributeString("name", str);
      IShapes shapes = this.shape.ParentSheet.Shapes;
      if (shapes.Count > 0)
      {
        for (int index = 0; index < shapes.Count; ++index)
        {
          if (shapes[index].ShapeType == ExcelShapeType.AutoShape && shapes[index] is AutoShapeImpl autoShapeImpl && this.shape == autoShapeImpl.ShapeExt)
            shapeImpl = shapes[index] as ShapeImpl;
        }
      }
    }
    if (this.shape.Description != null && this.shape.Description.Length > 0)
      xmlTextWriter.WriteAttributeString("descr", this.shape.Description);
    if (this.shape.IsHidden)
      xmlTextWriter.WriteAttributeString("hidden", "1");
    if (this.shape.Title != null && this.shape.Title.Length > 0)
      xmlTextWriter.WriteAttributeString("title", this.shape.Title);
    if (shapeImpl != null && shapeImpl.IsHyperlink)
    {
      xmlTextWriter.WriteStartElement("hlinkClick", "http://schemas.openxmlformats.org/drawingml/2006/main");
      IHyperLink hyperlink = shapeImpl.Hyperlink;
      string str1 = hyperlink.Address;
      if (hyperlink.Type == ExcelHyperLinkType.File && !str1.StartsWith("..") && str1.Contains(":\\") && !str1.StartsWith("file:///") || hyperlink.Type == ExcelHyperLinkType.Unc)
        str1 = "file:///" + str1;
      if (hyperlink.Type != ExcelHyperLinkType.Workbook)
        str1 = str1?.Replace(" ", "%20");
      bool isExternal = hyperlink.Type != ExcelHyperLinkType.Workbook;
      if (!isExternal)
        str1 = str1.StartsWith("#") ? str1 : "#" + str1;
      string str2 = this.shape.ParentSheet.DataHolder.DrawingsRelations.FindRelationByTarget(str1) ?? this.shape.ParentSheet.DataHolder.DrawingsRelations.Add(new Relation(str1, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/hyperlink", isExternal));
      xmlTextWriter.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", str2);
      if (!string.IsNullOrEmpty(hyperlink.ScreenTip))
        xmlTextWriter.WriteAttributeString("tooltip", hyperlink.ScreenTip);
      xmlTextWriter.WriteEndElement();
    }
    xmlTextWriter.WriteEndElement();
  }

  private void SerializeNonVisualDrawingShapeProps(XmlWriter xmlTextWriter)
  {
    xmlTextWriter.WriteStartElement(this.attribute, this.drawingShapeProps, this.nameSpace);
    xmlTextWriter.WriteEndElement();
  }

  internal void Write(XmlWriter xmlTextWriter)
  {
    this.InitializeShapeType(this.shape.ShapeType);
    xmlTextWriter.WriteStartElement(this.attribute, this.nonVisual, this.nameSpace);
    this.SerializeNonVisualDrawingProps(xmlTextWriter);
    this.SerializeNonVisualDrawingShapeProps(xmlTextWriter);
    xmlTextWriter.WriteEndElement();
  }
}
