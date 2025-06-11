// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODTConvertion.DocToODTConverter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ODF.Base;
using Syncfusion.DocIO.ODF.Base.ODFImplementation;
using Syncfusion.DocIO.ODF.Base.ODFSerialization;
using Syncfusion.DocIO.ODFConverter.Base.ODFImplementation;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.Office;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.DocIO.ODTConvertion;

internal class DocToODTConverter
{
  private WordDocument m_document;
  internal ODocument m_oDocument = new ODocument();
  internal OParagraph paragraph;
  internal OParagraphItem opara;
  internal ODFStyle odfStyle;
  private ODFWriter m_writer;
  private ODFStyleCollection odfstyleCollection1 = new ODFStyleCollection();
  private int m_docPrId;
  private List<string> pageNames;
  private BeforeBreak m_lastBreak;
  private byte m_flag;
  private int m_relationShipID;

  internal bool IsWritingHeaderFooter
  {
    get => ((int) this.m_flag & 1) != 0;
    set => this.m_flag = (byte) ((int) this.m_flag & 254 | (value ? 1 : 0));
  }

  public DocToODTConverter(WordDocument document)
  {
    this.m_document = document;
    this.m_writer = new ODFWriter();
    this.pageNames = new List<string>();
  }

  internal void ConvertToODF(string filename)
  {
    this.m_writer.SerializeMetaData();
    this.m_writer.SerializeMimeType();
    this.m_writer.SerializeSettings();
    this.MapDocumentStyles();
    this.MapContent();
    this.m_writer.SerializeDocumentManifest();
    this.m_writer.SaveDocument(filename);
    this.Close();
  }

  internal void ConvertToODF(Stream stream)
  {
    this.m_writer.SerializeMetaData();
    this.m_writer.SerializeMimeType();
    this.m_writer.SerializeSettings();
    this.MapDocumentStyles();
    this.MapContent();
    this.m_writer.SerializeDocumentManifest();
    this.m_writer.SaveDocument(stream);
    this.Close();
  }

  internal void MapDocumentStyles()
  {
    MemoryStream stream = this.m_writer.SerializeStyleStart();
    this.ConvertFontFace();
    this.m_writer.SerializeDataStylesStart();
    this.m_writer.SerializeTableDefaultStyle();
    this.ConvertDefaultStyles();
    this.m_writer.SerializeEnd();
    this.ConvertAutomaticAndMasterStyles();
    this.m_writer.SerializeStylesEnd(stream);
  }

  internal void ConvertFontFace()
  {
    List<FontFace> fonts = new List<FontFace>();
    if (this.m_document.FFNStringTable != null && this.m_document.FFNStringTable.FontFamilyNameRecords != null)
    {
      foreach (FontFamilyNameRecord familyNameRecord in this.m_document.FFNStringTable.FontFamilyNameRecords)
      {
        IStyleCollection styles = this.m_document.Styles;
        string[] strArray = familyNameRecord.FontName.Split(new char[1]);
        FontFace fontFace = new FontFace(strArray[0]);
        fontFace.Name = strArray[0];
        FontFamilyGeneric fontFamilyId = (FontFamilyGeneric) familyNameRecord.FontFamilyID;
        fontFace.FontFamilyGeneric = fontFamilyId;
        fontFace.FontPitch = (FontPitch) familyNameRecord.PitchRequest;
        fonts.Add(fontFace);
      }
    }
    this.m_writer.SerializeFontFaceDecls(fonts);
  }

  internal void MapContent()
  {
    MemoryStream stream = this.m_writer.SerializeContentNameSpace();
    this.ConvertFontFace();
    this.GetBody();
    this.m_writer.SerializeContentEnd(stream);
  }

  private TextProperties CopyCharFormatToTextFormat(
    WCharacterFormat charFormat,
    TextProperties textProp)
  {
    if (charFormat == null)
      return (TextProperties) null;
    if (charFormat.PropertiesHash.Count > 0)
      textProp = new TextProperties();
    if (textProp != null)
    {
      if ((double) charFormat.Position != 0.0)
      {
        float num = charFormat.Position * 100f / charFormat.FontSize;
        textProp.TextPosition = num.ToString() + "% 100%";
      }
      if (charFormat.CharStyleName != null)
        textProp.CharStyleName = charFormat.CharStyleName;
      if (charFormat.HasValue(53) && charFormat.Hidden)
        textProp.IsTextDisplay = false;
      if (charFormat.HasValue(10))
      {
        switch (charFormat.SubSuperScript)
        {
          case SubSuperScript.SuperScript:
            textProp.TextPosition = "super 63.6%";
            break;
          case SubSuperScript.SubScript:
            textProp.TextPosition = "sub 63.6%";
            break;
        }
      }
      if (charFormat.HasValue(2))
      {
        textProp.FontFamily = charFormat.Font.FontFamily.Name;
        textProp.FontName = charFormat.FontName;
      }
      if (charFormat.HasValue(3))
        textProp.FontSize = (double) charFormat.FontSize;
      if (charFormat.HasValue(4) && charFormat.Bold)
      {
        textProp.FontWeight = FontWeight.bold;
        textProp.FontWeightAsian = FontWeight.bold;
        textProp.FontWeightComplex = charFormat.Bold ? FontWeight.bold : FontWeight.normal;
      }
      if (charFormat.HasValue((int) sbyte.MaxValue))
        textProp.TextScale = charFormat.Scaling;
      if (charFormat.HasValue(1))
        textProp.Color = charFormat.TextColor;
      if (charFormat.HasValue(5) && charFormat.Italic)
        textProp.FontStyle = ODFFontStyle.italic;
      if (charFormat.HasValue(18))
        textProp.LetterSpacing = charFormat.CharacterSpacing;
      if (charFormat.HasValue(50) && charFormat.Shadow)
        textProp.Shadow = charFormat.Shadow;
      if (charFormat.HasValue(55) && charFormat.SmallCaps)
        textProp.TextTransform = Transform.uppercase;
      else if (charFormat.HasValue(54) && charFormat.AllCaps)
        textProp.TextTransform = Transform.uppercase;
      if (charFormat.HasValue(71) && charFormat.OutLine)
        textProp.TextOutline = true;
      if (charFormat.HasValue(125))
        textProp.LetterKerning = charFormat.IsKernFont;
      if (charFormat.HasValue(6) || charFormat.HasValue(14))
      {
        textProp.LinethroughStyle = BorderLineStyle.solid;
        textProp.LinethroughColor = this.GetColor(charFormat.TextColor);
        textProp.LinethroughType = !charFormat.HasValue(14) || !charFormat.DoubleStrike ? (!charFormat.HasValue(6) || !charFormat.Strikeout ? LineType.none : (textProp.LinethroughType = LineType.single)) : LineType.Double;
      }
      if (charFormat.HasValue(63 /*0x3F*/))
        textProp.BackgroundColor = charFormat.HighlightColor;
      if (charFormat.HasValue(7))
      {
        textProp.TextUnderlineStyle = this.GetUnderlineStyle(charFormat.UnderlineStyle);
        textProp.TextUnderlineColor = this.GetColor(charFormat.TextBackgroundColor);
      }
      if (charFormat.HasValue(24))
      {
        textProp.TextDisplay = TextDisplay.none;
        textProp.IsTextDisplay = true;
      }
      if (charFormat.HasValue(51) && charFormat.Emboss)
        textProp.FontRelief = FontRelief.embossed;
    }
    return textProp;
  }

  private BorderLineStyle GetUnderlineStyle(UnderlineStyle charstyle)
  {
    BorderLineStyle underlineStyle = BorderLineStyle.none;
    switch (charstyle)
    {
      case UnderlineStyle.Single:
        underlineStyle = BorderLineStyle.solid;
        break;
      case UnderlineStyle.Dotted:
        underlineStyle = BorderLineStyle.dotted;
        break;
      case UnderlineStyle.Dash:
        underlineStyle = BorderLineStyle.dashed;
        break;
      case UnderlineStyle.DotDash:
        underlineStyle = BorderLineStyle.dotdash;
        break;
      case UnderlineStyle.DotDotDash:
        underlineStyle = BorderLineStyle.dotdotdash;
        break;
      case UnderlineStyle.Wavy:
        underlineStyle = BorderLineStyle.wave;
        break;
      case UnderlineStyle.DashLong:
        underlineStyle = BorderLineStyle.longdash;
        break;
    }
    return underlineStyle;
  }

