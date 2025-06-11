// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Charts.ChartParserCommon
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Compression;
using Syncfusion.XlsIO.Implementation.Charts;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Implementation.XmlReaders;
using Syncfusion.XlsIO.Implementation.XmlReaders.Shapes;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Interfaces.Charts;
using Syncfusion.XlsIO.Parser.Biff_Records.Charts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization.Charts;

internal class ChartParserCommon
{
  private const string NullString = "null";
  private const int DefaultShadowSize = 100;
  private const int DefaultBlurValue = 0;
  private const int DefaultAngleValue = 0;
  private const int DefaultDistanceValue = 0;
  private static Dictionary<KeyValuePair<string, string>, ExcelChartLinePattern> s_dicLinePatterns = new Dictionary<KeyValuePair<string, string>, ExcelChartLinePattern>();
  private static WorkbookImpl m_book;

  static ChartParserCommon()
  {
    ChartParserCommon.s_dicLinePatterns.Add(new KeyValuePair<string, string>("solid", string.Empty), ExcelChartLinePattern.Solid);
    ChartParserCommon.s_dicLinePatterns.Add(new KeyValuePair<string, string>("lgDash", string.Empty), ExcelChartLinePattern.LongDash);
    ChartParserCommon.s_dicLinePatterns.Add(new KeyValuePair<string, string>("sysDash", string.Empty), ExcelChartLinePattern.Dot);
    ChartParserCommon.s_dicLinePatterns.Add(new KeyValuePair<string, string>("sysDot", string.Empty), ExcelChartLinePattern.CircleDot);
    ChartParserCommon.s_dicLinePatterns.Add(new KeyValuePair<string, string>("dash", string.Empty), ExcelChartLinePattern.Dash);
    ChartParserCommon.s_dicLinePatterns.Add(new KeyValuePair<string, string>("lgDashDot", string.Empty), ExcelChartLinePattern.LongDashDot);
    ChartParserCommon.s_dicLinePatterns.Add(new KeyValuePair<string, string>("dashDot", string.Empty), ExcelChartLinePattern.DashDot);
    ChartParserCommon.s_dicLinePatterns.Add(new KeyValuePair<string, string>("lgDashDotDot", string.Empty), ExcelChartLinePattern.LongDashDotDot);
    ChartParserCommon.s_dicLinePatterns.Add(new KeyValuePair<string, string>("solid", "pct75"), ExcelChartLinePattern.DarkGray);
    ChartParserCommon.s_dicLinePatterns.Add(new KeyValuePair<string, string>("solid", "pct50"), ExcelChartLinePattern.MediumGray);
    ChartParserCommon.s_dicLinePatterns.Add(new KeyValuePair<string, string>("solid", "pct25"), ExcelChartLinePattern.LightGray);
  }

  public static void SetWorkbook(WorkbookImpl book) => ChartParserCommon.m_book = book;

  public static void ParseTextArea(
    XmlReader reader,
    IInternalChartTextArea textArea,
    FileDataHolder holder,
    RelationCollection relations)
  {
    ChartParserCommon.ParseTextArea(reader, textArea, holder, relations, new float?());
  }

