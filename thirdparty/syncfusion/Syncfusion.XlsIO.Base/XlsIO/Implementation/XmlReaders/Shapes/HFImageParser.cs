// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlReaders.Shapes.HFImageParser
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Compression.Zip;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlReaders.Shapes;

public class HFImageParser : ShapeParser
{
  internal const int MaxCropValue = 65536 /*0x010000*/;
  private Regex m_cropPattern = new Regex("((?<number>-?\\d+\\s*)(?<letter>[a-zA-Z%]*))");

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
    if (reader.MoveToAttribute("alt"))
      bitmapShapeImpl.AlternativeText = reader.Value;
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
          case "FmlaMacro":
            shape.FormulaMacroStream = ShapeParser.ReadNodeAsStream(reader);
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
    WorkbookImpl workbook = (WorkbookImpl) shape.Workbook;
    ZipArchiveItem zipArchiveItem = workbook.DataHolder[relation, parentItemPath];
    WorksheetBaseImpl worksheet = shape.Worksheet;
    string strFullPath = FileDataHolder.CombinePath(parentItemPath, relation.Target);
    Image image = worksheet.DataHolder.ParentHolder.GetImage(strFullPath);
    if (shape.ParentShapes is HeaderFooterShapeCollection)
    {
      shape = worksheet.HeaderFooterShapes.SetPicture(shapeName, image, -1, false, shape.PreserveStyleString) as BitmapShapeImpl;
    }
    else
    {
      shape.Picture = image;
      workbook.ShapesData.Pictures[(int) shape.BlipId - 1].PicturePath = zipArchiveItem.ItemName;
      worksheet.InnerShapes.Add((IShape) shape);
    }
    if (reader.MoveToAttribute("cropleft"))
      shape.CropLeftOffset = this.GetCropValue(reader.Value);
    if (reader.MoveToAttribute("croptop"))
      shape.CropTopOffset = this.GetCropValue(reader.Value);
    if (reader.MoveToAttribute("cropright"))
      shape.CropRightOffset = this.GetCropValue(reader.Value);
    if (reader.MoveToAttribute("cropbottom"))
      shape.CropBottomOffset = this.GetCropValue(reader.Value);
    reader.Skip();
  }

  internal override ShapeImpl CreateShape(ShapeCollectionBase shapes)
  {
    BitmapShapeImpl shape = new BitmapShapeImpl(shapes.Application, (object) shapes);
    shape.VmlShape = true;
    return (ShapeImpl) shape;
  }

  private int GetCropValue(string value)
  {
    if (this.m_cropPattern.IsMatch(value))
    {
      Match match = this.m_cropPattern.Match(value);
      if (match.Groups["letter"].Value.Equals("f"))
        return Convert.ToInt32(match.Groups["number"].Value);
      if (match.Groups["letter"].Value.Equals("%"))
        return Convert.ToInt32((double) Convert.ToInt32(match.Groups["number"].Value) * 655.36);
      if (match.Groups["letter"].Value.Equals(string.Empty))
        return Convert.ToInt32(Convert.ToDouble(value) * 65536.0);
    }
    return 0;
  }
}