  private ODFParagraphProperties CopyParaFormatToParagraphPropertiesFormat(
    WParagraphFormat paraformat)
  {
    ODFParagraphProperties propertiesFormat = (ODFParagraphProperties) null;
    if (paraformat != null)
    {
      if (paraformat.HasValue(21) || paraformat.HasValue(6) || paraformat.HasValue(10) || !paraformat.Borders.NoBorder || paraformat.HasValue(5) || paraformat.HasValue(52) || paraformat.HasValue(8) || paraformat.HasValue(9) || paraformat.HasValue(2) || paraformat.HasValue(3) || paraformat.HasKey(58) || paraformat.HasKey(57) || paraformat.Borders.Left.IsBorderDefined && !paraformat.Borders.Left.IsDefault || paraformat.Borders.Right.IsBorderDefined && !paraformat.Borders.Right.IsDefault || paraformat.Borders.Top.IsBorderDefined && !paraformat.Borders.Top.IsDefault || paraformat.Borders.Bottom.IsBorderDefined && !paraformat.Borders.Bottom.IsDefault || paraformat.HasValue(0) || paraformat.HasValue(31 /*0x1F*/) || paraformat.Tabs != null && paraformat.Tabs.Count > 0)
        propertiesFormat = new ODFParagraphProperties();
      if (paraformat.Borders.Top.BorderType != BorderStyle.None && paraformat.Borders.Top.BorderType == paraformat.Borders.Left.BorderType && paraformat.Borders.Top.BorderType == paraformat.Borders.Bottom.BorderType && paraformat.Borders.Top.BorderType == paraformat.Borders.Right.BorderType)
      {
        propertiesFormat.Border = new ODFBorder();
        if ((double) paraformat.Borders.Top.Space > 0.0)
        {
          propertiesFormat.PaddingTop = paraformat.Borders.Top.Space / 72f;
          propertiesFormat.PaddingBottom = paraformat.Borders.Bottom.Space / 72f;
          propertiesFormat.PaddingLeft = paraformat.Borders.Left.Space / 72f;
          propertiesFormat.PaddingRight = paraformat.Borders.Right.Space / 72f;
        }
        float num = paraformat.Borders.Top.LineWidth / 72f;
        propertiesFormat.Border.LineWidth = num.ToString();
        propertiesFormat.Border.LineStyle = this.GetUnderlineStyle(paraformat.Borders.Top.BorderType);
        propertiesFormat.Border.LineColor = paraformat.Borders.Top.Color;
      }
      else
      {
        if (paraformat.Borders.Left.IsBorderDefined && !paraformat.Borders.Left.IsDefault)
        {
          propertiesFormat.BorderLeft = new ODFBorder();
          if ((double) paraformat.Borders.Left.Space > 0.0)
            propertiesFormat.PaddingLeft = paraformat.Borders.Left.Space / 72f;
          float num = paraformat.Borders.Left.LineWidth / 72f;
          propertiesFormat.BorderLeft.LineWidth = num.ToString();
          propertiesFormat.BorderLeft.LineStyle = this.GetUnderlineStyle(paraformat.Borders.Left.BorderType);
          propertiesFormat.BorderLeft.LineColor = paraformat.Borders.Left.Color;
        }
        if (paraformat.Borders.Right.IsBorderDefined && !paraformat.Borders.Right.IsDefault)
        {
          propertiesFormat.BorderRight = new ODFBorder();
          if ((double) paraformat.Borders.Right.Space > 0.0)
            propertiesFormat.PaddingRight = paraformat.Borders.Right.Space / 72f;
          float num = paraformat.Borders.Right.LineWidth / 72f;
          propertiesFormat.BorderRight.LineWidth = num.ToString();
          propertiesFormat.BorderRight.LineStyle = this.GetUnderlineStyle(paraformat.Borders.Right.BorderType);
          propertiesFormat.BorderRight.LineColor = paraformat.Borders.Right.Color;
        }
        if (paraformat.Borders.Top.IsBorderDefined && !paraformat.Borders.Top.IsDefault)
        {
          propertiesFormat.BorderTop = new ODFBorder();
          if ((double) paraformat.Borders.Top.Space > 0.0)
            propertiesFormat.PaddingTop = paraformat.Borders.Top.Space / 72f;
          float num = paraformat.Borders.Top.LineWidth / 72f;
          propertiesFormat.BorderTop.LineWidth = num.ToString();
          propertiesFormat.BorderTop.LineStyle = this.GetUnderlineStyle(paraformat.Borders.Top.BorderType);
          propertiesFormat.BorderTop.LineColor = paraformat.Borders.Top.Color;
        }
        if (paraformat.Borders.Bottom.IsBorderDefined && !paraformat.Borders.Bottom.IsDefault)
        {
          propertiesFormat.BorderBottom = new ODFBorder();
          if ((double) paraformat.Borders.Top.Space > 0.0)
            propertiesFormat.PaddingBottom = paraformat.Borders.Bottom.Space / 72f;
          float num = paraformat.Borders.Bottom.LineWidth / 72f;
          propertiesFormat.BorderBottom.LineWidth = num.ToString();
          propertiesFormat.BorderBottom.LineStyle = this.GetUnderlineStyle(paraformat.Borders.Bottom.BorderType);
          propertiesFormat.BorderBottom.LineColor = paraformat.Borders.Bottom.Color;
        }
      }
      if (paraformat.HasValue(0))
        propertiesFormat.TextAlign = this.GetAlignment(paraformat);
      if (paraformat.HasValue(5))
        propertiesFormat.TextIndent = paraformat.FirstLineIndent / 72f;
      if (paraformat.HasValue(21))
        propertiesFormat.BackgroundColor = this.GetColor(paraformat.BackColor);
      if (paraformat.HasValue(8))
        propertiesFormat.BeforeSpacing = (double) paraformat.BeforeSpacing / 72.0;
      if (paraformat.HasValue(9))
        propertiesFormat.AfterSpacing = (double) paraformat.AfterSpacing / 72.0;
      if (paraformat.HasValue(2))
        propertiesFormat.LeftIndent = (double) paraformat.LeftIndent / 72.0;
      if (paraformat.HasValue(3))
        propertiesFormat.RightIndent = (double) paraformat.RightIndent / 72.0;
      if (paraformat.HasValue(52))
      {
        if (paraformat.LineSpacingRule == LineSpacingRule.Multiple && (double) paraformat.LineSpacing != 12.8)
          propertiesFormat.LineSpacing = Math.Floor((double) paraformat.LineSpacing / 12.0 * 100.0);
        if (paraformat.LineSpacingRule == LineSpacingRule.AtLeast)
          propertiesFormat.LineHeightAtLeast = Math.Round((double) paraformat.LineSpacing / 72.0, 4);
        if (paraformat.LineSpacingRule == LineSpacingRule.Exactly)
          propertiesFormat.LineHeight = paraformat.LineSpacing / 72f;
      }
      if (paraformat.HasValue(6))
        propertiesFormat.KeepTogether = KeepTogether.always;
      if (paraformat.HasValue(10))
        propertiesFormat.KeepWithNext = KeepTogether.always;
      if (paraformat.HasValue(31 /*0x1F*/) && paraformat.Bidi)
        propertiesFormat.WritingMode = WritingMode.RLTB;
      if (paraformat.HasValue(31 /*0x1F*/) && paraformat.Bidi)
        propertiesFormat.WritingMode = WritingMode.RLTB;
      if (paraformat.Tabs != null)
      {
        TabCollection tabs = paraformat.Tabs;
        if (tabs.Count > 0)
        {
          for (int index = 0; index < tabs.Count; ++index)
          {
            TabStops tabStops = new TabStops();
            Tab tab = tabs[index];
            tabStops.TextPosition = (double) tab.Position / 72.0;
            tabStops.TextAlignType = this.GetTabAlignment(tab.Justification);
            tabStops.TabStopLeader = this.GetTabStop(tab.TabLeader);
            propertiesFormat.TabStops.Add(tabStops);
          }
        }
      }
    }
    return propertiesFormat;
  }

  private ODFParagraphProperties ResetInlineParagraphFormat(
    WParagraphFormat paraformat,
    ODFParagraphProperties paraProp)
  {
    if (paraformat != null)
    {
      if (paraProp == null)
        paraProp = new ODFParagraphProperties();
      if (paraformat.Borders.Top.BorderType != BorderStyle.None && paraformat.Borders.Top.BorderType == paraformat.Borders.Left.BorderType && paraformat.Borders.Top.BorderType == paraformat.Borders.Bottom.BorderType && paraformat.Borders.Top.BorderType == paraformat.Borders.Right.BorderType)
      {
        paraProp.Border = new ODFBorder();
        if ((double) paraformat.Borders.Top.Space > 0.0)
        {
          paraProp.PaddingTop = paraformat.Borders.Top.Space / 72f;
          paraProp.PaddingBottom = paraformat.Borders.Bottom.Space / 72f;
          paraProp.PaddingLeft = paraformat.Borders.Left.Space / 72f;
          paraProp.PaddingRight = paraformat.Borders.Right.Space / 72f;
        }
        float num = paraformat.Borders.Top.LineWidth / 72f;
        paraProp.Border.LineWidth = num.ToString();
        paraProp.Border.LineStyle = this.GetUnderlineStyle(paraformat.Borders.Top.BorderType);
        paraProp.Border.LineColor = paraformat.Borders.Top.Color;
      }
      else
      {
        if (paraformat.Borders.Left.IsBorderDefined && !paraformat.Borders.Left.IsDefault)
        {
          paraProp.BorderLeft = new ODFBorder();
          if ((double) paraformat.Borders.Left.Space > 0.0)
            paraProp.PaddingLeft = paraformat.Borders.Left.Space / 72f;
          float num = paraformat.Borders.Left.LineWidth / 72f;
          paraProp.BorderLeft.LineWidth = num.ToString();
          paraProp.BorderLeft.LineStyle = this.GetUnderlineStyle(paraformat.Borders.Left.BorderType);
          paraProp.BorderLeft.LineColor = paraformat.Borders.Left.Color;
        }
        if (paraformat.Borders.Right.IsBorderDefined && !paraformat.Borders.Right.IsDefault)
        {
          paraProp.BorderRight = new ODFBorder();
          if ((double) paraformat.Borders.Right.Space > 0.0)
            paraProp.PaddingRight = paraformat.Borders.Right.Space / 72f;
          float num = paraformat.Borders.Right.LineWidth / 72f;
          paraProp.BorderRight.LineWidth = num.ToString();
          paraProp.BorderRight.LineStyle = this.GetUnderlineStyle(paraformat.Borders.Right.BorderType);
          paraProp.BorderRight.LineColor = paraformat.Borders.Right.Color;
        }
        if (paraformat.Borders.Top.IsBorderDefined && !paraformat.Borders.Top.IsDefault)
        {
          paraProp.BorderTop = new ODFBorder();
          if ((double) paraformat.Borders.Top.Space > 0.0)
            paraProp.PaddingTop = paraformat.Borders.Top.Space / 72f;
          float num = paraformat.Borders.Top.LineWidth / 72f;
          paraProp.BorderTop.LineWidth = num.ToString();
          paraProp.BorderTop.LineStyle = this.GetUnderlineStyle(paraformat.Borders.Top.BorderType);
          paraProp.BorderTop.LineColor = paraformat.Borders.Top.Color;
        }
        if (paraformat.Borders.Bottom.IsBorderDefined && !paraformat.Borders.Bottom.IsDefault)
        {
          paraProp.BorderBottom = new ODFBorder();
          if ((double) paraformat.Borders.Top.Space > 0.0)
            paraProp.PaddingBottom = paraformat.Borders.Bottom.Space / 72f;
          float num = paraformat.Borders.Bottom.LineWidth / 72f;
          paraProp.BorderBottom.LineWidth = num.ToString();
          paraProp.BorderBottom.LineStyle = this.GetUnderlineStyle(paraformat.Borders.Bottom.BorderType);
          paraProp.BorderBottom.LineColor = paraformat.Borders.Bottom.Color;
        }
      }
      if (paraformat.HasValue(0))
        paraProp.TextAlign = this.GetAlignment(paraformat);
      if (paraformat.HasValue(5))
        paraProp.TextIndent = paraformat.FirstLineIndent / 72f;
      if (paraformat.HasValue(21))
        paraProp.BackgroundColor = this.GetColor(paraformat.BackColor);
      if (paraformat.HasValue(8))
        paraProp.BeforeSpacing = (double) paraformat.BeforeSpacing / 72.0;
      if (paraformat.HasValue(9))
        paraProp.AfterSpacing = (double) paraformat.AfterSpacing / 72.0;
      if (paraformat.HasValue(2))
        paraProp.LeftIndent = (double) paraformat.LeftIndent / 72.0;
      if (paraformat.HasValue(3))
        paraProp.RightIndent = (double) paraformat.RightIndent / 72.0;
      if (paraformat.HasValue(52))
      {
        if (paraformat.LineSpacingRule == LineSpacingRule.Multiple && (double) paraformat.LineSpacing != 12.8)
          paraProp.LineSpacing = Math.Floor((double) paraformat.LineSpacing / 12.0 * 100.0);
        if (paraformat.LineSpacingRule == LineSpacingRule.AtLeast)
          paraProp.LineHeightAtLeast = Math.Round((double) paraformat.LineSpacing / 72.0, 4);
        if (paraformat.LineSpacingRule == LineSpacingRule.Exactly)
          paraProp.LineHeight = paraformat.LineSpacing / 72f;
      }
      if (paraformat.HasValue(6))
        paraProp.KeepTogether = KeepTogether.always;
      if (paraformat.HasValue(10))
        paraProp.KeepWithNext = KeepTogether.always;
      if (paraformat.HasValue(31 /*0x1F*/) && paraformat.Bidi)
        paraProp.WritingMode = WritingMode.RLTB;
      if (paraformat.HasValue(31 /*0x1F*/) && paraformat.Bidi)
        paraProp.WritingMode = WritingMode.RLTB;
      if (paraformat.Tabs != null)
      {
        TabCollection tabs = paraformat.Tabs;
        if (tabs.Count > 0)
        {
          for (int index = 0; index < tabs.Count; ++index)
          {
            TabStops tabStops = new TabStops();
            Tab tab = tabs[index];
            tabStops.TextPosition = (double) tab.Position / 72.0;
            tabStops.TextAlignType = this.GetTabAlignment(tab.Justification);
            tabStops.TabStopLeader = this.GetTabStop(tab.TabLeader);
            paraProp.TabStops.Add(tabStops);
          }
        }
      }
    }
    return paraProp;
  }

  private TextAlign GetAlignment(WParagraphFormat paragraphFormat)
  {
    TextAlign alignment = TextAlign.left;
    switch (paragraphFormat.LogicalJustification)
    {
      case HorizontalAlignment.Center:
        alignment = TextAlign.center;
        break;
      case HorizontalAlignment.Right:
        alignment = TextAlign.right;
        break;
    }
    return alignment;
  }

  private BorderLineStyle GetUnderlineStyle(BorderStyle style)
  {
    BorderLineStyle underlineStyle = BorderLineStyle.none;
    switch (style)
    {
      case BorderStyle.Single:
        underlineStyle = BorderLineStyle.solid;
        break;
      case BorderStyle.Dot:
        underlineStyle = BorderLineStyle.dotted;
        break;
      case BorderStyle.DotDash:
        underlineStyle = BorderLineStyle.dotdash;
        break;
      case BorderStyle.DotDotDash:
        underlineStyle = BorderLineStyle.dotdotdash;
        break;
      case BorderStyle.Wave:
        underlineStyle = BorderLineStyle.wave;
        break;
      case BorderStyle.DashSmallGap:
        underlineStyle = BorderLineStyle.dashed;
        break;
    }
    return underlineStyle;
  }

  private string GetColor(Color color)
  {
    return "#" + (color.ToArgb() & 16777215 /*0xFFFFFF*/).ToString("X6");
  }