  public static void ParseTextArea(
    XmlReader reader,
    IInternalChartTextArea textArea,
    FileDataHolder holder,
    RelationCollection relations,
    float? defaultFontSize)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textArea == null)
      throw new ArgumentNullException(nameof (textArea));
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
        ChartParserCommon.ParseTextAreaTag(reader, textArea, relations, holder, defaultFontSize);
    }
    reader.Read();
  }

  public static void ParseTextAreaTag(
    XmlReader reader,
    IInternalChartTextArea textArea,
    RelationCollection relations,
    FileDataHolder holder,
    float? defaultFontSize)
  {
    if (reader.NodeType == XmlNodeType.Element)
    {
      switch (reader.LocalName)
      {
        case "tx":
          textArea.Text = string.Empty;
          if (textArea is ChartDataLabelsImpl)
          {
            bool showTextProperties = (textArea as ChartDataLabelsImpl).ShowTextProperties;
            ChartParserCommon.ParseTextAreaText(reader, textArea, holder.Parser, defaultFontSize);
            if (showTextProperties)
              break;
            (textArea as ChartDataLabelsImpl).ShowTextProperties = false;
            break;
          }
          ChartParserCommon.ParseTextAreaText(reader, textArea, holder.Parser, defaultFontSize);
          break;
        case "layout":
          textArea.Layout = (IChartLayout) new ChartLayoutImpl(ChartParserCommon.m_book.Application, (object) textArea, textArea.Parent);
          ChartParserCommon.ParseChartLayout(reader, textArea.Layout);
          break;
        case "spPr":
          IChartFrameFormat frameFormat = textArea.FrameFormat;
          IChartFillObjectGetter objectGetter = (IChartFillObjectGetter) new ChartFillObjectGetterAny(frameFormat.Border as ChartBorderImpl, frameFormat.Interior as ChartInteriorImpl, frameFormat.Fill as IInternalFill, frameFormat.Shadow as ShadowImpl, frameFormat.ThreeD as ThreeDFormatImpl);
          ChartParserCommon.ParseShapeProperties(reader, objectGetter, holder, relations);
          break;
        case "txPr":
          if (textArea is ChartTextAreaImpl chartTextAreaImpl1 && !chartTextAreaImpl1.IsTextParsed)
          {
            chartTextAreaImpl1.ParagraphType = ChartParagraphType.CustomDefault;
            XmlReader reader1 = reader;
            IInternalChartTextArea textFormatting = textArea;
            Excel2007Parser parser = holder.Parser;
            float? nullable = defaultFontSize;
            double? defaultFontSize1 = nullable.HasValue ? new double?((double) nullable.GetValueOrDefault()) : new double?();
            ChartParserCommon.ParseDefaultTextFormatting(reader1, textFormatting, parser, defaultFontSize1);
            break;
          }
          reader.Skip();
          break;
        case "overlay":
          if (!(textArea is ChartTextAreaImpl chartTextAreaImpl2))
            break;
          chartTextAreaImpl2.Overlay = ChartParserCommon.ParseBoolValueTag(reader);
          break;
        case "unitsLabel":
          reader.Read();
          break;
        default:
          reader.Skip();
          break;
      }
    }
    else
      reader.Skip();
  }

  internal static void ParseDefaultTextFormatting(
    XmlReader reader,
    IInternalChartTextArea textFormatting,
    Excel2007Parser parser,
    double? defaultFontSize)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textFormatting == null)
      throw new ArgumentNullException(nameof (textFormatting));
    if (reader.LocalName != "txPr")
      throw new XmlException("Unexpected xml tag");
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement && reader.LocalName != "txPr" && reader.LocalName != "defRPr")
    {
      switch (reader.LocalName)
      {
        case "bodyPr":
          if (reader.MoveToAttribute("rot"))
            textFormatting.TextRotationAngle = XmlConvertExtension.ToInt32(reader.Value) / 60000;
          reader.Skip();
          continue;
        default:
          reader.Read();
          continue;
      }
    }
    if (defaultFontSize.HasValue)
      textFormatting.Size = defaultFontSize.Value;
    if (reader.LocalName == "defRPr")
    {
      ChartParserCommon.ParseParagraphRunProperites(reader, textFormatting, parser, (TextSettings) null);
      while (reader.LocalName != "txPr")
      {
        if (reader.LocalName == "r")
        {
          List<ChartAlrunsRecord.TRuns> tRuns = (List<ChartAlrunsRecord.TRuns>) null;
          ChartParserCommon.ParseParagraphRun(reader, textFormatting, parser, (TextSettings) null, tRuns);
        }
        reader.Read();
      }
    }
    reader.Read();
  }

  public static string ParseValueTag(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    string valueTag = !reader.MoveToAttribute("val") ? ChartParserCommon.AssignValTag(reader.LocalName) : reader.Value;
    reader.Read();
    return valueTag;
  }

  private static string AssignValTag(string localName)
  {
    switch (localName)
    {
      case "baseTimeUnit":
      case "majorTimeUnit":
      case "minorTimeUnit":
        return "days";
      case "depthPercent":
      case "hPercent":
        return "100";
      case "hMode":
      case "xMode":
      case "manualLayout":
      case "yMode":
      case "wMode":
        return "factor";
      case "idx":
      case "overlap":
      case "rotX":
      case "rotY":
      case "thickness":
        return "0";
      case "orientation":
        return "minMax";
      case "errDir":
        return (string) null;
      case "barDir":
        return "col";
      case "grouping":
        return "clustered";
      case "shape":
        return "box";
      case "sizeRepresents":
        return "area";
      case "crossBetween":
        return "between";
      case "crosses":
        return "autoZero";
      case "errBarType":
        return "both";
      case "errValType":
        return "fixedVal";
      case "gapWidth":
        return "150";
      case "lblAlgn":
        return "ctr";
      case "layoutTarget":
        return "outer";
      case "legendPos":
        return "r";
      case "symbol":
        return "none";
      case "ofPieType":
        return "pie";
      case "perspective":
        return "30";
      case "radarStyle":
        return "standard";
      case "rAngAx":
        return "true";
      case "scatterStyle":
        return "marker";
      case "order":
        return "2";
      case "splitType":
        return "auto";
      case "tickLblPos":
        return "nextTo";
      case "trendlineType":
        return "linear";
      case "prstDash":
        return (string) null;
      default:
        throw new XmlException();
    }
  }

  public static bool ParseBoolValueTag(XmlReader reader)
  {
    return reader != null ? XmlConvertExtension.ToBoolean(ChartParserCommon.ParseValueTag(reader)) : throw new ArgumentNullException(nameof (reader));
  }

  public static int ParseIntValueTag(XmlReader reader)
  {
    return reader != null ? XmlConvertExtension.ToInt32(ChartParserCommon.ParseValueTag(reader)) : throw new ArgumentNullException(nameof (reader));
  }

  public static double ParseDoubleValueTag(XmlReader reader)
  {
    return reader != null ? XmlConvertExtension.ToDouble(ChartParserCommon.ParseValueTag(reader)) : throw new ArgumentNullException(nameof (reader));
  }

  public static void ParseLineProperties(
    XmlReader reader,
    ChartBorderImpl border,
    Excel2007Parser parser)
  {
    ChartParserCommon.ParseLineProperties(reader, border, false, parser);
  }

  public static void ParsePatternFill(XmlReader reader, IFill fill, Excel2007Parser parser)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "pattFill")
      throw new XmlException("Unexpected xml tag.");
    string str = reader.MoveToAttribute("prst") ? reader.Value : (string) null;
    ExcelGradientPattern excelGradientPattern = str != null ? (ExcelGradientPattern) Enum.Parse(typeof (Excel2007GradientPattern), str, false) : ExcelGradientPattern.Pat_Dashed_Downward_Diagonal;
    fill.Pattern = excelGradientPattern;
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      int transparecy;
      int tint;
      int shade;
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "fgClr":
              reader.Read();
              Color color1 = ChartParserCommon.ReadColor(reader, out transparecy, out tint, out shade, parser);
              fill.ForeColor = color1;
              reader.Read();
              continue;
            case "bgClr":
              reader.Read();
              Color color2 = ChartParserCommon.ReadColor(reader, out transparecy, out tint, out shade, parser);
              fill.BackColor = color2;
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

  internal static void ParseSolidFill(
    IInternalFill fill,
    XmlReader reader,
    Excel2007Parser parser,
    ColorObject color)
  {
    int Alpha = 100000;
    ChartParserCommon.ParseSolidFill(reader, parser, color, out Alpha);
    fill.Transparency = 1.0 - (double) Alpha / 100000.0;
    fill.ForeColor = color.GetRGB((IWorkbook) ChartParserCommon.m_book);
  }

  public static void ParseSolidFill(
    XmlReader reader,
    ChartInteriorImpl interior,
    Excel2007Parser parser,
    out int Alpha)
  {
    if (interior == null)
      throw new ArgumentNullException(nameof (interior));
    ChartParserCommon.ParseSolidFill(reader, parser, interior.ForegroundColorObject, out Alpha);
    interior.UseAutomaticFormat = false;
  }

  public static void ParseSolidFill(
    XmlReader reader,
    ChartInteriorImpl interior,
    Excel2007Parser parser)
  {
    if (interior == null)
      throw new ArgumentNullException(nameof (interior));
    ChartParserCommon.ParseSolidFill(reader, parser, interior.ForegroundColorObject, out int _);
  }

  public static void ParseSolidFill(XmlReader reader, Excel2007Parser parser, ColorObject color)
  {
    int Alpha = 100000;
    ChartParserCommon.ParseSolidFill(reader, parser, color, out Alpha);
  }

  internal static void ParseSolidFill(
    XmlReader reader,
    Excel2007Parser parser,
    ColorObject color,
    ShapeLineFormatImpl shape)
  {
    ChartParserCommon.ParseSolidFill(reader, parser, shape, color);
  }

  public static void ParseSolidFill(XmlReader reader, Excel2007Parser parser, IInternalFill fill)
  {
    int Alpha = 100000;
    ChartParserCommon.ParseSolidFill(reader, parser, fill.ForeColorObject, out Alpha);
    fill.Transparency = 1.0 - (double) Alpha / 100000.0;
  }

  public static void ParseSolidFill(
    XmlReader reader,
    Excel2007Parser parser,
    ColorObject color,
    out int Alpha)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "solidFill")
      throw new XmlException("Unexpected xml tag.");
    Alpha = 100000;
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      int tint;
      int shade;
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "srgbClr":
              color.SetRGB(ChartParserCommon.ParseSRgbColor(reader, out Alpha, out tint, out shade, parser));
              continue;
            case "schemeClr":
              color.SetRGB(ChartParserCommon.ParseSchemeColor(reader, out Alpha, parser));
              continue;
            case "sysClr":
              color.SetRGB(ChartParserCommon.ReadColor(reader, out Alpha, out tint, out shade, parser));
              continue;
            case "prstClr":
              color.SetRGB(ChartParserCommon.ParsePresetColor(reader, out Alpha, parser));
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

  internal static void ParseColorObject(
    XmlReader reader,
    Excel2007Parser parser,
    ColorObject color,
    out int Alpha)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    Alpha = 100000;
    int tint;
    int shade;
    switch (reader.LocalName)
    {
      case "srgbClr":
        color.SetRGB(ChartParserCommon.ParseSRgbColor(reader, out Alpha, out tint, out shade, parser, true));
        break;
      case "schemeClr":
        color.SetRGB(ChartParserCommon.ParseSchemeColor(reader, out Alpha, parser, true));
        break;
      case "sysClr":
        color.SetRGB(ChartParserCommon.ReadColor(reader, out Alpha, out tint, out shade, parser, true));
        break;
      case "prstClr":
        color.SetRGB(ChartParserCommon.ParsePresetColor(reader, out Alpha, parser, true));
        break;
      default:
        reader.Skip();
        break;
    }
  }

  internal static void ParseSolidFill(
    XmlReader reader,
    Excel2007Parser parser,
    ShapeLineFormatImpl shape,
    ColorObject color)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "solidFill")
      throw new XmlException("Unexpected xml tag.");
    int num = 100000;
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      int tint;
      int shade;
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "srgbClr":
              color.SetRGB(ChartParserCommon.ParseSRgbColor(reader, out num, out tint, out shade, parser));
              continue;
            case "schemeClr":
              color.SetRGB(ChartParserCommon.ParseSchemeColor(reader, parser, shape));
              continue;
            case "sysClr":
              color.SetRGB(ChartParserCommon.ReadColor(reader, out num, out tint, out shade, parser));
              continue;
            case "prstClr":
              color.SetRGB(ChartParserCommon.ParsePresetColor(reader, out num, parser));
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

  internal static Color ParseSchemeColor(
    XmlReader reader,
    Excel2007Parser parser,
    ShapeLineFormatImpl shape)
  {
    if (reader == null)
      throw new ArgumentNullException();
    bool flag = !(reader.LocalName != "schemeClr") ? reader.IsEmptyElement : throw new XmlException("Unexpected xml tag");
    string colorName = (string) null;
    if (reader.MoveToAttribute("val"))
      colorName = reader.Value;
    Color themeColor = parser.GetThemeColor(colorName);
    reader.MoveToElement();
    if (!flag)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          if (shape != null)
          {
            while (reader.NodeType != XmlNodeType.EndElement)
            {
              if (reader.NodeType == XmlNodeType.Whitespace)
                reader.Read();
              if (reader.NodeType == XmlNodeType.Element)
              {
                switch (reader.LocalName)
                {
                  case "alpha":
                    shape.SchemeColorPreservedElements.Add("alpha", ShapeParser.ReadNodeAsStream(reader));
                    continue;
                  case "hueMod":
                    shape.SchemeColorPreservedElements.Add("hueMod", ShapeParser.ReadNodeAsStream(reader));
                    continue;
                  case "lum":
                    shape.SchemeColorPreservedElements.Add("lum", ShapeParser.ReadNodeAsStream(reader));
                    continue;
                  case "lumMod":
                    shape.SchemeColorPreservedElements.Add("lumMod", ShapeParser.ReadNodeAsStream(reader));
                    continue;
                  case "lumOff":
                    shape.SchemeColorPreservedElements.Add("lumOff", ShapeParser.ReadNodeAsStream(reader));
                    continue;
                  case "sat":
                    shape.SchemeColorPreservedElements.Add("sat", ShapeParser.ReadNodeAsStream(reader));
                    continue;
                  case "satMod":
                    shape.SchemeColorPreservedElements.Add("satMod", ShapeParser.ReadNodeAsStream(reader));
                    continue;
                  case "satOff":
                    shape.SchemeColorPreservedElements.Add("satOff", ShapeParser.ReadNodeAsStream(reader));
                    continue;
                  case "shade":
                    shape.SchemeColorPreservedElements.Add("shade", ShapeParser.ReadNodeAsStream(reader));
                    continue;
                  case "tint":
                    shape.SchemeColorPreservedElements.Add("tint", ShapeParser.ReadNodeAsStream(reader));
                    continue;
                  default:
                    continue;
                }
              }
            }
            break;
          }
          break;
        }
        reader.Skip();
      }
    }
    reader.Read();
    return themeColor;
  }

  public static Color ParseSRgbColor(XmlReader reader, Excel2007Parser parser)
  {
    return ChartParserCommon.ParseSRgbColor(reader, out int _, out int _, out int _, parser);
  }

  public static Color ParseSRgbColor(
    XmlReader reader,
    out int alpha,
    out int tint,
    out int shade,
    Excel2007Parser parser)
  {
    return ChartParserCommon.ParseSRgbColor(reader, out alpha, out tint, out shade, parser, false);
  }

  private static Color ParseSRgbColor(
    XmlReader reader,
    out int alpha,
    out int tint,
    out int shade,
    Excel2007Parser parser,
    bool isBlipImage)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    bool flag = !(reader.LocalName != "srgbClr") ? reader.IsEmptyElement : throw new XmlException("Unexpeced xml tag.");
    Color empty = ColorExtension.Empty;
    alpha = 100000;
    tint = -1;
    shade = -1;
    Color result = reader.MoveToAttribute("val") ? ColorExtension.FromArgb(int.Parse(reader.Value, NumberStyles.HexNumber, (IFormatProvider) null)) : throw new XmlException();
    reader.MoveToElement();
    reader.Read();
    if (!flag)
    {
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case nameof (alpha):
              alpha = ChartParserCommon.ParseIntValueTag(reader);
              continue;
            case "gamma":
              reader.Skip();
              continue;
            case "invGamma":
              reader.Skip();
              continue;
            case nameof (tint):
              if (isBlipImage)
              {
                result = ChartParserCommon.ParseColorUpdater(reader, result, parser, out alpha, isBlipImage);
                continue;
              }
              tint = ChartParserCommon.ParseIntValueTag(reader);
              continue;
            case nameof (shade):
              if (isBlipImage)
              {
                result = ChartParserCommon.ParseColorUpdater(reader, result, parser, out alpha, isBlipImage);
                continue;
              }
              shade = ChartParserCommon.ParseIntValueTag(reader);
              continue;
            default:
              result = ChartParserCommon.ParseColorUpdater(reader, result, parser, out alpha, isBlipImage);
              continue;
          }
        }
        else
          reader.Skip();
      }
      reader.Read();
    }
    return result;
  }

  public static Color ParseSchemeColor(XmlReader reader, Excel2007Parser parser)
  {
    return ChartParserCommon.ParseSchemeColor(reader, out int _, parser);
  }

  public static Color ParseSchemeColor(XmlReader reader, out int alpha, Excel2007Parser parser)
  {
    return ChartParserCommon.ParseSchemeColor(reader, out alpha, parser, false);
  }

  private static Color ParseSchemeColor(
    XmlReader reader,
    out int alpha,
    Excel2007Parser parser,
    bool isBlipImage)
  {
    if (reader == null)
      throw new ArgumentNullException();
    bool flag = !(reader.LocalName != "schemeClr") ? reader.IsEmptyElement : throw new XmlException("Unexpected xml tag");
    alpha = 100000;
    string str = (string) null;
    if (reader.MoveToAttribute("val"))
      str = reader.Value;
    Color result = ChartParserCommon.m_book == null || !ChartParserCommon.m_book.Loading || ChartParserCommon.m_book.Version != ExcelVersion.Excel97to2003 ? (parser.CurrentChartThemeColors == null || !parser.CurrentChartThemeColors.ContainsKey(str) ? parser.GetThemeColor(str) : parser.CurrentChartThemeColors[str]) : ChartParserCommon.GetStandardThemeColors(str);
    reader.MoveToElement();
    if (!flag)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
          result = ChartParserCommon.ParseColorUpdater(reader, result, parser, out alpha, isBlipImage);
        else
          reader.Skip();
      }
    }
    reader.Read();
    return result;
  }

  private static Color GetStandardThemeColors(string strColorName)
  {
    Color standardThemeColors = Color.Empty;
    switch (strColorName.ToLower())
    {
      case "dk1":
        standardThemeColors = WorkbookImpl.DefaultThemeColors[1];
        break;
      case "lt1":
      case "bg1":
        standardThemeColors = WorkbookImpl.DefaultThemeColors[0];
        break;
      case "dk2":
        standardThemeColors = WorkbookImpl.DefaultThemeColors[3];
        break;
      case "lt2":
      case "bg2":
        standardThemeColors = WorkbookImpl.DefaultThemeColors[2];
        break;
      case "accent1":
        standardThemeColors = WorkbookImpl.DefaultThemeColors[4];
        break;
      case "accent2":
        standardThemeColors = WorkbookImpl.DefaultThemeColors[5];
        break;
      case "accent3":
        standardThemeColors = WorkbookImpl.DefaultThemeColors[6];
        break;
      case "accent4":
        standardThemeColors = WorkbookImpl.DefaultThemeColors[7];
        break;
      case "accent5":
        standardThemeColors = WorkbookImpl.DefaultThemeColors[8];
        break;
      case "accent6":
        standardThemeColors = WorkbookImpl.DefaultThemeColors[9];
        break;
      case "hlink":
        standardThemeColors = WorkbookImpl.DefaultThemeColors[10];
        break;
      case "folHlink":
        standardThemeColors = WorkbookImpl.DefaultThemeColors[11];
        break;
    }
    return standardThemeColors;
  }

  public static Color ParsePresetColor(XmlReader reader, out int alpha, Excel2007Parser parser)
  {
    return ChartParserCommon.ParsePresetColor(reader, out alpha, parser, false);
  }

  private static Color ParsePresetColor(
    XmlReader reader,
    out int alpha,
    Excel2007Parser parser,
    bool isBlipImage)
  {
    if (reader == null)
      throw new ArgumentNullException();
    bool flag = !(reader.LocalName != "prstClr") ? reader.IsEmptyElement : throw new XmlException("Unexpected xml tag");
    alpha = 100000;
    string name = (string) null;
    if (reader.MoveToAttribute("val"))
      name = reader.Value;
    Color result = Color.FromName(name);
    reader.MoveToElement();
    if (!flag)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
          result = ChartParserCommon.ParseColorUpdater(reader, result, parser, out alpha, isBlipImage);
        else
          reader.Skip();
      }
    }
    reader.Read();
    return result;
  }

  public static Color ParseSystemColor(XmlReader reader, out int alpha, Excel2007Parser parser)
  {
    return ChartParserCommon.ParseSystemColor(reader, out alpha, parser, false);
  }

  private static Color ParseSystemColor(
    XmlReader reader,
    out int alpha,
    Excel2007Parser parser,
    bool isBlipImage)
  {
    if (reader == null)
      throw new ArgumentNullException();
    if (reader.LocalName != "sysClr")
      throw new XmlException("Unexpected xml tag");
    alpha = 100000;
    Color result = !reader.MoveToAttribute("lastClr") ? ColorExtension.Empty : ColorExtension.FromArgb(int.Parse(reader.Value, NumberStyles.HexNumber, (IFormatProvider) null));
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
          result = ChartParserCommon.ParseColorUpdater(reader, result, parser, out alpha, isBlipImage);
        else
          reader.Skip();
      }
    }
    reader.Read();
    return result;
  }

  private static Color ParseColorUpdater(
    XmlReader reader,
    Color result,
    Excel2007Parser parser,
    out int alpha)
  {
    return ChartParserCommon.ParseColorUpdater(reader, result, parser, out alpha, false);
  }

  private static Color ParseColorUpdater(
    XmlReader reader,
    Color result,
    Excel2007Parser parser,
    out int alpha,
    bool isBlipImage)
  {
    alpha = 100000;
    switch (reader.LocalName)
    {
      case "lumMod":
        int intPercentageValue1 = reader.MoveToAttribute("val") ? ChartParserCommon.ParseIntPercentageValue(reader, 1000) : 0;
        double dHue1;
        double dLuminance1;
        double dSaturation1;
        if (isBlipImage)
          Excel2007Parser.ConvertRGBtoHLSBlip(result, out dHue1, out dLuminance1, out dSaturation1);
        else
          Excel2007Parser.ConvertRGBtoHLS(result, out dHue1, out dLuminance1, out dSaturation1);
        dLuminance1 *= (double) intPercentageValue1 / 100000.0;
        if (isBlipImage)
        {
          dLuminance1 = ChartParserCommon.RoundOffLuminance(dLuminance1);
          result = Excel2007Parser.ConvertHLSToRGBBlip(dHue1, dLuminance1, dSaturation1);
          break;
        }
        result = Excel2007Parser.ConvertHLSToRGB(dHue1, dLuminance1, dSaturation1);
        break;
      case "lumOff":
        int intPercentageValue2 = reader.MoveToAttribute("val") ? ChartParserCommon.ParseIntPercentageValue(reader, 1000) : 0;
        double dHue2;
        double dLuminance2;
        double dSaturation2;
        if (isBlipImage)
          Excel2007Parser.ConvertRGBtoHLSBlip(result, out dHue2, out dLuminance2, out dSaturation2);
        else
          Excel2007Parser.ConvertRGBtoHLS(result, out dHue2, out dLuminance2, out dSaturation2);
        dLuminance2 += (double) ((int) byte.MaxValue * intPercentageValue2) / 100000.0;
        if (isBlipImage)
        {
          dLuminance2 = ChartParserCommon.RoundOffLuminance(dLuminance2);
          result = Excel2007Parser.ConvertHLSToRGBBlip(dHue2, dLuminance2, dSaturation2);
          break;
        }
        result = Excel2007Parser.ConvertHLSToRGB(dHue2, dLuminance2, dSaturation2);
        break;
      case "satMod":
        int intPercentageValue3 = reader.MoveToAttribute("val") ? ChartParserCommon.ParseIntPercentageValue(reader, 1000) : 0;
        double dHue3;
        double dLuminance3;
        double dSaturation3;
        if (isBlipImage)
          Excel2007Parser.ConvertRGBtoHLSBlip(result, out dHue3, out dLuminance3, out dSaturation3);
        else
          Excel2007Parser.ConvertRGBtoHLS(result, out dHue3, out dLuminance3, out dSaturation3);
        dSaturation3 *= (double) intPercentageValue3 / 100000.0;
        if (isBlipImage)
        {
          dSaturation3 = ChartParserCommon.RoundOffSaturation(dSaturation3);
          result = Excel2007Parser.ConvertHLSToRGBBlip(dHue3, dLuminance3, dSaturation3);
          break;
        }
        result = Excel2007Parser.ConvertHLSToRGB(dHue3, dLuminance3, dSaturation3);
        break;
      case "tint":
        double doubleValueTag = ChartParserCommon.ParseDoubleValueTag(reader);
        if (doubleValueTag > 100.0)
          doubleValueTag /= 100000.0;
        result = Excel2007Parser.ConvertColorByTintBlip(result, doubleValueTag);
        break;
      case "shade":
        double shade = (double) ChartParserCommon.ParseIntValueTag(reader) / 100000.0;
        result = parser.ConvertColorByShadeBlip(result, shade);
        break;
      case nameof (alpha):
        alpha = ChartParserCommon.ParseIntValueTag(reader);
        break;
      default:
        reader.Skip();
        break;
    }
    return result;
  }

  internal static double RoundOffLuminance(double dLuminance)
  {
    if (dLuminance > 1.0)
      return 1.0;
    return dLuminance >= 0.0 ? dLuminance : 0.0;
  }

  internal static double RoundOffSaturation(double dSaturation)
  {
    if (dSaturation > 1.0)
      return 1.0;
    return dSaturation >= 0.0 ? dSaturation : 0.0;
  }

  private static void ParseLineProperties(
    XmlReader reader,
    ChartBorderImpl border,
    bool bRoundCorners,
    Excel2007Parser parser)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (border == null)
      throw new ArgumentNullException(nameof (border));
    bool flag1 = !(reader.LocalName != "ln") ? reader.IsEmptyElement : throw new XmlException("Unexpected xml tag");
    bool flag2 = border.IsAutoLineColor;
    if (reader.MoveToAttribute("w"))
    {
      int num = (int) ((double) int.Parse(reader.Value) / 12700.0) - 1;
      border.LineWeight = (ExcelChartLineWeight) num;
      border.LineWeightString = reader.Value;
      if (flag2 && !border.IsAutoLineColor)
      {
        border.IsAutoLineColor = flag2;
        flag2 = false;
      }
    }
    else
      border.LineWeight = ExcelChartLineWeight.Hairline;
    int Alpha = 100000;
    bool flag3 = false;
    string key1 = (string) null;
    string str = (string) null;
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
              border.LinePattern = ExcelChartLinePattern.None;
              border.HasLineProperties = true;
              reader.Read();
              continue;
            case "round":
              border.JoinType = Excel2007BorderJoinType.Round;
              reader.Read();
              continue;
            case "miter":
              border.JoinType = Excel2007BorderJoinType.Mitter;
              reader.Read();
              continue;
            case "bevel":
              border.JoinType = Excel2007BorderJoinType.Bevel;
              reader.Read();
              continue;
            case "solidFill":
              border.Color.AfterChange += new ColorObject.AfterChangeHandler(border.ClearAutoColor);
              bool isEmptyElement = reader.IsEmptyElement;
              ChartParserCommon.ParseSolidFill(reader, parser, border.Color, out Alpha);
              border.Transparency = 1.0 - (double) Alpha / 100000.0;
              border.Color.AfterChange -= new ColorObject.AfterChangeHandler(border.ClearAutoColor);
              border.AutoFormat = false;
              if (!flag2)
                border.TryAndClearAutoColor();
              if (border.IsAutoLineColor && !isEmptyElement && border.FindParent(typeof (ChartDataPointImpl)) is ChartDataPointImpl parent && !parent.IsDefault)
                border.IsAutoLineColor = false;
              flag3 = true;
              continue;
            case "prstDash":
              key1 = ChartParserCommon.ParseValueTag(reader);
              continue;
            case "pattFill":
              str = reader.MoveToAttribute("prst") ? reader.Value : (string) null;
              continue;
            case "gradFill":
              border.AutoFormat = false;
              GradientStops gradientFill = ChartParserCommon.ParseGradientFill(reader, parser);
              ChartParserCommon.ConvertGradientStopsToProperties(gradientFill, border.Fill);
              border.Fill.PreservedGradient = gradientFill;
              border.HasLineProperties = true;
              if (!flag2)
              {
                border.TryAndClearAutoColor();
                continue;
              }
              continue;
            case "headEnd":
              reader.Skip();
              continue;
            case "tailEnd":
              reader.Skip();
              continue;
            default:
              throw new NotImplementedException();
          }
        }
        else
          reader.Skip();
      }
    }
    else if (border.FindParent(typeof (ChartLegendImpl)) is ChartLegendImpl)
      border.LinePattern = ~ExcelChartLinePattern.Solid;
    else
      flag3 = true;
    if (flag3)
    {
      border.LinePattern = ExcelChartLinePattern.Solid;
      if (!flag1)
        border.HasLineProperties = true;
      if (key1 != null)
      {
        KeyValuePair<string, string> key2 = new KeyValuePair<string, string>(key1, string.Empty);
        ExcelChartLinePattern chartLinePattern;
        if (ChartParserCommon.s_dicLinePatterns.TryGetValue(key2, out chartLinePattern))
          border.LinePattern = chartLinePattern;
      }
    }
    else if (str != null)
    {
      KeyValuePair<string, string> key3 = new KeyValuePair<string, string>("solid", str);
      ExcelChartLinePattern chartLinePattern;
      if (ChartParserCommon.s_dicLinePatterns.TryGetValue(key3, out chartLinePattern))
        border.LinePattern = chartLinePattern;
    }
    reader.Read();
  }

  public static void ParsePictureFill(
    XmlReader reader,
    IFill fill,
    RelationCollection relations,
    FileDataHolder holder,
    WorkbookImpl workbookImpl)
  {
    ChartParserCommon.ParsePictureFill(reader, fill, relations, holder, workbookImpl, (List<string>) null);
  }

  internal static void ParsePictureFill(
    XmlReader reader,
    IFill fill,
    RelationCollection relations,
    FileDataHolder holder,
    WorkbookImpl workbookImpl,
    List<string> lstRelationIds)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (fill == null)
      throw new ArgumentNullException(nameof (fill));
    if (reader.LocalName != "blipFill")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
