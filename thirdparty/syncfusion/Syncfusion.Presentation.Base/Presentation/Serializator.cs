// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Serializator
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Animation;
using Syncfusion.Presentation.CommentImplementation;
using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.RichText;
using Syncfusion.Presentation.SlideImplementation;
using Syncfusion.Presentation.SlideTransition;
using Syncfusion.Presentation.SlideTransition.Internal;
using Syncfusion.Presentation.SmartArtImplementation;
using Syncfusion.Presentation.TableImplementation;
using Syncfusion.Presentation.Themes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.Presentation;

internal class Serializator
{
  private const string PPrefix = "p";
  private const string APrefix = "a";
  private const string RPrefix = "r";
  private const string CorePropertyPrefix = "cp";
  private const string DCMITypePrefix = "dcmitype";
  private const string XSIPrefix = "xsi";
  private const string DGMPrefix = "dgm";
  private const string DGM14Prefix = "dgm14";
  private const string DSPPrefix = "dsp";
  private const string P14Prefix = "p14";
  private const string P15Prefix = "p15";
  private const string AlternetPrefix = "mc";
  private const string P159Prefix = "p159";
  private const string HyperLinkPrefix = "ahyp";
  internal const string DocPropsVTypesPrefix = "vt";
  private const string VersionValue = "12.0000";
  internal const string XSIPartType = "http://www.w3.org/2001/XMLSchema-instance";
  internal const string CorePropertiesPrefix = "cp";
  internal const string DublinCorePartType = "http://purl.org/dc/elements/1.1/";
  internal const string DublinCorePrefix = "dc";
  internal const string DublinCoreTermsPartType = "http://purl.org/dc/terms/";
  internal const string DublinCoreTermsPrefix = "dcterms";
  internal const string DCMITypePartType = "http://purl.org/dc/dcmitype/";
  private const string ExtendedPropertiesPartType = "http://schemas.openxmlformats.org/officeDocument/2006/extended-properties";
  private const string CorePropertiesPartType = "http://schemas.openxmlformats.org/package/2006/metadata/core-properties";