  internal void ConvertDefaultStyles()
  {
    DefaultStyleCollection defaultStyle = new DefaultStyleCollection();
    IStyleCollection styles = this.m_document.Styles;
    DefaultStyle style1 = new DefaultStyle()
    {
      ParagraphProperties = this.CopyParaFormatToParagraphPropertiesFormat(this.m_document.DefParaFormat)
    };
    style1.Textproperties = this.CopyCharFormatToTextFormat(this.m_document.DefCharFormat, style1.Textproperties);
    defaultStyle.Add(style1);
    this.m_writer.SerializeDefaultStyles(defaultStyle);
    for (int index1 = 0; index1 < styles.Count; ++index1)
    {
      ODFStyleCollection ODFStyles = new ODFStyleCollection();
      ODFStyle style2 = new ODFStyle();
      IStyle style3 = styles[index1];
      if (style3.StyleType == StyleType.ParagraphStyle)
      {
        WParagraphStyle wparagraphStyle = styles[index1] as WParagraphStyle;
        WParagraphFormat paragraphFormat = wparagraphStyle.ParagraphFormat;
        WCharacterFormat characterFormat = wparagraphStyle.CharacterFormat;
        style2.ParagraphProperties = this.CopyParaFormatToParagraphPropertiesFormat(paragraphFormat);
        style2.Textproperties = this.CopyCharFormatToTextFormat(characterFormat, style2.Textproperties);
        style2.Name = wparagraphStyle.Name;
        if (paragraphFormat.Tabs != null)
        {
          TabCollection tabs = paragraphFormat.Tabs;
          if (tabs.Count > 0)
          {
            for (int index2 = 0; index2 < tabs.Count; ++index2)
            {
              TabStops tabStops = new TabStops();
              Tab tab = tabs[index2];
              tabStops.TextPosition = (double) tab.Position / 72.0;
              tabStops.TextAlignType = this.GetTabAlignment(tab.Justification);
              tabStops.TabStopLeader = this.GetTabStop(tab.TabLeader);
              style2.ParagraphProperties = new ODFParagraphProperties();
              style2.ParagraphProperties.TabStops.Add(tabStops);
            }
          }
        }
        style2.Family = this.GetStyleType(wparagraphStyle.StyleType);
        if (this.StartsWithExt(style3.Name, "TOC"))
          this.m_oDocument.TOCStyles.Add(style2);
        ODFStyles.Add(style2);
        this.m_writer.SerializeODFStyles(ODFStyles);
      }
      if (style3.StyleType == StyleType.CharacterStyle)
      {
        WCharacterStyle wcharacterStyle = styles[index1] as WCharacterStyle;
        WCharacterFormat characterFormat = wcharacterStyle.CharacterFormat;
        style2.Textproperties = this.CopyCharFormatToTextFormat(characterFormat, style2.Textproperties);
        style2.Name = wcharacterStyle.Name;
        style2.Family = this.GetStyleType(wcharacterStyle.StyleType);
        ODFStyles.Add(style2);
        this.m_writer.SerializeODFStyles(ODFStyles);
      }
    }
  }

  private TextAlign GetTabAlignment(TabJustification tabAlignment)
  {
    TextAlign tabAlignment1 = TextAlign.left;
    switch (tabAlignment)
    {
      case TabJustification.Left:
        tabAlignment1 = TextAlign.left;
        break;
      case TabJustification.Centered:
        tabAlignment1 = TextAlign.center;
        break;
      case TabJustification.Right:
        tabAlignment1 = TextAlign.right;
        break;
    }
    return tabAlignment1;
  }

  private TabStopLeader GetTabStop(TabLeader tabLeader)
  {
    switch (tabLeader)
    {
      case TabLeader.Dotted:
        return TabStopLeader.Dotted;
      case TabLeader.Hyphenated:
        return TabStopLeader.Hyphenated;
      case TabLeader.Single:
        return TabStopLeader.Single;
      case TabLeader.Heavy:
        return TabStopLeader.Heavy;
      default:
        return TabStopLeader.NoLeader;
    }
  }

  private ODFFontFamily GetStyleType(StyleType styleType)
  {
    ODFFontFamily styleType1 = ODFFontFamily.Paragraph;
    switch (styleType)
    {
      case StyleType.ParagraphStyle:
        styleType1 = ODFFontFamily.Paragraph;
        break;
      case StyleType.CharacterStyle:
        styleType1 = ODFFontFamily.Text;
        break;
      case StyleType.TableStyle:
        styleType1 = ODFFontFamily.Table;
        break;
      case StyleType.OtherStyle:
        styleType1 = ODFFontFamily.Section;
        break;
    }
    return styleType1;
  }

  internal void GetBody()
  {
    OTextBodyItem otextBodyItem = (OTextBodyItem) null;
    this.odfstyleCollection1.Dispose();
    int fieldEndMarkIndex = 0;
    int breakIndex = -1;
    foreach (WSection section in (Syncfusion.DocIO.DLS.CollectionImpl) this.m_document.Sections)
    {
      bool isContinuousSection = false;
      string str = string.Empty;
      if (section.BreakCode == SectionBreakCode.NoBreak)
      {
        isContinuousSection = true;
        this.odfStyle = new ODFStyle();
        this.odfStyle.Family = ODFFontFamily.Section;
        this.odfStyle.ODFSectionProperties = this.ConvertDocToODFSectionProperties(section);
        str = this.odfstyleCollection1.Add(this.odfStyle);
      }
      for (int index = 0; index < section.Body.Items.Count; ++index)
      {
        TextBodyItem TextbodyItem = section.Body.Items[index];
        switch (TextbodyItem.EntityType)
        {
          case Syncfusion.DocIO.DLS.EntityType.Paragraph:
            bool isInSameTextBody = false;
            bool isInSameParagraph = false;
            int fieldEndOwnerParagraphIndex = 0;
            otextBodyItem = this.GetOParagraph(TextbodyItem, ref fieldEndMarkIndex, ref isInSameParagraph, ref isInSameTextBody, ref fieldEndOwnerParagraphIndex, ref breakIndex, isContinuousSection);
            if (index == 0 && isContinuousSection)
            {
              otextBodyItem.IsFirstItemOfSection = true;
              otextBodyItem.SectionStyleName = str;
            }
            if (index == section.Body.Items.Count - 1 && isContinuousSection)
              otextBodyItem.IsLastItemOfSection = true;
            if ((otextBodyItem as OParagraph).OParagraphItemCollection.Count > 0 && !isInSameParagraph)
            {
              switch ((otextBodyItem as OParagraph).OParagraphItemCollection[(otextBodyItem as OParagraph).OParagraphItemCollection.Count - 1])
              {
                case OMergeField _:
                case OField _:
                  index = fieldEndOwnerParagraphIndex - 1;
                  break;
              }
            }
            else
            {
              fieldEndMarkIndex = 0;
              break;
            }
            break;
          case Syncfusion.DocIO.DLS.EntityType.Table:
            otextBodyItem = this.GetTableContent(TextbodyItem);
            break;
        }
        this.m_oDocument.Body.TextBodyItems.Add(otextBodyItem);
        if (breakIndex != -1)
        {
          --index;
          fieldEndMarkIndex = breakIndex + 1;
          breakIndex = -1;
        }
      }
    }
    this.m_writer.SerializeAutoStyleStart();
    this.m_writer.SerializeContentAutoStyles(this.odfstyleCollection1);
    this.m_writer.SerializeContentListStyles(this.m_oDocument.ListStyles);
    this.m_writer.SerializeEnd();
    this.m_writer.SerializeDocIOContent(this.m_oDocument);
  }

  private Syncfusion.DocIO.ODF.Base.ODFImplementation.SectionProperties ConvertDocToODFSectionProperties(
    WSection section)
  {
    return new Syncfusion.DocIO.ODF.Base.ODFImplementation.SectionProperties()
    {
      MarginRight = 0,
      MarginLeft = 0
    };
  }

  private OTextBodyItem GetOTextBodyItem(TextBodyItem TextbodyItem)
  {
    OTextBodyItem otextBodyItem = (OTextBodyItem) null;
    switch (TextbodyItem.EntityType)
    {
      case Syncfusion.DocIO.DLS.EntityType.Paragraph:
        int fieldEndMarkIndex = 0;
        bool isInSameTextBody = false;
        bool isInSameParagraph = false;
        int fieldEndOwnerParagraphIndex = 0;
        int breakIndex = -1;
        otextBodyItem = this.GetOParagraph(TextbodyItem, ref fieldEndMarkIndex, ref isInSameParagraph, ref isInSameTextBody, ref fieldEndOwnerParagraphIndex, ref breakIndex, false);
        break;
      case Syncfusion.DocIO.DLS.EntityType.Table:
        otextBodyItem = this.GetTableContent(TextbodyItem);
        break;
    }
    return otextBodyItem;
  }

