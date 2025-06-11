// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlReaders.Shapes.TextBoxShapeParser
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Compression;
using Syncfusion.Drawing;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
using Syncfusion.XlsIO.Implementation.XmlSerialization.Charts;
using Syncfusion.XlsIO.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlReaders.Shapes;

internal class TextBoxShapeParser
{
  public static void ParseTextBox(ITextBox textBox, XmlReader reader, Excel2007Parser parser)
  {
    TextBoxShapeParser.ParseTextBox(textBox, reader, parser, (List<string>) null);
  }

  internal static void ParseTextBox(
    ITextBox textBox,
    XmlReader reader,
    Excel2007Parser parser,
    List<string> lstRelationIds)
  {
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    reader.Read();
    ShapeImpl shapeImpl = textBox as ShapeImpl;
    shapeImpl.HasLineFormat = false;
    shapeImpl.HasFill = false;
    bool flag = false;
    TextBoxShapeImpl textBoxShapeImpl = textBox as TextBoxShapeImpl;
    while (reader.NodeType != XmlNodeType.None)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "txBody":
            TextBoxShapeParser.ParseRichText(reader, parser, textBox);
            continue;
          case "spPr":
            TextBoxShapeParser.ParseShapeProperties(textBox, reader, parser, lstRelationIds);
            continue;
          case "nvSpPr":
            TextBoxShapeParser.ParseNonVisualShapeProperties(textBox as IShape, reader, parser);
            continue;
          case "style":
            flag = true;
            shapeImpl.StyleStream = ShapeParser.ReadNodeAsStream(reader);
            XmlReader reader1 = UtilityMethods.CreateReader(shapeImpl.StyleStream, "lnRef");
            ShapeLineFormatImpl line = textBoxShapeImpl.Line as ShapeLineFormatImpl;
            int num = -1;
            if (reader1.MoveToAttribute("idx"))
              num = int.Parse(reader1.Value);
            if ((!textBoxShapeImpl.IsLineProperties || !line.IsNoFill) && !line.IsSolidFill)
            {
              switch (num)
              {
                case -1:
                  break;
                case 0:
                  line.Visible = false;
                  break;
                default:
                  line.Visible = true;
                  break;
              }
            }
            line.DefaultLineStyleIndex = num;
            XmlReader reader2 = UtilityMethods.CreateReader(shapeImpl.StyleStream, "fillRef");
            if (!textBoxShapeImpl.IsNoFill && !textBoxShapeImpl.IsFill && reader2.MoveToAttribute("idx"))
            {
              if (int.Parse(reader2.Value) == 0)
              {
                textBoxShapeImpl.Fill.Visible = false;
                continue;
              }
              textBoxShapeImpl.Fill.Visible = true;
              continue;
            }
            continue;
          case "solidFill":
            IInternalFill fill = (textBox as TextBoxShapeImpl).Fill as IInternalFill;
            ChartParserCommon.ParseSolidFill(reader, parser, fill);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Read();
    }
    if (textBoxShapeImpl == null || flag || textBoxShapeImpl.IsFill || textBoxShapeImpl.IsNoFill)
      return;
    textBoxShapeImpl.Fill.Visible = false;
  }

  private static void ParseNonVisualShapeProperties(
    IShape shape,
    XmlReader reader,
    Excel2007Parser parser)
  {
    if (shape == null)
      throw new ArgumentNullException(nameof (shape));
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (parser == null)
      throw new ArgumentNullException(nameof (parser));
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "cNvPr":
              if (shape is ShapeImpl shape1)
              {
                WorksheetDataHolder dataHolder = shape1.Worksheet.DataHolder;
                RelationCollection drawingsRelations = dataHolder.DrawingsRelations;
                FileDataHolder parentHolder = dataHolder.ParentHolder;
                Excel2007Parser.ParseNVCanvasProperties(reader, shape1, drawingsRelations, " ", parentHolder, new List<string>(), parentHolder.ItemsToRemove);
                continue;
              }
              Excel2007Parser.ParseNVCanvasProperties(reader, shape);
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

  private static void ParseShapeProperties(
    ITextBox textBox,
    XmlReader reader,
    Excel2007Parser parser,
    List<string> lstRelationIds)
  {
    reader.Read();
    TextBoxShapeImpl textBoxShapeImpl = (TextBoxShapeImpl) textBox;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "solidFill":
            IInternalFill fill1 = textBoxShapeImpl.Fill as IInternalFill;
            ChartParserCommon.ParseSolidFill(reader, parser, fill1);
            textBoxShapeImpl.IsFill = true;
            continue;
          case "noFill":
            (textBoxShapeImpl.Fill as IInternalFill).Visible = false;
            textBoxShapeImpl.IsNoFill = true;
            reader.Read();
            continue;
          case "ln":
            textBoxShapeImpl.IsLineProperties = true;
            ShapeLineFormatImpl line = (ShapeLineFormatImpl) textBoxShapeImpl.Line;
            TextBoxShapeParser.ParseLineProperties(reader, line, false, parser);
            continue;
          case "xfrm":
            if (reader.MoveToAttribute("rot"))
              textBoxShapeImpl.ShapeRotation = (int) Math.Round((double) Convert.ToInt64(reader.Value) / 60000.0);
            if (reader.MoveToAttribute("flipH"))
              textBoxShapeImpl.FlipHorizontal = XmlConvertExtension.ToBoolean(reader.Value);
            if (reader.MoveToAttribute("flipV"))
              textBoxShapeImpl.FlipVertical = XmlConvertExtension.ToBoolean(reader.Value);
            textBoxShapeImpl.Coordinates2007 = TextBoxShapeParser.ParseForm(reader);
            continue;
          case "gradFill":
            Stream data = ShapeParser.ReadNodeAsStream(reader);
            data.Position = 0L;
            XmlReader reader1 = UtilityMethods.CreateReader(data);
            IInternalFill fill2 = textBoxShapeImpl.Fill as IInternalFill;
            fill2.FillType = ExcelFillType.Gradient;
            fill2.PreservedGradient = ChartParserCommon.ParseGradientFill(reader1, parser, textBoxShapeImpl.Fill as ShapeFillImpl);
            if (fill2.PreservedGradient != null && fill2.PreservedGradient.Count > 2)
              fill2.GradientColorType = ExcelGradientColor.MultiColor;
            textBoxShapeImpl.IsFill = true;
            continue;
          case "prstGeom":
            if (reader.LocalName == "prstGeom")
            {
              string str = "";
              if (reader.MoveToAttribute("prst"))
                str = reader.Value;
              textBoxShapeImpl.PresetGeometry = str;
              reader.Skip();
              continue;
            }
            continue;
          case "grpFill":
            textBoxShapeImpl.IsFill = true;
            textBoxShapeImpl.IsGroupFill = true;
            reader.Skip();
            continue;
          case "blipFill":
            textBoxShapeImpl.IsFill = true;
            IInternalFill fill3 = textBoxShapeImpl.Fill as IInternalFill;
            ChartParserCommon.ParsePictureFill(reader, (IFill) fill3, textBoxShapeImpl.Worksheet.DataHolder.DrawingsRelations, textBoxShapeImpl.ParentWorkbook.DataHolder, textBoxShapeImpl.ParentWorkbook, lstRelationIds);
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

  private static Rectangle ParseForm(XmlReader reader)
  {
    Rectangle form = Rectangle.Empty;
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      int result1 = 0;
      int result2 = 0;
      int result3 = 0;
      int result4 = 0;
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "off":
              if (reader.MoveToAttribute("x") && !int.TryParse(reader.Value, out result1))
                result1 = int.MaxValue;
              if (reader.MoveToAttribute("y") && !int.TryParse(reader.Value, out result2))
              {
                result2 = int.MaxValue;
                continue;
              }
              continue;
            case "ext":
              if (reader.MoveToAttribute("cx") && !int.TryParse(reader.Value, out result3))
                result3 = int.MaxValue;
              if (reader.MoveToAttribute("cy") && !int.TryParse(reader.Value, out result4))
              {
                result4 = int.MaxValue;
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
      form = new Rectangle(result1, result2, result3, result4);
    }
    reader.Read();
    return form;
  }

  private static void ParseRichText(XmlReader reader, Excel2007Parser parser, ITextBox textBox)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    RichTextString textArea = textBox != null ? textBox.RichText as RichTextString : throw new ArgumentNullException(nameof (textBox));
    if (reader.MoveToAttribute("fLocksText"))
      textBox.IsTextLocked = XmlConvertExtension.ToBoolean(reader.Value);
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "bodyPr":
            TextBoxShapeParser.ParseBodyProperties(reader, textArea, textBox);
            continue;
          case "lstStyle":
            TextBoxShapeParser.ParseListStyles(reader, textArea);
            continue;
          case "p":
            TextBoxShapeParser.ParseParagraphs(reader, textBox, parser);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    textArea.TextObject.Defragment();
    reader.Read();
  }

  private static void ParseBodyProperties(
    XmlReader reader,
    RichTextString textArea,
    ITextBox textBox)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textArea == null)
      throw new ArgumentNullException(nameof (textArea));
    if (reader.LocalName != "bodyPr")
      throw new XmlException("Unexpected xml tag.");
    if (reader.HasAttributes)
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      reader.MoveToFirstAttribute();
      int num = 0;
      for (int attributeCount = reader.AttributeCount; num < attributeCount; ++num)
      {
        dictionary[reader.LocalName] = reader.Value;
        reader.MoveToNextAttribute();
      }
      if (dictionary.ContainsKey("anchor"))
      {
        TextBoxShapeParser.ParseAnchor(dictionary["anchor"], textBox);
        dictionary.Remove("anchor");
      }
      if (dictionary.ContainsKey("anchorCtr"))
      {
        if (dictionary["anchorCtr"] == "1")
          textBox.HAlignment = ExcelCommentHAlign.Center;
        dictionary.Remove("anchorCtr");
      }
      if (dictionary.ContainsKey("vert"))
      {
        TextBoxShapeParser.ParseTextRotation(dictionary["vert"], textBox);
        dictionary.Remove("vert");
      }
      if (dictionary.ContainsKey("lIns"))
      {
        (textBox as TextBoxShapeImpl).TextBodyPropertiesHolder.SetLeftMargin(Helper.ParseInt(dictionary["lIns"]));
        (textBox as TextBoxShapeImpl).TextBodyPropertiesHolder.IsAutoMargins = false;
      }
      if (dictionary.ContainsKey("rIns"))
      {
        (textBox as TextBoxShapeImpl).TextBodyPropertiesHolder.SetRightMargin(Helper.ParseInt(dictionary["rIns"]));
        (textBox as TextBoxShapeImpl).TextBodyPropertiesHolder.IsAutoMargins = false;
      }
      if (dictionary.ContainsKey("tIns"))
      {
        (textBox as TextBoxShapeImpl).TextBodyPropertiesHolder.SetTopMargin(Helper.ParseInt(dictionary["tIns"]));
        (textBox as TextBoxShapeImpl).TextBodyPropertiesHolder.IsAutoMargins = false;
      }
      if (dictionary.ContainsKey("bIns"))
      {
        (textBox as TextBoxShapeImpl).TextBodyPropertiesHolder.SetBottomMargin(Helper.ParseInt(dictionary["bIns"]));
        (textBox as TextBoxShapeImpl).TextBodyPropertiesHolder.IsAutoMargins = false;
      }
      dictionary.Remove("a");
      (textBox as TextBoxShapeBase).UnknownBodyProperties = dictionary;
    }
    reader.MoveToElement();
    if (reader.LocalName == "bodyPr" && !reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "spAutoFit":
              if (textBox is TextBoxShapeImpl textBoxShapeImpl)
                textBoxShapeImpl.IsAutoSize = true;
              reader.Read();
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          Excel2007Parser.SkipWhiteSpaces(reader);
      }
      reader.Read();
    }
    else
      reader.Skip();
  }

  private static void ParseTextRotation(string rotationValue, ITextBox textBox)
  {
    Excel2007TextRotation excel2007TextRotation = (Excel2007TextRotation) Enum.Parse(typeof (Excel2007TextRotation), rotationValue, false);
    textBox.TextRotation = (ExcelTextRotation) excel2007TextRotation;
    (textBox as TextBoxShapeImpl).TextBodyPropertiesHolder.TextDirection = Helper.SetTextDirection(rotationValue);
  }

  private static void ParseAnchor(string anchorValue, ITextBox textBox)
  {
    Excel2007CommentVAlign excel2007CommentValign = (Excel2007CommentVAlign) Enum.Parse(typeof (Excel2007CommentVAlign), anchorValue, false);
    textBox.VAlignment = (ExcelCommentVAlign) excel2007CommentValign;
  }

  internal static void ParseListStyles(XmlReader reader, RichTextString textArea)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textArea == null)
      throw new ArgumentNullException(nameof (textArea));
    textArea.LevelStyleStream = !(reader.LocalName != "lstStyle") ? ShapeParser.ReadNodeAsStream(reader) : throw new XmlException("Unexpected xml tag.");
    textArea.LevelStyleStream.Position = 0L;
    XmlReader reader1 = XmlReader.Create(UtilityMethods.CreateReader(textArea.LevelStyleStream), new XmlReaderSettings()
    {
      IgnoreWhitespace = true
    });
    reader1.Read();
    reader1.Read();
    while (reader1.NodeType != XmlNodeType.EndElement && reader1.NodeType != XmlNodeType.None)
    {
      if (reader1.NodeType == XmlNodeType.Element)
      {
        switch (reader1.LocalName)
        {
          case "lvl1pPr":
          case "lvl2pPr":
          case "lvl3pPr":
          case "lvl4pPr":
          case "lvl5pPr":
          case "lvl6pPr":
          case "lvl7pPr":
          case "lvl8pPr":
          case "lvl9pPr":
            TextBoxShapeParser.ParseLevelProperties(reader1, textArea);
            reader1.Read();
            continue;
          default:
            reader1.Skip();
            continue;
        }
      }
      else
        reader1.Skip();
    }
  }

  private static void ParseLevelProperties(XmlReader reader, RichTextString textArea)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textArea == null)
      throw new ArgumentNullException(nameof (textArea));
    LevelProperties levelProperties1 = new LevelProperties();
    LevelProperties levelProperties2 = new LevelProperties();
    if (reader.MoveToAttribute("algn"))
    {
      switch (reader.Value)
      {
        case "l":
          levelProperties2.TextAlignment = TextAlignType.Left;
          break;
        case "ctr":
          levelProperties2.TextAlignment = TextAlignType.Center;
          break;
        case "dist":
          levelProperties2.TextAlignment = TextAlignType.Distributed;
          break;
        case "r":
          levelProperties2.TextAlignment = TextAlignType.Right;
          break;
        case "just":
          levelProperties2.TextAlignment = TextAlignType.Right;
          break;
      }
    }
    if (reader.MoveToAttribute("defTabSz"))
      levelProperties2.TabSize = int.Parse(reader.Value);
    if (reader.MoveToAttribute("eaLnBrk"))
      levelProperties2.EastAsianBreak = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("fontAlgn"))
    {
      switch (reader.Value)
      {
        case "auto":
          levelProperties2.FontAlignment = FontAlignmentType.Automatic;
          break;
        case "b":
          levelProperties2.FontAlignment = FontAlignmentType.Bottom;
          break;
        case "base":
          levelProperties2.FontAlignment = FontAlignmentType.Baseline;
          break;
        case "ctr":
          levelProperties2.FontAlignment = FontAlignmentType.Center;
          break;
        case "t":
          levelProperties2.FontAlignment = FontAlignmentType.Top;
          break;
      }
    }
    if (reader.MoveToAttribute("indent"))
      levelProperties2.Indent = int.Parse(reader.Value);
    if (reader.MoveToAttribute("latinLnBrk "))
      levelProperties2.IsLatinBreak = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("eaLnBrk"))
      levelProperties2.EastAsianBreak = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("marL"))
      levelProperties2.LeftMargin = int.Parse(reader.Value);
    if (reader.MoveToAttribute("marR"))
      levelProperties2.RightMargin = int.Parse(reader.Value);
    if (reader.MoveToAttribute("rtl"))
      levelProperties2.IsRightToLeft = XmlConvertExtension.ToBoolean(reader.Value);
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (reader.LocalName == "defRPr")
        {
          TextBoxShapeParser.ParseDefaultRunProperties(reader, levelProperties2);
          reader.Skip();
        }
        else
          reader.Skip();
      }
      else
        reader.Read();
    }
    textArea.LevelStyles.Add(levelProperties2);
  }

  private static void ParseDefaultRunProperties(XmlReader reader, LevelProperties levelProperties)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (levelProperties == null)
      throw new ArgumentNullException(nameof (levelProperties));
    if (reader.LocalName != "defRPr")
      throw new XmlException("Unexpected xml tag.");
    DefaultRunProperties runProperties = levelProperties.RunProperties;
    if (reader.MoveToAttribute("sz"))
      runProperties.FontSize = int.Parse(reader.Value);
    if (reader.MoveToAttribute("b "))
      runProperties.IsBold = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("i "))
      runProperties.IsItalic = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("u "))
    {
      switch (reader.Value)
      {
        case "sng":
          runProperties.UnderlineStyle = UnderlineStyle.Continuous;
          break;
        case "dbl":
          runProperties.UnderlineStyle = UnderlineStyle.Double;
          break;
        default:
          runProperties.UnderlineStyle = UnderlineStyle.None;
          break;
      }
    }
    if (reader.MoveToAttribute("strike"))
    {
      switch (reader.Value)
      {
        case "sngStrike":
          runProperties.StrikeType = StrikeType.Single;
          break;
        case "noStrike":
          runProperties.StrikeType = StrikeType.None;
          break;
        case "dblStrike":
          runProperties.StrikeType = StrikeType.Double;
          break;
      }
    }
    reader.MoveToElement();
  }

  private static void ParseParagraphs(XmlReader reader, ITextBox textBox, Excel2007Parser parser)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    if (reader.LocalName != "p")
      throw new XmlException("Unexpected xml tag.");
    RichTextString richText = textBox.RichText as RichTextString;
    string text = richText.Text;
    if (text != null && text.Length != 0 && !text.EndsWith("\n"))
      richText.AddText("\n", richText.GetFont(text.Length - 1));
    reader.Read();
    BulletImpl bullet = (BulletImpl) null;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "pPr":
            TextBoxShapeParser.ParseParagraphProperites(reader, richText, textBox, out bullet);
            continue;
          case "r":
            TextBoxShapeParser.ParseParagraphRun(reader, richText, parser, textBox, bullet);
            continue;
          case "br":
            IFont paragraphRunProperites = (IFont) TextBoxShapeParser.ParseParagraphRunProperites(reader, richText, parser);
            richText.AddText("\n", paragraphRunProperites);
            continue;
          case "endParaRPr":
            TextBoxShapeParser.ParseParagraphEnd(reader, richText, parser);
            continue;
          case "fld":
            TextBoxShapeParser.ParseTextField(reader, textBox, richText, parser);
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

  internal static void ParseTextField(
    XmlReader reader,
    ITextBox textBox,
    RichTextString textArea,
    Excel2007Parser parser)
  {
    TextBoxShapeImpl textBoxShapeImpl = textBox as TextBoxShapeImpl;
    MemoryStream data = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((Stream) data, Encoding.UTF8);
    writer.WriteNode(reader, false);
    writer.Flush();
    if (textBoxShapeImpl != null)
      textBoxShapeImpl.FldElementStream = (Stream) data;
    data.Position = 0L;
    XmlReader reader1 = UtilityMethods.CreateReader((Stream) data);
    reader1.Read();
    if (textBoxShapeImpl != null)
    {
      if (reader1.MoveToAttribute("id"))
        textBoxShapeImpl.FieldId = reader1.Value;
      if (reader1.MoveToAttribute("type"))
        textBoxShapeImpl.FieldType = reader1.Value;
    }
    string text = (string) null;
    IFont font = (IFont) null;
    while (reader1.NodeType != XmlNodeType.EndElement)
    {
      if (reader1.NodeType == XmlNodeType.Element)
      {
        switch (reader1.LocalName)
        {
          case "rPr":
            font = (IFont) TextBoxShapeParser.ParseParagraphRunProperites(reader1, textArea, parser);
            continue;
          case "t":
            text = reader1.ReadElementContentAsString();
            if (textBoxShapeImpl != null)
            {
              textBoxShapeImpl.IsFldText = true;
              continue;
            }
            continue;
          default:
            reader1.Skip();
            continue;
        }
      }
      else
        reader1.Skip();
    }
    if (text == null || text.Length == 0)
      text = "\n";
    if (textBox != null && font != null)
    {
      (font as FontImpl).ParaAlign = (Excel2007CommentHAlign) textBox.HAlignment;
      (font as FontImpl).HasParagrapAlign = true;
    }
    if (font == null)
      return;
    textArea.AddText(text, font);
  }

  internal static void ParseParagraphEnd(
    XmlReader reader,
    RichTextString textArea,
    Excel2007Parser parser)
  {
    IFont paragraphRunProperites = (IFont) TextBoxShapeParser.ParseParagraphRunProperites(reader, textArea, parser);
    textArea.AddText("\n", paragraphRunProperites);
  }

  private static void ParseParagraphProperites(
    XmlReader reader,
    RichTextString textString,
    ITextBox textBox,
    out BulletImpl bullet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    if (reader.MoveToAttribute("algn"))
    {
      Excel2007CommentHAlign excel2007CommentHalign = (Excel2007CommentHAlign) Enum.Parse(typeof (Excel2007CommentHAlign), reader.Value, false);
      textBox.HAlignment = (ExcelCommentHAlign) excel2007CommentHalign;
    }
    if (reader.MoveToAttribute("lvl"))
    {
      if (textString != null)
        textString.StyleLevel = int.Parse(reader.Value);
    }
    else
      textString.StyleLevel = 0;
    bullet = (BulletImpl) null;
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "buFont":
              bullet = new BulletImpl();
              TextBoxShapeParser.ParseBulletFont(reader, bullet);
              continue;
            case "buChar":
              if (reader.MoveToAttribute("char"))
                bullet.BulletChar = reader.Value;
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
    }
    reader.Read();
  }

  internal static void ParseBulletFont(XmlReader reader, BulletImpl bullet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (bullet == null)
      throw new ArgumentNullException(nameof (bullet));
    if (reader.MoveToAttribute("typeface"))
      bullet.TypeFace = reader.Value;
    if (reader.MoveToAttribute("panose"))
      bullet.Panose = reader.Value;
    if (reader.MoveToAttribute("pitchFamily"))
      bullet.PitchFamily = int.Parse(reader.Value);
    if (reader.MoveToAttribute("charset"))
      bullet.CharSet = int.Parse(reader.Value);
    reader.Read();
  }

  internal static void ParseParagraphRun(
    XmlReader reader,
    RichTextString textArea,
    Excel2007Parser parser,
    ITextBox textBox,
    BulletImpl bullet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textArea == null)
      throw new ArgumentNullException(nameof (textArea));
    if (reader.LocalName != "r")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    string text = (string) null;
    IFont font = (IFont) null;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "rPr":
            font = (IFont) TextBoxShapeParser.ParseParagraphRunProperites(reader, textArea, parser);
            continue;
          case "t":
            text = reader.ReadElementContentAsString();
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    if (text == null || text.Length == 0)
      text = "\n";
    if (textBox != null)
    {
      (font as FontImpl).ParaAlign = (Excel2007CommentHAlign) textBox.HAlignment;
      (font as FontImpl).HasParagrapAlign = true;
    }
    textArea.AddText(text, font);
    if (bullet != null)
      textArea.Bullet = bullet;
    reader.Read();
  }

  private static FontImpl ParseParagraphRunProperites(
    XmlReader reader,
    RichTextString textArea,
    Excel2007Parser parser)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textArea == null)
      throw new ArgumentNullException(nameof (textArea));
    LevelProperties levelProperties = (LevelProperties) null;
    if (textArea.StyleLevel != -1 && textArea.LevelStyles != null && textArea.LevelStyles.Count > 0)
      levelProperties = textArea.LevelStyles[textArea.StyleLevel];
    WorkbookImpl workbook = textArea.Workbook;
    FontImpl font = (FontImpl) workbook.CreateFont(workbook.InnerFonts[0], false);
    if (reader.MoveToAttribute("b"))
      font.Bold = XmlConvertExtension.ToBoolean(reader.Value);
    else if (levelProperties != null)
      font.Bold = levelProperties.RunProperties.IsBold;
    if (reader.MoveToAttribute("i"))
      font.Italic = XmlConvertExtension.ToBoolean(reader.Value);
    else if (levelProperties != null)
      font.Italic = levelProperties.RunProperties.IsItalic;
    if (reader.MoveToAttribute("strike"))
    {
      font.Strikethrough = reader.Value != "noStrike";
      if (font.Strikethrough && reader.Value == "dblStrike")
        textArea.IsDoubleStrike = true;
    }
    else if (levelProperties != null)
    {
      StrikeType strikeType = levelProperties.RunProperties.StrikeType;
      font.Strikethrough = strikeType != StrikeType.None;
      if (strikeType == StrikeType.Double)
        textArea.IsDoubleStrike = true;
    }
    if (reader.MoveToAttribute("sz"))
      font.Size = (double) int.Parse(reader.Value) / 100.0;
    else if (levelProperties != null)
    {
      int fontSize = levelProperties.RunProperties.FontSize;
      if (fontSize > 0)
        font.Size = (double) fontSize / 100.0;
    }
    if (reader.MoveToAttribute("u"))
    {
      if (reader.Value == "sng")
        font.Underline = ExcelUnderline.Single;
      else if (reader.Value == "dbl")
        font.Underline = ExcelUnderline.Double;
    }
    else if (levelProperties != null)
    {
      switch (levelProperties.RunProperties.UnderlineStyle)
      {
        case UnderlineStyle.Continuous:
          font.Underline = ExcelUnderline.Single;
          break;
        case UnderlineStyle.Double:
          font.Underline = ExcelUnderline.Double;
          break;
      }
    }
    if (reader.MoveToAttribute("baseline"))
    {
      string str = reader.Value;
      int num = str == null || !str.Contains("%") ? int.Parse(reader.Value) : int.Parse(str.Replace("%", ""));
      if (num > 0)
        font.Superscript = true;
      else if (num < 0)
      {
        font.Subscript = true;
      }
      else
      {
        font.Superscript = false;
        font.Subscript = false;
      }
    }
    if (reader.MoveToAttribute("normalizeH") && reader.Value != "0")
      textArea.IsNormalizeHeights = new bool?(true);
    if (reader.MoveToAttribute("cap"))
    {
      textArea.IsCapsUsed = true;
      textArea.CapitalizationType = !(reader.Value == "small") ? (!(reader.Value == "all") ? TextCapsType.None : TextCapsType.All) : TextCapsType.Small;
    }
    font.showFontName = false;
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "latin":
              if (reader.MoveToAttribute("typeface"))
              {
                font.FontName = reader.Value;
                reader.MoveToElement();
              }
              font.showFontName = true;
              reader.Skip();
              continue;
            case "solidFill":
              ChartParserCommon.ParseSolidFill(reader, parser, font.ColorObject);
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
    reader.Skip();
    return font;
  }

  internal static void ParseLineProperties(
    XmlReader reader,
    ShapeLineFormatImpl border,
    bool bRoundCorners,
    Excel2007Parser parser)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (border == null)
      throw new ArgumentNullException(nameof (border));
    bool flag1 = !(reader.LocalName != "ln") ? reader.IsEmptyElement : throw new XmlException("Unexpected xml tag");
    if (reader.MoveToAttribute("w"))
    {
      double num = (double) int.Parse(reader.Value) / 12700.0;
      border.Weight = num;
      border.IsWidthExist = true;
    }
    if (reader.MoveToAttribute("cmpd"))
    {
      Excel2007ShapeLineStyle excel2007ShapeLineStyle = (Excel2007ShapeLineStyle) Enum.Parse(typeof (Excel2007ShapeLineStyle), reader.Value, false);
      border.Style = (ExcelShapeLineStyle) excel2007ShapeLineStyle;
    }
    bool flag2 = false;
    string key = (string) null;
    if (!flag1)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "noFill":
              border.Weight = 0.0;
              border.IsNoFill = true;
              reader.Read();
              continue;
            case "round":
              border.JoinType = Excel2007BorderJoinType.Round;
              border.HasBorderJoin = true;
              reader.Skip();
              continue;
            case "solidFill":
              ColorObject color = new ColorObject(ExcelKnownColors.None);
              ChartParserCommon.ParseSolidFill(reader, parser, border, color);
              border.ForeColor = color.GetRGB((IWorkbook) border.Workbook);
              border.IsSolidFill = true;
              flag2 = true;
              continue;
            case "prstDash":
              key = ChartParserCommon.ParseValueTag(reader);
              continue;
            case "pattFill":
              reader.Skip();
              continue;
            case "headEnd":
              TextBoxShapeParser.ParseArrowSettings(reader, border, true);
              continue;
            case "tailEnd":
              TextBoxShapeParser.ParseArrowSettings(reader, border, false);
              continue;
            case "miter":
              border.JoinType = Excel2007BorderJoinType.Mitter;
              if (reader.MoveToAttribute("lim"))
              {
                int result = 0;
                if (int.TryParse(reader.Value, out result))
                  border.MiterLim = result;
                else if (reader.Value.Contains("%") && int.TryParse(reader.Value.Replace("%", ""), out result))
                  border.MiterLim = result / 100;
              }
              border.HasBorderJoin = true;
              reader.Skip();
              continue;
            case "bevel":
              border.JoinType = Excel2007BorderJoinType.Bevel;
              border.HasBorderJoin = true;
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
      if (border.IsNoFill)
        border.Visible = false;
    }
    else
      flag2 = true;
    if (flag2)
    {
      border.DashStyle = ExcelShapeDashLineStyle.Solid;
      if (key != null)
      {
        ExcelShapeDashLineStyle shapeDashLineStyle;
        border.AppImplementation.StringEnum.LineDashTypeXmltoEnum.TryGetValue(key, out shapeDashLineStyle);
        border.DashStyle = shapeDashLineStyle;
      }
    }
    reader.Read();
  }

  internal static void ParseLineProperties(
    XmlReader reader,
    ShapeLineFormatImpl border,
    Excel2007Parser parser)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (border == null)
      throw new ArgumentNullException(nameof (border));
    bool flag1 = !(reader.LocalName != "ln") ? reader.IsEmptyElement : throw new XmlException("Unexpected xml tag");
    if (reader.MoveToAttribute("w"))
    {
      double num = (double) int.Parse(reader.Value) / 12700.0;
      border.Weight = num;
      border.IsWidthExist = true;
    }
    if (reader.MoveToAttribute("cmpd"))
    {
      Excel2007ShapeLineStyle excel2007ShapeLineStyle = (Excel2007ShapeLineStyle) Enum.Parse(typeof (Excel2007ShapeLineStyle), reader.Value, false);
      border.Style = (ExcelShapeLineStyle) excel2007ShapeLineStyle;
    }
    bool flag2 = false;
    bool flag3 = false;
    string key = (string) null;
    if (!flag1)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "noFill":
              border.Weight = 0.0;
              border.Visible = false;
              flag3 = true;
              reader.Read();
              continue;
            case "round":
              border.JoinType = Excel2007BorderJoinType.Round;
              border.HasBorderJoin = true;
              reader.Skip();
              continue;
            case "solidFill":
              ColorObject color = new ColorObject(ExcelKnownColors.None);
              ChartParserCommon.ParseSolidFill(reader, parser, color, border);
              border.ForeColor = color.GetRGB((IWorkbook) border.Workbook);
              flag2 = true;
              continue;
            case "prstDash":
              key = ChartParserCommon.ParseValueTag(reader);
              continue;
            case "pattFill":
              reader.Skip();
              continue;
            case "headEnd":
              TextBoxShapeParser.ParseArrowSettings(reader, border, true);
              continue;
            case "tailEnd":
              TextBoxShapeParser.ParseArrowSettings(reader, border, false);
              continue;
            case "miter":
              border.JoinType = Excel2007BorderJoinType.Mitter;
              if (reader.MoveToAttribute("lim"))
              {
                int result = 0;
                if (int.TryParse(reader.Value, out result))
                  border.MiterLim = result;
                else if (reader.Value.Contains("%") && int.TryParse(reader.Value.Replace("%", ""), out result))
                  border.MiterLim = result / 100;
              }
              border.HasBorderJoin = true;
              reader.Skip();
              continue;
            case "bevel":
              border.JoinType = Excel2007BorderJoinType.Bevel;
              border.HasBorderJoin = true;
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
    else
      flag2 = true;
    if (flag2)
    {
      border.DashStyle = ExcelShapeDashLineStyle.Solid;
      if (key != null)
      {
        ExcelShapeDashLineStyle shapeDashLineStyle;
        border.AppImplementation.StringEnum.LineDashTypeXmltoEnum.TryGetValue(key, out shapeDashLineStyle);
        border.DashStyle = shapeDashLineStyle;
      }
    }
    if (flag3)
      border.Visible = false;
    reader.Read();
  }

  private static void ParseArrowSettings(XmlReader reader, ShapeLineFormatImpl border, bool isHead)
  {
    if (reader.HasAttributes)
    {
      if (reader.MoveToAttribute("len"))
      {
        if (isHead)
          border.BeginArrowheadLength = TextBoxShapeParser.GetHeadLength(reader.Value);
        else
          border.EndArrowheadLength = TextBoxShapeParser.GetHeadLength(reader.Value);
      }
      if (reader.MoveToAttribute("type"))
      {
        if (isHead)
          border.BeginArrowHeadStyle = TextBoxShapeParser.GetHeadStyle(reader.Value);
        else
          border.EndArrowHeadStyle = TextBoxShapeParser.GetHeadStyle(reader.Value);
      }
      if (reader.MoveToAttribute("w") && isHead)
        border.BeginArrowheadWidth = TextBoxShapeParser.GetHeadWidth(reader.Value);
    }
    reader.Skip();
  }

  private static ExcelShapeArrowWidth GetHeadWidth(string value)
  {
    switch (value)
    {
      case "lg":
        return ExcelShapeArrowWidth.ArrowHeadWide;
      case "med":
        return ExcelShapeArrowWidth.ArrowHeadMedium;
      case "sm":
        return ExcelShapeArrowWidth.ArrowHeadNarrow;
      default:
        return ExcelShapeArrowWidth.ArrowHeadMedium;
    }
  }

  private static ExcelShapeArrowStyle GetHeadStyle(string value)
  {
    switch (value)
    {
      case "arrow":
        return ExcelShapeArrowStyle.LineArrowOpen;
      case "diamond":
        return ExcelShapeArrowStyle.LineArrowDiamond;
      case "none":
        return ExcelShapeArrowStyle.LineNoArrow;
      case "oval":
        return ExcelShapeArrowStyle.LineArrowOval;
      case "stealth":
        return ExcelShapeArrowStyle.LineArrowStealth;
      case "triangle":
        return ExcelShapeArrowStyle.LineArrow;
      default:
        return ExcelShapeArrowStyle.LineNoArrow;
    }
  }

  private static ExcelShapeArrowLength GetHeadLength(string value)
  {
    switch (value)
    {
      case "lg":
        return ExcelShapeArrowLength.ArrowHeadLong;
      case "med":
        return ExcelShapeArrowLength.ArrowHeadMedium;
      case "sm":
        return ExcelShapeArrowLength.ArrowHeadShort;
      default:
        return ExcelShapeArrowLength.ArrowHeadMedium;
    }
  }
}
