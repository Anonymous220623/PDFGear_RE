// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.RichText.RichTextParser
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Office;
using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.SlideImplementation;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace Syncfusion.Presentation.RichText;

internal class RichTextParser
{
  internal static void ParseTextBody(XmlReader reader, ITextBody textFrame1)
  {
    TextBody textFrame = (TextBody) textFrame1;
    Paragraphs paragraphs = (Paragraphs) textFrame.Paragraphs;
    bool isFirstRun = true;
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
            case "bodyPr":
              RichTextParser.ParseBodyProperties(reader, textFrame);
              continue;
            case "p":
              Paragraph collection = new Paragraph(paragraphs);
              RichTextParser.ParseTextParagraph(reader, collection, isFirstRun);
              paragraphs.Add((IParagraph) collection);
              if (isFirstRun)
              {
                isFirstRun = false;
                continue;
              }
              continue;
            case "lstStyle":
              RichTextParser.ParseListStyle(reader, textFrame);
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

  private static int AddTextRun(Paragraph collection, int i, TextPart textRun)
  {
    if (collection.TextParts.Count > 0)
    {
      TextPart textPart = (TextPart) collection.TextParts[i];
      if (textPart != null && textPart.Compare((ITextPart) textRun))
        textPart.Text += textRun.Text;
      else
        collection.Add(textRun);
    }
    else
      collection.Add(textRun);
    return collection.TextParts.Count - 1;
  }

  private static void ParseBodyProperties(XmlReader reader, TextBody textFrame)
  {
    if (reader.HasAttributes)
    {
      int minValue1 = int.MinValue;
      int minValue2 = int.MinValue;
      int minValue3 = int.MinValue;
      int minValue4 = int.MinValue;
      while (reader.MoveToNextAttribute())
      {
        switch (reader.LocalName)
        {
          case "numCol":
            textFrame.NumberOfColumns = Helper.ToInt(reader.Value);
            continue;
          case "spcCol":
            textFrame.SpaceBetweenColumns = Helper.EmuToPoint(Helper.ToLong(reader.Value));
            continue;
          case "rtlCol":
            textFrame.RTLColumns = Helper.ToBoolean(reader.Value);
            continue;
          case "lIns":
            minValue1 = Helper.ToInt(reader.Value);
            textFrame.IsAutoMargins = false;
            continue;
          case "tIns":
            minValue2 = Helper.ToInt(reader.Value);
            textFrame.IsAutoMargins = false;
            continue;
          case "rIns":
            minValue3 = Helper.ToInt(reader.Value);
            textFrame.IsAutoMargins = false;
            continue;
          case "bIns":
            minValue4 = Helper.ToInt(reader.Value);
            textFrame.IsAutoMargins = false;
            continue;
          case "vertOverflow":
            textFrame.TextVerticalOverflow = Helper.GetTextOverflowType(reader.Value);
            continue;
          case "horzOverflow":
            textFrame.TextHorizontalOverflow = Helper.GetTextOverflowType(reader.Value);
            continue;
          case "anchor":
            textFrame.SetVerticalAlign(Helper.GetVerticalAlignType(reader.Value));
            continue;
          case "vert":
            textFrame.SetTextDirection(Helper.GetTextDirection(reader.Value));
            continue;
          case "wrap":
            textFrame.WrapText = reader.Value == null || reader.Value != "none";
            continue;
          case "anchorCtr":
            textFrame.AnchorCenter = reader.Value == "1";
            continue;
          case "rot":
            int num = Helper.ToInt(reader.Value);
            textFrame.Rotation = num >= 0 ? num : 21600000 + num;
            continue;
          default:
            continue;
        }
      }
      textFrame.SetMargin(minValue1, minValue2, minValue3, minValue4);
    }
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
            case "normAutofit":
              textFrame.AutoFitType = AutoMarginType.NormalAutoFit;
              textFrame.IsAutoSize = true;
              if (reader.MoveToAttribute("fontScale"))
              {
                textFrame.SetFontScale(Helper.ToInt(reader.Value));
                textFrame.HasFontScale = true;
              }
              if (reader.MoveToAttribute("lnSpcReduction"))
                textFrame.SetLnSpcReductionValue(Helper.ToInt(reader.Value));
              reader.Skip();
              continue;
            case "noAutofit":
              textFrame.AutoFitType = AutoMarginType.NoAutoFit;
              reader.Skip();
              continue;
            case "spAutoFit":
              textFrame.AutoFitType = AutoMarginType.TextShapeAutoFit;
              reader.Skip();
              continue;
            default:
              textFrame.PreservedElements.Add(reader.LocalName, UtilityMethods.ReadSingleNodeIntoStream(reader));
              continue;
          }
        }
        else
          reader.Skip();
      }
    }
    reader.Read();
  }

  private static void ParseBulletPicture(XmlReader reader, ListFormat bulletFormat)
  {
    if (reader.IsEmptyElement)
      return;
    while (reader.LocalName != "blip")
      reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (reader.LocalName == "blip" && reader.MoveToAttribute("embed", "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
        {
          bulletFormat.ImageRelationId = reader.Value;
          bulletFormat.AddImageStream();
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  internal static void ParseListStyle(XmlReader reader, TextBody textFrame)
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
            case "defPPr":
              RichTextParser.ParseParagraphStyle(reader, "defPPr", textFrame);
              continue;
            case "lvl1pPr":
              RichTextParser.ParseParagraphStyle(reader, "lvl1pPr", textFrame);
              continue;
            case "lvl2pPr":
              RichTextParser.ParseParagraphStyle(reader, "lvl2pPr", textFrame);
              continue;
            case "lvl3pPr":
              RichTextParser.ParseParagraphStyle(reader, "lvl3pPr", textFrame);
              continue;
            case "lvl4pPr":
              RichTextParser.ParseParagraphStyle(reader, "lvl4pPr", textFrame);
              continue;
            case "lvl5pPr":
              RichTextParser.ParseParagraphStyle(reader, "lvl5pPr", textFrame);
              continue;
            case "lvl6pPr":
              RichTextParser.ParseParagraphStyle(reader, "lvl6pPr", textFrame);
              continue;
            case "lvl7pPr":
              RichTextParser.ParseParagraphStyle(reader, "lvl7pPr", textFrame);
              continue;
            case "lvl8pPr":
              RichTextParser.ParseParagraphStyle(reader, "lvl8pPr", textFrame);
              continue;
            case "lvl9pPr":
              RichTextParser.ParseParagraphStyle(reader, "lvl9pPr", textFrame);
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

  private static void ParseParagraphStyle(XmlReader reader, string name, TextBody textFrame)
  {
    Paragraph collection = new Paragraph((Paragraphs) textFrame.Paragraphs);
    collection.IsWithinList = true;
    RichTextParser.ParseTextParagraphProperties(reader, collection);
    textFrame.StyleList.Add(name, collection);
  }

  private static TextPart ParseRegularTextRun(XmlReader reader, Paragraph collection)
  {
    TextPart textPart = new TextPart(collection);
    string localName = reader.LocalName;
    if (reader.HasAttributes)
    {
      if (reader.MoveToAttribute("id"))
        textPart.UniqueId = reader.Value;
      if (reader.MoveToAttribute("type"))
        textPart.Type = reader.Value;
    }
    int content = (int) reader.MoveToContent();
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "rPr":
              Font font = new Font(textPart);
              RichTextParser.ParseTextCharacterProperties(reader, font, textPart);
              textPart.SetFont(font);
              continue;
            case "t":
              textPart.Text = reader.ReadElementContentAsString();
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
    return textPart;
  }

  private static Font ParseTextCharacterProperties(
    XmlReader xmlReader,
    Font font,
    TextPart textPart)
  {
    if (xmlReader.HasAttributes)
    {
      while (xmlReader.MoveToNextAttribute())
      {
        switch (xmlReader.LocalName)
        {
          case "lang":
            string str = xmlReader.Value;
            if (RichTextParser.IsEnumDefined(ref str))
            {
              font.LanguageID = (short) (LocaleIDs) Enum.Parse(typeof (LocaleIDs), str.Replace('-', '_'));
              continue;
            }
            short languageId = Helper.GetLanguageID(str);
            if (languageId != (short) 1033)
            {
              font.LanguageID = languageId;
              continue;
            }
            continue;
          case "altLang":
            font.AltLanguage = xmlReader.Value;
            continue;
          case "sz":
            font.SetFontSize(Helper.ToInt(xmlReader.Value));
            continue;
          case "b":
            font.Bold = xmlReader.Value == "1";
            font.HasBoldValue = true;
            continue;
          case "i":
            font.Italic = xmlReader.Value == "1";
            continue;
          case "u":
            font.Underline = (TextUnderlineType) Enum.Parse(typeof (TextUnderlineType), Helper.GetFontUnderlineType(xmlReader.Value).ToString(), true);
            continue;
          case "strike":
            font.StrikeType = Helper.GetStrikeType(xmlReader.Value);
            continue;
          case "kern":
            font.Kerning = new int?(Helper.ToInt(xmlReader.Value));
            continue;
          case "cap":
            font.CapsType = Helper.GetTextCapsType(xmlReader.Value);
            continue;
          case "spc":
            font.AssignCharacterSpacing(Helper.ToInt(xmlReader.Value));
            continue;
          case "baseline":
            double num = Helper.ToDouble(xmlReader.Value);
            if (num != 0.0)
            {
              if (num > 0.0)
                font.Superscript = true;
              if (num < 0.0)
                font.Subscript = true;
              font.SetBaseLine(num);
              continue;
            }
            continue;
          case "err":
            font.IsSpellingError = xmlReader.Value == "1";
            continue;
          default:
            continue;
        }
      }
      xmlReader.MoveToElement();
    }
    if (!xmlReader.IsEmptyElement)
    {
      xmlReader.Read();
      while (xmlReader.NodeType != XmlNodeType.EndElement)
      {
        if (xmlReader.NodeType == XmlNodeType.Element)
        {
          switch (xmlReader.LocalName)
          {
            case "solidFill":
              font.Fill.FillType = FillType.Solid;
              font.IsColorSet = true;
              ColorObject colorObject1 = new ColorObject();
              BaseSlide baseSlide = font.Paragraph.Parent.TextBody.BaseSlide;
              MasterSlide masterSlide1 = baseSlide as MasterSlide;
              LayoutSlide layoutSlide = baseSlide as LayoutSlide;
              ColorObject colorObject2;
              if (masterSlide1 == null && layoutSlide == null)
              {
                MasterSlide masterSlide2 = (MasterSlide) font.Paragraph.Parent.TextBody.Presentation.Masters[0];
                if (baseSlide is Slide && (baseSlide as Slide).LayoutSlide != null && (baseSlide as Slide).LayoutSlide.MasterSlide != null)
                  masterSlide2 = (baseSlide as Slide).LayoutSlide.MasterSlide as MasterSlide;
                colorObject2 = DrawingParser.ParseColorChoice(xmlReader, masterSlide2);
              }
              else
                colorObject2 = layoutSlide == null || layoutSlide.ColorMap.Count == 0 ? DrawingParser.ParseColorChoice(xmlReader, masterSlide1) : DrawingParser.ParseColorChoice(xmlReader, layoutSlide);
              SolidFill solidFill = (SolidFill) font.Fill.SolidFill;
              font.SetColorObject(colorObject2);
              continue;
            case "gradFill":
              font.Fill.FillType = FillType.Gradient;
              GradientFill gradientFill = (GradientFill) font.Fill.GradientFill;
              DrawingParser.ParseGradientFill(xmlReader, gradientFill);
              continue;
            case "noFill":
              font.Fill.FillType = FillType.None;
              xmlReader.Skip();
              continue;
            case "uFill":
              RichTextParser.ParseUnderlineColor(xmlReader, font);
              xmlReader.Skip();
              continue;
            case "effectLst":
              Paragraph paragraph = font.Paragraph;
              font.EffectList = new EffectList(paragraph.BaseSlide.Presentation);
              Parser.ParseEffectList(xmlReader, font.EffectList);
              xmlReader.Skip();
              continue;
            case "sym":
              if (xmlReader.MoveToAttribute("typeface"))
                font.SetSymbolFontName(xmlReader.Value);
              xmlReader.Skip();
              continue;
            case "latin":
              if (xmlReader.MoveToAttribute("typeface"))
                font.SetName(xmlReader.Value);
              xmlReader.Skip();
              continue;
            case "ea":
              if (xmlReader.MoveToAttribute("typeface"))
                font.SetEastAsianFontName(xmlReader.Value);
              xmlReader.Skip();
              continue;
            case "cs":
              if (xmlReader.MoveToAttribute("typeface"))
                font.FontNameBidi = xmlReader.Value;
              xmlReader.Skip();
              continue;
            case "rtl":
              font.RightToLeft = new bool?(true);
              if (xmlReader.MoveToAttribute("val"))
              {
                string str = xmlReader.Value;
                if (str == "0" || str == "false" || str == "off")
                  font.RightToLeft = new bool?(false);
              }
              xmlReader.Skip();
              continue;
            case "hlinkClick":
              if (textPart != null)
              {
                Hyperlink hyperlink = new Hyperlink(textPart);
                textPart.SetHyperlink(DrawingParser.ParseHyperlink(xmlReader, hyperlink));
              }
              xmlReader.Skip();
              continue;
            default:
              font.PreservedElements.Add(xmlReader.LocalName, UtilityMethods.ReadSingleNodeIntoStream(xmlReader));
              continue;
          }
        }
        else
          xmlReader.Read();
      }
    }
    xmlReader.Read();
    return font;
  }

  private static void ParseUnderlineColor(XmlReader reader, Font font)
  {
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (reader.IsEmptyElement)
      return;
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "solidFill":
            if (!(font.Paragraph.Parent.TextBody.BaseSlide is MasterSlide masterSlide))
              masterSlide = (MasterSlide) font.Paragraph.Parent.TextBody.Presentation.Masters[0];
            font.SetUnderlineColorObject(DrawingParser.ParseColorChoice(reader, masterSlide));
            continue;
          default:
            continue;
        }
      }
      else
        reader.Skip();
    }
  }

  private static bool IsEnumDefined(ref string value)
  {
    value = value.Replace('-', '_');
    if (Enum.IsDefined(typeof (LocaleIDs), (object) value))
      return true;
    string[] strArray = value.Split('_');
    if (strArray.Length == 2)
    {
      string formattedValue1 = $"{strArray[0].ToLower()}_{strArray[1].ToUpper()}";
      if (RichTextParser.IsEnumDefined(ref value, formattedValue1))
        return true;
      string formattedValue2 = $"{strArray[0].ToLower()}_{RichTextParser.FirstLetterToUpper(strArray[1])}";
      if (RichTextParser.IsEnumDefined(ref value, formattedValue2))
        return true;
    }
    else if (strArray.Length == 3)
    {
      string formattedValue3 = $"{strArray[0].ToLower()}_{RichTextParser.FirstLetterToUpper(strArray[1])}_{strArray[2].ToUpper()}";
      if (RichTextParser.IsEnumDefined(ref value, formattedValue3))
        return true;
      string formattedValue4 = $"{strArray[0].ToLower()}_{strArray[1].ToLower()}_{strArray[2].ToUpper()}";
      if (RichTextParser.IsEnumDefined(ref value, formattedValue4))
        return true;
      string formattedValue5 = $"{strArray[0].ToLower()}_{strArray[1].ToUpper()}_{strArray[2].ToLower()}";
      if (RichTextParser.IsEnumDefined(ref value, formattedValue5))
        return true;
    }
    return false;
  }

  private static bool IsEnumDefined(ref string value, string formattedValue)
  {
    if (!Enum.IsDefined(typeof (LocaleIDs), (object) formattedValue))
      return false;
    value = formattedValue;
    return true;
  }

  private static string FirstLetterToUpper(string str)
  {
    return str.Length > 1 ? char.ToUpper(str[0]).ToString() + str.Substring(1) : str;
  }

  private static void ParseTextParagraph(XmlReader reader, Paragraph collection, bool isFirstRun)
  {
    int i = 0;
    TextPart textPart = (TextPart) null;
    bool flag = false;
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
            case "pPr":
              RichTextParser.ParseTextParagraphProperties(reader, collection);
              if (((ListFormat) collection.ListFormat).GetListType() == ListType.Numbered)
              {
                flag = true;
                collection.SetLevelValue();
                collection.SetBulletCharacter((ListFormat) collection.ListFormat);
                continue;
              }
              continue;
            case "r":
              TextPart regularTextRun1 = RichTextParser.ParseRegularTextRun(reader, collection);
              i = RichTextParser.AddTextRun(collection, i, regularTextRun1);
              if (textPart == null)
              {
                textPart = regularTextRun1;
                continue;
              }
              continue;
            case "fld":
              TextPart regularTextRun2 = RichTextParser.ParseRegularTextRun(reader, collection);
              i = RichTextParser.AddTextRun(collection, i, regularTextRun2);
              continue;
            case "br":
              TextPart regularTextRun3 = RichTextParser.ParseRegularTextRun(reader, collection);
              regularTextRun3.SetLineBreakProps((Font) regularTextRun3.Font);
              i = RichTextParser.AddTextRun(collection, i, regularTextRun3);
              if (textPart == null)
              {
                textPart = regularTextRun3;
                continue;
              }
              continue;
            case "defRPr":
              if (reader.AttributeCount != 0 || !reader.IsEmptyElement)
              {
                collection.SetFont(RichTextParser.ParseTextCharacterProperties(reader, new Font(collection), (TextPart) null));
                continue;
              }
              reader.Skip();
              continue;
            case "endParaRPr":
              collection.SetIsLastPara(true);
              Font characterProperties = RichTextParser.ParseTextCharacterProperties(reader, new Font(collection), (TextPart) null);
              characterProperties.LastParaTextPart = true;
              collection.SetEndParaProps(characterProperties);
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          reader.Read();
      }
      if (!isFirstRun && textPart == null)
        collection.Add(new TextPart(collection)
        {
          Text = ""
        });
    }
    reader.Read();
    if (((ListFormat) collection.ListFormat).GetDefaultListType() != ListType.Numbered || collection.GetTextBody().StyleList == null || collection.GetTextBody().StyleList.Count <= 0 || flag)
      return;
    collection.SetLevelValue();
    ((ListFormat) collection.ListFormat).AssignType(ListType.Numbered);
    ((ListFormat) collection.ListFormat).IsTypeChanged = true;
    string key = $"lvl{(object) (collection.IndentLevelNumber + 1)}pPr";
    if (collection.GetTextBody().StyleList.ContainsKey(key))
    {
      ListFormat listFormat = (ListFormat) collection.GetTextBody().StyleList[key].ListFormat;
      ((ListFormat) collection.ListFormat).NumberStyle = listFormat.NumberStyle;
    }
    collection.SetBulletCharacter((ListFormat) collection.ListFormat);
  }

  private static void ParseTextParagraphProperties(XmlReader reader, Paragraph collection)
  {
    if (reader.MoveToAttribute("rtl"))
    {
      switch (reader.Value)
      {
        case null:
          break;
        case "1":
          collection.RightToLeft = new bool?(true);
          break;
        default:
          collection.RightToLeft = new bool?(false);
          break;
      }
    }
    if (reader.MoveToAttribute("marL") && reader.Value != null)
      collection.SetMarginLeft(Helper.ToInt(reader.Value));
    if (reader.MoveToAttribute("marR") && reader.Value != null)
      collection.SetMarginRight(Helper.ToInt(reader.Value));
    if (reader.MoveToAttribute("indent") && reader.Value != null)
      collection.SetIndent(Helper.ToInt(reader.Value));
    if (reader.MoveToAttribute("algn"))
    {
      string str = reader.Value;
      if (str != null)
        collection.SetAlignmentType(Helper.GetHorizontalAlignType(str));
    }
    if (reader.MoveToAttribute("fontAlgn"))
    {
      string str = reader.Value;
      if (str != null)
        collection.SetFontAlignType(Helper.GetFontAlignType(str));
    }
    if (reader.MoveToAttribute("defTabSz") && reader.Value != null)
      collection.SetDefaultTabSize(Helper.ToLong(reader.Value));
    if (reader.MoveToAttribute("lvl"))
    {
      if (reader.Value != null)
        collection.SetIndentLevel(Helper.ToInt(reader.Value));
    }
    else
      collection.SetIndentLevel(0);
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      string localName = reader.LocalName;
      reader.Read();
      ListFormat listFormat = (ListFormat) collection.ListFormat;
      while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "lnSpc":
              collection.SetLineSpacing(RichTextParser.ParseTextSpacing(reader, collection, "lnSpc"));
              reader.Skip();
              continue;
            case "spcBef":
              collection.SetSpaceBefore(RichTextParser.ParseTextSpacing(reader, collection, "spcBef"));
              reader.Skip();
              continue;
            case "spcAft":
              collection.SetSpaceAfter(RichTextParser.ParseTextSpacing(reader, collection, "spcAft"));
              reader.Skip();
              continue;
            case "defRPr":
              if (reader.AttributeCount != 0 || !reader.IsEmptyElement)
              {
                collection.SetFont(RichTextParser.ParseTextCharacterProperties(reader, new Font(collection), (TextPart) null));
                continue;
              }
              reader.Skip();
              continue;
            case "tabLst":
              collection.TabPositionList = new List<long>();
              collection.TabAlignmentTypeList = new List<TabAlignmentType>();
              RichTextParser.ParseTabList(reader, collection);
              reader.Skip();
              continue;
            case "buSzPct":
              if (reader.MoveToAttribute("val"))
              {
                listFormat.SetBulletSize(Helper.ToInt(reader.Value), SizeType.Percentage);
                listFormat.IsSizeChanged = true;
              }
              reader.Skip();
              continue;
            case "buSzPts":
              if (reader.MoveToAttribute("val"))
              {
                listFormat.SetBulletSize(Helper.ToInt(reader.Value), SizeType.Points);
                listFormat.IsSizeChanged = true;
              }
              reader.Skip();
              continue;
            case "buClr":
              if (!(listFormat.Paragraph.GetTextBody().BaseSlide is MasterSlide masterSlide))
                masterSlide = (MasterSlide) listFormat.Paragraph.GetTextBody().BaseSlide.Presentation.Masters[0];
              listFormat.SetColorObject(DrawingParser.ParseColorChoice(reader, masterSlide));
              continue;
            case "buClrTx":
              listFormat.IsTextColor = true;
              reader.Skip();
              continue;
            case "buFont":
              if (reader.MoveToAttribute("typeface"))
                listFormat.FontName = reader.Value;
              reader.Skip();
              continue;
            case "buNone":
              listFormat.AssignType(ListType.None);
              reader.Skip();
              continue;
            case "buAutoNum":
              listFormat.AssignType(ListType.Numbered);
              if (reader.MoveToAttribute("type"))
                listFormat.NumberStyle = Helper.GetNumberStyle(reader.Value);
              if (reader.MoveToAttribute("startAt"))
                listFormat.StartValue = Helper.ToInt(reader.Value);
              reader.Skip();
              continue;
            case "buFontTx":
              listFormat.IsTextFont = true;
              reader.Skip();
              continue;
            case "buSzTx":
              listFormat.IsTextSize = true;
              reader.Skip();
              continue;
            case "buChar":
              listFormat.AssignType(ListType.Bulleted);
              if (reader.MoveToAttribute("char"))
              {
                listFormat.HasBulletCharacter = true;
                listFormat.SetCharacter(reader.Value);
              }
              reader.Skip();
              continue;
            case "buBlip":
              RichTextParser.ParseBulletPicture(reader, listFormat);
              listFormat.AssignType(ListType.Picture);
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

  private static void ParseTabList(XmlReader reader, Paragraph collection)
  {
    if (reader.IsEmptyElement)
      return;
    long num = 0;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (reader.LocalName == "tab")
        {
          if (reader.MoveToAttribute("pos"))
            num = Helper.ToLong(reader.Value);
          if (reader.MoveToAttribute("algn"))
          {
            if (reader.Value != null)
            {
              collection.TabPositionList.Add(num);
              collection.TabAlignmentTypeList.Add(Helper.GetTabAlignType(reader.Value));
            }
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

  private static int ParseTextSpacing(XmlReader reader, Paragraph collection, string tag)
  {
    string localName = reader.LocalName;
    reader.Read();
    int textSpacing = 0;
    SizeType type = SizeType.None;
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "spcPct":
            if (reader.MoveToAttribute("val"))
            {
              textSpacing = Helper.ToInt(reader.Value);
              type = SizeType.Percentage;
              reader.Skip();
              continue;
            }
            continue;
          case "spcPts":
            if (reader.MoveToAttribute("val"))
            {
              textSpacing = Helper.ToInt(reader.Value);
              type = SizeType.Points;
              reader.Skip();
              continue;
            }
            continue;
          default:
            continue;
        }
      }
      else
        reader.Skip();
    }
    RichTextParser.SetValue(collection, tag, type);
    return textSpacing;
  }

  private static void SetValue(Paragraph collection, string tag, SizeType type)
  {
    switch (tag)
    {
      case "lnSpc":
        collection.LineSpacingType = type;
        break;
      case "spcBef":
        collection.SpaceBeforeType = type;
        break;
      case "spcAft":
        collection.SpaceAfterType = type;
        break;
    }
  }
}
