// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.PdfFontHelper
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using PDFKit.Contents.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

#nullable disable
namespace PDFKit.Contents;

internal static class PdfFontHelper
{
  internal const float DefaultFontSize = 9f;
  private static readonly Dictionary<FontCharSet, (string, float)[]> DefaultFontNames = new Dictionary<FontCharSet, (string, float)[]>()
  {
    [FontCharSet.ANSI_CHARSET] = new (string, float)[2]
    {
      ("Calibri", 1f),
      ("Cambria", 1f)
    },
    [FontCharSet.SHIFTJIS_CHARSET] = new (string, float)[2]
    {
      ("Meiryo", 0.9f),
      ("MS Mincho", 1f)
    },
    [FontCharSet.HANGEUL_CHARSET] = new (string, float)[2]
    {
      ("Malgun Gothic", 0.9f),
      ("Batang", 1f)
    },
    [FontCharSet.HANGEUL_CHARSET] = new (string, float)[2]
    {
      ("Malgun Gothic", 0.9f),
      ("Batang", 1f)
    },
    [FontCharSet.GB2312_CHARSET] = new (string, float)[2]
    {
      ("Microsoft YaHei", 0.9f),
      ("SimSun", 1f)
    },
    [FontCharSet.CHINESEBIG5_CHARSET] = new (string, float)[2]
    {
      ("Microsoft JhengHei", 0.9f),
      ("MingLiU", 1f)
    },
    [FontCharSet.GREEK_CHARSET] = new (string, float)[2]
    {
      ("Calibri", 1f),
      ("Cambria", 1f)
    },
    [FontCharSet.TURKISH_CHARSET] = new (string, float)[2]
    {
      ("Calibri", 1f),
      ("Cambria", 1f)
    },
    [FontCharSet.VIETNAMESE_CHARSET] = new (string, float)[2]
    {
      ("Calibri", 1f),
      ("Cambria", 1f)
    },
    [FontCharSet.HEBREW_CHARSET] = new (string, float)[2]
    {
      ("Arial", 0.9f),
      ("Times New Roman", 1f)
    },
    [FontCharSet.ARABIC_CHARSET] = new (string, float)[2]
    {
      ("Arial", 0.9f),
      ("Sakkal Majalla", 1.27f)
    },
    [FontCharSet.BALTIC_CHARSET] = new (string, float)[2]
    {
      ("Calibri", 1f),
      ("Cambria", 1f)
    },
    [FontCharSet.RUSSIAN_CHARSET] = new (string, float)[2]
    {
      ("Calibri", 1f),
      ("Cambria", 1f)
    },
    [FontCharSet.THAI_CHARSET] = new (string, float)[2]
    {
      ("Browallia New", 1.41f),
      ("Angsana New", 1.45f)
    },
    [FontCharSet.EASTEUROPE_CHARSET] = new (string, float)[2]
    {
      ("Calibri", 1f),
      ("Cambria", 1f)
    }
  };
  private static readonly HashSet<string> pdfStandardFontFace = new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
  {
    "Arial",
    "Arial,Bold",
    "Arial,BoldItalic",
    "Arial,Italic",
    "Arial-Bold",
    "Arial-BoldItalic",
    "Arial-BoldItalicMT",
    "Arial-BoldMT",
    "Arial-Italic",
    "Arial-ItalicMT",
    "ArialBold",
    "ArialBoldItalic",
    "ArialItalic",
    "ArialMT",
    "ArialMT,Bold",
    "ArialMT,BoldItalic",
    "ArialMT,Italic",
    "ArialRoundedMTBold",
    "Courier",
    "Courier,Bold",
    "Courier,BoldItalic",
    "Courier,Italic",
    "Courier-Bold",
    "Courier-BoldOblique",
    "Courier-Oblique",
    "CourierBold",
    "CourierBoldItalic",
    "CourierItalic",
    "CourierNew",
    "CourierNew,Bold",
    "CourierNew,BoldItalic",
    "CourierNew,Italic",
    "CourierNew-Bold",
    "CourierNew-BoldItalic",
    "CourierNew-Italic",
    "CourierNewBold",
    "CourierNewBoldItalic",
    "CourierNewItalic",
    "CourierNewPS-BoldItalicMT",
    "CourierNewPS-BoldMT",
    "CourierNewPS-ItalicMT",
    "CourierNewPSMT",
    "CourierStd",
    "CourierStd-Bold",
    "CourierStd-BoldOblique",
    "CourierStd-Oblique",
    "Helvetica",
    "Helvetica,Bold",
    "Helvetica,BoldItalic",
    "Helvetica,Italic",
    "Helvetica-Bold",
    "Helvetica-BoldItalic",
    "Helvetica-BoldOblique",
    "Helvetica-Italic",
    "Helvetica-Oblique",
    "HelveticaBold",
    "HelveticaBoldItalic",
    "HelveticaItalic",
    "Symbol",
    "SymbolMT",
    "Times-Bold",
    "Times-BoldItalic",
    "Times-Italic",
    "Times-Roman",
    "TimesBold",
    "TimesBoldItalic",
    "TimesItalic",
    "TimesNewRoman",
    "TimesNewRoman,Bold",
    "TimesNewRoman,BoldItalic",
    "TimesNewRoman,Italic",
    "TimesNewRoman-Bold",
    "TimesNewRoman-BoldItalic",
    "TimesNewRoman-Italic",
    "TimesNewRomanBold",
    "TimesNewRomanBoldItalic",
    "TimesNewRomanItalic",
    "TimesNewRomanPS",
    "TimesNewRomanPS-Bold",
    "TimesNewRomanPS-BoldItalic",
    "TimesNewRomanPS-BoldItalicMT",
    "TimesNewRomanPS-BoldMT",
    "TimesNewRomanPS-Italic",
    "TimesNewRomanPS-ItalicMT",
    "TimesNewRomanPSMT",
    "TimesNewRomanPSMT,Bold",
    "TimesNewRomanPSMT,BoldItalic",
    "TimesNewRomanPSMT,Italic",
    "ZapfDingbats"
  };

