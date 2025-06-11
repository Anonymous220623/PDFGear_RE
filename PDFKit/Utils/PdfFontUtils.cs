// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PdfFontUtils
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using PDFKit.Contents.Utils;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace PDFKit.Utils;

public static class PdfFontUtils
{
  private static PdfDocument innerDoc;
  private const string GlobalUI = "#GLOBAL USER INTERFACE";
  private const string GlobalSerif = "#GLOBAL SERIF";
  private static bool? hasWinRTSupport;
  private static FontFamily FontFamilyGlobalUI = new FontFamily("#GLOBAL USER INTERFACE");
  private static Func<FontFamilyMap, int, bool> fontFamilyMapInRangeFunc;
  private static Func<FamilyTypeface, int, bool> familyTypefaceContainsCharacterFunc;
  private static Func<XmlLanguage, CultureInfo, bool> matchCultureFunc;
  private static Dictionary<string, System.Collections.Generic.IReadOnlyList<FontFamily>> targetFontFamilies = new Dictionary<string, System.Collections.Generic.IReadOnlyList<FontFamily>>();
  private static Dictionary<int, FontStockNames> fontStockNamesIndexes;
  private static Dictionary<string, PdfFontUtils.FontFamilyCreateType> fontFamilyTypeCache = new Dictionary<string, PdfFontUtils.FontFamilyCreateType>();
  private static HashSet<string> italicStockFonts;
  private static HashSet<string> obliqueStockFonts;
  private static HashSet<string> boldStockFonts;
  private static System.Collections.Generic.IReadOnlyList<FontCharSet> fontCharSets;
  private static Dictionary<string, string> cultureNameTypeFontNameDict = new Dictionary<string, string>();

  private static void Init()
  {
    if (PdfFontUtils.innerDoc != null)
      return;
    lock (nameof (PdfFontUtils))
    {
      if (PdfFontUtils.innerDoc == null)
      {
        PdfFontUtils.innerDoc = PdfDocument.CreateNew();
        PdfFontUtils.InitDefaultFonts(PdfFontUtils.innerDoc);
      }
    }
  }

  private static void InitDefaultFonts(PdfDocument doc)
  {
  }

  private static void InitStockFontDict()
  {
    if (PdfFontUtils.fontStockNamesIndexes != null)
      return;
    lock (typeof (PdfFontUtils))
    {
      if (PdfFontUtils.fontStockNamesIndexes == null)
        PdfFontUtils.fontStockNamesIndexes = Enum.GetValues(typeof (FontStockNames)).OfType<FontStockNames>().Select<FontStockNames, (int, FontStockNames)>((Func<FontStockNames, (int, FontStockNames)>) (c => ((int) c, c))).GroupBy<(int, FontStockNames), int>((Func<(int, FontStockNames), int>) (c => c.Item1)).Select<IGrouping<int, (int, FontStockNames)>, (int, FontStockNames)>((Func<IGrouping<int, (int, FontStockNames)>, (int, FontStockNames)>) (c => (c.Key, c.FirstOrDefault<(int, FontStockNames)>().Item2))).ToDictionary<(int, FontStockNames), int, FontStockNames>((Func<(int, FontStockNames), int>) (c => c.Key), (Func<(int, FontStockNames), FontStockNames>) (c => c.c));
    }
  }

  public static bool CheckStockFontSupport(string fontname, string text)
  {
    if (string.IsNullOrEmpty(fontname))
      return false;
    if (string.IsNullOrEmpty(fontname))
      return true;
    PdfFontUtils.Init();
    PdfFont font = (PdfFont) null;
    string str = fontname?.Trim();
    FontStockNames? nullable = new FontStockNames?();
    int result;
    if (str.Length > 0 && str[0] >= '0' && str[0] <= '9' && int.TryParse(str, out result))
    {
      PdfFontUtils.InitStockFontDict();
      FontStockNames fontStockNames;
      if (PdfFontUtils.fontStockNamesIndexes.TryGetValue(result, out fontStockNames))
        nullable = new FontStockNames?(fontStockNames);
    }
    if (!nullable.HasValue)
    {
      PdfFontUtils.InitStockFontDict();
      FontStockNames fontStockNames;
      if (!string.IsNullOrEmpty(str) && EnumHelper<FontStockNames>.NameValueDict.TryGetValue(str, out fontStockNames))
        nullable = new FontStockNames?(fontStockNames);
    }
    if (nullable.HasValue)
    {
      try
      {
        font = PdfFont.CreateStock(PdfFontUtils.innerDoc, nullable.Value);
      }
      catch
      {
      }
    }
    else
    {
      try
      {
        font = PdfFont.CreateStock(PdfFontUtils.innerDoc, str);
      }
      catch
      {
      }
    }
    return font != null && PdfTextObject.Create(text, 0.0f, 0.0f, font, 1f).TextUnicode == text;
  }

  public static PdfFont CreateFont(
    PdfDocument doc,
    FontFamily fontFamily,
    System.Windows.FontWeight fontWeight,
    FontStyle fontStyle,
    FontCharSet charSet)
  {
    return PdfFontUtils.CreateFont(doc, fontFamily?.Source, fontWeight, fontStyle, charSet);
  }

  public static PdfFont CreateFont(
    PdfDocument doc,
    string fontFamilyName,
    System.Windows.FontWeight fontWeight,
    FontStyle fontStyle,
    FontCharSet charSet)
  {
    PdfFont font;
    if (PdfFontUtils.TryCreateFont(doc, fontFamilyName, fontWeight, fontStyle, charSet, out font))
      return font;
    PdfFontUtils.InitFontStyleStockFonts();
    if (fontStyle == FontStyles.Italic)
      return PdfFont.CreateStock(doc, PdfFontUtils.italicStockFonts.First<string>());
    if (fontStyle == FontStyles.Oblique)
      return PdfFont.CreateStock(doc, PdfFontUtils.obliqueStockFonts.First<string>());
    return fontWeight == FontWeights.Bold ? PdfFont.CreateStock(doc, PdfFontUtils.boldStockFonts.First<string>()) : PdfFont.CreateStock(doc, FontStockNames.Arial);
  }