  private OTextBodyItem GetOParagraph(
    TextBodyItem TextbodyItem,
    ref int fieldEndMarkIndex,
    ref bool isInSameParagraph,
    ref bool isInSameTextBody,
    ref int fieldEndOwnerParagraphIndex,
    ref int breakIndex,
    bool isContinuousSection)
  {
    OParagraph oparagraph = new OParagraph();
    WParagraph wparagraph = TextbodyItem as WParagraph;
    WParagraphFormat paragraphFormat = wparagraph.ParagraphFormat;
    ParagraphItemCollection items = wparagraph.Items;
    bool flag = false;
    this.odfStyle = new ODFStyle();
    IWParagraphStyle style = wparagraph.GetStyle();
    if (!this.IsWritingHeaderFooter && !isContinuousSection && !wparagraph.IsInCell && wparagraph.Index == 0 && this.pageNames.Count > 0 && this.pageNames.Count > 0)
    {
      this.odfStyle.MasterPageName = this.pageNames[0];
      flag = true;
      this.pageNames.RemoveAt(0);
    }
    this.odfStyle.ParentStyleName = wparagraph.StyleName;
    if (style != null && !style.Name.Equals("Normal") && (this.StartsWithExt(style.Name, "Heading") || this.StartsWithExt(style.Name, "Title")))
    {
      Heading heading = new Heading();
      heading.StyleName = style.Name;
      oparagraph.Header = new Heading();
      if (style.CharacterFormat != null)
      {
        this.odfStyle.ParagraphProperties = this.CopyParaFormatToParagraphPropertiesFormat(style.ParagraphFormat);
        this.odfStyle.Textproperties = this.CopyCharFormatToTextFormat(style.CharacterFormat, this.odfStyle.Textproperties);
        this.odfStyle.Family = ODFFontFamily.Paragraph;
        oparagraph.Header.StyleName = heading.StyleName;
      }
    }
    else if (wparagraph.ListFormat.CurrentListStyle != null)
    {
      string name = wparagraph.ListFormat.CurrentListStyle.Name;
      for (int index = 0; index < this.m_oDocument.ListStyles.Count; ++index)
      {
        if (this.m_oDocument.ListStyles[index].Name == name)
        {
          oparagraph.ListStyleName = this.m_oDocument.ListStyles[index].CurrentStyleName;
          oparagraph.ListLevelNumber = wparagraph.ListFormat.ListLevelNumber;
          break;
        }
      }
    }
    else if (paragraphFormat != null)
    {
      this.odfStyle.ParagraphProperties = this.CopyParaFormatToParagraphPropertiesFormat(paragraphFormat);
      if (this.odfStyle.ParagraphProperties != null)
        this.odfStyle.Family = ODFFontFamily.Paragraph;
    }
    if (wparagraph.ParagraphFormat.PropertiesHash.Count > 0)
      this.odfStyle.ParagraphProperties = this.ResetInlineParagraphFormat(wparagraph.ParagraphFormat, this.odfStyle.ParagraphProperties);
    if (flag)
      this.odfStyle.ParagraphProperties.BeforeBreak = BeforeBreak.page;
    else if (this.m_lastBreak != BeforeBreak.auto)
    {
      this.odfStyle.ParagraphProperties.BeforeBreak = this.m_lastBreak;
      this.m_lastBreak = BeforeBreak.auto;
    }
    this.odfstyleCollection1.Add(this.odfStyle);
    oparagraph.StyleName = this.odfStyle.Name;
    if (this.StartsWithExt(this.odfStyle.ParentStyleName, "TOC"))
    {
      oparagraph.TocMark = this.odfStyle.ParentStyleName;
      foreach (ODFStyle tocStyle in this.m_oDocument.TOCStyles)
      {
        if (this.odfStyle.ParentStyleName == tocStyle.Name && tocStyle.ParagraphProperties.TabStops.Count == 0)
          tocStyle.ParagraphProperties = this.odfStyle.ParagraphProperties;
      }
    }
    for (int index = fieldEndMarkIndex; index < items.Count; ++index)
    {
      ParagraphItem paragraphItem = items[index];
      switch (paragraphItem.EntityType)
      {
        case Syncfusion.DocIO.DLS.EntityType.TextRange:
          this.opara = (OParagraphItem) new OTextRange();
          this.odfStyle = new ODFStyle();
          this.opara.TextProperties = this.CopyCharFormatToTextFormat((paragraphItem as WTextRange).CharacterFormat, this.odfStyle.Textproperties);
          if (this.opara.TextProperties != null)
          {
            if (this.opara.TextProperties != null)
            {
              this.odfStyle = new ODFStyle();
              this.opara.Span = true;
              this.odfStyle.Family = ODFFontFamily.Text;
              this.odfStyle.Textproperties = this.opara.TextProperties;
            }
            this.odfstyleCollection1.Add(this.odfStyle);
            this.opara.StyleName = this.odfStyle.Name;
          }
          this.opara.Text = this.CombineTextInSubsequentTextRanges(items, ref index);
          oparagraph.OParagraphItemCollection.Add(this.opara);
          break;
        case Syncfusion.DocIO.DLS.EntityType.Picture:
          OPicture oPicture = new OPicture();
          WPicture picture = paragraphItem as WPicture;
          this.odfStyle = new ODFStyle();
          this.GetOPicture(oPicture, picture, this.odfStyle);
          string str = this.UpdateShapeId(picture, false, false, (WOleObject) null);
          if (string.IsNullOrEmpty(oPicture.OPictureHRef))
            oPicture.OPictureHRef = "media/image" + str;
          oparagraph.OParagraphItemCollection.Add((OParagraphItem) oPicture);
          break;
        case Syncfusion.DocIO.DLS.EntityType.Field:
          OField ofield = new OField();
          WField wfield = paragraphItem as WField;
          ofield.TextProperties = this.CopyCharFormatToTextFormat(wfield.CharacterFormat, this.odfStyle.Textproperties);
          if (ofield.TextProperties != null)
          {
            if (ofield.TextProperties != null)
            {
              this.odfStyle = new ODFStyle();
              ofield.Span = true;
              this.odfStyle.Family = ODFFontFamily.Text;
              this.odfStyle.Textproperties = ofield.TextProperties;
            }
            this.odfstyleCollection1.Add(this.odfStyle);
            ofield.StyleName = this.odfStyle.Name;
          }
          ofield.FormattingString = wfield.FormattingString;
          ofield.Text = wfield.FieldResult;
          ofield.FieldValue = wfield.FieldValue;
          ofield.Text = wfield.Text;
          ofield.FieldCulture = !wfield.CharacterFormat.HasValue(73) ? CultureInfo.CurrentCulture : wfield.GetCulture((LocaleIDs) wfield.CharacterFormat.LocaleIdASCII);
          if (wfield.FieldType == FieldType.FieldDate)
            ofield.OFieldType = OFieldType.FieldDate;
          else if (wfield.FieldType == FieldType.FieldHyperlink)
            ofield.OFieldType = OFieldType.FieldHyperlink;
          else if (wfield.FieldType == FieldType.FieldNumPages)
          {
            ofield.OFieldType = OFieldType.FieldNumPages;
            string empty = string.Empty;
            string pageNumberFormat = wfield.RemoveMergeFormat(wfield.FormattingString, ref empty);
            if (!string.IsNullOrEmpty(pageNumberFormat))
              ofield.PageNumberFormat = this.GetNumberFormat(pageNumberFormat);
          }
          else if (wfield.FieldType == FieldType.FieldPage)
          {
            ofield.OFieldType = OFieldType.FieldPage;
            string empty = string.Empty;
            string pageNumberFormat = wfield.RemoveMergeFormat(wfield.FormattingString, ref empty);
            if (!string.IsNullOrEmpty(pageNumberFormat))
              ofield.PageNumberFormat = this.GetNumberFormat(pageNumberFormat);
          }
          else if (wfield.FieldType == FieldType.FieldAuthor)
            ofield.OFieldType = OFieldType.FieldAuthor;
          else if (wfield.FieldType == FieldType.FieldIf)
          {
            ofield.OFieldType = OFieldType.FieldIf;
            this.opara = (OParagraphItem) new OTextRange();
            this.opara.Text = wfield.Text;
            oparagraph.OParagraphItemCollection.Add(this.opara);
          }
          if (ofield.OFieldType != OFieldType.FieldIf)
            oparagraph.OParagraphItemCollection.Add((OParagraphItem) ofield);
          WFieldMark fieldEnd1 = wfield.FieldEnd;
          int inOwnerCollection1 = fieldEnd1.GetIndexInOwnerCollection();
          if (wfield.OwnerParagraph == fieldEnd1.OwnerParagraph)
          {
            int num1 = inOwnerCollection1;
            int num2 = num1 + 1;
            index = num1;
            isInSameTextBody = true;
            isInSameParagraph = true;
            break;
          }
          if (wfield.OwnerParagraph.OwnerTextBody == fieldEnd1.OwnerParagraph.OwnerTextBody)
          {
            WParagraph ownerParagraph = fieldEnd1.OwnerParagraph;
            if (ownerParagraph != null)
              fieldEndOwnerParagraphIndex = ownerParagraph.GetIndexInOwnerCollection();
            isInSameTextBody = true;
            fieldEndMarkIndex = inOwnerCollection1;
            return (OTextBodyItem) oparagraph;
          }
          break;
        case Syncfusion.DocIO.DLS.EntityType.MergeField:
          OMergeField omergeField = new OMergeField();
          WMergeField wmergeField = paragraphItem as WMergeField;
          List<WCharacterFormat> characterFormatting = wmergeField.GetResultCharacterFormatting();
          omergeField.TextProperties = this.CopyCharFormatToTextFormat(characterFormatting.Count == 0 ? (WCharacterFormat) null : characterFormatting[0], this.odfStyle.Textproperties);
          if (omergeField.TextProperties != null)
          {
            if (omergeField.TextProperties != null)
            {
              this.odfStyle = new ODFStyle();
              omergeField.Span = true;
              this.odfStyle.Family = ODFFontFamily.Text;
              this.odfStyle.Textproperties = omergeField.TextProperties;
            }
            this.odfstyleCollection1.Add(this.odfStyle);
            omergeField.StyleName = this.odfStyle.Name;
          }
          if (!string.IsNullOrEmpty(wmergeField.Text))
            omergeField.Text = wmergeField.Text;
          if (!string.IsNullOrEmpty(wmergeField.FieldName))
            omergeField.FieldName = wmergeField.FieldName;
          if (!string.IsNullOrEmpty(wmergeField.TextBefore))
            omergeField.TextBefore = wmergeField.TextBefore;
          if (!string.IsNullOrEmpty(wmergeField.TextAfter))
            omergeField.TextAfter = wmergeField.TextAfter;
          oparagraph.OParagraphItemCollection.Add((OParagraphItem) omergeField);
          WFieldMark fieldEnd2 = wmergeField.FieldEnd;
          int inOwnerCollection2 = fieldEnd2.GetIndexInOwnerCollection();
          if (wmergeField.OwnerParagraph == fieldEnd2.OwnerParagraph)
          {
            int num3 = inOwnerCollection2;
            int num4 = num3 + 1;
            index = num3;
            isInSameTextBody = true;
            isInSameParagraph = true;
            break;
          }
          if (wmergeField.OwnerParagraph.OwnerTextBody == fieldEnd2.OwnerParagraph.OwnerTextBody)
          {
            WParagraph ownerParagraph = fieldEnd2.OwnerParagraph;
            if (ownerParagraph != null)
              fieldEndOwnerParagraphIndex = ownerParagraph.GetIndexInOwnerCollection();
            isInSameTextBody = true;
            fieldEndMarkIndex = inOwnerCollection2;
            return (OTextBodyItem) oparagraph;
          }
          break;
        case Syncfusion.DocIO.DLS.EntityType.BookmarkStart:
          oparagraph.OParagraphItemCollection.Add((OParagraphItem) new OBookmarkStart()
          {
            Name = (paragraphItem as BookmarkStart).Name
          });
          break;
        case Syncfusion.DocIO.DLS.EntityType.BookmarkEnd:
          oparagraph.OParagraphItemCollection.Add((OParagraphItem) new OBookmarkEnd()
          {
            Name = (paragraphItem as BookmarkEnd).Name
          });
          break;
        case Syncfusion.DocIO.DLS.EntityType.Break:
          this.opara = (OParagraphItem) new OBreak();
          OBreakType breakType = (OBreakType) (paragraphItem as Break).BreakType;
          if (breakType == OBreakType.PageBreak)
          {
            (this.opara as OBreak).BreakType = OBreakType.PageBreak;
            this.m_lastBreak = BeforeBreak.page;
            if (index != items.Count - 1)
            {
              breakIndex = index;
              return (OTextBodyItem) oparagraph;
            }
          }
          else if (breakType == OBreakType.ColumnBreak)
          {
            (this.opara as OBreak).BreakType = OBreakType.ColumnBreak;
            this.m_lastBreak = BeforeBreak.column;
            if (index != items.Count - 1)
            {
              breakIndex = index;
              return (OTextBodyItem) oparagraph;
            }
          }
          if (breakType == OBreakType.LineBreak)
          {
            (this.opara as OBreak).BreakType = OBreakType.LineBreak;
            this.opara.ParagraphProperties = new ODFParagraphProperties();
            this.opara.ParagraphProperties.LineBreak = true;
          }
          oparagraph.OParagraphItemCollection.Add(this.opara);
          break;
        case Syncfusion.DocIO.DLS.EntityType.Symbol:
          this.opara = (OParagraphItem) new OTextRange();
          this.odfStyle = new ODFStyle();
          WSymbol wsymbol = paragraphItem as WSymbol;
          this.opara.TextProperties = this.CopyCharFormatToTextFormat(wsymbol.CharacterFormat, this.odfStyle.Textproperties);
          if (this.opara.TextProperties == null && !string.IsNullOrEmpty(wsymbol.FontName))
          {
            this.opara.TextProperties = new TextProperties();
            this.opara.TextProperties.FontName = wsymbol.FontName;
          }
          if (this.opara.TextProperties != null)
          {
            if (this.opara.TextProperties != null)
            {
              this.odfStyle = new ODFStyle();
              this.opara.Span = true;
              this.odfStyle.Family = ODFFontFamily.Text;
              this.odfStyle.Textproperties = this.opara.TextProperties;
            }
            this.odfstyleCollection1.Add(this.odfStyle);
            this.opara.StyleName = this.odfStyle.Name;
          }
          this.opara.Text = Convert.ToChar(wsymbol.CharacterCode).ToString();
          oparagraph.OParagraphItemCollection.Add(this.opara);
          break;
      }
    }
    return (OTextBodyItem) oparagraph;
  }

  private PageNumberFormat GetNumberFormat(string pageNumberFormat)
  {
    if (pageNumberFormat.EndsWith("ROMAN"))
      return PageNumberFormat.UpperRoman;
    if (pageNumberFormat.EndsWith("roman"))
      return PageNumberFormat.LowerRoman;
    if (pageNumberFormat.EndsWith("ALPHABET"))
      return PageNumberFormat.UpperAlphabet;
    if (pageNumberFormat.EndsWith("alphabetic"))
      return PageNumberFormat.LowerAlphabet;
    if (pageNumberFormat.EndsWith("Ordinal"))
      return PageNumberFormat.Ordinal;
    if (pageNumberFormat.EndsWith("OrdText"))
      return PageNumberFormat.OrdinalText;
    if (pageNumberFormat.EndsWith("Arabic"))
      return PageNumberFormat.Arabic;
    if (pageNumberFormat.EndsWith("CardText"))
      return PageNumberFormat.CardinalText;
    if (pageNumberFormat.EndsWith("Hex"))
      return PageNumberFormat.Hexa;
    if (pageNumberFormat.EndsWith("DollarText"))
      return PageNumberFormat.DollorText;
    return pageNumberFormat.EndsWith("ARABICDASH") || pageNumberFormat.EndsWith("ArabicDash") ? PageNumberFormat.ArabicDash : PageNumberFormat.Arabic;
  }