  internal static bool IsStandardFontFace(string fontName)
  {
    return PdfFontHelper.pdfStandardFontFace.Contains(fontName);
  }

  internal static IReadOnlyDictionary<FontCharSet, PdfFont> CreateCharSetFont(
    PdfDocument doc,
    string fontFamilyName,
    IEnumerable<FontCharSet> charSets)
  {
    FontCharSet[] array = charSets != null ? charSets.Distinct<FontCharSet>().ToArray<FontCharSet>() : (FontCharSet[]) null;
    if (array == null || array.Length == 0)
      return (IReadOnlyDictionary<FontCharSet, PdfFont>) null;
    if (PdfFontHelper.IsStandardFontFace(fontFamilyName))
    {
      PdfFont font = PdfFont.CreateStock(doc, fontFamilyName);
      return (IReadOnlyDictionary<FontCharSet, PdfFont>) ((IEnumerable<FontCharSet>) array).Select<FontCharSet, (FontCharSet, PdfFont)>((Func<FontCharSet, (FontCharSet, PdfFont)>) (c => (c, font))).ToDictionary<(FontCharSet, PdfFont), FontCharSet, PdfFont>((Func<(FontCharSet, PdfFont), FontCharSet>) (c => c.Key), (Func<(FontCharSet, PdfFont), PdfFont>) (c => c.Value));
    }
    WindowsFontFamily fontFamily = WindowsFonts.GetFontFamily(fontFamilyName);
    if (fontFamily == null)
      return (IReadOnlyDictionary<FontCharSet, PdfFont>) null;
    Dictionary<FontCharSet, PdfFont> dictionary = new Dictionary<FontCharSet, PdfFont>();
    foreach (FontCharSet key in array)
    {
      Patagames.Pdf.LOGFONT logfont;
      if (fontFamily.LOGFONT.TryGetValue(key, out logfont))
      {
        try
        {
          PdfFont windowsFont = PdfFont.CreateWindowsFont(doc, logfont, false, false);
          dictionary[key] = windowsFont;
        }
        catch
        {
        }
      }
    }
    return dictionary.Count > 0 ? (IReadOnlyDictionary<FontCharSet, PdfFont>) dictionary : (IReadOnlyDictionary<FontCharSet, PdfFont>) null;
  }