  private static bool TryGetFontFamilyCharSet(string fontFamilyName, out FontCharSet charSet)
  {
    charSet = FontCharSet.ANSI_CHARSET;
    if (string.IsNullOrEmpty(fontFamilyName))
      return false;
    FontFamily fontFamily = new FontFamily(fontFamilyName);
    CultureInfo currentCulture = CultureInfo.CurrentCulture;
    FontFamilyMap fontFamilyMap1 = (FontFamilyMap) null;
    FontFamilyMap fontFamilyMap2 = (FontFamilyMap) null;
    foreach (FontFamilyMap familyMap in PdfFontUtils.FontFamilyGlobalUI.FamilyMaps)
    {
      if (!string.IsNullOrEmpty(familyMap.Target) && familyMap.Language != null)
      {
        if (familyMap.Target.Contains(fontFamily.Source))
        {
          if (PdfFontUtils.MatchCulture(familyMap.Language, currentCulture))
            fontFamilyMap2 = familyMap;
          fontFamilyMap1 = familyMap;
          if (fontFamilyMap2 != null)
            break;
        }
        if (fontFamily.FamilyNames != null)
        {
          foreach (KeyValuePair<XmlLanguage, string> familyName in fontFamily.FamilyNames)
          {
            if (familyMap.Target.Contains(fontFamily.Source))
            {
              if (PdfFontUtils.MatchCulture(familyMap.Language, currentCulture))
                fontFamilyMap2 = familyMap;
              fontFamilyMap1 = familyMap;
              if (fontFamilyMap2 != null)
                break;
            }
          }
        }
      }
    }
    if (fontFamilyMap1 != null)
    {
      FontCharSet[] recommendCharSets = PdfFontUtils.GetRecommendCharSets(CultureInfo.GetCultureInfoByIetfLanguageTag(fontFamilyMap1.Language.IetfLanguageTag));
      if (recommendCharSets.Length != 0)
      {
        charSet = recommendCharSets[0];
        return true;
      }
    }
    return false;
  }

  private static bool TryCreateFont(
    PdfDocument doc,
    string fontFamilyName,
    System.Windows.FontWeight fontWeight,
    FontStyle fontStyle,
    FontCharSet charSet,
    out PdfFont font)
  {
    font = (PdfFont) null;
    PdfFontUtils.FontFamilyCreateType familyCreateType;
    if (PdfFontUtils.fontFamilyTypeCache.TryGetValue(fontFamilyName, out familyCreateType))
    {
      if (familyCreateType == PdfFontUtils.FontFamilyCreateType.Unsupported)
        return false;
    }
    else
      familyCreateType = PdfFontUtils.FontFamilyCreateType.Stock;
    if (familyCreateType == PdfFontUtils.FontFamilyCreateType.Stock)
    {
      bool flag = false;
      string str = fontFamilyName?.Trim();
      FontStockNames? nullable = new FontStockNames?();
      int result;
      if (str != null && str.Length > 0 && str[0] >= '0' && str[0] <= '9' && int.TryParse(str, out result))
      {
        PdfFontUtils.InitStockFontDict();
        FontStockNames fontStockNames;
        if (PdfFontUtils.fontStockNamesIndexes.TryGetValue(result, out fontStockNames))
          nullable = new FontStockNames?(fontStockNames);
      }
      if (!nullable.HasValue)
      {
        PdfFontUtils.InitStockFontDict();
        FontStockNames fontStockNames;
        if (!string.IsNullOrEmpty(str) && EnumHelper<FontStockNames>.NameValueDict.TryGetValue(str, out fontStockNames))
          nullable = new FontStockNames?(fontStockNames);
      }
      if (nullable.HasValue)
      {
        try
        {
          font = PdfFont.CreateStock(doc, nullable.Value);
          flag = true;
        }
        catch
        {
        }
      }
      else
      {
        try
        {
          font = PdfFont.CreateStock(doc, str);
          flag = true;
        }
        catch
        {
        }
      }
      if (!flag)
        familyCreateType = PdfFontUtils.FontFamilyCreateType.Standard;
    }
    if (familyCreateType == PdfFontUtils.FontFamilyCreateType.Standard)
    {
      bool flag = false;
      try
      {
        font = PdfFont.CreateStandardFont(doc, fontFamilyName, 1);
        flag = true;
      }
      catch
      {
      }
      if (!flag)
        familyCreateType = PdfFontUtils.FontFamilyCreateType.DefaultCreate;
    }
    if (font != null)
    {
      PdfFontUtils.fontFamilyTypeCache[fontFamilyName] = familyCreateType;
      return true;
    }
    string str1 = fontFamilyName;
    Patagames.Pdf.LOGFONT.FontPitchAndFamily fontPitchAndFamily = Patagames.Pdf.LOGFONT.FontPitchAndFamily.FF_ROMAN;
    Patagames.Pdf.Enums.FontWeight weight = (Patagames.Pdf.Enums.FontWeight) fontWeight.ToOpenTypeWeight();
    FontCharSet charSet1 = charSet;
    FontFlags flags = (FontFlags) 0;
    if (familyCreateType == PdfFontUtils.FontFamilyCreateType.DefaultCreate)
    {
      try
      {
        string faceName = str1;
        if (fontWeight == FontWeights.Bold)
        {
          weight = Patagames.Pdf.Enums.FontWeight.FW_DONTCARE;
          if (weight == Patagames.Pdf.Enums.FontWeight.FW_BOLD || fontStyle != FontStyles.Normal)
          {
            faceName += ",";
            if (weight == Patagames.Pdf.Enums.FontWeight.FW_BOLD)
              faceName += "Bold";
            if (fontStyle == FontStyles.Italic)
              faceName += "Italic";
            else if (fontStyle == FontStyles.Oblique)
              faceName += "Oblique";
          }
        }
        font = PdfFont.CreateFont(doc, faceName, true, flags, weight, 0, charSet1, false);
      }
      catch
      {
        familyCreateType = PdfFontUtils.FontFamilyCreateType.Windows;
      }
    }
    if (font != null)
    {
      PdfFontUtils.fontFamilyTypeCache[fontFamilyName] = familyCreateType;
      return true;
    }
    if (familyCreateType == PdfFontUtils.FontFamilyCreateType.Windows)
    {
      try
      {
        string str2 = str1;
        if (weight == Patagames.Pdf.Enums.FontWeight.FW_BOLD || fontStyle != FontStyles.Normal)
        {
          str2 += ",";
          if (weight == Patagames.Pdf.Enums.FontWeight.FW_BOLD)
            str2 += "Bold";
          if (fontStyle == FontStyles.Italic)
            str2 += "Italic";
          else if (fontStyle == FontStyles.Oblique)
            str2 += "Oblique";
        }
        Patagames.Pdf.LOGFONT logfont = new Patagames.Pdf.LOGFONT()
        {
          lfFaceName = str2,
          lfPitchAndFamily = fontPitchAndFamily,
          lfWeight = Patagames.Pdf.Enums.FontWeight.FW_DONTCARE,
          lfCharSet = charSet1,
          lfItalic = true
        };
        font = PdfFont.CreateWindowsFont(doc, logfont, false, false);
      }
      catch
      {
        familyCreateType = PdfFontUtils.FontFamilyCreateType.Unsupported;
      }
    }
    PdfFontUtils.fontFamilyTypeCache[fontFamilyName] = familyCreateType;
    return font != null;
  }