  private void GetOPicture(OPicture oPicture, WPicture picture, ODFStyle odfStyle)
  {
    oPicture.TextProperties = this.CopyCharFormatToTextFormat(picture.CharacterFormat, odfStyle.Textproperties);
    if (oPicture.TextProperties != null)
    {
      if (oPicture.TextProperties != null)
      {
        odfStyle = new ODFStyle();
        oPicture.Span = true;
        odfStyle.Family = ODFFontFamily.Text;
        odfStyle.Textproperties = oPicture.TextProperties;
      }
      this.odfstyleCollection1.Add(odfStyle);
      oPicture.StyleName = odfStyle.Name;
    }
    oPicture.Height = picture.Height;
    oPicture.Width = picture.Width;
    oPicture.HeightScale = picture.HeightScale;
    oPicture.WidthScale = picture.WidthScale;
    oPicture.HorizontalPosition = picture.HorizontalPosition;
    oPicture.VerticalPosition = picture.VerticalPosition;
    oPicture.OrderIndex = picture.OrderIndex;
    if (picture.TextWrappingStyle == Syncfusion.DocIO.DLS.TextWrappingStyle.Inline)
      oPicture.TextWrappingStyle = Syncfusion.DocIO.ODF.Base.TextWrappingStyle.Inline;
    else if (picture.TextWrappingStyle == Syncfusion.DocIO.DLS.TextWrappingStyle.Behind)
      oPicture.TextWrappingStyle = Syncfusion.DocIO.ODF.Base.TextWrappingStyle.Behind;
    else if (picture.TextWrappingStyle == Syncfusion.DocIO.DLS.TextWrappingStyle.InFrontOfText)
      oPicture.TextWrappingStyle = Syncfusion.DocIO.ODF.Base.TextWrappingStyle.InFrontOfText;
    else if (picture.TextWrappingStyle == Syncfusion.DocIO.DLS.TextWrappingStyle.Square)
      oPicture.TextWrappingStyle = Syncfusion.DocIO.ODF.Base.TextWrappingStyle.Square;
    else if (picture.TextWrappingStyle == Syncfusion.DocIO.DLS.TextWrappingStyle.Through)
      oPicture.TextWrappingStyle = Syncfusion.DocIO.ODF.Base.TextWrappingStyle.Through;
    else if (picture.TextWrappingStyle == Syncfusion.DocIO.DLS.TextWrappingStyle.Tight)
      oPicture.TextWrappingStyle = Syncfusion.DocIO.ODF.Base.TextWrappingStyle.Tight;
    else if (picture.TextWrappingStyle == Syncfusion.DocIO.DLS.TextWrappingStyle.TopAndBottom)
      oPicture.TextWrappingStyle = Syncfusion.DocIO.ODF.Base.TextWrappingStyle.TopAndBottom;
    if (!string.IsNullOrEmpty(picture.Name))
      oPicture.Name = picture.Name;
    oPicture.ShapeId = this.GetNextDocPrID();
    if (string.IsNullOrEmpty(picture.OPictureHRef))
      return;
    oPicture.OPictureHRef = picture.OPictureHRef;
  }

  private string ConvertToValidXmlString(string text)
  {
    string validXmlString = string.Empty;
    for (int index = 0; index < text.Length; ++index)
    {
      char character = text[index];
      validXmlString = !this.IsValidXmlChar((ushort) character) ? validXmlString + XmlConvert.EncodeName(character.ToString()) : validXmlString + character.ToString();
    }
    return validXmlString;
  }

  private bool IsValidXmlChar(ushort character)
  {
    if (character == (ushort) 9 || character == (ushort) 10 || character == (ushort) 13 || character >= (ushort) 32 /*0x20*/ && character <= (ushort) 55295)
      return true;
    return character >= (ushort) 57344 /*0xE000*/ && character <= (ushort) 65533;
  }

  private int GetNextDocPrID() => ++this.m_docPrId;

  private string UpdateShapeId(
    WPicture picture,
    bool isOlePicture,
    bool isPictureBullet,
    WOleObject oleObject)
  {
    string str = string.Empty;
    if (!isPictureBullet)
    {
      switch (isOlePicture ? this.GetOleObjectOwner(oleObject) : this.GetPictureOwner(picture))
      {
        case WSection _:
        case WTextBox _:
        case WTableRow _:
        case WParagraph _:
        case BlockContentControl _:
        case Shape _:
        case HeaderFooter _:
          str = this.AddImageRelation(this.m_oDocument.DocumentImages, picture.ImageRecord);
          break;
      }
    }
    else
      str = this.AddImageRelation(this.m_oDocument.DocumentImages, picture.ImageRecord);
    return str;
  }

  private string AddImageRelation(
    Dictionary<string, ImageRecord> imageCollection,
    ImageRecord imageRecord)
  {
    string key1 = string.Empty;
    if (imageCollection.ContainsValue(imageRecord))
    {
      foreach (string key2 in imageCollection.Keys)
      {
        if (imageRecord == imageCollection[key2])
        {
          key1 = key2;
          break;
        }
      }
    }
    else
    {
      key1 = this.GetNextRelationShipID();
      imageCollection.Add(key1, imageRecord);
    }
    return key1;
  }

  private string GetNextRelationShipID() => $"rId{++this.m_relationShipID}";

  private void ResetRelationShipID() => this.m_relationShipID = 0;

  private IEntity GetPictureOwner(WPicture pic)
  {
    Entity entity = pic.Owner;
    WParagraph wparagraph = (WParagraph) null;
    if (pic.Owner is WOleObject)
      entity = (Entity) (pic.Owner as WOleObject).OwnerParagraph;
    if (entity.EntityType == Syncfusion.DocIO.DLS.EntityType.InlineContentControl)
      wparagraph = entity.Owner as WParagraph;
    else if (entity.EntityType == Syncfusion.DocIO.DLS.EntityType.Paragraph)
      wparagraph = entity as WParagraph;
    WTableCell owner1 = wparagraph.Owner as WTableCell;
    Entity owner2 = wparagraph.Owner.Owner;
    Entity owner3 = (owner1 == null ? (IEntity) wparagraph.Owner : (IEntity) owner1.OwnerRow.OwnerTable.OwnerTextBody).Owner;
    return this.GetBaseEntity((Entity) pic) is HeaderFooter baseEntity ? (IEntity) baseEntity : (IEntity) owner3;
  }

  private IEntity GetOleObjectOwner(WOleObject oleObject)
  {
    WParagraph ownerParagraph = oleObject.OwnerParagraph;
    WTableCell owner1 = ownerParagraph.Owner as WTableCell;
    Entity owner2 = ownerParagraph.Owner.Owner;
    Entity owner3 = (owner1 == null ? (IEntity) oleObject.OwnerParagraph.Owner : (IEntity) owner1.OwnerRow.OwnerTable.OwnerTextBody).Owner;
    return this.GetBaseEntity((Entity) oleObject) is HeaderFooter baseEntity ? (IEntity) baseEntity : (IEntity) owner3;
  }

  private Entity GetBaseEntity(Entity entity)
  {
    Entity baseEntity = entity;
    while (baseEntity.Owner != null)
    {
      baseEntity = baseEntity.Owner;
      if (baseEntity is WSection || baseEntity is HeaderFooter)
        return baseEntity;
    }
    return baseEntity;
  }

  private void GetTableBorder(WTable table, OTable table1)
  {
    this.odfStyle = new ODFStyle();
    this.odfStyle.Family = ODFFontFamily.Table;
    this.odfStyle.TableProperties = new OTableProperties();
    this.odfStyle.TableProperties.TableWidth = table.Width / 72f;
    this.odfStyle.TableProperties.MarginLeft = (double) table.IndentFromLeft / 72.0;
    if (table.TableFormat.HorizontalAlignment == RowAlignment.Right)
      this.odfStyle.TableProperties.HoriAlignment = HoriAlignment.Right;
    if (table.Index == 0 && this.pageNames.Count > 0 && this.pageNames.Count > 0)
    {
      for (int index = 0; index <= this.pageNames.Count; ++index)
      {
        if (this.pageNames.Count != index)
        {
          this.odfStyle.MasterPageName = this.pageNames[index];
          this.odfStyle.ParentStyleName = table.StyleName;
        }
      }
    }
    this.odfstyleCollection1.Add(this.odfStyle);
    table1.StyleName = this.odfStyle.Name;
  }

  private void GetRowHeight(WTableRow row, OTableRow tableRow)
  {
    this.odfStyle = new ODFStyle();
    this.odfStyle.TableRowProperties = new OTableRowProperties();
    if ((double) row.Height > 0.0)
    {
      this.odfStyle.TableRowProperties = new OTableRowProperties();
      this.odfStyle.TableRowProperties.RowHeight = (double) row.Height / 72.0;
    }
    if (!row.RowFormat.IsBreakAcrossPages)
      this.odfStyle.TableRowProperties.IsBreakAcrossPages = true;
    if (row.RowFormat.IsHeaderRow)
      this.odfStyle.TableRowProperties.IsHeaderRow = true;
    this.odfStyle.Family = ODFFontFamily.Table_Row;
    this.odfstyleCollection1.Add(this.odfStyle);
    tableRow.StyleName = this.odfstyleCollection1.Add(this.odfStyle);
    this.odfstyleCollection1.Add(this.odfStyle);
    tableRow.StyleName = this.odfStyle.Name;
  }

  private Border GetRightBorder(int cellIndex, int cellLast, Borders borders, WTableCell m_cell)
  {
    Border rightBorder = borders.Right;
    if (!rightBorder.IsBorderDefined || rightBorder.IsBorderDefined && rightBorder.BorderType == BorderStyle.None && (double) rightBorder.LineWidth == 0.0 && rightBorder.Color.IsEmpty)
      rightBorder = cellIndex != cellLast ? m_cell.OwnerRow.RowFormat.Borders.Vertical : m_cell.OwnerRow.RowFormat.Borders.Right;
    if (!rightBorder.IsBorderDefined)
      rightBorder = cellIndex != cellLast ? m_cell.OwnerRow.OwnerTable.TableFormat.Borders.Vertical : m_cell.OwnerRow.OwnerTable.TableFormat.Borders.Right;
    return rightBorder;
  }

  private Border GetLeftBorder(int cellIndex, WTableCell m_cell, Borders borders)
  {
    Border leftBorder = borders.Left;
    if (!leftBorder.IsBorderDefined || leftBorder.IsBorderDefined && leftBorder.BorderType == BorderStyle.None && (double) leftBorder.LineWidth == 0.0 && leftBorder.Color.IsEmpty)
      leftBorder = cellIndex != 0 ? m_cell.OwnerRow.RowFormat.Borders.Vertical : m_cell.OwnerRow.RowFormat.Borders.Left;
    if (!leftBorder.IsBorderDefined)
      leftBorder = cellIndex != 0 ? m_cell.OwnerRow.OwnerTable.TableFormat.Borders.Vertical : m_cell.OwnerRow.OwnerTable.TableFormat.Borders.Left;
    return leftBorder;
  }

  private Border GetBottomBorder(
    int cellIndex,
    int cellLast,
    int rowIndex,
    int rowLast,
    WTableCell m_cell,
    Borders borders)
  {
    Border bottomBorder = borders.Bottom;
    if (!bottomBorder.IsBorderDefined || bottomBorder.IsBorderDefined && bottomBorder.BorderType == BorderStyle.None && (double) bottomBorder.LineWidth == 0.0 && bottomBorder.Color.IsEmpty)
      bottomBorder = rowIndex != rowLast ? m_cell.OwnerRow.RowFormat.Borders.Horizontal : m_cell.OwnerRow.RowFormat.Borders.Bottom;
    if (!bottomBorder.IsBorderDefined)
      bottomBorder = rowIndex != rowLast ? m_cell.OwnerRow.OwnerTable.TableFormat.Borders.Horizontal : m_cell.OwnerRow.OwnerTable.TableFormat.Borders.Bottom;
    return bottomBorder;
  }

