// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes.BitmapShapeSerializator
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Drawing;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Implementation.XmlSerialization.Charts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes;

public class BitmapShapeSerializator : DrawingShapeSerializator
{
  private Dictionary<string, Stream> m_svgDataCollection;

  private Dictionary<string, Stream> SvgDataCollection
  {
    get
    {
      if (this.m_svgDataCollection == null)
        this.m_svgDataCollection = new Dictionary<string, Stream>();
      return this.m_svgDataCollection;
    }
  }

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
    bool flag1 = !(shape.Worksheet is WorksheetImpl);
    bool flag2 = false;
    if (shape.EnableAlternateContent)
      Excel2007Serializator.WriteAlternateContentHeader(writer);
    string localName;
    string str;
    if (flag1)
    {
      localName = "relSizeAnchor";
      str = "http://schemas.openxmlformats.org/drawingml/2006/chartDrawing";
    }
    else if (!(shape.Workbook as WorkbookImpl).IsCreated && bitmapShapeImpl.ClientAnchor.OneCellAnchor)
    {
      flag2 = true;
      localName = "oneCellAnchor";
      str = "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing";
    }
    else
    {
      localName = "twoCellAnchor";
      str = "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing";
    }
    writer.WriteStartElement(localName, str);
    if (!flag1 && !flag2)
      writer.WriteAttributeString("editAs", DrawingShapeSerializator.GetEditAsValue((ShapeImpl) bitmapShapeImpl));
    this.SerializeAnchorPoint(writer, "from", shape.LeftColumn, shape.LeftColumnOffset, shape.TopRow, shape.TopRowOffset, shape.Worksheet, str);
    if (!flag2)
    {
      this.SerializeAnchorPoint(writer, "to", shape.RightColumn, shape.RightColumnOffset, shape.BottomRow, shape.BottomRowOffset, shape.Worksheet, str);
    }
    else
    {
      writer.WriteStartElement("ext", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
      int num1 = (int) ApplicationImpl.ConvertFromPixel((double) shape.Width, MeasureUnits.EMU);
      writer.WriteAttributeString("cx", num1.ToString());
      int num2 = (int) ApplicationImpl.ConvertFromPixel((double) shape.Height, MeasureUnits.EMU);
      writer.WriteAttributeString("cy", num2.ToString());
      writer.WriteEndElement();
    }
    this.SerializePicture(writer, bitmapShapeImpl, relationId, holder, str);
    writer.WriteEndElement();
    if (!shape.EnableAlternateContent)
      return;
    Excel2007Serializator.WriteAlternateContentFooter(writer);
  }