label_39:
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "blip":
            if (reader.MoveToAttribute("embed", workbookImpl.IsStrict ? "http://purl.oclc.org/ooxml/officeDocument/relationships" : "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
            {
              string id = reader.Value;
              Relation relation = relations[id];
              lstRelationIds?.Add(id);
              string itemPath = relations.ItemPath;
              int length1 = itemPath.LastIndexOf('/');
              string str = itemPath.Substring(0, length1);
              int length2 = str.LastIndexOf('/');
              string strFullPath = FileDataHolder.CombinePath(str.Substring(0, length2), relation.Target);
              if (relation.Type == "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image")
              {
                Image image = holder.GetImage(strFullPath);
                if (ShapeFillImpl.m_isTexture)
                  fill.UserTexture(image, "image");
                else
                  fill.UserPicture(image, "image");
              }
            }
            reader.Read();
            while (true)
            {
              if (reader.LocalName != "srcRect" && reader.LocalName != "tile" && reader.LocalName != "stretch")
              {
                if (reader.LocalName == "alphaModFix")
                {
                  int num = reader.MoveToAttribute("amt") ? int.Parse(reader.Value) : 0;
                  (fill as IInternalFill).TransparencyColor = (float) (1.0 - (double) num / 100000.0);
                  reader.Read();
                }
                reader.Read();
              }
              else
                goto label_39;
            }
          case "tile":
            if (reader.NodeType != XmlNodeType.EndElement)
            {
              if (reader.MoveToAttribute("tx"))
                (fill as IInternalFill).TextureOffsetX = float.Parse(reader.Value) / 12700f;
              if (reader.MoveToAttribute("ty"))
                (fill as IInternalFill).TextureOffsetY = float.Parse(reader.Value) / 12700f;
              if (reader.MoveToAttribute("sx"))
                (fill as IInternalFill).TextureHorizontalScale = float.Parse(reader.Value) / 100000f;
              if (reader.MoveToAttribute("sy"))
                (fill as IInternalFill).TextureVerticalScale = float.Parse(reader.Value) / 100000f;
              if (reader.MoveToAttribute("flip"))
                (fill as IInternalFill).TileFlipping = reader.Value;
              if (reader.MoveToAttribute("algn"))
              {
                (fill as IInternalFill).Tile = true;
                (fill as IInternalFill).Alignment = reader.Value;
              }
            }
            reader.Read();
            continue;
          case "stretch":
            reader.Read();
            int num1 = reader.LocalName == "fillRect" ? 1 : 0;
            int intPercentageValue1 = reader.MoveToAttribute("l") ? ChartParserCommon.ParseIntPercentageValue(reader, 1) : 0;
            int intPercentageValue2 = reader.MoveToAttribute("t") ? ChartParserCommon.ParseIntPercentageValue(reader, 1) : 0;
            int intPercentageValue3 = reader.MoveToAttribute("r") ? ChartParserCommon.ParseIntPercentageValue(reader, 1) : 0;
            int intPercentageValue4 = reader.MoveToAttribute("b") ? ChartParserCommon.ParseIntPercentageValue(reader, 1) : 0;
            (fill as ShapeFillImpl).FillRect = Rectangle.FromLTRB(intPercentageValue1, intPercentageValue2, intPercentageValue3, intPercentageValue4);
            reader.Read();
            reader.Read();
            continue;
          case "srcRect":
            int left = reader.MoveToAttribute("l") ? int.Parse(reader.Value) : 0;
            int top = reader.MoveToAttribute("t") ? int.Parse(reader.Value) : 0;
            int right = reader.MoveToAttribute("r") ? int.Parse(reader.Value) : 0;
            int bottom = reader.MoveToAttribute("b") ? int.Parse(reader.Value) : 0;
            (fill as ShapeFillImpl).SourceRect = Rectangle.FromLTRB(left, top, right, bottom);
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

  internal static void ParseTextAreaText(
    XmlReader reader,
    IInternalChartTextArea textArea,
    Excel2007Parser parser,
    float? defaultFontSize)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textArea == null)
      throw new ArgumentNullException(nameof (textArea));
    if (reader.LocalName != "tx")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "rich":
            List<ChartAlrunsRecord.TRuns> tRuns = (List<ChartAlrunsRecord.TRuns>) null;
            switch (textArea)
            {
              case ChartTextAreaImpl _ when (textArea as ChartTextAreaImpl).ChartAlRuns != null:
                tRuns = new List<ChartAlrunsRecord.TRuns>((IEnumerable<ChartAlrunsRecord.TRuns>) (textArea as ChartTextAreaImpl).ChartAlRuns.Runs);
                break;
              case ChartDataLabelsImpl _ when (textArea as ChartDataLabelsImpl).TextArea.ChartAlRuns != null:
                tRuns = new List<ChartAlrunsRecord.TRuns>((IEnumerable<ChartAlrunsRecord.TRuns>) (textArea as ChartDataLabelsImpl).TextArea.ChartAlRuns.Runs);
                break;
            }
            ChartParserCommon.ParseRichText(reader, textArea, parser, defaultFontSize, tRuns, false);
            continue;
          case "strRef":
            ChartParserCommon.ParseStringReference(reader, textArea);
            continue;
          case "txData":
            string formula = (string) null;
            textArea.Text = ChartParserCommon.ParseFormulaOrValue(reader, out formula);
            if (formula != null && ChartParserCommon.m_book != null)
            {
              IRange range = ChartParser.GetRange(ChartParserCommon.m_book, formula);
              switch (range)
              {
                case null:
                case ExternalRange _:
                  break;
                default:
                  textArea.Text = range.Text;
                  break;
              }
            }
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

  internal static string ParseFormulaOrValue(XmlReader reader, out string formula)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "txData")
      throw new XmlException("Unexpected xml tag.");
    string formulaOrValue = (string) null;
    formula = (string) null;
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "v":
              formulaOrValue = reader.ReadElementContentAsString();
              continue;
            case "f":
              formula = reader.ReadElementContentAsString();
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
    else
      reader.Skip();
    return formulaOrValue;
  }

  public static void ParseChartLayout(XmlReader reader, IChartLayout layout)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (layout == null)
      throw new ArgumentNullException(nameof (layout));
    if (reader.LocalName != nameof (layout))
      throw new XmlException("Unexpected xml tag.");
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      IChartManualLayout manualLayout = (IChartManualLayout) null;
      if (reader.IsStartElement() && reader.LocalName == "manualLayout")
        manualLayout = layout.ManualLayout;
      if (manualLayout == null)
        return;
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "manualLayout":
              ChartParserCommon.ParseManualLayout(reader, manualLayout);
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
    else
      reader.Skip();
  }

  private static void ParseManualLayout(XmlReader reader, IChartManualLayout manualLayout)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (manualLayout == null)
      throw new ArgumentNullException(nameof (manualLayout));
    if (reader.LocalName != nameof (manualLayout))
      throw new XmlException("Unexpected xml tag.");
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "layoutTarget":
              manualLayout.LayoutTarget = (LayoutTargets) Enum.Parse(typeof (LayoutTargets), ChartParserCommon.ParseValueTag(reader), true);
              continue;
            case "xMode":
              manualLayout.LeftMode = (LayoutModes) Enum.Parse(typeof (LayoutModes), ChartParserCommon.ParseValueTag(reader), true);
              continue;
            case "yMode":
              manualLayout.TopMode = (LayoutModes) Enum.Parse(typeof (LayoutModes), ChartParserCommon.ParseValueTag(reader), true);
              continue;
            case "x":
              manualLayout.Left = ChartParserCommon.ParseDoubleValueTag(reader);
              continue;
            case "y":
              manualLayout.Top = ChartParserCommon.ParseDoubleValueTag(reader);
              continue;
            case "dX":
              reader.Skip();
              continue;
            case "dY":
              reader.Skip();
              continue;
            case "wMode":
              manualLayout.WidthMode = (LayoutModes) Enum.Parse(typeof (LayoutModes), ChartParserCommon.ParseValueTag(reader), true);
              continue;
            case "hMode":
              manualLayout.HeightMode = (LayoutModes) Enum.Parse(typeof (LayoutModes), ChartParserCommon.ParseValueTag(reader), true);
              continue;
            case "w":
              manualLayout.Width = ChartParserCommon.ParseDoubleValueTag(reader);
              continue;
            case "h":
              manualLayout.Height = ChartParserCommon.ParseDoubleValueTag(reader);
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
    else
      reader.Skip();
  }

  private static void ParseStringReference(XmlReader reader, IInternalChartTextArea textArea)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "strRef")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    bool flag = false;
    ChartTextAreaImpl chartTextAreaImpl = textArea as ChartTextAreaImpl;
    ChartDataLabelsImpl chartDataLabelsImpl = textArea as ChartDataLabelsImpl;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "f":
            if (chartTextAreaImpl != null)
              chartTextAreaImpl.IsFormula = true;
            else if (chartDataLabelsImpl != null)
              chartDataLabelsImpl.IsFormula = true;
            textArea.Text = reader.ReadElementContentAsString();
            IRange range = ChartParser.GetRange((textArea as CommonObject).FindParent(typeof (WorkbookImpl)) as WorkbookImpl, textArea.Text);
            flag = range == null || range is ExternalRange;
            continue;
          case "strCache":
            if (flag)
            {
              if (chartTextAreaImpl != null)
                chartTextAreaImpl.StringCache = ChartParserCommon.ParseDirectlyEnteredValues(reader);
              else if (chartDataLabelsImpl != null)
                chartDataLabelsImpl.StringCache = ChartParserCommon.ParseDirectlyEnteredValues(reader);
              if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "strCache")
              {
                reader.Read();
                continue;
              }
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
        reader.Skip();
    }
    reader.Read();
  }

  private static string[] ParseDirectlyEnteredValues(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    reader.Read();
    List<string> stringList = new List<string>();
    if (reader.NodeType == XmlNodeType.EndElement)
      return stringList.ToArray();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "pt" && !reader.IsEmptyElement)
      {
        reader.Read();
        while (reader.NodeType != XmlNodeType.EndElement)
        {
          if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "v")
            stringList.Add(reader.ReadElementContentAsString());
          else
            reader.Skip();
        }
      }
      else
        reader.Skip();
      if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "pt")
        reader.Read();
    }
    return stringList.ToArray();
  }

  internal static void ParseRichText(
    XmlReader reader,
    IInternalChartTextArea textArea,
    Excel2007Parser parser,
    float? defaultFontSize,
    List<ChartAlrunsRecord.TRuns> tRuns,
    bool isRichTextStreamRecord)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textArea == null)
      throw new ArgumentNullException(nameof (textArea));
    if (reader.LocalName != "rich" && !isRichTextStreamRecord)
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    bool flag = true;
    IInternalChartTextArea defaultTextArea = (IInternalChartTextArea) null;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "bodyPr":
            ChartParserCommon.ParseBodyProperties(reader, (IChartTextArea) textArea);
            continue;
          case "lstStyle":
            ChartParserCommon.ParseListStyles(reader, (IChartTextArea) textArea);
            continue;
          case "p":
            if (!flag)
              textArea.Text += (string) (object) '\n';
            else
              flag = false;
            if (tRuns != null && tRuns.Count > 0)
              tRuns[tRuns.Count - 1].HasNewParagarphStart = true;
            if (defaultTextArea == null && textArea is ChartTextAreaImpl)
              defaultTextArea = (IInternalChartTextArea) (textArea as ChartTextAreaImpl).Clone(textArea.Parent);
            tRuns = ChartParserCommon.ParseParagraphs(reader, textArea, parser, defaultFontSize, tRuns, defaultTextArea);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    if (defaultTextArea != null && (textArea as ChartTextAreaImpl).DefaultParagarphProperties.Count > 1)
      ChartParserCommon.CopyDefaultTextAreaSettings(textArea, (textArea as ChartTextAreaImpl).DefaultParagarphProperties[0]);
    reader.Read();
  }

  private static void ParseBodyProperties(XmlReader reader, IChartTextArea textArea)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textArea == null)
      throw new ArgumentNullException(nameof (textArea));
    if (reader.LocalName != "bodyPr")
      throw new XmlException("Unexpected xml tag.");
    if (reader.MoveToAttribute("rot"))
    {
      int int32 = XmlConvertExtension.ToInt32(reader.Value);
      textArea.TextRotationAngle = int32 / 60000;
      if (reader.MoveToAttribute("vert"))
      {
        switch (textArea)
        {
          case ChartTextAreaImpl _:
            (textArea as ChartTextAreaImpl).TextRotation = (Excel2007TextRotation) Enum.Parse(typeof (Excel2007TextRotation), reader.Value, false);
            break;
          case ChartDataLabelsImpl _:
            (textArea as ChartDataLabelsImpl).TextRotation = (Excel2007TextRotation) Enum.Parse(typeof (Excel2007TextRotation), reader.Value, false);
            break;
        }
      }
      reader.MoveToElement();
    }
    reader.Skip();
  }

  private static void ParseListStyles(XmlReader reader, IChartTextArea textArea)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textArea == null)
      throw new ArgumentNullException(nameof (textArea));
    if (reader.LocalName != "lstStyle")
      throw new XmlException("Unexpected xml tag.");
    reader.Skip();
  }

  private static List<ChartAlrunsRecord.TRuns> ParseParagraphs(
    XmlReader reader,
    IInternalChartTextArea textArea,
    Excel2007Parser parser,
    float? defaultFontSize,
    List<ChartAlrunsRecord.TRuns> tRuns,
    IInternalChartTextArea defaultTextArea)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textArea == null)
      throw new ArgumentNullException(nameof (textArea));
    if (reader.LocalName != "p")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    TextSettings defaultSettings = (TextSettings) null;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "pPr":
            defaultSettings = ChartParserCommon.ParseParagraphProperties(reader, textArea, parser, defaultFontSize);
            continue;
          case "r":
            if (textArea is ChartTextAreaImpl && !(textArea as ChartTextAreaImpl).isEmptyDefPara)
              ChartParserCommon.CopyDefaultTextAreaSettings(textArea, defaultTextArea);
            tRuns = ChartParserCommon.ParseParagraphRun(reader, textArea, parser, defaultSettings, tRuns);
            continue;
          case "fld":
            tRuns = ChartParserCommon.ParseFldElement(reader, textArea, parser, defaultSettings, tRuns, defaultFontSize);
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
    if (defaultTextArea != null)
    {
      IInternalChartTextArea textArea1 = (IInternalChartTextArea) (defaultTextArea as ChartTextAreaImpl).Clone(defaultTextArea.Parent);
      ChartParserCommon.CopyDefaultSettings((IInternalFont) textArea1, defaultSettings);
      (textArea1 as ChartTextAreaImpl).ChartAlRuns.Runs = (textArea as ChartTextAreaImpl).ChartAlRuns.Runs;
      (textArea as ChartTextAreaImpl).DefaultParagarphProperties.Add(textArea1);
    }
    return tRuns;
  }

  private static List<ChartAlrunsRecord.TRuns> ParseFldElement(
    XmlReader reader,
    IInternalChartTextArea textArea,
    Excel2007Parser parser,
    TextSettings defaultSettings,
    List<ChartAlrunsRecord.TRuns> tRuns,
    float? defaultFontSize)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textArea == null)
      throw new ArgumentNullException(nameof (textArea));
    if (reader.LocalName != "fld")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "rPr":
            ChartParserCommon.ParseParagraphRunProperites(reader, textArea, parser, defaultSettings);
            continue;
          case "pPr":
            defaultSettings = ChartParserCommon.ParseParagraphProperties(reader, textArea, parser, defaultFontSize);
            continue;
          case "t":
            ushort firstChar = 0;
            string str = reader.ReadElementContentAsString();
            if (textArea.Text == "Chart Title" && str != null)
            {
              textArea.Text = str;
            }
            else
            {
              if (textArea.Text != null)
                firstChar = (ushort) textArea.Text.Length;
              textArea.Text += str;
            }
            if (tRuns != null)
            {
              tRuns.Add(new ChartAlrunsRecord.TRuns(firstChar, (ushort) textArea.Font.Index));
              if (textArea is ChartTextAreaImpl && (textArea as ChartTextAreaImpl).ChartAlRuns != null)
                (textArea as ChartTextAreaImpl).ChartAlRuns.Runs = tRuns.ToArray();
              else if (textArea is ChartDataLabelsImpl && (textArea as ChartDataLabelsImpl).TextArea.ChartAlRuns != null)
                (textArea as ChartDataLabelsImpl).TextArea.ChartAlRuns.Runs = tRuns.ToArray();
            }
            if (textArea.Text.Contains("\r"))
            {
              textArea.Text = textArea.Text.Remove(textArea.Text.IndexOf('\r'), 1);
              continue;
            }
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
    }
    reader.Read();
    return tRuns;
  }

  private static TextSettings ParseParagraphProperties(
    XmlReader reader,
    IInternalChartTextArea textarea,
    Excel2007Parser parser,
    float? defaultFontSize)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    TextSettings paragraphProperties = (TextSettings) null;
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "defRPr":
              if (reader.IsEmptyElement && textarea is ChartTextAreaImpl)
                (textarea as ChartTextAreaImpl).isEmptyDefPara = true;
              paragraphProperties = ChartParserCommon.ParseDefaultParagraphProperties(reader, parser, defaultFontSize);
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
    return paragraphProperties;
  }

  internal static TextSettings ParseDefaultParagraphProperties(
    XmlReader reader,
    Excel2007Parser parser)
  {
    return ChartParserCommon.ParseDefaultParagraphProperties(reader, parser, new float?());
  }

  internal static TextSettings ParseDefaultParagraphProperties(
    XmlReader reader,
    Excel2007Parser parser,
    float? defaultFontSize)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    TextSettings result = new TextSettings();
    result.FontSize = defaultFontSize;
    if (reader.MoveToAttribute("b"))
    {
      result.Bold = new bool?(XmlConvertExtension.ToBoolean(reader.Value));
      result.ShowBoldProperties = new bool?(true);
    }
    if (reader.MoveToAttribute("i"))
      result.Italic = new bool?(XmlConvertExtension.ToBoolean(reader.Value));
    if (reader.MoveToAttribute("sz"))
    {
      result.FontSize = new float?(XmlConvertExtension.ToSingle(reader.Value) / 100f);
      result.ShowSizeProperties = new bool?(true);
    }
    if (reader.MoveToAttribute("u"))
      result.Underline = reader.Value;
    if (reader.MoveToAttribute("strike"))
      result.Striked = new bool?(reader.Value != "noStrike");
    if (reader.MoveToAttribute("lang"))
      result.Language = reader.Value;
    if (reader.MoveToAttribute("normalizeH") && reader.Value != "0")
      result.IsNormalizeHeights = true;
    if (reader.MoveToAttribute("cap"))
      result.CapitalizationType = !(reader.Value == "small") ? (!(reader.Value == "all") ? TextCapsType.None : TextCapsType.All) : TextCapsType.Small;
    if (reader.MoveToAttribute("baseline"))
    {
      int intPercentageValue = ChartParserCommon.ParseIntPercentageValue(reader, 1);
      result.Baseline = intPercentageValue;
    }
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
                result.ActualFontName = reader.Value;
                string str = ChartParserCommon.CheckValue(reader.Value.ToString());
                result.FontName = str;
                result.HasLatin = new bool?(true);
              }
              reader.MoveToElement();
              reader.Skip();
              continue;
            case "solidFill":
              ChartParserCommon.ParseDefaultFontColor(reader, result, parser);
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
    return result;
  }

  public static string CheckValue(string strValue)
  {
    FontImpl fontImpl = (FontImpl) null;
    if (!(strValue == "+mj-lt") && !(strValue == "+mj-cs") && !(strValue == "+mj-ea"))
      return strValue;
    string[] strArray = strValue.Split('-');
    if (strArray[0] == "+mj")
    {
      switch (strArray[1])
      {
        case "lt":
          ChartParserCommon.m_book.MajorFonts.TryGetValue("lt", out fontImpl);
          break;
        case "ea":
          ChartParserCommon.m_book.MajorFonts.TryGetValue("ea", out fontImpl);
          break;
        case "cs":
          ChartParserCommon.m_book.MajorFonts.TryGetValue("cs", out fontImpl);
          break;
      }
    }
    return fontImpl.FontName;
  }

  private static void ParseDefaultFontColor(
    XmlReader reader,
    TextSettings result,
    Excel2007Parser parser)
  {
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "srgbClr":
              result.FontColor = new Color?(ChartParserCommon.ParseSRgbColor(reader, parser));
              continue;
            case "schemeClr":
              result.FontColor = new Color?(ChartParserCommon.ParseSchemeColor(reader, parser));
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

  private static List<ChartAlrunsRecord.TRuns> ParseParagraphRun(
    XmlReader reader,
    IInternalChartTextArea textArea,
    Excel2007Parser parser,
    TextSettings defaultSettings,
    List<ChartAlrunsRecord.TRuns> tRuns)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textArea == null)
      throw new ArgumentNullException(nameof (textArea));
    if (reader.LocalName != "r")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    ChartParserCommon.CopyDefaultSettings((IInternalFont) textArea, defaultSettings);
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "rPr":
            if (reader.IsEmptyElement && textArea is ChartTextAreaImpl)
              (textArea as ChartTextAreaImpl).isEmptyTextPara = true;
            ChartParserCommon.ParseParagraphRunProperites(reader, textArea, parser, defaultSettings);
            continue;
          case "t":
            ushort firstChar = 0;
            if (textArea.Text != null)
              firstChar = (ushort) textArea.Text.Length;
            textArea.Text += reader.ReadElementContentAsString();
            if (textArea.Text.Contains("\r"))
              textArea.Text = textArea.Text.Remove(textArea.Text.IndexOf('\r'), 1);
            if (tRuns != null)
            {
              tRuns.Add(new ChartAlrunsRecord.TRuns(firstChar, (ushort) textArea.Font.Index));
              if (textArea is ChartTextAreaImpl && (textArea as ChartTextAreaImpl).ChartAlRuns != null)
              {
                (textArea as ChartTextAreaImpl).ChartAlRuns.Runs = tRuns.ToArray();
                continue;
              }
              if (textArea is ChartDataLabelsImpl && (textArea as ChartDataLabelsImpl).TextArea.ChartAlRuns != null)
              {
                (textArea as ChartDataLabelsImpl).TextArea.ChartAlRuns.Runs = tRuns.ToArray();
                continue;
              }
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
    reader.Read();
    return tRuns;
  }

  public static void ParseParagraphRunProperites(
    XmlReader reader,
    IInternalChartTextArea textArea,
    Excel2007Parser parser,
    TextSettings defaultSettings)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textArea == null)
      throw new ArgumentNullException(nameof (textArea));
    bool flag = false;
    ChartParserCommon.CopyDefaultSettings((IInternalFont) textArea, defaultSettings);
    if (reader.MoveToAttribute("lang") && ChartParserCommon.m_book.Loading && ChartParserCommon.m_book.Version != ExcelVersion.Excel97to2003)
    {
      textArea.Font.Language = reader.Value;
      FontImpl fontImpl = (FontImpl) null;
      if (ChartParserCommon.m_book.MinorFonts.TryGetValue("latin", out fontImpl))
        textArea.FontName = fontImpl.FontName;
    }
    if (defaultSettings != null && defaultSettings.FontName != textArea.FontName && defaultSettings.FontName != null)
      textArea.FontName = defaultSettings.FontName;
    if (reader.MoveToAttribute("b"))
    {
      textArea.Bold = XmlConvertExtension.ToBoolean(reader.Value);
      if (textArea is ChartDataLabelsImpl)
        (textArea as ChartDataLabelsImpl).ShowBoldProperties = true;
      else if (textArea is ChartTextAreaImpl)
        (textArea as ChartTextAreaImpl).ShowBoldProperties = true;
    }
    if (reader.MoveToAttribute("i"))
      textArea.Italic = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("strike"))
      textArea.Strikethrough = reader.Value != "noStrike";
    if (reader.MoveToAttribute("sz"))
    {
      textArea.Size = (double) int.Parse(reader.Value) / 100.0;
      if (textArea is ChartDataLabelsImpl)
        (textArea as ChartDataLabelsImpl).ShowSizeProperties = true;
      else if (textArea is ChartTextAreaImpl)
        (textArea as ChartTextAreaImpl).ShowSizeProperties = true;
    }
    if (reader.MoveToAttribute("u"))
    {
      if (reader.Value == "sng")
        textArea.Underline = ExcelUnderline.Single;
      else if (reader.Value == "dbl")
        textArea.Underline = ExcelUnderline.Double;
    }
    if (reader.MoveToAttribute("normalizeH") && reader.Value != "0")
      (textArea as ChartTextAreaImpl).IsNormalizeHeights = new bool?(true);
    if (reader.MoveToAttribute("cap"))
    {
      ChartTextAreaImpl chartTextAreaImpl = (ChartTextAreaImpl) null;
      switch (textArea)
      {
        case ChartDataLabelsImpl _:
          chartTextAreaImpl = (textArea as ChartDataLabelsImpl).TextArea;
          break;
        case ChartTextAreaImpl _:
          chartTextAreaImpl = textArea as ChartTextAreaImpl;
          break;
      }
      chartTextAreaImpl.IsCapsUsed = true;
      switch (reader.Value)
      {
        case "small":
          chartTextAreaImpl.CapitalizationType = TextCapsType.Small;
          break;
        case "all":
          chartTextAreaImpl.CapitalizationType = TextCapsType.All;
          break;
        case "none":
          chartTextAreaImpl.CapitalizationType = TextCapsType.None;
          break;
      }
    }
    if (reader.MoveToAttribute("baseline"))
    {
      int intPercentageValue = ChartParserCommon.ParseIntPercentageValue(reader, 1);
      textArea.Font.BaseLine = intPercentageValue;
      if (intPercentageValue > 0)
        textArea.Superscript = true;
      else if (intPercentageValue < 0)
      {
        textArea.Subscript = true;
      }
      else
      {
        textArea.Superscript = false;
        textArea.Subscript = false;
      }
    }
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
                textArea.Font.ActualFontName = reader.Value;
                textArea.FontName = ChartParserCommon.CheckValue(reader.Value);
                textArea.Font.ColorObject.CopyFrom(textArea.ColorObject, true);
                textArea.Font.HasLatin = true;
                reader.MoveToElement();
              }
              reader.Skip();
              continue;
            case "solidFill":
              ChartParserCommon.ParseSolidFill(reader, parser, textArea.ColorObject);
              flag = true;
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
    if (!flag && defaultSettings != null && defaultSettings.FontColor.HasValue)
    {
      Color? fontColor = defaultSettings.FontColor;
      Color rgbColor = textArea.RGBColor;
      if ((!fontColor.HasValue ? 1 : (fontColor.GetValueOrDefault() != rgbColor ? 1 : 0)) != 0)
      {
        textArea.RGBColor = Color.Empty;
        textArea.RGBColor = defaultSettings.FontColor.Value;
      }
    }
    reader.Skip();
  }

  private static void CopyDefaultTextAreaSettings(
    IInternalChartTextArea textArea,
    IInternalChartTextArea defaultTextArea)
  {
    if (defaultTextArea == null)
      return;
    if (textArea.Bold != defaultTextArea.Bold)
      textArea.Bold = defaultTextArea.Bold;
    if (textArea.Color != defaultTextArea.Color)
      textArea.Color = defaultTextArea.Color;
    if (textArea.RGBColor != defaultTextArea.RGBColor)
      textArea.RGBColor = defaultTextArea.RGBColor;
    if (textArea.BackgroundMode != defaultTextArea.BackgroundMode)
      textArea.BackgroundMode = defaultTextArea.BackgroundMode;
    if (textArea.FontName != defaultTextArea.FontName)
      textArea.FontName = defaultTextArea.FontName;
    if (textArea.Size != defaultTextArea.Size)
      textArea.Size = defaultTextArea.Size;
    if (textArea.Strikethrough != textArea.Strikethrough)
      textArea.Strikethrough = defaultTextArea.Strikethrough;
    if (textArea.Subscript != defaultTextArea.Subscript)
      textArea.Subscript = defaultTextArea.Subscript;
    if (textArea.Superscript != defaultTextArea.Superscript)
      textArea.Superscript = defaultTextArea.Superscript;
    if (textArea.TextRotationAngle != defaultTextArea.TextRotationAngle)
      textArea.TextRotationAngle = defaultTextArea.TextRotationAngle;
    if (textArea.Underline != defaultTextArea.Underline)
      textArea.Underline = defaultTextArea.Underline;
    if (textArea.VerticalAlignment != defaultTextArea.VerticalAlignment)
      textArea.VerticalAlignment = defaultTextArea.VerticalAlignment;
    if (textArea.Italic != defaultTextArea.Italic)
      textArea.Italic = defaultTextArea.Italic;
    if (textArea.Font == defaultTextArea.Font)
      return;
    textArea.Font.Clone(defaultTextArea.Font.Parent);
  }

  public static void CopyDefaultSettings(IInternalFont textArea, TextSettings defaultSettings)
  {
    if (defaultSettings == null)
      return;
    if (defaultSettings.Bold.HasValue)
      textArea.Bold = defaultSettings.Bold.Value;
    if (defaultSettings.Italic.HasValue)
      textArea.Italic = defaultSettings.Italic.Value;
    if (defaultSettings.FontSize.HasValue)
      textArea.Size = (double) defaultSettings.FontSize.Value;
    if (defaultSettings.Underline != null)
    {
      if (defaultSettings.Underline.Equals("sng"))
        textArea.Underline = ExcelUnderline.Single;
      else if (defaultSettings.Underline.Equals("dbl"))
        textArea.Underline = ExcelUnderline.Double;
      else
        textArea.Underline = ExcelUnderline.None;
    }
    if (defaultSettings.FontName != null)
      textArea.FontName = defaultSettings.FontName;
    if (defaultSettings.Striked.HasValue)
      textArea.Strikethrough = defaultSettings.Striked.Value;
    int baseline = defaultSettings.Baseline;
    if (textArea is FontWrapper)
      (textArea as FontWrapper).Baseline = defaultSettings.Baseline;
    if (defaultSettings.Baseline > 0)
      textArea.Superscript = true;
    else if (defaultSettings.Baseline < 0)
    {
      textArea.Subscript = true;
    }
    else
    {
      textArea.Subscript = false;
      textArea.Superscript = false;
    }
    if (defaultSettings.Language != null && defaultSettings.FontName == null)
    {
      textArea.Font.Language = defaultSettings.Language;
      FontImpl fontImpl = (FontImpl) null;
      if (ChartParserCommon.m_book.MinorFonts.TryGetValue("latin", out fontImpl))
        textArea.FontName = fontImpl.FontName;
    }
    if (defaultSettings.FontColor.HasValue)
      textArea.RGBColor = defaultSettings.FontColor.Value;
    textArea.Font.m_textSettings = defaultSettings;
    if (defaultSettings.HasLatin.HasValue)
      textArea.Font.HasLatin = defaultSettings.HasLatin.Value;
    if (defaultSettings.HasComplexScripts.HasValue)
      textArea.Font.HasComplexScripts = defaultSettings.HasComplexScripts.Value;
    if (defaultSettings.HasEastAsianFont.HasValue)
      textArea.Font.HasEastAsianFont = defaultSettings.HasEastAsianFont.Value;
    if (defaultSettings.ActualFontName != null)
      textArea.Font.ActualFontName = defaultSettings.ActualFontName.ToString();
    if (defaultSettings.ShowSizeProperties.HasValue)
    {
      if (textArea is ChartTextAreaImpl && defaultSettings.ShowSizeProperties.HasValue)
        (textArea as ChartTextAreaImpl).ShowSizeProperties = defaultSettings.ShowSizeProperties.Value;
      else if (textArea is ChartDataLabelsImpl && defaultSettings.ShowSizeProperties.HasValue)
        (textArea as ChartDataLabelsImpl).ShowSizeProperties = defaultSettings.ShowSizeProperties.Value;
    }
    if (!defaultSettings.ShowBoldProperties.HasValue || !(textArea is ChartTextAreaImpl) || !defaultSettings.ShowBoldProperties.HasValue)
      return;
    (textArea as ChartTextAreaImpl).ShowBoldProperties = defaultSettings.ShowBoldProperties.Value;
  }

  internal static void CopyChartTitleDefaultSettings(
    ChartTextAreaImpl textArea,
    TextSettings defaultSettings)
  {
    if (defaultSettings == null || textArea.ShowBoldProperties || !defaultSettings.Bold.HasValue)
      return;
    textArea.Bold = defaultSettings.Bold.Value;
  }

  public static GradientStops ParseGradientFill(XmlReader reader, Excel2007Parser parser)
  {
    return ChartParserCommon.ParseGradientFill(reader, parser, (ShapeFillImpl) null);
  }

  internal static GradientStops ParseGradientFill(
    XmlReader reader,
    Excel2007Parser parser,
    ShapeFillImpl fill)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "gradFill")
      throw new XmlException("Unexpected xml tag.");
    GradientStops result = (GradientStops) null;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "gsLst":
            result = ChartParserCommon.ParseGradientStops(reader, parser, fill);
            continue;
          case "lin":
            result.GradientType = GradientType.Liniar;
            reader.MoveToAttribute("ang");
            result.Angle = int.Parse(reader.Value);
            reader.Read();
            continue;
          case "path":
            ChartParserCommon.ParseGradientPath(reader, result);
            continue;
          case "tileRect":
            int intPercentageValue1 = reader.MoveToAttribute("l") ? ChartParserCommon.ParseIntPercentageValue(reader, 1) : 0;
            int intPercentageValue2 = reader.MoveToAttribute("t") ? ChartParserCommon.ParseIntPercentageValue(reader, 1) : 0;
            int intPercentageValue3 = reader.MoveToAttribute("r") ? ChartParserCommon.ParseIntPercentageValue(reader, 1) : 0;
            int intPercentageValue4 = reader.MoveToAttribute("b") ? ChartParserCommon.ParseIntPercentageValue(reader, 1) : 0;
            result.TileRect = Rectangle.FromLTRB(intPercentageValue1, intPercentageValue2, intPercentageValue3, intPercentageValue4);
            reader.Read();
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
    return result;
  }

  public static int ParseIntPercentageValue(XmlReader reader, int multiplyValue)
  {
    string str = reader != null ? reader.Value : throw new ArgumentException("reader is null");
    double result = 0.0;
    int intPercentageValue = double.TryParse(str.Replace("%", ""), out result) ? (int) result : 0;
    if (str.Contains("%"))
      intPercentageValue *= multiplyValue;
    return intPercentageValue;
  }

  private static void ParseGradientPath(XmlReader reader, GradientStops result)
  {
    bool isEmptyElement = reader.IsEmptyElement;
    reader.MoveToAttribute("path");
    result.GradientType = (GradientType) Enum.Parse(typeof (GradientType), reader.Value, true);
    if (isEmptyElement)
      return;
    reader.Read();
    if (!(reader.LocalName == "fillToRect"))
      return;
    int intPercentageValue1 = reader.MoveToAttribute("l") ? ChartParserCommon.ParseIntPercentageValue(reader, 1) : 0;
    int intPercentageValue2 = reader.MoveToAttribute("t") ? ChartParserCommon.ParseIntPercentageValue(reader, 1) : 0;
    int intPercentageValue3 = reader.MoveToAttribute("r") ? ChartParserCommon.ParseIntPercentageValue(reader, 1) : 0;
    int intPercentageValue4 = reader.MoveToAttribute("b") ? ChartParserCommon.ParseIntPercentageValue(reader, 1) : 0;
    result.FillToRect = Rectangle.FromLTRB(intPercentageValue1, intPercentageValue2, intPercentageValue3, intPercentageValue4);
    reader.Read();
    reader.Read();
  }

  private static GradientStops ParseGradientStops(
    XmlReader reader,
    Excel2007Parser parser,
    ShapeFillImpl fill)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "gsLst")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    GradientStops gradientStops = new GradientStops();
    while (reader.NodeType != XmlNodeType.EndElement || reader.LocalName == "gs")
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "gs":
            GradientStopImpl gradientStop = ChartParserCommon.ParseGradientStop(reader, parser);
            gradientStop.ParentFill = fill;
            gradientStops.Add(gradientStop);
            continue;
          default:
            reader.Read();
            continue;
        }
      }
      else
        reader.Read();
    }
    reader.Read();
    return gradientStops;
  }

  private static GradientStopImpl ParseGradientStop(XmlReader reader, Excel2007Parser parser)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "gs")
      throw new XmlException("Unexpected xml tag.");
    int position = -1;
    if (reader.MoveToAttribute("pos"))
      position = XmlConvertExtension.ToInt32(reader.Value);
    reader.Read();
    int transparecy = 0;
    if (reader.NodeType == XmlNodeType.Whitespace)
      reader.Read();
    GradientStopImpl stop;
    if (reader.LocalName == "schemeClr" && reader.NodeType != XmlNodeType.EndElement)
    {
      string colorName = (string) null;
      if (reader.MoveToAttribute("val"))
        colorName = reader.Value;
      Color themeColor = parser.GetThemeColor(colorName);
      reader.MoveToElement();
      stop = new GradientStopImpl((ColorObject) themeColor, position, transparecy);
      ColorObject colorObject = stop.ColorObject;
      colorObject.IsSchemeColor = true;
      colorObject.SchemaName = colorName;
      ChartParserCommon.ParseSchemeColor(reader, parser, stop);
      reader.Read();
      reader.Read();
    }
    else
    {
      int tint;
      int shade;
      Color color = ChartParserCommon.ReadColor(reader, out transparecy, out tint, out shade, parser);
      reader.Read();
      stop = new GradientStopImpl((ColorObject) color, position, transparecy, tint, shade);
    }
    return stop;
  }

  private static void ParseSchemeColor(
    XmlReader reader,
    Excel2007Parser parser,
    GradientStopImpl stop)
  {
    if (reader.LocalName != "schemeClr")
      throw new ArgumentException("Invaild Tag");
    if (reader.IsEmptyElement)
      return;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      ColorObject colorObject = stop.ColorObject;
      switch (reader.LocalName)
      {
        case "lumMod":
          colorObject.Luminance = (double) ChartParserCommon.ParseIntValueTag(reader);
          continue;
        case "lumOff":
          colorObject.LuminanceOffSet = (double) ChartParserCommon.ParseIntValueTag(reader);
          continue;
        case "satMod":
          colorObject.Saturation = (double) ChartParserCommon.ParseIntValueTag(reader);
          continue;
        case "tint":
          colorObject.Tint = (double) ChartParserCommon.ParseIntValueTag(reader);
          continue;
        case "shade":
          stop.Shade = ChartParserCommon.ParseIntValueTag(reader);
          continue;
        case "alpha":
          stop.Transparency = ChartParserCommon.ParseIntValueTag(reader);
          continue;
        default:
          reader.Skip();
          continue;
      }
    }
  }

  internal static Color ReadColor(
    XmlReader reader,
    out int transparecy,
    out int tint,
    out int shade,
    Excel2007Parser parser)
  {
    return ChartParserCommon.ReadColor(reader, out transparecy, out tint, out shade, parser, false);
  }

  internal static Color ReadColor(
    XmlReader reader,
    out int transparecy,
    out int tint,
    out int shade,
    Excel2007Parser parser,
    bool isBlipImage)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    Color color = ColorExtension.Empty;
    transparecy = -1;
    tint = -1;
    shade = -1;
    if (reader.NodeType == XmlNodeType.Whitespace)
      reader.Skip();
    switch (reader.LocalName)
    {
      case "srgbClr":
        color = ChartParserCommon.ParseSRgbColor(reader, out transparecy, out tint, out shade, parser, isBlipImage);
        break;
      case "schemeClr":
        color = ChartParserCommon.ParseSchemeColor(reader, out transparecy, parser, isBlipImage);
        break;
      case "sysClr":
        color = ChartParserCommon.ParseSystemColor(reader, out transparecy, parser, isBlipImage);
        break;
      default:
        reader.Skip();
        break;
    }
    return color;
  }

  private static void ConvertGradientStopsToProperties(
    GradientStops gradientStops,
    IInternalFill fill)
  {
    if (gradientStops == null)
      throw new ArgumentNullException(nameof (gradientStops));
    if (fill == null)
      throw new ArgumentNullException(nameof (fill));
    bool isInverted = false;
    ExcelGradientColor gradientColor = ChartParserCommon.DetectGradientColor(gradientStops);
    fill.IsGradientSupported = true;
    if (gradientColor < ExcelGradientColor.OneColor)
    {
      ExcelGradientPreset preset = ChartParserCommon.FindPreset(gradientStops, out isInverted);
      if (preset >= (ExcelGradientPreset) 0)
      {
        gradientColor = ExcelGradientColor.Preset;
        fill.PresetGradient(preset);
      }
    }
    if (gradientColor < ExcelGradientColor.OneColor)
    {
      gradientColor = ExcelGradientColor.TwoColor;
      fill.IsGradientSupported = false;
    }
    if (gradientColor != ExcelGradientColor.Preset)
    {
      ChartParserCommon.CopyGradientColor(fill.ForeColorObject, gradientStops[0]);
      ChartParserCommon.CopyGradientColor(fill.BackColorObject, gradientStops[gradientStops.Count - 1]);
      fill.FillType = ExcelFillType.Gradient;
      fill.GradientColorType = gradientColor;
    }
    fill.FillType = ExcelFillType.Gradient;
    ExcelGradientStyle gradientStyle;
    fill.GradientStyle = gradientStyle = ChartParserCommon.DetectGradientStyle(gradientStops);
    fill.GradientVariant = ChartParserCommon.DetectGradientVariant(gradientStops, gradientStyle, gradientColor, isInverted);
    ChartParserCommon.SetGradientDegree(gradientStops, gradientColor, (IFill) fill);
  }

  internal static void CheckDefaultSettings(ChartTextAreaImpl textArea)
  {
    bool flag1 = textArea.Font.Language != null;
    WorkbookImpl parentWorkbook = textArea.ParentWorkbook;
    bool flag2 = textArea.FontName != "Calibri";
    bool flag3 = textArea.Size != 10.0;
    bool flag4 = textArea.Bold;
    bool flag5 = textArea.Parent is ChartDataLabelsImpl;
    if (!flag5)
    {
      if (flag2 || flag3)
      {
        ChartImpl parent = textArea.FindParent(typeof (ChartImpl)) as ChartImpl;
        bool flag6 = false;
        if (parent != null && parent.IsChartFontAvail)
          flag6 = true;
        if (flag6)
        {
          flag2 = textArea.FontName != parent.Font.FontName;
          flag3 = textArea.Size != parent.Font.Size;
        }
        if (flag2)
          flag2 = textArea.FontName != parentWorkbook.InnerFonts[0].FontName;
        if (flag3)
          flag3 = textArea.Size != parentWorkbook.InnerFonts[0].Size;
        if (flag3 && textArea.Size == 10.0 && textArea.Parent is ChartAxisImpl)
          flag3 = false;
      }
      if (flag4)
      {
        ChartImpl parent1 = textArea.Parent as ChartImpl;
        if (parentWorkbook.InnerFonts[0].Bold || parent1 != null && parent1.HasChartTitle && parent1.ChartTitleArea == textArea)
          flag4 = false;
        else if (textArea.Parent is ChartAxisImpl parent2)
        {
          if (parent2.HasAxisTitle && parent2.TitleArea == textArea)
            flag4 = false;
          else if (parent2.AxisType == ExcelAxisType.Value)
          {
            ChartValueAxisImpl chartValueAxisImpl = parent2 as ChartValueAxisImpl;
            if (chartValueAxisImpl.HasDisplayUnitLabel && chartValueAxisImpl.DisplayUnitLabel == textArea)
              flag4 = false;
          }
        }
      }
    }
    bool italic = textArea.Italic;
    bool flag7 = textArea.Underline != ExcelUnderline.None;
    bool superscript = textArea.Superscript;
    bool subscript = textArea.Subscript;
    bool strikethrough = textArea.Strikethrough;
    bool hasLatin = textArea.Font.HasLatin;
    bool flag8 = !textArea.IsAutoColor;
    if (!flag1 && !flag2 && !flag3 && !flag4 && !italic && !flag7 && !superscript && !subscript && !strikethrough && !hasLatin && !flag8 || (!flag5 ? (textArea.FontIndex != 0 ? 1 : 0) : 1) == 0)
      return;
    textArea.ParagraphType = ChartParagraphType.CustomDefault;
  }

  private static void CopyGradientColor(ColorObject colorObject, GradientStopImpl gradientStop)
  {
    ColorObject colorObject1 = gradientStop.ColorObject;
    int tint = gradientStop.Tint;
    if (tint >= 0)
    {
      double dTint = (double) tint / 100000.0;
      colorObject1 = (ColorObject) Excel2007Parser.ConvertColorByTint(colorObject1.GetRGB((IWorkbook) null), dTint);
    }
    colorObject.CopyFrom(colorObject1, true);
  }

  public static void ParseShapeProperties(
    XmlReader reader,
    IChartFillObjectGetter objectGetter,
    FileDataHolder dataHolder,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "spPr")
      throw new XmlException("Unexpected xml tag.");
    if (objectGetter == null)
      throw new ArgumentNullException(nameof (objectGetter));
    bool flag = false;
    if (objectGetter.Border != null && objectGetter.Fill == null && objectGetter.Interior == null && objectGetter.Shadow == null && objectGetter.ThreeD == null)
      flag = true;
    IChartInterior chartInterior = (IChartInterior) null;
    if (!flag)
      chartInterior = (IChartInterior) objectGetter.Interior;
    int Alpha = 100000;
    if (chartInterior != null)
    {
      chartInterior.Pattern = ExcelPattern.None;
      chartInterior.UseAutomaticFormat = true;
    }
    Excel2007Parser parser = dataHolder.Parser;
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      objectGetter.Border.AutoFormat = true;
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "ln":
              if (!reader.IsEmptyElement || reader.AttributeCount > 0)
              {
                ChartParserCommon.ParseLineProperties(reader, objectGetter.Border, parser);
                continue;
              }
              reader.Skip();
              continue;
            case "solidFill":
              if (!flag && objectGetter.Fill != null)
              {
                ChartParserCommon.ParseSolidFill(reader, objectGetter.Interior, parser, out Alpha);
                if (objectGetter.Fill.FillType != ExcelFillType.SolidColor)
                  objectGetter.Fill.FillType = ExcelFillType.SolidColor;
                objectGetter.Fill.Transparency = 1.0 - (double) Alpha / 100000.0;
                continue;
              }
              reader.Skip();
              continue;
            case "pattFill":
              if (!flag && objectGetter.Fill != null)
              {
                if (objectGetter.Fill.FillType != ExcelFillType.Pattern)
                  objectGetter.Fill.FillType = ExcelFillType.Pattern;
                ChartParserCommon.ParsePatternFill(reader, (IFill) objectGetter.Fill, parser);
                ExcelGradientPattern pattern = objectGetter.Fill.Pattern;
                if (objectGetter.Interior != null)
                {
                  using (Dictionary<ExcelPattern, ExcelGradientPattern>.Enumerator enumerator = ChartInteriorImpl.m_hashPat.GetEnumerator())
                  {
                    while (enumerator.MoveNext())
                    {
                      KeyValuePair<ExcelPattern, ExcelGradientPattern> current = enumerator.Current;
                      if (current.Value == pattern)
                        objectGetter.Interior.Pattern = current.Key;
                    }
                    continue;
                  }
                }
                continue;
              }
              reader.Skip();
              continue;
            case "gradFill":
              if (!flag && objectGetter.Fill != null)
              {
                GradientStops gradientFill = ChartParserCommon.ParseGradientFill(reader, parser);
                ChartParserCommon.ConvertGradientStopsToProperties(gradientFill, objectGetter.Fill);
                objectGetter.Fill.PreservedGradient = gradientFill;
                objectGetter.Interior.UseAutomaticFormat = false;
                continue;
              }
              reader.Skip();
              continue;
            case "blipFill":
              if (objectGetter.Fill != null)
              {
                ChartParserCommon.ParsePictureFill(reader, (IFill) objectGetter.Fill, relations, dataHolder, ChartParserCommon.m_book, (List<string>) null);
                continue;
              }
              reader.Skip();
              continue;
            case "noFill":
              if (objectGetter.Fill != null)
              {
                objectGetter.Fill.FillType = ExcelFillType.Pattern;
                objectGetter.Fill.Pattern = ~(ExcelGradientPattern.Pat_Mixed | ExcelGradientPattern.Pat_5_Percent);
                objectGetter.Interior.Pattern = ExcelPattern.None;
              }
              reader.Skip();
              continue;
            case "effectLst":
              if (flag)
              {
                reader.Skip();
                continue;
              }
              ChartParserCommon.ParseShadowproperties(reader, objectGetter.Shadow, relations, dataHolder, parser);
              continue;
            case "scene3d":
              ChartParserCommon.ParseLighting(reader, objectGetter.ThreeD, relations, dataHolder);
              continue;
            case "sp3d":
              if (reader.MoveToAttribute("prstMaterial"))
              {
                string material = reader.Value;
                objectGetter.ThreeD.Material = ChartParserCommon.Check(material, reader);
              }
              else
                objectGetter.ThreeD.Material = Excel2007ChartMaterialProperties.NoEffect;
              reader.MoveToElement();
              if (!reader.IsEmptyElement)
              {
                reader.Read();
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                  if (reader.NodeType == XmlNodeType.Element)
                  {
                    string LineWidth = "null";
                    string LineHeight = "null";
                    string PresetShape = "null";
                    switch (reader.LocalName)
                    {
                      case "bevelT":
                        if (reader.MoveToAttribute("w"))
                        {
                          LineWidth = reader.Value;
                          objectGetter.ThreeD.BevelTopWidth = Convert.ToInt32(LineWidth) / 12700;
                        }
                        if (reader.MoveToAttribute("h"))
                        {
                          LineHeight = reader.Value;
                          objectGetter.ThreeD.BevelTopHeight = Convert.ToInt32(LineHeight) / 12700;
                        }
                        if (reader.MoveToAttribute("prst"))
                          PresetShape = reader.Value;
                        objectGetter.ThreeD.BevelTop = ChartParserCommon.Check(LineWidth, LineHeight, PresetShape, reader);
                        continue;
                      case "bevelB":
                        if (reader.MoveToAttribute("w"))
                        {
                          LineWidth = reader.Value;
                          objectGetter.ThreeD.BevelBottomWidth = Convert.ToInt32(LineWidth) / 12700;
                        }
                        if (reader.MoveToAttribute("h"))
                        {
                          LineHeight = reader.Value;
                          objectGetter.ThreeD.BevelBottomHeight = Convert.ToInt32(LineHeight) / 12700;
                        }
                        if (reader.MoveToAttribute("prst"))
                          PresetShape = reader.Value;
                        objectGetter.ThreeD.BevelBottom = ChartParserCommon.Check(LineWidth, LineHeight, PresetShape, reader);
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

  private static void ParseLighting(
    XmlReader reader,
    ThreeDFormatImpl Three_D,
    RelationCollection relations,
    FileDataHolder holder)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "scene3d")
      throw new XmlException("Unexpected xml tag.");
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "lightRig":
              reader.MoveToAttribute("rig");
              string lighttype = reader.Value;
              Three_D.Lighting = ChartParserCommon.Check(lighttype);
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

  public static Excel2007ChartLightingProperties Check(string lighttype)
  {
    Excel2007ChartLightingProperties lightingProperties = Excel2007ChartLightingProperties.ThreePoint;
    for (int index = 0; index < ChartSerializatorCommon.LightingProperties.GetLength(0); ++index)
    {
      if (lighttype.Equals(ChartSerializatorCommon.LightingProperties[index][0]))
      {
        lightingProperties = (Excel2007ChartLightingProperties) index;
        break;
      }
    }
    return lightingProperties;
  }

  public static Excel2007ChartBevelProperties Check(
    string LineWidth,
    string LineHeight,
    string PresetShape,
    XmlReader reader)
  {
    Excel2007ChartBevelProperties chartBevelProperties = Excel2007ChartBevelProperties.NoAngle;
    if (!LineWidth.Equals("null") && !LineHeight.Equals("null") && PresetShape.Equals("null"))
    {
      chartBevelProperties = Excel2007ChartBevelProperties.Circle;
      reader.Skip();
    }
    else
    {
      for (int index = 0; index < ChartSerializatorCommon.BevelProperties.GetLength(0); ++index)
      {
        if (PresetShape.Equals(ChartSerializatorCommon.BevelProperties[index][2]))
        {
          chartBevelProperties = (Excel2007ChartBevelProperties) index;
          reader.Skip();
          break;
        }
      }
    }
    return chartBevelProperties;
  }

  public static Excel2007ChartMaterialProperties Check(string material, XmlReader reader)
  {
    Excel2007ChartMaterialProperties materialProperties = Excel2007ChartMaterialProperties.NoEffect;
    for (int index = 0; index < ChartSerializatorCommon.MaterialProperties.GetLength(0); ++index)
    {
      if (material.Equals(ChartSerializatorCommon.MaterialProperties[index][0]))
      {
        materialProperties = (Excel2007ChartMaterialProperties) (index + 1);
        break;
      }
    }
    return materialProperties;
  }

  private static void ParseShadowproperties(
    XmlReader reader,
    ShadowImpl shadow,
    RelationCollection relations,
    FileDataHolder holder,
    Excel2007Parser parser)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "effectLst")
      throw new XmlException("Unexpected xml tag.");
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          string blurval = "null";
          string sizex = "null";
          string sizey = "null";
          string disttag = "null";
          string dirtag = "null";
          string align = "null";
          string rot = "null";
          string kxtag = "null";
          switch (reader.LocalName)
          {
            case "outerShdw":
              if (reader.MoveToAttribute("blurRad"))
                blurval = reader.Value;
              if (reader.MoveToAttribute("sx"))
                sizex = ChartParserCommon.ParseIntPercentageValue(reader, 1).ToString();
              if (reader.MoveToAttribute("sy"))
                sizey = ChartParserCommon.ParseIntPercentageValue(reader, 1).ToString();
              if (reader.MoveToAttribute("kx"))
                kxtag = reader.Value;
              if (reader.MoveToAttribute("dist"))
                disttag = reader.Value;
              if (reader.MoveToAttribute("dir"))
                dirtag = reader.Value;
              if (reader.MoveToAttribute("algn"))
                align = reader.Value;
              if (reader.MoveToAttribute("rotWithShape"))
                rot = reader.Value;
              if (kxtag == "null" && blurval.Equals("50800") || kxtag == "null" && blurval.Equals("63500") && !sizex.Equals("null"))
              {
                shadow.ShadowOuterPresets = ChartParserCommon.Check(blurval, sizex, sizey, disttag, dirtag, align, rot, shadow, reader, parser);
                continue;
              }
              if (!kxtag.Equals("null") || sizex.Equals("90000") && kxtag.Equals("null"))
              {
                shadow.ShadowPrespectivePresets = ChartParserCommon.Check(blurval, sizex, sizey, kxtag, disttag, dirtag, align, rot);
                continue;
              }
              shadow.ShadowOuterPresets = ChartParserCommon.Check(blurval, sizex, sizey, disttag, dirtag, align, rot, shadow, reader, parser);
              continue;
            case "innerShdw":
              if (reader.MoveToAttribute("blurRad"))
                blurval = reader.Value;
              if (reader.MoveToAttribute("dist"))
                disttag = reader.Value;
              if (reader.MoveToAttribute("dir"))
                dirtag = reader.Value;
              shadow.ShadowInnerPresets = ChartParserCommon.Check(blurval, disttag, dirtag, reader, shadow, parser);
              continue;
            case "glow":
              if (!reader.IsEmptyElement || reader.AttributeCount > 0)
              {
                MemoryStream data = new MemoryStream();
                XmlWriter writer = UtilityMethods.CreateWriter((Stream) data, Encoding.UTF8);
                writer.WriteNode(reader, false);
                writer.Flush();
                shadow.GlowStream = (Stream) data;
                continue;
              }
              reader.Skip();
              continue;
            case "softEdge":
              if (reader.MoveToAttribute("rad"))
              {
                shadow.SoftEdgeRadius = int.Parse(reader.Value);
                reader.MoveToElement();
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
    else
      shadow.HasCustomShadowStyle = true;
    reader.Read();
  }

  public static Excel2007ChartPresetsOuter Check(
    string blurval,
    string sizex,
    string sizey,
    string disttag,
    string dirtag,
    string align,
    string rot,
    ShadowImpl Shadow,
    XmlReader reader,
    Excel2007Parser parser)
  {
    int num = 0;
    Excel2007ChartPresetsOuter chartPresetsOuter = Excel2007ChartPresetsOuter.NoShadow;
    if (blurval.Equals("null") && sizex.Equals("null") && sizey.Equals("null") && disttag.Equals("null") && dirtag.Equals("null") && align.Equals("null"))
    {
      chartPresetsOuter = Excel2007ChartPresetsOuter.NoShadow;
    }
    else
    {
      for (int index = 0; index < ChartSerializatorCommon.OuterAttributeArray.GetLength(0); ++index)
      {
        ++num;
        if (blurval.Equals(ChartSerializatorCommon.OuterAttributeArray[index][0]) && sizex.Equals(ChartSerializatorCommon.OuterAttributeArray[index][1]) && sizey.Equals(ChartSerializatorCommon.OuterAttributeArray[index][2]) && disttag.Equals(ChartSerializatorCommon.OuterAttributeArray[index][3]) && dirtag.Equals(ChartSerializatorCommon.OuterAttributeArray[index][4]) && align.Equals(ChartSerializatorCommon.OuterAttributeArray[index][5]) && rot.Equals(ChartSerializatorCommon.OuterAttributeArray[index][6]))
        {
          chartPresetsOuter = (Excel2007ChartPresetsOuter) (index + 1);
          break;
        }
      }
      if (num == ChartSerializatorCommon.OuterAttributeArray.GetLength(0))
        chartPresetsOuter = ChartParserCommon.Check(blurval, sizex, disttag, dirtag, align, rot, Shadow, reader, parser);
    }
    return chartPresetsOuter;
  }

  public static Excel2007ChartPresetsOuter Check(
    string blurval,
    string sizex,
    string disttag,
    string dirtag,
    string align,
    string rot,
    ShadowImpl Shadow,
    XmlReader reader,
    Excel2007Parser parser)
  {
    Excel2007ChartPresetsOuter chartPresetsOuter = Excel2007ChartPresetsOuter.NoShadow;
    for (int index = 0; index < ChartSerializatorCommon.OuterAttributeArray.GetLength(0); ++index)
    {
      if (align.Equals(ChartSerializatorCommon.OuterAttributeArray[index][5]))
      {
        chartPresetsOuter = (Excel2007ChartPresetsOuter) (index + 1);
        break;
      }
    }
    Shadow.HasCustomShadowStyle = true;
    Shadow.Blur = blurval != "null" ? Convert.ToInt32(blurval) / 12700 : 0;
    Shadow.Size = !(sizex != "null") || !(sizex != "0") ? 100 : Convert.ToInt32(sizex) / 1000;
    Shadow.Distance = disttag != "null" ? Convert.ToInt32(disttag) / 12700 : 0;
    Shadow.Angle = dirtag != "null" ? Convert.ToInt32(dirtag) / 60000 : 0;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "prstClr":
            if (reader.MoveToAttribute("val"))
            {
              string colorName = reader.Value;
              Shadow.ShadowColor = parser.GetThemeColor(colorName);
            }
            reader.MoveToElement();
            if (!reader.IsEmptyElement)
            {
              ChartParserCommon.ParseShadowAlpha(reader, Shadow);
              continue;
            }
            reader.Skip();
            continue;
          case "schemeClr":
            if (reader.MoveToAttribute("val"))
            {
              string colorName = reader.Value;
              Shadow.ShadowColor = parser.GetThemeColor(colorName);
            }
            reader.MoveToElement();
            if (!reader.IsEmptyElement)
            {
              ChartParserCommon.ParseShadowAlpha(reader, Shadow);
              continue;
            }
            reader.Skip();
            continue;
          case "srgbClr":
            if (reader.MoveToAttribute("val"))
            {
              int num = int.Parse(reader.Value, NumberStyles.HexNumber, (IFormatProvider) null);
              Shadow.ShadowColor = ColorExtension.FromArgb(num);
            }
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
                    case "alpha":
                      Shadow.Transparency = !reader.MoveToAttribute("val") ? 100000 : 100 - Convert.ToInt32(reader.Value) / 1000;
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
    return chartPresetsOuter;
  }

  public static void ParseShadowAlpha(XmlReader reader, ShadowImpl Shadow)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "prstClr" && reader.LocalName != "schemeClr")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "alpha":
            if (reader.MoveToAttribute("val"))
            {
              Shadow.Transparency = 100 - XmlConvertExtension.ToInt32(reader.Value) / 1000;
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
    reader.Read();
  }

  public static void ParseNumberFormat(XmlReader reader, IChartDataLabels dataLabels)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.MoveToAttribute("formatCode"))
    {
      string str = reader.Value;
      if (str != "")
        (dataLabels as ChartDataLabelsImpl).NumberFormat = str;
    }
    if (!reader.MoveToAttribute("sourceLinked"))
      return;
    (dataLabels as ChartDataLabelsImpl).IsSourceLinked = XmlConvertExtension.ToBoolean(reader.Value);
  }

  public static Excel2007ChartPresetsPrespective Check(
    string blurval,
    string sizex,
    string sizey,
    string kxtag,
    string disttag,
    string dirtag,
    string align,
    string rot)
  {
    Excel2007ChartPresetsPrespective presetsPrespective = Excel2007ChartPresetsPrespective.NoShadow;
    for (int index = 0; index < ChartSerializatorCommon.PerspectiveAttributeArray.GetLength(0); ++index)
    {
      if (blurval.Equals(ChartSerializatorCommon.PerspectiveAttributeArray[index][0]) && sizex.Equals(ChartSerializatorCommon.PerspectiveAttributeArray[index][4]) && sizey.Equals(ChartSerializatorCommon.PerspectiveAttributeArray[index][3]) && disttag.Equals(ChartSerializatorCommon.PerspectiveAttributeArray[index][2]) && dirtag.Equals(ChartSerializatorCommon.PerspectiveAttributeArray[index][1]) && kxtag.Equals(ChartSerializatorCommon.PerspectiveAttributeArray[index][5]) && align.Equals(ChartSerializatorCommon.PerspectiveAttributeArray[index][6]) && rot.Equals(ChartSerializatorCommon.PerspectiveAttributeArray[index][7]))
      {
        presetsPrespective = (Excel2007ChartPresetsPrespective) index;
        break;
      }
    }
    return presetsPrespective;
  }

  public static Excel2007ChartPresetsInner Check(
    string blurval,
    string disttag,
    string dirtag,
    XmlReader reader,
    ShadowImpl Shadow,
    Excel2007Parser parser)
  {
    int num = 0;
    Excel2007ChartPresetsInner chartPresetsInner = Excel2007ChartPresetsInner.NoShadow;
    if (blurval.Equals("null") && disttag.Equals("null") && dirtag.Equals("null"))
    {
      chartPresetsInner = Excel2007ChartPresetsInner.NoShadow;
    }
    else
    {
      for (int index = 0; index < ChartSerializatorCommon.InnerAttributeArray.GetLength(0); ++index)
      {
        ++num;
        if (blurval.Equals(ChartSerializatorCommon.InnerAttributeArray[index][0]) && disttag.Equals(ChartSerializatorCommon.InnerAttributeArray[index][1]) && dirtag.Equals(ChartSerializatorCommon.InnerAttributeArray[index][2]))
        {
          chartPresetsInner = (Excel2007ChartPresetsInner) (index + 1);
          break;
        }
      }
      if (num == ChartSerializatorCommon.InnerAttributeArray.GetLength(0))
      {
        if (blurval == "null")
          blurval = "0";
        if (disttag == "null")
          disttag = "0";
        if (dirtag == "null")
          dirtag = "0";
        chartPresetsInner = ChartParserCommon.Check(blurval, disttag, dirtag, Shadow, reader, true, parser);
      }
    }
    return chartPresetsInner;
  }

  public static Excel2007ChartPresetsInner Check(
    string blurval,
    string disttag,
    string dirtag,
    ShadowImpl Shadow,
    XmlReader reader,
    bool m_HasShadowStyle,
    Excel2007Parser parser)
  {
    Excel2007ChartPresetsInner chartPresetsInner = Excel2007ChartPresetsInner.InsideDiagonalTopLeft;
    Shadow.HasCustomShadowStyle = m_HasShadowStyle;
    Shadow.Blur = Convert.ToInt32(blurval) / 12700;
    Shadow.Distance = Convert.ToInt32(disttag) / 12700;
    Shadow.Angle = Convert.ToInt32(dirtag) / 60000;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "prstClr":
            if (reader.MoveToAttribute("val"))
            {
              string colorName = reader.Value;
              Shadow.ShadowColor = parser.GetThemeColor(colorName);
            }
            reader.MoveToElement();
            if (!reader.IsEmptyElement)
            {
              ChartParserCommon.ParseShadowAlpha(reader, Shadow);
              continue;
            }
            reader.Skip();
            continue;
          case "schemeClr":
            if (reader.MoveToAttribute("val"))
            {
              string colorName = reader.Value;
              Shadow.ShadowColor = parser.GetThemeColor(colorName);
            }
            reader.MoveToElement();
            if (!reader.IsEmptyElement)
            {
              ChartParserCommon.ParseShadowAlpha(reader, Shadow);
              continue;
            }
            reader.Skip();
            continue;
          case "srgbClr":
            if (reader.MoveToAttribute("val"))
            {
              int num = int.Parse(reader.Value, NumberStyles.HexNumber, (IFormatProvider) null);
              Shadow.ShadowColor = ColorExtension.FromArgb(num);
            }
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
                    case "alpha":
                      Shadow.Transparency = !reader.MoveToAttribute("val") ? 100000 : 100 - Convert.ToInt32(reader.Value) / 1000;
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
    return chartPresetsInner;
  }

  private static ExcelGradientColor DetectGradientColor(GradientStops gradientStops)
  {
    if (gradientStops == null)
      throw new ArgumentNullException(nameof (gradientStops));
    ExcelGradientColor excelGradientColor = ~ExcelGradientColor.OneColor;
    switch (gradientStops.Count)
    {
      case 2:
        excelGradientColor = gradientStops[0].ColorObject == gradientStops[1].ColorObject ? ExcelGradientColor.OneColor : ExcelGradientColor.TwoColor;
        break;
      case 3:
        GradientStopImpl gradientStop1 = gradientStops[0];
        GradientStopImpl gradientStop2 = gradientStops[1];
        GradientStopImpl gradientStop3 = gradientStops[2];
        if (gradientStop1.ColorObject == gradientStop3.ColorObject)
        {
          excelGradientColor = gradientStop1.ColorObject == gradientStop2.ColorObject ? ExcelGradientColor.OneColor : ExcelGradientColor.TwoColor;
          break;
        }
        break;
    }
    return excelGradientColor;
  }

  private static ExcelGradientVariants DetectGradientVariant(
    GradientStops gradientStops,
    ExcelGradientStyle gradientStyle,
    ExcelGradientColor gradientColor,
    bool isPresetInverted)
  {
    if (gradientStops == null)
      throw new ArgumentNullException(nameof (gradientStops));
    ExcelGradientVariants gradientVariants = ExcelGradientVariants.ShadingVariants_1;
    bool bInverted = ChartParserCommon.IsInverted(gradientStops, gradientColor, isPresetInverted);
    bool isDoubled = gradientStops.IsDoubled;
    switch (gradientStyle)
    {
      case ExcelGradientStyle.Horizontal:
      case ExcelGradientStyle.Vertical:
      case ExcelGradientStyle.Diagonl_Up:
        gradientVariants = ChartParserCommon.DetectStandardVariant(bInverted, isDoubled);
        break;
      case ExcelGradientStyle.Diagonl_Down:
        gradientVariants = ChartParserCommon.DetectDiagonalDownVariant(bInverted, isDoubled);
        break;
      case ExcelGradientStyle.From_Corner:
        gradientVariants = ChartParserCommon.DetectGradientVariantCorner(gradientStops.FillToRect);
        break;
      case ExcelGradientStyle.From_Center:
        gradientVariants = bInverted ? ExcelGradientVariants.ShadingVariants_1 : ExcelGradientVariants.ShadingVariants_2;
        break;
    }
    return gradientVariants;
  }

  private static ExcelGradientVariants DetectDiagonalDownVariant(bool bInverted, bool bDoubled)
  {
    return !bInverted || !bDoubled ? (!bDoubled ? (!bInverted ? ExcelGradientVariants.ShadingVariants_2 : ExcelGradientVariants.ShadingVariants_1) : ExcelGradientVariants.ShadingVariants_3) : ExcelGradientVariants.ShadingVariants_4;
  }

  private static ExcelGradientVariants DetectStandardVariant(bool bInverted, bool bDoubled)
  {
    return !bInverted || !bDoubled ? (!bDoubled ? (!bInverted ? ExcelGradientVariants.ShadingVariants_1 : ExcelGradientVariants.ShadingVariants_2) : ExcelGradientVariants.ShadingVariants_3) : ExcelGradientVariants.ShadingVariants_4;
  }

  private static ExcelGradientVariants DetectGradientVariantCorner(Rectangle rectangle)
  {
    Rectangle[] rectanglesCorner = ShapeFillImpl.RectanglesCorner;
    ExcelGradientVariants gradientVariants = ExcelGradientVariants.ShadingVariants_1;
    int index = 0;
    for (int length = rectanglesCorner.Length; index < length; ++index)
    {
      if (rectanglesCorner[index] == rectangle)
      {
        gradientVariants = (ExcelGradientVariants) index;
        break;
      }
    }
    return gradientVariants;
  }

  private static bool IsInverted(
    GradientStops gradientStops,
    ExcelGradientColor gradientColor,
    bool isPresetInverted)
  {
    if (gradientStops == null)
      throw new ArgumentNullException(nameof (gradientStops));
    bool flag = false;
    switch (gradientColor)
    {
      case ExcelGradientColor.OneColor:
        if (gradientStops[0].Shade > 0 || gradientStops[0].Tint > 0)
        {
          flag = true;
          break;
        }
        break;
      case ExcelGradientColor.TwoColor:
        flag = false;
        break;
      case ExcelGradientColor.Preset:
        flag = isPresetInverted;
        break;
    }
    return flag;
  }

  private static ExcelGradientStyle DetectGradientStyle(GradientStops gradientStops)
  {
    if (gradientStops == null)
      throw new ArgumentNullException(nameof (gradientStops));
    ExcelGradientStyle excelGradientStyle = ExcelGradientStyle.Horizontal;
    switch (gradientStops.GradientType)
    {
      case GradientType.Liniar:
        excelGradientStyle = ChartParserCommon.GetLiniarGradientStyle(gradientStops);
        break;
      case GradientType.Rect:
        excelGradientStyle = ChartParserCommon.GetRectGradientStyle(gradientStops);
        break;
    }
    return excelGradientStyle;
  }

  private static ExcelGradientStyle GetRectGradientStyle(GradientStops gradientStops)
  {
    if (gradientStops == null)
      throw new ArgumentNullException(nameof (gradientStops));
    return !(gradientStops.FillToRect == ShapeFillImpl.RectangleFromCenter) ? ExcelGradientStyle.From_Corner : ExcelGradientStyle.From_Center;
  }

  private static ExcelGradientStyle GetLiniarGradientStyle(GradientStops gradientStops)
  {
    int num = gradientStops != null ? gradientStops.Angle : throw new ArgumentNullException(nameof (gradientStops));
    return num != 0 ? (num < 0 || num > 2700000 ? (num < 2700000 || num > 5400000 ? ExcelGradientStyle.Diagonl_Down : ExcelGradientStyle.Horizontal) : ExcelGradientStyle.Diagonl_Up) : ExcelGradientStyle.Vertical;
  }

  private static ExcelGradientPreset FindPreset(GradientStops gradientStops, out bool isInverted)
  {
    if (gradientStops == null)
      throw new ArgumentNullException(nameof (gradientStops));
    ExcelGradientPreset[] values = (ExcelGradientPreset[]) Enum.GetValues(typeof (ExcelGradientPreset));
    if (gradientStops.IsDoubled)
      gradientStops = gradientStops.ShrinkGradientStops();
    isInverted = false;
    ExcelGradientPreset preset1 = (ExcelGradientPreset) -1;
    GradientStops gradientStops1 = gradientStops.Clone();
    gradientStops1.InvertGradientStops();
    int index = 0;
    for (int length = values.Length; index < length; ++index)
    {
      ExcelGradientPreset preset2 = values[index];
      GradientStops presetGradientStops = ShapeFillImpl.GetPresetGradientStops(preset2);
      if (presetGradientStops.EqualColors(gradientStops))
      {
        preset1 = preset2;
        break;
      }
      if (presetGradientStops.EqualColors(gradientStops1))
      {
        isInverted = true;
        preset1 = preset2;
        break;
      }
    }
    return preset1;
  }

  private static void SetGradientDegree(
    GradientStops gradientStops,
    ExcelGradientColor gradientColor,
    IFill fill)
  {
    if (gradientStops == null)
      throw new ArgumentNullException(nameof (gradientStops));
    if (fill == null)
      throw new ArgumentNullException(nameof (fill));
    if (gradientColor != ExcelGradientColor.OneColor)
      return;
    int num1 = Math.Max(gradientStops[0].Tint, gradientStops[1].Tint);
    int num2 = Math.Max(gradientStops[0].Shade, gradientStops[1].Shade);
    double num3 = num2 < 0 ? (num1 < 0 ? 0.5 : 1.0 - (double) num1 / 100000.0) : (double) num2 / 100000.0;
    fill.GradientDegree = num3;
  }

  internal static void ParseChartTitleElement(
    Stream titleAreaStream,
    IInternalChartTextArea textArea,
    FileDataHolder holder,
    RelationCollection relations,
    float fontSize)
  {
    titleAreaStream.Position = 0L;
    XmlReader reader1 = UtilityMethods.CreateReader(titleAreaStream);
    reader1.Read();
    if ((reader1.LocalName == "title" || reader1.LocalName == "units") && !reader1.IsEmptyElement)
    {
      reader1.Read();
      while (reader1.NodeType != XmlNodeType.EndElement)
      {
        if (reader1.NodeType == XmlNodeType.Element)
        {
          switch (reader1.LocalName)
          {
            case "txPr":
              if (textArea is ChartTextAreaImpl chartTextAreaImpl)
              {
                chartTextAreaImpl.ParagraphType = ChartParagraphType.CustomDefault;
                ChartParserCommon.SetWorkbook(ChartParserCommon.m_book);
                ChartParserCommon.ParseDefaultTextFormatting(reader1, textArea, holder.Parser, new double?((double) fontSize));
                chartTextAreaImpl.IsTextParsed = true;
                continue;
              }
              continue;
            case "unitsLabel":
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
    titleAreaStream.Position = 0L;
    XmlReader reader2 = UtilityMethods.CreateReader(titleAreaStream);
    reader2.Read();
    if ((reader2.LocalName == "title" || reader2.LocalName == "units") && !reader2.IsEmptyElement)
    {
      ChartParserCommon.SetWorkbook(ChartParserCommon.m_book);
      ChartParserCommon.ParseTextArea(reader2, textArea, holder, relations, new float?(fontSize));
    }
    reader2.Close();
    reader1.Close();
    titleAreaStream.Dispose();
  }
}