  private static void InitFontStyleStockFonts()
  {
    if (PdfFontUtils.italicStockFonts != null)
      return;
    lock (typeof (PdfFontUtils))
    {
      if (PdfFontUtils.italicStockFonts == null)
      {
        PdfFontUtils.boldStockFonts = new HashSet<string>()
        {
          "Arial,Bold",
          "Arial-Bold",
          "Arial-BoldMT",
          "ArialBold",
          "ArialMT,Bold",
          "ArialRoundedMTBold",
          "Courier,Bold",
          "Courier-Bold",
          "CourierBold",
          "CourierNew,Bold",
          "CourierNewPS-BoldMT",
          "CourierNew-Bold",
          "CourierNewBold",
          "CourierStd-Bold",
          "Helvetica,Bold",
          "Helvetica-Bold",
          "HelveticaBold",
          "Times-Bold",
          "TimesNewRoman,Bold",
          "TimesBold",
          "TimesNewRoman-Bold",
          "TimesNewRomanBold",
          "TimesNewRomanPS-Bold",
          "TimesNewRomanPS-BoldMT",
          "TimesNewRomanPSMT,Bold",
          "Arial,BoldItalic",
          "Arial-BoldItalic",
          "Arial-BoldItalicMT",
          "ArialBoldItalic",
          "ArialMT,BoldItalic",
          "CourierBoldItalic",
          "Courier,BoldItalic",
          "CourierNew,BoldItalic",
          "CourierNewBoldItalic",
          "CourierNewPS-BoldItalicMT",
          "CourierNew-BoldItalic",
          "Helvetica,BoldItalic",
          "Helvetica-BoldItalic",
          "HelveticaBoldItalic",
          "Times-BoldItalic",
          "TimesBoldItalic",
          "TimesNewRoman,BoldItalic",
          "TimesNewRoman-BoldItalic",
          "TimesNewRomanBoldItalic",
          "TimesNewRomanPS-BoldItalic",
          "TimesNewRomanPS-BoldItalicMT",
          "TimesNewRomanPSMT,BoldItalic",
          "Courier-BoldOblique",
          "CourierStd-BoldOblique",
          "Helvetica-BoldOblique"
        };
        PdfFontUtils.italicStockFonts = new HashSet<string>()
        {
          "Arial,BoldItalic",
          "Arial-BoldItalic",
          "Arial-BoldItalicMT",
          "ArialBoldItalic",
          "ArialMT,BoldItalic",
          "CourierBoldItalic",
          "Courier,BoldItalic",
          "CourierNew,BoldItalic",
          "CourierNewBoldItalic",
          "CourierNewPS-BoldItalicMT",
          "CourierNew-BoldItalic",
          "Helvetica,BoldItalic",
          "Helvetica-BoldItalic",
          "HelveticaBoldItalic",
          "Times-BoldItalic",
          "TimesBoldItalic",
          "TimesNewRoman,BoldItalic",
          "TimesNewRoman-BoldItalic",
          "TimesNewRomanBoldItalic",
          "TimesNewRomanPS-BoldItalic",
          "TimesNewRomanPS-BoldItalicMT",
          "TimesNewRomanPSMT,BoldItalic",
          "Arial,Italic",
          "Arial-Italic",
          "Arial-ItalicMT",
          "ArialItalic",
          "ArialMT,Italic",
          "Courier,Italic",
          "CourierItalic",
          "CourierNew,Italic",
          "CourierNew-Italic",
          "CourierNewItalic",
          "CourierNewPS-ItalicMT",
          "Helvetica,Italic",
          "Helvetica-Italic",
          "HelveticaItalic",
          "Times-Italic",
          "TimesItalic",
          "TimesNewRoman,Italic",
          "TimesNewRoman-Italic",
          "TimesNewRomanItalic",
          "TimesNewRomanPS-Italic",
          "TimesNewRomanPS-ItalicMT",
          "TimesNewRomanPSMT,Italic"
        };
        PdfFontUtils.obliqueStockFonts = new HashSet<string>()
        {
          "Courier-BoldOblique",
          "CourierStd-BoldOblique",
          "Helvetica-BoldOblique",
          "Courier-Oblique",
          "CourierStd-Oblique",
          "Helvetica-Oblique"
        };
      }
    }
  }

  private static FontCharSet[] GetCharSet(string text)
  {
    if (string.IsNullOrEmpty(text))
      return Array.Empty<FontCharSet>();
    List<FontCharSet> fontCharSetList = new List<FontCharSet>();
    FontCharSet[] charSets1 = PdfFontUtils.GetRecommendCharSets(CultureInfo.CurrentCulture);
    foreach (FontCharSet charSet in charSets1)
    {
      if (Test(charSet))
        fontCharSetList.Add(charSet);
    }
    if (PdfFontUtils.fontCharSets == null)
      PdfFontUtils.fontCharSets = (System.Collections.Generic.IReadOnlyList<FontCharSet>) EnumHelper<FontCharSet>.AllValues.Where<FontCharSet>((Func<FontCharSet, bool>) (c => c != FontCharSet.DEFAULT_CHARSET && c != FontCharSet.UNICODE_CHARSET && c != FontCharSet.SYMBOL_CHARSET)).ToArray<FontCharSet>();
    foreach (FontCharSet charSet in PdfFontUtils.fontCharSets.Where<FontCharSet>((Func<FontCharSet, bool>) (c => !((IEnumerable<FontCharSet>) charSets1).Contains<FontCharSet>(c))))
    {
      if (Test(charSet))
        fontCharSetList.Add(charSet);
    }
    if (fontCharSetList.Count == 0 && Test(FontCharSet.UNICODE_CHARSET))
      fontCharSetList.Add(FontCharSet.UNICODE_CHARSET);
    return fontCharSetList.ToArray();

    bool Test(FontCharSet charSet)
    {
      Encoding encoding = PdfFontUtils.GetEncoding(charSet);
      if (encoding != null)
      {
        try
        {
          byte[] numArray = ArrayPool<byte>.Shared.Rent(encoding.GetMaxByteCount(text.Length));
          int bytes = encoding.GetBytes(text, 0, text.Length, numArray, 0);
          string str = encoding.GetString(numArray, 0, bytes);
          ArrayPool<byte>.Shared.Return(numArray);
          return text == str;
        }
        catch
        {
        }
      }
      return false;
    }
  }