  private Border GetTopBorder(
    int cellIndex,
    int rowIndex,
    Borders borders,
    WTableCell m_cell,
    int previousRowIndex)
  {
    Border topBorder = borders.Top;
    if (!topBorder.IsBorderDefined || topBorder.IsBorderDefined && topBorder.BorderType == BorderStyle.None && (double) topBorder.LineWidth == 0.0 && topBorder.Color.IsEmpty)
      topBorder = rowIndex != 0 ? m_cell.OwnerRow.RowFormat.Borders.Horizontal : m_cell.OwnerRow.RowFormat.Borders.Top;
    if (!topBorder.IsBorderDefined)
      topBorder = rowIndex != 0 ? m_cell.OwnerRow.OwnerTable.TableFormat.Borders.Horizontal : m_cell.OwnerRow.OwnerTable.TableFormat.Borders.Top;
    return topBorder;
  }

  private void GetCellStyle(
    WTableCell m_cell,
    TableStyleTableProperties tableStyle,
    OTableCell cell,
    Paddings paddings,
    WTableStyle tablebackcolor)
  {
    Borders borders = m_cell.CellFormat.Borders;
    int cellIndex = m_cell.GetCellIndex();
    int rowIndex = m_cell.OwnerRow.GetRowIndex();
    int cellLast = m_cell.OwnerRow.Cells.Count - 1;
    int rowLast = m_cell.OwnerRow.OwnerTable.Rows.Count - 1;
    Border topBorder = this.GetTopBorder(cellIndex, rowIndex, borders, m_cell, rowIndex - 1);
    Border bottomBorder = this.GetBottomBorder(cellIndex, cellLast, rowIndex, rowLast, m_cell, borders);
    Border leftBorder = this.GetLeftBorder(cellIndex, m_cell, borders);
    Border rightBorder = this.GetRightBorder(cellIndex, cellLast, borders, m_cell);
    if (topBorder.BorderType != BorderStyle.None && topBorder.BorderType == bottomBorder.BorderType && topBorder.BorderType == leftBorder.BorderType && topBorder.BorderType == rightBorder.BorderType && (double) topBorder.LineWidth != 0.0 && (double) topBorder.LineWidth == (double) bottomBorder.LineWidth && (double) topBorder.LineWidth == (double) leftBorder.LineWidth && (double) topBorder.LineWidth == (double) rightBorder.LineWidth && topBorder.Color != Color.Empty && topBorder.Color == bottomBorder.Color && topBorder.Color == leftBorder.Color && topBorder.Color == rightBorder.Color)
    {
      this.odfStyle = new ODFStyle();
      this.odfStyle.Family = ODFFontFamily.Table_Cell;
      this.odfStyle.TableCellProperties = new OTableCellProperties();
      this.odfStyle.TableCellProperties.Border = new ODFBorder();
      this.odfStyle.TableCellProperties.Border.LineWidth = (topBorder.LineWidth / 72f).ToString();
      this.odfStyle.TableCellProperties.Border.LineColor = topBorder.Color;
      this.odfStyle.TableCellProperties.Border.LineStyle = this.GetUnderlineStyle(topBorder.BorderType);
      if (paddings != null && (double) m_cell.CellFormat.Paddings.Top == 0.0 && (double) m_cell.CellFormat.Paddings.Bottom == 0.0 && (double) m_cell.CellFormat.Paddings.Left == 0.0 && (double) m_cell.CellFormat.Paddings.Right == 0.0)
      {
        this.odfStyle.TableCellProperties.PaddingTop = paddings.Top / 72f;
        this.odfStyle.TableCellProperties.PaddingRight = paddings.Right / 72f;
        this.odfStyle.TableCellProperties.PaddingLeft = paddings.Left / 72f;
        this.odfStyle.TableCellProperties.PaddingBottom = paddings.Bottom / 72f;
      }
      else if ((double) m_cell.CellFormat.Paddings.Bottom != 0.0 || (double) m_cell.CellFormat.Paddings.Left != 0.0 || (double) m_cell.CellFormat.Paddings.Right != 0.0 || (double) m_cell.CellFormat.Paddings.Top != 0.0)
      {
        this.odfStyle.TableCellProperties.PaddingTop = m_cell.CellFormat.Paddings.Top / 72f;
        this.odfStyle.TableCellProperties.PaddingRight = m_cell.CellFormat.Paddings.Right / 72f;
        this.odfStyle.TableCellProperties.PaddingLeft = m_cell.CellFormat.Paddings.Left / 72f;
        this.odfStyle.TableCellProperties.PaddingBottom = m_cell.CellFormat.Paddings.Bottom / 72f;
      }
      if (m_cell.CellFormat.BackColor != Color.Empty || m_cell.OwnerRow.RowFormat.BackColor != Color.Empty)
        this.odfStyle.TableCellProperties.BackColor = m_cell.CellFormat.BackColor;
      if (m_cell.CellFormat.VerticalAlignment == VerticalAlignment.Bottom)
        this.odfStyle.TableCellProperties.VerticalAlign = new VerticalAlign?(VerticalAlign.bottom);
      else if (m_cell.CellFormat.VerticalAlignment == VerticalAlignment.Middle)
        this.odfStyle.TableCellProperties.VerticalAlign = new VerticalAlign?(VerticalAlign.middle);
      else if (m_cell.CellFormat.VerticalAlignment == VerticalAlignment.Top)
        this.odfStyle.TableCellProperties.VerticalAlign = new VerticalAlign?(VerticalAlign.top);
      this.odfstyleCollection1.Add(this.odfStyle);
      cell.StyleName = this.odfStyle.Name;
    }
    else if ((double) topBorder.LineWidth > 0.0 || topBorder.BorderType != BorderStyle.None || topBorder.Color != Color.Empty || (double) rightBorder.LineWidth > 0.0 || rightBorder.BorderType != BorderStyle.None || rightBorder.Color != Color.Empty || (double) bottomBorder.LineWidth > 0.0 || bottomBorder.BorderType != BorderStyle.None || bottomBorder.Color != Color.Empty || (double) leftBorder.LineWidth > 0.0 || leftBorder.BorderType != BorderStyle.None || leftBorder.Color != Color.Empty)
    {
      this.odfStyle = new ODFStyle();
      this.odfStyle.Family = ODFFontFamily.Table_Cell;
      this.odfStyle.TableCellProperties = new OTableCellProperties();
      if ((double) topBorder.LineWidth > 0.0 || topBorder.BorderType != BorderStyle.None || topBorder.Color != Color.Empty)
      {
        this.odfStyle.TableCellProperties.BorderTop = new ODFBorder();
        if ((double) paddings.Top > 0.0)
          this.odfStyle.TableCellProperties.PaddingTop = paddings.Top / 72f;
        this.odfStyle.TableCellProperties.BorderTop.LineWidth = (topBorder.LineWidth / 72f).ToString();
        this.odfStyle.TableCellProperties.BorderTop.LineStyle = this.GetUnderlineStyle(topBorder.BorderType);
        this.odfStyle.TableCellProperties.BorderTop.LineColor = topBorder.Color;
      }
      if ((double) rightBorder.LineWidth > 0.0 || rightBorder.BorderType != BorderStyle.None || rightBorder.Color != Color.Empty)
      {
        this.odfStyle.TableCellProperties.BorderRight = new ODFBorder();
        if ((double) paddings.Right > 0.0)
          this.odfStyle.TableCellProperties.PaddingRight = paddings.Right / 72f;
        this.odfStyle.TableCellProperties.BorderRight.LineWidth = (rightBorder.LineWidth / 72f).ToString();
        this.odfStyle.TableCellProperties.BorderRight.LineStyle = this.GetUnderlineStyle(rightBorder.BorderType);
        this.odfStyle.TableCellProperties.BorderRight.LineColor = rightBorder.Color;
      }
      if ((double) bottomBorder.LineWidth > 0.0 || bottomBorder.BorderType != BorderStyle.None || bottomBorder.Color != Color.Empty)
      {
        this.odfStyle.TableCellProperties.BorderBottom = new ODFBorder();
        if ((double) paddings.Bottom > 0.0)
          this.odfStyle.TableCellProperties.PaddingBottom = paddings.Bottom / 72f;
        this.odfStyle.TableCellProperties.BorderBottom.LineWidth = (bottomBorder.LineWidth / 72f).ToString();
        this.odfStyle.TableCellProperties.BorderBottom.LineStyle = this.GetUnderlineStyle(bottomBorder.BorderType);
        this.odfStyle.TableCellProperties.BorderBottom.LineColor = bottomBorder.Color;
      }
      if ((double) leftBorder.LineWidth > 0.0 || leftBorder.BorderType != BorderStyle.None || leftBorder.Color != Color.Empty)
      {
        this.odfStyle.TableCellProperties.BorderLeft = new ODFBorder();
        if ((double) paddings.Left > 0.0)
          this.odfStyle.TableCellProperties.PaddingLeft = paddings.Left / 72f;
        this.odfStyle.TableCellProperties.BorderLeft.LineWidth = (leftBorder.LineWidth / 72f).ToString();
        this.odfStyle.TableCellProperties.BorderLeft.LineStyle = this.GetUnderlineStyle(leftBorder.BorderType);
        this.odfStyle.TableCellProperties.BorderLeft.LineColor = leftBorder.Color;
      }
      if (m_cell.CellFormat.BackColor != Color.Empty || m_cell.OwnerRow.RowFormat.BackColor != Color.Empty)
        this.odfStyle.TableCellProperties.BackColor = m_cell.CellFormat.BackColor;
      if (m_cell.CellFormat.VerticalAlignment == VerticalAlignment.Bottom)
        this.odfStyle.TableCellProperties.VerticalAlign = new VerticalAlign?(VerticalAlign.bottom);
      else if (m_cell.CellFormat.VerticalAlignment == VerticalAlignment.Middle)
        this.odfStyle.TableCellProperties.VerticalAlign = new VerticalAlign?(VerticalAlign.middle);
      else if (m_cell.CellFormat.VerticalAlignment == VerticalAlignment.Top)
        this.odfStyle.TableCellProperties.VerticalAlign = new VerticalAlign?(VerticalAlign.top);
      this.odfstyleCollection1.Add(this.odfStyle);
      cell.StyleName = this.odfStyle.Name;
    }
    else
    {
      if (tableStyle == null)
        return;
      if (tableStyle.Borders.Top.BorderType != BorderStyle.None && tableStyle.Borders.Top.BorderType == tableStyle.Borders.Bottom.BorderType && tableStyle.Borders.Top.BorderType == tableStyle.Borders.Left.BorderType && tableStyle.Borders.Top.BorderType == tableStyle.Borders.Right.BorderType && (double) tableStyle.Borders.Top.LineWidth != 0.0 && (double) tableStyle.Borders.Top.LineWidth == (double) tableStyle.Borders.Bottom.LineWidth && (double) tableStyle.Borders.Top.LineWidth == (double) tableStyle.Borders.Left.LineWidth && (double) tableStyle.Borders.Top.LineWidth == (double) tableStyle.Borders.Right.LineWidth && tableStyle.Borders.Top.Color != Color.Empty && tableStyle.Borders.Top.Color == tableStyle.Borders.Bottom.Color && tableStyle.Borders.Top.Color == tableStyle.Borders.Left.Color && tableStyle.Borders.Top.Color == tableStyle.Borders.Right.Color)
      {
        this.odfStyle = new ODFStyle();
        this.odfStyle.Family = ODFFontFamily.Table_Cell;
        this.odfStyle.TableCellProperties = new OTableCellProperties();
        if (bottomBorder.BorderType != BorderStyle.None && topBorder.BorderType != BorderStyle.None && leftBorder.BorderType != BorderStyle.None && rightBorder.BorderType != BorderStyle.None)
        {
          this.odfStyle.TableCellProperties.Border = new ODFBorder();
          this.odfStyle.TableCellProperties.Border.LineWidth = (tableStyle.Borders.Top.LineWidth / 72f).ToString();
          this.odfStyle.TableCellProperties.Border.LineColor = tableStyle.Borders.Top.Color;
          this.odfStyle.TableCellProperties.Border.LineStyle = this.GetUnderlineStyle(tableStyle.Borders.Top.BorderType);
        }
        if (tableStyle.Paddings != null)
        {
          this.odfStyle.TableCellProperties.PaddingTop = tableStyle.Paddings.Top / 72f;
          this.odfStyle.TableCellProperties.PaddingRight = tableStyle.Paddings.Right / 72f;
          this.odfStyle.TableCellProperties.PaddingLeft = tableStyle.Paddings.Left / 72f;
          this.odfStyle.TableCellProperties.PaddingBottom = tableStyle.Paddings.Bottom / 72f;
        }
        if (tablebackcolor.CellProperties.BackColor != Color.Empty)
          this.odfStyle.TableCellProperties.BackColor = tablebackcolor.CellProperties.BackColor;
        if (m_cell.CellFormat.VerticalAlignment == VerticalAlignment.Bottom)
          this.odfStyle.TableCellProperties.VerticalAlign = new VerticalAlign?(VerticalAlign.bottom);
        else if (m_cell.CellFormat.VerticalAlignment == VerticalAlignment.Middle)
          this.odfStyle.TableCellProperties.VerticalAlign = new VerticalAlign?(VerticalAlign.middle);
        else if (m_cell.CellFormat.VerticalAlignment == VerticalAlignment.Top)
          this.odfStyle.TableCellProperties.VerticalAlign = new VerticalAlign?(VerticalAlign.top);
        this.odfstyleCollection1.Add(this.odfStyle);
        cell.StyleName = this.odfStyle.Name;
      }
      else
      {
        this.odfStyle = new ODFStyle();
        this.odfStyle.Family = ODFFontFamily.Table_Cell;
        this.odfStyle.TableCellProperties = new OTableCellProperties();
        if ((double) tableStyle.Borders.Top.LineWidth > 0.0 || tableStyle.Borders.Top.BorderType != BorderStyle.None || tableStyle.Borders.Top.Color != Color.Empty)
        {
          this.odfStyle.TableCellProperties.BorderTop = new ODFBorder();
          if ((double) tableStyle.Borders.Top.Space > 0.0)
            this.odfStyle.TableCellProperties.PaddingTop = tableStyle.Borders.Top.Space / 72f;
          else if ((double) m_cell.CellFormat.Paddings.Top > 0.0)
            this.odfStyle.TableCellProperties.PaddingTop = m_cell.CellFormat.Paddings.Top / 72f;
          this.odfStyle.TableCellProperties.BorderTop.LineWidth = (tableStyle.Borders.Top.LineWidth / 72f).ToString();
          this.odfStyle.TableCellProperties.BorderTop.LineStyle = this.GetUnderlineStyle(tableStyle.Borders.Top.BorderType);
          this.odfStyle.TableCellProperties.BorderTop.LineColor = tableStyle.Borders.Top.Color;
        }
        if ((double) tableStyle.Borders.Right.LineWidth > 0.0 || tableStyle.Borders.Right.BorderType != BorderStyle.None || tableStyle.Borders.Right.Color != Color.Empty)
        {
          this.odfStyle.TableCellProperties.BorderRight = new ODFBorder();
          if ((double) tableStyle.Borders.Right.Space > 0.0)
            this.odfStyle.TableCellProperties.PaddingRight = tableStyle.Borders.Right.Space / 72f;
          else if ((double) m_cell.CellFormat.Paddings.Right > 0.0)
            this.odfStyle.TableCellProperties.PaddingRight = m_cell.CellFormat.Paddings.Right / 72f;
          this.odfStyle.TableCellProperties.BorderRight.LineWidth = (tableStyle.Borders.Right.LineWidth / 72f).ToString();
          this.odfStyle.TableCellProperties.BorderRight.LineStyle = this.GetUnderlineStyle(tableStyle.Borders.Right.BorderType);
          this.odfStyle.TableCellProperties.BorderRight.LineColor = tableStyle.Borders.Right.Color;
        }
        if ((double) tableStyle.Borders.Bottom.LineWidth > 0.0 || tableStyle.Borders.Bottom.BorderType != BorderStyle.None || tableStyle.Borders.Bottom.Color != Color.Empty)
        {
          this.odfStyle.TableCellProperties.BorderBottom = new ODFBorder();
          if ((double) tableStyle.Borders.Bottom.Space > 0.0)
            this.odfStyle.TableCellProperties.PaddingBottom = tableStyle.Borders.Bottom.Space / 72f;
          else if ((double) m_cell.CellFormat.Paddings.Bottom > 0.0)
            this.odfStyle.TableCellProperties.PaddingBottom = m_cell.CellFormat.Paddings.Bottom / 72f;
          this.odfStyle.TableCellProperties.BorderBottom.LineWidth = (tableStyle.Borders.Bottom.LineWidth / 72f).ToString();
          this.odfStyle.TableCellProperties.BorderBottom.LineStyle = this.GetUnderlineStyle(tableStyle.Borders.Bottom.BorderType);
          this.odfStyle.TableCellProperties.BorderBottom.LineColor = tableStyle.Borders.Bottom.Color;
        }
        if ((double) tableStyle.Borders.Left.LineWidth > 0.0 || tableStyle.Borders.Left.BorderType != BorderStyle.None || tableStyle.Borders.Left.Color != Color.Empty)
        {
          this.odfStyle.TableCellProperties.BorderLeft = new ODFBorder();
          if ((double) tableStyle.Borders.Left.Space > 0.0)
            this.odfStyle.TableCellProperties.PaddingLeft = tableStyle.Borders.Left.Space / 72f;
          else if ((double) m_cell.CellFormat.Paddings.Left > 0.0)
            this.odfStyle.TableCellProperties.PaddingLeft = m_cell.CellFormat.Paddings.Left / 72f;
          this.odfStyle.TableCellProperties.BorderLeft.LineWidth = (tableStyle.Borders.Left.LineWidth / 72f).ToString();
          this.odfStyle.TableCellProperties.BorderLeft.LineStyle = this.GetUnderlineStyle(tableStyle.Borders.Left.BorderType);
          this.odfStyle.TableCellProperties.BorderLeft.LineColor = tableStyle.Borders.Left.Color;
        }
        if (tablebackcolor.CellProperties.BackColor != Color.Empty)
          this.odfStyle.TableCellProperties.BackColor = tablebackcolor.CellProperties.BackColor;
        if (m_cell.CellFormat.VerticalAlignment == VerticalAlignment.Bottom)
          this.odfStyle.TableCellProperties.VerticalAlign = new VerticalAlign?(VerticalAlign.bottom);
        else if (m_cell.CellFormat.VerticalAlignment == VerticalAlignment.Middle)
          this.odfStyle.TableCellProperties.VerticalAlign = new VerticalAlign?(VerticalAlign.middle);
        else if (m_cell.CellFormat.VerticalAlignment == VerticalAlignment.Top)
          this.odfStyle.TableCellProperties.VerticalAlign = new VerticalAlign?(VerticalAlign.top);
        this.odfstyleCollection1.Add(this.odfStyle);
        cell.StyleName = this.odfStyle.Name;
      }
    }
  }

