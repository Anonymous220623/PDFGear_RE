// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.DrawingSerializator
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.OfficeChart;
using Syncfusion.Presentation.Charts;
using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.RichText;
using Syncfusion.Presentation.SlideImplementation;
using Syncfusion.Presentation.SmartArtImplementation;
using Syncfusion.Presentation.TableImplementation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.Presentation;

internal class DrawingSerializator
{
  private const string PPrefix = "p";
  private const string APrefix = "a";
  private const string RPrefix = "r";
  private const string CPrefix = "c";
  private const string DGMPrefix = "dgm";
  private const string DSPPrefix = "dsp";
  private const string CXPrefix = "cx";
  private const string MCPrefix = "mc";

  internal static void SerializeShapeTree(XmlWriter xmlWriter, BaseSlide slide)
  {
    xmlWriter.WriteStartElement("p", "spTree", "http://schemas.openxmlformats.org/presentationml/2006/main");
    xmlWriter.WriteStartElement("p", "nvGrpSpPr", "http://schemas.openxmlformats.org/presentationml/2006/main");
    xmlWriter.WriteStartElement("p", "cNvPr", "http://schemas.openxmlformats.org/presentationml/2006/main");
    xmlWriter.WriteAttributeString("id", "1");
    xmlWriter.WriteAttributeString("name", "");
    xmlWriter.WriteEndElement();
    xmlWriter.WriteStartElement("p", "cNvGrpSpPr", "http://schemas.openxmlformats.org/presentationml/2006/main");
    xmlWriter.WriteEndElement();
    xmlWriter.WriteStartElement("p", "nvPr", "http://schemas.openxmlformats.org/presentationml/2006/main");
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
    xmlWriter.WriteStartElement("p", "grpSpPr", "http://schemas.openxmlformats.org/presentationml/2006/main");
    xmlWriter.WriteStartElement("a", "xfrm", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteStartElement("a", "off", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("x", "0");
    xmlWriter.WriteAttributeString("y", "0");
    xmlWriter.WriteEndElement();
    xmlWriter.WriteStartElement("a", "ext", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("cx", "0");
    xmlWriter.WriteAttributeString("cy", "0");
    xmlWriter.WriteEndElement();
    xmlWriter.WriteStartElement("a", "chOff", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("x", "0");
    xmlWriter.WriteAttributeString("y", "0");
    xmlWriter.WriteEndElement();
    xmlWriter.WriteStartElement("a", "chExt", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("cx", "0");
    xmlWriter.WriteAttributeString("cy", "0");
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
    DrawingSerializator.SerializeShapes(xmlWriter, slide);
    xmlWriter.WriteEndElement();
  }

  internal static void Write(XmlWriter xmlWriter, Shape shape)
  {
    DrawingSerializator.SerializeGraphicFrame(xmlWriter, shape);
    if (shape.ShapeType != ShapeType.GraphicFrame)
      DrawingSerializator.SerializePicElement(xmlWriter, shape);
    DrawingSerializator.SerializeConShape(xmlWriter, shape);
  }

  private static void SerializeGroupShape(XmlWriter xmlWriter, Shape shape)
  {
    if (shape.ShapeType != ShapeType.GrpSp)
      return;
    xmlWriter.WriteStartElement("p", "grpSp", "http://schemas.openxmlformats.org/presentationml/2006/main");
    DrawingSerializator.SerializeNonVisualGrpSpProp(xmlWriter, shape);
    DrawingSerializator.SerializeGrpSpProp(xmlWriter, shape);
    foreach (IShape shape1 in (IEnumerable<ISlideItem>) ((GroupShape) shape).Shapes)
    {
      if (shape1 != null)
      {
        switch (((Shape) shape1).ShapeType)
        {
          case ShapeType.Sp:
          case ShapeType.GrpSp:
          case ShapeType.GraphicFrame:
          case ShapeType.CxnSp:
          case ShapeType.Pic:
          case ShapeType.AlternateContent:
          case ShapeType.Chart:
            DrawingSerializator.Write(xmlWriter, (Shape) shape1);
            continue;
          default:
            continue;
        }
      }
    }
    xmlWriter.WriteEndElement();
  }

  private static void SerializeRichText(XmlWriter xmlWriter, Shape shape)
  {
    Serializator.SerializeTextBody(xmlWriter, (TextBody) shape.TextBody);
  }

  internal static void SerializePicElement(XmlWriter xmlWriter, Shape shape)
  {
    if (shape.ShapeType != ShapeType.Pic && shape.DrawingType != DrawingType.OleObject)
      return;
    xmlWriter.WriteStartElement("p", "pic", "http://schemas.openxmlformats.org/presentationml/2006/main");
    DrawingSerializator.SerializeGraphicAttributes(xmlWriter, shape);
    DrawingSerializator.SerializeNonVisualPicProp(xmlWriter, shape);
    if (shape.PreservedElements != null)
      DrawingSerializator.WriteRawData(xmlWriter, shape.PreservedElements, "AlternateContent");
    DrawingSerializator.SerializeBlipFill(xmlWriter, shape);
    DrawingSerializator.SerializeGrpSpProp(xmlWriter, shape);
    xmlWriter.WriteEndElement();
  }

  private static void SerializeFrame(XmlWriter xmlWriter, Shape shape)
  {
    bool flipVertical = shape.ShapeFrame.FlipVertical;
    bool flipHorizontal = shape.ShapeFrame.FlipHorizontal;
    if (shape.DrawingType == DrawingType.Table || shape.DrawingType == DrawingType.OleObject || shape.DrawingType == DrawingType.Chart || shape.DrawingType == DrawingType.SmartArt && shape.ShapeType == ShapeType.GraphicFrame)
      xmlWriter.WriteStartElement("p", "xfrm", "http://schemas.openxmlformats.org/presentationml/2006/main");
    else
      xmlWriter.WriteStartElement("a", "xfrm", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (shape.ShapeFrame.Rotation != -1)
      xmlWriter.WriteAttributeString("rot", Helper.ToString(shape.ShapeFrame.Rotation));
    if (flipHorizontal)
      xmlWriter.WriteAttributeString("flipH", "1");
    if (flipVertical)
      xmlWriter.WriteAttributeString("flipV", "1");
    xmlWriter.WriteStartElement("a", "off", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("x", Helper.ToString(shape.ShapeFrame.OffsetX));
    xmlWriter.WriteAttributeString("y", Helper.ToString(shape.ShapeFrame.OffsetY));
    xmlWriter.WriteEndElement();
    xmlWriter.WriteStartElement("a", "ext", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("cx", Helper.ToString(shape.ShapeFrame.OffsetCX));
    xmlWriter.WriteAttributeString("cy", Helper.ToString(shape.ShapeFrame.OffsetCY));
    xmlWriter.WriteEndElement();
    if (shape is GroupShape)
    {
      GroupShape groupShape = (GroupShape) shape;
      if (groupShape.ShapeFrame.IsChildOffsetSet)
      {
        xmlWriter.WriteStartElement("a", "chOff", "http://schemas.openxmlformats.org/drawingml/2006/main");
        xmlWriter.WriteAttributeString("x", Helper.ToString(groupShape.ShapeFrame.ChOffsetX));
        xmlWriter.WriteAttributeString("y", Helper.ToString(groupShape.ShapeFrame.ChOffsetY));
        xmlWriter.WriteEndElement();
        xmlWriter.WriteStartElement("a", "chExt", "http://schemas.openxmlformats.org/drawingml/2006/main");
        xmlWriter.WriteAttributeString("cx", Helper.ToString(groupShape.ShapeFrame.ChOffsetCX));
        xmlWriter.WriteAttributeString("cy", Helper.ToString(groupShape.ShapeFrame.ChOffsetCY));
        xmlWriter.WriteEndElement();
      }
    }
    xmlWriter.WriteEndElement();
  }

  private static void SerializePresetGeometry(XmlWriter xmlWriter, Shape shape)
  {
    xmlWriter.WriteStartElement("a", "prstGeom", "http://schemas.openxmlformats.org/drawingml/2006/main");
    AutoShapeConstant autoShapeConstant = AutoShapeHelper.GetAutoShapeConstant(shape.AutoShapeType);
    xmlWriter.WriteAttributeString("prst", autoShapeConstant != AutoShapeConstant.Index187 ? AutoShapeHelper.GetAutoShapeString(autoShapeConstant) : "rect");
    xmlWriter.WriteStartElement("a", "avLst", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (shape.GetGuideList().Count != 0)
      DrawingSerializator.SerializeGuideList(xmlWriter, shape);
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
  }

  private static void SerializeGuideList(XmlWriter xmlWriter, Shape shape)
  {
    foreach (KeyValuePair<string, string> guide in shape.GetGuideList())
    {
      xmlWriter.WriteStartElement("a", "gd", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteAttributeString("name", guide.Key);
      xmlWriter.WriteAttributeString("fmla", guide.Value);
      xmlWriter.WriteEndElement();
    }
  }

  private static void SerializeAVList(XmlWriter xmlWriter, Shape shape)
  {
    foreach (KeyValuePair<string, string> av in shape.GetAvList())
    {
      xmlWriter.WriteStartElement("a", "gd", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteAttributeString("name", av.Key);
      xmlWriter.WriteAttributeString("fmla", av.Value);
      xmlWriter.WriteEndElement();
    }
  }

  private static void SerializeNonVisualGrpSpProp(XmlWriter xmlWriter, Shape shape)
  {
    xmlWriter.WriteStartElement("p", "nvGrpSpPr", "http://schemas.openxmlformats.org/presentationml/2006/main");
    DrawingSerializator.SerializeNonVisualDrngProp(xmlWriter, shape);
    xmlWriter.WriteStartElement("p", "cNvGrpSpPr", "http://schemas.openxmlformats.org/presentationml/2006/main");
    xmlWriter.WriteStartElement("a", "grpSpLocks", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
    xmlWriter.WriteStartElement("p", "nvPr", "http://schemas.openxmlformats.org/presentationml/2006/main");
    DrawingSerializator.WriteCustomDataList(xmlWriter, shape);
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
  }

  private static void SerializeFillElement(XmlWriter xmlWriter, Shape shape)
  {
    Serializator.SerializeFillProperties(xmlWriter, shape.GetFillFormat(), shape.BaseSlide.Presentation);
  }

  internal static void SerializeGrpSpProp(XmlWriter xmlWriter, Shape shape)
  {
    bool isGroup = shape.IsGroup;
    if (shape.DrawingType == DrawingType.SmartArt)
      xmlWriter.WriteStartElement("dgm", "spPr", "http://schemas.openxmlformats.org/drawingml/2006/diagram");
    else if (shape.ShapeType == ShapeType.Point)
      xmlWriter.WriteStartElement("dsp", "spPr", "http://schemas.microsoft.com/office/drawing/2008/diagram");
    else
      xmlWriter.WriteStartElement("p", isGroup ? "grpSpPr" : "spPr", "http://schemas.openxmlformats.org/presentationml/2006/main");
    string str = Helper.ToString(shape.ShapeFrame.BlackWhiteMode);
    if (str != null)
      xmlWriter.WriteAttributeString("bwMode", str);
    if (shape.ShapeFrame.GetIsFrameChanged())
      DrawingSerializator.SerializeFrame(xmlWriter, shape);
    if (!isGroup)
    {
      if (!shape.IsCustomGeometry)
      {
        if (shape.ShapeType == ShapeType.Pic || shape.DrawingType == DrawingType.OleObject || shape.DrawingType == DrawingType.TextBox || shape.AutoShapeType != ~AutoShapeType.Unknown)
          DrawingSerializator.SerializePresetGeometry(xmlWriter, shape);
      }
      else if (!shape.IsPresetGeometry && shape.IsCustomGeometry)
        DrawingSerializator.SerializeCustomGeometry(xmlWriter, shape);
    }
    if (shape.IsGroupFill && shape.Group != null && shape.Fill.FillType == FillType.Automatic)
    {
      xmlWriter.WriteStartElement("a", "grpFill", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteEndElement();
    }
    DrawingSerializator.SerializeFillElement(xmlWriter, shape);
    DrawingSerializator.SerializeLine(xmlWriter, shape);
    EffectList effectList = shape.EffectList;
    if (effectList != null)
    {
      xmlWriter.WriteStartElement("a", "effectLst", "http://schemas.openxmlformats.org/drawingml/2006/main");
      Serializator.SerializeEffectList(xmlWriter, effectList);
      xmlWriter.WriteEndElement();
    }
    if (shape.PreservedElements.ContainsKey("scene3d"))
      DrawingSerializator.WriteRawData(xmlWriter, shape.PreservedElements, "scene3d");
    if (shape.PreservedElements.ContainsKey("sp3d"))
      DrawingSerializator.WriteRawData(xmlWriter, shape.PreservedElements, "sp3d");
    xmlWriter.WriteEndElement();
  }

  private static void SerializeCustomGeometry(XmlWriter xmlWriter, Shape shape)
  {
    xmlWriter.WriteStartElement("a", "custGeom", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteStartElement("a", "avLst", "http://schemas.openxmlformats.org/drawingml/2006/main");
    DrawingSerializator.SerializeAVList(xmlWriter, shape);
    xmlWriter.WriteEndElement();
    xmlWriter.WriteStartElement("a", "gdLst", "http://schemas.openxmlformats.org/drawingml/2006/main");
    DrawingSerializator.SerializeGuideList(xmlWriter, shape);
    xmlWriter.WriteEndElement();
    if (shape.PreservedElements != null && shape.PreservedElements.Count != 0)
    {
      if (shape.PreservedElements.ContainsKey("ahLst"))
        DrawingSerializator.WriteRawData(xmlWriter, shape.PreservedElements, "ahLst");
      if (shape.PreservedElements.ContainsKey("cxnLst"))
        DrawingSerializator.WriteRawData(xmlWriter, shape.PreservedElements, "cxnLst");
      if (shape.PreservedElements.ContainsKey("rect"))
        DrawingSerializator.WriteRawData(xmlWriter, shape.PreservedElements, "rect");
    }
    xmlWriter.WriteStartElement("a", "pathLst", "http://schemas.openxmlformats.org/drawingml/2006/main");
    foreach (Path2D path2D in shape.Path2DList)
    {
      xmlWriter.WriteStartElement("a", "path", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteAttributeString("w", Helper.ToString(path2D.Width));
      xmlWriter.WriteAttributeString("h", Helper.ToString(path2D.Height));
      xmlWriter.WriteAttributeString("fill", Helper.ToString(path2D.FillMode));
      if (!path2D.IsStroke)
        xmlWriter.WriteAttributeString("stroke", "0");
      DrawingSerializator.SerializePathElements(xmlWriter, path2D.PathElements);
      xmlWriter.WriteEndElement();
    }
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
  }

  private static void SerializePathElements(XmlWriter xmlWriter, List<double> pathElements)
  {
    for (int index1 = 0; index1 < pathElements.Count; ++index1)
    {
      switch (pathElements[index1])
      {
        case 1.0:
          xmlWriter.WriteStartElement("a", "close", "http://schemas.openxmlformats.org/drawingml/2006/main");
          xmlWriter.WriteEndElement();
          ++index1;
          break;
        case 2.0:
          xmlWriter.WriteStartElement("a", "moveTo", "http://schemas.openxmlformats.org/drawingml/2006/main");
          DrawingSerializator.SerializePoint(xmlWriter, pathElements, index1 + 2);
          xmlWriter.WriteEndElement();
          index1 += 3;
          break;
        case 3.0:
          xmlWriter.WriteStartElement("a", "lnTo", "http://schemas.openxmlformats.org/drawingml/2006/main");
          DrawingSerializator.SerializePoint(xmlWriter, pathElements, index1 + 2);
          xmlWriter.WriteEndElement();
          index1 += 3;
          break;
        case 4.0:
          xmlWriter.WriteStartElement("a", "arcTo", "http://schemas.openxmlformats.org/drawingml/2006/main");
          int index2 = index1 + 2;
          xmlWriter.WriteAttributeString("wR", Helper.ToString((int) pathElements[index2]));
          xmlWriter.WriteAttributeString("hR", Helper.ToString((int) pathElements[index2 + 1]));
          xmlWriter.WriteAttributeString("stAng", Helper.ToString((int) pathElements[index2 + 2]));
          xmlWriter.WriteAttributeString("swAng", Helper.ToString((int) pathElements[index2 + 3]));
          xmlWriter.WriteEndElement();
          index1 = index2 + 3;
          break;
        case 5.0:
          xmlWriter.WriteStartElement("a", "quadBezTo", "http://schemas.openxmlformats.org/drawingml/2006/main");
          DrawingSerializator.SerializePoint(xmlWriter, pathElements, index1 + 2);
          DrawingSerializator.SerializePoint(xmlWriter, pathElements, index1 + 4);
          DrawingSerializator.SerializePoint(xmlWriter, pathElements, index1 + 6);
          xmlWriter.WriteEndElement();
          index1 += 5;
          break;
        case 6.0:
          xmlWriter.WriteStartElement("a", "cubicBezTo", "http://schemas.openxmlformats.org/drawingml/2006/main");
          DrawingSerializator.SerializePoint(xmlWriter, pathElements, index1 + 2);
          DrawingSerializator.SerializePoint(xmlWriter, pathElements, index1 + 4);
          DrawingSerializator.SerializePoint(xmlWriter, pathElements, index1 + 6);
          xmlWriter.WriteEndElement();
          index1 += 7;
          break;
      }
    }
  }

  private static void SerializePoint(XmlWriter xmlWriter, List<double> pathElements, int k)
  {
    xmlWriter.WriteStartElement("a", "pt", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("x", Helper.ToString((int) pathElements[k]));
    xmlWriter.WriteAttributeString("y", Helper.ToString((int) pathElements[k + 1]));
    xmlWriter.WriteEndElement();
  }

  private static void SerializeLine(XmlWriter xmlWriter, Shape shape)
  {
    if (shape.ShapeType == ShapeType.Point && !shape.HasLineProperties)
      return;
    if (shape.AutoShapeType != AutoShapeType.Unknown)
    {
      xmlWriter.WriteStartElement("a", "ln", "http://schemas.openxmlformats.org/drawingml/2006/main");
      Serializator.SerializeLineFillProperties(xmlWriter, (LineFormat) shape.LineFormat, shape.BaseSlide.Presentation);
      xmlWriter.WriteEndElement();
    }
    else
    {
      xmlWriter.WriteStartElement("a", "ln", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteStartElement("a", "noFill", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteEndElement();
      xmlWriter.WriteEndElement();
    }
  }

  private static void SerializeBlipFill(XmlWriter xmlWriter, Shape shape)
  {
    if (!(shape is Picture pic) && shape is OleObject)
      pic = ((OleObject) shape).OlePicture;
    if (pic != null && pic.ImageData == null && pic.LinkId == null)
    {
      if (pic.PreservedElements.ContainsKey("AlternateContent"))
        return;
      xmlWriter.WriteStartElement("p", "blipFill", "http://schemas.openxmlformats.org/presentationml/2006/main");
      xmlWriter.WriteEndElement();
    }
    else
    {
      xmlWriter.WriteStartElement("p", "blipFill", "http://schemas.openxmlformats.org/presentationml/2006/main");
      if (pic != null)
      {
        xmlWriter.WriteStartElement("a", "blip", "http://schemas.openxmlformats.org/drawingml/2006/main");
        if (pic.ImageData != null)
          pic.AddImageToArchive(false);
        if (pic.IsLink)
          xmlWriter.WriteAttributeString("r", "link", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", pic.LinkId);
        if (pic.IsEmbed)
          xmlWriter.WriteAttributeString("r", "embed", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", pic.EmbedId);
        if (pic.Threshold >= 0)
        {
          xmlWriter.WriteStartElement("a", "biLevel", "http://schemas.openxmlformats.org/drawingml/2006/main");
          xmlWriter.WriteAttributeString("thresh", Helper.ToString(pic.Threshold));
          xmlWriter.WriteEndElement();
        }
        if (pic.GrayScale)
        {
          xmlWriter.WriteStartElement("a", "grayscl", "http://schemas.openxmlformats.org/drawingml/2006/main");
          xmlWriter.WriteEndElement();
        }
        DrawingSerializator.SerializeLumProperties(xmlWriter, pic);
        if (pic.DuoTone != null)
          DrawingSerializator.SerializeDuoTone(xmlWriter, pic);
        if (!string.IsNullOrEmpty(pic.ImageEmbedId))
          DrawingSerializator.SerializePictureExtensionList(xmlWriter, pic);
        if (pic.ColorChange.Count != 0)
          DrawingSerializator.SerializeColorChange(xmlWriter, pic);
        if (pic.Amount != 100000)
        {
          xmlWriter.WriteStartElement("a", "alphaModFix", "http://schemas.openxmlformats.org/drawingml/2006/main");
          xmlWriter.WriteAttributeString("amt", Helper.ToString(pic.Amount));
          xmlWriter.WriteEndElement();
        }
        if (pic.SvgData != null)
        {
          pic.AddImageToArchive(true);
          DrawingSerializator.SerializeExtensionList(xmlWriter, pic.SvgRelationId);
        }
        xmlWriter.WriteEndElement();
        DrawingSerializator.SerializeRelativeRect(xmlWriter, pic.FormatPicture);
        if (!pic.Flag)
        {
          xmlWriter.WriteStartElement("a", "stretch", "http://schemas.openxmlformats.org/drawingml/2006/main");
          xmlWriter.WriteStartElement("a", "fillRect", "http://schemas.openxmlformats.org/drawingml/2006/main");
          xmlWriter.WriteEndElement();
          xmlWriter.WriteEndElement();
        }
      }
      xmlWriter.WriteEndElement();
    }
  }

  private static void SerializeExtensionList(XmlWriter xmlWriter, string relationId)
  {
    xmlWriter.WriteStartElement("a", "extLst", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteStartElement("a", "ext", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("uri", "{28A0092B-C50C-407E-A947-70E740481C1C}");
    xmlWriter.WriteStartElement("a14", "useLocalDpi", "http://schemas.microsoft.com/office/drawing/2016/SVG/main");
    xmlWriter.WriteAttributeString("val", "0");
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
    xmlWriter.WriteStartElement("a", "ext", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("uri", "{96DAC541-7B7A-43D3-8B79-37D633B846F1}");
    xmlWriter.WriteStartElement("asvg", "svgBlip", "http://schemas.microsoft.com/office/drawing/2016/SVG/main");
    xmlWriter.WriteAttributeString("r", "embed", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", relationId);
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
  }

  private static void SerializePictureExtensionList(XmlWriter xmlWriter, Picture pic)
  {
    xmlWriter.WriteStartElement("a", "extLst", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteStartElement("a", "ext", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("uri", "{BEBA8EAE-BF5A-486C-A8C5-ECC9F3942E4B}");
    xmlWriter.WriteStartElement("a14", "imgProps", "http://schemas.microsoft.com/office/drawing/2010/main");
    xmlWriter.WriteStartElement("a14", "imgLayer", "http://schemas.microsoft.com/office/drawing/2010/main");
    if (!string.IsNullOrEmpty(pic.ImageEmbedId))
      xmlWriter.WriteAttributeString("r", "embed", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", pic.ImageEmbedId);
    if (pic.ImageAmount != 0)
      DrawingSerializator.SerializeSharpenProperties(xmlWriter, pic);
    if (pic.ColorTemp != 0)
      DrawingSerializator.SerializaColorTemp(xmlWriter, pic);
    if (pic.Saturation != 0)
      DrawingSerializator.serializeSaturation(xmlWriter, pic);
    if (pic.Bright != 0)
      DrawingSerializator.SerializeBrightProperties(xmlWriter, pic);
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
  }

  private static void SerializeDuoTone(XmlWriter xmlWriter, Picture pic)
  {
    if (pic == null || pic.DuoTone.Count == 0)
      return;
    xmlWriter.WriteStartElement("a", "duotone", "http://schemas.openxmlformats.org/drawingml/2006/main");
    foreach (ColorObject colorObject in pic.DuoTone)
    {
      bool isPreset = colorObject.ColorName != null;
      Serializator.SerializeSolidFill(xmlWriter, colorObject, -1, pic.BaseSlide.Presentation, isPreset, pic.BaseSlide);
    }
    xmlWriter.WriteEndElement();
  }

  private static void SerializeColorChange(XmlWriter xmlWriter, Picture pic)
  {
    xmlWriter.WriteStartElement("a", "clrChange", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (!pic.IsUseAlpha)
      xmlWriter.WriteAttributeString("useA", "0");
    for (int index = 0; index < pic.ColorChange.Count; ++index)
    {
      xmlWriter.WriteStartElement("a", index == 0 ? "clrFrom" : "clrTo", "http://schemas.openxmlformats.org/drawingml/2006/main");
      bool isPreset = pic.ColorChange[index].ColorName != null;
      Serializator.SerializeSolidFill(xmlWriter, pic.ColorChange[index], -1, pic.BaseSlide.Presentation, isPreset, pic.BaseSlide);
      xmlWriter.WriteEndElement();
    }
    xmlWriter.WriteEndElement();
  }

  private static void SerializeLumProperties(XmlWriter xmlWriter, Picture pic)
  {
    xmlWriter.WriteStartElement("a", "lum", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (pic.Brightness != 0)
      xmlWriter.WriteAttributeString("bright", Helper.ToString(pic.Brightness));
    if (pic.Contrast != 0)
      xmlWriter.WriteAttributeString("contrast", Helper.ToString(pic.Contrast));
    xmlWriter.WriteEndElement();
  }

  private static void SerializeBrightProperties(XmlWriter xmlWriter, Picture pic)
  {
    xmlWriter.WriteStartElement("a14", "imgEffect", "http://schemas.microsoft.com/office/drawing/2010/main");
    xmlWriter.WriteStartElement("a14", "brightnessContrast", "http://schemas.microsoft.com/office/drawing/2010/main");
    if (pic.Bright != 0)
      xmlWriter.WriteAttributeString("bright", Helper.ToString(pic.Bright));
    if (pic.ImageContrast != 0)
      xmlWriter.WriteAttributeString("contrast", Helper.ToString(pic.ImageContrast));
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
  }

  private static void SerializeSharpenProperties(XmlWriter xmlWriter, Picture pic)
  {
    xmlWriter.WriteStartElement("a14", "imgEffect", "http://schemas.microsoft.com/office/drawing/2010/main");
    xmlWriter.WriteStartElement("a14", "sharpenSoften", "http://schemas.microsoft.com/office/drawing/2010/main");
    if (pic.ImageAmount != 0)
      xmlWriter.WriteAttributeString("amount", Helper.ToString(pic.ImageAmount));
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
  }

  private static void SerializaColorTemp(XmlWriter xmlWriter, Picture pic)
  {
    xmlWriter.WriteStartElement("a14", "imgEffect", "http://schemas.microsoft.com/office/drawing/2010/main");
    xmlWriter.WriteStartElement("a14", "colorTemperature", "http://schemas.microsoft.com/office/drawing/2010/main");
    if (pic.ColorTemp != 0)
      xmlWriter.WriteAttributeString("colorTemp", Helper.ToString(pic.ColorTemp));
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
  }

  private static void serializeSaturation(XmlWriter xmlWriter, Picture pic)
  {
    xmlWriter.WriteStartElement("a14", "imgEffect", "http://schemas.microsoft.com/office/drawing/2010/main");
    xmlWriter.WriteStartElement("a14", "saturation", "http://schemas.microsoft.com/office/drawing/2010/main");
    if (pic.Saturation != 0)
      xmlWriter.WriteAttributeString("sat", Helper.ToString(pic.Saturation));
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
  }

  private static void SerializeRelativeRect(XmlWriter xmlWriter, FormatPicture formatPicture)
  {
    int topCrop = formatPicture.ObtainTopCrop();
    int bottomCrop = formatPicture.ObtainBottomCrop();
    int leftCrop = formatPicture.ObtainLeftCrop();
    int rightCrop = formatPicture.ObtainRightCrop();
    xmlWriter.WriteStartElement("a", "srcRect", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (leftCrop != 0)
      xmlWriter.WriteAttributeString("l", Helper.ToString(leftCrop));
    if (topCrop != 0)
      xmlWriter.WriteAttributeString("t", Helper.ToString(topCrop));
    if (rightCrop != 0)
      xmlWriter.WriteAttributeString("r", Helper.ToString(rightCrop));
    if (bottomCrop != 0)
      xmlWriter.WriteAttributeString("b", Helper.ToString(bottomCrop));
    xmlWriter.WriteEndElement();
  }

  private static void SerializeNonVisualPicProp(XmlWriter xmlWriter, Shape shape)
  {
    xmlWriter.WriteStartElement("p", "nvPicPr", "http://schemas.openxmlformats.org/presentationml/2006/main");
    DrawingSerializator.SerializeNonVisualDrngProp(xmlWriter, shape);
    xmlWriter.WriteStartElement("p", "cNvPicPr", "http://schemas.openxmlformats.org/presentationml/2006/main");
    xmlWriter.WriteStartElement("a", "picLocks", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("noChangeAspect", "1");
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
    xmlWriter.WriteStartElement("p", "nvPr", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (shape.VideoRelationId != null)
      xmlWriter.WriteAttributeString("r", "link", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", shape.VideoRelationId);
    if (shape.GetPlaceholder() != null)
    {
      xmlWriter.WriteStartElement("p", "ph", "http://schemas.openxmlformats.org/presentationml/2006/main");
      if (shape.GetPlaceholder().GetPlaceholderType() != (PlaceholderType) 0)
        xmlWriter.WriteAttributeString("type", Helper.ToString(shape.GetPlaceholder().GetPlaceholderType()));
      xmlWriter.WriteAttributeString("idx", Helper.ToString((long) shape.GetPlaceholder().Index));
      xmlWriter.WriteEndElement();
    }
    else if (shape.GetVideoPath() != null && shape.PreservedElements.ContainsKey("extLst"))
      DrawingSerializator.WriteRawData(xmlWriter, shape.PreservedElements, "extLst");
    DrawingSerializator.WriteCustomDataList(xmlWriter, shape);
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
  }

  private static void SerializeNonVisualDrngProp(XmlWriter xmlWriter, Shape shape)
  {
    xmlWriter.WriteStartElement("p", "cNvPr", "http://schemas.openxmlformats.org/presentationml/2006/main");
    string str = Helper.ToString(shape.ShapeId);
    if (str != null)
      xmlWriter.WriteAttributeString("id", str);
    string shapeName = shape.ShapeName;
    xmlWriter.WriteAttributeString("name", shapeName);
    string description = shape.Description;
    if (!string.IsNullOrEmpty(description))
      xmlWriter.WriteAttributeString("descr", description);
    string title = shape.Title;
    if (!string.IsNullOrEmpty(title))
      xmlWriter.WriteAttributeString("title", title);
    if (shape.Hidden)
      xmlWriter.WriteAttributeString("hidden", "1");
    if (shape.Hyperlink != null)
      Serializator.SerializeHyperlink(xmlWriter, (Hyperlink) shape.Hyperlink);
    xmlWriter.WriteEndElement();
  }

  private static void SerializeGraphicFrame(XmlWriter xmlWriter, Shape shape)
  {
    if (shape.ShapeType != ShapeType.GraphicFrame && shape.DrawingType != DrawingType.Chart)
      return;
    switch (shape.DrawingType)
    {
      case DrawingType.Table:
        xmlWriter.WriteStartElement("p", "graphicFrame", "http://schemas.openxmlformats.org/presentationml/2006/main");
        DrawingSerializator.SerializeGraphicAttributes(xmlWriter, shape);
        xmlWriter.WriteStartElement("p", "nvGraphicFramePr", "http://schemas.openxmlformats.org/presentationml/2006/main");
        DrawingSerializator.SerializeNonVisualDrngProp(xmlWriter, shape);
        xmlWriter.WriteStartElement("p", "cNvGraphicFramePr", "http://schemas.openxmlformats.org/presentationml/2006/main");
        xmlWriter.WriteEndElement();
        xmlWriter.WriteStartElement("p", "nvPr", "http://schemas.openxmlformats.org/presentationml/2006/main");
        DrawingSerializator.WriteCustomDataList(xmlWriter, shape);
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
        DrawingSerializator.SerializeFrame(xmlWriter, shape);
        xmlWriter.WriteStartElement("a", "graphic", "http://schemas.openxmlformats.org/drawingml/2006/main");
        xmlWriter.WriteStartElement("a", "graphicData", "http://schemas.openxmlformats.org/drawingml/2006/main");
        xmlWriter.WriteAttributeString("uri", "http://schemas.openxmlformats.org/drawingml/2006/table");
        Serializator.SerializeTable(xmlWriter, shape as Table);
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
        break;
      case DrawingType.Chart:
        shape.BaseSlide.Presentation.DataHolder.WriteChart(shape as PresentationChart);
        bool flag = DrawingSerializator.IsChartExSerieType((shape as PresentationChart).ChartType);
        if (flag)
        {
          xmlWriter.WriteStartElement("mc", "AlternateContent", "http://schemas.openxmlformats.org/markup-compatibility/2006");
          xmlWriter.WriteAttributeString("xmlns", "mc", (string) null, "http://schemas.openxmlformats.org/markup-compatibility/2006");
          xmlWriter.WriteStartElement("mc", "Choice", (string) null);
          xmlWriter.WriteAttributeString("xmlns", "cx", (string) null, "http://schemas.microsoft.com/office/drawing/2015/9/8/chartex");
          xmlWriter.WriteAttributeString("Requires", "cx");
        }
        xmlWriter.WriteStartElement("p", "graphicFrame", "http://schemas.openxmlformats.org/presentationml/2006/main");
        DrawingSerializator.SerializeGraphicAttributes(xmlWriter, shape);
        xmlWriter.WriteStartElement("p", "nvGraphicFramePr", "http://schemas.openxmlformats.org/presentationml/2006/main");
        DrawingSerializator.SerializeNonVisualDrngProp(xmlWriter, shape);
        xmlWriter.WriteStartElement("p", "cNvGraphicFramePr", "http://schemas.openxmlformats.org/presentationml/2006/main");
        xmlWriter.WriteEndElement();
        xmlWriter.WriteStartElement("p", "nvPr", "http://schemas.openxmlformats.org/presentationml/2006/main");
        DrawingSerializator.WriteCustomDataList(xmlWriter, shape);
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
        DrawingSerializator.SerializeFrame(xmlWriter, shape);
        xmlWriter.WriteStartElement("a", "graphic", "http://schemas.openxmlformats.org/drawingml/2006/main");
        xmlWriter.WriteStartElement("a", "graphicData", "http://schemas.openxmlformats.org/drawingml/2006/main");
        if (flag)
        {
          xmlWriter.WriteAttributeString("uri", "http://schemas.microsoft.com/office/drawing/2014/chartex");
          xmlWriter.WriteStartElement("cx", "chart", "http://schemas.microsoft.com/office/drawing/2014/chartex");
          xmlWriter.WriteAttributeString("xmlns", "r", (string) null, "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
        }
        else
        {
          xmlWriter.WriteAttributeString("uri", "http://schemas.openxmlformats.org/drawingml/2006/chart");
          xmlWriter.WriteStartElement("c", "chart", "http://schemas.openxmlformats.org/drawingml/2006/chart");
          xmlWriter.WriteAttributeString("xmlns", "r", (string) null, "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
        }
        xmlWriter.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", (shape as PresentationChart).RelationId);
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
        if (!flag)
          break;
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
        break;
      case DrawingType.SmartArt:
        SmartArt smartArt = (SmartArt) shape;
        shape.BaseSlide.Presentation.DataHolder.WriteSmartArt(smartArt);
        xmlWriter.WriteStartElement("p", "graphicFrame", "http://schemas.openxmlformats.org/presentationml/2006/main");
        DrawingSerializator.SerializeGraphicAttributes(xmlWriter, shape);
        xmlWriter.WriteStartElement("p", "nvGraphicFramePr", "http://schemas.openxmlformats.org/presentationml/2006/main");
        DrawingSerializator.SerializeNonVisualDrngProp(xmlWriter, shape);
        xmlWriter.WriteStartElement("p", "cNvGraphicFramePr", "http://schemas.openxmlformats.org/presentationml/2006/main");
        xmlWriter.WriteEndElement();
        xmlWriter.WriteStartElement("p", "nvPr", "http://schemas.openxmlformats.org/presentationml/2006/main");
        DrawingSerializator.WriteCustomDataList(xmlWriter, shape);
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
        DrawingSerializator.SerializeFrame(xmlWriter, shape);
        xmlWriter.WriteStartElement("a", "graphic", "http://schemas.openxmlformats.org/drawingml/2006/main");
        xmlWriter.WriteStartElement("a", "graphicData", "http://schemas.openxmlformats.org/drawingml/2006/main");
        xmlWriter.WriteAttributeString("uri", "http://schemas.openxmlformats.org/drawingml/2006/diagram");
        xmlWriter.WriteStartElement("dgm", "relIds", "http://schemas.openxmlformats.org/drawingml/2006/diagram");
        xmlWriter.WriteAttributeString("xmlns", "dgm", (string) null, "http://schemas.openxmlformats.org/drawingml/2006/diagram");
        xmlWriter.WriteAttributeString("xmlns", "r", (string) null, "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
        xmlWriter.WriteAttributeString("dm", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", smartArt.DataModel.RelationId);
        if (smartArt.LayoutRelationId != null)
          xmlWriter.WriteAttributeString("lo", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", smartArt.LayoutRelationId);
        if (smartArt.QuickStyleRelationId != null)
          xmlWriter.WriteAttributeString("qs", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", smartArt.QuickStyleRelationId);
        if (smartArt.ColorsRelationId != null)
          xmlWriter.WriteAttributeString("cs", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", smartArt.ColorsRelationId);
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
        break;
      case DrawingType.OleObject:
        OleObject oleObject = shape as OleObject;
        if (oleObject.RelationId == null && oleObject.OleStream != null)
        {
          if (!shape.BaseSlide.HasOLEObject)
          {
            int vmlDrawingCount = shape.BaseSlide.Presentation.GetVmlDrawingCount(shape.BaseSlide as Slide);
            if (vmlDrawingCount > 1)
              --vmlDrawingCount;
            string relationId = Helper.GenerateRelationId(shape.BaseSlide.TopRelation);
            Relation relation = new Relation(relationId, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/vmlDrawing", $"../drawings/vmlDrawing{(object) vmlDrawingCount}.vml", (string) null);
            shape.BaseSlide.TopRelation.Add(relationId, relation);
          }
          switch (oleObject.OleObjectType)
          {
            case OleObjectType.AdobeAcrobatDocument:
              if (!shape.BaseSlide.Presentation.DefaultContentType.ContainsKey("bin"))
              {
                shape.BaseSlide.Presentation.AddDefaultContentType("bin", "application/vnd.openxmlformats-officedocument.oleObject");
                break;
              }
              break;
            case OleObjectType.ExcelWorksheet:
              if (!shape.BaseSlide.Presentation.DefaultContentType.ContainsKey("xlsx"))
              {
                shape.BaseSlide.Presentation.AddDefaultContentType("xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                break;
              }
              break;
            case OleObjectType.PowerPointPresentation:
              if (!shape.BaseSlide.Presentation.DefaultContentType.ContainsKey("pptx"))
              {
                shape.BaseSlide.Presentation.AddDefaultContentType("pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
                break;
              }
              break;
            case OleObjectType.WordDocument:
              if (!shape.BaseSlide.Presentation.DefaultContentType.ContainsKey("docx"))
              {
                shape.BaseSlide.Presentation.AddDefaultContentType("docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
                break;
              }
              break;
          }
          if (!shape.BaseSlide.Presentation.DefaultContentType.ContainsKey("vml"))
            shape.BaseSlide.Presentation.DefaultContentType.Add("vml", "application/vnd.openxmlformats-officedocument.vmlDrawing");
          ++shape.BaseSlide.Presentation.OleObjectCount;
          string relationId1 = Helper.GenerateRelationId(shape.BaseSlide.TopRelation);
          Relation relation1 = new Relation(relationId1, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/package", $"../embeddings/oleObject{shape.BaseSlide.Presentation.OleObjectCount.ToString()}{oleObject.OleExtension}", (string) null);
          shape.BaseSlide.TopRelation.Add(relationId1, relation1);
          oleObject.RelationId = relationId1;
        }
        xmlWriter.WriteStartElement("p", "graphicFrame", "http://schemas.openxmlformats.org/presentationml/2006/main");
        DrawingSerializator.SerializeGraphicAttributes(xmlWriter, shape);
        xmlWriter.WriteStartElement("p", "nvGraphicFramePr", "http://schemas.openxmlformats.org/presentationml/2006/main");
        DrawingSerializator.SerializeNonVisualDrngProp(xmlWriter, shape);
        xmlWriter.WriteStartElement("p", "cNvGraphicFramePr", "http://schemas.openxmlformats.org/presentationml/2006/main");
        xmlWriter.WriteEndElement();
        xmlWriter.WriteStartElement("p", "nvPr", "http://schemas.openxmlformats.org/presentationml/2006/main");
        DrawingSerializator.WriteCustomDataList(xmlWriter, shape);
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
        DrawingSerializator.SerializeFrame(xmlWriter, shape);
        xmlWriter.WriteStartElement("a", "graphic", "http://schemas.openxmlformats.org/drawingml/2006/main");
        xmlWriter.WriteStartElement("a", "graphicData", "http://schemas.openxmlformats.org/drawingml/2006/main");
        xmlWriter.WriteAttributeString("uri", "http://schemas.openxmlformats.org/presentationml/2006/ole");
        Serializator.SerializeOleObject(xmlWriter, shape);
        string itemPath = (string) null;
        if (oleObject.LinkType == OleLinkType.Embed)
        {
          if (oleObject.RelationId != null)
          {
            itemPath = Helper.FormatPathForZipArchive(shape.BaseSlide.TopRelation.GetTargetByRelationId(oleObject.RelationId));
            if (itemPath.Contains("Microsoft_Excel_Worksheet"))
              ++oleObject.BaseSlide.Presentation.ExcelCount;
          }
          if (itemPath != null)
            oleObject.BaseSlide.Presentation.DataHolder.AddItemToZipStream(itemPath, oleObject.OleStream);
        }
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
        break;
      default:
        DrawingSerializator.WriteRawData(xmlWriter, shape.PreservedElements, "graphicFrame");
        break;
    }
  }

  internal static bool IsChartExSerieType(OfficeChartType type)
  {
    switch (type)
    {
      case OfficeChartType.Pareto:
      case OfficeChartType.Funnel:
      case OfficeChartType.Histogram:
      case OfficeChartType.WaterFall:
      case OfficeChartType.TreeMap:
      case OfficeChartType.SunBurst:
      case OfficeChartType.BoxAndWhisker:
        return true;
      default:
        return false;
    }
  }

  private static void SerializeGraphicAttributes(XmlWriter xmlWriter, Shape shape)
  {
    string macro = shape.Macro;
    if (!string.IsNullOrEmpty(macro))
      xmlWriter.WriteAttributeString("macro", macro);
    if (shape.FPublished)
      xmlWriter.WriteAttributeString("fPublished", "1");
    if (shape.FLocksText)
      xmlWriter.WriteAttributeString("fLocksText", "1");
    string textLink = shape.TextLink;
    if (!string.IsNullOrEmpty(textLink))
      xmlWriter.WriteAttributeString("textlink", textLink);
    if (!shape.IsBgFill)
      return;
    xmlWriter.WriteAttributeString("useBgFill", "1");
  }

  private static bool IsConnectorShape(Shape shape)
  {
    if (shape.ShapeType == ShapeType.CxnSp)
      return true;
    switch (shape.AutoShapeType)
    {
      case AutoShapeType.Line:
      case AutoShapeType.StraightConnector:
      case AutoShapeType.ElbowConnector:
      case AutoShapeType.CurvedConnector:
      case AutoShapeType.BentConnector2:
      case AutoShapeType.BentConnector4:
      case AutoShapeType.BentConnector5:
      case AutoShapeType.CurvedConnector2:
      case AutoShapeType.CurvedConnector4:
      case AutoShapeType.CurvedConnector5:
        return true;
      default:
        return false;
    }
  }

  private static void SerializeConShape(XmlWriter xmlWriter, Shape shape)
  {
    switch (shape.ShapeType)
    {
      case ShapeType.Sp:
      case ShapeType.CxnSp:
        if (shape.ShapeType == ShapeType.CxnSp && shape is Connector)
        {
          Connector connector = shape as Connector;
          if (connector.IsChanged)
            connector.Update();
        }
        string localName = DrawingSerializator.IsConnectorShape(shape) ? "cxnSp" : "sp";
        xmlWriter.WriteStartElement("p", localName, "http://schemas.openxmlformats.org/presentationml/2006/main");
        DrawingSerializator.SerializeGraphicAttributes(xmlWriter, shape);
        DrawingSerializator.SerializeNonVisualShapePr(xmlWriter, shape);
        DrawingSerializator.SerializeGrpSpProp(xmlWriter, shape);
        DrawingSerializator.WriteStyle(xmlWriter, shape);
        if (localName == "sp")
          DrawingSerializator.SerializeRichText(xmlWriter, shape);
        xmlWriter.WriteEndElement();
        break;
      case ShapeType.GrpSp:
        DrawingSerializator.SerializeGroupShape(xmlWriter, shape);
        break;
      case ShapeType.AlternateContent:
        if (!shape.PreservedElements.ContainsKey("AlternateContent"))
          break;
        DrawingSerializator.WriteRawData(xmlWriter, shape.PreservedElements, "AlternateContent");
        break;
    }
  }

  private static void WriteCustomDataList(XmlWriter xmlWriter, Shape shape)
  {
    if (!shape.PreservedElements.ContainsKey("custDataLst"))
      return;
    DrawingSerializator.WriteRawData(xmlWriter, shape.PreservedElements, "custDataLst");
  }

  private static void WriteStyle(XmlWriter xmlWriter, Shape shape)
  {
    if (!shape.PreservedElements.ContainsKey("style"))
      return;
    DrawingSerializator.WriteRawData(xmlWriter, shape.PreservedElements, "style");
  }

  internal static void FontRef(XmlWriter xmlTextWriter)
  {
    xmlTextWriter.WriteStartElement("a", "fontRef", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlTextWriter.WriteAttributeString("idx", "minor");
    xmlTextWriter.WriteStartElement("a", "schemeClr", (string) null);
    xmlTextWriter.WriteAttributeString("val", "tx1");
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteEndElement();
  }

  internal static void EffectRef(XmlWriter xmlTextWriter)
  {
    xmlTextWriter.WriteStartElement("a", "effectRef", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlTextWriter.WriteAttributeString("idx", "0");
    xmlTextWriter.WriteStartElement("a", "schemeClr", (string) null);
    xmlTextWriter.WriteAttributeString("val", "accent1");
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteEndElement();
  }

  internal static void FillRef(XmlWriter xmlTextWriter, IShape shape)
  {
    xmlTextWriter.WriteStartElement("a", "fillRef", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlTextWriter.WriteAttributeString("idx", (shape as Shape).IsLineShape ? "0" : "1");
    xmlTextWriter.WriteStartElement("a", "schemeClr", (string) null);
    xmlTextWriter.WriteAttributeString("val", "accent1");
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteEndElement();
  }

  internal static void LnRef(XmlWriter xmlTextWriter, IShape shape)
  {
    xmlTextWriter.WriteStartElement("a", "lnRef", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlTextWriter.WriteAttributeString("idx", (shape as Shape).IsLineShape ? "1" : "2");
    xmlTextWriter.WriteStartElement("a", "schemeClr", (string) null);
    xmlTextWriter.WriteAttributeString("val", "accent1");
    if (!(shape is Connector))
    {
      xmlTextWriter.WriteStartElement("a", "shade", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlTextWriter.WriteAttributeString("val", "50000");
      xmlTextWriter.WriteEndElement();
    }
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.WriteEndElement();
  }

  private static void SerializeNonVisualShapePr(XmlWriter xmlWriter, Shape shape)
  {
    xmlWriter.WriteStartElement("p", DrawingSerializator.IsConnectorShape(shape) ? "nvCxnSpPr" : "nvSpPr", "http://schemas.openxmlformats.org/presentationml/2006/main");
    DrawingSerializator.SerializeNonVisualDrngProp(xmlWriter, shape);
    if (!DrawingSerializator.IsConnectorShape(shape))
    {
      xmlWriter.WriteStartElement("p", "cNvSpPr", "http://schemas.openxmlformats.org/presentationml/2006/main");
      if (shape.DrawingType == DrawingType.TextBox)
        xmlWriter.WriteAttributeString("txBox", "1");
      xmlWriter.WriteStartElement("a", "spLocks", "http://schemas.openxmlformats.org/drawingml/2006/main");
      if (shape.IsLockAspectRatio)
        xmlWriter.WriteAttributeString("noChangeAspect", "1");
      if (shape.DrawingType == DrawingType.TextBox)
        xmlWriter.WriteAttributeString("noChangeArrowheads", "1");
      if (shape.DrawingType == DrawingType.PlaceHolder)
        xmlWriter.WriteAttributeString("noGrp", "1");
      xmlWriter.WriteEndElement();
      xmlWriter.WriteEndElement();
      DrawingSerializator.SerializePlaceHolder(xmlWriter, shape);
    }
    else
    {
      xmlWriter.WriteStartElement("p", "cNvCxnSpPr", "http://schemas.openxmlformats.org/presentationml/2006/main");
      if (shape is Connector)
      {
        Connector connector = shape as Connector;
        if (connector.BeginConnected)
        {
          xmlWriter.WriteStartElement("a", "stCxn", "http://schemas.openxmlformats.org/drawingml/2006/main");
          string str1 = Helper.ToString((connector.BeginConnectedShape as Shape).ShapeId);
          xmlWriter.WriteAttributeString("id", str1);
          string str2 = Helper.ToString(connector.BeginConnectionSiteIndex);
          xmlWriter.WriteAttributeString("idx", str2);
          xmlWriter.WriteEndElement();
        }
        if (connector.EndConnected)
        {
          xmlWriter.WriteStartElement("a", "endCxn", "http://schemas.openxmlformats.org/drawingml/2006/main");
          string str3 = Helper.ToString((connector.EndConnectedShape as Shape).ShapeId);
          xmlWriter.WriteAttributeString("id", str3);
          string str4 = Helper.ToString(connector.EndConnectionSiteIndex);
          xmlWriter.WriteAttributeString("idx", str4);
          xmlWriter.WriteEndElement();
        }
      }
      xmlWriter.WriteEndElement();
      xmlWriter.WriteStartElement("p", "nvPr", (string) null);
      DrawingSerializator.WriteCustomDataList(xmlWriter, shape);
      xmlWriter.WriteEndElement();
    }
    xmlWriter.WriteEndElement();
  }

  private static void SerializePlaceHolder(XmlWriter xmlWriter, Shape shape)
  {
    Placeholder placeholderFormat = (Placeholder) shape.PlaceholderFormat;
    xmlWriter.WriteStartElement("p", "nvPr", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (shape.IsPhoto)
      xmlWriter.WriteAttributeString("isPhoto", "1");
    if (shape.IsUserDrawn)
      xmlWriter.WriteAttributeString("userDrawn", "1");
    if (shape.DrawingType == DrawingType.PlaceHolder)
    {
      xmlWriter.WriteStartElement("p", "ph", "http://schemas.openxmlformats.org/presentationml/2006/main");
      if (placeholderFormat.GetPlaceholderType() != (PlaceholderType) 0)
        xmlWriter.WriteAttributeString("type", Helper.ToString(placeholderFormat.GetPlaceholderType()));
      if (placeholderFormat.HasCustomPrompt)
        xmlWriter.WriteAttributeString("hasCustomPrompt", "1");
      if (placeholderFormat.Orientation != Orientation.None)
        xmlWriter.WriteAttributeString("orient", Helper.ToString(placeholderFormat.Orientation));
      if (placeholderFormat.Size != PlaceholderSize.None)
        xmlWriter.WriteAttributeString("sz", Helper.ToString(placeholderFormat.Size));
      if (placeholderFormat.ObtainIndex() != null)
        xmlWriter.WriteAttributeString("idx", placeholderFormat.ObtainIndex());
      xmlWriter.WriteEndElement();
    }
    DrawingSerializator.WriteCustomDataList(xmlWriter, shape);
    xmlWriter.WriteEndElement();
  }

  private static void SerializeShapes(XmlWriter xmlWriter, BaseSlide slide)
  {
    if (slide.Shapes == null || slide.Shapes.Count <= 0)
      return;
    foreach (IShape shape in (IEnumerable<ISlideItem>) slide.Shapes)
    {
      if (shape != null)
      {
        switch (((Shape) shape).ShapeType)
        {
          case ShapeType.Sp:
          case ShapeType.GrpSp:
          case ShapeType.GraphicFrame:
          case ShapeType.CxnSp:
          case ShapeType.Pic:
          case ShapeType.AlternateContent:
          case ShapeType.Chart:
            DrawingSerializator.Write(xmlWriter, (Shape) shape);
            continue;
          default:
            continue;
        }
      }
    }
  }

  internal static void WriteRawData(
    XmlWriter xmlWriter,
    Dictionary<string, Stream> preservedStream,
    string elementName)
  {
    if (!preservedStream.ContainsKey(elementName))
      return;
    Stream input = preservedStream[elementName];
    if (input == null || input.Length <= 0L)
      return;
    input.Position = 0L;
    using (XmlReader reader = XmlReader.Create(input))
    {
      while (reader.LocalName != elementName)
        reader.Read();
      xmlWriter.WriteNode(reader, false);
    }
  }

  internal static void SerializeBackGround(XmlWriter xmlWriter, BaseSlide slide)
  {
    Background background = (Background) slide.Background;
    if (background.Type == BackgroundType.NotDefined || background.GetFillFormat().FillType == FillType.Automatic)
      return;
    xmlWriter.WriteStartElement("p", "bg", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (background.BlackWhiteMode != BlackWhiteMode.None)
      xmlWriter.WriteAttributeString("bwMode", Helper.ToString(background.BlackWhiteMode));
    switch (background.Type)
    {
      case BackgroundType.Themed:
        DrawingSerializator.SerializeMatrixReference(xmlWriter, (Background) slide.Background);
        break;
      case BackgroundType.OwnBackground:
        DrawingSerializator.SerializeBackGroundProperties(xmlWriter, (Background) slide.Background);
        break;
    }
    xmlWriter.WriteEndElement();
  }

  private static void SerializeMatrixReference(XmlWriter xmlWriter, Background background)
  {
    xmlWriter.WriteStartElement("p", "bgRef", "http://schemas.openxmlformats.org/presentationml/2006/main");
    xmlWriter.WriteAttributeString("idx", Helper.ToString(background.Index));
    Serializator.SerializeSolidFill(xmlWriter, background.GetColorObject(), -1, background.BaseSlide.Presentation, false, background.BaseSlide);
    xmlWriter.WriteEndElement();
  }

  private static void SerializeBackGroundProperties(XmlWriter xmlWriter, Background background)
  {
    xmlWriter.WriteStartElement("p", "bgPr", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (background.ShadeToTitle)
      xmlWriter.WriteAttributeString("shadeToTitle", "1");
    if (background.GetFillFormat() != null)
      Serializator.SerializeFillProperties(xmlWriter, background.GetFillFormat(), background.BaseSlide.Presentation);
    xmlWriter.WriteEndElement();
  }

  internal static void SerializeShapeTree(XmlWriter xmlWriter, SmartArt smartArt)
  {
    xmlWriter.WriteStartElement("dsp", "spTree", "http://schemas.microsoft.com/office/drawing/2008/diagram");
    xmlWriter.WriteStartElement("dsp", "nvGrpSpPr", "http://schemas.microsoft.com/office/drawing/2008/diagram");
    xmlWriter.WriteStartElement("dsp", "cNvPr", "http://schemas.microsoft.com/office/drawing/2008/diagram");
    xmlWriter.WriteAttributeString("id", "1");
    xmlWriter.WriteAttributeString("name", "");
    xmlWriter.WriteEndElement();
    xmlWriter.WriteStartElement("dsp", "cNvGrpSpPr", "http://schemas.microsoft.com/office/drawing/2008/diagram");
    xmlWriter.WriteEndElement();
    xmlWriter.WriteStartElement("dsp", "nvPr", "http://schemas.microsoft.com/office/drawing/2008/diagram");
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
    xmlWriter.WriteStartElement("dsp", "grpSpPr", "http://schemas.microsoft.com/office/drawing/2008/diagram");
    xmlWriter.WriteStartElement("a", "xfrm", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteStartElement("a", "off", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("x", "0");
    xmlWriter.WriteAttributeString("y", "0");
    xmlWriter.WriteEndElement();
    xmlWriter.WriteStartElement("a", "ext", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("cx", "0");
    xmlWriter.WriteAttributeString("cy", "0");
    xmlWriter.WriteEndElement();
    xmlWriter.WriteStartElement("a", "chOff", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("x", "0");
    xmlWriter.WriteAttributeString("y", "0");
    xmlWriter.WriteEndElement();
    xmlWriter.WriteStartElement("a", "chExt", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("cx", "0");
    xmlWriter.WriteAttributeString("cy", "0");
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
    DrawingSerializator.SerializeShapes(xmlWriter, smartArt);
    xmlWriter.WriteEndElement();
  }

  private static void SerializeShapes(XmlWriter xmlWriter, SmartArt smartArt)
  {
    Dictionary<Guid, SmartArtShape> artShapeCollection = smartArt.DataModel.SmartArtShapeCollection;
    if (artShapeCollection == null || artShapeCollection.Count <= 0)
      return;
    foreach (KeyValuePair<Guid, SmartArtShape> keyValuePair in artShapeCollection)
    {
      SmartArtShape shape = keyValuePair.Value;
      if (shape != null && shape.ShapeType == ShapeType.Point)
        DrawingSerializator.WriteSmartArtShape(xmlWriter, shape, keyValuePair.Key);
    }
  }

  private static void WriteSmartArtShape(XmlWriter xmlWriter, SmartArtShape shape, Guid modelId)
  {
    string localName = DrawingSerializator.IsConnectorShape((Shape) shape) ? "cxnSp" : "sp";
    xmlWriter.WriteStartElement("dsp", localName, "http://schemas.microsoft.com/office/drawing/2008/diagram");
    xmlWriter.WriteAttributeString(nameof (modelId), Serializator.GetGuidString(modelId));
    DrawingSerializator.SerializeGraphicAttributes(xmlWriter, (Shape) shape);
    DrawingSerializator.SerializeNonVisualSmartArtShapePr(xmlWriter, (Shape) shape);
    DrawingSerializator.SerializeGrpSpProp(xmlWriter, (Shape) shape);
    DrawingSerializator.WriteStyle(xmlWriter, (Shape) shape);
    if (localName == "sp")
      DrawingSerializator.SerializeRichText(xmlWriter, (Shape) shape);
    xmlWriter.WriteEndElement();
  }

  private static void SerializeNonVisualSmartArtShapePr(XmlWriter xmlWriter, Shape shape)
  {
    xmlWriter.WriteStartElement("dsp", DrawingSerializator.IsConnectorShape(shape) ? "nvCxnSpPr" : "nvSpPr", "http://schemas.microsoft.com/office/drawing/2008/diagram");
    DrawingSerializator.SerializeNonVisualSmartArtDrngProp(xmlWriter, shape);
    if (!DrawingSerializator.IsConnectorShape(shape))
    {
      xmlWriter.WriteStartElement("dsp", "cNvSpPr", "http://schemas.microsoft.com/office/drawing/2008/diagram");
      if (shape.DrawingType == DrawingType.TextBox)
        xmlWriter.WriteAttributeString("txBox", "1");
      xmlWriter.WriteStartElement("a", "spLocks", "http://schemas.openxmlformats.org/drawingml/2006/main");
      if (shape.IsLockAspectRatio)
        xmlWriter.WriteAttributeString("noChangeAspect", "1");
      if (shape.DrawingType == DrawingType.TextBox)
        xmlWriter.WriteAttributeString("noChangeArrowheads", "1");
      xmlWriter.WriteEndElement();
      xmlWriter.WriteEndElement();
    }
    else
    {
      xmlWriter.WriteStartElement("dsp", "cNvCxnSpPr", (string) null);
      xmlWriter.WriteEndElement();
      xmlWriter.WriteStartElement("dsp", "nvPr", (string) null);
      DrawingSerializator.WriteCustomDataList(xmlWriter, shape);
      xmlWriter.WriteEndElement();
    }
    xmlWriter.WriteEndElement();
  }

  private static void SerializeNonVisualSmartArtDrngProp(XmlWriter xmlWriter, Shape shape)
  {
    xmlWriter.WriteStartElement("dsp", "cNvPr", "http://schemas.microsoft.com/office/drawing/2008/diagram");
    string str = Helper.ToString(shape.ShapeId);
    if (str != null)
      xmlWriter.WriteAttributeString("id", str);
    string shapeName = shape.ShapeName;
    xmlWriter.WriteAttributeString("name", shapeName);
    string description = shape.Description;
    if (!string.IsNullOrEmpty(description))
      xmlWriter.WriteAttributeString("descr", description);
    string title = shape.Title;
    if (!string.IsNullOrEmpty(title))
      xmlWriter.WriteAttributeString("title", title);
    if (shape.Hidden)
      xmlWriter.WriteAttributeString("hidden", "1");
    if (shape.Hyperlink != null)
      Serializator.SerializeHyperlink(xmlWriter, (Hyperlink) shape.Hyperlink);
    xmlWriter.WriteEndElement();
  }
}