  public static PdfFont FindSubstFont(
    PdfDocument document,
    PdfFont font,
    char unicode,
    CultureInfo cultureInfo)
  {
    FontCharSet charSet1 = CharHelper.CharsetFromUnicode(unicode, cultureInfo);
    bool flag = CharHelper.IsCJK(unicode, false);
    PdfFont substFont = PdfFontHelper.FindSubstFont(document, font, charSet1);
    if (!flag)
      return substFont;
    if (substFont != null)
    {
      if (substFont.ContainsChar(unicode))
        return substFont;
      List<FontCharSet> fontCharSetList = new List<FontCharSet>();
      if (charSet1 != FontCharSet.SHIFTJIS_CHARSET)
        fontCharSetList.Add(FontCharSet.SHIFTJIS_CHARSET);
      if (charSet1 != FontCharSet.GB2312_CHARSET)
        fontCharSetList.Add(FontCharSet.GB2312_CHARSET);
      if (charSet1 != FontCharSet.CHINESEBIG5_CHARSET)
        fontCharSetList.Add(FontCharSet.CHINESEBIG5_CHARSET);
      if (charSet1 != FontCharSet.HANGEUL_CHARSET)
        fontCharSetList.Add(FontCharSet.HANGEUL_CHARSET);
      fontCharSetList.Add(FontCharSet.DEFAULT_CHARSET);
      foreach (FontCharSet charSet2 in fontCharSetList)
      {
        if (substFont != null && !substFont.IsStandardFont)
          Pdfium.FPDFOBJ_Release(substFont.Dictionary.Handle);
        if (charSet2 != FontCharSet.DEFAULT_CHARSET)
        {
          substFont = PdfFontHelper.FindSubstFont(document, font, charSet2);
          if (substFont.ContainsChar(unicode))
            return substFont;
        }
      }
    }
    return (PdfFont) null;
  }

  public static PdfFont FindSubstFont(PdfDocument document, PdfFont font, FontCharSet charSet)
  {
    if (document == null || font == null)
      return (PdfFont) null;
    IntPtr ExtHandle;
    int Charset;
    Pdfium.FPDFFont_GetSubstFont(font.Handle, out ExtHandle, out string _, out Charset, out FontSubstFlags _, out int _, out int _, out bool _, out int _, out bool _);
    if (ExtHandle != IntPtr.Zero && (FontCharSet) Charset == charSet)
      return font;
    string faceName = font.BaseFontName;
    int num = faceName.IndexOf('+');
    if (num >= 0)
    {
      if (num == faceName.Length - 1)
        return (PdfFont) null;
      faceName = faceName.Substring(num + 1);
    }
    bool flag1 = font.IsBold();
    bool flag2 = font.IsItalic();
    FontWeight weight = flag1 ? FontWeight.FW_BOLD : FontWeight.FW_NORMAL;
    int italicAngel = flag2 ? 14 : 0;
    if (faceName.Contains("MinionPro"))
    {
      if (flag1 & flag2)
        return PdfFont.CreateStandardFont(document, "Times-BoldItalic", 1);
      if (flag1)
        return PdfFont.CreateStandardFont(document, "Times-Bold", 1);
      return flag2 ? PdfFont.CreateStandardFont(document, "Times-Italic", 1) : PdfFont.CreateStandardFont(document, "Times-Roman", 1);
    }
    try
    {
      return PdfFont.CreateFont(document, faceName, true, font.Flags, weight, italicAngel, charSet, font.IsVertWriting);
    }
    catch
    {
    }
    return (PdfFont) null;
  }

