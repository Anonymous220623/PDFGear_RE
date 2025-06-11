// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.Helper
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Compression.Zip;
using Syncfusion.Office;
using Syncfusion.Presentation.RichText;
using Syncfusion.Presentation.SlideImplementation;
using Syncfusion.Presentation.TableImplementation;
using Syncfusion.Presentation.Themes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class Helper
{
  [ThreadStatic]
  private static Dictionary<string, int> _presetColors;

  internal static short GetLanguageID(string value)
  {
    switch (value)
    {
      case "ar":
        return 1;
      case "bg":
        return 2;
      case "ca":
        return 3;
      case "zh":
        return 4;
      case "cs":
        return 5;
      case "da":
        return 6;
      case "de":
        return 7;
      case "el":
        return 8;
      case "en":
        return 9;
      case "es":
        return 10;
      case "fi":
        return 11;
      case "fr":
        return 12;
      case "he":
        return 13;
      case "hu":
        return 14;
      case "is":
        return 15;
      case "it":
        return 16 /*0x10*/;
      case "ja":
        return 17;
      case "ko":
        return 18;
      case "nl":
        return 19;
      case "nb":
        return 20;
      case "pl":
        return 21;
      case "pt":
        return 22;
      case "rm":
        return 23;
      case "ro":
        return 24;
      case "ru":
        return 25;
      case "hr":
        return 26;
      case "sk":
        return 27;
      case "sq":
        return 28;
      case "sv":
        return 29;
      case "th":
        return 30;
      case "tr":
        return 31 /*0x1F*/;
      case "ur":
        return 32 /*0x20*/;
      case "id":
        return 33;
      case "uk":
        return 34;
      case "be":
        return 35;
      case "sl":
        return 36;
      case "et":
        return 37;
      case "lv":
        return 38;
      case "lt":
        return 39;
      case "tg":
        return 40;
      case "fa":
        return 41;
      case "vi":
        return 42;
      case "hy":
        return 43;
      case "az":
        return 44;
      case "eu":
        return 45;
      case "hsb":
        return 46;
      case "mk":
        return 47;
      case "tn":
        return 50;
      case "xh":
        return 52;
      case "zu":
        return 53;
      case "af":
        return 54;
      case "ka":
        return 55;
      case "fo":
        return 56;
      case "hi":
        return 57;
      case "mt":
        return 58;
      case "smn":
        return 59;
      case "ms":
        return 62;
      case "kk":
        return 63 /*0x3F*/;
      case "ky":
        return 64 /*0x40*/;
      case "sw":
        return 65;
      case "tk":
        return 66;
      case "uz":
        return 67;
      case "tt":
        return 68;
      case "bn":
        return 69;
      case "pa":
        return 70;
      case "gu":
        return 71;
      case "or":
        return 72;
      case "ta":
        return 73;
      case "te":
        return 74;
      case "kn":
        return 75;
      case "ml":
        return 76;
      case "mr":
        return 78;
      case "sa":
        return 79;
      case "mn":
        return 80 /*0x50*/;
      case "bo":
        return 81;
      case "cy":
        return 82;
      case "lo":
        return 84;
      case "gl":
        return 86;
      case "kok":
        return 87;
      case "syr":
        return 90;
      case "si":
        return 91;
      case "chr":
        return 92;
      case "iu":
        return 93;
      case "am":
        return 94;
      case "ne":
        return 97;
      case "fy":
        return 98;
      case "ps":
        return 99;
      case "fil":
        return 100;
      case "dv":
        return 101;
      case "ff":
        return 103;
      case "ha":
        return 104;
      case "quz":
        return 107;
      case "nso":
        return 108;
      case "ba":
        return 109;
      case "lb":
        return 110;
      case "kl":
        return 111;
      case "ig":
        return 112 /*0x70*/;
      case "ti":
        return 115;
      case "haw":
        return 117;
      case "ii":
        return 120;
      case "arn":
        return 122;
      case "br":
        return 126;
      case "ug":
        return 128 /*0x80*/;
      case "mi":
        return 129;
      case "oc":
        return 130;
      case "co":
        return 131;
      case "gsw":
        return 132;
      case "sah":
        return 133;
      case "qut":
        return 134;
      case "rw":
        return 135;
      case "prs":
        return 140;
      default:
        return 1033;
    }
  }

  internal static string GetLanguage(short key)
  {
    switch (key)
    {
      case 1:
        return "ar";
      case 2:
        return "bg";
      case 3:
        return "ca";
      case 4:
        return "zh";
      case 5:
        return "cs";
      case 6:
        return "da";
      case 7:
        return "de";
      case 8:
        return "el";
      case 9:
        return "en";
      case 10:
        return "es";
      case 11:
        return "fi";
      case 12:
        return "fr";
      case 13:
        return "he";
      case 14:
        return "hu";
      case 15:
        return "is";
      case 16 /*0x10*/:
        return "it";
      case 17:
        return "ja";
      case 18:
        return "ko";
      case 19:
        return "nl";
      case 20:
        return "nb";
      case 21:
        return "pl";
      case 22:
        return "pt";
      case 23:
        return "rm";
      case 24:
        return "ro";
      case 25:
        return "ru";
      case 26:
        return "hr";
      case 27:
        return "sk";
      case 28:
        return "sq";
      case 29:
        return "sv";
      case 30:
        return "th";
      case 31 /*0x1F*/:
        return "tr";
      case 32 /*0x20*/:
        return "ur";
      case 33:
        return "id";
      case 34:
        return "uk";
      case 35:
        return "be";
      case 36:
        return "sl";
      case 37:
        return "et";
      case 38:
        return "lv";
      case 39:
        return "lt";
      case 40:
        return "tg";
      case 41:
        return "fa";
      case 42:
        return "vi";
      case 43:
        return "hy";
      case 44:
        return "az";
      case 45:
        return "eu";
      case 46:
        return "hsb";
      case 47:
        return "mk";
      case 50:
        return "tn";
      case 52:
        return "xh";
      case 53:
        return "zu";
      case 54:
        return "af";
      case 55:
        return "ka";
      case 56:
        return "fo";
      case 57:
        return "hi";
      case 58:
        return "mt";
      case 59:
        return "smn";
      case 62:
        return "ms";
      case 63 /*0x3F*/:
        return "kk";
      case 64 /*0x40*/:
        return "ky";
      case 65:
        return "sw";
      case 66:
        return "tk";
      case 67:
        return "uz";
      case 68:
        return "tt";
      case 69:
        return "bn";
      case 70:
        return "pa";
      case 71:
        return "gu";
      case 72:
        return "or";
      case 73:
        return "ta";
      case 74:
        return "te";
      case 75:
        return "kn";
      case 76:
        return "ml";
      case 78:
        return "mr";
      case 79:
        return "sa";
      case 80 /*0x50*/:
        return "mn";
      case 81:
        return "bo";
      case 82:
        return "cy";
      case 84:
        return "lo";
      case 86:
        return "gl";
      case 87:
        return "kok";
      case 90:
        return "syr";
      case 91:
        return "si";
      case 92:
        return "chr";
      case 93:
        return "iu";
      case 94:
        return "am";
      case 97:
        return "ne";
      case 98:
        return "fy";
      case 99:
        return "ps";
      case 100:
        return "fil";
      case 101:
        return "dv";
      case 103:
        return "ff";
      case 104:
        return "ha";
      case 107:
        return "quz";
      case 108:
        return "nso";
      case 109:
        return "ba";
      case 110:
        return "lb";
      case 111:
        return "kl";
      case 112 /*0x70*/:
        return "ig";
      case 115:
        return "ti";
      case 117:
        return "haw";
      case 120:
        return "ii";
      case 122:
        return "arn";
      case 126:
        return "br";
      case 128 /*0x80*/:
        return "ug";
      case 129:
        return "mi";
      case 130:
        return "oc";
      case 131:
        return "co";
      case 132:
        return "gsw";
      case 133:
        return "sah";
      case 134:
        return "qut";
      case 135:
        return "rw";
      case 140:
        return "prs";
      default:
        return "en-US";
    }
  }

  internal static bool HasFlag(Enum variable, Enum value)
  {
    ulong num = !(variable.GetType() != value.GetType()) ? Convert.ToUInt64((object) value) : throw new ArgumentException("The checked flag is not from the same type as the checked enum variable.");
    return ((long) Convert.ToUInt64((object) variable) & (long) num) == (long) num;
  }

  internal static bool IsNullOrWhiteSpace(string text)
  {
    if (text != null)
    {
      for (int index = 0; index < text.Length; ++index)
      {
        if (!char.IsWhiteSpace(text[index]))
          return false;
      }
    }
    return true;
  }

  internal static string GetSlidePath(
    Dictionary<string, string> slideIdList,
    RelationCollection topRelation,
    string slideId)
  {
    string slideRelationId = (string) null;
    foreach (KeyValuePair<string, string> slideId1 in slideIdList)
    {
      if (slideId1.Value == slideId)
      {
        slideRelationId = slideId1.Key;
        break;
      }
    }
    return topRelation.GetItemPathByRelation(slideRelationId);
  }

  internal static string GetFileNameWithoutExtension(string strUrl)
  {
    if (strUrl == null || strUrl.Length == 0)
      return strUrl;
    strUrl = strUrl.Replace("/", "\\");
    int num1 = strUrl.LastIndexOf('\\');
    int num2 = strUrl.LastIndexOf('.');
    int startIndex = 0;
    int num3 = strUrl.Length;
    if (num1 > 0)
      startIndex = num1 + 1;
    if (num2 > startIndex)
      num3 = num2;
    return strUrl.Substring(startIndex, num3 - startIndex);
  }

  internal static string GetFileName(string strUrl)
  {
    if (strUrl == null || strUrl.Length == 0)
      return strUrl;
    strUrl = strUrl.Replace("/", "\\");
    int num = strUrl.LastIndexOf('\\');
    int startIndex = 0;
    int length = strUrl.Length;
    if (num > 0)
      startIndex = num + 1;
    return strUrl.Substring(startIndex, length - startIndex);
  }

  internal static string GetExtension(string fileName)
  {
    string[] strArray = fileName.Split('.');
    fileName = strArray[strArray.Length - 1];
    return "." + fileName;
  }

  internal static string GenerateItemName(
    out int itemsCount,
    string pathFormat,
    ZipArchive archive)
  {
    itemsCount = 0;
    string itemName;
    do
    {
      ++itemsCount;
      itemName = string.Format(pathFormat, (object) itemsCount);
    }
    while (archive.Find(itemName) >= 0);
    return itemName;
  }

  internal static bool EnumContains(BorderType availBorderType, BorderType expectBorderType)
  {
    int int32_1 = Convert.ToInt32((object) availBorderType);
    int int32_2 = Convert.ToInt32((object) expectBorderType);
    return (int32_1 & int32_2) == int32_2;
  }

  internal static PlaceholderType GetPlaceHolderType(HeaderFooterType headerFooterType)
  {
    switch (headerFooterType)
    {
      case HeaderFooterType.Footer:
        return PlaceholderType.Footer;
      case HeaderFooterType.DateAndTime:
        return PlaceholderType.Date;
      case HeaderFooterType.SlideNumber:
        return PlaceholderType.SlideNumber;
      case HeaderFooterType.Header:
        return PlaceholderType.Header;
      default:
        return PlaceholderType.Object;
    }
  }

  internal static bool IsHeaderFooterShape(PlaceholderType placeholderType)
  {
    return placeholderType == PlaceholderType.Date || placeholderType == PlaceholderType.Header || placeholderType == PlaceholderType.Footer || placeholderType == PlaceholderType.SlideNumber;
  }

  internal static DateTimeFormatType GetDateTimeFormatType(string type)
  {
    switch (type)
    {
      case "datetime":
      case "datetime1":
        return DateTimeFormatType.DateTimeMdyy;
      case "datetime2":
        return DateTimeFormatType.DateTimeddddMMMMddyyyy;
      case "datetime3":
        return DateTimeFormatType.DateTimedMMMMyyyy;
      case "datetime4":
        return DateTimeFormatType.DateTimeMMMMdyyyy;
      case "datetime5":
        return DateTimeFormatType.DateTimedMMMyy;
      case "datetime6":
        return DateTimeFormatType.DateTimeMMMMyy;
      case "datetime7":
        return DateTimeFormatType.DateTimeMMMyy;
      case "datetime8":
        return DateTimeFormatType.DateTimeMMddyyhmmAMPM;
      case "datetime9":
        return DateTimeFormatType.DateTimeMMddyyhmmssAMPM;
      case "datetime10":
        return DateTimeFormatType.DateTimeHmm;
      case "datetime11":
        return DateTimeFormatType.DateTimeHmmss;
      case "datetime12":
        return DateTimeFormatType.DateTimehmmAMPM;
      case "datetime13":
        return DateTimeFormatType.DateTimehmmssAMPM;
      default:
        return DateTimeFormatType.None;
    }
  }

  internal static string GetDateTimeFieldType(DateTimeFormatType type)
  {
    switch (type)
    {
      case DateTimeFormatType.DateTimeMdyy:
        return "datetime1";
      case DateTimeFormatType.DateTimeddddMMMMddyyyy:
        return "datetime2";
      case DateTimeFormatType.DateTimedMMMMyyyy:
        return "datetime3";
      case DateTimeFormatType.DateTimeMMMMdyyyy:
        return "datetime4";
      case DateTimeFormatType.DateTimedMMMyy:
        return "datetime5";
      case DateTimeFormatType.DateTimeMMMMyy:
        return "datetime6";
      case DateTimeFormatType.DateTimeMMMyy:
        return "datetime7";
      case DateTimeFormatType.DateTimeMMddyyhmmAMPM:
        return "datetime8";
      case DateTimeFormatType.DateTimeMMddyyhmmssAMPM:
        return "datetime9";
      case DateTimeFormatType.DateTimeHmm:
        return "datetime10";
      case DateTimeFormatType.DateTimeHmmss:
        return "datetime11";
      case DateTimeFormatType.DateTimehmmAMPM:
        return "datetime12";
      case DateTimeFormatType.DateTimehmmssAMPM:
        return "datetime13";
      default:
        return string.Empty;
    }
  }

  internal static bool CheckPlaceholder(
    IPlaceholderFormat iLayoutPlaceholder,
    IPlaceholderFormat iCurrentPlaceholder)
  {
    Placeholder placeholder1 = (Placeholder) iLayoutPlaceholder;
    Placeholder placeholder2 = (Placeholder) iCurrentPlaceholder;
    if ((int) placeholder1.Index == (int) placeholder2.Index)
    {
      if (placeholder1.Index == 0U || placeholder2.Index == 0U)
        return placeholder1.GetPlaceholderType() == placeholder2.GetPlaceholderType() ? placeholder1.GetPlaceholderType() != (PlaceholderType) 0 && placeholder2.GetPlaceholderType() != (PlaceholderType) 0 : (placeholder1.GetPlaceholderType() == PlaceholderType.Title || placeholder1.GetPlaceholderType() == PlaceholderType.CenterTitle) && (placeholder2.GetPlaceholderType() == PlaceholderType.CenterTitle || placeholder2.GetPlaceholderType() == PlaceholderType.Title);
      if (placeholder1.GetPlaceholderType() != (PlaceholderType) 0 || placeholder2.GetPlaceholderType() != (PlaceholderType) 0)
      {
        switch (placeholder1.GetPlaceholderType())
        {
          case PlaceholderType.SlideNumber:
          case PlaceholderType.Header:
          case PlaceholderType.Footer:
          case PlaceholderType.Date:
            if (placeholder2.GetPlaceholderType() == (PlaceholderType) 0)
              return false;
            break;
        }
      }
      return true;
    }
    return placeholder1.GetPlaceholderType() == placeholder2.GetPlaceholderType() && placeholder1.GetPlaceholderType() != (PlaceholderType) 0 && placeholder2.GetPlaceholderType() != (PlaceholderType) 0 && (placeholder1.Index == 0U || placeholder2.Index == 0U);
  }

  internal static bool CheckPlaceholder(
    IPlaceholderFormat iLayoutPlaceholder,
    IPlaceholderFormat iCurrentPlaceholder,
    bool isMasterSlide)
  {
    Placeholder placeholder1 = (Placeholder) iLayoutPlaceholder;
    Placeholder placeholder2 = (Placeholder) iCurrentPlaceholder;
    if (!isMasterSlide)
      return false;
    if ((int) placeholder1.Index == (int) placeholder2.Index)
    {
      if (placeholder1.Index == 0U || placeholder2.Index == 0U)
        return placeholder1.GetPlaceholderType() == placeholder2.GetPlaceholderType() ? placeholder1.GetPlaceholderType() != (PlaceholderType) 0 && placeholder2.GetPlaceholderType() != (PlaceholderType) 0 : (placeholder1.GetPlaceholderType() == PlaceholderType.Title || placeholder1.GetPlaceholderType() == PlaceholderType.CenterTitle) && (placeholder2.GetPlaceholderType() == PlaceholderType.CenterTitle || placeholder2.GetPlaceholderType() == PlaceholderType.Title);
      if (placeholder2.GetBaseShape().BaseSlide is NotesSlide && placeholder1.GetPlaceholderType() != placeholder2.GetPlaceholderType())
        return false;
      if (placeholder1.GetPlaceholderType() != (PlaceholderType) 0 || placeholder2.GetPlaceholderType() != (PlaceholderType) 0)
      {
        switch (placeholder1.GetPlaceholderType())
        {
          case PlaceholderType.SlideNumber:
          case PlaceholderType.Header:
          case PlaceholderType.Footer:
          case PlaceholderType.Date:
            if (placeholder2.GetPlaceholderType() == (PlaceholderType) 0)
              return false;
            break;
        }
      }
      return true;
    }
    return placeholder1.GetPlaceholderType() == placeholder2.GetPlaceholderType() && placeholder1.GetPlaceholderType() != (PlaceholderType) 0 && placeholder2.GetPlaceholderType() != (PlaceholderType) 0;
  }

  internal static string GetColorName(int value)
  {
    return value.ToString("X8", (IFormatProvider) CultureInfo.InvariantCulture);
  }

  internal static bool IsEmu(double value) => value / 12700.0 > 1.0;

  internal static double GetAlphaModeEffect(int alpha) => (double) alpha / 100000.0;

  internal static double EmuToPoint(int emu) => Convert.ToDouble((double) emu / 12700.0);

  internal static double EmuToPoint(double emu) => Convert.ToDouble(emu / 12700.0);

  internal static double EmuToInch(int emu) => Convert.ToDouble((double) emu / 914400.0);

  internal static int InchToEmu(double inch) => Convert.ToInt32(inch * 914400.0);

  internal static string GenerateRelationId(RelationCollection topRelation)
  {
    string id = (string) null;
    for (int index = 1; index < int.MaxValue; ++index)
    {
      id = "rId" + (object) index;
      if (!topRelation.Contains(id) && !topRelation.GetImageRemoveList().Contains(id))
        break;
    }
    return id;
  }

  internal static string GenerateRelationTarget(Syncfusion.Presentation.Presentation presentation)
  {
    int num = presentation.Slides.Count + 1;
    string relationTarget = $"slides/slide{(object) num}.xml";
    List<Relation> relationList = presentation.TopRelation.GetRelationList();
    for (int index = 0; index < presentation.TopRelation.Count; ++index)
    {
      Relation relation = relationList[index];
      if (relationTarget == relation.Target)
      {
        ++num;
        relationTarget = $"slides/slide{(object) num}.xml";
        index = 0;
      }
    }
    return relationTarget;
  }

  internal static string GenerateRelationIdentifier(RelationCollection topRelation)
  {
    string relationIdentifier = (string) null;
    List<string> stringList = new List<string>();
    foreach (Relation relation in topRelation.GetRelationList())
      stringList.Add(relation.Id);
    foreach (string imageRemove in topRelation.GetImageRemoveList())
      stringList.Add(imageRemove);
    for (int index = 1; index < int.MaxValue; ++index)
    {
      relationIdentifier = "rId" + (object) index;
      if (!stringList.Contains(relationIdentifier))
        break;
    }
    stringList.Clear();
    return relationIdentifier;
  }

  internal static string GenerateLayoutRelationId(
    RelationCollection topRelation,
    List<string> imageList)
  {
    string id = (string) null;
    for (int index = 1; index < int.MaxValue; ++index)
    {
      id = "rId" + (object) index;
      if (!topRelation.Contains(id) && !imageList.Contains(id))
        break;
    }
    return id;
  }

  internal static VerticalAlignment GetVerticalAlignType(string value)
  {
    switch (value)
    {
      case "b":
        return VerticalAlignment.Bottom;
      case "ctr":
        return VerticalAlignment.Middle;
      case "dist":
        return VerticalAlignment.Distributed;
      case "just":
        return VerticalAlignment.Justify;
      case "t":
        return VerticalAlignment.Top;
      default:
        throw new ArgumentException("Invalid ChartTextAnchor string val");
    }
  }

  internal static ArrowheadLength GetArrowHeadLength(string value)
  {
    switch (value)
    {
      case "lg":
        return ArrowheadLength.Long;
      case "med":
        return ArrowheadLength.Medium;
      case "sm":
        return ArrowheadLength.Short;
      default:
        return ArrowheadLength.Medium;
    }
  }

  internal static OleObjectType ToOleType(string oleTypeStr)
  {
    oleTypeStr = oleTypeStr.TrimEnd(new char[1]);
    OleObjectType oleType = OleObjectType.Undefined;
    if (oleTypeStr.StartsWith("Acrobat Document") || oleTypeStr.StartsWith("AcroExch.Document.7") || oleTypeStr.StartsWith("AcroExch.Document"))
      oleType = OleObjectType.AdobeAcrobatDocument;
    else if (oleTypeStr.StartsWith("Package"))
      oleType = OleObjectType.Package;
    else if (oleTypeStr.StartsWith("PBrush"))
      oleType = OleObjectType.BitmapImage;
    else if (oleTypeStr.StartsWith("Media Clip") || oleTypeStr.StartsWith("MPlayer"))
      oleType = OleObjectType.MediaClip;
    else if (oleTypeStr.StartsWith("Microsoft Equation 3.0") || oleTypeStr.StartsWith("Equation.3"))
      oleType = OleObjectType.Equation;
    else if (oleTypeStr.StartsWith("Microsoft Graph Chart") || oleTypeStr.StartsWith("MSGraph.Chart.8"))
      oleType = OleObjectType.GraphChart;
    else if (oleTypeStr.Contains("Excel 2003 Worksheet") || oleTypeStr.StartsWith("Excel.Sheet.8"))
      oleType = OleObjectType.Excel_97_2003_Worksheet;
    else if (oleTypeStr.Contains("Excel Binary Worksheet") || oleTypeStr.StartsWith("Excel.SheetBinaryMacroEnabled.12"))
      oleType = OleObjectType.ExcelBinaryWorksheet;
    else if (oleTypeStr.Contains("Excel Chart") || oleTypeStr.StartsWith("Excel.Chart.8"))
      oleType = OleObjectType.ExcelChart;
    else if (oleTypeStr.Contains("Excel Worksheet (code)") || oleTypeStr.StartsWith("Excel.SheetMacroEnabled.12"))
      oleType = OleObjectType.ExcelMacroWorksheet;
    else if (oleTypeStr.Contains("Excel Worksheet") || oleTypeStr.StartsWith("Excel.Sheet.12"))
      oleType = OleObjectType.ExcelWorksheet;
    else if (oleTypeStr.Contains("PowerPoint 97-2003 Presentation") || oleTypeStr.StartsWith("PowerPoint.Show.8"))
      oleType = OleObjectType.PowerPoint_97_2003_Presentation;
    else if (oleTypeStr.Contains("PowerPoint 97-2003 Slide") || oleTypeStr.StartsWith("PowerPoint.Slide.8"))
      oleType = OleObjectType.PowerPoint_97_2003_Slide;
    else if (oleTypeStr.Contains("PowerPoint Macro-Enabled Presentation") || oleTypeStr.StartsWith("PowerPoint.ShowMacroEnabled.12"))
      oleType = OleObjectType.PowerPointMacroPresentation;
    else if (oleTypeStr.Contains("PowerPoint Macro-Enabled Slide") || oleTypeStr.StartsWith("PowerPoint.SlideMacroEnabled.12"))
      oleType = OleObjectType.PowerPointMacroSlide;
    else if (oleTypeStr.Contains("PowerPoint Presentation") || oleTypeStr.StartsWith("PowerPoint.Show.12"))
      oleType = OleObjectType.PowerPointPresentation;
    else if (oleTypeStr.Contains("PowerPoint Slide") || oleTypeStr.StartsWith("PowerPoint.Slide.12"))
      oleType = OleObjectType.PowerPointSlide;
    else if (oleTypeStr.Contains("Word 97-2003 Document") || oleTypeStr.StartsWith("Word.Document.8"))
      oleType = OleObjectType.Word_97_2003_Document;
    else if (oleTypeStr.Contains("Word Document") || oleTypeStr.StartsWith("Word.Document.12"))
      oleType = OleObjectType.WordDocument;
    else if (oleTypeStr.Contains("Word Macro-Enabled Document") || oleTypeStr.StartsWith("Word.DocumentMacroEnabled.12"))
      oleType = OleObjectType.WordMacroDocument;
    else if (oleTypeStr.StartsWith("Microsoft Visio Drawing") || oleTypeStr.StartsWith("Visio.Drawing.11"))
      oleType = OleObjectType.VisioDrawing;
    else if (oleTypeStr.StartsWith("OpenDocument Presentation") || oleTypeStr.StartsWith("PowerPoint.OpenDocumentPresentation.12"))
      oleType = OleObjectType.OpenDocumentPresentation;
    else if (oleTypeStr.StartsWith("OpenDocument Spreadsheet") || oleTypeStr.StartsWith("Excel.OpenDocumentSpreadsheet.12"))
      oleType = OleObjectType.OpenDocumentSpreadsheet;
    else if (oleTypeStr.StartsWith("opendocument.CalcDocument.1"))
      oleType = OleObjectType.OpenOfficeSpreadsheet;
    else if (oleTypeStr.StartsWith("opendocument.WriterDocument.1"))
      oleType = OleObjectType.OpenOfficeText;
    else if (oleTypeStr.StartsWith("soffice.StarCalcDocument.6"))
      oleType = OleObjectType.OpenOfficeSpreadsheet1_1;
    else if (oleTypeStr.StartsWith("soffice.StarWriterDocument.6"))
      oleType = OleObjectType.OpenOfficeText_1_1;
    else if (oleTypeStr.StartsWith("Video Clip") || oleTypeStr.StartsWith("AVIFile"))
      oleType = OleObjectType.VideoClip;
    else if (oleTypeStr.StartsWith("WaveSound") || oleTypeStr.StartsWith("SoundRec"))
      oleType = OleObjectType.WaveSound;
    else if (oleTypeStr.StartsWith("WordPad Document") || oleTypeStr.StartsWith("WordPad.Document.1"))
      oleType = OleObjectType.WordPadDocument;
    return oleType;
  }

  internal static string GetOleExtension(string oleType)
  {
    if (oleType != null && oleType.StartsWith("."))
    {
      string[] strArray = oleType.Split('.');
      if (strArray.Length == 2)
        return strArray[1];
    }
    switch (oleType)
    {
      case "Excel.Chart.8":
      case "Excel.Sheet.8":
        return "xls";
      case "Excel.Sheet.12":
        return "xlsx";
      case "Excel.SheetBinaryMacroEnabled.12":
        return "xlsb";
      case "Excel.SheetMacroEnabled.12":
        return "xlsm";
      case "PowerPoint.Show.8":
        return "ppt";
      case "PowerPoint.Show.12":
        return "pptx";
      case "Word.Document.8":
        return "doc";
      case "Word.Document.12":
        return "docx";
      case "Word.DocumentMacroEnabled.12":
        return "docm";
      case "PowerPoint.ShowMacroEnabled.12":
        return "pptm";
      case "PowerPoint.SlideMacroEnabled.12":
        return "sldm";
      case "PowerPoint.Slide.12":
        return "sldx";
      case "Visio.Drawing.11":
        return "vsd";
      case "Visio.Drawing.15":
        return "vsdx";
      case "Acrobat Document":
      case "AcroExch.Document.7":
        return "pdf";
      default:
        return "bin";
    }
  }

  internal static string ToString(OleObjectType oleType, bool isPowerPoint2003)
  {
    string str = string.Empty;
    switch (oleType)
    {
      case OleObjectType.AdobeAcrobatDocument:
        str = isPowerPoint2003 ? "Acrobat Document" : "AcroExch.Document.7";
        break;
      case OleObjectType.BitmapImage:
        str = "PBrush";
        break;
      case OleObjectType.MediaClip:
        str = isPowerPoint2003 ? "Media Clip" : "MPlayer";
        break;
      case OleObjectType.Equation:
        str = isPowerPoint2003 ? "Microsoft Equation 3.0" : "Equation.3";
        break;
      case OleObjectType.GraphChart:
        str = isPowerPoint2003 ? "Microsoft Graph Chart" : "MSGraph.Chart.8";
        break;
      case OleObjectType.Excel_97_2003_Worksheet:
        str = isPowerPoint2003 ? "Microsoft Office Excel 2003 Worksheet" : "Excel.Sheet.8";
        break;
      case OleObjectType.ExcelBinaryWorksheet:
        str = isPowerPoint2003 ? "Microsoft Office Excel Binary Worksheet" : "Excel.SheetBinaryMacroEnabled.12";
        break;
      case OleObjectType.ExcelChart:
        str = isPowerPoint2003 ? "Microsoft Office Excel Chart" : "Excel.Chart.8";
        break;
      case OleObjectType.ExcelMacroWorksheet:
        str = isPowerPoint2003 ? "Microsoft Office Excel Worksheet (code)" : "Excel.SheetMacroEnabled.12";
        break;
      case OleObjectType.ExcelWorksheet:
        str = isPowerPoint2003 ? "Microsoft Office Excel Worksheet" : "Excel.Sheet.12";
        break;
      case OleObjectType.PowerPoint_97_2003_Presentation:
        str = isPowerPoint2003 ? "Microsoft Office PowerPoint 97-2003 Presentation" : "PowerPoint.Show.8";
        break;
      case OleObjectType.PowerPoint_97_2003_Slide:
        str = isPowerPoint2003 ? "Microsoft Office PowerPoint 97-2003 Slide" : "PowerPoint.Slide.8";
        break;
      case OleObjectType.PowerPointMacroPresentation:
        str = isPowerPoint2003 ? "Microsoft Office PowerPoint Macro-Enabled Presentation" : "PowerPoint.ShowMacroEnabled.12";
        break;
      case OleObjectType.PowerPointMacroSlide:
        str = isPowerPoint2003 ? "Microsoft Office PowerPoint Macro-Enabled Slide" : "PowerPoint.SlideMacroEnabled.12";
        break;
      case OleObjectType.PowerPointPresentation:
        str = isPowerPoint2003 ? "Microsoft Office PowerPoint Presentation" : "PowerPoint.Show.12";
        break;
      case OleObjectType.PowerPointSlide:
        str = isPowerPoint2003 ? "Microsoft Office PowerPoint Slide" : "PowerPoint.Slide.12";
        break;
      case OleObjectType.Word_97_2003_Document:
        str = isPowerPoint2003 ? "Microsoft Office Word 97-2003 Document" : "Word.Document.8";
        break;
      case OleObjectType.WordDocument:
        str = isPowerPoint2003 ? "Microsoft Office Word Document" : "Word.Document.12";
        break;
      case OleObjectType.WordMacroDocument:
        str = isPowerPoint2003 ? "Microsoft Office Word Macro-Enabled Document" : "Word.DocumentMacroEnabled.12";
        break;
      case OleObjectType.VisioDrawing:
        str = isPowerPoint2003 ? "Microsoft Visio Drawing" : "Visio.Drawing.11";
        break;
      case OleObjectType.MIDISequence:
        str = "MIDI Sequence";
        break;
      case OleObjectType.OpenDocumentPresentation:
        str = isPowerPoint2003 ? "OpenDocument Presentation" : "PowerPoint.OpenDocumentPresentation.12";
        break;
      case OleObjectType.OpenDocumentSpreadsheet:
        str = isPowerPoint2003 ? "OpenDocument Spreadsheet" : "Excel.OpenDocumentSpreadsheet.12";
        break;
      case OleObjectType.OpenOfficeSpreadsheet1_1:
        str = "soffice.StarCalcDocument.6";
        break;
      case OleObjectType.OpenOfficeText_1_1:
        str = "soffice.StarWriterDocument.6";
        break;
      case OleObjectType.Package:
        str = "Package";
        break;
      case OleObjectType.VideoClip:
        str = isPowerPoint2003 ? "Video Clip" : "AVIFile";
        break;
      case OleObjectType.WaveSound:
        str = isPowerPoint2003 ? "WaveSound" : "SoundRec";
        break;
      case OleObjectType.WordPadDocument:
        str = isPowerPoint2003 ? "WordPad Document" : "WordPad.Document.1";
        break;
      case OleObjectType.OpenOfficeSpreadsheet:
        str = "opendocument.CalcDocument.1";
        break;
      case OleObjectType.OpenOfficeText:
        str = "opendocument.WriterDocument.1";
        break;
    }
    return str;
  }

  internal static ArrowheadStyle GetArrowHeadStyle(string value)
  {
    switch (value)
    {
      case "arrow":
        return ArrowheadStyle.ArrowOpen;
      case "diamond":
        return ArrowheadStyle.ArrowDiamond;
      case "none":
        return ArrowheadStyle.None;
      case "oval":
        return ArrowheadStyle.ArrowOval;
      case "stealth":
        return ArrowheadStyle.ArrowStealth;
      case "triangle":
        return ArrowheadStyle.Arrow;
      default:
        return ArrowheadStyle.None;
    }
  }

  internal static ArrowheadWidth GetArrowHeadWidth(string value)
  {
    switch (value)
    {
      case "lg":
        return ArrowheadWidth.Wide;
      case "med":
        return ArrowheadWidth.Medium;
      case "sm":
        return ArrowheadWidth.Narrow;
      default:
        return ArrowheadWidth.Medium;
    }
  }

  internal static BlackWhiteMode GetBlackWhiteMode(string value)
  {
    switch (value)
    {
      case "clr":
        return BlackWhiteMode.Clear;
      case "auto":
        return BlackWhiteMode.Auto;
      case "gray":
        return BlackWhiteMode.Gray;
      case "ltGray":
        return BlackWhiteMode.LightGray;
      case "invGray":
        return BlackWhiteMode.InverseGray;
      case "grayWhite":
        return BlackWhiteMode.GrayWhite;
      case "blackGray":
        return BlackWhiteMode.BlackGray;
      case "blackWhite":
        return BlackWhiteMode.BlackWhite;
      case "black":
        return BlackWhiteMode.Black;
      case "white":
        return BlackWhiteMode.White;
      case "hidden":
        return BlackWhiteMode.Hidden;
      default:
        return BlackWhiteMode.White;
    }
  }

  internal static LineCapStyle GetCapStyle(string lineCapStyle)
  {
    switch (lineCapStyle)
    {
      case "rnd":
        return LineCapStyle.Round;
      case "sq":
        return LineCapStyle.Square;
      default:
        return LineCapStyle.Flat;
    }
  }

  internal static IColor GetColor(string value)
  {
    return ColorObject.FromArgb(int.Parse(value, NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture));
  }

  internal static IColor GetColorFromName(string colorName)
  {
    string key = colorName;
    if (key != null)
    {
      if (Helper._presetColors == null)
      {
        Helper._presetColors = new Dictionary<string, int>(140);
        Helper._presetColors.Add("aliceBlue", 0);
        Helper._presetColors.Add("antiqueWhite", 1);
        Helper._presetColors.Add("aqua", 2);
        Helper._presetColors.Add("aquamarine", 3);
        Helper._presetColors.Add("azure", 4);
        Helper._presetColors.Add("beige", 5);
        Helper._presetColors.Add("bisque", 6);
        Helper._presetColors.Add("black", 7);
        Helper._presetColors.Add("blanchedAlmond", 8);
        Helper._presetColors.Add("blue", 9);
        Helper._presetColors.Add("blueViolet", 10);
        Helper._presetColors.Add("brown", 11);
        Helper._presetColors.Add("burlyWood", 12);
        Helper._presetColors.Add("cadetBlue", 13);
        Helper._presetColors.Add("chartreuse", 14);
        Helper._presetColors.Add("chocolate", 15);
        Helper._presetColors.Add("coral", 16 /*0x10*/);
        Helper._presetColors.Add("cornflowerBlue", 17);
        Helper._presetColors.Add("cornsilk", 18);
        Helper._presetColors.Add("crimson", 19);
        Helper._presetColors.Add("cyan", 20);
        Helper._presetColors.Add("deepPink", 21);
        Helper._presetColors.Add("deepSkyBlue", 22);
        Helper._presetColors.Add("dimGray", 23);
        Helper._presetColors.Add("dkBlue", 24);
        Helper._presetColors.Add("dkCyan", 25);
        Helper._presetColors.Add("dkGoldenrod", 26);
        Helper._presetColors.Add("dkGray", 27);
        Helper._presetColors.Add("dkGreen", 28);
        Helper._presetColors.Add("dkKhaki", 29);
        Helper._presetColors.Add("dkMagenta", 30);
        Helper._presetColors.Add("dkOliveGreen", 31 /*0x1F*/);
        Helper._presetColors.Add("dkOrange", 32 /*0x20*/);
        Helper._presetColors.Add("dkOrchid", 33);
        Helper._presetColors.Add("dkRed", 34);
        Helper._presetColors.Add("dkSalmon", 35);
        Helper._presetColors.Add("dkSeaGreen", 36);
        Helper._presetColors.Add("dkSlateBlue", 37);
        Helper._presetColors.Add("dkSlateGray", 38);
        Helper._presetColors.Add("dkTurquoise", 39);
        Helper._presetColors.Add("dkViolet", 40);
        Helper._presetColors.Add("dodgerBlue", 41);
        Helper._presetColors.Add("firebrick", 42);
        Helper._presetColors.Add("floralWhite", 43);
        Helper._presetColors.Add("forestGreen", 44);
        Helper._presetColors.Add("fuchsia", 45);
        Helper._presetColors.Add("gainsboro", 46);
        Helper._presetColors.Add("ghostWhite", 47);
        Helper._presetColors.Add("gold", 48 /*0x30*/);
        Helper._presetColors.Add("goldenrod", 49);
        Helper._presetColors.Add("gray", 50);
        Helper._presetColors.Add("green", 51);
        Helper._presetColors.Add("greenYellow", 52);
        Helper._presetColors.Add("honeydew", 53);
        Helper._presetColors.Add("hotPink", 54);
        Helper._presetColors.Add("indianRed", 55);
        Helper._presetColors.Add("indigo", 56);
        Helper._presetColors.Add("ivory", 57);
        Helper._presetColors.Add("khaki", 58);
        Helper._presetColors.Add("lavender", 59);
        Helper._presetColors.Add("lavenderBlush", 60);
        Helper._presetColors.Add("lawnGreen", 61);
        Helper._presetColors.Add("lemonChiffon", 62);
        Helper._presetColors.Add("lime", 63 /*0x3F*/);
        Helper._presetColors.Add("limeGreen", 64 /*0x40*/);
        Helper._presetColors.Add("linen", 65);
        Helper._presetColors.Add("ltBlue", 66);
        Helper._presetColors.Add("ltCoral", 67);
        Helper._presetColors.Add("ltCyan", 68);
        Helper._presetColors.Add("ltGoldenrodYellow", 69);
        Helper._presetColors.Add("ltGray", 70);
        Helper._presetColors.Add("ltGreen", 71);
        Helper._presetColors.Add("ltPink", 72);
        Helper._presetColors.Add("ltSalmon", 73);
        Helper._presetColors.Add("ltSeaGreen", 74);
        Helper._presetColors.Add("ltSkyBlue", 75);
        Helper._presetColors.Add("ltSlateGray", 76);
        Helper._presetColors.Add("ltSteelBlue", 77);
        Helper._presetColors.Add("ltYellow", 78);
        Helper._presetColors.Add("magenta", 79);
        Helper._presetColors.Add("maroon", 80 /*0x50*/);
        Helper._presetColors.Add("medAquamarine", 81);
        Helper._presetColors.Add("medBlue", 82);
        Helper._presetColors.Add("medOrchid", 83);
        Helper._presetColors.Add("medPurple", 84);
        Helper._presetColors.Add("medSeaGreen", 85);
        Helper._presetColors.Add("medSlateBlue", 86);
        Helper._presetColors.Add("medSpringGreen", 87);
        Helper._presetColors.Add("medTurquoise", 88);
        Helper._presetColors.Add("medVioletRed", 89);
        Helper._presetColors.Add("midnightBlue", 90);
        Helper._presetColors.Add("mintCream", 91);
        Helper._presetColors.Add("mistyRose", 92);
        Helper._presetColors.Add("moccasin", 93);
        Helper._presetColors.Add("navajoWhite", 94);
        Helper._presetColors.Add("navy", 95);
        Helper._presetColors.Add("oldLace", 96 /*0x60*/);
        Helper._presetColors.Add("olive", 97);
        Helper._presetColors.Add("oliveDrab", 98);
        Helper._presetColors.Add("orange", 99);
        Helper._presetColors.Add("orangeRed", 100);
        Helper._presetColors.Add("orchid", 101);
        Helper._presetColors.Add("paleGoldenrod", 102);
        Helper._presetColors.Add("paleGreen", 103);
        Helper._presetColors.Add("paleTurquoise", 104);
        Helper._presetColors.Add("paleVioletRed", 105);
        Helper._presetColors.Add("papayaWhip", 106);
        Helper._presetColors.Add("peachPuff", 107);
        Helper._presetColors.Add("peru", 108);
        Helper._presetColors.Add("pink", 109);
        Helper._presetColors.Add("plum", 110);
        Helper._presetColors.Add("powderBlue", 111);
        Helper._presetColors.Add("purple", 112 /*0x70*/);
        Helper._presetColors.Add("red", 113);
        Helper._presetColors.Add("rosyBrown", 114);
        Helper._presetColors.Add("royalBlue", 115);
        Helper._presetColors.Add("saddleBrown", 116);
        Helper._presetColors.Add("salmon", 117);
        Helper._presetColors.Add("sandyBrown", 118);
        Helper._presetColors.Add("seaGreen", 119);
        Helper._presetColors.Add("seaShell", 120);
        Helper._presetColors.Add("sienna", 121);
        Helper._presetColors.Add("silver", 122);
        Helper._presetColors.Add("skyBlue", 123);
        Helper._presetColors.Add("slateBlue", 124);
        Helper._presetColors.Add("slateGray", 125);
        Helper._presetColors.Add("snow", 126);
        Helper._presetColors.Add("springGreen", (int) sbyte.MaxValue);
        Helper._presetColors.Add("steelBlue", 128 /*0x80*/);
        Helper._presetColors.Add("tan", 129);
        Helper._presetColors.Add("teal", 130);
        Helper._presetColors.Add("thistle", 131);
        Helper._presetColors.Add("tomato", 132);
        Helper._presetColors.Add("turquoise", 133);
        Helper._presetColors.Add("violet", 134);
        Helper._presetColors.Add("wheat", 135);
        Helper._presetColors.Add("white", 136);
        Helper._presetColors.Add("whiteSmoke", 137);
        Helper._presetColors.Add("yellow", 138);
        Helper._presetColors.Add("yellowGreen", 139);
      }
      if (!Helper._presetColors.ContainsKey(key))
        return ColorObject.FromArgb((int) byte.MaxValue, 0, 0, 0);
      switch (Helper._presetColors[key])
      {
        case 0:
          return ColorObject.FromArgb((int) byte.MaxValue, 240 /*0xF0*/, 248, (int) byte.MaxValue);
        case 1:
          return ColorObject.FromArgb((int) byte.MaxValue, 250, 235, 215);
        case 2:
          return ColorObject.FromArgb((int) byte.MaxValue, 0, (int) byte.MaxValue, (int) byte.MaxValue);
        case 3:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) sbyte.MaxValue, (int) byte.MaxValue, 212);
        case 4:
          return ColorObject.FromArgb((int) byte.MaxValue, 240 /*0xF0*/, (int) byte.MaxValue, (int) byte.MaxValue);
        case 5:
          return ColorObject.FromArgb((int) byte.MaxValue, 245, 245, 220);
        case 6:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 228, 196);
        case 7:
          return ColorObject.FromArgb((int) byte.MaxValue, 0, 0, 0);
        case 8:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 235, 205);
        case 9:
          return ColorObject.FromArgb((int) byte.MaxValue, 0, 0, (int) byte.MaxValue);
        case 10:
          return ColorObject.FromArgb((int) byte.MaxValue, 138, 43, 226);
        case 11:
          return ColorObject.FromArgb((int) byte.MaxValue, 165, 42, 42);
        case 12:
          return ColorObject.FromArgb((int) byte.MaxValue, 222, 184, 135);
        case 13:
          return ColorObject.FromArgb((int) byte.MaxValue, 95, 158, 160 /*0xA0*/);
        case 14:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) sbyte.MaxValue, (int) byte.MaxValue, 0);
        case 15:
          return ColorObject.FromArgb((int) byte.MaxValue, 210, 105, 30);
        case 16 /*0x10*/:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) sbyte.MaxValue, 80 /*0x50*/);
        case 17:
          return ColorObject.FromArgb((int) byte.MaxValue, 100, 149, 237);
        case 18:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 248, 220);
        case 19:
          return ColorObject.FromArgb((int) byte.MaxValue, 220, 20, 60);
        case 20:
          return ColorObject.FromArgb((int) byte.MaxValue, 0, (int) byte.MaxValue, (int) byte.MaxValue);
        case 21:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 20, 147);
        case 22:
          return ColorObject.FromArgb((int) byte.MaxValue, 0, 191, (int) byte.MaxValue);
        case 23:
          return ColorObject.FromArgb((int) byte.MaxValue, 105, 105, 105);
        case 24:
          return ColorObject.FromArgb((int) byte.MaxValue, 0, 0, 139);
        case 25:
          return ColorObject.FromArgb((int) byte.MaxValue, 0, 139, 139);
        case 26:
          return ColorObject.FromArgb((int) byte.MaxValue, 184, 134, 11);
        case 27:
          return ColorObject.FromArgb((int) byte.MaxValue, 169, 169, 169);
        case 28:
          return ColorObject.FromArgb((int) byte.MaxValue, 0, 100, 0);
        case 29:
          return ColorObject.FromArgb((int) byte.MaxValue, 189, 183, 107);
        case 30:
          return ColorObject.FromArgb((int) byte.MaxValue, 139, 0, 139);
        case 31 /*0x1F*/:
          return ColorObject.FromArgb((int) byte.MaxValue, 85, 107, 47);
        case 32 /*0x20*/:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 140, 0);
        case 33:
          return ColorObject.FromArgb((int) byte.MaxValue, 153, 50, 204);
        case 34:
          return ColorObject.FromArgb((int) byte.MaxValue, 139, 0, 0);
        case 35:
          return ColorObject.FromArgb((int) byte.MaxValue, 233, 150, 122);
        case 36:
          return ColorObject.FromArgb((int) byte.MaxValue, 143, 188, 139);
        case 37:
          return ColorObject.FromArgb((int) byte.MaxValue, 72, 61, 139);
        case 38:
          return ColorObject.FromArgb((int) byte.MaxValue, 47, 79, 79);
        case 39:
          return ColorObject.FromArgb((int) byte.MaxValue, 0, 206, 209);
        case 40:
          return ColorObject.FromArgb((int) byte.MaxValue, 148, 0, 211);
        case 41:
          return ColorObject.FromArgb((int) byte.MaxValue, 30, 144 /*0x90*/, (int) byte.MaxValue);
        case 42:
          return ColorObject.FromArgb((int) byte.MaxValue, 178, 34, 34);
        case 43:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 250, 240 /*0xF0*/);
        case 44:
          return ColorObject.FromArgb((int) byte.MaxValue, 34, 139, 34);
        case 45:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 0, (int) byte.MaxValue);
        case 46:
          return ColorObject.FromArgb((int) byte.MaxValue, 220, 220, 220);
        case 47:
          return ColorObject.FromArgb((int) byte.MaxValue, 248, 248, (int) byte.MaxValue);
        case 48 /*0x30*/:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 215, 0);
        case 49:
          return ColorObject.FromArgb((int) byte.MaxValue, 218, 165, 32 /*0x20*/);
        case 50:
          return ColorObject.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
        case 51:
          return ColorObject.FromArgb((int) byte.MaxValue, 0, 128 /*0x80*/, 0);
        case 52:
          return ColorObject.FromArgb((int) byte.MaxValue, 173, (int) byte.MaxValue, 47);
        case 53:
          return ColorObject.FromArgb((int) byte.MaxValue, 240 /*0xF0*/, (int) byte.MaxValue, 240 /*0xF0*/);
        case 54:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 105, 180);
        case 55:
          return ColorObject.FromArgb((int) byte.MaxValue, 205, 92, 92);
        case 56:
          return ColorObject.FromArgb((int) byte.MaxValue, 75, 0, 130);
        case 57:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 240 /*0xF0*/);
        case 58:
          return ColorObject.FromArgb((int) byte.MaxValue, 240 /*0xF0*/, 230, 140);
        case 59:
          return ColorObject.FromArgb((int) byte.MaxValue, 230, 230, 250);
        case 60:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 240 /*0xF0*/, 245);
        case 61:
          return ColorObject.FromArgb((int) byte.MaxValue, 124, 252, 0);
        case 62:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 250, 205);
        case 63 /*0x3F*/:
          return ColorObject.FromArgb((int) byte.MaxValue, 0, (int) byte.MaxValue, 0);
        case 64 /*0x40*/:
          return ColorObject.FromArgb((int) byte.MaxValue, 50, 205, 50);
        case 65:
          return ColorObject.FromArgb((int) byte.MaxValue, 250, 240 /*0xF0*/, 230);
        case 66:
          return ColorObject.FromArgb((int) byte.MaxValue, 173, 216, 230);
        case 67:
          return ColorObject.FromArgb((int) byte.MaxValue, 240 /*0xF0*/, 128 /*0x80*/, 128 /*0x80*/);
        case 68:
          return ColorObject.FromArgb((int) byte.MaxValue, 224 /*0xE0*/, (int) byte.MaxValue, (int) byte.MaxValue);
        case 69:
          return ColorObject.FromArgb((int) byte.MaxValue, 250, 250, 120);
        case 70:
          return ColorObject.FromArgb((int) byte.MaxValue, 211, 211, 211);
        case 71:
          return ColorObject.FromArgb((int) byte.MaxValue, 144 /*0x90*/, 238, 144 /*0x90*/);
        case 72:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 182, 193);
        case 73:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 160 /*0xA0*/, 122);
        case 74:
          return ColorObject.FromArgb((int) byte.MaxValue, 32 /*0x20*/, 178, 170);
        case 75:
          return ColorObject.FromArgb((int) byte.MaxValue, 135, 206, 250);
        case 76:
          return ColorObject.FromArgb((int) byte.MaxValue, 119, 136, 153);
        case 77:
          return ColorObject.FromArgb((int) byte.MaxValue, 176 /*0xB0*/, 196, 222);
        case 78:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 224 /*0xE0*/);
        case 79:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 0, (int) byte.MaxValue);
        case 80 /*0x50*/:
          return ColorObject.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 0, 0);
        case 81:
          return ColorObject.FromArgb((int) byte.MaxValue, 102, 205, 170);
        case 82:
          return ColorObject.FromArgb((int) byte.MaxValue, 0, 0, 205);
        case 83:
          return ColorObject.FromArgb((int) byte.MaxValue, 186, 85, 211);
        case 84:
          return ColorObject.FromArgb((int) byte.MaxValue, 147, 112 /*0x70*/, 219);
        case 85:
          return ColorObject.FromArgb((int) byte.MaxValue, 60, 179, 113);
        case 86:
          return ColorObject.FromArgb((int) byte.MaxValue, 123, 104, 238);
        case 87:
          return ColorObject.FromArgb((int) byte.MaxValue, 0, 250, 154);
        case 88:
          return ColorObject.FromArgb((int) byte.MaxValue, 72, 209, 204);
        case 89:
          return ColorObject.FromArgb((int) byte.MaxValue, 199, 21, 133);
        case 90:
          return ColorObject.FromArgb((int) byte.MaxValue, 25, 25, 112 /*0x70*/);
        case 91:
          return ColorObject.FromArgb((int) byte.MaxValue, 245, (int) byte.MaxValue, 250);
        case 92:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 228, 225);
        case 93:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 228, 181);
        case 94:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 222, 173);
        case 95:
          return ColorObject.FromArgb((int) byte.MaxValue, 0, 0, 128 /*0x80*/);
        case 96 /*0x60*/:
          return ColorObject.FromArgb((int) byte.MaxValue, 253, 245, 230);
        case 97:
          return ColorObject.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 0);
        case 98:
          return ColorObject.FromArgb((int) byte.MaxValue, 107, 142, 35);
        case 99:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 165, 0);
        case 100:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 69, 0);
        case 101:
          return ColorObject.FromArgb((int) byte.MaxValue, 218, 112 /*0x70*/, 214);
        case 102:
          return ColorObject.FromArgb((int) byte.MaxValue, 238, 232, 170);
        case 103:
          return ColorObject.FromArgb((int) byte.MaxValue, 152, 251, 152);
        case 104:
          return ColorObject.FromArgb((int) byte.MaxValue, 175, 238, 238);
        case 105:
          return ColorObject.FromArgb((int) byte.MaxValue, 219, 112 /*0x70*/, 147);
        case 106:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 239, 213);
        case 107:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 218, 185);
        case 108:
          return ColorObject.FromArgb((int) byte.MaxValue, 205, 133, 63 /*0x3F*/);
        case 109:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192 /*0xC0*/, 203);
        case 110:
          return ColorObject.FromArgb((int) byte.MaxValue, 221, 160 /*0xA0*/, 221);
        case 111:
          return ColorObject.FromArgb((int) byte.MaxValue, 176 /*0xB0*/, 224 /*0xE0*/, 230);
        case 112 /*0x70*/:
          return ColorObject.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 0, 128 /*0x80*/);
        case 113:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 0, 0);
        case 114:
          return ColorObject.FromArgb((int) byte.MaxValue, 188, 143, 143);
        case 115:
          return ColorObject.FromArgb((int) byte.MaxValue, 65, 105, 225);
        case 116:
          return ColorObject.FromArgb((int) byte.MaxValue, 139, 69, 19);
        case 117:
          return ColorObject.FromArgb((int) byte.MaxValue, 250, 128 /*0x80*/, 114);
        case 118:
          return ColorObject.FromArgb((int) byte.MaxValue, 244, 164, 96 /*0x60*/);
        case 119:
          return ColorObject.FromArgb((int) byte.MaxValue, 46, 139, 87);
        case 120:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 245, 238);
        case 121:
          return ColorObject.FromArgb((int) byte.MaxValue, 160 /*0xA0*/, 82, 45);
        case 122:
          return ColorObject.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 192 /*0xC0*/, 192 /*0xC0*/);
        case 123:
          return ColorObject.FromArgb((int) byte.MaxValue, 135, 206, 235);
        case 124:
          return ColorObject.FromArgb((int) byte.MaxValue, 106, 90, 205);
        case 125:
          return ColorObject.FromArgb((int) byte.MaxValue, 112 /*0x70*/, 128 /*0x80*/, 144 /*0x90*/);
        case 126:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 250, 250);
        case (int) sbyte.MaxValue:
          return ColorObject.FromArgb((int) byte.MaxValue, 0, (int) byte.MaxValue, (int) sbyte.MaxValue);
        case 128 /*0x80*/:
          return ColorObject.FromArgb((int) byte.MaxValue, 70, 130, 180);
        case 129:
          return ColorObject.FromArgb((int) byte.MaxValue, 210, 180, 140);
        case 130:
          return ColorObject.FromArgb((int) byte.MaxValue, 0, 128 /*0x80*/, 128 /*0x80*/);
        case 131:
          return ColorObject.FromArgb((int) byte.MaxValue, 216, 191, 216);
        case 132:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 99, 71);
        case 133:
          return ColorObject.FromArgb((int) byte.MaxValue, 64 /*0x40*/, 224 /*0xE0*/, 208 /*0xD0*/);
        case 134:
          return ColorObject.FromArgb((int) byte.MaxValue, 238, 130, 238);
        case 135:
          return ColorObject.FromArgb((int) byte.MaxValue, 245, 222, 179);
        case 136:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
        case 137:
          return ColorObject.FromArgb((int) byte.MaxValue, 245, 245, 245);
        case 138:
          return ColorObject.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 0);
        case 139:
          return ColorObject.FromArgb((int) byte.MaxValue, 154, 205, 50);
      }
    }
    return ColorObject.FromArgb(0, 0, 0, 0);
  }

  internal static string GetColorName(IColor color)
  {
    return Helper.GetColorName(color.ToArgb()).Substring(2);
  }

  internal static LineStyle GetCompoundLineStyle(string compoundLineStyle)
  {
    switch (compoundLineStyle)
    {
      case "dbl":
        return LineStyle.ThinThin;
      case "thickThin":
        return LineStyle.ThickThin;
      case "thinThick":
        return LineStyle.ThinThick;
      case "tri":
        return LineStyle.ThickBetweenThin;
      default:
        return LineStyle.Single;
    }
  }

  internal static string GetDefaultString(string value, MasterSlide masterSlide)
  {
    if (masterSlide == null || masterSlide.ColorMap.Count <= 0 || !(value != "phClr"))
      return value;
    foreach (KeyValuePair<string, string> color in masterSlide.ColorMap)
    {
      if (value == color.Value)
        return color.Key;
    }
    return value;
  }

  internal static string GetDefaultString(string value, LayoutSlide layoutSlide)
  {
    if (layoutSlide == null || layoutSlide.ColorMap.Count <= 0 || !(value != "phClr"))
      return value;
    foreach (KeyValuePair<string, string> color in layoutSlide.ColorMap)
    {
      if (value == color.Value)
        return color.Key;
    }
    return value;
  }

  internal static PatternFillType GetFillPattern(string fillPattern)
  {
    switch (fillPattern)
    {
      case "cross":
        return PatternFillType.Cross;
      case "diagCross":
        return PatternFillType.DiagonalCross;
      case "dnDiag":
        return PatternFillType.DownwardDiagonal;
      case "horz":
        return PatternFillType.Horizontal;
      case "upDiag":
        return PatternFillType.UpwardDiagonal;
      case "vert":
        return PatternFillType.Vertical;
      case "dashDnDiag":
        return PatternFillType.DashedDownwardDiagonal;
      case "dashHorz":
        return PatternFillType.DashedHorizontal;
      case "dashUpDiag":
        return PatternFillType.DashedUpwardDiagonal;
      case "dashVert":
        return PatternFillType.DashedVertical;
      case "diagBrick":
        return PatternFillType.DiagonalBrick;
      case "divot":
        return PatternFillType.Divot;
      case "dkDnDiag":
        return PatternFillType.DarkDownwardDiagonal;
      case "dkHorz":
        return PatternFillType.DarkHorizontal;
      case "dkUpDiag":
        return PatternFillType.DarkUpwardDiagonal;
      case "dkVert":
        return PatternFillType.DarkVertical;
      case "dotDmnd":
        return PatternFillType.DottedDiamond;
      case "dotGrid":
        return PatternFillType.DottedGrid;
      case "horzBrick":
        return PatternFillType.HorizontalBrick;
      case "lgCheck":
        return PatternFillType.LargeCheckerBoard;
      case "lgConfetti":
        return PatternFillType.LargeConfetti;
      case "lgGrid":
        return PatternFillType.LargeGrid;
      case "ltDnDiag":
        return PatternFillType.LightDownwardDiagonal;
      case "ltHorz":
        return PatternFillType.LightHorizontal;
      case "ltUpDiag":
        return PatternFillType.LightUpwardDiagonal;
      case "ltVert":
        return PatternFillType.LightVertical;
      case "narHorz":
        return PatternFillType.NarrowHorizontal;
      case "narVert":
        return PatternFillType.NarrowVertical;
      case "pct10":
        return PatternFillType.Gray10;
      case "pct20":
        return PatternFillType.Gray20;
      case "pct25":
        return PatternFillType.Gray25;
      case "pct30":
        return PatternFillType.Gray30;
      case "pct40":
        return PatternFillType.Gray40;
      case "pct5":
        return PatternFillType.Gray5;
      case "pct50":
        return PatternFillType.Gray50;
      case "pct60":
        return PatternFillType.Gray60;
      case "pct70":
        return PatternFillType.Gray70;
      case "pct75":
        return PatternFillType.Gray75;
      case "pct80":
        return PatternFillType.Gray80;
      case "pct90":
        return PatternFillType.Gray90;
      case "plaid":
        return PatternFillType.Plaid;
      case "shingle":
        return PatternFillType.Shingle;
      case "smCheck":
        return PatternFillType.SmallCheckerBoard;
      case "smConfetti":
        return PatternFillType.SmallConfetti;
      case "smGrid":
        return PatternFillType.SmallGrid;
      case "solidDmnd":
        return PatternFillType.SolidDiamond;
      case "sphere":
        return PatternFillType.Sphere;
      case "trellis":
        return PatternFillType.Trellis;
      case "wave":
        return PatternFillType.Wave;
      case "wdDnDiag":
        return PatternFillType.WideDownwardDiagonal;
      case "wdUpDiag":
        return PatternFillType.WideUpwardDiagonal;
      case "weave":
        return PatternFillType.Weave;
      case "zigZag":
        return PatternFillType.ZigZag;
      case "openDmnd":
        return PatternFillType.OutlinedDiamond;
      default:
        return PatternFillType.Gray5;
    }
  }

  internal static TextUnderline GetFontUnderlineType(string value)
  {
    switch (value)
    {
      case "none":
        return TextUnderline.None;
      case "dbl":
        return TextUnderline.Double;
      case "sng":
        return TextUnderline.Single;
      case "heavy":
        return TextUnderline.Heavy;
      case "dotted":
        return TextUnderline.Dotted;
      case "dottedHeavy":
        return TextUnderline.DottedHeavy;
      case "dash":
        return TextUnderline.Dash;
      case "dashHeavy":
        return TextUnderline.DashedHeavy;
      case "dashLong":
        return TextUnderline.DashLong;
      case "dashLongHeavy":
        return TextUnderline.DashLongHeavy;
      case "dotDash":
        return TextUnderline.DotDash;
      case "dotDashHeavy":
        return TextUnderline.DashDotHeavy;
      case "dotDotDash":
        return TextUnderline.DotDotDash;
      case "dotDotDashHeavy":
        return TextUnderline.DashDotDotHeavy;
      case "wavy":
        return TextUnderline.Wave;
      case "wavyHeavy":
        return TextUnderline.WavyHeavy;
      case "wavyDbl":
        return TextUnderline.WavyDouble;
      default:
        return TextUnderline.Single;
    }
  }

  internal static SlideLayoutType GetSlideLayoutType(string type)
  {
    switch (type)
    {
      case "title":
        return SlideLayoutType.Title;
      case "blank":
        return SlideLayoutType.Blank;
      case "obj":
        return SlideLayoutType.TitleAndContent;
      case "secHead":
        return SlideLayoutType.SectionHeader;
      case "twoObj":
        return SlideLayoutType.TwoContent;
      case "twoTxTwoObj":
        return SlideLayoutType.Comparison;
      case "titleOnly":
        return SlideLayoutType.TitleOnly;
      case "objTx":
        return SlideLayoutType.ContentWithCaption;
      case "picTx":
        return SlideLayoutType.PictureWithCaption;
      case "vertTitleAndTx":
        return SlideLayoutType.VerticalTitleAndText;
      case "vertTx":
        return SlideLayoutType.TitleAndVerticalText;
      default:
        return SlideLayoutType.Custom;
    }
  }

  internal static LineCapStyle GetLineCap(string value)
  {
    switch (value)
    {
      case "rnd":
        return LineCapStyle.Round;
      case "sq":
        return LineCapStyle.Square;
      default:
        return LineCapStyle.Flat;
    }
  }

  internal static LineDashStyle GetLineDashStyle(string value)
  {
    switch (value)
    {
      case "sysDot":
        return LineDashStyle.RoundDot;
      case "sysDash":
        return LineDashStyle.SquareDot;
      case "dash":
        return LineDashStyle.Dash;
      case "dashDot":
        return LineDashStyle.DashDot;
      case "lgDash":
        return LineDashStyle.DashLongDash;
      case "lgDashDot":
        return LineDashStyle.DashLongDashDot;
      case "lgDashDotDot":
        return LineDashStyle.DashDotDot;
      case "solid":
        return LineDashStyle.Solid;
      case "sysDashDot":
        return LineDashStyle.SystemDashDot;
      case "sysDashDotDot":
        return LineDashStyle.SystemDashDot;
      case "dot":
        return LineDashStyle.Dot;
      default:
        throw new ArgumentException("Invalid LineDashStyle string val");
    }
  }

  internal static LineStyle GetLineStyle(string value)
  {
    switch (value)
    {
      case "dbl":
        return LineStyle.ThinThin;
      case "thickThin":
        return LineStyle.ThickThin;
      case "thinThick":
        return LineStyle.ThinThick;
      case "tri":
        return LineStyle.ThickBetweenThin;
      case "sng":
        return LineStyle.Single;
      default:
        return LineStyle.Single;
    }
  }

  internal static MirrorType GetMirrorType(string value)
  {
    switch (value)
    {
      case "x":
        return MirrorType.Horizonal;
      case "y":
        return MirrorType.Vertical;
      case "xy":
        return MirrorType.Both;
      default:
        return MirrorType.None;
    }
  }

  internal static string GetName(PlaceholderType placeHolderType)
  {
    switch (placeHolderType)
    {
      case PlaceholderType.Title:
        return "Title ";
      case PlaceholderType.Body:
        return "Text Placeholder ";
      case PlaceholderType.CenterTitle:
        return "Title ";
      case PlaceholderType.Subtitle:
        return "Subtitle ";
      case PlaceholderType.Object:
        return "Content Placeholder ";
      case PlaceholderType.Chart:
        return "Chart Placeholder ";
      case PlaceholderType.Bitmap:
        return "Online Image Placeholder ";
      case PlaceholderType.MediaClip:
        return "Media Placeholder ";
      case PlaceholderType.OrganizationChart:
        return "SmartArt Placeholder ";
      case PlaceholderType.Table:
        return "Table Placeholder ";
      case PlaceholderType.SlideNumber:
        return "Slide Number Placeholder ";
      case PlaceholderType.Footer:
        return "Footer Placeholder ";
      case PlaceholderType.Date:
        return "Date Placeholder ";
      default:
        return (string) null;
    }
  }

  internal static bool ComparePlaceholderType(
    List<PlaceholderType> sourcePlaceholderTypes,
    List<PlaceholderType> destPlaceholderTypes)
  {
    if (sourcePlaceholderTypes.Count != destPlaceholderTypes.Count)
      return false;
    sourcePlaceholderTypes.Sort();
    destPlaceholderTypes.Sort();
    for (int index = 0; index < sourcePlaceholderTypes.Count; ++index)
    {
      if (sourcePlaceholderTypes[index] != destPlaceholderTypes[index])
        return false;
    }
    return true;
  }

  internal static NumberedListStyle GetNumberStyle(string value)
  {
    switch (value)
    {
      case "alphaLcParenBoth":
        return NumberedListStyle.AlphaLcParenBoth;
      case "alphaUcParenBoth":
        return NumberedListStyle.AlphaUcParenBoth;
      case "alphaLcParenR":
        return NumberedListStyle.AlphaLcParenRight;
      case "alphaUcParenR":
        return NumberedListStyle.AlphaUcParenRight;
      case "alphaLcPeriod":
        return NumberedListStyle.AlphaLcPeriod;
      case "alphaUcPeriod":
        return NumberedListStyle.AlphaUcPeriod;
      case "arabicParenBoth":
        return NumberedListStyle.ArabicParenBoth;
      case "arabicParenR":
        return NumberedListStyle.ArabicParenRight;
      case "arabicPeriod":
        return NumberedListStyle.ArabicPeriod;
      case "arabicPlain":
        return NumberedListStyle.ArabicPlain;
      case "romanLcParenBoth":
        return NumberedListStyle.RomanLcParenBoth;
      case "romanUcParenBoth":
        return NumberedListStyle.RomanUcParenBoth;
      case "romanLcParenR":
        return NumberedListStyle.RomanLcParenRight;
      case "romanUcParenR":
        return NumberedListStyle.RomanUcParenRight;
      case "romanLcPeriod":
        return NumberedListStyle.RomanLcPeriod;
      case "romanUcPeriod":
        return NumberedListStyle.RomanUcPeriod;
      case "circleNumDbPlain":
        return NumberedListStyle.CircleNumDbPlain;
      case "circleNumWdBlackPlain":
        return NumberedListStyle.CircleNumWdBlackPlain;
      case "circleNumWdWhitePlain":
        return NumberedListStyle.CircleNumWdWhitePlain;
      case "arabicDbPeriod":
        return NumberedListStyle.ArabicDbPeriod;
      case "arabicDbPlain":
        return NumberedListStyle.ArabicDbPlain;
      case "ea1ChsPeriod":
      case "ea1ChsPlain":
      case "ea1ChtPeriod":
      case "ea1ChtPlain":
      case "ea1JpnChsDbPeriod":
      case "ea1JpnKorPlain":
      case "ea1JpnKorPeriod":
      case "arabic1Minus":
      case "arabic2Minus":
      case "thaiAlphaPeriod":
        return NumberedListStyle.ThaiAlphaPeriod;
      case "hebrew2Minus":
        return NumberedListStyle.HebrewAlphaDash;
      case "thaiAlphaParenR":
        return NumberedListStyle.ThaiAlphaParenRight;
      case "thaiAlphaParenBoth":
        return NumberedListStyle.ThaiAlphaParenBoth;
      case "thaiNumPeriod":
        return NumberedListStyle.ThaiNumPeriod;
      case "thaiNumParenR":
        return NumberedListStyle.ThaiNumParenRight;
      case "thaiNumParenBoth":
        return NumberedListStyle.ThaiNumParenBoth;
      case "hindiAlphaPeriod":
        return NumberedListStyle.HindiAlphaPeriod;
      case "hindiNumPeriod":
        return NumberedListStyle.HindiNumPeriod;
      case "hindiNumParenR":
        return NumberedListStyle.HindiNumParenRight;
      case "hindiAlpha1Period":
        return NumberedListStyle.HindiAlpha1Period;
      default:
        return NumberedListStyle.AlphaUcPeriod;
    }
  }

  internal static TextOrientationType GetOrientationType(string value)
  {
    switch (value)
    {
      case "vert":
        return TextOrientationType.ClockWise;
      case "vert270":
        return TextOrientationType.CounterClockWise;
      case "wordArtVertRtl":
        return TextOrientationType.TopToBottom;
      default:
        return TextOrientationType.NoRotation;
    }
  }

  internal static PathShapeType GetPathShapeType(string shapeTypeName)
  {
    switch (shapeTypeName)
    {
      case "circle":
        return PathShapeType.Circle;
      case "shape":
        return PathShapeType.Shape;
      default:
        return PathShapeType.Rectangle;
    }
  }

  internal static PenAlignmentType GetPenAlignmentType(string penAlignmentType)
  {
    switch (penAlignmentType)
    {
      case "in":
        return PenAlignmentType.In;
      default:
        return PenAlignmentType.Center;
    }
  }

  internal static Orientation GetPlaceHolderDirection(string value)
  {
    switch (value)
    {
      case "horz":
        return Orientation.Horizontal;
      case "vert":
        return Orientation.Vertical;
      default:
        return Orientation.Horizontal;
    }
  }

  internal static PathFillMode GetPathFillMode(string value)
  {
    switch (value)
    {
      case "darken":
        return PathFillMode.Darken;
      case "darkenLess":
        return PathFillMode.DarkenLess;
      case "lighten":
        return PathFillMode.Lighten;
      case "lightenLess":
        return PathFillMode.LightenLess;
      case "none":
        return PathFillMode.None;
      case "norm":
        return PathFillMode.Normal;
      default:
        return PathFillMode.Normal;
    }
  }

  internal static PlaceholderSize GetPlaceHolderSize(string value)
  {
    switch (value)
    {
      case "full":
        return PlaceholderSize.Full;
      case "half":
        return PlaceholderSize.Half;
      case "quarter":
        return PlaceholderSize.Quarter;
      default:
        return PlaceholderSize.Full;
    }
  }

  internal static PlaceholderType GetPlaceHolderType(string value)
  {
    switch (value)
    {
      case "title":
        return PlaceholderType.Title;
      case "body":
        return PlaceholderType.Body;
      case "ctrTitle":
        return PlaceholderType.CenterTitle;
      case "subTitle":
        return PlaceholderType.Subtitle;
      case "dt":
        return PlaceholderType.Date;
      case "sldNum":
        return PlaceholderType.SlideNumber;
      case "ftr":
        return PlaceholderType.Footer;
      case "hdr":
        return PlaceholderType.Header;
      case "obj":
        return PlaceholderType.Object;
      case "chart":
        return PlaceholderType.Chart;
      case "sldImg":
        return PlaceholderType.SlideImage;
      case "tbl":
        return PlaceholderType.Table;
      case "clipArt":
        return PlaceholderType.Bitmap;
      case "dgm":
        return PlaceholderType.OrganizationChart;
      case "media":
        return PlaceholderType.MediaClip;
      case "pic":
        return PlaceholderType.Picture;
      default:
        return PlaceholderType.Object;
    }
  }

  internal static RectangleAlignmentType GetRectangleAlignType(string value)
  {
    switch (value)
    {
      case "b":
        return RectangleAlignmentType.Bottom;
      case "bl":
        return RectangleAlignmentType.BottomLeft;
      case "br":
        return RectangleAlignmentType.BottomRight;
      case "ctr":
        return RectangleAlignmentType.Center;
      case "l":
        return RectangleAlignmentType.Left;
      case "r":
        return RectangleAlignmentType.Right;
      case "t":
        return RectangleAlignmentType.Top;
      case "tl":
        return RectangleAlignmentType.TopLeft;
      case "tr":
        return RectangleAlignmentType.TopRight;
      default:
        return RectangleAlignmentType.TopLeft;
    }
  }

  internal static SlideSizeType GetSlideType(string value)
  {
    switch (value)
    {
      case "screen4x3":
        return SlideSizeType.OnScreen;
      case "letter":
        return SlideSizeType.LetterPaper;
      case "A4":
        return SlideSizeType.A4Paper;
      case "35mm":
        return SlideSizeType.Slide35Mm;
      case "overhead":
        return SlideSizeType.Overhead;
      case "banner":
        return SlideSizeType.Banner;
      case "custom":
        return SlideSizeType.Custom;
      case "ledger":
        return SlideSizeType.Ledger;
      case "A3":
        return SlideSizeType.A3Paper;
      case "B4ISO":
        return SlideSizeType.B4IsoPaper;
      case "B5ISO":
        return SlideSizeType.B5IsoPaper;
      case "screen16x9":
        return SlideSizeType.OnScreen16X9;
      case "screen16x10":
        return SlideSizeType.OnScreen16X10;
      default:
        return SlideSizeType.Custom;
    }
  }

  internal static TextStrikethroughType GetStrikeType(string value)
  {
    switch (value)
    {
      case "noStrike":
        return TextStrikethroughType.None;
      case "sngStrike":
        return TextStrikethroughType.Single;
      case "dblStrike":
        return TextStrikethroughType.Double;
      default:
        return TextStrikethroughType.None;
    }
  }

  internal static HorizontalAlignment GetHorizontalAlignType(string value)
  {
    switch (value)
    {
      case "r":
        return HorizontalAlignment.Right;
      case "ctr":
        return HorizontalAlignment.Center;
      case "dist":
        return HorizontalAlignment.Distributed;
      case "just":
        return HorizontalAlignment.Justify;
      case "l":
        return HorizontalAlignment.Left;
      case "justLow":
        return HorizontalAlignment.JustifyLow;
      case "thaiDist":
        return HorizontalAlignment.ThaiDistributed;
      default:
        return HorizontalAlignment.Center;
    }
  }

  internal static TabAlignmentType GetTabAlignType(string value)
  {
    switch (value)
    {
      case "r":
        return TabAlignmentType.Right;
      case "ctr":
        return TabAlignmentType.Center;
      case "dec":
        return TabAlignmentType.Decimal;
      case "l":
        return TabAlignmentType.Left;
      default:
        return TabAlignmentType.Left;
    }
  }

  internal static FontAlignmentType GetFontAlignType(string value)
  {
    switch (value)
    {
      case "b":
        return FontAlignmentType.Bottom;
      case "base":
        return FontAlignmentType.Baseline;
      case "ctr":
        return FontAlignmentType.Center;
      case "t":
        return FontAlignmentType.Top;
      default:
        return FontAlignmentType.None;
    }
  }

  internal static TextCapsType GetTextCapsType(string value)
  {
    switch (value)
    {
      case "none":
        return TextCapsType.None;
      case "small":
        return TextCapsType.Small;
      case "all":
        return TextCapsType.All;
      default:
        return TextCapsType.None;
    }
  }

  internal static TextOverflowType GetTextOverflowType(string value)
  {
    switch (value)
    {
      case "clip":
        return TextOverflowType.Clip;
      case "ellipsis":
        return TextOverflowType.Ellipsis;
      default:
        return TextOverflowType.Overflow;
    }
  }

  internal static string GetThemeIndex(string value, MasterSlide masterSlide)
  {
    return masterSlide != null && masterSlide.ColorMap.ContainsKey(value) ? masterSlide.ColorMap[value] : value;
  }

  internal static string GetThemeIndex(string value, LayoutSlide layoutSlide)
  {
    return layoutSlide != null && layoutSlide.ColorMap.ContainsKey(value) ? layoutSlide.ColorMap[value] : value;
  }

  internal static string GetThemeString(int value)
  {
    switch (value)
    {
      case 0:
        return "dk1";
      case 1:
        return "lt1";
      case 2:
        return "dk2";
      case 3:
        return "lt2";
      case 4:
        return "accent1";
      case 5:
        return "accent2";
      case 6:
        return "accent3";
      case 7:
        return "accent4";
      case 8:
        return "accent5";
      case 9:
        return "accent6";
      case 10:
        return "hlink";
      case 11:
        return "folHlink";
      default:
        return "phClr";
    }
  }

  internal static string GetThemeStringFromIndex(string value, MasterSlide masterSlide)
  {
    return Helper.GetDefaultString(value, masterSlide);
  }

  internal static string GetThemeStringFromIndex(string value, LayoutSlide layoutSlide)
  {
    return Helper.GetDefaultString(value, layoutSlide);
  }

  internal static TextDirection GetTextDirection(string directionType)
  {
    switch (directionType)
    {
      case "vert":
        return TextDirection.Vertical;
      case "vert270":
        return TextDirection.Vertical270;
      case "wordArtVert":
        return TextDirection.WordArtVertical;
      case "eaVert":
        return TextDirection.EastAsianVertical;
      case "mongolianVert":
        return TextDirection.MongolianVertical;
      case "wordArtVertRtl":
        return TextDirection.WordArtRightToLeft;
      default:
        return TextDirection.Horizontal;
    }
  }

  internal static int ParseComponents(double value)
  {
    return (int) Math.Round((value >= 0.0 ? (value > 0.0031308 ? (value >= 1.0 ? 1.0 : 1.055 * Math.Pow(value, 5.0 / 12.0) - 0.055) : value * 12.92) : 0.0) * (double) byte.MaxValue, 0);
  }

  internal static int PointToEmu(double point) => (int) Math.Round(point * 12700.0);

  internal static long PointToEmuLong(double point) => (long) Math.Round(point * 12700.0);

  internal static char ToChar(string value) => Convert.ToChar(value);

  internal static double ToDouble(string value)
  {
    return double.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
  }

  internal static bool ToBoolean(string value)
  {
    value = value.ToLower();
    return value == "1" || value == "true";
  }

  internal static float ToFloat(string value)
  {
    return float.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
  }

  internal static int ToInt(string value)
  {
    return int.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
  }

  internal static ushort ToUshort(string value)
  {
    return ushort.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
  }

  internal static uint ToUInt(string value)
  {
    return uint.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
  }

  internal static long ToLong(string value)
  {
    return long.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
  }

  internal static string ToString(int value)
  {
    return value.ToString((IFormatProvider) CultureInfo.InvariantCulture);
  }

  internal static string ToString(long value)
  {
    return value.ToString((IFormatProvider) CultureInfo.InvariantCulture);
  }

  internal static string ToString(double value)
  {
    return value.ToString((IFormatProvider) CultureInfo.InvariantCulture);
  }

  internal static string ToString(float value)
  {
    return value.ToString((IFormatProvider) CultureInfo.InvariantCulture);
  }

  internal static string ToString(PathShapeType pathShapeType)
  {
    switch (pathShapeType)
    {
      case PathShapeType.Circle:
        return "circle";
      case PathShapeType.Shape:
        return "shape";
      default:
        return "rect";
    }
  }

  internal static string ToString(PatternFillType fillPattern)
  {
    switch (fillPattern)
    {
      case PatternFillType.Gray5:
        return "pct5";
      case PatternFillType.Gray10:
        return "pct10";
      case PatternFillType.Gray20:
        return "pct20";
      case PatternFillType.Gray30:
        return "pct30";
      case PatternFillType.Gray40:
        return "pct40";
      case PatternFillType.Gray50:
        return "pct50";
      case PatternFillType.Gray60:
        return "pct60";
      case PatternFillType.Gray70:
        return "pct70";
      case PatternFillType.Gray75:
        return "pct75";
      case PatternFillType.Gray80:
        return "pct80";
      case PatternFillType.Gray90:
        return "pct90";
      case PatternFillType.Gray25:
        return "pct25";
      case PatternFillType.Cross:
        return "cross";
      case PatternFillType.DiagonalCross:
        return "diagCross";
      case PatternFillType.DownwardDiagonal:
        return "dnDiag";
      case PatternFillType.Horizontal:
        return "horz";
      case PatternFillType.UpwardDiagonal:
        return "upDiag";
      case PatternFillType.Vertical:
        return "vert";
      case PatternFillType.LightDownwardDiagonal:
        return "ltDnDiag";
      case PatternFillType.LightUpwardDiagonal:
        return "ltUpDiag";
      case PatternFillType.DarkDownwardDiagonal:
        return "dkDnDiag";
      case PatternFillType.DarkUpwardDiagonal:
        return "dkUpDiag";
      case PatternFillType.WideDownwardDiagonal:
        return "wdDnDiag";
      case PatternFillType.WideUpwardDiagonal:
        return "wdUpDiag";
      case PatternFillType.LightVertical:
        return "ltVert";
      case PatternFillType.LightHorizontal:
        return "ltHorz";
      case PatternFillType.NarrowVertical:
        return "narVert";
      case PatternFillType.NarrowHorizontal:
        return "narHorz";
      case PatternFillType.DarkVertical:
        return "dkVert";
      case PatternFillType.DarkHorizontal:
        return "dkHorz";
      case PatternFillType.DashedDownwardDiagonal:
        return "dashDnDiag";
      case PatternFillType.DashedUpwardDiagonal:
        return "dashUpDiag";
      case PatternFillType.DashedVertical:
        return "dashVert";
      case PatternFillType.DashedHorizontal:
        return "dashHorz";
      case PatternFillType.SmallConfetti:
        return "smConfetti";
      case PatternFillType.LargeConfetti:
        return "lgConfetti";
      case PatternFillType.ZigZag:
        return "zigZag";
      case PatternFillType.Wave:
        return "wave";
      case PatternFillType.DiagonalBrick:
        return "diagBrick";
      case PatternFillType.HorizontalBrick:
        return "horzBrick";
      case PatternFillType.Weave:
        return "weave";
      case PatternFillType.Plaid:
        return "plaid";
      case PatternFillType.Divot:
        return "divot";
      case PatternFillType.DottedGrid:
        return "dotGrid";
      case PatternFillType.DottedDiamond:
        return "dotDmnd";
      case PatternFillType.Shingle:
        return "shingle";
      case PatternFillType.Trellis:
        return "trellis";
      case PatternFillType.Sphere:
        return "sphere";
      case PatternFillType.SmallGrid:
        return "smGrid";
      case PatternFillType.LargeGrid:
        return "lgGrid";
      case PatternFillType.SmallCheckerBoard:
        return "smCheck";
      case PatternFillType.LargeCheckerBoard:
        return "lgCheck";
      case PatternFillType.OutlinedDiamond:
        return "openDmnd";
      case PatternFillType.SolidDiamond:
        return "solidDmnd";
      default:
        return "pct5";
    }
  }

  internal static string ToString(MirrorType mirrorType)
  {
    switch (mirrorType)
    {
      case MirrorType.Horizonal:
        return "x";
      case MirrorType.Vertical:
        return "y";
      case MirrorType.Both:
        return "xy";
      default:
        return "none";
    }
  }

  internal static string ToString(RectangleAlignmentType rectangleAlignmentType)
  {
    switch (rectangleAlignmentType)
    {
      case RectangleAlignmentType.Bottom:
        return "b";
      case RectangleAlignmentType.BottomLeft:
        return "bl";
      case RectangleAlignmentType.BottomRight:
        return "br";
      case RectangleAlignmentType.Center:
        return "ctr";
      case RectangleAlignmentType.Left:
        return "l";
      case RectangleAlignmentType.Right:
        return "r";
      case RectangleAlignmentType.Top:
        return "t";
      case RectangleAlignmentType.TopLeft:
        return "tl";
      case RectangleAlignmentType.TopRight:
        return "tr";
      default:
        return "tl";
    }
  }

  internal static string ToString(TabAlignmentType tabAlignmentType)
  {
    switch (tabAlignmentType)
    {
      case TabAlignmentType.Left:
        return "l";
      case TabAlignmentType.Center:
        return "ctr";
      case TabAlignmentType.Right:
        return "r";
      case TabAlignmentType.Decimal:
        return "dec";
      default:
        throw new ArgumentException("Invalid TabAlignmentType val");
    }
  }

  internal static string ToString(HorizontalAlignment HorizontalAlignment)
  {
    switch (HorizontalAlignment)
    {
      case HorizontalAlignment.Left:
        return "l";
      case HorizontalAlignment.Center:
        return "ctr";
      case HorizontalAlignment.Right:
        return "r";
      case HorizontalAlignment.Justify:
        return "just";
      case HorizontalAlignment.Distributed:
        return "dist";
      case HorizontalAlignment.JustifyLow:
        return "justLow";
      case HorizontalAlignment.ThaiDistributed:
        return "thaiDist";
      default:
        throw new ArgumentException("Invalid HorizontalAlignment val");
    }
  }

  internal static string ToString(VerticalAlignment VerticalAlignment)
  {
    switch (VerticalAlignment)
    {
      case VerticalAlignment.Top:
        return "t";
      case VerticalAlignment.Middle:
        return "ctr";
      case VerticalAlignment.Bottom:
        return "b";
      case VerticalAlignment.Justify:
        return "just";
      case VerticalAlignment.Distributed:
        return "dist";
      default:
        throw new ArgumentException("Invalid VerticalAlignment val");
    }
  }

  internal static string ToString(FontAlignmentType fontAlignmentType)
  {
    switch (fontAlignmentType)
    {
      case FontAlignmentType.Baseline:
        return "base";
      case FontAlignmentType.Bottom:
        return "b";
      case FontAlignmentType.Top:
        return "t";
      case FontAlignmentType.Center:
        return "ctr";
      default:
        throw new ArgumentException("Invalid FontAlignmentType val");
    }
  }

  internal static string ToString(TextUnderline fontUnderlineType)
  {
    switch (fontUnderlineType)
    {
      case TextUnderline.None:
        return "none";
      case TextUnderline.Single:
        return "sng";
      case TextUnderline.Double:
        return "dbl";
      case TextUnderline.Dash:
        return "dash";
      case TextUnderline.DashDotDotHeavy:
        return "dotDotDashHeavy";
      case TextUnderline.DashDotHeavy:
        return "dotDashHeavy";
      case TextUnderline.DashedHeavy:
        return "dashHeavy";
      case TextUnderline.DashLong:
        return "dashLong";
      case TextUnderline.DashLongHeavy:
        return "dashLongHeavy";
      case TextUnderline.DotDash:
        return "dotDash";
      case TextUnderline.DotDotDash:
        return "dotDotDash";
      case TextUnderline.Dotted:
        return "dotted";
      case TextUnderline.DottedHeavy:
        return "dottedHeavy";
      case TextUnderline.Heavy:
        return "heavy";
      case TextUnderline.Wave:
        return "wavy";
      case TextUnderline.WavyDouble:
        return "wavyDbl";
      case TextUnderline.WavyHeavy:
        return "wavyHeavy";
      case TextUnderline.Words:
        return "words";
      default:
        throw new ArgumentException("Invalid FontUnderlineType value");
    }
  }

  internal static string ToString(TextOverflowType textOverflowType)
  {
    switch (textOverflowType)
    {
      case TextOverflowType.Clip:
        return "clip";
      case TextOverflowType.Ellipsis:
        return "ellipsis";
      default:
        return "overflow";
    }
  }

  internal static string ToString(TextOrientationType textOrientationType)
  {
    switch (textOrientationType)
    {
      case TextOrientationType.ClockWise:
        return "vert";
      case TextOrientationType.CounterClockWise:
        return "vert270";
      case TextOrientationType.TopToBottom:
        return "wordArtVertRtl";
      default:
        return (string) null;
    }
  }

  internal static string ToString(NumberedListStyle numberedBulletStyle)
  {
    switch (numberedBulletStyle)
    {
      case NumberedListStyle.AlphaLcPeriod:
        return "alphaLcPeriod";
      case NumberedListStyle.AlphaUcPeriod:
        return "alphaUcPeriod";
      case NumberedListStyle.ArabicParenRight:
        return "arabicParenR";
      case NumberedListStyle.ArabicPeriod:
        return "arabicPeriod";
      case NumberedListStyle.RomanLcParenBoth:
        return "romanLcParenBoth";
      case NumberedListStyle.RomanLcParenRight:
        return "romanLcParenR";
      case NumberedListStyle.RomanLcPeriod:
        return "romanLcPeriod";
      case NumberedListStyle.RomanUcPeriod:
        return "romanUcPeriod";
      case NumberedListStyle.AlphaLcParenBoth:
        return "alphaLcParenBoth";
      case NumberedListStyle.AlphaLcParenRight:
        return "alphaLcParenR";
      case NumberedListStyle.AlphaUcParenBoth:
        return "alphaUcParenBoth";
      case NumberedListStyle.AlphaUcParenRight:
        return "alphaUcParenR";
      case NumberedListStyle.ArabicParenBoth:
        return "arabicParenBoth";
      case NumberedListStyle.ArabicPlain:
        return "arabicPlain";
      case NumberedListStyle.RomanUcParenBoth:
        return "romanUcParenBoth";
      case NumberedListStyle.RomanUcParenRight:
        return "romanUcParenR";
      case NumberedListStyle.CircleNumDbPlain:
        return "circleNumDbPlain";
      case NumberedListStyle.CircleNumWdWhitePlain:
        return "circleNumWdWhitePlain";
      case NumberedListStyle.CircleNumWdBlackPlain:
        return "circleNumWdBlackPlain";
      case NumberedListStyle.HebrewAlphaDash:
        return "hebrew2Minus";
      case NumberedListStyle.ArabicDbPlain:
        return "arabicDbPlain";
      case NumberedListStyle.ArabicDbPeriod:
        return "arabicDbPeriod";
      case NumberedListStyle.ThaiAlphaPeriod:
        return "thaiAlphaPeriod";
      case NumberedListStyle.ThaiAlphaParenRight:
        return "thaiAlphaParenR";
      case NumberedListStyle.ThaiAlphaParenBoth:
        return "thaiAlphaParenBoth";
      case NumberedListStyle.ThaiNumPeriod:
        return "thaiNumPeriod";
      case NumberedListStyle.ThaiNumParenRight:
        return "thaiNumParenR";
      case NumberedListStyle.ThaiNumParenBoth:
        return "thaiNumParenBoth";
      case NumberedListStyle.HindiAlphaPeriod:
        return "hindiAlphaPeriod";
      case NumberedListStyle.HindiNumPeriod:
        return "hindiNumPeriod";
      case NumberedListStyle.HindiNumParenRight:
        return "hindiNumParenR";
      case NumberedListStyle.HindiAlpha1Period:
        return "hindiAlpha1Period";
      default:
        throw new NotSupportedException("Bullet Style is not supported.");
    }
  }

  internal static string ToString(TextStrikethroughType textStrikeType)
  {
    switch (textStrikeType)
    {
      case TextStrikethroughType.Single:
        return "sngStrike";
      case TextStrikethroughType.Double:
        return "dblStrike";
      default:
        return "noStrike";
    }
  }

  internal static string ToString(TextCapsType textCapsType)
  {
    switch (textCapsType)
    {
      case TextCapsType.Small:
        return "small";
      case TextCapsType.All:
        return "all";
      default:
        return "none";
    }
  }

  internal static string ToString(TextDirection textDirection)
  {
    switch (textDirection)
    {
      case TextDirection.Vertical:
        return "vert";
      case TextDirection.Vertical270:
        return "vert270";
      case TextDirection.WordArtVertical:
        return "wordArtVert";
      case TextDirection.EastAsianVertical:
        return "eaVert";
      case TextDirection.MongolianVertical:
        return "mongolianVert";
      case TextDirection.WordArtRightToLeft:
        return "wordArtVertRtl";
      default:
        return "horz";
    }
  }

  internal static string ToString(LineDashStyle lineDashStyle)
  {
    switch (lineDashStyle)
    {
      case LineDashStyle.Solid:
        return "solid";
      case LineDashStyle.Dash:
        return "dash";
      case LineDashStyle.DashDot:
        return "dashDot";
      case LineDashStyle.DashDotDot:
        return "lgDashDotDot";
      case LineDashStyle.DashLongDash:
        return "lgDash";
      case LineDashStyle.DashLongDashDot:
        return "lgDashDot";
      case LineDashStyle.RoundDot:
        return "sysDot";
      case LineDashStyle.SquareDot:
        return "sysDash";
      case LineDashStyle.SystemDashDot:
        return "sysDashDot";
      case LineDashStyle.Dot:
        return "dot";
      default:
        return (string) null;
    }
  }

  internal static string ToString(ArrowheadStyle arrowheadStyle)
  {
    switch (arrowheadStyle)
    {
      case ArrowheadStyle.None:
        return "none";
      case ArrowheadStyle.Arrow:
        return "triangle";
      case ArrowheadStyle.ArrowStealth:
        return "stealth";
      case ArrowheadStyle.ArrowDiamond:
        return "diamond";
      case ArrowheadStyle.ArrowOval:
        return "oval";
      case ArrowheadStyle.ArrowOpen:
        return "arrow";
      default:
        return (string) null;
    }
  }

  internal static string ToString(ArrowheadWidth arrowheadWidth)
  {
    switch (arrowheadWidth)
    {
      case ArrowheadWidth.Narrow:
        return "sm";
      case ArrowheadWidth.Medium:
        return "med";
      case ArrowheadWidth.Wide:
        return "lg";
      default:
        return (string) null;
    }
  }

  internal static string ToString(PathFillMode fillMode)
  {
    switch (fillMode)
    {
      case PathFillMode.Darken:
        return "darken";
      case PathFillMode.DarkenLess:
        return "darkenLess";
      case PathFillMode.Lighten:
        return "lighten";
      case PathFillMode.LightenLess:
        return "lightenLess";
      case PathFillMode.None:
        return "none";
      default:
        return "norm";
    }
  }

  internal static string ToString(ArrowheadLength arrowheadLength)
  {
    switch (arrowheadLength)
    {
      case ArrowheadLength.Short:
        return "sm";
      case ArrowheadLength.Medium:
        return "med";
      case ArrowheadLength.Long:
        return "lg";
      default:
        return (string) null;
    }
  }

  internal static string ToString(LineCapStyle lineCapStyle)
  {
    switch (lineCapStyle)
    {
      case LineCapStyle.Round:
        return "rnd";
      case LineCapStyle.Square:
        return "sq";
      case LineCapStyle.Flat:
        return "flat";
      default:
        return (string) null;
    }
  }

  internal static string ToString(LineStyle lineStyle)
  {
    switch (lineStyle)
    {
      case LineStyle.Single:
        return "sng";
      case LineStyle.ThickBetweenThin:
        return "tri";
      case LineStyle.ThinThick:
        return "thinThick";
      case LineStyle.ThickThin:
        return "thickThin";
      case LineStyle.ThinThin:
        return "dbl";
      default:
        return (string) null;
    }
  }

  internal static string ToString(PenAlignmentType penAlignmentType)
  {
    return penAlignmentType == PenAlignmentType.In ? "in" : "ctr";
  }

  internal static string ToString(BlackWhiteMode blackWhiteMode)
  {
    switch (blackWhiteMode)
    {
      case BlackWhiteMode.Clear:
        return "clr";
      case BlackWhiteMode.Auto:
        return "auto";
      case BlackWhiteMode.Gray:
        return "gray";
      case BlackWhiteMode.LightGray:
        return "ltGray";
      case BlackWhiteMode.InverseGray:
        return "invGray";
      case BlackWhiteMode.GrayWhite:
        return "grayWhite";
      case BlackWhiteMode.BlackGray:
        return "blackGray";
      case BlackWhiteMode.BlackWhite:
        return "blackWhite";
      case BlackWhiteMode.Black:
        return "black";
      case BlackWhiteMode.White:
        return "white";
      case BlackWhiteMode.Hidden:
        return "hidden";
      default:
        return "white";
    }
  }

  internal static string ToString(PlaceholderType value)
  {
    switch (value)
    {
      case PlaceholderType.Title:
      case PlaceholderType.VerticalTitle:
        return "title";
      case PlaceholderType.Body:
      case PlaceholderType.VerticalBody:
        return "body";
      case PlaceholderType.CenterTitle:
        return "ctrTitle";
      case PlaceholderType.Subtitle:
        return "subTitle";
      case PlaceholderType.Object:
        return "obj";
      case PlaceholderType.Chart:
        return "chart";
      case PlaceholderType.Bitmap:
        return "clipArt";
      case PlaceholderType.MediaClip:
        return "media";
      case PlaceholderType.OrganizationChart:
        return "dgm";
      case PlaceholderType.Table:
        return "tbl";
      case PlaceholderType.SlideNumber:
        return "sldNum";
      case PlaceholderType.Header:
        return "hdr";
      case PlaceholderType.Footer:
        return "ftr";
      case PlaceholderType.Date:
        return "dt";
      case PlaceholderType.Picture:
        return "pic";
      case PlaceholderType.SlideImage:
        return "sldImg";
      default:
        return "none";
    }
  }

  internal static string ToString(Orientation orientation)
  {
    switch (orientation)
    {
      case Orientation.Vertical:
        return "vert";
      case Orientation.Horizontal:
        return "horz";
      default:
        return "horz";
    }
  }

  internal static string ToString(PlaceholderSize placeholderSize)
  {
    switch (placeholderSize)
    {
      case PlaceholderSize.Half:
        return "half";
      case PlaceholderSize.Quarter:
        return "quarter";
      case PlaceholderSize.Full:
        return "full";
      default:
        return "full";
    }
  }

  internal static float InchToPoint(double inch) => (float) (inch * 72.0);

  internal static float InchToPixel(double inch) => (float) (inch * 96.0);

  internal static OnOffStyleType SetOnOff(string value)
  {
    switch (value)
    {
      case "on":
        return OnOffStyleType.On;
      case "off":
        return OnOffStyleType.Off;
      default:
        return OnOffStyleType.Def;
    }
  }

  internal static string ToString(OnOffStyleType value)
  {
    switch (value)
    {
      case OnOffStyleType.On:
        return "on";
      case OnOffStyleType.Off:
        return "off";
      default:
        return "def";
    }
  }

  internal static string GetNameFromPlaceholder(string shapeName)
  {
    if (shapeName.Contains("Title"))
      return "Title";
    if (shapeName.Contains("Subtitle"))
      return "Subtitle";
    if (shapeName.Contains("Text") && !shapeName.Contains("Box"))
      return "Text";
    return shapeName.Contains("Content") ? "Content" : "shape";
  }

  internal static TextBody GetTextStyle(MasterSlide masterSlide, string styleName)
  {
    if (masterSlide == null)
      return (TextBody) null;
    switch (styleName)
    {
      case "Title":
        return masterSlide.TitleStyle;
      case "Text":
      case "Content":
      case "Subtitle":
        return masterSlide.BodyStyle;
      default:
        return masterSlide.OtherStyle;
    }
  }

  internal static string GetFontNameFromTheme(
    Dictionary<string, string> themeValues,
    FontScriptType scriptType)
  {
    if (scriptType == FontScriptType.Korean && themeValues.ContainsKey("Hang"))
      return themeValues["Hang"];
    if (scriptType == FontScriptType.Chinese && themeValues.ContainsKey("Hans"))
      return themeValues["Hans"];
    if (scriptType == FontScriptType.Japanese && themeValues.ContainsKey("Jpan"))
      return themeValues["Jpan"];
    if (scriptType == FontScriptType.Arabic && themeValues.ContainsKey("Arab"))
      return themeValues["Arab"];
    if (scriptType == FontScriptType.Hebrew && themeValues.ContainsKey("Hebr"))
      return themeValues["Hebr"];
    return scriptType == FontScriptType.Hindi && themeValues.ContainsKey("Deva") ? themeValues["Deva"] : "";
  }

  internal static bool IsValidLang(string lang, FontScriptType scriptType)
  {
    if (!string.IsNullOrEmpty(lang))
    {
      lang = lang.TrimStart().ToLower();
      if (lang.StartsWith("ar-") && scriptType == FontScriptType.Arabic || lang.StartsWith("he-") && scriptType == FontScriptType.Hebrew || lang.StartsWith("hi-") && scriptType == FontScriptType.Hindi || lang.StartsWith("zh-") && scriptType == FontScriptType.Chinese || lang.StartsWith("ko-") && scriptType == FontScriptType.Korean || lang.StartsWith("ja-") && scriptType == FontScriptType.Japanese)
        return true;
    }
    return false;
  }

  internal static string GetFontNameFromTheme(
    Theme theme,
    string name,
    FontScriptType scriptType,
    string lang)
  {
    string str = (string) null;
    switch (name)
    {
      case "+mj-lt":
        string latin1 = theme.MajorFont.Latin;
        return !string.IsNullOrEmpty(latin1) ? latin1 : Helper.GetDefaultFontName(scriptType);
      case "+mn-lt":
        string latin2 = theme.MinorFont.Latin;
        return !string.IsNullOrEmpty(latin2) ? latin2 : Helper.GetDefaultFontName(scriptType);
      case "+mn-ea":
        if (Helper.IsValidLang(lang, scriptType))
          str = Helper.GetFontNameFromTheme(theme.MinorFont.Font, scriptType);
        if (string.IsNullOrEmpty(str))
          str = theme.MinorFont.Ea;
        return !string.IsNullOrEmpty(str) ? str : Helper.GetDefaultFontName(scriptType);
      case "+mj-ea":
        if (Helper.IsValidLang(lang, scriptType))
          str = Helper.GetFontNameFromTheme(theme.MajorFont.Font, scriptType);
        if (string.IsNullOrEmpty(str))
          str = theme.MajorFont.Ea;
        return !string.IsNullOrEmpty(str) ? str : Helper.GetDefaultFontName(scriptType);
      case "+mn-cs":
        if (Helper.IsValidLang(lang, scriptType))
          str = Helper.GetFontNameFromTheme(theme.MinorFont.Font, scriptType);
        if (string.IsNullOrEmpty(str))
          str = theme.MinorFont.Cs;
        return !string.IsNullOrEmpty(str) ? str : Helper.GetDefaultFontName(scriptType);
      case "+mj-cs":
        if (Helper.IsValidLang(lang, scriptType))
          str = Helper.GetFontNameFromTheme(theme.MajorFont.Font, scriptType);
        if (string.IsNullOrEmpty(str))
          str = theme.MajorFont.Cs;
        return !string.IsNullOrEmpty(str) ? str : "Arial";
      case "minor":
        if (Helper.IsValidLang(lang, scriptType))
          str = Helper.GetFontNameFromTheme(theme.MinorFont.Font, scriptType);
        if (string.IsNullOrEmpty(str) && Helper.IsEastAsianScript(scriptType))
          str = theme.MinorFont.Ea;
        else if (string.IsNullOrEmpty(str) && Helper.IsComplexScript(scriptType))
          str = theme.MinorFont.Cs;
        else if (string.IsNullOrEmpty(str))
          str = theme.MinorFont.Latin;
        return !string.IsNullOrEmpty(str) ? str : Helper.GetDefaultFontName(scriptType);
      case "major":
        if (Helper.IsValidLang(lang, scriptType))
          str = Helper.GetFontNameFromTheme(theme.MajorFont.Font, scriptType);
        if (string.IsNullOrEmpty(str) && Helper.IsEastAsianScript(scriptType))
          str = theme.MajorFont.Ea;
        else if (string.IsNullOrEmpty(str) && Helper.IsComplexScript(scriptType))
          str = theme.MajorFont.Cs;
        else if (string.IsNullOrEmpty(str))
          str = theme.MajorFont.Latin;
        return !string.IsNullOrEmpty(str) ? str : Helper.GetDefaultFontName(scriptType);
      default:
        return name == null ? "" : name;
    }
  }

  internal static string GetDefaultFontName(FontScriptType scriptType)
  {
    return Helper.IsEastAsianScript(scriptType) ? "MS Gothic" : "Arial";
  }

  internal static bool IsEastAsianScript(FontScriptType scriptType)
  {
    return scriptType == FontScriptType.Chinese || scriptType == FontScriptType.Japanese || scriptType == FontScriptType.Korean;
  }

  internal static bool IsComplexScript(FontScriptType scriptType)
  {
    return scriptType == FontScriptType.Arabic || scriptType == FontScriptType.Hebrew || scriptType == FontScriptType.Hindi;
  }

  internal static bool GetBoolFromOnOff(OnOffStyleType onOffStyleType)
  {
    return onOffStyleType == OnOffStyleType.On;
  }

  internal static string GetActionString(HyperLinkType _actionType, string url)
  {
    switch (_actionType)
    {
      case HyperLinkType.NoAction:
        return "ppaction://noaction";
      case HyperLinkType.Hyperlink:
        string actionString = (string) null;
        if (url != null)
        {
          if (url.Contains("#"))
          {
            int startIndex = url.IndexOf("#");
            url = url.Remove(startIndex, url.Length - startIndex);
          }
          actionString = !url.ToLowerInvariant().EndsWith(".pptx") ? "ppaction://hlinkfile" : "ppaction://hlinkpres";
        }
        return actionString;
      case HyperLinkType.JumpFirstSlide:
        return "ppaction://hlinkshowjump?jump=firstslide";
      case HyperLinkType.JumpPreviousSlide:
        return "ppaction://hlinkshowjump?jump=previousslide";
      case HyperLinkType.JumpNextSlide:
        return "ppaction://hlinkshowjump?jump=nextslide";
      case HyperLinkType.JumpLastSlide:
        return "ppaction://hlinkshowjump?jump=lastslide";
      case HyperLinkType.JumpEndShow:
        return "ppaction://hlinkshowjump?jump=endshow";
      case HyperLinkType.JumpLastViewedSlide:
        return "ppaction://hlinkshowjump?jump=lastslideviewed";
      case HyperLinkType.JumpSpecificSlide:
        return "ppaction://hlinksldjump";
      case HyperLinkType.StartProgram:
        return "ppaction://program";
      default:
        return (string) null;
    }
  }

  internal static HyperLinkType GetActionType(string actionString)
  {
    if (actionString.Contains("hlinkpres"))
      actionString = "ppaction://hlinkpres";
    if (!string.IsNullOrEmpty(actionString))
    {
      switch (actionString)
      {
        case "ppaction://hlinkshowjump?jump=firstslide":
          return HyperLinkType.JumpFirstSlide;
        case "ppaction://hlinkshowjump?jump=lastslide":
          return HyperLinkType.JumpLastSlide;
        case "ppaction://hlinkshowjump?jump=nextslide":
          return HyperLinkType.JumpNextSlide;
        case "ppaction://hlinkshowjump?jump=previousslide":
          return HyperLinkType.JumpPreviousSlide;
        case "ppaction://hlinksldjump":
          return HyperLinkType.JumpSpecificSlide;
        case "ppaction://program":
          return HyperLinkType.StartProgram;
        case "ppaction://noaction":
          return HyperLinkType.NoAction;
        case "ppaction://hlinkshowjump?jump=lastslideviewed":
          return HyperLinkType.JumpLastViewedSlide;
        case "ppaction://hlinkshowjump?jump=endshow":
          return HyperLinkType.JumpEndShow;
        case "ppaction://hlinkpres":
        case "ppaction://hlinkfile":
          return HyperLinkType.Hyperlink;
      }
    }
    return HyperLinkType.Unknown;
  }

  internal static string GetAutoFitType(AutoMarginType autoMarginType)
  {
    switch (autoMarginType)
    {
      case AutoMarginType.TextShapeAutoFit:
        return "spAutoFit";
      case AutoMarginType.NormalAutoFit:
        return "normAutofit";
      case AutoMarginType.NoAutoFit:
        return "noAutofit";
      default:
        return (string) null;
    }
  }

  internal static bool IsSymbol(string text)
  {
    bool flag = false;
    if (text != null)
    {
      foreach (char ch in text.ToCharArray())
      {
        if (ch >= '◰' && ch <= '◿' || ch >= '가' && ch <= '\uFFEF')
        {
          flag = true;
          break;
        }
      }
    }
    return flag;
  }

  internal static bool IsGeometricShapesSymbol(string text)
  {
    if (text != null)
    {
      foreach (char ch in text.ToCharArray())
      {
        if (ch >= '◰' && ch <= '◿')
          return true;
      }
    }
    return false;
  }

  internal static bool HasFlag(FontStyle fontStyle, FontStyle styleType)
  {
    byte num = (byte) styleType;
    return ((int) (byte) fontStyle & (int) num) != 0;
  }

  internal static bool HasFlag(BorderType borderStyle, BorderType borderType)
  {
    byte num = (byte) borderType;
    return ((int) (byte) borderStyle & (int) num) != 0;
  }

  internal static string GenerateSlideId(Dictionary<string, string> dictionary)
  {
    string[] strArray = new string[dictionary.Count];
    dictionary.Values.CopyTo(strArray, 0);
    List<string> stringList = new List<string>((IEnumerable<string>) strArray);
    if (stringList.Count == 0)
      return "256";
    int int32 = Convert.ToInt32(stringList[stringList.Count - 1]);
    while (dictionary.ContainsValue(int32.ToString((IFormatProvider) CultureInfo.InvariantCulture)))
      ++int32;
    return int32.ToString((IFormatProvider) CultureInfo.InvariantCulture);
  }

  internal static string GetThemeStringValue(int value)
  {
    switch (value)
    {
      case 0:
        return "dk1";
      case 1:
        return "lt1";
      case 2:
        return "dk2";
      case 3:
        return "lt2";
      case 4:
        return "accent1";
      case 5:
        return "accent2";
      case 6:
        return "accent3";
      case 7:
        return "accent4";
      case 8:
        return "accent5";
      case 9:
        return "accent6";
      case 10:
        return "hlink";
      case 11:
        return "folHlink";
      default:
        return "phClr";
    }
  }

  internal static double EmuToPoint(long emu) => Convert.ToDouble((double) emu / 12700.0);

  internal static int PointToPixel(double value) => Convert.ToInt32(value * 96.0 / 72.0);

  internal static string ToString(SlideSizeType slideSizeType)
  {
    switch (slideSizeType)
    {
      case SlideSizeType.OnScreen:
        return "screen4x3";
      case SlideSizeType.LetterPaper:
        return "letter";
      case SlideSizeType.A4Paper:
        return "A4";
      case SlideSizeType.Slide35Mm:
        return "35mm";
      case SlideSizeType.Overhead:
        return "overhead";
      case SlideSizeType.Banner:
        return "banner";
      case SlideSizeType.Custom:
        return "custom";
      case SlideSizeType.Ledger:
        return "ledger";
      case SlideSizeType.A3Paper:
        return "A3";
      case SlideSizeType.B4IsoPaper:
        return "B4ISO";
      case SlideSizeType.B5IsoPaper:
        return "B5ISO";
      case SlideSizeType.OnScreen16X9:
        return "screen16x9";
      case SlideSizeType.OnScreen16X10:
        return "screen16x10";
      default:
        return (string) null;
    }
  }

  internal static int GenerateSlideId(List<int> list)
  {
    int slideId = 256 /*0x0100*/;
    if (list.Count != 0)
    {
      foreach (int num in list)
      {
        if (num == slideId)
          ++slideId;
      }
    }
    return slideId;
  }

  internal static Dictionary<string, Stream> CloneDictionary(Dictionary<string, Stream> dictionary)
  {
    Dictionary<string, Stream> dictionary1 = new Dictionary<string, Stream>();
    foreach (KeyValuePair<string, Stream> keyValuePair in dictionary)
    {
      string key = keyValuePair.Key;
      if (keyValuePair.Value != null)
      {
        keyValuePair.Value.Position = 0L;
        byte[] buffer = new byte[keyValuePair.Value.Length];
        keyValuePair.Value.Read(buffer, 0, buffer.Length);
        keyValuePair.Value.Position = 0L;
        MemoryStream memoryStream = new MemoryStream(buffer);
        dictionary1.Add(key, (Stream) memoryStream);
      }
    }
    return dictionary1;
  }

  internal static Dictionary<string, string> CloneDictionary(Dictionary<string, string> dictionary)
  {
    Dictionary<string, string> dictionary1 = new Dictionary<string, string>();
    foreach (KeyValuePair<string, string> keyValuePair in dictionary)
      dictionary1.Add(keyValuePair.Key, keyValuePair.Value);
    return dictionary1;
  }

  internal static List<double> CloneList(List<double> list)
  {
    List<double> doubleList = new List<double>();
    foreach (double num in list)
      doubleList.Add(num);
    return doubleList;
  }

  internal static byte[] CloneByteArray(byte[] arr)
  {
    if (arr == null)
      return (byte[]) null;
    int length = arr.Length;
    byte[] numArray = new byte[length];
    for (int index = 0; index < length; ++index)
      numArray[index] = arr[index];
    return numArray;
  }

  internal static List<string> CloneList(List<string> list)
  {
    List<string> stringList = new List<string>();
    foreach (string str in list)
      stringList.Add(str);
    return stringList;
  }

  internal static string GetKeyFromDictionary(Dictionary<string, string> dictionary, string value)
  {
    string empty = string.Empty;
    if (dictionary.ContainsValue(value))
    {
      foreach (KeyValuePair<string, string> keyValuePair in dictionary)
      {
        if (keyValuePair.Value == value)
          return keyValuePair.Key;
      }
    }
    return empty;
  }

  internal static Dictionary<string, int> CloneDictionary(Dictionary<string, int> dictionary)
  {
    Dictionary<string, int> dictionary1 = new Dictionary<string, int>();
    foreach (KeyValuePair<string, int> keyValuePair in dictionary)
      dictionary1.Add(keyValuePair.Key, keyValuePair.Value);
    return dictionary1;
  }

  internal static List<long> CloneList(List<long> list)
  {
    List<long> longList = new List<long>();
    foreach (long num in list)
      longList.Add(num);
    return longList;
  }

  internal static List<ColorObject> CloneColorObjectList(List<ColorObject> list)
  {
    List<ColorObject> colorObjectList = new List<ColorObject>();
    foreach (ColorObject colorObject in list)
      colorObjectList.Add(colorObject.CloneColorObject());
    return colorObjectList;
  }

  internal static List<Stream> CloneList(List<Stream> list)
  {
    List<Stream> streamList = new List<Stream>();
    foreach (Stream stream in list)
    {
      if (stream != null)
      {
        stream.Position = 0L;
        byte[] buffer = new byte[stream.Length];
        stream.Read(buffer, 0, buffer.Length);
        stream.Position = 0L;
        MemoryStream memoryStream = new MemoryStream(buffer);
        streamList.Add(stream);
      }
    }
    return streamList;
  }

  internal static string GenerateLayoutId(Dictionary<string, string> dictionary, string masterId)
  {
    string[] strArray = new string[dictionary.Count];
    dictionary.Values.CopyTo(strArray, 0);
    List<string> stringList = new List<string>((IEnumerable<string>) strArray);
    stringList.Add(masterId);
    stringList.Sort();
    long num;
    return stringList.Count == 0 ? "2147483648" : (num = long.Parse(stringList[stringList.Count - 1]) + 1L).ToString();
  }

  internal static SmartArtPointType GetSmartArtPointType(string smartArtPointType)
  {
    switch (smartArtPointType)
    {
      case "node":
        return SmartArtPointType.Node;
      case "asst":
        return SmartArtPointType.AssistantElement;
      case "doc":
        return SmartArtPointType.Document;
      case "pres":
        return SmartArtPointType.Presentation;
      case "parTrans":
        return SmartArtPointType.ParentTransition;
      case "sibTrans":
        return SmartArtPointType.SiblingTransition;
      default:
        return SmartArtPointType.Node;
    }
  }

  internal static string ToString(SmartArtPointType smartArtPointType)
  {
    switch (smartArtPointType)
    {
      case SmartArtPointType.Node:
        return "node";
      case SmartArtPointType.AssistantElement:
        return "asst";
      case SmartArtPointType.Document:
        return "doc";
      case SmartArtPointType.Presentation:
        return "pres";
      case SmartArtPointType.ParentTransition:
        return "parTrans";
      case SmartArtPointType.SiblingTransition:
        return "sibTrans";
      default:
        return "node";
    }
  }

  internal static string ToString(SmartArtType smartArtType)
  {
    switch (smartArtType)
    {
      case SmartArtType.BasicBlockList:
        return "default";
      case SmartArtType.AlternatingHexagons:
        return "AlternatingHexagons";
      case SmartArtType.PictureCaptionList:
        return "pList1";
      case SmartArtType.LinedList:
        return "LinedList";
      case SmartArtType.VerticalBulletList:
        return "vList2";
      case SmartArtType.VerticalBoxList:
        return "list1";
      case SmartArtType.HorizontalBulletList:
        return "hList1";
      case SmartArtType.SquareAccentList:
        return "SquareAccentList";
      case SmartArtType.PictureAccentList:
        return "hList2";
      case SmartArtType.BendingPictureAccentList:
        return "bList2";
      case SmartArtType.StackedList:
        return "hList9";
      case SmartArtType.IncreasingCircleProcess:
        return "IncreasingCircleProcess";
      case SmartArtType.PieProcess:
        return "PieProcess";
      case SmartArtType.DetailedProcess:
        return "hProcess7";
      case SmartArtType.GroupedList:
        return "lProcess2";
      case SmartArtType.HorizontalPictureList:
        return "pList2";
      case SmartArtType.ContinuousPictureList:
        return "hList7";
      case SmartArtType.PictureStrips:
        return "PictureStrips";
      case SmartArtType.VerticalPictureList:
        return "vList4";
      case SmartArtType.AlternatingPictureBlocks:
        return "AlternatingPictureBlocks";
      case SmartArtType.VerticalPictureAccentList:
        return "vList3";
      case SmartArtType.TitledPictureAccentList:
        return "PictureAccentList";
      case SmartArtType.VerticalBlockList:
        return "vList5";
      case SmartArtType.VerticalChevronList:
        return "chevron2";
      case SmartArtType.VerticalAccentList:
        return "VerticalAccentList";
      case SmartArtType.VerticalArrowList:
        return "vList6";
      case SmartArtType.TrapezoidList:
        return "hList6";
      case SmartArtType.DescendingBlockList:
        return "BlockDescendingList";
      case SmartArtType.TableList:
        return "hList3";
      case SmartArtType.SegmentedProcess:
        return "process4";
      case SmartArtType.VerticalCurvedList:
        return "VerticalCurvedList";
      case SmartArtType.PyramidList:
        return "pyramid2";
      case SmartArtType.TargetList:
        return "target3";
      case SmartArtType.VerticalCircleList:
        return "VerticalCircleList";
      case SmartArtType.TableHierarchy:
        return "hierarchy4";
      case SmartArtType.BasicProcess:
        return "process1";
      case SmartArtType.StepUpProcess:
        return "StepUpProcess";
      case SmartArtType.StepDownProcess:
        return "StepDownProcess";
      case SmartArtType.AccentProcess:
        return "process3";
      case SmartArtType.AlternatingFlow:
        return "hProcess4";
      case SmartArtType.ContinuousBlockProcess:
        return "hProcess9";
      case SmartArtType.IncreasingArrowsProcess:
        return "IncreasingArrowsProcess";
      case SmartArtType.ContinuousArrowProcess:
        return "hProcess3";
      case SmartArtType.ProcessArrows:
        return "hProcess6";
      case SmartArtType.CircleAccentTimeLine:
        return "CircleAccentTimeline";
      case SmartArtType.BasicTimeLine:
        return "hProcess11";
      case SmartArtType.BasicChevronProcess:
        return "chevron1";
      case SmartArtType.ClosedChevronProcess:
        return "hChevron3";
      case SmartArtType.ChevronList:
        return "lProcess3";
      case SmartArtType.SubStepProcess:
        return "SubStepProcess";
      case SmartArtType.PhasedProcess:
        return "PhasedProcess";
      case SmartArtType.RandomToResultProcess:
        return "RandomtoResultProcess";
      case SmartArtType.StaggeredProcess:
        return "vProcess5";
      case SmartArtType.ProcessList:
        return "lProcess1";
      case SmartArtType.CircleArrowProcess:
        return "CircleArrowProcess";
      case SmartArtType.BasicBendingProcess:
        return "process5";
      case SmartArtType.VerticalBendingProcess:
        return "bProcess4";
      case SmartArtType.AscendingPictureAccentprocess:
        return "AscendingPictureAccentProcess";
      case SmartArtType.UpwardArrow:
        return "arrow2";
      case SmartArtType.DescendingProcess:
        return "DescendingProcess";
      case SmartArtType.CircularBendingProcess:
        return "bProcess2";
      case SmartArtType.Equation:
        return "equation1";
      case SmartArtType.VerticalEquation:
        return "equation2";
      case SmartArtType.Funnel:
        return "funnel1";
      case SmartArtType.Gear:
        return "gear1";
      case SmartArtType.ArrowRibbon:
        return "arrow6";
      case SmartArtType.OpposingArrows:
        return "arrow4";
      case SmartArtType.ConvergingArrows:
        return "arrow5";
      case SmartArtType.DivergingArrows:
        return "arrow1";
      case SmartArtType.BasicCycle:
        return "cycle2";
      case SmartArtType.TextCycle:
        return "cycle1";
      case SmartArtType.BlockCycle:
        return "cycle5";
      case SmartArtType.NondirectionalCycle:
        return "cycle6";
      case SmartArtType.ContinuousCycle:
        return "cycle3";
      case SmartArtType.MultiDirectionalCycle:
        return "cycle7";
      case SmartArtType.SegmentedCycle:
        return "cycle8";
      case SmartArtType.BasicPie:
        return "chart3";
      case SmartArtType.RadialCycle:
        return "radial6";
      case SmartArtType.BasicRadial:
        return "radial1";
      case SmartArtType.DivergingRadial:
        return "radial5";
      case SmartArtType.RadialVenn:
        return "radial3";
      case SmartArtType.RadialCluster:
        return "RadialCluster";
      case SmartArtType.OrganizationChart:
        return "orgChart1";
      case SmartArtType.NameAndTitleOrganizationChart:
        return "NameandTitleOrganizationalChart";
      case SmartArtType.HalfCircleOrganizationChart:
        return "HalfCircleOrganizationChart";
      case SmartArtType.CirclePictureHierarchy:
        return "CirclePictureHierarchy";
      case SmartArtType.Hierarchy:
        return "hierarchy1";
      case SmartArtType.LabeledHierarchy:
        return "hierarchy6";
      case SmartArtType.HorizontalOrganizationChart:
        return "HorizontalOrganizationChart";
      case SmartArtType.HorizontalMulti_levelHierarchy:
        return "HorizontalMultiLevelHierarchy";
      case SmartArtType.HorizontalHierarchy:
        return "hierarchy2";
      case SmartArtType.HorizontalLabeledHierarchy:
        return "hierarchy5";
      case SmartArtType.Balance:
        return "balance1";
      case SmartArtType.CircleRelationship:
        return "CircleRelationship";
      case SmartArtType.HexagonCluster:
        return "HexagonCluster";
      case SmartArtType.OpposingIdeas:
        return "OpposingIdeas";
      case SmartArtType.PlusAndMinus:
        return "PlusandMinus";
      case SmartArtType.ReverseList:
        return "ReverseList";
      case SmartArtType.CounterBalanceArrows:
        return "arrow3";
      case SmartArtType.SegmentedPyramid:
        return "pyramid4";
      case SmartArtType.NestedTarget:
        return "target2";
      case SmartArtType.ConvergingRadial:
        return "radial4";
      case SmartArtType.RadialList:
        return "radial2";
      case SmartArtType.BasicTarget:
        return "target1";
      case SmartArtType.BasicMatrix:
        return "matrix3";
      case SmartArtType.TitledMatrix:
        return "matrix1";
      case SmartArtType.GridMatrix:
        return "matrix2";
      case SmartArtType.CycleMatrix:
        return "cycle4";
      case SmartArtType.AccentedPicture:
        return "AccentedPicture";
      case SmartArtType.CircularPictureCallOut:
        return "CircularPictureCallout";
      case SmartArtType.SnapshotPictureList:
        return "SnapshotPictureList";
      case SmartArtType.SpiralPicture:
        return "SpiralPicture";
      case SmartArtType.CaptionedPictures:
        return "CaptionedPictures";
      case SmartArtType.BendingPictureCaption:
        return "BendingPictureCaption";
      case SmartArtType.BendingPictureSemiTransparentText:
        return "BendingPictureSemiTransparentText";
      case SmartArtType.BendingPictureBlocks:
        return "BendingPictureBlocks";
      case SmartArtType.BendingPictureCaptionList:
        return "BendingPictureCaptionList";
      case SmartArtType.TitledPictureBlocks:
        return "TitledPictureBlocks";
      case SmartArtType.PictureGrid:
        return "PictureGrid";
      case SmartArtType.PictureAccentBlocks:
        return "PictureAccentBlocks";
      case SmartArtType.AlternatingPictureCircles:
        return "AlternatingPictureCircles";
      case SmartArtType.TitlePictureLineup:
        return "TitlePictureLineup";
      case SmartArtType.PictureLineUp:
        return "PictureLineup";
      case SmartArtType.FramedTextPicture:
        return "FramedTextPicture";
      case SmartArtType.BubblePictureList:
        return "BubblePictureList";
      case SmartArtType.BasicPyramid:
        return "pyramid1";
      case SmartArtType.InvertedPyramid:
        return "pyramid3";
      case SmartArtType.BasicVenn:
        return "venn1";
      case SmartArtType.LinearVenn:
        return "venn3";
      case SmartArtType.StackedVenn:
        return "venn2";
      case SmartArtType.HierarchyList:
        return "hierarchy3";
      case SmartArtType.PictureAccentProcess:
        return "hProcess10";
      case SmartArtType.RepeatingBendingProcess:
        return "bProcess3";
      case SmartArtType.VerticalProcess:
        return "process2";
      default:
        return "default";
    }
  }

  internal static SmartArtType GetSmartArtType(string smartArtType)
  {
    switch (smartArtType)
    {
      case "default":
        return SmartArtType.BasicBlockList;
      case "AlternatingHexagons":
        return SmartArtType.AlternatingHexagons;
      case "pList1":
        return SmartArtType.PictureCaptionList;
      case "LinedList":
        return SmartArtType.LinedList;
      case "vList2":
        return SmartArtType.VerticalBulletList;
      case "list1":
        return SmartArtType.VerticalBoxList;
      case "hList1":
        return SmartArtType.HorizontalBulletList;
      case "SquareAccentList":
        return SmartArtType.SquareAccentList;
      case "hList2":
        return SmartArtType.PictureAccentList;
      case "bList2":
        return SmartArtType.BendingPictureAccentList;
      case "hList9":
        return SmartArtType.StackedList;
      case "IncreasingCircleProcess":
        return SmartArtType.IncreasingCircleProcess;
      case "PieProcess":
        return SmartArtType.PieProcess;
      case "hProcess7":
        return SmartArtType.DetailedProcess;
      case "lProcess2":
        return SmartArtType.GroupedList;
      case "pList2":
        return SmartArtType.HorizontalPictureList;
      case "hList7":
        return SmartArtType.ContinuousPictureList;
      case "PictureStrips":
        return SmartArtType.PictureStrips;
      case "vList4":
        return SmartArtType.VerticalPictureList;
      case "AlternatingPictureBlocks":
        return SmartArtType.AlternatingPictureBlocks;
      case "vList3":
        return SmartArtType.VerticalPictureAccentList;
      case "PictureAccentList":
        return SmartArtType.TitledPictureAccentList;
      case "vList5":
        return SmartArtType.VerticalBlockList;
      case "chevron2":
        return SmartArtType.VerticalChevronList;
      case "VerticalAccentList":
        return SmartArtType.VerticalAccentList;
      case "vList6":
        return SmartArtType.VerticalArrowList;
      case "hList6":
        return SmartArtType.TrapezoidList;
      case "BlockDescendingList":
        return SmartArtType.DescendingBlockList;
      case "hList3":
        return SmartArtType.TableList;
      case "process4":
        return SmartArtType.SegmentedProcess;
      case "VerticalCurvedList":
        return SmartArtType.VerticalCurvedList;
      case "pyramid2":
        return SmartArtType.PyramidList;
      case "target3":
        return SmartArtType.TargetList;
      case "VerticalCircleList":
        return SmartArtType.VerticalCircleList;
      case "hierarchy4":
        return SmartArtType.TableHierarchy;
      case "process1":
        return SmartArtType.BasicProcess;
      case "StepUpProcess":
        return SmartArtType.StepUpProcess;
      case "StepDownProcess":
        return SmartArtType.StepDownProcess;
      case "process3":
        return SmartArtType.AccentProcess;
      case "hProcess4":
        return SmartArtType.AlternatingFlow;
      case "hProcess9":
        return SmartArtType.ContinuousBlockProcess;
      case "IncreasingArrowsProcess":
        return SmartArtType.IncreasingArrowsProcess;
      case "hProcess3":
        return SmartArtType.ContinuousArrowProcess;
      case "hProcess6":
        return SmartArtType.ProcessArrows;
      case "CircleAccentTimeline":
        return SmartArtType.CircleAccentTimeLine;
      case "hProcess11":
        return SmartArtType.BasicTimeLine;
      case "chevron1":
        return SmartArtType.BasicChevronProcess;
      case "hChevron3":
        return SmartArtType.ClosedChevronProcess;
      case "lProcess3":
        return SmartArtType.ChevronList;
      case "SubStepProcess":
        return SmartArtType.SubStepProcess;
      case "PhasedProcess":
        return SmartArtType.PhasedProcess;
      case "RandomtoResultProcess":
        return SmartArtType.RandomToResultProcess;
      case "vProcess5":
        return SmartArtType.StaggeredProcess;
      case "lProcess1":
        return SmartArtType.ProcessList;
      case "CircleArrowProcess":
        return SmartArtType.CircleArrowProcess;
      case "process5":
        return SmartArtType.BasicBendingProcess;
      case "bProcess4":
        return SmartArtType.VerticalBendingProcess;
      case "AscendingPictureAccentProcess":
        return SmartArtType.AscendingPictureAccentprocess;
      case "arrow2":
        return SmartArtType.UpwardArrow;
      case "DescendingProcess":
        return SmartArtType.DescendingProcess;
      case "bProcess2":
        return SmartArtType.CircularBendingProcess;
      case "equation1":
        return SmartArtType.Equation;
      case "equation2":
        return SmartArtType.VerticalEquation;
      case "funnel1":
        return SmartArtType.Funnel;
      case "gear1":
        return SmartArtType.Gear;
      case "arrow6":
        return SmartArtType.ArrowRibbon;
      case "arrow4":
        return SmartArtType.OpposingArrows;
      case "arrow5":
        return SmartArtType.ConvergingArrows;
      case "arrow1":
        return SmartArtType.DivergingArrows;
      case "bProcess3":
        return SmartArtType.RepeatingBendingProcess;
      case "process2":
        return SmartArtType.VerticalProcess;
      case "cycle2":
        return SmartArtType.BasicCycle;
      case "cycle1":
        return SmartArtType.TextCycle;
      case "cycle5":
        return SmartArtType.BlockCycle;
      case "cycle6":
        return SmartArtType.NondirectionalCycle;
      case "cycle3":
        return SmartArtType.ContinuousCycle;
      case "cycle7":
        return SmartArtType.MultiDirectionalCycle;
      case "cycle8":
        return SmartArtType.SegmentedCycle;
      case "chart3":
        return SmartArtType.BasicPie;
      case "radial6":
        return SmartArtType.RadialCycle;
      case "radial1":
        return SmartArtType.BasicRadial;
      case "radial5":
        return SmartArtType.DivergingRadial;
      case "radial3":
        return SmartArtType.RadialVenn;
      case "RadialCluster":
        return SmartArtType.RadialCluster;
      case "orgChart1":
        return SmartArtType.OrganizationChart;
      case "NameandTitleOrganizationalChart":
        return SmartArtType.NameAndTitleOrganizationChart;
      case "HalfCircleOrganizationChart":
        return SmartArtType.HalfCircleOrganizationChart;
      case "CirclePictureHierarchy":
        return SmartArtType.CirclePictureHierarchy;
      case "hirearchy1":
        return SmartArtType.Hierarchy;
      case "hierarchy6":
        return SmartArtType.LabeledHierarchy;
      case "HorizontalOrganizationChart":
        return SmartArtType.HorizontalOrganizationChart;
      case "HorizontalMultiLevelHierarchy":
        return SmartArtType.HorizontalMulti_levelHierarchy;
      case "hierarchy2":
        return SmartArtType.HorizontalHierarchy;
      case "hierarchy5":
        return SmartArtType.HorizontalLabeledHierarchy;
      case "balance1":
        return SmartArtType.Balance;
      case "CircleRelationship":
        return SmartArtType.CircleRelationship;
      case "HexagonCluster":
        return SmartArtType.HexagonCluster;
      case "OpposingIdeas":
        return SmartArtType.OpposingIdeas;
      case "PlusandMinus":
        return SmartArtType.PlusAndMinus;
      case "ReverseList":
        return SmartArtType.ReverseList;
      case "arrow3":
        return SmartArtType.CounterBalanceArrows;
      case "pyramid4":
        return SmartArtType.SegmentedPyramid;
      case "target2":
        return SmartArtType.NestedTarget;
      case "radial4":
        return SmartArtType.ConvergingRadial;
      case "radial2":
        return SmartArtType.RadialList;
      case "target1":
        return SmartArtType.BasicTarget;
      case "venn1":
        return SmartArtType.BasicVenn;
      case "venn3":
        return SmartArtType.LinearVenn;
      case "venn2":
        return SmartArtType.StackedVenn;
      case "hierarchy3":
        return SmartArtType.HierarchyList;
      case "matrix3":
        return SmartArtType.BasicMatrix;
      case "matrix1":
        return SmartArtType.TitledMatrix;
      case "matrix2":
        return SmartArtType.GridMatrix;
      case "cycle4":
        return SmartArtType.CycleMatrix;
      case "AccentedPicture":
        return SmartArtType.AccentedPicture;
      case "CircularPictureCallout":
        return SmartArtType.CircularPictureCallOut;
      case "SnapshotPictureList":
        return SmartArtType.SnapshotPictureList;
      case "SpiralPicture":
        return SmartArtType.SpiralPicture;
      case "CaptionedPictures":
        return SmartArtType.CaptionedPictures;
      case "BendingPictureCaption":
        return SmartArtType.BendingPictureCaption;
      case "BendingPictureSemiTransparentText":
        return SmartArtType.BendingPictureSemiTransparentText;
      case "BendingPictureBlocks":
        return SmartArtType.BendingPictureBlocks;
      case "BendingPictureCaptionList":
        return SmartArtType.BendingPictureCaptionList;
      case "TitledPictureBlocks":
        return SmartArtType.TitledPictureBlocks;
      case "PictureGrid":
        return SmartArtType.PictureGrid;
      case "PictureAccentBlocks":
        return SmartArtType.PictureAccentBlocks;
      case "AlternatingPictureCircles":
        return SmartArtType.AlternatingPictureCircles;
      case "TitlePictureLineup":
        return SmartArtType.TitlePictureLineup;
      case "PictureLineup":
        return SmartArtType.PictureLineUp;
      case "FramedTextPicture":
        return SmartArtType.FramedTextPicture;
      case "BubblePictureList":
        return SmartArtType.BubblePictureList;
      case "hProcess10":
        return SmartArtType.PictureAccentProcess;
      case "pyramid1":
        return SmartArtType.BasicPyramid;
      case "pyramid3":
        return SmartArtType.InvertedPyramid;
      default:
        return SmartArtType.BasicBlockList;
    }
  }

  internal static SmartArtConnectionType GetSmartArtConnectionType(string connectionType)
  {
    switch (connectionType)
    {
      case "parOf":
        return SmartArtConnectionType.ParentOf;
      case "presOf":
        return SmartArtConnectionType.PresentationOf;
      case "presParOf":
        return SmartArtConnectionType.PresentationParentOf;
      case "unknownRelationship":
        return SmartArtConnectionType.UnknownRelationship;
      default:
        return SmartArtConnectionType.ParentOf;
    }
  }

  internal static string ToString(SmartArtConnectionType smartArtConnectionType)
  {
    switch (smartArtConnectionType)
    {
      case SmartArtConnectionType.ParentOf:
        return "parOf";
      case SmartArtConnectionType.PresentationOf:
        return "presOf";
      case SmartArtConnectionType.PresentationParentOf:
        return "presParOf";
      case SmartArtConnectionType.UnknownRelationship:
        return "unknownRelationship";
      default:
        return "parOf";
    }
  }

  internal static SmartArtChildOrderType GetSmartArtChildOrder(string childOrder)
  {
    return childOrder == "t" ? SmartArtChildOrderType.Top : SmartArtChildOrderType.Bottom;
  }

  internal static SmartArtAlgorithmType GetAlgorithmType(string algorithmType)
  {
    switch (algorithmType)
    {
      case "composite":
        return SmartArtAlgorithmType.Composite;
      case "conn":
        return SmartArtAlgorithmType.Connector;
      case "cycle":
        return SmartArtAlgorithmType.Cycle;
      case "hierChild":
        return SmartArtAlgorithmType.HierarchyChild;
      case "hierRoot":
        return SmartArtAlgorithmType.HierarchyRoot;
      case "pyra":
        return SmartArtAlgorithmType.Pyramid;
      case "lin":
        return SmartArtAlgorithmType.Linear;
      case "sp":
        return SmartArtAlgorithmType.Space;
      case "tx":
        return SmartArtAlgorithmType.Text;
      case "snake":
        return SmartArtAlgorithmType.Snake;
      default:
        return SmartArtAlgorithmType.Composite;
    }
  }

  internal static SmartArtParameterId GetParameterId(string parameterId)
  {
    switch (parameterId)
    {
      case "horzAlign":
        return SmartArtParameterId.HorizontalAlignment;
      case "vertAlign":
        return SmartArtParameterId.VerticalAlignment;
      case "chDir":
        return SmartArtParameterId.ChildDirection;
      case "chAlign":
        return SmartArtParameterId.ChildAlignment;
      case "secChAlign":
        return SmartArtParameterId.SecondaryChildAlignment;
      case "linDir":
        return SmartArtParameterId.LinearDirection;
      case "secLinDir":
        return SmartArtParameterId.SecondaryLinearDirection;
      case "stElem":
        return SmartArtParameterId.StartElement;
      case "bendPt":
        return SmartArtParameterId.BendPoint;
      case "connRout":
        return SmartArtParameterId.ConnectionRoute;
      case "begSty":
        return SmartArtParameterId.BeginningArrowHeadStyle;
      case "endSty":
        return SmartArtParameterId.EndStyle;
      case "dim":
        return SmartArtParameterId.ConnectorDimension;
      case "rotPath":
        return SmartArtParameterId.RotationPath;
      case "ctrShpMap":
        return SmartArtParameterId.CenterShapeMapping;
      case "nodeHorzAlign":
        return SmartArtParameterId.NodeHorizontalAlignment;
      case "nodeVertAlign":
        return SmartArtParameterId.NodeVerticalAlignment;
      case "fallback":
        return SmartArtParameterId.FallBackScale;
      case "txDir":
        return SmartArtParameterId.TextDirection;
      case "pyraAcctPos":
        return SmartArtParameterId.PyramidAccentPosition;
      case "pyraAcctTxMar":
        return SmartArtParameterId.PyramidAccentTextMargin;
      case "txBlDir":
        return SmartArtParameterId.TextBlockDirection;
      case "txAnchorHorz":
        return SmartArtParameterId.TextAnchorHorizontal;
      case "txAnchorVert":
        return SmartArtParameterId.TextAnchorVertical;
      case "txAnchorHorzCh":
        return SmartArtParameterId.TextAnchorHorizontalWithChildren;
      case "txAnchorVertCh":
        return SmartArtParameterId.TextAnchorVerticalWithChildren;
      case "parTxLTRAlign":
        return SmartArtParameterId.ParentTextLeftToRightAlignment;
      case "parTxRTLAlign":
        return SmartArtParameterId.ParentTextRightToLeftAlignment;
      case "shpTxLTRAlignCh":
        return SmartArtParameterId.ShapeTextLeftToRightAlignment;
      case "shpTxRTLAlignCh":
        return SmartArtParameterId.ShapeTextRightToLeftAlignment;
      case "autoTxRot":
        return SmartArtParameterId.AutoTextRotation;
      case "grDir":
        return SmartArtParameterId.GrowDirection;
      case "flowDir":
        return SmartArtParameterId.FlowDirection;
      case "contDir":
        return SmartArtParameterId.ContinueDirection;
      case "bkpt":
        return SmartArtParameterId.BreakPoint;
      case "off":
        return SmartArtParameterId.Offset;
      case "hierAlign":
        return SmartArtParameterId.HierarchyAlignment;
      case "bkPtFixedVal":
        return SmartArtParameterId.BreakPointFixedValue;
      case "stBulletLvl":
        return SmartArtParameterId.StartBulletsAtLevel;
      case "stAng":
        return SmartArtParameterId.StartAngle;
      case "spanAng":
        return SmartArtParameterId.SpanAngle;
      case "ar":
        return SmartArtParameterId.AspectRatio;
      case "lnSpPar":
        return SmartArtParameterId.LineSpacingAfterParent;
      case "lnSpAfParP":
        return SmartArtParameterId.LineSpacingAfterParentParagraph;
      case "lnSpCh":
        return SmartArtParameterId.LineSpacingAfterChildren;
      case "lnSpAfChP":
        return SmartArtParameterId.LineSpacingAfterChildrenParagraph;
      case "rtShortDist":
        return SmartArtParameterId.RouteShortestDistance;
      case "alignTx":
        return SmartArtParameterId.TextAlignment;
      case "pyraLvlNode":
        return SmartArtParameterId.PyramidLevelNode;
      case "pyraAcctBkgdNode":
        return SmartArtParameterId.PyramidAccentBackgroundNode;
      case "pyraAcctTxNode":
        return SmartArtParameterId.PyramidAccentTextNode;
      case "srcNode":
        return SmartArtParameterId.SourceNode;
      case "dstNode":
        return SmartArtParameterId.DestinationNode;
      case "begPts":
        return SmartArtParameterId.BeginningPoints;
      case "endPts":
        return SmartArtParameterId.EndPoints;
      default:
        return SmartArtParameterId.TextAlignment;
    }
  }

  internal static object GetParamArrowHeadStyle(string parameterValue)
  {
    switch (parameterValue)
    {
      case "arr":
        return (object) ParamArrowHeadStyle.ArrowheadPresent;
      case "auto":
        return (object) ParamArrowHeadStyle.Auto;
      case "noArr":
        return (object) ParamArrowHeadStyle.NoArrowhead;
      default:
        return (object) null;
    }
  }

  internal static object GetParamAutoTextRotation(string paramValue)
  {
    switch (paramValue)
    {
      case "grav":
        return (object) ParamAutoTextRotation.Gravity;
      case "none":
        return (object) ParamAutoTextRotation.None;
      case "upr":
        return (object) ParamAutoTextRotation.Upright;
      default:
        return (object) null;
    }
  }

  internal static object GetParamBendPoint(string paramValue)
  {
    switch (paramValue)
    {
      case "beg":
        return (object) ParamBendPoint.Beginning;
      case "def":
        return (object) ParamBendPoint.Default;
      case "end":
        return (object) ParamBendPoint.End;
      default:
        return (object) null;
    }
  }

  internal static object GetParamBreakPoint(string paramValue)
  {
    switch (paramValue)
    {
      case "bal":
        return (object) ParamBreakPoint.Balanced;
      case "endCnv":
        return (object) ParamBreakPoint.EndofCanvas;
      case "fixed":
        return (object) ParamBreakPoint.Fixed;
      default:
        return (object) null;
    }
  }

  internal static object GetParamCenterShapeMapping(string paramValue)
  {
    switch (paramValue)
    {
      case "fNode":
        return (object) ParamCenterShapeMapping.FirstNode;
      case "none":
        return (object) ParamCenterShapeMapping.None;
      default:
        return (object) null;
    }
  }

  internal static object GetParamChildAlignment(string paramValue)
  {
    switch (paramValue)
    {
      case "b":
        return (object) ParamChildAlignment.Bottom;
      case "l":
        return (object) ParamChildAlignment.Left;
      case "r":
        return (object) ParamChildAlignment.Right;
      case "t":
        return (object) ParamChildAlignment.Top;
      default:
        return (object) null;
    }
  }

  internal static object GetParamChildDirection(string paramValue)
  {
    switch (paramValue)
    {
      case "horz":
        return (object) ParamChildDirection.Horizontal;
      case "vert":
        return (object) ParamChildDirection.Vertical;
      default:
        return (object) null;
    }
  }

  internal static object GetParamConnectorDimension(string paramValue)
  {
    switch (paramValue)
    {
      case "1D":
        return (object) ParamConnectorDimension.OneDimension;
      case "2D":
        return (object) ParamConnectorDimension.TwoDimensions;
      case "cust":
        return (object) ParamConnectorDimension.Custom;
      default:
        return (object) null;
    }
  }

  internal static object GetParamContinueDirection(string paramValue)
  {
    return paramValue == "revDir" ? (object) ParamContinueDirection.ReverseDirection : (object) ParamContinueDirection.SameDirection;
  }

  internal static object GetParamFallBackScale(string paramValue)
  {
    return paramValue == "1D" ? (object) ParamFallbackDimension.OneDimension : (object) ParamFallbackDimension.TwoDimensions;
  }

  internal static object GetParamFlowDirection(string paramValue)
  {
    return paramValue == "col" ? (object) ParamFlowDirection.Column : (object) ParamFlowDirection.Row;
  }

  internal static object GetParamGrowDirection(string paramValue)
  {
    switch (paramValue)
    {
      case "bL":
        return (object) ParamGrowDirection.BottomLeft;
      case "bR":
        return (object) ParamGrowDirection.BottomRight;
      case "tL":
        return (object) ParamGrowDirection.TopLeft;
      case "tR":
        return (object) ParamGrowDirection.TopRight;
      default:
        return (object) null;
    }
  }

  internal static object GetParamHierarchyAlignment(string paramValue)
  {
    switch (paramValue)
    {
      case "bCtrCh":
        return (object) ParamHierarchyAlignment.BottomCenterChild;
      case "bCtrDes":
        return (object) ParamHierarchyAlignment.BottomCenterDescendant;
      case "bL":
        return (object) ParamHierarchyAlignment.BottomLeft;
      case "bR":
        return (object) ParamHierarchyAlignment.BottomLeft;
      case "lB":
        return (object) ParamHierarchyAlignment.LeftBottom;
      case "lCtrCh":
        return (object) ParamHierarchyAlignment.LeftCenterChild;
      case "lCtrDes":
        return (object) ParamHierarchyAlignment.LeftCenterDescendant;
      case "lT":
        return (object) ParamHierarchyAlignment.LeftTop;
      case "rB":
        return (object) ParamHierarchyAlignment.RightBottom;
      case "rCtrCh":
        return (object) ParamHierarchyAlignment.RightCenterChildren;
      case "rCtrDes":
        return (object) ParamHierarchyAlignment.RightCenterDescendants;
      case "rT":
        return (object) ParamHierarchyAlignment.RightTop;
      case "tCtrCh":
        return (object) ParamHierarchyAlignment.TopCenterChildren;
      case "tCtrDes":
        return (object) ParamHierarchyAlignment.TopCenterDescendants;
      case "tL":
        return (object) ParamHierarchyAlignment.TopLeft;
      case "tR":
        return (object) ParamHierarchyAlignment.TopRight;
      default:
        return (object) null;
    }
  }

  internal static object GetParamLinearDirection(string paramValue)
  {
    switch (paramValue)
    {
      case "fromB":
        return (object) ParamLinearDirection.FromBottom;
      case "fromL":
        return (object) ParamLinearDirection.FromLeft;
      case "fromT":
        return (object) ParamLinearDirection.FromTop;
      case "fromR":
        return (object) ParamLinearDirection.FromRight;
      default:
        return (object) null;
    }
  }

  internal static object GetParamNodeHorizontalAlignment(string paramValue)
  {
    switch (paramValue)
    {
      case "ctr":
        return (object) ParamNodeHorizontalAlignment.Center;
      case "l":
        return (object) ParamNodeHorizontalAlignment.Left;
      default:
        return (object) ParamNodeHorizontalAlignment.Right;
    }
  }

  internal static object GetParamNodeVerticalAlignment(string paramValue)
  {
    switch (paramValue)
    {
      case "b":
        return (object) ParamNodeVerticalAlignment.Bottom;
      case "mod":
        return (object) ParamNodeVerticalAlignment.Middle;
      default:
        return (object) ParamNodeVerticalAlignment.Top;
    }
  }

  internal static object GetParamOffset(string paramValue)
  {
    return paramValue == "ctr" ? (object) ParamOffset.Center : (object) ParamOffset.Offset;
  }

  internal static object GetParamPyramidAccentPosition(string paramValue)
  {
    return paramValue == "aft" ? (object) ParamPyramidAccentPosition.PyramidAccentAfter : (object) ParamPyramidAccentPosition.Before;
  }

  internal static object GetParamPyramidAccentTextMargin(string paramValue)
  {
    return paramValue == "stack" ? (object) ParamPyramidAccentTextMargin.Stack : (object) ParamPyramidAccentTextMargin.Step;
  }

  internal static object GetParamRotationPath(string paramValue)
  {
    return paramValue == "alongPath" ? (object) ParamRotationPath.AlongPath : (object) ParamRotationPath.None;
  }

  internal static object GetParamSecondaryChildAlignment(string paramValue)
  {
    switch (paramValue)
    {
      case "b":
        return (object) ParamSecondaryChildAlignment.Bottom;
      case "r":
        return (object) ParamSecondaryChildAlignment.Right;
      case "l":
        return (object) ParamSecondaryChildAlignment.Left;
      case "t":
        return (object) ParamSecondaryChildAlignment.Top;
      case "none":
        return (object) ParamSecondaryChildAlignment.None;
      default:
        return (object) null;
    }
  }

  internal static object GetParamSecondaryLinearDirection(string paramValue)
  {
    switch (paramValue)
    {
      case "fromB":
        return (object) ParamLinearDirection.FromBottom;
      case "fromL":
        return (object) ParamLinearDirection.FromLeft;
      case "fromT":
        return (object) ParamLinearDirection.FromTop;
      case "fromR":
        return (object) ParamLinearDirection.FromRight;
      default:
        return (object) null;
    }
  }

  internal static object GetParamStartElement(string paramValue)
  {
    return paramValue == "node" ? (object) ParamStartingElement.Node : (object) ParamStartingElement.Transition;
  }

  internal static object GetParamTextAnchorHorizontal(string paramValue)
  {
    return paramValue == "ctr" ? (object) ParamTextAnchorHorizontal.Center : (object) ParamTextAnchorHorizontal.None;
  }

  internal static object GetParamTextAnchorVertcal(string paramValue)
  {
    switch (paramValue)
    {
      case "b":
        return (object) ParamTextAnchorVertical.Bottom;
      case "mid":
        return (object) ParamTextAnchorVertical.Middle;
      default:
        return (object) ParamTextAnchorVertical.Top;
    }
  }

  internal static object GetParamTextBlockDirection(string paramValue)
  {
    return paramValue == "horz" ? (object) ParamTextBlockDirection.Horizontal : (object) ParamTextBlockDirection.VerticalDirection;
  }

  internal static object GetParamTextDirection(string paramValue)
  {
    return paramValue == "fromT" ? (object) ParamTextDirection.FromBottom : (object) ParamTextDirection.FromTop;
  }

  internal static object GetParamVerticalAlignment(string paramValue)
  {
    switch (paramValue)
    {
      case "b":
        return (object) ParamVerticalAlignment.Bottom;
      case "mid":
        return (object) ParamVerticalAlignment.Middle;
      case "t":
        return (object) ParamVerticalAlignment.Top;
      default:
        return (object) ParamVerticalAlignment.None;
    }
  }

  internal static SmartArtAxisType GetAxisType(string axisValue)
  {
    switch (axisValue)
    {
      case "ancst":
        return SmartArtAxisType.Ancestor;
      case "ancstOrSelf":
        return SmartArtAxisType.AncestororSelf;
      case "ch":
        return SmartArtAxisType.Child;
      case "des":
        return SmartArtAxisType.Descendant;
      case "desOrSelf":
        return SmartArtAxisType.DescendantorSelf;
      case "follow":
        return SmartArtAxisType.Follow;
      case "followSib":
        return SmartArtAxisType.FollowSibling;
      case "par":
        return SmartArtAxisType.Parent;
      case "preced":
        return SmartArtAxisType.Preceding;
      case "precedSib":
        return SmartArtAxisType.PrecedingSibling;
      case "root":
        return SmartArtAxisType.Root;
      case "self":
        return SmartArtAxisType.Self;
      default:
        return SmartArtAxisType.None;
    }
  }

  internal static SmartArtConstraintType GetSmartArtConstraintType(string constraintType)
  {
    switch (constraintType)
    {
      case "w":
        return SmartArtConstraintType.Width;
      case "h":
        return SmartArtConstraintType.Height;
      case "sp":
        return SmartArtConstraintType.Space;
      case "primFontSz":
        return SmartArtConstraintType.PrimaryFontSize;
      case "lMarg":
        return SmartArtConstraintType.LeftMargin;
      case "rMarg":
        return SmartArtConstraintType.RightMargin;
      case "tMarg":
        return SmartArtConstraintType.TopMargin;
      case "bMarg":
        return SmartArtConstraintType.BottomMargin;
      default:
        return SmartArtConstraintType.Width;
    }
  }

  internal static SmartArtConstraintRelationShip GetConstraintRelationShip(string constraintRelation)
  {
    switch (constraintRelation)
    {
      case "des":
        return SmartArtConstraintRelationShip.Descendant;
      case "ch":
        return SmartArtConstraintRelationShip.Child;
      default:
        return SmartArtConstraintRelationShip.Self;
    }
  }

  internal static SplitterBarState GetSplitterBarState(string horizontalBarState)
  {
    switch (horizontalBarState)
    {
      case "minimized":
        return SplitterBarState.Minimized;
      case "maximized":
        return SplitterBarState.Maximized;
      default:
        return SplitterBarState.Restored;
    }
  }

  internal static string ToString(SplitterBarState splitterBarState)
  {
    if (splitterBarState == SplitterBarState.Minimized)
      return "minimized";
    return splitterBarState == SplitterBarState.Maximized ? "maximized" : "restored";
  }

  internal static string FormatPathForZipArchive(string path)
  {
    if (path.StartsWith("/ppt"))
      return path.TrimStart('/');
    if (path.StartsWith("ppt"))
      return path;
    return path.StartsWith("..") ? "ppt" + path.Remove(0, 2) : "ppt/" + path;
  }
}