  internal static void SerializeCommonSlide(XmlWriter xmlWriter, BaseSlide slide)
  {
    xmlWriter.WriteStartElement("p", "cSld", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (slide.Name != null)
      xmlWriter.WriteAttributeString("name", slide.Name);
    DrawingSerializator.SerializeBackGround(xmlWriter, slide);
    DrawingSerializator.SerializeShapeTree(xmlWriter, slide);
    xmlWriter.WriteEndElement();
  }

  internal static void SerializeVmlDrawing(XmlWriter xmlWriter, BaseSlide slide)
  {
    xmlWriter.WriteStartDocument(true);
    xmlWriter.WriteStartElement("xml");
    xmlWriter.WriteAttributeString("xmlns", "v", (string) null, "urn:schemas-microsoft-com:vml");
    xmlWriter.WriteAttributeString("xmlns", "o", (string) null, "urn:schemas-microsoft-com:office:office");
    xmlWriter.WriteAttributeString("xmlns", "p", (string) null, "http://schemas.openxmlformats.org/presentationml/2006/main");
    foreach (Shape shape1 in (IEnumerable<ISlideItem>) slide.Shapes)
    {
      if (shape1.SlideItemType == SlideItemType.GroupShape)
      {
        foreach (Shape shape2 in (IEnumerable<ISlideItem>) ((GroupShape) shape1).Shapes)
          Serializator.WriteVmlShapes(shape2, xmlWriter);
      }
      else
        Serializator.WriteVmlShapes(shape1, xmlWriter);
    }
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndDocument();
    xmlWriter.Flush();
  }

  private static void WriteVmlShapes(Shape shape, XmlWriter xmlWriter)
  {
    if (shape.SlideItemType != SlideItemType.OleObject)
      return;
    VmlShape vmlShape = ((OleObject) shape).VmlShape;
    if (vmlShape.ImageRelationId != null)
    {
      xmlWriter.WriteStartElement("v", nameof (shape), "urn:schemas-microsoft-com:vml");
      if (vmlShape.VmlShapeID != null)
        xmlWriter.WriteAttributeString("id", vmlShape.VmlShapeID);
      if (vmlShape.ShapeType != null)
        xmlWriter.WriteAttributeString("type", vmlShape.ShapeType);
      if (vmlShape.Style != null)
        xmlWriter.WriteAttributeString("style", vmlShape.Style);
      xmlWriter.WriteStartElement("v", "imagedata", "urn:schemas-microsoft-com:vml");
      xmlWriter.WriteAttributeString("o", "relid", (string) null, vmlShape.ImageRelationId);
      xmlWriter.WriteEndElement();
      xmlWriter.WriteEndElement();
    }
    else
    {
      xmlWriter.WriteStartElement("v", "rect", "urn:schemas-microsoft-com:vml");
      if (vmlShape.VmlShapeID != null)
        xmlWriter.WriteAttributeString("id", vmlShape.VmlShapeID);
      if (vmlShape.Style != null)
        xmlWriter.WriteAttributeString("style", vmlShape.Style);
      xmlWriter.WriteEndElement();
    }
  }

  internal static void SerializeOleObject(XmlWriter xmlWriter, Shape shape)
  {
    Serializator.SerializeOleData(xmlWriter, shape as OleObject);
    if (((OleObject) shape).OlePicture == null)
      return;
    DrawingSerializator.SerializePicElement(xmlWriter, (Shape) ((OleObject) shape).OlePicture);
  }

  private static void SerializeOleData(XmlWriter xmlWriter, OleObject oleObject)
  {
    xmlWriter.WriteStartElement("oleObj", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (!string.IsNullOrEmpty(oleObject.VMLShapeId))
      xmlWriter.WriteAttributeString("spid", oleObject.VMLShapeId);
    if (!string.IsNullOrEmpty(oleObject.Name))
      xmlWriter.WriteAttributeString("name", oleObject.Name);
    if (oleObject.DisplayAsIcon)
      xmlWriter.WriteAttributeString("showAsIcon", "1");
    xmlWriter.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", oleObject.RelationId);
    if (oleObject.ImageWidth != -1)
      xmlWriter.WriteAttributeString("imgW", oleObject.ImageWidth.ToString());
    if (oleObject.ImageHeight != -1)
      xmlWriter.WriteAttributeString("imgH", oleObject.ImageHeight.ToString());
    xmlWriter.WriteAttributeString("progId", oleObject.ProgID);
    xmlWriter.WriteStartElement(oleObject.LinkType == OleLinkType.Embed ? "embed" : "link", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (oleObject.LinkType == OleLinkType.Link)
      xmlWriter.WriteAttributeString("updateAutomatic", "1");
    xmlWriter.WriteEndElement();
  }

  internal static void SerializeFillProperties(
    XmlWriter xmlWriter,
    Fill fillFormat,
    Syncfusion.Presentation.Presentation presentation)
  {
    switch (fillFormat.FillType)
    {
      case FillType.Solid:
        xmlWriter.WriteStartElement("a", "solidFill", "http://schemas.openxmlformats.org/drawingml/2006/main");
        SolidFill solidFill = (SolidFill) fillFormat.SolidFill;
        Serializator.SerializeSolidFill(xmlWriter, solidFill.GetColorObject(), -1, presentation, false, fillFormat.BaseSlide);
        xmlWriter.WriteEndElement();
        break;
      case FillType.Gradient:
        GradientFill gradientFill = (GradientFill) fillFormat.GradientFill;
        Serializator.SerializeGradientFill(xmlWriter, gradientFill);
        break;
      case FillType.Picture:
      case FillType.Texture:
        TextureFill pictureFill = (TextureFill) fillFormat.PictureFill;
        Serializator.SerializeTextureFill(xmlWriter, pictureFill);
        break;
      case FillType.Pattern:
        PatternFill patternFill = (PatternFill) fillFormat.PatternFill;
        Serializator.SerialisePatternFill(xmlWriter, patternFill);
        break;
      case FillType.None:
        xmlWriter.WriteStartElement("a", "noFill", "http://schemas.openxmlformats.org/drawingml/2006/main");
        xmlWriter.WriteEndElement();
        break;
    }
  }

  internal static void SerializeLineFillProperties(
    XmlWriter xmlWriter,
    LineFormat lineFormat,
    Syncfusion.Presentation.Presentation presentation)
  {
    if (lineFormat.GetWidth() != 0)
      xmlWriter.WriteAttributeString("w", Helper.ToString(lineFormat.GetWidth()));
    if (lineFormat.CapStyle != LineCapStyle.None)
      xmlWriter.WriteAttributeString("cap", Helper.ToString(lineFormat.CapStyle));
    if (lineFormat.Style != LineStyle.Single)
      xmlWriter.WriteAttributeString("cmpd", Helper.ToString(lineFormat.Style));
    if (lineFormat.GetFillFormat() != null)
      Serializator.SerializeFillProperties(xmlWriter, lineFormat.GetFillFormat(), presentation);
    Serializator.SerializeLineDashProperties(xmlWriter, lineFormat);
    Serializator.SerializeLineJoinProperties(xmlWriter, lineFormat.LineJoinType);
    Serializator.SerializeLineEndProperties(xmlWriter, lineFormat, true);
    Serializator.SerializeLineEndProperties(xmlWriter, lineFormat, false);
  }

  internal static void SerializeSlide(XmlWriter xmlWriter, Slide slide)
  {
    xmlWriter.WriteStartDocument(true);
    xmlWriter.WriteStartElement("p", "sld", "http://schemas.openxmlformats.org/presentationml/2006/main");
    xmlWriter.WriteAttributeString("xmlns", "a", (string) null, "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("xmlns", "r", (string) null, "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
    xmlWriter.WriteAttributeString("xmlns", "p", (string) null, "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (!slide.ShowMasterShape)
      xmlWriter.WriteAttributeString("showMasterSp", XmlConvert.ToString(slide.ShowMasterShape));
    if (!slide.Visible)
      xmlWriter.WriteAttributeString("show", XmlConvert.ToString(slide.Visible));
    Serializator.SerializeCommonSlide(xmlWriter, (BaseSlide) slide);
    Serializator.SerializeColorMap(xmlWriter, (BaseSlide) slide);
    Serializator.SerializeSlideTransition(xmlWriter, slide);
    AnimSerializator.SerializeAnimation(xmlWriter, (BaseSlide) slide);
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndDocument();
    xmlWriter.Flush();
  }

  internal static void SerializeTable(XmlWriter xmlWriter, Table table)
  {
    xmlWriter.WriteStartElement("a", "tbl", "http://schemas.openxmlformats.org/drawingml/2006/main");
    Serializator.SerializeTableProperties(xmlWriter, table);
    Serializator.SerializeTableGrid(xmlWriter, table);
    Serializator.SerializeTableRow(xmlWriter, table);
    xmlWriter.WriteEndElement();
  }

  internal static void SerializeTextBody(XmlWriter xmlWriter, TextBody textFrame)
  {
    if (textFrame.GetBaseShape().DrawingType == DrawingType.Table)
      xmlWriter.WriteStartElement("a", "txBody", "http://schemas.openxmlformats.org/drawingml/2006/main");
    else if (textFrame.GetBaseShape().DrawingType == DrawingType.SmartArt)
      xmlWriter.WriteStartElement("dgm", "t", "http://schemas.openxmlformats.org/drawingml/2006/diagram");
    else
      xmlWriter.WriteStartElement("p", "txBody", "http://schemas.openxmlformats.org/presentationml/2006/main");
    Serializator.SerializeTextBodyElements(xmlWriter, textFrame);
    xmlWriter.WriteEndElement();
  }

  internal static void WriteContentType(XmlWriter xmlWriter, Syncfusion.Presentation.Presentation presentation)
  {
    xmlWriter.WriteStartDocument(true);
    xmlWriter.WriteStartElement("Types", "http://schemas.openxmlformats.org/package/2006/content-types");
    foreach (KeyValuePair<string, string> keyValuePair in presentation.DefaultContentType)
      Serializator.WriteDefaultContentType(xmlWriter, keyValuePair.Key, keyValuePair.Value);
    foreach (KeyValuePair<string, string> keyValuePair in presentation.OverrideContentType)
      Serializator.WriteOvverideContentType(xmlWriter, keyValuePair.Key, keyValuePair.Value);
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndDocument();
    xmlWriter.Flush();
  }

  internal static void WriteDefaultContentType(
    XmlWriter xmlWriter,
    string extension,
    string contentType)
  {
    xmlWriter.WriteStartElement("Default");
    xmlWriter.WriteAttributeString("Extension", (string) null, extension);
    xmlWriter.WriteAttributeString("ContentType", (string) null, contentType);
    xmlWriter.WriteEndElement();
  }

  internal static void WriteOvverideContentType(
    XmlWriter xmlWriter,
    string extension,
    string contentType)
  {
    xmlWriter.WriteStartElement("Override");
    xmlWriter.WriteAttributeString("PartName", extension);
    xmlWriter.WriteAttributeString("ContentType", contentType);
    xmlWriter.WriteEndElement();
  }

  internal static void WritePresentation(XmlWriter xmlWriter, Syncfusion.Presentation.Presentation presentation)
  {
    xmlWriter.WriteStartElement("p", nameof (presentation), "http://schemas.openxmlformats.org/presentationml/2006/main");
    xmlWriter.WriteAttributeString("xmlns", "a", (string) null, "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("xmlns", "r", (string) null, "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
    if (presentation.FirstSlideNumber != 1)
      xmlWriter.WriteAttributeString("firstSlideNum", Helper.ToString(presentation.FirstSlideNumber));
    if (presentation.IsEmbedTrueTypeFont)
      xmlWriter.WriteAttributeString("embedTrueTypeFonts", "1");
    if (!presentation.ShowSpecialPlsOnTitleSld)
      xmlWriter.WriteAttributeString("showSpecialPlsOnTitleSld", "0");
    if (presentation.RightToLeftView)
      xmlWriter.WriteAttributeString("rtl", "1");
    if (presentation.MasterList != null && presentation.MasterList.Count > 0)
      Serializator.SerializeMasterList(xmlWriter, presentation.MasterList);
    if (presentation.NotesList != null && presentation.NotesList.Count > 0)
      Serializator.SerializeIdList(xmlWriter, presentation, "notesMasterIdLst", "notesMasterId");
    if (presentation.HandoutList != null && presentation.HandoutList.Count > 0)
      Serializator.SerializeIdList(xmlWriter, presentation, "handoutMasterIdLst", "handoutMasterId");
    if (presentation.SlideList != null && presentation.SlideList.Count > 0)
      Serializator.SerializeIdList(xmlWriter, presentation, "sldIdLst", "sldId");
    Serializator.SerializeSlideSize(xmlWriter, (SlideSize) presentation.SlideSize);
    Serializator.SerializeNotesSize(xmlWriter, presentation.NotesSize as NotesSize);
    if (presentation.IsEmbedTrueTypeFont && presentation.EmbeddedFontList.Count != 0)
    {
      xmlWriter.WriteStartElement("p", "embeddedFontLst", "http://schemas.openxmlformats.org/presentationml/2006/main");
      Serializator.SerializeEmbeddedFontList(xmlWriter, presentation.EmbeddedFontList);
      xmlWriter.WriteEndElement();
    }
    if (presentation.DefaultTextStyle != null)
    {
      xmlWriter.WriteStartElement("p", "defaultTextStyle", "http://schemas.openxmlformats.org/presentationml/2006/main");
      Serializator.SerializeStyles(xmlWriter, presentation.DefaultTextStyle.StyleList);
      xmlWriter.WriteEndElement();
    }
    else if (presentation.PreservedElements != null && presentation.PreservedElements.ContainsKey("defaultTextStyle"))
      DrawingSerializator.WriteRawData(xmlWriter, presentation.PreservedElements, "defaultTextStyle");
    Serializator.SerializeModifyVerifier(xmlWriter, presentation);
    Serializator.SerializeExtensionList(xmlWriter, presentation);
    xmlWriter.WriteEndDocument();
  }

  private static void SerializeModifyVerifier(XmlWriter xmlWriter, Syncfusion.Presentation.Presentation presentation)
  {
    if (!presentation.WriteProtection.IsWriteProtected)
      return;
    xmlWriter.WriteStartElement("p", "modifyVerifier", "http://schemas.openxmlformats.org/presentationml/2006/main");
    foreach (KeyValuePair<string, string> attribute in presentation.WriteProtection.Attributes)
      xmlWriter.WriteAttributeString(attribute.Key, attribute.Value);
    xmlWriter.WriteEndElement();
  }

  private static void SerializeExtensionList(XmlWriter xmlWriter, Syncfusion.Presentation.Presentation presentation)
  {
    xmlWriter.WriteStartElement("p", "extLst", "http://schemas.openxmlformats.org/presentationml/2006/main");
    xmlWriter.WriteStartElement("p", "ext", "http://schemas.openxmlformats.org/presentationml/2006/main");
    xmlWriter.WriteAttributeString("uri", "{521415D9-36F7-43E2-AB2F-B90AF26B5E84}");
    if (presentation.Sections.Count > 0)
      Serializator.SerializeSectionList(xmlWriter, (Sections) presentation.Sections);
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
  }

  private static void SerializeSectionList(XmlWriter xmlWriter, Sections sections)
  {
    xmlWriter.WriteStartElement("p14", "sectionLst", "http://schemas.microsoft.com/office/powerpoint/2010/main");
    xmlWriter.WriteAttributeString("xmlns", "p14", (string) null, "http://schemas.microsoft.com/office/powerpoint/2010/main");
    foreach (Section section in sections.GetSectionList())
      Serializator.SerializeSection(xmlWriter, section);
    xmlWriter.WriteEndElement();
  }

  private static void SerializeSection(XmlWriter xmlWriter, Section section)
  {
    xmlWriter.WriteStartElement("p14", nameof (section), "http://schemas.microsoft.com/office/powerpoint/2010/main");
    xmlWriter.WriteAttributeString("name", section.Name);
    xmlWriter.WriteAttributeString("id", Serializator.GetGuidString(Guid.NewGuid()));
    Serializator.SerializeSectionSlideIdList(xmlWriter, section.SlideIdList);
    xmlWriter.WriteEndElement();
  }

  private static void SerializeSectionSlideIdList(XmlWriter xmlWriter, List<string> slideIdList)
  {
    xmlWriter.WriteStartElement("p14", "sldIdLst", "http://schemas.microsoft.com/office/powerpoint/2010/main");
    foreach (string slideId in slideIdList)
    {
      xmlWriter.WriteStartElement("p14", "sldId", "http://schemas.microsoft.com/office/powerpoint/2010/main");
      xmlWriter.WriteAttributeString("id", slideId);
      xmlWriter.WriteEndElement();
    }
    xmlWriter.WriteEndElement();
  }

  private static void SerializeEmbeddedFontList(
    XmlWriter xmlWriter,
    List<EmbeddedFont> embeddedFontList)
  {
    foreach (EmbeddedFont embeddedFont in embeddedFontList)
    {
      xmlWriter.WriteStartElement("p", "embeddedFont", "http://schemas.openxmlformats.org/presentationml/2006/main");
      if (embeddedFont.Typeface != null)
      {
        xmlWriter.WriteStartElement("p", "font", "http://schemas.openxmlformats.org/presentationml/2006/main");
        xmlWriter.WriteAttributeString("typeface", embeddedFont.Typeface);
        xmlWriter.WriteEndElement();
      }
      if (embeddedFont.RegularRelationId != null)
      {
        xmlWriter.WriteStartElement("p", "regular", "http://schemas.openxmlformats.org/presentationml/2006/main");
        xmlWriter.WriteAttributeString("r", "id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", embeddedFont.RegularRelationId);
        xmlWriter.WriteEndElement();
      }
      if (embeddedFont.BoldRelationId != null)
      {
        xmlWriter.WriteStartElement("p", "bold", "http://schemas.openxmlformats.org/presentationml/2006/main");
        xmlWriter.WriteAttributeString("r", "id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", embeddedFont.BoldRelationId);
        xmlWriter.WriteEndElement();
      }
      if (embeddedFont.ItalicRelationId != null)
      {
        xmlWriter.WriteStartElement("p", "italic", "http://schemas.openxmlformats.org/presentationml/2006/main");
        xmlWriter.WriteAttributeString("r", "id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", embeddedFont.ItalicRelationId);
        xmlWriter.WriteEndElement();
      }
      if (embeddedFont.BoldItalicRelationId != null)
      {
        xmlWriter.WriteStartElement("p", "boldItalic", "http://schemas.openxmlformats.org/presentationml/2006/main");
        xmlWriter.WriteAttributeString("r", "id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", embeddedFont.BoldItalicRelationId);
        xmlWriter.WriteEndElement();
      }
      xmlWriter.WriteEndElement();
    }
  }

  private static void SerializeMasterList(
    XmlWriter xmlWriter,
    Dictionary<string, string> masterList)
  {
    xmlWriter.WriteStartElement("p", "sldMasterIdLst", "http://schemas.openxmlformats.org/presentationml/2006/main");
    foreach (KeyValuePair<string, string> master in masterList)
    {
      xmlWriter.WriteStartElement("p", "sldMasterId", "http://schemas.openxmlformats.org/presentationml/2006/main");
      xmlWriter.WriteAttributeString("id", master.Value);
      xmlWriter.WriteAttributeString("r", "id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", master.Key);
      xmlWriter.WriteEndElement();
    }
    xmlWriter.WriteEndElement();
  }

  internal static void WriteRelation(XmlWriter xmlWriter, Relation relation)
  {
    xmlWriter.WriteStartElement("Relationship");
    xmlWriter.WriteAttributeString("Id", relation.Id);
    xmlWriter.WriteAttributeString("Type", relation.Type);
    xmlWriter.WriteAttributeString("Target", relation.Target);
    if (!string.IsNullOrEmpty(relation.TargetMode))
      xmlWriter.WriteAttributeString("TargetMode", relation.TargetMode);
    xmlWriter.WriteEndElement();
  }

  internal static void WriteRelationShip(XmlWriter xmlWriter, List<Relation> relationList)
  {
    xmlWriter.WriteStartDocument(true);
    xmlWriter.WriteStartElement("Relationships", "http://schemas.openxmlformats.org/package/2006/relationships");
    foreach (Relation relation in relationList)
      Serializator.WriteRelation(xmlWriter, relation);
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndDocument();
    xmlWriter.Flush();
  }

  internal static void SerializeSolidFill(
    XmlWriter xmlWriter,
    ColorObject colorObject,
    int alphaValue,
    Syncfusion.Presentation.Presentation presentation,
    bool isPreset,
    BaseSlide baseSlide)
  {
    bool flag = false;
    if (isPreset)
    {
      xmlWriter.WriteStartElement("a", "prstClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteAttributeString("val", colorObject.ColorName);
      flag = true;
    }
    if (!flag)
    {
      if (colorObject.ColorType == ColorType.Theme)
      {
        xmlWriter.WriteStartElement("a", "schemeClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
        MasterSlide masterSlide1 = (MasterSlide) null;
        LayoutSlide layoutSlide = (LayoutSlide) null;
        if (baseSlide == null)
        {
          if (presentation != null && presentation.Masters != null && presentation.Masters.Count != 0)
            masterSlide1 = presentation.Masters[0] as MasterSlide;
        }
        else
        {
          IMasterSlide masterSlide2 = (IMasterSlide) null;
          if (baseSlide is Slide)
          {
            Slide slide = (Slide) baseSlide;
            if (!((BaseSlide) slide.LayoutSlide.MasterSlide).IsDisposed)
              masterSlide2 = slide.LayoutSlide.MasterSlide;
          }
          else if (baseSlide is MasterSlide && !baseSlide.IsDisposed)
            masterSlide1 = (MasterSlide) baseSlide;
          else if (baseSlide is LayoutSlide)
            layoutSlide = (LayoutSlide) baseSlide;
          else if (baseSlide is HandoutMaster)
            masterSlide2 = presentation != null || baseSlide.Presentation == null ? presentation.Masters[0] : baseSlide.Presentation.Masters[0];
          if (masterSlide2 != null)
            masterSlide1 = (MasterSlide) masterSlide2;
        }
        if (masterSlide1 != null)
          xmlWriter.WriteAttributeString("val", Helper.GetThemeStringFromIndex(colorObject.GetColorString(), masterSlide1));
        else
          xmlWriter.WriteAttributeString("val", Helper.GetThemeStringFromIndex(colorObject.GetColorString(), layoutSlide));
      }
      else
      {
        xmlWriter.WriteStartElement("a", "srgbClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
        string colorName;
        if (colorObject.ColorType == ColorType.RGB)
        {
          colorName = Helper.GetColorName(ColorExtension.FromArgb(colorObject.GetColorInt()));
        }
        else
        {
          colorObject.UpdateColorObject((object) presentation);
          colorName = Helper.GetColorName((IColor) colorObject);
        }
        xmlWriter.WriteAttributeString("val", colorName);
      }
    }
    if (alphaValue != -1)
      Serializator.SetAlphaValue(colorObject, alphaValue);
    foreach (ColorTransForm colorTransForm in (List<ColorTransForm>) colorObject.ColorTransFormCollection)
    {
      string str = Serializator.GetString(colorTransForm.ColorMode);
      if (str != null)
      {
        if (colorTransForm.ColorMode != ColorMode.Gamma && colorTransForm.ColorMode != ColorMode.InvGamma)
        {
          Serializator.WriteColorMode(xmlWriter, "a", str, Helper.ToString(colorTransForm.HexValue));
        }
        else
        {
          xmlWriter.WriteStartElement("a", str, "http://schemas.openxmlformats.org/drawingml/2006/main");
          xmlWriter.WriteEndElement();
        }
      }
    }
    xmlWriter.WriteEndElement();
  }

  private static string GetString(ColorMode colorMode)
  {
    switch (colorMode)
    {
      case ColorMode.Tint:
        return "tint";
      case ColorMode.Shade:
        return "shade";
      case ColorMode.Alpha:
        return "alpha";
      case ColorMode.AlphaMod:
        return "alphaMod";
      case ColorMode.AlphaOff:
        return "alphaOff";
      case ColorMode.Red:
        return "red";
      case ColorMode.RedMod:
        return "redMod";
      case ColorMode.RedOff:
        return "redOff";
      case ColorMode.Green:
        return "green";
      case ColorMode.GreenMod:
        return "greenMod";
      case ColorMode.GreenOff:
        return "greenOff";
      case ColorMode.Blue:
        return "blue";
      case ColorMode.BlueMod:
        return "blueMod";
      case ColorMode.BlueOff:
        return "blueOff";
      case ColorMode.Hue:
        return "hue";
      case ColorMode.HueMod:
        return "hueMod";
      case ColorMode.HueOff:
        return "hueOff";
      case ColorMode.Sat:
        return "sat";
      case ColorMode.SatMod:
        return "satMod";
      case ColorMode.SatOff:
        return "satOff";
      case ColorMode.Lum:
        return "lum";
      case ColorMode.LumMod:
        return "lumMod";
      case ColorMode.LumOff:
        return "lumOff";
      case ColorMode.Gamma:
        return "gamma";
      case ColorMode.InvGamma:
        return "invGamma";
      case ColorMode.Comp:
        return "comp";
      case ColorMode.Gray:
        return "gray";
      case ColorMode.Inv:
        return "inv";
      default:
        return (string) null;
    }
  }

  private static void SetAlphaValue(ColorObject colorObject, int alpha)
  {
    if (alpha == -1)
      return;
    ColorTransForm colorTransForm = colorObject.ColorTransFormCollection.GetColorTransForm(ColorMode.Alpha);
    if (colorTransForm != null)
      colorTransForm.HexValue = alpha;
    else
      colorObject.ColorTransFormCollection.AddColorTransForm(ColorMode.Alpha, alpha);
  }

  private static void WriteColorMode(
    XmlWriter xmlWriter,
    string preFix,
    string elementName,
    string hexValue)
  {
    xmlWriter.WriteStartElement(preFix, elementName, "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("val", hexValue);
    xmlWriter.WriteEndElement();
  }

  internal static void SerializeGradientFill(XmlWriter xmlWriter, GradientFill gradientFill)
  {
    xmlWriter.WriteStartElement("a", "gradFill", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (gradientFill.RotWithShape.HasValue)
      xmlWriter.WriteAttributeString("rotWithShape", Convert.ToBoolean((object) gradientFill.RotWithShape) ? "1" : "0");
    if (gradientFill.GradientStops.Count > 0)
    {
      if (gradientFill.GradientStops.Count < 2)
        throw new Exception("The minimum number of gradient stops that can be set is 2.");
      xmlWriter.WriteStartElement("a", "gsLst", "http://schemas.openxmlformats.org/drawingml/2006/main");
      foreach (GradientStop gradientStop in (IEnumerable<IGradientStop>) gradientFill.GradientStops)
      {
        xmlWriter.WriteStartElement("a", "gs", "http://schemas.openxmlformats.org/drawingml/2006/main");
        xmlWriter.WriteAttributeString("pos", Helper.ToString(Convert.ToInt32(gradientStop.Position * 1000f)));
        int alphaValue = gradientStop.GetMaxValue() == 100000 ? -1 : gradientStop.GetMaxValue();
        Serializator.SerializeSolidFill(xmlWriter, gradientStop.GetColorObject(), alphaValue, (Syncfusion.Presentation.Presentation) null, false, gradientFill.FillFormat.BaseSlide);
        xmlWriter.WriteEndElement();
      }
      xmlWriter.WriteEndElement();
    }
    if (gradientFill.ShadeProperties != null)
    {
      if (gradientFill.Type == GradientFillType.Linear)
      {
        xmlWriter.WriteStartElement("a", "lin", "http://schemas.openxmlformats.org/drawingml/2006/main");
        xmlWriter.WriteAttributeString("ang", Helper.ToString(gradientFill.GetLineShade().Angle));
        if (gradientFill.GetLineShade().IsScaled)
          xmlWriter.WriteAttributeString("scaled", "1");
        xmlWriter.WriteEndElement();
      }
      else
      {
        PathShadeImpl pathShade = gradientFill.GetPathShade();
        xmlWriter.WriteStartElement("a", "path", "http://schemas.openxmlformats.org/drawingml/2006/main");
        xmlWriter.WriteAttributeString("path", Helper.ToString(pathShade.PathShapeType));
        Serializator.SerializeFillRectAttributes(xmlWriter, pathShade, "fillToRect");
        xmlWriter.WriteEndElement();
      }
    }
    PathShadeImpl tileRectangle = gradientFill.ObtainTileRectangle();
    if (tileRectangle != null)
      Serializator.SerializeFillRectAttributes(xmlWriter, tileRectangle, "tileRect");
    xmlWriter.WriteEndElement();
  }

  private static void SerializeFillRectAttributes(
    XmlWriter xmlWriter,
    PathShadeImpl pathShade,
    string stringValue)
  {
    xmlWriter.WriteStartElement("a", stringValue, "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (pathShade.Left != 0)
      xmlWriter.WriteAttributeString("l", Helper.ToString(pathShade.Left));
    if (pathShade.Top != 0)
      xmlWriter.WriteAttributeString("t", Helper.ToString(pathShade.Top));
    if (pathShade.Right != 0)
      xmlWriter.WriteAttributeString("r", Helper.ToString(pathShade.Right));
    if (pathShade.Bottom != 0)
      xmlWriter.WriteAttributeString("b", Helper.ToString(pathShade.Bottom));
    xmlWriter.WriteEndElement();
  }

  internal static void SerialisePatternFill(XmlWriter xmlWriter, PatternFill patternFill)
  {
    string str = Helper.ToString(patternFill.Pattern);
    xmlWriter.WriteStartElement("a", "pattFill", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("prst", str);
    xmlWriter.WriteStartElement("a", "fgClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    Serializator.SerializeSolidFill(xmlWriter, patternFill.GetForeColorObject(), -1, (Syncfusion.Presentation.Presentation) null, false, patternFill.Fill.BaseSlide);
    xmlWriter.WriteEndElement();
    xmlWriter.WriteStartElement("a", "bgClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    Serializator.SerializeSolidFill(xmlWriter, patternFill.GetBackColorObject(), -1, (Syncfusion.Presentation.Presentation) null, false, patternFill.Fill.BaseSlide);
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
  }

  internal static void SerializeTextureFill(XmlWriter xmlWriter, TextureFill textureFill)
  {
    xmlWriter.WriteStartElement("a", "blipFill", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteStartElement("a", "blip", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (textureFill.Data != null)
      textureFill.AddImageToArchive();
    xmlWriter.WriteAttributeString("r", "embed", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", textureFill.ImageRelationId);
    if (textureFill.Transparency != 100000)
    {
      xmlWriter.WriteStartElement("a", "alphaModFix", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteAttributeString("amt", Helper.ToString(textureFill.ObtainTransparency()));
      xmlWriter.WriteEndElement();
    }
    if (textureFill.DuoTone != null)
      Serializator.SerializeDuoTone(xmlWriter, textureFill);
    xmlWriter.WriteEndElement();
    xmlWriter.WriteStartElement("a", "srcRect", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteEndElement();
    if (textureFill.CheckFormatOptionIsTile())
      Serializator.SerializeTilePicOption(xmlWriter, textureFill);
    else
      Serializator.SerializePicFormatOption(xmlWriter, textureFill);
    xmlWriter.WriteEndElement();
  }

  private static void SerializeDuoTone(XmlWriter xmlWriter, TextureFill textureFill)
  {
    if (textureFill == null || textureFill.DuoTone.Count == 0)
      return;
    xmlWriter.WriteStartElement("a", "duotone", "http://schemas.openxmlformats.org/drawingml/2006/main");
    Serializator.SerializeSolidFill(xmlWriter, textureFill.DuoTone[0], -1, textureFill.FillFormat.Presentation, false, textureFill.FillFormat.BaseSlide);
    Serializator.SerializeSolidFill(xmlWriter, textureFill.DuoTone[1], -1, textureFill.FillFormat.Presentation, false, textureFill.FillFormat.BaseSlide);
    xmlWriter.WriteEndElement();
  }

  private static void SerializePicFormatOption(XmlWriter xmlWriter, TextureFill textureFill)
  {
    PicFormatOption picFormatOption = textureFill.PicFormatOption;
    xmlWriter.WriteStartElement("a", "stretch", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteStartElement("a", "fillRect", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if ((double) picFormatOption.ObtainLeft() != 0.0)
      xmlWriter.WriteAttributeString("l", Helper.ToString(picFormatOption.ObtainLeft()));
    if ((double) picFormatOption.ObtainTop() != 0.0)
      xmlWriter.WriteAttributeString("t", Helper.ToString(picFormatOption.ObtainTop()));
    if ((double) picFormatOption.ObtainRight() != 0.0)
      xmlWriter.WriteAttributeString("r", Helper.ToString(picFormatOption.ObtainRight()));
    if ((double) picFormatOption.ObtainBottom() != 0.0)
      xmlWriter.WriteAttributeString("b", Helper.ToString(picFormatOption.ObtainBottom()));
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
  }

  private static void SerializeTilePicOption(XmlWriter xmlWriter, TextureFill textureFill)
  {
    TilePicOption tilePicOption = textureFill.TilePicOption;
    xmlWriter.WriteStartElement("a", "tile", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("tx", Helper.ToString((int) (tilePicOption.OffsetX * 12700.0)));
    xmlWriter.WriteAttributeString("ty", Helper.ToString((int) (tilePicOption.OffsetY * 12700.0)));
    xmlWriter.WriteAttributeString("sx", Helper.ToString((int) tilePicOption.ScaleX * 1000));
    xmlWriter.WriteAttributeString("sy", Helper.ToString((int) tilePicOption.ScaleY * 1000));
    xmlWriter.WriteAttributeString("flip", Helper.ToString(tilePicOption.MirrorType));
    xmlWriter.WriteAttributeString("algn", Helper.ToString(tilePicOption.AlignmentType));
    xmlWriter.WriteEndElement();
  }

  private static void SerializeStyleList(XmlWriter xmlWriter, KeyValuePair<string, Paragraph> style)
  {
    switch (style.Key)
    {
      case "defPPr":
        xmlWriter.WriteStartElement("a", style.Key, "http://schemas.openxmlformats.org/drawingml/2006/main");
        Serializator.SerializeParagraphProperties(xmlWriter, style.Value);
        xmlWriter.WriteEndElement();
        break;
      case "lvl1pPr":
        xmlWriter.WriteStartElement("a", style.Key, "http://schemas.openxmlformats.org/drawingml/2006/main");
        Serializator.SerializeParagraphProperties(xmlWriter, style.Value);
        xmlWriter.WriteEndElement();
        break;
      case "lvl2pPr":
        xmlWriter.WriteStartElement("a", style.Key, "http://schemas.openxmlformats.org/drawingml/2006/main");
        Serializator.SerializeParagraphProperties(xmlWriter, style.Value);
        xmlWriter.WriteEndElement();
        break;
      case "lvl3pPr":
        xmlWriter.WriteStartElement("a", style.Key, "http://schemas.openxmlformats.org/drawingml/2006/main");
        Serializator.SerializeParagraphProperties(xmlWriter, style.Value);
        xmlWriter.WriteEndElement();
        break;
      case "lvl4pPr":
        xmlWriter.WriteStartElement("a", style.Key, "http://schemas.openxmlformats.org/drawingml/2006/main");
        Serializator.SerializeParagraphProperties(xmlWriter, style.Value);
        xmlWriter.WriteEndElement();
        break;
      case "lvl5pPr":
        xmlWriter.WriteStartElement("a", style.Key, "http://schemas.openxmlformats.org/drawingml/2006/main");
        Serializator.SerializeParagraphProperties(xmlWriter, style.Value);
        xmlWriter.WriteEndElement();
        break;
      case "lvl6pPr":
        xmlWriter.WriteStartElement("a", style.Key, "http://schemas.openxmlformats.org/drawingml/2006/main");
        Serializator.SerializeParagraphProperties(xmlWriter, style.Value);
        xmlWriter.WriteEndElement();
        break;
      case "lvl7pPr":
        xmlWriter.WriteStartElement("a", style.Key, "http://schemas.openxmlformats.org/drawingml/2006/main");
        Serializator.SerializeParagraphProperties(xmlWriter, style.Value);
        xmlWriter.WriteEndElement();
        break;
      case "lvl8pPr":
        xmlWriter.WriteStartElement("a", style.Key, "http://schemas.openxmlformats.org/drawingml/2006/main");
        Serializator.SerializeParagraphProperties(xmlWriter, style.Value);
        xmlWriter.WriteEndElement();
        break;
      case "lvl9pPr":
        xmlWriter.WriteStartElement("a", style.Key, "http://schemas.openxmlformats.org/drawingml/2006/main");
        Serializator.SerializeParagraphProperties(xmlWriter, style.Value);
        xmlWriter.WriteEndElement();
        break;
    }
  }

  private static void SerializeTextBodyElements(XmlWriter xmlWriter, TextBody textFrame)
  {
    Serializator.SerializeBodyProperties(xmlWriter, textFrame);
    xmlWriter.WriteStartElement("a", "lstStyle", "http://schemas.openxmlformats.org/drawingml/2006/main");
    Serializator.SerializeStyles(xmlWriter, textFrame.StyleList);
    xmlWriter.WriteEndElement();
    Serializator.SerializeParagraph(xmlWriter, textFrame);
  }

  private static void SerializeParagraph(XmlWriter xmlWriter, TextBody textFrame)
  {
    if (textFrame.Paragraphs.Count != 0)
    {
      foreach (Paragraph paragraph in (IEnumerable<IParagraph>) textFrame.Paragraphs)
      {
        xmlWriter.WriteStartElement("a", "p", "http://schemas.openxmlformats.org/drawingml/2006/main");
        xmlWriter.WriteStartElement("a", "pPr", "http://schemas.openxmlformats.org/drawingml/2006/main");
        Serializator.SerializeParagraphProperties(xmlWriter, paragraph);
        xmlWriter.WriteEndElement();
        Serializator.SerializeRegularTextRunCollection(xmlWriter, paragraph);
        xmlWriter.WriteEndElement();
      }
    }
    else
    {
      xmlWriter.WriteStartElement("a", "p", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteEndElement();
    }
  }

  private static void SerializeSlideNumber(XmlWriter xmlWriter, TextPart textPart)
  {
    xmlWriter.WriteAttributeString("id", textPart.UniqueId);
    xmlWriter.WriteAttributeString("type", textPart.Type);
  }

  private static void SerializeParagraphProperties(XmlWriter xmlWriter, Paragraph paragraph)
  {
    if (paragraph.IndentLevelNumber != 0)
      xmlWriter.WriteAttributeString("lvl", Helper.ToString(paragraph.GetIndentLevel()));
    if (paragraph.IsMarginChanged)
      xmlWriter.WriteAttributeString("marL", Helper.ToString(paragraph.GetMarginLeft()));
    if (paragraph.GetMarginRight() != 0)
      xmlWriter.WriteAttributeString("marR", Helper.ToString(paragraph.GetMarginRight()));
    if (paragraph.IsIndentChanged)
      xmlWriter.WriteAttributeString("indent", Helper.ToString(paragraph.GetIndent()));
    if (paragraph.RightToLeft.HasValue)
      xmlWriter.WriteAttributeString("rtl", paragraph.RightToLeft.Value ? "1" : "0");
    if (paragraph.GetAlignmentType() != HorizontalAlignment.None)
      xmlWriter.WriteAttributeString("algn", Helper.ToString(paragraph.GetAlignmentType()));
    if (paragraph.GetFontAlignType() != FontAlignmentType.None)
      xmlWriter.WriteAttributeString("fontAlgn", Helper.ToString(paragraph.GetFontAlignType()));
    if (paragraph.GetDefaultTabsize() != -1L)
      xmlWriter.WriteAttributeString("defTabSz", Helper.ToString(paragraph.GetDefaultTabsize()));
    Serializator.SerializeTextSpacing(xmlWriter, paragraph);
    Serializator.SerializeBullet(xmlWriter, paragraph);
    if (paragraph.TabPositionList != null)
      Serializator.SerializeTabList(xmlWriter, paragraph);
    Font fontObject = paragraph.GetFontObject();
    if (fontObject == null)
      return;
    xmlWriter.WriteStartElement("a", "defRPr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    Serializator.SerializeTextCharacterProperties(xmlWriter, paragraph.GetFontObject(), paragraph);
    if (fontObject.HasFontName && fontObject.GetName() != string.Empty)
    {
      xmlWriter.WriteStartElement("a", "latin", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteAttributeString("typeface", paragraph.GetFontObject().GetName());
      xmlWriter.WriteEndElement();
    }
    if (!string.IsNullOrEmpty(fontObject.GetEastAsianFontName()))
    {
      xmlWriter.WriteStartElement("a", "ea", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteAttributeString("typeface", fontObject.GetEastAsianFontName());
      xmlWriter.WriteEndElement();
    }
    if (!string.IsNullOrEmpty(fontObject.GetComplexScriptFontName()))
    {
      xmlWriter.WriteStartElement("a", "cs", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteAttributeString("typeface", fontObject.GetComplexScriptFontName());
      xmlWriter.WriteEndElement();
    }
    if (!string.IsNullOrEmpty(fontObject.GetSymbolFontName()))
    {
      xmlWriter.WriteStartElement("a", "sym", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteAttributeString("typeface", fontObject.GetSymbolFontName());
      xmlWriter.WriteEndElement();
    }
    xmlWriter.WriteEndElement();
  }

  private static void SerializeTabList(XmlWriter xmlWriter, Paragraph paragraph)
  {
    xmlWriter.WriteStartElement("a", "tabLst", "http://schemas.openxmlformats.org/drawingml/2006/main");
    for (int index = 0; index < paragraph.TabPositionList.Count; ++index)
    {
      xmlWriter.WriteStartElement("a", "tab", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteAttributeString("pos", Helper.ToString(paragraph.TabPositionList[index]));
      xmlWriter.WriteAttributeString("algn", Helper.ToString(paragraph.TabAlignmentTypeList[index]));
      xmlWriter.WriteEndElement();
    }
    xmlWriter.WriteEndElement();
  }

  private static void SerializeTextSpacing(XmlWriter xmlWriter, Paragraph paragraph)
  {
    if (paragraph.LineSpacingType != SizeType.None)
    {
      xmlWriter.WriteStartElement("a", "lnSpc", "http://schemas.openxmlformats.org/drawingml/2006/main");
      Serializator.SerializeTextSpacingElements(xmlWriter, paragraph.ObtainLineSpacing(), paragraph.LineSpacingType);
      xmlWriter.WriteEndElement();
    }
    if (paragraph.SpaceBeforeType != SizeType.None)
    {
      xmlWriter.WriteStartElement("a", "spcBef", "http://schemas.openxmlformats.org/drawingml/2006/main");
      Serializator.SerializeTextSpacingElements(xmlWriter, paragraph.ObtainSpaceBefore(), paragraph.SpaceBeforeType);
      xmlWriter.WriteEndElement();
    }
    if (paragraph.SpaceAfterType == SizeType.None)
      return;
    xmlWriter.WriteStartElement("a", "spcAft", "http://schemas.openxmlformats.org/drawingml/2006/main");
    Serializator.SerializeTextSpacingElements(xmlWriter, paragraph.ObtainSpaceAfter(), paragraph.SpaceAfterType);
    xmlWriter.WriteEndElement();
  }

  private static void SerializeTextSpacingElements(
    XmlWriter xmlWriter,
    int lineSpacing,
    SizeType sizeType)
  {
    switch (sizeType)
    {
      case SizeType.Points:
        xmlWriter.WriteStartElement("a", "spcPts", "http://schemas.openxmlformats.org/drawingml/2006/main");
        xmlWriter.WriteAttributeString("val", Helper.ToString(lineSpacing));
        xmlWriter.WriteEndElement();
        break;
      case SizeType.Percentage:
        xmlWriter.WriteStartElement("a", "spcPct", "http://schemas.openxmlformats.org/drawingml/2006/main");
        xmlWriter.WriteAttributeString("val", Helper.ToString(lineSpacing));
        xmlWriter.WriteEndElement();
        break;
    }
  }

  private static void SerializeEndParaProperties(XmlWriter xmlWriter, Paragraph paragraph)
  {
    Font endParaProps = paragraph.GetEndParaProps();
    if (endParaProps == null)
      return;
    Serializator.SerializeTextCharacterProperties(xmlWriter, endParaProps, paragraph);
    if (endParaProps.HasFontName && !string.IsNullOrEmpty(endParaProps.GetName()))
    {
      xmlWriter.WriteStartElement("a", "latin", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteAttributeString("typeface", endParaProps.GetName());
      xmlWriter.WriteEndElement();
    }
    if (!string.IsNullOrEmpty(endParaProps.GetEastAsianFontName()))
    {
      xmlWriter.WriteStartElement("a", "ea", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteAttributeString("typeface", endParaProps.GetEastAsianFontName());
      xmlWriter.WriteEndElement();
    }
    if (!string.IsNullOrEmpty(endParaProps.GetComplexScriptFontName()))
    {
      xmlWriter.WriteStartElement("a", "cs", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteAttributeString("typeface", endParaProps.GetComplexScriptFontName());
      xmlWriter.WriteEndElement();
    }
    if (string.IsNullOrEmpty(endParaProps.GetSymbolFontName()))
      return;
    xmlWriter.WriteStartElement("a", "sym", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("typeface", endParaProps.GetSymbolFontName());
    xmlWriter.WriteEndElement();
  }

  private static void SerializeEndParaProperties(XmlWriter xmlWriter, TextPart textpart)
  {
    Font fontObject = textpart.GetFontObject();
    if (fontObject == null)
      return;
    Serializator.SerializeTextCharacterProperties(xmlWriter, fontObject, textpart.Paragraph);
    if (fontObject.HasFontName && !string.IsNullOrEmpty(fontObject.GetName()))
    {
      xmlWriter.WriteStartElement("a", "latin", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteAttributeString("typeface", fontObject.GetName());
      xmlWriter.WriteEndElement();
    }
    if (!string.IsNullOrEmpty(fontObject.GetEastAsianFontName()))
    {
      xmlWriter.WriteStartElement("a", "ea", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteAttributeString("typeface", fontObject.GetEastAsianFontName());
      xmlWriter.WriteEndElement();
    }
    if (!string.IsNullOrEmpty(fontObject.GetComplexScriptFontName()))
    {
      xmlWriter.WriteStartElement("a", "cs", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteAttributeString("typeface", fontObject.GetComplexScriptFontName());
      xmlWriter.WriteEndElement();
    }
    if (string.IsNullOrEmpty(fontObject.GetSymbolFontName()))
      return;
    xmlWriter.WriteStartElement("a", "sym", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("typeface", fontObject.GetSymbolFontName());
    xmlWriter.WriteEndElement();
  }

  private static void SerializeRegularTextRunCollection(XmlWriter xmlWriter, Paragraph paragraph)
  {
    foreach (TextPart textPart in (IEnumerable<ITextPart>) paragraph.TextParts)
    {
      Font fontObject = textPart.GetFontObject();
      if (!string.IsNullOrEmpty(textPart.Text) || fontObject != null)
      {
        if (textPart.GetLineBreakProps() == null && textPart.UniqueId == null)
        {
          if (paragraph.Text == string.Empty && fontObject != null && fontObject.LastParaTextPart)
          {
            xmlWriter.WriteStartElement("a", "endParaRPr", "http://schemas.openxmlformats.org/drawingml/2006/main");
            Serializator.SerializeEndParaProperties(xmlWriter, textPart);
            xmlWriter.WriteEndElement();
          }
          else
          {
            xmlWriter.WriteStartElement("a", "r", "http://schemas.openxmlformats.org/drawingml/2006/main");
            Serializator.SerializeRegularTextRun(xmlWriter, textPart, paragraph);
            xmlWriter.WriteEndElement();
          }
        }
        else if (textPart.UniqueId != null)
        {
          xmlWriter.WriteStartElement("a", "fld", "http://schemas.openxmlformats.org/drawingml/2006/main");
          Serializator.SerializeSlideNumber(xmlWriter, textPart);
          Serializator.SerializeRegularTextRun(xmlWriter, textPart, paragraph);
          xmlWriter.WriteEndElement();
        }
        else
        {
          xmlWriter.WriteStartElement("a", "br", "http://schemas.openxmlformats.org/drawingml/2006/main");
          Serializator.SerializeRegularTextRun(xmlWriter, textPart, paragraph);
          xmlWriter.WriteEndElement();
        }
      }
    }
    if (!paragraph.GetIsLastPara())
      return;
    xmlWriter.WriteStartElement("a", "endParaRPr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    Serializator.SerializeEndParaProperties(xmlWriter, paragraph);
    xmlWriter.WriteEndElement();
  }

  private static void SerializeRegularTextRun(
    XmlWriter xmlWriter,
    TextPart textPart,
    Paragraph paragraph)
  {
    Font fontObject = textPart.GetFontObject();
    xmlWriter.WriteStartElement("a", "rPr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (fontObject != null)
    {
      Serializator.SerializeTextCharacterProperties(xmlWriter, fontObject, paragraph);
      if (fontObject.HasFontName && !string.IsNullOrEmpty(fontObject.GetName()))
      {
        xmlWriter.WriteStartElement("a", "latin", "http://schemas.openxmlformats.org/drawingml/2006/main");
        xmlWriter.WriteAttributeString("typeface", fontObject.GetName());
        xmlWriter.WriteEndElement();
      }
      if (!string.IsNullOrEmpty(fontObject.GetEastAsianFontName()))
      {
        xmlWriter.WriteStartElement("a", "ea", "http://schemas.openxmlformats.org/drawingml/2006/main");
        xmlWriter.WriteAttributeString("typeface", fontObject.GetEastAsianFontName());
        xmlWriter.WriteEndElement();
      }
      if (!string.IsNullOrEmpty(fontObject.GetComplexScriptFontName()))
      {
        xmlWriter.WriteStartElement("a", "cs", "http://schemas.openxmlformats.org/drawingml/2006/main");
        xmlWriter.WriteAttributeString("typeface", fontObject.GetComplexScriptFontName());
        xmlWriter.WriteEndElement();
      }
      if (!string.IsNullOrEmpty(fontObject.GetSymbolFontName()))
      {
        xmlWriter.WriteStartElement("a", "sym", "http://schemas.openxmlformats.org/drawingml/2006/main");
        xmlWriter.WriteAttributeString("typeface", fontObject.GetSymbolFontName());
        xmlWriter.WriteEndElement();
      }
    }
    if (textPart.Hyperlink != null)
      Serializator.SerializeHyperlink(xmlWriter, (Hyperlink) textPart.Hyperlink);
    xmlWriter.WriteEndElement();
    if (textPart.GetLineBreakProps() != null)
      return;
    xmlWriter.WriteStartElement("a", "t", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteValue(textPart.Text);
    xmlWriter.WriteEndElement();
  }

  internal static void SerializeHyperlink(XmlWriter xmlWriter, Hyperlink hyperlink)
  {
    hyperlink.EnsureTargetSlide();
    xmlWriter.WriteStartElement("a", "hlinkClick", "http://schemas.openxmlformats.org/drawingml/2006/main");
    string relationId = hyperlink.RelationId;
    xmlWriter.WriteAttributeString("r", "id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", relationId);
    if (hyperlink.ActionString != null)
      xmlWriter.WriteAttributeString("action", hyperlink.ActionString);
    if (hyperlink.ScreenTip != null)
      xmlWriter.WriteAttributeString("tooltip", hyperlink.ScreenTip);
    if (hyperlink.HyperLinkColorType == HyperLinkColor.Tx)
    {
      xmlWriter.WriteStartElement("a", "extLst", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteStartElement("a", "ext", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteAttributeString("uri", "{A12FA001-AC4F-418D-AE19-62706E023703}");
      xmlWriter.WriteStartElement("ahyp", "hlinkClr", "http://schemas.microsoft.com/office/drawing/2018/hyperlinkcolor");
      xmlWriter.WriteAttributeString("xmlns", "ahyp", (string) null, "http://schemas.microsoft.com/office/drawing/2018/hyperlinkcolor");
      xmlWriter.WriteAttributeString("val", "tx");
      xmlWriter.WriteEndElement();
      xmlWriter.WriteEndElement();
      xmlWriter.WriteEndElement();
    }
    xmlWriter.WriteEndElement();
  }

  private static void SerializeTextCharacterProperties(
    XmlWriter xmlWriter,
    Font font,
    Paragraph paragraph)
  {
    if (!string.IsNullOrEmpty(font.AltLanguage))
      xmlWriter.WriteAttributeString("altLang", font.AltLanguage);
    if (!string.IsNullOrEmpty(font.Language))
      xmlWriter.WriteAttributeString("lang", font.Language);
    if (font.IsSizeChanged)
      xmlWriter.WriteAttributeString("sz", Helper.ToString(font.GetSize()));
    if ((font.GetDefaultOptions() & 1) != 0)
      xmlWriter.WriteAttributeString("b", font.Bold ? "1" : "0");
    if ((font.GetDefaultOptions() & 2) != 0)
      xmlWriter.WriteAttributeString("i", font.Italic ? "1" : "0");
    if ((font.GetDefaultOptions() & 8) != 0)
      xmlWriter.WriteAttributeString("u", Helper.ToString(font.GetUnderlineType()));
    if (font.StrikeType != TextStrikethroughType.None)
      xmlWriter.WriteAttributeString("strike", Helper.ToString(font.StrikeType));
    if (font.GetCapsTypeFromOptions(font.GetOptions()) != TextCapsType.None)
      xmlWriter.WriteAttributeString("cap", Helper.ToString(font.CapsType));
    if (font.IsCharacterSpacingApplied())
      xmlWriter.WriteAttributeString("spc", Helper.ToString(font.ObtainCharacterSpacing()));
    if (font.Kerning.HasValue)
    {
      int? kerning = font.Kerning;
      if ((kerning.GetValueOrDefault() < 0 ? 0 : (kerning.HasValue ? 1 : 0)) != 0)
        xmlWriter.WriteAttributeString("kern", Helper.ToString(font.Kerning.Value));
    }
    if (font.Subscript)
      xmlWriter.WriteAttributeString("baseline", Convert.ToString(font.GetBaseline()));
    if (font.Superscript)
      xmlWriter.WriteAttributeString("baseline", Convert.ToString(font.GetBaseline()));
    xmlWriter.WriteAttributeString("dirty", "0");
    if (font.IsSpellingError)
      xmlWriter.WriteAttributeString("err", "1");
    if (font.PreservedElements != null && font.PreservedElements.ContainsKey("ln"))
      DrawingSerializator.WriteRawData(xmlWriter, font.PreservedElements, "ln");
    if (font.RightToLeft.HasValue && font.RightToLeft.Value)
    {
      xmlWriter.WriteStartElement("a", "rtl", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteEndElement();
    }
    if (font.Fill.FillType == FillType.Gradient)
    {
      GradientFill gradientFill = (GradientFill) font.Fill.GradientFill;
      Serializator.SerializeGradientFill(xmlWriter, gradientFill);
    }
    ColorObject colorObject = font.GetColorObject();
    if (colorObject.ColorType != ColorType.Automatic)
    {
      xmlWriter.WriteStartElement("a", "solidFill", "http://schemas.openxmlformats.org/drawingml/2006/main");
      BaseSlide baseSlide = (BaseSlide) null;
      Syncfusion.Presentation.Presentation presentation;
      if (paragraph.BaseSlide != null)
      {
        presentation = paragraph.BaseSlide.Presentation;
        baseSlide = paragraph.BaseSlide;
      }
      else
        presentation = paragraph.Presentation;
      Serializator.SerializeSolidFill(xmlWriter, colorObject, -1, presentation, false, baseSlide);
      xmlWriter.WriteEndElement();
    }
    else if (font.Fill.FillType == FillType.None)
    {
      xmlWriter.WriteStartElement("a", "noFill", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteEndElement();
    }
    ColorObject underlineColorObject = font.GetUnderlineColorObject();
    if (underlineColorObject != null && underlineColorObject.ColorType != ColorType.Automatic)
    {
      xmlWriter.WriteStartElement("a", "uFill", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteStartElement("a", "solidFill", "http://schemas.openxmlformats.org/drawingml/2006/main");
      BaseSlide baseSlide = (BaseSlide) null;
      Syncfusion.Presentation.Presentation presentation;
      if (paragraph.BaseSlide != null)
      {
        presentation = paragraph.BaseSlide.Presentation;
        baseSlide = paragraph.BaseSlide;
      }
      else
        presentation = paragraph.Presentation;
      Serializator.SerializeSolidFill(xmlWriter, underlineColorObject, -1, presentation, false, baseSlide);
      xmlWriter.WriteEndElement();
      xmlWriter.WriteEndElement();
    }
    EffectList effectList = font.EffectList;
    if (effectList != null)
    {
      xmlWriter.WriteStartElement("a", "effectLst", "http://schemas.openxmlformats.org/drawingml/2006/main");
      Serializator.SerializeEffectList(xmlWriter, effectList);
      xmlWriter.WriteEndElement();
    }
    if (font.PreservedElements == null)
      return;
    if (font.PreservedElements.ContainsKey("uLnTx"))
      DrawingSerializator.WriteRawData(xmlWriter, font.PreservedElements, "uLnTx");
    if (font.PreservedElements.ContainsKey("uLn"))
      DrawingSerializator.WriteRawData(xmlWriter, font.PreservedElements, "uLn");
    if (font.PreservedElements.ContainsKey("uFillTx"))
      DrawingSerializator.WriteRawData(xmlWriter, font.PreservedElements, "uFillTx");
    if (!font.PreservedElements.ContainsKey("uFill"))
      return;
    DrawingSerializator.WriteRawData(xmlWriter, font.PreservedElements, "uFill");
  }

  private static void SerializeBullet(XmlWriter xmlWriter, Paragraph paragraph)
  {
    ListFormat listFormat = (ListFormat) paragraph.ListFormat;
    if (listFormat.IsTextColor)
    {
      xmlWriter.WriteStartElement("a", "buClrTx", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteEndElement();
    }
    if (listFormat.GetColorObject().ColorType != ColorType.Automatic)
    {
      xmlWriter.WriteStartElement("a", "buClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
      BaseSlide baseSlide = (BaseSlide) null;
      Syncfusion.Presentation.Presentation presentation;
      if (paragraph.BaseSlide != null)
      {
        presentation = paragraph.BaseSlide.Presentation;
        baseSlide = paragraph.BaseSlide;
      }
      else
        presentation = paragraph.Presentation;
      Serializator.SerializeSolidFill(xmlWriter, listFormat.GetColorObject(), -1, presentation, false, baseSlide);
      xmlWriter.WriteEndElement();
    }
    if (listFormat.IsTextSize)
    {
      xmlWriter.WriteStartElement("a", "buSzTx", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteEndElement();
    }
    switch (listFormat.GetSizeType())
    {
      case SizeType.Points:
        xmlWriter.WriteStartElement("a", "buSzPts", "http://schemas.openxmlformats.org/drawingml/2006/main");
        xmlWriter.WriteAttributeString("val", Helper.ToString(listFormat.GetBulletSize()));
        xmlWriter.WriteEndElement();
        break;
      case SizeType.Percentage:
        xmlWriter.WriteStartElement("a", "buSzPct", "http://schemas.openxmlformats.org/drawingml/2006/main");
        xmlWriter.WriteAttributeString("val", Helper.ToString(listFormat.GetBulletSize()));
        xmlWriter.WriteEndElement();
        break;
    }
    if (listFormat.IsTextFont)
    {
      xmlWriter.WriteStartElement("a", "buFontTx", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteEndElement();
    }
    if (listFormat.IsNameChanged)
    {
      xmlWriter.WriteStartElement("a", "buFont", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteAttributeString("typeface", listFormat.GetBulletFontName());
      xmlWriter.WriteEndElement();
    }
    if (listFormat.GetListType() == ListType.None)
    {
      xmlWriter.WriteStartElement("a", "buNone", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteEndElement();
    }
    if (listFormat.GetListType() == ListType.None || listFormat.IsTypeChanged)
      return;
    switch (listFormat.GetListType())
    {
      case ListType.Bulleted:
        xmlWriter.WriteStartElement("a", "buChar", "http://schemas.openxmlformats.org/drawingml/2006/main");
        xmlWriter.WriteAttributeString("char", listFormat.GetCharacter());
        xmlWriter.WriteEndElement();
        break;
      case ListType.Numbered:
        xmlWriter.WriteStartElement("a", "buAutoNum", "http://schemas.openxmlformats.org/drawingml/2006/main");
        xmlWriter.WriteAttributeString("type", Helper.ToString(listFormat.NumberStyle));
        if (listFormat.StartValue != 0)
          xmlWriter.WriteAttributeString("startAt", Helper.ToString(listFormat.StartValue));
        xmlWriter.WriteEndElement();
        break;
      case ListType.Picture:
        xmlWriter.WriteStartElement("a", "buBlip", "http://schemas.openxmlformats.org/drawingml/2006/main");
        xmlWriter.WriteStartElement("a", "blip", "http://schemas.openxmlformats.org/drawingml/2006/main");
        listFormat.AddImageToArchive();
        xmlWriter.WriteAttributeString("r", "embed", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", listFormat.ImageRelationId);
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
        break;
    }
  }

  private static void SerializeBodyProperties(XmlWriter xmlWriter, TextBody textFrame)
  {
    xmlWriter.WriteStartElement("a", "bodyPr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (textFrame.GetBaseShape().DrawingType == DrawingType.Table)
    {
      xmlWriter.WriteEndElement();
    }
    else
    {
      if (textFrame.NumberOfColumns != 0)
      {
        int numberOfColumns = textFrame.NumberOfColumns;
        xmlWriter.WriteAttributeString("numCol", Helper.ToString(numberOfColumns));
      }
      if (textFrame.SpaceBetweenColumns != 0.0)
      {
        long emu = (long) Helper.PointToEmu(textFrame.SpaceBetweenColumns);
        xmlWriter.WriteAttributeString("spcCol", Helper.ToString(emu));
      }
      if (textFrame.RTLColumns)
        xmlWriter.WriteAttributeString("rtlCol", Helper.ToString(1));
      if (!textFrame.IsAutoMargins)
      {
        int leftMargin = textFrame.GetLeftMargin();
        int topMargin = textFrame.GetTopMargin();
        int rightMargin = textFrame.GetRightMargin();
        int bottomMargin = textFrame.GetBottomMargin();
        if (leftMargin != int.MinValue)
          xmlWriter.WriteAttributeString("lIns", Helper.ToString(leftMargin));
        if (topMargin != int.MinValue)
          xmlWriter.WriteAttributeString("tIns", Helper.ToString(topMargin));
        if (rightMargin != int.MinValue)
          xmlWriter.WriteAttributeString("rIns", Helper.ToString(rightMargin));
        if (bottomMargin != int.MinValue)
          xmlWriter.WriteAttributeString("bIns", Helper.ToString(bottomMargin));
      }
      if (textFrame.Rotation != 0)
        xmlWriter.WriteAttributeString("rot", Helper.ToString(textFrame.Rotation * 60000));
      if (textFrame.GetVerticalAlignmentType() != VerticalAlignment.None)
        xmlWriter.WriteAttributeString("anchor", Helper.ToString(textFrame.GetVerticalAlignmentType()));
      if (textFrame.TextHorizontalOverflow != TextOverflowType.None)
        xmlWriter.WriteAttributeString("horzOverflow", Helper.ToString(textFrame.TextHorizontalOverflow));
      if (textFrame.TextVerticalOverflow != TextOverflowType.None)
        xmlWriter.WriteAttributeString("vertOverflow", Helper.ToString(textFrame.TextVerticalOverflow));
      if (textFrame.AnchorCenter)
        xmlWriter.WriteAttributeString("anchorCtr", "1");
      if (textFrame.ObatinTextDirection() != TextDirection.Horizontal)
        xmlWriter.WriteAttributeString("vert", Helper.ToString(textFrame.ObatinTextDirection()));
      xmlWriter.WriteAttributeString("wrap", textFrame.WrapText ? "square" : "none");
      if (textFrame.PreservedElements.ContainsKey("prstTxWarp"))
        DrawingSerializator.WriteRawData(xmlWriter, textFrame.PreservedElements, "prstTxWarp");
      if (textFrame.GetFitTextOptionChanged())
        textFrame.IsAutoSize = false;
      if (textFrame.IsAutoSize)
      {
        xmlWriter.WriteStartElement("a", "normAutofit", "http://schemas.openxmlformats.org/drawingml/2006/main");
        if (textFrame.GetFontScale() > 0)
          xmlWriter.WriteAttributeString("fontScale", Helper.ToString(textFrame.GetFontScale()));
        if (textFrame.GetLnSpcReductionValue() > 0)
          xmlWriter.WriteAttributeString("lnSpcReduction", Helper.ToString(textFrame.GetLnSpcReductionValue()));
        xmlWriter.WriteEndElement();
      }
      else
      {
        string autoFitType = Helper.GetAutoFitType(textFrame.AutoFitType);
        if (autoFitType != null)
        {
          xmlWriter.WriteStartElement("a", autoFitType, "http://schemas.openxmlformats.org/drawingml/2006/main");
          if (textFrame.AutoFitType == AutoMarginType.NormalAutoFit && textFrame.GetFitTextOptionChanged())
            xmlWriter.WriteAttributeString("fontScale", Helper.ToString(textFrame.GetUpdatedFontScale()));
          xmlWriter.WriteEndElement();
        }
      }
      if (textFrame.PreservedElements != null)
      {
        foreach (KeyValuePair<string, Stream> preservedElement in textFrame.PreservedElements)
        {
          if (preservedElement.Key != "prstTxWarp")
            DrawingSerializator.WriteRawData(xmlWriter, textFrame.PreservedElements, preservedElement.Key);
        }
      }
      xmlWriter.WriteEndElement();
    }
  }

  private static Dictionary<string, string> GetIdList(Syncfusion.Presentation.Presentation presentation, string listName)
  {
    switch (listName)
    {
      case "sldMasterIdLst":
        return presentation.MasterList;
      case "notesMasterIdLst":
        return presentation.NotesList;
      case "handoutMasterIdLst":
        return presentation.HandoutList;
      case "sldIdLst":
        return presentation.SlideList;
      default:
        return (Dictionary<string, string>) null;
    }
  }

  private static string GetRelationId(IEnumerable<Relation> relList, string listName)
  {
    foreach (Relation rel in relList)
    {
      string[] strArray = rel.Type.Split('/');
      strArray[6] = strArray[6] + "IdLst";
      if (strArray[6].Equals(listName))
        return rel.Id;
    }
    return (string) null;
  }

  private static void SerializeCell(XmlWriter xmlWriter, Row row)
  {
    foreach (Cell cell in (IEnumerable<ICell>) row.Cells)
    {
      xmlWriter.WriteStartElement("a", "tc", "http://schemas.openxmlformats.org/drawingml/2006/main");
      if (cell.RowSpan > 1)
        xmlWriter.WriteAttributeString("rowSpan", Helper.ToString(cell.RowSpan));
      if (cell.ColumnSpan > 1)
        xmlWriter.WriteAttributeString("gridSpan", Helper.ToString(cell.ColumnSpan));
      if (cell.IsHorizontalMerge)
        xmlWriter.WriteAttributeString("hMerge", XmlConvert.ToString(Convert.ToByte(cell.IsHorizontalMerge)));
      if (cell.IsVerticalMerge)
        xmlWriter.WriteAttributeString("vMerge", XmlConvert.ToString(Convert.ToByte(cell.IsVerticalMerge)));
      if (cell.Id != null)
        xmlWriter.WriteAttributeString("id", cell.Id);
      Serializator.SerializeTextBody(xmlWriter, (TextBody) cell.TextBody);
      Serializator.SerializeCellProperties(xmlWriter, cell);
      xmlWriter.WriteEndElement();
    }
  }

  private static void SerializeCellProperties(XmlWriter xmlWriter, Cell cell)
  {
    TextBody textBody = cell.TextBody as TextBody;
    xmlWriter.WriteStartElement("a", "tcPr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (textBody.GetLeftMargin() != int.MinValue)
      xmlWriter.WriteAttributeString("marL", Helper.ToString(textBody.GetLeftMargin()));
    if (textBody.GetRightMargin() != int.MinValue)
      xmlWriter.WriteAttributeString("marR", Helper.ToString(textBody.GetRightMargin()));
    if (textBody.GetTopMargin() != int.MinValue)
      xmlWriter.WriteAttributeString("marT", Helper.ToString(textBody.GetTopMargin()));
    if (textBody.GetBottomMargin() != int.MinValue)
      xmlWriter.WriteAttributeString("marB", Helper.ToString(textBody.GetBottomMargin()));
    if (textBody.ObatinTextDirection() != TextDirection.Horizontal)
      xmlWriter.WriteAttributeString("vert", Helper.ToString(textBody.ObatinTextDirection()));
    if (textBody.VerticalAlignment != VerticalAlignmentType.None)
      xmlWriter.WriteAttributeString("anchor", Helper.ToString(textBody.GetVerticalAlignmentType()));
    if (textBody.AnchorCenter)
      xmlWriter.WriteAttributeString("anchorCtr", XmlConvert.ToString(textBody.AnchorCenter));
    if (textBody.TextHorizontalOverflow != TextOverflowType.Clip)
      xmlWriter.WriteAttributeString("horzOverflow", Helper.ToString(textBody.TextHorizontalOverflow));
    Serializator.SerializeLineProperties(xmlWriter, cell);
    if (cell.IsFillSet)
      Serializator.SerializeFillProperties(xmlWriter, (Fill) cell.Fill, cell.Table.BaseSlide.Presentation);
    xmlWriter.WriteEndElement();
  }

  private static void SerializeIdList(
    XmlWriter xmlWriter,
    Syncfusion.Presentation.Presentation presentation,
    string listName,
    string idName)
  {
    Dictionary<string, string> idList = Serializator.GetIdList(presentation, listName);
    xmlWriter.WriteStartElement("p", listName, "http://schemas.openxmlformats.org/presentationml/2006/main");
    foreach (KeyValuePair<string, string> keyValuePair in idList)
    {
      xmlWriter.WriteStartElement("p", idName, "http://schemas.openxmlformats.org/presentationml/2006/main");
      if (keyValuePair.Value != null)
        xmlWriter.WriteAttributeString("id", keyValuePair.Value);
      if (listName != "sldMasterIdLst" && listName != "sldIdLst")
      {
        string relationId = Serializator.GetRelationId((IEnumerable<Relation>) presentation.TopRelation.GetRelationList(), listName);
        xmlWriter.WriteAttributeString("r", "id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", relationId);
      }
      else
        xmlWriter.WriteAttributeString("r", "id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", keyValuePair.Key);
      xmlWriter.WriteEndElement();
    }
    xmlWriter.WriteEndElement();
  }

  private static void SerializeLineDashProperties(XmlWriter xmlWriter, LineFormat lineFormat)
  {
    if (lineFormat.DashStyle == LineDashStyle.None)
      return;
    xmlWriter.WriteStartElement("a", "prstDash", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("val", Helper.ToString(lineFormat.DashStyle));
    xmlWriter.WriteEndElement();
  }

  private static void SerializeLineEndProperties(
    XmlWriter xmlWriter,
    LineFormat lineFormat,
    bool isHead)
  {
    if (isHead)
    {
      xmlWriter.WriteStartElement("a", "headEnd", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteAttributeString("type", Helper.ToString(lineFormat.BeginArrowheadStyle));
      if (lineFormat.BeginArrowheadWidth != ArrowheadWidth.None)
        xmlWriter.WriteAttributeString("w", Helper.ToString(lineFormat.BeginArrowheadWidth));
      if (lineFormat.BeginArrowheadLength != ArrowheadLength.None)
        xmlWriter.WriteAttributeString("len", Helper.ToString(lineFormat.BeginArrowheadLength));
      xmlWriter.WriteEndElement();
    }
    else
    {
      xmlWriter.WriteStartElement("a", "tailEnd", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteAttributeString("type", Helper.ToString(lineFormat.EndArrowheadStyle));
      if (lineFormat.EndArrowheadWidth != ArrowheadWidth.None)
        xmlWriter.WriteAttributeString("w", Helper.ToString(lineFormat.EndArrowheadWidth));
      if (lineFormat.EndArrowheadLength != ArrowheadLength.None)
        xmlWriter.WriteAttributeString("len", Helper.ToString(lineFormat.EndArrowheadLength));
      xmlWriter.WriteEndElement();
    }
  }

  private static void SerializeLineJoinProperties(XmlWriter xmlWriter, LineJoinType lineJoinType)
  {
    switch (lineJoinType)
    {
      case LineJoinType.Miter:
        xmlWriter.WriteStartElement("a", "miter", "http://schemas.openxmlformats.org/drawingml/2006/main");
        xmlWriter.WriteEndElement();
        break;
      case LineJoinType.Round:
        xmlWriter.WriteStartElement("a", "round", "http://schemas.openxmlformats.org/drawingml/2006/main");
        xmlWriter.WriteEndElement();
        break;
      case LineJoinType.Bevel:
        xmlWriter.WriteStartElement("a", "bevel", "http://schemas.openxmlformats.org/drawingml/2006/main");
        xmlWriter.WriteEndElement();
        break;
    }
  }

  private static void SerializeLineProperties(XmlWriter xmlWriter, Cell cell)
  {
    if (cell.GetLeftBorder() != null)
    {
      xmlWriter.WriteStartElement("a", "lnL", "http://schemas.openxmlformats.org/drawingml/2006/main");
      Serializator.SerializeLineFillProperties(xmlWriter, (LineFormat) cell.GetLeftBorder(), cell.Table.BaseSlide.Presentation);
      xmlWriter.WriteEndElement();
    }
    if (cell.GetRightBorder() != null)
    {
      xmlWriter.WriteStartElement("a", "lnR", "http://schemas.openxmlformats.org/drawingml/2006/main");
      Serializator.SerializeLineFillProperties(xmlWriter, (LineFormat) cell.GetRightBorder(), cell.Table.BaseSlide.Presentation);
      xmlWriter.WriteEndElement();
    }
    if (cell.GetTopBorder() != null)
    {
      xmlWriter.WriteStartElement("a", "lnT", "http://schemas.openxmlformats.org/drawingml/2006/main");
      Serializator.SerializeLineFillProperties(xmlWriter, (LineFormat) cell.GetTopBorder(), cell.Table.BaseSlide.Presentation);
      xmlWriter.WriteEndElement();
    }
    if (cell.GetBottomBorder() != null)
    {
      xmlWriter.WriteStartElement("a", "lnB", "http://schemas.openxmlformats.org/drawingml/2006/main");
      Serializator.SerializeLineFillProperties(xmlWriter, (LineFormat) cell.GetBottomBorder(), cell.Table.BaseSlide.Presentation);
      xmlWriter.WriteEndElement();
    }
    if (cell.HasDiagonalDownBorder)
    {
      xmlWriter.WriteStartElement("a", "lnTlToBr", "http://schemas.openxmlformats.org/drawingml/2006/main");
      Serializator.SerializeLineFillProperties(xmlWriter, (LineFormat) cell.GetDiagonalDownBorder(), cell.Table.BaseSlide.Presentation);
      xmlWriter.WriteEndElement();
    }
    if (!cell.HasDiagonalUpBorder)
      return;
    xmlWriter.WriteStartElement("a", "lnBlToTr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    Serializator.SerializeLineFillProperties(xmlWriter, (LineFormat) cell.GetDiagonalUpBorder(), cell.Table.BaseSlide.Presentation);
    xmlWriter.WriteEndElement();
  }

  private static void SerializeNotesSize(XmlWriter xmlWriter, NotesSize notesSize)
  {
    xmlWriter.WriteStartElement("p", "notesSz", "http://schemas.openxmlformats.org/presentationml/2006/main");
    xmlWriter.WriteAttributeString("cx", Helper.ToString(notesSize.CX));
    xmlWriter.WriteAttributeString("cy", Helper.ToString(notesSize.CY));
    xmlWriter.WriteEndElement();
  }

  private static void SerializeSlideSize(XmlWriter xmlWriter, SlideSize slideSize)
  {
    xmlWriter.WriteStartElement("p", "sldSz", "http://schemas.openxmlformats.org/presentationml/2006/main");
    xmlWriter.WriteAttributeString("cx", Helper.ToString(slideSize.GetCx()));
    xmlWriter.WriteAttributeString("cy", Helper.ToString(slideSize.GetCy()));
    if (Helper.ToString(slideSize.Type) != null)
      xmlWriter.WriteAttributeString("type", Helper.ToString(slideSize.Type));
    xmlWriter.WriteEndElement();
  }

  private static void SerializeTableGrid(XmlWriter xmlWriter, Table table)
  {
    xmlWriter.WriteStartElement("a", "tblGrid", "http://schemas.openxmlformats.org/drawingml/2006/main");
    foreach (Column column in (IEnumerable<IColumn>) table.Columns)
    {
      xmlWriter.WriteStartElement("a", "gridCol", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteAttributeString("w", Helper.ToString(column.ObtainWidth()));
      xmlWriter.WriteEndElement();
    }
    xmlWriter.WriteEndElement();
  }

  private static void SerializeTableProperties(XmlWriter xmlWriter, Table table)
  {
    xmlWriter.WriteStartElement("a", "tblPr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (table.RightToLeft)
      xmlWriter.WriteAttributeString("rtl", XmlConvert.ToString(Convert.ToByte(table.RightToLeft)));
    if (table.HasHeaderRow)
      xmlWriter.WriteAttributeString("firstRow", XmlConvert.ToString(Convert.ToByte(table.HasHeaderRow)));
    if (table.HasFirstColumn)
      xmlWriter.WriteAttributeString("firstCol", XmlConvert.ToString(Convert.ToByte(table.HasFirstColumn)));
    if (table.HasTotalRow)
      xmlWriter.WriteAttributeString("lastRow", XmlConvert.ToString(Convert.ToByte(table.HasTotalRow)));
    if (table.HasLastColumn)
      xmlWriter.WriteAttributeString("lastCol", XmlConvert.ToString(Convert.ToByte(table.HasLastColumn)));
    if (table.HasBandedRows)
      xmlWriter.WriteAttributeString("bandRow", XmlConvert.ToString(Convert.ToByte(table.HasBandedRows)));
    if (table.HasBandedColumns)
      xmlWriter.WriteAttributeString("bandCol", XmlConvert.ToString(Convert.ToByte(table.HasBandedColumns)));
    EffectList effectList = table.EffectList;
    if (effectList != null)
    {
      xmlWriter.WriteStartElement("a", "effectLst", "http://schemas.openxmlformats.org/drawingml/2006/main");
      Serializator.SerializeEffectList(xmlWriter, effectList);
      xmlWriter.WriteEndElement();
    }
    if (table.Fill.FillType != FillType.Automatic)
      Serializator.SerializeFillProperties(xmlWriter, table.GetFillFormat(), table.BaseSlide.Presentation);
    if (table.BuiltInStyle != BuiltInTableStyle.None)
      Serializator.SerializeTableStyleId(xmlWriter, table);
    xmlWriter.WriteEndElement();
  }

  private static void SerializeTableRow(XmlWriter xmlWriter, Table table)
  {
    foreach (Row row in (IEnumerable<IRow>) table.Rows)
    {
      xmlWriter.WriteStartElement("a", "tr", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteAttributeString("h", Helper.ToString(row.ObtainHeight()));
      Serializator.SerializeCell(xmlWriter, row);
      xmlWriter.WriteEndElement();
    }
  }

  private static void SerializeTableStyleId(XmlWriter xmlWriter, Table table)
  {
    xmlWriter.WriteStartElement("a", "tableStyleId", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteValue(table.GetStyleList()[table.BuiltInStyle]);
    xmlWriter.WriteEndElement();
  }

  internal static void SerializeHandoutMasterSlide(XmlWriter xmlWriter, BaseSlide handoutMaster)
  {
    xmlWriter.WriteStartDocument(true);
    xmlWriter.WriteStartElement("p", nameof (handoutMaster), "http://schemas.openxmlformats.org/presentationml/2006/main");
    xmlWriter.WriteAttributeString("xmlns", "a", (string) null, "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("xmlns", "r", (string) null, "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
    xmlWriter.WriteAttributeString("xmlns", "p", (string) null, "http://schemas.openxmlformats.org/presentationml/2006/main");
    Serializator.SerializeCommonSlide(xmlWriter, handoutMaster);
    Serializator.SerializeColorMap(xmlWriter, (handoutMaster as HandoutMaster).ColorMap);
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndDocument();
    xmlWriter.Flush();
  }

  private static void SerializeColorMap(XmlWriter xmlWriter, Dictionary<string, string> colorMap)
  {
    xmlWriter.WriteStartElement("p", "clrMap", "http://schemas.openxmlformats.org/presentationml/2006/main");
    foreach (KeyValuePair<string, string> color in colorMap)
      xmlWriter.WriteAttributeString(color.Key, color.Value);
    xmlWriter.WriteEndElement();
  }

  private static void SerializeColorMap(XmlWriter xmlWriter, BaseSlide baseSlide)
  {
    xmlWriter.WriteStartElement("p", "clrMapOvr", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (baseSlide.ColorMap != null && baseSlide.ColorMap.Count != 0)
    {
      xmlWriter.WriteStartElement("a", "overrideClrMapping", "http://schemas.openxmlformats.org/drawingml/2006/main");
      foreach (KeyValuePair<string, string> color in baseSlide.ColorMap)
        xmlWriter.WriteAttributeString(color.Key, color.Value);
    }
    else
      xmlWriter.WriteStartElement("a", "masterClrMapping", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
  }

  internal static void WriteTheme1(XmlWriter xmlWriter, Theme theme)
  {
    xmlWriter.WriteStartDocument(true);
    xmlWriter.WriteStartElement("a", nameof (theme), "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("xmlns", "a", (string) null, "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("name", theme.Name);
    xmlWriter.WriteStartElement("a", "themeElements", "http://schemas.openxmlformats.org/drawingml/2006/main");
    Serializator.SerializeThemeElements(xmlWriter, theme);
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndDocument();
  }

  private static void SerializeThemeElements(XmlWriter xmlWriter, Theme theme)
  {
    Serializator.SerializeColorScheme(xmlWriter, theme);
    Serializator.SerializeFontScheme(xmlWriter, theme);
    Serializator.SerializeStyleMatrix(xmlWriter, theme);
  }

  private static void SerializeStyleMatrix(XmlWriter xmlWriter, Theme theme)
  {
    xmlWriter.WriteStartElement("a", "fmtScheme", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("name", theme.FormatSchemeName);
    Serializator.SerializeFillStyleList(xmlWriter, theme);
    Serializator.SerializeLineStyleList(xmlWriter, theme);
    Serializator.SerializeEffectStyleList(xmlWriter, theme);
    Serializator.SerializeBgFillStyleList(xmlWriter, theme);
    xmlWriter.WriteEndElement();
  }

  private static void SerializeBgFillStyleList(XmlWriter xmlWriter, Theme theme)
  {
    xmlWriter.WriteStartElement("a", "bgFillStyleLst", "http://schemas.openxmlformats.org/drawingml/2006/main");
    foreach (Fill bgFillFormat in theme.BgFillFormats)
      Serializator.SerializeFillProperties(xmlWriter, bgFillFormat, theme.BaseSlide.Presentation);
    xmlWriter.WriteEndElement();
  }

  private static void SerializeEffectStyleList(XmlWriter xmlWriter, Theme theme)
  {
    xmlWriter.WriteStartElement("a", "effectStyleLst", "http://schemas.openxmlformats.org/drawingml/2006/main");
    foreach (EffectStyle effectStyle in theme.EffectStyles)
      Serializator.SerializeEffectStyle(xmlWriter, effectStyle);
    xmlWriter.WriteEndElement();
  }

  private static void SerializeEffectStyle(XmlWriter xmlWriter, EffectStyle effectStyle)
  {
    xmlWriter.WriteStartElement("a", nameof (effectStyle), "http://schemas.openxmlformats.org/drawingml/2006/main");
    Serializator.SerializeEffectProperties(xmlWriter, effectStyle);
    xmlWriter.WriteEndElement();
  }

  private static void SerializeEffectProperties(XmlWriter xmlWriter, EffectStyle effectStyle)
  {
    xmlWriter.WriteStartElement("a", "effectLst", "http://schemas.openxmlformats.org/drawingml/2006/main");
    Serializator.SerializeEffectList(xmlWriter, effectStyle.EffectProperties.EffectList);
    xmlWriter.WriteEndElement();
    if (effectStyle.PreservedElements == null)
      return;
    foreach (KeyValuePair<string, Stream> preservedElement in effectStyle.PreservedElements)
      DrawingSerializator.WriteRawData(xmlWriter, effectStyle.PreservedElements, preservedElement.Key);
  }

  internal static void SerializeEffectList(XmlWriter xmlWriter, EffectList effectList)
  {
    Dictionary<string, Stream> preservedElements = effectList.PreservedElements;
    if (preservedElements != null)
    {
      if (preservedElements.ContainsKey("blur"))
        DrawingSerializator.WriteRawData(xmlWriter, preservedElements, "blur");
      if (preservedElements.ContainsKey("fillOverlay"))
        DrawingSerializator.WriteRawData(xmlWriter, preservedElements, "fillOverlay");
      if (preservedElements.ContainsKey("glow"))
        DrawingSerializator.WriteRawData(xmlWriter, preservedElements, "glow");
      if (preservedElements.ContainsKey("innerShdw"))
        DrawingSerializator.WriteRawData(xmlWriter, preservedElements, "innerShdw");
    }
    if (effectList.OuterShadow != null)
    {
      xmlWriter.WriteStartElement("a", "outerShdw", "http://schemas.openxmlformats.org/drawingml/2006/main");
      if (effectList.BlurRadius != -1)
        xmlWriter.WriteAttributeString("blurRad", Helper.ToString(effectList.BlurRadius));
      if (effectList.ShadowDistance != -1)
        xmlWriter.WriteAttributeString("dist", Helper.ToString(effectList.ShadowDistance));
      if (effectList.ShadowDirection != -1)
        xmlWriter.WriteAttributeString("dir", Helper.ToString(effectList.ShadowDirection));
      if (effectList.HorizontalSkew != 0)
        xmlWriter.WriteAttributeString("kx", Helper.ToString(effectList.HorizontalSkew));
      if (effectList.VerticalSkew != 0)
        xmlWriter.WriteAttributeString("ky", Helper.ToString(effectList.VerticalSkew));
      if (effectList.GetHorizontalScaling() != 0)
        xmlWriter.WriteAttributeString("sx", Helper.ToString(effectList.GetHorizontalScaling()));
      if (effectList.GetVerticalScaling() != 0)
        xmlWriter.WriteAttributeString("sy", Helper.ToString(effectList.GetVerticalScaling()));
      xmlWriter.WriteAttributeString("algn", Helper.ToString(effectList.RectangleAlignType));
      xmlWriter.WriteAttributeString("rotWithShape", effectList.RotateWithShape ? "1" : "0");
      Serializator.SerializeSolidFill(xmlWriter, effectList.OuterShadow, -1, effectList.Presentation, false, (BaseSlide) null);
      xmlWriter.WriteEndElement();
    }
    if (preservedElements == null)
      return;
    if (preservedElements.ContainsKey("prstShdw"))
      DrawingSerializator.WriteRawData(xmlWriter, preservedElements, "prstShdw");
    if (preservedElements.ContainsKey("reflection"))
      DrawingSerializator.WriteRawData(xmlWriter, preservedElements, "reflection");
    if (!preservedElements.ContainsKey("softEdge"))
      return;
    DrawingSerializator.WriteRawData(xmlWriter, preservedElements, "softEdge");
  }

  private static void SerializeLineStyleList(XmlWriter xmlWriter, Theme theme)
  {
    xmlWriter.WriteStartElement("a", "lnStyleLst", "http://schemas.openxmlformats.org/drawingml/2006/main");
    foreach (LineFormat lineFormat in theme.LineFormats)
    {
      xmlWriter.WriteStartElement("a", "ln", "http://schemas.openxmlformats.org/drawingml/2006/main");
      Serializator.SerializeLineFillProperties(xmlWriter, lineFormat, theme.BaseSlide.Presentation);
      xmlWriter.WriteEndElement();
    }
    xmlWriter.WriteEndElement();
  }

  private static void SerializeFillStyleList(XmlWriter xmlWriter, Theme theme)
  {
    xmlWriter.WriteStartElement("a", "fillStyleLst", "http://schemas.openxmlformats.org/drawingml/2006/main");
    foreach (Fill fillFormat in theme.FillFormats)
      Serializator.SerializeFillProperties(xmlWriter, fillFormat, theme.BaseSlide.Presentation);
    xmlWriter.WriteEndElement();
  }

  private static void SerializeFontScheme(XmlWriter xmlWriter, Theme theme)
  {
    xmlWriter.WriteStartElement("a", "fontScheme", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("name", theme.FontSchemeName);
    Serializator.SerializeFontSchemeElements(xmlWriter, theme.MajorFont, "majorFont");
    Serializator.SerializeFontSchemeElements(xmlWriter, theme.MinorFont, "minorFont");
    xmlWriter.WriteEndElement();
  }

  private static void SerializeFontSchemeElements(
    XmlWriter xmlWriter,
    DefaultFonts defaultFonts,
    string element)
  {
    xmlWriter.WriteStartElement("a", element, "http://schemas.openxmlformats.org/drawingml/2006/main");
    Serializator.SerializeFont(xmlWriter, defaultFonts.Latin, "latin");
    Serializator.SerializeFont(xmlWriter, defaultFonts.Ea, "ea");
    Serializator.SerializeFont(xmlWriter, defaultFonts.Cs, "cs");
    if (defaultFonts.Font != null)
    {
      foreach (KeyValuePair<string, string> keyValuePair in defaultFonts.Font)
      {
        xmlWriter.WriteStartElement("a", "font", "http://schemas.openxmlformats.org/drawingml/2006/main");
        xmlWriter.WriteAttributeString("script", keyValuePair.Key);
        xmlWriter.WriteAttributeString("typeface", keyValuePair.Value);
        xmlWriter.WriteEndElement();
      }
    }
    xmlWriter.WriteEndElement();
  }

  private static void SerializeFont(XmlWriter xmlWriter, string value, string element)
  {
    xmlWriter.WriteStartElement("a", element, "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("typeface", value);
    xmlWriter.WriteEndElement();
  }

  private static void SerializeColorScheme(XmlWriter xmlWriter, Theme theme)
  {
    xmlWriter.WriteStartElement("a", "clrScheme", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("name", theme.Name);
    foreach (KeyValuePair<string, string> themeColor in theme.ThemeColors)
      Serializator.SerializeColorElement(xmlWriter, themeColor.Key, themeColor.Value);
    xmlWriter.WriteEndElement();
  }

  private static void SerializeColorElement(XmlWriter xmlWriter, string element, string value)
  {
    xmlWriter.WriteStartElement("a", element, "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteStartElement("a", "srgbClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("val", value);
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
  }

  internal static void SerializeMasterSlide(XmlWriter xmlWriter, MasterSlide masterSlide)
  {
    xmlWriter.WriteStartDocument(true);
    xmlWriter.WriteStartElement("p", "sldMaster", "http://schemas.openxmlformats.org/presentationml/2006/main");
    xmlWriter.WriteAttributeString("xmlns", "a", (string) null, "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("xmlns", "r", (string) null, "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
    xmlWriter.WriteAttributeString("xmlns", "p", (string) null, "http://schemas.openxmlformats.org/presentationml/2006/main");
    Serializator.SerializeCommonSlide(xmlWriter, (BaseSlide) masterSlide);
    Serializator.SerializeColorMap(xmlWriter, masterSlide.ColorMap);
    Serializator.SerializeLayoutIdList(xmlWriter, masterSlide.LayoutList);
    AnimSerializator.SerializeAnimation(xmlWriter, (BaseSlide) masterSlide);
    Serializator.SerializeHeaderFooter(xmlWriter, masterSlide.HeaderFooter);
    Serializator.SerializeTextStyles(xmlWriter, masterSlide);
    Serializator.SerializeGuideList(xmlWriter, masterSlide);
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndDocument();
  }

  private static void SerializeHeaderFooter(XmlWriter xmlWriter, Dictionary<string, bool> hf)
  {
    if (hf == null)
      return;
    xmlWriter.WriteStartElement("p", nameof (hf), "http://schemas.openxmlformats.org/presentationml/2006/main");
    foreach (KeyValuePair<string, bool> keyValuePair in hf)
      xmlWriter.WriteAttributeString(keyValuePair.Key, keyValuePair.Value ? "1" : "0");
    xmlWriter.WriteEndElement();
  }

  private static void SerializeTextStyles(XmlWriter xmlWriter, MasterSlide masterSlide)
  {
    xmlWriter.WriteStartElement("p", "txStyles", "http://schemas.openxmlformats.org/presentationml/2006/main");
    xmlWriter.WriteStartElement("p", "titleStyle", "http://schemas.openxmlformats.org/presentationml/2006/main");
    Serializator.SerializeStyles(xmlWriter, masterSlide.TitleStyle.StyleList);
    xmlWriter.WriteEndElement();
    xmlWriter.WriteStartElement("p", "bodyStyle", "http://schemas.openxmlformats.org/presentationml/2006/main");
    Serializator.SerializeStyles(xmlWriter, masterSlide.BodyStyle.StyleList);
    xmlWriter.WriteEndElement();
    xmlWriter.WriteStartElement("p", "otherStyle", "http://schemas.openxmlformats.org/presentationml/2006/main");
    Serializator.SerializeStyles(xmlWriter, masterSlide.OtherStyle.StyleList);
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
  }

  private static void SerializeGuideList(XmlWriter xmlWriter, MasterSlide masterSlide)
  {
    if (!masterSlide.SlidePrsvedElts.ContainsKey("extLst"))
      return;
    DrawingSerializator.WriteRawData(xmlWriter, masterSlide.SlidePrsvedElts, "extLst");
  }

  private static void SerializeStyles(XmlWriter xmlWriter, Dictionary<string, Paragraph> styles)
  {
    foreach (KeyValuePair<string, Paragraph> style in styles)
      Serializator.SerializeStyleList(xmlWriter, style);
  }

  private static void SerializeLayoutIdList(
    XmlWriter xmlWriter,
    Dictionary<string, string> layoutList)
  {
    if (layoutList == null)
      return;
    xmlWriter.WriteStartElement("p", "sldLayoutIdLst", "http://schemas.openxmlformats.org/presentationml/2006/main");
    foreach (KeyValuePair<string, string> layout in layoutList)
    {
      xmlWriter.WriteStartElement("p", "sldLayoutId", "http://schemas.openxmlformats.org/presentationml/2006/main");
      xmlWriter.WriteAttributeString("id", layout.Value);
      xmlWriter.WriteAttributeString("r", "id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", layout.Key);
      xmlWriter.WriteEndElement();
    }
    xmlWriter.WriteEndElement();
  }

  internal static void SerializeLayoutSlide(XmlWriter xmlWriter, BaseSlide baseSlide)
  {
    xmlWriter.WriteStartDocument(true);
    xmlWriter.WriteStartElement("p", "sldLayout", "http://schemas.openxmlformats.org/presentationml/2006/main");
    xmlWriter.WriteAttributeString("xmlns", "a", (string) null, "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("xmlns", "r", (string) null, "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
    xmlWriter.WriteAttributeString("xmlns", "p", (string) null, "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (((LayoutSlide) baseSlide).IsChanged())
      xmlWriter.WriteAttributeString("showMasterSp", XmlConvert.ToString(((LayoutSlide) baseSlide).ShowMasterShape));
    Serializator.SerializeCommonSlide(xmlWriter, baseSlide);
    Serializator.SerializeColorMap(xmlWriter, baseSlide);
    AnimSerializator.SerializeAnimation(xmlWriter, baseSlide);
    if ((baseSlide as LayoutSlide).HeaderFooter != null)
      Serializator.SerializeHeaderFooter(xmlWriter, (baseSlide as LayoutSlide).HeaderFooter);
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndDocument();
  }

  internal static void SerializeTableStyles(XmlWriter xmlWriter, Syncfusion.Presentation.Presentation presentation)
  {
    xmlWriter.WriteStartElement("a", "tblStyleLst", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (presentation.DefaultTableStyle == null)
      xmlWriter.WriteAttributeString("def", "{5C22544A-7EE6-4342-B048-85BDC9FD1C3A}");
    else
      xmlWriter.WriteAttributeString("def", presentation.DefaultTableStyle);
    if (presentation.TableStyleList != null && presentation.TableStyleList.Count != 0)
    {
      foreach (TableStyle tableStyle in presentation.TableStyleList.Values)
      {
        if (tableStyle.IsCustom)
          Serializator.SerializeTableStyle(xmlWriter, tableStyle);
      }
    }
    xmlWriter.WriteEndElement();
  }

  private static void SerializeTableStyle(XmlWriter xmlWriter, TableStyle tableStyle)
  {
    xmlWriter.WriteStartElement("a", "tblStyle", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("styleId", tableStyle.Id);
    xmlWriter.WriteAttributeString("styleName", tableStyle.Name);
    Serializator.SerializeTableBackgroundStyle(xmlWriter, tableStyle);
    Serializator.SerializeTablePartStyle(xmlWriter, tableStyle.WholeTableStyle, "wholeTbl");
    Serializator.SerializeTablePartStyle(xmlWriter, tableStyle.HorizontalBand1Style, "band1H");
    Serializator.SerializeTablePartStyle(xmlWriter, tableStyle.HorizontalBand2Style, "band2H");
    Serializator.SerializeTablePartStyle(xmlWriter, tableStyle.VerticalBand1Style, "band1V");
    Serializator.SerializeTablePartStyle(xmlWriter, tableStyle.VerticalBand2Style, "band2V");
    Serializator.SerializeTablePartStyle(xmlWriter, tableStyle.LastColumn, "lastCol");
    Serializator.SerializeTablePartStyle(xmlWriter, tableStyle.FirstColumn, "firstCol");
    Serializator.SerializeTablePartStyle(xmlWriter, tableStyle.LastRow, "lastRow");
    Serializator.SerializeTablePartStyle(xmlWriter, tableStyle.FirstRow, "firstRow");
    xmlWriter.WriteEndElement();
  }

  private static void SerializeTableBackgroundStyle(XmlWriter xmlWriter, TableStyle tableStyle)
  {
    xmlWriter.WriteStartElement("a", "tblBg", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (tableStyle.TableBackgroundFill != null)
    {
      xmlWriter.WriteStartElement("a", "fill", "http://schemas.openxmlformats.org/drawingml/2006/main");
      Serializator.SerializeFillProperties(xmlWriter, tableStyle.TableBackgroundFill, tableStyle.Presentation);
      xmlWriter.WriteEndElement();
    }
    xmlWriter.WriteEndElement();
  }

  private static void SerializeTablePartStyle(
    XmlWriter xmlWriter,
    TablePartStyle tablePartStyle,
    string tag)
  {
    if (tablePartStyle == null)
      return;
    xmlWriter.WriteStartElement("a", tag, "http://schemas.openxmlformats.org/drawingml/2006/main");
    Serializator.SerializeTableCellTextStyle(xmlWriter, tablePartStyle.TableTextStyle);
    Serializator.SerializeCellStyle(xmlWriter, tablePartStyle.TableCellStyle);
    xmlWriter.WriteEndElement();
  }

  private static void SerializeCellStyle(XmlWriter xmlWriter, TableCellStyle tableCellStyle)
  {
    if (tableCellStyle == null)
      return;
    xmlWriter.WriteStartElement("a", "tcStyle", "http://schemas.openxmlformats.org/drawingml/2006/main");
    Serializator.SerializeCellBorderStyle(xmlWriter, tableCellStyle.CellBorderStyle);
    Serializator.SerializeCellFill(xmlWriter, tableCellStyle.Fill);
    xmlWriter.WriteEndElement();
  }

  private static void SerializeCellFill(XmlWriter xmlWriter, Fill fillFormat)
  {
    if (fillFormat == null)
      return;
    xmlWriter.WriteStartElement("a", "fill", "http://schemas.openxmlformats.org/drawingml/2006/main");
    Serializator.SerializeFillProperties(xmlWriter, fillFormat, fillFormat.Presentation);
    xmlWriter.WriteEndElement();
  }

  private static void SerializeCellBorderStyle(XmlWriter xmlWriter, BorderStyle borderStyle)
  {
    xmlWriter.WriteStartElement("a", "tcBdr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (borderStyle != null)
    {
      Serializator.SerializeCellBorderLine(xmlWriter, borderStyle.Left, "left");
      Serializator.SerializeCellBorderLine(xmlWriter, borderStyle.Right, "right");
      Serializator.SerializeCellBorderLine(xmlWriter, borderStyle.Top, "top");
      Serializator.SerializeCellBorderLine(xmlWriter, borderStyle.Bottom, "bottom");
      Serializator.SerializeCellBorderLine(xmlWriter, borderStyle.InsideHorzBorder, "insideH");
      Serializator.SerializeCellBorderLine(xmlWriter, borderStyle.InsideVertBorder, "insideV");
    }
    xmlWriter.WriteEndElement();
  }

  private static void SerializeCellBorderLine(
    XmlWriter xmlWriter,
    LineFormat lineFormat,
    string tag)
  {
    if (lineFormat == null)
      return;
    xmlWriter.WriteStartElement("a", tag, "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteStartElement("a", "ln", "http://schemas.openxmlformats.org/drawingml/2006/main");
    Serializator.SerializeLineFillProperties(xmlWriter, lineFormat, ((Fill) lineFormat.Fill).Presentation);
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
  }

  private static void SerializeTableCellTextStyle(
    XmlWriter xmlWriter,
    TableTextStyle tableTextStyle)
  {
    if (tableTextStyle == null)
      return;
    xmlWriter.WriteStartElement("a", "tcTxStyle", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("b", Helper.ToString(tableTextStyle.Bold));
    xmlWriter.WriteAttributeString("i", Helper.ToString(tableTextStyle.Italic));
    if (tableTextStyle.FontRefIndex != null)
    {
      xmlWriter.WriteStartElement("a", "fontRef", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlWriter.WriteAttributeString("idx", tableTextStyle.FontRefIndex);
      Serializator.SerializeSolidFill(xmlWriter, (ColorObject) tableTextStyle.TextRefColor, -1, tableTextStyle.Parent.Parent.Presentation, false, (BaseSlide) null);
      xmlWriter.WriteEndElement();
    }
    if (tableTextStyle.TextColor != null)
      Serializator.SerializeSolidFill(xmlWriter, (ColorObject) tableTextStyle.TextColor, -1, tableTextStyle.Parent.Parent.Presentation, false, (BaseSlide) null);
    xmlWriter.WriteEndElement();
  }

  private static void SerializeElementString(
    XmlWriter writer,
    string elementName,
    string value,
    string prefix)
  {
    if (value == null)
      return;
    writer.WriteElementString(prefix, elementName, (string) null, value);
  }

  protected static void SerializeElementString(XmlWriter writer, string elementName, string value)
  {
    if (value == null)
      return;
    writer.WriteElementString(elementName, value);
  }

  protected static void SerializeElementString(XmlWriter writer, string elementName, bool value)
  {
    if (value)
      writer.WriteElementString(elementName, "true");
    if (value)
      return;
    writer.WriteElementString(elementName, "false");
  }

  private static void SerializeElementString(
    XmlWriter writer,
    string elementName,
    int value,
    int defaultValue)
  {
    if (value == defaultValue)
      return;
    string str = value.ToString();
    writer.WriteElementString(elementName, str);
  }

  internal static void SerializeExtendedProperties(
    XmlWriter writer,
    IBuiltInDocumentProperties builtInDocumentProperties,
    ICustomDocumentProperties customDocumentProperties)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartDocument(true);
    writer.WriteStartElement("Properties", "http://schemas.openxmlformats.org/officeDocument/2006/extended-properties");
    IBuiltInDocumentProperties documentProperties = builtInDocumentProperties;
    if (builtInDocumentProperties.ApplicationName == null || builtInDocumentProperties.ApplicationName == string.Empty)
      documentProperties.ApplicationName = "Essential Presentation";
    Serializator.SerializeElementString(writer, "Application", documentProperties.ApplicationName);
    Serializator.SerializeElementString(writer, "Characters", documentProperties.CharCount, int.MinValue);
    Serializator.SerializeElementString(writer, "Company", documentProperties.Company);
    Serializator.SerializeElementString(writer, "Lines", documentProperties.LineCount, int.MinValue);
    if (!string.IsNullOrEmpty(builtInDocumentProperties.Manager))
      Serializator.SerializeElementString(writer, "Manager", documentProperties.Manager);
    Serializator.SerializeElementString(writer, "MMClips", documentProperties.MultimediaClipCount, int.MinValue);
    Serializator.SerializeElementString(writer, "Notes", documentProperties.NoteCount, int.MinValue);
    Serializator.SerializeElementString(writer, "Pages", documentProperties.PageCount, int.MinValue);
    Serializator.SerializeElementString(writer, "Paragraphs", documentProperties.ParagraphCount, int.MinValue);
    if (!string.IsNullOrEmpty(builtInDocumentProperties.PresentationTarget))
      Serializator.SerializeElementString(writer, "PresentationFormat", documentProperties.PresentationTarget);
    if (!string.IsNullOrEmpty(builtInDocumentProperties.Template))
      Serializator.SerializeElementString(writer, "Template", documentProperties.Template);
    Serializator.SerializeElementString(writer, "HiddenSlides", documentProperties.HiddenCount, int.MinValue);
    Serializator.SerializeElementString(writer, "LinksUpToDate", documentProperties.LinksDirty);
    Serializator.SerializeElementString(writer, "ScaleCrop", (documentProperties as BuiltInDocumentProperties).ScaleCrop);
    Serializator.SerializeElementString(writer, "Slides", documentProperties.SlideCount, int.MinValue);
    TimeSpan editTime = documentProperties.EditTime;
    if (editTime != TimeSpan.MinValue)
    {
      uint totalMinutes = (uint) editTime.TotalMinutes;
      writer.WriteElementString("TotalTime", totalMinutes.ToString());
    }
    Serializator.SerializeElementString(writer, "Words", documentProperties.WordCount, int.MinValue);
    if (((CustomDocumentProperties) customDocumentProperties).GetProperty("_PID_LINKBASE") is DocumentPropertyImpl property)
    {
      byte[] blob = property.Blob;
      string str1 = Encoding.Unicode.GetString(blob, 0, blob.Length);
      string str2 = str1.Remove(str1.Length - 1);
      writer.WriteElementString("HyperlinkBase", str2);
    }
    Serializator.SerializeAppVersion(writer);
    writer.WriteEndElement();
    writer.WriteEndDocument();
  }

  protected static void SerializeAppVersion(XmlWriter writer)
  {
    Serializator.SerializeElementString(writer, "AppVersion", "12.0000");
  }

  internal static void SerializeCoreProperties(
    XmlWriter writer,
    IBuiltInDocumentProperties builtInDocumentProperties)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    IBuiltInDocumentProperties documentProperties = builtInDocumentProperties;
    writer.WriteStartDocument(true);
    writer.WriteStartElement("cp", "coreProperties", "http://schemas.openxmlformats.org/package/2006/metadata/core-properties");
    writer.WriteAttributeString("xmlns", "dc", (string) null, "http://purl.org/dc/elements/1.1/");
    writer.WriteAttributeString("xmlns", "dcterms", (string) null, "http://purl.org/dc/terms/");
    writer.WriteAttributeString("xmlns", "dcmitype", (string) null, "http://purl.org/dc/dcmitype/");
    writer.WriteAttributeString("xmlns", "xsi", (string) null, "http://www.w3.org/2001/XMLSchema-instance");
    if (!string.IsNullOrEmpty(builtInDocumentProperties.Category))
      Serializator.SerializeElementString(writer, "category", documentProperties.Category, "cp");
    Serializator.SerializeElementString(writer, "creator", documentProperties.Author, "dc");
    Serializator.SerializeElementString(writer, "description", documentProperties.Comments, "dc");
    if (!string.IsNullOrEmpty(builtInDocumentProperties.Keywords))
      Serializator.SerializeElementString(writer, "keywords", documentProperties.Keywords, "cp");
    if (!string.IsNullOrEmpty(builtInDocumentProperties.LastAuthor))
      Serializator.SerializeElementString(writer, "lastModifiedBy", documentProperties.LastAuthor, "cp");
    Serializator.SerializeCreatedModifiedTimeElement(writer, "created", documentProperties.CreationDate);
    Serializator.SerializeCreatedModifiedTimeElement(writer, "modified", documentProperties.LastSaveDate);
    Serializator.SerializeElementString(writer, "subject", documentProperties.Subject, "dc");
    if (!string.IsNullOrEmpty(builtInDocumentProperties.RevisionNumber))
      Serializator.SerializeElementString(writer, "revision", documentProperties.RevisionNumber, "cp");
    if (!string.IsNullOrEmpty(builtInDocumentProperties.ContentStatus))
      Serializator.SerializeElementString(writer, "contentStatus", documentProperties.ContentStatus, "cp");
    if (!string.IsNullOrEmpty(builtInDocumentProperties.Language))
      Serializator.SerializeElementString(writer, "language", documentProperties.Language, "dc");
    Serializator.SerializeElementString(writer, "version", documentProperties.Version, "cp");
    DateTime lastPrinted = documentProperties.LastPrinted;
    if (lastPrinted != DateTime.MinValue)
    {
      string str = lastPrinted.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ", (IFormatProvider) CultureInfo.InvariantCulture);
      writer.WriteElementString("cp", "lastPrinted", (string) null, str);
    }
    Serializator.SerializeElementString(writer, "title", documentProperties.Title, "dc");
    writer.WriteEndElement();
    writer.WriteEndDocument();
  }

  private static void SerializeCreatedModifiedTimeElement(
    XmlWriter writer,
    string tagName,
    DateTime dateTime)
  {
    if (!(dateTime.Date != DateTime.MinValue))
      return;
    writer.WriteStartElement("dcterms", tagName, (string) null);
    writer.WriteAttributeString("xsi", "type", (string) null, "dcterms:W3CDTF");
    string data = dateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ", (IFormatProvider) CultureInfo.InvariantCulture);
    writer.WriteRaw(data);
    writer.WriteEndElement();
  }

  internal static void SerializeCustomProperties(
    XmlWriter writer,
    ICustomDocumentProperties customDocumentProperties)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartDocument(true);
    writer.WriteStartElement("Properties", "http://schemas.openxmlformats.org/officeDocument/2006/custom-properties");
    writer.WriteAttributeString("xmlns", "vt", (string) null, "http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes");
    CustomDocumentProperties documentProperties = (CustomDocumentProperties) customDocumentProperties;
    int iPropertyId = 2;
    int iIndex = 0;
    for (int count = documentProperties.Count; iIndex < count; ++iIndex)
    {
      DocumentPropertyImpl property = (DocumentPropertyImpl) documentProperties[iIndex];
      if (property.Name != "_PID_LINKBASE" && property.Name != "_PID_HLINKS")
      {
        Serializator.SerializeCustomProperty(writer, property, iPropertyId);
        ++iPropertyId;
      }
    }
    writer.WriteEndElement();
    writer.WriteEndDocument();
  }

  internal static void SerializeAttribute(
    XmlWriter writer,
    string attributeName,
    string value,
    string defaultValue)
  {
    if (!(value != defaultValue))
      return;
    writer.WriteAttributeString(attributeName, value);
  }

  internal static void SerializeAttribute(
    XmlWriter writer,
    string attributeName,
    int value,
    int defaultValue)
  {
    if (value == defaultValue)
      return;
    string str = value.ToString();
    writer.WriteAttributeString(attributeName, str);
  }

  private static void SerializeCustomProperty(
    XmlWriter writer,
    DocumentPropertyImpl property,
    int iPropertyId)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (property == null)
      throw new ArgumentNullException(nameof (property));
    writer.WriteStartElement(nameof (property));
    Serializator.SerializeAttribute(writer, "fmtid", "{D5CDD505-2E9C-101B-9397-08002B2CF9AE}", string.Empty);
    Serializator.SerializeAttribute(writer, "pid", iPropertyId, int.MinValue);
    Serializator.SerializeAttribute(writer, "name", property.Name, string.Empty);
    switch (property.PropertyType)
    {
      case PropertyType.Int32:
        string str1 = property.Int32.ToString();
        writer.WriteElementString("vt", "i4", (string) null, str1);
        break;
      case PropertyType.Double:
        string str2 = property.Double.ToString((IFormatProvider) NumberFormatInfo.InvariantInfo);
        writer.WriteElementString("vt", "r8", (string) null, str2);
        break;
      case PropertyType.Bool:
        string lower = property.Boolean.ToString().ToLower(CultureInfo.InvariantCulture);
        writer.WriteElementString("vt", "bool", (string) null, lower);
        break;
      case PropertyType.Int:
        string str3 = property.Integer.ToString();
        writer.WriteElementString("vt", "int", (string) null, str3);
        break;
      case PropertyType.AsciiString:
        writer.WriteElementString("vt", "lpstr", (string) null, property.Text);
        break;
      case PropertyType.String:
        writer.WriteElementString("vt", "lpwstr", (string) null, property.Text);
        break;
      case PropertyType.DateTime:
        string str4 = property.DateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
        writer.WriteElementString("vt", "filetime", (string) null, str4);
        break;
    }
    writer.WriteEndElement();
  }

  internal static void SerializeDataModel(XmlWriter xmlWriter, DataModel dataModel)
  {
    xmlWriter.WriteStartDocument(true);
    xmlWriter.WriteStartElement("dgm", nameof (dataModel), "http://schemas.openxmlformats.org/drawingml/2006/diagram");
    xmlWriter.WriteAttributeString("xmlns", "dgm", (string) null, "http://schemas.openxmlformats.org/drawingml/2006/diagram");
    xmlWriter.WriteAttributeString("xmlns", "a", (string) null, "http://schemas.openxmlformats.org/drawingml/2006/main");
    Serializator.SerializeSmartArtPointList(xmlWriter, dataModel);
    Serializator.SerializeSmartArtConnectionList(xmlWriter, dataModel);
    Serializator.SerializeSmartArtBackground(xmlWriter, dataModel.ParentSmartArt);
    Serializator.SerializeSmartArtLineProperties(xmlWriter, dataModel);
    xmlWriter.WriteStartElement("dgm", "extLst", "http://schemas.openxmlformats.org/drawingml/2006/diagram");
    xmlWriter.WriteStartElement("a", "ext", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("uri", (string) null, "http://schemas.microsoft.com/office/drawing/2008/diagram");
    xmlWriter.WriteStartElement("dsp", "dataModelExt", "http://schemas.microsoft.com/office/drawing/2008/diagram");
    xmlWriter.WriteAttributeString("xmlns", "dsp", (string) null, "http://schemas.microsoft.com/office/drawing/2008/diagram");
    xmlWriter.WriteAttributeString("relId", "rId6");
    xmlWriter.WriteAttributeString("minVer", "http://schemas.openxmlformats.org/drawingml/2006/diagram");
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndDocument();
  }

  private static void SerializeSmartArtLineProperties(XmlWriter xmlWriter, DataModel dataModel)
  {
    xmlWriter.WriteStartElement("dgm", "whole", "http://schemas.openxmlformats.org/drawingml/2006/diagram");
    if (dataModel.ParentSmartArt.HasLineProperties)
    {
      xmlWriter.WriteStartElement("a", "ln", "http://schemas.openxmlformats.org/drawingml/2006/main");
      Serializator.SerializeLineFillProperties(xmlWriter, (LineFormat) dataModel.ParentSmartArt.LineFormat, dataModel.ParentSmartArt.BaseSlide.Presentation);
      xmlWriter.WriteEndElement();
    }
    xmlWriter.WriteEndElement();
  }

  private static void SerializeSmartArtBackground(XmlWriter xmlWriter, SmartArt smartArt)
  {
    xmlWriter.WriteStartElement("dgm", "bg", "http://schemas.openxmlformats.org/drawingml/2006/diagram");
    Serializator.SerializeFillProperties(xmlWriter, smartArt.GetFillFormat(), smartArt.BaseSlide.Presentation);
    xmlWriter.WriteEndElement();
  }

  private static void SerializeSmartArtConnectionList(XmlWriter xmlWriter, DataModel dataModel)
  {
    xmlWriter.WriteStartElement("dgm", "cxnLst", "http://schemas.openxmlformats.org/drawingml/2006/diagram");
    foreach (SmartArtConnection connection in dataModel.ConnectionCollection.GetConnectionList())
      Serializator.SerializeSmartArtConnectionList(xmlWriter, connection);
    xmlWriter.WriteEndElement();
  }

  private static void SerializeSmartArtConnectionList(
    XmlWriter xmlWriter,
    SmartArtConnection connection)
  {
    xmlWriter.WriteStartElement("dgm", "cxn", "http://schemas.openxmlformats.org/drawingml/2006/diagram");
    if (connection.ModelId != Guid.Empty)
      xmlWriter.WriteAttributeString("modelId", Serializator.GetGuidString(connection.ModelId));
    if (connection.Type != SmartArtConnectionType.ParentOf)
      xmlWriter.WriteAttributeString("type", Helper.ToString(connection.Type));
    if (connection.SourceId != Guid.Empty)
      xmlWriter.WriteAttributeString("srcId", Serializator.GetGuidString(connection.SourceId));
    if (connection.DestinationId != Guid.Empty)
      xmlWriter.WriteAttributeString("destId", Serializator.GetGuidString(connection.DestinationId));
    xmlWriter.WriteAttributeString("srcOrd", Helper.ToString((long) connection.SourcePosition));
    xmlWriter.WriteAttributeString("destOrd", Helper.ToString((long) connection.DestinationPosition));
    if (connection.ParentTransitionId != Guid.Empty)
      xmlWriter.WriteAttributeString("parTransId", Serializator.GetGuidString(connection.ParentTransitionId));
    if (connection.SiblingTransitionId != Guid.Empty)
      xmlWriter.WriteAttributeString("sibTransId", Serializator.GetGuidString(connection.SiblingTransitionId));
    if (!string.IsNullOrEmpty(connection.PresentationId))
      xmlWriter.WriteAttributeString("presId", connection.PresentationId);
    xmlWriter.WriteEndElement();
  }

  private static void SerializeSmartArtPointList(XmlWriter xmlWriter, DataModel dataModel)
  {
    xmlWriter.WriteStartElement("dgm", "ptLst", "http://schemas.openxmlformats.org/drawingml/2006/diagram");
    foreach (SmartArtPoint point in dataModel.PointCollection.GetPointList())
      Serializator.SerializeSmartArtPoint(xmlWriter, point);
    xmlWriter.WriteEndElement();
  }

  private static void SerializeSmartArtPoint(XmlWriter xmlWriter, SmartArtPoint point)
  {
    xmlWriter.WriteStartElement("dgm", "pt", "http://schemas.openxmlformats.org/drawingml/2006/diagram");
    if (point.ModelId != Guid.Empty)
      xmlWriter.WriteAttributeString("modelId", Serializator.GetGuidString(point.ModelId));
    if (point.Type != SmartArtPointType.Node)
      xmlWriter.WriteAttributeString("type", Helper.ToString(point.Type));
    if (point.ConnectionId != Guid.Empty)
      xmlWriter.WriteAttributeString("cxnId", Serializator.GetGuidString(point.ConnectionId));
    Serializator.SerializeSmartArtPointElements(xmlWriter, point);
    xmlWriter.WriteEndElement();
  }

  internal static string GetGuidString(Guid guid) => $"{{{guid.ToString()}}}".ToUpper();

  private static void SerializeSmartArtPointElements(XmlWriter xmlWriter, SmartArtPoint point)
  {
    Serializator.SerializeSmartArtPropertySet(xmlWriter, point);
    if (point.HasShapeProperties)
      DrawingSerializator.SerializeGrpSpProp(xmlWriter, (Shape) point.PointShapeProperties);
    else
      xmlWriter.WriteElementString("dgm", "spPr", "http://schemas.openxmlformats.org/drawingml/2006/diagram", (string) null);
    Serializator.SerializeTextBody(xmlWriter, point.TextBody);
    Serializator.SerializeExtensionList(xmlWriter, point);
  }

  private static void SerializeExtensionList(XmlWriter xmlWriter, SmartArtPoint point)
  {
    if (point.Hyperlink == null)
      return;
    xmlWriter.WriteStartElement("dgm", "extLst", "http://schemas.openxmlformats.org/drawingml/2006/diagram");
    xmlWriter.WriteStartElement("a", "ext", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("uri", "{E40237B7-FDA0-4F09-8148-C483321AD2D9}");
    Serializator.SerializeNonVisualSmartArtPoint(xmlWriter, point);
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndElement();
  }

  private static void SerializeNonVisualSmartArtPoint(XmlWriter xmlWriter, SmartArtPoint point)
  {
    xmlWriter.WriteStartElement("dgm14", "cNvPr", "http://schemas.microsoft.com/office/drawing/2010/diagram");
    string str = Helper.ToString(point.SmartArtId);
    if (str != null)
      xmlWriter.WriteAttributeString("id", str);
    string smartArtName = point.SmartArtName;
    xmlWriter.WriteAttributeString("name", smartArtName);
    if (point.Hyperlink != null)
      Serializator.SerializeHyperlink(xmlWriter, (Hyperlink) point.Hyperlink);
    xmlWriter.WriteEndElement();
  }

  private static void SerializeSmartArtPropertySet(XmlWriter xmlWriter, SmartArtPoint point)
  {
    if (!point.HasPropertySet)
      return;
    xmlWriter.WriteStartElement("dgm", "prSet", "http://schemas.openxmlformats.org/drawingml/2006/diagram");
    if (point.PresentationElementId != Guid.Empty)
      xmlWriter.WriteAttributeString("presAssocID", Serializator.GetGuidString(point.PresentationElementId));
    if (!string.IsNullOrEmpty(point.PresentationName))
      xmlWriter.WriteAttributeString("presName", point.PresentationName);
    if (point.DataModel.IsSmartArtTypeSet && point.Type == SmartArtPointType.Document)
    {
      if (Serializator.CheckSpecialSmartArtType(point.DataModel.SmartArtType))
        xmlWriter.WriteAttributeString("loTypeId", "urn:microsoft.com/office/officeart/2009/3/layout/" + Helper.ToString(point.DataModel.SmartArtType));
      else if (point.DataModel.SmartArtType == SmartArtType.CirclePictureHierarchy || point.DataModel.SmartArtType == SmartArtType.CircleArrowProcess || point.DataModel.SmartArtType == SmartArtType.ReverseList)
        xmlWriter.WriteAttributeString("loTypeId", "urn:microsoft.com/office/officeart/2009/layout/" + Helper.ToString(point.DataModel.SmartArtType));
      else if (point.DataModel.ParentSmartArt.Is2005Layout)
        xmlWriter.WriteAttributeString("loTypeId", "urn:microsoft.com/office/officeart/2005/8/layout/" + Helper.ToString(point.DataModel.SmartArtType));
      else
        xmlWriter.WriteAttributeString("loTypeId", "urn:microsoft.com/office/officeart/2008/layout/" + Helper.ToString(point.DataModel.SmartArtType));
      if (point.DataModel.ColorSchemeType != null)
        xmlWriter.WriteAttributeString("csTypeId", point.DataModel.ColorSchemeType);
      else
        xmlWriter.WriteAttributeString("csTypeId", "urn:microsoft.com/office/officeart/2005/8/colors/accent1_2");
    }
    if (!string.IsNullOrEmpty(point.DataModel.Category) && point.Type == SmartArtPointType.Document)
      xmlWriter.WriteAttributeString("loCatId", point.DataModel.Category);
    if (point.DataModel.QuickStyleType != null && point.Type == SmartArtPointType.Document)
      xmlWriter.WriteAttributeString("qsTypeId", point.DataModel.QuickStyleType);
    if (!string.IsNullOrEmpty(point.PlaceholderText))
      xmlWriter.WriteAttributeString("phldrT", point.PlaceholderText);
    if (point.IsPlaceholder)
      xmlWriter.WriteAttributeString("phldr", "1");
    if (point.CustomAngle != 0)
      xmlWriter.WriteAttributeString("custAng", Helper.ToString(point.CustomAngle));
    if (point.CustomScaleX != 0)
      xmlWriter.WriteAttributeString("custScaleX", Helper.ToString(point.CustomScaleX));
    if (point.CustomScaleY != 0)
      xmlWriter.WriteAttributeString("custScaleY", Helper.ToString(point.CustomScaleY));
    if (point.FactorNeighbourX != 0)
      xmlWriter.WriteAttributeString("custLinFactNeighborX", Helper.ToString(point.FactorNeighbourX));
    if (point.FactorNeighbourY != 0)
      xmlWriter.WriteAttributeString("custLinFactNeighborY", Helper.ToString(point.FactorNeighbourY));
    if (point.IsTextChanged.HasValue)
      xmlWriter.WriteAttributeString("custT", point.IsTextChanged.Value ? "1" : "0");
    if (point.CustomAttributes != null && point.CustomAttributes.Count != 0)
    {
      foreach (KeyValuePair<string, string> customAttribute in point.CustomAttributes)
        xmlWriter.WriteAttributeString(customAttribute.Key, customAttribute.Value);
    }
    xmlWriter.WriteEndElement();
  }

  private static bool CheckSpecialSmartArtType(SmartArtType smartArtType)
  {
    switch (smartArtType)
    {
      case SmartArtType.PieProcess:
      case SmartArtType.DescendingBlockList:
      case SmartArtType.StepUpProcess:
      case SmartArtType.IncreasingArrowsProcess:
      case SmartArtType.SubStepProcess:
      case SmartArtType.PhasedProcess:
      case SmartArtType.RandomToResultProcess:
      case SmartArtType.DescendingProcess:
      case SmartArtType.HorizontalOrganizationChart:
      case SmartArtType.CircleRelationship:
      case SmartArtType.OpposingIdeas:
      case SmartArtType.PlusAndMinus:
      case SmartArtType.SnapshotPictureList:
      case SmartArtType.SpiralPicture:
      case SmartArtType.FramedTextPicture:
        return true;
      default:
        return false;
    }
  }

  internal static void SerializeSmartArtDrawing(XmlWriter xmlWriter, DataModel dataModel)
  {
    xmlWriter.WriteStartDocument(true);
    xmlWriter.WriteStartElement("dsp", "drawing", "http://schemas.microsoft.com/office/drawing/2008/diagram");
    xmlWriter.WriteAttributeString("xmlns", "dgm", (string) null, "http://schemas.openxmlformats.org/drawingml/2006/diagram");
    xmlWriter.WriteAttributeString("xmlns", "dsp", (string) null, "http://schemas.microsoft.com/office/drawing/2008/diagram");
    xmlWriter.WriteAttributeString("xmlns", "a", (string) null, "http://schemas.openxmlformats.org/drawingml/2006/main");
    DrawingSerializator.SerializeShapeTree(xmlWriter, dataModel.ParentSmartArt);
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndDocument();
  }

  internal static void SerializeSlideTransition(XmlWriter writer, Slide slide)
  {
    if (!slide.IsSlideTransition)
      return;
    if (slide.IsAlternateContent)
    {
      if (slide.InternalTransition.AlternateContent == null)
        return;
      writer.WriteStartElement("mc", "AlternateContent", "http://schemas.openxmlformats.org/markup-compatibility/2006");
      if (slide.InternalTransition.AlternateContent.Choice != null)
        Serializator.SerializeChoiceElements(writer, slide.InternalTransition.AlternateContent.Choice, slide);
      if (slide.InternalTransition.AlternateContent.FallBack != null)
        Serializator.SerializeFallBackElements(writer, slide.InternalTransition.AlternateContent.FallBack, slide);
      writer.WriteEndElement();
    }
    else
    {
      slide.InternalTransition.Transition.SlideShowTransition = (SlideShowTransition) slide.SlideTransition;
      Serializator.SerializeTransition(writer, slide.InternalTransition.Transition, slide);
    }
  }

  private static void SerializeChoiceElements(XmlWriter writer, Choice choice, Slide slide)
  {
    writer.WriteStartElement("mc", "Choice", "http://schemas.openxmlformats.org/markup-compatibility/2006");
    choice.Transition.SlideShowTransition = (SlideShowTransition) slide.SlideTransition;
    if (choice.Transition.PrstTrans != null)
    {
      writer.WriteAttributeString("xmlns", "p15", (string) null, "http://schemas.microsoft.com/office/powerpoint/2012/main");
      writer.WriteAttributeString("Requires", "p15");
      Serializator.SerializeTransition(writer, choice.Transition, slide);
    }
    else if (choice.Transition.Morph != null)
    {
      writer.WriteAttributeString("xmlns", "p159", (string) null, "http://schemas.microsoft.com/office/powerpoint/2015/09/main");
      writer.WriteAttributeString("Requires", "p159");
      Serializator.SerializeTransition(writer, choice.Transition, slide);
    }
    else
    {
      writer.WriteAttributeString("xmlns", "p14", (string) null, "http://schemas.microsoft.com/office/powerpoint/2010/main");
      writer.WriteAttributeString("Requires", "p14");
      Serializator.SerializeTransition(writer, choice.Transition, slide);
    }
    writer.WriteEndElement();
  }

  private static void SerializeFallBackElements(XmlWriter writer, FallBack fallBack, Slide slide)
  {
    writer.WriteStartElement("mc", "Fallback", "http://schemas.openxmlformats.org/markup-compatibility/2006");
    Serializator.SerializeTransition(writer, fallBack.Transition, slide);
    writer.WriteEndElement();
  }

  private static void SerializeTransition(
    XmlWriter writer,
    TransitionInternal slideShowTransition_internal,
    Slide slide)
  {
    writer.WriteStartElement("p", "transition", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (!string.IsNullOrEmpty(slideShowTransition_internal.TransitionNameSpace))
      writer.WriteAttributeString("xmlns", "p14", (string) null, "http://schemas.microsoft.com/office/powerpoint/2010/main");
    if (slideShowTransition_internal.SlideShowTransition.Speed != TransitionSpeed.None && slideShowTransition_internal.SlideShowTransition.Speed != TransitionSpeed.Fast)
      writer.WriteAttributeString("spd", SlideTransitionConstant.GetSlideTransitionSpeed(slideShowTransition_internal.SlideShowTransition.Speed));
    if ((double) slideShowTransition_internal.SlideShowTransition.Duration != 0.0)
      writer.WriteAttributeString("p14", "dur", (string) null, (slideShowTransition_internal.SlideShowTransition.Duration * 1000f).ToString());
    if (!slide.SlideTransition.TriggerOnClick)
      writer.WriteAttributeString("advClick", "0");
    if (slideShowTransition_internal.SlideShowTransition.TriggerOnTimeDelay || slideShowTransition_internal.IsAdvanceOnTime)
    {
      Decimal num = Decimal.Parse((slide.SlideTransition.TimeDelay * 1000f).ToString(), NumberStyles.Float);
      writer.WriteAttributeString("advTm", num.ToString());
    }
    if (slideShowTransition_internal.Blinds != null)
      Serializator.SerializeBlindsTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Checker != null)
      Serializator.SerializeCheckerTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Circle != null)
      Serializator.SerializeCircleTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Comb != null)
      Serializator.SerializeCombTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Conveyor != null)
      Serializator.SerializeConveyorTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Cover != null)
      Serializator.SerializeCoverTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Cut != null)
      Serializator.SerializeCutTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Diamond != null)
      Serializator.SerializeDiamondTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Dissolve != null)
      Serializator.SerializeDissolveTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Door != null)
      Serializator.SerializeDoorsTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Fade != null)
      Serializator.SerializeFadeTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Ferris != null)
      Serializator.SerializeFerrisTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Flash != null)
      Serializator.SerializeFlashTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Flip != null)
      Serializator.SerializeFlipTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Flythrough != null)
      Serializator.SerializeFlyThroughTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Gallery != null)
      Serializator.SerializeGalleryTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Glitter != null)
      Serializator.SerializeGlitterTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.HoneyComb != null)
      Serializator.SerializeHoneyCombTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Morph != null)
      Serializator.SerializeMorphTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.NewsFlash != null)
      Serializator.SerializeNewsFlashTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Pan != null)
      Serializator.SerializePanTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Plus != null)
      Serializator.SerializePlusTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Prism != null)
      Serializator.SerializePrismTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.PrstTrans != null)
      Serializator.SerializePrstTransTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Pull != null)
      Serializator.SerializePullTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Push != null)
      Serializator.SerializePushTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Random != null)
      Serializator.SerializeRandomTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.RandomBar != null)
      Serializator.SerializeRandomBarTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Reveal != null)
      Serializator.SerializeRevealTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Ripple != null)
      Serializator.SerializeRippleTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Shred != null)
      Serializator.SerializeShredTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Split != null)
      Serializator.SerializeSplitTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Strips != null)
      Serializator.SerializeStripsTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Switch != null)
      Serializator.SerializeSwitchTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Vortex != null)
      Serializator.SerializeVortexTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Warp != null)
      Serializator.SerializeWarpTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Wedge != null)
      Serializator.SerializeWedgeTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Wheel != null)
      Serializator.SerializeWheelTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.WheelReverse != null)
      Serializator.SerializeWheelReverseTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Window != null)
      Serializator.SerializeWindowTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Wipe != null)
      Serializator.SerializeWipeTransition(writer, slideShowTransition_internal);
    else if (slideShowTransition_internal.Zoom != null)
      Serializator.SerializeZoomTransition(writer, slideShowTransition_internal);
    writer.WriteEndElement();
  }

  private static void SerializeCutTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p", "cut", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (internalSlideTransition.Cut.ThrowBlack)
      writer.WriteAttributeString("thruBlk", "1");
    writer.WriteEndElement();
  }

  private static void SerializeRandomTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p", "random", "http://schemas.openxmlformats.org/presentationml/2006/main");
    writer.WriteEndElement();
  }

  private static void SerializeBlindsTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p", "blinds", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (internalSlideTransition.Blinds.TransitionDirection != Direction.None)
      writer.WriteAttributeString("dir", SlideTransitionConstant.GetSlideTransitionDirection(internalSlideTransition.Blinds.TransitionDirection));
    writer.WriteEndElement();
  }

  private static void SerializeCheckerTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p", "checker", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (internalSlideTransition.Checker.TransitionDirection != Direction.None)
      writer.WriteAttributeString("dir", SlideTransitionConstant.GetSlideTransitionDirection(internalSlideTransition.Checker.TransitionDirection));
    writer.WriteEndElement();
  }

  private static void SerializeCircleTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p", "circle", "http://schemas.openxmlformats.org/presentationml/2006/main");
    writer.WriteEndElement();
  }

  private static void SerializeCombTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p", "comb", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (internalSlideTransition.Comb.TransitionDirection != Direction.None)
      writer.WriteAttributeString("dir", SlideTransitionConstant.GetSlideTransitionDirection(internalSlideTransition.Comb.TransitionDirection));
    writer.WriteEndElement();
  }

  private static void SerializeCoverTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p", "cover", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (internalSlideTransition.Cover.TransitionDirection != TransitionEightDirectionType.None)
      writer.WriteAttributeString("dir", SlideTransitionConstant.GetSlideTransitionEightDirectionType(internalSlideTransition.Cover.TransitionDirection));
    writer.WriteEndElement();
  }

  private static void SerializeDiamondTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p", "diamond", "http://schemas.openxmlformats.org/presentationml/2006/main");
    writer.WriteEndElement();
  }

  private static void SerializeDissolveTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p", "dissolve", "http://schemas.openxmlformats.org/presentationml/2006/main");
    writer.WriteEndElement();
  }

  private static void SerializeFadeTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p", "fade", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (internalSlideTransition.Fade.ThrowBlack)
      writer.WriteAttributeString("thruBlk", "1");
    writer.WriteEndElement();
  }

  private static void SerializeNewsFlashTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p", "newsflash", "http://schemas.openxmlformats.org/presentationml/2006/main");
    writer.WriteEndElement();
  }

  private static void SerializePlusTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p", "plus", "http://schemas.openxmlformats.org/presentationml/2006/main");
    writer.WriteEndElement();
  }

  private static void SerializePullTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p", "pull", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (internalSlideTransition.Pull.TransitionDirection != TransitionEightDirectionType.None)
      writer.WriteAttributeString("dir", SlideTransitionConstant.GetSlideTransitionEightDirectionType(internalSlideTransition.Pull.TransitionDirection));
    writer.WriteEndElement();
  }

  private static void SerializePushTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p", "push", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (internalSlideTransition.Push.TransitionDirection != TransitionSideDirectionType.None)
      writer.WriteAttributeString("dir", SlideTransitionConstant.GetSideDirectionTransitionTypes(internalSlideTransition.Push.TransitionDirection));
    writer.WriteEndElement();
  }

  private static void SerializeRandomBarTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p", "randomBar", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (internalSlideTransition.RandomBar.TransitionDirection != Direction.None)
      writer.WriteAttributeString("dir", SlideTransitionConstant.GetSlideTransitionDirection(internalSlideTransition.RandomBar.TransitionDirection));
    writer.WriteEndElement();
  }

  private static void SerializeSplitTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p", "split", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (internalSlideTransition.Split.TransitionDirection != TransitionInOutDirectionType.None)
      writer.WriteAttributeString("dir", SlideTransitionConstant.GetTransitionInOutDirectionType(internalSlideTransition.Split.TransitionDirection));
    if (internalSlideTransition.Split.TransitionOrientation != Direction.None)
      writer.WriteAttributeString("orient", SlideTransitionConstant.GetSlideTransitionDirection(internalSlideTransition.Split.TransitionOrientation));
    writer.WriteEndElement();
  }

  private static void SerializeStripsTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p", "strips", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (internalSlideTransition.Strips.TransitionDirection != TransitionCornerDirectionType.None)
      writer.WriteAttributeString("dir", SlideTransitionConstant.GetCornerDirectionTransitionTypes(internalSlideTransition.Strips.TransitionDirection));
    writer.WriteEndElement();
  }

  private static void SerializeWedgeTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p", "wedge", "http://schemas.openxmlformats.org/presentationml/2006/main");
    writer.WriteEndElement();
  }

  private static void SerializeWheelTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p", "wheel", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (internalSlideTransition.Wheel.Spokes != 0 && internalSlideTransition.Wheel.Spokes <= 4 || internalSlideTransition.Wheel.Spokes == 8)
      writer.WriteAttributeString("spokes", internalSlideTransition.Wheel.Spokes.ToString());
    writer.WriteEndElement();
  }

  private static void SerializeWipeTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p", "wipe", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (internalSlideTransition.Wipe.TransitionDirection != TransitionSideDirectionType.None)
      writer.WriteAttributeString("dir", SlideTransitionConstant.GetSideDirectionTransitionTypes(internalSlideTransition.Wipe.TransitionDirection));
    writer.WriteEndElement();
  }

  private static void SerializeZoomTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p", "zoom", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (internalSlideTransition.Zoom.TransitionDirection != TransitionInOutDirectionType.None)
      writer.WriteAttributeString("dir", SlideTransitionConstant.GetTransitionInOutDirectionType(internalSlideTransition.Zoom.TransitionDirection));
    writer.WriteEndElement();
  }

  private static void SerializeRevealTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p14", "reveal", "http://schemas.microsoft.com/office/powerpoint/2010/main");
    if (internalSlideTransition.Reveal.Direction != TransitionLeftRightDirectionType.None)
      writer.WriteAttributeString("dir", SlideTransitionConstant.GetTransitionLeftRightDirection(internalSlideTransition.Reveal.Direction));
    if (internalSlideTransition.Reveal.ThrowBlack)
      writer.WriteAttributeString("thruBlk", "1");
    writer.WriteEndElement();
  }

  private static void SerializeHoneyCombTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p14", "honeycomb", "http://schemas.microsoft.com/office/powerpoint/2010/main");
    writer.WriteEndElement();
  }

  private static void SerializeFerrisTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p14", "ferris", "http://schemas.microsoft.com/office/powerpoint/2010/main");
    if (internalSlideTransition.Ferris.Direction != TransitionLeftRightDirectionType.None)
      writer.WriteAttributeString("dir", SlideTransitionConstant.GetTransitionLeftRightDirection(internalSlideTransition.Ferris.Direction));
    writer.WriteEndElement();
  }

  private static void SerializeSwitchTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p14", "switch", "http://schemas.microsoft.com/office/powerpoint/2010/main");
    if (internalSlideTransition.Switch.Direction != TransitionLeftRightDirectionType.None)
      writer.WriteAttributeString("dir", SlideTransitionConstant.GetTransitionLeftRightDirection(internalSlideTransition.Switch.Direction));
    writer.WriteEndElement();
  }

  private static void SerializeFlipTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p14", "flip", "http://schemas.microsoft.com/office/powerpoint/2010/main");
    if (internalSlideTransition.Flip.Direction != TransitionLeftRightDirectionType.None)
      writer.WriteAttributeString("dir", SlideTransitionConstant.GetTransitionLeftRightDirection(internalSlideTransition.Flip.Direction));
    writer.WriteEndElement();
  }

  private static void SerializeFlashTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p14", "flash", "http://schemas.microsoft.com/office/powerpoint/2010/main");
    writer.WriteEndElement();
  }

  private static void SerializeShredTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p14", "shred", "http://schemas.microsoft.com/office/powerpoint/2010/main");
    if (internalSlideTransition.Shred.Direction != TransitionInOutDirectionType.None)
      writer.WriteAttributeString("dir", SlideTransitionConstant.GetTransitionInOutDirectionType(internalSlideTransition.Shred.Direction));
    if (internalSlideTransition.Shred.Pattern != TransitionShredPattern.None)
      writer.WriteAttributeString("pattern", SlideTransitionConstant.GetTransitionShredPatternValue(internalSlideTransition.Shred.Pattern));
    writer.WriteEndElement();
  }

  private static void SerializePrismTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p14", "prism", "http://schemas.microsoft.com/office/powerpoint/2010/main");
    if (internalSlideTransition.Prism.Direction != TransitionSideDirectionType.None)
      writer.WriteAttributeString("dir", SlideTransitionConstant.GetSideDirectionTransitionTypes(internalSlideTransition.Prism.Direction));
    if (internalSlideTransition.Prism.IsContent)
      writer.WriteAttributeString("isContent", "1");
    if (internalSlideTransition.Prism.IsInverted)
      writer.WriteAttributeString("isInverted", "1");
    writer.WriteEndElement();
  }

  private static void SerializePanTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p14", "pan", "http://schemas.microsoft.com/office/powerpoint/2010/main");
    if (internalSlideTransition.Pan.Direction != TransitionSideDirectionType.None)
      writer.WriteAttributeString("dir", SlideTransitionConstant.GetSideDirectionTransitionTypes(internalSlideTransition.Pan.Direction));
    writer.WriteEndElement();
  }

  private static void SerializePrstTransTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p15", "prstTrans", "http://schemas.microsoft.com/office/powerpoint/2012/main");
    if (!string.IsNullOrEmpty(internalSlideTransition.PrstTrans.PresetTransition))
      writer.WriteAttributeString("prst", internalSlideTransition.PrstTrans.PresetTransition);
    if (internalSlideTransition.PrstTrans.InvX)
      writer.WriteAttributeString("invX", "1");
    writer.WriteEndElement();
  }

  private static void SerializeWheelReverseTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p14", "wheelReverse", "http://schemas.microsoft.com/office/powerpoint/2010/main");
    if (!string.IsNullOrEmpty(internalSlideTransition.WheelReverse.Spokes))
      writer.WriteAttributeString("spokes", internalSlideTransition.WheelReverse.Spokes);
    writer.WriteEndElement();
  }

  private static void SerializeVortexTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p14", "vortex", "http://schemas.microsoft.com/office/powerpoint/2010/main");
    if (internalSlideTransition.Vortex.Direction != TransitionSideDirectionType.None)
      writer.WriteAttributeString("dir", SlideTransitionConstant.GetSideDirectionTransitionTypes(internalSlideTransition.Vortex.Direction));
    writer.WriteEndElement();
  }

  private static void SerializeRippleTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p14", "ripple", "http://schemas.microsoft.com/office/powerpoint/2010/main");
    if (internalSlideTransition.Ripple.Direction != TransitionCornerDirectionType.None)
      writer.WriteAttributeString("dir", SlideTransitionConstant.GetCornerDirectionTransitionTypes(internalSlideTransition.Ripple.Direction));
    writer.WriteEndElement();
  }

  private static void SerializeGlitterTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p14", "glitter", "http://schemas.microsoft.com/office/powerpoint/2010/main");
    if (internalSlideTransition.Glitter.Direction != TransitionSideDirectionType.None)
      writer.WriteAttributeString("dir", SlideTransitionConstant.GetSideDirectionTransitionTypes(internalSlideTransition.Glitter.Direction));
    if (internalSlideTransition.Glitter.Pattern != TransitionPattern.None)
      writer.WriteAttributeString("pattern", SlideTransitionConstant.GetTransitionPatternValue(internalSlideTransition.Glitter.Pattern));
    writer.WriteEndElement();
  }

  private static void SerializeGalleryTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p14", "gallery", "http://schemas.microsoft.com/office/powerpoint/2010/main");
    if (internalSlideTransition.Gallery.Direction != TransitionLeftRightDirectionType.None)
      writer.WriteAttributeString("dir", SlideTransitionConstant.GetTransitionLeftRightDirection(internalSlideTransition.Gallery.Direction));
    writer.WriteEndElement();
  }

  private static void SerializeConveyorTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p14", "conveyor", "http://schemas.microsoft.com/office/powerpoint/2010/main");
    if (internalSlideTransition.Conveyor.Direction != TransitionLeftRightDirectionType.None)
      writer.WriteAttributeString("dir", SlideTransitionConstant.GetTransitionLeftRightDirection(internalSlideTransition.Conveyor.Direction));
    writer.WriteEndElement();
  }

  private static void SerializeDoorsTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p14", "doors", "http://schemas.microsoft.com/office/powerpoint/2010/main");
    if (internalSlideTransition.Door.Direction != Direction.None)
      writer.WriteAttributeString("dir", SlideTransitionConstant.GetSlideTransitionDirection(internalSlideTransition.Door.Direction));
    writer.WriteEndElement();
  }

  private static void SerializeWindowTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p14", "window", "http://schemas.microsoft.com/office/powerpoint/2010/main");
    if (internalSlideTransition.Window.Direction != Direction.None)
      writer.WriteAttributeString("dir", SlideTransitionConstant.GetSlideTransitionDirection(internalSlideTransition.Window.Direction));
    writer.WriteEndElement();
  }

  private static void SerializeWarpTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p14", "warp", "http://schemas.microsoft.com/office/powerpoint/2010/main");
    if (internalSlideTransition.Warp.Direction != TransitionInOutDirectionType.None)
      writer.WriteAttributeString("dir", SlideTransitionConstant.GetTransitionInOutDirectionType(internalSlideTransition.Warp.Direction));
    writer.WriteEndElement();
  }

  private static void SerializeFlyThroughTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p14", "flythrough", "http://schemas.microsoft.com/office/powerpoint/2010/main");
    if (internalSlideTransition.Flythrough.Direction != TransitionInOutDirectionType.None)
      writer.WriteAttributeString("dir", SlideTransitionConstant.GetTransitionInOutDirectionType(internalSlideTransition.Flythrough.Direction));
    if (internalSlideTransition.Flythrough.hasBounce)
      writer.WriteAttributeString("hasBounce", "1");
    writer.WriteEndElement();
  }