  internal void SerializePicture(
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
    string onAction = picture.OnAction;
    if (onAction != null)
      writer.WriteAttributeString("macro", onAction);
    this.SerializeNonVisualProperties(writer, picture, holder, mainNamespace);
    this.SerializeBlipFill(writer, picture, relationId, mainNamespace);
    this.SerializeShapeProperties(writer, picture, mainNamespace);
    writer.WriteEndElement();
    if (picture.Group != null || !(mainNamespace == "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing"))
      return;
    writer.WriteStartElement("clientData", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    writer.WriteEndElement();
  }

  internal void AddSvg(
    BitmapShapeImpl picture,
    WorksheetDataHolder holder,
    RelationCollection relations)
  {
    if (picture == null)
      throw new ArgumentNullException(nameof (picture));
    bool isSvgExternalLink;
    string str1 = (isSvgExternalLink = picture.IsSvgExternalLink) ? picture.ExternalLink : picture.SvgPicturePath;
    string id = picture.SvgRelId;
    if (string.IsNullOrEmpty(picture.SvgRelId))
    {
      if (picture.IsSvgUpdated)
        holder.ParentHolder.Archive.RemoveItem(picture.SvgPicturePath);
      int num = (int) picture.BlipId - 1;
      FileDataHolder parentHolder = holder.ParentHolder;
      string str2 = holder.ParentHolder.RegisterSvgContentType();
      Stream stream = (Stream) null;
      string pattern;
      Regex itemRegex;
      do
      {
        ++num;
        pattern = $"xl/media/image{num}.";
        itemRegex = new Regex(pattern);
      }
      while (parentHolder.Archive.Find(itemRegex) != -1);
      str1 = pattern + str2;
      id = relations.GenerateRelationId();
      picture.SvgRelId = id;
      this.SvgDataCollection.TryGetValue(str1, out stream);
      if (stream != picture.SvgData)
      {
        Stream svgData = picture.SvgData;
        svgData.Position = 0L;
        int int32 = Convert.ToInt32(svgData.Length);
        byte[] buffer = new byte[int32];
        svgData.Read(buffer, 0, int32);
        this.SvgDataCollection.Add(str1, picture.SvgData);
        holder.ParentHolder.Archive.AddItem(str1, (Stream) new MemoryStream(buffer), true, FileAttributes.Archive);
      }
    }
    if (!isSvgExternalLink)
      str1 = "/" + str1;
    relations[id] = new Relation(str1, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", isSvgExternalLink);
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
    writer.WriteStartElement("spPr", mainNamespace);
    DrawingShapeSerializator.SerializeForm(writer, "http://schemas.openxmlformats.org/drawingml/2006/main", "http://schemas.openxmlformats.org/drawingml/2006/main", picture);
    this.SerializePreservedNode(writer, picture, "prstGeom");
    this.SerializePreservedNode(writer, picture, "custGeom");
    if (picture.Fill.Visible)
    {
      string[] strArray = new string[5]
      {
        "solidFill",
        "pattFill",
        "grpFill",
        "blipFill",
        "gradFill"
      };
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (picture.PreservedElements.ContainsKey(strArray[index]))
          this.SerializeFillProperties(writer, picture, strArray[index]);
      }
    }
    else
      writer.WriteElementString("noFill", "http://schemas.openxmlformats.org/drawingml/2006/main", string.Empty);
    if (picture.Line.Visible)
      DrawingShapeSerializator.SerializeLineSettings(writer, picture.Line, picture.Workbook);
    this.SerializePreservedNode(writer, picture, "effectLst");
    this.SerializePreservedNode(writer, picture, "scene3d");
    this.SerializePreservedNode(writer, picture, "sp3d");
    this.SerializePreservedNode(writer, picture, "extLst");
    writer.WriteEndElement();
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
    if (picture.Camera != null)
    {
      writer.WriteStartElement("extLst", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteStartElement("ext", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteAttributeString("uri", "{84589F7E-364E-4C9E-8A38-B11213B215E9}");
      writer.WriteStartElement("a14", "cameraTool", (string) null);
      writer.WriteAttributeString("cellRange", picture.Camera.CellRange);
      string str = $"_x0000_s{picture.Camera.ShapeID}";
      writer.WriteAttributeString("spid", str);
      writer.WriteEndElement();
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
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
    if (!picture.IsSvgExternalLink && !string.IsNullOrEmpty(picture.ExternalLink))
      writer.WriteAttributeString("link", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", relationId);
    else
      writer.WriteAttributeString("embed", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", relationId);
    this.SerializePreservedNode(writer, picture, "alphaBiLevel");
    this.SerializePreservedNode(writer, picture, "alphaCeiling");
    this.SerializePreservedNode(writer, picture, "alphaFloor");
    if (picture.Threshold > 0)
    {
      writer.WriteStartElement("biLevel", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteAttributeString("thresh", Helper.ToString(picture.Threshold));
      writer.WriteEndElement();
    }
    if (picture.GrayScale)
    {
      writer.WriteStartElement("grayscl", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteEndElement();
    }
    if (picture.DuoTone != null && picture.DuoTone.Count == 2)
      BitmapShapeSerializator.SerializeDuoTone(writer, picture);
    if (picture.ColorChange != null && picture.ColorChange.Count == 2)
      BitmapShapeSerializator.SerializeColorChange(writer, picture);
    if (picture.Amount != 100000)
    {
      writer.WriteStartElement("alphaModFix", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteAttributeString("amt", Helper.ToString(picture.Amount));
      writer.WriteEndElement();
    }
    this.SerializePreservedNode(writer, picture, "fillOverlay");
    this.SerializePreservedNode(writer, picture, "alphaMod");
    this.SerializePreservedNode(writer, picture, "alphaInv");
    this.SerializePreservedNode(writer, picture, "alphaRepl");
    this.SerializePreservedNode(writer, picture, "blur");
    this.SerializePreservedNode(writer, picture, "hsl");
    this.SerializePreservedNode(writer, picture, "lum");
    this.SerializePreservedNode(writer, picture, "tint");
    this.SerializeExtensionList(writer, picture);
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

  private static void SerializeDuoTone(XmlWriter xmlWriter, BitmapShapeImpl picture)
  {
    if (picture == null || picture.DuoTone.Count == 0)
      return;
    xmlWriter.WriteStartElement("duotone", "http://schemas.openxmlformats.org/drawingml/2006/main");
    for (int index = 0; index < picture.DuoTone.Count; ++index)
    {
      ColorObject colorObject = picture.DuoTone[index];
      ChartSerializatorCommon.SerializeRgbColor(xmlWriter, colorObject.GetRGB(picture.Workbook));
    }
    xmlWriter.WriteEndElement();
  }

  private static void SerializeColorChange(XmlWriter xmlWriter, BitmapShapeImpl picture)
  {
    xmlWriter.WriteStartElement("clrChange", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (!picture.IsUseAlpha)
      xmlWriter.WriteAttributeString("useA", "0");
    for (int index = 0; index < picture.ColorChange.Count; ++index)
    {
      ColorObject colorObject = picture.ColorChange[index];
      xmlWriter.WriteStartElement(index == 0 ? "clrFrom" : "clrTo", "http://schemas.openxmlformats.org/drawingml/2006/main");
      Color rgb = colorObject.GetRGB(picture.Workbook);
      ChartSerializatorCommon.SerializeRgbColor(xmlWriter, rgb, (double) rgb.A / (double) byte.MaxValue);
      xmlWriter.WriteEndElement();
    }
    xmlWriter.WriteEndElement();
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
    bool isExternal = false;
    string str;
    if (picture.BlipId > 0U)
    {
      str = '/'.ToString() + holder.ParentHolder.GetImageItemName((int) picture.BlipId - 1);
      id = drawingsRelations.FindRelationByTarget(str);
    }
    else
      str = "NULL";
    if (!picture.IsSvgExternalLink && !string.IsNullOrEmpty(picture.ExternalLink))
    {
      str = picture.ExternalLink;
      isExternal = true;
      id = drawingsRelations.FindRelationByTarget(str);
    }
    if (id == null)
    {
      id = drawingsRelations.GenerateRelationId();
      drawingsRelations[id] = new Relation(str, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", isExternal);
    }
    if (picture.IsSvgExternalLink || picture.SvgData != null)
    {
      if (picture.SvgRelId == null && picture.SvgPicturePath == null && this.SvgDataCollection.Count > 0)
      {
        string svgPicturePath = this.GetSvgPicturePath(picture.SvgData, holder);
        picture.SvgRelId = drawingsRelations.FindRelationByTarget("/" + svgPicturePath);
        picture.SvgPicturePath = svgPicturePath;
      }
      this.AddSvg(picture, holder, drawingsRelations);
    }
    return id;
  }

  internal string GetSvgPicturePath(Stream svgStream, WorksheetDataHolder holder)
  {
    string svgPicturePath = (string) null;
    foreach (KeyValuePair<string, Stream> svgData in this.SvgDataCollection)
    {
      Stream stream = svgData.Value;
      if (stream != null && stream.Equals((object) svgStream))
      {
        svgPicturePath = svgData.Key;
        break;
      }
    }
    return svgPicturePath;
  }

  private void SerializeExtensionList(XmlWriter xmlWriter, BitmapShapeImpl picture)
  {
    xmlWriter.WriteStartElement("a", "extLst", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (picture.PreservedElements.ContainsKey("imgProps"))
    {
      xmlWriter.WriteStartElement("a", "ext", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteAttributeString("uri", "{BEBA8EAE-BF5A-486C-A8C5-ECC9F3942E4B}");
      this.SerializePreservedNode(xmlWriter, picture, "imgProps");
      xmlWriter.WriteEndElement();
    }
    if (picture.PreservedElements.ContainsKey("useLocalDpi"))
    {
      xmlWriter.WriteStartElement("a", "ext", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteAttributeString("uri", "{28A0092B-C50C-407E-A947-70E740481C1C}");
      this.SerializePreservedNode(xmlWriter, picture, "useLocalDpi");
      xmlWriter.WriteEndElement();
    }
    if (picture.SvgData != null || picture.IsSvgExternalLink)
    {
      xmlWriter.WriteStartElement("a", "ext", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteAttributeString("uri", "{96DAC541-7B7A-43D3-8B79-37D633B846F1}");
      xmlWriter.WriteStartElement("asvg", "svgBlip", "http://schemas.microsoft.com/office/drawing/2016/SVG/main");
      if (string.IsNullOrEmpty(picture.ExternalLink))
        xmlWriter.WriteAttributeString("embed", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", picture.SvgRelId);
      else
        xmlWriter.WriteAttributeString("link", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", picture.SvgRelId);
      xmlWriter.WriteEndElement();
      xmlWriter.WriteEndElement();
    }
    xmlWriter.WriteEndElement();
  }

  private void SerializePreservedNode(
    XmlWriter xmlTextWriter,
    BitmapShapeImpl picture,
    string node)
  {
    Stream data;
    if (picture.PreservedElements.TryGetValue(node, out data))
    {
      if (data == null || data.Length <= 0L)
        return;
      Excel2007Serializator.SerializeStream(xmlTextWriter, data, "root");
    }
    else
    {
      if (!(node == "prstGeom"))
        return;
      this.SerializePresetGeometry(xmlTextWriter, picture.PresetGeometry);
    }
  }

  private void SerializeFillProperties(
    XmlWriter xmlTextWriter,
    BitmapShapeImpl picture,
    string node)
  {
    this.SerializePreservedNode(xmlTextWriter, picture, node);
  }

  internal override void Clear()
  {
    if (this.m_svgDataCollection == null)
      return;
    this.m_svgDataCollection.Clear();
    this.m_svgDataCollection = (Dictionary<string, Stream>) null;
  }
}
