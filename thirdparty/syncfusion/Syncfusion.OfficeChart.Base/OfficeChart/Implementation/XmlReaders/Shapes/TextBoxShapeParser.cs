// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.XmlReaders.Shapes.TextBoxShapeParser
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.Compression;
using Syncfusion.OfficeChart.Implementation.Shapes;
using Syncfusion.OfficeChart.Implementation.XmlSerialization.Charts;
using Syncfusion.OfficeChart.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.XmlReaders.Shapes;

internal class TextBoxShapeParser
{
  public static void ParseTextBox(ITextBox textBox, XmlReader reader, Excel2007Parser parser)
  {
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    reader.Read();
    ShapeImpl shapeImpl = textBox as ShapeImpl;
    shapeImpl.HasLineFormat = false;
    shapeImpl.HasFill = false;
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
            TextBoxShapeParser.ParseShapeProperties(textBox, reader, parser);
            continue;
          case "nvSpPr":
            TextBoxShapeParser.ParseNonVisualShapeProperties(textBox as IShape, reader, parser);
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
    Excel2007Parser parser)
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
            IInternalFill fill = textBoxShapeImpl.Fill as IInternalFill;
            ChartParserCommon.ParseSolidFill(reader, parser, fill);
            continue;
          case "noFill":
            (textBoxShapeImpl.Fill as IInternalFill).Visible = false;
            reader.Read();
            continue;
          case "ln":
            ShapeLineFormatImpl line = (ShapeLineFormatImpl) textBoxShapeImpl.Line;
            TextBoxShapeParser.ParseLineProperties(reader, line, false, parser);
            continue;
          case "xfrm":
            if (reader.MoveToAttribute("rot"))
              textBoxShapeImpl.ShapeRotation = (int) (Convert.ToInt64(reader.Value) / 60000L);
            textBoxShapeImpl.Coordinates2007 = TextBoxShapeParser.ParseForm(reader);
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
      int x = 0;
      int y = 0;
      int width = 0;
      int height = 0;
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "off":
              if (reader.MoveToAttribute("x"))
                x = XmlConvertExtension.ToInt32(reader.Value);
              if (reader.MoveToAttribute("y"))
              {
                y = XmlConvertExtension.ToInt32(reader.Value);
                continue;
              }
              continue;
            case "ext":
              if (reader.MoveToAttribute("cx"))
                width = XmlConvertExtension.ToInt32(reader.Value);
              if (reader.MoveToAttribute("cy"))
              {
                height = XmlConvertExtension.ToInt32(reader.Value);
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
      form = new Rectangle(x, y, width, height);
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
      if (dictionary.ContainsKey("vert"))
      {
        TextBoxShapeParser.ParseTextRotation(dictionary["vert"], textBox);
        dictionary.Remove("vert");
      }
      dictionary.Remove("a");
      (textBox as TextBoxShapeBase).UnknownBodyProperties = dictionary;
    }
    reader.MoveToElement();
    reader.Skip();
  }

  private static void ParseTextRotation(string rotationValue, ITextBox textBox)
  {
    Excel2007TextRotation excel2007TextRotation = (Excel2007TextRotation) Enum.Parse(typeof (Excel2007TextRotation), rotationValue, false);
    textBox.TextRotation = (OfficeTextRotation) excel2007TextRotation;
  }

  private static void ParseAnchor(string anchorValue, ITextBox textBox)
  {
    Excel2007CommentVAlign excel2007CommentValign = (Excel2007CommentVAlign) Enum.Parse(typeof (Excel2007CommentVAlign), anchorValue, false);
    textBox.VAlignment = (OfficeCommentVAlign) excel2007CommentValign;
  }

  private static void ParseListStyles(XmlReader reader, RichTextString textArea)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textArea == null)
      throw new ArgumentNullException(nameof (textArea));
    if (reader.LocalName != "lstStyle")
      throw new XmlException("Unexpected xml tag.");
    reader.Skip();
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
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "pPr":
            TextBoxShapeParser.ParseParagraphProperites(reader, textBox);
            continue;
          case "r":
            TextBoxShapeParser.ParseParagraphRun(reader, richText, parser, textBox);
            continue;
          case "endParaRPr":
            TextBoxShapeParser.ParseParagraphEnd(reader, richText, parser);
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

  internal static void ParseParagraphEnd(
    XmlReader reader,
    RichTextString textArea,
    Excel2007Parser parser)
  {
    IOfficeFont paragraphRunProperites = (IOfficeFont) TextBoxShapeParser.ParseParagraphRunProperites(reader, textArea, parser);
    textArea.AddText("\n", paragraphRunProperites);
  }

  private static void ParseParagraphProperites(XmlReader reader, ITextBox textBox)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    if (reader.MoveToAttribute("algn"))
    {
      Excel2007CommentHAlign excel2007CommentHalign = (Excel2007CommentHAlign) Enum.Parse(typeof (Excel2007CommentHAlign), reader.Value, false);
      textBox.HAlignment = (OfficeCommentHAlign) excel2007CommentHalign;
    }
    reader.MoveToElement();
    reader.Skip();
  }

  internal static void ParseParagraphRun(
    XmlReader reader,
    RichTextString textArea,
    Excel2007Parser parser,
    ITextBox textBox)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textArea == null)
      throw new ArgumentNullException(nameof (textArea));
    if (reader.LocalName != "r")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    string text = (string) null;
    IOfficeFont font = (IOfficeFont) null;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "rPr":
            font = (IOfficeFont) TextBoxShapeParser.ParseParagraphRunProperites(reader, textArea, parser);
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
    reader.Read();
  }

  private static FontImpl ParseParagraphRunProperites(
    XmlReader reader,
    RichTextString textArea,
    Excel2007Parser parser)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    WorkbookImpl workbookImpl = textArea != null ? textArea.Workbook : throw new ArgumentNullException(nameof (textArea));
    FontImpl font = (FontImpl) workbookImpl.CreateFont(workbookImpl.InnerFonts[0], false);
    if (reader.MoveToAttribute("b"))
      font.Bold = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("i"))
      font.Italic = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("strike"))
      font.Strikethrough = reader.Value != "noStrike";
    if (reader.MoveToAttribute("sz"))
      font.Size = (double) int.Parse(reader.Value) / 100.0;
    if (reader.MoveToAttribute("u"))
    {
      if (reader.Value == "sng")
        font.Underline = OfficeUnderline.Single;
      else if (reader.Value == "dbl")
        font.Underline = OfficeUnderline.Double;
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
      int num = (int) Math.Round((double) int.Parse(reader.Value) / 12700.0);
      border.Weight = (double) num;
    }
    if (reader.MoveToAttribute("cmpd"))
    {
      Excel2007ShapeLineStyle excel2007ShapeLineStyle = (Excel2007ShapeLineStyle) Enum.Parse(typeof (Excel2007ShapeLineStyle), reader.Value, false);
      border.Style = (OfficeShapeLineStyle) excel2007ShapeLineStyle;
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
              border.Visible = false;
              reader.Read();
              continue;
            case "round":
              border.IsRound = true;
              reader.Skip();
              continue;
            case "solidFill":
              ChartColor color = new ChartColor(OfficeKnownColors.Black);
              ChartParserCommon.ParseSolidFill(reader, parser, color);
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
      border.DashStyle = OfficeShapeDashLineStyle.Solid;
      if (key != null)
      {
        OfficeShapeDashLineStyle shapeDashLineStyle;
        border.AppImplementation.StringEnum.LineDashTypeXmltoEnum.TryGetValue(key, out shapeDashLineStyle);
        border.DashStyle = shapeDashLineStyle;
      }
    }
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

  private static OfficeShapeArrowWidth GetHeadWidth(string value)
  {
    switch (value)
    {
      case "lg":
        return OfficeShapeArrowWidth.ArrowHeadWide;
      case "med":
        return OfficeShapeArrowWidth.ArrowHeadMedium;
      case "sm":
        return OfficeShapeArrowWidth.ArrowHeadNarrow;
      default:
        return OfficeShapeArrowWidth.ArrowHeadMedium;
    }
  }

  private static OfficeShapeArrowStyle GetHeadStyle(string value)
  {
    switch (value)
    {
      case "arrow":
        return OfficeShapeArrowStyle.LineArrowOpen;
      case "diamond":
        return OfficeShapeArrowStyle.LineArrowDiamond;
      case "none":
        return OfficeShapeArrowStyle.LineNoArrow;
      case "oval":
        return OfficeShapeArrowStyle.LineArrowOval;
      case "stealth":
        return OfficeShapeArrowStyle.LineArrowStealth;
      case "triangle":
        return OfficeShapeArrowStyle.LineArrow;
      default:
        return OfficeShapeArrowStyle.LineNoArrow;
    }
  }

  private static OfficeShapeArrowLength GetHeadLength(string value)
  {
    switch (value)
    {
      case "lg":
        return OfficeShapeArrowLength.ArrowHeadLong;
      case "med":
        return OfficeShapeArrowLength.ArrowHeadMedium;
      case "sm":
        return OfficeShapeArrowLength.ArrowHeadShort;
      default:
        return OfficeShapeArrowLength.ArrowHeadMedium;
    }
  }
}
