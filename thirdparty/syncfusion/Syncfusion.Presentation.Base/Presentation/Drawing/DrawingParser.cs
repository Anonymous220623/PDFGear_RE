// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.DrawingParser
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.RichText;
using Syncfusion.Presentation.SlideImplementation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class DrawingParser
{
  internal static void ParsePicture(XmlReader reader, Picture picture)
  {
    string localName = reader.LocalName;
    if (reader.MoveToAttribute("macro"))
      picture.Macro = reader.Value;
    if (reader.MoveToAttribute("fPublished"))
      picture.FPublished = XmlConvert.ToBoolean(reader.Value);
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "nvPicPr":
              DrawingParser.ParseShapeNonVisual(reader, (Shape) picture);
              continue;
            case "spPr":
              DrawingParser.ParseShapeProperties(reader, (Shape) picture);
              continue;
            case "blipFill":
              DrawingParser.ParseBlipFill(reader, picture);
              continue;
            case "AlternateContent":
              picture.PreservedElements.Add("AlternateContent", UtilityMethods.ReadSingleNodeIntoStream(reader));
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          reader.Skip();
      }
    }
    reader.Read();
  }

  internal static void ParseFillProperties(XmlReader reader, Fill fillFormat)
  {
    string localName = reader.LocalName;
    while (localName == reader.LocalName && reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "noFill":
            fillFormat.FillType = FillType.None;
            reader.Skip();
            continue;
          case "solidFill":
            DrawingParser.ParseSolidFill(reader, fillFormat);
            continue;
          case "gradFill":
            DrawingParser.ParseGradientFill(reader, fillFormat);
            continue;
          case "pattFill":
            DrawingParser.ParsePatternFill(reader, fillFormat);
            continue;
          case "blipFill":
            DrawingParser.ParseBlipFill(reader, fillFormat);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
  }

  internal static void ParseBlipFill(XmlReader reader, Fill fillFormat)
  {
    fillFormat.FillType = FillType.Picture;
    TextureFill pictureFill = (TextureFill) fillFormat.PictureFill;
    DrawingParser.ParseTextureFill(reader, pictureFill);
  }

  internal static void ParsePatternFill(XmlReader reader, Fill fillFormat)
  {
    fillFormat.FillType = FillType.Pattern;
    PatternFill patternFill = (PatternFill) fillFormat.PatternFill;
    DrawingParser.ParsePatternFill(reader, patternFill);
  }

  internal static void ParseGradientFill(XmlReader reader, Fill fillFormat)
  {
    fillFormat.FillType = FillType.Gradient;
    GradientFill gradientFill = (GradientFill) fillFormat.GradientFill;
    DrawingParser.ParseGradientFill(reader, gradientFill);
  }

  internal static void ParseSolidFill(XmlReader reader, Fill fillFormat)
  {
    fillFormat.FillType = FillType.Solid;
    ColorObject colorObject1 = new ColorObject();
    MasterSlide baseSlide1 = fillFormat.BaseSlide as MasterSlide;
    LayoutSlide baseSlide2 = fillFormat.BaseSlide as LayoutSlide;
    ColorObject colorObject2 = fillFormat.BaseSlide == null || baseSlide1 != null || baseSlide2 != null ? (baseSlide2 == null || baseSlide2.ColorMap.Count == 0 ? DrawingParser.ParseColorChoice(reader, baseSlide1) : DrawingParser.ParseColorChoice(reader, baseSlide2)) : (fillFormat.BaseSlide.BaseTheme.BaseSlide is HandoutMaster || fillFormat.BaseSlide.BaseTheme.BaseSlide is NotesMasterSlide ? DrawingParser.ParseColorChoice(reader, (MasterSlide) fillFormat.Presentation.Masters[0]) : DrawingParser.ParseColorChoice(reader, (MasterSlide) fillFormat.BaseSlide.BaseTheme.BaseSlide));
    ((SolidFill) fillFormat.SolidFill).SetColorObject(colorObject2);
  }

  internal static void ParseGeneralShape(XmlReader reader, Shape shape)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.MoveToAttribute("macro"))
      shape.Macro = reader.Value;
    if (reader.MoveToAttribute("textlink"))
      shape.TextLink = reader.Value;
    if (reader.MoveToAttribute("fLocksText"))
      shape.FLocksText = XmlConvert.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("fPublished"))
      shape.FPublished = XmlConvert.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("useBgFill"))
      shape.IsBgFill = true;
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      string localName = reader.LocalName;
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "nvSpPr":
              DrawingParser.ParseShapeNonVisual(reader, shape);
              continue;
            case "spPr":
              DrawingParser.ParseShapeProperties(reader, shape);
              continue;
            case "txBody":
              RichTextParser.ParseTextBody(reader, shape.TextBody);
              continue;
            case "style":
              shape.PreservedElements.Add("style", UtilityMethods.ReadSingleNodeIntoStream(reader));
              continue;
            case "txXfrm":
              DrawingParser.ParseTextBodyTransform(reader, shape);
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          reader.Skip();
      }
    }
    reader.Read();
  }

  internal static void ParseTextBodyTransform(XmlReader reader, Shape shape)
  {
    DrawingParser.ParseTransformation(reader, shape, false);
  }

  internal static void ParseLineProperties(XmlReader reader, LineFormat lineFormat)
  {
    if (reader.MoveToAttribute("w"))
      lineFormat.SetWidth(Helper.ToInt(reader.Value));
    if (reader.MoveToAttribute("cmpd"))
      lineFormat.SetStyle(Helper.GetLineStyle(reader.Value));
    if (reader.MoveToAttribute("cap"))
      lineFormat.SetCapStyle(Helper.GetLineCap(reader.Value));
    reader.MoveToElement();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "noFill":
            (lineFormat.Fill as Fill).SetFillType(FillType.None);
            reader.Skip();
            continue;
          case "solidFill":
            (lineFormat.Fill as Fill).SetFillType(FillType.Solid);
            if (!reader.IsEmptyElement)
            {
              if (!(lineFormat.BaseSlide is MasterSlide masterSlide))
                masterSlide = (MasterSlide) lineFormat.Presentation.Masters[0];
              ColorObject colorChoice = DrawingParser.ParseColorChoice(reader, masterSlide);
              ((SolidFill) lineFormat.Fill.SolidFill).SetColorObject(colorChoice);
              continue;
            }
            reader.Skip();
            continue;
          case "gradFill":
            lineFormat.Fill.FillType = FillType.Gradient;
            GradientFill gradientFill = (GradientFill) lineFormat.Fill.GradientFill;
            DrawingParser.ParseGradientFill(reader, gradientFill);
            continue;
          case "headEnd":
            DrawingParser.ParseLineEndProperties(reader, lineFormat, true);
            continue;
          case "tailEnd":
            DrawingParser.ParseLineEndProperties(reader, lineFormat, false);
            continue;
          case "prstDash":
            DrawingParser.ParsePresetDashProperties(reader, lineFormat);
            continue;
          case "round":
            lineFormat.SetLineJoinType(LineJoinType.Round);
            reader.Skip();
            continue;
          case "bevel":
            lineFormat.SetLineJoinType(LineJoinType.Bevel);
            reader.Skip();
            continue;
          case "miter":
            lineFormat.SetLineJoinType(LineJoinType.Miter);
            reader.Skip();
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
  }

  internal static void ParseNonVisualDrawingProps(XmlReader reader, Shape shape)
  {
    if (reader.MoveToAttribute("id"))
      shape.ShapeId = Helper.ToInt(reader.Value);
    if (reader.MoveToAttribute("name"))
      shape.ShapeName = reader.Value;
    if (reader.MoveToAttribute("descr"))
      shape.Description = reader.Value;
    if (reader.MoveToAttribute("hidden"))
      shape.Hidden = XmlConvert.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("title"))
      shape.Title = reader.Value;
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      string localName = reader.LocalName;
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "hlinkClick":
              Hyperlink hyperlink = new Hyperlink(shape);
              shape.SetHyperlink(DrawingParser.ParseHyperlink(reader, hyperlink));
              reader.Skip();
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          reader.Skip();
      }
    }
    reader.Read();
  }

  internal static Hyperlink ParseHyperlink(XmlReader xmlReader, Hyperlink hyperlink)
  {
    if (xmlReader.HasAttributes)
    {
      while (xmlReader.MoveToNextAttribute())
      {
        switch (xmlReader.LocalName)
        {
          case "id":
            hyperlink.RelationId = xmlReader.Value;
            continue;
          case "action":
            hyperlink.ActionString = xmlReader.Value;
            continue;
          case "tooltip":
            hyperlink.ScreenTip = xmlReader.Value;
            continue;
          default:
            continue;
        }
      }
      xmlReader.MoveToElement();
    }
    if (hyperlink.RelationId != null && hyperlink.ActionString == null)
      hyperlink.Action = HyperLinkType.Hyperlink;
    if (!xmlReader.IsEmptyElement)
    {
      string localName = xmlReader.LocalName;
      xmlReader.Read();
      while (!(localName == xmlReader.LocalName) || xmlReader.NodeType != XmlNodeType.EndElement)
      {
        if (xmlReader.NodeType == XmlNodeType.Element)
        {
          switch (xmlReader.LocalName)
          {
            case "extLst":
              Parser.ParseExtensionList(xmlReader, (Syncfusion.Presentation.Presentation) null, hyperlink);
              continue;
            default:
              xmlReader.Skip();
              continue;
          }
        }
        else
          xmlReader.Skip();
      }
    }
    return hyperlink;
  }

  internal static void ParseTransformation(XmlReader reader, Shape shape, bool isShapeTransfm)
  {
    bool? flipV = new bool?();
    bool? flipH = new bool?();
    int rotation = -1;
    long offsetX = 0;
    long offsetY = 0;
    long offsetCx = 0;
    long offsetCy = 0;
    long childOffsetX = 0;
    long childOffsetY = 0;
    long childOffsetCx = 0;
    long childOffsetCy = 0;
    if (reader.MoveToAttribute("flipV"))
      flipV = new bool?(XmlConvert.ToBoolean(reader.Value));
    if (reader.MoveToAttribute("flipH"))
      flipH = new bool?(XmlConvert.ToBoolean(reader.Value));
    if (reader.MoveToAttribute("rot"))
      rotation = Helper.ToInt(reader.Value);
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      string localName = reader.LocalName;
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "off":
              if (reader.MoveToAttribute("x"))
                offsetX = (long) Helper.ToInt(reader.Value);
              if (reader.MoveToAttribute("y"))
              {
                offsetY = (long) Helper.ToInt(reader.Value);
                continue;
              }
              continue;
            case "ext":
              if (reader.MoveToAttribute("cx"))
                offsetCx = (long) Helper.ToInt(reader.Value);
              if (reader.MoveToAttribute("cy"))
              {
                offsetCy = (long) Helper.ToInt(reader.Value);
                continue;
              }
              continue;
            case "chOff":
              if (reader.MoveToAttribute("x"))
              {
                shape.ShapeFrame.IsChildOffsetSet = true;
                childOffsetX = (long) Helper.ToInt(reader.Value);
              }
              if (reader.MoveToAttribute("y"))
              {
                shape.ShapeFrame.IsChildOffsetSet = true;
                childOffsetY = (long) Helper.ToInt(reader.Value);
                continue;
              }
              continue;
            case "chExt":
              if (reader.MoveToAttribute("cx"))
              {
                shape.ShapeFrame.IsChildOffsetSet = true;
                childOffsetCx = (long) Helper.ToInt(reader.Value);
              }
              if (reader.MoveToAttribute("cy"))
              {
                shape.ShapeFrame.IsChildOffsetSet = true;
                childOffsetCy = (long) Helper.ToInt(reader.Value);
                continue;
              }
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          reader.Skip();
      }
    }
    reader.Read();
    if (isShapeTransfm)
    {
      shape.ShapeFrame.SetAnchor(flipV, flipH, rotation, offsetX, offsetY, offsetCx, offsetCy);
      shape.ShapeFrame.SetChildAnchor(childOffsetX, childOffsetY, childOffsetCx, childOffsetCy);
    }
    else
      (shape.TextBody as TextBody).SetAnchor(rotation, offsetX, offsetY, offsetCx, offsetCy);
  }

  internal static ColorObject ParseColorChoice(XmlReader xmlReader, MasterSlide masterSlide)
  {
    ColorObject colorChoice = new ColorObject(true);
    xmlReader.Read();
    while (xmlReader.NodeType != XmlNodeType.EndElement)
    {
      if (xmlReader.NodeType == XmlNodeType.Element)
      {
        switch (xmlReader.LocalName)
        {
          case "srgbClr":
            DrawingParser.ParseSrgbColors(xmlReader, colorChoice);
            DrawingParser.ParseColorTransform(xmlReader, colorChoice);
            continue;
          case "scrgbClr":
            DrawingParser.ParseScRgbColors(xmlReader, colorChoice);
            DrawingParser.ParseColorTransform(xmlReader, colorChoice);
            continue;
          case "schemeClr":
            DrawingParser.ParseScheme(xmlReader, colorChoice, masterSlide);
            continue;
          case "sysClr":
            DrawingParser.ParseSystemColor(xmlReader, colorChoice);
            DrawingParser.ParseColorTransform(xmlReader, colorChoice);
            continue;
          case "prstClr":
            DrawingParser.ParsePresetColors(xmlReader, colorChoice);
            DrawingParser.ParseColorTransform(xmlReader, colorChoice);
            continue;
          default:
            xmlReader.Skip();
            continue;
        }
      }
      else
        xmlReader.Read();
    }
    xmlReader.Read();
    return colorChoice;
  }

  internal static ColorObject ParseColorChoice(XmlReader xmlReader, LayoutSlide layoutSlide)
  {
    ColorObject colorChoice = new ColorObject(true);
    xmlReader.Read();
    while (xmlReader.NodeType != XmlNodeType.EndElement)
    {
      if (xmlReader.NodeType == XmlNodeType.Element)
      {
        switch (xmlReader.LocalName)
        {
          case "srgbClr":
            DrawingParser.ParseSrgbColors(xmlReader, colorChoice);
            DrawingParser.ParseColorTransform(xmlReader, colorChoice);
            continue;
          case "scrgbClr":
            DrawingParser.ParseScRgbColors(xmlReader, colorChoice);
            DrawingParser.ParseColorTransform(xmlReader, colorChoice);
            continue;
          case "schemeClr":
            DrawingParser.ParseScheme(xmlReader, colorChoice, layoutSlide);
            continue;
          case "sysClr":
            DrawingParser.ParseSystemColor(xmlReader, colorChoice);
            DrawingParser.ParseColorTransform(xmlReader, colorChoice);
            continue;
          case "prstClr":
            DrawingParser.ParsePresetColors(xmlReader, colorChoice);
            DrawingParser.ParseColorTransform(xmlReader, colorChoice);
            continue;
          default:
            xmlReader.Skip();
            continue;
        }
      }
      else
        xmlReader.Read();
    }
    xmlReader.Read();
    return colorChoice;
  }

  internal static ColorObject ParseColorChoice(
    XmlReader xmlReader,
    ColorObject color,
    LayoutSlide layoutSlide)
  {
    switch (xmlReader.LocalName)
    {
      case "srgbClr":
        DrawingParser.ParseSrgbColors(xmlReader, color);
        DrawingParser.ParseColorTransform(xmlReader, color);
        break;
      case "scrgbClr":
        DrawingParser.ParseScRgbColors(xmlReader, color);
        DrawingParser.ParseColorTransform(xmlReader, color);
        break;
      case "schemeClr":
        DrawingParser.ParseScheme(xmlReader, color, layoutSlide);
        break;
      case "sysClr":
        DrawingParser.ParseSystemColor(xmlReader, color);
        DrawingParser.ParseColorTransform(xmlReader, color);
        break;
      case "prstClr":
        DrawingParser.ParsePresetColors(xmlReader, color);
        DrawingParser.ParseColorTransform(xmlReader, color);
        break;
      default:
        xmlReader.Skip();
        break;
    }
    return color;
  }

  internal static ColorObject ParseColorChoice(
    XmlReader xmlReader,
    ColorObject color,
    MasterSlide masterSlide)
  {
    switch (xmlReader.LocalName)
    {
      case "srgbClr":
        DrawingParser.ParseSrgbColors(xmlReader, color);
        DrawingParser.ParseColorTransform(xmlReader, color);
        break;
      case "scrgbClr":
        DrawingParser.ParseScRgbColors(xmlReader, color);
        DrawingParser.ParseColorTransform(xmlReader, color);
        break;
      case "schemeClr":
        DrawingParser.ParseScheme(xmlReader, color, masterSlide);
        break;
      case "sysClr":
        DrawingParser.ParseSystemColor(xmlReader, color);
        DrawingParser.ParseColorTransform(xmlReader, color);
        break;
      case "prstClr":
        DrawingParser.ParsePresetColors(xmlReader, color);
        DrawingParser.ParseColorTransform(xmlReader, color);
        break;
      default:
        xmlReader.Skip();
        break;
    }
    return color;
  }

  internal static void ParseScheme(XmlReader xmlReader, ColorObject color, MasterSlide masterSlide)
  {
    DrawingParser.ParseSchemeColors(xmlReader, color, masterSlide);
    DrawingParser.ParseColorTransform(xmlReader, color);
  }

  internal static void ParseSrgbClr(XmlReader xmlReader, ColorObject color)
  {
    DrawingParser.ParseSrgbColors(xmlReader, color);
    DrawingParser.ParseColorTransform(xmlReader, color);
  }

  internal static void ParseScheme(XmlReader xmlReader, ColorObject color, LayoutSlide layoutSlide)
  {
    DrawingParser.ParseSchemeColors(xmlReader, color, layoutSlide);
    DrawingParser.ParseColorTransform(xmlReader, color);
  }

  internal static void ParseGradientFill(XmlReader reader, GradientFill gradientFill)
  {
    if (reader.MoveToAttribute("rotWithShape"))
      gradientFill.RotWithShape = new bool?(reader.Value == "1");
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      string localName = reader.LocalName;
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "gsLst":
              DrawingParser.ParseGradientStops(reader, (GradientStops) gradientFill.GradientStops);
              continue;
            case "lin":
              gradientFill.Type = GradientFillType.Linear;
              gradientFill.ShadeProperties = (object) new LineShadeImpl();
              LineShadeImpl lineShade = gradientFill.GetLineShade();
              if (reader.MoveToAttribute("ang"))
                lineShade.Angle = Helper.ToInt(reader.Value);
              if (reader.MoveToAttribute("scaled"))
              {
                lineShade.IsScaled = reader.Value == "1";
                continue;
              }
              continue;
            case "path":
              gradientFill.ShadeProperties = (object) new PathShadeImpl();
              PathShadeImpl pathShade = gradientFill.GetPathShade();
              DrawingParser.ParsePathShadeProperties(reader, pathShade);
              switch (pathShade.PathShapeType)
              {
                case PathShapeType.Circle:
                  gradientFill.Type = GradientFillType.Radial;
                  continue;
                case PathShapeType.Rectangle:
                  gradientFill.Type = GradientFillType.Rectangle;
                  continue;
                case PathShapeType.Shape:
                  gradientFill.Type = GradientFillType.Shape;
                  continue;
                default:
                  continue;
              }
            case "tileRect":
              gradientFill.TileRectangle = (object) new PathShadeImpl();
              DrawingParser.ParseFillToRectangle(reader, gradientFill.ObtainTileRectangle());
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          reader.Read();
      }
    }
    reader.Read();
  }

  internal static void ParsePatternFill(XmlReader reader, PatternFill patternFill)
  {
    if (reader.MoveToAttribute("prst"))
      patternFill.Pattern = Helper.GetFillPattern(reader.Value);
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      string localName = reader.LocalName;
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "fgClr":
              patternFill.SetForeColorObject(DrawingParser.ParseColorChoice(reader, (MasterSlide) patternFill.Presentation.Masters[0]));
              continue;
            case "bgClr":
              patternFill.SetBackColorObject(DrawingParser.ParseColorChoice(reader, (MasterSlide) patternFill.Presentation.Masters[0]));
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          reader.Read();
      }
    }
    reader.Read();
  }

  internal static void ParseTextureFill(XmlReader reader, TextureFill textureFill)
  {
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      string localName = reader.LocalName;
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "blip":
              DrawingParser.ParseBlipFill(reader, textureFill);
              continue;
            case "stretch":
              DrawingParser.ParsePicFormatOption(reader, textureFill);
              continue;
            case "tile":
              DrawingParser.ParseTilePicOption(reader, textureFill);
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          reader.Read();
      }
    }
    reader.Read();
  }

  internal static void ParseGroupShapes(
    XmlReader reader,
    Shapes shapeCollection,
    GroupShape groupShape)
  {
    List<Shape> shapes = new List<Shape>();
    bool flag = false;
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      string localName = reader.LocalName;
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "nvGrpSpPr":
              DrawingParser.ParseShapeNonVisual(reader, (Shape) groupShape);
              groupShape.GroupName = groupShape.ShapeName;
              continue;
            case "grpSpPr":
              DrawingParser.ParseShapeProperties(reader, (Shape) groupShape);
              flag = true;
              continue;
            case "sp":
              Shape shape1 = new Shape(ShapeType.Sp, shapeCollection.BaseSlide);
              shape1.SetSlideItemType(SlideItemType.AutoShape);
              DrawingParser.ParseGeneralShape(reader, shape1);
              shapes.Add(shape1);
              continue;
            case "cxnSp":
              Shape shape2 = (Shape) new Connector(ShapeType.CxnSp, shapeCollection.BaseSlide);
              shape2.SetSlideItemType(SlideItemType.ConnectionShape);
              DrawingParser.ParseConnectorShape(reader, shape2);
              shapes.Add(shape2);
              continue;
            case "pic":
              Picture picture = new Picture(shapeCollection.BaseSlide);
              picture.SetSlideItemType(SlideItemType.Picture);
              DrawingParser.ParsePicture(reader, picture);
              picture.GetImagePath();
              shapes.Add((Shape) picture);
              continue;
            case "grpSp":
              GroupShape groupShape1 = new GroupShape("Group1", shapeCollection.BaseSlide);
              groupShape1.SetSlideItemType(SlideItemType.GroupShape);
              DrawingParser.ParseGroupShapes(reader, shapeCollection, groupShape1);
              shapes.Add((Shape) groupShape1);
              continue;
            case "AlternateContent":
              shapes.Add(new Shape(ShapeType.AlternateContent, shapeCollection.BaseSlide)
              {
                PreservedElements = {
                  {
                    "AlternateContent",
                    UtilityMethods.ReadSingleNodeIntoStream(reader)
                  }
                }
              });
              continue;
            case "graphicFrame":
              Stream stream = UtilityMethods.ReadSingleNodeIntoStream(reader);
              stream.Position = 0L;
              if (!Parser.CheckTableOrChart(stream, shapes, shapeCollection))
              {
                stream.Position = 0L;
                Shape shape3 = new Shape(ShapeType.GraphicFrame, shapeCollection.BaseSlide);
                shape3.SetSlideItemType(SlideItemType.Unknown);
                shape3.DrawingType = DrawingType.None;
                shape3.PreservedElements.Add("graphicFrame", stream);
                shapes.Add(shape3);
                continue;
              }
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          reader.Skip();
      }
    }
    reader.Read();
    if (!flag || shapes.Count <= 0)
      return;
    shapeCollection.AddGroupShape(groupShape, shapes.ToArray());
  }

  internal static void ParseConnectorShape(XmlReader reader, Shape shape)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (!reader.IsEmptyElement)
    {
      string localName = reader.LocalName;
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "nvCxnSpPr":
              DrawingParser.ParseConnectorShapeNonVisual(reader, shape);
              continue;
            case "spPr":
              DrawingParser.ParseShapeProperties(reader, shape);
              continue;
            case "style":
              shape.PreservedElements.Add("style", UtilityMethods.ReadSingleNodeIntoStream(reader));
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          reader.Skip();
      }
    }
    reader.Read();
  }

  private static void AddPictureToBlipList(Picture picture)
  {
    if (picture.FormatPicture.BlipList == null)
      picture.FormatPicture.BlipList = new List<Stream>();
    picture.FormatPicture.BlipList.Add((Stream) null);
  }

  private static void ParseBlip(XmlReader reader, Picture picture)
  {
    DrawingParser.ParsePictureAttribute(reader, picture);
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      string localName1 = reader.LocalName;
      reader.Read();
      while (!(localName1 == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "extLst":
              if (reader.IsEmptyElement)
              {
                reader.Skip();
                continue;
              }
              DrawingParser.ParseImageToStream(reader, picture);
              continue;
            case "alphaModFix":
              if (reader.MoveToAttribute("amt"))
                picture.Amount = Helper.ToInt(reader.Value);
              reader.Skip();
              continue;
            case "duotone":
              string localName2 = reader.LocalName;
              reader.Read();
              while (!(localName2 == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
              {
                if (reader.NodeType == XmlNodeType.Element)
                {
                  ColorObject color = new ColorObject(true);
                  picture.DuoTone.Add(DrawingParser.ParseColorChoice(reader, color, (MasterSlide) picture.BaseSlide.Presentation.Masters[0]));
                }
              }
              reader.Read();
              continue;
            case "lum":
              DrawingParser.ParseLumProperties(reader, picture);
              continue;
            case "biLevel":
              if (reader.MoveToAttribute("thresh"))
                picture.Threshold = Helper.ToInt(reader.Value);
              reader.Skip();
              continue;
            case "grayscl":
              picture.GrayScale = true;
              reader.Skip();
              continue;
            case "clrChange":
              DrawingParser.ParseColorChangeEffect(reader, picture);
              reader.Skip();
              continue;
            default:
              DrawingParser.AddPictureToBlipList(picture);
              reader.Skip();
              continue;
          }
        }
        else
          reader.Skip();
      }
    }
    reader.Read();
  }

  private static void ParseColorChangeEffect(XmlReader reader, Picture picture)
  {
    if (reader.MoveToAttribute("useA"))
    {
      picture.IsUseAlpha = XmlConvert.ToBoolean(reader.Value);
      reader.MoveToElement();
    }
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "clrFrom":
          case "clrTo":
            reader.Read();
            DrawingParser.SkipWhitespaces(reader);
            ColorObject color = new ColorObject(true, (short) 2);
            picture.ColorChange.Add(DrawingParser.ParseColorChoice(reader, color, (MasterSlide) picture.BaseSlide.Presentation.Masters[0]));
            DrawingParser.SkipWhitespaces(reader);
            reader.Skip();
            continue;
          default:
            continue;
        }
      }
      else
        reader.Skip();
    }
  }

  private static void ParseLumProperties(XmlReader reader, Picture picture)
  {
    if (reader.MoveToAttribute("bright") && reader.Value != null)
      picture.Brightness = Helper.ToInt(reader.Value);
    if (reader.MoveToAttribute("contrast") && reader.Value != null)
      picture.Contrast = Helper.ToInt(reader.Value);
    reader.Read();
  }

  private static void ParseBlipFill(XmlReader reader, Picture picture)
  {
    bool flag = true;
    if (!reader.IsEmptyElement)
    {
      string localName = reader.LocalName;
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "srcRect":
              DrawingParser.SetPictureFormat(reader, picture);
              continue;
            case "blip":
              DrawingParser.ParseBlip(reader, picture);
              continue;
            case "stretch":
              flag = false;
              reader.Skip();
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          reader.Skip();
      }
    }
    reader.Read();
    if (!flag)
      return;
    picture.Flag = true;
  }

  private static void ParseImageToStream(XmlReader reader, Picture picture)
  {
    if (reader.Read())
    {
      if (reader.MoveToAttribute("embed", "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
      {
        if (reader.Value != null)
        {
          DrawingParser.ParsePictureAttribute(reader, picture);
          DrawingParser.AddPictureToBlipList(picture);
        }
      }
      else
      {
        while (!(reader.LocalName == "extLst") || reader.NodeType != XmlNodeType.EndElement)
        {
          reader.Read();
          if (reader.LocalName == "svgBlip")
          {
            if (reader.MoveToAttribute("embed", "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
            {
              if (reader.Value == null)
                break;
              DrawingParser.ParsePictureAttribute(reader, picture);
              DrawingParser.AddPictureToBlipList(picture);
              break;
            }
          }
          else if (reader.LocalName == "imgProps")
          {
            while (!(reader.LocalName == "imgProps") || reader.NodeType != XmlNodeType.EndElement)
            {
              switch (reader.LocalName)
              {
                case "imgLayer":
                  DrawingParser.ParseImageAttribute(reader, picture);
                  break;
                case "sharpenSoften":
                  if (reader.MoveToAttribute("amount"))
                  {
                    picture.ImageAmount = Helper.ToInt(reader.Value);
                    break;
                  }
                  break;
                case "brightnessContrast":
                  if (reader.MoveToAttribute("contrast"))
                    picture.ImageContrast = Helper.ToInt(reader.Value);
                  if (reader.MoveToAttribute("bright"))
                  {
                    picture.Bright = Helper.ToInt(reader.Value);
                    break;
                  }
                  break;
                case "colorTemperature":
                  if (reader.MoveToAttribute("colorTemp"))
                  {
                    picture.ColorTemp = Helper.ToInt(reader.Value);
                    break;
                  }
                  break;
                case "saturation":
                  if (reader.MoveToAttribute("sat"))
                  {
                    picture.Saturation = Helper.ToInt(reader.Value);
                    break;
                  }
                  break;
              }
              reader.Read();
            }
          }
        }
        return;
      }
    }
    do
      ;
    while (reader.Read() && !(reader.LocalName == "blip"));
  }

  private static void ParsePictureAttribute(XmlReader reader, Picture picture)
  {
    string attribute1 = reader.GetAttribute("embed", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
    string attribute2 = reader.GetAttribute("link", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
    if (attribute1 != null)
    {
      if (picture.ImageData == null)
      {
        picture.EmbedId = attribute1;
        picture.IsEmbed = true;
      }
      picture.AddImageStream(attribute1);
    }
    if (attribute2 == null)
      return;
    picture.LinkId = attribute2;
    picture.IsLink = true;
  }

  private static void ParseImageAttribute(XmlReader reader, Picture picture)
  {
    string attribute = reader.GetAttribute("embed", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
    if (attribute == null)
      return;
    picture.ImageEmbedId = attribute;
  }

  private static void SetPictureFormat(XmlReader reader, Picture picture)
  {
    FormatPicture formatPicture = picture.FormatPicture;
    string attribute1 = reader.GetAttribute("l");
    if (attribute1 != null)
      formatPicture.AssignLeftCrop(Helper.ToInt(attribute1));
    string attribute2 = reader.GetAttribute("b");
    if (attribute2 != null)
      formatPicture.AssignBottomCrop(Helper.ToInt(attribute2));
    string attribute3 = reader.GetAttribute("r");
    if (attribute3 != null)
      formatPicture.AssignRightCrop(Helper.ToInt(attribute3));
    string attribute4 = reader.GetAttribute("t");
    if (attribute4 != null)
      formatPicture.AssignTopCrop(Helper.ToInt(attribute4));
    reader.Skip();
  }

  internal static void ParseApplicationNonVisualDrawing(
    XmlReader reader,
    Shape shape,
    bool isGraphicFrame)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.MoveToAttribute("isPhoto"))
      shape.IsPhoto = XmlConvert.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("userDrawn"))
      shape.IsUserDrawn = XmlConvert.ToBoolean(reader.Value);
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      string localName = reader.LocalName;
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "ph":
              if (!isGraphicFrame)
              {
                Placeholder placeholder = new Placeholder(shape);
                DrawingParser.ParsePlaceHolder(reader, placeholder);
                shape.DrawingType = DrawingType.PlaceHolder;
                shape.SetSlideItemType(SlideItemType.Placeholder);
                shape.SetPlaceholder(placeholder);
                continue;
              }
              reader.Skip();
              continue;
            case "videoFile":
              if (!isGraphicFrame)
              {
                string attribute = reader.GetAttribute("link", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
                if (attribute != null)
                {
                  shape.VideoRelationId = attribute;
                  shape.SetVideoPath(attribute);
                  reader.Skip();
                  continue;
                }
                continue;
              }
              reader.Skip();
              continue;
            case "custDataLst":
            case "extLst":
              shape.PreservedElements.Add(reader.LocalName, UtilityMethods.ReadSingleNodeIntoStream(reader));
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          reader.Skip();
      }
    }
    reader.Read();
  }

  private static void ParseLineEndProperties(XmlReader reader, LineFormat lineFormat, bool isHead)
  {
    if (reader.MoveToAttribute("len"))
    {
      if (isHead)
        lineFormat.SetBeginArrowheadLength(Helper.GetArrowHeadLength(reader.Value));
      else
        lineFormat.SetEndArrowheadLength(Helper.GetArrowHeadLength(reader.Value));
    }
    if (reader.MoveToAttribute("type"))
    {
      if (isHead)
        lineFormat.SetBeginArrowheadStyle(Helper.GetArrowHeadStyle(reader.Value));
      else
        lineFormat.SetEndArrowheadStyle(Helper.GetArrowHeadStyle(reader.Value));
    }
    if (reader.MoveToAttribute("w"))
    {
      if (isHead)
        lineFormat.SetBeginArrowheadWidth(Helper.GetArrowHeadWidth(reader.Value));
      else
        lineFormat.SetEndArrowheadWidth(Helper.GetArrowHeadWidth(reader.Value));
    }
    reader.Skip();
  }

  private static void ParsePresetDashProperties(XmlReader reader, LineFormat lineFormat)
  {
    string attribute = reader.GetAttribute("val");
    switch (attribute)
    {
      case "dot":
        if (lineFormat.CapStyle == LineCapStyle.Round)
        {
          lineFormat.SetDashStyle(LineDashStyle.RoundDot);
          goto case null;
        }
        if (lineFormat.CapStyle == LineCapStyle.Flat)
        {
          lineFormat.SetDashStyle(LineDashStyle.Dot);
          goto case null;
        }
        lineFormat.SetDashStyle(LineDashStyle.SquareDot);
        goto case null;
      case null:
        reader.Skip();
        break;
      default:
        lineFormat.SetDashStyle(Helper.GetLineDashStyle(attribute));
        goto case null;
    }
  }

  private static void ParsePresetGeomentry(XmlReader reader, Shape shape)
  {
    string attribute = reader.GetAttribute("prst");
    if (attribute != null)
    {
      AutoShapeType autoShapeType = AutoShapeHelper.GetAutoShapeType(AutoShapeHelper.GetAutoShapeConstant(attribute));
      shape.AutoShapeType = autoShapeType;
      if (shape.ShapeType == ShapeType.CxnSp && shape is Connector connector)
        connector.SetConnectorType(connector.GetConnectorTypeWithAutoShapeType(shape.AutoShapeType));
      reader.MoveToElement();
    }
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (reader.LocalName == "avLst")
        {
          if (!reader.IsEmptyElement)
          {
            DrawingParser.ParseGeomGuideList(reader, shape);
            reader.Skip();
          }
          else
          {
            reader.Read();
            DrawingParser.SkipWhitespaces(reader);
            break;
          }
        }
      }
      else
        reader.Skip();
    }
  }

  internal static void SkipWhitespaces(XmlReader reader)
  {
    if (reader.NodeType == XmlNodeType.Element)
      return;
    while (reader.NodeType == XmlNodeType.Whitespace)
      reader.Read();
  }

  private static void ParseGeomGuideList(XmlReader reader, Shape shape)
  {
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (reader.LocalName == "gd")
        {
          string key = (string) null;
          string str = (string) null;
          if (reader.MoveToAttribute("name"))
            key = reader.Value;
          if (reader.MoveToAttribute("fmla"))
            str = reader.Value;
          Dictionary<string, string> guideList = shape.GetGuideList();
          if (key != null && str != null)
            guideList.Add(key, str);
          reader.Skip();
        }
      }
      else
        reader.Skip();
    }
  }

  private static void ParseShapeNonVisual(XmlReader reader, Shape shape)
  {
    if (!reader.IsEmptyElement)
    {
      string localName = reader.LocalName;
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "cNvPr":
              DrawingParser.ParseNonVisualDrawingProps(reader, shape);
              continue;
            case "cNvSpPr":
              if (reader.MoveToAttribute("txBox") && XmlConvert.ToBoolean(reader.Value))
              {
                shape.DrawingType = DrawingType.TextBox;
                reader.MoveToElement();
              }
              DrawingParser.ParseNonVisualShapeProperties(reader, shape);
              reader.Skip();
              continue;
            case "cNvCxnSpPr":
            case "cNvGrpSpPr":
              if (reader.MoveToAttribute("txBox") && XmlConvert.ToBoolean(reader.Value))
                shape.DrawingType = DrawingType.TextBox;
              reader.Skip();
              continue;
            case "nvPr":
              DrawingParser.ParseApplicationNonVisualDrawing(reader, shape, false);
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          reader.Skip();
      }
    }
    reader.Read();
  }

  private static void ParseNonVisualShapeProperties(XmlReader reader, Shape shape)
  {
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (reader.LocalName == "spLocks")
        {
          if (reader.MoveToAttribute("noChangeAspect"))
          {
            shape.IsLockAspectRatio = XmlConvert.ToBoolean(reader.Value);
            reader.MoveToElement();
          }
          reader.Skip();
        }
        else
          reader.Skip();
      }
      else
        reader.Skip();
    }
  }

  internal static void ParseShapeProperties(XmlReader reader, Shape shape)
  {
    if (reader.MoveToAttribute("bwMode"))
      shape.ShapeFrame.BlackWhiteMode = Helper.GetBlackWhiteMode(reader.Value);
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      string localName = reader.LocalName;
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "xfrm":
              DrawingParser.ParseTransformation(reader, shape, true);
              continue;
            case "custGeom":
              shape.IsCustomGeometry = true;
              DrawingParser.ParseCustomGeometry(reader, shape);
              reader.Skip();
              continue;
            case "prstGeom":
              shape.IsPresetGeometry = true;
              DrawingParser.ParsePresetGeomentry(reader, shape);
              reader.Skip();
              continue;
            case "noFill":
              shape.Fill.FillType = FillType.None;
              reader.Skip();
              continue;
            case "solidFill":
              shape.Fill.FillType = FillType.Solid;
              MasterSlide baseSlide1 = shape.BaseSlide as MasterSlide;
              LayoutSlide baseSlide2 = shape.BaseSlide as LayoutSlide;
              ColorObject colorObject = new ColorObject();
              ColorObject colorChoice;
              if (baseSlide1 == null && baseSlide2 == null)
              {
                MasterSlide masterSlide = shape.BaseSlide is NotesMasterSlide || shape.BaseSlide is NotesSlide || shape.BaseSlide is HandoutMaster ? shape.BaseSlide.Presentation.Masters[0] as MasterSlide : (MasterSlide) shape.BaseSlide.BaseTheme.BaseSlide;
                colorChoice = DrawingParser.ParseColorChoice(reader, masterSlide);
              }
              else if (baseSlide2 != null && baseSlide2.ColorMap.Count != 0)
                colorChoice = DrawingParser.ParseColorChoice(reader, baseSlide2);
              else if (baseSlide2 != null)
              {
                MasterSlide masterSlide = baseSlide2.MasterSlide as MasterSlide;
                colorChoice = DrawingParser.ParseColorChoice(reader, masterSlide);
              }
              else
                colorChoice = DrawingParser.ParseColorChoice(reader, baseSlide1);
              ((SolidFill) shape.Fill.SolidFill).SetColorObject(colorChoice);
              continue;
            case "gradFill":
              shape.Fill.FillType = FillType.Gradient;
              GradientFill gradientFill = (GradientFill) shape.Fill.GradientFill;
              DrawingParser.ParseGradientFill(reader, gradientFill);
              continue;
            case "pattFill":
              shape.Fill.FillType = FillType.Pattern;
              PatternFill patternFill = (PatternFill) shape.Fill.PatternFill;
              DrawingParser.ParsePatternFill(reader, patternFill);
              continue;
            case "blipFill":
              shape.Fill.FillType = FillType.Picture;
              TextureFill pictureFill = (TextureFill) shape.Fill.PictureFill;
              DrawingParser.ParseTextureFill(reader, pictureFill);
              continue;
            case "ln":
              if (shape.ShapeType == ShapeType.Point || shape.ShapeType == ShapeType.Drawing)
                shape.HasLineProperties = true;
              DrawingParser.ParseLineProperties(reader, (LineFormat) shape.LineFormat);
              reader.Read();
              continue;
            case "effectLst":
              shape.EffectList = new EffectList(shape.BaseSlide.Presentation);
              Parser.ParseEffectList(reader, shape.EffectList);
              reader.Skip();
              continue;
            case "grpFill":
              shape.IsGroupFill = true;
              reader.Skip();
              continue;
            default:
              if (!shape.PreservedElements.ContainsKey(reader.LocalName))
              {
                shape.PreservedElements.Add(reader.LocalName, UtilityMethods.ReadSingleNodeIntoStream(reader));
                continue;
              }
              reader.Skip();
              continue;
          }
        }
        else
          reader.Skip();
      }
    }
    reader.Read();
  }

  private static void ParseCustomGeometry(XmlReader reader, Shape shape)
  {
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    Dictionary<string, string> guideList = shape.GetGuideList();
    Dictionary<string, string> avList = shape.GetAvList();
    Dictionary<string, string> combinedValues = new Dictionary<string, string>();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "avLst":
            DrawingParser.ParseGuideList(reader, avList);
            reader.Skip();
            continue;
          case "gdLst":
            DrawingParser.ParseGuideList(reader, guideList);
            reader.Skip();
            continue;
          case "pathLst":
            shape.Path2DList = new List<Path2D>();
            foreach (KeyValuePair<string, string> keyValuePair in guideList)
              combinedValues.Add(keyValuePair.Key, keyValuePair.Value);
            foreach (KeyValuePair<string, string> keyValuePair in avList)
            {
              if (!guideList.ContainsKey(keyValuePair.Key))
                combinedValues.Add(keyValuePair.Key, keyValuePair.Value);
            }
            DrawingParser.ParsePath2D(reader, shape, combinedValues);
            combinedValues.Clear();
            reader.Skip();
            continue;
          default:
            shape.PreservedElements.Add(reader.LocalName, UtilityMethods.ReadSingleNodeIntoStream(reader));
            continue;
        }
      }
      else
        reader.Skip();
    }
    DrawingParser.SetReaderPosition(reader);
  }

  internal static void ParseGuideList(XmlReader reader, Dictionary<string, string> gdValues)
  {
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (reader.LocalName == "gdLst" || reader.LocalName == "avLst")
    {
      localName = reader.LocalName;
      reader.Read();
    }
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (reader.LocalName == "gd")
        {
          string key = (string) null;
          string str = (string) null;
          if (reader.MoveToAttribute("name"))
            key = reader.Value;
          if (reader.MoveToAttribute("fmla"))
            str = reader.Value;
          if (!gdValues.ContainsKey(key))
            gdValues.Add(key, str);
          else
            gdValues[key] = str;
        }
        reader.Skip();
      }
      else
        reader.Skip();
    }
  }

  internal static void SetReaderPosition(XmlReader reader)
  {
    while (reader.LocalName != "custGeom")
      reader.Read();
  }

  internal static void ParsePath2D(
    XmlReader reader,
    Shape shape,
    Dictionary<string, string> combinedValues)
  {
    List<Path2D> path2Dlist = shape.Path2DList;
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (reader.LocalName == "pathLst")
    {
      localName = reader.LocalName;
      reader.Read();
    }
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (reader.LocalName == "path")
        {
          Path2D path = new Path2D();
          if (reader.MoveToAttribute("w"))
            path.Width = Helper.ToDouble(reader.Value);
          if (reader.MoveToAttribute("h"))
            path.Height = Helper.ToDouble(reader.Value);
          if (reader.MoveToAttribute("fill"))
            path.FillMode = Helper.GetPathFillMode(reader.Value);
          if (reader.MoveToAttribute("stroke"))
            path.IsStroke = reader.Value == "1";
          if (path.Width == 0.0)
            path.Width = (double) shape.ShapeFrame.OffsetCX;
          if (path.Height == 0.0)
            path.Height = (double) shape.ShapeFrame.OffsetCY;
          reader.MoveToElement();
          DrawingParser.Parse2DElements(reader, path, combinedValues);
          path2Dlist.Add(path);
        }
        reader.Skip();
      }
      else
        reader.Skip();
    }
  }

  private static void Parse2DElements(
    XmlReader reader,
    Path2D path,
    Dictionary<string, string> combinedValues)
  {
    Dictionary<string, float> calculatedValues = new Dictionary<string, float>();
    if (!reader.IsEmptyElement)
    {
      string localName = reader.LocalName;
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "close":
              path.PathElements.Add(1.0);
              path.PathElements.Add(0.0);
              reader.Skip();
              continue;
            case "moveTo":
              path.PathElements.Add(2.0);
              path.PathElements.Add(1.0);
              DrawingParser.ParsePath2DPoint(reader, path.PathElements, combinedValues, path, calculatedValues);
              reader.Skip();
              continue;
            case "lnTo":
              path.PathElements.Add(3.0);
              path.PathElements.Add(1.0);
              DrawingParser.ParsePath2DPoint(reader, path.PathElements, combinedValues, path, calculatedValues);
              reader.Skip();
              continue;
            case "quadBezTo":
              path.PathElements.Add(5.0);
              path.PathElements.Add(2.0);
              DrawingParser.ParsePath2DPoint(reader, path.PathElements, combinedValues, path, calculatedValues);
              reader.Skip();
              continue;
            case "cubicBezTo":
              path.PathElements.Add(6.0);
              path.PathElements.Add(3.0);
              DrawingParser.ParsePath2DPoint(reader, path.PathElements, combinedValues, path, calculatedValues);
              reader.Skip();
              continue;
            case "arcTo":
              path.PathElements.Add(4.0);
              path.PathElements.Add(4.0);
              if (reader.MoveToAttribute("wR"))
                DrawingParser.ParsePathPoints(reader, path.PathElements, combinedValues, path, calculatedValues);
              if (reader.MoveToAttribute("hR"))
                DrawingParser.ParsePathPoints(reader, path.PathElements, combinedValues, path, calculatedValues);
              if (reader.MoveToAttribute("stAng"))
                DrawingParser.ParsePathPoints(reader, path.PathElements, combinedValues, path, calculatedValues);
              if (reader.MoveToAttribute("swAng"))
                DrawingParser.ParsePathPoints(reader, path.PathElements, combinedValues, path, calculatedValues);
              reader.Skip();
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          reader.Skip();
      }
    }
    calculatedValues.Clear();
  }

  private static void ParsePath2DPoint(
    XmlReader reader,
    List<double> pathElements,
    Dictionary<string, string> combinedValues,
    Path2D path,
    Dictionary<string, float> calculatedValues)
  {
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (!(reader.LocalName == "pt"))
          break;
        if (reader.MoveToAttribute("x"))
          DrawingParser.ParsePathPoints(reader, pathElements, combinedValues, path, calculatedValues);
        if (reader.MoveToAttribute("y"))
          DrawingParser.ParsePathPoints(reader, pathElements, combinedValues, path, calculatedValues);
        reader.Skip();
      }
      else
        reader.Skip();
    }
  }

  private static void ParsePathPoints(
    XmlReader reader,
    List<double> pathElements,
    Dictionary<string, string> combinedValues,
    Path2D path,
    Dictionary<string, float> calculatedValues)
  {
    int result = 0;
    if (int.TryParse(reader.Value, out result))
      pathElements.Add(Helper.ToDouble(reader.Value));
    else if (combinedValues != null && combinedValues.Count > 0 && calculatedValues.Count == 0)
    {
      FormulaValues formulaValues1 = new FormulaValues(new RectangleF(0.0f, 0.0f, (float) path.Width, (float) path.Height), new Dictionary<string, string>());
      Dictionary<string, float> formulaValues2 = formulaValues1.GetFormulaValues(AutoShapeType.Unknown, combinedValues, false);
      foreach (KeyValuePair<string, float> keyValuePair in formulaValues2)
        calculatedValues.Add(keyValuePair.Key, keyValuePair.Value);
      formulaValues1.Close();
      formulaValues2.Clear();
      if (!calculatedValues.ContainsKey(reader.Value))
        return;
      pathElements.Add((double) calculatedValues[reader.Value]);
    }
    else
    {
      if (!calculatedValues.ContainsKey(reader.Value))
        return;
      pathElements.Add((double) calculatedValues[reader.Value]);
    }
  }

  private static string GetValueAttribute(XmlReader xmlReader)
  {
    string attribute = xmlReader.GetAttribute("val");
    xmlReader.Skip();
    return attribute;
  }

  private static bool IsElement(XmlReader xmlReader)
  {
    while (true)
    {
      switch (xmlReader.NodeType)
      {
        case XmlNodeType.None:
          goto label_1;
        case XmlNodeType.Element:
          goto label_2;
        case XmlNodeType.EndElement:
          goto label_3;
        default:
          xmlReader.Skip();
          continue;
      }
    }
label_1:
    return false;
label_2:
    return true;
label_3:
    xmlReader.ReadEndElement();
    return false;
  }

  internal static void ParseColorTransform(XmlReader xmlReader, ColorObject colorObject)
  {
    if (xmlReader.IsEmptyElement)
    {
      xmlReader.Skip();
    }
    else
    {
      ColorTransFormCollection transFormCollection = colorObject.ColorTransFormCollection;
      colorObject.IsShapeColor = true;
      xmlReader.Read();
      while (DrawingParser.IsElement(xmlReader))
      {
        if (xmlReader.LocalName == "alpha")
        {
          string valueAttribute = DrawingParser.GetValueAttribute(xmlReader);
          transFormCollection.AddColorTransForm(ColorMode.Alpha, Helper.ToInt(valueAttribute));
        }
        else if (xmlReader.LocalName == "alphaMod")
        {
          string valueAttribute = DrawingParser.GetValueAttribute(xmlReader);
          transFormCollection.AddColorTransForm(ColorMode.AlphaMod, Helper.ToInt(valueAttribute));
        }
        else if (xmlReader.LocalName == "alphaOff")
        {
          string valueAttribute = DrawingParser.GetValueAttribute(xmlReader);
          transFormCollection.AddColorTransForm(ColorMode.AlphaOff, Helper.ToInt(valueAttribute));
        }
        else if (xmlReader.LocalName == "red")
        {
          string valueAttribute = DrawingParser.GetValueAttribute(xmlReader);
          transFormCollection.AddColorTransForm(ColorMode.Red, Helper.ToInt(valueAttribute));
        }
        else if (xmlReader.LocalName == "redMod")
        {
          string valueAttribute = DrawingParser.GetValueAttribute(xmlReader);
          transFormCollection.AddColorTransForm(ColorMode.RedMod, Helper.ToInt(valueAttribute));
        }
        else if (xmlReader.LocalName == "redOff")
        {
          string valueAttribute = DrawingParser.GetValueAttribute(xmlReader);
          transFormCollection.AddColorTransForm(ColorMode.RedOff, Helper.ToInt(valueAttribute));
        }
        else if (xmlReader.LocalName == "green")
        {
          string valueAttribute = DrawingParser.GetValueAttribute(xmlReader);
          transFormCollection.AddColorTransForm(ColorMode.Green, Helper.ToInt(valueAttribute));
        }
        else if (xmlReader.LocalName == "greenMod")
        {
          string valueAttribute = DrawingParser.GetValueAttribute(xmlReader);
          transFormCollection.AddColorTransForm(ColorMode.GreenMod, Helper.ToInt(valueAttribute));
        }
        else if (xmlReader.LocalName == "greenOff")
        {
          string valueAttribute = DrawingParser.GetValueAttribute(xmlReader);
          transFormCollection.AddColorTransForm(ColorMode.GreenOff, Helper.ToInt(valueAttribute));
        }
        else if (xmlReader.LocalName == "blue")
        {
          string valueAttribute = DrawingParser.GetValueAttribute(xmlReader);
          transFormCollection.AddColorTransForm(ColorMode.Blue, Helper.ToInt(valueAttribute));
        }
        else if (xmlReader.LocalName == "blueMod")
        {
          string valueAttribute = DrawingParser.GetValueAttribute(xmlReader);
          transFormCollection.AddColorTransForm(ColorMode.BlueMod, Helper.ToInt(valueAttribute));
        }
        else if (xmlReader.LocalName == "blueOff")
        {
          string valueAttribute = DrawingParser.GetValueAttribute(xmlReader);
          transFormCollection.AddColorTransForm(ColorMode.BlueOff, Helper.ToInt(valueAttribute));
        }
        else if (xmlReader.LocalName == "hue")
        {
          string valueAttribute = DrawingParser.GetValueAttribute(xmlReader);
          transFormCollection.AddColorTransForm(ColorMode.Hue, Helper.ToInt(valueAttribute));
        }
        else if (xmlReader.LocalName == "hueMod")
        {
          string valueAttribute = DrawingParser.GetValueAttribute(xmlReader);
          transFormCollection.AddColorTransForm(ColorMode.HueMod, Helper.ToInt(valueAttribute));
        }
        else if (xmlReader.LocalName == "hueOff")
        {
          string valueAttribute = DrawingParser.GetValueAttribute(xmlReader);
          transFormCollection.AddColorTransForm(ColorMode.HueOff, Helper.ToInt(valueAttribute));
        }
        else if (xmlReader.LocalName == "sat")
        {
          string valueAttribute = DrawingParser.GetValueAttribute(xmlReader);
          transFormCollection.AddColorTransForm(ColorMode.Sat, Helper.ToInt(valueAttribute));
        }
        else if (xmlReader.LocalName == "satMod")
        {
          string valueAttribute = DrawingParser.GetValueAttribute(xmlReader);
          transFormCollection.AddColorTransForm(ColorMode.SatMod, Helper.ToInt(valueAttribute));
        }
        else if (xmlReader.LocalName == "satOff")
        {
          string valueAttribute = DrawingParser.GetValueAttribute(xmlReader);
          transFormCollection.AddColorTransForm(ColorMode.SatOff, Helper.ToInt(valueAttribute));
        }
        else if (xmlReader.LocalName == "lum")
        {
          string valueAttribute = DrawingParser.GetValueAttribute(xmlReader);
          transFormCollection.AddColorTransForm(ColorMode.Lum, Helper.ToInt(valueAttribute));
        }
        else if (xmlReader.LocalName == "lumMod")
        {
          string valueAttribute = DrawingParser.GetValueAttribute(xmlReader);
          transFormCollection.AddColorTransForm(ColorMode.LumMod, Helper.ToInt(valueAttribute));
        }
        else if (xmlReader.LocalName == "lumOff")
        {
          string valueAttribute = DrawingParser.GetValueAttribute(xmlReader);
          transFormCollection.AddColorTransForm(ColorMode.LumOff, Helper.ToInt(valueAttribute));
        }
        else if (xmlReader.LocalName == "shade")
        {
          string valueAttribute = DrawingParser.GetValueAttribute(xmlReader);
          transFormCollection.AddColorTransForm(ColorMode.Shade, Helper.ToInt(valueAttribute));
        }
        else if (xmlReader.LocalName == "tint")
        {
          string valueAttribute = DrawingParser.GetValueAttribute(xmlReader);
          transFormCollection.AddColorTransForm(ColorMode.Tint, Helper.ToInt(valueAttribute));
        }
        else if (xmlReader.LocalName == "comp")
        {
          DrawingParser.GetValueAttribute(xmlReader);
          transFormCollection.AddColorTransForm(ColorMode.Comp, 0);
        }
        else if (xmlReader.LocalName == "inv")
        {
          DrawingParser.GetValueAttribute(xmlReader);
          transFormCollection.AddColorTransForm(ColorMode.Inv, 0);
        }
        else if (xmlReader.LocalName == "gamma")
        {
          DrawingParser.GetValueAttribute(xmlReader);
          transFormCollection.AddColorTransForm(ColorMode.Gamma, 0);
        }
        else if (xmlReader.LocalName == "invGamma")
        {
          DrawingParser.GetValueAttribute(xmlReader);
          transFormCollection.AddColorTransForm(ColorMode.InvGamma, 0);
        }
        else if (xmlReader.LocalName == "gray")
        {
          DrawingParser.GetValueAttribute(xmlReader);
          transFormCollection.AddColorTransForm(ColorMode.Gray, 0);
        }
      }
    }
  }

  internal static void ParsePresetColors(XmlReader xmlReader, ColorObject colorObject)
  {
    string attribute = xmlReader.GetAttribute("val");
    if (attribute == null)
      return;
    colorObject.ColorName = attribute;
    colorObject.SetColor(ColorType.RGB, Helper.GetColorFromName(attribute).ToArgb());
  }

  private static void ParseSchemeColors(
    XmlReader xmlReader,
    ColorObject colorObject,
    MasterSlide masterSlide)
  {
    string attribute = xmlReader.GetAttribute("val");
    if (attribute == null)
      return;
    string themeIndex = Helper.GetThemeIndex(attribute, masterSlide);
    colorObject.ReplaceColor = themeIndex;
    colorObject.SetColor(ColorType.Theme, themeIndex);
  }

  internal static void ParseSchemeColors(
    XmlReader xmlReader,
    ColorObject colorObject,
    LayoutSlide layoutSlide)
  {
    string attribute = xmlReader.GetAttribute("val");
    if (attribute == null)
      return;
    string themeIndex = Helper.GetThemeIndex(attribute, layoutSlide);
    colorObject.SetColor(ColorType.Theme, themeIndex);
  }

  internal static void ParseScRgbColors(XmlReader xmlReader, ColorObject colorObject)
  {
    string attribute1 = xmlReader.GetAttribute("r");
    string attribute2 = xmlReader.GetAttribute("g");
    string attribute3 = xmlReader.GetAttribute("b");
    if (attribute1 == null || attribute2 == null)
      return;
    if (attribute3 == null)
      return;
    try
    {
      byte components1 = (byte) Helper.ParseComponents((double) Helper.ToInt(attribute1) / 100000.0);
      byte components2 = (byte) Helper.ParseComponents((double) Helper.ToInt(attribute2) / 100000.0);
      byte components3 = (byte) Helper.ParseComponents((double) Helper.ToInt(attribute3) / 100000.0);
      colorObject.SetColor(ColorType.RGB, ColorObject.FromArgb((int) byte.MaxValue, (int) components1, (int) components2, (int) components3).ToArgb());
    }
    catch
    {
      throw new Exception();
    }
  }

  internal static void ParseSrgbColors(XmlReader xmlReader, ColorObject colorObject)
  {
    string attribute = xmlReader.GetAttribute("val");
    if (attribute == null)
      return;
    colorObject.SetColor(ColorType.RGB, Helper.GetColor(attribute).ToArgb());
  }

  internal static void ParseSystemColor(XmlReader xmlReader, ColorObject colorObject)
  {
    string attribute = xmlReader.GetAttribute("val");
    string str = xmlReader.GetAttribute("lastClr");
    if (str == null)
    {
      switch (attribute)
      {
        case "window":
          str = "ffffff";
          break;
        default:
          if (attribute != null && attribute.Equals("windowText"))
          {
            str = "000000";
            break;
          }
          break;
      }
    }
    if (str == null)
      return;
    colorObject.SetColor(ColorType.RGB, Helper.GetColor(str).ToArgb());
  }

  private static void ParseFillToRectangle(XmlReader reader, PathShadeImpl pathShade)
  {
    if (reader.MoveToAttribute("l"))
      pathShade.Left = Helper.ToInt(reader.Value);
    if (reader.MoveToAttribute("t"))
      pathShade.Top = Helper.ToInt(reader.Value);
    if (reader.MoveToAttribute("r"))
      pathShade.Right = Helper.ToInt(reader.Value);
    if (reader.MoveToAttribute("b"))
      pathShade.Bottom = Helper.ToInt(reader.Value);
    reader.Skip();
  }

  private static void ParseGradientStops(XmlReader reader, GradientStops collection)
  {
    if (!reader.IsEmptyElement)
    {
      string localName = reader.LocalName;
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "gs":
              GradientStop gradientStop = new GradientStop(collection);
              if (reader.MoveToAttribute("pos"))
                gradientStop.Position = (float) Helper.ToInt(reader.Value) / 1000f;
              ColorObject colorObject = new ColorObject();
              MasterSlide baseSlide1 = gradientStop.GradientStops.BaseSlide as MasterSlide;
              LayoutSlide baseSlide2 = gradientStop.GradientStops.BaseSlide as LayoutSlide;
              ColorObject colorChoice;
              if (baseSlide1 == null && baseSlide2 == null)
              {
                MasterSlide masterSlide = gradientStop.GradientStops.BaseSlide.BaseTheme.BaseSlide is HandoutMaster || gradientStop.GradientStops.BaseSlide.BaseTheme.BaseSlide is NotesMasterSlide ? (MasterSlide) gradientStop.GradientStops.Presentation.Masters[0] : (MasterSlide) gradientStop.GradientStops.BaseSlide.BaseTheme.BaseSlide;
                colorChoice = DrawingParser.ParseColorChoice(reader, masterSlide);
              }
              else if (baseSlide2 != null)
              {
                if (baseSlide2.ColorMap.Count != 0)
                {
                  colorChoice = DrawingParser.ParseColorChoice(reader, baseSlide2);
                }
                else
                {
                  MasterSlide masterSlide = baseSlide2.GetMasterSlide(gradientStop.GradientStops.Presentation);
                  colorChoice = DrawingParser.ParseColorChoice(reader, masterSlide);
                }
              }
              else
                colorChoice = DrawingParser.ParseColorChoice(reader, baseSlide1);
              gradientStop.SetColorObject(colorChoice);
              collection.Add(gradientStop);
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          reader.Read();
      }
    }
    reader.Read();
  }

  private static void ParsePathShadeProperties(XmlReader reader, PathShadeImpl pathShade)
  {
    if (reader.MoveToAttribute("path"))
      pathShade.PathShapeType = Helper.GetPathShapeType(reader.Value);
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      string localName = reader.LocalName;
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "fillToRect":
              DrawingParser.ParseFillToRectangle(reader, pathShade);
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          reader.Read();
      }
    }
    reader.Read();
  }

  private static void ParseBlipFill(XmlReader reader, TextureFill textureFill)
  {
    string attribute = reader.GetAttribute("embed", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
    if (attribute == null)
    {
      reader.Skip();
    }
    else
    {
      textureFill.ImageRelationId = attribute;
      textureFill.AddImageStream();
      if (!reader.IsEmptyElement)
      {
        string localName1 = reader.LocalName;
        reader.Read();
        while (!(localName1 == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
        {
          if (reader.NodeType == XmlNodeType.Element)
          {
            switch (reader.LocalName)
            {
              case "alphaModFix":
                if (reader.MoveToAttribute("amt"))
                  textureFill.AssignTransparency(Helper.ToInt(reader.Value));
                reader.Skip();
                continue;
              case "duotone":
                string localName2 = reader.LocalName;
                reader.Read();
                MasterSlide masterSlide = (MasterSlide) null;
                while (!(localName2 == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
                {
                  if (reader.NodeType == XmlNodeType.Element)
                  {
                    ColorObject color = new ColorObject(true);
                    textureFill.DuoTone.Add(DrawingParser.ParseColorChoice(reader, color, masterSlide));
                  }
                  else
                    reader.Skip();
                }
                reader.Read();
                continue;
              case "extLst":
                if (!reader.IsEmptyElement)
                {
                  DrawingParser.ParseImageToStream(reader, textureFill);
                  continue;
                }
                reader.Skip();
                continue;
              default:
                reader.Skip();
                continue;
            }
          }
          else
            reader.Read();
        }
      }
      reader.Read();
    }
  }

  private static void AddPictureToBlipList(TextureFill textureFill)
  {
    if (textureFill.FormatPicture.BlipList == null)
      textureFill.FormatPicture.BlipList = new List<Stream>();
    textureFill.FormatPicture.BlipList.Add((Stream) null);
  }

  private static void ParseImageToStream(XmlReader reader, TextureFill textureFill)
  {
    while (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.MoveToAttribute("embed", "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
      {
        if (reader.Value != null)
        {
          DrawingParser.ParsePictureAttribute(reader, textureFill);
          DrawingParser.AddPictureToBlipList(textureFill);
          break;
        }
        while (reader.LocalName != "blip")
          reader.Read();
        return;
      }
    }
    do
      ;
    while (reader.Read() && !(reader.LocalName == "blip"));
  }

  private static void ParsePictureAttribute(XmlReader reader, TextureFill textureFill)
  {
    string attribute = reader.GetAttribute("embed", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
    reader.GetAttribute("link", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
    if (attribute == null || textureFill.ImageRelationId != null)
      return;
    textureFill.ImageRelationId = attribute;
    textureFill.AddImageStream();
  }

  private static void ParsePicFormatOption(XmlReader reader, TextureFill textureFill)
  {
    textureFill.PicFormatOption = new PicFormatOption();
    PicFormatOption picFormatOption = textureFill.PicFormatOption;
    if (!reader.IsEmptyElement)
    {
      string localName = reader.LocalName;
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "fillRect":
              if (reader.MoveToAttribute("l"))
                picFormatOption.SetLeft(Helper.ToInt(reader.Value));
              if (reader.MoveToAttribute("t"))
                picFormatOption.SetTop(Helper.ToInt(reader.Value));
              if (reader.MoveToAttribute("r"))
                picFormatOption.SetRight(Helper.ToInt(reader.Value));
              if (reader.MoveToAttribute("b"))
                picFormatOption.SetBottom(Helper.ToInt(reader.Value));
              reader.Skip();
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          reader.Read();
      }
    }
    reader.Read();
  }

  private static void ParseTilePicOption(XmlReader reader, TextureFill textureFill)
  {
    textureFill.TilePicOption = new TilePicOption();
    if (reader.HasAttributes)
    {
      TilePicOption tilePicOption = textureFill.TilePicOption;
      while (reader.MoveToNextAttribute())
      {
        switch (reader.LocalName)
        {
          case "tx":
            tilePicOption.OffsetX = Helper.ToDouble(reader.Value) / 12700.0;
            continue;
          case "ty":
            tilePicOption.OffsetY = Helper.ToDouble(reader.Value) / 12700.0;
            continue;
          case "sx":
            tilePicOption.ScaleX = Helper.ToDouble(reader.Value) / 1000.0;
            continue;
          case "sy":
            tilePicOption.ScaleY = Helper.ToDouble(reader.Value) / 1000.0;
            continue;
          case "flip":
            tilePicOption.MirrorType = Helper.GetMirrorType(reader.Value);
            continue;
          case "algn":
            tilePicOption.AlignmentType = Helper.GetRectangleAlignType(reader.Value);
            continue;
          default:
            continue;
        }
      }
    }
    reader.Skip();
  }

  private static void ParsePlaceHolder(XmlReader reader, Placeholder placeHolder)
  {
    if (reader.MoveToAttribute("type"))
      placeHolder.SetType(Helper.GetPlaceHolderType(reader.Value));
    if (reader.MoveToAttribute("orient"))
      placeHolder.SetDirection(Helper.GetPlaceHolderDirection(reader.Value));
    if (reader.MoveToAttribute("sz"))
      placeHolder.AssignSize(Helper.GetPlaceHolderSize(reader.Value));
    if (reader.MoveToAttribute("idx"))
      placeHolder.SetIndex(reader.Value);
    if (reader.MoveToAttribute("hasCustomPrompt"))
      placeHolder.HasCustomPrompt = XmlConvert.ToBoolean(reader.Value);
    reader.Skip();
  }

  private static void ParseConnectorShapeNonVisual(XmlReader reader, Shape shape)
  {
    if (!reader.IsEmptyElement)
    {
      string localName = reader.LocalName;
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "cNvPr":
              DrawingParser.ParseNonVisualDrawingProps(reader, shape);
              continue;
            case "cNvCxnSpPr":
              DrawingParser.ParseNonVisualConnectorProperties(reader, shape);
              continue;
            case "nvPr":
              DrawingParser.ParseApplicationNonVisualDrawing(reader, shape, false);
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          reader.Skip();
      }
    }
    reader.Read();
  }

  private static void ParseNonVisualConnectorProperties(XmlReader reader, Shape shape)
  {
    if (!reader.IsEmptyElement)
    {
      string localName = reader.LocalName;
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          Connector connector = shape as Connector;
          switch (reader.LocalName)
          {
            case "stCxn":
              if (reader.MoveToAttribute("id"))
                connector.SetBeginShapeId(Helper.ToInt(reader.Value));
              if (reader.MoveToAttribute("idx"))
              {
                connector.BeginConnectionSiteIndex = Helper.ToInt(reader.Value);
                continue;
              }
              continue;
            case "endCxn":
              if (reader.MoveToAttribute("id"))
                connector.SetEndShapeId(Helper.ToInt(reader.Value));
              if (reader.MoveToAttribute("idx"))
              {
                connector.EndConnectionSiteIndex = Helper.ToInt(reader.Value);
                continue;
              }
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          reader.Skip();
      }
    }
    reader.Read();
  }
}