  private void GetColumnWidth(WTable table, OTableColumn cellColumn, OTable table1)
  {
    if (table.TableGrid.Count == 0)
      return;
    WTableColumnCollection tableGrid = table.TableGrid;
    float num1 = 0.0f;
    if (tableGrid.Count <= 0)
      return;
    int index = 0;
    for (int count = tableGrid.Count; index < count; ++index)
    {
      cellColumn = new OTableColumn();
      float endOffset = tableGrid[index].EndOffset;
      double num2 = Math.Round((double) (endOffset - num1)) / 1440.0;
      this.odfStyle = new ODFStyle();
      this.odfStyle.Family = ODFFontFamily.Table_Column;
      this.odfStyle.TableColumnProperties = new OTableColumnProperties();
      this.odfStyle.TableColumnProperties.ColumnWidth = num2;
      this.odfstyleCollection1.Add(this.odfStyle);
      num1 = endOffset;
      cellColumn.StyleName = this.odfStyle.Name;
      table1.Columns.Add(cellColumn);
    }
  }

  private Paddings GetCellPaddingBasedOnTable(WTableCell cell)
  {
    return new Paddings()
    {
      Left = !cell.OwnerRow.RowFormat.Paddings.HasKey(1) ? (!cell.OwnerRow.OwnerTable.TableFormat.Paddings.HasKey(1) ? (cell.Document.ActualFormatType != FormatType.Doc ? 5.4f : 0.0f) : cell.OwnerRow.OwnerTable.TableFormat.Paddings.Left) : cell.OwnerRow.RowFormat.Paddings.Left,
      Right = !cell.OwnerRow.RowFormat.Paddings.HasKey(4) ? (!cell.OwnerRow.OwnerTable.TableFormat.Paddings.HasKey(4) ? (cell.Document.ActualFormatType != FormatType.Doc ? 5.4f : 0.0f) : cell.OwnerRow.OwnerTable.TableFormat.Paddings.Right) : cell.OwnerRow.RowFormat.Paddings.Right,
      Top = !cell.OwnerRow.RowFormat.Paddings.HasKey(2) ? (!cell.OwnerRow.OwnerTable.TableFormat.Paddings.HasKey(2) ? 0.0f : cell.OwnerRow.OwnerTable.TableFormat.Paddings.Top) : cell.OwnerRow.RowFormat.Paddings.Top,
      Bottom = !cell.OwnerRow.RowFormat.Paddings.HasKey(3) ? (!cell.OwnerRow.OwnerTable.TableFormat.Paddings.HasKey(3) ? 0.0f : cell.OwnerRow.OwnerTable.TableFormat.Paddings.Bottom) : cell.OwnerRow.RowFormat.Paddings.Bottom
    };
  }

  private OTextBodyItem GetTableContent(TextBodyItem TextbodyItem)
  {
    List<OTable> otableList = new List<OTable>();
    WTable table = TextbodyItem as WTable;
    WTableStyle style = table.GetStyle() as WTableStyle;
    TableStyleTableProperties tableStyle = (TableStyleTableProperties) null;
    if (style != null)
      tableStyle = style.TableProperties;
    OTableColumn otableColumn = new OTableColumn();
    OTable table1 = new OTable();
    this.GetTableBorder(table, table1);
    OTableColumn cellColumn = new OTableColumn();
    this.GetColumnWidth(table, cellColumn, table1);
    WRowCollection rows = table.Rows;
    for (int index1 = 0; index1 < rows.Count; ++index1)
    {
      WTableRow row = rows[index1];
      int num = row.RowFormat.IsBreakAcrossPages ? 1 : 0;
      OTableRow tableRow = new OTableRow();
      this.GetRowHeight(row, tableRow);
      if (row.Cells.Count > 0)
      {
        for (int index2 = 0; index2 < row.Cells.Count; ++index2)
        {
          WTableCell cell1 = row.Cells[index2];
          OTableCell cell2 = new OTableCell();
          Paddings paddingBasedOnTable = this.GetCellPaddingBasedOnTable(cell1);
          if (cell1.GridSpan > (short) 1)
            cell2.ColumnsSpanned = (int) cell1.GridSpan;
          this.GetCellStyle(cell1, tableStyle, cell2, paddingBasedOnTable, style);
          BodyItemCollection items = cell1.Items;
          for (int index3 = 0; index3 < items.Count; ++index3)
          {
            OTextBodyItem otextBodyItem = this.GetOTextBodyItem(items[index3]);
            cell2.TextBodyIetm.Add(otextBodyItem);
          }
          tableRow.Cells.Add(cell2);
        }
      }
      table1.Rows.Add(tableRow);
    }
    otableList.Add(table1);
    return (OTextBodyItem) table1;
  }

  private string CombineTextInSubsequentTextRanges(
    ParagraphItemCollection paraItemCollection,
    ref int index)
  {
    WTextRange wtextRange = paraItemCollection[index] as WTextRange;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(wtextRange.Text);
    while (wtextRange.NextSibling != null && wtextRange.NextSibling.EntityType == Syncfusion.DocIO.DLS.EntityType.TextRange && wtextRange.CharacterFormat.Compare((wtextRange.NextSibling as WTextRange).CharacterFormat))
    {
      wtextRange = wtextRange.NextSibling as WTextRange;
      stringBuilder.Append(wtextRange.Text);
      ++index;
    }
    return stringBuilder.ToString();
  }

