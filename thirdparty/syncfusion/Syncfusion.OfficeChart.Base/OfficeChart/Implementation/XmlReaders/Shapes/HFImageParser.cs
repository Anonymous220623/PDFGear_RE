// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.XmlReaders.Shapes.HFImageParser
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Collections;
using Syncfusion.OfficeChart.Implementation.Shapes;
using Syncfusion.OfficeChart.Implementation.XmlSerialization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.XmlReaders.Shapes;

internal class HFImageParser : ShapeParser
{
  public override ShapeImpl ParseShapeType(XmlReader reader, ShapeCollectionBase shapes)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (shapes == null)
      throw new ArgumentNullException(nameof (shapes));
    reader.Skip();
    BitmapShapeImpl shapeType = new BitmapShapeImpl(shapes.Application, (object) shapes);
    shapeType.VmlShape = true;
    return (ShapeImpl) shapeType;
  }

  public override bool ParseShape(
    XmlReader reader,
    ShapeImpl defaultShape,
    RelationCollection relations,
    string parentItemPath)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (defaultShape == null)
      throw new ArgumentNullException(nameof (defaultShape));
    if (reader.LocalName != "shape")
      throw new XmlException("Unexpected xml tag.");
    string str = (string) null;
    int num = -1;
    if (reader.MoveToAttribute("spid", "urn:schemas-microsoft-com:office:office"))
    {
      str = reader.Value;
      num = this.ParseShapeId(str);
    }
    if (reader.MoveToAttribute("id"))
    {
      str = reader.Value;
      if (num == -1)
        num = this.ParseShapeId(str);
    }
    BitmapShapeImpl bitmapShapeImpl = (BitmapShapeImpl) defaultShape.Clone(defaultShape.Parent, (Dictionary<string, string>) null, (Dictionary<int, int>) null, false);
    if (reader.MoveToAttribute("stroked"))
      bitmapShapeImpl.HasBorder = true;
    else
      bitmapShapeImpl.HasBorder = false;
    if (reader.MoveToAttribute("style"))
      this.ParseStyle(reader, bitmapShapeImpl);
    reader.Read();
    if (num != -1)
    {
      bitmapShapeImpl.ShapeId = num;
      bitmapShapeImpl.Name = str;
    }
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "imagedata":
            this.ParseImageData(reader, str, bitmapShapeImpl, relations, parentItemPath);
            continue;
          case "ClientData":
            this.ParseClientData(reader, bitmapShapeImpl);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Read();
    }
    reader.Read();
    return true;
  }

  private void ParseStyle(XmlReader reader, BitmapShapeImpl result)
  {
    if (result == null)
      throw new ArgumentNullException(nameof (reader));
    if (result == null)
      throw new ArgumentNullException("textBox");
    result.PreserveStyleString = reader.Value;
  }

  protected virtual void ParseStyle(
    BitmapShapeImpl result,
    Dictionary<string, string> styleProperties)
  {
    string str1;
    if (styleProperties.TryGetValue("height", out str1))
      result.Height = (int) result.AppImplementation.ConvertUnits(Convert.ToDouble(str1.Substring(0, str1.Length - 2)), MeasureUnits.Point, MeasureUnits.Pixel);
    string str2;
    if (!styleProperties.TryGetValue("width", out str2))
      return;
    result.Width = (int) result.AppImplementation.ConvertUnits(Convert.ToDouble(str2.Substring(0, str2.Length - 2)), MeasureUnits.Point, MeasureUnits.Pixel);
  }

  private int ParseShapeId(string shapeId)
  {
    int result = shapeId.IndexOf("_s");
    int shapeId1 = -1;
    if (result >= 0 && int.TryParse(shapeId.Substring(result + 2), out result))
      shapeId1 = result;
    return shapeId1;
  }

  private void ParseClientData(XmlReader reader, BitmapShapeImpl shape)
  {
    if (reader.LocalName != "ClientData")
      throw new XmlException("Unexpected xml token");
    if (shape == null)
      throw new ArgumentNullException(nameof (shape));
    reader.Read();
    shape.IsMoveWithCell = true;
    shape.IsSizeWithCell = true;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "MoveWithCells":
            shape.IsMoveWithCell = VmlTextBoxBaseParser.ParseBoolOrEmpty(reader, true);
            continue;
          case "SizeWithCells":
            shape.IsSizeWithCell = VmlTextBoxBaseParser.ParseBoolOrEmpty(reader, true);
            continue;
          case "Anchor":
            this.ParseAnchor(reader, (ShapeImpl) shape);
            continue;
          case "DDE":
            shape.IsDDE = true;
            reader.Read();
            continue;
          case "Camera":
            shape.IsCamera = true;
            reader.Read();
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    reader.Read();
  }

  private void ParseImageData(
    XmlReader reader,
    string shapeName,
    BitmapShapeImpl shape,
    RelationCollection relations,
    string parentItemPath)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (shape == null)
      throw new ArgumentNullException(nameof (shape));
    if (shapeName == null || shapeName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (shapeName));
    if (parentItemPath == null || parentItemPath.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (parentItemPath));
    if (relations == null)
      throw new ArgumentNullException(nameof (relations));
    if (reader.LocalName != "imagedata")
      throw new XmlException("Unexpected xml tag.");
    string id = reader.MoveToAttribute("relid", "urn:schemas-microsoft-com:office:office") ? reader.Value : throw new XmlException("Wrong xml format.");
    Relation relation = relations[id];
    if (relation == null)
      throw new XmlException("Cannot find required relation.");
    WorksheetBaseImpl worksheet = shape.Worksheet;
    string strFullPath = FileDataHolder.CombinePath(parentItemPath, relation.Target);
    Image image = worksheet.DataHolder.ParentHolder.GetImage(strFullPath);
    if (shape.ParentShapes is HeaderFooterShapeCollection)
    {
      worksheet.HeaderFooterShapes.SetPicture(shapeName, image, -1, false, shape.PreserveStyleString);
    }
    else
    {
      shape.Picture = image;
      worksheet.InnerShapes.Add((IShape) shape);
    }
    reader.Skip();
  }
}