  public static PdfFont GetDefaultFont(
    PdfDocument document,
    FontCharSet charSet,
    out float fontSize)
  {
    fontSize = 0.0f;
    (string, float)[] tupleArray;
    if (PdfFontHelper.DefaultFontNames.TryGetValue(charSet, out tupleArray))
    {
      foreach ((string, float) tuple in tupleArray)
      {
        try
        {
          PdfFont font = PdfFont.CreateFont(document, tuple.Item1, charSet);
          if (font != null)
            fontSize = 9f * tuple.Item2;
          return font;
        }
        catch
        {
        }
      }
    }
    return (PdfFont) null;
  }

  internal static System.Collections.Generic.IReadOnlyList<(PdfFont font, string text, float fontScale)> FindSubstFonts(
    PdfDocument document,
    string text,
    PdfFont font,
    CultureInfo cultureInfo)
  {
    if (string.IsNullOrEmpty(text))
      return (System.Collections.Generic.IReadOnlyList<(PdfFont, string, float)>) Array.Empty<(PdfFont, string, float)>();
    System.Collections.Generic.IReadOnlyList<KeyValuePair<FontCharSet, string>> keyValuePairList = CharHelper.SplitTextByCharSet(text, cultureInfo);
    List<(PdfFont, float)> valueTupleList = new List<(PdfFont, float)>();
    List<(PdfFont, StringBuilder, float)> source = new List<(PdfFont, StringBuilder, float)>();
    foreach (KeyValuePair<FontCharSet, string> keyValuePair in (IEnumerable<KeyValuePair<FontCharSet, string>>) keyValuePairList)
    {
      FontCharSet key = keyValuePair.Key;
      string str1 = keyValuePair.Value;
      if (key != FontCharSet.DEFAULT_CHARSET)
      {
        StringInfo stringInfo = new StringInfo(str1);
        for (int startingTextElement = 0; startingTextElement < stringInfo.LengthInTextElements; ++startingTextElement)
        {
          string str2 = stringInfo.SubstringByTextElements(startingTextElement, 1);
          bool flag1 = false;
          PdfFont pdfFont = (PdfFont) null;
          float num = 1f;
          foreach ((PdfFont, float) valueTuple in valueTupleList)
          {
            if (valueTuple.Item1.ContainsChar(str2[0]))
            {
              flag1 = true;
              pdfFont = valueTuple.Item1;
              num = valueTuple.Item2;
              break;
            }
          }
          if (pdfFont == null && font != null)
            pdfFont = PdfFontHelper.FindSubstFont(document, font, str2[0], cultureInfo);
          if (pdfFont == null)
          {
            float fontSize;
            pdfFont = PdfFontHelper.GetDefaultFont(document, key, out fontSize);
            num = fontSize / 9f;
          }
          if (!flag1 && pdfFont != null)
            valueTupleList.Add((pdfFont, num));
          bool flag2 = false;
          if (source.Count == 0)
          {
            flag2 = true;
          }
          else
          {
            (PdfFont, StringBuilder, float) valueTuple = source[source.Count - 1];
            if (pdfFont != valueTuple.Item1 || (double) num != (double) valueTuple.Item3)
              flag2 = true;
          }
          if (flag2)
            source.Add((pdfFont, new StringBuilder().Append(str2[0]), num));
          else
            source[source.Count - 1].Item2.Append(str2[0]);
        }
      }
    }
    return (System.Collections.Generic.IReadOnlyList<(PdfFont, string, float)>) source.Select<(PdfFont, StringBuilder, float), (PdfFont, string, float)>((Func<(PdfFont, StringBuilder, float), (PdfFont, string, float)>) (c => (c.Item1, c.Item2.ToString(), c.Item3))).ToArray<(PdfFont, string, float)>();
  }
}