  internal void LoadHeaderFooterContents(WHeadersFooters headersFooters, MasterPage page)
  {
    this.IsWritingHeaderFooter = true;
    if (headersFooters.OddHeader != null)
    {
      HeaderFooterContent oHeaderFooter = new HeaderFooterContent();
      page.Header = this.GetHeaderFooterContent(oHeaderFooter, headersFooters.OddHeader);
    }
    if (headersFooters.EvenHeader != null)
    {
      HeaderFooterContent oHeaderFooter = new HeaderFooterContent();
      page.HeaderLeft = this.GetHeaderFooterContent(oHeaderFooter, headersFooters.EvenHeader);
    }
    if (headersFooters.FirstPageHeader != null)
    {
      HeaderFooterContent oHeaderFooter = new HeaderFooterContent();
      page.FirstPageHeader = this.GetHeaderFooterContent(oHeaderFooter, headersFooters.FirstPageHeader);
    }
    if (headersFooters.OddFooter != null)
    {
      HeaderFooterContent oHeaderFooter = new HeaderFooterContent();
      page.Footer = this.GetHeaderFooterContent(oHeaderFooter, headersFooters.OddFooter);
    }
    if (headersFooters.EvenFooter != null)
    {
      HeaderFooterContent oHeaderFooter = new HeaderFooterContent();
      page.FooterLeft = this.GetHeaderFooterContent(oHeaderFooter, headersFooters.EvenFooter);
    }
    if (headersFooters.FirstPageFooter != null)
    {
      HeaderFooterContent oHeaderFooter = new HeaderFooterContent();
      page.FirstPageFooter = this.GetHeaderFooterContent(oHeaderFooter, headersFooters.FirstPageFooter);
    }
    this.IsWritingHeaderFooter = false;
  }

  internal HeaderFooterContent GetHeaderFooterContent(
    HeaderFooterContent oHeaderFooter,
    HeaderFooter headerFooter)
  {
    foreach (TextBodyItem childEntity in (Syncfusion.DocIO.DLS.CollectionImpl) headerFooter.ChildEntities)
    {
      OTextBodyItem otextBodyItem = this.GetOTextBodyItem(childEntity);
      oHeaderFooter.ChildItems.Add(otextBodyItem);
    }
    return oHeaderFooter.ChildItems.Count <= 0 ? (HeaderFooterContent) null : oHeaderFooter;
  }

  private void ConvertAutomaticAndMasterStyles()
  {
    PageLayoutCollection layouts = new PageLayoutCollection();
    MasterPageCollection mPages = new MasterPageCollection();
    for (int index = 0; index < this.m_document.Sections.Count; ++index)
    {
      PageLayout layout = new PageLayout();
      WSection section = this.m_document.Sections[index];
      if (section.Columns.Count > 1)
      {
        layout.ColumnsCount = section.Columns.Count;
        layout.ColumnsGap = section.Columns[0].Space / 72f;
      }
      bool flag = section.BreakCode == SectionBreakCode.NoBreak;
      string key1 = layouts.Add(layout);
      if (flag)
        layouts.Remove(key1);
      MasterPage masterPage = new MasterPage();
      this.LoadHeaderFooterContents(section.HeadersFooters, masterPage);
      masterPage.PageLayoutName = key1;
      if (!flag)
        this.ConvertPageLayOut(section.PageSetup, layout, masterPage);
      string key2 = mPages.Add(masterPage);
      this.pageNames.Add(key2);
      if (flag)
      {
        mPages.Remove(key2);
        this.pageNames.Remove(key2);
      }
    }
    this.m_writer.SerializeAutomaticStyles(layouts);
    if (this.m_document.ListStyles != null && this.m_document.ListStyles.Count > 0)
      this.MapListStyles();
    if (this.odfstyleCollection1.DictStyles.Count > 0)
      this.m_writer.SerializeContentAutoStyles(this.odfstyleCollection1);
    this.m_writer.SerializeEnd();
    if (this.m_oDocument.ListStyles != null && this.m_oDocument.ListStyles.Count > 0)
      this.m_writer.SerializeContentListStyles(this.m_oDocument.ListStyles);
    this.m_writer.SerializeMasterStylesStart();
    this.m_writer.SerializeMasterStyles(mPages, this.pageNames);
    this.m_writer.SerializeEnd();
  }

  private void MapListStyles()
  {
    this.odfStyle = new ODFStyle();
    foreach (ListStyle listStyle in (Syncfusion.DocIO.DLS.CollectionImpl) this.m_document.ListStyles)
    {
      OListStyle olistStyle = new OListStyle();
      int count = this.m_oDocument.ListStyles.Count;
      int num1;
      this.odfStyle.ListStyleName = "LFO" + (object) (num1 = count + 1);
      olistStyle.Name = listStyle.Name;
      olistStyle.CurrentStyleName = this.odfStyle.ListStyleName;
      int num2 = 1;
      for (int index = 0; index < listStyle.Levels.Count; ++index)
      {
        WListLevel level = listStyle.Levels[index];
        ListLevelProperties listLevelProperties = new ListLevelProperties();
        if (level.CharacterFormat != null && !level.CharacterFormat.IsDefault)
        {
          listLevelProperties.Style = new ODFStyle();
          listLevelProperties.Style.Textproperties = this.CopyCharFormatToTextFormat(level.CharacterFormat, listLevelProperties.Style.Textproperties);
          if (listLevelProperties.Style.Textproperties != null)
          {
            listLevelProperties.Style.Family = ODFFontFamily.Text;
            listLevelProperties.Style.Name = $"WWChar{this.odfStyle.ListStyleName}LVL{(object) num2}";
          }
          this.odfstyleCollection1.Add(listLevelProperties.Style);
        }
        if (listLevelProperties.Style != null && listLevelProperties.Style.Textproperties != null)
          listLevelProperties.TextProperties = listLevelProperties.Style.Textproperties;
        if (level.PatternType == ListPatternType.Arabic)
          listLevelProperties.NumberFormat = ListNumberFormat.Decimal;
        else if (level.PatternType == ListPatternType.LowRoman)
          listLevelProperties.NumberFormat = ListNumberFormat.LowerRoman;
        else if (level.PatternType == ListPatternType.UpLetter)
          listLevelProperties.NumberFormat = ListNumberFormat.UpperLetter;
        else if (level.PatternType == ListPatternType.UpRoman)
          listLevelProperties.NumberFormat = ListNumberFormat.UpperRoman;
        else if (level.PatternType == ListPatternType.LowLetter)
          listLevelProperties.NumberFormat = ListNumberFormat.LowerLetter;
        else if (level.PatternType == ListPatternType.Bullet)
          listLevelProperties.NumberFormat = ListNumberFormat.Bullet;
        listLevelProperties.NumberSufix = level.NumberSuffix;
        listLevelProperties.LeftMargin = level.ParagraphFormat.LeftIndent / 72f;
        listLevelProperties.TextIndent = level.ParagraphFormat.FirstLineIndent / 72f;
        listLevelProperties.SpaceBefore = listLevelProperties.LeftMargin - Math.Abs(listLevelProperties.TextIndent);
        listLevelProperties.MinimumLabelWidth = Math.Abs(listLevelProperties.TextIndent);
        listLevelProperties.TextAlignment = this.GetListLevelAlingment(level.NumberAlignment);
        if (!string.IsNullOrEmpty(level.BulletCharacter))
          listLevelProperties.BulletCharacter = level.BulletCharacter;
        if (level.PicBullet != null)
        {
          OPicture oPicture = new OPicture();
          WPicture picBullet = level.PicBullet;
          this.odfStyle = new ODFStyle();
          this.GetOPicture(oPicture, picBullet, this.odfStyle);
          this.UpdateShapeId(picBullet, false, true, (WOleObject) null);
          listLevelProperties.PictureBullet = oPicture;
          listLevelProperties.PictureHRef = picBullet.OPictureHRef;
          listLevelProperties.IsPictureBullet = true;
        }
        olistStyle.ListLevels.Add(listLevelProperties);
        ++num2;
      }
      this.m_oDocument.ListStyles.Add(olistStyle);
    }
  }

  private TextAlign GetListLevelAlingment(ListNumberAlignment numberAlignment)
  {
    switch (numberAlignment)
    {
      case ListNumberAlignment.Left:
        return TextAlign.start;
      case ListNumberAlignment.Center:
        return TextAlign.center;
      case ListNumberAlignment.Right:
        return TextAlign.end;
      default:
        return TextAlign.start;
    }
  }

  private void ConvertPageLayOut(WPageSetup pageSetup, PageLayout layout, MasterPage masterPage)
  {
    layout.PageLayoutProperties.PageWidth = (double) pageSetup.PageSize.Width / 72.0;
    layout.PageLayoutProperties.PageHeight = (double) pageSetup.PageSize.Height / 72.0;
    if (!pageSetup.Borders.NoBorder)
    {
      layout.PageLayoutProperties.MarginTop = (double) pageSetup.Borders.Top.Space / 72.0;
      layout.PageLayoutProperties.MarginBottom = (double) pageSetup.Borders.Bottom.Space / 72.0;
      layout.PageLayoutProperties.MarginLeft = (double) pageSetup.Borders.Left.Space / 72.0;
      layout.PageLayoutProperties.MarginRight = (double) pageSetup.Borders.Right.Space / 72.0;
      layout.PageLayoutProperties.Border = new ODFBorder();
      float num = pageSetup.Borders.Top.LineWidth / 72f;
      layout.PageLayoutProperties.Border.LineWidth = num.ToString();
      layout.PageLayoutProperties.Border.LineColor = pageSetup.Borders.Top.Color;
      layout.PageLayoutProperties.Border.LineStyle = this.GetUnderlineStyle(pageSetup.Borders.Top.BorderType);
    }
    if (pageSetup.Margins != null)
    {
      layout.PageLayoutProperties.MarginTop = (double) pageSetup.Margins.Top / 72.0;
      layout.PageLayoutProperties.MarginBottom = (double) pageSetup.Margins.Bottom / 72.0;
      layout.PageLayoutProperties.MarginLeft = (double) pageSetup.Margins.Left / 72.0;
      layout.PageLayoutProperties.MarginRight = (double) pageSetup.Margins.Right / 72.0;
    }
    layout.PageLayoutProperties.PageOrientation = (PrintOrientation) pageSetup.Orientation;
    if (masterPage.Header == null && masterPage.HeaderLeft == null && masterPage.FirstPageHeader == null && masterPage.Footer == null && masterPage.FooterLeft == null && masterPage.FirstPageFooter == null)
      return;
    double num1 = (double) pageSetup.HeaderDistance / 72.0;
    double marginTop = layout.PageLayoutProperties.MarginTop;
    if (marginTop > num1)
      layout.HeaderStyle.HeaderDistance = marginTop - num1;
    layout.PageLayoutProperties.MarginTop = num1;
    double num2 = (double) pageSetup.FooterDistance / 72.0;
    double marginBottom = layout.PageLayoutProperties.MarginBottom;
    if (marginBottom > num2)
      layout.FooterStyle.FooterDistance = marginBottom - num2;
    layout.PageLayoutProperties.MarginBottom = num2;
  }

  internal bool StartsWithExt(string text, string value) => text.StartsWithExt(value);

  internal void Close()
  {
    if (this.m_oDocument != null)
    {
      this.m_oDocument.Close();
      this.m_oDocument = (ODocument) null;
    }
    if (this.paragraph != null)
    {
      this.paragraph.Dispose();
      this.paragraph = (OParagraph) null;
    }
    if (this.opara != null)
    {
      this.opara.Dispose();
      this.opara = (OParagraphItem) null;
    }
    if (this.odfStyle != null)
    {
      this.odfStyle.Close();
      this.odfStyle = (ODFStyle) null;
    }
    if (this.m_writer != null)
    {
      this.m_writer.Dispose();
      this.m_writer = (ODFWriter) null;
    }
    if (this.odfstyleCollection1 == null)
      return;
    this.odfstyleCollection1.Dispose();
    this.odfstyleCollection1 = (ODFStyleCollection) null;
  }
}