  internal static void SerializeMorphTransition(
    XmlWriter writer,
    TransitionInternal internalSlideTransition)
  {
    writer.WriteStartElement("p159", "morph", "http://schemas.microsoft.com/office/powerpoint/2015/09/main");
    writer.WriteAttributeString("option", SlideTransitionConstant.GetTransitionMorphOptions(internalSlideTransition.Morph.Option));
    writer.WriteEndElement();
  }

  internal static void SerializeNotesMasterSlide(XmlWriter xmlWriter, NotesMasterSlide notesMaster)
  {
    xmlWriter.WriteStartDocument(true);
    xmlWriter.WriteStartElement("p", nameof (notesMaster), "http://schemas.openxmlformats.org/presentationml/2006/main");
    xmlWriter.WriteAttributeString("xmlns", "a", (string) null, "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("xmlns", "r", (string) null, "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
    xmlWriter.WriteAttributeString("xmlns", "p", (string) null, "http://schemas.openxmlformats.org/presentationml/2006/main");
    Serializator.SerializeCommonSlide(xmlWriter, (BaseSlide) notesMaster);
    Serializator.SerializeColorMap(xmlWriter, notesMaster.ColorMap);
    if (notesMaster.NotesTextStyle != null)
    {
      xmlWriter.WriteStartElement("p", "notesStyle", "http://schemas.openxmlformats.org/presentationml/2006/main");
      Serializator.SerializeStyles(xmlWriter, notesMaster.NotesTextStyle.StyleList);
      xmlWriter.WriteEndElement();
    }
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndDocument();
  }

  internal static void SerializeNotesSlide(XmlWriter xmlWriter, NotesSlide notesSlide)
  {
    xmlWriter.WriteStartDocument(true);
    xmlWriter.WriteStartElement("p", "notes", "http://schemas.openxmlformats.org/presentationml/2006/main");
    xmlWriter.WriteAttributeString("xmlns", "a", (string) null, "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("xmlns", "r", (string) null, "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
    xmlWriter.WriteAttributeString("xmlns", "p", (string) null, "http://schemas.openxmlformats.org/presentationml/2006/main");
    Serializator.SerializeCommonSlide(xmlWriter, (BaseSlide) notesSlide);
    Serializator.SerializeColorMap(xmlWriter, (BaseSlide) notesSlide);
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndDocument();
    xmlWriter.Flush();
  }

  internal static void SerializeComments(XmlWriter xmlWriter, Comments comments)
  {
    xmlWriter.WriteStartDocument(true);
    xmlWriter.WriteStartElement("p", "cmLst", "http://schemas.openxmlformats.org/presentationml/2006/main");
    xmlWriter.WriteAttributeString("xmlns", "a", (string) null, "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlWriter.WriteAttributeString("xmlns", "r", (string) null, "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
    xmlWriter.WriteAttributeString("xmlns", "p", (string) null, "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (comments.Count != 0)
      Serializator.SerializeComment(xmlWriter, comments);
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndDocument();
    xmlWriter.Flush();
  }

  internal static void SerializeComment(XmlWriter xmlWriter, Comments comments)
  {
    foreach (Comment comment in comments)
    {
      if (comment.Parent == null)
      {
        Serializator.SerializaCommentText(xmlWriter, comment);
      }
      else
      {
        Serializator.SerializaCommentText(xmlWriter, comment);
        xmlWriter.WriteStartElement("p", "extLst", "http://schemas.openxmlformats.org/presentationml/2006/main");
        xmlWriter.WriteStartElement("p", "ext", "http://schemas.openxmlformats.org/presentationml/2006/main");
        xmlWriter.WriteAttributeString("uri", "{C676402C-5697-4E1C-873F-D02D1690AC5C}");
        xmlWriter.WriteStartElement("p15", "threadingInfo", "http://schemas.microsoft.com/office/powerpoint/2012/main");
        xmlWriter.WriteAttributeString("xmlns", "p15", (string) null, "http://schemas.microsoft.com/office/powerpoint/2012/main");
        xmlWriter.WriteAttributeString("timeZoneBias", "-330");
        xmlWriter.WriteStartElement("p15", "parentCm", "http://schemas.microsoft.com/office/powerpoint/2012/main");
        xmlWriter.WriteAttributeString("authorId", ((Comment) comment.Parent).GetCommentAuthor().AuthorId.ToString());
        xmlWriter.WriteAttributeString("idx", ((Comment) comment.Parent).Index.ToString());
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
      }
      xmlWriter.WriteEndElement();
    }
  }

  internal static void SerializaCommentText(XmlWriter xmlWriter, Comment comment)
  {
    xmlWriter.WriteStartElement("p", "cm", "http://schemas.openxmlformats.org/presentationml/2006/main");
    xmlWriter.WriteAttributeString("authorId", comment.GetCommentAuthor().AuthorId.ToString());
    xmlWriter.WriteAttributeString("dt", comment.DateTime.ToString("o").Remove(23));
    xmlWriter.WriteAttributeString("idx", comment.Index.ToString());
    xmlWriter.WriteStartElement("p", "pos", "http://schemas.openxmlformats.org/presentationml/2006/main");
    xmlWriter.WriteAttributeString("x", ((long) comment.Left * 8L).ToString());
    xmlWriter.WriteAttributeString("y", ((long) comment.Top * 8L).ToString());
    xmlWriter.WriteEndElement();
    xmlWriter.WriteStartElement("p", "text", "http://schemas.openxmlformats.org/presentationml/2006/main");
    xmlWriter.WriteString(comment.Text);
    xmlWriter.WriteEndElement();
  }

  internal static void SerializeViewProperties(XmlWriter writer, Syncfusion.Presentation.Presentation presentation)
  {
    writer.WriteStartDocument(true);
    writer.WriteStartElement("p", "viewPr", "http://schemas.openxmlformats.org/presentationml/2006/main");
    writer.WriteAttributeString("xmlns", "a", (string) null, "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("xmlns", "r", (string) null, "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
    writer.WriteAttributeString("xmlns", "p", (string) null, "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (presentation.LastView != null)
      writer.WriteAttributeString("lastView", presentation.LastView);
    Serializator.SerializeNormalViewProperties(writer, presentation);
    DrawingSerializator.WriteRawData(writer, presentation.PreservedElements, "slideViewPr");
    DrawingSerializator.WriteRawData(writer, presentation.PreservedElements, "outlineViewPr");
    DrawingSerializator.WriteRawData(writer, presentation.PreservedElements, "notesTextViewPr");
    DrawingSerializator.WriteRawData(writer, presentation.PreservedElements, "sorterViewPr");
    DrawingSerializator.WriteRawData(writer, presentation.PreservedElements, "notesViewPr");
    DrawingSerializator.WriteRawData(writer, presentation.PreservedElements, "gridSpacing");
    writer.WriteEndElement();
    writer.WriteEndDocument();
  }

  internal static void SerializeCommentAuthors(XmlWriter writer, Syncfusion.Presentation.Presentation presentation)
  {
    CommentAuthors commentAuthors = presentation.CommentAuthors;
    writer.WriteStartDocument(true);
    writer.WriteStartElement("p", "cmAuthorLst", "http://schemas.openxmlformats.org/presentationml/2006/main");
    writer.WriteAttributeString("xmlns", "a", (string) null, "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("xmlns", "r", (string) null, "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
    writer.WriteAttributeString("xmlns", "p", (string) null, "http://schemas.openxmlformats.org/presentationml/2006/main");
    foreach (CommentAuthor commentAuthor in commentAuthors)
    {
      writer.WriteStartElement("p", "cmAuthor", "http://schemas.openxmlformats.org/presentationml/2006/main");
      writer.WriteAttributeString("id", commentAuthor.AuthorId.ToString());
      writer.WriteAttributeString("name", commentAuthor.Name);
      writer.WriteAttributeString("initials", commentAuthor.Initials);
      writer.WriteAttributeString("lastIdx", commentAuthor.LastIndex.ToString());
      writer.WriteAttributeString("clrIdx", commentAuthor.ColorIndex.ToString());
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private static void SerializeNormalViewProperties(XmlWriter writer, Syncfusion.Presentation.Presentation presentation)
  {
    NotesSize notesSize = presentation.NotesSize as NotesSize;
    if (!notesSize.GetLeft().HasValue || !notesSize.GetTop().HasValue)
      return;
    writer.WriteStartElement("p", "normalViewPr", "http://schemas.openxmlformats.org/presentationml/2006/main");
    if (notesSize.HorizontalBarState != SplitterBarState.Restored)
      writer.WriteAttributeString("horzBarState", Helper.ToString(notesSize.HorizontalBarState));
    writer.WriteStartElement("p", "restoredLeft", "http://schemas.openxmlformats.org/presentationml/2006/main");
    writer.WriteAttributeString("sz", Helper.ToString((long) notesSize.GetLeft().Value));
    writer.WriteEndElement();
    writer.WriteStartElement("p", "restoredTop", "http://schemas.openxmlformats.org/presentationml/2006/main");
    writer.WriteAttributeString("sz", Helper.ToString((long) notesSize.GetTop().Value));
    writer.WriteEndElement();
    writer.WriteEndElement();
  }
}