  private static Encoding GetEncoding(FontCharSet charSet)
  {
    try
    {
      FontCharSet fontCharSet = charSet;
      if ((uint) fontCharSet <= 177U)
      {
        if ((uint) fontCharSet <= 77U)
        {
          switch (fontCharSet)
          {
            case FontCharSet.ANSI_CHARSET:
              return Encoding.ASCII;
            case FontCharSet.DEFAULT_CHARSET:
              return Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.ANSICodePage);
            case FontCharSet.SYMBOL_CHARSET:
              return (Encoding) null;
            case FontCharSet.MAC_CHARSET:
              return Encoding.GetEncoding(10000);
          }
        }
        else
        {
          switch (fontCharSet)
          {
            case FontCharSet.SHIFTJIS_CHARSET:
              return Encoding.GetEncoding(932);
            case FontCharSet.HANGEUL_CHARSET:
              return Encoding.GetEncoding(949);
            case FontCharSet.JOHAB_CHARSET:
              return Encoding.GetEncoding(1361);
            case FontCharSet.GB2312_CHARSET:
              return Encoding.GetEncoding(936);
            case FontCharSet.CHINESEBIG5_CHARSET:
              return Encoding.GetEncoding(950);
            case FontCharSet.GREEK_CHARSET:
              return Encoding.GetEncoding(1253);
            case FontCharSet.TURKISH_CHARSET:
              return Encoding.GetEncoding(1254);
            case FontCharSet.VIETNAMESE_CHARSET:
              return Encoding.GetEncoding(1258);
            case FontCharSet.HEBREW_CHARSET:
              return Encoding.GetEncoding(1255);
          }
        }
      }
      else if ((uint) fontCharSet <= 204U)
      {
        switch (fontCharSet)
        {
          case FontCharSet.ARABIC_CHARSET:
            return Encoding.GetEncoding(1256);
          case FontCharSet.BALTIC_CHARSET:
            return Encoding.GetEncoding(1257);
          case FontCharSet.RUSSIAN_CHARSET:
            return Encoding.GetEncoding(1251);
        }
      }
      else
      {
        switch (fontCharSet)
        {
          case FontCharSet.THAI_CHARSET:
            return Encoding.GetEncoding(874);
          case FontCharSet.EASTEUROPE_CHARSET:
            return Encoding.GetEncoding(1250);
          case FontCharSet.UNICODE_CHARSET:
            return Encoding.GetEncoding(850);
        }
      }
    }
    catch
    {
    }
    return (Encoding) null;
  }

  private static FontCharSet[] GetRecommendCharSets(CultureInfo cultureInfo)
  {
    if (cultureInfo == null)
      return Array.Empty<FontCharSet>();
    switch (cultureInfo.TwoLetterISOLanguageName)
    {
      case "ar":
        return new FontCharSet[1]
        {
          FontCharSet.ARABIC_CHARSET
        };
      case "el":
        return new FontCharSet[1]
        {
          FontCharSet.GREEK_CHARSET
        };
      case "he":
        return new FontCharSet[1]
        {
          FontCharSet.HEBREW_CHARSET
        };
      case "ja":
        return new FontCharSet[1]
        {
          FontCharSet.SHIFTJIS_CHARSET
        };
      case "ko":
        return new FontCharSet[2]
        {
          FontCharSet.HANGEUL_CHARSET,
          FontCharSet.JOHAB_CHARSET
        };
      case "ru":
        return new FontCharSet[1]
        {
          FontCharSet.RUSSIAN_CHARSET
        };
      case "th":
        return new FontCharSet[1]
        {
          FontCharSet.THAI_CHARSET
        };
      case "tr":
        return new FontCharSet[1]
        {
          FontCharSet.TURKISH_CHARSET
        };
      case "vi":
        return new FontCharSet[1]
        {
          FontCharSet.VIETNAMESE_CHARSET
        };
      default:
        switch (cultureInfo.ThreeLetterWindowsLanguageName)
        {
          case "CHS":
          case "ZHI":
            return new FontCharSet[2]
            {
              FontCharSet.GB2312_CHARSET,
              FontCharSet.CHINESEBIG5_CHARSET
            };
          case "CHT":
          case "ZHH":
          case "ZHM":
            return new FontCharSet[2]
            {
              FontCharSet.CHINESEBIG5_CHARSET,
              FontCharSet.GB2312_CHARSET
            };
          case "CSY":
          case "HRV":
          case "HUN":
          case "PLK":
          case "ROM":
          case "SKY":
          case "SLV":
          case "SQI":
          case "SRL":
            return new FontCharSet[1]
            {
              FontCharSet.EASTEUROPE_CHARSET
            };
          case "ETI":
          case "LTC":
          case "LTH":
          case "LVI":
            return new FontCharSet[1]
            {
              FontCharSet.BALTIC_CHARSET
            };
          default:
            return Array.Empty<FontCharSet>();
        }
    }
  }

  public static System.Collections.Generic.IReadOnlyList<TextWithFallbackFontFamily> GetTextWithFallbackFonts(
    string str,
    string fontFamily,
    float fontSize,
    FS_RECTF? textBounds = null,
    System.Windows.FontWeight? fontWeight = null,
    FontStyle? fontStyle = null,
    CultureInfo cultureInfo = null)
  {
    if (cultureInfo == null)
      cultureInfo = CultureInfo.CurrentUICulture;
    return PdfFontUtils.GetTextWithFallbackFontsCore(str, fontFamily, fontSize, textBounds ?? new FS_RECTF(0.0f, 0.0f, float.MaxValue, float.MinValue), fontStyle ?? FontStyles.Normal, fontWeight ?? FontWeights.Normal, cultureInfo);
  }

  private static System.Collections.Generic.IReadOnlyList<TextWithFallbackFontFamily> GetTextWithFallbackFontsCore(
    string str,
    string fontFamily,
    float fontSize,
    FS_RECTF textBounds,
    FontStyle fontStyle,
    System.Windows.FontWeight fontWeight,
    CultureInfo cultureInfo = null)
  {
    if (string.IsNullOrEmpty(str))
      return (System.Collections.Generic.IReadOnlyList<TextWithFallbackFontFamily>) null;
    PdfFontUtils.Init();
    if (cultureInfo == null)
      cultureInfo = CultureInfo.CurrentUICulture;
    FontFamily family = (FontFamily) null;
    string str1 = "";
    FontCharSet[] charSet1 = PdfFontUtils.GetCharSet(str);
    FontCharSet charSet2 = charSet1.Length == 0 ? FontCharSet.DEFAULT_CHARSET : charSet1[0];
    PdfFont font;
    if (PdfFontUtils.TryCreateFont(PdfFontUtils.innerDoc, fontFamily, fontWeight, fontStyle, charSet2, out font))
      str1 = font.BaseFontName;
    else
      font = PdfFont.CreateStock(PdfFontUtils.innerDoc, FontStockNames.Arial);
    try
    {
      System.Collections.Generic.IReadOnlyList<TextWithFallbackFontFamily> withFallbackFonts = PdfFontUtils.GetTextWithFallbackFonts(str, font, fontSize, textBounds, cultureInfo);
      if (!(str1 != font.BaseFontName))
        return withFallbackFonts;
      family = new FontFamily(font.BaseFontName);
      return (System.Collections.Generic.IReadOnlyList<TextWithFallbackFontFamily>) withFallbackFonts.Select<TextWithFallbackFontFamily, TextWithFallbackFontFamily>((Func<TextWithFallbackFontFamily, TextWithFallbackFontFamily>) (c => c.FallbackFontFamily == null ? new TextWithFallbackFontFamily(c.Text, family, c.FontWeight, c.FontStyle, c.FontSize, c.ScaledFontSize, c.Bounds, c.Baseline, c.CharSet) : c)).ToList<TextWithFallbackFontFamily>();
    }
    finally
    {
      if (!font.IsStandardFont)
        Pdfium.FPDFOBJ_Release(font.Dictionary.Handle);
    }
  }

  public static System.Collections.Generic.IReadOnlyList<TextWithFallbackFontFamily> GetTextWithFallbackFonts(
    string str,
    PdfFont font,
    float fontSize,
    FS_RECTF textBounds,
    CultureInfo cultureInfo = null)
  {
    if (font == null || string.IsNullOrEmpty(str))
      return (System.Collections.Generic.IReadOnlyList<TextWithFallbackFontFamily>) null;
    PdfFontUtils.Init();
    if (cultureInfo == null)
      cultureInfo = CultureInfo.CurrentUICulture;
    Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
    System.Windows.FontWeight weight = GetFontWeight(font);
    FontStyle style = GetFontStyle(font);
    FontStretch stretch = FontStretches.Normal;
    StringInfo stringInfo = new StringInfo(str);
    List<TextWithFallbackFontFamily> list = new List<TextWithFallbackFontFamily>();
    int startingTextElement1 = 0;
    float startLeft = 0.0f;
    float curTop = 0.0f;
    FontFamily curFont = (FontFamily) null;
    FamilyTypeface curTypeface = (FamilyTypeface) null;
    float curScaledFontSize = fontSize;
    string curText = (string) null;
    float baseline = 0.0f;
    float width = 0.0f;
    float height = 0.0f;
    PdfTextObject pdfTextObject = PdfTextObject.Create("", 0.0f, 0.0f, font, fontSize);
    try
    {
      for (int startingTextElement2 = 0; startingTextElement2 < stringInfo.LengthInTextElements; ++startingTextElement2)
      {
        string str1 = stringInfo.SubstringByTextElements(startingTextElement2, 1);
        if (!string.IsNullOrEmpty(str1))
        {
          bool flag1;
          if (!dictionary.TryGetValue(str1, out flag1))
          {
            if (str1.Length == 1)
            {
              int charcode = Pdfium.FPDFFont_CharCodeFromUnicode(font.Handle, str1[0]);
              flag1 = charcode < 0 || (int) Pdfium.FPDFFont_UnicodeFromCharCode(font.Handle, charcode) != (int) str1[0];
            }
            else
            {
              pdfTextObject.TextUnicode = str1;
              flag1 = pdfTextObject.TextUnicode != str1;
            }
            dictionary[str1] = flag1;
          }
          FontFamily fontFamily = (FontFamily) null;
          FamilyTypeface typeface = (FamilyTypeface) null;
          float scaledfontSize = 0.0f;
          bool flag2;
          if (flag1)
          {
            int unicodeChar = PdfFontUtils.GetUnicodeChar(str1);
            if (unicodeChar < 0)
            {
              scaledfontSize = fontSize;
              typeface = (FamilyTypeface) null;
              flag2 = curFont != null;
            }
            else
            {
              fontFamily = PdfFontUtils.GetDefaultFontFamily(unicodeChar, fontSize, style, weight, stretch, cultureInfo, out scaledfontSize, out typeface);
              if (fontFamily == null)
              {
                scaledfontSize = fontSize;
                typeface = (FamilyTypeface) null;
              }
              flag2 = fontFamily != curFont;
            }
          }
          else
          {
            scaledfontSize = fontSize;
            typeface = (FamilyTypeface) null;
            flag2 = curFont != null;
          }
          if (flag2)
          {
            curText = stringInfo.SubstringByTextElements(startingTextElement1, startingTextElement2 - startingTextElement1);
            if (startingTextElement2 != 0)
            {
              UpdateProps(curFont != null);
              Do();
              startingTextElement1 = startingTextElement2;
              curScaledFontSize = scaledfontSize;
              curTypeface = typeface;
              startLeft += width;
              if ((double) startLeft > (double) textBounds.Width)
              {
                startLeft = 0.0f;
                curTop += height;
              }
            }
            curFont = fontFamily;
          }
        }
      }
      curText = stringInfo.SubstringByTextElements(startingTextElement1, stringInfo.LengthInTextElements - startingTextElement1);
      UpdateProps(curFont != null);
      Do();
      return (System.Collections.Generic.IReadOnlyList<TextWithFallbackFontFamily>) list;
    }
    finally
    {
      Pdfium.FPDFPageObj_Release(pdfTextObject.Handle);
    }

    void UpdateProps(bool useFallback)
    {
      System.Windows.FontWeight _weight = weight;
      FontStyle _style = style;
      Typeface typeface;
      if (useFallback)
      {
        typeface = new Typeface(curFont, _style, _weight, stretch);
      }
      else
      {
        FontFamily fontFamily = new FontFamily(font.BaseFontName);
        typeface = (fontFamily.GetTypefaces().FirstOrDefault<Typeface>((Func<Typeface, bool>) (c => c.Style == _style && c.Weight == _weight)) ?? fontFamily.GetTypefaces().FirstOrDefault<Typeface>((Func<Typeface, bool>) (c => c.Style == FontStyles.Normal && c.Weight == FontWeights.Normal))) ?? fontFamily.GetTypefaces().FirstOrDefault<Typeface>();
      }
      float emSize = (float) ((double) curScaledFontSize * 96.0 / 72.0);
      FormattedText formattedText = new FormattedText(curText, cultureInfo ?? CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, (double) emSize, (Brush) Brushes.Black, 1.0);
      if (formattedText.BuildGeometry(new Point()).Bounds.IsEmpty)
      {
        width = 0.0f;
        height = 0.0f;
      }
      else
      {
        width = (float) (formattedText.Width * 72.0 / 96.0);
        height = (float) (formattedText.Height * 72.0 / 96.0);
      }
      baseline = (float) ((formattedText.Height - formattedText.Baseline) * 72.0 / 96.0);
    }

    void Do()
    {
      FontCharSet suggestFontCharSet = GetSuggestFontCharSet(curText, curFont?.Source ?? font.BaseFontName, curFont != null ? curScaledFontSize : fontSize, width);
      TextWithFallbackFontFamily fallbackFontFamily1;
      ref TextWithFallbackFontFamily local = ref fallbackFontFamily1;
      string curText = curText;
      FontFamily fallbackFontFamily2 = curFont ?? (FontFamily) null;
      System.Windows.FontWeight fontWeight;
      if (curFont != null)
      {
        FamilyTypeface curTypeface = curTypeface;
        fontWeight = curTypeface != null ? curTypeface.Weight : weight;
      }
      else
        fontWeight = FontWeights.Normal;
      FontStyle fontStyle;
      if (curFont != null)
      {
        FamilyTypeface curTypeface = curTypeface;
        fontStyle = curTypeface != null ? curTypeface.Style : style;
      }
      else
        fontStyle = FontStyles.Normal;
      double fontSize = (double) fontSize;
      double curScaledFontSize = (double) curScaledFontSize;
      Rect bounds = new Rect((double) textBounds.left + (double) startLeft, (double) textBounds.top + (double) curTop, (double) Math.Max(width, 0.0f), (double) Math.Max(height, 0.0f));
      double baseline = (double) baseline;
      int charSet = (int) suggestFontCharSet;
      local = new TextWithFallbackFontFamily(curText, fallbackFontFamily2, fontWeight, fontStyle, (float) fontSize, (float) curScaledFontSize, bounds, (float) baseline, (FontCharSet) charSet);
      list.Add(fallbackFontFamily1);
    }

    static System.Windows.FontWeight GetFontWeight(PdfFont _font)
    {
      if (_font.IsStandardFont)
      {
        PdfFontUtils.InitFontStyleStockFonts();
        if (PdfFontUtils.boldStockFonts.Contains(_font.BaseFontName))
          return FontWeights.Bold;
      }
      int weight = _font.Weight;
      return weight <= 900 && weight >= 400 && weight % 100 == 0 ? System.Windows.FontWeight.FromOpenTypeWeight(weight) : FontWeights.Normal;
    }

    static FontStyle GetFontStyle(PdfFont _font)
    {
      if (_font.IsStandardFont)
      {
        PdfFontUtils.InitFontStyleStockFonts();
        if (PdfFontUtils.italicStockFonts.Contains(_font.BaseFontName))
          return FontStyles.Italic;
        if (PdfFontUtils.obliqueStockFonts.Contains(_font.BaseFontName))
          return FontStyles.Oblique;
      }
      return FontStyles.Normal;
    }

    FontCharSet GetSuggestFontCharSet(
      string _text,
      string _fontFamilyName,
      float _scaledFontSize,
      float charWidth)
    {
      if (string.IsNullOrEmpty(_text))
        return FontCharSet.ANSI_CHARSET;
      StringInfo stringInfo = new StringInfo(_text);
      bool flag = true;
      char ch = char.MinValue;
      for (int startingTextElement = 0; startingTextElement < stringInfo.LengthInTextElements; ++startingTextElement)
      {
        int unicodeChar = PdfFontUtils.GetUnicodeChar(stringInfo.SubstringByTextElements(startingTextElement, 1));
        if (ch == char.MinValue)
          ch = (char) unicodeChar;
        if (unicodeChar < 0 || unicodeChar > 121)
        {
          flag = false;
          break;
        }
      }
      if (flag)
        return FontCharSet.ANSI_CHARSET;
      FontCharSet suggestFontCharSet = CharHelper.CharsetFromUnicode(ch, (CultureInfo) null, FontCharSet.DEFAULT_CHARSET);
      if (suggestFontCharSet != FontCharSet.DEFAULT_CHARSET)
        return suggestFontCharSet;
      FontCharSet[] charSet1 = PdfFontUtils.GetCharSet(_text);
      FontCharSet? nullable = new FontCharSet?();
      float num1 = float.MaxValue;
      if (charSet1.Length != 0)
      {
        foreach (FontCharSet charSet2 in charSet1)
        {
          PdfFont font;
          if (PdfFontUtils.TryCreateFont(PdfFontUtils.innerDoc, _fontFamilyName, weight, style, charSet2, out font))
          {
            PdfTextObject pdfTextObject = PdfTextObject.Create(_text, 0.0f, 0.0f, font, _scaledFontSize);
            if (!(pdfTextObject.TextUnicode != _text))
            {
              float num2 = Math.Abs(pdfTextObject.BoundingBox.Width - charWidth);
              if (!nullable.HasValue || (double) num2 < (double) num1)
              {
                nullable = new FontCharSet?(charSet2);
                num1 = num2;
              }
              Pdfium.FPDFPageObj_Release(pdfTextObject.Handle);
              if (!font.IsStandardFont)
                Pdfium.FPDFOBJ_Release(font.Handle);
            }
          }
        }
      }
      return nullable.GetValueOrDefault();
    }
  }

  private static int GetUnicodeChar(string character)
  {
    byte[] bytes = Encoding.Unicode.GetBytes(character);
    byte[] destinationArray;
    if (bytes.Length == 4)
    {
      destinationArray = bytes;
    }
    else
    {
      if (bytes.Length >= 4)
        return -1;
      destinationArray = new byte[4];
      int destinationIndex = 0;
      if (!BitConverter.IsLittleEndian)
        destinationIndex = destinationArray.Length - bytes.Length;
      Array.Copy((Array) bytes, 0, (Array) destinationArray, destinationIndex, bytes.Length);
    }
    return BitConverter.ToInt32(destinationArray, 0);
  }

  private static FamilyTypeface GetFamilyTypeface(
    int ch,
    System.Windows.FontWeight fontWeight,
    FontStretch fontStretch)
  {
    FontFamilyMap defaultFamilyMap = PdfFontUtils.GetDefaultFamilyMap(ch);
    if (defaultFamilyMap == null || string.IsNullOrEmpty(defaultFamilyMap.Target))
      return (FamilyTypeface) null;
    FontFamily fontFamily = new FontFamily(defaultFamilyMap.Target);
    FamilyTypeface familyTypeface1 = (FamilyTypeface) null;
    FamilyTypeface familyTypeface2 = (FamilyTypeface) null;
    for (int index = 0; index < fontFamily.FamilyTypefaces.Count; ++index)
    {
      FamilyTypeface familyTypeface3 = fontFamily.FamilyTypefaces[index];
      if (PdfFontUtils.ContainsCharacter(familyTypeface3, ch))
      {
        if (familyTypeface2 == null)
          familyTypeface2 = familyTypeface3;
        if (familyTypeface3.Weight == fontWeight && familyTypeface3.Stretch == fontStretch)
        {
          familyTypeface1 = familyTypeface3;
          break;
        }
      }
    }
    return familyTypeface1 ?? familyTypeface2;
  }

  private static System.Collections.Generic.IReadOnlyList<FontFamily> GetFontFamiliesFromMap(
    FontFamilyMap familyMap)
  {
    if (familyMap == null)
      throw new ArgumentNullException(nameof (familyMap));
    System.Collections.Generic.IReadOnlyList<FontFamily> array;
    if (!PdfFontUtils.targetFontFamilies.TryGetValue(familyMap.Target, out array))
    {
      List<string> list = ((IEnumerable<string>) familyMap.Target.Split(',')).Select<string, string>((Func<string, string>) (c => c.Trim())).ToList<string>();
      List<string> collection1 = new List<string>();
      List<string> collection2 = new List<string>();
      int num = list.Count - 1;
      while (num >= 0)
        --num;
      if (collection2.Count > 0)
        list.AddRange((IEnumerable<string>) collection2);
      if (collection1.Count > 0)
        list.AddRange((IEnumerable<string>) collection1);
      array = (System.Collections.Generic.IReadOnlyList<FontFamily>) list.Select<string, FontFamily>((Func<string, FontFamily>) (c => new FontFamily(c))).ToArray<FontFamily>();
      PdfFontUtils.targetFontFamilies[familyMap.Target] = array;
    }
    return array;
  }

  private static FontFamily GetDefaultFontFamily(
    int ch,
    float fontSize,
    FontStyle fontStyle,
    System.Windows.FontWeight fontWeight,
    FontStretch fontStretch,
    CultureInfo cultureInfo,
    out float scaledfontSize,
    out FamilyTypeface typeface)
  {
    typeface = (FamilyTypeface) null;
    scaledfontSize = fontSize;
    FontFamilyMap defaultFamilyMap = PdfFontUtils.GetDefaultFamilyMap(ch, cultureInfo);
    if (defaultFamilyMap == null || string.IsNullOrEmpty(defaultFamilyMap.Target))
      return (FontFamily) null;
    scaledfontSize = fontSize * (float) defaultFamilyMap.Scale;
    System.Collections.Generic.IReadOnlyList<FontFamily> fontFamiliesFromMap = PdfFontUtils.GetFontFamiliesFromMap(defaultFamilyMap);
    string defaultDocumentFont = PdfFontUtils.TryGetDefaultDocumentFont(cultureInfo);
    foreach (FontFamily defaultFontFamily in (IEnumerable<FontFamily>) fontFamiliesFromMap)
    {
      FamilyTypeface[] array = defaultFontFamily.FamilyTypefaces.Where<FamilyTypeface>((Func<FamilyTypeface, bool>) (c => c.Style == fontStyle && c.Weight == fontWeight && c.Stretch == fontStretch)).ToArray<FamilyTypeface>();
      if (array.Length != 0)
      {
        if (!string.IsNullOrEmpty(defaultDocumentFont))
        {
          if (defaultFontFamily.Source.StartsWith(defaultDocumentFont, StringComparison.OrdinalIgnoreCase))
          {
            typeface = array[0];
            return defaultFontFamily;
          }
        }
        else
        {
          typeface = array[0];
          return defaultFontFamily;
        }
      }
    }
    typeface = fontFamiliesFromMap[0].FamilyTypefaces.FirstOrDefault<FamilyTypeface>((Func<FamilyTypeface, bool>) (c => c.Style == FontStyles.Normal && c.Weight == FontWeights.Normal));
    if (typeface == null)
      typeface = fontFamiliesFromMap[0].FamilyTypefaces.FirstOrDefault<FamilyTypeface>();
    return fontFamiliesFromMap[0];
  }

  public static string TryGetDefaultDocumentFont(CultureInfo cultureInfo)
  {
    return PdfFontUtils.GetLanguageDefaultFont(new FontFamily("#GLOBAL SERIF"), cultureInfo);
  }

  public static string TryGetDefaultUIFont(CultureInfo cultureInfo)
  {
    return PdfFontUtils.GetLanguageDefaultFont(new FontFamily("#GLOBAL USER INTERFACE"), cultureInfo);
  }

  private static string GetLanguageDefaultFont(FontFamily fontFamily, CultureInfo cultureInfo)
  {
    if (fontFamily == null || cultureInfo == null || cultureInfo.Name == "")
      return string.Empty;
    string source = fontFamily.Source;
    string key = cultureInfo.Name + source;
    string languageDefaultFont;
    if (PdfFontUtils.cultureNameTypeFontNameDict.TryGetValue(key, out languageDefaultFont))
      return languageDefaultFont;
    CultureInfo cultureInfo1 = cultureInfo;
    CultureInfo cultureInfo2 = (CultureInfo) null;
    if (!cultureInfo1.IsNeutralCulture)
    {
      cultureInfo2 = cultureInfo1;
      while (cultureInfo2 != null && !cultureInfo2.IsNeutralCulture)
        cultureInfo2 = cultureInfo2.Parent;
    }
    int[] array = GetTextElementIEnumerable((cultureInfo2 ?? cultureInfo1).NativeName).Select<string, int>((Func<string, int>) (c => PdfFontUtils.GetUnicodeChar(c))).ToArray<int>();
    FontFamilyMapCollection familyMaps = fontFamily.FamilyMaps;
    int num1 = 0;
    FontFamilyMap fontFamilyMap1 = (FontFamilyMap) null;
    int num2 = 0;
    FontFamilyMap fontFamilyMap2 = (FontFamilyMap) null;
    foreach (FontFamilyMap _map in familyMaps)
    {
      int? nullable1 = new int?();
      int? nullable2;
      if (PdfFontUtils.MatchCulture(_map.Language, cultureInfo1))
      {
        nullable1 = new int?(GetMatchLength(array, _map));
        nullable2 = nullable1;
        int num3 = num1;
        if (nullable2.GetValueOrDefault() > num3 & nullable2.HasValue)
        {
          num1 = nullable1.Value;
          fontFamilyMap1 = _map;
          if (num1 == array.Length)
            break;
        }
      }
      if (cultureInfo2 != null && PdfFontUtils.MatchCulture(_map.Language, cultureInfo2))
      {
        if (!nullable1.HasValue)
          nullable1 = new int?(GetMatchLength(array, _map));
        nullable2 = nullable1;
        int num4 = num2;
        if (nullable2.GetValueOrDefault() > num4 & nullable2.HasValue)
        {
          num2 = nullable1.Value;
          fontFamilyMap2 = _map;
        }
      }
    }
    bool flag = false;
    if (!string.IsNullOrEmpty(fontFamilyMap1?.Target))
    {
      flag = true;
      languageDefaultFont = ((IEnumerable<string>) fontFamilyMap1.Target.Split(',')).FirstOrDefault<string>().Trim();
    }
    if (!string.IsNullOrEmpty(fontFamilyMap2?.Target))
    {
      flag = true;
      languageDefaultFont = ((IEnumerable<string>) fontFamilyMap2.Target.Split(',')).FirstOrDefault<string>().Trim();
    }
    if (flag)
      PdfFontUtils.cultureNameTypeFontNameDict[key] = languageDefaultFont;
    return languageDefaultFont;

    static IEnumerable<string> GetTextElementIEnumerable(string _str)
    {
      TextElementEnumerator _e = StringInfo.GetTextElementEnumerator(_str);
      while (_e.MoveNext())
        yield return (string) _e.Current;
    }

    static int GetMatchLength(int[] _testStringChars, FontFamilyMap _map)
    {
      int matchLength = 0;
      for (int index = 0; index < _testStringChars.Length; ++index)
      {
        if (PdfFontUtils.InFamilyMapRange(_map, _testStringChars[index]))
          ++matchLength;
      }
      return matchLength;
    }
  }

  private static FontFamilyMap GetFamilyMap(int ch, FontFamily fontFamily, CultureInfo cultureInfo = null)
  {
    if (fontFamily == null)
      throw new ArgumentNullException(nameof (fontFamily));
    if (fontFamily.FamilyMaps.Count == 0)
      return (FontFamilyMap) null;
    if (cultureInfo != null && cultureInfo.Name == CultureInfo.InvariantCulture.Name)
      cultureInfo = (CultureInfo) null;
    FontFamilyMap fontFamilyMap1 = (FontFamilyMap) null;
    FontFamilyMap fontFamilyMap2 = (FontFamilyMap) null;
    for (int index = 0; index < fontFamily.FamilyMaps.Count; ++index)
    {
      FontFamilyMap familyMap = PdfFontUtils.FontFamilyGlobalUI.FamilyMaps[index];
      if (PdfFontUtils.InFamilyMapRange(familyMap, ch))
      {
        fontFamilyMap2 = familyMap;
        if (cultureInfo != null)
        {
          if (familyMap.Language?.GetSpecificCulture().ThreeLetterISOLanguageName == cultureInfo.ThreeLetterISOLanguageName)
          {
            fontFamilyMap1 = familyMap;
            break;
          }
        }
        else
          break;
      }
    }
    return fontFamilyMap1 ?? fontFamilyMap2;
  }

  public static FontFamilyMap GetDefaultFamilyMap(int ch, CultureInfo cultureInfo = null)
  {
    if (cultureInfo != null && cultureInfo.Name == CultureInfo.InvariantCulture.Name)
      cultureInfo = (CultureInfo) null;
    FontFamilyMap fontFamilyMap1 = (FontFamilyMap) null;
    FontFamilyMap fontFamilyMap2 = (FontFamilyMap) null;
    for (int index = 0; index < PdfFontUtils.FontFamilyGlobalUI.FamilyMaps.Count; ++index)
    {
      FontFamilyMap familyMap = PdfFontUtils.FontFamilyGlobalUI.FamilyMaps[index];
      if (PdfFontUtils.InFamilyMapRange(familyMap, ch))
      {
        fontFamilyMap2 = familyMap;
        if (cultureInfo != null)
        {
          if (familyMap.Language?.GetSpecificCulture()?.ThreeLetterISOLanguageName == cultureInfo.ThreeLetterISOLanguageName)
          {
            fontFamilyMap1 = familyMap;
            break;
          }
        }
        else
          break;
      }
    }
    return fontFamilyMap1 ?? fontFamilyMap2;
  }

  private static bool InFamilyMapRange(FontFamilyMap map, int ch)
  {
    if (map == null)
      throw new ArgumentNullException(nameof (map));
    if (PdfFontUtils.fontFamilyMapInRangeFunc == null)
    {
      MethodInfo method = typeof (FontFamilyMap)?.GetMethod("InRange", BindingFlags.Instance | BindingFlags.NonPublic);
      PdfFontUtils.fontFamilyMapInRangeFunc = !(method != (MethodInfo) null) ? (Func<FontFamilyMap, int, bool>) ((f, c) => true) : ReflectionHelper.BuildMethodFunction<FontFamilyMap, int, bool>(method);
    }
    return PdfFontUtils.fontFamilyMapInRangeFunc(map, ch);
  }

  private static bool ContainsCharacter(FamilyTypeface familyTypeface, int ch)
  {
    if (familyTypeface == null)
      throw new ArgumentNullException(nameof (familyTypeface));
    if (PdfFontUtils.familyTypefaceContainsCharacterFunc == null)
    {
      Type type = typeof (FamilyTypeface).Assembly.GetType("MS.Internal.FontFace.IDeviceFont");
      MethodInfo method = type?.GetMethod(nameof (ContainsCharacter));
      PdfFontUtils.familyTypefaceContainsCharacterFunc = !(method != (MethodInfo) null) ? (Func<FamilyTypeface, int, bool>) ((f, c) => true) : ReflectionHelper.BuildMethodFunction<FamilyTypeface, int, bool>(type, method);
    }
    return PdfFontUtils.familyTypefaceContainsCharacterFunc(familyTypeface, ch);
  }

  private static bool MatchCulture(XmlLanguage familyMapLanguage, CultureInfo cultureInfo)
  {
    if (familyMapLanguage == null && cultureInfo == null)
      return true;
    if (familyMapLanguage == null || cultureInfo == null)
      return false;
    if (PdfFontUtils.matchCultureFunc == null)
    {
      MethodInfo method = typeof (FontFamilyMap)?.GetMethod(nameof (MatchCulture), BindingFlags.Static | BindingFlags.NonPublic);
      if (method != (MethodInfo) null)
      {
        Func<FontFamilyMap, XmlLanguage, CultureInfo, bool> func = ReflectionHelper.BuildMethodFunction<FontFamilyMap, XmlLanguage, CultureInfo, bool>(method);
        PdfFontUtils.matchCultureFunc = (Func<XmlLanguage, CultureInfo, bool>) ((x, c) => func((FontFamilyMap) null, x, c));
      }
      if (PdfFontUtils.matchCultureFunc == null)
        PdfFontUtils.matchCultureFunc = (Func<XmlLanguage, CultureInfo, bool>) ((x, c) => x.IetfLanguageTag == c.IetfLanguageTag);
    }
    return PdfFontUtils.matchCultureFunc(familyMapLanguage, cultureInfo);
  }

  private enum FontFamilyCreateType
  {
    Stock,
    Standard,
    DefaultCreate,
    Windows,
    Unsupported,
  }
}
