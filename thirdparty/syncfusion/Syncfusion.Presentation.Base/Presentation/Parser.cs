// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Parser
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Animation;
using Syncfusion.Presentation.Charts;
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
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.Presentation;

internal class Parser
{
  internal const string MainNameSpace = "http://schemas.openxmlformats.org/presentationml/2006/main";
  internal const string PresentationContentPart = "application/vnd.openxmlformats-officedocument.presentationml.presentation.main+xml";
  internal const string SildeShowContentPart = "application/vnd.openxmlformats-officedocument.presentationml.slideshow.main+xml";
  internal const string TemplateContentPart = "application/vnd.openxmlformats-officedocument.presentationml.template.main+xml";
  internal const string AlternateContentDrawingPart = "http://schemas.microsoft.com/office/drawing/2010/main";
  internal const string AlternatContentPowerPointPart = "http://schemas.microsoft.com/office/powerpoint/2010/main";

  internal static FileFormatType DetectFileFormat(XmlReader reader, FileDataHolder dataHolder)
  {
    Parser.CheckContentTypeRootElement(reader);
    if (reader.IsEmptyElement)
      return FileFormatType.Pptx;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      int content = (int) reader.MoveToContent();
      if (reader.NodeType != XmlNodeType.Element)
      {
        reader.Skip();
      }
      else
      {
        if (reader.LocalName == "Override")
        {
          string attribute1 = reader.GetAttribute("PartName");
          string attribute2 = reader.GetAttribute("ContentType");
          if (attribute1 != null && attribute1.ToLower().Equals("/ppt/presentation.xml"))
          {
            switch (attribute2)
            {
              case "application/vnd.openxmlformats-officedocument.presentationml.presentation.main+xml":
                return FileFormatType.Pptx;
              case "application/vnd.ms-Presentation.presentation.macroEnabled.main+xml":
                return FileFormatType.Pptm;
              case "application/vnd.openxmlformats-officedocument.presentationml.template.main+xml":
                return FileFormatType.Potx;
              case "application/vnd.ms-Presentation.template.macroEnabled.main+xml":
                return FileFormatType.Potm;
              case "application/vnd.openxmlformats-officedocument.presentationml.slideshow.main+xml":
                return FileFormatType.Ppsx;
              case "application/vnd.ms-Presentation.slideshow.macroEnabled.main+xml":
                return FileFormatType.Ppsm;
            }
          }
        }
        reader.Skip();
      }
    }
    reader.ReadEndElement();
    reader.Dispose();
    return FileFormatType.Pptx;
  }

  internal static void ParseBackGround(XmlReader reader, Background backGround)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.MoveToAttribute("bwMode"))
      backGround.BlackWhiteMode = Helper.GetBlackWhiteMode(reader.Value);
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
            case "bgPr":
              backGround.Type = BackgroundType.OwnBackground;
              Parser.ParseBackGroundProperties(reader, backGround);
              continue;
            case "bgRef":
              backGround.Type = BackgroundType.Themed;
              Parser.ParseStyleMatrixReference(reader, backGround);
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

  internal static void ParseCommonSilde(XmlReader reader, BaseSlide slide)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.MoveToAttribute("name"))
      slide.Name = reader.Value;
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
            case "bg":
              Parser.ParseBackGround(reader, (Background) slide.Background);
              continue;
            case "spTree":
              Parser.ParseShapeTree(reader, (Shapes) slide.Shapes);
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

  internal static void ParseContentTypes(XmlReader reader, Syncfusion.Presentation.Presentation presentaion)
  {
    Parser.CheckContentTypeRootElement(reader);
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      int content = (int) reader.MoveToContent();
      if (reader.NodeType != XmlNodeType.Element)
        reader.Skip();
      else if (reader.LocalName == "Override")
      {
        string attribute1 = reader.GetAttribute("PartName");
        string attribute2 = reader.GetAttribute("ContentType");
        reader.Skip();
        Parser.AddContentOverride(attribute1, attribute2, presentaion);
        DrawingParser.SkipWhitespaces(reader);
      }
      else if (reader.LocalName == "Default")
      {
        string attribute3 = reader.GetAttribute("Extension");
        if (attribute3 != null)
        {
          string lower = attribute3.ToLower();
          string attribute4 = reader.GetAttribute("ContentType");
          reader.Skip();
          Parser.AddDefaultContentType(lower, attribute4, presentaion);
        }
      }
      else
        reader.Skip();
    }
  }

  internal static void ParseLayoutSlide(XmlReader reader, LayoutSlide layoutSlide)
  {
    if (reader.NamespaceURI != "http://schemas.openxmlformats.org/presentationml/2006/main")
      throw new ArgumentException("Invalid Presentation Header");
    if (!reader.IsEmptyElement)
    {
      string localName = reader.LocalName;
      if (reader.MoveToAttribute("showMasterSp"))
      {
        layoutSlide.IsChanged(true);
        layoutSlide.ShowMasterShape = XmlConvert.ToBoolean(reader.Value);
        reader.MoveToElement();
      }
      if (reader.MoveToAttribute("type"))
        layoutSlide.SetType(Helper.GetSlideLayoutType(reader.Value));
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "cSld":
              layoutSlide.SlidePrsvedElts.Add("cSld", UtilityMethods.ReadSingleNodeIntoStream(reader));
              continue;
            case "clrMapOvr":
              Parser.ParseColorMapOvr(reader, (BaseSlide) layoutSlide);
              continue;
            case "timing":
              AnimParser.ParseAnimation(reader, (BaseSlide) layoutSlide);
              continue;
            case "hf":
              layoutSlide.HeaderFooter = Parser.ParseHeaderFooter(reader);
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          reader.Skip();
      }
      Stream data;
      if (layoutSlide.SlidePrsvedElts.TryGetValue("cSld", out data) && data != null && data.Length > 0L)
      {
        data.Position = 0L;
        XmlReader reader1 = UtilityMethods.CreateReader(data);
        reader1.ReadToFollowing("cSld", "http://schemas.openxmlformats.org/presentationml/2006/main");
        Parser.ParseCommonSilde(reader1, (BaseSlide) layoutSlide);
      }
      reader.Skip();
    }
    reader.Read();
  }

  internal static void ParseMasterSlide(XmlReader reader, MasterSlide masterSlide)
  {
    if (reader.NamespaceURI != "http://schemas.openxmlformats.org/presentationml/2006/main")
      throw new ArgumentException("Invalid Presentation Header");
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
            case "cSld":
              masterSlide.SlidePrsvedElts.Add("cSld", UtilityMethods.ReadSingleNodeIntoStream(reader));
              continue;
            case "sldLayoutIdLst":
              masterSlide.LayoutList = Parser.ParseSlideList(reader, "sldLayoutId", ref masterSlide.Presentation._customId);
              continue;
            case "clrMap":
              masterSlide.ColorMap = Parser.ParseColorMap(reader);
              Stream data;
              if (masterSlide.SlidePrsvedElts.TryGetValue("cSld", out data) && data != null && data.Length > 0L)
              {
                data.Position = 0L;
                XmlReader reader1 = UtilityMethods.CreateReader(data);
                reader1.ReadToFollowing("cSld", "http://schemas.openxmlformats.org/presentationml/2006/main");
                Parser.ParseCommonSilde(reader1, (BaseSlide) masterSlide);
              }
              if (masterSlide.Presentation.PreservedElements != null && masterSlide.Presentation.DefaultTextStyle == null && masterSlide.Presentation.PreservedElements.TryGetValue("defaultTextStyle", out data) && data != null && data.Length > 0L)
              {
                data.Position = 0L;
                XmlReader reader2 = UtilityMethods.CreateReader(data);
                reader2.ReadToFollowing("defaultTextStyle", "http://schemas.openxmlformats.org/presentationml/2006/main");
                masterSlide.Presentation.DefaultTextStyle = new TextBody((BaseSlide) masterSlide);
                RichTextParser.ParseListStyle(reader2, masterSlide.Presentation.DefaultTextStyle);
                continue;
              }
              continue;
            case "timing":
              AnimParser.ParseAnimation(reader, (BaseSlide) masterSlide);
              continue;
            case "hf":
              masterSlide.HeaderFooter = Parser.ParseHeaderFooter(reader);
              continue;
            case "txStyles":
              Parser.ParseTextStyleList(reader, masterSlide);
              continue;
            case "extLst":
              masterSlide.SlidePrsvedElts.Add("extLst", UtilityMethods.ReadSingleNodeIntoStream(reader));
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          reader.Skip();
      }
      Parser.RemoveImageRelation(masterSlide.TopRelation);
    }
    reader.Read();
  }

  private static void ParseTextStyleList(XmlReader reader, MasterSlide masterSlide)
  {
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
          case "titleStyle":
            RichTextParser.ParseListStyle(reader, masterSlide.TitleStyle);
            continue;
          case "bodyStyle":
            RichTextParser.ParseListStyle(reader, masterSlide.BodyStyle);
            continue;
          case "otherStyle":
            RichTextParser.ParseListStyle(reader, masterSlide.OtherStyle);
            continue;
          default:
            continue;
        }
      }
      else
        reader.Skip();
    }
  }

  private static Dictionary<string, bool> ParseHeaderFooter(XmlReader reader)
  {
    Dictionary<string, bool> headerFooter = new Dictionary<string, bool>();
    while (reader.MoveToNextAttribute())
      headerFooter.Add(reader.Name, XmlConvert.ToBoolean(reader.Value));
    return headerFooter;
  }

  private static void ParseColorMapOvr(XmlReader reader, BaseSlide baseSlide)
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
            case "overrideClrMapping":
              baseSlide.ColorMap = Parser.ParseColorMap(reader);
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

  private static Dictionary<string, string> ParseColorMap(XmlReader reader)
  {
    Dictionary<string, string> colorMap = new Dictionary<string, string>();
    while (reader.MoveToNextAttribute())
    {
      if (reader.Name != "xmlns:a")
        colorMap.Add(reader.Name, reader.Value);
    }
    return colorMap;
  }

  internal static void ParseVmlDrawing(XmlReader reader, BaseSlide slide)
  {
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    int num1 = 0;
    int num2 = 0;
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      DrawingParser.SkipWhitespaces(reader);
      if (reader.NodeType == XmlNodeType.Element)
      {
        VmlShape vmlShape = (VmlShape) null;
        switch (reader.LocalName)
        {
          case "shape":
            vmlShape = new VmlShape();
            Parser.ParseVmlShape(reader, vmlShape);
            reader.Skip();
            break;
          case "rect":
            vmlShape = new VmlShape();
            if (reader.MoveToAttribute("id"))
              vmlShape.VmlShapeID = reader.Value;
            if (reader.MoveToAttribute("spid", "urn:schemas-microsoft-com:office:office"))
              vmlShape.VmlShapeID = reader.Value;
            if (reader.MoveToAttribute("style"))
              vmlShape.Style = reader.Value;
            reader.MoveToElement();
            reader.Skip();
            break;
          default:
            reader.Skip();
            break;
        }
        if (vmlShape != null)
        {
          for (int index1 = num1; index1 < slide.Shapes.Count; ++index1)
          {
            bool flag = false;
            if (slide.Shapes[index1].SlideItemType == SlideItemType.OleObject)
            {
              ((OleObject) slide.Shapes[index1]).VmlShape = vmlShape;
              num1 = index1 + 1;
              break;
            }
            if (slide.Shapes[index1].SlideItemType == SlideItemType.GroupShape)
            {
              GroupShape shape = slide.Shapes[index1] as GroupShape;
              for (int index2 = num2; index2 < shape.Shapes.Count; ++index2)
              {
                if (shape.Shapes[index2].SlideItemType == SlideItemType.OleObject)
                {
                  ((OleObject) shape.Shapes[index2]).VmlShape = vmlShape;
                  num2 = index2 + 1;
                  flag = true;
                  break;
                }
              }
              if (flag)
                break;
            }
          }
        }
      }
    }
  }

  internal static VmlShape ParseVmlShape(XmlReader reader, VmlShape vmlShape)
  {
    if (reader.HasAttributes)
    {
      while (reader.MoveToNextAttribute())
      {
        switch (reader.LocalName)
        {
          case "id":
            vmlShape.VmlShapeID = reader.Value;
            continue;
          case "type":
            vmlShape.ShapeType = reader.Value;
            continue;
          case "style":
            vmlShape.Style = reader.Value;
            continue;
          case "spid":
            vmlShape.VmlShapeID = reader.Value;
            continue;
          default:
            continue;
        }
      }
      reader.MoveToElement();
    }
    string localName = reader.LocalName;
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        DrawingParser.SkipWhitespaces(reader);
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "imagedata":
              string attribute = reader.GetAttribute("o:relid");
              if (!string.IsNullOrEmpty(attribute))
                vmlShape.ImageRelationId = attribute;
              reader.Skip();
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
      }
    }
    return vmlShape;
  }

  internal static void ParsePresentation(XmlReader reader, Syncfusion.Presentation.Presentation presentation)
  {
    if (reader.NamespaceURI != "http://schemas.openxmlformats.org/presentationml/2006/main")
      throw new ArgumentException("Invalid Presentation Header");
    if (reader.HasAttributes)
    {
      while (reader.MoveToNextAttribute())
      {
        switch (reader.LocalName)
        {
          case "firstSlideNum":
            presentation.FirstSlideNumber = Helper.ToInt(reader.Value);
            continue;
          case "embedTrueTypeFonts":
            presentation.IsEmbedTrueTypeFont = true;
            continue;
          case "rtl":
            presentation.RightToLeftView = reader.Value == "1";
            continue;
          case "showSpecialPlsOnTitleSld":
            presentation.ShowSpecialPlsOnTitleSld = reader.Value != "0";
            continue;
          default:
            continue;
        }
      }
      reader.MoveToElement();
    }
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
            case "sldMasterIdLst":
              presentation.MasterList = Parser.ParseSlideList(reader, "sldMasterId", ref presentation._customId);
              continue;
            case "notesMasterIdLst":
              presentation.NotesList = Parser.ParseSlideList(reader, "notesMasterId");
              continue;
            case "handoutMasterIdLst":
              presentation.HandoutList = Parser.ParseSlideList(reader, "handoutMasterId");
              continue;
            case "sldIdLst":
              presentation.SlideList = Parser.ParseSlideList(reader, "sldId");
              continue;
            case "sldSz":
              Parser.ParseSlideSize(reader, (SlideSize) presentation.SlideSize);
              continue;
            case "notesSz":
              Parser.ParseNoteSize(reader, presentation.NotesSize as NotesSize);
              continue;
            case "defaultTextStyle":
              presentation.PreservedElements.Add(reader.LocalName, UtilityMethods.ReadSingleNodeIntoStream(reader));
              continue;
            case "embeddedFontLst":
              Parser.ParseEmbeddedFontList(reader, presentation.EmbeddedFontList);
              continue;
            case "extLst":
              Parser.ParseExtensionList(reader, presentation, (Hyperlink) null);
              continue;
            case "modifyVerifier":
              Parser.ParseModifyVerifier(reader, presentation);
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

  internal static void ParseEmbeddedFontList(XmlReader reader, List<EmbeddedFont> embeddedFontList)
  {
    if (reader.IsEmptyElement)
      return;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      DrawingParser.SkipWhitespaces(reader);
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "embeddedFont":
            embeddedFontList.Add(Parser.ParseEmbeddedFont(reader));
            reader.Skip();
            continue;
          default:
            continue;
        }
      }
    }
  }

  internal static EmbeddedFont ParseEmbeddedFont(XmlReader reader)
  {
    EmbeddedFont embeddedFont = new EmbeddedFont();
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        DrawingParser.SkipWhitespaces(reader);
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "font":
              if (reader.MoveToAttribute("typeface"))
                embeddedFont.SetTypeface(reader.Value);
              reader.Skip();
              continue;
            case "regular":
              embeddedFont.SetRegularId(reader.GetAttribute("r:id"));
              reader.Skip();
              continue;
            case "bold":
              embeddedFont.SetBoldId(reader.GetAttribute("r:id"));
              reader.Skip();
              continue;
            case "italic":
              embeddedFont.SetItalicId(reader.GetAttribute("r:id"));
              reader.Skip();
              continue;
            case "boldItalic":
              embeddedFont.SetBoldItalicId(reader.GetAttribute("r:id"));
              reader.Skip();
              continue;
            default:
              continue;
          }
        }
      }
    }
    return embeddedFont;
  }

  internal static void ParseExtensionList(
    XmlReader reader,
    Syncfusion.Presentation.Presentation presentation,
    Hyperlink hyperlink)
  {
    string localName = reader.LocalName;
    if (reader.IsEmptyElement)
    {
      reader.Skip();
    }
    else
    {
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "ext":
              Parser.ParseExtension(reader, presentation, hyperlink);
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
  }

  private static void ParseModifyVerifier(XmlReader reader, Syncfusion.Presentation.Presentation presentation)
  {
    while (reader.MoveToNextAttribute())
      presentation.WriteProtection.Attributes.Add(reader.Name, reader.Value);
  }

  private static void ParseExtension(
    XmlReader reader,
    Syncfusion.Presentation.Presentation presentation,
    Hyperlink hyperlink)
  {
    string localName = reader.LocalName;
    if (reader.IsEmptyElement)
    {
      reader.Skip();
    }
    else
    {
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "sectionLst":
              Parser.ParseSectionList(reader, presentation);
              continue;
            case "hlinkClr":
              switch (reader.GetAttribute("val"))
              {
                case "tx":
                  hyperlink.HyperLinkColorType = HyperLinkColor.Tx;
                  break;
              }
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
  }

  private static void ParseSectionList(XmlReader reader, Syncfusion.Presentation.Presentation presentation)
  {
    string localName = reader.LocalName;
    if (reader.IsEmptyElement)
      return;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "section":
            Section section = Parser.ParseSection(reader, presentation);
            if (section != null)
            {
              ((Sections) presentation.Sections).Add(section);
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

  private static Section ParseSection(XmlReader reader, Syncfusion.Presentation.Presentation presentation)
  {
    string localName = reader.LocalName;
    if (reader.IsEmptyElement)
      return (Section) null;
    Section section = new Section((Sections) presentation.Sections);
    if (reader.MoveToAttribute("name"))
      section.Name = reader.Value;
    if (reader.MoveToAttribute("id"))
      section.ID = reader.Value;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "sldIdLst":
            Parser.ParseSectionSlideIdList(reader, section);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    return section;
  }

  private static void ParseSectionSlideIdList(XmlReader reader, Section section)
  {
    string localName = reader.LocalName;
    if (reader.IsEmptyElement)
    {
      reader.Skip();
    }
    else
    {
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "sldId":
              if (reader.MoveToAttribute("id"))
                section.SlideIdList.Add(reader.Value);
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
  }

  internal static RelationCollection ParseRelationCollection(XmlReader reader)
  {
    if (reader.IsEmptyElement)
      return (RelationCollection) null;
    RelationCollection relationCollection = new RelationCollection();
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      int content = (int) reader.MoveToContent();
      if (reader.NodeType != XmlNodeType.Element)
        reader.Skip();
      else if (reader.LocalName == "Relationship" && reader.NamespaceURI.Equals("http://schemas.openxmlformats.org/package/2006/relationships"))
      {
        Relation relation = Parser.ParseRelation(reader);
        relationCollection.Add(relation.Id, relation);
        DrawingParser.SkipWhitespaces(reader);
      }
      else
        reader.Skip();
    }
    return relationCollection;
  }

  internal static void ParseSlide(XmlReader reader, Slide slide)
  {
    string str = !(reader.NamespaceURI != "http://schemas.openxmlformats.org/presentationml/2006/main") ? reader.LocalName : throw new ArgumentException("Invalid Presentation Header");
    if (reader.MoveToAttribute("show"))
      slide.Visible = XmlConvert.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("showMasterSp"))
      slide.ShowMasterShape = XmlConvert.ToBoolean(reader.Value);
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (!(str == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "cSld":
              Parser.ParseCommonSilde(reader, (BaseSlide) slide);
              continue;
            case "clrMapOvr":
              Parser.ParseColorMapOvr(reader, (BaseSlide) slide);
              continue;
            case "timing":
              AnimParser.ParseAnimation(reader, (BaseSlide) slide);
              continue;
            case "transition":
              slide.IsAlternateContent = false;
              slide.IsSlideTransition = true;
              slide.InternalTransition = new InternalSlideTransition();
              slide.InternalTransition.Transition = new TransitionInternal();
              TransitionEffect transition = Parser.ParseTransition(reader, (BaseSlide) slide, slide.InternalTransition.Transition);
              slide.SlideTransition.TransitionEffectOption = slide.InternalTransition.Transition.SlideShowTransition.TransitionEffectOption;
              slide.SlideTransition.TransitionEffect = transition;
              slide.SlideTransition.Speed = slide.InternalTransition.Transition.SlideShowTransition.Speed;
              slide.SlideTransition.Duration = slide.InternalTransition.Transition.SlideShowTransition.Duration;
              slide.SlideTransition.TimeDelay = slide.InternalTransition.Transition.SlideShowTransition.TimeDelay;
              slide.SlideTransition.TriggerOnTimeDelay = slide.InternalTransition.Transition.SlideShowTransition.TriggerOnTimeDelay;
              slide.SlideTransition.TriggerOnClick = slide.InternalTransition.Transition.SlideShowTransition.TriggerOnClick;
              continue;
            case "AlternateContent":
              slide.IsAlternateContent = true;
              slide.IsSlideTransition = true;
              Parser.ParseSlideTransition(reader, (BaseSlide) slide);
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

  private static void AddContentOverride(
    string partName,
    string contentType,
    Syncfusion.Presentation.Presentation presentation)
  {
    switch (contentType)
    {
      case "application/vnd.openxmlformats-officedocument.presentationml.presentation.main+xml":
        Parser.SetPresentationName(partName, presentation);
        break;
      case "application/vnd.openxmlformats-officedocument.presentationml.slideshow.main+xml":
        Parser.SetPresentationName(partName, presentation);
        break;
      case "application/vnd.openxmlformats-officedocument.presentationml.template.main+xml":
        Parser.SetPresentationName(partName, presentation);
        break;
    }
    if (presentation.OverrideContentType.ContainsKey(partName))
      return;
    presentation.OverrideContentType.Add(partName, contentType);
  }

  private static void AddDefaultContentType(
    string extension,
    string contentType,
    Syncfusion.Presentation.Presentation presentation)
  {
    if (presentation.DefaultContentType.ContainsKey(extension))
      return;
    presentation.DefaultContentType.Add(extension, contentType);
  }

  private static void CheckContentTypeRootElement(XmlReader xmlReader)
  {
    int content = (int) xmlReader.MoveToContent();
    if (xmlReader.NodeType != XmlNodeType.Element || xmlReader.LocalName != "Types")
      throw new ArgumentException("ContentTypes root element eror");
  }

  internal static bool CheckGraphicData(Stream stream, Shapes shapeCollection)
  {
    using (XmlReader reader = XmlReader.Create(stream))
    {
      reader.ReadToFollowing("graphicData", "http://schemas.openxmlformats.org/drawingml/2006/main");
      if (!reader.MoveToAttribute("uri"))
        return false;
      switch (reader.Value)
      {
        case "http://schemas.openxmlformats.org/drawingml/2006/table":
          Table table = new Table(shapeCollection.BaseSlide);
          table.SetSlideItemType(SlideItemType.Table);
          Parser.ParseShapeCommon(stream, (Shape) table);
          reader.ReadToFollowing("tbl", "http://schemas.openxmlformats.org/drawingml/2006/main");
          Parser.ParseTable(reader, table);
          shapeCollection.Add((Shape) table);
          return true;
        case "http://schemas.openxmlformats.org/presentationml/2006/ole":
          OleObject oleObject = new OleObject(shapeCollection.BaseSlide);
          oleObject.SetSlideItemType(SlideItemType.OleObject);
          Parser.ParseShapeCommon(stream, (Shape) oleObject);
          if (!Parser.HasNode(stream, "oleObj"))
            return false;
          shapeCollection.Add((Shape) Parser.ParseOleObject(stream, oleObject));
          if (oleObject.RelationId != null)
          {
            string targetByRelationId = shapeCollection.BaseSlide.TopRelation.GetTargetByRelationId(oleObject.RelationId);
            if (oleObject.LinkType == OleLinkType.Link)
              oleObject.SetLinkPath(targetByRelationId);
            else
              oleObject.SetFileName(Helper.GetFileName(targetByRelationId));
            string str = Helper.FormatPathForZipArchive(targetByRelationId);
            MemoryStream output = new MemoryStream();
            if (shapeCollection.BaseSlide.Presentation.DataHolder.Archive[str] != null)
            {
              Picture.CopyStream(shapeCollection.BaseSlide.Presentation.DataHolder.Archive[str].DataStream, (Stream) output);
              oleObject.OleStream = (Stream) output;
              oleObject.OleExtension = Helper.GetExtension(str);
            }
            ++oleObject.BaseSlide.Presentation.OleObjectCount;
          }
          return true;
        case "http://schemas.openxmlformats.org/drawingml/2006/chart":
        case "http://schemas.microsoft.com/office/drawing/2014/chartex":
          PresentationChart chart = new PresentationChart(shapeCollection.BaseSlide, true);
          if (reader.Value == "http://schemas.microsoft.com/office/drawing/2014/chartex")
            chart.IsChartEx = true;
          chart.SetSlideItemType(SlideItemType.Chart);
          Parser.ParseShapeCommon(stream, (Shape) chart);
          reader.Read();
          chart.RelationId = reader.GetAttribute("r:id");
          shapeCollection.BaseSlide.Presentation.DataHolder.ParseChart(chart);
          shapeCollection.Add((Shape) chart);
          return true;
        case "http://schemas.openxmlformats.org/drawingml/2006/diagram":
          SmartArt smartArt = new SmartArt(shapeCollection.BaseSlide);
          smartArt.SetSlideItemType(SlideItemType.SmartArt);
          Parser.ParseShapeCommon(stream, (Shape) smartArt);
          reader.ReadToFollowing("relIds", "http://schemas.openxmlformats.org/drawingml/2006/diagram");
          DataModel dataModel = smartArt.DataModel;
          dataModel.RelationId = reader.GetAttribute("dm", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
          if (dataModel.RelationId == null)
            return false;
          string itemPathByRelation = smartArt.BaseSlide.TopRelation.GetItemPathByRelation(dataModel.RelationId);
          shapeCollection.BaseSlide.Presentation.DataHolder.ParseSmartArt(dataModel, itemPathByRelation);
          smartArt.LayoutRelationId = reader.GetAttribute("lo", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
          smartArt.QuickStyleRelationId = reader.GetAttribute("qs", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
          smartArt.ColorsRelationId = reader.GetAttribute("cs", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
          shapeCollection.BaseSlide.Presentation.DataHolder.ParseSmartArtDrawing(dataModel, smartArt.BaseSlide.TopRelation.GetTarget("diagrams/drawing" + itemPathByRelation.Remove(0, 16 /*0x10*/)));
          shapeCollection.Add((Shape) smartArt);
          ++shapeCollection.BaseSlide.Presentation.SmartArtCount;
          return true;
      }
    }
    return false;
  }

  private static OleObject ParseOleObject(Stream objectStream, OleObject oleObject)
  {
    bool flag1 = false;
    bool flag2 = false;
    bool flag3 = false;
    XmlReader reader = UtilityMethods.CreateReader(objectStream);
    string localName = reader.LocalName;
    reader.Read();
    while (reader.LocalName != localName)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "AlternateContent":
            flag1 = true;
            break;
          case "Choice":
            if (flag1)
            {
              flag2 = true;
              break;
            }
            break;
          case "Fallback":
            if (flag1)
            {
              flag3 = true;
              break;
            }
            break;
          case "oleObj":
            if (flag3)
            {
              Parser.ParseOleData(reader, oleObject);
              break;
            }
            if (!flag2)
            {
              Parser.ParseOleData(reader, oleObject);
              break;
            }
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
    return oleObject;
  }

  private static void ParseOleData(XmlReader reader, OleObject oleObject)
  {
    if (reader.LocalName != "oleObj")
      throw new XmlException("OLE Object data");
    if (!reader.HasAttributes)
      return;
    while (reader.MoveToNextAttribute())
    {
      switch (reader.LocalName)
      {
        case "spid":
          oleObject.VMLShapeId = reader.Value;
          continue;
        case "name":
          oleObject.Name = reader.Value;
          continue;
        case "imgW":
          oleObject.ImageWidth = Convert.ToInt32(reader.Value);
          continue;
        case "id":
          oleObject.RelationId = reader.Value;
          continue;
        case "imgH":
          oleObject.ImageHeight = Convert.ToInt32(reader.Value);
          continue;
        case "progId":
          oleObject.ProgID = reader.Value;
          continue;
        case "showAsIcon":
          oleObject.DisplayAsIcon = reader.Value == "1";
          continue;
        default:
          continue;
      }
    }
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
          case "embed":
            oleObject.SetLinkType(OleLinkType.Embed);
            reader.Skip();
            break;
          case "link":
            oleObject.SetLinkType(OleLinkType.Link);
            reader.Skip();
            break;
          case "pic":
            Picture picture = new Picture(oleObject.BaseSlide);
            picture.SetSlideItemType(SlideItemType.Picture);
            DrawingParser.ParsePicture(reader, picture);
            oleObject.SetOlePicture(picture);
            break;
        }
      }
      DrawingParser.SkipWhitespaces(reader);
    }
  }

  private static bool HasNode(Stream objectStream, string elementName)
  {
    objectStream.Position = 0L;
    XmlReader reader = UtilityMethods.CreateReader(objectStream);
    while (reader.Read())
    {
      if (reader.LocalName == elementName)
      {
        objectStream.Position = 0L;
        return true;
      }
    }
    objectStream.Position = 0L;
    return false;
  }

  private static void ParseShapeCommon(Stream stream, Shape shape)
  {
    stream.Position = 0L;
    using (XmlReader reader = XmlReader.Create(stream))
    {
      reader.ReadToFollowing("cNvPr", "http://schemas.openxmlformats.org/presentationml/2006/main");
      DrawingParser.ParseNonVisualDrawingProps(reader, shape);
      DrawingParser.SkipWhitespaces(reader);
      reader.ReadToFollowing("nvPr", "http://schemas.openxmlformats.org/presentationml/2006/main");
      DrawingParser.ParseApplicationNonVisualDrawing(reader, shape, true);
      reader.ReadToFollowing("xfrm", "http://schemas.openxmlformats.org/presentationml/2006/main");
      DrawingParser.ParseTransformation(reader, shape, true);
    }
  }

  private static void ParseBackGroundProperties(XmlReader reader, Background backGround)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.MoveToAttribute("shadeToTitle"))
      backGround.ShadeToTitle = XmlConvert.ToBoolean(reader.Value);
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
            case "noFill":
            case "solidFill":
            case "gradFill":
            case "blipFill":
            case "pattFill":
            case "grpFill":
              DrawingParser.ParseFillProperties(reader, backGround.GetFillFormat());
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

  private static void ParseNoteSize(XmlReader reader, NotesSize notesSize)
  {
    if (reader.HasAttributes)
    {
      long cx = 0;
      long cy = 0;
      while (reader.MoveToNextAttribute())
      {
        switch (reader.LocalName)
        {
          case "cx":
            cx = Helper.ToLong(reader.Value);
            continue;
          case "cy":
            cy = Helper.ToLong(reader.Value);
            continue;
          default:
            continue;
        }
      }
      notesSize.SetSize(cx, cy);
    }
    reader.Skip();
  }

  private static Relation ParseRelation(XmlReader reader)
  {
    if (!reader.HasAttributes)
      throw new ArgumentException("Invalid Relationship element");
    Relation relation = new Relation();
    while (reader.MoveToNextAttribute())
    {
      if (!(reader.NamespaceURI != ""))
      {
        if (reader.LocalName == "Id")
          relation.Id = reader.Value;
        else if (reader.LocalName == "Type")
          relation.Type = reader.Value;
        else if (reader.LocalName == "Target")
          relation.Target = reader.Value;
        else if (reader.LocalName == "TargetMode")
          relation.TargetMode = reader.Value;
      }
    }
    reader.MoveToElement();
    reader.Skip();
    return relation;
  }

  private static void ParseShapeTree(XmlReader reader, Shapes shapeCollection)
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
            case "nvGrpSpPr":
              reader.Skip();
              continue;
            case "grpSpPr":
              reader.Skip();
              continue;
            case "sp":
              Shape shape1 = new Shape(ShapeType.Sp, shapeCollection.BaseSlide);
              shape1.SetSlideItemType(SlideItemType.AutoShape);
              DrawingParser.ParseGeneralShape(reader, shape1);
              shapeCollection.Add(shape1);
              continue;
            case "grpSp":
              GroupShape groupShape = new GroupShape("Group1", shapeCollection.BaseSlide);
              groupShape.SetSlideItemType(SlideItemType.GroupShape);
              DrawingParser.ParseGroupShapes(reader, shapeCollection, groupShape);
              shapeCollection.Add((Shape) groupShape);
              continue;
            case "cxnSp":
              Shape shape2 = (Shape) new Connector(ShapeType.CxnSp, shapeCollection.BaseSlide);
              shape2.SetSlideItemType(SlideItemType.ConnectionShape);
              DrawingParser.ParseConnectorShape(reader, shape2);
              shapeCollection.Add(shape2);
              continue;
            case "pic":
              Picture picture = new Picture(shapeCollection.BaseSlide);
              picture.SetSlideItemType(SlideItemType.Picture);
              DrawingParser.ParsePicture(reader, picture);
              shapeCollection.Add((Shape) picture);
              continue;
            case "graphicFrame":
              Stream stream1 = UtilityMethods.ReadSingleNodeIntoStream(reader);
              stream1.Position = 0L;
              if (!Parser.CheckGraphicData(stream1, shapeCollection))
              {
                stream1.Position = 0L;
                Shape shape3 = new Shape(ShapeType.GraphicFrame, shapeCollection.BaseSlide);
                shape3.SetSlideItemType(SlideItemType.Table);
                shape3.DrawingType = DrawingType.None;
                shape3.PreservedElements.Add("graphicFrame", stream1);
                shapeCollection.Add(shape3);
                continue;
              }
              continue;
            case "AlternateContent":
              string str = (string) null;
              Stream data = UtilityMethods.ReadSingleNodeIntoStream(reader);
              data.Position = 0L;
              XmlReader reader1 = UtilityMethods.CreateReader(data, true);
              data.Position = 0L;
              XmlReader reader2 = UtilityMethods.CreateReader(data, true);
              while (reader1.NodeType != XmlNodeType.EndElement || !(reader1.LocalName == "AlternateContent"))
              {
                if (reader1.NodeType == XmlNodeType.Element && (reader1.LocalName == "Choice" || reader1.LocalName == "graphicData" || reader1.LocalName == "AlternateContent") && reader1.AttributeCount > 0)
                {
                  for (int i = 0; i < reader1.AttributeCount; ++i)
                  {
                    if (reader1.GetAttribute(i).ToLower().Contains("chartex"))
                    {
                      str = reader1.GetAttribute(i);
                      break;
                    }
                  }
                }
                reader1.Read();
                if (str != null)
                  break;
              }
              if (str != null)
              {
                while (reader2.LocalName != "graphicFrame")
                  reader2.Read();
                Stream stream2 = UtilityMethods.ReadSingleNodeIntoStream(reader2);
                stream2.Position = 0L;
                if (!Parser.CheckGraphicData(stream2, shapeCollection))
                {
                  stream2.Position = 0L;
                  Shape shape4 = new Shape(ShapeType.GraphicFrame, shapeCollection.BaseSlide);
                  shape4.SetSlideItemType(SlideItemType.Table);
                  shape4.DrawingType = DrawingType.None;
                  shape4.PreservedElements.Add("graphicFrame", stream2);
                  shapeCollection.Add(shape4);
                  continue;
                }
                continue;
              }
              shapeCollection.Add(new Shape(ShapeType.AlternateContent, shapeCollection.BaseSlide)
              {
                PreservedElements = {
                  {
                    "AlternateContent",
                    data
                  }
                }
              });
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else if (!reader.EOF)
          reader.Skip();
        else
          break;
      }
      Parser.RemoveImageRelation(shapeCollection.BaseSlide.TopRelation);
    }
    reader.Read();
  }

  private static Dictionary<string, string> ParseSlideList(
    XmlReader reader,
    string elementName,
    ref uint customId)
  {
    Dictionary<string, string> slideList = new Dictionary<string, string>();
    if (!reader.IsEmptyElement)
    {
      string localName = reader.LocalName;
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          if (reader.LocalName == elementName)
          {
            string attribute1 = reader.GetAttribute("id");
            string attribute2 = reader.GetAttribute("r:id");
            if (attribute2 != null)
            {
              if (attribute1 == null)
              {
                attribute1 = (++customId).ToString();
              }
              else
              {
                uint uint32 = Convert.ToUInt32(attribute1);
                if (uint32 > customId)
                  customId = uint32;
              }
              slideList.Add(attribute2, attribute1);
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
    reader.Read();
    return slideList;
  }

  private static Dictionary<string, string> ParseSlideList(XmlReader reader, string elementName)
  {
    Dictionary<string, string> slideList = new Dictionary<string, string>();
    if (!reader.IsEmptyElement)
    {
      string localName = reader.LocalName;
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          if (reader.LocalName == elementName)
          {
            string attribute1 = reader.GetAttribute("id");
            string attribute2 = reader.GetAttribute("r:id");
            if (attribute1 != null || attribute2 != null)
              slideList.Add(attribute2, attribute1);
            reader.Skip();
          }
          else
            reader.Skip();
        }
        else
          reader.Skip();
      }
    }
    reader.Read();
    return slideList;
  }

  private static void ParseSlideSize(XmlReader reader, SlideSize slideSize)
  {
    if (reader.HasAttributes)
    {
      int cx = 0;
      int cy = 0;
      while (reader.MoveToNextAttribute())
      {
        switch (reader.LocalName)
        {
          case "cx":
            cx = Helper.ToInt(reader.Value);
            continue;
          case "cy":
            cy = Helper.ToInt(reader.Value);
            continue;
          case "type":
            slideSize.Type = Helper.GetSlideType(reader.Value);
            continue;
          default:
            continue;
        }
      }
      slideSize.SetSize(cx, cy);
    }
    reader.Skip();
  }

  private static void ParseStyleMatrixReference(XmlReader reader, Background backGround)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.MoveToAttribute("idx"))
      backGround.Index = Helper.ToInt(reader.Value);
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
          case "srgbClr":
          case "schemeClr":
            Parser.ParseBackGroundScheme(reader, backGround);
            continue;
          case "noFill":
          case "gradFill":
          case "blipFill":
          case "pattFill":
          case "grpFill":
            DrawingParser.ParseFillProperties(reader, backGround.GetFillFormat());
            continue;
          default:
            continue;
        }
      }
      else
        reader.Skip();
    }
  }

  private static void ParseBackGroundScheme(XmlReader reader, Background backGround)
  {
    MasterSlide baseSlide1 = backGround.BaseSlide as MasterSlide;
    LayoutSlide baseSlide2 = backGround.BaseSlide as LayoutSlide;
    if (baseSlide1 == null && baseSlide2 == null)
    {
      MasterSlide master = (MasterSlide) backGround.BaseSlide.Presentation.Masters[0];
      DrawingParser.ParseColorChoice(reader, backGround.GetColorObject(), master);
    }
    else if (baseSlide2 != null)
      DrawingParser.ParseColorChoice(reader, backGround.GetColorObject(), baseSlide2);
    else
      DrawingParser.ParseColorChoice(reader, backGround.GetColorObject(), baseSlide1);
    Fill fillFormat = backGround.GetFillFormat();
    switch (backGround.Index)
    {
      case 1:
      case 1001:
        fillFormat.FillType = fillFormat.BaseSlide.BaseTheme.BgFillFormats[0].FillType;
        backGround.SetFillFormat(fillFormat);
        break;
      case 2:
      case 1002:
        Fill bgFillFormat1 = fillFormat.BaseSlide.BaseTheme.BgFillFormats[1];
        backGround.SetFillFormat(bgFillFormat1);
        break;
      case 3:
      case 1003:
        Fill bgFillFormat2 = fillFormat.BaseSlide.BaseTheme.BgFillFormats[2];
        backGround.SetFillFormat(bgFillFormat2);
        break;
    }
  }

  private static void RemoveImageRelation(RelationCollection collection)
  {
    if (collection == null)
      return;
    foreach (string imageRemove in collection.GetImageRemoveList())
    {
      if (collection.Contains(imageRemove))
        collection.RemoveRelation(imageRemove);
    }
  }

  private static void SetPresentationName(string partName, Syncfusion.Presentation.Presentation presentation)
  {
    if (presentation.PresentationName != null || partName.EndsWith("presentation.xml"))
      return;
    presentation.PresentationName = partName;
    if (partName[0] != '/')
      return;
    presentation.PresentationName = partName.Substring(1);
  }

  internal static void ParseTable(XmlReader reader, Table table)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
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
          case "tblPr":
            if (!reader.IsEmptyElement)
            {
              Parser.ParseTableProperties(reader, table);
              break;
            }
            table.BuiltInStyle = BuiltInTableStyle.NoStyleTableGrid;
            break;
          case "tblGrid":
            Parser.ParseTableGrid(reader, table);
            break;
          case "tr":
            Row row = new Row(table);
            table.AddRow(row);
            if (reader.MoveToAttribute("h"))
              row.SetHeight(Helper.ToLong(reader.Value));
            Parser.ParseTableRow(reader, row);
            break;
        }
      }
      reader.Skip();
    }
  }

  private static void SetCellAttribute(XmlReader reader, Cell cell)
  {
    while (reader.MoveToNextAttribute())
    {
      switch (reader.Name)
      {
        case "gridSpan":
          cell.ColumnSpan = Helper.ToInt(reader.Value);
          continue;
        case "hMerge":
          cell.IsHorizontalMerge = XmlConvert.ToBoolean(reader.Value);
          continue;
        case "vMerge":
          cell.IsVerticalMerge = XmlConvert.ToBoolean(reader.Value);
          continue;
        case "rowSpan":
          cell.SetRowSpan(Helper.ToInt(reader.Value));
          continue;
        case "id":
          cell.Id = reader.Value;
          continue;
        default:
          continue;
      }
    }
  }

  private static void ParseTableCell(XmlReader reader, Cell cell)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
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
          case "txBody":
            RichTextParser.ParseTextBody(reader, cell.TextBody);
            continue;
          case "tcPr":
            Parser.ParseTableCellProperties(reader, cell);
            continue;
          default:
            continue;
        }
      }
      else
        reader.Skip();
    }
  }

  private static void ParseTableCellProperties(XmlReader reader, Cell cell)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    int minValue1 = int.MinValue;
    int minValue2 = int.MinValue;
    int minValue3 = int.MinValue;
    int minValue4 = int.MinValue;
    TextBody textBody = cell.TextBody as TextBody;
    if (reader.HasAttributes)
    {
      while (reader.MoveToNextAttribute())
      {
        switch (reader.LocalName)
        {
          case "marL":
            minValue1 = Helper.ToInt(reader.Value);
            continue;
          case "marR":
            minValue2 = Helper.ToInt(reader.Value);
            continue;
          case "marT":
            minValue3 = Helper.ToInt(reader.Value);
            continue;
          case "marB":
            minValue4 = Helper.ToInt(reader.Value);
            continue;
          case "vert":
            textBody.SetTextDirection(Helper.GetTextDirection(reader.Value));
            continue;
          case "anchor":
            textBody.SetVerticalAlign(Helper.GetVerticalAlignType(reader.Value));
            continue;
          case "anchorCtr":
            textBody.AnchorCenter = XmlConvert.ToBoolean(reader.Value);
            continue;
          case "horzOverflow":
            textBody.TextHorizontalOverflow = Helper.GetTextOverflowType(reader.Value);
            continue;
          default:
            continue;
        }
      }
      reader.MoveToElement();
    }
    textBody.SetMargin(minValue1, minValue3, minValue2, minValue4);
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
            case "lnL":
              DrawingParser.ParseLineProperties(reader, (LineFormat) cell.BorderLeft);
              reader.Read();
              continue;
            case "lnR":
              DrawingParser.ParseLineProperties(reader, (LineFormat) cell.BorderRight);
              reader.Read();
              continue;
            case "lnT":
              DrawingParser.ParseLineProperties(reader, (LineFormat) cell.BorderTop);
              reader.Read();
              continue;
            case "lnB":
              DrawingParser.ParseLineProperties(reader, (LineFormat) cell.BorderBottom);
              reader.Read();
              continue;
            case "lnTlToBr":
              cell.HasDiagonalDownBorder = true;
              DrawingParser.ParseLineProperties(reader, (LineFormat) cell.BorderDiagonalDown);
              reader.Read();
              continue;
            case "lnBlToTr":
              cell.HasDiagonalUpBorder = true;
              DrawingParser.ParseLineProperties(reader, (LineFormat) cell.BorderDiagonalUp);
              reader.Read();
              continue;
            case "cell3D":
              reader.Skip();
              continue;
            default:
              DrawingParser.ParseFillProperties(reader, cell.GetFillFormat());
              cell.IsFillSet = true;
              continue;
          }
        }
        else
          reader.Skip();
      }
    }
    reader.Read();
  }

  private static void ParseTableGrid(XmlReader reader, Table table)
  {
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    int num = 0;
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (!(reader.LocalName == "gridCol"))
          break;
        Column column = new Column(table);
        table.AddColumn(column);
        column.Index = num;
        ++num;
        if (reader.MoveToAttribute("w"))
          column.SetWidth(Helper.ToLong(reader.Value));
      }
      else
        reader.Skip();
    }
  }

  private static void ParseTableProperties(XmlReader reader, Table table)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.MoveToAttribute("rtl"))
      table.RightToLeft = XmlConvert.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("firstRow"))
      table.HasHeaderRow = XmlConvert.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("firstCol"))
      table.HasFirstColumn = XmlConvert.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("lastRow"))
      table.HasTotalRow = XmlConvert.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("lastCol"))
      table.HasLastColumn = XmlConvert.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("bandRow"))
      table.HasBandedRows = XmlConvert.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("bandCol"))
      table.HasBandedColumns = XmlConvert.ToBoolean(reader.Value);
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
          case "tableStyleId":
            string str = reader.ReadElementContentAsString();
            if (!table.GetStyleList().ContainsValue(str))
              table.GetStyleList().Add(BuiltInTableStyle.Custom, str);
            foreach (KeyValuePair<BuiltInTableStyle, string> style in table.GetStyleList())
            {
              if (style.Value == str)
              {
                table.BuiltInStyle = style.Key;
                table.Id = style.Value;
                break;
              }
            }
            if (table.TableStyle == null && table.BuiltInStyle == BuiltInTableStyle.Custom)
            {
              table.BaseSlide.Presentation.DataHolder.ParseTableStyle(table);
              continue;
            }
            continue;
          case "effectLst":
            table.EffectList = new EffectList(table.BaseSlide.Presentation);
            Parser.ParseEffectList(reader, table.EffectList);
            reader.Skip();
            continue;
          default:
            DrawingParser.ParseFillProperties(reader, table.GetFillFormat());
            continue;
        }
      }
      else
        reader.Skip();
    }
  }

  private static void ParseTableRow(XmlReader reader, Row row)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    reader.MoveToElement();
    if (reader.IsEmptyElement)
      return;
    int column = 1;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "tc")
      {
        Cell cell = new Cell(row);
        cell.GetCellIndex(column, row.Table.Rows.IndexOf((IRow) row) + 1);
        ++column;
        row.AddCell(cell);
        Parser.SetCellAttribute(reader, cell);
        reader.MoveToElement();
        Parser.ParseTableCell(reader, cell);
      }
      reader.Skip();
    }
  }

  internal static void ParseHandoutMaster(XmlReader reader, HandoutMaster handoutMaster)
  {
    if (reader.NamespaceURI != "http://schemas.openxmlformats.org/presentationml/2006/main")
      throw new ArgumentException("Invalid Presentation Header");
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
            case "cSld":
              Parser.ParseCommonSilde(reader, (BaseSlide) handoutMaster);
              continue;
            case "clrMap":
              handoutMaster.ColorMap = Parser.ParseColorMap(reader);
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

  internal static void ParseTheme(XmlReader reader, Theme theme)
  {
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
          case "themeElements":
            Parser.ParseThemeElements(reader, theme);
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
    reader.Read();
  }

  private static void ParseThemeElements(XmlReader reader, Theme collection)
  {
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
          case "clrScheme":
            Parser.ParseColorScheme(reader, collection);
            continue;
          case "fontScheme":
            if (reader.MoveToAttribute("name"))
              collection.FontSchemeName = reader.Value;
            reader.MoveToElement();
            Parser.ParseFontScheme(reader, collection);
            continue;
          case "fmtScheme":
            if (reader.MoveToAttribute("name"))
              collection.FormatSchemeName = reader.Value;
            reader.MoveToElement();
            Parser.ParseFormatScheme(reader, collection);
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

  private static void ParseFormatScheme(XmlReader reader, Theme collection)
  {
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
          case "fillStyleLst":
            collection.SetFillFormats(Parser.ParseFillStyleList(reader, collection));
            reader.Skip();
            continue;
          case "lnStyleLst":
            Parser.ParseLineStyleList(reader, collection);
            reader.Skip();
            continue;
          case "bgFillStyleLst":
            Parser.ParseBgFillStyleList(reader, collection);
            reader.Skip();
            continue;
          case "effectStyleLst":
            Parser.ParseEffectStyleList(reader, collection);
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

  private static void ParseEffectStyleList(XmlReader reader, Theme collection)
  {
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (!(reader.LocalName == "effectStyle"))
          break;
        EffectStyle effectStyle = new EffectStyle(collection.BaseSlide.Presentation);
        Parser.ParseEffectProperties(reader, effectStyle);
        collection.EffectStyles.Add(effectStyle);
        reader.Skip();
      }
      else
        reader.Skip();
    }
  }

  private static void ParseEffectProperties(XmlReader reader, EffectStyle effectStyle)
  {
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
          case "effectLst":
            Parser.ParseEffectList(reader, effectStyle.EffectProperties.EffectList);
            reader.Skip();
            continue;
          default:
            effectStyle.PreservedElements.Add(reader.LocalName, UtilityMethods.ReadSingleNodeIntoStream(reader));
            continue;
        }
      }
      else
        reader.Skip();
    }
  }

  internal static void ParseEffectList(XmlReader reader, EffectList effectList)
  {
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
          case "outerShdw":
            Parser.ParseOuterShadowProperties(reader, effectList);
            effectList.OuterShadow = DrawingParser.ParseColorChoice(reader, (MasterSlide) effectList.Presentation.Masters[0]);
            continue;
          default:
            effectList.PreservedElements.Add(reader.LocalName, UtilityMethods.ReadSingleNodeIntoStream(reader));
            continue;
        }
      }
      else
        reader.Skip();
    }
  }

  private static void ParseOuterShadowProperties(XmlReader reader, EffectList effectList)
  {
    if (!reader.HasAttributes)
      return;
    while (reader.MoveToNextAttribute())
    {
      switch (reader.LocalName)
      {
        case "blurRad":
          effectList.BlurRadius = Helper.ToInt(reader.Value);
          continue;
        case "dist":
          effectList.ShadowDistance = Helper.ToInt(reader.Value);
          continue;
        case "dir":
          effectList.ShadowDirection = Helper.ToInt(reader.Value);
          continue;
        case "algn":
          effectList.RectangleAlignType = Helper.GetRectangleAlignType(reader.Value);
          continue;
        case "rotWithShape":
          effectList.RotateWithShape = reader.Value == "1";
          continue;
        case "sx":
          effectList.SetHorizontalScaling(Helper.ToInt(reader.Value));
          continue;
        case "sy":
          effectList.SetVerticalScaling(Helper.ToInt(reader.Value));
          continue;
        case "kx":
          effectList.HorizontalSkew = Helper.ToInt(reader.Value);
          continue;
        case "ky":
          effectList.VerticalSkew = Helper.ToInt(reader.Value);
          continue;
        default:
          continue;
      }
    }
    reader.MoveToElement();
  }

  private static void ParseBgFillStyleList(XmlReader reader, Theme collection)
  {
    collection.SetBgFillFormats(Parser.ParseFillStyleList(reader, collection));
  }

  private static void ParseLineStyleList(XmlReader reader, Theme collection)
  {
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        LineFormat lineFormat = new LineFormat(collection.BaseSlide.Presentation);
        DrawingParser.ParseLineProperties(reader, lineFormat);
        collection.LineFormats.Add(lineFormat);
        reader.Read();
      }
      else
        reader.Skip();
    }
  }

  private static List<Fill> ParseFillStyleList(XmlReader reader, Theme collection)
  {
    List<Fill> fillStyleList = new List<Fill>();
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "solidFill":
            Fill fillFormat1 = new Fill(collection);
            DrawingParser.ParseSolidFill(reader, fillFormat1);
            fillStyleList.Add(fillFormat1);
            continue;
          case "gradFill":
            Fill fillFormat2 = new Fill(collection);
            DrawingParser.ParseGradientFill(reader, fillFormat2);
            fillStyleList.Add(fillFormat2);
            continue;
          case "pattFill":
            Fill fillFormat3 = new Fill(collection);
            DrawingParser.ParsePatternFill(reader, fillFormat3);
            fillStyleList.Add(fillFormat3);
            continue;
          case "blipFill":
            Fill fillFormat4 = new Fill(collection);
            DrawingParser.ParseBlipFill(reader, fillFormat4);
            fillStyleList.Add(fillFormat4);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    return fillStyleList;
  }

  private static void ParseColorScheme(XmlReader reader, Theme collection)
  {
    if (reader.MoveToAttribute("name"))
      collection.Name = reader.Value;
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      string localName = reader.LocalName;
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          Parser.ParseColorSchemeElements(reader, collection);
          reader.Skip();
        }
        else
          reader.Skip();
      }
    }
    reader.Read();
  }

  private static void ParseColorSchemeElements(XmlReader reader, Theme collection)
  {
    string localName1 = reader.LocalName;
    if (localName1 == "dk1" || localName1 == "lt1")
    {
      string localName2 = reader.LocalName;
      reader.Read();
      while (!(localName2 == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "sysClr":
              string str = (string) null;
              collection.ColorCheck.Add(localName1, "sysClr");
              if (reader.MoveToAttribute("val"))
                str = reader.Value == "window" ? "ffffff" : "000000";
              collection.ThemeColors.Add(localName1, str);
              break;
            case "srgbClr":
              if (reader.MoveToAttribute("val"))
              {
                collection.ColorCheck.Add(localName1, "srgbClr");
                collection.ThemeColors.Add(localName1, reader.Value);
                break;
              }
              break;
          }
        }
        reader.Skip();
      }
    }
    else
    {
      string localName3 = reader.LocalName;
      reader.Read();
      while (!(localName3 == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "srgbClr":
              if (reader.MoveToAttribute("val"))
                collection.ThemeColors.Add(localName1, reader.Value);
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
  }

  private static DefaultFonts ParseFont(XmlReader reader)
  {
    DefaultFonts font = new DefaultFonts();
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
            case "latin":
              string attribute1 = reader.GetAttribute("typeface");
              if (!string.IsNullOrEmpty(attribute1))
                font.Latin = attribute1;
              reader.Skip();
              continue;
            case "ea":
              string attribute2 = reader.GetAttribute("typeface");
              if (!string.IsNullOrEmpty(attribute2))
                font.Ea = attribute2;
              reader.Skip();
              continue;
            case "cs":
              string attribute3 = reader.GetAttribute("typeface");
              if (!string.IsNullOrEmpty(attribute3))
                font.Cs = attribute3;
              reader.Skip();
              continue;
            case "font":
              string key = (string) null;
              string str = (string) null;
              if (reader.MoveToAttribute("script"))
                key = reader.Value;
              if (reader.MoveToAttribute("typeface"))
                str = reader.Value;
              font.Font.Add(key, str);
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
    return font;
  }

  private static void ParseFontScheme(XmlReader reader, Theme collection)
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
            case "majorFont":
              collection.MajorFont = Parser.ParseFont(reader);
              continue;
            case "minorFont":
              collection.MinorFont = Parser.ParseFont(reader);
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

  private static void ParseTableStyle(XmlReader reader, Syncfusion.Presentation.Presentation presentation, string id)
  {
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (reader.LocalName == "tblStyle")
        {
          TableStyle tableStyleElements = Parser.ParseTableStyleElements(reader, presentation, id);
          if (tableStyleElements != null && id == tableStyleElements.Id)
          {
            tableStyleElements.IsCustom = false;
            presentation.TableStyleList.Add(id, tableStyleElements);
            break;
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

  internal static TableStyle ParseTableStyleElements(
    XmlReader reader,
    Syncfusion.Presentation.Presentation presentation,
    string tableId)
  {
    TableStyle style = new TableStyle(presentation);
    if (!reader.IsEmptyElement)
    {
      string localName = reader.LocalName;
      if (reader.MoveToAttribute("styleId"))
        style.Id = reader.Value;
      if (!string.IsNullOrEmpty(tableId) && style.Id != tableId)
        return (TableStyle) null;
      if (reader.MoveToAttribute("styleName"))
        style.Name = reader.Value;
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "tblBg":
              Parser.ParseTableBackgroundStyle(reader, style);
              reader.Skip();
              continue;
            case "wholeTbl":
              style.WholeTableStyle = Parser.ParseTablePartStyle(reader, style);
              reader.Skip();
              continue;
            case "band1H":
              style.HorizontalBand1Style = Parser.ParseTablePartStyle(reader, style);
              reader.Skip();
              continue;
            case "band2H":
              style.HorizontalBand2Style = Parser.ParseTablePartStyle(reader, style);
              reader.Skip();
              continue;
            case "band1V":
              style.VerticalBand1Style = Parser.ParseTablePartStyle(reader, style);
              reader.Skip();
              continue;
            case "band2V":
              style.VerticalBand2Style = Parser.ParseTablePartStyle(reader, style);
              reader.Skip();
              continue;
            case "lastCol":
              style.LastColumn = Parser.ParseTablePartStyle(reader, style);
              reader.Skip();
              continue;
            case "firstCol":
              style.FirstColumn = Parser.ParseTablePartStyle(reader, style);
              reader.Skip();
              continue;
            case "lastRow":
              style.LastRow = Parser.ParseTablePartStyle(reader, style);
              reader.Skip();
              continue;
            case "firstRow":
              style.FirstRow = Parser.ParseTablePartStyle(reader, style);
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
    return style;
  }

  private static TablePartStyle ParseTablePartStyle(XmlReader reader, TableStyle style)
  {
    TablePartStyle tablePartStyle = new TablePartStyle(style);
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
            case "tcTxStyle":
              tablePartStyle.TableTextStyle = new TableTextStyle(tablePartStyle);
              Parser.ParseTableTextStyle(reader, tablePartStyle.TableTextStyle);
              reader.Skip();
              continue;
            case "tcStyle":
              tablePartStyle.TableCellStyle = new TableCellStyle(tablePartStyle);
              Parser.ParseTableCellStyle(reader, tablePartStyle.TableCellStyle);
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
    return tablePartStyle;
  }

  private static void ParseTableCellStyle(XmlReader reader, TableCellStyle tableCellStyle)
  {
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
          case "tcBdr":
            Parser.ParseCellBorderStyle(reader, tableCellStyle);
            reader.Skip();
            continue;
          case "fill":
            tableCellStyle.Fill = new Fill(tableCellStyle.Parent.Parent.Presentation);
            do
            {
              reader.Read();
            }
            while (reader.NodeType != XmlNodeType.Element);
            DrawingParser.ParseFillProperties(reader, tableCellStyle.Fill);
            reader.Skip();
            continue;
          case "fillRef":
            Parser.ParseCellReference(reader, tableCellStyle);
            continue;
          default:
            continue;
        }
      }
      else
        reader.Skip();
    }
  }

  private static void ParseCellReference(XmlReader reader, TableCellStyle tableCellStyle)
  {
    if (reader.MoveToAttribute("idx"))
      tableCellStyle.FillRefIndex = reader.Value;
    tableCellStyle.FillRefColor = DrawingParser.ParseColorChoice(reader, (MasterSlide) tableCellStyle.Parent.Parent.Presentation.Masters[0]);
  }

  private static void ParseCellBorderStyle(XmlReader reader, TableCellStyle tableCellStyle)
  {
    if (reader.IsEmptyElement)
      return;
    tableCellStyle.CellBorderStyle = new BorderStyle(tableCellStyle);
    BorderStyle cellBorderStyle = tableCellStyle.CellBorderStyle;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "left":
            cellBorderStyle.Left = Parser.GetLineInstance(cellBorderStyle);
            Parser.ParseBorderStyle(reader, cellBorderStyle.Left);
            reader.Skip();
            continue;
          case "right":
            cellBorderStyle.Right = Parser.GetLineInstance(cellBorderStyle);
            Parser.ParseBorderStyle(reader, cellBorderStyle.Right);
            reader.Skip();
            continue;
          case "top":
            cellBorderStyle.Top = Parser.GetLineInstance(cellBorderStyle);
            Parser.ParseBorderStyle(reader, cellBorderStyle.Top);
            reader.Skip();
            continue;
          case "bottom":
            cellBorderStyle.Bottom = Parser.GetLineInstance(cellBorderStyle);
            Parser.ParseBorderStyle(reader, cellBorderStyle.Bottom);
            reader.Skip();
            continue;
          case "insideH":
            cellBorderStyle.InsideHorzBorder = Parser.GetLineInstance(cellBorderStyle);
            Parser.ParseBorderStyle(reader, cellBorderStyle.InsideHorzBorder);
            reader.Skip();
            continue;
          case "insideV":
            cellBorderStyle.InsideVertBorder = Parser.GetLineInstance(cellBorderStyle);
            Parser.ParseBorderStyle(reader, cellBorderStyle.InsideVertBorder);
            reader.Skip();
            continue;
          case "tl2br":
            reader.Skip();
            continue;
          case "tr2bl":
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

  private static void ParseBorderStyle(XmlReader reader, LineFormat borderLine)
  {
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
          case "ln":
            DrawingParser.ParseLineProperties(reader, borderLine);
            reader.Skip();
            continue;
          case "lnRef":
            Parser.ParseLineReference(reader, borderLine);
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

  private static void ParseLineReference(XmlReader reader, LineFormat borderLine)
  {
    string localName = reader.LocalName;
    if (reader.MoveToAttribute("idx"))
      borderLine.Index = reader.Value;
    reader.Read();
    borderLine.GetFillFormat().SetFillType(FillType.Solid);
    ColorObject colorObject = new ColorObject(true);
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "prstClr":
            DrawingParser.ParsePresetColors(reader, colorObject);
            DrawingParser.ParseColorTransform(reader, colorObject);
            ((SolidFill) borderLine.GetFillFormat().SolidFill).SetColorObject(colorObject);
            continue;
          case "scrgbClr":
            DrawingParser.ParseScRgbColors(reader, colorObject);
            DrawingParser.ParseColorTransform(reader, colorObject);
            ((SolidFill) borderLine.GetFillFormat().SolidFill).SetColorObject(colorObject);
            continue;
          case "schemeClr":
            DrawingParser.ParseScheme(reader, colorObject, (MasterSlide) borderLine.Presentation.Masters[0]);
            ((SolidFill) borderLine.GetFillFormat().SolidFill).SetColorObject(colorObject);
            continue;
          default:
            continue;
        }
      }
      else
        reader.Skip();
    }
  }

  private static LineFormat GetLineInstance(BorderStyle borderStyle)
  {
    return new LineFormat(borderStyle.Parent.Parent.Parent.Presentation);
  }

  private static void ParseTableTextStyle(XmlReader reader, TableTextStyle tableTextStyle)
  {
    string localName = reader.LocalName;
    if (reader.MoveToAttribute("b"))
      tableTextStyle.Bold = Helper.SetOnOff(reader.Value);
    if (reader.MoveToAttribute("i"))
      tableTextStyle.Italic = Helper.SetOnOff(reader.Value);
    reader.MoveToElement();
    if (reader.IsEmptyElement)
      return;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "font":
            Parser.ParseFontCollection(reader, tableTextStyle);
            continue;
          case "fontRef":
            tableTextStyle.HasFontColor = true;
            Parser.ParseFontReference(reader, tableTextStyle);
            if (reader.LocalName == "fontRef")
            {
              reader.Skip();
              continue;
            }
            continue;
          case "schemeClr":
            tableTextStyle.HasFontColor = true;
            ColorObject color = new ColorObject(true);
            DrawingParser.ParseScheme(reader, color, (MasterSlide) tableTextStyle.Parent.Parent.Presentation.Masters[0]);
            tableTextStyle.SetColorObject(color);
            continue;
          case "srgbClr":
            tableTextStyle.HasFontColor = true;
            ColorObject colorObject = new ColorObject(true);
            DrawingParser.ParseSrgbColors(reader, colorObject);
            DrawingParser.ParseColorTransform(reader, colorObject);
            tableTextStyle.SetColorObject(colorObject);
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

  private static void ParseFontReference(XmlReader reader, TableTextStyle tableTextStyle)
  {
    string localName = reader.LocalName;
    if (reader.MoveToAttribute("idx"))
      tableTextStyle.FontRefIndex = reader.Value;
    if (reader.MoveToElement() && reader.IsEmptyElement)
      return;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "prstClr":
            tableTextStyle.TextRefColor = (IColor) new ColorObject(true);
            ColorObject textRefColor1 = (ColorObject) tableTextStyle.TextRefColor;
            DrawingParser.ParsePresetColors(reader, textRefColor1);
            DrawingParser.ParseColorTransform(reader, textRefColor1);
            continue;
          case "scrgbClr":
            tableTextStyle.TextRefColor = (IColor) new ColorObject(true);
            ColorObject textRefColor2 = (ColorObject) tableTextStyle.TextRefColor;
            DrawingParser.ParseScRgbColors(reader, textRefColor2);
            DrawingParser.ParseColorTransform(reader, textRefColor2);
            continue;
          case "schemeClr":
            tableTextStyle.TextRefColor = (IColor) new ColorObject(true);
            DrawingParser.ParseScheme(reader, (ColorObject) tableTextStyle.TextRefColor, (MasterSlide) tableTextStyle.Parent.Parent.Presentation.Masters[0]);
            continue;
          case "srgbClr":
            tableTextStyle.TextRefColor = (IColor) new ColorObject(true);
            DrawingParser.ParseSrgbColors(reader, (ColorObject) tableTextStyle.TextRefColor);
            DrawingParser.ParseColorTransform(reader, (ColorObject) tableTextStyle.TextRefColor);
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

  private static void ParseFontCollection(XmlReader reader, TableTextStyle tableTextStyle)
  {
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
          case "latin":
            if (reader.MoveToAttribute("typeface"))
            {
              tableTextStyle.Latin = reader.Value;
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

  private static void ParseTableBackgroundStyle(XmlReader reader, TableStyle style)
  {
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
          case "fill":
            style.TableBackgroundFill = new Fill(style.Presentation);
            do
            {
              reader.Read();
            }
            while (reader.NodeType != XmlNodeType.Element);
            DrawingParser.ParseFillProperties(reader, style.TableBackgroundFill);
            reader.Skip();
            continue;
          case "fillRef":
            if (reader.MoveToAttribute("idx"))
              style.BgRefIndex = reader.Value;
            style.BgRefColor = DrawingParser.ParseColorChoice(reader, (MasterSlide) style.Presentation.Masters[0]);
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

  internal static void ParseTableStyles(XmlReader reader, Syncfusion.Presentation.Presentation presentation, string id)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.MoveToAttribute("def"))
      presentation.DefaultTableStyle = reader.Value;
    reader.MoveToElement();
    if (reader.IsEmptyElement)
      return;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (reader.LocalName == "tblStyleLst")
          Parser.ParseTableStyle(reader, presentation, id);
        else
          reader.Skip();
      }
      else
        reader.Skip();
    }
  }

  internal static void ParseExtendedProperties(
    XmlReader reader,
    IBuiltInDocumentProperties builtInDocumentProperties,
    ICustomDocumentProperties customDocumentProperties)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.LocalName != "Properties")
      throw new XmlException("Unexpected xml tag " + reader.LocalName);
    if (reader.IsEmptyElement)
      return;
    IBuiltInDocumentProperties documentProperties = builtInDocumentProperties;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "Application":
            documentProperties.ApplicationName = Parser.GetReaderElementValue(reader);
            continue;
          case "Characters":
            documentProperties.CharCount = XmlConvert.ToInt32(Parser.GetReaderElementValue(reader));
            continue;
          case "Company":
            documentProperties.Company = Parser.GetReaderElementValue(reader);
            continue;
          case "Lines":
            documentProperties.LineCount = XmlConvert.ToInt32(Parser.GetReaderElementValue(reader));
            continue;
          case "HeadingPairs":
            ((BuiltInDocumentProperties) documentProperties).HasHeadingPair = true;
            reader.Skip();
            continue;
          case "Manager":
            documentProperties.Manager = Parser.GetReaderElementValue(reader);
            continue;
          case "MMClips":
            documentProperties.MultimediaClipCount = XmlConvert.ToInt32(Parser.GetReaderElementValue(reader));
            continue;
          case "Notes":
            documentProperties.NoteCount = XmlConvert.ToInt32(Parser.GetReaderElementValue(reader));
            continue;
          case "Pages":
            documentProperties.PageCount = XmlConvert.ToInt32(Parser.GetReaderElementValue(reader));
            continue;
          case "Paragraphs":
            documentProperties.ParagraphCount = XmlConvert.ToInt32(Parser.GetReaderElementValue(reader));
            continue;
          case "PresentationFormat":
            documentProperties.PresentationTarget = Parser.GetReaderElementValue(reader);
            continue;
          case "Template":
            documentProperties.Template = Parser.GetReaderElementValue(reader);
            continue;
          case "TotalTime":
            ((BuiltInDocumentProperties) documentProperties).EditTime = TimeSpan.FromMinutes((double) XmlConvert.ToUInt32(Parser.GetReaderElementValue(reader)));
            continue;
          case "Words":
            documentProperties.WordCount = XmlConvert.ToInt32(Parser.GetReaderElementValue(reader));
            continue;
          case "HyperlinkBase":
            ((DocumentPropertyImpl) ((CustomDocumentProperties) customDocumentProperties).Add("_PID_LINKBASE")).Blob = Encoding.Unicode.GetBytes(Parser.GetReaderElementValue(reader) + "\0");
            continue;
          case "AppVersion":
            reader.ReadElementContentAsDouble();
            continue;
          case "HiddenSlides":
            documentProperties.HiddenCount = XmlConvert.ToInt32(Parser.GetReaderElementValue(reader));
            continue;
          case "LinksUpToDate":
            documentProperties.LinksDirty = XmlConvert.ToBoolean(Parser.GetReaderElementValue(reader));
            continue;
          case "ScaleCrop":
            ((BuiltInDocumentProperties) documentProperties).ScaleCrop = XmlConvert.ToBoolean(Parser.GetReaderElementValue(reader));
            continue;
          case "Slides":
            documentProperties.SlideCount = XmlConvert.ToInt32(Parser.GetReaderElementValue(reader));
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    if (((BuiltInDocumentProperties) documentProperties).HasHeadingPair || documentProperties.ApplicationName != null)
      return;
    documentProperties.ApplicationName = "Essential Presentation";
  }

  internal static void ParseDocumentCoreProperties(
    XmlReader reader,
    IBuiltInDocumentProperties builtInDocumentProperties)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.LocalName != "coreProperties")
      throw new XmlException("Unexpected xml tag " + reader.LocalName);
    if (reader.IsEmptyElement)
      return;
    IBuiltInDocumentProperties documentProperties = builtInDocumentProperties;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "category":
            documentProperties.Category = Parser.GetReaderElementValue(reader);
            continue;
          case "created":
            documentProperties.CreationDate = DateTime.Parse(Parser.GetReaderElementValue(reader));
            continue;
          case "creator":
            documentProperties.Author = Parser.GetReaderElementValue(reader);
            continue;
          case "description":
            documentProperties.Comments = Parser.GetReaderElementValue(reader);
            continue;
          case "keywords":
            documentProperties.Keywords = Parser.GetReaderElementValue(reader);
            continue;
          case "lastModifiedBy":
            documentProperties.LastAuthor = Parser.GetReaderElementValue(reader);
            continue;
          case "lastPrinted":
            documentProperties.LastPrinted = DateTime.Parse(Parser.GetReaderElementValue(reader));
            continue;
          case "modified":
            documentProperties.LastSaveDate = DateTime.Parse(Parser.GetReaderElementValue(reader));
            continue;
          case "subject":
            documentProperties.Subject = Parser.GetReaderElementValue(reader);
            continue;
          case "revision":
            documentProperties.RevisionNumber = Parser.GetReaderElementValue(reader);
            continue;
          case "language":
            documentProperties.Language = Parser.GetReaderElementValue(reader);
            continue;
          case "version":
            documentProperties.Version = Parser.GetReaderElementValue(reader);
            continue;
          case "contentStatus":
            documentProperties.ContentStatus = Parser.GetReaderElementValue(reader);
            continue;
          case "title":
            documentProperties.Title = Parser.GetReaderElementValue(reader);
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

  private static string GetReaderElementValue(XmlReader reader)
  {
    if (reader.IsEmptyElement)
    {
      reader.Read();
      return string.Empty;
    }
    reader.Read();
    string empty;
    if (reader.NodeType != XmlNodeType.EndElement)
    {
      empty = reader.Value;
      reader.Skip();
    }
    else
      empty = string.Empty;
    reader.Skip();
    return empty;
  }

  internal static void ParseCustomProperties(
    XmlReader reader,
    ICustomDocumentProperties customDocumentProperties)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.LocalName != "Properties")
      throw new XmlException("Unexpected xml tag " + reader.LocalName);
    if (reader.IsEmptyElement)
      return;
    CustomDocumentProperties customProperties = (CustomDocumentProperties) customDocumentProperties;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (reader.LocalName == "property")
          Parser.ParseCustomProperty(reader, customProperties);
      }
      else
        reader.Skip();
    }
  }

  internal static void ParseCustomProperty(
    XmlReader reader,
    CustomDocumentProperties customProperties)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (customProperties == null)
      throw new ArgumentNullException(nameof (customProperties));
    if (reader.LocalName != "property")
      throw new XmlException("Unexpected xml tag " + reader.LocalName);
    DocumentPropertyImpl documentPropertyImpl = (DocumentPropertyImpl) null;
    if (reader.MoveToAttribute("name"))
      documentPropertyImpl = (DocumentPropertyImpl) customProperties.Add(reader.Value);
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
            case "lpwstr":
              documentPropertyImpl.PropertyType = PropertyType.String;
              documentPropertyImpl.Text = Parser.GetReaderElementValue(reader);
              continue;
            case "lpstr":
              documentPropertyImpl.PropertyType = PropertyType.AsciiString;
              documentPropertyImpl.Text = Parser.GetReaderElementValue(reader);
              continue;
            case "filetime":
              documentPropertyImpl.PropertyType = PropertyType.DateTime;
              documentPropertyImpl.DateTime = DateTime.Parse(Parser.GetReaderElementValue(reader));
              continue;
            case "r8":
              documentPropertyImpl.PropertyType = PropertyType.Double;
              documentPropertyImpl.Double = XmlConvert.ToDouble(Parser.GetReaderElementValue(reader));
              continue;
            case "i4":
              documentPropertyImpl.PropertyType = PropertyType.Int32;
              documentPropertyImpl.Int32 = XmlConvert.ToInt32(Parser.GetReaderElementValue(reader));
              continue;
            case "int":
              documentPropertyImpl.PropertyType = PropertyType.Int;
              documentPropertyImpl.Integer = XmlConvert.ToInt32(Parser.GetReaderElementValue(reader));
              continue;
            case "bool":
              documentPropertyImpl.PropertyType = PropertyType.Bool;
              documentPropertyImpl.Boolean = Helper.ToBoolean(Parser.GetReaderElementValue(reader));
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

  internal static bool CheckTableOrChart(Stream stream, List<Shape> shapes, Shapes shapeCollection)
  {
    using (XmlReader reader = XmlReader.Create(stream))
    {
      reader.ReadToFollowing("graphicData", "http://schemas.openxmlformats.org/drawingml/2006/main");
      if (!reader.MoveToAttribute("uri"))
        return false;
      if (reader.Value == "http://schemas.openxmlformats.org/drawingml/2006/table")
      {
        Table table = new Table(shapeCollection.BaseSlide);
        table.SetSlideItemType(SlideItemType.Table);
        Parser.ParseShapeCommon(stream, (Shape) table);
        reader.ReadToFollowing("tbl", "http://schemas.openxmlformats.org/drawingml/2006/main");
        Parser.ParseTable(reader, table);
        shapes.Add((Shape) table);
        return true;
      }
      if (reader.Value == "http://schemas.openxmlformats.org/drawingml/2006/chart")
      {
        PresentationChart chart = new PresentationChart(shapeCollection.BaseSlide, true);
        chart.SetSlideItemType(SlideItemType.Chart);
        Parser.ParseShapeCommon(stream, (Shape) chart);
        reader.Read();
        chart.RelationId = reader.GetAttribute("r:id");
        shapeCollection.BaseSlide.Presentation.DataHolder.ParseChart(chart);
        shapes.Add((Shape) chart);
        return true;
      }
      if (reader.Value == "http://schemas.openxmlformats.org/presentationml/2006/ole")
      {
        OleObject oleObject = new OleObject(shapeCollection.BaseSlide);
        oleObject.SetSlideItemType(SlideItemType.OleObject);
        Parser.ParseShapeCommon(stream, (Shape) oleObject);
        if (!Parser.HasNode(stream, "oleObj"))
          return false;
        shapes.Add((Shape) Parser.ParseOleObject(stream, oleObject));
        if (oleObject.RelationId != null)
        {
          string targetByRelationId = shapeCollection.BaseSlide.TopRelation.GetTargetByRelationId(oleObject.RelationId);
          if (oleObject.LinkType == OleLinkType.Link)
            oleObject.SetLinkPath(targetByRelationId);
          else
            oleObject.SetFileName(Helper.GetFileName(targetByRelationId));
          string str = Helper.FormatPathForZipArchive(targetByRelationId);
          MemoryStream output = new MemoryStream();
          Picture.CopyStream(shapeCollection.BaseSlide.Presentation.DataHolder.Archive[str].DataStream, (Stream) output);
          oleObject.OleStream = (Stream) output;
          oleObject.OleExtension = Helper.GetExtension(str);
          ++oleObject.BaseSlide.Presentation.OleObjectCount;
        }
        return true;
      }
      if (reader.Value == "http://schemas.openxmlformats.org/drawingml/2006/diagram")
      {
        SmartArt smartArt = new SmartArt(shapeCollection.BaseSlide);
        smartArt.SetSlideItemType(SlideItemType.SmartArt);
        Parser.ParseShapeCommon(stream, (Shape) smartArt);
        reader.ReadToFollowing("relIds", "http://schemas.openxmlformats.org/drawingml/2006/diagram");
        DataModel dataModel = smartArt.DataModel;
        dataModel.RelationId = reader.GetAttribute("dm", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
        if (dataModel.RelationId == null)
          return false;
        string itemPathByRelation = smartArt.BaseSlide.TopRelation.GetItemPathByRelation(dataModel.RelationId);
        shapeCollection.BaseSlide.Presentation.DataHolder.ParseSmartArt(dataModel, itemPathByRelation);
        smartArt.LayoutRelationId = reader.GetAttribute("lo", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
        smartArt.QuickStyleRelationId = reader.GetAttribute("qs", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
        smartArt.ColorsRelationId = reader.GetAttribute("cs", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
        shapeCollection.BaseSlide.Presentation.DataHolder.ParseSmartArtDrawing(dataModel, smartArt.BaseSlide.TopRelation.GetTarget("diagrams/drawing" + itemPathByRelation.Remove(0, 16 /*0x10*/)));
        shapes.Add((Shape) smartArt);
        return true;
      }
    }
    return false;
  }

  internal static void ParseDataModel(XmlReader reader, DataModel dataModel)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
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
          case "ptLst":
            Parser.ParseSmartArtPointList(reader, dataModel);
            reader.Skip();
            continue;
          case "cxnLst":
            Parser.ParseSmartArtConnectionList(reader, dataModel);
            dataModel.AddParentNodesToSmartArt();
            dataModel.AddChildNodes();
            reader.Skip();
            continue;
          case "bg":
            DrawingParser.ParseShapeProperties(reader, (Shape) dataModel.ParentSmartArt);
            continue;
          case "whole":
            if (!reader.IsEmptyElement)
              Parser.ParseSmartArtLineProperties(reader, dataModel);
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

  private static void ParseSmartArtLineProperties(XmlReader reader, DataModel dataModel)
  {
    reader.Read();
    if (reader.NodeType == XmlNodeType.EndElement || reader.IsEmptyElement)
      return;
    if (reader.LocalName != "ln")
      reader.ReadToFollowing("ln", "http://schemas.openxmlformats.org/drawingml/2006/main");
    dataModel.ParentSmartArt.HasLineProperties = true;
    DrawingParser.ParseLineProperties(reader, (LineFormat) dataModel.ParentSmartArt.LineFormat);
    reader.Read();
    DrawingParser.SkipWhitespaces(reader);
  }

  private static void ParseSmartArtConnectionList(XmlReader reader, DataModel dataModel)
  {
    SmartArtConnections connectionCollection = dataModel.ConnectionCollection;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (reader.LocalName == "cxn")
        {
          SmartArtConnection connection = new SmartArtConnection(dataModel);
          Parser.ParseConnectionAttributes(reader, connection);
          connectionCollection.Add(connection.ModelId, connection);
          reader.Read();
        }
        else
          reader.Skip();
      }
      else
        reader.Skip();
    }
  }

  private static void ParseConnectionAttributes(XmlReader reader, SmartArtConnection connection)
  {
    if (reader.AttributeCount == 0)
    {
      reader.Skip();
    }
    else
    {
      while (reader.MoveToNextAttribute())
      {
        switch (reader.LocalName)
        {
          case "modelId":
            connection.ModelId = new Guid(reader.Value);
            continue;
          case "type":
            connection.Type = Helper.GetSmartArtConnectionType(reader.Value);
            continue;
          case "srcId":
            connection.SourceId = new Guid(reader.Value);
            continue;
          case "destId":
            connection.DestinationId = new Guid(reader.Value);
            continue;
          case "srcOrd":
            connection.SourcePosition = (uint) Helper.ToInt(reader.Value);
            continue;
          case "destOrd":
            connection.DestinationPosition = (uint) Helper.ToInt(reader.Value);
            continue;
          case "parTransId":
            connection.ParentTransitionId = new Guid(reader.Value);
            continue;
          case "sibTransId":
            connection.SiblingTransitionId = new Guid(reader.Value);
            continue;
          case "presId":
            connection.PresentationId = reader.Value;
            continue;
          default:
            continue;
        }
      }
    }
  }

  private static void ParseSmartArtPointList(XmlReader reader, DataModel dataModel)
  {
    SmartArtPoints pointCollection = dataModel.PointCollection;
    SmartArtPoint smartArtPoint1 = (SmartArtPoint) null;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (reader.LocalName == "pt")
        {
          SmartArtPoint smartArtPoint2 = new SmartArtPoint(dataModel);
          string attribute1 = reader.GetAttribute("modelId");
          if (attribute1 != null)
            smartArtPoint2.ModelId = new Guid(attribute1);
          smartArtPoint2.Type = Helper.GetSmartArtPointType(reader.GetAttribute("type"));
          string attribute2 = reader.GetAttribute("cxnId");
          if (attribute2 != null)
            smartArtPoint2.ConnectionId = new Guid(attribute2);
          Parser.ParseSmartArtPoint(reader, smartArtPoint2);
          pointCollection.Add(smartArtPoint2.ModelId, smartArtPoint2);
          switch (smartArtPoint2.Type)
          {
            case SmartArtPointType.Node:
              smartArtPoint1 = smartArtPoint2;
              break;
            case SmartArtPointType.ParentTransition:
              smartArtPoint1.ParentTransitionId = smartArtPoint2.ModelId;
              smartArtPoint1.ParentSiblingConnectionId = smartArtPoint2.ConnectionId;
              break;
            case SmartArtPointType.SiblingTransition:
              smartArtPoint1.SiblingTransitionId = smartArtPoint2.ModelId;
              smartArtPoint1.ParentSiblingConnectionId = smartArtPoint2.ConnectionId;
              break;
          }
          reader.Read();
        }
        else
          reader.Skip();
      }
      else
        reader.Skip();
    }
  }

  private static void ParseSmartArtPoint(XmlReader reader, SmartArtPoint smartArtPoint)
  {
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
          case "prSet":
            smartArtPoint.HasPropertySet = true;
            Parser.ParseElementPropertySetAttribute(reader, smartArtPoint);
            continue;
          case "spPr":
            if (!reader.IsEmptyElement)
            {
              DrawingParser.ParseShapeProperties(reader, (Shape) smartArtPoint.PointShapeProperties);
              smartArtPoint.HasShapeProperties = true;
              continue;
            }
            reader.Skip();
            continue;
          case "t":
            RichTextParser.ParseTextBody(reader, (ITextBody) smartArtPoint.TextBody);
            continue;
          case "extLst":
            Parser.ParseExtensionList(reader, smartArtPoint);
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

  internal static void ParseExtensionList(XmlReader reader, SmartArtPoint smartArtPoint)
  {
    string localName = reader.LocalName;
    if (reader.IsEmptyElement)
    {
      reader.Skip();
    }
    else
    {
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "ext":
              Parser.ParseExtension(reader, smartArtPoint);
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
  }

  private static void ParseExtension(XmlReader reader, SmartArtPoint smartArtPoint)
  {
    string localName = reader.LocalName;
    if (reader.IsEmptyElement)
    {
      reader.Skip();
    }
    else
    {
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "cNvPr":
              Parser.ParseNonVisualDrawingProps(reader, smartArtPoint);
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
  }

  internal static void ParseNonVisualDrawingProps(XmlReader reader, SmartArtPoint smartArtPoint)
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
            case "hlinkClick":
              Hyperlink hyperlink = new Hyperlink(smartArtPoint);
              smartArtPoint.SetHyperlink(DrawingParser.ParseHyperlink(reader, hyperlink));
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

  private static void ParseElementPropertySetAttribute(
    XmlReader reader,
    SmartArtPoint smartArtPoint)
  {
    Dictionary<string, string> customAttributes = smartArtPoint.CustomAttributes;
    if (reader.AttributeCount == 0)
    {
      reader.Skip();
    }
    else
    {
      while (reader.MoveToNextAttribute())
      {
        switch (reader.LocalName)
        {
          case "presAssocID":
            smartArtPoint.PresentationElementId = new Guid(reader.Value);
            continue;
          case "presName":
            smartArtPoint.PresentationName = reader.Value;
            continue;
          case "loTypeId":
            string[] strArray = reader.Value.Split('/');
            smartArtPoint.DataModel.SmartArtType = Helper.GetSmartArtType(strArray[strArray.Length - 1]);
            continue;
          case "loCatId":
            smartArtPoint.DataModel.Category = reader.Value;
            continue;
          case "qsTypeId":
            smartArtPoint.DataModel.QuickStyleType = reader.Value;
            continue;
          case "csTypeId":
            smartArtPoint.DataModel.ColorSchemeType = reader.Value;
            continue;
          case "phldrT":
            smartArtPoint.PlaceholderText = reader.Value;
            continue;
          case "phldr":
            smartArtPoint.IsPlaceholder = XmlConvert.ToBoolean(reader.Value);
            continue;
          case "custAng":
            smartArtPoint.CustomAngle = Helper.ToInt(reader.Value);
            continue;
          case "custFlipVert":
            customAttributes.Add(reader.LocalName, reader.Value);
            continue;
          case "custFlipHor":
            customAttributes.Add(reader.LocalName, reader.Value);
            continue;
          case "custSzX":
            customAttributes.Add(reader.LocalName, reader.Value);
            continue;
          case "custSzY":
            customAttributes.Add(reader.LocalName, reader.Value);
            continue;
          case "custScaleX":
            smartArtPoint.CustomScaleX = Helper.ToInt(reader.Value);
            continue;
          case "custScaleY":
            smartArtPoint.CustomScaleY = Helper.ToInt(reader.Value);
            continue;
          case "custT":
            smartArtPoint.IsTextChanged = new bool?(XmlConvert.ToBoolean(reader.Value));
            continue;
          case "custLinFactX":
            customAttributes.Add(reader.LocalName, reader.Value);
            continue;
          case "custLinFactY":
            customAttributes.Add(reader.LocalName, reader.Value);
            continue;
          case "custLinFactNeighborX":
            smartArtPoint.FactorNeighbourX = Helper.ToInt(reader.Value);
            continue;
          case "custLinFactNeighborY":
            smartArtPoint.FactorNeighbourY = Helper.ToInt(reader.Value);
            continue;
          case "custRadScaleRad":
            customAttributes.Add(reader.LocalName, reader.Value);
            continue;
          case "custRadScaleInc":
            customAttributes.Add(reader.LocalName, reader.Value);
            continue;
          default:
            continue;
        }
      }
    }
  }

  internal static void ParseSmartArtDrawing(XmlReader reader, DataModel dataModel)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    reader.MoveToElement();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (reader.LocalName == "spTree")
          Parser.ParseShapeTree(reader, dataModel);
        else
          reader.Skip();
      }
      else
        reader.Skip();
    }
  }

  private static void ParseShapeTree(XmlReader reader, DataModel dataModel)
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
            case "sp":
              SmartArtShape smartArtShape = new SmartArtShape(dataModel.ParentSmartArt);
              Guid key = Guid.Empty;
              if (reader.MoveToAttribute("modelId"))
                key = new Guid(reader.Value);
              smartArtShape.SetSlideItemType(SlideItemType.AutoShape);
              reader.MoveToElement();
              DrawingParser.ParseGeneralShape(reader, (Shape) smartArtShape);
              dataModel.SmartArtShapeCollection.Add(key, smartArtShape);
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          reader.Skip();
      }
      Parser.RemoveImageRelation(dataModel.TopRelation);
    }
    reader.Read();
  }

  internal static void ParseLayoutDefinition(XmlReader reader, SmartArtLayout layoutDefinition)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
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
          case "layoutNode":
            Parser.ParseSmartArtLayoutNode(reader, layoutDefinition.LayoutNode);
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

  private static void ParseSmartArtLayoutNode(
    XmlReader reader,
    SmartArtLayoutNode smartArtLayoutNode)
  {
    Parser.ParseLayoutNodeAttributes(reader, smartArtLayoutNode);
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
          case "alg":
            Parser.ParseSmartArtAlgorithm(reader, smartArtLayoutNode);
            reader.Skip();
            continue;
          case "shape":
            if (reader.MoveToAttribute("type"))
              smartArtLayoutNode.ShapeType = reader.Value;
            reader.Skip();
            continue;
          case "presOf":
            Parser.ParsePresentationOfAttributes(reader, smartArtLayoutNode);
            reader.MoveToElement();
            reader.Skip();
            continue;
          case "constrLst":
            Parser.ParseConstraintList(reader, smartArtLayoutNode);
            reader.Skip();
            continue;
          case "ruleLst":
            Parser.ParseRuleList(reader, smartArtLayoutNode);
            reader.Skip();
            continue;
          case "varLst":
            reader.Skip();
            continue;
          case "forEach":
            Parser.ParseSmartArtLayoutNode(reader, smartArtLayoutNode.ForEach);
            reader.Skip();
            continue;
          case "layoutNode":
            Parser.ParseSmartArtLayoutNode(reader, smartArtLayoutNode.LayoutNode);
            continue;
          case "choose":
            Parser.ParseLayoutNodeChoose(reader, smartArtLayoutNode);
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

  private static void ParseLayoutNodeChoose(XmlReader reader, SmartArtLayoutNode smartArtLayoutNode)
  {
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (reader.LocalName == "if")
        {
          Parser.ParseSmartArtLayoutNode(reader, smartArtLayoutNode.If);
          reader.Skip();
        }
        else if (reader.LocalName == "else")
        {
          Parser.ParseSmartArtLayoutNode(reader, smartArtLayoutNode.Else);
          reader.Skip();
        }
        else
          reader.Skip();
      }
      else
        reader.Skip();
    }
  }

  private static void ParseRuleList(XmlReader reader, SmartArtLayoutNode smartArtLayoutNode)
  {
    if (reader.IsEmptyElement)
      return;
    smartArtLayoutNode.RuleList = new List<SmartArtRule>();
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (reader.LocalName == "rule")
        {
          SmartArtRule smartArtRule = new SmartArtRule(smartArtLayoutNode);
          Parser.ParseConstraintAttributes(reader, smartArtRule.ConstraintAttributes);
          if (reader.MoveToAttribute("factor"))
            smartArtRule.Factor = Helper.ToDouble(reader.Value);
          if (reader.MoveToAttribute("max"))
            smartArtRule.MaxValue = Helper.ToDouble(reader.Value);
          if (reader.MoveToAttribute("val") && reader.Value != "INF")
            smartArtRule.Value = Helper.ToDouble(reader.Value);
        }
        else
          reader.Skip();
      }
      else
        reader.Skip();
    }
  }

  private static void ParseConstraintList(XmlReader reader, SmartArtLayoutNode smartArtLayoutNode)
  {
    if (reader.IsEmptyElement)
      return;
    smartArtLayoutNode.ConstraintList = new List<SmartArtConstraint>();
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (reader.LocalName == "constr")
        {
          SmartArtConstraint constraint = new SmartArtConstraint(smartArtLayoutNode);
          Parser.ParseConstraintAttributes(reader, constraint);
          smartArtLayoutNode.ConstraintList.Add(constraint);
          reader.Skip();
        }
        else
          reader.Skip();
      }
      else
        reader.Skip();
    }
  }

  private static void ParseConstraintAttributes(XmlReader reader, SmartArtConstraint constraint)
  {
    if (reader.AttributeCount == 0)
      return;
    if (reader.MoveToAttribute("type"))
      constraint.Type = Helper.GetSmartArtConstraintType(reader.Value);
    if (reader.MoveToAttribute("for"))
      constraint.ConstraintRelationShip = Helper.GetConstraintRelationShip(reader.Value);
    if (reader.MoveToAttribute("forName"))
      constraint.ForName = reader.Value;
    if (reader.MoveToAttribute("ptType"))
      constraint.PointType = Helper.GetSmartArtPointType(reader.Value);
    reader.MoveToElement();
  }

  private static void ParsePresentationOfAttributes(
    XmlReader reader,
    SmartArtLayoutNode smartArtLayoutNode)
  {
    if (reader.AttributeCount == 0)
      return;
    while (reader.MoveToNextAttribute())
    {
      switch (reader.LocalName)
      {
        case "axis":
          smartArtLayoutNode.AxisType = Helper.GetAxisType(reader.Value);
          continue;
        case "ptType":
          smartArtLayoutNode.PointType = Helper.GetSmartArtPointType(reader.Value);
          continue;
        default:
          continue;
      }
    }
  }

  private static void ParseSmartArtAlgorithm(
    XmlReader reader,
    SmartArtLayoutNode smartArtLayoutNode)
  {
    if (reader.MoveToAttribute("type"))
      smartArtLayoutNode.AlgorithmType = Helper.GetAlgorithmType(reader.Value);
    if (reader.MoveToAttribute("rev"))
      smartArtLayoutNode.AlgorithmRevision = Convert.ToUInt32(reader.Value);
    reader.MoveToElement();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (reader.LocalName == "param")
        {
          SmartArtParameterId key = SmartArtParameterId.TextAlignment;
          object obj = (object) null;
          if (reader.MoveToAttribute("type"))
            key = Helper.GetParameterId(reader.Value);
          if (reader.MoveToAttribute("val"))
          {
            switch (key)
            {
              case SmartArtParameterId.AutoTextRotation:
                obj = Helper.GetParamAutoTextRotation(reader.Value);
                break;
              case SmartArtParameterId.BeginningArrowHeadStyle:
                obj = Helper.GetParamArrowHeadStyle(reader.Value);
                break;
              case SmartArtParameterId.BendPoint:
                obj = Helper.GetParamBendPoint(reader.Value);
                break;
              case SmartArtParameterId.BreakPoint:
                obj = Helper.GetParamBreakPoint(reader.Value);
                break;
              case SmartArtParameterId.ChildAlignment:
                obj = Helper.GetParamChildAlignment(reader.Value);
                break;
              case SmartArtParameterId.ChildDirection:
                obj = Helper.GetParamChildDirection(reader.Value);
                break;
              case SmartArtParameterId.ContinueDirection:
                obj = Helper.GetParamContinueDirection(reader.Value);
                break;
              case SmartArtParameterId.CenterShapeMapping:
                obj = Helper.GetParamCenterShapeMapping(reader.Value);
                break;
              case SmartArtParameterId.ConnectorDimension:
                obj = Helper.GetParamConnectorDimension(reader.Value);
                break;
              case SmartArtParameterId.FallBackScale:
                obj = Helper.GetParamFallBackScale(reader.Value);
                break;
              case SmartArtParameterId.FlowDirection:
                obj = Helper.GetParamFlowDirection(reader.Value);
                break;
              case SmartArtParameterId.GrowDirection:
                obj = Helper.GetParamGrowDirection(reader.Value);
                break;
              case SmartArtParameterId.HierarchyAlignment:
                obj = Helper.GetParamHierarchyAlignment(reader.Value);
                break;
              case SmartArtParameterId.LinearDirection:
                obj = Helper.GetParamLinearDirection(reader.Value);
                break;
              case SmartArtParameterId.NodeHorizontalAlignment:
                obj = Helper.GetParamNodeHorizontalAlignment(reader.Value);
                break;
              case SmartArtParameterId.NodeVerticalAlignment:
                obj = Helper.GetParamNodeVerticalAlignment(reader.Value);
                break;
              case SmartArtParameterId.Offset:
                obj = Helper.GetParamOffset(reader.Value);
                break;
              case SmartArtParameterId.PyramidAccentPosition:
                obj = Helper.GetParamPyramidAccentPosition(reader.Value);
                break;
              case SmartArtParameterId.PyramidAccentTextMargin:
                obj = Helper.GetParamPyramidAccentTextMargin(reader.Value);
                break;
              case SmartArtParameterId.RotationPath:
                obj = Helper.GetParamRotationPath(reader.Value);
                break;
              case SmartArtParameterId.SecondaryChildAlignment:
                obj = Helper.GetParamSecondaryChildAlignment(reader.Value);
                break;
              case SmartArtParameterId.SecondaryLinearDirection:
                obj = Helper.GetParamSecondaryLinearDirection(reader.Value);
                break;
              case SmartArtParameterId.StartElement:
                obj = Helper.GetParamStartElement(reader.Value);
                break;
              case SmartArtParameterId.TextAnchorHorizontal:
                obj = Helper.GetParamTextAnchorHorizontal(reader.Value);
                break;
              case SmartArtParameterId.TextAnchorVertical:
                obj = Helper.GetParamTextAnchorVertcal(reader.Value);
                break;
              case SmartArtParameterId.TextBlockDirection:
                obj = Helper.GetParamTextBlockDirection(reader.Value);
                break;
              case SmartArtParameterId.TextDirection:
                obj = Helper.GetParamTextDirection(reader.Value);
                break;
              case SmartArtParameterId.VerticalAlignment:
                obj = Helper.GetParamVerticalAlignment(reader.Value);
                break;
              default:
                obj = (object) reader.Value;
                break;
            }
          }
          smartArtLayoutNode.ParameterList.Add(key, obj);
        }
        else
          reader.Skip();
      }
      else
        reader.Skip();
    }
  }

  private static void ParseLayoutNodeAttributes(
    XmlReader reader,
    SmartArtLayoutNode smartArtLayoutNode)
  {
    if (reader.AttributeCount == 0)
      return;
    while (reader.MoveToNextAttribute())
    {
      switch (reader.LocalName)
      {
        case "name":
          smartArtLayoutNode.Name = reader.Value;
          continue;
        case "styleLbl":
          smartArtLayoutNode.StyleLabel = reader.Value;
          continue;
        case "chOrder":
          smartArtLayoutNode.ChildOrder = Helper.GetSmartArtChildOrder(reader.Value);
          continue;
        case "moveWith":
          smartArtLayoutNode.MoveWith = reader.Value;
          continue;
        default:
          continue;
      }
    }
  }

  internal static void GetSlideTransitionInternalDOM(
    XmlReader reader,
    TransitionEffect effect,
    TransitionEffectOption subType,
    BaseSlide slide)
  {
    reader.ReadToFollowing("p:" + effect.ToString().ToLower());
    reader.ReadToFollowing("p:" + subType.ToString().ToLower());
    Parser.ParseSlideTransition(reader, slide);
  }

  private static void ParseSlideTransition(XmlReader reader, BaseSlide slide)
  {
    slide.InternalTransition = new InternalSlideTransition();
    slide.InternalTransition.AlternateContent = new AlternateContent();
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
            case "Choice":
              reader.Read();
              slide.InternalTransition.AlternateContent.Choice = new Choice();
              slide.InternalTransition.AlternateContent.Choice.Transition = new TransitionInternal();
              TransitionEffect transition1 = Parser.ParseTransition(reader, slide, slide.InternalTransition.AlternateContent.Choice.Transition);
              slide.SlideTransition.TransitionEffectOption = slide.InternalTransition.AlternateContent.Choice.Transition.SlideShowTransition.TransitionEffectOption;
              slide.SlideTransition.TransitionEffect = transition1;
              slide.SlideTransition.Speed = slide.InternalTransition.AlternateContent.Choice.Transition.SlideShowTransition.Speed;
              slide.SlideTransition.Duration = slide.InternalTransition.AlternateContent.Choice.Transition.SlideShowTransition.Duration;
              slide.SlideTransition.TimeDelay = slide.InternalTransition.AlternateContent.Choice.Transition.SlideShowTransition.TimeDelay;
              slide.SlideTransition.TriggerOnTimeDelay = slide.InternalTransition.AlternateContent.Choice.Transition.SlideShowTransition.TriggerOnTimeDelay;
              slide.SlideTransition.TriggerOnClick = slide.InternalTransition.AlternateContent.Choice.Transition.SlideShowTransition.TriggerOnClick;
              break;
            case "Fallback":
              reader.Read();
              slide.InternalTransition.AlternateContent.FallBack = new FallBack();
              slide.InternalTransition.AlternateContent.FallBack.Transition = new TransitionInternal();
              int transition2 = (int) Parser.ParseTransition(reader, slide, slide.InternalTransition.AlternateContent.FallBack.Transition);
              slide.SlideTransition.Duration = slide.InternalTransition.AlternateContent.Choice.Transition.SlideShowTransition.Duration;
              break;
          }
          reader.Read();
        }
        else
          reader.Skip();
      }
    }
    reader.Read();
  }

  private static TransitionEffect ParseTransition(
    XmlReader reader,
    BaseSlide slide,
    TransitionInternal internalTransition)
  {
    TransitionEffect transition = TransitionEffect.None;
    internalTransition.SlideShowTransition = new SlideShowTransition(slide);
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.MoveToAttribute("advClick"))
      internalTransition.SlideShowTransition.TriggerOnClick = XmlConvert.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("advTm"))
    {
      internalTransition.SlideShowTransition.TimeDelay = reader.ReadContentAsFloat() / 1000f;
      internalTransition.SlideShowTransition.TriggerOnTimeDelay = true;
    }
    if (reader.MoveToAttribute("spd"))
      internalTransition.SlideShowTransition.Speed = SlideTransitionConstant.GetSlideTransitionSpeedValue(reader.Value);
    if (reader.MoveToAttribute("p14:dur"))
      internalTransition.SlideShowTransition.Duration = reader.ReadContentAsFloat() / 1000f;
    if (reader.MoveToAttribute("xmlns:p14"))
      internalTransition.TransitionNameSpace = reader.Value;
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
            case "blinds":
              internalTransition.Blinds = new Blinds();
              transition = Parser.ParseBlindsTransition(reader, internalTransition);
              break;
            case "checker":
              internalTransition.Checker = new Checker();
              transition = Parser.ParseCheckerTransition(reader, internalTransition);
              break;
            case "circle":
              internalTransition.Circle = new Circle();
              internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Circle;
              transition = TransitionEffect.Circle;
              break;
            case "comb":
              internalTransition.Comb = new Comb();
              transition = Parser.ParseCombTransition(reader, internalTransition);
              break;
            case "cover":
              internalTransition.Cover = new Cover();
              transition = Parser.ParseCoverTransition(reader, internalTransition);
              break;
            case "cut":
              internalTransition.Cut = new Cut();
              transition = Parser.ParseCutTransition(reader, internalTransition);
              break;
            case "diamond":
              internalTransition.Diamond = new Diamond();
              internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Diamond;
              transition = TransitionEffect.Diamond;
              break;
            case "dissolve":
              internalTransition.Dissolve = new Dissolve();
              internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Dissolve;
              transition = TransitionEffect.Dissolve;
              break;
            case "fade":
              internalTransition.Fade = new Fade();
              transition = Parser.ParseFadeTransition(reader, internalTransition);
              break;
            case "newsflash":
              internalTransition.NewsFlash = new NewsFlash();
              internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Newsflash;
              transition = TransitionEffect.Newsflash;
              break;
            case "plus":
              internalTransition.Plus = new Plus();
              internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Plus;
              transition = TransitionEffect.Plus;
              break;
            case "pull":
              internalTransition.Pull = new Pull();
              transition = Parser.ParsePullTransition(reader, internalTransition);
              break;
            case "push":
              internalTransition.Push = new Push();
              transition = Parser.ParsePushTransition(reader, internalTransition);
              break;
            case "random":
              internalTransition.Random = new Syncfusion.Presentation.SlideTransition.Internal.Random();
              internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Random;
              transition = TransitionEffect.Random;
              break;
            case "randomBar":
              internalTransition.RandomBar = new RandomBar();
              transition = Parser.ParseRandomBarTransition(reader, internalTransition);
              break;
            case "split":
              internalTransition.Split = new Split();
              transition = Parser.ParseSplitTransition(reader, internalTransition);
              break;
            case "strips":
              internalTransition.Strips = new Strips();
              transition = Parser.ParseStripsTransition(reader, internalTransition);
              break;
            case "wedge":
              internalTransition.Wedge = new Wedge();
              internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Wedge;
              transition = TransitionEffect.Wedge;
              break;
            case "wheel":
              internalTransition.Wheel = new Wheel();
              transition = Parser.ParseWheelTransition(reader, internalTransition);
              break;
            case "wipe":
              internalTransition.Wipe = new Wipe();
              transition = Parser.ParseWipeTransition(reader, internalTransition);
              break;
            case "zoom":
              internalTransition.Zoom = new Zoom();
              transition = Parser.ParseZoomTransition(reader, internalTransition);
              break;
            case "reveal":
              internalTransition.Reveal = new Reveal();
              transition = Parser.ParseRevealTransition(reader, internalTransition);
              break;
            case "honeycomb":
              internalTransition.HoneyComb = new HoneyComb();
              internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Honeycomb;
              transition = TransitionEffect.Honeycomb;
              break;
            case "ferris":
              internalTransition.Ferris = new Ferris();
              transition = Parser.ParseFerrisTransition(reader, internalTransition);
              break;
            case "switch":
              internalTransition.Switch = new Switch();
              transition = Parser.ParseSwitchTransition(reader, internalTransition);
              break;
            case "flip":
              internalTransition.Flip = new Flip();
              transition = Parser.ParseFlipTransition(reader, internalTransition);
              break;
            case "flash":
              internalTransition.Flash = new Flash();
              internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Flashbulb;
              transition = TransitionEffect.Flashbulb;
              break;
            case "shred":
              internalTransition.Shred = new Shred();
              transition = Parser.ParseShredTransition(reader, internalTransition);
              break;
            case "prism":
              internalTransition.Prism = new Prism();
              transition = Parser.ParsePrismTransition(reader, internalTransition);
              break;
            case "pan":
              internalTransition.Pan = new Pan();
              transition = Parser.ParsePanTransition(reader, internalTransition);
              break;
            case "prstTrans":
              internalTransition.PrstTrans = new PrstTrans();
              transition = Parser.ParsePrstTransTransition(reader, internalTransition);
              break;
            case "wheelReverse":
              internalTransition.WheelReverse = new WheelReverse();
              transition = Parser.ParseWheelreverseTransition(reader, internalTransition);
              break;
            case "vortex":
              internalTransition.Vortex = new Vortex();
              transition = Parser.ParseVortexTransition(reader, internalTransition);
              break;
            case "ripple":
              internalTransition.Ripple = new Ripple();
              transition = Parser.ParseRippleTransition(reader, internalTransition);
              break;
            case "glitter":
              internalTransition.Glitter = new Glitter();
              transition = Parser.ParseGlitterTransition(reader, internalTransition);
              break;
            case "gallery":
              internalTransition.Gallery = new Gallery();
              transition = Parser.ParseGalleryTransition(reader, internalTransition);
              break;
            case "conveyor":
              internalTransition.Conveyor = new Conveyor();
              transition = Parser.ParseConveyorTransition(reader, internalTransition);
              break;
            case "doors":
              internalTransition.Door = new Doors();
              transition = Parser.ParseDoorsTransition(reader, internalTransition);
              break;
            case "window":
              internalTransition.Window = new Window();
              transition = Parser.ParseWindowTransition(reader, internalTransition);
              break;
            case "warp":
              internalTransition.Warp = new Warp();
              transition = Parser.ParseWarpTransition(reader, internalTransition);
              break;
            case "flythrough":
              internalTransition.Flythrough = new FlyThrough();
              transition = Parser.ParseFlyThroughTransition(reader, internalTransition);
              break;
            case "morph":
              internalTransition.Morph = new Morph();
              transition = Parser.ParseMorphTransition(reader, internalTransition);
              break;
          }
          reader.Read();
        }
        else
          reader.Skip();
      }
    }
    reader.Read();
    return transition;
  }

  private static TransitionEffect ParseBlindsTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("dir"))
      internalTransition.Blinds.TransitionDirection = SlideTransitionConstant.GetSlideTransitionDirectionValue(reader.Value);
    reader.MoveToElement();
    internalTransition.SlideShowTransition.TransitionEffectOption = internalTransition.Blinds.TransitionDirection == Direction.Horizontal || internalTransition.Blinds.TransitionDirection == Direction.None ? TransitionEffectOption.Horizontal : TransitionEffectOption.Vertical;
    internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Blinds;
    return TransitionEffect.Blinds;
  }

  private static TransitionEffect ParseCheckerTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("dir"))
      internalTransition.Checker.TransitionDirection = SlideTransitionConstant.GetSlideTransitionDirectionValue(reader.Value);
    reader.MoveToElement();
    internalTransition.SlideShowTransition.TransitionEffectOption = internalTransition.Checker.TransitionDirection == Direction.Horizontal || internalTransition.Checker.TransitionDirection == Direction.None ? TransitionEffectOption.Across : TransitionEffectOption.Down;
    internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Checkerboard;
    return TransitionEffect.Checkerboard;
  }

  private static TransitionEffect ParseCombTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("dir"))
      internalTransition.Comb.TransitionDirection = SlideTransitionConstant.GetSlideTransitionDirectionValue(reader.Value);
    reader.MoveToElement();
    internalTransition.SlideShowTransition.TransitionEffectOption = internalTransition.Comb.TransitionDirection == Direction.Horizontal || internalTransition.Comb.TransitionDirection == Direction.None ? TransitionEffectOption.Horizontal : TransitionEffectOption.Vertical;
    internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Comb;
    return TransitionEffect.Comb;
  }

  private static TransitionEffect ParseCoverTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("dir"))
      internalTransition.Cover.TransitionDirection = SlideTransitionConstant.GetSlideTransitionEightDirectionValue(reader.Value);
    reader.MoveToElement();
    Parser.GetSlideTransitionCoverEffectType(reader, internalTransition.Cover.TransitionDirection, internalTransition);
    internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Cover;
    return TransitionEffect.Cover;
  }

  private static TransitionEffect ParseCutTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("thruBlk"))
      internalTransition.Cut.ThrowBlack = XmlConvert.ToBoolean(reader.Value);
    reader.MoveToElement();
    if (internalTransition.Cut.ThrowBlack)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.ThroughBlack;
    internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Cut;
    return TransitionEffect.Cut;
  }

  private static TransitionEffect ParseFadeTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("thruBlk"))
      internalTransition.Fade.ThrowBlack = XmlConvert.ToBoolean(reader.Value);
    reader.MoveToElement();
    internalTransition.SlideShowTransition.TransitionEffectOption = internalTransition.Fade.ThrowBlack ? TransitionEffectOption.ThroughBlack : TransitionEffectOption.Smoothly;
    internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.FadeAway;
    return TransitionEffect.FadeAway;
  }

  private static TransitionEffect ParsePullTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("dir"))
      internalTransition.Pull.TransitionDirection = SlideTransitionConstant.GetSlideTransitionEightDirectionValue(reader.Value);
    reader.MoveToElement();
    Parser.GetSlideTransitionPullEffectType(reader, internalTransition.Pull.TransitionDirection, internalTransition);
    internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Uncover;
    return TransitionEffect.Uncover;
  }

  private static TransitionEffect ParsePushTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("dir"))
      internalTransition.Push.TransitionDirection = SlideTransitionConstant.GetSideDirectionTransitionValue(reader.Value);
    if (internalTransition.Push.TransitionDirection == TransitionSideDirectionType.Down)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Down;
    else if (internalTransition.Push.TransitionDirection == TransitionSideDirectionType.Left || internalTransition.Push.TransitionDirection == TransitionSideDirectionType.None)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Left;
    else if (internalTransition.Push.TransitionDirection == TransitionSideDirectionType.Right)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Right;
    else if (internalTransition.Push.TransitionDirection == TransitionSideDirectionType.Up)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Up;
    internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Push;
    return TransitionEffect.Push;
  }

  private static TransitionEffect ParseRandomBarTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("dir"))
      internalTransition.RandomBar.TransitionDirection = SlideTransitionConstant.GetSlideTransitionDirectionValue(reader.Value);
    reader.MoveToElement();
    internalTransition.SlideShowTransition.TransitionEffectOption = internalTransition.RandomBar.TransitionDirection == Direction.Horizontal || internalTransition.RandomBar.TransitionDirection == Direction.None ? TransitionEffectOption.Horizontal : TransitionEffectOption.Vertical;
    internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.RandomBars;
    return TransitionEffect.RandomBars;
  }

  private static TransitionEffect ParseSplitTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("dir"))
      internalTransition.Split.TransitionDirection = SlideTransitionConstant.GetTransitionInOutDirectionValue(reader.Value);
    if (reader.MoveToAttribute("orient"))
      internalTransition.Split.TransitionOrientation = SlideTransitionConstant.GetSlideTransitionDirectionValue(reader.Value);
    reader.MoveToElement();
    if ((internalTransition.Split.TransitionOrientation == Direction.Horizontal || internalTransition.Split.TransitionOrientation == Direction.None) && internalTransition.Split.TransitionDirection == TransitionInOutDirectionType.In)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.HorizontalIn;
    else if (internalTransition.Split.TransitionOrientation == Direction.Horizontal && internalTransition.Split.TransitionDirection == TransitionInOutDirectionType.Out || internalTransition.Split.TransitionOrientation == Direction.None && internalTransition.Split.TransitionDirection == TransitionInOutDirectionType.None)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.HorizontalOut;
    else if (internalTransition.Split.TransitionOrientation == Direction.Vertical && internalTransition.Split.TransitionDirection == TransitionInOutDirectionType.In)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.VerticalIn;
    else if (internalTransition.Split.TransitionOrientation == Direction.Vertical && (internalTransition.Split.TransitionDirection == TransitionInOutDirectionType.Out || internalTransition.Split.TransitionDirection == TransitionInOutDirectionType.None))
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.VerticalOut;
    internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Split;
    return TransitionEffect.Split;
  }

  private static TransitionEffect ParseStripsTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("dir"))
      internalTransition.Strips.TransitionDirection = SlideTransitionConstant.GetCornerDirectionTransitionValue(reader.Value);
    reader.MoveToElement();
    if (internalTransition.Strips.TransitionDirection == TransitionCornerDirectionType.LeftDown)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.LeftDown;
    else if (internalTransition.Strips.TransitionDirection == TransitionCornerDirectionType.LeftUp || internalTransition.Strips.TransitionDirection == TransitionCornerDirectionType.None)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.LeftUp;
    else if (internalTransition.Strips.TransitionDirection == TransitionCornerDirectionType.RightDown)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.RightDown;
    else if (internalTransition.Strips.TransitionDirection == TransitionCornerDirectionType.RightUp)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.RightUp;
    internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Strips;
    return TransitionEffect.Strips;
  }

  private static TransitionEffect ParseWheelTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("spokes"))
      internalTransition.Wheel.Spokes = XmlConvert.ToInt32(reader.Value);
    reader.MoveToElement();
    if (internalTransition.Wheel.Spokes == 1)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Spoke1;
    else if (internalTransition.Wheel.Spokes == 2)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Spokes2;
    else if (internalTransition.Wheel.Spokes == 3)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Spokes3;
    else if (internalTransition.Wheel.Spokes == 4 || internalTransition.Wheel.Spokes == 0)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Spokes4;
    else if (internalTransition.Wheel.Spokes == 8)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Spokes8;
    internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Wheel;
    return TransitionEffect.Wheel;
  }

  private static TransitionEffect ParseWipeTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("dir"))
      internalTransition.Wipe.TransitionDirection = SlideTransitionConstant.GetSideDirectionTransitionValue(reader.Value);
    reader.MoveToElement();
    if (internalTransition.Wipe.TransitionDirection == TransitionSideDirectionType.Down)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Down;
    else if (internalTransition.Wipe.TransitionDirection == TransitionSideDirectionType.Left || internalTransition.Wipe.TransitionDirection == TransitionSideDirectionType.None)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Left;
    else if (internalTransition.Wipe.TransitionDirection == TransitionSideDirectionType.Right)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Right;
    else if (internalTransition.Wipe.TransitionDirection == TransitionSideDirectionType.Up)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Up;
    internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Wipe;
    return TransitionEffect.Wipe;
  }

  private static TransitionEffect ParseZoomTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("dir"))
      internalTransition.Zoom.TransitionDirection = SlideTransitionConstant.GetTransitionInOutDirectionValue(reader.Value);
    reader.MoveToElement();
    internalTransition.SlideShowTransition.TransitionEffectOption = internalTransition.Zoom.TransitionDirection != TransitionInOutDirectionType.In ? TransitionEffectOption.Out : TransitionEffectOption.In;
    internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Zoom;
    return TransitionEffect.Zoom;
  }

  private static TransitionEffect ParseRevealTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("dir"))
      internalTransition.Reveal.Direction = SlideTransitionConstant.GetTransitionLeftRightDirectionValue(reader.Value);
    if (reader.MoveToAttribute("thruBlk"))
      internalTransition.Reveal.ThrowBlack = XmlConvert.ToBoolean(reader.Value);
    reader.MoveToElement();
    if (internalTransition.Reveal.Direction == TransitionLeftRightDirectionType.None && internalTransition.Reveal.ThrowBlack)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.BlackLeft;
    else if (internalTransition.Reveal.Direction == TransitionLeftRightDirectionType.Right && internalTransition.Reveal.ThrowBlack)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.BlackRight;
    else if ((internalTransition.Reveal.Direction == TransitionLeftRightDirectionType.None || internalTransition.Reveal.Direction == TransitionLeftRightDirectionType.Left) && !internalTransition.Reveal.ThrowBlack)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.SmoothLeft;
    else if (internalTransition.Reveal.Direction == TransitionLeftRightDirectionType.Right && !internalTransition.Reveal.ThrowBlack)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.SmoothRight;
    internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Reveal;
    return TransitionEffect.Reveal;
  }

  private static TransitionEffect ParseFerrisTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("dir"))
      internalTransition.Ferris.Direction = SlideTransitionConstant.GetTransitionLeftRightDirectionValue(reader.Value);
    reader.MoveToElement();
    internalTransition.SlideShowTransition.TransitionEffectOption = internalTransition.Ferris.Direction != TransitionLeftRightDirectionType.Left ? TransitionEffectOption.Right : TransitionEffectOption.Left;
    internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.FerrisWheel;
    return TransitionEffect.FerrisWheel;
  }

  private static TransitionEffect ParseSwitchTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("dir"))
      internalTransition.Switch.Direction = SlideTransitionConstant.GetTransitionLeftRightDirectionValue(reader.Value);
    reader.MoveToElement();
    internalTransition.SlideShowTransition.TransitionEffectOption = internalTransition.Switch.Direction != TransitionLeftRightDirectionType.Left ? TransitionEffectOption.Right : TransitionEffectOption.Left;
    internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Switch;
    return TransitionEffect.Switch;
  }

  private static TransitionEffect ParseFlipTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("dir"))
      internalTransition.Flip.Direction = SlideTransitionConstant.GetTransitionLeftRightDirectionValue(reader.Value);
    reader.MoveToElement();
    internalTransition.SlideShowTransition.TransitionEffectOption = internalTransition.Flip.Direction != TransitionLeftRightDirectionType.Left ? TransitionEffectOption.Right : TransitionEffectOption.Left;
    internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Flip;
    return TransitionEffect.Flip;
  }

  private static TransitionEffect ParseShredTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("dir"))
      internalTransition.Shred.Direction = SlideTransitionConstant.GetTransitionInOutDirectionValue(reader.Value);
    if (reader.MoveToAttribute("pattern"))
      internalTransition.Shred.Pattern = SlideTransitionConstant.GetTransitionShredPattern(reader.Value);
    reader.MoveToElement();
    internalTransition.SlideShowTransition.TransitionEffectOption = internalTransition.Shred.Direction != TransitionInOutDirectionType.In && internalTransition.Shred.Direction != TransitionInOutDirectionType.None || internalTransition.Shred.Pattern != TransitionShredPattern.Rectangle ? (internalTransition.Shred.Direction == TransitionInOutDirectionType.In && internalTransition.Shred.Pattern == TransitionShredPattern.Strip || internalTransition.Shred.Direction == TransitionInOutDirectionType.None && internalTransition.Shred.Pattern == TransitionShredPattern.None ? TransitionEffectOption.StripsIn : (internalTransition.Shred.Direction != TransitionInOutDirectionType.Out || internalTransition.Shred.Pattern != TransitionShredPattern.Rectangle ? TransitionEffectOption.StripsOut : TransitionEffectOption.RectangleOut)) : TransitionEffectOption.RectangleIn;
    internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Shred;
    return TransitionEffect.Shred;
  }

  private static TransitionEffect ParsePrismTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("dir"))
      internalTransition.Prism.Direction = SlideTransitionConstant.GetSideDirectionTransitionValue(reader.Value);
    if (reader.MoveToAttribute("isInverted"))
      internalTransition.Prism.IsInverted = XmlConvert.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("isContent"))
      internalTransition.Prism.IsContent = XmlConvert.ToBoolean(reader.Value);
    reader.MoveToElement();
    if (internalTransition.Prism.Direction == TransitionSideDirectionType.Down && !internalTransition.Prism.IsInverted && !internalTransition.Prism.IsContent)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Cube;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Down;
    }
    else if (internalTransition.Prism.Direction == TransitionSideDirectionType.Right && !internalTransition.Prism.IsInverted && !internalTransition.Prism.IsContent)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Cube;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Right;
    }
    else if (internalTransition.Prism.Direction == TransitionSideDirectionType.Up && !internalTransition.Prism.IsInverted && !internalTransition.Prism.IsContent)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Cube;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Up;
    }
    else if ((internalTransition.Prism.Direction == TransitionSideDirectionType.Left || internalTransition.Prism.Direction == TransitionSideDirectionType.None) && !internalTransition.Prism.IsInverted && !internalTransition.Prism.IsContent)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Cube;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Left;
    }
    else if ((internalTransition.Prism.Direction == TransitionSideDirectionType.Left || internalTransition.Prism.Direction == TransitionSideDirectionType.None) && !internalTransition.Prism.IsInverted && internalTransition.Prism.IsContent)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Rotate;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Left;
    }
    else if (internalTransition.Prism.Direction == TransitionSideDirectionType.Right && !internalTransition.Prism.IsInverted && internalTransition.Prism.IsContent)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Rotate;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Right;
    }
    else if (internalTransition.Prism.Direction == TransitionSideDirectionType.Up && !internalTransition.Prism.IsInverted && internalTransition.Prism.IsContent)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Rotate;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Up;
    }
    else if (internalTransition.Prism.Direction == TransitionSideDirectionType.Down && !internalTransition.Prism.IsInverted && internalTransition.Prism.IsContent)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Rotate;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Down;
    }
    else if ((internalTransition.Prism.Direction == TransitionSideDirectionType.Left || internalTransition.Prism.Direction == TransitionSideDirectionType.None) && internalTransition.Prism.IsInverted && !internalTransition.Prism.IsContent)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Box;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Left;
    }
    else if (internalTransition.Prism.Direction == TransitionSideDirectionType.Right && internalTransition.Prism.IsInverted && !internalTransition.Prism.IsContent)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Box;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Right;
    }
    else if (internalTransition.Prism.Direction == TransitionSideDirectionType.Up && internalTransition.Prism.IsInverted && !internalTransition.Prism.IsContent)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Box;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Up;
    }
    else if (internalTransition.Prism.Direction == TransitionSideDirectionType.Down && internalTransition.Prism.IsInverted && !internalTransition.Prism.IsContent)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Box;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Down;
    }
    else if ((internalTransition.Prism.Direction == TransitionSideDirectionType.Left || internalTransition.Prism.Direction == TransitionSideDirectionType.None) && internalTransition.Prism.IsInverted && internalTransition.Prism.IsContent)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Orbit;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Left;
    }
    else if (internalTransition.Prism.Direction == TransitionSideDirectionType.Right && internalTransition.Prism.IsInverted && internalTransition.Prism.IsContent)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Orbit;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Right;
    }
    else if (internalTransition.Prism.Direction == TransitionSideDirectionType.Up && internalTransition.Prism.IsInverted && internalTransition.Prism.IsContent)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Orbit;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Up;
    }
    else if (internalTransition.Prism.Direction == TransitionSideDirectionType.Down && internalTransition.Prism.IsInverted && internalTransition.Prism.IsContent)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Orbit;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Down;
    }
    return internalTransition.SlideShowTransition.TransitionEffect;
  }

  private static TransitionEffect ParsePanTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("dir"))
      internalTransition.Pan.Direction = SlideTransitionConstant.GetSideDirectionTransitionValue(reader.Value);
    reader.MoveToElement();
    if (internalTransition.Pan.Direction == TransitionSideDirectionType.Down)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Down;
    else if (internalTransition.Pan.Direction == TransitionSideDirectionType.Left || internalTransition.Pan.Direction == TransitionSideDirectionType.None)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Left;
    else if (internalTransition.Pan.Direction == TransitionSideDirectionType.Right)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Right;
    else if (internalTransition.Pan.Direction == TransitionSideDirectionType.Up)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Up;
    internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Pan;
    return internalTransition.SlideShowTransition.TransitionEffect;
  }

  private static TransitionEffect ParsePrstTransTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("prst"))
      internalTransition.PrstTrans.PresetTransition = reader.Value;
    if (reader.MoveToAttribute("invX"))
      internalTransition.PrstTrans.InvX = XmlConvert.ToBoolean(reader.Value);
    reader.MoveToElement();
    if (internalTransition.PrstTrans.PresetTransition == "fallOver" && !internalTransition.PrstTrans.InvX)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.FallOver;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Left;
    }
    else if (internalTransition.PrstTrans.PresetTransition == "fallOver" && internalTransition.PrstTrans.InvX)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.FallOver;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Right;
    }
    else if (internalTransition.PrstTrans.PresetTransition == "drape" && !internalTransition.PrstTrans.InvX)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Drape;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Left;
    }
    else if (internalTransition.PrstTrans.PresetTransition == "drape" && internalTransition.PrstTrans.InvX)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Drape;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Right;
    }
    else if (internalTransition.PrstTrans.PresetTransition == "curtains" && !internalTransition.PrstTrans.InvX)
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Curtains;
    else if (internalTransition.PrstTrans.PresetTransition == "wind" && internalTransition.PrstTrans.InvX)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Wind;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Left;
    }
    else if (internalTransition.PrstTrans.PresetTransition == "wind" && !internalTransition.PrstTrans.InvX)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Wind;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Right;
    }
    else if (internalTransition.PrstTrans.PresetTransition == "prestige" && !internalTransition.PrstTrans.InvX)
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Prestige;
    else if (internalTransition.PrstTrans.PresetTransition == "fracture" && !internalTransition.PrstTrans.InvX)
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Fracture;
    else if (internalTransition.PrstTrans.PresetTransition == "crush" && !internalTransition.PrstTrans.InvX)
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Crush;
    else if (internalTransition.PrstTrans.PresetTransition == "peelOff" && !internalTransition.PrstTrans.InvX)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.PeelOff;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Left;
    }
    else if (internalTransition.PrstTrans.PresetTransition == "peelOff" && internalTransition.PrstTrans.InvX)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.PeelOff;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Right;
    }
    else if (internalTransition.PrstTrans.PresetTransition == "pageCurlSingle" && !internalTransition.PrstTrans.InvX)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.PageCurlSingle;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Left;
    }
    else if (internalTransition.PrstTrans.PresetTransition == "pageCurlSingle" && internalTransition.PrstTrans.InvX)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.PageCurlSingle;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Right;
    }
    else if (internalTransition.PrstTrans.PresetTransition == "pageCurlDouble" && !internalTransition.PrstTrans.InvX)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.PageCurlDouble;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Left;
    }
    else if (internalTransition.PrstTrans.PresetTransition == "pageCurlDouble" && internalTransition.PrstTrans.InvX)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.PageCurlDouble;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Right;
    }
    else if (internalTransition.PrstTrans.PresetTransition == "airplane" && internalTransition.PrstTrans.InvX)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Airplane;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Left;
    }
    else if (internalTransition.PrstTrans.PresetTransition == "airplane" && !internalTransition.PrstTrans.InvX)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Airplane;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Right;
    }
    else if (internalTransition.PrstTrans.PresetTransition == "origami" && internalTransition.PrstTrans.InvX)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Origami;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Left;
    }
    else if (internalTransition.PrstTrans.PresetTransition == "origami" && !internalTransition.PrstTrans.InvX)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Origami;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Right;
    }
    return internalTransition.SlideShowTransition.TransitionEffect;
  }

  private static TransitionEffect ParseWheelreverseTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("spokes"))
      internalTransition.WheelReverse.Spokes = reader.Value;
    reader.MoveToElement();
    internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Reverse1Spoke;
    internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Wheel;
    return internalTransition.SlideShowTransition.TransitionEffect;
  }

  private static TransitionEffect ParseVortexTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("dir"))
      internalTransition.Vortex.Direction = SlideTransitionConstant.GetSideDirectionTransitionValue(reader.Value);
    reader.MoveToElement();
    if (internalTransition.Vortex.Direction == TransitionSideDirectionType.Down)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Down;
    else if (internalTransition.Vortex.Direction == TransitionSideDirectionType.Left || internalTransition.Vortex.Direction == TransitionSideDirectionType.None)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Left;
    else if (internalTransition.Vortex.Direction == TransitionSideDirectionType.Right)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Right;
    else if (internalTransition.Vortex.Direction == TransitionSideDirectionType.Up)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Up;
    internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Vortex;
    return internalTransition.SlideShowTransition.TransitionEffect;
  }

  private static TransitionEffect ParseRippleTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("dir"))
      internalTransition.Ripple.Direction = SlideTransitionConstant.GetCornerDirectionTransitionValue(reader.Value);
    reader.MoveToElement();
    if (internalTransition.Ripple.Direction == TransitionCornerDirectionType.LeftDown)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.LeftDown;
    else if (internalTransition.Ripple.Direction == TransitionCornerDirectionType.LeftUp)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.LeftUp;
    else if (internalTransition.Ripple.Direction == TransitionCornerDirectionType.RightDown)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.RightDown;
    else if (internalTransition.Ripple.Direction == TransitionCornerDirectionType.RightUp)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.RightUp;
    else if (internalTransition.Ripple.Direction == TransitionCornerDirectionType.None)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Center;
    internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Ripple;
    return internalTransition.SlideShowTransition.TransitionEffect;
  }

  private static TransitionEffect ParseGlitterTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("dir"))
      internalTransition.Glitter.Direction = SlideTransitionConstant.GetSideDirectionTransitionValue(reader.Value);
    if (reader.MoveToAttribute("pattern"))
      internalTransition.Glitter.Pattern = SlideTransitionConstant.GetTransitionPattern(reader.Value);
    reader.MoveToElement();
    if (internalTransition.Glitter.Direction == TransitionSideDirectionType.Down && (internalTransition.Glitter.Pattern == TransitionPattern.Diamond || internalTransition.Glitter.Pattern == TransitionPattern.None))
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.GlitterDiamond;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Up;
    }
    else if ((internalTransition.Glitter.Direction == TransitionSideDirectionType.Left || internalTransition.Glitter.Direction == TransitionSideDirectionType.None) && (internalTransition.Glitter.Pattern == TransitionPattern.Diamond || internalTransition.Glitter.Pattern == TransitionPattern.None))
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.GlitterDiamond;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Right;
    }
    else if (internalTransition.Glitter.Direction == TransitionSideDirectionType.Right && (internalTransition.Glitter.Pattern == TransitionPattern.Diamond || internalTransition.Glitter.Pattern == TransitionPattern.None))
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.GlitterDiamond;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Left;
    }
    else if (internalTransition.Glitter.Direction == TransitionSideDirectionType.Up && (internalTransition.Glitter.Pattern == TransitionPattern.Diamond || internalTransition.Glitter.Pattern == TransitionPattern.None))
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.GlitterDiamond;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Down;
    }
    else if (internalTransition.Glitter.Direction == TransitionSideDirectionType.Down && internalTransition.Glitter.Pattern == TransitionPattern.Hexagon)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.GlitterHexagon;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Up;
    }
    else if ((internalTransition.Glitter.Direction == TransitionSideDirectionType.Left || internalTransition.Glitter.Direction == TransitionSideDirectionType.None) && internalTransition.Glitter.Pattern == TransitionPattern.Hexagon)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.GlitterHexagon;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Right;
    }
    else if (internalTransition.Glitter.Direction == TransitionSideDirectionType.Right && internalTransition.Glitter.Pattern == TransitionPattern.Hexagon)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.GlitterHexagon;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Left;
    }
    else if (internalTransition.Glitter.Direction == TransitionSideDirectionType.Up && internalTransition.Glitter.Pattern == TransitionPattern.Hexagon)
    {
      internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.GlitterHexagon;
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Down;
    }
    return internalTransition.SlideShowTransition.TransitionEffect;
  }

  private static TransitionEffect ParseGalleryTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("dir"))
      internalTransition.Gallery.Direction = SlideTransitionConstant.GetTransitionLeftRightDirectionValue(reader.Value);
    reader.MoveToElement();
    if (internalTransition.Gallery.Direction == TransitionLeftRightDirectionType.Right)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Right;
    else if (internalTransition.Gallery.Direction == TransitionLeftRightDirectionType.Left || internalTransition.Gallery.Direction == TransitionLeftRightDirectionType.None)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Left;
    internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Gallery;
    return internalTransition.SlideShowTransition.TransitionEffect;
  }

  private static TransitionEffect ParseConveyorTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("dir"))
      internalTransition.Conveyor.Direction = SlideTransitionConstant.GetTransitionLeftRightDirectionValue(reader.Value);
    reader.MoveToElement();
    if (internalTransition.Conveyor.Direction == TransitionLeftRightDirectionType.Right)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Right;
    else if (internalTransition.Conveyor.Direction == TransitionLeftRightDirectionType.Left || internalTransition.Conveyor.Direction == TransitionLeftRightDirectionType.None)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Left;
    internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Conveyor;
    return internalTransition.SlideShowTransition.TransitionEffect;
  }

  private static TransitionEffect ParseDoorsTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("dir"))
      internalTransition.Door.Direction = SlideTransitionConstant.GetSlideTransitionDirectionValue(reader.Value);
    reader.MoveToElement();
    if (internalTransition.Door.Direction == Direction.Vertical)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Vertical;
    else if (internalTransition.Door.Direction == Direction.Horizontal || internalTransition.Door.Direction == Direction.None)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Horizontal;
    internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Doors;
    return internalTransition.SlideShowTransition.TransitionEffect;
  }

  private static TransitionEffect ParseWindowTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("dir"))
      internalTransition.Window.Direction = SlideTransitionConstant.GetSlideTransitionDirectionValue(reader.Value);
    reader.MoveToElement();
    if (internalTransition.Window.Direction == Direction.Vertical)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Vertical;
    else if (internalTransition.Window.Direction == Direction.Horizontal || internalTransition.Window.Direction == Direction.None)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Horizontal;
    internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Window;
    return internalTransition.SlideShowTransition.TransitionEffect;
  }

  private static TransitionEffect ParseWarpTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("dir"))
      internalTransition.Warp.Direction = SlideTransitionConstant.GetTransitionInOutDirectionValue(reader.Value);
    reader.MoveToElement();
    if (internalTransition.Warp.Direction == TransitionInOutDirectionType.In)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.In;
    else if (internalTransition.Warp.Direction == TransitionInOutDirectionType.Out || internalTransition.Warp.Direction == TransitionInOutDirectionType.None)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Out;
    internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Warp;
    return internalTransition.SlideShowTransition.TransitionEffect;
  }

  private static TransitionEffect ParseFlyThroughTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("dir"))
      internalTransition.Flythrough.Direction = SlideTransitionConstant.GetTransitionInOutDirectionValue(reader.Value);
    if (reader.MoveToAttribute("hasBounce"))
      internalTransition.Flythrough.hasBounce = XmlConvert.ToBoolean(reader.Value);
    reader.MoveToElement();
    if (internalTransition.Flythrough.Direction == TransitionInOutDirectionType.Out && !internalTransition.Flythrough.hasBounce)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Out;
    else if (internalTransition.Flythrough.Direction == TransitionInOutDirectionType.Out && internalTransition.Flythrough.hasBounce)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Outbounce;
    else if ((internalTransition.Flythrough.Direction == TransitionInOutDirectionType.In || internalTransition.Flythrough.Direction == TransitionInOutDirectionType.None) && !internalTransition.Flythrough.hasBounce)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.In;
    else if ((internalTransition.Flythrough.Direction == TransitionInOutDirectionType.Out || internalTransition.Flythrough.Direction == TransitionInOutDirectionType.None) && internalTransition.Flythrough.hasBounce)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Inbounce;
    internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.FlyThrough;
    return internalTransition.SlideShowTransition.TransitionEffect;
  }

  private static TransitionEffect ParseMorphTransition(
    XmlReader reader,
    TransitionInternal internalTransition)
  {
    if (reader.MoveToAttribute("option"))
      internalTransition.Morph.Option = SlideTransitionConstant.GetMorphValues(reader.Value);
    reader.MoveToElement();
    if (internalTransition.Morph.Option == TransitionMorphOption.ByChar)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.ByChar;
    else if (internalTransition.Morph.Option == TransitionMorphOption.ByObject)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.ByObject;
    else if (internalTransition.Morph.Option == TransitionMorphOption.ByWord)
      internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.ByWord;
    internalTransition.SlideShowTransition.TransitionEffect = TransitionEffect.Morph;
    return internalTransition.SlideShowTransition.TransitionEffect;
  }

  private static void GetSlideTransitionCoverEffectType(
    XmlReader reader,
    TransitionEightDirectionType directionValue,
    TransitionInternal internalTransition)
  {
    switch (directionValue)
    {
      case TransitionEightDirectionType.None:
      case TransitionEightDirectionType.Left:
        internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Left;
        break;
      case TransitionEightDirectionType.LeftDown:
        internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.LeftDown;
        break;
      case TransitionEightDirectionType.LeftUp:
        internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.LeftUp;
        break;
      case TransitionEightDirectionType.RightDown:
        internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.RightDown;
        break;
      case TransitionEightDirectionType.RightUp:
        internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.RightUp;
        break;
      case TransitionEightDirectionType.Up:
        internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Up;
        break;
      case TransitionEightDirectionType.Down:
        internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Down;
        break;
      case TransitionEightDirectionType.Right:
        internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Right;
        break;
    }
  }

  private static void GetSlideTransitionPullEffectType(
    XmlReader reader,
    TransitionEightDirectionType directionValue,
    TransitionInternal internalTransition)
  {
    switch (directionValue)
    {
      case TransitionEightDirectionType.None:
      case TransitionEightDirectionType.Left:
        internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Left;
        break;
      case TransitionEightDirectionType.LeftDown:
        internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.LeftDown;
        break;
      case TransitionEightDirectionType.LeftUp:
        internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.LeftUp;
        break;
      case TransitionEightDirectionType.RightDown:
        internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.RightDown;
        break;
      case TransitionEightDirectionType.RightUp:
        internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.RightUp;
        break;
      case TransitionEightDirectionType.Up:
        internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Up;
        break;
      case TransitionEightDirectionType.Down:
        internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Down;
        break;
      case TransitionEightDirectionType.Right:
        internalTransition.SlideShowTransition.TransitionEffectOption = TransitionEffectOption.Right;
        break;
    }
  }

  internal static void ParseNotesMasterSlide(XmlReader reader, NotesMasterSlide notesMasterSlide)
  {
    if (reader.NamespaceURI != "http://schemas.openxmlformats.org/presentationml/2006/main")
      throw new ArgumentException("Invalid Presentation Header");
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
            case "cSld":
              notesMasterSlide.SlidePrsvedElts.Add("cSld", UtilityMethods.ReadSingleNodeIntoStream(reader));
              continue;
            case "clrMap":
              notesMasterSlide.ColorMap = Parser.ParseColorMap(reader);
              Stream data;
              if (notesMasterSlide.SlidePrsvedElts.TryGetValue("cSld", out data) && data != null && data.Length > 0L)
              {
                data.Position = 0L;
                XmlReader reader1 = UtilityMethods.CreateReader(data);
                reader1.ReadToFollowing("cSld", "http://schemas.openxmlformats.org/presentationml/2006/main");
                Parser.ParseCommonSilde(reader1, (BaseSlide) notesMasterSlide);
                continue;
              }
              continue;
            case "notesStyle":
              notesMasterSlide.NotesTextStyle = new TextBody((BaseSlide) notesMasterSlide);
              RichTextParser.ParseListStyle(reader, notesMasterSlide.NotesTextStyle);
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          reader.Skip();
      }
      Parser.RemoveImageRelation(notesMasterSlide.TopRelation);
    }
    reader.Read();
  }

  internal static void ParseCommentList(XmlReader reader, Slide slide)
  {
    Comments comments = (Comments) slide.Comments;
    if (reader.NamespaceURI != "http://schemas.openxmlformats.org/presentationml/2006/main")
      throw new ArgumentException("Invalid Presentation Header");
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
          case "cm":
            Comment comment = Parser.ParseComment(reader, slide);
            comments.Add(comment);
            continue;
          default:
            continue;
        }
      }
      else
        reader.Read();
    }
  }

  internal static Comment ParseComment(XmlReader reader, Slide slide)
  {
    Comment comment = new Comment(slide);
    if (reader.HasAttributes)
    {
      while (reader.MoveToNextAttribute())
      {
        switch (reader.LocalName)
        {
          case "authorId":
            comment.GetCommentAuthor().AuthorId = Helper.ToUInt(reader.Value);
            continue;
          case "dt":
            comment.DateTime = DateTime.Parse(reader.Value);
            continue;
          case "idx":
            comment.Index = Helper.ToUInt(reader.Value);
            continue;
          default:
            continue;
        }
      }
      reader.MoveToElement();
    }
    string localName = reader.LocalName;
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (localName != reader.LocalName && reader.NodeType != XmlNodeType.EndElement)
      {
        DrawingParser.SkipWhitespaces(reader);
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "pos":
              Parser.ParseCommentPosition(reader, comment);
              continue;
            case "text":
              comment.Text = reader.ReadElementContentAsString();
              continue;
            case "extLst":
              Parser.ParseExtensionList(reader, comment);
              reader.Skip();
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
      }
    }
    return comment;
  }

  internal static void ParseExtensionList(XmlReader reader, Comment comment)
  {
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      reader.Read();
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "parentCm":
            uint num1 = 0;
            uint num2 = 0;
            if (reader.MoveToAttribute("authorId"))
              num1 = Convert.ToUInt32(reader.Value);
            if (reader.MoveToAttribute("idx"))
              num2 = Convert.ToUInt32(reader.Value);
            foreach (Comment comment1 in (IEnumerable<IComment>) comment.GetParentSlide().Comments)
            {
              CommentAuthor commentAuthor = comment1.GetCommentAuthor();
              if ((int) num1 == (int) commentAuthor.AuthorId && (int) num2 == (int) comment1.Index)
              {
                comment.IsReply = true;
                comment.SetParent((IComment) comment1);
              }
            }
            reader.MoveToElement();
            continue;
          default:
            continue;
        }
      }
      else
        reader.Skip();
    }
    reader.Skip();
  }

  internal static void ParseCommentPosition(XmlReader reader, Comment comment)
  {
    if (reader.HasAttributes)
    {
      while (reader.MoveToNextAttribute())
      {
        switch (reader.LocalName)
        {
          case "x":
            comment.Left = Convert.ToDouble(reader.Value) / 8.0;
            continue;
          case "y":
            comment.Top = Convert.ToDouble(reader.Value) / 8.0;
            continue;
          default:
            continue;
        }
      }
    }
    reader.Skip();
  }

  internal static void ParseNotesSlide(XmlReader reader, NotesSlide notesSlide)
  {
    if (reader.NamespaceURI != "http://schemas.openxmlformats.org/presentationml/2006/main")
      throw new ArgumentException("Invalid Presentation Header");
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
            case "cSld":
              Parser.ParseCommonSilde(reader, (BaseSlide) notesSlide);
              using (IEnumerator<ISlideItem> enumerator = notesSlide.Shapes.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  Shape current = (Shape) enumerator.Current;
                  if (current.PlaceholderFormat != null && current.PlaceholderFormat.Type == PlaceholderType.Body)
                  {
                    notesSlide.SetTextBody(current.TextBody as TextBody);
                    break;
                  }
                }
                continue;
              }
            case "clrMapOvr":
              Parser.ParseColorMapOvr(reader, (BaseSlide) notesSlide);
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

  internal static void ParseViewProperties(XmlReader reader, Syncfusion.Presentation.Presentation presentation)
  {
    NotesSize notesSize = (NotesSize) presentation.NotesSize;
    if (!reader.IsEmptyElement)
    {
      string localName = reader.LocalName;
      if (reader.HasAttributes && reader.MoveToAttribute("lastView"))
        presentation.LastView = reader.Value;
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "normalViewPr":
              if (reader.MoveToAttribute("horzBarState"))
              {
                notesSize.HorizontalBarState = Helper.GetSplitterBarState(reader.Value);
                reader.MoveToElement();
              }
              reader.ReadToFollowing("restoredLeft", "http://schemas.openxmlformats.org/presentationml/2006/main");
              if (reader.MoveToAttribute("sz"))
                notesSize.SetWidth(Convert.ToUInt32(reader.Value));
              reader.ReadToFollowing("restoredTop", "http://schemas.openxmlformats.org/presentationml/2006/main");
              if (reader.MoveToAttribute("sz"))
                notesSize.AssignHeight(Convert.ToUInt32(reader.Value));
              reader.ReadToNextSibling(reader.LocalName);
              reader.Skip();
              continue;
            default:
              if (!presentation.PreservedElements.ContainsKey(reader.LocalName))
              {
                presentation.PreservedElements.Add(reader.LocalName, UtilityMethods.ReadSingleNodeIntoStream(reader));
                continue;
              }
              continue;
          }
        }
        else
          reader.Skip();
      }
    }
    reader.Read();
  }

  internal static void ParseCommentAuthors(XmlReader reader, Syncfusion.Presentation.Presentation presentation)
  {
    CommentAuthors commentAuthors = presentation.CommentAuthors;
    if (reader.NamespaceURI != "http://schemas.openxmlformats.org/presentationml/2006/main")
      throw new ArgumentException("Invalid Presentation Header");
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
          case "cmAuthor":
            commentAuthors.Add(Parser.ParseCommentAuthor(reader, presentation));
            reader.Skip();
            continue;
          default:
            continue;
        }
      }
      else
        reader.Read();
    }
  }

  internal static CommentAuthor ParseCommentAuthor(XmlReader reader, Syncfusion.Presentation.Presentation presentation)
  {
    CommentAuthor commentAuthor = new CommentAuthor(presentation);
    if (reader.HasAttributes)
    {
      while (reader.MoveToNextAttribute())
      {
        switch (reader.LocalName)
        {
          case "id":
            commentAuthor.AuthorId = Helper.ToUInt(reader.Value);
            continue;
          case "name":
            commentAuthor.Name = reader.Value;
            continue;
          case "initials":
            commentAuthor.Initials = reader.Value;
            continue;
          case "lastIdx":
            commentAuthor.LastIndex = Helper.ToUInt(reader.Value);
            continue;
          case "clrIdx":
            commentAuthor.ColorIndex = Helper.ToUInt(reader.Value);
            continue;
          default:
            continue;
        }
      }
      reader.MoveToElement();
    }
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "extLst")
          reader.Skip();
      }
    }
    return commentAuthor;
  }
}
